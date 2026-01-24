// ======================================================================================================
// Glow - System Analysis Software
// © Copyright 2019-2026, Eray Türkay.
// Project Type: Open Source
// License: MIT License
// Website: https://www.turkaysoftware.com/glow
// GitHub: https://github.com/turkaysoftware/glow
// ======================================================================================================

using Microsoft.VisualBasic.Devices;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
// TS Modules
using Glow.glow_tools;
using static Glow.TSModules;
 
namespace Glow{
    public partial class GlowMain : Form{
        public GlowMain(){
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            // LANGUAGE SET TAGS
            // ==================
            arabicToolStripMenuItem.Tag = "ar";
            chineseToolStripMenuItem.Tag = "zh";
            englishToolStripMenuItem.Tag = "en";
            dutchToolStripMenuItem.Tag = "nl";
            frenchToolStripMenuItem.Tag = "fr";
            germanToolStripMenuItem.Tag = "de";
            hindiToolStripMenuItem.Tag = "hi";
            italianToolStripMenuItem.Tag = "it";
            japaneseToolStripMenuItem.Tag = "ja";
            koreanToolStripMenuItem.Tag = "ko";
            polishToolStripMenuItem.Tag = "pl";
            portugueseToolStripMenuItem.Tag = "pt";
            russianToolStripMenuItem.Tag = "ru";
            spanishToolStripMenuItem.Tag = "es";
            turkishToolStripMenuItem.Tag = "tr";
            // LANGUAGE SET EVENTS
            // ==================
            arabicToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            chineseToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            englishToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            dutchToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            frenchToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            germanToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            hindiToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            italianToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            japaneseToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            koreanToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            polishToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            portugueseToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            russianToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            spanishToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            turkishToolStripMenuItem.Click += LanguageToolStripMenuItem_Click;
            // DYNAMIC THEME LISTENER
            // ==================
            SystemEvents.UserPreferenceChanged += (s, e) => TSUseSystemTheme();
            // OS MODE & ROOT DIRECTORY
            // ==================
            try{
                windows_mode = new ManagementObjectSearcher("root\\CIMV2", "SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>().Select(mo => mo["Caption"]?.ToString()?.ToLower()).FirstOrDefault()?.Contains("windows 11") == true ? 1 : 0;
                windows_disk = Path.GetPathRoot(Environment.ExpandEnvironmentVariables("%SystemRoot%")).Trim();
            }catch{ }
            // PAGE CONTROL FOR PARALLEL LOADING PROCEDURE
            // ==================
            var rotateButtons = new[] {
                OS_RotateBtn, MB_RotateBtn, CPU_RotateBtn, RAM_RotateBtn, GPU_RotateBtn,
                DISK_RotateBtn, NET_RotateBtn, USB_RotateBtn, SOUND_RotateBtn,
                BATTERY_RotateBtn, OSD_RotateBtn, SERVICES_RotateBtn, INSTALLED_RotateBtn, PRINT_RotateBtn
            };
            foreach (var r_btns in rotateButtons) r_btns.Enabled = false;
            var rotatePages = new[] {
                /*(Control)OS,*/ (Control)MB, (Control)CPU, (Control)RAM, (Control)GPU,
                (Control)DISK, (Control)NETWORK, (Control)USB, (Control)SOUND,
                (Control)BATTERY, (Control)DRIVERS, (Control)SERVICES, (Control)INSTAPPS, (Control)EXPORT
            };
            foreach (var r_pages in rotatePages) r_pages.Enabled = false;
        }
        // GLOBAL VARIABLES
        // ======================================================================================================
        public static string lang, lang_path, wp_rotate, windows_disk = @"C:\";
        public static int theme, themeSystem, monitor_engine_mode;
        public static bool CPUbenchMode = false, DISKbenchMode = false, RAMbenchMode = false, BTfinderMode = false, SFCandDISMprocessStatus = false;
        // VARIABLES
        // ======================================================================================================
        private int menu_btns = 1, menu_rp = 1, startup_status, hiding_status, hiding_mode_wrapper;
        private readonly int windows_mode = 0;
        private bool loop_status = true, laptop_mode = false, ts_token_engine_stopper = false, cpu_virtual_mod;
        private string iapps_unknown;
        private decimal battery_fullChargedCapacity_mWh = 0;
        private const decimal battery_MIN_POWER_THRESHOLD = 0.1m;
        private readonly string ts_wizard_name = "TS Wizard";
        private readonly List<Label> allCopyableLabels = new List<Label>();
        // VISIBLE MODE DYNAMIC STAR
        // ======================================================================================================
        private static readonly List<int> vn_range = new List<int>(){ 10, 24 };
        private static readonly Random vis_m_property = new Random();
        // ======================================================================================================
        // UI COLORS
        private readonly List<Color> btn_colors_active = new List<Color>(){ Color.Transparent };
        private static readonly List<Color> header_colors = new List<Color>() { Color.Transparent, Color.Transparent, Color.Transparent };
        // HEADER SETTINGS
        // ======================================================================================================
        private class HeaderMenuColors : ToolStripProfessionalRenderer{
            public HeaderMenuColors() : base(new HeaderColors()){ }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e){ e.ArrowColor = header_colors[1]; base.OnRenderArrow(e); }
            protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e){
                Graphics g = e.Graphics;
                g.SmoothingMode = SmoothingMode.AntiAlias;
                float dpiScale = g.DpiX / 96f;
                Rectangle rect = e.ImageRectangle;
                using (Pen anti_alias_pen = new Pen(header_colors[2], 2.2f * dpiScale)){
                    anti_alias_pen.StartCap = LineCap.Round;
                    anti_alias_pen.EndCap = LineCap.Round;
                    anti_alias_pen.LineJoin = LineJoin.Round;
                    PointF p1 = new PointF(rect.Left + rect.Width * 0.18f, rect.Top + rect.Height * 0.52f);
                    PointF p2 = new PointF(rect.Left + rect.Width * 0.38f, rect.Top + rect.Height * 0.72f);
                    PointF p3 = new PointF(rect.Left + rect.Width * 0.78f, rect.Top + rect.Height * 0.28f);
                    g.DrawLines(anti_alias_pen, new[] { p1, p2, p3 });
                }
            }
        }
        private class HeaderColors : ProfessionalColorTable{
            public override Color MenuItemSelected => header_colors[0];
            public override Color ToolStripDropDownBackground => header_colors[0];
            public override Color ImageMarginGradientBegin => header_colors[0];
            public override Color ImageMarginGradientEnd => header_colors[0];
            public override Color ImageMarginGradientMiddle => header_colors[0];
            public override Color MenuItemSelectedGradientBegin => header_colors[0];
            public override Color MenuItemSelectedGradientEnd => header_colors[0];
            public override Color MenuItemPressedGradientBegin => header_colors[0];
            public override Color MenuItemPressedGradientMiddle => header_colors[0];
            public override Color MenuItemPressedGradientEnd => header_colors[0];
            public override Color MenuItemBorder => header_colors[0];
            public override Color CheckBackground => header_colors[0];
            public override Color ButtonSelectedBorder => header_colors[0];
            public override Color CheckSelectedBackground => header_colors[0];
            public override Color CheckPressedBackground => header_colors[0];
            public override Color MenuBorder => header_colors[0];
            public override Color SeparatorLight => header_colors[1];
            public override Color SeparatorDark => header_colors[1];
        }
        // TOOLTIP SETTINGS
        // ======================================================================================================
        private void MainToolTip_Draw(object sender, DrawToolTipEventArgs e){ e.DrawBackground(); e.DrawBorder(); e.DrawText(); }
        // DYNAMIC CLICK AND COPY SYSTEM
        // ======================================================================================================
        private void TSCAC_Properties(){
            try{
                var OSLabels = new Label[] { OS_SystemUser_V, OS_ComputerName_V, OS_Name_V, OS_Manufacturer_V, OS_SystemVersion_V, OS_SystemArchitectural_V, OS_Country_V, OS_TimeZone_V, OS_EncryptionType_V, OS_SystemTime_V, OS_Install_V, OS_SystemWorkTime_V, OS_LastBootTime_V, OS_SystemLastShutDown_V, OS_PrimaryOS_V, OS_PortableOS_V, OS_MouseWheelStatus_V, OS_ScrollLockStatus_V, OS_NumLockStatus_V, OS_CapsLockStatus_V, OS_FastBoot_V, OS_WinPageFile_V, OS_TempWinPageFile_V, OS_Hiberfil_V, OS_AVProgram_V, OS_FirewallProgram_V, OS_AntiSpywareProgram_V, OS_WinDefCoreIsolation_V, OS_CA2023_Status_V, OS_CA2023_Capable_V, OS_CA2023_Error_V, OS_ActivePower_V, OS_ActivePowerGUID_V, OS_ActivePowerScreenTimeOutP_V, OS_ActivePowerScreenTimeOutB_V, OS_ActivePowerSleepTimeP_V, OS_ActivePowerSleepTimeB_V, OS_MSEdge_V, OS_MSEdgeWebView_V, OS_MSStoreVersion_V, OS_MSOfficeVersion_V, OS_WinActiveChannel_V, OS_NETFrameworkVersion_V, OS_Minidump_V, OS_BSODDate_V };
                var MBLabels = new Label[] { MB_MotherBoardName_V, MB_MotherBoardMan_V, MB_SystemModelMan_V, MB_SystemModelFamily_V, MB_SystemFamily_V, MB_SystemModel_V, MB_Chipset_V, MB_BiosManufacturer_V, MB_BiosDate_V, MB_BiosVersion_V, MB_SmBiosVersion_V, MB_BiosMode_V, MB_LastBIOSTime_V, MB_SecureBoot_V, MB_SecureBootCA2023_V, MB_TPMStatus_V, MB_TPMPhysicalVersion_V, MB_TPMMan_V, MB_TPMManVersion_V, MB_TPMManFullVersion_V, MB_TPMManPublisher_V };
                var CPULabels = new Label[] { CPU_Name_V, CPU_Manufacturer_V, CPU_Architectural_V, CPU_IntelME_V, CPU_NormalSpeed_V, CPU_DefaultSpeed_V, CPU_L1_V, CPU_L2_V, CPU_L3_V, CPU_CoreCount_V, CPU_LogicalCore_V, CPU_Usage_V, CPU_Process_V, CPU_Threads_V, CPU_Handles_V, CPU_SocketDefinition_V, CPU_Family_V, CPU_Virtualization_V, CPU_HyperV_V, CPU_VMExtension_V };
                var RAMLabels = new Label[] { RAM_TotalRAM_V, RAM_UsageRAMCount_V, RAM_EmptyRamCount_V, RAM_TotalVirtualRam_V, RAM_UsageVirtualRam_V, RAM_EmptyVirtualRam_V, RAM_SlotStatus_V, RAM_Amount_V, RAM_Type_V, RAM_Frequency_V, RAM_Volt_V, RAM_FormFactor_V, RAM_Manufacturer_V, RAM_BankLabel_V, RAM_DataWidth_V, RAM_BellekType_V };
                var GPULabels = new Label[] { GPU_Manufacturer_V, GPU_VRAM_V, GPU_Version_V, GPU_DriverDate_V, GPU_Status_V, GPU_DeviceID_V, GPU_DacType_V, GPU_GraphicDriversName_V, GPU_InfFileName_V, GPU_INFSectionFile_V, GPU_CurrentColor_V, GPU_MonitorUserFriendlyName_V, GPU_MonitorManName_V, GPU_MonitorProductCodeID_V, GPU_MonitorConType_V, GPU_MonitorManfDate_V, GPU_MonitorManfDateWeek_V, GPU_MonitorHID_V, GPU_MonitorResLabel_V, GPU_MonitorVirtualRes_V, GPU_MonitorBounds_V, GPU_MonitorWorking_V, GPU_ScreenRefreshRate_V, GPU_ScreenBit_V, GPU_MonitorPrimary_V };
                var DISKLabels = new Label[] { DISK_Model_V, DISK_Man_V, DISK_VolumeID_V, DISK_VolumeName_V, DISK_Firmware_V, DISK_Size_V, DISK_FreeSpace_V, DISK_FileSystem_V, DISK_FormattingType_V, DISK_Type_V, DISK_DriveType_V, DISK_InterFace_V, DISK_PartitionCount_V, DISK_MediaLoaded_V, DISK_MediaStatus_V, DISK_Health_V, DISK_Boot_V, DISK_Bootable_V, DISK_BitLockerStatus_V, DISK_BitLockerConversionStatus_V, DISK_BitLockerEncryptMehod_V, DISK_DriveCompressed_V };
                var NETLabels = new Label[] { NET_LT_Device_V, NET_LT_BandWidth_V, NET_LT_LocalIP_V, NET_LT_GatewayIP_V, NET_NetMan_V, NET_DriverVersion_V, NET_DriverDate_V, NET_ServiceName_V, NET_AdapterType_V, NET_Physical_V, NET_DeviceID_V, NET_ConnectionType_V, NET_Dhcp_status_V, NET_Dhcp_server_V, NET_DHCPFirstIpTime_V, NET_DHCPLastIpTime_V, NET_LocalConSpeed_V };
                var USBLabels = new Label[] { USB_ConName_V, USB_ConMan_V, USB_ConDeviceID_V, USB_ConPNPDeviceID_V, USB_ConDeviceStatus_V, USB_DeviceName_V, USB_DeviceMan_V, USB_DriverVersion_V, USB_DriverDate_V, USB_InfFile_V, USB_DeviceID_V, USB_HardwareID_V };
                var SOUNDLabels = new Label[] { SOUND_DeviceName_V, SOUND_DeviceManufacturer_V, SOUND_DriverVersion_V, SOUND_DriverDate_V, SOUND_DeviceID_V, SOUND_PNPDeviceID_V, SOUND_DeviceStatus_V };
                var BATTERYLabels = new Label[] { BATTERY_Status_V, BATTERY_Model_V,  BATTERY_Chemistry_V, BATTERY_DesignCapacity_V, BATTERY_FullChargeCapacity_V, BATTERY_RemainingChargeCapacity_V, BATTERY_Voltage_V, BATTERY_ChargePower_V, BATTERY_ChargeCurrent_V, BATTERY_DeChargePower_V, BATTERY_DeChargeCurrent_V };
                //
                allCopyableLabels.Clear();
                allCopyableLabels.AddRange(OSLabels);
                allCopyableLabels.AddRange(MBLabels);
                allCopyableLabels.AddRange(CPULabels);
                allCopyableLabels.AddRange(RAMLabels);
                allCopyableLabels.AddRange(GPULabels);
                allCopyableLabels.AddRange(DISKLabels);
                allCopyableLabels.AddRange(NETLabels);
                allCopyableLabels.AddRange(USBLabels);
                allCopyableLabels.AddRange(SOUNDLabels);
                allCopyableLabels.AddRange(BATTERYLabels);
                //
                if (hiding_mode_wrapper != 1){
                    var HIDINGLabels = new Label[] { OS_SavedUser_V, OS_DeviceID_V, OS_Serial_V, OS_WinKey_V, OS_Wallpaper_V, MB_DeviceSerialNumber_V, MB_MotherBoardSerial_V, MB_SystemSKU_V, MB_TPMManID_V, CPU_SerialName_V, RAM_Serial_V, RAM_PartNumber_V, GPU_MonitorSerialNumberID_V, DISK_Serial_V, DISK_VolumeSerial_V, NET_MacAdress_V, NET_Guid_V, NET_IPv4Adress_V, NET_IPv6Adress_V, NET_DNS1_V, NET_DNS2_V, USB_DeviceGUID_V, BATTERY_Serial_V };
                    allCopyableLabels.AddRange(HIDINGLabels);
                }
                //
                TSInitLabels(allCopyableLabels);
                TSSetTooltips(allCopyableLabels);
            }catch (Exception) { }
        }
        private void TSInitLabels(IEnumerable<Label> labelList){
            foreach (Label label in labelList){
                label.Cursor = Cursors.Hand;
                label.DoubleClick -= TSLabel_DoubleClick;
                label.DoubleClick += TSLabel_DoubleClick;
            }
        }
        private void TSSetTooltips(IEnumerable<Label> labelList){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            string tooltipText = software_lang.TSReadLangs("ContentCrossFeature", "ccf_copy_hover");
            foreach (Label label in labelList){
                MainToolTip.SetToolTip(label, tooltipText);
            }
        }
        private void TSLabel_DoubleClick(object sender, EventArgs e){
            if (sender is Label label){
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                Clipboard.SetText(label.Text);
                TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("ContentCrossFeature", "ccf_copy_message"), label.Text));
            }
        }
        // UI DPI CHANGER
        // ======================================================================================================
        private int TSDPIChanger(int size){
            return (int)(size * this.DeviceDpi / 96f);
        }
        // LOAD SOFTWARE SETTINGS
        // ======================================================================================================
        private async void RunSoftwareEngine(){
            HeaderMenu.Cursor = Cursors.Hand;
            if (this.DeviceDpi / 96f >= 1.5){
                HeaderPanel.Padding = new Padding(TSDPIChanger(4), 0, 0, 0);
            }else if (this.DeviceDpi / 96f >= 1.25){
                HeaderPanel.Padding = new Padding(TSDPIChanger(3), 0, 0, 0);
            }
            if (this.DeviceDpi / 96f >= 1.25){
                OS.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                MB.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                CPU.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                RAM.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                GPU.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                DISK.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                NETWORK.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                USB.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                SOUND.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                BATTERY.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                DRIVERS.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                SERVICES.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                INSTAPPS.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
                EXPORT.Padding = new Padding(TSDPIChanger(2), TSDPIChanger(2), TSDPIChanger(0), TSDPIChanger(4));
            }
            // INSTALLED DRIVERS
            // ======================================================================================================
            for (int i = 0; i < 6; i++) OSD_DataMainTable.Columns.Add($"osd_file_{i}", "osd_variable");
            foreach (DataGridViewColumn OSD_Column in OSD_DataMainTable.Columns) OSD_Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            // SERVICES
            // ======================================================================================================
            for (int i = 0; i < 6; i++) SERVICE_DataMainTable.Columns.Add($"ss_file_{i}", "ss_variable");
            foreach (DataGridViewColumn SERVICE_Column in SERVICE_DataMainTable.Columns) SERVICE_Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            // INSTALLED APPS
            // ======================================================================================================
            INSTAPPS_DataMainTable.Columns.Add(new DataGridViewImageColumn { Name = "Icon", Width = 40, ImageLayout = DataGridViewImageCellLayout.Zoom });
            for (int i = 0; i < 6; i++) INSTAPPS_DataMainTable.Columns.Add($"iapps_file_{i}", "iapps_variable");
            foreach (DataGridViewColumn OSD_Column in INSTAPPS_DataMainTable.Columns) OSD_Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            // ALL DGV WIDTH - HEIGHT - PADDING
            // ======================================================================================================
            int[] columnWidths = { 160, 120, 120, 120, 100, 100 };
            int[] columnWidthsApps = { 65, 175, 100, 125, 75, 75, 150 };
            for (int i = 0; i < columnWidths.Length; i++){
                OSD_DataMainTable.Columns[i].Width = (int)(columnWidths[i] * this.DeviceDpi / 96f);
                SERVICE_DataMainTable.Columns[i].Width = (int)(columnWidths[i] * this.DeviceDpi / 96f);
            }
            for (int i = 0; i < columnWidthsApps.Length; i++){
                INSTAPPS_DataMainTable.Columns[i].Width = (int)(columnWidthsApps[i] * this.DeviceDpi / 96f);
            }
            //
            OSD_DataMainTable.RowTemplate.Height = (int)(32 * this.DeviceDpi / 96f);
            SERVICE_DataMainTable.RowTemplate.Height = (int)(32 * this.DeviceDpi / 96f);
            INSTAPPS_DataMainTable.RowTemplate.Height = (int)(36 * this.DeviceDpi / 96f);
            //
            int scaledPadding = (int)(3 * this.DeviceDpi / 96f);
            foreach (var tableMod in new[] { OSD_DataMainTable, SERVICE_DataMainTable, INSTAPPS_DataMainTable }){
                foreach (DataGridViewColumn columnPadding in tableMod.Columns){
                    if (tableMod == INSTAPPS_DataMainTable && columnPadding.Index == 0)
                        continue;
                    columnPadding.DefaultCellStyle.Padding = new Padding(scaledPadding, 0, 0, 0);
                }
            }
            // OSD - SERVICE - INSTAPP CLEAR BTN DPI HEIGHT
            // ======================================================================================================
            OSD_TextBoxClearBtn.Height = OSD_TextBox.Height + 2;
            SERVICE_TextBoxClearBtn.Height = SERVICE_TextBox.Height + 2;
            INSTAPPS_TextBoxClearBtn.Height = INSTAPPS_TextBox.Height + 2;
            // DGV DOUBLE BUFFER
            // ======================================================================================================
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, OSD_DataMainTable, new object[]{ true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, SERVICE_DataMainTable, new object[]{ true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, INSTAPPS_DataMainTable, new object[]{ true });
            // THEME - LANG - STARTUP - HIDIN MODE PRELOADER
            // ======================================================================================================
            TSSettingsSave software_read_settings = new TSSettingsSave(ts_sf);
            //
            int theme_mode = int.TryParse(software_read_settings.TSReadSettings(ts_settings_container, "ThemeStatus"), out int the_status) && (the_status == 0 || the_status == 1 || the_status == 2) ? the_status : 1;
            if (theme_mode == 2) { themeSystem = 2; Theme_engine(GetSystemTheme(2)); } else Theme_engine(theme_mode);
            darkThemeToolStripMenuItem.Checked = theme_mode == 0;
            lightThemeToolStripMenuItem.Checked = theme_mode == 1;
            systemThemeToolStripMenuItem.Checked = theme_mode == 2;
            //
            string lang_mode = software_read_settings.TSReadSettings(ts_settings_container, "LanguageStatus");
            var languageFiles = new Dictionary<string, (object langResource, ToolStripMenuItem menuItem, bool fileExists)>{
                { "ar", (ts_lang_ar, arabicToolStripMenuItem, File.Exists(ts_lang_ar)) },
                { "zh", (ts_lang_zh, chineseToolStripMenuItem, File.Exists(ts_lang_zh)) },
                { "en", (ts_lang_en, englishToolStripMenuItem, File.Exists(ts_lang_en)) },
                { "nl", (ts_lang_nl, dutchToolStripMenuItem, File.Exists(ts_lang_nl)) },
                { "fr", (ts_lang_fr, frenchToolStripMenuItem, File.Exists(ts_lang_fr)) },
                { "de", (ts_lang_de, germanToolStripMenuItem, File.Exists(ts_lang_de)) },
                { "hi", (ts_lang_hi, hindiToolStripMenuItem, File.Exists(ts_lang_hi)) },
                { "it", (ts_lang_it, italianToolStripMenuItem, File.Exists(ts_lang_it)) },
                { "ja", (ts_lang_ja, japaneseToolStripMenuItem, File.Exists(ts_lang_ja)) },
                { "ko", (ts_lang_ko, koreanToolStripMenuItem, File.Exists(ts_lang_ko)) },
                { "pl", (ts_lang_pl, polishToolStripMenuItem, File.Exists(ts_lang_pl)) },
                { "pt", (ts_lang_pt, portugueseToolStripMenuItem, File.Exists(ts_lang_pt)) },
                { "ru", (ts_lang_ru, russianToolStripMenuItem, File.Exists(ts_lang_ru)) },
                { "es", (ts_lang_es, spanishToolStripMenuItem, File.Exists(ts_lang_es)) },
                { "tr", (ts_lang_tr, turkishToolStripMenuItem, File.Exists(ts_lang_tr)) },
            };
            foreach (var langLoader in languageFiles){ langLoader.Value.menuItem.Enabled = langLoader.Value.fileExists; }
            var (langResource, selectedMenuItem, _) = languageFiles.ContainsKey(lang_mode) ? languageFiles[lang_mode] : languageFiles["en"];
            Lang_engine(Convert.ToString(langResource), lang_mode);
            selectedMenuItem.Checked = true;
            //
            string startup_mode = software_read_settings.TSReadSettings(ts_settings_container, "StartupStatus");
            startup_status = int.TryParse(startup_mode, out int str_status) && (str_status == 0 || str_status == 1) ? str_status : 0;
            WindowState = startup_status == 1 ? FormWindowState.Maximized : FormWindowState.Normal;
            windowedToolStripMenuItem.Checked = startup_status == 0;
            fullScreenToolStripMenuItem.Checked = startup_status == 1;
            //
            string hiding_mode = software_read_settings.TSReadSettings(ts_settings_container, "HidingStatus");
            hiding_status = int.TryParse(hiding_mode, out int hid_status) && (hid_status == 0 || hid_status == 1) ? hid_status : 0;
            hiding_mode_wrapper = hiding_status;
            hidingModeOffToolStripMenuItem.Checked = hiding_status == 0;
            hidingModeOnToolStripMenuItem.Checked = hiding_status == 1;
            // PARALLEL LOADER START
            await GlowBootstrapper();
        }
        // TS PARALLEL RUN SECTION MODULE
        // ======================================================================================================
        private async Task RunSec(Action __action){
            var cnToken = Program.TS_TokenEngine.Token;
            if (!cnToken.IsCancellationRequested){
                try{
                    await Task.Run(__action, cnToken);
                }catch (OperationCanceledException){
                    if (Program.debug_mode) Console.WriteLine("Task canceled.");
                }catch (Exception ex){
                    if (Program.debug_mode) Console.WriteLine($"Task error: {ex.Message}");
                }
            }
        }
        private async Task RunSec(Func<Task> __action){
            var cnToken = Program.TS_TokenEngine.Token;
            if (!cnToken.IsCancellationRequested){
                try{
                    await Task.Run(async () => await __action(), cnToken);
                }catch (OperationCanceledException){
                    if (Program.debug_mode) Console.WriteLine("Task canceled.");
                }catch (Exception ex){
                    if (Program.debug_mode) Console.WriteLine($"Task error: {ex.Message}");
                }
            }
        }
        // TASK ALL PROCESS
        // ======================================================================================================
        private async Task GlowBootstrapper(){
            // PROPERTIES
            // ---------------
            var cnToken = Program.TS_TokenEngine.Token;
            var btTime = Stopwatch.StartNew();
            var tList = new List<Task>();
            // RUN PARALLEL START
            // ---------------
            try{
                tList.Add(RunSec(() => Os()));
                tList.Add(RunSec(() => Mb()));
                tList.Add(RunSec(() => Cpu()));
                tList.Add(RunSec(() => Ram()));
                tList.Add(RunSec(() => Gpu()));
                tList.Add(RunSec(() => Disk()));
                tList.Add(RunSec(() => Network()));
                tList.Add(RunSec(() => Usb()));
                tList.Add(RunSec(() => Sound()));
            }catch (Exception){ }
            try{
                laptop_mode = SystemInformation.PowerStatus.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery;
                if (laptop_mode){
                    Battery_visible_on();
                    tList.Add(RunSec(Battery));
                    tList.Add(RunSec(BatteryBgProcess));
                }else{
                    Battery_visible_off();
                    //
                    void EnableBatteryControls(){
                        BATTERY_RotateBtn.Enabled = true;
                        ((Control)BATTERY).Enabled = true;
                    }
                    if (MainContent.InvokeRequired) MainContent.Invoke((Action)EnableBatteryControls);
                    else EnableBatteryControls();
                    //
                    if (Program.debug_mode) Console.WriteLine("<--- Battery Section Loaded / Desktop Mode --->");
                }
            }catch (Exception){ }
            try{
                tList.Add(RunSec(() => InstalledDrivers()));
                tList.Add(RunSec(() => InstalledServices()));
                tList.Add(RunSec(() => InstalledApps()));
                //
                tList.Add(RunSec(() => OsBgProcess()));
                tList.Add(RunSec(() => ProcessorBgProcess()));
                tList.Add(RunSec(() => CpuBgProcess()));
                tList.Add(RunSec(() => RamBgProcess()));
                tList.Add(RunSec(() => NetBgProcess()));
                tList.Add(RunSec(() => NetBGProcessGateway()));
                //
                tList.Add(RunSec(() => ExportModsAdd()));
            }catch (Exception){ }
            // RUN PARALLEL PROCESS END
            // ---------------
            try{
                await Task.WhenAll(tList);
                if (MainContent.InvokeRequired){
                    MainContent.Invoke(new Action(() => { ((Control)EXPORT).Enabled = true; PRINT_RotateBtn.Enabled = true; }));
                }else{
                    ((Control)EXPORT).Enabled = true; PRINT_RotateBtn.Enabled = true;
                }
                btTime.Stop();
                if (Program.debug_mode){
                    Console.WriteLine("<--- Export Section Loaded --->");
                    Console.WriteLine("<--------------------------->");
                    Console.WriteLine("<--- ALL TASKS COMPLETED --->");
                    Console.WriteLine($"<--- Total Load Time: {btTime.Elapsed.TotalSeconds:F2} seconds --->");
                    Console.WriteLine("<--------------------------->");
                }
            }catch (Exception){ }
        }
        // SOFTWARE LOAD
        // ======================================================================================================
        private void Glow_Load(object sender, EventArgs e){ 
            Text = TS_VersionEngine.TS_SofwareVersion(0);
            // LAUNCH PROCESS 
            // ====================================
            RunSoftwareEngine();
            // SOFTWARE UPDATE CHECK
            // ====================================
            Task.Run(() => Software_update_check(0));
            // CLICKABLE AND COPYABLE LABELS
            // ====================================
            Task.Run(() => TSCAC_Properties());
        }
        #region OS_Section
        // OPERATING SYSTEM
        // ======================================================================================================
        readonly List<string> minidump_files_list = new List<string>();
        readonly List<string> minidump_files_date_list = new List<string>();
        readonly string mdp_1 = Path.Combine(windows_disk, @"Windows\Minidump");
        readonly string mdp_2 = Path.Combine(windows_disk, @"Windows\memory.dmp");
        private void Os(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher search_pfu = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PageFileUsage");
            ManagementObjectSearcher search_av = new ManagementObjectSearcher("root\\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
            ManagementObjectSearcher search_fw = new ManagementObjectSearcher("root\\SecurityCenter2", "SELECT * FROM FirewallProduct");
            ManagementObjectSearcher search_as = new ManagementObjectSearcher("root\\SecurityCenter2", "SELECT * FROM AntiSpywareProduct");
            try{
                // SYSTEM USER
                OS_SystemUser_V.Text = Environment.UserName;
            }catch (Exception){ }
            try{
                // PC NAME
                OS_ComputerName_V.Text = Dns.GetHostName();
            }catch (Exception){ }
            foreach (ManagementObject query_os_rotate in search_os.Get().Cast<ManagementObject>()){
                try{
                    // REGISTERED USER
                    string os_saved_user = Convert.ToString(query_os_rotate["RegisteredUser"]).Trim();
                    if (hiding_mode_wrapper != 1){
                        OS_SavedUser_V.Text = os_saved_user;
                    }else{
                        OS_SavedUser_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                    }
                }catch (Exception){ }
                try{
                    // OS NAME
                    string os_name = Convert.ToString(query_os_rotate["Caption"]);
                    OS_Name_V.Text = os_name;
                }catch (Exception){ }
                try{
                    // OS MANUFACTURER
                    OS_Manufacturer_V.Text = Convert.ToString(query_os_rotate["Manufacturer"]);
                }catch (Exception){ }
                try{
                    // OS VERSION
                    const string regPath = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion";
                    string displayVersion = Convert.ToString(Registry.GetValue(regPath, "DisplayVersion", ""))?.Trim();
                    string build = Convert.ToString(Registry.GetValue(regPath, "CurrentBuild", ""))?.Trim();
                    string ubr = Convert.ToString(Registry.GetValue(regPath, "UBR", ""))?.Trim();
                    if (!string.IsNullOrEmpty(displayVersion) && !string.IsNullOrEmpty(build) && !string.IsNullOrEmpty(ubr))
                        OS_SystemVersion_V.Text = $"{displayVersion} ({build}.{ubr})";
                    else
                        OS_SystemVersion_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_unknown");
                }catch (Exception){ }
                try{
                    // OS ARCHITECTURE
                    string system_bit = Regex.Replace(Convert.ToString(query_os_rotate["OSArchitecture"]), @"\D", string.Empty).Trim();
                    OS_SystemArchitectural_V.Text = system_bit + " " + software_lang.TSReadLangs("Os_Content", "os_c_bit") + " - " + string.Format("(x{0})", system_bit);
                }catch (Exception){ }
                try{
                    // OS DEVICE ID
                    string os_device_id = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\SQMClient", "MachineId", "").ToString().Trim();
                    if (!string.IsNullOrEmpty(os_device_id)){
                        string os_device_id_replacer = os_device_id.Replace("{", string.Empty).Replace("}", string.Empty);
                        if (hiding_mode_wrapper != 1){
                            OS_DeviceID_V.Text = os_device_id_replacer;
                        }else{
                            OS_DeviceID_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                        }
                    }else{
                        OS_DeviceID_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_unknown");
                    }
                }catch (Exception){ }
                try{
                    // OS SERIAL
                    string os_serial = Convert.ToString(query_os_rotate["SerialNumber"]).Trim();
                    if (hiding_mode_wrapper != 1){
                        OS_Serial_V.Text = os_serial;
                    }else{
                        OS_Serial_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                    }
                }catch (Exception){ }
                try{
                    // SYSTEM COUNTRY AND LANGUAGE
                    CultureInfo culture_info = CultureInfo.InstalledUICulture;
                    OS_Country_V.Text = culture_info.DisplayName.Trim();
                }catch (Exception){ }
                try{
                    // SYSTEM TIME ZONE
                    TimeZone os_time_zone = TimeZone.CurrentTimeZone;
                    OS_TimeZone_V.Text = os_time_zone.StandardName;
                }catch (Exception){ }
                try{
                    // OS ENCRYPTION BIT VALUE
                    OS_EncryptionType_V.Text = Convert.ToString(query_os_rotate["EncryptionLevel"]) + " " + software_lang.TSReadLangs("Os_Content", "os_c_bit");
                }catch (Exception){ }
                try{
                    // OS LAST BOOT
                    string last_bt = Convert.ToString(query_os_rotate["LastBootUpTime"]);
                    DateTime last_boot_time = ManagementDateTimeConverter.ToDateTime(last_bt);
                    OS_LastBootTime_V.Text = $"{last_boot_time:dd.MM.yyyy} - {last_boot_time:HH:mm:ss}";
                }catch (Exception){ }
                try{
                    // OS SHUTDOWN TIME
                    string sd_time_path = @"System\CurrentControlSet\Control\Windows";
                    RegistryKey sd_time_key = Registry.LocalMachine.OpenSubKey(sd_time_path);
                    byte[] sd_time_val = (byte[])sd_time_key.GetValue("ShutdownTime");
                    sd_time_key.Close();
                    long sd_time_as_long = BitConverter.ToInt64(sd_time_val, 0);
                    DateTime shut_down_time = DateTime.FromFileTime(sd_time_as_long);
                    OS_SystemLastShutDown_V.Text = shut_down_time.ToString("dd.MM.yyyy - HH:mm:ss");
                }catch (Exception){ }
                try{
                    // PRIMARY OS STATUS
                    bool system_primary_os_status = Convert.ToBoolean(query_os_rotate["Primary"]);
                    if (system_primary_os_status == true){
                        OS_PrimaryOS_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_yes");
                    }else if (system_primary_os_status == false){
                        OS_PrimaryOS_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_no");
                    }
                }catch (Exception){ }
                try{
                    // PORTABLE OS STATUS
                    bool system_portable_status = Convert.ToBoolean(query_os_rotate["PortableOperatingSystem"]);
                    if (system_portable_status == true){
                        OS_PortableOS_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_yes");
                    }else if (system_portable_status == false){
                        OS_PortableOS_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_no");
                    }
                }catch (Exception){ }
                try{
                    // FASTBOOT STATUS
                    RegistryKey get_fastboot_status = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Power");
                    string get_fs_val = get_fastboot_status?.GetValue("HiberbootEnabled")?.ToString().Trim();
                    string langKey = get_fs_val == "1" ? "os_c_fastboot_active" : get_fs_val == "0" ? "os_c_fastboot_deactive" : "os_c_unknown";
                    OS_FastBoot_V.Text = software_lang.TSReadLangs("Os_Content", langKey);
                    get_fastboot_status?.Close();
                }catch (Exception){ }
                try{
                    // WINDOWS TEMP PAGEFILE
                    string temp_pagefile = Path.Combine(windows_disk, "swapfile.sys");
                    if (File.Exists(temp_pagefile)){
                        OS_TempWinPageFile_V.Text = string.Format("{0} - {1}", temp_pagefile, TS_FormatSize(new FileInfo(temp_pagefile).Length));
                    }else{
                        OS_TempWinPageFile_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_unknown");
                    }
                }catch (Exception){ }
                try{
                    // WINDOWS HIBERFIL FILE
                    string hiberfil_file = Path.Combine(windows_disk, "hiberfil.sys");
                    if (File.Exists(hiberfil_file)){
                        OS_Hiberfil_V.Text = string.Format("{0} - {1}", hiberfil_file, TS_FormatSize(new FileInfo(hiberfil_file).Length));
                    }else{
                        OS_Hiberfil_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_unknown");
                    }
                }catch (Exception){ }
            }
            try{
                // WINDOWS PAGEFILE
                foreach (ManagementObject query_os_pfu in search_pfu.Get().Cast<ManagementObject>()){
                    object pagefile_caption = query_os_pfu["Caption"];
                    object pagefile_name = query_os_pfu["Name"];
                    string pagefile_result;
                    //
                    if (pagefile_caption != null && !string.IsNullOrEmpty(pagefile_caption.ToString())){
                        pagefile_result = string.Format("{0} - {1}", pagefile_caption, TS_FormatSize(new FileInfo(Convert.ToString(pagefile_caption)).Length));
                    }else if (pagefile_name != null && !string.IsNullOrEmpty(pagefile_name.ToString())){
                        pagefile_result = string.Format("{0} - {1}", pagefile_name, TS_FormatSize(new FileInfo(Convert.ToString(pagefile_name)).Length));
                    }else{
                        pagefile_result = software_lang.TSReadLangs("Os_Content", "os_c_unknown");
                    }
                    OS_WinPageFile_V.Text = pagefile_result.Trim();
                }
            }catch (Exception){ }
            try{
                // SYSTEM ANTI-VIRUS PRODUCT
                List<string> av_list = new List<string>();
                foreach (ManagementObject query_av in search_av.Get().Cast<ManagementObject>()){
                    string av_path = Convert.ToString(query_av["pathToSignedProductExe"]).Trim();
                    av_path = Environment.ExpandEnvironmentVariables(av_path);
                    if (string.IsNullOrEmpty(av_path) || !File.Exists(av_path)){
                        av_path = Convert.ToString(query_av["pathToSignedReportingExe"]).Trim();
                        av_path = Environment.ExpandEnvironmentVariables(av_path);
                    }
                    if (!string.IsNullOrEmpty(av_path) && File.Exists(av_path)){
                        string av_name = Convert.ToString(query_av["DisplayName"]).Trim();
                        av_list.Add(av_name);
                    }
                }
                if (av_list.Count < 1){
                    av_list.Add(software_lang.TSReadLangs("Os_Content", "os_c_anti_virus_count"));
                }
                string av_list_split = string.Join(" - ", av_list);
                OS_AVProgram_V.Text = av_list_split.Trim();
            }catch (Exception){ }
            try{
                // SYSTEM FIREWALL PRODUCT
                List<string> fw_list = new List<string>();
                foreach (ManagementObject query_fw in search_fw.Get().Cast<ManagementObject>()){
                    string fw_path = Convert.ToString(query_fw["pathToSignedProductExe"]).Trim();
                    fw_path = Environment.ExpandEnvironmentVariables(fw_path);
                    if (string.IsNullOrEmpty(fw_path) || !File.Exists(fw_path)){
                        fw_path = Convert.ToString(query_fw["pathToSignedReportingExe"]).Trim();
                        fw_path = Environment.ExpandEnvironmentVariables(fw_path);
                    }
                    if (!string.IsNullOrEmpty(fw_path) && File.Exists(fw_path)){
                        string fw_name = Convert.ToString(query_fw["DisplayName"]).Trim();
                        fw_list.Add(fw_name);
                    }
                }
                if (fw_list.Count < 1){
                    fw_list.Add(software_lang.TSReadLangs("Os_Content", "os_c_firewall_count"));
                }
                string fw_list_split = string.Join(" - ", fw_list);
                OS_FirewallProgram_V.Text = fw_list_split.Trim();
            }catch (Exception){ }
            try{
                // SYSTEM ANTI-SPYWARE PRODUCT
                List<string> as_list = new List<string>();
                foreach (ManagementObject query_as in search_as.Get().Cast<ManagementObject>()){
                    string as_path = Convert.ToString(query_as["pathToSignedProductExe"]).Trim();
                    as_path = Environment.ExpandEnvironmentVariables(as_path);
                    if (string.IsNullOrEmpty(as_path) || !File.Exists(as_path)){
                        as_path = Convert.ToString(query_as["pathToSignedReportingExe"]).Trim();
                        as_path = Environment.ExpandEnvironmentVariables(as_path);
                    }
                    if (!string.IsNullOrEmpty(as_path) && File.Exists(as_path)){
                        string as_name = Convert.ToString(query_as["DisplayName"]).Trim();
                        as_list.Add(as_name);
                    }
                }
                if (as_list.Count < 1){
                    as_list.Add(software_lang.TSReadLangs("Os_Content", "os_c_anti_spyware_count"));
                }
                string as_list_split = string.Join(" - ", as_list);
                OS_AntiSpywareProgram_V.Text = as_list_split.Trim();
            }catch (Exception){ }
            try{
                // WINDOWS DEFENDER CORE ISOLATION STATUS
                using (var key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity")){
                    string val = key?.GetValue("Enabled")?.ToString()?.Trim() ?? "0";
                    string langKey = val == "1" ? "os_c_win_core_isolation_active" : "os_c_win_core_isolation_deactive";
                    OS_WinDefCoreIsolation_V.Text = software_lang.TSReadLangs("Os_Content", langKey);
                }
            }catch{ }
            try{
                // https://support.microsoft.com/en-us/topic/registry-key-updates-for-secure-boot-windows-devices-with-it-managed-updates-a7be69c9-4634-42e1-9ca1-df06f43f360d
                // WINDOWS CA 2023 CERT STATUS
                string ca2023_status_result = string.Empty;
                string ca2023_capable_result = string.Empty;
                string ca2023_error_result = string.Empty;
                string __ca2023_unknown = software_lang.TSReadLangs("Os_Content", "os_c_unknown");
                string __ca_2023_not_found_error = software_lang.TSReadLangs("Os_Content", "os_c_ca2023_not_found_error");
                try{
                    using (var get_ca2023 = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\SecureBoot\Servicing", writable: false)){
                        if (get_ca2023 != null){
                            // --- Status (REG_SZ) ---
                            object ca2023_status = get_ca2023.GetValue("UEFICA2023Status", null, RegistryValueOptions.DoNotExpandEnvironmentNames);
                            string ca2023_status_q = (ca2023_status as string) ?? (ca2023_status != null ? Convert.ToString(ca2023_status) : null);
                            ca2023_status_q = (ca2023_status_q ?? string.Empty).Trim();
                            if (ca2023_status_q == "Updated"){
                                ca2023_status_result = software_lang.TSReadLangs("Os_Content", "os_c_ca2023_active");
                            }else if (ca2023_status_q == "InProgress"){
                                ca2023_status_result = software_lang.TSReadLangs("Os_Content", "os_c_ca2023_progress");
                            }else if (ca2023_status_q == "NotStarted"){
                                ca2023_status_result = software_lang.TSReadLangs("Os_Content", "os_c_ca2023_not_started");
                            }else if (!string.IsNullOrEmpty(ca2023_status_q)){
                                ca2023_status_result = __ca2023_unknown + ": " + ca2023_status_q;
                            }else{
                                ca2023_status_result = __ca2023_unknown;
                            }
                            // --- Capable (REG_DWORD) ---
                            object ca2023_capable = get_ca2023.GetValue("WindowsUEFICA2023Capable", null);
                            if (ca2023_capable == null){
                                ca2023_capable_result = __ca2023_unknown;
                            }else{
                                int ca2023_capable_q;
                                try { ca2023_capable_q = (ca2023_capable is int i) ? i : Convert.ToInt32(ca2023_capable); } catch { ca2023_capable_q = int.MinValue; }
                                if (ca2023_capable_q == int.MinValue){
                                    ca2023_capable_result = __ca2023_unknown;
                                }else if (ca2023_capable_q == 0){
                                    ca2023_capable_result = software_lang.TSReadLangs("Os_Content", "os_c_ca2023_c_not_found");
                                }else if (ca2023_capable_q == 1){
                                    ca2023_capable_result = software_lang.TSReadLangs("Os_Content", "os_c_ca2023_c_found");
                                }else if (ca2023_capable_q == 2){
                                    ca2023_capable_result = software_lang.TSReadLangs("Os_Content", "os_c_ca2023_active");
                                }else{
                                    ca2023_capable_result = __ca2023_unknown + ": " + ca2023_capable_q;
                                }
                            }
                            // --- Error (REG_DWORD) ---
                            object ca2023_error = get_ca2023.GetValue("UEFICA2023Error", null);
                            if (ca2023_error == null){
                                ca2023_error_result = __ca_2023_not_found_error;
                            }else{
                                int ca2023_error_q;
                                try { ca2023_error_q = (ca2023_error is int i) ? i : Convert.ToInt32(ca2023_error); } catch { ca2023_error_q = int.MinValue; }
                                if (ca2023_error_q == int.MinValue){
                                    ca2023_error_result = __ca2023_unknown;
                                }else if (ca2023_error_q == 0){
                                    ca2023_error_result = __ca_2023_not_found_error;
                                }else{
                                    ca2023_error_result = __ca2023_unknown + ": " + ca2023_error_q;
                                }
                            }
                        }else{
                            ca2023_status_result = __ca2023_unknown;
                            ca2023_capable_result = __ca2023_unknown;
                            ca2023_error_result = __ca2023_unknown;
                        }
                    }
                }catch{
                    ca2023_status_result = __ca2023_unknown;
                    ca2023_capable_result = __ca2023_unknown;
                    ca2023_error_result = __ca2023_unknown;
                }
                //
                OS_CA2023_Status_V.Text = ca2023_status_result;
                OS_CA2023_Capable_V.Text = ca2023_capable_result;
                OS_CA2023_Error_V.Text = ca2023_error_result;
            }catch (Exception){ }
            try{
                // ACTIVE POWER PLAN
                string powerOutput;
                string activePlanName = null;
                string activePlanGuid = null;
                //
                using (var get_power_mode = new Process()){
                    get_power_mode.StartInfo.FileName = "powercfg";
                    get_power_mode.StartInfo.Arguments = "/L";
                    get_power_mode.StartInfo.RedirectStandardOutput = true;
                    get_power_mode.StartInfo.UseShellExecute = false;
                    get_power_mode.StartInfo.CreateNoWindow = true;
                    get_power_mode.Start();
                    powerOutput = get_power_mode.StandardOutput.ReadToEnd();
                    get_power_mode.WaitForExit();
                }
                //
                var split_power_line = powerOutput.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                //
                foreach (var target_power_line in split_power_line){
                    if (target_power_line.Contains("GUID:") && target_power_line.Trim().EndsWith("*")){
                        var planMatch = Regex.Match(target_power_line, @"\((.*?)\)");
                        if (planMatch.Success){
                            activePlanName = planMatch.Groups[1].Value;
                        }
                        var guidMatch = Regex.Match(target_power_line, @"GUID:\s*([a-fA-F0-9\-]{36})");
                        if (guidMatch.Success){
                            activePlanGuid = guidMatch.Groups[1].Value;
                        }
                        break;
                    }
                }
                //
                OS_ActivePower_V.Text = activePlanName != null ? $"{activePlanName}" : software_lang.TSReadLangs("Os_Content", "os_c_a_power_null");
                OS_ActivePowerGUID_V.Text = activePlanGuid != null ? $"{activePlanGuid}" : software_lang.TSReadLangs("Os_Content", "os_c_a_power_null");
                bool isLaptop = SystemInformation.PowerStatus.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery;
                // SCREEN CLOSE AND SLEEP TIME
                if (!string.IsNullOrEmpty(activePlanGuid)){
                    // TIMEOUT
                    int acTimeout = SCQueryTimeout(activePlanGuid, "SUB_VIDEO", "VIDEOIDLE", true);
                    OS_ActivePowerScreenTimeOutP_V.Text = SCFormatTimeout(acTimeout);
                    if (isLaptop){
                        int dcTimeout = SCQueryTimeout(activePlanGuid, "SUB_VIDEO", "VIDEOIDLE", false);
                        OS_ActivePowerScreenTimeOutB_V.Text = SCFormatTimeout(dcTimeout);
                    }else{
                        OS_ActivePowerScreenTimeOutB_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_a_power_desktop");
                    }
                    // SLEE TIME
                    int acSleep = SCQueryTimeout(activePlanGuid, "SUB_SLEEP", "STANDBYIDLE", true);
                    OS_ActivePowerSleepTimeP_V.Text = SCFormatTimeout(acSleep);
                    if (isLaptop){
                        int dcSleep = SCQueryTimeout(activePlanGuid, "SUB_SLEEP", "STANDBYIDLE", false);
                        OS_ActivePowerSleepTimeB_V.Text = SCFormatTimeout(dcSleep);
                    }else{
                        OS_ActivePowerSleepTimeB_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_a_power_desktop");
                    }
                }
            }catch (Exception){ }
            try{
                // MS EDGE VERSION
                string ms_edge_path = Path.Combine(windows_disk, @"Program Files (x86)\Microsoft\Edge\Application\msedge.exe");
                if (File.Exists(ms_edge_path)){
                    FileVersionInfo edgeFileVersionInfo = FileVersionInfo.GetVersionInfo(ms_edge_path);
                    OS_MSEdge_V.Text = edgeFileVersionInfo.ProductVersion.Trim();
                }else{
                    OS_MSEdge_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_msedge_not");
                }
            }catch (Exception){ }
            try{
                // MS EDGE WEBVIEW2 VERSION
                string webview2_path = Path.Combine(windows_disk, @"Program Files (x86)\Microsoft\EdgeWebView\Application");
                if (Directory.Exists(webview2_path)){
                    string[] webview2_subdirectories = Directory.GetDirectories(webview2_path);
                    var webview2_numeric_folders = webview2_subdirectories.Select(get_dir_name => new DirectoryInfo(get_dir_name).Name).Where(get_numeric_name => char.IsDigit(get_numeric_name[0])).OrderByDescending(version_object_sort => Version.Parse(version_object_sort)).ToList();
                    if (webview2_numeric_folders.Count > 0){
                        string webview_lastest_version = webview2_numeric_folders.First();
                        OS_MSEdgeWebView_V.Text = webview_lastest_version.Trim();
                    }else{
                        OS_MSEdgeWebView_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_msedge_webview2_not");
                    }
                }else{
                    OS_MSEdgeWebView_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_msedge_webview2_not");
                }
            }catch (Exception){ }
            try{
                // MS STORE VERSION
                StartMsStoreVersionCheck();
            }catch (Exception){ }
            try{
                // MS OFFICE VERSION
                StartMsOfficeVersionCheck();
            }catch (Exception){ }
            try{
                // WINDOWS LICENSE KEY
                string win_key = TSWindowsProductKey.GetWindowsProductKey().Trim();
                if (!string.IsNullOrEmpty(win_key) && win_key != "NS_OS" && win_key != "NO_SUB_KEY" && win_key != "NO_KEY"){
                    if (hiding_mode_wrapper != 1){
                        OS_WinKey_V.Text = win_key;
                    }else{
                        OS_WinKey_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                    }
                }else{
                    switch (win_key){
                        case "NS_OS":
                            OS_WinKey_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_license_unsupported_os");
                            break;
                        default:
                            OS_WinKey_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_license_null");
                            break;
                    }
                }
            }catch (Exception) { }
            try{
                // WINDOWS LICSENSE TYPE
                StartWindowsLicenseTypeCheck();
            }catch (Exception){ }
            try{
                // .NET FRAMEWORK VERSION
                // https://learn.microsoft.com/tr-tr/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed
                using (RegistryKey get_net_version = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\")){
                    if (get_net_version != null){
                        object releaseKey = get_net_version.GetValue("Release");
                        if (releaseKey != null){
                            int releaseKeyValue = (int)releaseKey;
                            var netFrameworkVersions = new Dictionary<int, string>{
                                { 533320, "4.8.1" },
                                { 528040, "4.8" },
                                { 461808, "4.7.2" },
                                { 461308, "4.7.1" },
                                { 460798, "4.7" },
                                { 394802, "4.6.2" },
                                { 394254, "4.6.1" },
                                { 393295, "4.6" },
                                { 379893, "4.5.2" },
                                { 378675, "4.5.1" },
                                { 378389, "4.5" }
                            };
                            string version = software_lang.TSReadLangs("Os_Content", "os_c_net_framework_null");
                            foreach (var kvp in netFrameworkVersions.OrderByDescending(k => k.Key)){
                                if (releaseKeyValue >= kvp.Key){
                                    version = kvp.Value;
                                    break;
                                }
                            }
                            OS_NETFrameworkVersion_V.Text = version;
                        }else{
                            OS_NETFrameworkVersion_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_net_framework_null");
                        }
                    }else{
                        OS_NETFrameworkVersion_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_net_framework_null");
                    }
                }
            }catch (Exception){ }
            try{
                // OS BLUESCREEN CHECK
                // Check Folder
                if (Directory.Exists(mdp_1)){
                    // Check minidump count
                    int minidump_count = Directory.GetFiles(mdp_1, "*.dmp", SearchOption.AllDirectories).Length;
                    if (minidump_count > 0){
                        string[] minidump_files = Directory.GetFiles(mdp_1, "*.dmp", SearchOption.AllDirectories);
                        for (int i = 0; i <= minidump_files.Length - 1; i++){
                            minidump_files_list.Add(minidump_files[i]);
                        }
                    }
                    // Check memory.dmp file
                    if (File.Exists(mdp_2)){
                        minidump_files_list.Add(mdp_2);
                    }
                }else{
                    // Check memory.dmp file
                    if (File.Exists(mdp_2)){
                        minidump_files_list.Add(mdp_2);
                    }
                }
                // no memory.dmp and no folder
                if (minidump_files_list.Count > 0){
                    if (OS_MinidumpOpen.InvokeRequired){
                        OS_MinidumpOpen.Invoke((MethodInvoker)(() => {
                            OS_MinidumpOpen.Visible = true;
                            OS_BSoDZIP.Visible = true;
                            OS_Minidump_V.Text = string.Format(software_lang.TSReadLangs("Os_Content", "os_c_bsod_detect"), minidump_files_list.Count);
                            bool hasMemoryDump = minidump_files_list.Any(f => string.Equals(Path.GetFileName(f), "memory.dmp", StringComparison.OrdinalIgnoreCase));
                            MainToolTip.SetToolTip(OS_MinidumpOpen, Directory.Exists(mdp_1) ? string.Format(software_lang.TSReadLangs("Os_Content", "os_c_bsod_open_folder"), mdp_1) : string.Format(software_lang.TSReadLangs("Os_Content", "os_c_bsod_open_file"), mdp_2) );
                            MainToolTip.SetToolTip(OS_BSoDZIP, software_lang.TSReadLangs("Os_Content", "os_c_bsod_zip_folder"));
                        }));
                    }else{
                        OS_MinidumpOpen.Visible = true;
                        OS_BSoDZIP.Visible = true;
                        OS_Minidump_V.Text = string.Format(software_lang.TSReadLangs("Os_Content", "os_c_bsod_detect"), minidump_files_list.Count);
                        MainToolTip.SetToolTip(OS_MinidumpOpen, string.Format(software_lang.TSReadLangs("Os_Content", "os_c_bsod_open_folder"), mdp_1));
                        MainToolTip.SetToolTip(OS_BSoDZIP, software_lang.TSReadLangs("Os_Content", "os_c_bsod_zip_folder"));
                    }
                }else{
                    if (OS_MinidumpOpen.InvokeRequired){
                        OS_MinidumpOpen.Invoke((MethodInvoker)(() => {
                            OS_MinidumpOpen.Visible = false;
                            OS_BSoDZIP.Visible = false;
                            OS_Minidump_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_bsod_not_detect");
                            OS_BSODDate_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_bsod_not_detect");
                        }));
                    }else{
                        OS_MinidumpOpen.Visible = false;
                        OS_BSoDZIP.Visible = false;
                        OS_Minidump_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_bsod_not_detect");
                        OS_BSODDate_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_bsod_not_detect");
                    }
                }
                // Check minidump files count
                if (minidump_files_list.Count > 0){
                    // Get minidump date
                    for (int i = 0; i <= minidump_files_list.Count - 1; i++){
                        DateTime modification = File.GetLastWriteTime(minidump_files_list[i]);
                        minidump_files_date_list.Add(modification.ToString());
                    }
                    // Reverse date
                    minidump_files_date_list.Reverse();
                    // Start dynamic Last BSoD Date
                    StartBsodTimeDynamicCheck();
                }
            }catch (Exception){ }
            try{
                // DESKTOP WALLPAPER
                StringBuilder wallpaperPath = new StringBuilder(260);
                bool wallpaperResult = SystemParametersInfo(0x0073, 260, wallpaperPath, 0);
                if (wallpaperResult && File.Exists(wallpaperPath.ToString())){
                    wp_rotate = wallpaperPath.ToString().Trim();
                    string wp_resolution;
                    using (var fs = new FileStream(wp_rotate, FileMode.Open, FileAccess.Read, FileShare.Read))
                    using (var image = Image.FromStream(fs, false, false)){
                        wp_resolution = $"{image.Width}x{image.Height}";
                    }
                    FileInfo fileInfo = new FileInfo(wp_rotate);
                    string fileName = Path.GetFileName(wp_rotate);
                    string fileExtension = Path.GetExtension(wp_rotate);
                    string formattedSize = TS_FormatSize(Convert.ToDouble(fileInfo.Length));
                    string wallpaperText;
                    if (hiding_mode_wrapper != 1){
                        wallpaperText = $"{fileName} - {wp_resolution} - {formattedSize}";
                    }else{
                        wallpaperText = $"{new string('*', vis_m_property.Next(vn_range[0], vn_range[1]))}{fileExtension} - {wp_resolution} - {formattedSize} ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                    }
                    OS_Wallpaper_V.Invoke((MethodInvoker)(() => {
                        OS_Wallpaper_V.Text = wallpaperText;
                        MainToolTip.SetToolTip(OS_WallpaperOpen, software_lang.TSReadLangs("Os_Content", "os_c_open_wallpaper"));
                        MainToolTip.SetToolTip(OS_WallpaperPreview, software_lang.TSReadLangs("Os_Content", "os_c_preview_wallpaper"));
                    }));
                }else{
                    OS_Wallpaper_V.Invoke((MethodInvoker)(() => {
                        OS_Wallpaper_V.Text = software_lang.TSReadLangs("Os_Content", "os_c_wallpaper_not_detect");
                        OS_WallpaperOpen.Visible = false;
                        OS_WallpaperPreview.Visible = false;
                    }));
                }
            }catch (Exception){ }
            // OS PROCESS END ENABLED
            OS_RotateBtn.Enabled = true;
            ((Control)OS).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Operating System Section Installed --->"); }
        }
        private async void OsBgProcess(){
            try{
                var software_lang = new TSGetLangs(lang_path);
                var search_os = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                while (loop_status){
                    var s_result = new Dictionary<string, string>();
                    try{
                        foreach (ManagementObject query_os in search_os.Get().Cast<ManagementObject>()){
                            double free_virtual_ram = Convert.ToDouble(query_os["FreeVirtualMemory"]) * 1024;
                            double total_virtual_memory = Convert.ToDouble(query_os["TotalVirtualMemorySize"]) * 1024;
                            double used_virtual_ram = total_virtual_memory - free_virtual_ram;
                            //
                            s_result["freeRam"] = TS_FormatSize(free_virtual_ram);
                            s_result["usedRam"] = TS_FormatSize(used_virtual_ram);
                            //
                            string rawDate = query_os["InstallDate"].ToString();
                            DateTime osInstallDate = ManagementDateTimeConverter.ToDateTime(rawDate);
                            //
                            DateTime now = DateTime.Now;
                            TimeSpan uptime = now - osInstallDate;
                            int years = uptime.Days / 365;
                            int days = uptime.Days % 365;
                            //
                            string oi_year = software_lang.TSReadLangs("Os_Content", "os_c_year");
                            string oi_day = software_lang.TSReadLangs("Os_Content", "os_c_day");
                            string oi_hour = software_lang.TSReadLangs("Os_Content", "os_c_hour");
                            string oi_minute = software_lang.TSReadLangs("Os_Content", "os_c_minute");
                            string oi_second = software_lang.TSReadLangs("Os_Content", "os_c_second");
                            string oi_ago = software_lang.TSReadLangs("Os_Content", "os_c_ago");
                            //
                            List<string> parts = new List<string>();
                            if (years > 0) parts.Add($"{years} {oi_year}");
                            if (days > 0) parts.Add($"{days} {oi_day}");
                            if (uptime.Hours > 0) parts.Add($"{uptime.Hours} {oi_hour}");
                            if (uptime.Minutes > 0) parts.Add($"{uptime.Minutes} {oi_minute}");
                            string uptimeText = string.Join(", ", parts);
                            //
                            s_result["install"] = $"{osInstallDate:dd.MM.yyyy} - {osInstallDate:HH:mm:ss} - ({uptimeText.Trim()} {oi_ago})";
                            string last_boot = Convert.ToString(query_os["LastBootUpTime"]);
                            DateTime boot_date = ManagementDateTimeConverter.ToDateTime(last_boot);
                            TimeSpan system_uptime = DateTime.Now - boot_date;
                            s_result["uptime"] = $"{system_uptime.Days} {oi_day}, {system_uptime.Hours} {oi_hour}, {system_uptime.Minutes} {oi_minute}, {system_uptime.Seconds} {oi_second}";
                        }
                        //
                        int mouseSpeed = new Computer().Mouse.WheelScrollLines;
                        s_result["mouseSpeed"] = string.Format(software_lang.TSReadLangs("Os_Content", "os_c_scroll_speed"), mouseSpeed);
                        s_result["scrollLock"] = new Computer().Keyboard.ScrollLock ? software_lang.TSReadLangs("Os_Content", "os_c_on") : software_lang.TSReadLangs("Os_Content", "os_c_off");
                        s_result["numLock"] = new Computer().Keyboard.NumLock ? software_lang.TSReadLangs("Os_Content", "os_c_on") : software_lang.TSReadLangs("Os_Content", "os_c_off");
                        s_result["capsLock"] = new Computer().Keyboard.CapsLock ? software_lang.TSReadLangs("Os_Content", "os_c_on") : software_lang.TSReadLangs("Os_Content", "os_c_off");
                    }catch{ }
                    if (IsHandleCreated){
                        BeginInvoke(new Action(() =>{
                            OS_SystemTime_V.Text = DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss");
                            //
                            if (!s_result.TryGetValue("install", out var install)) install = "N/A";
                            if (!s_result.TryGetValue("uptime", out var uptime)) uptime = "N/A";
                            if (!s_result.TryGetValue("mouseSpeed", out var mouseSpeed)) mouseSpeed = "N/A";
                            if (!s_result.TryGetValue("scrollLock", out var scrollLock)) scrollLock = "N/A";
                            if (!s_result.TryGetValue("numLock", out var numLock)) numLock = "N/A";
                            if (!s_result.TryGetValue("capsLock", out var capsLock)) capsLock = "N/A";
                            if (!s_result.TryGetValue("freeRam", out var freeRam)) freeRam = "N/A";
                            if (!s_result.TryGetValue("usedRam", out var usedRam)) usedRam = "N/A";
                            //
                            OS_Install_V.Text = install;
                            OS_SystemWorkTime_V.Text = uptime;
                            OS_MouseWheelStatus_V.Text = mouseSpeed;
                            OS_ScrollLockStatus_V.Text = scrollLock;
                            OS_NumLockStatus_V.Text = numLock;
                            OS_CapsLockStatus_V.Text = capsLock;
                            //
                            RAM_EmptyVirtualRam_V.Text = freeRam;
                            RAM_UsageVirtualRam_V.Text = usedRam;
                        }));
                    }
                    await Task.Delay(1000, Program.TS_TokenEngine.Token);
                }
            }catch { }
        }
        // SYSTEM SCREEN CLOSE AND SLEEP TIMER CONVERTER
        private static string SCFormatTimeout(int seconds){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            string time_never = software_lang.TSReadLangs("Os_Content", "os_c_a_power_never");
            string time_hour = software_lang.TSReadLangs("Os_Content", "os_c_hour");
            string time_miniutes = software_lang.TSReadLangs("Os_Content", "os_c_minute");
            //
            if (seconds <= 0) return time_never;
            int h = seconds / 3600;
            int m = (seconds % 3600) / 60;
            string result_timeout = "";
            if (h > 0) result_timeout += $"{h} {time_hour}";
            if (m > 0) result_timeout += (result_timeout != "" ? " " : "") + $"{m} {time_miniutes}";
            return result_timeout;
        }
        // SYSTEM SCREEN CLOSE AND SLEEP GET DATA
        private static int SCQueryTimeout(string planGuid, string subgroupAlias, string settingAlias, bool acPower){
            string cmd = $"/QUERY {planGuid} {subgroupAlias} {settingAlias}";
            string get_time_output;
            using (var getTimeProcess = new Process()){
                getTimeProcess.StartInfo.FileName = "powercfg";
                getTimeProcess.StartInfo.Arguments = cmd;
                getTimeProcess.StartInfo.RedirectStandardOutput = true;
                getTimeProcess.StartInfo.UseShellExecute = false;
                getTimeProcess.StartInfo.CreateNoWindow = true;
                getTimeProcess.Start();
                get_time_output = getTimeProcess.StandardOutput.ReadToEnd();
                getTimeProcess.WaitForExit();
            }
            string searchTime = acPower ? "Current AC Power Setting Index:" : "Current DC Power Setting Index:";
            foreach (var splitLine in get_time_output.Split(new[] { Environment.NewLine }, StringSplitOptions.None)){
                if (splitLine.Contains(searchTime)){
                    string convertTime = splitLine.Split(':')[1].Trim();
                    return Convert.ToInt32(convertTime, 16);
                }
            }
            return -1;
        }
        // GET MS STORE VERSION
        private async void StartMsStoreVersionCheck(){
            try{
                await Ms_store_version();
            }catch { }
        }
        private async Task Ms_store_version(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                string version = await Task.Run(async () => {
                    using (var process = new Process()){
                        process.StartInfo = new ProcessStartInfo{
                            FileName = "powershell.exe",
                            Arguments = "-NoProfile -ExecutionPolicy Bypass -Command \"(Get-AppxPackage -Name Microsoft.WindowsStore).Version\"",
                            RedirectStandardOutput = true,
                            UseShellExecute = false,
                            CreateNoWindow = true
                        };
                        process.Start();
                        string get_store_version = await process.StandardOutput.ReadToEndAsync();
                        process.WaitForExit();
                        return get_store_version.Trim();
                    }
                });
                string displayText = string.IsNullOrEmpty(version) ? software_lang.TSReadLangs("Os_Content", "os_c_unknown") : version;
                if (IsHandleCreated){
                    BeginInvoke(new Action(() => OS_MSStoreVersion_V.Text = displayText));
                }
            }catch { }
        }
        // GET MS OFFICE VERSION
        private async void StartMsOfficeVersionCheck(){
            try{
                await Ms_office_version();
            }catch{ }
        }
        private async Task Ms_office_version(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                string LicensedText = software_lang.TSReadLangs("Os_Content", "os_c_office_licensed");
                string GraceText = software_lang.TSReadLangs("Os_Content", "os_c_office_grace");
                string OtherText = software_lang.TSReadLangs("Os_Content", "os_c_office_other");
                string OfficeNotFoundText = software_lang.TSReadLangs("Os_Content", "os_c_office_not_found");
                var productsList = await Task.Run(() => {
                    var found = new List<(string fullName, string license)>();
                    var baseDirs = new[] {
                        $@"{windows_disk}Program Files\Microsoft Office",
                        $@"{windows_disk}Program Files (x86)\Microsoft Office"
                    };
                    IEnumerable<string> EnumerateDirectoriesSafe(string root, int maxDepth){
                        var dirs = new List<string>();
                        void Recurse(string path, int depth){
                            if (depth > maxDepth) return;
                            try{
                                foreach (var dir in Directory.EnumerateDirectories(path)){
                                    dirs.Add(dir);
                                    Recurse(dir, depth + 1);
                                }
                            }catch { }
                        }
                        Recurse(root, 1);
                        return dirs;
                    }
                    foreach (var baseDir in baseDirs){
                        if (!Directory.Exists(baseDir)) continue;
                        try{
                            var dirs = EnumerateDirectoriesSafe(baseDir, 5).ToList();
                            foreach (var dir in dirs){
                                string osppPath = Path.Combine(dir, "ospp.vbs");
                                if (!File.Exists(osppPath)) continue;
                                var psi = new ProcessStartInfo("cscript.exe", $"\"{osppPath}\" /dstatus"){
                                    RedirectStandardOutput = true,
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                };
                                using (var process = Process.Start(psi)){
                                    if (process == null) continue;
                                    string output = process.StandardOutput.ReadToEnd();
                                    process.WaitForExit();
                                    string currentName = null, currentChannel = null;
                                    foreach (string line in output.Split('\n')){
                                        string readLine = line.Trim();
                                        if (string.IsNullOrEmpty(readLine)) continue;
                                        if (readLine.StartsWith("LICENSE NAME:"))
                                            currentName = readLine.Substring(13).Trim();
                                        else if (readLine.StartsWith("LICENSE DESCRIPTION:")){
                                            string desc = readLine.Substring(20).Trim().ToUpperInvariant();
                                            if (desc.Contains("RETAIL")) currentChannel = "Retail";
                                            else if (desc.Contains("VL")) currentChannel = "Volume";
                                            else if (desc.Contains("OEM")) currentChannel = "OEM";
                                            else currentChannel = OtherText;
                                        }else if (readLine.StartsWith("LICENSE STATUS:") && currentName != null){
                                            string status = readLine.Substring(15).Trim();
                                            string readable = status.Contains("LICENSED") ? LicensedText : status.Contains("OOB_GRACE") ? GraceText : status;
                                            string version = currentName.Split(',')[0].Trim();
                                            found.Add((version, $"{readable} / {currentChannel ?? OtherText}"));
                                            currentName = null;
                                            currentChannel = null;
                                        }
                                    }
                                }
                            }
                        }catch { }
                    }
                    return found;
                });
                string displayText;
                if (!productsList.Any()){
                    displayText = OfficeNotFoundText;
                }else{
                    var groupedOffice = productsList.GroupBy(p => p.fullName).Select(g => {
                        string licenseStatus = g.Select(x => x.license).Distinct().FirstOrDefault() ?? OtherText;
                        return $"{g.Key} ({licenseStatus})";
                    });
                    displayText = string.Join(", ", groupedOffice);
                }
                if (IsHandleCreated){
                    BeginInvoke(new Action(() => OS_MSOfficeVersion_V.Text = displayText));
                }
            }catch { }
        }
        // WIN LICENSE TYPE
        private async void StartWindowsLicenseTypeCheck(){
            try{
                await Win_license_type();
            }catch { }
        }
        private async Task Win_license_type(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                string licenseType = software_lang.TSReadLangs("Os_Content", "os_c_unknown");
                using (var process = new Process()){
                    process.StartInfo = new ProcessStartInfo{
                        FileName = "cmd.exe",
                        Arguments = $"/c set LANG=en && cscript //NoLogo {windows_disk}Windows\\System32\\slmgr.vbs /dli",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    process.Start();
                    string output = await process.StandardOutput.ReadToEndAsync();
                    process.WaitForExit();
                    if (!string.IsNullOrWhiteSpace(output)){
                        string[] lines = output.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        if (lines.Length > 1){
                            string[] parts = lines[1].Split(new[] { ',' }, 2);
                            if (parts.Length > 1)
                                licenseType = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(parts[1].Trim());
                        }
                    }
                }
                if (IsHandleCreated){
                    BeginInvoke(new Action(() => OS_WinActiveChannel_V.Text = licenseType));
                }
            }catch{ }
        }
        // BSOD TIME DYNAMIC
        private async void StartBsodTimeDynamicCheck(){
            try{
                await Task.Run(() => Bsod_time_dynamic(), Program.TS_TokenEngine.Token);
            }catch (Exception){ }
        }
        private async void Bsod_time_dynamic(){
            try{
                var software_lang = new TSGetLangs(lang_path);
                DateTime last_bsod_date = Convert.ToDateTime(minidump_files_date_list[0]);
                var last_bsod_x64 = new DateTime(last_bsod_date.Year, last_bsod_date.Month, last_bsod_date.Day, last_bsod_date.Hour, last_bsod_date.Minute, last_bsod_date.Second);
                while (loop_status){
                    var now = DateTime.Now;
                    var span = now - last_bsod_x64;
                    string day = span.Days + " " + software_lang.TSReadLangs("Os_Content", "os_c_day");
                    string hour = span.Hours + " " + software_lang.TSReadLangs("Os_Content", "os_c_hour");
                    string minute = span.Minutes + " " + software_lang.TSReadLangs("Os_Content", "os_c_minute");
                    string second = span.Seconds + " " + software_lang.TSReadLangs("Os_Content", "os_c_second") + " " + software_lang.TSReadLangs("Os_Content", "os_c_ago");
                    var bsodText = $"{day}, {hour}, {minute}, {second}";
                    if (IsHandleCreated){
                        BeginInvoke(new Action(() =>{
                            OS_BSODDate_V.Text = bsodText;
                        }));
                    }
                    await Task.Delay(1000, Program.TS_TokenEngine.Token);
                }
            }catch { }
        }
        // MINIDIUMP FOLDER OPEN
        private void OS_MinidumpOpen_Click(object sender, EventArgs e){
            try{
                // OPEN MINIDUMP FOLDER
                bool hasMemoryDump = minidump_files_list.Any(f => string.Equals(Path.GetFileName(f), "memory.dmp", StringComparison.OrdinalIgnoreCase));
                bool hasMinidumpFolder = Directory.Exists(mdp_1);
                bool hasMemoryDumpFile = File.Exists(mdp_2);
                if (hasMinidumpFolder){
                    Process.Start("explorer.exe", mdp_1);
                }else if (hasMemoryDump && hasMemoryDumpFile){
                    Process.Start("explorer.exe", $"/select,\"{mdp_2}\"");
                }
            }catch (Exception){
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 3, software_lang.TSReadLangs("Os_Content", "os_c_bsod_folder_open_error"));
            }
        }
        // BSOD ZIP COMPRESS
        private async void OS_BSoDZIP_Click(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            try{
                string minidump_target_file = "GlowMinidumpTempFile";
                if (Directory.Exists(minidump_target_file)){
                    Directory.Delete(minidump_target_file, true);
                }
                Directory.CreateDirectory(minidump_target_file);
                string minidump_zip_file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Application.ProductName + "_Minidump_" + Dns.GetHostName() + "_" + DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss") + ".zip");
                // COPY ASYNC
                int currentFile = 0;
                Text = string.Format(software_lang.TSReadLangs("Os_Content", "os_c_bsod_progress_copy"), TS_VersionEngine.TS_SofwareVersion(0));
                foreach (string file_path in minidump_files_list){
                    string file_name = Path.GetFileName(file_path);
                    string target_file_path = Path.Combine(minidump_target_file, file_name);
                    await Task.Run(() => File.Copy(file_path, target_file_path, true), Program.TS_TokenEngine.Token);
                    currentFile++;
                }
                // ZIP ASYNC
                Text = string.Format(software_lang.TSReadLangs("Os_Content", "os_c_bsod_progress_compress"), TS_VersionEngine.TS_SofwareVersion(0));
                await Task.Run(() => {
                    using (FileStream zipToOpen = new FileStream(minidump_zip_file, FileMode.Create)){
                        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create)){
                            currentFile = 0;
                            foreach (string file_path in minidump_files_list){
                                string file_name = Path.GetFileName(file_path);
                                string target_file_path = Path.Combine(minidump_target_file, file_name);
                                ZipArchiveEntry zip_entry = archive.CreateEntry(file_name, CompressionLevel.NoCompression);
                                using (var entryStream = zip_entry.Open())
                                using (var fileStream = new FileStream(target_file_path, FileMode.Open, FileAccess.Read)){
                                    fileStream.CopyTo(entryStream);
                                }
                                currentFile++;
                            }
                        }
                    }
                }, Program.TS_TokenEngine.Token);
                if (Directory.Exists(minidump_target_file)){
                    Directory.Delete(minidump_target_file, true);
                }
                Text = TS_VersionEngine.TS_SofwareVersion(0);
                DialogResult open_minidump_zip_target = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("Os_Content", "os_c_bsod_zip_success"), minidump_zip_file, "\n\n"));
                if (open_minidump_zip_target == DialogResult.Yes){
                    string open_mdzt = string.Format("/select, \"{0}\"", minidump_zip_file.Trim().Replace("/", @"\"));
                    ProcessStartInfo psi = new ProcessStartInfo("explorer.exe", open_mdzt);
                    Process.Start(psi);
                }
            }catch (Exception ex){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("Os_Content", "os_c_bsod_zip_error"), "\n", ex.Message));
            }
        }
        // OPEN WALLPAPER
        private void OS_WallpaperOpen_Click(object sender, EventArgs e){
            try{
                string wallpaper_start_path = string.Format("/select, \"{0}\"", wp_rotate.Trim().Replace("/", @"\"));
                ProcessStartInfo psi = new ProcessStartInfo("explorer.exe", wallpaper_start_path);
                Process.Start(psi);
            }catch (Exception){
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 3, software_lang.TSReadLangs("Os_Content", "os_c_wallpaper_open_error"));
            }
        }
        // OPEN WALLPAPER PREVIEW
        private void OS_WallpaperPreview_Click(object sender, EventArgs e){
            TSToolLauncher<GlowWallpaperPreview>("glow_wallpaper_preview_tool", "wpt_name");
        }
        #endregion
        #region MB_Section
        // MB
        // ======================================================================================================
        private void Mb(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_bb = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
            ManagementObjectSearcher search_cs = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");
            ManagementObjectSearcher search_bios = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");
            ManagementObjectSearcher search_tpm = new ManagementObjectSearcher("root\\CIMV2\\Security\\MicrosoftTpm", "SELECT * FROM Win32_Tpm");
            foreach (ManagementObject query_bb_rotate in search_bb.Get().Cast<ManagementObject>()){
                try{
                    // MB NAME
                    MB_MotherBoardName_V.Text = Convert.ToString(query_bb_rotate["Product"]);
                }catch (Exception){ }
                try{
                    // MB MAN
                    MB_MotherBoardMan_V.Text = Convert.ToString(query_bb_rotate["Manufacturer"]);
                }catch (Exception){ }
                try{
                    // MB SERIAL
                    string mb_serial = Convert.ToString(query_bb_rotate["SerialNumber"]).Trim();
                    if (!string.IsNullOrEmpty(mb_serial)){
                        if (hiding_mode_wrapper != 1){
                            MB_MotherBoardSerial_V.Text = mb_serial;
                        }else{
                            MB_MotherBoardSerial_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                        }
                    }else{
                        MB_MotherBoardSerial_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected");
                    }
                }catch (Exception){ }
            }
            try{
                RegistryKey get_bios_data = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\BIOS");
                if (!string.IsNullOrEmpty(get_bios_data.ToString())){
                    // SYSTEM MODEL MANUFACTURER
                    try{
                        string systemManufacturer = Convert.ToString(get_bios_data.GetValue("SystemManufacturer"));
                        if (!string.IsNullOrEmpty(systemManufacturer)){
                            MB_SystemModelMan_V.Text = systemManufacturer.ToString().Trim();
                        }else{
                            MB_SystemModelMan_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected");
                        }
                    }catch (Exception){ }
                    // SYSTEM MODEL FAMILY
                    try{
                        string systemFamily = Convert.ToString(get_bios_data.GetValue("SystemFamily"));
                        if (!string.IsNullOrEmpty(systemFamily)){
                            MB_SystemModelFamily_V.Text = systemFamily.ToString().Trim();
                        }else{
                            MB_SystemModelFamily_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected");
                        }
                    }catch (Exception){ }
                    // SYSTEM MODEL
                    try{
                        string systemProductName = Convert.ToString(get_bios_data.GetValue("SystemProductName"));
                        if (!string.IsNullOrEmpty(systemProductName)){
                            MB_SystemModel_V.Text = systemProductName.ToString().Trim();
                        }else{
                            MB_SystemModel_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected");
                        }
                    }catch (Exception){ }
                }else{
                    MB_SystemModelMan_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected");
                    MB_SystemModelFamily_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected");
                    MB_SystemModel_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected");
                }
                get_bios_data.Close();
            }catch (Exception){ }
            foreach (ManagementObject query_cs in search_cs.Get().Cast<ManagementObject>()){
                // SYSTEM FAMILY
                try{
                    string system_family = Convert.ToString(query_cs["SystemFamily"]).Trim();
                    string system_family_check = system_family.ToLower();
                    if (system_family_check == "default string" || system_family_check == "to be filled by o.e.m."){
                        MB_SystemFamily_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected");
                    }else{
                        MB_SystemFamily_V.Text = system_family;
                    }
                }catch (Exception){ }
                // SYSTEM SKU
                try{
                    string system_sku = Convert.ToString(query_cs["SystemSKUNumber"]).Trim();
                    string system_sku_check = system_sku.ToLower();
                    if (system_sku_check != "default string" && system_sku_check != "sku"){
                        if (hiding_mode_wrapper != 1){
                            MB_SystemSKU_V.Text = system_sku;
                        }else{
                            MB_SystemSKU_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                        }
                    }else{
                        MB_SystemSKU_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected");
                    }
                }catch (Exception){ }
            }
            foreach (ManagementObject query_bios_rotate in search_bios.Get().Cast<ManagementObject>()){
                try{
                    // BIOS MAN
                    MB_BiosManufacturer_V.Text = Convert.ToString(query_bios_rotate["Manufacturer"]);
                }catch (Exception){ }
                try{
                    // BIOS DATE
                    string bios_date = Convert.ToString(query_bios_rotate["ReleaseDate"]);
                    DateTime bios_date_last = ManagementDateTimeConverter.ToDateTime(bios_date);
                    MB_BiosDate_V.Text = $"{bios_date_last:dd.MM.yyyy}";
                }catch (Exception){ }
                try{
                    // BIOS VERSION
                    MB_BiosVersion_V.Text = Convert.ToString(query_bios_rotate["Caption"]);
                }catch (Exception){ }
                try{
                    // SM-BIOS MAJOR MINOR
                    object sm_bios_major = query_bios_rotate["SMBIOSMajorVersion"];
                    object sm_bios_minor = query_bios_rotate["SMBIOSMinorVersion"];
                    MB_SmBiosVersion_V.Text = sm_bios_major.ToString() + "." + sm_bios_minor.ToString();
                }catch (Exception){ }
                try{
                    // BIOS SERIAL NUMBER
                    string mb_device_serial = Convert.ToString(query_bios_rotate["SerialNumber"]);
                    if (!string.IsNullOrEmpty(mb_device_serial)){
                        if (hiding_mode_wrapper != 1){
                            MB_DeviceSerialNumber_V.Text = mb_device_serial;
                        }else{
                            MB_DeviceSerialNumber_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                        }
                    }else{
                        MB_DeviceSerialNumber_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected");
                    }
                }catch (Exception){ }
            }
            try{
                // MB SECURE BOOT
                bool mb_secure_boot_status = Convert.ToBoolean(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", ""));
                if (mb_secure_boot_status == true){
                    MB_SecureBoot_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_on");
                }else if (mb_secure_boot_status == false){
                    MB_SecureBoot_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_off");
                }
            }catch (Exception){ }
            try{
                // MB FIRMWARE WINDOWS CA 2023 CERT CHECK
                Task.Run(() => CheckMBFirmwareCA2023Cert());
            }catch (Exception){ }
            foreach (ManagementObject query_tpm in search_tpm.Get().Cast<ManagementObject>()){
                try{
                    // TPM VERSION
                    bool tpm_status = Convert.ToBoolean(query_tpm["IsActivated_InitialValue"]);
                    string tpm_version = Convert.ToString(query_tpm["SpecVersion"]);
                    char[] split_char = { ',' };
                    string[] split_keywords = tpm_version.Trim().Split(split_char);
                    if (tpm_status == true){
                        MB_TPMStatus_V.Text = string.Format(software_lang.TSReadLangs("Mb_Content", "mb_c_active"), split_keywords[0], tpm_version);
                    }else{
                        MB_TPMStatus_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off");
                    }
                }catch (Exception){ }
                try{
                    // TPM PHYSICAL VERSION
                    string tpm_phy_version = Convert.ToString(query_tpm["PhysicalPresenceVersionInfo"]).Trim();
                    if (!string.IsNullOrEmpty(tpm_phy_version)){
                        MB_TPMPhysicalVersion_V.Text = tpm_phy_version;
                    }else{
                        MB_TPMPhysicalVersion_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off");
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER ID TXT
                    string tpm_man_id_txt = Convert.ToString(query_tpm["ManufacturerIdTxt"]).Trim();
                    if (!string.IsNullOrEmpty(tpm_man_id_txt)){
                        MB_TPMMan_V.Text = tpm_man_id_txt;
                    }else{
                        MB_TPMMan_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off");
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER ID
                    string tpm_man_id = Convert.ToString(query_tpm["ManufacturerId"]).Trim();
                    if (!string.IsNullOrEmpty(tpm_man_id)){
                        if (hiding_mode_wrapper != 1){
                            MB_TPMManID_V.Text = tpm_man_id;
                        }else{
                            MB_TPMManID_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                        }
                    }else{
                        MB_TPMManID_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off");
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER VERSION
                    string tpm_man_version = Convert.ToString(query_tpm["ManufacturerVersion"]).Trim();
                    if (!string.IsNullOrEmpty(tpm_man_version)){
                        MB_TPMManVersion_V.Text = tpm_man_version;
                    }else{
                        MB_TPMManVersion_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off");
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER VERSION FULL
                    string tpm_man_version_full = Convert.ToString(query_tpm["ManufacturerVersionFull20"]).Trim();
                    if (!string.IsNullOrEmpty(tpm_man_version_full)){
                        MB_TPMManFullVersion_V.Text = tpm_man_version_full;
                    }else{
                        MB_TPMManFullVersion_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off");
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER INFO
                    string tpm_man_version_info = Convert.ToString(query_tpm["ManufacturerVersionInfo"]).Trim();
                    if (!string.IsNullOrEmpty(tpm_man_version_info)){
                        MB_TPMManPublisher_V.Text = tpm_man_version_info;
                    }else{
                        MB_TPMManPublisher_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off");
                    }
                }catch (Exception){ }
            }
            // TPM MODE CHECK WRAPPER
            try{
                string tpmOffMessage = software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off");
                var tpm_controls = new Control[] {
                    MB_TPMStatus_V,
                    MB_TPMPhysicalVersion_V,
                    MB_TPMMan_V,
                    MB_TPMManID_V,
                    MB_TPMManVersion_V,
                    MB_TPMManFullVersion_V,
                    MB_TPMManPublisher_V
                };
                foreach (var tpm_control in tpm_controls){
                    if (tpm_control.Text == "N/A" || string.IsNullOrEmpty(tpm_control.Text)){
                        tpm_control.Text = tpmOffMessage;
                    }
                }
            }catch (Exception){ }
            try{
                // LAST BIOS TIME | POSTTime
                int last_bios_time = Convert.ToInt32(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "FwPOSTTime", ""));
                string last_bios_time_x64 = string.Format("{0:0.0}", TimeSpan.FromMilliseconds(last_bios_time).TotalSeconds).Replace(",", ".");
                MB_LastBIOSTime_V.Text = string.Format(software_lang.TSReadLangs("Mb_Content", "mb_c_boot_time"), last_bios_time_x64);
            }catch (Exception){ }
            // MB PROCESS END ENABLED
            MB_RotateBtn.Enabled = true;
            ((Control)MB).Enabled = true;
            MB_BIOSUpdateBtn.Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Motherboard Section Loaded --->"); }
        }
        // CHECK MOTHERBOARD FIRMWARE WINDOWS CA 2023 CERT
        private void CheckMBFirmwareCA2023Cert(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            void SetCA2023Label(string text){
                if (MB_SecureBootCA2023_V.InvokeRequired){
                    MB_SecureBootCA2023_V.Invoke(new Action(() => MB_SecureBootCA2023_V.Text = text));
                }else{
                    MB_SecureBootCA2023_V.Text = text;
                }
            }
            string CA2023ReadOrFallback(string section, string key, string fallback){
                try{
                    var v = software_lang.TSReadLangs(section, key);
                    return string.IsNullOrWhiteSpace(v) ? fallback : v;
                }catch{
                    return fallback;
                }
            }
            try{
                bool hasCa2023;
                //
                string ca2023_true = software_lang.TSReadLangs("Mb_Content", "mb_c_ca2023_true");
                string ca2023_false = software_lang.TSReadLangs("Mb_Content", "mb_c_ca2023_false");
                string ps_not_start = software_lang.TSReadLangs("Mb_Content", "mb_c_ps_not_start");
                string ps_error_code = software_lang.TSReadLangs("Mb_Content", "mb_c_ps_error_code");
                string unknown_err = software_lang.TSReadLangs("Mb_Content", "mb_c_unknown");
                //
                string msg_sb_disabled = CA2023ReadOrFallback("Mb_Content", "mb_c_sb_disabled", unknown_err);
                string msg_not_uefi = CA2023ReadOrFallback("Mb_Content", "mb_c_not_uefi", unknown_err);
                string msg_admin_required = CA2023ReadOrFallback("Mb_Content", "mb_c_admin_required", unknown_err);
                string msg_cmdlet_missing = CA2023ReadOrFallback("Mb_Content", "mb_c_cmdlet_missing", unknown_err);
                string msg_ps_missing = CA2023ReadOrFallback("Mb_Content", "mb_c_ps_missing", unknown_err);
                //
                ProcessStartInfo psi_ca2023 = new ProcessStartInfo{
                    FileName = "powershell.exe",
                    Arguments = "-NoProfile -Command \"[System.Text.Encoding]::ASCII.GetString((Get-SecureBootUEFI db).bytes) -match 'Windows UEFI CA 2023'\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                //
                using (Process p_run = Process.Start(psi_ca2023)){
                    if (p_run == null){
                        throw new Exception(ps_not_start);
                    }
                    //
                    string output = p_run.StandardOutput.ReadToEnd().Trim();
                    string err = p_run.StandardError.ReadToEnd();
                    p_run.WaitForExit();
                    //
                    if (p_run.ExitCode != 0){
                        if (string.IsNullOrWhiteSpace(err)){
                            throw new Exception(string.Format(ps_error_code, p_run.ExitCode));
                        }
                        string errTrim = err.Trim();
                        if (errTrim.IndexOf("Secure Boot is not enabled", StringComparison.OrdinalIgnoreCase) >= 0 || errTrim.IndexOf("SecureBoot is not enabled", StringComparison.OrdinalIgnoreCase) >= 0){
                            throw new Exception(msg_sb_disabled);
                        }
                        if (errTrim.IndexOf("not supported on this platform", StringComparison.OrdinalIgnoreCase) >= 0 || errTrim.IndexOf("Cmdlet not supported", StringComparison.OrdinalIgnoreCase) >= 0){
                            throw new Exception(msg_not_uefi);
                        }
                        if (errTrim.IndexOf("Access is denied", StringComparison.OrdinalIgnoreCase) >= 0 || errTrim.IndexOf("UnauthorizedAccess", StringComparison.OrdinalIgnoreCase) >= 0){
                            throw new Exception(msg_admin_required);
                        }
                        if (errTrim.IndexOf("Get-SecureBootUEFI", StringComparison.OrdinalIgnoreCase) >= 0 && errTrim.IndexOf("is not recognized", StringComparison.OrdinalIgnoreCase) >= 0){
                            throw new Exception(msg_cmdlet_missing);
                        }
                        throw new Exception(unknown_err);
                    }
                    if (!bool.TryParse(output, out hasCa2023)){
                        throw new Exception(unknown_err);
                    }
                }
                string response_msg = hasCa2023 ? ca2023_true : ca2023_false;
                SetCA2023Label(response_msg);
            }catch (Win32Exception){
                string unknown_err = software_lang.TSReadLangs("Mb_Content", "mb_c_unknown");
                string msg_ps_missing = CA2023ReadOrFallback("Mb_Content", "mb_c_ps_missing", unknown_err);
                SetCA2023Label(msg_ps_missing);
            }catch (Exception){
                string unknown_err = software_lang.TSReadLangs("Mb_Content", "mb_c_unknown");
                SetCA2023Label(unknown_err);
            }
        }
        private void MB_BIOSUpdateBtn_Click(object sender, EventArgs e){
            try{
                string bios_prompt = "";
                if (laptop_mode == true){
                    bios_prompt = string.Format("{0} {1} {2}", MB_SystemModelMan_V.Text.Trim(), MB_SystemModel_V.Text.Trim(), "BIOS Update");
                }else if (laptop_mode == false){
                    bios_prompt = string.Format("{0} {1} {2}", MB_MotherBoardMan_V.Text.Trim(), MB_MotherBoardName_V.Text.Trim(), "BIOS Update");
                }
                string search_browser_q = $"https://www.google.com/search?q={Uri.EscapeDataString(bios_prompt)}";
                Process.Start(new ProcessStartInfo(search_browser_q) { UseShellExecute = true });
            }catch (Exception){ }
        }
        #endregion
        #region CPU_Section
        // CPU
        // ======================================================================================================
        public static List<string> bench_cpu_info = new List<string>();
        private void Cpu(){
            // CPU MODE
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_process = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            ManagementObjectSearcher search_intel_me = new ManagementObjectSearcher("root\\Intel_ME", "SELECT * FROM ME_System");
            ManagementObjectSearcher search_cs = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");
            double cpu_l1_total_size = 0;
            double cpu_l2_total_size = 0;
            double cpu_l3_total_size = 0;
            foreach (ManagementObject query_process_rotate in search_process.Get().Cast<ManagementObject>()){
                try{
                    // CPU NAME
                    CPU_Name_V.Text = Convert.ToString(query_process_rotate["Name"]).Trim();
                    bench_cpu_info.Add(CPU_Name_V.Text);
                }catch (Exception){ }
                try{
                    // CPU MANUFACTURER
                    string cpu_man = Convert.ToString(query_process_rotate["Manufacturer"]).Trim();
                    bool cpu_man_intel = cpu_man.Contains("Intel");
                    bool cpu_man_amd = cpu_man.Contains("Advanced Micro");
                    if (cpu_man_intel == true){
                        CPU_Manufacturer_V.Text = "Intel Corporation";
                        MB_Chipset_V.Text = "Intel";
                    }else if (cpu_man_amd == true){
                        CPU_Manufacturer_V.Text = cpu_man;
                        MB_Chipset_V.Text = "AMD";
                    }else{
                        CPU_Manufacturer_V.Text = cpu_man;
                        MB_Chipset_V.Text = cpu_man;
                    }
                }catch (Exception){ }
                try{
                    // CPU ARCHITECTURE
                    int cpu_arch_num = Convert.ToInt32(query_process_rotate["Architecture"]);
                    string[] cpu_architectures = { "32 " + software_lang.TSReadLangs("Cpu_Content", "cpu_c_bit") + " - (x86)", "MIPS", "ALPHA", "POWER PC", "ARM", "IA64", "64 " + software_lang.TSReadLangs("Cpu_Content", "cpu_c_bit") + " - (x64)" };
                    if (cpu_arch_num == 0){
                        CPU_Architectural_V.Text = cpu_architectures[0];
                    }else if (cpu_arch_num == 1){
                        CPU_Architectural_V.Text = cpu_architectures[1];
                    }else if (cpu_arch_num == 2){
                        CPU_Architectural_V.Text = cpu_architectures[2];
                    }else if (cpu_arch_num == 3){
                        CPU_Architectural_V.Text = cpu_architectures[3];
                    }else if (cpu_arch_num == 5){
                        CPU_Architectural_V.Text = cpu_architectures[4];
                    }else if (cpu_arch_num == 6){
                        CPU_Architectural_V.Text = cpu_architectures[5];
                    }else if (cpu_arch_num == 9){
                        CPU_Architectural_V.Text = cpu_architectures[6];
                    }else{
                        CPU_Architectural_V.Text = cpu_arch_num.ToString();
                    }
                }catch (Exception){ }
                try{
                    // CPU NORMAL SPEED
                    double cpu_speed = Convert.ToDouble(query_process_rotate["CurrentClockSpeed"]);
                    if (cpu_speed > 1024){
                        CPU_NormalSpeed_V.Text = string.Format("{0:0.00} GHz", cpu_speed / 1000);
                    }else{
                        CPU_NormalSpeed_V.Text = cpu_speed.ToString() + " MHz";
                    }
                }catch (Exception){ }
                try{
                    // CPU DEFAULT SPEED
                    double cpu_max_speed = Convert.ToDouble(query_process_rotate["MaxClockSpeed"]);
                    if (cpu_max_speed > 1024){
                        CPU_DefaultSpeed_V.Text = string.Format("{0:0.00} GHz", cpu_max_speed / 1000);
                    }else{
                        CPU_DefaultSpeed_V.Text = cpu_max_speed.ToString() + " MHz";
                    }
                }catch (Exception){ }
                try{
                    // L2 CACHE
                    double l2_size = Convert.ToDouble(query_process_rotate["L2CacheSize"]) * 1024;
                    cpu_l2_total_size += l2_size;
                    CPU_L2_V.Text = TS_FormatSize(cpu_l2_total_size);
                }catch (Exception){ }
                try{
                    // L3 CACHE
                    double l3_size = Convert.ToDouble(query_process_rotate["L3CacheSize"]) * 1024;
                    cpu_l3_total_size += l3_size;
                    CPU_L3_V.Text = TS_FormatSize(cpu_l3_total_size);
                }catch (Exception){ }
                try{
                    // CPU CORES
                    CPU_CoreCount_V.Text = Convert.ToString(query_process_rotate["NumberOfCores"]);
                    bench_cpu_info.Add(CPU_CoreCount_V.Text);
                }catch (Exception){ }
                try{
                    // CPU LOGICAL CORES
                    string thread_count = Convert.ToString(query_process_rotate["ThreadCount"]);
                    if (!string.IsNullOrEmpty(thread_count)){
                        CPU_LogicalCore_V.Text = thread_count;
                    }else{
                        CPU_LogicalCore_V.Text = software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown");
                    }
                    bench_cpu_info.Add(CPU_LogicalCore_V.Text);
                }catch (Exception){ }
                //
                benchCPUTool.Enabled = true;
                //
                try{
                    // CPU SOCKET
                    CPU_SocketDefinition_V.Text = Convert.ToString(query_process_rotate["SocketDesignation"]);
                }catch (Exception){ }
                try{
                    // CPU FAMILY
                    string cpu_description = Convert.ToString(query_process_rotate["Description"]);
                    string cpu_tanim = cpu_description.Replace("Family", software_lang.TSReadLangs("Cpu_Content", "cpu_c_family"));
                    string cpu_tanim_2 = cpu_tanim.Replace("Model", software_lang.TSReadLangs("Cpu_Content", "cpu_c_model"));
                    string cpu_tanim_3 = cpu_tanim_2.Replace("Stepping", software_lang.TSReadLangs("Cpu_Content", "cpu_c_stage"));
                    string cpu_tanim_4 = cpu_tanim_3.Replace("64", " X64");
                    CPU_Family_V.Text = cpu_tanim_4;
                }catch (Exception){ }
                try{
                    // CPU VIRTUALIZATION
                    cpu_virtual_mod = Convert.ToBoolean(query_process_rotate["VirtualizationFirmwareEnabled"]);
                    if (cpu_virtual_mod == true){
                        CPU_Virtualization_V.Text = software_lang.TSReadLangs("Cpu_Content", "cpu_c_active");
                    }else if (cpu_virtual_mod == false){
                        CPU_Virtualization_V.Text = software_lang.TSReadLangs("Cpu_Content", "cpu_c_disabled");
                    }
                }catch (Exception){ }
                try{
                    // CPU VM MONITOR EXTENSION
                    bool cpu_vm_extension = Convert.ToBoolean(query_process_rotate["VMMonitorModeExtensions"]);
                    if (cpu_vm_extension == true){
                        CPU_VMExtension_V.Text = software_lang.TSReadLangs("Cpu_Content", "cpu_c_active");
                    }else if (cpu_vm_extension == false){
                        CPU_VMExtension_V.Text = software_lang.TSReadLangs("Cpu_Content", "cpu_c_disabled");
                    }
                }catch (Exception){ }
                try{
                    // CPU SERIAL ID
                    string cpu_serial = Convert.ToString(query_process_rotate["ProcessorId"]).Trim();
                    GlowSystemIDGenerator.__system_processor_id = cpu_serial;
                    if (hiding_mode_wrapper != 1){
                        CPU_SerialName_V.Text = cpu_serial;
                    }else{
                        CPU_SerialName_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                    }
                }catch (Exception){ }
            }
            ManagementObjectSearcher search_cm = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_CacheMemory WHERE Level = {3}");
            foreach (ManagementObject query_cm_rotate in search_cm.Get().Cast<ManagementObject>()){
                // L1 CACHE
                double l1_size = Convert.ToDouble(query_cm_rotate["MaxCacheSize"]) * 1024;
                cpu_l1_total_size += l1_size;
                CPU_L1_V.Text = TS_FormatSize(cpu_l1_total_size);
            }
            // INTEL ME VERSION
            try{
                CPU_IntelME_V.Text = software_lang.TSReadLangs("Cpu_Content", "cpu_c_not_supported");
                foreach (ManagementObject query_intel_me in search_intel_me.Get().Cast<ManagementObject>()){
                    string intel_me_version = Convert.ToString(query_intel_me["FWVersion"]);
                    if (MB_Chipset_V.Text.Trim().Contains("Intel") || CPU_Name_V.Text.Trim().Contains("Intel")){
                        if (!string.IsNullOrEmpty(intel_me_version)){
                            CPU_IntelME_V.Text = intel_me_version.Trim();
                        }else{
                            CPU_IntelME_V.Text = software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown");
                        }
                    }else{
                        CPU_IntelME_V.Text = software_lang.TSReadLangs("Cpu_Content", "cpu_c_not_supported");
                    }
                }
            }catch (Exception){ }
            // HYPER-V
            try{
                foreach (var query_hyperv in search_cs.Get()){
                    var get_hyperv = query_hyperv["HypervisorPresent"];
                    if (get_hyperv != null && bool.TryParse(get_hyperv.ToString(), out bool isPresentHyperV)){
                        if (isPresentHyperV){
                            CPU_HyperV_V.Text = software_lang.TSReadLangs("Cpu_Content", "cpu_c_active");
                            if (!cpu_virtual_mod)
                                CPU_Virtualization_V.Text = software_lang.TSReadLangs("Cpu_Content", "cpu_c_active");
                        }else{
                            CPU_HyperV_V.Text = software_lang.TSReadLangs("Cpu_Content", "cpu_c_disabled");
                        }
                    }
                }
            }catch (Exception){ }
            // CPU PROCESS END ENABLED
            CPU_RotateBtn.Enabled = true;
            ((Control)CPU).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Processor Section Loaded --->"); }
        }
        private async void ProcessorBgProcess(){
            var searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT NumberOfProcesses FROM Win32_OperatingSystem");
            while (loop_status){
                long processCount = 0;
                long totalThreads = 0;
                long totalHandles = 0;
                try{
                    processCount = searcher.Get().Cast<ManagementObject>().Select(mo => Convert.ToInt32(mo["NumberOfProcesses"])).FirstOrDefault();
                    foreach (Process proc in Process.GetProcesses()){
                        try{
                            totalThreads += proc.Threads.Count;
                            totalHandles += proc.HandleCount;
                        }catch{ }
                    }
                }catch (Exception){ }
                if (IsHandleCreated){
                    BeginInvoke(new Action(() =>{
                        CPU_Process_V.Text = processCount.ToString("N0");
                        CPU_Threads_V.Text = totalThreads.ToString("N0");
                        CPU_Handles_V.Text = totalHandles.ToString("N0");
                    }));
                }
                try{
                    await Task.Delay(1000, Program.TS_TokenEngine.Token);
                }catch (TaskCanceledException){
                    break;
                }
            }
        }
        // CPU ENGINE
        // ======================================================================================================
        private async void CpuBgProcess(){
            PerformanceCounter cpuCounter = null;
            try{
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                while (loop_status){
                    float cpuUsage = 0f;
                    try{
                        cpuUsage = cpuCounter.NextValue();
                        await Task.Delay(500, Program.TS_TokenEngine.Token);
                        cpuUsage = cpuCounter.NextValue();
                    }catch (TaskCanceledException){
                        break;
                    }catch{
                        cpuUsage = 0f;
                    }
                    if (cpuUsage < 1.0f){
                        try { await Task.Delay(500, Program.TS_TokenEngine.Token); }
                        catch (TaskCanceledException) { break; }
                        continue;
                    }
                    if (IsHandleCreated){
                        BeginInvoke(new Action(() =>{
                            CPU_Usage_V.Text = $"{cpuUsage:F1}%";
                        }));
                    }
                    try { await Task.Delay(1000, Program.TS_TokenEngine.Token); }
                    catch (TaskCanceledException) { break; }
                }
            }finally{
                cpuCounter?.Dispose();
            }
        }
        #endregion
        #region RAM_Section
        // RAM
        // ======================================================================================================
        readonly List<string> ram_slot_count = new List<string>();
        readonly List<string> ram_slot_list = new List<string>();
        readonly List<string> ram_amount_list = new List<string>();
        readonly List<string> ram_type_list = new List<string>();
        readonly List<string> ram_frekans_list = new List<string>();
        readonly List<string> ram_voltage_list = new List<string>();
        readonly List<string> ram_form_factor = new List<string>();
        readonly List<string> ram_serial_list = new List<string>();
        readonly List<string> ram_manufacturer_list = new List<string>();
        readonly List<string> ram_bank_label_list = new List<string>();
        readonly List<string> ram_data_width_list = new List<string>();
        readonly List<string> bellek_type_list = new List<string>();
        readonly List<string> ram_part_number_list = new List<string>();
        private void Ram(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher search_pm = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
            try{
                // TOTAL RAM
                ComputerInfo main_query = new ComputerInfo();
                ulong total_ram_x64_tick = ulong.Parse(main_query.TotalPhysicalMemory.ToString());
                RAM_TotalRAM_V.Text = TS_FormatSize(total_ram_x64_tick);
            }catch (Exception){ }
            try{
                foreach (ManagementObject query_os_rotate in search_os.Get().Cast<ManagementObject>()){
                    // TOTAL VIRTUAL RAM
                    double total_virtual_ram = Convert.ToDouble(query_os_rotate["TotalVirtualMemorySize"]) * 1024;
                    RAM_TotalVirtualRam_V.Text = TS_FormatSize(total_virtual_ram);
                }
            }catch (Exception){ }
            foreach (ManagementObject queryObj in search_pm.Get().Cast<ManagementObject>()){
                try{
                    // RAM AMOUNT
                    string ram_count = Convert.ToString(queryObj["BankLabel"]);
                    if (string.IsNullOrEmpty(ram_count)){
                        ram_slot_list.Add(software_lang.TSReadLangs("Ram_Content", "ram_c_no_bank_label"));
                    }else{
                        ram_slot_list.Add(ram_count);
                    }
                }catch (Exception){ }
                try{
                    // RAM SLOT COUNT
                    ram_slot_count.Add(Convert.ToString(queryObj["Capacity"]));
                    RAM_SlotStatus_V.Text = ram_slot_count.Count + " " + software_lang.TSReadLangs("Ram_Content", "ram_c_slot_count");
                }catch (Exception){ }
                try{
                    // RAM CAPACITY
                    double ram_amount = Convert.ToDouble(queryObj["Capacity"]);
                    ram_amount_list.Add(TS_FormatSize(ram_amount));
                    RAM_Amount_V.Text = ram_amount_list[0];
                }catch (Exception){ }
                try{
                    // MEMORY TYPE
                    // https://learn.microsoft.com/en-us/windows/win32/cimwin32prov/win32-physicalmemory
                    int sm_bios_memory_type = Convert.ToInt32(queryObj["SMBIOSMemoryType"]);
                    int memory_type = Convert.ToInt32(queryObj["MemoryType"]);
                    var memoryTypes = new Dictionary<int, string>{
                        { 0, software_lang.TSReadLangs("Ram_Content", "ram_c_unknown") },
                        { 1, software_lang.TSReadLangs("Ram_Content", "ram_c_other_memory_type") },
                        { 2, "DRAM" },
                        { 3, "Synchronous DRAM" },
                        { 4, "Cache Ram" },
                        { 5, "EDO" },
                        { 6, "EDRAM" },
                        { 7, "VRAM" },
                        { 8, "SRAM" },
                        { 9, "RAM" },
                        { 10, "ROM" },
                        { 11, "FLASH" },
                        { 12, "EEPROM" },
                        { 13, "FEPROM" },
                        { 14, "EPROM" },
                        { 15, "CDRAM" },
                        { 16, "3DRAM" },
                        { 17, "SDRAM" },
                        { 18, "SGRAM" },
                        { 19, "RDRAM" },
                        { 20, "DDR" },
                        { 21, "DDR2" },
                        { 22, "DDR2 FB-DIMM" },
                        { 24, "DDR3" },
                        { 25, "FBD2" },
                        { 26, "DDR4" },
                        { 34, "DDR5" }
                    };
                    if (memoryTypes.TryGetValue(sm_bios_memory_type, out string ramTypeMessage) || memoryTypes.TryGetValue(memory_type, out ramTypeMessage)){
                        ram_type_list.Add(ramTypeMessage);
                    }else{
                        ram_type_list.Add(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown"));
                    }
                    RAM_Type_V.Text = ram_type_list[0];
                }catch (Exception){ }
                try{
                    // RAM SPEED
                    double ram_speed = Convert.ToInt32(queryObj["Speed"]);
                    ram_frekans_list.Add(string.Format("{0} MT/s ({1} MHz)", ram_speed, ram_speed / 2));
                    RAM_Frequency_V.Text = ram_frekans_list[0];
                }catch (Exception){ }
                try{
                    // RAM VOLTAGE
                    string ramVoltStr = queryObj["ConfiguredVoltage"]?.ToString();
                    if (string.IsNullOrEmpty(ramVoltStr) || !double.TryParse(ramVoltStr, out double ramVolt) || ramVolt == 0){
                        ram_voltage_list.Add(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown"));
                    }else{
                        ram_voltage_list.Add(string.Format("{0:0.00} " + software_lang.TSReadLangs("Ram_Content", "ram_c_voltage"), ramVolt / 1000.0));
                    }
                    RAM_Volt_V.Text = ram_voltage_list[0];
                }catch (Exception){ }
                try{
                    // FORM FACTOR
                    int form_factor = Convert.ToInt32(queryObj["FormFactor"]);
                    Dictionary<int, string> formFactorDict = new Dictionary<int, string>{
                        { 0, software_lang.TSReadLangs("Ram_Content", "ram_c_unknown") },
                        { 1, "Other" },
                        { 2, "SIP" },
                        { 3, "DIP" },
                        { 4, "ZIP" },
                        { 5, "SOJ" },
                        { 6, "Proprietary" },
                        { 7, "SIMM" },
                        { 8, "DIMM" },
                        { 9, "TSOP" },
                        { 10, "PGA" },
                        { 11, "RIMM" },
                        { 12, "SO-DIMM" },
                        { 13, "SRIMM" },
                        { 14, "SMD" },
                        { 15, "SSMP" },
                        { 16, "QFP" },
                        { 17, "TQFP" },
                        { 18, "SOIC" },
                        { 19, "LCC" },
                        { 20, "PLCC" },
                        { 21, "BGA" },
                        { 22, "FPBGA" },
                        { 23, "LGA" }
                    };
                    //
                    if (formFactorDict.TryGetValue(form_factor, out string formFactorValue)){
                        ram_form_factor.Add(formFactorValue);
                    }else{
                        ram_form_factor.Add(formFactorDict[0]);
                    }
                    RAM_FormFactor_V.Text = ram_form_factor[0];
                }catch (Exception){ }
                try{
                    // RAM SERIAL
                    string ram_serial = Convert.ToString(queryObj["SerialNumber"]).Trim();
                    if (string.IsNullOrEmpty(ram_serial) || ram_serial.Equals("Unknown", StringComparison.OrdinalIgnoreCase)){
                        ram_serial_list.Add(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown"));
                    }else{
                        // Hiding mode kontrolü
                        if (hiding_mode_wrapper == 1){
                            int maskLength = vis_m_property.Next(vn_range[0], vn_range[1]);
                            ram_serial_list.Add(new string('*', maskLength) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})");
                        }else{
                            ram_serial_list.Add(ram_serial);
                        }
                    }
                    RAM_Serial_V.Text = ram_serial_list[0];
                }catch (Exception){ }
                try{
                    // RAM MAN
                    string ram_man = Convert.ToString(queryObj["Manufacturer"]).Trim();
                    Dictionary<string, string> manufacturerDict = new Dictionary<string, string>{
                        { "017A", "Apacer" },
                        { "059B", "Crucial" },
                        { "04CD", "G.Skill" },
                        { "0198", "HyperX" },
                        { "029E", "Corsair" },
                        { "04CB", "A-DATA" },
                        { "00CE", "Samsung" },
                        { "00FE", "Micron" },
                        { "00AD", "Hynix" },
                        { "00B3", "Elpida" },
                        { "2C00", "Micron Technology" },
                        { "014F", "Transcend" },
                        { "1C1F", "Kingston" },
                        { "00A1", "Infineon" },
                        { "7F7F", "Silicon Power" }
                    };
                    //
                    if (string.IsNullOrEmpty(ram_man) || ram_man.Equals("unknown", StringComparison.OrdinalIgnoreCase)){
                        ram_manufacturer_list.Add(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown"));
                    }else{
                        if (manufacturerDict.TryGetValue(ram_man, out string manufacturerName)){
                            ram_manufacturer_list.Add(manufacturerName);
                        }else{
                            ram_manufacturer_list.Add(ram_man);
                        }
                    }
                    RAM_Manufacturer_V.Text = ram_manufacturer_list[0];
                }catch (Exception){ }
                try{
                    // RAM BANK LABEL
                    string bank_label = Convert.ToString(queryObj["BankLabel"]);
                    if (string.IsNullOrEmpty(bank_label)){
                        ram_bank_label_list.Add(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown"));
                    }else{
                        ram_bank_label_list.Add(bank_label);
                    }
                    RAM_BankLabel_V.Text = ram_bank_label_list[0];
                }catch (Exception){ }
                try{
                    // RAM TOTAL WIDTH
                    string ram_data_width = Convert.ToString(queryObj["TotalWidth"]);
                    if (string.IsNullOrEmpty(ram_data_width)){
                        ram_data_width_list.Add(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown"));
                    }else{
                        ram_data_width_list.Add(ram_data_width + " Bit");
                    }
                    RAM_DataWidth_V.Text = ram_data_width_list[0];
                }catch (Exception){ }
                try{
                    // RAM LOCATOR
                    bellek_type_list.Add(Convert.ToString(queryObj["DeviceLocator"]));
                    RAM_BellekType_V.Text = bellek_type_list[0];
                }catch (Exception){ }
                try{
                    // PART NUMBER
                    string part_number = Convert.ToString(queryObj["PartNumber"]).Trim();
                    if (string.IsNullOrEmpty(part_number)){
                        ram_part_number_list.Add(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown"));
                    }else{
                        if (hiding_mode_wrapper == 1){
                            int maskLength = vis_m_property.Next(vn_range[0], vn_range[1]);
                            ram_part_number_list.Add(new string('*', maskLength) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})");
                        }else{
                            ram_part_number_list.Add(part_number);
                        }
                    }
                    RAM_PartNumber_V.Text = ram_part_number_list[0];
                }catch (Exception){ }
            }
            // RAM SELECT
            try{
                int ram_amount = ram_slot_list.Count - 1;
                for (int rs = 0; rs <= ram_amount; rs++){
                    RAM_Selector_List.Items.Add(string.Format("{0} #{1} - {2} / {3}", software_lang.TSReadLangs("Ram_Content", "ram_c_ram_slot_select"), (rs + 1), ram_manufacturer_list[rs], ram_amount_list[rs]));
                }
                RAM_Selector_List.SelectedIndex = 0;
            }catch (Exception){ }
            // RAM PROCESS END ENABLED
            RAM_RotateBtn.Enabled = true;
            ((Control)RAM).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- RAM Section Loaded --->"); }
        }
        private void RAM_Selector_List_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int ram_slot = RAM_Selector_List.SelectedIndex;
                try { RAM_Amount_V.Text = ram_amount_list[ram_slot]; } catch (Exception) { }
                try { RAM_Type_V.Text = ram_type_list[ram_slot]; } catch (Exception) { }
                try { RAM_Frequency_V.Text = ram_frekans_list[ram_slot]; } catch (Exception) { }
                try { RAM_Volt_V.Text = ram_voltage_list[ram_slot]; } catch (Exception) { }
                try { RAM_FormFactor_V.Text = ram_form_factor[ram_slot]; } catch (Exception) { }
                try { RAM_Serial_V.Text = ram_serial_list[ram_slot]; } catch (Exception) { }
                try { RAM_Manufacturer_V.Text = ram_manufacturer_list[ram_slot]; } catch (Exception) { }
                try { RAM_BankLabel_V.Text = ram_bank_label_list[ram_slot]; } catch (Exception) { }
                try { RAM_DataWidth_V.Text = ram_data_width_list[ram_slot]; } catch (Exception) { }
                try { RAM_BellekType_V.Text = bellek_type_list[ram_slot]; } catch (Exception) { }
                try { RAM_PartNumber_V.Text = ram_part_number_list[ram_slot]; } catch (Exception) { }
            }catch (Exception){ }
        }
        private async void RamBgProcess(){
            var get_ram_info = new ComputerInfo();
            while (loop_status){
                ulong total = get_ram_info.TotalPhysicalMemory;
                ulong free = get_ram_info.AvailablePhysicalMemory;
                double usedRatio = (TS_FormatSizeNoType(total) - TS_FormatSizeNoType(free)) / TS_FormatSizeNoType(total) * 100;
                if (IsHandleCreated){
                    BeginInvoke(new Action(() =>{
                        // RAM_UsageRAMCount_V.Text = $"{TS_FormatSize(total - free)} - {usedRatio:0.00}%";
                        RAM_UsageRAMCount_V.Text = $"{TS_FormatSize(total - free)}";
                        RAM_EmptyRamCount_V.Text = TS_FormatSize(free);
                        RAM_ProgressFEPanel.Height = (int)(RAM_ProgressBGPanel.Height * (usedRatio / 100.0));
                        RAM_ProgressLabel.Text = $"{usedRatio:0.0}%";
                        RAM_ProgressLabel.Top = RAM_ProgressFEPanel.Top + 6;
                    }));
                }
                try{
                    await Task.Delay(1000, Program.TS_TokenEngine.Token);
                }catch (TaskCanceledException){
                    break;
                }
            }
        }
        #endregion
        #region GPU_Section
        // GPU
        // ======================================================================================================
        readonly List<string> gpu_man_list = new List<string>();
        readonly List<string> gpu_vram_list = new List<string>();
        readonly List<string> gpu_driver_version_list = new List<string>();
        readonly List<string> gpu_driver_date_list = new List<string>();
        readonly List<string> gpu_status_list = new List<string>();
        readonly List<string> gpu_device_id_list = new List<string>();
        readonly List<string> gpu_dac_type_list = new List<string>();
        readonly List<string> gpu_drivers_list = new List<string>();
        readonly List<string> gpu_inf_file_list = new List<string>();
        readonly List<string> gpu_inf_file_section_list = new List<string>();
        readonly List<string> gpu_current_colors_list = new List<string>();
        // ------------------------------------------------------------------------------------
        readonly List<string> gpu_monitor_user_friendly_name_list = new List<string>();
        readonly List<string> gpu_monitor_manufacturer_list = new List<string>();
        readonly List<string> gpu_monitor_product_code_id_list = new List<string>();
        readonly List<string> gpu_monitor_serial_number_id_list = new List<string>();
        readonly List<string> gpu_monitor_con_type_list = new List<string>();
        readonly List<string> gpu_monitor_manuf_list = new List<string>();
        readonly List<string> gpu_monitor_manuf_week_list = new List<string>();
        readonly List<string> gpu_monitor_hid_list = new List<string>();
        readonly List<string> gpu_monitor_res_list = new List<string>();
        readonly List<string> gpu_monitor_virtual_res_list = new List<string>();
        readonly List<string> gpu_monitor_bounds_list = new List<string>();
        readonly List<string> gpu_monitor_work_list = new List<string>();
        readonly List<string> gpu_monitor_refresh_rate_list = new List<string>();
        readonly List<string> gpu_monitor_bit_deep_list = new List<string>();
        readonly List<string> gpu_monitor_primary_list = new List<string>();
        private void Gpu(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_vc = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            foreach (ManagementObject query_vc_rotate in search_vc.Get().Cast<ManagementObject>()){
                try{
                    // GPU NAME
                    string gpu_name = Convert.ToString(query_vc_rotate["Name"]);
                    if (!string.IsNullOrEmpty(gpu_name)){
                        GPU_Selector_List.Items.Add(gpu_name);
                    }
                }catch (Exception){ }
                try{
                    // GPU MAN
                    string gpu_man = Convert.ToString(query_vc_rotate["AdapterCompatibility"]).Trim();
                    if (!string.IsNullOrEmpty(gpu_man)){
                        gpu_man_list.Add(gpu_man);
                        GPU_Manufacturer_V.Text = gpu_man_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU DRIVER VERSION
                    string driver_version = Convert.ToString(query_vc_rotate["DriverVersion"]);
                    if (!string.IsNullOrEmpty(driver_version)){
                        gpu_driver_version_list.Add(driver_version);
                        GPU_Version_V.Text = gpu_driver_version_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU DRIVER DATE
                    string gpu_date = Convert.ToString(query_vc_rotate["DriverDate"]);
                    if (!string.IsNullOrEmpty(gpu_date)){
                        DateTime gpu_date_last = ManagementDateTimeConverter.ToDateTime(gpu_date);
                        gpu_driver_date_list.Add($"{gpu_date_last:dd.MM.yyyy}");
                        GPU_DriverDate_V.Text = gpu_driver_date_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU STATUS
                    int gpu_status = Convert.ToInt32(query_vc_rotate["Availability"]);
                    var gpuStatusMessages = new Dictionary<int, string>{
                        { 1, software_lang.TSReadLangs("Gpu_Content", "gpu_c_other") },
                        { 2, software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown") },
                        { 3, software_lang.TSReadLangs("Gpu_Content", "gpu_c_works_smoothly") },
                        { 4, software_lang.TSReadLangs("Gpu_Content", "gpu_c_warning") },
                        { 5, software_lang.TSReadLangs("Gpu_Content", "gpu_c_test") },
                        { 6, software_lang.TSReadLangs("Gpu_Content", "gpu_c_not_applicable") },
                        { 7, software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_off") },
                        { 8, software_lang.TSReadLangs("Gpu_Content", "gpu_c_offline") },
                        { 9, software_lang.TSReadLangs("Gpu_Content", "gpu_c_off_duty") },
                        { 10, software_lang.TSReadLangs("Gpu_Content", "gpu_c_corrupted") },
                        { 11, software_lang.TSReadLangs("Gpu_Content", "gpu_c_not_installed") },
                        { 12, software_lang.TSReadLangs("Gpu_Content", "gpu_c_install_error") },
                        { 13, software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_saving_mode") },
                        { 14, software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_saving_mode_low_power") },
                        { 15, software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_saving_mode_wait") },
                        { 16, software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_loop") },
                        { 17, software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_saving_warning") },
                        { 18, software_lang.TSReadLangs("Gpu_Content", "gpu_c_paused") },
                        { 19, software_lang.TSReadLangs("Gpu_Content", "gpu_c_not_ready") },
                        { 20, software_lang.TSReadLangs("Gpu_Content", "gpu_c_not_configured") },
                        { 21, software_lang.TSReadLangs("Gpu_Content", "gpu_c_silent_mode") }
                    };
                    if (gpuStatusMessages.TryGetValue(gpu_status, out string gpuStatusMessage)){
                        gpu_status_list.Add(gpuStatusMessage);
                    }else{
                        gpu_status_list.Add(software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown"));
                    }
                    GPU_Status_V.Text = gpu_status_list[0];
                }catch (Exception){ }
                try{
                    // GPU DEVICE ID
                    string gpu_device_id = Convert.ToString(query_vc_rotate["PNPDeviceID"]).Trim();
                    if (!string.IsNullOrEmpty(gpu_device_id)){
                        char[] split_char = { '\\' };
                        string[] gpu_device_split = gpu_device_id.Trim().Split(split_char);
                        gpu_device_id_list.Add($"{gpu_device_split[0]}\\{gpu_device_split[1]}");
                    }else{
                        gpu_device_id_list.Add(software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown"));
                    }
                    GPU_DeviceID_V.Text = gpu_device_id_list[0];
                }catch (Exception){ }
                try{
                    // GPU DAC TYPE
                    string adaptor_dac_type = Convert.ToString(query_vc_rotate["AdapterDACType"]);
                    if (string.IsNullOrEmpty(adaptor_dac_type)){
                        gpu_dac_type_list.Add(software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown"));
                    }else{
                        if (adaptor_dac_type == "Integrated RAMDAC"){
                            gpu_dac_type_list.Add(software_lang.TSReadLangs("Gpu_Content", "gpu_c_integrated_ramdac"));
                        }else if (adaptor_dac_type == "Internal"){
                            gpu_dac_type_list.Add(software_lang.TSReadLangs("Gpu_Content", "gpu_c_internal_ramdac"));
                        }else{
                            gpu_dac_type_list.Add(adaptor_dac_type);
                        }
                    }
                    GPU_DacType_V.Text = gpu_dac_type_list[0];
                }catch (Exception){ }
                try{
                    // GPU DIRECTX DRIVERS
                    var grouped = new Dictionary<string, HashSet<string>>(StringComparer.OrdinalIgnoreCase);
                    var drivers = (query_vc_rotate["InstalledDisplayDrivers"] ?? "").ToString().Split(',');
                    foreach (var p in drivers){
                        var path = p.Trim();
                        if (string.IsNullOrEmpty(path)) continue;
                        int lastSlash = path.LastIndexOf('\\');
                        var fileName = lastSlash >= 0 && lastSlash < path.Length - 1 ? path.Substring(lastSlash + 1) : path;
                        if (string.IsNullOrEmpty(fileName) || fileName.IndexOfAny(new[] { '<', '>' }) >= 0)
                            continue;
                        var directory = lastSlash > 0 ? path.Substring(0, lastSlash) : "";
                        int secondLastSlash = directory.LastIndexOf('\\');
                        var key = secondLastSlash >= 0 ? directory.Substring(secondLastSlash + 1) : directory;
                        if (string.IsNullOrEmpty(key)) continue;
                        if (!grouped.TryGetValue(key, out var set))
                            grouped[key] = set = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                        set.Add(fileName);
                    }
                    gpu_drivers_list.AddRange(grouped.Values.Select(dlls => string.Join(", ", dlls.OrderBy(x => x, StringComparer.OrdinalIgnoreCase))));
                    GPU_GraphicDriversName_V.Text = gpu_drivers_list[0];
                }catch (Exception){ }
                try{
                    // GPU INF FILE NAME
                    string gpu_inf_file = Convert.ToString(query_vc_rotate["InfFilename"]);
                    if (!string.IsNullOrEmpty(gpu_inf_file)){
                        gpu_inf_file_list.Add(gpu_inf_file);
                        GPU_InfFileName_V.Text = gpu_inf_file_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU INF FILE GPU INFO PARTITION
                    string gpu_inf_section = Convert.ToString(query_vc_rotate["InfSection"]);
                    if (!string.IsNullOrEmpty(gpu_inf_section)){
                        gpu_inf_file_section_list.Add(gpu_inf_section);
                        GPU_INFSectionFile_V.Text = gpu_inf_file_section_list[0];
                    }
                }catch (Exception){ }
                try{
                    // CURRENT NUMBER OF COLORS
                    long gpu_current_color = Convert.ToInt64(query_vc_rotate["CurrentNumberOfColors"]);
                    CultureInfo currentCulture = CultureInfo.CurrentCulture;
                    string formattedNumber = gpu_current_color.ToString("N0", currentCulture);
                    gpu_current_colors_list.Add(formattedNumber);
                    GPU_CurrentColor_V.Text = gpu_current_colors_list[0];
                }catch (Exception){ }
            }
            // MONITOR INFORMATIONS
            // ======================================
            try{
                List<string> edid_model_list = new List<string>();
                List<string> edid_manufacturer_list = new List<string>();
                List<string> edid_product_code_list = new List<string>();
                List<string> edid_serial_list = new List<string>();
                List<string> edid_manuf_list = new List<string>();
                List<string> edid_manuf_week_list = new List<string>();
                List<string> edid_manuf_hid_list = new List<string>();
                //
                string local_display_name = software_lang.TSReadLangs("Gpu_Content", "gpu_c_d_name");
                string unknown_message = software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown");
                //
                using (var searchMonitorInfo = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM WmiMonitorID")){
                    foreach (ManagementObject mo in searchMonitorInfo.Get().Cast<ManagementObject>()){
                        string instanceName = "", model = "", manufacturer = "", productCode = "", serial = "", manuf_week = "";
                        UInt16 manuf_year = 0;
                        //
                        try { instanceName = mo["InstanceName"]?.ToString().Trim() ?? ""; } catch { }
                        try { model = GetMonitorFromUShortArray((ushort[])mo["UserFriendlyName"]); } catch { }
                        try { manufacturer = GetMonitorFromUShortArray((ushort[])mo["ManufacturerName"]); } catch { }
                        try { productCode = GetMonitorFromUShortArray((ushort[])mo["ProductCodeID"]); } catch { }
                        try { serial = GetMonitorFromUShortArray((ushort[])mo["SerialNumberID"]); } catch { }
                        try { manuf_year = (UInt16)mo["YearOfManufacture"]; } catch { }
                        try { manuf_week = TSGetFormattedWeekInfo(manuf_year, Convert.ToInt32(Convert.ToUInt32(mo["WeekOfManufacture"]))); } catch { }
                        //
                        if (string.IsNullOrWhiteSpace(model)) model = local_display_name;
                        //
                        edid_model_list.Add(model.Trim('\0').Trim());
                        edid_manufacturer_list.Add(string.IsNullOrWhiteSpace(manufacturer) ? unknown_message : manufacturer.Trim());
                        edid_product_code_list.Add(string.IsNullOrWhiteSpace(productCode) ? unknown_message : productCode.Trim());
                        edid_serial_list.Add(string.IsNullOrWhiteSpace(serial) ? unknown_message : serial.Trim());
                        edid_manuf_list.Add(manuf_year == 0 ? unknown_message : manuf_year.ToString().Trim());
                        edid_manuf_week_list.Add(string.IsNullOrWhiteSpace(manuf_week) ? unknown_message : manuf_week.Trim());
                        edid_manuf_hid_list.Add(string.IsNullOrWhiteSpace(instanceName) ? unknown_message : instanceName.Trim());
                    }
                }
                //
                var connectionParamsSearcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM WmiMonitorConnectionParams");
                var videoModeMapByKey = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                var videoModeMapStatic = new Dictionary<long, string>{
                    { -2, software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown") },
                    { -1, software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown") },
                    { 0, "HD15 (VGA)" },
                    { 1, "S-Video" },
                    { 2, "Composite Video" },
                    { 3, "Component Video" },
                    { 4, "DVI" },
                    { 5, "HDMI" },
                    { 6, "LVDS / MIPI DSI" },
                    { 8, "D-Jpn" },
                    { 9, "SDI" },
                    { 10, string.Format("DisplayPort ({0})", software_lang.TSReadLangs("Gpu_Content", "gpu_c_external")) },
                    { 11, string.Format("DisplayPort ({0})", software_lang.TSReadLangs("Gpu_Content", "gpu_c_embedded")) },
                    { 12, string.Format("UDI ({0})", software_lang.TSReadLangs("Gpu_Content", "gpu_c_external")) },
                    { 13, string.Format("UDI ({0})", software_lang.TSReadLangs("Gpu_Content", "gpu_c_embedded")) },
                    { 14, "SDTV Dongle" },
                    { 15, string.Format("Miracast ({0})", software_lang.TSReadLangs("Gpu_Content", "gpu_c_wireless")) },
                    { 16, "Indirect Wired" },
                    { unchecked((int)0x80000000), software_lang.TSReadLangs("Gpu_Content", "gpu_c_d_name") }
                };
                foreach (ManagementObject mp in connectionParamsSearcher.Get().Cast<ManagementObject>()){
                    try{
                        string inst = mp["InstanceName"]?.ToString() ?? "";
                        long video_mode = Convert.ToInt64(mp["VideoOutputTechnology"]);
                        string videoModeRender = videoModeMapStatic.TryGetValue(video_mode, out var vm) ? vm : "eDP";
                        string instNorm = inst.Trim();
                        if (!string.IsNullOrEmpty(instNorm)){
                            if (!videoModeMapByKey.ContainsKey(instNorm)){
                                videoModeMapByKey[instNorm] = videoModeRender;
                            }
                        }
                        string[] parts = instNorm.Split('\\');
                        if (parts.Length > 1){
                            string shortId = parts[1].Trim();
                            if (!string.IsNullOrEmpty(shortId) && !videoModeMapByKey.ContainsKey(shortId)){
                                videoModeMapByKey[shortId] = videoModeRender;
                            }
                        }
                        if (parts.Length > 2){
                            string tail = parts[parts.Length - 1].Trim();
                            if (!string.IsNullOrEmpty(tail) && !videoModeMapByKey.ContainsKey(tail)){
                                videoModeMapByKey[tail] = videoModeRender;
                            }
                        }
                    }catch { }
                }
                for (int i = 0; i < Screen.AllScreens.Length; i++){
                    var screen = Screen.AllScreens[i];
                    var dm = new DEVMODE { dmSize = (short)Marshal.SizeOf(typeof(DEVMODE)) };
                    EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);
                    //
                    DISPLAY_DEVICE d = new DISPLAY_DEVICE();
                    d.cb = Marshal.SizeOf(d);
                    EnumDisplayDevices(screen.DeviceName, 0, ref d, 0);
                    string deviceKey = d.DeviceID ?? "";
                    int matchedIndex = -1;
                    string devKeyPart = "";
                    if (deviceKey.Contains("\\")){
                        devKeyPart = deviceKey.Split('\\')[1].Trim().ToUpperInvariant();
                    }else{
                        devKeyPart = deviceKey.Trim().ToUpperInvariant();
                    }
                    //
                    for (int j = 0; j < edid_manuf_hid_list.Count; j++){
                        string instanceName = edid_manuf_hid_list[j] ?? "";
                        if (string.IsNullOrWhiteSpace(instanceName)){
                            continue;
                        }
                        if (instanceName.Equals(deviceKey, StringComparison.OrdinalIgnoreCase) || instanceName.IndexOf(devKeyPart, StringComparison.OrdinalIgnoreCase) >= 0){
                            matchedIndex = j;
                            break;
                        }
                        string[] p = instanceName.Split('\\');
                        if (p.Length > 1 && p[1].Trim().Equals(devKeyPart, StringComparison.OrdinalIgnoreCase)){
                            matchedIndex = j;
                            break;
                        }
                    }
                    string resolvedVideoOutput = null;
                    if (matchedIndex != -1){
                        string instCandidate = edid_manuf_hid_list[matchedIndex] ?? "";
                        if (!string.IsNullOrWhiteSpace(instCandidate) && videoModeMapByKey.TryGetValue(instCandidate, out var v1)){
                            resolvedVideoOutput = v1;
                        }else{
                            string[] p = instCandidate.Split('\\');
                            if (p.Length > 1 && videoModeMapByKey.TryGetValue(p[1], out var v2)){
                                resolvedVideoOutput = v2;
                            }
                        }
                    }
                    if (resolvedVideoOutput == null){
                        string[] dkParts = deviceKey.Split('\\');
                        if (dkParts.Length > 1 && videoModeMapByKey.TryGetValue(dkParts[1], out var vv)){
                            resolvedVideoOutput = vv;
                        }else if (dkParts.Length > 2 && videoModeMapByKey.TryGetValue(dkParts[dkParts.Length - 1], out var vv2)){
                            resolvedVideoOutput = vv2;
                        }
                    }
                    if (resolvedVideoOutput == null){
                        resolvedVideoOutput = software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown");
                    }
                    gpu_monitor_user_friendly_name_list.Add(matchedIndex != -1 ? edid_model_list[matchedIndex] : local_display_name);
                    gpu_monitor_manufacturer_list.Add(matchedIndex != -1 ? edid_manufacturer_list[matchedIndex] : unknown_message);
                    gpu_monitor_product_code_id_list.Add(matchedIndex != -1 ? edid_product_code_list[matchedIndex] : unknown_message);
                    gpu_monitor_serial_number_id_list.Add(matchedIndex != -1 ? edid_serial_list[matchedIndex] : unknown_message);
                    gpu_monitor_manuf_list.Add(matchedIndex != -1 ? edid_manuf_list[matchedIndex] : unknown_message);
                    gpu_monitor_manuf_week_list.Add(matchedIndex != -1 ? edid_manuf_week_list[matchedIndex] : unknown_message);
                    gpu_monitor_hid_list.Add(matchedIndex != -1 ? edid_manuf_hid_list[matchedIndex] : unknown_message);
                    gpu_monitor_con_type_list.Add(resolvedVideoOutput);
                    //
                    gpu_monitor_bounds_list.Add(GPUFormatScreenInfo(screen.Bounds));
                    gpu_monitor_work_list.Add(GPUFormatScreenInfo(screen.WorkingArea));
                    gpu_monitor_primary_list.Add(GPUFormatPrimaryScreen(screen.Primary));
                    gpu_monitor_res_list.Add(dm.dmPelsWidth + " x " + dm.dmPelsHeight);
                    gpu_monitor_virtual_res_list.Add(screen.Bounds.Width + " x " + screen.Bounds.Height);
                    gpu_monitor_refresh_rate_list.Add(dm.dmDisplayFrequency + " Hz");
                    gpu_monitor_bit_deep_list.Add(dm.dmBitsPerPel + " Bit");
                    //
                    GPU_MonitorSelector_List.Items.Add(string.Format("{0} #{1} - {2}", software_lang.TSReadLangs("Gpu_Content", "gpu_c_monitor_select"), i + 1, gpu_monitor_user_friendly_name_list[i]));
                    if (screen.Primary){
                        GPU_MonitorSelector_List.SelectedIndex = i;
                    }
                }
            }catch (Exception){ }
            try{
                // GPU VRAM
                gpu_vram_list.Clear();
                gpu_vram_list.AddRange(Enumerable.Repeat(software_lang.TSReadLangs("Gpu_Content", "gpu_c_loading"), GPU_Selector_List.Items.Count));
                _ = LoadGpuVRAMAsync();
            }catch (Exception){ }
            // GPU SELECT
            try { GPU_Selector_List.SelectedIndex = 0; }catch(Exception){ }
            // GPU PROCESS END ENABLED
            GPU_RotateBtn.Enabled = true;
            ((Control)GPU).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- GPU Section Loaded --->"); }
        }
        // GPU VRAM
        // ======================================================================================================
        private async Task LoadGpuVRAMAsync(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            GPU_VRAM_V.Text = software_lang.TSReadLangs("Gpu_Content", "gpu_c_loading");
            //
            await Task.Run(() =>{
                string dxdiagXml = Path.Combine(Path.GetTempPath(), $"dxdiag_{Guid.NewGuid()}.xml");
                try{
                    var proc = new Process{
                        StartInfo = {
                            FileName = "dxdiag.exe",
                            Arguments = $"/whql:off /x \"{dxdiagXml}\"",
                            CreateNoWindow = true,
                            UseShellExecute = false
                        }
                    };
                    proc.Start();
                    proc.WaitForExit();
                    //
                    var dx = new XmlDocument();
                    dx.Load(dxdiagXml);
                    //
                    XmlNodeList displayNodes = dx.SelectNodes("/DxDiag/DisplayDevices/DisplayDevice");
                    HashSet<string> seenGPUs = new HashSet<string>();
                    //
                    foreach (XmlNode node in displayNodes){
                        string gpuName = node.SelectSingleNode("CardName")?.InnerText ?? software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown");
                        if (seenGPUs.Contains(gpuName)){
                            continue;
                        }
                        seenGPUs.Add(gpuName);
                        //
                        long vramBytes = 0;
                        string dedicatedMemory = node.SelectSingleNode("DedicatedMemory")?.InnerText ?? "0 MB";
                        if (dedicatedMemory.EndsWith("MB") && int.TryParse(dedicatedMemory.Replace("MB", "").Trim(), out int parsedMB)){
                            vramBytes = (long)parsedMB * 1024 * 1024;
                        }
                        // Fuzzy Match
                        int bestIndex = -1;
                        int bestScore = int.MaxValue;
                        for (int i = 0; i < GPU_Selector_List.Items.Count; i++){
                            string itemName = GPU_Selector_List.Items[i].ToString();
                            int score = LevenshteinDistance(gpuName.ToLower(), itemName.ToLower());
                            if (score < bestScore){
                                bestScore = score;
                                bestIndex = i;
                            }
                        }
                        if (bestIndex >= 0){
                            gpu_vram_list[bestIndex] = TS_FormatSize(vramBytes);
                        }
                    }
                }catch{ }
                finally{
                    try{
                        if (File.Exists(dxdiagXml)){
                            File.Delete(dxdiagXml);
                        }
                    }catch{ }
                }
            });
            //
            GPU_VRAM_V.Invoke(new Action(() =>{
                int gpu_select = GPU_Selector_List.SelectedIndex;
                if (gpu_select >= 0 && gpu_select < gpu_vram_list.Count){
                    GPU_VRAM_V.Text = gpu_vram_list[gpu_select];
                }else{
                    GPU_VRAM_V.Text = software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown");
                }
            }));
        }
        // LEVENSHTEIN DISTANCE ALGORITHM
        // ======================================================================================================
        private int LevenshteinDistance(string a, string b){
            int[,] d = new int[a.Length + 1, b.Length + 1];
            for (int i = 0; i <= a.Length; i++) d[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) d[0, j] = j;
            for (int i = 1; i <= a.Length; i++){
                for (int j = 1; j <= b.Length; j++){
                    int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }
            return d[a.Length, b.Length];
        }
        // WEEKLY NUMBER CONVERTER
        // ======================================================================================================
        static string TSGetFormattedWeekInfo(int year, int weekNumber){
            var software_lang = new TSGetLangs(lang_path);
            if (weekNumber < 1 || weekNumber > 53)
                return software_lang.TSReadLangs("Gpu_Content", "gpu_c_date_valid_info");
            DateTime jan4 = new DateTime(year, 1, 4);
            int offset = ((int)jan4.DayOfWeek + 6) % 7;
            DateTime firstMonday = jan4.AddDays(-offset);
            DateTime targetMonday = firstMonday.AddDays((weekNumber - 1) * 7);
            DateTime firstDayOfMonth = new DateTime(targetMonday.Year, targetMonday.Month, 1);
            int firstDayOffset = ((int)firstDayOfMonth.DayOfWeek + 6) % 7;
            DateTime firstMondayOfMonth = firstDayOfMonth.AddDays(firstDayOffset == 0 ? 0 : 7 - firstDayOffset);
            int weekOfMonth = targetMonday < firstMondayOfMonth ? 1 : 1 + (int)((targetMonday - firstMondayOfMonth).TotalDays / 7);
            string monthName = targetMonday.ToString("MMMM", CultureInfo.CurrentCulture);
            return string.Format(software_lang.TSReadLangs("Gpu_Content", "gpu_c_date_info"), year, monthName, weekOfMonth);
        }
        // GPU SELECT
        // ======================================================================================================
        private void GPU_Selector_List_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int gpu_select = GPU_Selector_List.SelectedIndex;
                try { GPU_Manufacturer_V.Text = gpu_man_list[gpu_select]; } catch (Exception) { }
                try { TSGetLangs software_lang = new TSGetLangs(lang_path); GPU_VRAM_V.Text = (gpu_select >= 0 && gpu_select < gpu_vram_list.Count && !string.IsNullOrWhiteSpace(gpu_vram_list[gpu_select]) && gpu_vram_list[gpu_select] != software_lang.TSReadLangs("Gpu_Content", "gpu_c_loading")) ? gpu_vram_list[gpu_select] : software_lang.TSReadLangs("Gpu_Content", "gpu_c_loading"); } catch (Exception) { }
                try { GPU_Version_V.Text = gpu_driver_version_list[gpu_select]; } catch (Exception) { }
                try { GPU_DriverDate_V.Text = gpu_driver_date_list[gpu_select]; } catch (Exception) { }
                try { GPU_Status_V.Text = gpu_status_list[gpu_select]; } catch (Exception) { }
                try { GPU_DeviceID_V.Text = gpu_device_id_list[gpu_select]; } catch (Exception) { }
                try { GPU_DacType_V.Text = gpu_dac_type_list[gpu_select]; } catch (Exception) { }
                try { GPU_GraphicDriversName_V.Text = gpu_drivers_list[gpu_select]; } catch (Exception) { }
                try { GPU_InfFileName_V.Text = gpu_inf_file_list[gpu_select]; } catch (Exception) { }
                try { GPU_INFSectionFile_V.Text = gpu_inf_file_section_list[gpu_select]; } catch (Exception) { }
                try { GPU_CurrentColor_V.Text = gpu_current_colors_list[gpu_select]; } catch (Exception) { }
            }catch (Exception){ }
        }
        private void GPU_MonitorSelector_List_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int monitor_select = GPU_MonitorSelector_List.SelectedIndex;
                try { GPU_MonitorUserFriendlyName_V.Text = gpu_monitor_user_friendly_name_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorManName_V.Text = gpu_monitor_manufacturer_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorProductCodeID_V.Text = gpu_monitor_product_code_id_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorSerialNumberID_V.Text = gpu_monitor_serial_number_id_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorConType_V.Text = gpu_monitor_con_type_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorManfDate_V.Text = gpu_monitor_manuf_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorManfDateWeek_V.Text = gpu_monitor_manuf_week_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorHID_V.Text = gpu_monitor_hid_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorResLabel_V.Text = gpu_monitor_res_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorVirtualRes_V.Text = gpu_monitor_virtual_res_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorBounds_V.Text = gpu_monitor_bounds_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorWorking_V.Text = gpu_monitor_work_list[monitor_select]; } catch (Exception) { }
                try { GPU_ScreenRefreshRate_V.Text = gpu_monitor_refresh_rate_list[monitor_select]; } catch (Exception) { }
                try { GPU_ScreenBit_V.Text = gpu_monitor_bit_deep_list[monitor_select]; } catch (Exception) { }
                try { GPU_MonitorPrimary_V.Text = gpu_monitor_primary_list[monitor_select]; } catch (Exception) { }
            }catch (Exception){ }
        }
        // FORMATED SCREEN INFO
        // ======================================================================================================
        public static string GetMonitorFromUShortArray(ushort[] array){
            if (array == null) return string.Empty;
            var sb = new StringBuilder();
            foreach (var v in array){
                if (v > 0 && v <= char.MaxValue)
                    sb.Append((char)v);
            }
            return sb.ToString().Trim();
        }
        private string GPUFormatScreenInfo(Rectangle bounds){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            string formatted = bounds.ToString().Replace("Width", software_lang.TSReadLangs("Gpu_Content", "gpu_c_width")).Replace("Height", software_lang.TSReadLangs("Gpu_Content", "gpu_c_height")).Replace("{", "").Replace("}", "").Replace(",", ", ").Replace("=", ": ");
            return formatted;
        }
        private string GPUFormatPrimaryScreen(bool isPrimary){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            return isPrimary ? software_lang.TSReadLangs("Gpu_Content", "gpu_c_yes") : software_lang.TSReadLangs("Gpu_Content", "gpu_c_no");
        }
        #endregion
        #region DISK_Section
        // DISK
        // ======================================================================================================
        readonly List<string> disk_man_list = new List<string>();
        readonly List<string> disk_model_list = new List<string>();
        readonly List<string> disk_volume_id_list = new List<string>();
        readonly List<string> disk_volume_name_list = new List<string>();
        readonly List<string> disk_firmware_list = new List<string>();
        readonly List<string> disk_serial_list = new List<string>();
        readonly List<string> disk_volume_serial_list = new List<string>();
        readonly List<string> disk_total_space_list = new List<string>();
        readonly List<string> disk_free_space_list = new List<string>();
        readonly List<string> disk_file_system_list = new List<string>();
        readonly List<string> disk_formatting_system_list = new List<string>();
        readonly List<string> disk_type_list = new List<string>();
        readonly List<string> disk_drive_type_list = new List<string>();
        readonly List<string> disk_interface_list = new List<string>();
        readonly List<string> disk_partition_list = new List<string>();
        readonly List<string> disk_media_loaded_list = new List<string>();
        readonly List<string> disk_media_status_list = new List<string>();
        readonly List<string> disk_health_status_list = new List<string>();
        readonly List<string> disk_boot_list = new List<string>();
        readonly List<string> disk_bootable_list = new List<string>();
        readonly List<string> disk_bitlocker_status_list = new List<string>();
        readonly List<string> disk_bitlocker_conversionstatus_list = new List<string>();
        readonly List<string> disk_bitlocker_encryptionmethod_list = new List<string>();
        readonly List<string> disk_drive_compressed_list = new List<string>();
        private void Disk(){
            // DISK COUNTER
            int disk_ssd_count = 0;
            int disk_hdd_count = 0;
            int disk_usb_count = 0;
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            try{
                var get_drives = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject drive_info in get_drives.Get().Cast<ManagementObject>()){
                    var device_id = drive_info.Properties["DeviceId"].Value;
                    var disk_part_text_query = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", drive_info.Path.RelativePath);
                    var disk_part_query = new ManagementObjectSearcher(disk_part_text_query);
                    foreach (ManagementObject disk_partition in disk_part_query.Get().Cast<ManagementObject>()){
                        var logical_disk_text_query = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", disk_partition.Path.RelativePath);
                        var logical_disk_query = new ManagementObjectSearcher(logical_disk_text_query);
                        foreach (ManagementObject logical_drive_info in logical_disk_query.Get().Cast<ManagementObject>()){
                            try{
                                // DISK CAPTION / DISK SELECT LIST
                                var disk_caption = Convert.ToString(drive_info.Properties["Caption"].Value).Trim();
                                if (!string.IsNullOrEmpty(disk_caption)){
                                    DISK_Selector_List.Items.Add(disk_caption);
                                }else{
                                    DISK_Selector_List.Items.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                }
                            }catch (Exception){ }
                            try{
                                // DISK MANUFACTURER
                                List<string> disk_mfr = new List<string>(){
                                    "acer", "a-data", "adata", "addlink", "alpin", "apacer", "apple",
                                    "asus", "biostar", "codegen", "colorful", "corsair", "crucial", "ezcool", "geil", "gigabyte", "goodram",
                                    "hi level", "hikvision", "hp", "intel", "intenso", "james donkey", "kingspec", "kingston", "kioxia",
                                    "leven", "lexar", "micron", "mld", "msi", "mushkin", "neo forza", "neoforza", "netac", "patriot", "pioneer",
                                    "pny", "samsung", "sandisk", "seagate", "siliconpower", "team", "toshiba", "turbox", "wd", "western digital",
                                    "verbatim", "maxtor", "hitachi", "fujitsu", "transcend", "ocz", "hynix", "plextor", "liteon", "verico", "qnap",
                                    "lenovo", "maxell", "teac", "quantum", "panasonic", "nec", "iomega", "buffalo", "buslink", "busbi", "centon",
                                    "cm storm", "datawrite", "duracell", "dynamode", "emtec", "excelstor", "fuji", "hgst", "hyundai", "imation",
                                    "kanguru", "kingmax", "kodak", "lacie", "memorex", "minox", "mio", "olympus", "optiarc", "philips",
                                    "pinnacle", "pnypqi", "ramsta", "ricoh", "rokit", "sigma", "sk hynix", "smartbuy", "sony",
                                    "super talent", "trimble", "traxdata", "viking", "xerox", "xmedia", "zotac", "zoostorm"
                                };
                                disk_mfr.Sort();
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher disk_model_and_man_query = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                foreach (ManagementObject disk_model_and_man_search in disk_model_and_man_query.Get().Cast<ManagementObject>()){
                                    // DISK MODEL
                                    var disk_model = Convert.ToString(disk_model_and_man_search["Model"]).Trim();
                                    if (!string.IsNullOrEmpty(disk_model)){
                                        disk_model_list.Add(disk_model);
                                    }else{
                                        disk_model_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                    }
                                    // DISK MAN
                                    var disk_man = Convert.ToString(disk_model_and_man_search["Manufacturer"]).Trim();
                                    //
                                    string[] termsToCheck = { "nvme", "usb" };
                                    foreach (string term in termsToCheck){
                                        if (disk_man.ToLower() == term){
                                            disk_man = string.Empty;
                                            break;
                                        }
                                    }
                                    //
                                    if (string.IsNullOrEmpty(disk_man)){
                                        string disk_mod_lowercase = disk_model.ToLower().Replace("ı", "i");
                                        for (int i = 0; i <= disk_mfr.Count - 1; i++){
                                            bool target_detect_disk = disk_mod_lowercase.Contains(disk_mfr[i]);
                                            if (target_detect_disk == true){
                                                disk_man_list.Add(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(disk_mfr[i]));
                                                //disk_man_list.Add(disk_mans[i].ToUpper());
                                                break;
                                            }
                                            if (i == disk_mfr.Count - 1){
                                                disk_man_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                            }
                                        }
                                    }else{
                                        disk_man_list.Add(disk_man);
                                    }
                                }
                                DISK_Model_V.Text = disk_model_list[0];
                                DISK_Man_V.Text = disk_man_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK VOLUME ID
                                var disk_volume_id = Convert.ToString(logical_drive_info.Properties["Name"].Value).Trim();
                                if (!string.IsNullOrEmpty(disk_volume_id)){
                                    disk_volume_id_list.Add(disk_volume_id + @"\");
                                }else{
                                    disk_volume_id_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                }
                                DISK_VolumeID_V.Text = disk_volume_id_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK VOLUME NAME
                                var disk_volume_name = Convert.ToString(logical_drive_info.Properties["VolumeName"].Value).Trim();
                                if (!string.IsNullOrEmpty(disk_volume_name)){
                                    disk_volume_name_list.Add(disk_volume_name);
                                }else{
                                    disk_volume_name_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_local_disk"));
                                }
                                DISK_VolumeName_V.Text = disk_volume_name_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK FIRMWARE VERSION
                                var disk_firmware_version = Convert.ToString(drive_info.Properties["FirmwareRevision"].Value).Trim();
                                if (!string.IsNullOrEmpty(disk_firmware_version)){
                                    disk_firmware_list.Add(disk_firmware_version);
                                }else{
                                    disk_firmware_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                }
                                DISK_Firmware_V.Text = disk_firmware_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK SERIAL NUMBER
                                var disk_serial_number = Convert.ToString(drive_info.Properties["SerialNumber"].Value).Trim();
                                if (!string.IsNullOrEmpty(disk_serial_number)){
                                    if (hiding_mode_wrapper != 1){
                                        disk_serial_list.Add(disk_serial_number);
                                    }else{
                                        disk_serial_list.Add(new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})");
                                    }
                                }else{
                                    disk_serial_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                }
                                DISK_Serial_V.Text = disk_serial_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK VOLUME SERIAL NUMBER
                                var disk_volume_serial_number = Convert.ToString(logical_drive_info.Properties["VolumeSerialNumber"].Value).Trim();
                                if (!string.IsNullOrEmpty(disk_volume_serial_number)){
                                    if (hiding_mode_wrapper != 1){
                                        disk_volume_serial_list.Add(disk_volume_serial_number);
                                    }else{
                                        disk_volume_serial_list.Add(new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})");
                                    }
                                }else{
                                    disk_volume_serial_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                }
                                DISK_VolumeSerial_V.Text = disk_volume_serial_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK TOTAL SIZE
                                var disk_total_size = Convert.ToDouble(logical_drive_info.Properties["Size"].Value); // in byte
                                disk_total_space_list.Add(TS_FormatSize(disk_total_size));
                                DISK_Size_V.Text = disk_total_space_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK FREE SPACE
                                var disk_free_space = Convert.ToDouble(logical_drive_info.Properties["FreeSpace"].Value); // in byte
                                disk_free_space_list.Add(TS_FormatSize(disk_free_space));
                                DISK_FreeSpace_V.Text = disk_free_space_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK FILE SYSTEM
                                var disk_file_system = Convert.ToString(logical_drive_info.Properties["FileSystem"].Value).Trim();
                                if (!string.IsNullOrEmpty(disk_file_system)){
                                    disk_file_system_list.Add(disk_file_system);
                                }else{
                                    disk_file_system_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                }
                                DISK_FileSystem_V.Text = disk_file_system_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK FORMATTING SYSTEM
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher disk_part_search = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                foreach (ManagementObject dp_search in disk_part_search.Get().Cast<ManagementObject>()){
                                   var disk_part_style = Convert.ToInt32(dp_search["PartitionStyle"]);
                                    switch (disk_part_style){
                                        case 0:
                                            disk_formatting_system_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                            break;
                                        case 1:
                                            disk_formatting_system_list.Add("MBR");
                                            break;
                                        case 2:
                                            disk_formatting_system_list.Add("GPT");
                                            break;
                                    }
                                }
                            }catch (Exception){ }
                            try{
                                // DISK TYPE
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                var disk_media_type = Convert.ToString(drive_info.Properties["MediaType"].Value).ToLower().Trim();
                                ManagementObjectSearcher search_disk_type = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_PhysicalDisk WHERE DeviceID={disk_index}");
                                foreach (ManagementObject search_disk_t in search_disk_type.Get().Cast<ManagementObject>()){
                                    // DISK TYPE INTERFACE
                                    var disk_type = Convert.ToInt32(search_disk_t["MediaType"]);
                                    switch (disk_type){
                                        case 0:
                                            // Bilinmiyor
                                            switch (disk_media_type){
                                                case "external hard disk media":
                                                    disk_type_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_external_disk"));
                                                    disk_usb_count++;
                                                    break;
                                                case "removable media":
                                                    disk_type_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_usb_disk"));
                                                    disk_usb_count++;
                                                    break;
                                                default:
                                                    disk_type_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                                    break;
                                            }
                                            break;
                                        case 3:
                                            // HDD
                                            disk_type_list.Add("HDD");
                                            disk_hdd_count++;
                                            break;
                                        case 4:
                                            // SSD
                                            disk_type_list.Add("SSD");
                                            disk_ssd_count++;
                                            break;
                                        case 5:
                                            // SCM
                                            disk_type_list.Add("SCM");
                                            break;
                                    }
                                    DISK_Type_V.Text = disk_type_list[0];
                                    // DISK DRIVE TYPE INTERFACE
                                    switch (disk_media_type){
                                        case "fixed hard disk media":
                                            switch (disk_type){
                                                case 3:
                                                    disk_drive_type_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_fixed_drive"));
                                                    break;
                                                case 4:
                                                    disk_drive_type_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_ssd"));
                                                    break;
                                            }
                                            break;
                                        case "external hard disk media":
                                            disk_drive_type_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_external_drive"));
                                            break;
                                        case "removable media":
                                            disk_drive_type_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_removable_drive"));
                                            break;
                                        default:
                                            disk_drive_type_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                            break;
                                    }
                                    DISK_DriveType_V.Text = disk_drive_type_list[0];
                                }
                            }catch (Exception){ }
                            try{
                                // DISK INTERFACE
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher disk_interface = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                foreach (ManagementObject disk_int_query in disk_interface.Get().Cast<ManagementObject>()){
                                    var di_query = Convert.ToInt32(disk_int_query["BusType"]);
                                    var busTypeMessages = new Dictionary<int, string>{
                                        { 0, software_lang.TSReadLangs("StorageContent", "se_c_unknown") },
                                        { 1, "SCSI" },
                                        { 2, "ATAPI" },
                                        { 3, "ATA" },
                                        { 4, "1394 - IEEE 1394" },
                                        { 5, "SSA" },
                                        { 6, software_lang.TSReadLangs("StorageContent", "se_c_fiber_channel") },
                                        { 7, "USB" },
                                        { 8, "RAID" },
                                        { 9, "iSCSI" },
                                        { 10, software_lang.TSReadLangs("StorageContent", "se_c_scsi_sas") },
                                        { 11, "SATA" },
                                        { 12, software_lang.TSReadLangs("StorageContent", "se_c_secure_digital") },
                                        { 13, software_lang.TSReadLangs("StorageContent", "se_c_multi_media_card") },
                                        { 14, software_lang.TSReadLangs("StorageContent", "se_c_virtual") },
                                        { 15, software_lang.TSReadLangs("StorageContent", "se_c_file_supported_virtual") },
                                        { 16, software_lang.TSReadLangs("StorageContent", "se_c_storage_area") },
                                        { 17, "NVM-e" }
                                    };
                                    if (busTypeMessages.TryGetValue(di_query, out string busTypeMessage)){
                                        disk_interface_list.Add(busTypeMessage);
                                    }else{
                                        disk_interface_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                    }
                                }
                                DISK_InterFace_V.Text = disk_interface_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK PARTITION COUNT
                                var disk_partitions = Convert.ToString(drive_info.Properties["Partitions"].Value).Trim();
                                if (!string.IsNullOrEmpty(disk_partitions)){
                                    disk_partition_list.Add(disk_partitions);
                                }else{
                                    disk_partition_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_unknown"));
                                }
                                DISK_PartitionCount_V.Text = disk_partition_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK MEDIA STATUS
                                var disk_media_loaded = Convert.ToBoolean(drive_info.Properties["MediaLoaded"].Value);
                                if (disk_media_loaded == true){
                                    disk_media_loaded_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_disk_write_and_read"));
                                }else if (disk_media_loaded == false){
                                    disk_media_loaded_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_disk_not_write_and_read"));
                                }
                                DISK_MediaLoaded_V.Text = disk_media_loaded_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK MEDIA STATUS
                                var disk_media_status = Convert.ToString(drive_info.Properties["Status"].Value).ToLower().Trim();
                                var mediaStatusMessages = new Dictionary<string, string>{
                                    { "ok", software_lang.TSReadLangs("StorageContent", "se_c_disk_stable") },
                                    { "error", software_lang.TSReadLangs("StorageContent", "se_c_disk_error") },
                                    { "degraded", software_lang.TSReadLangs("StorageContent", "se_c_disk_broken") },
                                    { "unknown", software_lang.TSReadLangs("StorageContent", "se_c_disk_status_non") },
                                    { "pred fail", software_lang.TSReadLangs("StorageContent", "se_c_pred_fail") },
                                    { "starting", software_lang.TSReadLangs("StorageContent", "se_c_starting") },
                                    { "stopping", software_lang.TSReadLangs("StorageContent", "se_c_stopped") },
                                    { "service", software_lang.TSReadLangs("StorageContent", "se_c_serive") },
                                    { "stressed", software_lang.TSReadLangs("StorageContent", "se_c_stressed") },
                                    { "nonrecover", software_lang.TSReadLangs("StorageContent", "se_c_cannot_be_fixed") },
                                    { "no contact", software_lang.TSReadLangs("StorageContent", "se_c_no_contact") },
                                    { "lost comm", software_lang.TSReadLangs("StorageContent", "se_c_lost_comm") }
                                };
                                if (mediaStatusMessages.TryGetValue(disk_media_status, out string message)){
                                    disk_media_status_list.Add(message);
                                }else{
                                    disk_media_status_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_disk_status_non"));
                                }
                                DISK_MediaStatus_V.Text = disk_media_status_list[0];
                            }catch (Exception){ }
                            try{
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher search_disk_inf_4 = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                foreach (ManagementObject sdi_4 in search_disk_inf_4.Get().Cast<ManagementObject>()){
                                    // HEALTH STATUS
                                    var disk_health = Convert.ToInt32(sdi_4["HealthStatus"]);
                                    switch (disk_health){
                                        case 0:
                                            disk_health_status_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_healt_good"));
                                            break;
                                        case 1:
                                            disk_health_status_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_healt_running_and_error"));
                                            break;
                                        case 2:
                                            disk_health_status_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_defective"));
                                            break;
                                    }
                                    // BOOT DISK
                                    var disk_boot = Convert.ToBoolean(sdi_4["BootFromDisk"]);
                                    switch (disk_boot){
                                        case true:
                                            disk_boot_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_main_boot_disk"));
                                            break;
                                        case false:
                                            disk_boot_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_not_main_boot_disk"));
                                            break;
                                    }
                                    // BOOTABLE DISK
                                    var disk_bootable = Convert.ToBoolean(sdi_4["IsBoot"]);
                                    switch (disk_bootable){
                                        case true:
                                            disk_bootable_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_bootable_disk"));
                                            break;
                                        case false:
                                            disk_bootable_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_not_bootable_disk"));
                                            break;
                                    }
                                }
                                DISK_Health_V.Text = disk_health_status_list[0];
                                DISK_Boot_V.Text = disk_boot_list[0];
                                DISK_Bootable_V.Text = disk_bootable_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK COMPRESSED STATUS
                                var disk_compressed_status = Convert.ToBoolean(logical_drive_info.Properties["Compressed"].Value);
                                if (disk_compressed_status == true){
                                    disk_drive_compressed_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_compressed"));   
                                }else if (disk_compressed_status == false){
                                    disk_drive_compressed_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_not_compressed"));
                                }
                                DISK_DriveCompressed_V.Text = disk_drive_compressed_list[0];
                            }catch (Exception){ }
                            try{
                                // MB BIOS TYPE
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher search_bios_type = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                foreach (ManagementObject sbt in search_bios_type.Get().Cast<ManagementObject>()){
                                    var disk_volume_id = Convert.ToString(logical_drive_info.Properties["Name"].Value).Trim();
                                    if (!string.IsNullOrEmpty(disk_volume_id)){
                                        if (windows_disk.Replace("\\", string.Empty) == disk_volume_id.ToString().Trim()){
                                            var disk_style = Convert.ToInt32(sbt["PartitionStyle"]);
                                            if (disk_style == 1){
                                                // MBR
                                                MB_BiosMode_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_old") + " (Legacy)";
                                            }else if (disk_style == 2){
                                                // GPT
                                                MB_BiosMode_V.Text = "UEFI";
                                            }else{
                                                // NULL
                                                MB_BiosMode_V.Text = software_lang.TSReadLangs("Mb_Content", "mb_c_old") + " (Legacy)";
                                            }
                                        }
                                    }
                                }
                            }catch (Exception){ }
                        }
                    }
                }
            }catch (Exception){ }
            try{
                for (int i = 0; i<= disk_volume_id_list.Count - 1; i++){
                    ManagementObjectSearcher get_bitlocker_status = new ManagementObjectSearcher("root\\CIMV2\\Security\\MicrosoftVolumeEncryption", $"SELECT * FROM Win32_EncryptableVolume WHERE DriveLetter = '{disk_volume_id_list[i].Replace("\\", string.Empty)}'");
                    foreach (ManagementObject query_bitlocker in get_bitlocker_status.Get().Cast<ManagementObject>()){
                        try{
                            string bl_protection_status = Convert.ToString(query_bitlocker["ProtectionStatus"]);
                            var protectionStatusMessages = new Dictionary<int, string>{
                                { 0, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_ps_off") },
                                { 1, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_ps_on") },
                                { 2, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_ps_unknown") }
                            };
                            if (!string.IsNullOrEmpty(bl_protection_status)){
                                int bl_protection_status_process = Convert.ToInt32(bl_protection_status);
                                if (protectionStatusMessages.TryGetValue(bl_protection_status_process, out string message)){
                                    disk_bitlocker_status_list.Add(message);
                                }else{
                                    disk_bitlocker_status_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_ps_unknown"));
                                }
                            }else{
                                disk_bitlocker_status_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_ps_unknown"));
                            }
                            DISK_BitLockerStatus_V.Text = disk_bitlocker_status_list[0];
                        }catch (Exception){ }
                        try{
                            string bl_conversion_status = Convert.ToString(query_bitlocker["ConversionStatus"]);
                            var conversionStatusMessages = new Dictionary<int, string>{
                                { 0, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_cs_no_pass") },
                                { 1, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_cs_yes_pass") },
                                { 2, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_cs_encrypt_continue") },
                                { 3, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_cs_decrypt_continue") },
                                { 4, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_cs_encrypt_paused") },
                                { 5, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_cs_decrypt_paused") }
                            };
                            if (!string.IsNullOrEmpty(bl_conversion_status)){
                                int bl_conversion_status_process = Convert.ToInt32(bl_conversion_status);
                                if (conversionStatusMessages.TryGetValue(bl_conversion_status_process, out string message)){
                                    disk_bitlocker_conversionstatus_list.Add(message);
                                }else{
                                    disk_bitlocker_conversionstatus_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_cs_status_unknown"));
                                }
                            }else{
                                disk_bitlocker_conversionstatus_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_cs_status_unknown"));
                            }
                            DISK_BitLockerConversionStatus_V.Text = disk_bitlocker_conversionstatus_list[0];
                        }catch (Exception){ }
                        try{
                            string bl_encryption_method = Convert.ToString(query_bitlocker["EncryptionMethod"]);
                            var encryptionMethodMessages = new Dictionary<int, string>{
                                { 0, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_no_pass") },
                                { 1, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_pass_aes128d") },
                                { 2, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_pass_aes256d") },
                                { 3, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_pass_aes128") },
                                { 4, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_pass_aes256") },
                                { 5, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_pass_hardware") },
                                { 6, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_pass_xts_aes128") },
                                { 7, software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_pass_xts_aes256d") }
                            };
                            if (!string.IsNullOrEmpty(bl_encryption_method)){
                                int bl_encryption_method_process = Convert.ToInt32(bl_encryption_method);
                                if (encryptionMethodMessages.TryGetValue(bl_encryption_method_process, out string message)){
                                    disk_bitlocker_encryptionmethod_list.Add(message);
                                }else{
                                    disk_bitlocker_encryptionmethod_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_pass_unknown"));
                                }
                            }else{
                                disk_bitlocker_encryptionmethod_list.Add(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_pass_unknown"));
                            }
                            DISK_BitLockerEncryptMehod_V.Text = disk_bitlocker_encryptionmethod_list[0];
                        }catch (Exception){ }
                    }
                }
            }catch (Exception){ }
            // SELECT DISK
            try{
                int c_index = disk_volume_id_list.FindIndex(x => x.Contains(windows_disk));
                DISK_Selector_List.SelectedIndex = c_index;
                if (c_index == -1){
                    DISK_Selector_List.SelectedIndex = 0;
                }
            }catch (Exception){
                DISK_Selector_List.SelectedIndex = 0;
            }
            // DISK COUNTER RENDERER
            DISK_TTLP_P1_L2.Text = disk_ssd_count.ToString();
            DISK_TTLP_P2_L2.Text = disk_hdd_count.ToString();
            DISK_TTLP_P3_L2.Text = disk_usb_count.ToString();
            DISK_TTLP_P4_L2.Text = DISK_Selector_List.Items.Count.ToString();
            // DISK PROCESS END ENABLED
            DISK_RotateBtn.Enabled = true;
            ((Control)DISK).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Storage Section Loaded --->"); }
        }
        // DISK RIGHT PROGRESS FUNCTION
        private void Disk_progress_function(int _pb_disk){
            try{
                string totalDiskSpaceStr = disk_total_space_list[_pb_disk];
                string freeDiskSpaceStr = disk_free_space_list[_pb_disk];
                //
                long ConvertToMB(string sizeStr){
                    sizeStr = sizeStr.Trim().ToUpper();
                    //
                    string pattern = @"([\d.,]+)\s*(B|KB|MB|GB|TB|PB|EB|ZB|YB)";
                    Match match = Regex.Match(sizeStr, pattern);
                    string numericValueStr = match.Groups[1].Value.Replace(',', '.');
                    string unit = match.Groups[2].Value;
                    //
                    double numericValue = double.Parse(numericValueStr, CultureInfo.InvariantCulture);
                    string[] unitScale = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
                    int unitIndex = Array.IndexOf(unitScale, unit);
                    double sizeInMB = unitIndex >= 2 ? numericValue * Math.Pow(1024, unitIndex - 2) : numericValue / Math.Pow(1024, 2 - unitIndex);
                    //
                    return (long)Math.Floor(sizeInMB);
                }
                //
                long totalDiskSpaceMB = ConvertToMB(totalDiskSpaceStr);
                long freeDiskSpaceMB = ConvertToMB(freeDiskSpaceStr);
                long usedDiskSpaceMB = totalDiskSpaceMB - freeDiskSpaceMB;
                int usagePercentage = (int)((double)usedDiskSpaceMB / totalDiskSpaceMB * 100);
                //
                DISK_PBar_FE.Height = (int)(DISK_PBar_BG.Height * (usagePercentage / 100.0));
                DISK_PBar_Label.Text = usagePercentage + "%";
                if (usagePercentage <= 7){
                    DISK_PBar_Label.Top = DISK_PBar_FE.Top - 2;
                }else{
                    DISK_PBar_Label.Top = DISK_PBar_FE.Top + 6;
                }
            }catch (Exception){ }
        }
        private void DISK_Selector_List_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int disk_percent = DISK_Selector_List.SelectedIndex;
                try { Disk_progress_function(disk_percent); } catch (Exception) { }
                try { DISK_Model_V.Text = disk_model_list[disk_percent]; } catch (Exception) { }
                try { DISK_Man_V.Text = disk_man_list[disk_percent]; } catch (Exception) { }
                try { DISK_VolumeID_V.Text = disk_volume_id_list[disk_percent]; } catch (Exception) { }
                try { DISK_VolumeName_V.Text = disk_volume_name_list[disk_percent]; } catch (Exception) { }
                try { DISK_Firmware_V.Text = disk_firmware_list[disk_percent]; } catch (Exception) { }
                try { DISK_Serial_V.Text = disk_serial_list[disk_percent]; } catch (Exception) { }
                try { DISK_VolumeSerial_V.Text = disk_volume_serial_list[disk_percent]; } catch (Exception) { }
                try { DISK_Size_V.Text = disk_total_space_list[disk_percent]; } catch (Exception) { }
                try { DISK_FreeSpace_V.Text = disk_free_space_list[disk_percent]; } catch (Exception) { }
                try { DISK_FileSystem_V.Text = disk_file_system_list[disk_percent]; } catch (Exception) { }
                try { DISK_FormattingType_V.Text = disk_formatting_system_list[disk_percent]; } catch (Exception) { }
                try { DISK_Type_V.Text = disk_type_list[disk_percent]; } catch (Exception) { }
                try { DISK_DriveType_V.Text = disk_drive_type_list[disk_percent]; } catch (Exception) { }
                try { DISK_InterFace_V.Text = disk_interface_list[disk_percent]; } catch (Exception) { }
                try { DISK_PartitionCount_V.Text = disk_partition_list[disk_percent]; } catch (Exception) { }
                try { DISK_MediaLoaded_V.Text = disk_media_loaded_list[disk_percent]; } catch (Exception) { }
                try { DISK_MediaStatus_V.Text = disk_media_status_list[disk_percent]; } catch (Exception) { }
                try { DISK_Health_V.Text = disk_health_status_list[disk_percent]; } catch (Exception) { }
                try { DISK_Boot_V.Text = disk_boot_list[disk_percent]; } catch (Exception) { }
                try { DISK_Bootable_V.Text = disk_bootable_list[disk_percent]; } catch (Exception) { }
                try { DISK_BitLockerStatus_V.Text = disk_bitlocker_status_list[disk_percent]; } catch (Exception) { }
                try { DISK_BitLockerConversionStatus_V.Text = disk_bitlocker_conversionstatus_list[disk_percent]; } catch (Exception) { }
                try { DISK_BitLockerEncryptMehod_V.Text = disk_bitlocker_encryptionmethod_list[disk_percent]; } catch (Exception) { }
                try { DISK_DriveCompressed_V.Text = disk_drive_compressed_list[disk_percent]; } catch (Exception) { }
            }catch (Exception){ }
        }
        #endregion
        #region NETWORK_Section
        // NETWORK
        // ======================================================================================================
        readonly List<string> network_mac_adress_list = new List<string>();
        readonly List<string> network_man_list = new List<string>();
        readonly List<string> network_driver_version_list = new List<string>();
        readonly List<string> network_driver_date_list = new List<string>();
        readonly List<string> network_service_name_list = new List<string>();
        readonly List<string> network_adaptor_type_list = new List<string>();
        readonly List<string> network_physical_list = new List<string>();
        readonly List<string> network_device_id_list = new List<string>();
        readonly List<string> network_guid_list = new List<string>();
        readonly List<string> network_connection_type_list = new List<string>();
        readonly List<string> network_dhcp_status_list = new List<string>();
        readonly List<string> network_dhcp_server_list = new List<string>();
        readonly List<string> network_dhcp_first_ip_time_list = new List<string>();
        readonly List<string> network_dhcp_last_ip_time_list = new List<string>();
        readonly List<string> network_connection_speed_list = new List<string>();
        readonly List<string> network_ipv4_list = new List<string>();
        readonly List<string> network_ipv6_list = new List<string>();
        // DNS PROVIDER CLASS
        // -----------------------------------------
        public class DnsProvider{
            public string Name { get; }
            public List<string> DnsAddresses { get; }
            public int Id { get; }
            public DnsProvider(string name, List<string> dnsAddresses, int id){
                Name = name;
                DnsAddresses = dnsAddresses;
                Id = id;
            }
        }
        public static readonly List<DnsProvider> DnsProviders = new List<DnsProvider>{
            new DnsProvider("AdGuard DNS",  new List<string>{ "94.140.14.14", "94.140.15.15" }, 0),
            new DnsProvider("Cloudflare",   new List<string>{ "1.1.1.1", "1.0.0.1" }, 1),
            new DnsProvider("Comodo",       new List<string>{ "8.26.56.26", "8.20.247.20" }, 2),
            new DnsProvider("Control D",    new List<string>{ "76.76.2.0", "76.76.10.0" }, 3),
            new DnsProvider("DNS.WATCH",    new List<string>{ "84.200.69.80", "84.200.70.40" }, 4),
            new DnsProvider("Google",       new List<string>{ "8.8.8.8", "8.8.4.4" }, 5),
            new DnsProvider("Lumen DNS",    new List<string>{ "4.2.2.1", "4.2.2.2" }, 6),
            new DnsProvider("NextDNS",      new List<string>{ "45.90.28.0", "45.90.30.0" }, 7),
            new DnsProvider("OpenDNS",      new List<string>{ "208.67.222.222", "208.67.220.220" }, 8),
            new DnsProvider("Quad9",        new List<string>{ "9.9.9.9", "149.112.112.112" }, 9),
            new DnsProvider("Yandex DNS",   new List<string>{ "77.88.8.8", "77.88.8.1" }, 10),
        };
        private void Network(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_na = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter");
            var driverList = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPSignedDriver WHERE DeviceClass='Net'").Get().Cast<ManagementObject>().ToList();
            // NET CPU PERFORMANCE BOOST QUERY
            var driverDict = new Dictionary<string, ManagementObject>(StringComparer.OrdinalIgnoreCase);
            foreach (var d in driverList){
                string key = Net_replacer(Convert.ToString(d["DeviceName"]) ?? "");
                if (!string.IsNullOrEmpty(key) && !driverDict.ContainsKey(key))
                    driverDict[key] = d;
            }
            foreach (ManagementObject query_na_rotate in search_na.Get().Cast<ManagementObject>()){
                try{
                    // NET NAME
                    NET_Selector_List.Items.Add(Convert.ToString(query_na_rotate["Name"]));
                }catch (Exception){ }
                try{
                    // MAC ADRESS
                    string mac_adress = Convert.ToString(query_na_rotate["MACAddress"]);
                    if (string.IsNullOrEmpty(mac_adress)){
                        network_mac_adress_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_not_mac_adress"));
                    }else{
                        if (hiding_mode_wrapper != 1){
                            network_mac_adress_list.Add(mac_adress);
                        }else{
                            network_mac_adress_list.Add(new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})");
                        }
                    }
                    NET_MacAdress_V.Text = network_mac_adress_list[0];
                }catch (Exception){ }
                try{
                    // NET MAN
                    string net_man = Convert.ToString(query_na_rotate["Manufacturer"]);
                    if (string.IsNullOrEmpty(net_man)){
                        network_man_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_not_provider"));
                    }else{
                        network_man_list.Add(net_man);
                    }
                    NET_NetMan_V.Text = network_man_list[0];
                }catch (Exception){ }
                try{
                    string netName = Net_replacer(Convert.ToString(query_na_rotate["Name"]) ?? "");
                    string unknown = software_lang.TSReadLangs("Network_Content", "nk_c_unknown");
                    string driverVersion = unknown;
                    string driverDate = unknown;
                    if (driverDict.TryGetValue(netName, out var driverMatch)){
                        driverVersion = Convert.ToString(driverMatch["DriverVersion"]) ?? unknown;
                        string dateRaw = Convert.ToString(driverMatch["DriverDate"]);
                        if (!string.IsNullOrEmpty(dateRaw)){
                            try{
                                driverDate = ManagementDateTimeConverter.ToDateTime(dateRaw).ToString("dd.MM.yyyy");
                            }catch{ }
                        }
                    }
                    network_driver_version_list.Add(driverVersion);
                    network_driver_date_list.Add(driverDate);
                }catch{
                    string unknown = software_lang.TSReadLangs("Network_Content", "nk_c_unknown");
                    network_driver_version_list.Add(unknown);
                    network_driver_date_list.Add(unknown);
                }
                try{
                    // SERVICE NAME
                    string service_name = Convert.ToString(query_na_rotate["ServiceName"]);
                    if (string.IsNullOrEmpty(service_name)){
                        network_service_name_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_not_service_name"));
                    }else{
                        network_service_name_list.Add(service_name);
                    }
                    NET_ServiceName_V.Text = network_service_name_list[0];
                }catch (Exception){ }
                try{
                    // NET ADAPTER TYPE
                    string adaptor_type = Convert.ToString(query_na_rotate["AdapterType"]);
                    if (string.IsNullOrEmpty(adaptor_type)){
                        network_adaptor_type_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_not_adaptor_type"));
                    }else{
                        network_adaptor_type_list.Add(adaptor_type);
                    }
                    NET_AdapterType_V.Text = network_adaptor_type_list[0];
                }catch (Exception){ }
                try{
                    // NET PHYSICAL
                    bool net_physical = Convert.ToBoolean(query_na_rotate["PhysicalAdapter"]);
                    if (net_physical == true){
                        network_physical_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_yes"));
                    }else if (net_physical == false){
                        network_physical_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_no"));
                    }
                    NET_Physical_V.Text = network_physical_list[0];
                }catch (Exception) { }
                try{
                    // NETWORK DEVICE ID
                    string network_device_id = Convert.ToString(query_na_rotate["PNPDeviceID"]).Trim();
                    if (!string.IsNullOrEmpty(network_device_id)){
                        char[] split_char = { '\\' };
                        string[] network_device_split = network_device_id.Trim().Split(split_char);
                        network_device_id_list.Add($"{network_device_split[0]}\\{network_device_split[1]}");
                    }else{
                        network_device_id_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_not_hardware_id"));
                    }
                    NET_DeviceID_V.Text = network_device_id_list[0];
                }catch (Exception){ }
                try{
                    // NET GUID
                    string guid = Convert.ToString(query_na_rotate["GUID"]);
                    if (string.IsNullOrEmpty(guid)){
                        network_guid_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_not_guid"));
                    }else{
                        string net_guid_replacer = guid.Replace("{", string.Empty).Replace("}", string.Empty);
                        if (hiding_mode_wrapper != 1){
                            network_guid_list.Add(net_guid_replacer);
                        }else{
                            network_guid_list.Add(new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})");
                        }
                    }
                    NET_Guid_V.Text = network_guid_list[0];
                }catch (Exception){ }
                try{
                    // NET CONNECTION TYPE
                    string net_con_id = Convert.ToString(query_na_rotate["NetConnectionID"]);
                    if (string.IsNullOrEmpty(net_con_id)){
                        network_connection_type_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_not_connection_type"));
                    }else{
                        network_connection_type_list.Add(net_con_id);
                    }
                    NET_ConnectionType_V.Text = network_connection_type_list[0];
                }catch (Exception){ }
                // NETWORK ADAPTER CONFIG SECTION
                try{
                    var get_na_index = query_na_rotate["Index"];
                    ManagementObjectSearcher search_nac = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index={get_na_index}");
                    foreach (ManagementObject query_nac_rotate in search_nac.Get().Cast<ManagementObject>()){
                        try{
                            // DHCP STATUS
                            bool dhcp_enabled = Convert.ToBoolean(query_nac_rotate["DHCPEnabled"]);
                            if (dhcp_enabled == true){
                                network_dhcp_status_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_active"));
                            }else if (dhcp_enabled == false){
                                network_dhcp_status_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_out_of_order"));
                            }
                            NET_Dhcp_status_V.Text = network_dhcp_status_list[0];
                        }catch (Exception){ }
                        try{
                            // DHCP SERVER STATUS
                            string dhcp_server = Convert.ToString(query_nac_rotate["DHCPServer"]);
                            if (string.IsNullOrEmpty(dhcp_server)){
                                network_dhcp_server_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_unknown"));
                            }else{
                                network_dhcp_server_list.Add(dhcp_server);
                            }
                            NET_Dhcp_server_V.Text = network_dhcp_server_list[0];
                        }catch (Exception){ }
                        try{
                            // DHCP SERVER FIRST CONNECT TIME
                            string dhcp_first_ip_time = Convert.ToString(query_nac_rotate["DHCPLeaseObtained"]);
                            if (string.IsNullOrEmpty(dhcp_first_ip_time)){
                                network_dhcp_first_ip_time_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_unknown"));
                            }else{
                                DateTime osInstallDate = ManagementDateTimeConverter.ToDateTime(dhcp_first_ip_time);
                                network_dhcp_first_ip_time_list.Add($"{osInstallDate:dd.MM.yyyy} - {osInstallDate:HH:mm:ss} ");
                            }
                            NET_DHCPFirstIpTime_V.Text = network_dhcp_first_ip_time_list[0];
                        }catch (Exception){ }
                        try{
                            // DHCP SERVER LAST CONNECT TIME
                            string dhcp_last_ip_time = Convert.ToString(query_nac_rotate["DHCPLeaseExpires"]);
                            if (string.IsNullOrEmpty(dhcp_last_ip_time)){
                                network_dhcp_last_ip_time_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_unknown"));
                            }else{
                                DateTime osInstallDate = ManagementDateTimeConverter.ToDateTime(dhcp_last_ip_time);
                                network_dhcp_last_ip_time_list.Add($"{osInstallDate:dd.MM.yyyy} - {osInstallDate:HH:mm:ss} ");
                            }
                            NET_DHCPLastIpTime_V.Text = network_dhcp_last_ip_time_list[0];
                        }catch (Exception){ }
                    }
                }catch (Exception){ }
                // NETWORK ADAPTER CONFIG SECTION
                try{
                    // MODEM CONNECT SPEED
                    string local_con_speed = Convert.ToString(query_na_rotate["Speed"]);
                    if (string.IsNullOrEmpty(local_con_speed) || local_con_speed == "Unknown"){
                        network_connection_speed_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_not_connect"));
                    }else{
                        double net_speed_cal = Convert.ToInt64(local_con_speed) / 1000000.0;
                        double net_speed_download_cal = net_speed_cal / 8;
                        network_connection_speed_list.Add(net_speed_cal.ToString() + " Mbps - (" + string.Format("{0:0.0} MB/s)", net_speed_download_cal));
                    }
                    NET_LocalConSpeed_V.Text = network_connection_speed_list[0];
                }catch (Exception){ }
                try{
                    // IPV4 & IPV6 Adress
                    var get_na_index = query_na_rotate["Index"];
                    ManagementObjectSearcher query_na_ip = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index={get_na_index}");
                    foreach (ManagementObject search_ip in query_na_ip.Get().Cast<ManagementObject>()){
                        if (search_ip["IPAddress"] == null){
                            network_ipv4_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_unknown"));
                            network_ipv6_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_unknown"));
                        }else{
                            string[] arrIPAddress = (string[])(search_ip["IPAddress"]);
                            foreach (string ipAddress in arrIPAddress){
                                if (IPAddress.TryParse(ipAddress, out IPAddress address)){
                                    if (address.AddressFamily == AddressFamily.InterNetwork){
                                        if (hiding_mode_wrapper == 1){
                                            int maskLength = vis_m_property.Next(vn_range[0], vn_range[1]);
                                            network_ipv4_list.Add(new string('*', maskLength) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})");
                                        }else{
                                            network_ipv4_list.Add(ipAddress);
                                        }
                                    }else if (address.AddressFamily == AddressFamily.InterNetworkV6){
                                        if (hiding_mode_wrapper == 1){
                                            int maskLength = vis_m_property.Next(vn_range[0], vn_range[1]);
                                            network_ipv6_list.Add(new string('*', maskLength) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})");
                                        }else{
                                            network_ipv6_list.Add(ipAddress);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }catch (Exception){ }
            }
            // DNS 1 And DNS 2 Address
            try{
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True");
                ManagementObjectCollection results = searcher.Get();
                string dns1 = "", dns2 = "";
                foreach (ManagementObject obj in results.Cast<ManagementObject>()){
                    if (obj["DNSServerSearchOrder"] is string[] dnsAddresses && dnsAddresses.Length > 0){
                        dns1 = dnsAddresses[0];
                        if (dnsAddresses.Length > 1)
                            dns2 = dnsAddresses[1];
                        break;
                    }
                }
                // DNS1
                if (!string.IsNullOrEmpty(dns1)){
                    if (hiding_mode_wrapper == 1){
                        int maskLength = vis_m_property.Next(vn_range[0], vn_range[1]);
                        NET_DNS1_V.Text = new string('*', maskLength) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                    }else{
                        var provider = DnsProviders.FirstOrDefault(p => p.DnsAddresses.Contains(dns1));
                        NET_DNS1_V.Text = provider != null ? $"{dns1} ({provider.Name})" : dns1;
                    }
                }else{
                    NET_DNS1_V.Text = software_lang.TSReadLangs("Network_Content", "nk_c_dns_not");
                }
                // DNS2
                if (!string.IsNullOrEmpty(dns2)){
                    if (hiding_mode_wrapper == 1){
                        int maskLength = vis_m_property.Next(vn_range[0], vn_range[1]);
                        NET_DNS2_V.Text = new string('*', maskLength) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                    }else{
                        var provider = DnsProviders.FirstOrDefault(p => p.DnsAddresses.Contains(dns2));
                        NET_DNS2_V.Text = provider != null ? $"{dns2} ({provider.Name})" : dns2;
                    }
                }else{
                    NET_DNS2_V.Text = software_lang.TSReadLangs("Network_Content", "nk_c_dns_not");
                }
            }catch (Exception){ }
            // NETWORK SELECT
            try{
                int net_moduler = 0;
                var targetBrands = new[] { "realtek", "qualcomm", "killer", "mediatek", "intel" };
                for (int i = 0; i < NET_Selector_List.Items.Count; i++){
                    string net_list_item = NET_Selector_List.Items[i].ToString().ToLower();
                    if (targetBrands.Any(brand => net_list_item.Contains(brand))){
                        NET_Selector_List.SelectedIndex = net_moduler;
                        break;
                    }else{
                        NET_Selector_List.SelectedIndex = 1;
                        net_moduler++;
                    }
                }
            }catch (Exception){ }
            // NETWORK PROCESS END ENABLED
            NET_RotateBtn.Enabled = true;
            ((Control)NETWORK).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Network Section Loaded --->"); }
        }
        private void NET_Selector_List_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int network_select = NET_Selector_List.SelectedIndex;
                try { NET_MacAdress_V.Text = network_mac_adress_list[network_select]; } catch (Exception) { }
                try { NET_NetMan_V.Text = network_man_list[network_select]; } catch (Exception) { }
                try { NET_DriverVersion_V.Text = network_driver_version_list[network_select]; } catch (Exception) { }
                try { NET_DriverDate_V.Text = network_driver_date_list[network_select]; } catch (Exception) { }
                try { NET_ServiceName_V.Text = network_service_name_list[network_select]; } catch (Exception) { }
                try { NET_AdapterType_V.Text = network_adaptor_type_list[network_select]; } catch (Exception) { }
                try { NET_Physical_V.Text = network_physical_list[network_select]; } catch (Exception) { }
                try { NET_DeviceID_V.Text = network_device_id_list[network_select]; } catch (Exception) { }
                try { NET_Guid_V.Text = network_guid_list[network_select]; } catch (Exception) { }
                try { NET_ConnectionType_V.Text = network_connection_type_list[network_select]; } catch (Exception) { }
                try { NET_Dhcp_status_V.Text = network_dhcp_status_list[network_select]; } catch (Exception) { }
                try { NET_Dhcp_server_V.Text = network_dhcp_server_list[network_select]; } catch (Exception) { }
                try { NET_DHCPFirstIpTime_V.Text = network_dhcp_first_ip_time_list[network_select]; } catch (Exception) { }
                try { NET_DHCPLastIpTime_V.Text = network_dhcp_last_ip_time_list[network_select]; } catch (Exception) { }
                try { NET_LocalConSpeed_V.Text = network_connection_speed_list[network_select]; } catch (Exception) { }
                try { NET_IPv4Adress_V.Text = network_ipv4_list[network_select]; } catch (Exception) { }
                try { NET_IPv6Adress_V.Text = network_ipv6_list[network_select]; } catch (Exception) { }
            }catch (Exception){ }
        }
        private async void NetBgProcess(){
            try{
                var software_lang = new TSGetLangs(lang_path);
                string activeAdapter = GetActiveNetworkAdapter();
                string not_connect = software_lang.TSReadLangs("Network_Content", "nk_c_not_connect");
                if (string.IsNullOrEmpty(activeAdapter)){
                    SetUI_NotConnected(not_connect);
                    return;
                }
                var category = new PerformanceCounterCategory("Network Interface");
                string[] instanceNames = category.GetInstanceNames();
                string perfCounterAdapterName = instanceNames.FirstOrDefault(name => Net_replacer(name) == Net_replacer(activeAdapter));
                if (perfCounterAdapterName == null){
                    SetUI_NotConnected(not_connect);
                    return;
                }
                using (var bytesSentCounter = new PerformanceCounter("Network Interface", "Bytes Sent/sec", perfCounterAdapterName))
                using (var bytesReceivedCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", perfCounterAdapterName))
                using (var bandwidthCounter = new PerformanceCounter("Network Interface", "Current Bandwidth", perfCounterAdapterName)){
                    double bandwidth = bandwidthCounter.NextValue() / 1_000_000.0;
                    if (IsHandleCreated){
                        BeginInvoke(new Action(() =>{
                            NET_LT_Device_V.Text = activeAdapter;
                            NET_LT_BandWidth_V.Text = $"{(int)Math.Round(bandwidth)} Mbps";
                        }));
                    }
                    while (loop_status){
                        double mbpsSent = Math.Round(bytesSentCounter.NextValue() * 8 / 1_000_000.0, 2);
                        double mbpsReceived = Math.Round(bytesReceivedCounter.NextValue() * 8 / 1_000_000.0, 2);
                        if (IsHandleCreated){
                            BeginInvoke(new Action(() =>{
                                NET_LT_UL2.Text = $"{mbpsSent:0.00} Mbps / ({mbpsSent / 8:0.00} MB/s)";
                                NET_LT_DL2.Text = $"{mbpsReceived:0.00} Mbps / ({mbpsReceived / 8:0.00} MB/s)";
                            }));
                        }
                        try{
                            await Task.Delay(1000, Program.TS_TokenEngine.Token);
                        }catch (TaskCanceledException){
                            break;
                        }
                    }
                }
            }catch { }
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
        private void SetUI_NotConnected(string not_connect){
            if (!IsHandleCreated) return;
            BeginInvoke(new Action(() => {
                NET_LT_Device_V.Text = not_connect;
                NET_LT_BandWidth_V.Text = not_connect;
                NET_LT_UL2.Text = not_connect;
                NET_LT_DL2.Text = not_connect;
            }));
        }
        // GET PC IP & GETWAY IP
        private void NetBGProcessGateway(){
            try{
                var getLocalNetInfo = GetLocalNetworkInfo();
                if (this.InvokeRequired){
                    this.Invoke(new Action(() => {
                        UpdateNetworkUi(getLocalNetInfo);
                    }));
                }else{
                    UpdateNetworkUi(getLocalNetInfo);
                }
            }catch (Exception) { }
        }
        private void UpdateNetworkUi(GetLocalNetInfo getLocalNetInfo){
            if (getLocalNetInfo != null){
                NET_LT_LocalIP_V.Text = getLocalNetInfo.LocalIp.ToString();
                NET_LT_GatewayIP_V.Text = getLocalNetInfo.GatewayIp.ToString();
                NET_RotateGateway.Visible = true;
            }else{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                string not_connect = software_lang.TSReadLangs("Network_Content", "nk_c_not_connect");
                NET_LT_LocalIP_V.Text = not_connect;
                NET_LT_GatewayIP_V.Text = not_connect;
            }
        }
        public class GetLocalNetInfo{
            public IPAddress LocalIp { get; set; }
            public IPAddress GatewayIp { get; set; }
        }
        public static GetLocalNetInfo GetLocalNetworkInfo(){
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces()){
                if (ni.OperationalStatus != OperationalStatus.Up){
                    continue;
                }
                if (ni.NetworkInterfaceType != NetworkInterfaceType.Ethernet && ni.NetworkInterfaceType != NetworkInterfaceType.Wireless80211){
                    continue;
                }
                var ipProps = ni.GetIPProperties();
                var localIp = ipProps.UnicastAddresses.FirstOrDefault(a => a.Address.AddressFamily == AddressFamily.InterNetwork)?.Address;
                if (localIp == null){
                    continue;
                }
                var gateway = ipProps.GatewayAddresses.FirstOrDefault(g => g.Address.AddressFamily == AddressFamily.InterNetwork)?.Address;
                if (gateway == null){
                    continue;
                }
                return new GetLocalNetInfo{
                    LocalIp = localIp,
                    GatewayIp = gateway
                };
            }
            return null;
        }
        private void NET_RotateGateway_Click(object sender, EventArgs e){
            try{
                var gateway_url = $"http://{NET_LT_GatewayIP_V.Text.Trim()}";
                Process.Start(new ProcessStartInfo{
                    FileName = gateway_url,
                    UseShellExecute = true
                }); ;
            }catch (Exception){ }
        }
        #endregion
        #region USB_Section
        // USB
        // ======================================================================================================
        readonly List<string> usb_controller_name_list = new List<string>();
        readonly List<string> usb_controller_manufacturer_list = new List<string>();
        readonly List<string> usb_controller_device_id_list = new List<string>();
        readonly List<string> usb_controller_pnp_device_id_list = new List<string>();
        readonly List<string> usb_controller_device_status_list = new List<string>();
        // ----------------------------------------------------------------------
        readonly List<string> usb_device_name_list = new List<string>();
        readonly List<string> usb_device_man_list = new List<string>();
        readonly List<string> usb_device_driver_version_list = new List<string>();
        readonly List<string> usb_device_driver_date_list = new List<string>();
        readonly List<string> usb_device_inf_file_list = new List<string>();
        readonly List<string> usb_device_id_list = new List<string>();
        readonly List<string> usb_device_hardware_id_list = new List<string>();
        readonly List<string> usb_device_guid_list = new List<string>();
        private void Usb(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_usb_controller = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_USBController");
            ManagementObjectSearcher search_usb_device = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPSignedDriver WHERE DeviceID LIKE 'USB%'");
            foreach (ManagementObject query_usb_con in search_usb_controller.Get().Cast<ManagementObject>()){
                // USB CON CAPTION
                try{
                    string usb_con_caption = Convert.ToString(query_usb_con["Caption"]).Trim();
                    if (!string.IsNullOrEmpty(usb_con_caption)){
                        USB_Selector_List.Items.Add(usb_con_caption);
                    }else{
                        USB_Selector_List.Items.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown_device"));
                    }
                }catch (Exception){ }
                // USB CON NAME
                try{
                    string usb_con_name = Convert.ToString(query_usb_con["Name"]).Trim();
                    if (!string.IsNullOrEmpty(usb_con_name)){
                        usb_controller_name_list.Add(usb_con_name);
                    }else{
                        usb_controller_name_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_ConName_V.Text = usb_controller_name_list[0];
                }catch (Exception){ }
                // USB CON MANUFACTURER
                try{
                    string usb_con_man = Convert.ToString(query_usb_con["Manufacturer"]).Trim();
                    if (!string.IsNullOrEmpty(usb_con_man)){
                        usb_controller_manufacturer_list.Add(usb_con_man);
                    }else{
                        usb_controller_manufacturer_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_ConMan_V.Text = usb_controller_manufacturer_list[0];
                }catch (Exception){ }
                // USB CON DEVICE ID
                try{
                    string usb_con_device_id = Convert.ToString(query_usb_con["DeviceID"]).Trim();
                    if (!string.IsNullOrEmpty(usb_con_device_id)){
                        char[] split_char = { '\\' };
                        string[] usb_con_device_split = usb_con_device_id.Trim().Split(split_char);
                        usb_controller_device_id_list.Add($"{usb_con_device_split[0]}\\{usb_con_device_split[1]}");
                    }else{
                        usb_controller_device_id_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_ConDeviceID_V.Text = usb_controller_device_id_list[0];
                }catch (Exception){ }
                // USB CON PNP DEVICE ID
                try{
                    string usb_con_pnp_device_id = Convert.ToString(query_usb_con["PNPDeviceID"]).Trim();
                    if (!string.IsNullOrEmpty(usb_con_pnp_device_id)){
                        char[] split_char = { '\\' };
                        string[] usb_con_pnp_device_split = usb_con_pnp_device_id.Trim().Split(split_char);
                        usb_controller_pnp_device_id_list.Add($"{usb_con_pnp_device_split[0]}\\{usb_con_pnp_device_split[1]}");
                    }else{
                        usb_controller_pnp_device_id_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_ConPNPDeviceID_V.Text = usb_controller_pnp_device_id_list[0];
                }catch (Exception){ }
                // USB CON DEVICE STATUS
                try{
                    string usb_con_device_status = Convert.ToString(query_usb_con["Status"]).Trim().ToLower();
                    var usbConDeviceStatuses = new Dictionary<string, string>{
                        { "ok", "usb_c_works_smoothly" },
                        { "error", "usb_c_error" },
                        { "degraded", "usb_c_degraded" },
                        { "unknown", "usb_c_unknown" },
                        { "pred fail", "usb_c_prevention_failed" },
                        { "starting", "usb_c_starting" },
                        { "stopping", "usb_c_stopped" },
                        { "service", "usb_c_service_mode" },
                        { "stressed", "usb_c_stressed" },
                        { "nonrecover", "usb_c_unrecoverable" },
                        { "no contact", "usb_c_no_communication" },
                        { "lost comm", "usb_c_communication_loss" }
                    };
                    if (!string.IsNullOrEmpty(usb_con_device_status)){
                        if (usbConDeviceStatuses.TryGetValue(usb_con_device_status, out string statusKey)){
                            usb_controller_device_status_list.Add(software_lang.TSReadLangs("Usb_Content", statusKey));
                        }else{
                            usb_controller_device_status_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                        }
                    }else{
                        usb_controller_device_status_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_ConDeviceStatus_V.Text = usb_controller_device_status_list[0];
                }catch (Exception){ }
            }
            foreach (ManagementObject usb_device in search_usb_device.Get().Cast<ManagementObject>()){
                // USB FRIENDLY NAME
                try{
                    string usb_caption = Convert.ToString(usb_device["FriendlyName"]).Trim();
                    if (!string.IsNullOrEmpty(usb_caption)){
                        USB_DeviceSelector_List.Items.Add(usb_caption);
                    }else{
                        string usb_caption_2 = Convert.ToString(usb_device["DeviceName"]).Trim();
                        if (!string.IsNullOrEmpty(usb_caption_2)){
                            USB_DeviceSelector_List.Items.Add(usb_caption_2);
                        }else{
                            USB_DeviceSelector_List.Items.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown_device"));
                        }
                    }
                }catch (Exception){ }
                // USB DEVICE NAME
                try{
                    string usb_device_name = Convert.ToString(usb_device["DeviceName"]).Trim();
                    if (!string.IsNullOrEmpty(usb_device_name)){
                        usb_device_name_list.Add(usb_device_name);
                    }else{
                        usb_device_name_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_DeviceName_V.Text = usb_device_name_list[0];
                }catch (Exception){ }
                // USB MANUFACTURER
                try{
                    string usb_manufacturer= Convert.ToString(usb_device["Manufacturer"]).Trim();
                    if (!string.IsNullOrEmpty(usb_manufacturer)){
                        usb_device_man_list.Add(usb_manufacturer);
                    }else{
                        usb_device_man_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_DeviceMan_V.Text = usb_device_man_list[0];
                }catch (Exception){ }
                // USB DRIVER VERSION
                try{
                    string usb_driver_version = Convert.ToString(usb_device["DriverVersion"]).Trim();
                    if (!string.IsNullOrEmpty(usb_driver_version)){
                        usb_device_driver_version_list.Add(usb_driver_version);
                    }else{
                        usb_device_driver_version_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_DriverVersion_V.Text = usb_device_driver_version_list[0];
                }catch (Exception){ }
                // USB DRIVER DATE
                try{
                    string usb_driver_date = usb_device["DriverDate"]?.ToString().Trim();
                    if (!string.IsNullOrEmpty(usb_driver_date)){
                        try{
                            DateTime driverDate = ManagementDateTimeConverter.ToDateTime(usb_driver_date);
                            usb_device_driver_date_list.Add(driverDate.ToString("dd.MM.yyyy"));
                        }catch{
                            usb_device_driver_date_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                        }
                    }else{
                        usb_device_driver_date_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_DriverDate_V.Text = usb_device_driver_date_list[0];
                }catch (Exception){ }
                // USB INF FILE NAME
                try{
                    string usb_inf_name = Convert.ToString(usb_device["InfName"]).Trim();
                    if (!string.IsNullOrEmpty(usb_inf_name)){
                        usb_device_inf_file_list.Add(usb_inf_name);
                    }else{
                        usb_device_inf_file_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_InfFile_V.Text = usb_device_inf_file_list[0];
                }catch (Exception){ }
                // USB DEVICE ID
                try{
                    string usb_device_id = Convert.ToString(usb_device["DeviceID"]).Trim();
                    if (!string.IsNullOrEmpty(usb_device_id)){
                        char[] split_char = { '\\' };
                        string[] usb_con_device_split = usb_device_id.Trim().Split(split_char);
                        usb_device_id_list.Add($"{usb_con_device_split[0]}\\{usb_con_device_split[1]}");
                    }else{
                        usb_device_id_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_DeviceID_V.Text = usb_device_id_list[0];
                }catch (Exception){ }
                // USB HARDWARE ID
                try{
                    string usb_hardware_id = Convert.ToString(usb_device["HardWareID"]).Trim();
                    if (!string.IsNullOrEmpty(usb_hardware_id)){
                        char[] split_char = { '\\' };
                        string[] usb_con_device_split = usb_hardware_id.Trim().Split(split_char);
                        usb_device_hardware_id_list.Add($"{usb_con_device_split[0]}\\{usb_con_device_split[1]}");
                    }else{
                        usb_device_hardware_id_list.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown"));
                    }
                    USB_HardwareID_V.Text = usb_device_hardware_id_list[0];
                }catch (Exception){ }
                // USB DEVICE GUID
                try{
                    string usb_device_guid = Convert.ToString(usb_device["ClassGuid"]).Trim();
                    if (string.IsNullOrEmpty(usb_device_guid)){
                        usb_device_guid_list.Add(software_lang.TSReadLangs("Network_Content", "nk_c_not_guid"));
                    }else{
                        string usb_guid_replacer = usb_device_guid.Replace("{", string.Empty).Replace("}", string.Empty);
                        if (hiding_mode_wrapper != 1){
                            usb_device_guid_list.Add(usb_guid_replacer.ToUpper());
                        }else{
                            usb_device_guid_list.Add(new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})");
                        }
                    }
                    USB_DeviceGUID_V.Text = usb_device_guid_list[0];
                }catch (Exception){ }
            }
            try{ USB_Selector_List.SelectedIndex = 0; }catch (Exception){ }
            try{ USB_DeviceSelector_List.SelectedIndex = 0; }catch (Exception){ }
            // USB PROCESS END ENABLED
            USB_RotateBtn.Enabled = true;
            ((Control)USB).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- USB Section Installed --->"); }
        }
        private void USB_Selector_List_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int usb_con_select = USB_Selector_List.SelectedIndex;
                try { USB_ConName_V.Text = usb_controller_name_list[usb_con_select]; } catch (Exception) { }
                try { USB_ConMan_V.Text = usb_controller_manufacturer_list[usb_con_select]; } catch (Exception) { }
                try { USB_ConDeviceID_V.Text = usb_controller_device_id_list[usb_con_select]; } catch (Exception) { }
                try { USB_ConPNPDeviceID_V.Text = usb_controller_pnp_device_id_list[usb_con_select]; } catch (Exception) { }
                try { USB_ConDeviceStatus_V.Text = usb_controller_device_status_list[usb_con_select]; } catch (Exception) { }
            }catch (Exception){ }
        }
        private void USB_DeviceSelector_List_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int usb_select = USB_DeviceSelector_List.SelectedIndex;
                try { USB_DeviceName_V.Text = usb_device_name_list[usb_select]; } catch (Exception) { }
                try { USB_DeviceMan_V.Text = usb_device_man_list[usb_select]; } catch (Exception) { }
                try { USB_DriverVersion_V.Text = usb_device_driver_version_list[usb_select]; } catch (Exception) { }
                try { USB_DriverDate_V.Text = usb_device_driver_date_list[usb_select]; } catch (Exception) { }
                try { USB_InfFile_V.Text = usb_device_inf_file_list[usb_select]; } catch (Exception) { }
                try { USB_DeviceID_V.Text = usb_device_id_list[usb_select]; } catch (Exception) { }
                try { USB_HardwareID_V.Text = usb_device_hardware_id_list[usb_select]; } catch (Exception) { }
                try { USB_DeviceGUID_V.Text = usb_device_guid_list[usb_select]; } catch (Exception) { }
            }catch (Exception){ }
        }
        #endregion
        #region SOUND_Section
        // SOUND
        // ======================================================================================================
        readonly List<string> sound_device_name_list = new List<string>();
        readonly List<string> sound_device_manufacturer_list = new List<string>();
        readonly List<string> sound_device_driver_version_list = new List<string>();
        readonly List<string> sound_device_driver_date_list = new List<string>();
        readonly List<string> sound_device_id_list = new List<string>();
        readonly List<string> sound_pnp_device_id_list = new List<string>();
        readonly List<string> sound_device_status_list = new List<string>();
        private void Sound(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_sound = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SoundDevice");
            var soundDrivers = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPSignedDriver WHERE DeviceClass='MEDIA'").Get().Cast<ManagementObject>().ToList();
            // CPU PERFORMANCE BOOSTER SOUND QUERY
            var soundDriversDict = new Dictionary<string, ManagementObject>(StringComparer.OrdinalIgnoreCase);
            foreach (ManagementObject d in soundDrivers){
                var id = Convert.ToString(d["DeviceID"])?.Trim();
                if (!string.IsNullOrEmpty(id) && !soundDriversDict.ContainsKey(id))
                    soundDriversDict[id] = d;
            }
            foreach (ManagementObject query_sound in search_sound.Get().Cast<ManagementObject>()){
                // SOUND DEVICE CAPTION
                try{
                    string sound_caption = Convert.ToString(query_sound["Caption"]);
                    if (!string.IsNullOrEmpty(sound_caption)){
                        SOUND_Selector_List.Items.Add(sound_caption);
                    }else{
                        SOUND_Selector_List.Items.Add(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown_device"));
                    }
                }catch (Exception){ }
                // SOUND DEVICE NAME
                try{
                    string sound_name = Convert.ToString(query_sound["Name"]);
                    if (!string.IsNullOrEmpty(sound_name)){
                        sound_device_name_list.Add(sound_name);
                    }else{
                        sound_device_name_list.Add(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown"));
                    }
                    SOUND_DeviceName_V.Text = sound_device_name_list[0];
                }catch (Exception){ }
                // SOUND DEVICE MANUFACTURER
                try{
                    string sound_manfuacturer = Convert.ToString(query_sound["Manufacturer"]);
                    if (!string.IsNullOrEmpty(sound_manfuacturer)){
                        sound_device_manufacturer_list.Add(sound_manfuacturer);
                    }else{
                        sound_device_manufacturer_list.Add(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown"));
                    }
                    SOUND_DeviceManufacturer_V.Text = sound_device_manufacturer_list[0];
                }catch (Exception){ }
                // SOUND DRIVER VERSION & DATE
                try{
                    string unknown = software_lang.TSReadLangs("Sound_Content", "sound_c_unknown");
                    string sound_name = Convert.ToString(query_sound["Name"]) ?? unknown;
                    string sound_pnp_device_id = Convert.ToString(query_sound["PNPDeviceID"])?.Trim() ?? "";
                    string driverVersion = unknown;
                    string driverDate = unknown;
                    if (!string.IsNullOrEmpty(sound_pnp_device_id) && soundDriversDict.TryGetValue(sound_pnp_device_id, out var driverMatch)){
                        driverVersion = Convert.ToString(driverMatch["DriverVersion"]) ?? unknown;
                        if (driverMatch["DriverDate"] != null){
                            try{
                                DateTime dt = ManagementDateTimeConverter.ToDateTime(driverMatch["DriverDate"].ToString());
                                driverDate = dt.ToString("dd.MM.yyyy");
                            }catch{
                                driverDate = unknown;
                            }
                        }
                    }
                    sound_device_driver_version_list.Add(driverVersion);
                    sound_device_driver_date_list.Add(driverDate);
                }catch (Exception){
                    string unknown = software_lang.TSReadLangs("Sound_Content", "sound_c_unknown");
                    sound_device_driver_version_list.Add(unknown);
                    sound_device_driver_date_list.Add(unknown);
                }
                // SOUND DEVICE ID
                try{
                    string sound_device_id = Convert.ToString(query_sound["DeviceID"]);
                    if (!string.IsNullOrEmpty(sound_device_id)){
                        char[] split_char = { '\\' };
                        string[] sound_device_split = sound_device_id.Trim().Split(split_char);
                        sound_device_id_list.Add($"{sound_device_split[0]}\\{sound_device_split[1]}");
                    }else{
                        sound_device_id_list.Add(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown"));
                    }
                    SOUND_DeviceID_V.Text = sound_device_id_list[0];
                }catch (Exception){ }
                // SOUND PNP DEVICE ID
                try{
                    string sound_pnp_device_id = Convert.ToString(query_sound["PNPDeviceID"]);
                    if (!string.IsNullOrEmpty(sound_pnp_device_id)){
                        char[] split_char = { '\\' };
                        string[] sound_pnp_device_split = sound_pnp_device_id.Trim().Split(split_char);
                        sound_pnp_device_id_list.Add($"{sound_pnp_device_split[0]}\\{sound_pnp_device_split[1]}");
                    }else{
                        sound_pnp_device_id_list.Add(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown"));
                    }
                    SOUND_PNPDeviceID_V.Text = sound_pnp_device_id_list[0];
                }catch (Exception){ }
                // SOUND DEVICE STATUS
                try{
                    string sound_device_status = Convert.ToString(query_sound["Status"]).Trim().ToLower();
                    if (!string.IsNullOrEmpty(sound_device_status)){
                        var soundDeviceStatuses = new Dictionary<string, string>{
                            { "ok", "sound_c_works_smoothly" },
                            { "error", "sound_c_error" },
                            { "degraded", "sound_c_degraded" },
                            { "unknown", "sound_c_unknown" },
                            { "pred fail", "sound_c_prevention_failed" },
                            { "starting", "sound_c_starting" },
                            { "stopping", "sound_c_stopped" },
                            { "service", "sound_c_service_mode" },
                            { "stressed", "sound_c_stressed" },
                            { "nonrecover", "sound_c_unrecoverable" },
                            { "no contact", "sound_c_no_communication" },
                            { "lost comm", "sound_c_communication_loss" }
                        };
                        if (soundDeviceStatuses.TryGetValue(sound_device_status, out string statusKey)){
                            sound_device_status_list.Add(software_lang.TSReadLangs("Sound_Content", statusKey));
                        }else{
                            sound_device_status_list.Add(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown"));
                        }
                    }else{
                        sound_device_status_list.Add(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown"));
                    }
                    SOUND_DeviceStatus_V.Text = sound_device_status_list[0];
                }catch (Exception){ }
            }
            try { SOUND_Selector_List.SelectedIndex = 0; }catch (Exception){ }
            // SOUND PROCESS END ENABLED
            SOUND_RotateBtn.Enabled = true;
            ((Control)SOUND).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Sound Section Loaded --->"); }
        }
        private void SOUND_Selector_List_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int sound_select = SOUND_Selector_List.SelectedIndex;
                try { SOUND_DeviceName_V.Text = sound_device_name_list[sound_select]; } catch (Exception) { }
                try { SOUND_DeviceManufacturer_V.Text = sound_device_manufacturer_list[sound_select]; } catch (Exception) { }
                try { SOUND_DriverVersion_V.Text = sound_device_driver_version_list[sound_select]; } catch (Exception) { }
                try { SOUND_DriverDate_V.Text = sound_device_driver_date_list[sound_select]; } catch (Exception) { }
                try { SOUND_DeviceID_V.Text = sound_device_id_list[sound_select]; } catch (Exception) { }
                try { SOUND_PNPDeviceID_V.Text = sound_pnp_device_id_list[sound_select]; } catch (Exception) { }
                try { SOUND_DeviceStatus_V.Text = sound_device_status_list[sound_select]; } catch (Exception) { }
            }catch (Exception){ }
        }
        #endregion
        #region BATTERY_Section
        // BATTERY
        // ======================================================================================================
        private void Battery(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                ManagementObjectSearcher get_battery_static_data = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM BatteryStaticData");
                foreach (ManagementObject batteryStatic in get_battery_static_data.Get().Cast<ManagementObject>()){
                    try{
                        // BATTERY NAME
                        string battery_name = batteryStatic["DeviceName"].ToString().Trim();
                        if (!string.IsNullOrEmpty(battery_name)){
                            BATTERY_Model_V.Text = battery_name;
                        }else{
                            BATTERY_Model_V.Text = software_lang.TSReadLangs("Battery_Content", "by_c_unknown");
                        }
                    }catch (Exception){ }
                    try{
                        // BATTERY SERIAL
                        string battery_serial = batteryStatic["SerialNumber"].ToString().Trim();
                        if (!string.IsNullOrEmpty(battery_serial)){
                            if (hiding_mode_wrapper != 1){
                                BATTERY_Serial_V.Text = battery_serial;
                            }else{
                                BATTERY_Serial_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui")})";
                            }
                        }else{
                            BATTERY_Serial_V.Text = software_lang.TSReadLangs("Battery_Content", "by_c_unknown");
                        }
                    }catch (Exception){ }
                    try{
                        // BATTERY CHEMISTRY
                        byte[] chemistryBytes = BitConverter.GetBytes(Convert.ToUInt32(batteryStatic["Chemistry"]));
                        string chemistry = Encoding.ASCII.GetString(chemistryBytes).Trim('\0').Trim();
                        if (!string.IsNullOrEmpty(chemistry)){
                            BATTERY_Chemistry_V.Text = chemistry;
                        }else{
                            ManagementObjectSearcher battery_sv2_chemistry = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Battery");
                            foreach (ManagementObject b_sv2_chemistry in battery_sv2_chemistry.Get().Cast<ManagementObject>()){
                                int battery_structure = Convert.ToInt32(b_sv2_chemistry["Chemistry"]);
                                var batteryTypes = new Dictionary<int, string>{
                                    { 1, "by_c_other" },
                                    { 2, "by_c_unknown" },
                                    { 3, "by_c_lead_acid" },
                                    { 4, "by_c_nickel_cadmium" },
                                    { 5, "by_c_nickel_metal_hydride" },
                                    { 6, "by_c_lithium_ion" },
                                    { 7, "by_c_zinc_air" },
                                    { 8, "by_c_lithium_polymer" }
                                };
                                if (batteryTypes.TryGetValue(battery_structure, out string batteryKey)){
                                    BATTERY_Chemistry_V.Text = software_lang.TSReadLangs("Battery_Content", batteryKey);
                                }else{
                                    BATTERY_Chemistry_V.Text = software_lang.TSReadLangs("Battery_Content", "by_c_unknown");
                                }
                            }
                        }
                    }catch (Exception){ }
                    try{
                        // BATTERY DESIGN CAPACITY
                        decimal designedCapacityRaw = Convert.ToDecimal(batteryStatic["DesignedCapacity"]);
                        decimal designedCapacity_mWh = designedCapacityRaw / 1000m;
                        if (!string.IsNullOrEmpty(Convert.ToString(designedCapacity_mWh))){
                            BATTERY_DesignCapacity_V.Text = $"{designedCapacity_mWh:N3} mWh";
                        }else{
                            BATTERY_DesignCapacity_V.Text = software_lang.TSReadLangs("Battery_Content", "by_c_unknown");
                        }
                    }catch (Exception){ }
                }
                try{
                    // BATTERY FULL CHARAGE CAPACITY
                    ManagementObjectSearcher get_battery_full_charge_capacity = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM BatteryFullChargedCapacity");
                    foreach (ManagementObject batteryFullCharge in get_battery_full_charge_capacity.Get().Cast<ManagementObject>()){
                        long fullChargedCapacityRaw = Convert.ToInt64(batteryFullCharge["FullChargedCapacity"]);
                        battery_fullChargedCapacity_mWh = fullChargedCapacityRaw / 1000m;
                        if (!string.IsNullOrEmpty(Convert.ToString(battery_fullChargedCapacity_mWh))){
                            BATTERY_FullChargeCapacity_V.Text = $"{battery_fullChargedCapacity_mWh:N3} mWh";
                        }else{
                            BATTERY_FullChargeCapacity_V.Text = software_lang.TSReadLangs("Battery_Content", "by_c_unknown");
                        }
                    }
                }catch (Exception){ }
            }catch (Exception){ }
            // BATTERY PROCESS END ENABLED
            BATTERY_RotateBtn.Enabled = true;
            ((Control)BATTERY).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Battery Section Loaded --->"); }
        }
        private void Battery_visible_off(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            BATTERY_Status_V.Text = software_lang.TSReadLangs("Battery_Content", "by_c_battery_not_detect");
            battery_panel_1.Height = (int)(47 * this.DeviceDpi / 96f);
            battery_panel_2.Visible = false;
            //
            BATTERY_Model.Visible = false;
            BATTERY_Model_V.Visible = false;
            BATTERY_Serial.Visible = false;
            BATTERY_Serial_V.Visible = false;
            BATTERY_Chemistry.Visible = false;
            BATTERY_Chemistry_V.Visible = false;
            BATTERY_DesignCapacity.Visible = false;
            BATTERY_DesignCapacity_V.Visible = false;
            BATTERY_FullChargeCapacity.Visible = false;
            BATTERY_FullChargeCapacity_V.Visible = false;
            BATTERY_RemainingChargeCapacity.Visible = false;
            BATTERY_RemainingChargeCapacity_V.Visible = false;
            BATTERY_Voltage.Visible = false;
            BATTERY_Voltage_V.Visible = false;
            BATTERY_ChargePower.Visible = false;
            BATTERY_ChargePower_V.Visible = false;
            BATTERY_ChargeCurrent.Visible = false;
            BATTERY_ChargeCurrent_V.Visible = false;
            BATTERY_DeChargePower.Visible = false;
            BATTERY_DeChargePower_V.Visible = false;
            BATTERY_DeChargeCurrent.Visible = false;
            BATTERY_DeChargeCurrent_V.Visible = false;
            //
            BATTERY_ReportBtn.Visible = false;
            BATTERY_PBG_Panel.Visible = false;
            BATTERY_PFE_Panel.Visible = false;
            BATTERY_ProgressLabel.Visible = false;
        }
        private void Battery_visible_on(){
            BATTERY_Model.Visible = true;
            BATTERY_Model_V.Visible = true;
            BATTERY_Serial.Visible = true;
            BATTERY_Serial_V.Visible = true;
            BATTERY_Chemistry.Visible = true;
            BATTERY_Chemistry_V.Visible = true;
            BATTERY_DesignCapacity.Visible = true;
            BATTERY_DesignCapacity_V.Visible = true;
            BATTERY_FullChargeCapacity.Visible = true;
            BATTERY_FullChargeCapacity_V.Visible = true;
            BATTERY_RemainingChargeCapacity.Visible = true;
            BATTERY_RemainingChargeCapacity_V.Visible = true;
            BATTERY_Voltage.Visible = true;
            BATTERY_Voltage_V.Visible = true;
            BATTERY_ChargePower.Visible = true;
            BATTERY_ChargePower_V.Visible = true;
            BATTERY_ChargeCurrent.Visible = true;
            BATTERY_ChargeCurrent_V.Visible = true;
            BATTERY_DeChargePower.Visible = true;
            BATTERY_DeChargePower_V.Visible = true;
            BATTERY_DeChargeCurrent.Visible = true;
            BATTERY_DeChargeCurrent_V.Visible = true;
            //
            BATTERY_ReportBtn.Visible = true;
            BATTERY_PBG_Panel.Visible = true;
            BATTERY_PFE_Panel.Visible = true;
            BATTERY_ProgressLabel.Visible = true;
        }
        private async void BatteryBgProcess(){
            try{
                var get_dynamic_battery_info = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM BatteryStatus");
                PowerStatus power = SystemInformation.PowerStatus;
                while (loop_status){
                    var batteryData = new Dictionary<string, object>();
                    try{
                        var software_lang = new TSGetLangs(lang_path);
                        float batteryPercent = power.BatteryLifePercent * 100;
                        batteryData["BatteryPercent"] = batteryPercent;
                        batteryData["BatteryStatusText"] = string.Format("{0}%", batteryPercent.ToString("F0").Trim());
                        batteryData["SoftwareLang"] = software_lang;
                        //
                        decimal remainingCapacity_mWh = 0;
                        decimal voltage_V = 0;
                        decimal chargePower_W = 0;
                        decimal dischargePower_W = 0;
                        //
                        foreach (ManagementObject batteryStatus in get_dynamic_battery_info.Get().Cast<ManagementObject>()){
                            try { remainingCapacity_mWh = Convert.ToInt64(batteryStatus["RemainingCapacity"]) / 1000m; } catch { }
                            try { voltage_V = Convert.ToInt32(batteryStatus["Voltage"]) / 1000m; } catch { }
                            try{
                                chargePower_W = Convert.ToInt32(batteryStatus["ChargeRate"]) / 1000m;
                                dischargePower_W = Convert.ToInt32(batteryStatus["DischargeRate"]) / 1000m;
                            }catch{ }
                        }
                        //
                        batteryData["RemainingCapacity"] = remainingCapacity_mWh;
                        batteryData["Voltage"] = voltage_V;
                        batteryData["ChargePower"] = chargePower_W;
                        batteryData["DischargePower"] = dischargePower_W;
                    }catch { }
                    //
                    if (IsHandleCreated){
                        BeginInvoke(new Action(() =>{
                            try{
                                var software_lang = (TSGetLangs)batteryData["SoftwareLang"];
                                var battery_status = (string)batteryData["BatteryStatusText"];
                                var percent = (float)batteryData["BatteryPercent"];
                                var remainingCapacity = (decimal)batteryData["RemainingCapacity"];
                                var voltage = (decimal)batteryData["Voltage"];
                                var chargePower = (decimal)batteryData["ChargePower"];
                                var dischargePower = (decimal)batteryData["DischargePower"];
                                //
                                BATTERY_RotateBtn.Text = $"  {software_lang.TSReadLangs("LeftMenu", "left_battery")} - {battery_status}";
                                BATTERY_PFE_Panel.Height = (int)(BATTERY_PBG_Panel.Height * (percent / 100.0));
                                BATTERY_ProgressLabel.Text = battery_status;
                                BATTERY_ProgressLabel.Top = BATTERY_PFE_Panel.Top + 6;
                                //
                                BATTERY_RemainingChargeCapacity_V.Text = $"{remainingCapacity:N3} mWh";
                                BATTERY_Voltage_V.Text = $"{voltage:N1} V";
                                //
                                BATTERY_ChargePower_V.Text = chargePower > 0 ? $"{chargePower:N1} W" : software_lang.TSReadLangs("Battery_Content", "by_c_not_charging");
                                BATTERY_ChargeCurrent_V.Text = (chargePower > 0 && voltage > 0) ? $"{(chargePower / voltage):N1} A" : software_lang.TSReadLangs("Battery_Content", "by_c_not_charging");
                                //
                                BATTERY_DeChargePower_V.Text = dischargePower > 0 ? $"{dischargePower:N1} W" : software_lang.TSReadLangs("Battery_Content", "by_c_not_discharging");
                                BATTERY_DeChargeCurrent_V.Text = (dischargePower > 0 && voltage > 0) ? $"{(dischargePower / voltage):N1} A" : software_lang.TSReadLangs("Battery_Content", "by_c_not_discharging");
                                //
                                if (battery_fullChargedCapacity_mWh <= 0){
                                    BATTERY_Status_V.Text = software_lang.TSReadLangs("Battery_Content", "by_c_not_full_charge_capacity_get");
                                }else if (chargePower >= battery_MIN_POWER_THRESHOLD){
                                    decimal remainingToFull = battery_fullChargedCapacity_mWh - remainingCapacity;
                                    if (remainingToFull > 0){
                                        var t = TimeSpan.FromHours((double)(remainingToFull / chargePower));
                                        BATTERY_Status_V.Text = string.Format(software_lang.TSReadLangs("Battery_Content", "by_c_charging"), battery_status, t.Hours, t.Minutes);
                                    }else{
                                        BATTERY_Status_V.Text = software_lang.TSReadLangs("Battery_Content", "by_c_full_capacity");
                                    }
                                }else if (dischargePower >= battery_MIN_POWER_THRESHOLD){
                                    var t = TimeSpan.FromHours((double)(remainingCapacity / dischargePower));
                                    BATTERY_Status_V.Text = string.Format(software_lang.TSReadLangs("Battery_Content", "by_c_discharging"), battery_status, t.Hours, t.Minutes);
                                }
                                else{
                                    BATTERY_Status_V.Text = percent < 100 ? string.Format(software_lang.TSReadLangs("Battery_Content", "by_c_fixed_capacity"), battery_status) : software_lang.TSReadLangs("Battery_Content", "by_c_full_capacity");
                                }
                            }catch { }
                        }));
                    }
                    //
                    try{
                        await Task.Delay(1000, Program.TS_TokenEngine.Token);
                    }catch (TaskCanceledException){
                        break;
                    }
                }
            }catch{ }
        }
        readonly string battery_report_path = Application.StartupPath + @"\battery-report.html";
        private async void BATTERY_ReportBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                if (File.Exists(battery_report_path))
                    File.Delete(battery_report_path);
                Process.Start(new ProcessStartInfo("powercfg", "/batteryreport") { CreateNoWindow = true, UseShellExecute = false });
                await StartBatteryReportProcessAsync();
            }catch (Exception){ }
        }
        private async Task StartBatteryReportProcessAsync(){
            try{
                await Task.Run(async () => await Battery_report_check_process_async(), Program.TS_TokenEngine.Token);
            }catch (Exception){ }
        }
        private async Task Battery_report_check_process_async(){
            try{
                while (!File.Exists(battery_report_path)){
                    await Task.Delay(500, Program.TS_TokenEngine.Token);
                }
                bool fileReady = false;
                while (!fileReady){
                    try{
                        using (var stream = new FileStream(battery_report_path, FileMode.Open, FileAccess.Read, FileShare.None)) { }
                        fileReady = true;
                    }catch { await Task.Delay(200); }
                }
                string new_battery_report_path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{Application.ProductName}_{Dns.GetHostName()}_battery_report_{DateTime.Now:dd.MM.yyyy_HH.mm.ss}.html");
                File.Move(battery_report_path, new_battery_report_path);
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                DialogResult br_message = TS_MessageBoxEngine.TS_MessageBox(this, 5,
                    string.Format(software_lang.TSReadLangs("Battery_Content", "by_report_create_message"), new_battery_report_path, "\n\n")
                );
                if (br_message == DialogResult.Yes){
                    Process.Start(new_battery_report_path);
                }
            }catch (TaskCanceledException) { }
            catch (Exception) { }
        }
        #endregion
        #region OSD_Section
        // OSD
        // ======================================================================================================
        private Task InstalledDrivers(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            var driverTypeMappings = new Dictionary<string, string>{
                { "Kernel Driver", "osd_c_kernel_driver" },
                { "File System Driver", "osd_c_file_system_driver" },
                { "Unknown", "osd_c_unknown" }
            };
            var driverStartModeMappings = new Dictionary<string, string>{
                { "Boot", "osd_c_boot" },
                { "Manual", "osd_c_manuel" },
                { "System", "osd_c_system" },
                { "Auto", "osd_c_auto" },
                { "Disabled", "osd_c_disabled" },
                { "Unknown", "osd_c_unknown" }
            };
            var driverStatusMappings = new Dictionary<string, string>{
                { "Stopped", "osd_c_stopped" },
                { "Running", "osd_c_working" }
            };
            try{
                var search = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SystemDriver");
                var results = search.Get().Cast<ManagementObject>().ToList();
                var driverRows = new List<string[]>(results.Count);
                foreach (var d in results){
                    string path = Convert.ToString(d["PathName"])?.Replace("\"", "").Replace("\\??\\", "").Trim() ?? "";
                    string file = Path.GetFileName(path);
                    string disp = Convert.ToString(d["DisplayName"])?.Trim() ?? "";
                    string type = Convert.ToString(d["ServiceType"])?.Trim();
                    string start = Convert.ToString(d["StartMode"])?.Trim();
                    string state = Convert.ToString(d["State"])?.Trim();
                    //
                    type = driverTypeMappings.TryGetValue(type ?? "", out var t) ? software_lang.TSReadLangs("Osd_Content", t) : software_lang.TSReadLangs("Osd_Content", "osd_c_unknown");
                    start = driverStartModeMappings.TryGetValue(start ?? "", out var s) ? software_lang.TSReadLangs("Osd_Content", s) : software_lang.TSReadLangs("Osd_Content", "osd_c_unknown");
                    state = driverStatusMappings.TryGetValue(state ?? "", out var st) ? software_lang.TSReadLangs("Osd_Content", st) : software_lang.TSReadLangs("Osd_Content", "osd_c_unknown");
                    //
                    driverRows.Add(new[] { path, file, disp, type, start, state });
                }
                Action uiUpdate = () =>{
                    BulkAddRows(driverRows, software_lang);
                };
                if (OSD_DataMainTable.InvokeRequired)
                    OSD_DataMainTable.Invoke(uiUpdate);
                else
                    uiUpdate();
            }catch (ManagementException) { }
            finally{
                Action finalizeUi = () =>{
                    OSD_RotateBtn.Enabled = true;
                    ((Control)DRIVERS).Enabled = true;
                    OSD_DataMainTable.PerformLayout();
                    OSD_DataMainTable.Invalidate();
                    if (Program.debug_mode)
                        Console.WriteLine("<--- Installed Drivers Section Loaded --->");
                };
                if (OSD_DataMainTable.InvokeRequired)
                    OSD_DataMainTable.Invoke(finalizeUi);
                else
                    finalizeUi();
            }
            return Task.CompletedTask;
        }
        private void BulkAddRows(List<string[]> driverRows, TSGetLangs software_lang){
            OSD_DataMainTable.SuspendLayout();
            OSD_DataMainTable.Rows.Clear();
            foreach (var row in driverRows)
                OSD_DataMainTable.Rows.Add(row);
            string unknownValue = software_lang.TSReadLangs("Osd_Content", "osd_c_unknown");
            foreach (DataGridViewRow r in OSD_DataMainTable.Rows)
                foreach (DataGridViewCell c in r.Cells)
                    if (string.IsNullOrWhiteSpace(c.Value?.ToString()))
                        c.Value = unknownValue;
            OSD_DataMainTable.Sort(OSD_DataMainTable.Columns[1], ListSortDirection.Ascending);
            OSD_DataMainTable.ClearSelection();
            OSD_TYSS_V.Text = OSD_DataMainTable.Rows.Count.ToString();
            OSD_DataMainTable.ResumeLayout();
        }
        private void OSD_TextBox_TextChanged(object sender, EventArgs e){
            string searchText = OSD_TextBox.Text.Trim().ToLower();
            bool isTextBoxEmpty = string.IsNullOrEmpty(searchText);
            OSD_DataMainTable.ClearSelection();
            OSD_TextBoxClearBtn.Enabled = !isTextBoxEmpty;
            if (OSD_DataMainTable.Rows.Count > 0)
                OSD_DataMainTable.FirstDisplayedScrollingRowIndex = 0;
            if (!isTextBoxEmpty){
                foreach (DataGridViewRow driver_row in OSD_DataMainTable.Rows){
                    var cellValue = driver_row.Cells[1].Value;
                    if (cellValue != null && cellValue.ToString().ToLower().Contains(searchText)){
                        driver_row.Selected = true;
                        OSD_DataMainTable.FirstDisplayedScrollingRowIndex = driver_row.Index;
                        break;
                    }
                }
            }
        }
        private void OSD_DataMainTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            try{
                if (OSD_DataMainTable.SelectedRows.Count > 0){
                    Clipboard.SetText(string.Format("{0} | {1} | {2} | {3} | {4} | {5}", OSD_DataMainTable.Rows[e.RowIndex].Cells[0].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[1].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[2].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[3].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[4].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[5].Value.ToString()));
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("Osd_Content", "osd_c_copy_success"), OSD_DataMainTable.Rows[e.RowIndex].Cells[0].Value));
                }
            }catch (Exception){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("Osd_Content", "osd_c_copy_error"), OSD_DataMainTable.Rows[e.RowIndex].Cells[0].Value));
            }
        }
        private void OSD_DataMainTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e){
            try{
                if (OSD_DataMainTable.SelectedRows.Count > 0){
                    OSD_DataMainTable.ClearSelection();
                }
            }catch (Exception){ }
        }
        private void OSD_TextBoxClearBtn_Click(object sender, EventArgs e){
            try{
                OSD_TextBox.Text = string.Empty;
                OSD_TextBox.Focus();
                OSD_DataMainTable.FirstDisplayedScrollingRowIndex = 0;
            }catch (Exception){ }
        }
        private void OSD_SortMode_CheckedChanged(object sender, EventArgs e){
            try{
                if (OSD_SortMode.CheckState == CheckState.Checked){
                    OSD_DataMainTable.Sort(OSD_DataMainTable.Columns[1], ListSortDirection.Descending);
                }else if (OSD_SortMode.CheckState == CheckState.Unchecked){
                    OSD_DataMainTable.Sort(OSD_DataMainTable.Columns[1], ListSortDirection.Ascending);
                }
                OSD_DataMainTable.ClearSelection();
            }catch (Exception){ }
        }
        #endregion
        #region SERVICES_Section
        // GS SERVICE
        // ======================================================================================================
        private Task InstalledServices(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            var serviceTypeMappings = new Dictionary<string, string>{
                { "Kernel Driver", "ss_c_kernel_driver" },
                { "File System Driver", "ss_c_file_system_service" },
                { "Adapter", "ss_c_adaptor" },
                { "Recognizer Driver", "ss_c_recognizer_service" },
                { "Own Process", "ss_c_own_system" },
                { "Share Process", "ss_c_process_sharer" },
                { "Interactive Process", "ss_c_interactive_process" },
                { "Unknown", "ss_c_unknown" }
            };
            var startModeMappings = new Dictionary<string, string>{
                { "Boot", "ss_c_boot" },
                { "Manual", "ss_c_manuel" },
                { "System", "ss_c_system" },
                { "Auto", "ss_c_auto" },
                { "Disabled", "ss_c_disabled" },
                { "Unknown", "ss_c_unknown" }
            };
            var stateMappings = new Dictionary<string, string>{
                { "Stopped", "ss_c_stopped" },
                { "Running", "ss_c_working" }
            };
            try{
                var search = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Service");
                var results = search.Get().Cast<ManagementObject>().ToList();
                var serviceRows = new List<string[]>(results.Count);
                foreach (var s in results){
                    string path = Convert.ToString(s["PathName"])?.Replace("\"", "").Replace("\\??\\", "").Trim() ?? "";
                    string name = Convert.ToString(s["Name"])?.Trim() ?? "";
                    string disp = Convert.ToString(s["DisplayName"])?.Trim() ?? "";
                    string type = Convert.ToString(s["ServiceType"])?.Trim();
                    string start = Convert.ToString(s["StartMode"])?.Trim();
                    string state = Convert.ToString(s["State"])?.Trim();
                    //
                    type = serviceTypeMappings.TryGetValue(type ?? "", out var t) ? software_lang.TSReadLangs("Services_Content", t) : software_lang.TSReadLangs("Services_Content", "ss_c_unknown");
                    start = startModeMappings.TryGetValue(start ?? "", out var sm) ? software_lang.TSReadLangs("Services_Content", sm) : software_lang.TSReadLangs("Services_Content", "ss_c_unknown");
                    state = stateMappings.TryGetValue(state ?? "", out var st) ? software_lang.TSReadLangs("Services_Content", st) : software_lang.TSReadLangs("Services_Content", "ss_c_unknown");
                    //
                    serviceRows.Add(new[] { path, name, disp, type, start, state });
                }
                Action uiUpdate = () =>{
                    BulkAddServiceRows(serviceRows, software_lang);
                };
                if (SERVICE_DataMainTable.InvokeRequired)
                    SERVICE_DataMainTable.Invoke(uiUpdate);
                else
                    uiUpdate();
            }catch (ManagementException) { }
            finally{
                Action finalizeUi = () =>{
                    SERVICES_RotateBtn.Enabled = true;
                    ((Control)SERVICES).Enabled = true;
                    SERVICE_DataMainTable.PerformLayout();
                    SERVICE_DataMainTable.Invalidate();
                    if (Program.debug_mode)
                        Console.WriteLine("<--- Installed Services Section Installed --->");
                };
                if (SERVICE_DataMainTable.InvokeRequired)
                    SERVICE_DataMainTable.Invoke(finalizeUi);
                else
                    finalizeUi();
            }
            return Task.CompletedTask;
        }
        private void BulkAddServiceRows(List<string[]> serviceRows, TSGetLangs software_lang){
            SERVICE_DataMainTable.SuspendLayout();
            SERVICE_DataMainTable.Rows.Clear();
            foreach (var row in serviceRows)
                SERVICE_DataMainTable.Rows.Add(row);
            string unknownValue = software_lang.TSReadLangs("Services_Content", "ss_c_unknown");
            foreach (DataGridViewRow r in SERVICE_DataMainTable.Rows)
                foreach (DataGridViewCell c in r.Cells)
                    if (string.IsNullOrWhiteSpace(c.Value?.ToString()))
                        c.Value = unknownValue;
            SERVICE_DataMainTable.Sort(SERVICE_DataMainTable.Columns[1], ListSortDirection.Ascending);
            SERVICE_DataMainTable.ClearSelection();
            SERVICE_TYS_V.Text = SERVICE_DataMainTable.Rows.Count.ToString();
            SERVICE_DataMainTable.ResumeLayout();
        }
        private void Services_SearchTextBox_TextChanged(object sender, EventArgs e){
            string searchText = SERVICE_TextBox.Text.Trim().ToLower();
            bool isTextBoxEmpty = string.IsNullOrEmpty(searchText);
            SERVICE_DataMainTable.ClearSelection();
            SERVICE_TextBoxClearBtn.Enabled = !isTextBoxEmpty;
            if (SERVICE_DataMainTable.Rows.Count > 0)
                SERVICE_DataMainTable.FirstDisplayedScrollingRowIndex = 0;
            if (!isTextBoxEmpty){
                foreach (DataGridViewRow service_row in SERVICE_DataMainTable.Rows){
                    var cellValue = service_row.Cells[1].Value;
                    if (cellValue != null && cellValue.ToString().ToLower().Contains(searchText)){
                        service_row.Selected = true;
                        SERVICE_DataMainTable.FirstDisplayedScrollingRowIndex = service_row.Index;
                        break;
                    }
                }
            }
        }
        private void SERVICE_DataMainTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            try{
                if (SERVICE_DataMainTable.SelectedRows.Count > 0){
                    Clipboard.SetText(string.Format("{0} | {1} | {2} | {3} | {4} | {5}", SERVICE_DataMainTable.Rows[e.RowIndex].Cells[0].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[1].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[2].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[3].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[4].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[5].Value.ToString()));
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("Services_Content", "ss_c_copy_success"), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[0].Value));
                }
            }catch (Exception){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("Services_Content", "ss_c_copy_error"), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[0].Value));
            }
        }
        private void SERVICE_DataMainTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e){
            try{
                if (SERVICE_DataMainTable.SelectedRows.Count > 0){
                    SERVICE_DataMainTable.ClearSelection();
                }
            }catch (Exception){ }
        }
        private void SERVICE_TextBoxClearBtn_Click(object sender, EventArgs e){
            try{
                SERVICE_TextBox.Text = string.Empty;
                SERVICE_TextBox.Focus();
                SERVICE_DataMainTable.FirstDisplayedScrollingRowIndex = 0;
            }catch (Exception){ }
        }
        private void SERVICES_SortMode_CheckedChanged(object sender, EventArgs e){
            try{
                if (SERVICE_SortMode.CheckState == CheckState.Checked){
                    SERVICE_DataMainTable.Sort(SERVICE_DataMainTable.Columns[1], ListSortDirection.Descending);
                }else if (SERVICE_SortMode.CheckState == CheckState.Unchecked){
                    SERVICE_DataMainTable.Sort(SERVICE_DataMainTable.Columns[1], ListSortDirection.Ascending);
                }
                SERVICE_DataMainTable.ClearSelection();
            }catch (Exception){ }
        }
        #endregion
        #region INSTAPPS
        private Task InstalledApps(){
            try{
                LoadInstalledApplications();
                INSTAPPS_TYUS_V.Text = INSTAPPS_DataMainTable.Rows.Count.ToString();
                INSTAPPS_DataMainTable.ClearSelection();
            }catch (Exception){ }
            finally{
                Action finalizeUi = () =>{
                    INSTALLED_RotateBtn.Enabled = true;
                    ((Control)INSTAPPS).Enabled = true;
                    INSTAPPS_DataMainTable.PerformLayout();
                    INSTAPPS_DataMainTable.Invalidate();
                    if (Program.debug_mode){
                        Console.WriteLine("<--- Installed Application Section Installed --->");
                    }
                };
                if (INSTAPPS_DataMainTable.InvokeRequired)
                    INSTAPPS_DataMainTable.Invoke(finalizeUi);
                else
                    finalizeUi();
            }
            return Task.CompletedTask;
        }
        private void INSTAPPS_TextBox_TextChanged(object sender, EventArgs e){
            string searchText = INSTAPPS_TextBox.Text.Trim();
            bool isTextBoxEmpty = string.IsNullOrEmpty(searchText);
            INSTAPPS_DataMainTable.ClearSelection();
            INSTAPPS_TextBoxClearBtn.Enabled = !isTextBoxEmpty;
            if (INSTAPPS_DataMainTable.Rows.Count > 0)
                INSTAPPS_DataMainTable.FirstDisplayedScrollingRowIndex = 0;
            if (!isTextBoxEmpty){
                foreach (DataGridViewRow service_row in INSTAPPS_DataMainTable.Rows){
                    var cellVal = service_row.Cells[1].Value?.ToString();
                    if (!string.IsNullOrEmpty(cellVal) && cellVal.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0){
                        service_row.Selected = true;
                        INSTAPPS_DataMainTable.FirstDisplayedScrollingRowIndex = service_row.Index;
                        break;
                    }
                }
            }
        }
        private void INSTAPPS_DataMainTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e){
            if (INSTAPPS_DataMainTable.SelectedRows.Count == 0) return;
            //
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            var selected_row = INSTAPPS_DataMainTable.SelectedRows[0];
            //
            DialogResult open_apps_folder = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("Instapps_Content", "ia_open_apps"), selected_row.Cells[1].Value));
            if (open_apps_folder == DialogResult.Yes){
                var installPath = selected_row.Cells[6].Value as string;
                if (string.IsNullOrWhiteSpace(installPath) || installPath == iapps_unknown || !Directory.Exists(installPath)){
                    TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("Instapps_Content", "ia_not_path"));
                    return;
                }
                try{
                    var exePath = Directory.GetFiles(installPath, "*.exe").FirstOrDefault();
                    if (string.IsNullOrEmpty(exePath)){
                        TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("Instapps_Content", "ia_not_exe"));
                        return;
                    }
                    Process.Start(new ProcessStartInfo("explorer.exe", $"/select,\"{exePath}\"") { UseShellExecute = true });
                }catch (Exception ex){
                    TS_MessageBoxEngine.TS_MessageBox(this, 2, string.Format(software_lang.TSReadLangs("Instapps_Content", "ia_file_path_not_oppened"), ex.Message));
                }
            }
        }
        private void INSTAPPS_DataMainTable_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e){
            try{
                if (INSTAPPS_DataMainTable.SelectedRows.Count > 0){
                    INSTAPPS_DataMainTable.ClearSelection();
                }
            }catch (Exception){ }
        }
        private void INSTAPPS_TextBoxClearBtn_Click(object sender, EventArgs e){
            try{
                INSTAPPS_TextBox.Text = string.Empty;
                INSTAPPS_TextBox.Focus();
                INSTAPPS_DataMainTable.FirstDisplayedScrollingRowIndex = 0;
            }catch (Exception){ }
        }
        private void INSTAPPS_SortMode_CheckedChanged(object sender, EventArgs e){
            try{
                if (INSTAPPS_SortMode.CheckState == CheckState.Checked){
                    INSTAPPS_DataMainTable.Sort(INSTAPPS_DataMainTable.Columns[1], ListSortDirection.Descending);
                }else if (INSTAPPS_SortMode.CheckState == CheckState.Unchecked){
                    INSTAPPS_DataMainTable.Sort(INSTAPPS_DataMainTable.Columns[1], ListSortDirection.Ascending);
                }
                INSTAPPS_DataMainTable.ClearSelection();
            }catch (Exception){ }
        }
        public class InstalledAppConfig{
            public string DisplayIcon { get; set; }
            public Icon AppIcon { get; set; }
            public string Name { get; set; }
            public string Publisher { get; set; }
            public DateTime? InstallDate { get; set; }
            public long? Size { get; set; }
            public string Version { get; set; }
            public string InstallLocation { get; set; }
        }
        public static class AppWindowIcon{
            [DllImport("Shell32.dll", CharSet = CharSet.Auto)]
            private static extern uint ExtractIconEx(string lpszFile, int nIconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, uint nIcons);
            [DllImport("user32.dll", SetLastError = true)]
            private static extern bool DestroyIcon(IntPtr hIcon);
            public static Icon ExtractIconFromFile(string filePath, int index, bool large = true){
                if (string.IsNullOrWhiteSpace(filePath)){
                    return null;
                }
                IntPtr[] largeIcons = large ? new IntPtr[1] : null;
                IntPtr[] smallIcons = !large ? new IntPtr[1] : null;
                uint count = ExtractIconEx(filePath, index, largeIcons, smallIcons, 1);
                if (count == 0){
                    return null;
                }
                IntPtr hIcon = large ? largeIcons[0] : smallIcons[0];
                if (hIcon == IntPtr.Zero)
                    return null;
                try{
                    return (Icon)Icon.FromHandle(hIcon).Clone();
                }finally{
                    DestroyIcon(hIcon);
                }
            }
        }
        private static void ParseDisplayIcon(string displayIconRaw, out string iconPath, out int iconIndex){
            iconIndex = 0;
            iconPath = string.Empty;
            if (string.IsNullOrWhiteSpace(displayIconRaw)){
                return;
            }
            string s = Environment.ExpandEnvironmentVariables(displayIconRaw).Trim();
            if (s.StartsWith("@", StringComparison.Ordinal)){
                s = s.Substring(1).Trim();
            }
            s = s.Trim().Trim('"');
            int comma = s.LastIndexOf(',');
            if (comma > 0 && comma < s.Length - 1){
                string pathPart = s.Substring(0, comma).Trim().Trim('"');
                string indexPart = s.Substring(comma + 1).Trim();
                if (int.TryParse(indexPart, NumberStyles.Integer, CultureInfo.InvariantCulture, out int idx)){
                    iconPath = pathPart;
                    iconIndex = idx;
                    return;
                }
            }
            iconPath = s;
            iconIndex = 0;
        }
        private Image _defaultIconCache;
        private Image GetDefaultIcon(){
            if (_defaultIconCache != null){
                return (Image)_defaultIconCache.Clone();
            }
            try{
                using (var ico = AppWindowIcon.ExtractIconFromFile("shell32.dll", 2, large: true)){
                    if (ico != null){
                        _defaultIconCache = ico.ToBitmap();
                        return (Image)_defaultIconCache.Clone();
                    }
                }
            }catch { }
            Bitmap bmpFallback = new Bitmap(32, 32);
            using (Graphics g = Graphics.FromImage(bmpFallback)){
                g.Clear(TS_ThemeEngine.ColorMode(theme, "AccentColor"));
                g.DrawRectangle(Pens.Black, 4, 4, 24, 24);
            }
            _defaultIconCache = bmpFallback;
            return (Image)_defaultIconCache.Clone();
        }
        private void LoadInstalledApplications(){
            var apps = GetInstalledApplications();
            if (INSTAPPS_DataMainTable.InvokeRequired){
                INSTAPPS_DataMainTable.Invoke(new Action(() => INSTAPPS_DataMainTable.Rows.Clear()));
            }else{
                INSTAPPS_DataMainTable.Rows.Clear();
            }
            foreach (var app in apps){
                Image iconImage = null;
                if (!string.IsNullOrWhiteSpace(app.DisplayIcon)){
                    try{
                        ParseDisplayIcon(app.DisplayIcon, out string iconPath, out int iconIndex);
                        if (!string.IsNullOrWhiteSpace(iconPath) && File.Exists(iconPath)){
                            string ext = Path.GetExtension(iconPath);
                            if (string.Equals(ext, ".ico", StringComparison.OrdinalIgnoreCase)){
                                using (var ico = new Icon(iconPath)){
                                    iconImage = ico.ToBitmap();
                                }
                            }else{
                                using (var ico = AppWindowIcon.ExtractIconFromFile(iconPath, iconIndex, large: true)){
                                    if (ico != null){
                                        iconImage = ico.ToBitmap();
                                    }
                                }
                            }
                        }
                    }catch { }
                }
                if (iconImage == null && !string.IsNullOrWhiteSpace(app.InstallLocation) && Directory.Exists(app.InstallLocation)){
                    try{
                        var exe = Directory.EnumerateFiles(app.InstallLocation, "*.exe").FirstOrDefault(p => !p.EndsWith("uninstall.exe", StringComparison.OrdinalIgnoreCase) && !p.EndsWith("unins000.exe", StringComparison.OrdinalIgnoreCase));
                        if (!string.IsNullOrEmpty(exe)){
                            using (Icon icon = Icon.ExtractAssociatedIcon(exe)){
                                if (icon != null){
                                    iconImage = icon.ToBitmap();
                                }
                            }
                        }
                    }catch { }
                }
                if (iconImage == null){
                    iconImage = GetDefaultIcon();
                }
                iconImage = ResizeDGIcon(iconImage, 30, this.DeviceDpi);
                var nameText = string.IsNullOrWhiteSpace(app.Name) ? iapps_unknown : app.Name;
                var publisherText = string.IsNullOrWhiteSpace(app.Publisher) ? iapps_unknown : app.Publisher;
                var installDateText = app.InstallDate?.ToString("dd.MM.yyyy") ?? iapps_unknown;
                var sizeText = app.Size.HasValue ? TS_FormatSize(app.Size.Value * 1024) : iapps_unknown;
                var versionText = string.IsNullOrWhiteSpace(app.Version) ? iapps_unknown : app.Version;
                var installLocationText = string.IsNullOrWhiteSpace(app.InstallLocation) ? iapps_unknown : app.InstallLocation;
                if (INSTAPPS_DataMainTable.InvokeRequired){
                    INSTAPPS_DataMainTable.Invoke(new Action(() =>{
                        INSTAPPS_DataMainTable.Rows.Add(iconImage, nameText, publisherText, installDateText, sizeText, versionText, installLocationText);
                    }));
                }else{
                    INSTAPPS_DataMainTable.Rows.Add(iconImage, nameText, publisherText, installDateText, sizeText, versionText, installLocationText);
                }
            }
        }
        private List<InstalledAppConfig> GetInstalledApplications(){
            var apps = new List<InstalledAppConfig>();
            var registryViews = new[] { RegistryView.Registry64, RegistryView.Registry32 };
            var registryRoots = new[] { RegistryHive.LocalMachine, RegistryHive.CurrentUser };
            foreach (var view in registryViews){
                foreach (var root in registryRoots){
                    using (var baseKey = RegistryKey.OpenBaseKey(root, view))
                    using (var uninstallKey = baseKey.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall")){
                        if (uninstallKey == null) continue;
                        foreach (var subkeyName in uninstallKey.GetSubKeyNames()){
                            using (var subkey = uninstallKey.OpenSubKey(subkeyName)){
                                if (subkey == null) continue;
                                var displayIcon = (subkey.GetValue("DisplayIcon") as string)?.Trim()?.Trim('"');
                                var name = (subkey.GetValue("DisplayName") as string)?.Trim();
                                if (string.IsNullOrWhiteSpace(name)) continue;
                                var publisher = (subkey.GetValue("Publisher") as string)?.Trim();
                                var installLocationRaw = (subkey.GetValue("InstallLocation") as string)?.Trim();
                                var installLocation = installLocationRaw?.Trim('"');
                                var version = (subkey.GetValue("DisplayVersion") as string)?.Trim();
                                var installDateRaw = (subkey.GetValue("InstallDate") as string)?.Trim();
                                DateTime? installDate = null;
                                if (!string.IsNullOrWhiteSpace(installDateRaw) && installDateRaw.Length == 8){
                                    if (DateTime.TryParseExact(installDateRaw, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDate)){
                                        installDate = parsedDate;
                                    }
                                }
                                var sizeValue = subkey.GetValue("EstimatedSize");
                                long? sizeKb = null;
                                if (sizeValue != null){
                                    try{
                                        sizeKb = Convert.ToInt64(sizeValue);
                                    }catch { }
                                }
                                apps.Add(new InstalledAppConfig{
                                    DisplayIcon = displayIcon,
                                    AppIcon = null,
                                    Name = name,
                                    Publisher = publisher,
                                    InstallDate = installDate,
                                    Size = sizeKb,
                                    Version = version,
                                    InstallLocation = installLocation
                                });
                            }
                        }
                    }
                }
            }
            return apps.GroupBy(a => a.Name, StringComparer.OrdinalIgnoreCase).Select(g => g.First()).OrderBy(a => TSNaturalSortKey(a.Name, CultureInfo.CurrentCulture)).ToList();
        }
        private void INSTAPPS_DataMainTable_CellPainting(object sender, DataGridViewCellPaintingEventArgs e){
            if (e.ColumnIndex == 0 && e.RowIndex >= 0){
                e.PaintBackground(e.CellBounds, true);
                if (e.Value is Image img){
                    float scale = this.DeviceDpi / 96f;
                    int iconSize = (int)(30 * scale);
                    int x = e.CellBounds.X + (e.CellBounds.Width - iconSize) / 2;
                    int y = e.CellBounds.Y + (e.CellBounds.Height - iconSize) / 2;
                    var rect = new Rectangle(x, y, iconSize, iconSize);
                    e.Graphics.DrawImage(img, rect);
                }
                e.Handled = true;
            }
        }
        #endregion
        // BUTTONS ROTATE
        // ======================================================================================================
        private void Active_page(object btn_target){
            Button active_btn = null;
            Disabled_page();
            if (btn_target != null){
                if (active_btn != (Button)btn_target){
                    active_btn = (Button)btn_target;
                    active_btn.BackColor = TS_ThemeEngine.ColorMode(theme, "BtnActiveColor");
                    active_btn.Cursor = Cursors.Default;
                }
            }
        }
        private void Disabled_page(){
            foreach (Control disabled_btn in LeftMenuPanel.Controls){
                disabled_btn.BackColor = TS_ThemeEngine.ColorMode(theme, "BtnDeActiveColor");
                disabled_btn.Cursor = Cursors.Hand;
            }
        }
        // LEFT MENU SENDERS
        private void OSRotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(1, sender); }
        private void MBRotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(2, sender); }
        private void CPURotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(3, sender); }
        private void RAMRotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(4, sender); }
        private void GPURotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(5, sender); }
        private void DISKRotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(6, sender); }
        private void NETWORKRotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(7, sender); }
        private void USB_RotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(8, sender); }
        private void SOUND_RotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(9, sender); }
        private void PILRotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(10, sender); }
        private void OSDRotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(11, sender); }
        private void ServicesRotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(12, sender); }
        private void INSTALLED_RotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(13, sender); }
        private void PRINT_RotateBtn_Click(object sender, EventArgs e) { Left_menu_preloader(14, sender); }
        // DYNAMIC ARROW KEYS ROTATE
        private void MainContent_Selecting(object sender, TabControlCancelEventArgs e){
            try{
                if (!e.TabPage.Enabled)
                    e.Cancel = true;
            }catch (Exception){ }
        }
        private void MainContent_SelectedIndexChanged(object sender, EventArgs e){
            try{
                var tabButtons = new Dictionary<int, Button>{
                    { 0, OS_RotateBtn },
                    { 1, MB_RotateBtn },
                    { 2, CPU_RotateBtn },
                    { 3, RAM_RotateBtn },
                    { 4, GPU_RotateBtn },
                    { 5, DISK_RotateBtn },
                    { 6, NET_RotateBtn },
                    { 7, USB_RotateBtn },
                    { 8, SOUND_RotateBtn },
                    { 9, BATTERY_RotateBtn },
                    { 10, OSD_RotateBtn },
                    { 11, SERVICES_RotateBtn },
                    { 12, INSTALLED_RotateBtn },
                    { 13, PRINT_RotateBtn }
                };
                if (tabButtons.TryGetValue(MainContent.SelectedIndex, out Button btn)){
                    Active_page(btn);
                    menu_btns = MainContent.SelectedIndex + 1;
                    menu_rp = MainContent.SelectedIndex + 1;
                    UpdateHeaderByIndex(MainContent.SelectedIndex);
                }
            }catch (Exception){ }
        }
        // DYNAMIC LEFT MENU PRELOADER
        private void Left_menu_preloader(int target_menu, object sender){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                var menuTargets = new Dictionary<int, (TabPage tab, Button rotateBtn, string headerKey)>{
                    { 1,  (OS, OS_RotateBtn, "header_os") },
                    { 2,  (MB, MB_RotateBtn, "header_mb") },
                    { 3,  (CPU, CPU_RotateBtn, "header_cpu") },
                    { 4,  (RAM, RAM_RotateBtn, "header_ram") },
                    { 5,  (GPU, GPU_RotateBtn, "header_gpu") },
                    { 6,  (DISK, DISK_RotateBtn, "header_storage") },
                    { 7,  (NETWORK, NET_RotateBtn, "header_network") },
                    { 8,  (USB, USB_RotateBtn, "header_usb") },
                    { 9,  (SOUND, SOUND_RotateBtn, "header_sound") },
                    { 10, (BATTERY, BATTERY_RotateBtn, "header_battery") },
                    { 11, (DRIVERS, OSD_RotateBtn, "header_installed_drivers") },
                    { 12, (SERVICES, SERVICES_RotateBtn, "header_installed_services") },
                    { 13, (INSTAPPS, INSTALLED_RotateBtn, "header_installed_apps") },
                    { 14, (EXPORT, PRINT_RotateBtn, "header_export") }
                };
                if (menu_btns != target_menu && menuTargets.TryGetValue(target_menu, out var target)){
                    MainContent.SelectedTab = target.tab;
                    if (!btn_colors_active.Contains(target.rotateBtn.BackColor))
                        Active_page(sender);
                    HeaderText.Text = software_lang.TSReadLangs("Header", target.headerKey);
                }
                menu_btns = target_menu;
                menu_rp = target_menu;
                Header_image_reloader(menu_btns);
            }catch (Exception) { }
        }
        private void UpdateHeaderByIndex(int index){
            var keys = new[]{
                "header_os", "header_mb", "header_cpu", "header_ram", "header_gpu",
                "header_storage", "header_network", "header_usb", "header_sound",
                "header_battery", "header_installed_drivers", "header_installed_services",
                "header_installed_apps", "header_export"
            };
            if (index >= 0 && index < keys.Length){
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                HeaderText.Text = software_lang.TSReadLangs("Header", keys[index]);
                Header_image_reloader(index + 1);
            }
        }
        // LANGUAGES SETTINGS
        // ======================================================================================================
        private void Select_lang_active(object target_lang){
            ToolStripMenuItem selected_lang = null;
            Select_lang_deactive();
            if (target_lang != null){
                if (selected_lang != (ToolStripMenuItem)target_lang){
                    selected_lang = (ToolStripMenuItem)target_lang;
                    selected_lang.Checked = true;
                }
            }
        }
        private void Select_lang_deactive(){
            foreach (ToolStripMenuItem disabled_lang in languageToolStripMenuItem.DropDownItems){
                disabled_lang.Checked = false;
            }
        }
        // LANG SWAP
        // ======================================================================================================
        private void LanguageToolStripMenuItem_Click(object sender, EventArgs e){
            if (sender is ToolStripMenuItem menuItem && menuItem.Tag is string langCode){
                if (lang != langCode && AllLanguageFiles.ContainsKey(langCode)){
                    Lang_preload(AllLanguageFiles[langCode], langCode);
                    Select_lang_active(sender);
                }
            }
        }
        private void Lang_preload(string lang_type, string lang_code){
            Lang_engine(lang_type, lang_code);
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "LanguageStatus", lang_code);
            }catch (Exception){ }
            // LANG CHANGE NOTIFICATION
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            DialogResult lang_change_message = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("LangChange", "lang_change_notification"), "\n\n", "\n\n"));
            if (lang_change_message == DialogResult.Yes){ Application.Restart(); }
        }
        private void Lang_engine(string lang_type, string lang_code){
            try{
                lang_path = lang_type;
                lang = lang_code;
                // GLOBAL ENGINE
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                // HEADER TITLE
                var headers = new Dictionary<int, string>{
                    { 1, "header_os" },
                    { 2, "header_mb" },
                    { 3, "header_cpu" },
                    { 4, "header_ram" },
                    { 5, "header_gpu" },
                    { 6, "header_storage" },
                    { 7, "header_network" },
                    { 8, "header_usb" },
                    { 9, "header_sound" },
                    { 10, "header_battery" },
                    { 11, "header_installed_drivers" },
                    { 12, "header_installed_services" },
                    { 13, "header_installed_apps" },
                    { 14, "header_export" }
                };
                if (headers.TryGetValue(menu_rp, out string headerKey)){
                    HeaderText.Text = software_lang.TSReadLangs("Header", headerKey);
                }
                // SETTINGS
                settingsToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_settings");
                // THEMES
                themeToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_theme");
                lightThemeToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderThemes", "theme_light");
                darkThemeToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderThemes", "theme_dark");
                systemThemeToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderThemes", "theme_system");
                // LANGS
                languageToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_language");
                arabicToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_ar");
                chineseToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_zh");
                englishToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_en");
                dutchToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_nl");
                frenchToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_fr");
                germanToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_de");
                hindiToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_hi");
                italianToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_it");
                japaneseToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_ja");
                koreanToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_ko");
                polishToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_pl");
                portugueseToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_pt");
                russianToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_ru");
                spanishToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_es");
                turkishToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderLangs", "lang_tr");
                // STARTUP VIEW
                startupToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_start");
                windowedToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderViewMode", "header_view_mode_windowed");
                fullScreenToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderViewMode", "header_view_mode_full_screen");
                // HIDING MODE
                hidingModeToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_privacy_mode");
                hidingModeOnToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on");
                hidingModeOffToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_off");
                // UPDATE CHECK
                checkForUpdatesToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_update");
                // TOOLS
                toolsToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_tools");
                sFCandDISMAutoTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_sfc_and_dism_tool");
                cacheCleaningTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_cache_cleanup_tool");
                benchCPUTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_bench_cpu");
                benchRAMTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_bench_ram");
                benchDiskTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_bench_disk");
                screenOverlayTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_overlay");
                dnsTestTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_dns_test_tool");
                quickAccessTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_quick_access_tool");
                networkFixTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_network_fix_tool");
                showWiFiPasswordTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_show_wifi_password_tool");
                bluetoothFinderToolToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderTools", "ht_bluetooth_finder_tool");
                systemIdGeneratorTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_system_id_generator_tool");
                monitorTestTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_monitor_test");
                monitorDeadPixelTestTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_monitor_test_dead_pixel");
                monitorDynamicRangeTestTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_monitor_test_dynamic_range");
                monitorStuckPixelFixerTool.Text = software_lang.TSReadLangs("HeaderTools", "ht_stuck_pixel_fixer_tool");
                // TS WIZARD
                tSWizardToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_ts_wizard");
                // DONATE
                donateToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_donate");
                // ABOUT
                aboutToolStripMenuItem.Text = software_lang.TSReadLangs("HeaderMenu", "header_menu_about");
                // HIDING MENU
                OS.Text = software_lang.TSReadLangs("LeftMenu", "left_os");
                MB.Text = software_lang.TSReadLangs("LeftMenu", "left_mb");
                CPU.Text = software_lang.TSReadLangs("LeftMenu", "left_cpu");
                RAM.Text = software_lang.TSReadLangs("LeftMenu", "left_ram");
                GPU.Text = software_lang.TSReadLangs("LeftMenu", "left_gpu");
                DISK.Text = software_lang.TSReadLangs("LeftMenu", "left_storage");
                NETWORK.Text = software_lang.TSReadLangs("LeftMenu", "left_network");
                USB.Text = software_lang.TSReadLangs("LeftMenu", "left_usb");
                SOUND.Text = software_lang.TSReadLangs("LeftMenu", "left_sound");
                BATTERY.Text = software_lang.TSReadLangs("LeftMenu", "left_battery");
                DRIVERS.Text = software_lang.TSReadLangs("LeftMenu", "left_installed_drivers");
                SERVICES.Text = software_lang.TSReadLangs("LeftMenu", "left_installed_services");
                INSTAPPS.Text = software_lang.TSReadLangs("LeftMenu", "left_installed_apps");
                EXPORT.Text = software_lang.TSReadLangs("LeftMenu", "left_export");
                // MENU
                OS_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_os");
                MB_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_mb");
                CPU_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_cpu");
                RAM_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_ram");
                GPU_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_gpu");
                DISK_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_storage");
                NET_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_network");
                USB_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_usb");
                SOUND_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_sound");
                BATTERY_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_battery");
                OSD_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_installed_drivers");
                SERVICES_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_installed_services");
                INSTALLED_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_installed_apps");
                PRINT_RotateBtn.Text = " " + " " + software_lang.TSReadLangs("LeftMenu", "left_export");
                // OS
                OS_SystemUser.Text = software_lang.TSReadLangs("OperatingSystem", "os_user");
                OS_ComputerName.Text = software_lang.TSReadLangs("OperatingSystem", "os_computer_name");
                OS_SavedUser.Text = software_lang.TSReadLangs("OperatingSystem", "os_saved_user_account");
                OS_Name.Text = software_lang.TSReadLangs("OperatingSystem", "os_operating_system");
                OS_Manufacturer.Text = software_lang.TSReadLangs("OperatingSystem", "os_operating_system_publisher");
                OS_SystemVersion.Text = software_lang.TSReadLangs("OperatingSystem", "os_system_version");
                OS_SystemArchitectural.Text = software_lang.TSReadLangs("OperatingSystem", "os_system_architecture");
                OS_DeviceID.Text = software_lang.TSReadLangs("OperatingSystem", "os_device_id");
                OS_Serial.Text = software_lang.TSReadLangs("OperatingSystem", "os_product_id");
                OS_Country.Text = software_lang.TSReadLangs("OperatingSystem", "os_adjustable_language");
                OS_TimeZone.Text = software_lang.TSReadLangs("OperatingSystem", "os_timezone");
                OS_EncryptionType.Text = software_lang.TSReadLangs("OperatingSystem", "os_encrypt_type");
                OS_SystemTime.Text = software_lang.TSReadLangs("OperatingSystem", "os_time");
                OS_Install.Text = software_lang.TSReadLangs("OperatingSystem", "os_install_date");
                OS_SystemWorkTime.Text = software_lang.TSReadLangs("OperatingSystem", "os_system_work_time");
                OS_LastBootTime.Text = software_lang.TSReadLangs("OperatingSystem", "os_last_boot_time");
                OS_SystemLastShutDown.Text = software_lang.TSReadLangs("OperatingSystem", "os_last_shutdown_time");
                OS_PrimaryOS.Text = software_lang.TSReadLangs("OperatingSystem", "os_primary_os");
                OS_PortableOS.Text = software_lang.TSReadLangs("OperatingSystem", "os_portable_os");
                OS_MouseWheelStatus.Text = software_lang.TSReadLangs("OperatingSystem", "os_mouse_scroll_speed");
                OS_ScrollLockStatus.Text = software_lang.TSReadLangs("OperatingSystem", "os_scroll_lock_status");
                OS_NumLockStatus.Text = software_lang.TSReadLangs("OperatingSystem", "os_numpad_lock_status");
                OS_CapsLockStatus.Text = software_lang.TSReadLangs("OperatingSystem", "os_caps_lock_status");
                OS_FastBoot.Text = software_lang.TSReadLangs("OperatingSystem", "os_fastboot_status");
                OS_WinPageFile.Text = software_lang.TSReadLangs("OperatingSystem", "os_win_pagefile");
                OS_TempWinPageFile.Text = software_lang.TSReadLangs("OperatingSystem", "os_win_temp_pagefile");
                OS_Hiberfil.Text = software_lang.TSReadLangs("OperatingSystem", "os_win_hiberfil");
                OS_AVProgram.Text = software_lang.TSReadLangs("OperatingSystem", "os_installed_anti_virus_apps");
                OS_FirewallProgram.Text = software_lang.TSReadLangs("OperatingSystem", "os_installed_firewall_apps");
                OS_AntiSpywareProgram.Text = software_lang.TSReadLangs("OperatingSystem", "os_installed_anti_spyware_apps");
                OS_WinDefCoreIsolation.Text = software_lang.TSReadLangs("OperatingSystem", "os_win_core_isolation_status");
                OS_CA2023_Status.Text = software_lang.TSReadLangs("OperatingSystem", "os_ca2023_status");
                OS_CA2023_Capable.Text = software_lang.TSReadLangs("OperatingSystem", "os_ca2023_capable");
                OS_CA2023_Error.Text = software_lang.TSReadLangs("OperatingSystem", "os_ca2023_error");
                OS_ActivePower.Text = software_lang.TSReadLangs("OperatingSystem", "os_active_power");
                OS_ActivePowerGUID.Text = software_lang.TSReadLangs("OperatingSystem", "os_active_power_guid");
                OS_ActivePowerScreenTimeOutP.Text = software_lang.TSReadLangs("OperatingSystem", "os_active_power_sctp");
                OS_ActivePowerScreenTimeOutB.Text = software_lang.TSReadLangs("OperatingSystem", "os_active_power_sctb");
                OS_ActivePowerSleepTimeP.Text = software_lang.TSReadLangs("OperatingSystem", "os_active_power_sptp");
                OS_ActivePowerSleepTimeB.Text = software_lang.TSReadLangs("OperatingSystem", "os_active_power_sptb");
                OS_MSEdge.Text = software_lang.TSReadLangs("OperatingSystem", "os_msedge_version");
                OS_MSEdgeWebView.Text = software_lang.TSReadLangs("OperatingSystem", "os_msedge_webview2_version");
                OS_MSStoreVersion.Text = software_lang.TSReadLangs("OperatingSystem", "os_ms_store_version");
                OS_MSOfficeVersion.Text = software_lang.TSReadLangs("OperatingSystem", "os_ms_office_version");
                OS_WinKey.Text = software_lang.TSReadLangs("OperatingSystem", "os_win_license_key");
                OS_WinActiveChannel.Text = software_lang.TSReadLangs("OperatingSystem", "os_win_activation_channel");
                OS_NETFrameworkVersion.Text = software_lang.TSReadLangs("OperatingSystem", "os_net_framework_version");
                OS_Minidump.Text = software_lang.TSReadLangs("OperatingSystem", "os_detect_minidump_count");
                OS_BSODDate.Text = software_lang.TSReadLangs("OperatingSystem", "os_last_bsod_time");
                OS_Wallpaper.Text = software_lang.TSReadLangs("OperatingSystem", "os_wallpaper");
                if (!string.IsNullOrEmpty(wp_rotate)){
                    MainToolTip.SetToolTip(OS_WallpaperOpen, software_lang.TSReadLangs("Os_Content", "os_c_open_wallpaper"));
                    MainToolTip.SetToolTip(OS_WallpaperPreview, software_lang.TSReadLangs("Os_Content", "os_c_preview_wallpaper"));
                }
                // MB
                MB_BIOSUpdateBtn.Text = " " + software_lang.TSReadLangs("Motherboard", "mb_b_update_btn");
                MB_MotherBoardName.Text = software_lang.TSReadLangs("Motherboard", "mb_model");
                MB_MotherBoardMan.Text = software_lang.TSReadLangs("Motherboard", "mb_manufacturer");
                MB_SystemModelMan.Text = software_lang.TSReadLangs("Motherboard", "mb_system_model_manufacturer");
                MB_SystemModelFamily.Text = software_lang.TSReadLangs("Motherboard", "mb_system_model_family");
                MB_SystemFamily.Text = software_lang.TSReadLangs("Motherboard", "mb_system_family");
                MB_SystemModel.Text = software_lang.TSReadLangs("Motherboard", "mb_system_model");
                MB_DeviceSerialNumber.Text = software_lang.TSReadLangs("Motherboard", "mb_device_serial_number");
                MB_MotherBoardSerial.Text = software_lang.TSReadLangs("Motherboard", "mb_serial");
                MB_SystemSKU.Text = software_lang.TSReadLangs("Motherboard", "mb_system_sku");
                MB_Chipset.Text = software_lang.TSReadLangs("Motherboard", "mb_chipset");
                MB_BiosManufacturer.Text = software_lang.TSReadLangs("Motherboard", "mb_bios_publisher");
                MB_BiosDate.Text = software_lang.TSReadLangs("Motherboard", "mb_bios_date");
                MB_BiosVersion.Text = software_lang.TSReadLangs("Motherboard", "mb_bios_version");
                MB_SmBiosVersion.Text = software_lang.TSReadLangs("Motherboard", "mb_sm_bios_version");
                MB_BiosMode.Text = software_lang.TSReadLangs("Motherboard", "mb_bios_mode");
                MB_LastBIOSTime.Text = software_lang.TSReadLangs("Motherboard", "mb_last_bios_time");
                MB_SecureBoot.Text = software_lang.TSReadLangs("Motherboard", "mb_secure_boot_status");
                MB_SecureBootCA2023.Text = software_lang.TSReadLangs("Motherboard", "mb_secure_boot_ca2023_status");
                MB_TPMStatus.Text = software_lang.TSReadLangs("Motherboard", "mb_tpm_status");
                MB_TPMPhysicalVersion.Text = software_lang.TSReadLangs("Motherboard", "mb_tpm_physical_presence_version");
                MB_TPMMan.Text = software_lang.TSReadLangs("Motherboard", "mb_tpm_manufacturer");
                MB_TPMManID.Text = software_lang.TSReadLangs("Motherboard", "mb_tpm_manufacturer_id");
                MB_TPMManVersion.Text = software_lang.TSReadLangs("Motherboard", "mb_tpm_manufacturer_version");
                MB_TPMManFullVersion.Text = software_lang.TSReadLangs("Motherboard", "mb_tpm_manufacturer_full_version");
                MB_TPMManPublisher.Text = software_lang.TSReadLangs("Motherboard", "mb_tpm_publisher");
                // CPU
                CPU_Name.Text = software_lang.TSReadLangs("Processor", "pr_cpu_model");
                CPU_Manufacturer.Text = software_lang.TSReadLangs("Processor", "pr_cpu_manufacturer");
                CPU_Architectural.Text = software_lang.TSReadLangs("Processor", "pr_cpu_architecture");
                CPU_IntelME.Text = software_lang.TSReadLangs("Processor", "pr_cpu_intel_me");
                CPU_NormalSpeed.Text = software_lang.TSReadLangs("Processor", "pr_cpu_speed");
                CPU_DefaultSpeed.Text = software_lang.TSReadLangs("Processor", "pr_default_cpu_speed");
                CPU_L1.Text = software_lang.TSReadLangs("Processor", "pr_l1_cache_size");
                CPU_L2.Text = software_lang.TSReadLangs("Processor", "pr_l2_cache_size");
                CPU_L3.Text = software_lang.TSReadLangs("Processor", "pr_l3_cache_size");
                CPU_CoreCount.Text = software_lang.TSReadLangs("Processor", "pr_cpu_core_count");
                CPU_LogicalCore.Text = software_lang.TSReadLangs("Processor", "pr_cpu_logical_core_count");
                CPU_Usage.Text = software_lang.TSReadLangs("Processor", "pr_cpu_usage");
                CPU_Process.Text = software_lang.TSReadLangs("Processor", "pr_cpu_process_count");
                CPU_Threads.Text = software_lang.TSReadLangs("Processor", "pr_cpu_threads_count");
                CPU_Handles.Text = software_lang.TSReadLangs("Processor", "pr_cpu_handles_count");
                CPU_SocketDefinition.Text = software_lang.TSReadLangs("Processor", "pr_cpu_socket_definition");
                CPU_Family.Text = software_lang.TSReadLangs("Processor", "pr_cpu_family");
                CPU_Virtualization.Text = software_lang.TSReadLangs("Processor", "pr_cpu_virtualization");
                CPU_HyperV.Text = software_lang.TSReadLangs("Processor", "pr_cpu_hyperv");
                CPU_VMExtension.Text = software_lang.TSReadLangs("Processor", "pr_vm_extension");
                CPU_SerialName.Text = software_lang.TSReadLangs("Processor", "pr_unique_processor_id");
                // RAM
                RAM_TotalRAM.Text = software_lang.TSReadLangs("Memory", "my_total_ram_amount");
                RAM_UsageRAMCount.Text = software_lang.TSReadLangs("Memory", "my_usage_ram_amount");
                RAM_EmptyRamCount.Text = software_lang.TSReadLangs("Memory", "my_empty_ram_amount");
                RAM_TotalVirtualRam.Text = software_lang.TSReadLangs("Memory", "my_total_virtual_ram_amount");
                RAM_UsageVirtualRam.Text = software_lang.TSReadLangs("Memory", "my_usage_virtual_ram_amount");
                RAM_EmptyVirtualRam.Text = software_lang.TSReadLangs("Memory", "my_empty_virtual_ram_amount");
                RAM_SlotStatus.Text = software_lang.TSReadLangs("Memory", "my_ram_slot_fullness");
                RAM_Selector.Text = software_lang.TSReadLangs("Memory", "my_ram_slot");
                RAM_Amount.Text = software_lang.TSReadLangs("Memory", "my_ram_amount");
                RAM_Type.Text = software_lang.TSReadLangs("Memory", "my_ram_type");
                RAM_Frequency.Text = software_lang.TSReadLangs("Memory", "my_ram_frequency");
                RAM_Volt.Text = software_lang.TSReadLangs("Memory", "my_ram_voltage");
                RAM_FormFactor.Text = software_lang.TSReadLangs("Memory", "my_ram_form_factor");
                RAM_Serial.Text = software_lang.TSReadLangs("Memory", "my_ram_serial");
                RAM_Manufacturer.Text = software_lang.TSReadLangs("Memory", "my_ram_manufacturer");
                RAM_BankLabel.Text = software_lang.TSReadLangs("Memory", "my_ram_location");
                RAM_DataWidth.Text = software_lang.TSReadLangs("Memory", "my_ram_width");
                RAM_BellekType.Text = software_lang.TSReadLangs("Memory", "my_partition_type");
                RAM_PartNumber.Text = software_lang.TSReadLangs("Memory", "my_partition_number");
                // GPU
                GPU_Selector.Text = software_lang.TSReadLangs("Gpu", "gpu_model");
                GPU_Manufacturer.Text = software_lang.TSReadLangs("Gpu", "gpu_publisher");
                GPU_VRAM.Text = software_lang.TSReadLangs("Gpu", "gpu_vram");
                GPU_Version.Text = software_lang.TSReadLangs("Gpu", "gpu_driver_version");
                GPU_DriverDate.Text = software_lang.TSReadLangs("Gpu", "gpu_driver_date");
                GPU_Status.Text = software_lang.TSReadLangs("Gpu", "gpu_status");
                GPU_DeviceID.Text = software_lang.TSReadLangs("Gpu", "gpu_hardware_id");
                GPU_DacType.Text = software_lang.TSReadLangs("Gpu", "gpu_dac_type");
                GPU_GraphicDriversName.Text = software_lang.TSReadLangs("Gpu", "gpu_graphic_drivers");
                GPU_InfFileName.Text = software_lang.TSReadLangs("Gpu", "gpu_inf_file");
                GPU_INFSectionFile.Text = software_lang.TSReadLangs("Gpu", "gpu_inf_file_gpu_partition");
                GPU_CurrentColor.Text = software_lang.TSReadLangs("Gpu", "gpu_current_color");
                //
                GPU_MonitorSelector.Text = software_lang.TSReadLangs("Gpu", "gpu_monitors");
                GPU_MonitorUserFriendlyName.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_name");
                GPU_MonitorManName.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_manufacturer");
                GPU_MonitorProductCodeID.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_manufacturer_num");
                GPU_MonitorSerialNumberID.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_serial_number");
                GPU_MonitorConType.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_connect_type");
                GPU_MonitorManfDate.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_manf_date");
                GPU_MonitorManfDateWeek.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_manf_date_week");
                GPU_MonitorHID.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_hardware_id");
                GPU_MonitorResLabel.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_resolution");
                GPU_MonitorVirtualRes.Text = software_lang.TSReadLangs("Gpu", "gpu_virtual_resolution");
                GPU_MonitorBounds.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_limit");
                GPU_MonitorWorking.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_work_area_size");
                GPU_ScreenRefreshRate.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_refresh_rate");
                GPU_ScreenBit.Text = software_lang.TSReadLangs("Gpu", "gpu_monitor_bit_rate");
                GPU_MonitorPrimary.Text = software_lang.TSReadLangs("Gpu", "gpu_primary_screen");
                // DISK
                DISK_TTLP_L1.Text = software_lang.TSReadLangs("Storage", "se_t_title");
                DISK_TTLP_P1_L1.Text = software_lang.TSReadLangs("Storage", "se_t_ssd");
                DISK_TTLP_P2_L1.Text = software_lang.TSReadLangs("Storage", "se_t_hdd");
                DISK_TTLP_P3_L1.Text = software_lang.TSReadLangs("Storage", "se_t_usb");
                DISK_TTLP_P4_L1.Text = software_lang.TSReadLangs("Storage", "se_t_total");
                DISK_Selector.Text = software_lang.TSReadLangs("Storage", "se_name");
                DISK_Model.Text = software_lang.TSReadLangs("Storage", "se_model");
                DISK_Man.Text = software_lang.TSReadLangs("Storage", "se_manufacturer");
                DISK_VolumeID.Text = software_lang.TSReadLangs("Storage", "se_partition_letter");
                DISK_VolumeName.Text = software_lang.TSReadLangs("Storage", "se_partition_name");
                DISK_Firmware.Text = software_lang.TSReadLangs("Storage", "se_firmware_version");
                DISK_Serial.Text = software_lang.TSReadLangs("Storage", "se_serial");
                DISK_VolumeSerial.Text = software_lang.TSReadLangs("Storage", "se_partition_serial");
                DISK_Size.Text = software_lang.TSReadLangs("Storage", "se_size");
                DISK_FreeSpace.Text = software_lang.TSReadLangs("Storage", "se_empty_space");
                DISK_FileSystem.Text = software_lang.TSReadLangs("Storage", "se_file_system");
                DISK_FormattingType.Text = software_lang.TSReadLangs("Storage", "se_formatting_type");
                DISK_Type.Text = software_lang.TSReadLangs("Storage", "se_type");
                DISK_DriveType.Text = software_lang.TSReadLangs("Storage", "se_drive_type");
                DISK_InterFace.Text = software_lang.TSReadLangs("Storage", "se_interface_type");
                DISK_PartitionCount.Text = software_lang.TSReadLangs("Storage", "se_partition_count");
                DISK_MediaLoaded.Text = software_lang.TSReadLangs("Storage", "se_work_status");
                DISK_MediaStatus.Text = software_lang.TSReadLangs("Storage", "se_status");
                DISK_Health.Text = software_lang.TSReadLangs("Storage", "se_health");
                DISK_Boot.Text = software_lang.TSReadLangs("Storage", "se_primary_disk");
                DISK_Bootable.Text = software_lang.TSReadLangs("Storage", "se_bootable_disk");
                DISK_BitLockerStatus.Text = software_lang.TSReadLangs("Storage", "se_bitlocker_status");
                DISK_BitLockerConversionStatus.Text = software_lang.TSReadLangs("Storage", "se_bitlocker_encrypt_status");
                DISK_BitLockerEncryptMehod.Text = software_lang.TSReadLangs("Storage", "se_bitlocker_encrypt_method");
                DISK_DriveCompressed.Text = software_lang.TSReadLangs("Storage", "se_compress_status");
                // NETWORK
                NET_LT_Device.Text = software_lang.TSReadLangs("Network", "nk_live_speed_adapter");
                NET_LT_BandWidth.Text = software_lang.TSReadLangs("Network", "nk_live_speed_band_width");
                NET_LT_LocalIP.Text = software_lang.TSReadLangs("Network", "nk_local_ip_address");
                NET_LT_GatewayIP.Text = software_lang.TSReadLangs("Network", "nk_gateway_ip_address");
                if (!string.IsNullOrEmpty(NET_LT_GatewayIP_V.Text.Trim())){
                    MainToolTip.SetToolTip(NET_RotateGateway, software_lang.TSReadLangs("Network", "nk_gateway_ip_hover"));
                }
                NET_LT_DL1.Text = software_lang.TSReadLangs("Network", "nk_live_speed_download");
                NET_LT_UL1.Text = software_lang.TSReadLangs("Network", "nk_live_speed_upload");
                // 
                NET_Selector.Text = software_lang.TSReadLangs("Network", "nk_network_device");
                NET_MacAdress.Text = software_lang.TSReadLangs("Network", "nk_mac_adress");
                NET_NetMan.Text = software_lang.TSReadLangs("Network", "nk_driver_provider");
                NET_DriverVersion.Text = software_lang.TSReadLangs("Network", "nk_driver_version");
                NET_DriverDate.Text = software_lang.TSReadLangs("Network", "nk_driver_date");
                NET_ServiceName.Text = software_lang.TSReadLangs("Network", "nk_service_name");
                NET_AdapterType.Text = software_lang.TSReadLangs("Network", "nk_adaptor_type");
                NET_Physical.Text = software_lang.TSReadLangs("Network", "nk_physical_adaptor");
                NET_DeviceID.Text = software_lang.TSReadLangs("Network", "nk_network_hardware_id");
                NET_Guid.Text = software_lang.TSReadLangs("Network", "nk_guid");
                NET_ConnectionType.Text = software_lang.TSReadLangs("Network", "nk_connection_type");
                NET_Dhcp_status.Text = software_lang.TSReadLangs("Network", "nk_dhcp_status");
                NET_Dhcp_server.Text = software_lang.TSReadLangs("Network", "nk_dhcp_server");
                NET_DHCPFirstIpTime.Text = software_lang.TSReadLangs("Network", "nk_dhcp_first_ip_time");
                NET_DHCPLastIpTime.Text = software_lang.TSReadLangs("Network", "nk_dhcp_last_ip_time");
                NET_LocalConSpeed.Text = software_lang.TSReadLangs("Network", "nk_connection_speed");
                NET_IPv4Adress.Text = software_lang.TSReadLangs("Network", "nk_appointed_ipv4_adress");
                NET_IPv6Adress.Text = software_lang.TSReadLangs("Network", "nk_appointed_ipv6_adress");
                NET_DNS1.Text = software_lang.TSReadLangs("Network", "nk_dns1");
                NET_DNS2.Text = software_lang.TSReadLangs("Network", "nk_dns2");
                // USB
                USB_Selector.Text = software_lang.TSReadLangs("Usb", "usb_controller");
                USB_ConName.Text = software_lang.TSReadLangs("Usb", "usb_controller_name");
                USB_ConMan.Text = software_lang.TSReadLangs("Usb", "usb_controller_publisher");
                USB_ConDeviceID.Text = software_lang.TSReadLangs("Usb", "usb_controller_hardware_id");
                USB_ConPNPDeviceID.Text = software_lang.TSReadLangs("Usb", "usb_controller_pnp_hardware_id");
                USB_ConDeviceStatus.Text = software_lang.TSReadLangs("Usb", "usb_controller_status");
                USB_DeviceSelector.Text = software_lang.TSReadLangs("Usb", "usb_device");
                USB_DeviceName.Text = software_lang.TSReadLangs("Usb", "usb_device_name");
                USB_DeviceMan.Text = software_lang.TSReadLangs("Usb", "usb_device_manufacturer");
                USB_DriverVersion.Text = software_lang.TSReadLangs("Usb", "usb_device_driver_version");
                USB_DriverDate.Text = software_lang.TSReadLangs("Usb", "usb_device_driver_date");
                USB_InfFile.Text = software_lang.TSReadLangs("Usb", "usb_device_inf_file");
                USB_DeviceID.Text = software_lang.TSReadLangs("Usb", "usb_device_device_id");
                USB_HardwareID.Text = software_lang.TSReadLangs("Usb", "usb_device_hardware_id");
                USB_DeviceGUID.Text = software_lang.TSReadLangs("Usb", "usb_device_guid");
                // SOUND
                SOUND_Selector.Text = software_lang.TSReadLangs("Sound", "sound_device");
                SOUND_DeviceName.Text = software_lang.TSReadLangs("Sound", "sound_device_name");
                SOUND_DeviceManufacturer.Text = software_lang.TSReadLangs("Sound", "sound_device_publisher");
                SOUND_DriverVersion.Text = software_lang.TSReadLangs("Sound", "sound_device_version");
                SOUND_DriverDate.Text = software_lang.TSReadLangs("Sound", "sound_device_date");
                SOUND_DeviceID.Text = software_lang.TSReadLangs("Sound", "sound_device_hardware_id");
                SOUND_PNPDeviceID.Text = software_lang.TSReadLangs("Sound", "sound_device_pnp_hardware_id");
                SOUND_DeviceStatus.Text = software_lang.TSReadLangs("Sound", "sound_device_status");
                // BATTERY
                BATTERY_Status.Text = software_lang.TSReadLangs("Battery", "by_status");
                BATTERY_Model.Text = software_lang.TSReadLangs("Battery", "by_model");
                BATTERY_Serial.Text = software_lang.TSReadLangs("Battery", "by_serial");
                BATTERY_Chemistry.Text = software_lang.TSReadLangs("Battery", "by_chemistry");
                BATTERY_DesignCapacity.Text = software_lang.TSReadLangs("Battery", "by_design_capacity");
                BATTERY_FullChargeCapacity.Text = software_lang.TSReadLangs("Battery", "by_full_charge_capacity");
                BATTERY_RemainingChargeCapacity.Text = software_lang.TSReadLangs("Battery", "by_remaining_charge_capacity");
                BATTERY_Voltage.Text = software_lang.TSReadLangs("Battery", "by_voltage");
                BATTERY_ChargePower.Text = software_lang.TSReadLangs("Battery", "by_charge_power");
                BATTERY_ChargeCurrent.Text = software_lang.TSReadLangs("Battery", "by_charge_current");
                BATTERY_DeChargePower.Text = software_lang.TSReadLangs("Battery", "by_decharge_power");
                BATTERY_DeChargeCurrent.Text = software_lang.TSReadLangs("Battery", "by_decharge_current");
                BATTERY_ReportBtn.Text = " " + software_lang.TSReadLangs("Battery", "by_report_btn");
                TS_AdjustButtonWidth(BATTERY_ReportBtn); // Dynamic Width
                // OSD
                OSD_DataMainTable.Columns[0].HeaderText = software_lang.TSReadLangs("Osd", "osd_file_path");
                OSD_DataMainTable.Columns[1].HeaderText = software_lang.TSReadLangs("Osd", "osd_file_name");
                OSD_DataMainTable.Columns[2].HeaderText = software_lang.TSReadLangs("Osd", "osd_driver_name");
                OSD_DataMainTable.Columns[3].HeaderText = software_lang.TSReadLangs("Osd", "osd_driver_type");
                OSD_DataMainTable.Columns[4].HeaderText = software_lang.TSReadLangs("Osd", "osd_start");
                OSD_DataMainTable.Columns[5].HeaderText = software_lang.TSReadLangs("Osd", "osd_status");
                OSD_SearchDriverLabel.Text = software_lang.TSReadLangs("Osd", "osd_search_driver");
                OSD_TYSS.Text = software_lang.TSReadLangs("Osd", "osd_installed_driver_count");
                OSD_SortMode.Text = software_lang.TSReadLangs("Osd", "osd_order_in_reverse");
                // SERVICES
                SERVICE_DataMainTable.Columns[0].HeaderText = software_lang.TSReadLangs("Services", "ss_file_path");
                SERVICE_DataMainTable.Columns[1].HeaderText = software_lang.TSReadLangs("Services", "ss_file_name");
                SERVICE_DataMainTable.Columns[2].HeaderText = software_lang.TSReadLangs("Services", "ss_service_name");
                SERVICE_DataMainTable.Columns[3].HeaderText = software_lang.TSReadLangs("Services", "ss_service_type");
                SERVICE_DataMainTable.Columns[4].HeaderText = software_lang.TSReadLangs("Services", "ss_start");
                SERVICE_DataMainTable.Columns[5].HeaderText = software_lang.TSReadLangs("Services", "ss_status");
                SERVICE_SearchLabel.Text = software_lang.TSReadLangs("Services", "ss_search_service");
                SERVICE_TYS.Text = software_lang.TSReadLangs("Services", "ss_installed_service_count");
                SERVICE_SortMode.Text = software_lang.TSReadLangs("Services", "ss_order_in_reverse");
                // INSTALLED APPS
                INSTAPPS_DataMainTable.Columns[0].HeaderText = software_lang.TSReadLangs("Instapps", "ia_apps_icon");
                INSTAPPS_DataMainTable.Columns[1].HeaderText = software_lang.TSReadLangs("Instapps", "ia_apps_name");
                INSTAPPS_DataMainTable.Columns[2].HeaderText = software_lang.TSReadLangs("Instapps", "ia_apps_developer");
                INSTAPPS_DataMainTable.Columns[3].HeaderText = software_lang.TSReadLangs("Instapps", "ia_apps_date");
                INSTAPPS_DataMainTable.Columns[4].HeaderText = software_lang.TSReadLangs("Instapps", "ia_apps_size");
                INSTAPPS_DataMainTable.Columns[5].HeaderText = software_lang.TSReadLangs("Instapps", "ia_apps_version");
                INSTAPPS_DataMainTable.Columns[6].HeaderText = software_lang.TSReadLangs("Instapps", "ia_apps_path");
                INSTAPPS_SearchAppsLabel.Text = software_lang.TSReadLangs("Instapps", "ia_search_apps");
                INSTAPPS_TYUS.Text = software_lang.TSReadLangs("Instapps", "ia_installed_apps_count");
                INSTAPPS_SortMode.Text = software_lang.TSReadLangs("Instapps", "ia_order_in_reverse");
                iapps_unknown = software_lang.TSReadLangs("Instapps_Content", "ia_unknown");
                // EXPORT
                EXPORT_Selector.Text = software_lang.TSReadLangs("Export", "e_mode_title");
                EXPORT_StartEngineBtn.Text = " " + software_lang.TSReadLangs("Export", "e_mode_report_start");
                EXPORT_DonateLabel.Text = software_lang.TSReadLangs("ExportSupport", "es_text");
                EXPORT_Donate.Text = " " + " " + software_lang.TSReadLangs("ExportSupport", "es_btn_text");
                TS_AdjustButtonWidth(EXPORT_StartEngineBtn); // Dynamic Width
                TS_AdjustButtonWidth(EXPORT_Donate); // Dynamic Width
                // CHANGE COPYABLE LABELS TOOLTIP TEXT
                this.BeginInvoke(new MethodInvoker(() => { TSSetTooltips(allCopyableLabels); }));
                // OTHER PAGE DYNAMIC UI
                Glow_other_page_dynamic_ui();
            }catch (Exception){ }
        }
        // THEME SETTINGS
        // ======================================================================================================
        private ToolStripMenuItem selected_theme = null;
        private void Select_theme_active(object target_theme){
            if (target_theme == null)
                return;
            ToolStripMenuItem clicked_theme = (ToolStripMenuItem)target_theme;
            if (selected_theme == clicked_theme)
                return;
            Select_theme_deactive();
            selected_theme = clicked_theme;
            selected_theme.Checked = true;
        }
        private void Select_theme_deactive(){
            foreach (ToolStripMenuItem theme in themeToolStripMenuItem.DropDownItems){
                theme.Checked = false;
            }
        }
        // THEME SWAP
        // ======================================================================================================
        private void SystemThemeToolStripMenuItem_Click(object sender, EventArgs e){
            themeSystem = 2; Theme_engine(GetSystemTheme(2)); SaveTheme(2); Select_theme_active(sender);
        }
        private void LightThemeToolStripMenuItem_Click(object sender, EventArgs e){
            themeSystem = 0; Theme_engine(1); SaveTheme(1); Select_theme_active(sender); 
        }
        private void DarkThemeToolStripMenuItem_Click(object sender, EventArgs e){
            themeSystem = 0; Theme_engine(0); SaveTheme(0); Select_theme_active(sender); 
        }
        private void TSUseSystemTheme(){ if (themeSystem == 2) Theme_engine(GetSystemTheme(2)); }
        private void SaveTheme(int ts){
            // SAVE CURRENT THEME
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "ThemeStatus", Convert.ToString(ts));
            }catch (Exception){ }
        }
        // THEME ENGINE
        // ======================================================================================================
        private void Theme_engine(int ts){
            try{
                theme = ts;
                //
                TSThemeModeHelper.SetThemeMode(ts == 0);
                TSThemeModeHelper.InitializeThemeForForm(this);
                //
                if (theme == 1){
                    // LEFT MENU LOGO CHANGE
                    if (windows_mode == 1){
                        TSImageRenderer(OS_RotateBtn, Properties.Resources.lm_os_w11_light, 18, ContentAlignment.MiddleLeft);
                    }else{
                        TSImageRenderer(OS_RotateBtn, Properties.Resources.lm_os_w10_light, 18, ContentAlignment.MiddleLeft);
                    }
                    TSImageRenderer(MB_RotateBtn, Properties.Resources.lm_mb_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(CPU_RotateBtn, Properties.Resources.lm_cpu_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(RAM_RotateBtn, Properties.Resources.lm_ram_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(GPU_RotateBtn, Properties.Resources.lm_gpu_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(DISK_RotateBtn, Properties.Resources.lm_disk_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(NET_RotateBtn, Properties.Resources.lm_net_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(USB_RotateBtn, Properties.Resources.lm_usb_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(SOUND_RotateBtn, Properties.Resources.lm_sound_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(BATTERY_RotateBtn, Properties.Resources.lm_battery_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(OSD_RotateBtn, Properties.Resources.lm_idrivers_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(SERVICES_RotateBtn, Properties.Resources.lm_iservices_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(INSTALLED_RotateBtn, Properties.Resources.lm_iapps_light, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(PRINT_RotateBtn, Properties.Resources.lm_export_light, 18, ContentAlignment.MiddleLeft);
                    // TOP MENU LOGO CHANGE
                    TSImageRenderer(settingsToolStripMenuItem, Properties.Resources.tm_settings_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(themeToolStripMenuItem, Properties.Resources.tm_theme_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(languageToolStripMenuItem, Properties.Resources.tm_language_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(startupToolStripMenuItem, Properties.Resources.tm_startup_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(hidingModeToolStripMenuItem, Properties.Resources.tm_hidden_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(checkForUpdatesToolStripMenuItem, Properties.Resources.tm_update_light, 0, ContentAlignment.MiddleRight);
                    // TOOLS
                    TSImageRenderer(toolsToolStripMenuItem, Properties.Resources.tm_tools_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(sFCandDISMAutoTool, Properties.Resources.cx_sfc_and_dism_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(cacheCleaningTool, Properties.Resources.cx_cache_clean_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(benchCPUTool, Properties.Resources.cx_bench_cpu_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(benchRAMTool, Properties.Resources.cx_bench_ram_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(benchDiskTool, Properties.Resources.cx_bench_disk_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(screenOverlayTool, Properties.Resources.cx_overlay_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(dnsTestTool, Properties.Resources.cx_dns_test_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(quickAccessTool, Properties.Resources.cx_quick_access_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(networkFixTool, Properties.Resources.cx_network_fix_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(showWiFiPasswordTool, Properties.Resources.cx_swpt_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(bluetoothFinderToolToolStripMenuItem, Properties.Resources.cx_bt_finder_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(systemIdGeneratorTool, Properties.Resources.cx_system_id_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(monitorTestTool, Properties.Resources.cx_test_monitor_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(monitorDeadPixelTestTool, Properties.Resources.cx_test_dead_pixel_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(monitorDynamicRangeTestTool, Properties.Resources.cx_test_dynamic_range_light, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(monitorStuckPixelFixerTool, Properties.Resources.cx_stuck_pixel_fixer_light, 0, ContentAlignment.MiddleRight);
                    // MIDDLE CONTENT LOGO CHANGE
                    TSImageRenderer(OS_MinidumpOpen, Properties.Resources.ct_link_light, 0, ContentAlignment.MiddleCenter);
                    TSImageRenderer(OS_BSoDZIP, Properties.Resources.ct_zip_light, 0, ContentAlignment.MiddleCenter);
                    TSImageRenderer(OS_WallpaperOpen, Properties.Resources.ct_link_light, 0, ContentAlignment.MiddleCenter);
                    TSImageRenderer(OS_WallpaperPreview, Properties.Resources.ct_image_preview_light, 0, ContentAlignment.MiddleCenter);
                    //
                    TSImageRenderer(MB_BIOSUpdateBtn, Properties.Resources.ct_mb_update_light, 16, ContentAlignment.MiddleRight);
                    //
                    TSImageRenderer(NET_RotateGateway, Properties.Resources.ct_link_light, 0, ContentAlignment.MiddleCenter);
                    //
                    TSImageRenderer(BATTERY_ReportBtn, Properties.Resources.ct_battery_report_light, 18, ContentAlignment.MiddleRight);
                    TSImageRenderer(EXPORT_StartEngineBtn, Properties.Resources.ct_export_light, 18, ContentAlignment.MiddleRight);
                    TSImageRenderer(EXPORT_Donate, Properties.Resources.tm_donate_mc_light, 18, ContentAlignment.MiddleRight);
                    //
                    TSImageRenderer(INSTAPPS_TextBoxClearBtn, Properties.Resources.ct_clear_light, 15, ContentAlignment.MiddleCenter);
                    TSImageRenderer(SERVICE_TextBoxClearBtn, Properties.Resources.ct_clear_light, 15, ContentAlignment.MiddleCenter);
                    TSImageRenderer(OSD_TextBoxClearBtn, Properties.Resources.ct_clear_light, 15, ContentAlignment.MiddleCenter);
                    // DONATE
                    TSImageRenderer(donateToolStripMenuItem, Properties.Resources.tm_donate_light, 0, ContentAlignment.MiddleRight);
                    // TS WIZARD
                    TSImageRenderer(tSWizardToolStripMenuItem, Properties.Resources.tm_ts_wizard_light, 0, ContentAlignment.MiddleRight);
                    // ABOUT
                    TSImageRenderer(aboutToolStripMenuItem, Properties.Resources.tm_about_light, 0, ContentAlignment.MiddleRight);
                }else if (theme == 0){
                    // LEFT MENU LOGO CHANGE
                    if (windows_mode == 1){
                        TSImageRenderer(OS_RotateBtn, Properties.Resources.lm_os_w11_dark, 18, ContentAlignment.MiddleLeft);
                    }else{
                        TSImageRenderer(OS_RotateBtn, Properties.Resources.lm_os_w10_dark, 18, ContentAlignment.MiddleLeft);
                    }
                    TSImageRenderer(MB_RotateBtn, Properties.Resources.lm_mb_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(CPU_RotateBtn, Properties.Resources.lm_cpu_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(RAM_RotateBtn, Properties.Resources.lm_ram_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(GPU_RotateBtn, Properties.Resources.lm_gpu_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(DISK_RotateBtn, Properties.Resources.lm_disk_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(NET_RotateBtn, Properties.Resources.lm_net_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(USB_RotateBtn, Properties.Resources.lm_usb_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(SOUND_RotateBtn, Properties.Resources.lm_sound_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(BATTERY_RotateBtn, Properties.Resources.lm_battery_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(OSD_RotateBtn, Properties.Resources.lm_idrivers_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(SERVICES_RotateBtn, Properties.Resources.lm_iservices_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(INSTALLED_RotateBtn, Properties.Resources.lm_iapps_dark, 18, ContentAlignment.MiddleLeft);
                    TSImageRenderer(PRINT_RotateBtn, Properties.Resources.lm_export_dark, 18, ContentAlignment.MiddleLeft);
                    // TOP MENU LOGO CHANGE
                    TSImageRenderer(settingsToolStripMenuItem, Properties.Resources.tm_settings_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(themeToolStripMenuItem, Properties.Resources.tm_theme_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(languageToolStripMenuItem, Properties.Resources.tm_language_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(startupToolStripMenuItem, Properties.Resources.tm_startup_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(hidingModeToolStripMenuItem, Properties.Resources.tm_hidden_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(checkForUpdatesToolStripMenuItem, Properties.Resources.tm_update_dark, 0, ContentAlignment.MiddleRight);
                    // TOOLS
                    TSImageRenderer(toolsToolStripMenuItem, Properties.Resources.tm_tools_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(sFCandDISMAutoTool, Properties.Resources.cx_sfc_and_dism_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(cacheCleaningTool, Properties.Resources.cx_cache_clean_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(benchCPUTool, Properties.Resources.cx_bench_cpu_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(benchRAMTool, Properties.Resources.cx_bench_ram_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(benchDiskTool, Properties.Resources.cx_bench_disk_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(screenOverlayTool, Properties.Resources.cx_overlay_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(dnsTestTool, Properties.Resources.cx_dns_test_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(quickAccessTool, Properties.Resources.cx_quick_access_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(networkFixTool, Properties.Resources.cx_network_fix_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(showWiFiPasswordTool, Properties.Resources.cx_swpt_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(bluetoothFinderToolToolStripMenuItem, Properties.Resources.cx_bt_finder_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(systemIdGeneratorTool, Properties.Resources.cx_system_id_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(monitorTestTool, Properties.Resources.cx_test_monitor_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(monitorDeadPixelTestTool, Properties.Resources.cx_test_dead_pixel_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(monitorDynamicRangeTestTool, Properties.Resources.cx_test_dynamic_range_dark, 0, ContentAlignment.MiddleRight);
                    TSImageRenderer(monitorStuckPixelFixerTool, Properties.Resources.cx_stuck_pixel_fixer_dark, 0, ContentAlignment.MiddleRight);
                    // MIDDLE CONTENT LOGO CHANGE
                    TSImageRenderer(OS_MinidumpOpen, Properties.Resources.ct_link_dark, 0, ContentAlignment.MiddleCenter);
                    TSImageRenderer(OS_BSoDZIP, Properties.Resources.ct_zip_dark, 0, ContentAlignment.MiddleCenter);
                    TSImageRenderer(OS_WallpaperOpen, Properties.Resources.ct_link_dark, 0, ContentAlignment.MiddleCenter);
                    TSImageRenderer(OS_WallpaperPreview, Properties.Resources.ct_image_preview_dark, 0, ContentAlignment.MiddleCenter);
                    //
                    TSImageRenderer(MB_BIOSUpdateBtn, Properties.Resources.ct_mb_update_dark, 16, ContentAlignment.MiddleRight);
                    //
                    TSImageRenderer(NET_RotateGateway, Properties.Resources.ct_link_dark, 0, ContentAlignment.MiddleCenter);
                    //
                    TSImageRenderer(BATTERY_ReportBtn, Properties.Resources.ct_battery_report_dark, 18, ContentAlignment.MiddleRight);
                    TSImageRenderer(EXPORT_StartEngineBtn, Properties.Resources.ct_export_dark, 18, ContentAlignment.MiddleRight);
                    TSImageRenderer(EXPORT_Donate, Properties.Resources.tm_donate_mc_dark, 18, ContentAlignment.MiddleRight);
                    //
                    TSImageRenderer(INSTAPPS_TextBoxClearBtn, Properties.Resources.ct_clear_dark, 15, ContentAlignment.MiddleCenter);
                    TSImageRenderer(SERVICE_TextBoxClearBtn, Properties.Resources.ct_clear_dark, 15, ContentAlignment.MiddleCenter);
                    TSImageRenderer(OSD_TextBoxClearBtn, Properties.Resources.ct_clear_dark, 15, ContentAlignment.MiddleCenter);
                    // DONATE
                    TSImageRenderer(donateToolStripMenuItem, Properties.Resources.tm_donate_dark, 0, ContentAlignment.MiddleRight);
                    // TS WIZARD
                    TSImageRenderer(tSWizardToolStripMenuItem, Properties.Resources.tm_ts_wizard_dark, 0, ContentAlignment.MiddleRight);
                    // ABOUT
                    TSImageRenderer(aboutToolStripMenuItem, Properties.Resources.tm_about_dark, 0, ContentAlignment.MiddleRight);
                }
                // OTHER PAGE DYNAMIC UI
                Glow_other_page_dynamic_ui();
                // HEADER
                Header_image_reloader(menu_btns);
                header_colors[0] = TS_ThemeEngine.ColorMode(theme, "HeaderBGColorMain");
                header_colors[1] = TS_ThemeEngine.ColorMode(theme, "HeaderFEColorMain");
                header_colors[2] = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                HeaderMenu.Renderer = new HeaderMenuColors();
                // ACTIVE BTN 
                btn_colors_active[0] = TS_ThemeEngine.ColorMode(theme, "BtnActiveColor");
                // TOOLTIP
                MainToolTip.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                MainToolTip.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                // HEADER MENU
                var bg = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                var fg = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                Header_InPanel.BackColor = bg;
                HeaderText.ForeColor = fg;
                HeaderMenu.ForeColor = fg;
                HeaderMenu.BackColor = bg;
                SetMenuStripColors(HeaderMenu, bg, fg);
                // LEFT MENU
                LeftMenuPanel.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                OS_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                MB_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                CPU_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                RAM_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                GPU_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                DISK_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                NET_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                USB_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                SOUND_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                BATTERY_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                OSD_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                SERVICES_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                INSTALLED_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                PRINT_RotateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                // LEFT MENU BORDER
                OS_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                MB_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                CPU_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                RAM_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                GPU_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                DISK_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                NET_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                USB_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                SOUND_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                BATTERY_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                OSD_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                SERVICES_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                INSTALLED_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                PRINT_RotateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuBGAndBorderColor");
                // LEFT MENU MOUSE HOVER
                OS_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                MB_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                CPU_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                RAM_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                GPU_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                DISK_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                NET_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                USB_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                SOUND_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                BATTERY_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                OSD_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                SERVICES_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                INSTALLED_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                PRINT_RotateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                // LEFT MENU MOUSE DOWN
                OS_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                MB_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                CPU_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                RAM_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                GPU_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                DISK_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                NET_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                USB_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                SOUND_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                BATTERY_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                OSD_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                SERVICES_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                INSTALLED_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                PRINT_RotateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                // LEFT MENU BUTTON TEXT COLOR
                OS_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                MB_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                CPU_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                RAM_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                GPU_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                DISK_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                NET_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                USB_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                SOUND_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                BATTERY_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                OSD_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                SERVICES_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                INSTALLED_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                PRINT_RotateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonFEColor");
                // CONTENT BG
                BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                OS.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                MB.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                CPU.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                RAM.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                GPU.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                DISK.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                NETWORK.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                USB.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                SOUND.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                BATTERY.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                DRIVERS.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                SERVICES.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                INSTAPPS.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                EXPORT.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                // OS
                os_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_3.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_4.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_5.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_6.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_7.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                //
                OS_SystemUser.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemUser_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_ComputerName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_ComputerName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_SavedUser.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SavedUser_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_Name.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Name_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_Manufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Manufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_SystemVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_SystemArchitectural.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemArchitectural_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_DeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_DeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_Serial.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Serial_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_Country.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Country_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_TimeZone.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_TimeZone_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_EncryptionType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_EncryptionType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_SystemTime.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemTime_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_Install.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Install_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_SystemWorkTime.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemWorkTime_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_LastBootTime.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_LastBootTime_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_SystemLastShutDown.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemLastShutDown_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_PrimaryOS.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_PrimaryOS_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_PortableOS.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_PortableOS_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_MouseWheelStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_MouseWheelStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_ScrollLockStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_ScrollLockStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_NumLockStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_NumLockStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_CapsLockStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_CapsLockStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_FastBoot.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_FastBoot_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_WinPageFile.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_WinPageFile_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_TempWinPageFile.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_TempWinPageFile_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_Hiberfil.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Hiberfil_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_AVProgram.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_AVProgram_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_FirewallProgram.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_FirewallProgram_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_AntiSpywareProgram.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_AntiSpywareProgram_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_WinDefCoreIsolation.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_WinDefCoreIsolation_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_CA2023_Status.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_CA2023_Status_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_CA2023_Capable.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_CA2023_Capable_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_CA2023_Error.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_CA2023_Error_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_ActivePower.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_ActivePower_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_ActivePowerGUID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_ActivePowerGUID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_ActivePowerScreenTimeOutP.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_ActivePowerScreenTimeOutP_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_ActivePowerScreenTimeOutB.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_ActivePowerScreenTimeOutB_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_ActivePowerSleepTimeP.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_ActivePowerSleepTimeP_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_ActivePowerSleepTimeB.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_ActivePowerSleepTimeB_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_MSEdge.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_MSEdge_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_MSEdgeWebView.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_MSEdgeWebView_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_MSStoreVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_MSStoreVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_MSOfficeVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_MSOfficeVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_WinKey.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_WinKey_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_WinActiveChannel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_WinActiveChannel_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_NETFrameworkVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_NETFrameworkVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_Minidump.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Minidump_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_BSODDate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_BSODDate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OS_Wallpaper.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Wallpaper_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                // MB
                mb_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                mb_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                mb_panel_3.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                //
                MB_BIOSUpdateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "DynamicThemeActiveBtnBG");
                MB_BIOSUpdateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_BIOSUpdateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_BIOSUpdateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_BIOSUpdateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColorHover");
                //
                MB_MotherBoardName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_MotherBoardName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_MotherBoardMan.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_MotherBoardMan_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_SystemModelMan.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SystemModelMan_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_SystemModelFamily.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SystemModelFamily_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_SystemFamily.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SystemFamily_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_SystemModel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SystemModel_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_DeviceSerialNumber.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_DeviceSerialNumber_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_MotherBoardSerial.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_MotherBoardSerial_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_SystemSKU.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SystemSKU_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_Chipset.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_Chipset_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_BiosManufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_BiosManufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_BiosDate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_BiosDate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_BiosVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_BiosVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_SmBiosVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SmBiosVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_BiosMode.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_BiosMode_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_LastBIOSTime.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_LastBIOSTime_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_SecureBoot.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SecureBoot_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_SecureBootCA2023.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SecureBootCA2023_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_TPMStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_TPMPhysicalVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMPhysicalVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_TPMMan.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMMan_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_TPMManID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMManID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_TPMManVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMManVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_TPMManFullVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMManFullVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                MB_TPMManPublisher.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMManPublisher_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                // CPU
                cpu_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                cpu_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                //
                CPU_Name.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Name_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_Manufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Manufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_Architectural.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Architectural_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_IntelME.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_IntelME_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_NormalSpeed.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_NormalSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_DefaultSpeed.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_DefaultSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_L1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_L1_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_L2.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_L2_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_L3.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_L3_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_CoreCount.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_CoreCount_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_LogicalCore.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_LogicalCore_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_Usage.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Usage_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_Process.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Process_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_Threads.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Threads_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_Handles.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Handles_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_SocketDefinition.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_SocketDefinition_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_Family.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Family_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_Virtualization.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Virtualization_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_HyperV.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_HyperV_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_VMExtension.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_VMExtension_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                CPU_SerialName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_SerialName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                // RAM
                ram_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                ram_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                //
                RAM_TotalRAM.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_TotalRAM_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_UsageRAMCount.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_UsageRAMCount_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_EmptyRamCount.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_EmptyRamCount_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_TotalVirtualRam.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_TotalVirtualRam_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_UsageVirtualRam.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_UsageVirtualRam_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_EmptyVirtualRam.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_EmptyVirtualRam_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_SlotStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_SlotStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                //
                RAM_Selector.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Selector_List.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                RAM_Selector_List.ForeColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                RAM_Selector_List.HoverBackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                RAM_Selector_List.ButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                RAM_Selector_List.ArrowColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                RAM_Selector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                RAM_Selector_List.BorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                RAM_Selector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                //
                RAM_Amount.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Amount_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_Type.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Type_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_Frequency.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Frequency_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_Volt.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Volt_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_FormFactor.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_FormFactor_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_Serial.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Serial_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_Manufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Manufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_BankLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_BankLabel_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_DataWidth.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_DataWidth_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_BellekType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_BellekType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_PartNumber.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_PartNumber_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                //
                RAM_ProgressBGPanel.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                RAM_ProgressFEPanel.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                RAM_ProgressLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                // GPU
                gpu_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                gpu_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                //
                GPU_Selector.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_Selector_List.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                GPU_Selector_List.ForeColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                GPU_Selector_List.HoverBackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                GPU_Selector_List.ButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                GPU_Selector_List.ArrowColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                GPU_Selector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                GPU_Selector_List.BorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                GPU_Selector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                //
                GPU_Manufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_Manufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_VRAM.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_VRAM_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_Version.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_Version_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_DriverDate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_DriverDate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_Status.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_Status_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_DeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_DeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_DacType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_DacType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_GraphicDriversName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_GraphicDriversName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_InfFileName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_InfFileName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_INFSectionFile.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_INFSectionFile_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_CurrentColor.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_CurrentColor_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                //
                GPU_MonitorSelector.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorSelector_List.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                GPU_MonitorSelector_List.ForeColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                GPU_MonitorSelector_List.HoverBackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                GPU_MonitorSelector_List.ButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                GPU_MonitorSelector_List.ArrowColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                GPU_MonitorSelector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                GPU_MonitorSelector_List.BorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                GPU_MonitorSelector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                //
                GPU_MonitorUserFriendlyName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorUserFriendlyName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorManName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorManName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorProductCodeID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorProductCodeID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorSerialNumberID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorSerialNumberID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorConType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorConType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorManfDate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorManfDate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorManfDateWeek.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorManfDateWeek_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorHID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorHID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorResLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorResLabel_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorVirtualRes.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorVirtualRes_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorBounds.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorBounds_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorWorking.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorWorking_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_ScreenRefreshRate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_ScreenRefreshRate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_ScreenBit.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_ScreenBit_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                GPU_MonitorPrimary.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorPrimary_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                // DISK
                disk_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                disk_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                disk_panel_3.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                //
                DISK_TTLP_L1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_TTLP_P1_L1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_TTLP_P2_L1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_TTLP_P3_L1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_TTLP_P4_L1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                //
                DISK_TLP_PB_1.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentBlue");
                DISK_TLP_PB_2.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentPurple");
                DISK_TLP_PB_3.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentRed");
                DISK_TLP_PB_4.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentGreen");
                DISK_TTLP_P1_L2.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentBlue");
                DISK_TTLP_P2_L2.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentPurple");
                DISK_TTLP_P3_L2.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentRed");
                DISK_TTLP_P4_L2.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentGreen");
                //
                DISK_TTLP_Panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                DISK_TTLP_Panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                DISK_TTLP_Panel_3.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                DISK_TTLP_Panel_4.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                //
                DISK_PBar_BG.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                DISK_PBar_FE.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_PBar_Label.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                //
                DISK_Selector.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Selector_List.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                DISK_Selector_List.ForeColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                DISK_Selector_List.HoverBackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                DISK_Selector_List.ButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                DISK_Selector_List.ArrowColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                DISK_Selector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                DISK_Selector_List.BorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                DISK_Selector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                //
                DISK_Model.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Model_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_Man.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Man_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_VolumeID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_VolumeID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_VolumeName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_VolumeName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_Firmware.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Firmware_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_Serial.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Serial_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_VolumeSerial.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_VolumeSerial_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_Size.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Size_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_FreeSpace.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_FreeSpace_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_FileSystem.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_FileSystem_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_FormattingType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_FormattingType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_Type.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Type_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_DriveType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_DriveType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_InterFace.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_InterFace_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_PartitionCount.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_PartitionCount_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_MediaLoaded.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_MediaLoaded_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_MediaStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_MediaStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_Health.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Health_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_Boot.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Boot_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_Bootable.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Bootable_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_BitLockerStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_BitLockerStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_BitLockerConversionStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_BitLockerConversionStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_BitLockerEncryptMehod.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_BitLockerEncryptMehod_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                DISK_DriveCompressed.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_DriveCompressed_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                // NETWORK
                network_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                network_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                network_panel_3.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                //
                NET_LT_Device.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LT_Device_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_LT_BandWidth.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LT_BandWidth_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_LT_LocalIP.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LT_LocalIP_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_LT_GatewayIP.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LT_GatewayIP_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_LT_P1.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                NET_LT_P2.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                NET_LT_DL1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LT_DL2.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_LT_UL1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LT_UL2.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                //
                NET_Selector.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_Selector_List.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                NET_Selector_List.ForeColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                NET_Selector_List.HoverBackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                NET_Selector_List.ButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                NET_Selector_List.ArrowColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                NET_Selector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                NET_Selector_List.BorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                NET_Selector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                //
                NET_MacAdress.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_MacAdress_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_NetMan.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_NetMan_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_DriverVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_DriverVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_DriverDate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_DriverDate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_ServiceName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_ServiceName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_AdapterType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_AdapterType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_Physical.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_Physical_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_DeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_DeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_Guid.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_Guid_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_ConnectionType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_ConnectionType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_Dhcp_status.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_Dhcp_status_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_Dhcp_server.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_Dhcp_server_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_DHCPFirstIpTime.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_DHCPFirstIpTime_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_DHCPLastIpTime.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_DHCPLastIpTime_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_LocalConSpeed.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LocalConSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_IPv4Adress.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_IPv4Adress_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_IPv6Adress.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_IPv6Adress_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_DNS1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_DNS1_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                NET_DNS2.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_DNS2_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                // USB
                usb_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                usb_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                //
                USB_Selector.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_Selector_List.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                USB_Selector_List.ForeColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                USB_Selector_List.HoverBackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                USB_Selector_List.ButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                USB_Selector_List.ArrowColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                USB_Selector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                USB_Selector_List.BorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                USB_Selector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                //
                USB_ConName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_ConName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                USB_ConMan.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_ConMan_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                USB_ConDeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_ConDeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                USB_ConPNPDeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_ConPNPDeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                USB_ConDeviceStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_ConDeviceStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                //
                USB_DeviceSelector.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_DeviceSelector_List.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                USB_DeviceSelector_List.ForeColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                USB_DeviceSelector_List.HoverBackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                USB_DeviceSelector_List.ButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                USB_DeviceSelector_List.ArrowColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                USB_DeviceSelector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                USB_DeviceSelector_List.BorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                USB_DeviceSelector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                //
                USB_DeviceName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_DeviceName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                USB_DeviceMan.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_DeviceMan_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                USB_DriverVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_DriverVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                USB_DriverDate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_DriverDate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                USB_InfFile.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_InfFile_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                USB_DeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_DeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                USB_HardwareID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_HardwareID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                USB_DeviceGUID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_DeviceGUID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                // SOUND
                sound_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                //
                SOUND_Selector.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_Selector_List.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                SOUND_Selector_List.ForeColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                SOUND_Selector_List.HoverBackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                SOUND_Selector_List.ButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                SOUND_Selector_List.ArrowColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                SOUND_Selector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                SOUND_Selector_List.BorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                SOUND_Selector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                //
                SOUND_DeviceName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_DeviceName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                SOUND_DeviceManufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_DeviceManufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                SOUND_DriverVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_DriverVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                SOUND_DriverDate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_DriverDate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                SOUND_DeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_DeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                SOUND_PNPDeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_PNPDeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                SOUND_DeviceStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_DeviceStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                // BATTERY
                battery_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                battery_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                //
                BATTERY_Status.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_Status_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_Model.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_Model_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_Serial.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_Serial_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_Chemistry.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_Chemistry_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_DesignCapacity.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_DesignCapacity_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_FullChargeCapacity.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_FullChargeCapacity_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_RemainingChargeCapacity.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_RemainingChargeCapacity_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_Voltage.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_Voltage_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_ChargePower.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_ChargePower_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_ChargeCurrent.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_ChargeCurrent_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_DeChargePower.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_DeChargePower_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_DeChargeCurrent.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_DeChargeCurrent_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                //
                BATTERY_ReportBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "DynamicThemeActiveBtnBG");
                BATTERY_ReportBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_ReportBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_ReportBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_ReportBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColorHover");
                //
                BATTERY_PBG_Panel.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                BATTERY_PFE_Panel.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                BATTERY_ProgressLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                // INSTALLED DRIVERS
                osd_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                OSD_TextBox.BackColor = TS_ThemeEngine.ColorMode(theme, "TextBoxBGColor");
                OSD_TextBox.ForeColor = TS_ThemeEngine.ColorMode(theme, "TextBoxFEColor");
                OSD_TYSS.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OSD_TYSS_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OSD_SearchDriverLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OSD_TextBoxClearBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                OSD_TextBoxClearBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageFE");
                OSD_TextBoxClearBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OSD_TextBoxClearBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                OSD_TextBoxClearBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColorHover");
                OSD_SortMode.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OSD_DataMainTable.BackgroundColor = TS_ThemeEngine.ColorMode(theme, "DataGridBGColor");
                OSD_DataMainTable.GridColor = TS_ThemeEngine.ColorMode(theme, "DataGridColor");
                OSD_DataMainTable.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "DataGridBGColor");
                OSD_DataMainTable.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(theme, "DataGridFEColor");
                OSD_DataMainTable.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "DataGridAlternatingColor");
                OSD_DataMainTable.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                OSD_DataMainTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                OSD_DataMainTable.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageFE");
                OSD_DataMainTable.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                OSD_DataMainTable.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageFE");
                // SERVICES
                service_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                SERVICE_TextBox.BackColor = TS_ThemeEngine.ColorMode(theme, "TextBoxBGColor");
                SERVICE_TextBox.ForeColor = TS_ThemeEngine.ColorMode(theme, "TextBoxFEColor");
                SERVICE_TYS.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SERVICE_TYS_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                SERVICE_SearchLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SERVICE_TextBoxClearBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                SERVICE_TextBoxClearBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageFE");
                SERVICE_TextBoxClearBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                SERVICE_TextBoxClearBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                SERVICE_TextBoxClearBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColorHover");
                SERVICE_SortMode.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SERVICE_DataMainTable.BackgroundColor = TS_ThemeEngine.ColorMode(theme, "DataGridBGColor");
                SERVICE_DataMainTable.GridColor = TS_ThemeEngine.ColorMode(theme, "DataGridColor");
                SERVICE_DataMainTable.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "DataGridBGColor");
                SERVICE_DataMainTable.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(theme, "DataGridFEColor");
                SERVICE_DataMainTable.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "DataGridAlternatingColor");
                SERVICE_DataMainTable.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                SERVICE_DataMainTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                SERVICE_DataMainTable.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageFE");
                SERVICE_DataMainTable.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                SERVICE_DataMainTable.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageFE");
                // INSTALLED APPS
                instapps_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                INSTAPPS_TextBox.BackColor = TS_ThemeEngine.ColorMode(theme, "TextBoxBGColor");
                INSTAPPS_TextBox.ForeColor = TS_ThemeEngine.ColorMode(theme, "TextBoxFEColor");
                INSTAPPS_TYUS.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                INSTAPPS_TYUS_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                INSTAPPS_SearchAppsLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                INSTAPPS_TextBoxClearBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                INSTAPPS_TextBoxClearBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageFE");
                INSTAPPS_TextBoxClearBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                INSTAPPS_TextBoxClearBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                INSTAPPS_TextBoxClearBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColorHover");
                INSTAPPS_SortMode.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                INSTAPPS_DataMainTable.BackgroundColor = TS_ThemeEngine.ColorMode(theme, "DataGridBGColor");
                INSTAPPS_DataMainTable.GridColor = TS_ThemeEngine.ColorMode(theme, "DataGridColor");
                INSTAPPS_DataMainTable.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "DataGridBGColor");
                INSTAPPS_DataMainTable.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(theme, "DataGridFEColor");
                INSTAPPS_DataMainTable.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "DataGridAlternatingColor");
                INSTAPPS_DataMainTable.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                INSTAPPS_DataMainTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                INSTAPPS_DataMainTable.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageFE");
                INSTAPPS_DataMainTable.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                INSTAPPS_DataMainTable.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageFE");
                // PRINT
                print_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                print_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                EXPORT_Selector.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                EXPORT_Selector_List.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                EXPORT_Selector_List.ForeColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                EXPORT_Selector_List.HoverBackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                EXPORT_Selector_List.ButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                EXPORT_Selector_List.ArrowColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxFEColor");
                EXPORT_Selector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor2");
                EXPORT_Selector_List.BorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                EXPORT_Selector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBorderColor");
                EXPORT_ProgressBGPanel.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                EXPORT_ProgressFEPanel.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                EXPORT_ProgessLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                EXPORT_StartEngineBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "DynamicThemeActiveBtnBG");
                EXPORT_StartEngineBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                EXPORT_StartEngineBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                EXPORT_StartEngineBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                EXPORT_StartEngineBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColorHover");
                //
                EXPORT_Donate.ForeColor = TS_ThemeEngine.ColorMode(theme, "DynamicThemeActiveBtnBG");
                EXPORT_Donate.BackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                EXPORT_Donate.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                EXPORT_Donate.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColor");
                EXPORT_Donate.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "AccentColorHover");
                EXPORT_DonateLabel.BackColor = TS_ThemeEngine.ColorMode(theme, "LeftMenuButtonHoverAndMouseDownColor");
                EXPORT_DonateLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                // ROTATE MENU
                var buttonMap = new Dictionary<int, Button>{
                    { 1, OS_RotateBtn },
                    { 2, MB_RotateBtn },
                    { 3, CPU_RotateBtn },
                    { 4, RAM_RotateBtn },
                    { 5, GPU_RotateBtn },
                    { 6, DISK_RotateBtn },
                    { 7, NET_RotateBtn },
                    { 8, USB_RotateBtn },
                    { 9, SOUND_RotateBtn },
                    { 10, BATTERY_RotateBtn },
                    { 11, OSD_RotateBtn },
                    { 12, SERVICES_RotateBtn },
                    { 13, INSTALLED_RotateBtn },
                    { 14, PRINT_RotateBtn }
                };
                if (buttonMap.TryGetValue(menu_btns, out Button selectedButton)){
                    selectedButton.BackColor = TS_ThemeEngine.ColorMode(theme, "DynamicThemeActiveBtnBG");
                }
            }catch (Exception){ }
        }
        private void SetMenuStripColors(MenuStrip menuStrip, Color bgColor, Color fgColor){
            if (menuStrip == null) return;
            foreach (ToolStripItem item in menuStrip.Items){
                if (item is ToolStripMenuItem menuItem){
                    SetMenuItemColors(menuItem, bgColor, fgColor);
                }
            }
        }
        private void SetMenuItemColors(ToolStripMenuItem menuItem, Color bgColor, Color fgColor){
            if (menuItem == null) return;
            menuItem.BackColor = bgColor;
            menuItem.ForeColor = fgColor;
            foreach (ToolStripItem item in menuItem.DropDownItems){
                if (item is ToolStripMenuItem subMenuItem){
                    SetMenuItemColors(subMenuItem, bgColor, fgColor);
                }
            }
        }
        private void SetContextMenuColors(ContextMenuStrip contextMenu, Color bgColor, Color fgColor){
            if (contextMenu == null) return;
            foreach (ToolStripItem item in contextMenu.Items){
                if (item is ToolStripMenuItem menuItem){
                    SetMenuItemColors(menuItem, bgColor, fgColor);
                }
            }
        }
        private void Header_image_reloader(int hi_value){
            try{
                var imageMap = new Dictionary<int, string>{
                    { 1, (theme == 1) ? (windows_mode == 1 ? "lm_os_w11_light" : "lm_os_w10_light") : (windows_mode == 1 ? "lm_os_w11_dark" : "lm_os_w10_dark") },
                    { 2, theme == 1 ? "lm_mb_light" : "lm_mb_dark" },
                    { 3, theme == 1 ? "lm_cpu_light" : "lm_cpu_dark" },
                    { 4, theme == 1 ? "lm_ram_light" : "lm_ram_dark" },
                    { 5, theme == 1 ? "lm_gpu_light" : "lm_gpu_dark" },
                    { 6, theme == 1 ? "lm_disk_light" : "lm_disk_dark" },
                    { 7, theme == 1 ? "lm_net_light" : "lm_net_dark" },
                    { 8, theme == 1 ? "lm_usb_light" : "lm_usb_dark" },
                    { 9, theme == 1 ? "lm_sound_light" : "lm_sound_dark" },
                    { 10, theme == 1 ? "lm_battery_light" : "lm_battery_dark" },
                    { 11, theme == 1 ? "lm_idrivers_light" : "lm_idrivers_dark" },
                    { 12, theme == 1 ? "lm_iservices_light" : "lm_iservices_dark" },
                    { 13, theme == 1 ? "lm_iapps_light" : "lm_iapps_dark" },
                    { 14, theme == 1 ? "lm_export_light" : "lm_export_dark" }
                };
                if (imageMap.TryGetValue(hi_value, out string imageName)){
                    TSImageRenderer(HeaderImage, (Image)Properties.Resources.ResourceManager.GetObject(imageName), 0, ContentAlignment.MiddleCenter);
                }
            }catch (Exception){ }
        }
        // MODULES PAGE DYNAMIC UI
        // ======================================================================================================
        private void Glow_other_page_dynamic_ui(){
            try{
                var glow_other_pages = new (string name, Func<object> createTool, Action<object> applySettings)[]{
                    ("glow_sfc_and_dism_tool", () => new GlowSFCandDISMAutoTool(), tool => ((GlowSFCandDISMAutoTool)tool).SADTLoadEngine()),
                    ("glow_cache_cleanup_tool", () => new GlowCacheCleanupTool(), tool => ((GlowCacheCleanupTool)tool).Cct_theme_settings()),
                    ("glow_bench_cpu_tool", () => new GlowBenchCPU(), tool => ((GlowBenchCPU)tool).Bench_cpu_theme_settings()),
                    ("glow_bench_ram_tool", () => new GlowBenchMemory(), tool => ((GlowBenchMemory)tool).Bench_ram_settings()),
                    ("glow_bench_disk_tool", () => new GlowBenchDisk(), tool => ((GlowBenchDisk)tool).Bench_disk_theme_settings()),
                    ("glow_screen_overlay_tool", () => new GlowOverlay(), tool => ((GlowOverlay)tool).Screen_overlay_settings()),
                    ("glow_dns_test_tool", () => new GlowDNSTestTool(), tool => ((GlowDNSTestTool)tool).Dns_test_settings()),
                    ("glow_quick_access_tool", () => new GlowQuickAccessTool(), tool => ((GlowQuickAccessTool)tool).Quick_access_settings()),
                    ("glow_show_wifi_password_tool", () => new GlowShowWiFiPasswordTool(), tool => ((GlowShowWiFiPasswordTool)tool).Swpt_theme_settings()),
                    ("glow_network_fix_tool", () => new GlowNetworkFixTool(), tool => ((GlowNetworkFixTool)tool).Nft_theme_settings()),
                    ("glow_bluetooth_finder_tool", () => new GlowBluetoothFinderTool(), tool => ((GlowBluetoothFinderTool)tool).BTFinder_Preloader()),
                    ("glow_system_id_generator_tool", () => new GlowSystemIDGenerator(), tool => ((GlowSystemIDGenerator)tool).SIG_Preloader()),
                    ("glow_monitor_test_engine_dead_pixel", () => new GlowMonitorTestEngine(), tool => ((GlowMonitorTestEngine)tool).Monitor_test_engine_theme_settings()),
                    ("glow_monitor_test_engine_dynamic_range", () => new GlowMonitorTestEngine(), tool => ((GlowMonitorTestEngine)tool).Monitor_test_engine_theme_settings()),
                    ("glow_monitor_stuck_pixel_fixer", () => new GlowStuckPixelFixerTool(), tool => ((GlowStuckPixelFixerTool)tool).ChangeDynamicUI()),
                    ("glow_wallpaper_preview_tool", () => new GlowWallpaperPreview(), tool => ((GlowWallpaperPreview)tool).WP_Preview_theme_settings()),
                    ("glow_about", () => new GlowAbout(), tool => ((GlowAbout)tool).About_preloader()),
                };
                foreach (var (toolName, createTool, applySettings) in glow_other_pages){
                    try{
                        var tool = createTool();
                        tool.GetType().GetProperty("Name")?.SetValue(tool, toolName);
                        if (Application.OpenForms[toolName] != null){
                            tool = Application.OpenForms[toolName];
                            applySettings(tool);
                        }
                    }catch (Exception){ }
                }
            }catch (Exception){ }
        }
        // STARTUP SETINGS
        // ======================================================================================================
        private void Select_startup_mode_active(object target_startup_mode){
            ToolStripMenuItem selected_startup_mode = null;
            Select_startup_mode_deactive();
            if (target_startup_mode != null){
                if (selected_startup_mode != (ToolStripMenuItem)target_startup_mode){
                    selected_startup_mode = (ToolStripMenuItem)target_startup_mode;
                    selected_startup_mode.Checked = true;
                }
            }
        }
        private void Select_startup_mode_deactive(){
            foreach (ToolStripMenuItem disabled_startup in startupToolStripMenuItem.DropDownItems){
                disabled_startup.Checked = false;
            }
        }
        private void WindowedToolStripMenuItem_Click(object sender, EventArgs e){
            if (startup_status != 0){ startup_status = 0; Startup_mode_settings("0"); Select_startup_mode_active(sender); }
        }
        private void FullScreenToolStripMenuItem_Click(object sender, EventArgs e){
            if (startup_status != 1){ startup_status = 1; Startup_mode_settings("1"); Select_startup_mode_active(sender); }
        }
        private void Startup_mode_settings(string get_startup_value){
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "StartupStatus", get_startup_value);
            }catch (Exception){ }
        }
        // HIDING MODE
        // ======================================================================================================
        private void Select_hiding_mode_active(object target_hiding_mode){
            ToolStripMenuItem selected_hiding_mode = null;
            Select_hiding_mode_deactive();
            if (target_hiding_mode != null){
                if (selected_hiding_mode != (ToolStripMenuItem)target_hiding_mode){
                    selected_hiding_mode = (ToolStripMenuItem)target_hiding_mode;
                    selected_hiding_mode.Checked = true;
                }
            }
        }
        private void Select_hiding_mode_deactive(){
            foreach (ToolStripMenuItem disabled_hiding in hidingModeToolStripMenuItem.DropDownItems){
                disabled_hiding.Checked = false;
            }
        }
        private void HidingModeOnToolStripMenuItem_Click(object sender, EventArgs e){
            if (hiding_status != 1){ hiding_status = 1; Hiding_mode_settings("1"); Select_hiding_mode_active(sender); }
        }
        private void HidingModeOffToolStripMenuItem_Click(object sender, EventArgs e){
            if (hiding_status != 0){ hiding_status = 0; Hiding_mode_settings("0"); Select_hiding_mode_active(sender); }
        }
        private void Hiding_mode_settings(string get_hiding_value){
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "HidingStatus", get_hiding_value);
            }catch (Exception){ }
            // HIDING MODE CHANGE NOTIFICATION
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            DialogResult hiding_mode_change_message = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("HidingModeChange", "hiding_mode_change_notification"), "\n\n", "\n\n"));
            if (hiding_mode_change_message == DialogResult.Yes){ Application.Restart(); }
        }
        // SOFTWARE OPERATION CONTROLLER MODULE
        // ======================================================================================================
        private static bool Software_operation_controller(string __target_software_path){
            var exeFiles = Directory.GetFiles(__target_software_path, "*.exe");
            var runned_process = Process.GetProcesses();
            foreach (var exe_path in exeFiles){
                string exe_name = Path.GetFileNameWithoutExtension(exe_path);
                if (runned_process.Any(p => {
                    try{
                        return string.Equals(p.ProcessName, exe_name, StringComparison.OrdinalIgnoreCase);
                    }catch{
                        return false;
                    }
                })){
                    return true;
                }
            }
            return false;
        }
        // TS WIZARD STARTER MODE
        // ======================================================================================================
        private string[] Ts_wizard_starter_mode(){
            string[] ts_wizard_exe_files = { "TSWizard_arm64.exe", "TSWizard_x64.exe", "TSWizard.exe" };
            if (RuntimeInformation.OSArchitecture == Architecture.Arm64){
                return new[] { ts_wizard_exe_files[0], ts_wizard_exe_files[1], ts_wizard_exe_files[2] }; // arm64 > x64 > default
            }else if (Environment.Is64BitOperatingSystem){
                return new[] { ts_wizard_exe_files[1], ts_wizard_exe_files[0], ts_wizard_exe_files[2] }; // x64 > arm64 > default
            }else{
                return new[] { ts_wizard_exe_files[2], ts_wizard_exe_files[1], ts_wizard_exe_files[0] }; // default > x64 > arm64
            }
        }
        // UPDATE CHECK ENGINE
        // ======================================================================================================
        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e){
            Task.Run(() => Software_update_check(1));
        }
        public void Software_update_check(int _check_update_ui){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                SetUpdateMenuEnabled(false);
                if (!IsNetworkCheck()){
                    if (_check_update_ui == 1){
                        TS_MessageBoxEngine.TS_MessageBox(this, 2, string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_not_connection"), "\n\n"), string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_title"), Application.ProductName));
                    }
                    return;
                }
                using (WebClient getLastVersion = new WebClient()){
                    string client_version_raw = TS_VersionParser.ParseUINormalize(Application.ProductVersion);
                    string last_version_raw = TS_VersionParser.ParseUINormalize(getLastVersion.DownloadString(TS_LinkSystem.github_link_lv).Split('=')[1].Trim());
                    Version client_ver = Version.Parse(client_version_raw);
                    Version last_ver = Version.Parse(last_version_raw);
                    if (client_ver < last_ver){
                        string baseDir = Path.Combine(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName);
                        string ts_wizard_path = Ts_wizard_starter_mode().Select(name => Path.Combine(baseDir, name)).FirstOrDefault(File.Exists);
                        if (!string.IsNullOrEmpty(ts_wizard_path) && File.Exists(ts_wizard_path)){
                            if (!Software_operation_controller(Path.GetDirectoryName(ts_wizard_path))){
                                DialogResult info_update = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_available_ts_wizard"), Application.ProductName, "\n\n", client_version_raw, "\n", last_version_raw, "\n\n", ts_wizard_name), string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_title"), Application.ProductName));
                                if (info_update == DialogResult.Yes){
                                    Process.Start(new ProcessStartInfo{ FileName = ts_wizard_path, WorkingDirectory = Path.GetDirectoryName(ts_wizard_path) });
                                }
                            }else{
                                if (_check_update_ui == 1){
                                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("HeaderHelp", "header_help_info_notification"), ts_wizard_name));
                                }
                            }
                        }else{
                            DialogResult info_update = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_available"), Application.ProductName, "\n\n", client_version_raw, "\n", last_version_raw, "\n\n"), string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_title"), Application.ProductName));
                            if (info_update == DialogResult.Yes)
                                Process.Start(new ProcessStartInfo(TS_LinkSystem.github_link_lr) { UseShellExecute = true });
                        }
                    }else if (_check_update_ui == 1){
                        string update_msg = client_ver == last_ver ? string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_not_available"), Application.ProductName, "\n", client_version_raw) : string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_newer"), "\n\n", $"v{client_version_raw}");
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, update_msg, string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_title"), Application.ProductName));
                    }
                }
            }catch (Exception ex){
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_error"), "\n\n", ex.Message), string.Format(software_lang.TSReadLangs("SoftwareUpdate", "su_title"), Application.ProductName));
            }finally{
                SetUpdateMenuEnabled(true);
            }
        }
        private void SetUpdateMenuEnabled(bool enabled){
            if (InvokeRequired){
                BeginInvoke(new Action(() => checkForUpdatesToolStripMenuItem.Enabled = enabled));
            }else{
                checkForUpdatesToolStripMenuItem.Enabled = enabled;
            }
        }
        // EXPORT PAGE - SUPPORT SECTION
        // ======================================================================================================
        private void EXPORT_Donate_Click(object sender, EventArgs e){
            try{
                Process.Start(new ProcessStartInfo(TS_LinkSystem.ts_donate){ UseShellExecute = true });
            }catch (Exception){ }
        }
        // PRINT ENGINES
        // ======================================================================================================
        readonly List<string> PrintEngineList = new List<string>();
        private void ExportModsAdd(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                EXPORT_Selector_List.Items.Add(software_lang.TSReadLangs("Export", "e_mode_txt"));
                EXPORT_Selector_List.Items.Add(software_lang.TSReadLangs("Export", "e_mode_html"));
                EXPORT_Selector_List.SelectedIndex = 0;
            }catch (Exception){ }
        }
        private void EXPORT_StartEngineBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                DialogResult start_print_info = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("Export", "e_mode_report_start_info"), EXPORT_Selector_List.SelectedItem.ToString(), "\n\n"));
                if (start_print_info == DialogResult.Yes){
                    EXPORT_ProgressBGPanel.Visible = true;
                    EXPORT_ProgessLabel.Visible = true;
                    //
                    EXPORT_StartEngineBtn.Enabled = false;
                    if (EXPORT_Selector_List.SelectedIndex == 0){
                        Print_engine_mode(0);
                    }else if (EXPORT_Selector_List.SelectedIndex == 1){
                        Print_engine_mode(1);
                    }
                    EXPORT_StartEngineBtn.Enabled = true;
                }
            }catch (Exception){ }
        }
        private void Print_engine_progress_update(int status){
            try{
                const int total_pages = 13;
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                var progressPages = new Dictionary<int, string>{
                    { 1, software_lang.TSReadLangs("LeftMenu", "left_os") },
                    { 2, software_lang.TSReadLangs("LeftMenu", "left_mb") },
                    { 3, software_lang.TSReadLangs("LeftMenu", "left_cpu") },
                    { 4, software_lang.TSReadLangs("LeftMenu", "left_ram") },
                    { 5, software_lang.TSReadLangs("LeftMenu", "left_gpu") },
                    { 6, software_lang.TSReadLangs("LeftMenu", "left_storage") },
                    { 7, software_lang.TSReadLangs("LeftMenu", "left_network") },
                    { 8, software_lang.TSReadLangs("LeftMenu", "left_usb") },
                    { 9, software_lang.TSReadLangs("LeftMenu", "left_sound") },
                    { 10, software_lang.TSReadLangs("LeftMenu", "left_battery") },
                    { 11, software_lang.TSReadLangs("LeftMenu", "left_installed_drivers") },
                    { 12, software_lang.TSReadLangs("LeftMenu", "left_installed_services") },
                    { 13, software_lang.TSReadLangs("LeftMenu", "left_installed_apps") }
                };
                if (status <= total_pages){
                    progressPages.TryGetValue(status, out string progress_page);
                    EXPORT_ProgessLabel.Text = string.Format(software_lang.TSReadLangs("Export", "e_mode_process"), progress_page, status, total_pages);
                    EXPORT_ProgressFEPanel.Width = CalculateProgressWidth(status, total_pages);
                }else{
                    EXPORT_ProgessLabel.Text = string.Format(software_lang.TSReadLangs("Export", "e_mode_save_await"), status - 1, total_pages);
                    EXPORT_ProgressFEPanel.Width = CalculateProgressWidth(status - 1, total_pages);
                }
            }catch (Exception){ }
        }
        private int CalculateProgressWidth(int status, int total_pages){
            return (int)((EXPORT_ProgressBGPanel.Width * status) / total_pages);
        }
        private void Print_after_mode(){
            EXPORT_ProgressBGPanel.Visible = false;
            EXPORT_ProgressFEPanel.Width = 0;
            EXPORT_ProgessLabel.Visible = false;
        }
        private void Print_engine_mode(int pe_mode){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            try{
                switch (pe_mode){
                    case 0:
                        Print_engine_txt();
                    break;
                    case 1:
                        Print_engine_html();
                    break;
                }
            }catch (Exception){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, software_lang.TSReadLangs("PrintEngine", "pe_export_info_error"));
            }
        }
        // PRINT ENGINE TXT
        // ======================================================================================================
        private void Print_engine_txt(){
            // HEADER
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            PrintEngineList.Add($"<{new string('-', 13)} {Application.ProductName.ToUpper()} - {string.Format(software_lang.TSReadLangs("PrintEngine", "pe_report_title").ToUpper(), OS_ComputerName_V.Text)} {new string('-', 13)}>");
            PrintEngineList.Add(Environment.NewLine + new string('-', 60) + Environment.NewLine);
            // OS
            Print_engine_progress_update(1);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_os")} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(OS_SystemUser.Text + " " + OS_SystemUser_V.Text);
            PrintEngineList.Add(OS_ComputerName.Text + " " + OS_ComputerName_V.Text);
            PrintEngineList.Add(OS_SavedUser.Text + " " + OS_SavedUser_V.Text);
            PrintEngineList.Add(OS_Name.Text + " " + OS_Name_V.Text);
            PrintEngineList.Add(OS_Manufacturer.Text + " " + OS_Manufacturer_V.Text);
            PrintEngineList.Add(OS_SystemVersion.Text + " " + OS_SystemVersion_V.Text);
            PrintEngineList.Add(OS_SystemArchitectural.Text + " " + OS_SystemArchitectural_V.Text);
            PrintEngineList.Add(OS_DeviceID.Text + " " + OS_DeviceID_V.Text);
            PrintEngineList.Add(OS_Serial.Text + " " + OS_Serial_V.Text);
            PrintEngineList.Add(OS_Country.Text + " " + OS_Country_V.Text);
            PrintEngineList.Add(OS_TimeZone.Text + " " + OS_TimeZone_V.Text);
            PrintEngineList.Add(OS_EncryptionType.Text + " " + OS_EncryptionType_V.Text);
            PrintEngineList.Add(OS_SystemTime.Text + " " + OS_SystemTime_V.Text);
            PrintEngineList.Add(OS_Install.Text + " " + OS_Install_V.Text);
            PrintEngineList.Add(OS_SystemWorkTime.Text + " " + OS_SystemWorkTime_V.Text);
            PrintEngineList.Add(OS_LastBootTime.Text + " " + OS_LastBootTime_V.Text);
            PrintEngineList.Add(OS_SystemLastShutDown.Text + " " + OS_SystemLastShutDown_V.Text);
            PrintEngineList.Add(OS_PrimaryOS.Text + " " + OS_PrimaryOS_V.Text);
            PrintEngineList.Add(OS_PortableOS.Text + " " + OS_PortableOS_V.Text);
            PrintEngineList.Add(OS_MouseWheelStatus.Text + " " + OS_MouseWheelStatus_V.Text);
            PrintEngineList.Add(OS_ScrollLockStatus.Text + " " + OS_ScrollLockStatus_V.Text);
            PrintEngineList.Add(OS_NumLockStatus.Text + " " + OS_NumLockStatus_V.Text);
            PrintEngineList.Add(OS_CapsLockStatus.Text + " " + OS_CapsLockStatus_V.Text);
            PrintEngineList.Add(OS_FastBoot.Text + " " + OS_FastBoot_V.Text);
            PrintEngineList.Add(OS_WinPageFile.Text + " " + OS_WinPageFile_V.Text);
            PrintEngineList.Add(OS_TempWinPageFile.Text + " " + OS_TempWinPageFile_V.Text);
            PrintEngineList.Add(OS_Hiberfil.Text + " " + OS_Hiberfil_V.Text);
            PrintEngineList.Add(OS_AVProgram.Text + " " + OS_AVProgram_V.Text);
            PrintEngineList.Add(OS_FirewallProgram.Text + " " + OS_FirewallProgram_V.Text);
            PrintEngineList.Add(OS_AntiSpywareProgram.Text + " " + OS_AntiSpywareProgram_V.Text);
            PrintEngineList.Add(OS_WinDefCoreIsolation.Text + " " + OS_WinDefCoreIsolation_V.Text);
            PrintEngineList.Add(OS_CA2023_Status.Text + " " + OS_CA2023_Status_V.Text);
            PrintEngineList.Add(OS_CA2023_Capable.Text + " " + OS_CA2023_Capable_V.Text);
            PrintEngineList.Add(OS_CA2023_Error.Text + " " + OS_CA2023_Error_V.Text);
            PrintEngineList.Add(OS_ActivePower.Text + " " + OS_ActivePower_V.Text);
            PrintEngineList.Add(OS_ActivePowerGUID.Text + " " + OS_ActivePowerGUID_V.Text);
            PrintEngineList.Add(OS_ActivePowerScreenTimeOutP.Text + " " + OS_ActivePowerScreenTimeOutP_V.Text);
            PrintEngineList.Add(OS_ActivePowerScreenTimeOutB.Text + " " + OS_ActivePowerScreenTimeOutB_V.Text);
            PrintEngineList.Add(OS_ActivePowerSleepTimeP.Text + " " + OS_ActivePowerSleepTimeP_V.Text);
            PrintEngineList.Add(OS_ActivePowerSleepTimeB.Text + " " + OS_ActivePowerSleepTimeB_V.Text);
            PrintEngineList.Add(OS_MSEdge.Text + " " + OS_MSEdge_V.Text);
            PrintEngineList.Add(OS_MSEdgeWebView.Text + " " + OS_MSEdgeWebView_V.Text);
            PrintEngineList.Add(OS_MSStoreVersion.Text + " " + OS_MSStoreVersion_V.Text);
            PrintEngineList.Add(OS_MSOfficeVersion.Text + " " + OS_MSOfficeVersion_V.Text);
            PrintEngineList.Add(OS_WinKey.Text + " " + OS_WinKey_V.Text);
            PrintEngineList.Add(OS_WinActiveChannel.Text + " " + OS_WinActiveChannel_V.Text);
            PrintEngineList.Add(OS_NETFrameworkVersion.Text + " " + OS_NETFrameworkVersion_V.Text);
            PrintEngineList.Add(OS_Minidump.Text + " " + OS_Minidump_V.Text);
            PrintEngineList.Add(OS_BSODDate.Text + " " + OS_BSODDate_V.Text);
            PrintEngineList.Add(OS_Wallpaper.Text + " " + OS_Wallpaper_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            // MOTHERBOARD
            Print_engine_progress_update(2);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_mb")} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(MB_MotherBoardName.Text + " " + MB_MotherBoardName_V.Text);
            PrintEngineList.Add(MB_MotherBoardMan.Text + " " + MB_MotherBoardMan_V.Text);
            PrintEngineList.Add(MB_SystemModelMan.Text + " " + MB_SystemModelMan_V.Text);
            PrintEngineList.Add(MB_SystemModelFamily.Text + " " + MB_SystemModelFamily_V.Text);
            PrintEngineList.Add(MB_SystemFamily.Text + " " + MB_SystemFamily_V.Text);
            PrintEngineList.Add(MB_SystemModel.Text + " " + MB_SystemModel_V.Text);
            PrintEngineList.Add(MB_DeviceSerialNumber.Text + " " + MB_DeviceSerialNumber_V.Text);
            PrintEngineList.Add(MB_MotherBoardSerial.Text + " " + MB_MotherBoardSerial_V.Text);
            PrintEngineList.Add(MB_SystemSKU.Text + " " + MB_SystemSKU_V.Text);
            PrintEngineList.Add(MB_Chipset.Text + " " + MB_Chipset_V.Text);
            PrintEngineList.Add(MB_BiosManufacturer.Text + " " + MB_BiosManufacturer_V.Text);
            PrintEngineList.Add(MB_BiosDate.Text + " " + MB_BiosDate_V.Text);
            PrintEngineList.Add(MB_BiosVersion.Text + " " + MB_BiosVersion_V.Text);
            PrintEngineList.Add(MB_SmBiosVersion.Text + " " + MB_SmBiosVersion_V.Text);
            PrintEngineList.Add(MB_BiosMode.Text + " " + MB_BiosMode_V.Text);
            PrintEngineList.Add(MB_LastBIOSTime.Text + " " + MB_LastBIOSTime_V.Text);
            PrintEngineList.Add(MB_SecureBoot.Text + " " + MB_SecureBoot_V.Text);
            PrintEngineList.Add(MB_SecureBootCA2023.Text + " " + MB_SecureBootCA2023_V.Text);
            PrintEngineList.Add(MB_TPMStatus.Text + " " + MB_TPMStatus_V.Text);
            PrintEngineList.Add(MB_TPMPhysicalVersion.Text + " " + MB_TPMPhysicalVersion_V.Text);
            PrintEngineList.Add(MB_TPMMan.Text + " " + MB_TPMMan_V.Text);
            PrintEngineList.Add(MB_TPMManID.Text + " " + MB_TPMManID_V.Text);
            PrintEngineList.Add(MB_TPMManVersion.Text + " " + MB_TPMManVersion_V.Text);
            PrintEngineList.Add(MB_TPMManFullVersion.Text + " " + MB_TPMManFullVersion_V.Text);
            PrintEngineList.Add(MB_TPMManPublisher.Text + " " + MB_TPMManPublisher_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            // CPU
            Print_engine_progress_update(3);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_cpu")} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(CPU_Name.Text + " " + CPU_Name_V.Text);
            PrintEngineList.Add(CPU_Manufacturer.Text + " " + CPU_Manufacturer_V.Text);
            PrintEngineList.Add(CPU_Architectural.Text + " " + CPU_Architectural_V.Text);
            PrintEngineList.Add(CPU_IntelME.Text + " " + CPU_IntelME_V.Text);
            PrintEngineList.Add(CPU_NormalSpeed.Text + " " + CPU_NormalSpeed_V.Text);
            PrintEngineList.Add(CPU_DefaultSpeed.Text + " " + CPU_DefaultSpeed_V.Text);
            PrintEngineList.Add(CPU_L1.Text + " " + CPU_L1_V.Text);
            PrintEngineList.Add(CPU_L2.Text + " " + CPU_L2_V.Text);
            PrintEngineList.Add(CPU_L3.Text + " " + CPU_L3_V.Text);
            PrintEngineList.Add(CPU_CoreCount.Text + " " + CPU_CoreCount_V.Text);
            PrintEngineList.Add(CPU_LogicalCore.Text + " " + CPU_LogicalCore_V.Text);
            PrintEngineList.Add(CPU_Usage.Text + " " + CPU_Usage_V.Text);
            PrintEngineList.Add(CPU_Process.Text + " " + CPU_Process_V.Text);
            PrintEngineList.Add(CPU_Threads.Text + " " + CPU_Threads_V.Text);
            PrintEngineList.Add(CPU_Handles.Text + " " + CPU_Handles_V.Text);
            PrintEngineList.Add(CPU_SocketDefinition.Text + " " + CPU_SocketDefinition_V.Text);
            PrintEngineList.Add(CPU_Family.Text + " " + CPU_Family_V.Text);
            PrintEngineList.Add(CPU_Virtualization.Text + " " + CPU_Virtualization_V.Text);
            PrintEngineList.Add(CPU_HyperV.Text + " " + CPU_HyperV_V.Text);
            PrintEngineList.Add(CPU_VMExtension.Text + " " + CPU_VMExtension_V.Text);
            PrintEngineList.Add(CPU_SerialName.Text + " " + CPU_SerialName_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            // RAM
            Print_engine_progress_update(4);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_ram")} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(RAM_TotalRAM.Text + " " + RAM_TotalRAM_V.Text);
            PrintEngineList.Add(RAM_UsageRAMCount.Text + " " + RAM_UsageRAMCount_V.Text);
            PrintEngineList.Add(RAM_EmptyRamCount.Text + " " + RAM_EmptyRamCount_V.Text);
            PrintEngineList.Add(RAM_TotalVirtualRam.Text + " " + RAM_TotalVirtualRam_V.Text);
            PrintEngineList.Add(RAM_UsageVirtualRam.Text + " " + RAM_UsageVirtualRam_V.Text);
            PrintEngineList.Add(RAM_EmptyVirtualRam.Text + " " + RAM_EmptyVirtualRam_V.Text);
            PrintEngineList.Add(RAM_SlotStatus.Text + " " + RAM_SlotStatus_V.Text + Environment.NewLine);
            try{
                int ram_slot = RAM_Selector_List.Items.Count;
                for (int rs = 1; rs <= ram_slot; rs++){
                    RAM_Selector_List.SelectedIndex = rs - 1;
                    PrintEngineList.Add(RAM_Selector_List.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(RAM_Amount.Text + " " + RAM_Amount_V.Text);
                    PrintEngineList.Add(RAM_Type.Text + " " + RAM_Type_V.Text);
                    PrintEngineList.Add(RAM_Frequency.Text + " " + RAM_Frequency_V.Text);
                    PrintEngineList.Add(RAM_Volt.Text + " " + RAM_Volt_V.Text);
                    PrintEngineList.Add(RAM_FormFactor.Text + " " + RAM_FormFactor_V.Text);
                    PrintEngineList.Add(RAM_Serial.Text + " " + RAM_Serial_V.Text);
                    PrintEngineList.Add(RAM_Manufacturer.Text + " " + RAM_Manufacturer_V.Text);
                    PrintEngineList.Add(RAM_BankLabel.Text + " " + RAM_BankLabel_V.Text);
                    PrintEngineList.Add(RAM_DataWidth.Text + " " + RAM_DataWidth_V.Text);
                    PrintEngineList.Add(RAM_BellekType.Text + " " + RAM_BellekType_V.Text);
                    PrintEngineList.Add(RAM_PartNumber.Text + " " + RAM_PartNumber_V.Text + Environment.NewLine);
                }
                RAM_Selector_List.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // GPU
            Print_engine_progress_update(5);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_gpu")} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int gpu_amount = GPU_Selector_List.Items.Count;
                for (int gpu_render = 1; gpu_render <= gpu_amount; gpu_render++){
                    GPU_Selector_List.SelectedIndex = gpu_render - 1;
                    PrintEngineList.Add(software_lang.TSReadLangs("Gpu_Content", "gpu_c_gpu_print") + " " + GPU_Selector_List.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(GPU_Selector.Text + " " + GPU_Selector_List.SelectedItem.ToString());
                    PrintEngineList.Add(GPU_Manufacturer.Text + " " + GPU_Manufacturer_V.Text);
                    PrintEngineList.Add(GPU_VRAM.Text + " " + GPU_VRAM_V.Text);
                    PrintEngineList.Add(GPU_Version.Text + " " + GPU_Version_V.Text);
                    PrintEngineList.Add(GPU_DriverDate.Text + " " + GPU_DriverDate_V.Text);
                    PrintEngineList.Add(GPU_Status.Text + " " + GPU_Status_V.Text);
                    PrintEngineList.Add(GPU_DeviceID.Text + " " + GPU_DeviceID_V.Text);
                    PrintEngineList.Add(GPU_DacType.Text + " " + GPU_DacType_V.Text);
                    PrintEngineList.Add(GPU_GraphicDriversName.Text + " " + GPU_GraphicDriversName_V.Text);
                    PrintEngineList.Add(GPU_InfFileName.Text + " " + GPU_InfFileName_V.Text);
                    PrintEngineList.Add(GPU_INFSectionFile.Text + " " + GPU_INFSectionFile_V.Text);
                    PrintEngineList.Add(GPU_CurrentColor.Text + " " + GPU_CurrentColor_V.Text + Environment.NewLine);
                }
                GPU_Selector_List.SelectedIndex = 0;
            }catch (Exception){ }
            try{
                int screen_amount = GPU_MonitorSelector_List.Items.Count;
                for (int sa = 1; sa <= screen_amount; sa++){
                    GPU_MonitorSelector_List.SelectedIndex = sa - 1;
                    PrintEngineList.Add(GPU_MonitorSelector_List.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(GPU_MonitorUserFriendlyName.Text + " " + GPU_MonitorUserFriendlyName_V.Text);
                    PrintEngineList.Add(GPU_MonitorManName.Text + " " + GPU_MonitorManName_V.Text);
                    PrintEngineList.Add(GPU_MonitorProductCodeID.Text + " " + GPU_MonitorProductCodeID_V.Text);
                    PrintEngineList.Add(GPU_MonitorSerialNumberID.Text + " " + GPU_MonitorSerialNumberID_V.Text);
                    PrintEngineList.Add(GPU_MonitorConType.Text + " " + GPU_MonitorConType_V.Text);
                    PrintEngineList.Add(GPU_MonitorManfDate.Text + " " + GPU_MonitorManfDate_V.Text);
                    PrintEngineList.Add(GPU_MonitorManfDateWeek.Text + " " + GPU_MonitorManfDateWeek_V.Text);
                    PrintEngineList.Add(GPU_MonitorHID.Text + " " + GPU_MonitorHID_V.Text);
                    PrintEngineList.Add(GPU_MonitorResLabel.Text + " " + GPU_MonitorResLabel_V.Text);
                    PrintEngineList.Add(GPU_MonitorVirtualRes.Text + " " + GPU_MonitorVirtualRes_V.Text);
                    PrintEngineList.Add(GPU_MonitorBounds.Text + " " + GPU_MonitorBounds_V.Text);
                    PrintEngineList.Add(GPU_MonitorWorking.Text + " " + GPU_MonitorWorking_V.Text);
                    PrintEngineList.Add(GPU_ScreenRefreshRate.Text + " " + GPU_ScreenRefreshRate_V.Text);
                    PrintEngineList.Add(GPU_ScreenBit.Text + " " + GPU_ScreenBit_V.Text);
                    PrintEngineList.Add(GPU_MonitorPrimary.Text + " " + GPU_MonitorPrimary_V.Text + Environment.NewLine);
                }
                PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
                GPU_MonitorSelector_List.SelectedIndex = 0;
            }catch (Exception){ }
            // STORAGE
            Print_engine_progress_update(6);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_storage")} {new string('-', 7)}>" + Environment.NewLine);
            try{
                PrintEngineList.Add(DISK_TTLP_L1.Text + Environment.NewLine);
                PrintEngineList.Add(DISK_TTLP_P1_L1.Text + " " + DISK_TTLP_P1_L2.Text);
                PrintEngineList.Add(DISK_TTLP_P2_L1.Text + " " + DISK_TTLP_P2_L2.Text);
                PrintEngineList.Add(DISK_TTLP_P3_L1.Text + " " + DISK_TTLP_P3_L2.Text);
                PrintEngineList.Add(DISK_TTLP_P4_L1.Text + " " + DISK_TTLP_P4_L2.Text);
                PrintEngineList.Add(Environment.NewLine + new string('-', 12) + Environment.NewLine);
                int disk_amount = DISK_Selector_List.Items.Count;
                for (int disk_render = 1; disk_render <= disk_amount; disk_render++){
                    DISK_Selector_List.SelectedIndex = disk_render - 1;
                    PrintEngineList.Add(software_lang.TSReadLangs("StorageContent", "se_c_disk_print") + " " + DISK_Selector_List.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(DISK_Selector.Text + " " + DISK_Selector_List.SelectedItem.ToString());
                    PrintEngineList.Add(DISK_Model.Text + " " + DISK_Model_V.Text);
                    PrintEngineList.Add(DISK_Man.Text + " " + DISK_Man_V.Text);
                    PrintEngineList.Add(DISK_VolumeID.Text + " " + DISK_VolumeID_V.Text);
                    PrintEngineList.Add(DISK_VolumeName.Text + " " + DISK_VolumeName_V.Text);
                    PrintEngineList.Add(DISK_Firmware.Text + " " + DISK_Firmware_V.Text);
                    PrintEngineList.Add(DISK_Serial.Text + " " + DISK_Serial_V.Text);
                    PrintEngineList.Add(DISK_VolumeSerial.Text + " " + DISK_VolumeSerial_V.Text);
                    PrintEngineList.Add(DISK_Size.Text + " " + DISK_Size_V.Text);
                    PrintEngineList.Add(DISK_FreeSpace.Text + " " + DISK_FreeSpace_V.Text);
                    PrintEngineList.Add(DISK_FileSystem.Text + " " + DISK_FileSystem_V.Text);
                    PrintEngineList.Add(DISK_FormattingType.Text + " " + DISK_FormattingType_V.Text);
                    PrintEngineList.Add(DISK_Type.Text + " " + DISK_Type_V.Text);
                    PrintEngineList.Add(DISK_DriveType.Text + " " + DISK_DriveType_V.Text);
                    PrintEngineList.Add(DISK_InterFace.Text + " " + DISK_InterFace_V.Text);
                    PrintEngineList.Add(DISK_PartitionCount.Text + " " + DISK_PartitionCount_V.Text);
                    PrintEngineList.Add(DISK_MediaLoaded.Text + " " + DISK_MediaLoaded_V.Text);
                    PrintEngineList.Add(DISK_MediaStatus.Text + " " + DISK_MediaStatus_V.Text);
                    PrintEngineList.Add(DISK_Health.Text + " " + DISK_Health_V.Text);
                    PrintEngineList.Add(DISK_Boot.Text + " " + DISK_Boot_V.Text);
                    PrintEngineList.Add(DISK_Bootable.Text + " " + DISK_Bootable_V.Text);
                    PrintEngineList.Add(DISK_BitLockerStatus.Text + " " + DISK_BitLockerStatus_V.Text);
                    PrintEngineList.Add(DISK_BitLockerConversionStatus.Text + " " + DISK_BitLockerConversionStatus_V.Text);
                    PrintEngineList.Add(DISK_BitLockerEncryptMehod.Text + " " + DISK_BitLockerEncryptMehod_V.Text);
                    PrintEngineList.Add(DISK_DriveCompressed.Text + " " + DISK_DriveCompressed_V.Text + Environment.NewLine);
                }
                // SELECT DISK
                try{
                    int c_index = disk_volume_id_list.FindIndex(x => x.Contains(windows_disk));
                    DISK_Selector_List.SelectedIndex = c_index;
                    if (c_index == -1){
                        DISK_Selector_List.SelectedIndex = 0;
                    }
                }catch (Exception){
                    DISK_Selector_List.SelectedIndex = 0;
                }
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // NETWORK
            Print_engine_progress_update(7);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_network")} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int net_amount = NET_Selector_List.Items.Count;
                PrintEngineList.Add(NET_LT_Device.Text + " " + NET_LT_Device_V.Text);
                PrintEngineList.Add(NET_LT_BandWidth.Text + " " + NET_LT_BandWidth_V.Text);
                PrintEngineList.Add(NET_LT_LocalIP.Text + " " + NET_LT_LocalIP_V.Text);
                PrintEngineList.Add(NET_LT_GatewayIP.Text + " " + NET_LT_GatewayIP_V.Text);
                PrintEngineList.Add(NET_LT_DL1.Text + " " + NET_LT_DL2.Text);
                PrintEngineList.Add(NET_LT_UL1.Text + " " + NET_LT_UL2.Text + Environment.NewLine);
                for (int net_render = 1; net_render <= net_amount; net_render++){
                    NET_Selector_List.SelectedIndex = net_render - 1;
                    PrintEngineList.Add(software_lang.TSReadLangs("Network_Content", "nk_c_network_print") + " " + NET_Selector_List.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(NET_Selector.Text + " " + NET_Selector_List.SelectedItem.ToString());
                    PrintEngineList.Add(NET_MacAdress.Text + " " + NET_MacAdress_V.Text);
                    PrintEngineList.Add(NET_NetMan.Text + " " + NET_NetMan_V.Text);
                    PrintEngineList.Add(NET_DriverVersion.Text + " " + NET_DriverVersion.Text);
                    PrintEngineList.Add(NET_DriverDate.Text + " " + NET_DriverDate_V.Text);
                    PrintEngineList.Add(NET_ServiceName.Text + " " + NET_ServiceName_V.Text);
                    PrintEngineList.Add(NET_AdapterType.Text + " " + NET_AdapterType_V.Text);
                    PrintEngineList.Add(NET_Physical.Text + " " + NET_Physical_V.Text);
                    PrintEngineList.Add(NET_DeviceID.Text + " " + NET_DeviceID_V.Text);
                    PrintEngineList.Add(NET_Guid.Text + " " + NET_Guid_V.Text);
                    PrintEngineList.Add(NET_ConnectionType.Text + " " + NET_ConnectionType_V.Text);
                    PrintEngineList.Add(NET_Dhcp_status.Text + " " + NET_Dhcp_status_V.Text);
                    PrintEngineList.Add(NET_Dhcp_server.Text + " " + NET_Dhcp_server_V.Text);
                    PrintEngineList.Add(NET_DHCPFirstIpTime.Text + " " + NET_DHCPFirstIpTime_V.Text);
                    PrintEngineList.Add(NET_DHCPLastIpTime.Text + " " + NET_DHCPLastIpTime_V.Text);
                    PrintEngineList.Add(NET_LocalConSpeed.Text + " " + NET_LocalConSpeed_V.Text);
                    PrintEngineList.Add(NET_IPv4Adress.Text + " " + NET_IPv4Adress_V.Text);
                    PrintEngineList.Add(NET_IPv6Adress.Text + " " + NET_IPv6Adress_V.Text + Environment.NewLine);
                }
                PrintEngineList.Add(NET_DNS1.Text + " " + NET_DNS1_V.Text);
                PrintEngineList.Add(NET_DNS2.Text + " " + NET_DNS2_V.Text + Environment.NewLine);
                NET_Selector_List.SelectedIndex = 1;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // USB
            Print_engine_progress_update(8);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_usb")} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int usb_con_amount = USB_Selector_List.Items.Count;
                for (int usb_con_render = 1; usb_con_render <= usb_con_amount; usb_con_render++){
                    USB_Selector_List.SelectedIndex = usb_con_render - 1;
                    PrintEngineList.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_controller_print") + " " + USB_Selector_List.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(USB_Selector.Text + " " + USB_Selector_List.SelectedItem.ToString());
                    PrintEngineList.Add(USB_ConName.Text + " " + USB_ConName_V.Text);
                    PrintEngineList.Add(USB_ConMan.Text + " " + USB_ConMan_V.Text);
                    PrintEngineList.Add(USB_ConDeviceID.Text + " " + USB_ConDeviceID_V.Text);
                    PrintEngineList.Add(USB_ConPNPDeviceID.Text + " " + USB_ConPNPDeviceID_V.Text);
                    PrintEngineList.Add(USB_ConDeviceStatus.Text + " " + USB_ConDeviceStatus_V.Text + Environment.NewLine);
                }
                USB_Selector_List.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 40) + Environment.NewLine);
            try{
                int usb_amount = USB_DeviceSelector_List.Items.Count;
                for (int usb_render = 1; usb_render <= usb_amount; usb_render++){
                    USB_DeviceSelector_List.SelectedIndex = usb_render - 1;
                    PrintEngineList.Add(software_lang.TSReadLangs("Usb_Content", "usb_c_device_print") + " " + USB_DeviceSelector_List.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(USB_DeviceSelector.Text + " " + USB_DeviceSelector_List.SelectedItem.ToString());
                    PrintEngineList.Add(USB_DeviceName.Text + " " + USB_DeviceName_V.Text);
                    PrintEngineList.Add(USB_DeviceMan.Text + " " + USB_DeviceMan_V.Text);
                    PrintEngineList.Add(USB_DriverVersion.Text + " " + USB_DriverVersion_V.Text);
                    PrintEngineList.Add(USB_DriverDate.Text + " " + USB_DriverDate_V.Text);
                    PrintEngineList.Add(USB_InfFile.Text + " " + USB_InfFile_V.Text);
                    PrintEngineList.Add(USB_DeviceID.Text + " " + USB_DeviceID_V.Text);
                    PrintEngineList.Add(USB_HardwareID.Text + " " + USB_HardwareID_V.Text);
                    PrintEngineList.Add(USB_DeviceGUID.Text + " " + USB_DeviceGUID_V.Text + Environment.NewLine);
                }
                USB_DeviceSelector_List.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // SOUND
            Print_engine_progress_update(9);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_sound")} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int sound_amount = SOUND_Selector_List.Items.Count;
                for (int sound_render = 1; sound_render <= sound_amount; sound_render++){
                    SOUND_Selector_List.SelectedIndex = sound_render - 1;
                    PrintEngineList.Add(software_lang.TSReadLangs("Sound_Content", "sound_c_print") + " " + SOUND_Selector_List.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(SOUND_Selector.Text + " " + SOUND_Selector_List.SelectedItem.ToString());
                    PrintEngineList.Add(SOUND_DeviceName.Text + " " + SOUND_DeviceName_V.Text);
                    PrintEngineList.Add(SOUND_DeviceManufacturer.Text + " " + SOUND_DeviceManufacturer_V.Text);
                    PrintEngineList.Add(SOUND_DriverVersion.Text + " " + SOUND_DriverVersion_V.Text);
                    PrintEngineList.Add(SOUND_DriverDate.Text + " " + SOUND_DriverDate_V.Text);
                    PrintEngineList.Add(SOUND_DeviceID.Text + " " + SOUND_DeviceID_V.Text);
                    PrintEngineList.Add(SOUND_PNPDeviceID.Text + " " + SOUND_PNPDeviceID_V.Text);
                    PrintEngineList.Add(SOUND_DeviceStatus.Text + " " + SOUND_DeviceStatus_V.Text + Environment.NewLine);
                }
                SOUND_Selector_List.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // BATTERY
            Print_engine_progress_update(10);
            PowerStatus power_status = SystemInformation.PowerStatus;
            String battery_charging = power_status.BatteryChargeStatus.ToString();
            if (battery_charging == "NoSystemBattery"){
                PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_battery")} {new string('-', 7)}>" + Environment.NewLine);
                PrintEngineList.Add(BATTERY_Status.Text + " " + BATTERY_Status_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            }else{
                PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_battery")} {new string('-', 7)}>" + Environment.NewLine);
                PrintEngineList.Add(BATTERY_Status.Text + " " + BATTERY_Status_V.Text);
                PrintEngineList.Add(BATTERY_Model.Text + " " + BATTERY_Model_V.Text);
                PrintEngineList.Add(BATTERY_Serial.Text + " " + BATTERY_Serial_V.Text);
                PrintEngineList.Add(BATTERY_Chemistry.Text + " " + BATTERY_Chemistry_V.Text);
                PrintEngineList.Add(BATTERY_DesignCapacity.Text + " " + BATTERY_DesignCapacity_V.Text);
                PrintEngineList.Add(BATTERY_FullChargeCapacity.Text + " " + BATTERY_FullChargeCapacity_V.Text);
                PrintEngineList.Add(BATTERY_RemainingChargeCapacity.Text + " " + BATTERY_RemainingChargeCapacity_V.Text);
                PrintEngineList.Add(BATTERY_Voltage.Text + " " + BATTERY_Voltage_V.Text);
                PrintEngineList.Add(BATTERY_ChargePower.Text + " " + BATTERY_ChargePower_V.Text);
                PrintEngineList.Add(BATTERY_ChargeCurrent.Text + " " + BATTERY_ChargeCurrent_V.Text);
                PrintEngineList.Add(BATTERY_DeChargePower.Text + " " + BATTERY_DeChargePower_V.Text);
                PrintEngineList.Add(BATTERY_DeChargeCurrent.Text + " " + BATTERY_DeChargeCurrent_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            }
            // OSD
            Print_engine_progress_update(11);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_installed_drivers")} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(software_lang.TSReadLangs("PrintEngine", "pe_driver_sorting") + Environment.NewLine);
            try{
                foreach (DataGridViewRow row in OSD_DataMainTable.Rows){
                    if (row.IsNewRow) continue;
                    var cellValues = row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString() ?? string.Empty);
                    PrintEngineList.Add(string.Join(" | ", cellValues) + Environment.NewLine + new string('-', 155));
                }
            }catch (Exception){ }
            PrintEngineList.Add(Environment.NewLine + software_lang.TSReadLangs("Osd_Content", "osd_total_installed_driver_count") + " " + OSD_TYSS_V.Text + Environment.NewLine);
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // SERVICES
            Print_engine_progress_update(12);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_installed_services")} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(software_lang.TSReadLangs("PrintEngine", "pe_service_sorting") + Environment.NewLine);
            try{
                foreach (DataGridViewRow row in SERVICE_DataMainTable.Rows){
                    if (row.IsNewRow) continue;
                    var cellValues = row.Cells .Cast<DataGridViewCell>().Select(c => c.Value?.ToString() ?? string.Empty);
                    PrintEngineList.Add(string.Join(" | ", cellValues) + Environment.NewLine + new string('-', 155));
                }
            }catch (Exception){ }
            PrintEngineList.Add(Environment.NewLine + software_lang.TSReadLangs("Services_Content", "ss_total_installed_service_count") + " " + SERVICE_TYS_V.Text + Environment.NewLine);
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // INSTALLED APPS
            Print_engine_progress_update(13);
            PrintEngineList.Add($"<{new string('-', 7)} {software_lang.TSReadLangs("Header", "header_installed_apps")} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(software_lang.TSReadLangs("PrintEngine", "pe_instapps_sorting") + Environment.NewLine);
            try{
                foreach (DataGridViewRow row in INSTAPPS_DataMainTable.Rows){
                    if (row.IsNewRow) continue;
                    var cellValues = row.Cells.Cast<DataGridViewCell>().Skip(1).Select(c => c.Value?.ToString() ?? string.Empty);
                    PrintEngineList.Add(string.Join(" | ", cellValues) + Environment.NewLine + new string('-', 155));
                }
            }catch (Exception){ }
            PrintEngineList.Add(Environment.NewLine + software_lang.TSReadLangs("Instapps_Content", "ia_total_installed_apps_count") + " " + INSTAPPS_TYUS_V.Text + Environment.NewLine);
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // FOOTER
            PrintEngineList.Add(Application.ProductName + " " + software_lang.TSReadLangs("PrintEngine", "pe_version") + " " + TS_VersionEngine.TS_SofwareVersion(1));
            PrintEngineList.Add(TS_SoftwareCopyrightDate.ts_scd_preloader);
            PrintEngineList.Add(software_lang.TSReadLangs("PrintEngine", "pe_process_time") + " " + DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss"));
            PrintEngineList.Add(software_lang.TSReadLangs("PrintEngine", "pe_website") + " " + TS_LinkSystem.website_link);
            PrintEngineList.Add(software_lang.TSReadLangs("PrintEngine", "pe_github") + " " + TS_LinkSystem.github_link);
            PrintEngineList.Add(software_lang.TSReadLangs("PrintEngine", "pe_donate") + " " + TS_LinkSystem.ts_donate);
            Print_engine_progress_update(14);
            using (SaveFileDialog save_engine = new SaveFileDialog{
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = Application.ProductName + " - " + software_lang.TSReadLangs("PrintEngine", "pe_save_directory"),
                DefaultExt = "txt",
                FileName = Application.ProductName + " - " + string.Format(software_lang.TSReadLangs("PrintEngine", "pe_save_name"), OS_ComputerName_V.Text),
                Filter = software_lang.TSReadLangs("PrintEngine", "pe_save_txt") + " (*.txt)|*.txt"
            }){
                if (save_engine.ShowDialog() == DialogResult.OK){
                    string combinedText = String.Join(Environment.NewLine, PrintEngineList);
                    File.WriteAllText(save_engine.FileName, combinedText);
                    DialogResult glow_print_engine_query = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("PrintEngine", "pe_save_success") + "\n\n" + software_lang.TSReadLangs("PrintEngine", "pe_save_info_open"), Application.ProductName, save_engine.FileName));
                    if (glow_print_engine_query == DialogResult.Yes){
                        Process.Start(save_engine.FileName);
                    }
                }
                PrintEngineList.Clear();
                Print_after_mode();
            }
        }
        // PRINT ENGINE HTML
        // ======================================================================================================
        private void Print_engine_html(){
            // HEADER
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            // COLOR MODES
            Color html_body_bg_color = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
            string html_bbgc = string.Format("#{0}{1}{2}", html_body_bg_color.R.ToString("X2"), html_body_bg_color.G.ToString("X2"), html_body_bg_color.B.ToString("X2"));
            Color html_middle_bg_color = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
            string html_mbgc = string.Format("#{0}{1}{2}", html_middle_bg_color.R.ToString("X2"), html_middle_bg_color.G.ToString("X2"), html_middle_bg_color.B.ToString("X2"));
            Color html_ui_fe_color = TS_ThemeEngine.ColorMode(theme, "AccentColor");
            string html_uifc = string.Format("#{0}{1}{2}", html_ui_fe_color.R.ToString("X2"), html_ui_fe_color.G.ToString("X2"), html_ui_fe_color.B.ToString("X2"));
            Color html_ui_fe_hover_color = TS_ThemeEngine.ColorMode(theme, "AccentColorHover");
            string html_uifhc = string.Format("#{0}{1}{2}", html_ui_fe_hover_color.R.ToString("X2"), html_ui_fe_hover_color.G.ToString("X2"), html_ui_fe_hover_color.B.ToString("X2"));
            // EXTERNAL LINKS
            string print_html_font_url          = @"https://fonts.googleapis.com/css2?family=Open+Sans:ital,wght@0,300..800;1,300..800&display=swap";
            string print_html_glow_logo_url     = @"https://raw.githubusercontent.com/turkaysoftware/glow/main/Glow/glow_images/glow_logo/GlowLogo.ico";
            // HTML MAIN CODES
            PrintEngineList.Add("<!DOCTYPE html>");
            PrintEngineList.Add($"<html lang='{lang}'>");
            PrintEngineList.Add("<head>");
            PrintEngineList.Add("\t<meta charset='utf-8'>");
            PrintEngineList.Add("\t<meta name='author' content='" + Application.CompanyName + "'>");
            PrintEngineList.Add("\t<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            PrintEngineList.Add($"\t<title>{Application.ProductName} - {string.Format(software_lang.TSReadLangs("PrintEngine", "pe_report_title"), OS_ComputerName_V.Text)}</title>");
            PrintEngineList.Add("\t<style>");
            PrintEngineList.Add("\t\t@import url('" + print_html_font_url + "');");
            PrintEngineList.Add("\t\t*{ font-family: 'Open Sans', sans-serif; margin: 0; padding: 0; box-sizing: border-box; text-decoration: none; }");
            if (theme == 1){ PrintEngineList.Add("\t\t:root { color-scheme: light; }"); }
            else if (theme == 0){ PrintEngineList.Add("\t\t:root { color-scheme: dark; }"); }
            PrintEngineList.Add("\t\ta:visited{ color: " + html_uifhc + "; }");
            PrintEngineList.Add("\t\t.ts_scroll_top{ background-color: " + html_uifc + "; position: fixed; bottom: -100px; right: 15px; width: 45px; height: 45px; justify-content: center; align-items: center; display: flex; border-radius: 50%; cursor: pointer; z-index: 999; transition: 0.2s; }");
            PrintEngineList.Add("\t\t.ts_scroll_top > .ts_arrow_up { border-left: 5px solid " + html_mbgc + "; border-top: 5px solid " + html_mbgc + "; width: 16px; height: 16px; transform: rotate(45deg); border-radius: 2px; margin-top: 4px; }");
            PrintEngineList.Add("\t\t.ts_scroll_top:hover{ background-color: " + html_uifhc + "; }");
            PrintEngineList.Add("\t\t.ts_scroll_top.active{ bottom: 15px; }");
            PrintEngineList.Add("\t\t@media (max-width: 700px){ .ts_scroll_top{ bottom: -100px; right: 10px; } .ts_scroll_top.active{ bottom: 10px; } }");
            PrintEngineList.Add("\t\t.ts_page_wrapper{ background-color:" + html_mbgc + "; font-weight: 500; position:fixed; left: 15px; bottom: 15px; width: auto; height: 35px; padding: 0 5px; font-size: 16px; margin: 0; outline: none; border-radius: 5px; border: 1px solid " + html_uifc + "; transition: 0.2s; -webkit-box-shadow: 0px 0px 13px -3px rgba(0,0,0,0.7); -moz-box-shadow: 0px 0px 13px -3px rgba(0,0,0,0.7); box-shadow: 0px 0px 13px -3px rgba(0,0,0,0.7); }");
            PrintEngineList.Add("\t\t@media (max-width: 700px){ .ts_page_wrapper{ left: 10px; bottom: 10px; height: 30px; border-radius: 3px; } }");
            PrintEngineList.Add("\t\tbody{ background-color: " + html_bbgc + "; padding: 5px 10px; justify-content: center; align-items: center; display: flex; }");
            PrintEngineList.Add("\t\t#main_container{ width: 100%; height: auto; justify-content: center; align-items: center; display: flex; flex-direction: column; }");
            PrintEngineList.Add("\t\t#main_container > h2{ margin: 25px 0; font-weight: 500; color: " + html_uifc + "; }");
            PrintEngineList.Add("\t\t#main_container > .b1, .b2, .b3, .b4, .b5, .b6, .b7, .b8, .b9, .b10, .b11, .b12, .b13, .b14, .b15{ background-color: " + html_mbgc + "; width: 1250px; height: auto; border-radius: 10px; margin: 5px 0; padding: 15px; box-sizing: border-box; display: inline-block; word-break: break-word; table-layout: fixed; }");
            PrintEngineList.Add("\t\t#main_container > .b8 > h3{ margin: 0 0 15px 0; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1){ justify-content: start; align-items: center; display: flex; gap: 25px; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1) > img{ width: 75px; height: 75px; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1) > .ts_box_text{ justify-content: center; align-items: start; display: flex; flex-direction: column; gap: 5px; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1) > .ts_box_text > h2{ color: " + html_uifc + "; font-weight: 500; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1) > .ts_box_text > span{ font-weight: 500; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1) > .ts_box_text > span > label{ color: " + html_uifc + "; font-weight: 500; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > h2{ color: " + html_uifc + "; font-weight: 500; text-align: center; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > h3{ color: " + html_uifc + "; font-weight: 500; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > h4{ margin: 13px 0 0 11px; font-weight: 600; justify-content: start; align-items: center; display: flex; gap: 5px; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > h4 > span:nth-child(2){ color: " + html_uifc + "; font-weight: 600; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul{ margin: 15px 0 0 30px; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul > li{ margin: 5px 0; font-weight: 500; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul > li > span{ margin: 0 7px 0 0; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul > li > span:nth-child(2){ color: " + html_uifc + "; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul > li > a{ color: " + html_uifc + "; text-decoration: underline; transition: 0.3s; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul > li > a:hover{ color: " + html_uifhc + "; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > hr{ height: 1px; background-color: " + html_uifc + "; border: none; margin: 20px 0; padding: 0; }");
            PrintEngineList.Add("\t\t#main_container > .b15 > ul > li > span{ color: " + html_uifc + "; }");
            PrintEngineList.Add("\t\t@media (max-width: 1260px){ #main_container > .b1, .b2, .b3, .b4, .b5, .b6, .b7, .b8, .b9, .b10, .b11, .b12, .b13, .b14, .b15{ width: 100%; } }");
            PrintEngineList.Add("\t\t@media (max-width: 735px){ #main_container > .b1, .b2, .b3, .b4, .b5, .b6, .b7, .b8, .b9, .b10, .b11, .b12, .b13, .b14, .b15{ padding: 10px; } #main_container > .ts_box_wrapper > h3{ text-align: center; } }");
            PrintEngineList.Add("\t\t@media (max-width: 495px){ #main_container > .ts_box_wrapper:nth-child(1){ flex-direction: column; justify-content: center; gap: 10px; } #main_container > .ts_box_wrapper:nth-child(1) > .ts_box_text{ text-align: center; align-items: center; } #main_container > .ts_box_wrapper > ul{ margin: 15px 0 0 25px; } #main_container > .ts_box_wrapper > h4{ margin: 13px 0 0 6px; } }");
            PrintEngineList.Add("\t</style>");
            PrintEngineList.Add("\t<link rel='icon' type='image/x-icon' href='" + print_html_glow_logo_url + "'>");
            PrintEngineList.Add("</head>");
            PrintEngineList.Add("<body>");
            // SCROLL TOP BUTTON
            PrintEngineList.Add($"\t<div class='ts_scroll_top' onclick='ts_scroll_to_up();' title='{software_lang.TSReadLangs("PrintEngine", "pe_scroll_top_select")}'><span class='ts_arrow_up'></span></div>");
            // SELECT WRAPPER
            PrintEngineList.Add($"\t<select title='{software_lang.TSReadLangs("PrintEngine", "pe_html_partition_select")}' class='ts_page_wrapper' id='ts_session_select'>");
            PrintEngineList.Add($"\t\t<option value='b1'>{software_lang.TSReadLangs("PrintEngine", "pe_html_header_select")}</option>");
            PrintEngineList.Add($"\t\t<option value='b2'>{software_lang.TSReadLangs("LeftMenu", "left_os")}</option>");
            PrintEngineList.Add($"\t\t<option value='b3'>{software_lang.TSReadLangs("LeftMenu", "left_mb")}</option>");
            PrintEngineList.Add($"\t\t<option value='b4'>{software_lang.TSReadLangs("LeftMenu", "left_cpu")}</option>");
            PrintEngineList.Add($"\t\t<option value='b5'>{software_lang.TSReadLangs("LeftMenu", "left_ram")}</option>");
            PrintEngineList.Add($"\t\t<option value='b6'>{software_lang.TSReadLangs("LeftMenu", "left_gpu")}</option>");
            PrintEngineList.Add($"\t\t<option value='b7'>{software_lang.TSReadLangs("LeftMenu", "left_storage")}</option>");
            PrintEngineList.Add($"\t\t<option value='b8'>{software_lang.TSReadLangs("LeftMenu", "left_network")}</option>");
            PrintEngineList.Add($"\t\t<option value='b9'>{software_lang.TSReadLangs("LeftMenu", "left_usb")}</option>");
            PrintEngineList.Add($"\t\t<option value='b10'>{software_lang.TSReadLangs("LeftMenu", "left_sound")}</option>");
            PrintEngineList.Add($"\t\t<option value='b11'>{software_lang.TSReadLangs("LeftMenu", "left_battery")}</option>");
            PrintEngineList.Add($"\t\t<option value='b12'>{software_lang.TSReadLangs("LeftMenu", "left_installed_drivers")}</option>");
            PrintEngineList.Add($"\t\t<option value='b13'>{software_lang.TSReadLangs("LeftMenu", "left_installed_services")}</option>");
            PrintEngineList.Add($"\t\t<option value='b14'>{software_lang.TSReadLangs("LeftMenu", "left_installed_apps")}</option>");
            PrintEngineList.Add($"\t\t<option value='b15'>{software_lang.TSReadLangs("PrintEngine", "pe_html_footer_select")}</option>");
            PrintEngineList.Add("\t</select>");
            // MAIN CONTAINER
            PrintEngineList.Add("\t<div id='main_container'>");
            // HEADER
            PrintEngineList.Add("\t\t<div class='b1 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<img src='" + print_html_glow_logo_url + "' alt='" + string.Format(software_lang.TSReadLangs("PrintEngine", "pe_logo"), Application.ProductName) + "'>");
            PrintEngineList.Add($"\t\t\t<div class='ts_box_text'>");
            PrintEngineList.Add($"\t\t\t\t<h2>{Application.ProductName} - {string.Format(software_lang.TSReadLangs("PrintEngine", "pe_report_title").ToUpper(), OS_ComputerName_V.Text)}</h2>");
            PrintEngineList.Add($"\t\t\t\t<span>{software_lang.TSReadLangs("PrintEngine", "pe_process_time") + " <label>" + DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss")}</label></span>");
            PrintEngineList.Add($"\t\t\t</div>");
            PrintEngineList.Add("\t\t</div>");
            // OS
            Print_engine_progress_update(1);
            PrintEngineList.Add("\t\t<div class='b2 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_os")}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemUser.Text}</span><span>{OS_SystemUser_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ComputerName.Text}</span><span>{OS_ComputerName_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SavedUser.Text}</span><span>{OS_SavedUser_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Name.Text}</span><span>{OS_Name_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Manufacturer.Text}</span><span>{OS_Manufacturer_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemVersion.Text}</span><span>{OS_SystemVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemArchitectural.Text}</span><span>{OS_SystemArchitectural_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_DeviceID.Text}</span><span>{OS_DeviceID_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Serial.Text}</span><span>{OS_Serial_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Country.Text}</span><span>{OS_Country_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_TimeZone.Text}</span><span>{OS_TimeZone_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_EncryptionType.Text}</span><span>{OS_EncryptionType_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemTime.Text}</span><span>{OS_SystemTime_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Install.Text}</span><span>{OS_Install_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemWorkTime.Text}</span><span>{OS_SystemWorkTime_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_LastBootTime.Text}</span><span>{OS_LastBootTime_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemLastShutDown.Text}</span><span>{OS_SystemLastShutDown_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_PrimaryOS.Text}</span><span>{OS_PrimaryOS_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_PortableOS.Text}</span><span>{OS_PortableOS_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_MouseWheelStatus.Text}</span><span>{OS_MouseWheelStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ScrollLockStatus.Text}</span><span>{OS_ScrollLockStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_NumLockStatus.Text}</span><span>{OS_NumLockStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_CapsLockStatus.Text}</span><span>{OS_CapsLockStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_FastBoot.Text}</span><span>{OS_FastBoot_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_WinPageFile.Text}</span><span>{OS_WinPageFile_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_TempWinPageFile.Text}</span><span>{OS_TempWinPageFile_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Hiberfil.Text}</span><span>{OS_Hiberfil_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_AVProgram.Text}</span><span>{OS_AVProgram_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_FirewallProgram.Text}</span><span>{OS_FirewallProgram_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_AntiSpywareProgram.Text}</span><span>{OS_AntiSpywareProgram_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_WinDefCoreIsolation.Text}</span><span>{OS_WinDefCoreIsolation_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_CA2023_Status.Text}</span><span>{OS_CA2023_Status_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_CA2023_Capable.Text}</span><span>{OS_CA2023_Capable_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_CA2023_Error.Text}</span><span>{OS_CA2023_Error_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ActivePower.Text}</span><span>{OS_ActivePower_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ActivePowerGUID.Text}</span><span>{OS_ActivePowerGUID_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ActivePowerScreenTimeOutP.Text}</span><span>{OS_ActivePowerScreenTimeOutP_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ActivePowerScreenTimeOutB.Text}</span><span>{OS_ActivePowerScreenTimeOutB_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ActivePowerSleepTimeP.Text}</span><span>{OS_ActivePowerSleepTimeP_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ActivePowerSleepTimeB.Text}</span><span>{OS_ActivePowerSleepTimeB_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_MSEdge.Text}</span><span>{OS_MSEdge_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_MSEdgeWebView.Text}</span><span>{OS_MSEdgeWebView_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_MSStoreVersion.Text}</span><span>{OS_MSStoreVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_MSOfficeVersion.Text}</span><span>{OS_MSOfficeVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_WinKey.Text}</span><span>{OS_WinKey_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_WinActiveChannel.Text}</span><span>{OS_WinActiveChannel_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_NETFrameworkVersion.Text}</span><span>{OS_NETFrameworkVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Minidump.Text}</span><span>{OS_Minidump_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_BSODDate.Text}</span><span>{OS_BSODDate_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Wallpaper.Text}</span><span>{OS_Wallpaper_V.Text}</span></li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t</div>");
            // MB
            Print_engine_progress_update(2);
            PrintEngineList.Add("\t\t<div class='b3 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_mb")}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_MotherBoardName.Text}</span><span>{MB_MotherBoardName_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_MotherBoardMan.Text}</span><span>{MB_MotherBoardMan_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SystemModelMan.Text}</span><span>{MB_SystemModelMan_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SystemModelFamily.Text}</span><span>{MB_SystemModelFamily_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SystemFamily.Text}</span><span>{MB_SystemFamily_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SystemModel.Text}</span><span>{MB_SystemModel_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_DeviceSerialNumber.Text}</span><span>{MB_DeviceSerialNumber_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_MotherBoardSerial.Text}</span><span>{MB_MotherBoardSerial_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SystemSKU.Text}</span><span>{MB_SystemSKU_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_Chipset.Text}</span><span>{MB_Chipset_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_BiosManufacturer.Text}</span><span>{MB_BiosManufacturer_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_BiosDate.Text}</span><span>{MB_BiosDate_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_BiosVersion.Text}</span><span>{MB_BiosVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SmBiosVersion.Text}</span><span>{MB_SmBiosVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_BiosMode.Text}</span><span>{MB_BiosMode_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_LastBIOSTime.Text}</span><span>{MB_LastBIOSTime_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SecureBoot.Text}</span><span>{MB_SecureBoot_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SecureBootCA2023.Text}</span><span>{MB_SecureBootCA2023_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMStatus.Text}</span><span>{MB_TPMStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMPhysicalVersion.Text}</span><span>{MB_TPMPhysicalVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMMan.Text}</span><span>{MB_TPMMan_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMManID.Text}</span><span>{MB_TPMManID_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMManVersion.Text}</span><span>{MB_TPMManVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMManFullVersion.Text}</span><span>{MB_TPMManFullVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMManPublisher.Text}</span><span>{MB_TPMManPublisher_V.Text}</span></li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t</div>");
            // CPU
            Print_engine_progress_update(3);
            PrintEngineList.Add("\t\t<div class='b4 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_cpu")}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Name.Text}</span><span>{CPU_Name_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Manufacturer.Text}</span><span>{CPU_Manufacturer_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Architectural.Text}</span><span>{CPU_Architectural_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_IntelME.Text}</span><span>{CPU_IntelME_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_NormalSpeed.Text}</span><span>{CPU_NormalSpeed_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_DefaultSpeed.Text}</span><span>{CPU_DefaultSpeed_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_L1.Text}</span><span>{CPU_L1_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_L2.Text}</span><span>{CPU_L2_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_L3.Text}</span><span>{CPU_L3_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_CoreCount.Text}</span><span>{CPU_CoreCount_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_LogicalCore.Text}</span><span>{CPU_LogicalCore_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Usage.Text}</span><span>{CPU_Usage_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Process.Text}</span><span>{CPU_Process_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Threads.Text}</span><span>{CPU_Threads_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Handles.Text}</span><span>{CPU_Handles_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_SocketDefinition.Text}</span><span>{CPU_SocketDefinition_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Family.Text}</span><span>{CPU_Family_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Virtualization.Text}</span><span>{CPU_Virtualization_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_HyperV.Text}</span><span>{CPU_HyperV_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_VMExtension.Text}</span><span>{CPU_VMExtension_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_SerialName.Text}</span><span>{CPU_SerialName_V.Text}</span></li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t</div>");
            // RAM
            Print_engine_progress_update(4);
            PrintEngineList.Add("\t\t<div class='b5 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_ram")}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_TotalRAM.Text}</span><span>{RAM_TotalRAM_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_UsageRAMCount.Text}</span><span>{RAM_UsageRAMCount_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_EmptyRamCount.Text}</span><span>{RAM_EmptyRamCount_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_TotalVirtualRam.Text}</span><span>{RAM_TotalVirtualRam_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_UsageVirtualRam.Text}</span><span>{RAM_UsageVirtualRam_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_EmptyVirtualRam.Text}</span><span>{RAM_EmptyVirtualRam_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_SlotStatus.Text}</span><span>{RAM_SlotStatus_V.Text}</span></li>");
            PrintEngineList.Add("\t\t\t</ul>");
            try{
                int ram_slot = RAM_Selector_List.Items.Count;
                for (int rs = 1; rs <= ram_slot; rs++){
                    RAM_Selector_List.SelectedIndex = rs - 1;
                    string[] ram_explode = RAM_Selector_List.SelectedItem.ToString().Split('-');
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{ram_explode[0].Trim()}</span>-<span>{ram_explode[1].Trim()}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_Amount.Text}</span><span>{RAM_Amount_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_Type.Text}</span><span>{RAM_Type_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_Frequency.Text}</span><span>{RAM_Frequency_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_Volt.Text}</span><span>{RAM_Volt_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_FormFactor.Text}</span><span>{RAM_FormFactor_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_Serial.Text}</span><span>{RAM_Serial_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_Manufacturer.Text}</span><span>{RAM_Manufacturer_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_BankLabel.Text}</span><span>{RAM_BankLabel_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_DataWidth.Text}</span><span>{RAM_DataWidth_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_BellekType.Text}</span><span>{RAM_BellekType_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{RAM_PartNumber.Text}</span><span>{RAM_PartNumber_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                RAM_Selector_List.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add("\t\t</div>");
            // GPU
            Print_engine_progress_update(5);
            PrintEngineList.Add("\t\t<div class='b6 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_gpu")}</h3>");
            try{
                int gpu_amount = GPU_Selector_List.Items.Count;
                for (int gpu_render = 1; gpu_render <= gpu_amount; gpu_render++){
                    GPU_Selector_List.SelectedIndex = gpu_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{software_lang.TSReadLangs("Gpu_Content", "gpu_c_gpu_print") + "</span><span>" + GPU_Selector_List.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_Selector.Text}</span><span>{GPU_Selector_List.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_Manufacturer.Text}</span><span>{GPU_Manufacturer_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_VRAM.Text}</span><span>{GPU_VRAM_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_Version.Text}</span><span>{GPU_Version_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_DriverDate.Text}</span><span>{GPU_DriverDate_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_Status.Text}</span><span>{GPU_Status_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_DeviceID.Text}</span><span>{GPU_DeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_DacType.Text}</span><span>{GPU_DacType_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_GraphicDriversName.Text}</span><span>{GPU_GraphicDriversName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_InfFileName.Text}</span><span>{GPU_InfFileName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_INFSectionFile.Text}</span><span>{GPU_INFSectionFile_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_CurrentColor.Text}</span><span>{GPU_CurrentColor_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                GPU_Selector_List.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add("\t\t\t<hr>");
            try{
                int screen_amount = GPU_MonitorSelector_List.Items.Count;
                for (int sa = 1; sa <= screen_amount; sa++){
                    GPU_MonitorSelector_List.SelectedIndex = sa - 1;
                    string[] gpu_monitor_explode = GPU_MonitorSelector_List.SelectedItem.ToString().Split('-');
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{gpu_monitor_explode[0].Trim()}</span>-<span>{gpu_monitor_explode[1].Trim()}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorUserFriendlyName.Text}</span><span>{GPU_MonitorUserFriendlyName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorManName.Text}</span><span>{GPU_MonitorManName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorProductCodeID.Text}</span><span>{GPU_MonitorProductCodeID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorSerialNumberID.Text}</span><span>{GPU_MonitorSerialNumberID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorConType.Text}</span><span>{GPU_MonitorConType_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorManfDate.Text}</span><span>{GPU_MonitorManfDate_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorManfDateWeek.Text}</span><span>{GPU_MonitorManfDateWeek_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorHID.Text}</span><span>{GPU_MonitorHID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorResLabel.Text}</span><span>{GPU_MonitorResLabel_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorVirtualRes.Text}</span><span>{GPU_MonitorVirtualRes_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorBounds.Text}</span><span>{GPU_MonitorBounds_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorWorking.Text}</span><span>{GPU_MonitorWorking_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_ScreenRefreshRate.Text}</span><span>{GPU_ScreenRefreshRate_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_ScreenBit.Text}</span><span>{GPU_ScreenBit_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorPrimary.Text}</span><span>{GPU_MonitorPrimary_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                GPU_MonitorSelector_List.SelectedIndex = 0;
            }catch (Exception) { }
            PrintEngineList.Add("\t\t</div>");
            // STORAGE
            Print_engine_progress_update(6);
            PrintEngineList.Add("\t\t<div class='b7 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_storage")}</h3>");
            PrintEngineList.Add($"\t\t\t\t<h4><span>{DISK_TTLP_L1.Text}</span></h4>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_TTLP_P1_L1.Text}</span><span>{DISK_TTLP_P1_L2.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_TTLP_P2_L1.Text}</span><span>{DISK_TTLP_P2_L2.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_TTLP_P3_L1.Text}</span><span>{DISK_TTLP_P3_L2.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_TTLP_P4_L1.Text}</span><span>{DISK_TTLP_P4_L2.Text}</span></li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t\t<hr>");
            try{
                int disk_amount = DISK_Selector_List.Items.Count;
                for (int disk_render = 1; disk_render <= disk_amount; disk_render++){
                    DISK_Selector_List.SelectedIndex = disk_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{software_lang.TSReadLangs("StorageContent", "se_c_disk_print") + "</span><span>" + DISK_Selector_List.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Selector.Text}</span><span>{DISK_Selector_List.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Model.Text}</span><span>{DISK_Model_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Man.Text}</span><span>{DISK_Man_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_VolumeID.Text}</span><span>{DISK_VolumeID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_VolumeName.Text}</span><span>{DISK_VolumeName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Firmware.Text}</span><span>{DISK_Firmware_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Serial.Text}</span><span>{DISK_Serial_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_VolumeSerial.Text}</span><span>{DISK_VolumeSerial_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Size.Text}</span><span>{DISK_Size_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_FreeSpace.Text}</span><span>{DISK_FreeSpace_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_FileSystem.Text}</span><span>{DISK_FileSystem_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_FormattingType.Text}</span><span>{DISK_FormattingType_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Type.Text}</span><span>{DISK_Type_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_DriveType.Text}</span><span>{DISK_DriveType_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_InterFace.Text}</span><span>{DISK_InterFace_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_PartitionCount.Text}</span><span>{DISK_PartitionCount_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_MediaLoaded.Text}</span><span>{DISK_MediaLoaded_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_MediaStatus.Text}</span><span>{DISK_MediaStatus_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Health.Text}</span><span>{DISK_Health_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Boot.Text}</span><span>{DISK_Boot_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Bootable.Text}</span><span>{DISK_Bootable_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_BitLockerStatus.Text}</span><span>{DISK_BitLockerStatus_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_BitLockerConversionStatus.Text}</span><span>{DISK_BitLockerConversionStatus_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_BitLockerEncryptMehod.Text}</span><span>{DISK_BitLockerEncryptMehod_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_DriveCompressed.Text}</span><span>{DISK_DriveCompressed_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                // SELECT DISK
                try{
                    int c_index = disk_volume_id_list.FindIndex(x => x.Contains(windows_disk));
                    DISK_Selector_List.SelectedIndex = c_index;
                    if (c_index == -1){
                        DISK_Selector_List.SelectedIndex = 0;
                    }
                }catch (Exception){
                    DISK_Selector_List.SelectedIndex = 0;
                }
            }catch (Exception){ }
            PrintEngineList.Add("\t\t</div>");
            // NETWORK
            Print_engine_progress_update(7);
            PrintEngineList.Add("\t\t<div class='b8 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_network")}</h3>");
            try{
                int net_amount = NET_Selector_List.Items.Count;
                PrintEngineList.Add("\t\t\t<ul>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LT_Device.Text}</span><span>{NET_LT_Device_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LT_BandWidth.Text}</span><span>{NET_LT_BandWidth_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LT_LocalIP.Text}</span><span>{NET_LT_LocalIP_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LT_GatewayIP.Text}</span><span>{NET_LT_GatewayIP_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LT_DL1.Text}</span><span>{NET_LT_DL2.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LT_UL1.Text}</span><span>{NET_LT_UL2.Text}</span></li>");
                PrintEngineList.Add("\t\t\t</ul>");
                PrintEngineList.Add("\t\t\t<hr>");
                for (int net_render = 1; net_render <= net_amount; net_render++){
                    NET_Selector_List.SelectedIndex = net_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{software_lang.TSReadLangs("Network_Content", "nk_c_network_print") + "</span><span>" + NET_Selector_List.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_Selector.Text}</span><span>{NET_Selector_List.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_MacAdress.Text}</span><span>{NET_MacAdress_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_NetMan.Text}</span><span>{NET_NetMan_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_DriverVersion.Text}</span><span>{NET_DriverVersion_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_DriverDate.Text}</span><span>{NET_DriverDate_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_ServiceName.Text}</span><span>{NET_ServiceName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_AdapterType.Text}</span><span>{NET_AdapterType_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_Physical.Text}</span><span>{NET_Physical_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_DeviceID.Text}</span><span>{NET_DeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_Guid.Text}</span><span>{NET_Guid_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_ConnectionType.Text}</span><span>{NET_ConnectionType_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_Dhcp_status.Text}</span><span>{NET_Dhcp_status_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_Dhcp_server.Text}</span><span>{NET_Dhcp_server_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_DHCPFirstIpTime.Text}</span><span>{NET_DHCPFirstIpTime_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_DHCPLastIpTime.Text}</span><span>{NET_DHCPLastIpTime_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LocalConSpeed.Text}</span><span>{NET_LocalConSpeed_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_IPv4Adress.Text}</span><span>{NET_IPv4Adress_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_IPv6Adress.Text}</span><span>{NET_IPv6Adress_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                PrintEngineList.Add("\t\t\t<ul>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_DNS1.Text}</span><span>{NET_DNS1_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_DNS2.Text}</span><span>{NET_DNS2_V.Text}</span></li>");
                PrintEngineList.Add("\t\t\t</ul>");
                NET_Selector_List.SelectedIndex = 1;
            }catch (Exception){ }
            PrintEngineList.Add("\t\t</div>");
            // USB
            Print_engine_progress_update(8);
            PrintEngineList.Add("\t\t<div class='b9 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_usb")}</h3>");
            try{
                int usb_con_amount = USB_Selector_List.Items.Count;
                for (int usb_con_render = 1; usb_con_render <= usb_con_amount; usb_con_render++){
                    USB_Selector_List.SelectedIndex = usb_con_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{software_lang.TSReadLangs("Usb_Content", "usb_c_controller_print") + "</span><span>" + USB_Selector_List.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_Selector.Text}</span><span>{USB_Selector_List.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConName.Text}</span><span>{USB_ConName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConMan.Text}</span><span>{USB_ConMan_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConDeviceID.Text}</span><span>{USB_ConDeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConPNPDeviceID.Text}</span><span>{USB_ConPNPDeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConDeviceStatus.Text}</span><span>{USB_ConDeviceStatus_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                PrintEngineList.Add("\t\t\t<hr>");
                USB_Selector_List.SelectedIndex = 0;
            }catch (Exception) { }
            try{
                int usb_amount = USB_DeviceSelector_List.Items.Count;
                for (int usb_render = 1; usb_render <= usb_amount; usb_render++){
                    USB_DeviceSelector_List.SelectedIndex = usb_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{software_lang.TSReadLangs("Usb_Content", "usb_c_device_print") + "</span><span>" + USB_DeviceSelector_List.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_DeviceSelector.Text}</span><span>{USB_DeviceSelector_List.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_DeviceName.Text}</span><span>{USB_DeviceName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_DeviceMan.Text}</span><span>{USB_DeviceMan_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_DriverVersion.Text}</span><span>{USB_DriverVersion_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_DriverDate.Text}</span><span>{USB_DriverDate_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_InfFile.Text}</span><span>{USB_InfFile_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_DeviceID.Text}</span><span>{USB_DeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_HardwareID.Text}</span><span>{USB_HardwareID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_DeviceGUID.Text}</span><span>{USB_DeviceGUID_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                USB_DeviceSelector_List.SelectedIndex = 0;
            }catch (Exception) { }
            PrintEngineList.Add("\t\t</div>");
            // SOUND
            Print_engine_progress_update(9);
            PrintEngineList.Add("\t\t<div class='b10 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_sound")}</h3>");
            try{
                int sound_amount = SOUND_Selector_List.Items.Count;
                for (int sound_render = 1; sound_render <= sound_amount; sound_render++){
                    SOUND_Selector_List.SelectedIndex = sound_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{software_lang.TSReadLangs("Sound_Content", "sound_c_print") + "</span><span>" + SOUND_Selector_List.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_Selector.Text}</span><span>{SOUND_Selector_List.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_DeviceName.Text}</span><span>{SOUND_DeviceName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_DeviceManufacturer.Text}</span><span>{SOUND_DeviceManufacturer_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_DriverVersion.Text}</span><span>{SOUND_DriverVersion_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_DriverDate.Text}</span><span>{SOUND_DriverDate_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_DeviceID.Text}</span><span>{SOUND_DeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_PNPDeviceID.Text}</span><span>{SOUND_PNPDeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_DeviceStatus.Text}</span><span>{SOUND_DeviceStatus_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                SOUND_Selector_List.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add("\t\t</div>");
            // BATTERY
            Print_engine_progress_update(10);
            PrintEngineList.Add("\t\t<div class='b11 ts_box_wrapper'>");
            PowerStatus power_status = SystemInformation.PowerStatus;
            String battery_charging = power_status.BatteryChargeStatus.ToString();
            if (battery_charging == "NoSystemBattery"){
                PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_battery")}</h3>");
                PrintEngineList.Add("\t\t\t<ul>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Status.Text}</span><span>{BATTERY_Status_V.Text}</span></li>");
                PrintEngineList.Add("\t\t\t</ul>");
            }else{
                PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_battery")}</h3>");
                PrintEngineList.Add("\t\t\t<ul>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Status.Text}</span><span>{BATTERY_Status_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Model.Text}</span><span>{BATTERY_Model_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Serial.Text}</span><span>{BATTERY_Serial_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Chemistry.Text}</span><span>{BATTERY_Chemistry_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_DesignCapacity.Text}</span><span>{BATTERY_DesignCapacity_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_FullChargeCapacity.Text}</span><span>{BATTERY_FullChargeCapacity_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_RemainingChargeCapacity.Text}</span><span>{BATTERY_RemainingChargeCapacity_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Voltage.Text}</span><span>{BATTERY_Voltage_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_ChargePower.Text}</span><span>{BATTERY_ChargePower_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_ChargeCurrent.Text}</span><span>{BATTERY_ChargeCurrent_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_DeChargePower.Text}</span><span>{BATTERY_DeChargePower_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_DeChargeCurrent.Text}</span><span>{BATTERY_DeChargeCurrent_V.Text}</span></li>");
                PrintEngineList.Add("\t\t\t</ul>");
            }
            PrintEngineList.Add("\t\t</div>");
            // OSD
            Print_engine_progress_update(11);
            PrintEngineList.Add("\t\t<div class='b12 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_installed_drivers")}</h3>");
            char[] split_osd = { ':' };
            string osd_header = software_lang.TSReadLangs("PrintEngine", "pe_driver_sorting");
            string[] osd_texts = osd_header.Split(split_osd);
            PrintEngineList.Add($"\t\t\t<h4><span>{osd_texts[0]}:</span><span>{osd_texts[1]}</span></h4>");
            try{
                PrintEngineList.Add("\t\t\t<ul>");
                foreach (DataGridViewRow row in OSD_DataMainTable.Rows){
                    if (row.IsNewRow) continue;
                    var cellValues = row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString() ?? string.Empty);
                    PrintEngineList.Add($"\t\t\t\t<li>{string.Join(" | ", cellValues)}</li>");
                }
                PrintEngineList.Add("\t\t\t</ul>");
            }catch (Exception){ }
            PrintEngineList.Add($"\t\t\t<h4><span>{software_lang.TSReadLangs("Osd_Content", "osd_total_installed_driver_count")}</span><span>{OSD_TYSS_V.Text}</span></h4>");
            PrintEngineList.Add("\t\t</div>");
            // SERVICE
            Print_engine_progress_update(12);
            PrintEngineList.Add("\t\t<div class='b13 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_installed_services")}</h3>");
            char[] split_service = { ':' };
            string service_header = software_lang.TSReadLangs("PrintEngine", "pe_service_sorting");
            string[] service_texts = service_header.Split(split_service);
            PrintEngineList.Add($"\t\t\t<h4><span>{service_texts[0]}:</span><span>{service_texts[1]}</span></h4>");
            try{
                PrintEngineList.Add("\t\t\t<ul>");
                foreach (DataGridViewRow row in SERVICE_DataMainTable.Rows){
                    if (row.IsNewRow) continue;
                    var cellValues = row.Cells.Cast<DataGridViewCell>().Select(c => c.Value?.ToString() ?? string.Empty);
                    PrintEngineList.Add($"\t\t\t\t<li>{string.Join(" | ", cellValues)}</li>");
                }
                PrintEngineList.Add("\t\t\t</ul>");
            }catch (Exception){ }
            PrintEngineList.Add($"\t\t\t<h4><span>{software_lang.TSReadLangs("Services_Content", "ss_total_installed_service_count")}</span><span>{SERVICE_TYS_V.Text}</span></h4>");
            PrintEngineList.Add("\t\t</div>");
            // INSTALLED APPS
            Print_engine_progress_update(13);
            PrintEngineList.Add("\t\t<div class='b14 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{software_lang.TSReadLangs("Header", "header_installed_apps")}</h3>");
            char[] split_apps = { ':' };
            string apps_header = software_lang.TSReadLangs("PrintEngine", "pe_instapps_sorting");
            string[] apps_texts = apps_header.Split(split_apps);
            PrintEngineList.Add($"\t\t\t<h4><span>{apps_texts[0]}:</span><span>{apps_texts[1]}</span></h4>");
            try{
                PrintEngineList.Add("\t\t\t<ul>");
                foreach (DataGridViewRow row in INSTAPPS_DataMainTable.Rows){
                    if (row.IsNewRow) continue;
                    var cellValues = row.Cells.Cast<DataGridViewCell>().Skip(1).Select(c => c.Value?.ToString() ?? string.Empty);
                    PrintEngineList.Add($"\t\t\t\t<li>{string.Join(" | ", cellValues)}</li>");
                }
                PrintEngineList.Add("\t\t\t</ul>");
            }catch (Exception){ }
            PrintEngineList.Add($"\t\t\t<h4><span>{software_lang.TSReadLangs("Instapps_Content", "ia_total_installed_apps_count")}</span><span>{INSTAPPS_TYUS_V.Text}</span></h4>");
            PrintEngineList.Add("\t\t</div>");
            // FOOTER V1
            PrintEngineList.Add("\t\t<div class='b15 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{string.Format(software_lang.TSReadLangs("PrintEngine", "pe_html_footer"), Application.ProductName.ToUpper())}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li>{Application.ProductName + " " + software_lang.TSReadLangs("PrintEngine", "pe_version") + " <span>" + TS_VersionEngine.TS_SofwareVersion(1)}</span></li>");
            PrintEngineList.Add("\t\t\t\t<li>" + TS_SoftwareCopyrightDate.ts_scd_preloader + "</li>");
            PrintEngineList.Add($"\t\t\t\t<li>{software_lang.TSReadLangs("PrintEngine", "pe_website") + " " + "<a target='_blank' href='" + TS_LinkSystem.website_link + "'>" + TS_LinkSystem.website_link + "</a>"}</li>");
            PrintEngineList.Add($"\t\t\t\t<li>{software_lang.TSReadLangs("PrintEngine", "pe_github") + " " + "<a target='_blank' href='" + TS_LinkSystem.github_link + "'>" + TS_LinkSystem.github_link + "</a>"}</li>");
            PrintEngineList.Add($"\t\t\t\t<li>{software_lang.TSReadLangs("PrintEngine", "pe_donate") + " " + "<a target='_blank' href='" + TS_LinkSystem.ts_donate + "'>" + TS_LinkSystem.ts_donate + "</a>"}</li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t</div>");
            // MAIN CONTAINER END
            PrintEngineList.Add("\t</div>");
            // SCROLL TOP SCRIPT
            PrintEngineList.Add("<script>");
            PrintEngineList.Add("\tdocument.querySelector('select').addEventListener('change', function(){ var ts_d_id = this.value; var targetElement = document.querySelector('.' + ts_d_id); window.scrollTo({ top: targetElement.offsetTop, behavior: 'smooth' }); });");
            PrintEngineList.Add("\tvar ts_output = []; document.querySelectorAll('select option').forEach(function(option){ ts_output.push(option.value); });");
            PrintEngineList.Add("\twindow.addEventListener('scroll', function(){ if (!document.documentElement.classList.contains('animated')){ var ts_filtered = ts_output.filter(function(ts){ var targetElement = document.querySelector('.' + ts); return window.scrollY >= targetElement.offsetTop; }); if (ts_filtered.length > 0){ document.querySelector('select').value = ts_filtered[ts_filtered.length - 1]; } } });");
            PrintEngineList.Add("\tconst ts_scroll_top = document.querySelector('.ts_scroll_top'); window.addEventListener('scroll', function(){ ts_scroll_top.classList.toggle('active', window.scrollY > 350); });");
            PrintEngineList.Add("\tfunction ts_scroll_to_up(){ window.scrollTo({ top: 0, behavior: 'smooth' }); }");
            PrintEngineList.Add("</script>");
            // DOCUMENT END
            PrintEngineList.Add("</body>");
            PrintEngineList.Add("</html>");
            // FOOTER V2
            Print_engine_progress_update(14);
            using (SaveFileDialog save_engine = new SaveFileDialog{
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = Application.ProductName + " - " + software_lang.TSReadLangs("PrintEngine", "pe_save_directory"),
                DefaultExt = "html",
                FileName = Application.ProductName + " - " + string.Format(software_lang.TSReadLangs("PrintEngine", "pe_save_name"), OS_ComputerName_V.Text),
                Filter = software_lang.TSReadLangs("PrintEngine", "pe_save_html") + " (*.html)|*.html"
            }){
                if (save_engine.ShowDialog() == DialogResult.OK){
                    string combinedText = String.Join(Environment.NewLine, PrintEngineList);
                    File.WriteAllText(save_engine.FileName, combinedText);
                    DialogResult glow_print_engine_query = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("PrintEngine", "pe_save_success") + "\n\n" + software_lang.TSReadLangs("PrintEngine", "pe_save_info_open"), Application.ProductName, save_engine.FileName));
                    if (glow_print_engine_query == DialogResult.Yes){
                        Process.Start(save_engine.FileName);
                    }
                }
                PrintEngineList.Clear();
                Print_after_mode();
            }
        }
        // TS TOOL LAUNCHER MODULE
        // ======================================================================================================
        private void TSToolLauncher<T>(string formName, string langKey) where T : Form, new(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                T tool = new T{ Name = formName };
                if (Application.OpenForms[formName] == null){
                    tool.Show();
                }else{
                    if (Application.OpenForms[formName].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[formName].WindowState = FormWindowState.Normal;
                    }
                    string public_message;
                    public_message = string.Format(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification"), software_lang.TSReadLangs("HeaderTools", langKey));
                    if (formName == "glow_about"){
                        public_message = string.Format(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification"), software_lang.TSReadLangs("HeaderMenu", langKey));
                    }else if (formName == "glow_wallpaper_preview_tool"){
                        public_message = string.Format(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification"), software_lang.TSReadLangs("WallpaperPreviewTool", langKey));
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, public_message);
                    Application.OpenForms[formName].Activate();
                }
            }catch (Exception){ }
        }
        // SFC AND DISM AUTO TOOL
        // ======================================================================================================
        private void SFCandDISMAutoTool_Click(object sender, EventArgs e){
            TSToolLauncher<GlowSFCandDISMAutoTool>("glow_sfc_and_dism_tool", "ht_sfc_and_dism_tool");
        }
        // CACHE CLEANUP TOOL
        // ======================================================================================================
        private void CacheCleaningTool_Click(object sender, EventArgs e){
            TSToolLauncher<GlowCacheCleanupTool>("glow_cache_cleanup_tool", "ht_cache_cleanup_tool");
        }
        // CPU BENCH TOOL
        // ======================================================================================================
        private void BenchCPUTool_Click(object sender, EventArgs e){
            TSToolLauncher<GlowBenchCPU>("glow_bench_cpu_tool", "ht_bench_cpu");
        }
        // RAM BENCH TOOL
        // ======================================================================================================
        private void BenchRAMTool_Click(object sender, EventArgs e){
            TSToolLauncher<GlowBenchMemory>("glow_bench_ram_tool", "ht_bench_ram");
        }
        // DISK BENCH TOOL
        // ======================================================================================================
        private void BenchDiskTool_Click(object sender, EventArgs e){
            TSToolLauncher<GlowBenchDisk>("glow_bench_disk_tool", "ht_bench_disk");
        }
        // SCREEN OVERLAY
        // ======================================================================================================
        private void ScreenOverlayTool_Click(object sender, EventArgs e){
            TSToolLauncher<GlowOverlay>("glow_screen_overlay_tool", "ht_overlay");
        }
        // DNS TEST TOOL
        // ======================================================================================================
        private void DnsTestTool_Click(object sender, EventArgs e){
            TSToolLauncher<GlowDNSTestTool>("glow_dns_test_tool", "ht_dns_test_tool");
        }
        // QUICK ACCESS TOOL
        // ======================================================================================================
        private void QuickAccessTool_Click(object sender, EventArgs e){
            TSToolLauncher<GlowQuickAccessTool>("glow_quick_access_tool", "ht_quick_access_tool");
        }
        // NETWORK FIX TOOL
        // ======================================================================================================
        private void NetworkFixTool_Click(object sender, EventArgs e){
            TSToolLauncher<GlowNetworkFixTool>("glow_network_fix_tool", "ht_network_fix_tool");
        }
        // SHOW WIFI PASSWORD TOOL
        // ======================================================================================================
        private void ShowWiFiPasswordTool_Click(object sender, EventArgs e){
            TSToolLauncher<GlowShowWiFiPasswordTool>("glow_show_wifi_password_tool", "ht_show_wifi_password_tool");
        }
        // BLUETOOTH FINDER TOOL
        // ======================================================================================================
        private void BluetoothFinderToolToolStripMenuItem_Click(object sender, EventArgs e){
            TSToolLauncher<GlowBluetoothFinderTool>("glow_bluetooth_finder_tool", "ht_bluetooth_finder_tool");
        }
        // SYSTEM ID GENERATOR TOOL
        // ======================================================================================================
        private void SystemIdGeneratorTool_Click(object sender, EventArgs e){
            TSToolLauncher<GlowSystemIDGenerator>("glow_system_id_generator_tool", "ht_system_id_generator_tool");
        }
        // MONITOR TEST TOOL
        // ======================================================================================================
        private void MonitorDeadPixelTestTool_Click(object sender, EventArgs e){
            monitor_engine_mode = 0;
            MonitorStartEnginePending();
        }
        private void MonitorDynamicRangeTestTool_Click(object sender, EventArgs e){
            monitor_engine_mode = 1;
            MonitorStartEnginePending();
        }
        private void MonitorStartEnginePending(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                if (monitor_engine_mode == 0){
                    var warningMessage = string.Format(software_lang.TSReadLangs("MonitorTestTool", "mtt_epilepsy_warning"), "\n\n", software_lang.TSReadLangs("HeaderTools", "ht_monitor_test_dead_pixel"), "\n\n");
                    DialogResult mse_warning = TS_MessageBoxEngine.TS_MessageBox(this, 6, warningMessage);
                    if (mse_warning == DialogResult.Yes)
                        MonitorStartEngineSelect();
                }else{
                    MonitorStartEngineSelect();
                }
            }catch (Exception){ }
        }
        private void MonitorStartEngineSelect(){
            string glow_tool_name = monitor_engine_mode == 0 ? "glow_monitor_test_engine_dead_pixel" : "glow_monitor_test_engine_dynamic_range";
            string toolKey = monitor_engine_mode == 0 ? "ht_monitor_test_dead_pixel" : "ht_monitor_test_dynamic_range";
            TSToolLauncher<GlowMonitorTestEngine>(glow_tool_name, toolKey);
        }
        // MONITOR STUCK PIXEL FIXER TOOL
        // ======================================================================================================
        private void MonitorStuckPixelFixerToolToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                DialogResult mspf_open_warning = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("MonitorStuckPixelFixerTool", "mspf_open_warning"), "\n\n", "\n\n", "\n\n"));
                if (mspf_open_warning == DialogResult.Yes)
                    TSToolLauncher<GlowStuckPixelFixerTool>("glow_monitor_stuck_pixel_fixer", "ht_stuck_pixel_fixer_tool");
            }catch (Exception){ }
        }
        // DONATE LINK
        // ======================================================================================================
        private void DonateToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                Process.Start(new ProcessStartInfo(TS_LinkSystem.ts_donate) { UseShellExecute = true });
            }catch (Exception){ }
        }
        // TS WIZARD
        // ======================================================================================================
        private void TSWizardToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                string baseDir = Path.Combine(Directory.GetParent(Directory.GetParent(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)).FullName).FullName);
                string ts_wizard_path = Ts_wizard_starter_mode().Select(name => Path.Combine(baseDir, name)).FirstOrDefault(File.Exists);
                //
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                //
                if (ts_wizard_path != null){
                    if (!Software_operation_controller(Path.GetDirectoryName(ts_wizard_path))){
                        Process.Start(new ProcessStartInfo{ FileName = ts_wizard_path, WorkingDirectory = Path.GetDirectoryName(ts_wizard_path) });
                    }else{
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("HeaderHelp", "header_help_info_notification"), ts_wizard_name));
                    }
                }else{
                    DialogResult ts_wizard_query = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("TSWizard", "tsw_content"), software_lang.TSReadLangs("HeaderMenu", "header_menu_ts_wizard"), Application.CompanyName, "\n\n", Application.ProductName, Application.CompanyName, "\n\n"), string.Format("{0} - {1}", Application.ProductName, ts_wizard_name));
                    if (ts_wizard_query == DialogResult.Yes){
                        Process.Start(new ProcessStartInfo(TS_LinkSystem.ts_wizard) { UseShellExecute = true });
                    }
                }
            }catch (Exception){ }
        }
        // ABOUT
        // ======================================================================================================
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e){
            TSToolLauncher<GlowAbout>("glow_about", "header_menu_about");
        }
        // EXIT
        // ======================================================================================================
        private void CancelAllTasks(){
            try{
                if (ts_token_engine_stopper) return;
                Program.TS_TokenEngine?.Cancel();
                ts_token_engine_stopper = true;
                if (Program.debug_mode){
                    Console.WriteLine("<---------------------------->");
                    Console.WriteLine("<--- ALL TASKS TERMINATED --->");
                    Console.WriteLine("<---------------------------->");
                }
            }catch (Exception){ }
        }
        private void Software_exit(){ loop_status = false; CancelAllTasks(); Application.Exit(); }
        private void Glow_FormClosing(object sender, FormClosingEventArgs e){
            if (CPUbenchMode || DISKbenchMode || RAMbenchMode || BTfinderMode || SFCandDISMprocessStatus){
                e.Cancel = true;
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("GToolsMessage", "gtm_all_prs_msg"));
            }else{
                Software_exit();
            }
        }
    }
}