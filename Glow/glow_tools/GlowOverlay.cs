using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        // ENABLE EDGE WHEN BORDER IS CLOSED FOR WINDOWS 11
        // ======================================================================================================
        protected override void OnHandleCreated(EventArgs e){
            base.OnHandleCreated(e);
            if (Program.windows_mode == 1){
                int preference = (int)DWM_WINDOW_CORNER_PREFERENCE.Round;
                DwmSetWindowAttribute(this.Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref preference, sizeof(int));
            }
        }
        // LOAD
        // ======================================================================================================
        private async void GlowOverlay_Load(object sender, EventArgs e){
            await SetLabelTextSafeAsync(OVERLAY_CPU_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            await SetLabelTextSafeAsync(OVERLAY_RAM_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            await SetLabelTextSafeAsync(OVERLAY_DISK_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            await SetLabelTextSafeAsync(OVERLAY_NETWORK_V, software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            //
            Task cpu_engine = Task.Run(async () => await CPUEngine());
            Task ram_engine = Task.Run(async () => await RAMEngine());
            Task disk_engine = Task.Run(async () => await DISKEngine());
            Task network_engine = Task.Run(async () => await NETWORKEngine());
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
        private async Task CPUEngine(){
            using (var cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total")){
                try{
                    cpuCounter.NextValue();
                    while (Overlay_overlay_loop){
                        DateTime startTime = DateTime.Now;
                        try{
                            float cpuUsage = cpuCounter.NextValue();
                            await SetLabelTextSafeAsync(OVERLAY_CPU_V, $"{cpuUsage:F2}%");
                        }catch {}
                        int elapsed = (int)(DateTime.Now - startTime).TotalMilliseconds;
                        int nextDelay = Math.Max(10, 750 - elapsed);
                        try{
                            await Task.Delay(nextDelay);
                        }catch (TaskCanceledException){
                            break;
                        }
                    }
                }catch (Exception){ }
            }
        }
        // RAM ENGINE
        // ======================================================================================================
        private async Task RAMEngine(){
            try{
                var search_os = new ManagementObjectSearcher("root\\CIMV2", "SELECT TotalVisibleMemorySize, FreePhysicalMemory FROM Win32_OperatingSystem");
                while (Overlay_overlay_loop){
                    DateTime startTime = DateTime.Now;
                    try{
                        var get_ram_info = search_os.Get().Cast<ManagementObject>().FirstOrDefault();
                        ulong total_ram = (ulong)get_ram_info["TotalVisibleMemorySize"] * 1024;
                        ulong usable_ram = (ulong)get_ram_info["FreePhysicalMemory"] * 1024;
                        ulong used_ram = total_ram - usable_ram;
                        double usage_ram_percentage = (double)used_ram / total_ram * 100;
                        await SetLabelTextSafeAsync(OVERLAY_RAM_V, $"{TS_FormatSize(used_ram)} - {usage_ram_percentage:0.0}% / {TS_FormatSize(total_ram)}");
                    }catch{ }
                    int elapsed = (int)(DateTime.Now - startTime).TotalMilliseconds;
                    int nextDelay = Math.Max(10, 750 - elapsed);
                    try{
                        await Task.Delay(nextDelay);
                    }catch (TaskCanceledException){
                        break;
                    }
                }
            }catch (Exception){ }
        }
        // DISK ENGINE
        // ======================================================================================================
        private async Task DISKEngine(){
            try{
                string windowsDisk = Program.windows_disk.Replace("\\", string.Empty);
                var driveCounters = new List<(string DriveName, PerformanceCounter Read, PerformanceCounter Write)>();
                try{
                    var drives = DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed);
                    foreach (var drive in drives){
                        string driveName = drive.Name.Replace("\\", string.Empty);
                        string instance = GetDiskInstanceName(driveName);
                        if (instance != null){
                            driveCounters.Add((driveName, new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", instance), new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", instance)));
                        }
                    }
                }catch { }
                while (Overlay_overlay_loop){
                    DateTime startTime = DateTime.Now;
                    StringBuilder diskInformation = new StringBuilder();
                    try{
                        foreach (var (DriveName, Read, Write) in driveCounters){
                            float readMB = Read.NextValue() / (1024f * 1024f);
                            float writeMB = Write.NextValue() / (1024f * 1024f);
                            diskInformation.AppendLine($"{DriveName} - {readMB:F1} MB/s (R) - {writeMB:F1} MB/s (W)");
                        }
                        await SetLabelTextSafeAsync(OVERLAY_DISK_V, diskInformation.ToString().TrimEnd());
                    }catch { }
                    int elapsed = (int)(DateTime.Now - startTime).TotalMilliseconds;
                    int nextDelay = Math.Max(10, 750 - elapsed);
                    try{
                        await Task.Delay(nextDelay);
                    }catch (TaskCanceledException){
                        break;
                    }
                }
                driveCounters.ForEach(c => { c.Read.Dispose(); c.Write.Dispose(); });
            }catch (Exception){ }
        }
        private string GetDiskInstanceName(string driveLetter){
            var category = new PerformanceCounterCategory("PhysicalDisk");
            return category.GetInstanceNames().FirstOrDefault(name => name.EndsWith(driveLetter));
        }
        // NETWORK ENGINE
        // ======================================================================================================
        private async Task NETWORKEngine(){
            try{
                string activeAdapter = GetActiveNetworkAdapter();
                if (string.IsNullOrEmpty(activeAdapter)) return;
                var category = new PerformanceCounterCategory("Network Interface");
                string[] instanceNames = category.GetInstanceNames();
                string perfCounterAdapterName = instanceNames.FirstOrDefault(name => Net_replacer(name) == Net_replacer(activeAdapter));
                if (perfCounterAdapterName == null) return;
                using (var bytesReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", perfCounterAdapterName))
                using (var bytesSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", perfCounterAdapterName)){
                    while (Overlay_overlay_loop){
                        DateTime startTime = DateTime.Now;
                        try{
                            double mbpsReceived = bytesReceivedCounter.NextValue() * 8 / 1_000_000.0;
                            double mbpsSent = bytesSentCounter.NextValue() * 8 / 1_000_000.0;
                            string downloadStr = software_lang.TSReadLangs("ScreenOverlayTool", "sot_net_download");
                            string uploadStr = software_lang.TSReadLangs("ScreenOverlayTool", "sot_net_upload");
                            await SetLabelTextSafeAsync(OVERLAY_NETWORK_V, $"{downloadStr} {mbpsReceived:0.00} Mbps\n{uploadStr} {mbpsSent:0.00} Mbps");
                        }catch { }
                        //
                        int elapsed = (int)(DateTime.Now - startTime).TotalMilliseconds;
                        int nextDelay = Math.Max(10, 750 - elapsed);
                        try{
                            await Task.Delay(nextDelay);
                        }catch (TaskCanceledException){
                            break;
                        }
                    }
                }
            }catch (Exception) { }
        }
        private static string GetActiveNetworkAdapter(){
            try{
                ManagementObjectSearcher searcher_active_net = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, Index FROM Win32_NetworkAdapter WHERE NetConnectionStatus = 2");
                foreach (ManagementObject search_ip_enabled in searcher_active_net.Get().Cast<ManagementObject>()){
                    ManagementObjectSearcher configSearcher = new ManagementObjectSearcher("root\\CIMV2", $"SELECT IPEnabled FROM Win32_NetworkAdapterConfiguration WHERE Index={search_ip_enabled["Index"]} AND IPEnabled=True");
                    foreach (ManagementObject configObj in configSearcher.Get().Cast<ManagementObject>()){
                        return search_ip_enabled["Name"].ToString().Trim();
                    }
                }
            }catch (Exception) { }
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
            if (label == null || label.IsDisposed || !label.IsHandleCreated)
                return Task.CompletedTask;
            if (label.InvokeRequired){
                label.BeginInvoke(new Action(() =>{
                    if (!label.IsDisposed) label.Text = text;
                }));
                return Task.CompletedTask;
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