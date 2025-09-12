using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Management;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowOverlay : Form{
        // GLOBAL LANGS PATH
        // ======================================================================================================
        readonly TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
        // DRAG MODES
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        // LOOP CONTROL
        private bool overlay_loop = true;
        public GlowOverlay(){
            InitializeComponent();
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width - 20, 20);
            Opacity = 0.7;
        }
        private async void GlowOverlay_Load(object sender, EventArgs e){
            await SetLabelTextSafeAsync(OVERLAY_CPU_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            await SetLabelTextSafeAsync(OVERLAY_RAM_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            await SetLabelTextSafeAsync(OVERLAY_DISK_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            await SetLabelTextSafeAsync(OVERLAY_NETWORK_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            // Start monitoring tasks
            _ = Task.Run(Cpu_engine);
            _ = Task.Run(Ram_engine);
            _ = Task.Run(Disk_engine);
            _ = Task.Run(Network_engine);
            Screen_overlay_settings();
        }
        // THEME
        // ======================================================================================================
        public void Screen_overlay_settings(){
            BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuBGAndBorderColor");
            TransparencyKey = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuBGAndBorderColor");
            Color panelcolor = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuButtonHoverAndMouseDownColor");
            Color textcolor = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuButtonFEColor2");
            OVERLAY_BGPanel.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuButtonAlphaColor");
            OVERLAY_P1.BackColor = panelcolor;
            OVERLAY_P2.BackColor = panelcolor;
            OVERLAY_P3.BackColor = panelcolor;
            OVERLAY_P4.BackColor = panelcolor;
            OVERLAY_P5.BackColor = panelcolor;
            OVERLAY_P6.BackColor = panelcolor;
            OVERLAY_P7.BackColor = panelcolor;
            OVERLAY_P8.BackColor = panelcolor;
            OVERLAY_CPU.ForeColor = textcolor;
            OVERLAY_RAM.ForeColor = textcolor;
            OVERLAY_DISK.ForeColor = textcolor;
            OVERLAY_NETWORK.ForeColor = textcolor;
            OVERLAY_CPU_V.ForeColor = textcolor;
            OVERLAY_RAM_V.ForeColor = textcolor;
            OVERLAY_DISK_V.ForeColor = textcolor;
            OVERLAY_NETWORK_V.ForeColor = textcolor;
            TSImageRenderer(CloseOverlayBtn, GlowMain.theme == 1 ? Properties.Resources.ct_clear_dark : Properties.Resources.ct_clear_light, 15, ContentAlignment.MiddleCenter);
            CloseOverlayBtn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBG");
            CloseOverlayBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBG");
            CloseOverlayBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBGHover");
            CloseOverlayBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBGHover");
            Text = string.Format(software_lang.TSReadLangs("ScreenOverlayTool", "sot_title"), Application.ProductName);
            OVERLAY_CPU.Text = software_lang.TSReadLangs("ScreenOverlayTool", "sot_cpu");
            OVERLAY_RAM.Text = software_lang.TSReadLangs("ScreenOverlayTool", "sot_ram");
            OVERLAY_DISK.Text = software_lang.TSReadLangs("ScreenOverlayTool", "sot_disk");
            OVERLAY_NETWORK.Text = software_lang.TSReadLangs("ScreenOverlayTool", "sot_net");
        }
        // CPU ENGINE
        // ======================================================================================================
        private async Task Cpu_engine(){
            try{
                ManagementObjectSearcher cpuSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor");
                while (overlay_loop){
                    ManagementObjectCollection cpuResults = cpuSearcher.Get();
                    ulong totalCpuUsage = 0;
                    int cpuCount = 0;
                    foreach (ManagementObject obj in cpuResults.Cast<ManagementObject>()){
                        string cpuName = (string)obj["Name"];
                        ulong cpuUsage = (ulong)obj["PercentProcessorTime"];
                        if (cpuName != "_Total"){
                            totalCpuUsage += cpuUsage;
                            cpuCount++;
                        }
                    }
                    if (cpuCount > 0){
                        float averageCpuUsage = (float)totalCpuUsage / cpuCount;
                        await SetLabelTextSafeAsync(OVERLAY_CPU_V, $"{averageCpuUsage:F2}%");
                    }
                    await Task.Delay(750);
                }
            }catch (Exception){ }
        }
        // RAM ENGINE
        // ======================================================================================================
        private async Task Ram_engine(){
            try{
                ComputerInfo get_ram_info = new ComputerInfo();
                while (overlay_loop){
                    ulong total_ram = get_ram_info.TotalPhysicalMemory;
                    ulong usable_ram = get_ram_info.AvailablePhysicalMemory;
                    double usage_ram_percentage = (TS_FormatSizeNoType(total_ram) - TS_FormatSizeNoType(usable_ram)) / TS_FormatSizeNoType(total_ram) * 100;
                    await SetLabelTextSafeAsync(OVERLAY_RAM_V, string.Format("{0:0.0} - {1} / {2}", TS_FormatSize(total_ram - usable_ram), $"{usage_ram_percentage:0.0}%", TS_FormatSize(total_ram)));
                    await Task.Delay(750);
                }
            }catch (Exception) { }
        }
        // DISK ENGINE
        // ======================================================================================================
        private async Task Disk_engine(){
            try{
                while (overlay_loop){
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, DiskReadBytesPersec, DiskWriteBytesPersec FROM Win32_PerfFormattedData_PerfDisk_PhysicalDisk");
                    ManagementObjectCollection results = searcher.Get();
                    StringBuilder diskInformation = new StringBuilder();
                    Dictionary<string, (ulong readSpeed, ulong writeSpeed)> diskData = new Dictionary<string, (ulong, ulong)>();
                    foreach (ManagementObject obj in results.Cast<ManagementObject>()){
                        string diskName = (string)obj["Name"];
                        ulong diskReadSpeed = (ulong)obj["DiskReadBytesPersec"];
                        ulong diskWriteSpeed = (ulong)obj["DiskWriteBytesPersec"];
                        if (diskName.Trim() != "_Total"){
                            diskData[diskName] = (diskReadSpeed, diskWriteSpeed);
                        }
                    }
                    var cDiskEntry = diskData.FirstOrDefault(kvp => kvp.Key.EndsWith(GlowMain.windows_disk.Replace("\\", string.Empty)));
                    if (!cDiskEntry.Equals(default(KeyValuePair<string, (ulong, ulong)>))){
                        var (readSpeed, writeSpeed) = cDiskEntry.Value;
                        float cDiskReadSpeedMB = readSpeed / (1024f * 1024f);
                        float cDiskWriteSpeedMB = writeSpeed / (1024f * 1024f);
                        diskInformation.AppendLine($"{GlowMain.windows_disk.Replace("\\", string.Empty)} - {cDiskReadSpeedMB:F1} MB/s (R) - {cDiskWriteSpeedMB:F1} MB/s (W)");
                    }
                    var dDiskEntry = diskData.FirstOrDefault(kvp => kvp.Key.EndsWith("D:"));
                    if (!dDiskEntry.Equals(default(KeyValuePair<string, (ulong, ulong)>))){
                        var (readSpeed, writeSpeed) = dDiskEntry.Value;
                        float dDiskReadSpeedMB = readSpeed / (1024f * 1024f);
                        float dDiskWriteSpeedMB = writeSpeed / (1024f * 1024f);
                        diskInformation.AppendLine($"D: - {dDiskReadSpeedMB:F1} MB/s (R) - {dDiskWriteSpeedMB:F1} MB/s (W)");
                    }
                    await SetLabelTextSafeAsync(OVERLAY_DISK_V, diskInformation.ToString());
                    await Task.Delay(750);
                }
            }catch (Exception){ }
        }
        // NETWORK ENGINE
        // ======================================================================================================
        private async Task Network_engine(){
            try{
                string activeAdapter = Net_replacer(GetActiveNetworkAdapter());
                ManagementObjectSearcher searcher_net_speed = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkInterface");
                while (overlay_loop){
                    foreach (ManagementObject search_active_speed in searcher_net_speed.Get().Cast<ManagementObject>()){
                        if (Net_replacer(search_active_speed["Name"].ToString().Trim()) == activeAdapter){
                            ulong bytesReceived = (ulong)search_active_speed["BytesReceivedPerSec"];
                            ulong bytesSent = (ulong)search_active_speed["BytesSentPerSec"];
                            double mbpsReceived = Math.Round(bytesReceived * 8 / 1000000.0, 1);
                            double mbpsSent = Math.Round(bytesSent * 8 / 1000000.0, 1);
                            await SetLabelTextSafeAsync(OVERLAY_NETWORK_V, $"{software_lang.TSReadLangs("ScreenOverlayTool", "sot_net_download")} {mbpsReceived:0.00} Mbps\n" + $"{software_lang.TSReadLangs("ScreenOverlayTool", "sot_net_upload")} {mbpsSent:0.00} Mbps");
                        }
                    }
                    await Task.Delay(750);
                }
            }catch (Exception){ }
        }
        private static string GetActiveNetworkAdapter(){
            try{
                ManagementObjectSearcher searcher_active_net = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionStatus=2");
                foreach (ManagementObject search_ip_enabled in searcher_active_net.Get().Cast<ManagementObject>()){
                    ManagementObjectSearcher configSearcher = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index={search_ip_enabled["Index"]} AND IPEnabled=True");
                    foreach (ManagementObject configObj in configSearcher.Get().Cast<ManagementObject>()){
                        return search_ip_enabled["Name"].ToString().Trim();
                    }
                }
            }catch (Exception){ }
            return null;
        }
        // OVERLAY MODE
        // ======================================================================================================
        private void OVERLAY_TopPanel_MouseDown(object sender, MouseEventArgs e){
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = Location;
        }
        private void OVERLAY_TopPanel_MouseMove(object sender, MouseEventArgs e){
            if (dragging){
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }
        private void OVERLAY_TopPanel_MouseUp(object sender, MouseEventArgs e){
            dragging = false;
        }
        // EXIT
        // ======================================================================================================
        private void CloseOverlayBtn_Click(object sender, EventArgs e){
            overlay_loop = false;
            Close();
        }
        // HELPER
        // ======================================================================================================
        private Task SetLabelTextSafeAsync(Label label, string text){
            if (label.InvokeRequired){
                return label.InvokeAsync(() => label.Text = text);
            }else{
                label.Text = text;
                return Task.CompletedTask;
            }
        }
    }
    public static class ControlExtensions{
        public static Task InvokeAsync(this Control control, Action action){
            return Task.Factory.StartNew(() =>{
                if (!control.IsDisposed && control.IsHandleCreated)
                    control.Invoke(action);
            }, TaskCreationOptions.None);
        }
    }
}