using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
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
        private bool Overlay_dragging = false;
        private Point Overlay_dragCursorPoint;
        private Point Overlay_dragFormPoint;
        // LOOP CONTROL
        private bool Overlay_overlay_loop = true;
        public GlowOverlay(){
            InitializeComponent();
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width - 20, 20);
            Opacity = 0.7;
        }
        // TOOLTIP SETTINGS
        // ======================================================================================================
        private void MainToolTip_Draw(object sender, DrawToolTipEventArgs e){ e.DrawBackground(); e.DrawBorder(); e.DrawText(); }
        private async void GlowOverlay_Load(object sender, EventArgs e){
            await SetLabelTextSafeAsync(OVERLAY_CPU_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            await SetLabelTextSafeAsync(OVERLAY_RAM_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            await SetLabelTextSafeAsync(OVERLAY_DISK_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            await SetLabelTextSafeAsync(OVERLAY_NETWORK_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            //
            Task cpu_engine = Task.Run(() => { CPUEngine(); });
            Task ram_engine = Task.Run(() => { RAMEngine(); });
            Task disk_engine = Task.Run(() => { DISKEngine(); });
            Task network_engine = Task.Run(() => { NETWORKEngine(); });
            //
            Screen_overlay_settings();
        }
        // THEME
        // ======================================================================================================
        public void Screen_overlay_settings(){
            TSThemeModeHelper.InitializeThemeForForm(this);
            //
            MainToolTip.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "HeaderFEColor");
            MainToolTip.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "HeaderBGColor");
            //
            BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuBGAndBorderColor");
            TransparencyKey = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuBGAndBorderColor");
            //
            Color panelcolor = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuButtonHoverAndMouseDownColor");
            Color textcolor = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuButtonFEColor2");
            OVERLAY_BGPanel.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuButtonAlphaColor");
            //
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
            //
            TSImageRenderer(BtnOpacityChanger, GlowMain.theme == 1 ? Properties.Resources.ct_opacity_dark : Properties.Resources.ct_opacity_light, 12, ContentAlignment.MiddleCenter);
            TSImageRenderer(CloseOverlayBtn, GlowMain.theme == 1 ? Properties.Resources.ct_clear_dark : Properties.Resources.ct_clear_light, 15, ContentAlignment.MiddleCenter);
            //
            foreach (Control ui_buttons in OVERLAY_TopPanel.Controls){
                if (ui_buttons is Button control_button){
                    control_button.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                    control_button.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBG");
                    control_button.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBG");
                    control_button.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBGHover");
                    control_button.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBGHover");
                }
            }
            // TEXT
            // ==============
            Text = string.Format(software_lang.TSReadLangs("ScreenOverlayTool", "sot_title"), Application.ProductName);
            MainToolTip.SetToolTip(BtnOpacityChanger, software_lang.TSReadLangs("ScreenOverlayTool", "sot_opacity"));
            OVERLAY_CPU.Text = software_lang.TSReadLangs("ScreenOverlayTool", "sot_cpu");
            OVERLAY_RAM.Text = software_lang.TSReadLangs("ScreenOverlayTool", "sot_ram");
            OVERLAY_DISK.Text = software_lang.TSReadLangs("ScreenOverlayTool", "sot_disk");
            OVERLAY_NETWORK.Text = software_lang.TSReadLangs("ScreenOverlayTool", "sot_net");
        }
        // CPU ENGINE
        // ======================================================================================================
        private void CPUEngine(){
            try{
                ManagementObjectSearcher cpuSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor");
                while (Overlay_overlay_loop)
                {
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
                        SetLabelTextSafeAsync(OVERLAY_CPU_V, $"{averageCpuUsage:F2}%");
                    }
                    Thread.Sleep(750);
                }
            }catch (Exception){ }
        }
        // RAM ENGINE
        // ======================================================================================================
        private void RAMEngine(){
            try{
                ComputerInfo get_ram_info = new ComputerInfo();
                while (Overlay_overlay_loop)
                {
                    ulong total_ram = get_ram_info.TotalPhysicalMemory;
                    ulong usable_ram = get_ram_info.AvailablePhysicalMemory;
                    double usage_ram_percentage = (TS_FormatSizeNoType(total_ram) - TS_FormatSizeNoType(usable_ram)) / TS_FormatSizeNoType(total_ram) * 100;
                    SetLabelTextSafeAsync(OVERLAY_RAM_V, string.Format("{0:0.0} - {1} / {2}", TS_FormatSize(total_ram - usable_ram), $"{usage_ram_percentage:0.0}%", TS_FormatSize(total_ram)));
                    Thread.Sleep(750);
                }
            }catch (Exception){ }
        }
        // DISK ENGINE
        // ======================================================================================================
        private void DISKEngine(){
            try{
                while (Overlay_overlay_loop)
                {
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
                    string windowsDisk = GlowMain.windows_disk.Replace("\\", string.Empty);
                    var winDiskEntry = diskData.FirstOrDefault(kvp => kvp.Key.EndsWith(windowsDisk));
                    if (!winDiskEntry.Equals(default(KeyValuePair<string, (ulong, ulong)>))){
                        var (readSpeed, writeSpeed) = winDiskEntry.Value;
                        float readMB = readSpeed / (1024f * 1024f);
                        float writeMB = writeSpeed / (1024f * 1024f);
                        diskInformation.AppendLine($"{windowsDisk} - {readMB:F1} MB/s (R) - {writeMB:F1} MB/s (W)");
                    }
                    ManagementObjectSearcher logicalSearcher = new ManagementObjectSearcher("SELECT Name FROM Win32_LogicalDisk WHERE DriveType=3");
                    string secondDisk = null;
                    foreach (ManagementObject disk in logicalSearcher.Get().Cast<ManagementObject>()){
                        string drive = ((string)disk["Name"]).Replace("\\", string.Empty);
                        if (drive != windowsDisk){
                            secondDisk = drive;
                            break;
                        }
                    }
                    if (secondDisk != null){
                        var secondEntry = diskData.FirstOrDefault(kvp => kvp.Key.EndsWith(secondDisk));
                        if (!secondEntry.Equals(default(KeyValuePair<string, (ulong, ulong)>))){
                            var (readSpeed, writeSpeed) = secondEntry.Value;
                            float readMB = readSpeed / (1024f * 1024f);
                            float writeMB = writeSpeed / (1024f * 1024f);
                            diskInformation.AppendLine($"{secondDisk} - {readMB:F1} MB/s (R) - {writeMB:F1} MB/s (W)");
                        }
                    }
                    SetLabelTextSafeAsync(OVERLAY_DISK_V, diskInformation.ToString());
                    Thread.Sleep(750);
                }
            }catch (Exception){ }
        }
        // NETWORK ENGINE
        // ======================================================================================================
        private void NETWORKEngine(){
            try{
                string activeAdapter = Net_replacer(GetActiveNetworkAdapter());
                ManagementObjectSearcher searcher_net_speed = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkInterface");
                while (Overlay_overlay_loop)
                {
                    foreach (ManagementObject search_active_speed in searcher_net_speed.Get().Cast<ManagementObject>()){
                        if (Net_replacer(search_active_speed["Name"].ToString().Trim()) == activeAdapter){
                            ulong bytesReceived = (ulong)search_active_speed["BytesReceivedPerSec"];
                            ulong bytesSent = (ulong)search_active_speed["BytesSentPerSec"];
                            double mbpsReceived = Math.Round(bytesReceived * 8 / 1000000.0, 1);
                            double mbpsSent = Math.Round(bytesSent * 8 / 1000000.0, 1);
                            SetLabelTextSafeAsync(OVERLAY_NETWORK_V, $"{software_lang.TSReadLangs("ScreenOverlayTool", "sot_net_download")} {mbpsReceived:0.00} Mbps\n" + $"{software_lang.TSReadLangs("ScreenOverlayTool", "sot_net_upload")} {mbpsSent:0.00} Mbps");
                        }
                    }
                    Thread.Sleep(750);
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
            Overlay_dragging = true;
            Overlay_dragCursorPoint = Cursor.Position;
            Overlay_dragFormPoint = Location;
        }
        private void OVERLAY_TopPanel_MouseMove(object sender, MouseEventArgs e){
            if (Overlay_dragging){
                Point diff = Point.Subtract(Cursor.Position, new Size(Overlay_dragCursorPoint));
                Location = Point.Add(Overlay_dragFormPoint, new Size(diff));
            }
        }
        private void OVERLAY_TopPanel_MouseUp(object sender, MouseEventArgs e){
            Overlay_dragging = false;
        }
        // UI OPACITY CHANGER
        // ======================================================================================================
        private void BtnOpacityChanger_MouseDown(object sender, MouseEventArgs e){
            const double step = 0.1;
            if (e.Button == MouseButtons.Right){
                if (Opacity + step <= 1.0){
                    Opacity += step;
                }else{
                    Opacity = 1.0;
                }
            }else if (e.Button == MouseButtons.Left){
                if (Opacity - step >= 0.2){
                    Opacity -= step;
                }else{
                    Opacity = 0.2;
                }
            }
        }
        // EXIT
        // ======================================================================================================
        private void CloseOverlayBtn_Click(object sender, EventArgs e){
            Overlay_overlay_loop = false;
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
            return Task.Factory.StartNew(() => {
                if (!control.IsDisposed && control.IsHandleCreated)
                    control.Invoke(action);
            }, TaskCreationOptions.None);
        }
    }
}