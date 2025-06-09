// ======================================================================================================
// Glow - System Analysis Software
// © Copyright 2019-2025, Eray Türkay.
// Project Type: Open Source
// License: MIT License
// Website: https://www.turkaysoftware.com/glow
// GitHub: https://github.com/turkaysoftware/glow
// ======================================================================================================

using System;
// File and I/O Operations
using System.IO;
using System.IO.Compression;
// Network and Protocol Operations
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
// Graphics and User Interface Operations
using System.Drawing;
using System.Windows.Forms;
// Language and Culture Settings
using System.Globalization;
// Multithreading and Parallel Processing
using System.Threading;
using System.Threading.Tasks;
// Reflection and Runtime Operations
using System.Reflection;
using System.Runtime.InteropServices;
// Diagnostics and Management Tools
using System.Diagnostics;
using System.Management;
// General Collections and LINQ Queries
using System.Collections.Generic;
using System.Linq;
// Windows-based Application Development
using Microsoft.Win32;
using Microsoft.VisualBasic.Devices;
// Component Modeling
using System.ComponentModel;
// Text Regular Expressions
using System.Text.RegularExpressions;
// Glow Modules & UI Engines
using Glow.glow_tools;
using static Glow.TSModules;

namespace Glow{
    public partial class Glow : Form{
        public Glow(){
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //
            try{
                var search_os = new ManagementObjectSearcher("root\\CIMV2", "SELECT Caption, SystemDrive FROM Win32_OperatingSystem");
                var os_info = search_os.Get().Cast<ManagementObject>().FirstOrDefault();
                if (os_info != null){
                    string os_name = os_info["Caption"]?.ToString();
                    windows_mode = os_name?.ToLower().Contains("windows 11") == true ? 1 : 0;
                    //
                    string system_drive = os_info["SystemDrive"]?.ToString();
                    windows_disk = !string.IsNullOrEmpty(system_drive) ? (system_drive[0].ToString().ToUpper() + @":\").Trim() : @"C:\".Trim();
                }
            }catch (Exception){ }
        }
        // GLOBAL VARIABLES
        // ======================================================================================================
        public static string lang, lang_path, windows_disk = @"C:\";
        public static int theme, monitor_engine_mode;
        // VARIABLES
        // ======================================================================================================
        int menu_btns = 1, menu_rp = 1, initial_status, hiding_status, hiding_mode_wrapper, windows_mode = 0;
        bool loop_status = true, laptop_mode = false, ts_token_engine_stopper = false;
        string wp_rotate;
        // VISIBLE MODE DYNAMIC STAR
        // ======================================================================================================
        static List<int> vn_range = new List<int>(){ 10, 24 };
        static Random vis_m_property = new Random();
        // ======================================================================================================
        // UI COLORS
        List<Color> btn_colors_active = new List<Color>(){ Color.Transparent };
        static List<Color> header_colors = new List<Color>() { Color.Transparent, Color.Transparent };
        // HEADER SETTINGS
        // ======================================================================================================
        private class HeaderMenuColors : ToolStripProfessionalRenderer{
            public HeaderMenuColors() : base(new HeaderColors()){ }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e){ e.ArrowColor = header_colors[1]; base.OnRenderArrow(e); }
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
        // LOAD SOFTWARE SETTINGS
        // ======================================================================================================
        private void RunSoftwareEngine(){
            HeaderMenu.Cursor = Cursors.Hand;
            //
            var rotateButtons = new[] {
                /*OS_RotateBtn,*/ MB_RotateBtn, CPU_RotateBtn, RAM_RotateBtn, GPU_RotateBtn,
                DISK_RotateBtn, NET_RotateBtn, USB_RotateBtn, SOUND_RotateBtn,
                BATTERY_RotateBtn, OSD_RotateBtn, SERVICES_RotateBtn, PRINT_RotateBtn
            };
            foreach (var r_btns in rotateButtons) r_btns.Enabled = false;
            //
            var rotatePages = new[] {
                /*(Control)OS,*/ (Control)MB, (Control)CPU, (Control)RAM, (Control)GPU,
                (Control)DISK, (Control)NETWORK, (Control)USB, (Control)SOUND,
                (Control)BATTERY, (Control)OSD, (Control)GSERVICE, (Control)PRINT
            };
            foreach (var r_pages in rotatePages) r_pages.Enabled = false;
            // INSTALLED DRIVERS
            // ======================================================================================================
            for (int i = 0; i < 6; i++) OSD_DataMainTable.Columns.Add($"osd_file_{i}", "osd_variable");
            foreach (DataGridViewColumn OSD_Column in OSD_DataMainTable.Columns) OSD_Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            // SERVICES
            // ======================================================================================================
            for (int i = 0; i < 6; i++) SERVICE_DataMainTable.Columns.Add($"ss_file_{i}", "ss_variable");
            foreach (DataGridViewColumn SERVICE_Column in SERVICE_DataMainTable.Columns) SERVICE_Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            // ALL DGV AND PANEL WIDTH
            // ======================================================================================================
            int[] columnWidths = { 160, 120, 120, 120, 100, 100 };
            for (int i = 0; i < columnWidths.Length; i++) OSD_DataMainTable.Columns[i].Width = columnWidths[i];
            for (int i = 0; i < columnWidths.Length; i++) SERVICE_DataMainTable.Columns[i].Width = columnWidths[i];
            // OSD AND SERVICE CLEAR BTN DPI HEIGHT
            // ======================================================================================================
            OSD_TextBoxClearBtn.Height = OSD_TextBox.Height;
            SERVICE_TextBoxClearBtn.Height = SERVICE_TextBox.Height;
            // DGV DOUBLE BUFFER
            // ======================================================================================================
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, OSD_DataMainTable, new object[]{ true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, SERVICE_DataMainTable, new object[]{ true });
            // THEME - LANG - VIEW MODE PRELOADER
            // ======================================================================================================
            TSSettingsSave software_read_settings = new TSSettingsSave(ts_sf);
            //
            int theme_mode = int.TryParse(TS_String_Encoder(software_read_settings.TSReadSettings(ts_settings_container, "ThemeStatus")), out int the_status) ? the_status : 1;
            theme_engine(theme_mode);
            darkThemeToolStripMenuItem.Checked = theme_mode == 0;
            lightThemeToolStripMenuItem.Checked = theme_mode == 1;
            //
            string lang_mode = TS_String_Encoder(software_read_settings.TSReadSettings(ts_settings_container, "LanguageStatus"));
            var languageFiles = new Dictionary<string, (object langResource, ToolStripMenuItem menuItem, bool fileExists)>{
                { "zh", (ts_lang_zh, chineseToolStripMenuItem, File.Exists(ts_lang_zh)) },
                { "en", (ts_lang_en, englishToolStripMenuItem, File.Exists(ts_lang_en)) },
                { "fr", (ts_lang_fr, frenchToolStripMenuItem, File.Exists(ts_lang_fr)) },
                { "de", (ts_lang_de, germanToolStripMenuItem, File.Exists(ts_lang_de)) },
                { "it", (ts_lang_it, italianToolStripMenuItem, File.Exists(ts_lang_it)) },
                { "ko", (ts_lang_ko, koreanToolStripMenuItem, File.Exists(ts_lang_ko)) },
                { "pt", (ts_lang_pt, portugueseToolStripMenuItem, File.Exists(ts_lang_pt)) },
                { "ru", (ts_lang_ru, russianToolStripMenuItem, File.Exists(ts_lang_ru)) },
                { "es", (ts_lang_es, spanishToolStripMenuItem, File.Exists(ts_lang_es)) },
                { "tr", (ts_lang_tr, turkishToolStripMenuItem, File.Exists(ts_lang_tr)) },
            };
            foreach (var langLoader in languageFiles){ langLoader.Value.menuItem.Enabled = langLoader.Value.fileExists; }
            var (langResource, selectedMenuItem, _) = languageFiles.ContainsKey(lang_mode) ? languageFiles[lang_mode] : languageFiles["en"];
            lang_engine(Convert.ToString(langResource), lang_mode);
            selectedMenuItem.Checked = true;
            //
            string initial_mode = TS_String_Encoder(software_read_settings.TSReadSettings(ts_settings_container, "InitialStatus"));
            initial_status = int.TryParse(initial_mode, out int ini_status) && (ini_status == 0 || ini_status == 1) ? ini_status : 0;
            WindowState = initial_status == 1 ? FormWindowState.Maximized : FormWindowState.Normal;
            windowedToolStripMenuItem.Checked = initial_status == 0;
            fullScreenToolStripMenuItem.Checked = initial_status == 1;
            //
            string hiding_mode = TS_String_Encoder(software_read_settings.TSReadSettings(ts_settings_container, "HidingStatus"));
            hiding_status = int.TryParse(hiding_mode, out int hid_status) && (hid_status == 0 || hid_status == 1) ? hid_status : 0;
            hiding_mode_wrapper = hiding_status;
            hidingModeOffToolStripMenuItem.Checked = hiding_status == 0;
            hidingModeOnToolStripMenuItem.Checked = hiding_status == 1;
            //
            software_load_tasks();
        }
        // GLOW TASK ALL PROCESS
        // ======================================================================================================
        private async void software_load_tasks(){
            try{
                var cancellationToken = Program.TS_TokenEngine.Token;
                //
                async Task RunTaskAsync(Func<Task> get_action, CancellationToken run_token){
                    if (!run_token.IsCancellationRequested){
                        await get_action();
                    }
                }
                //
                var taskList = new List<Task>{
                    RunTaskAsync(() => Task.Run(() => os(), cancellationToken), cancellationToken),
                    RunTaskAsync(() => Task.Run(() => mb(), cancellationToken), cancellationToken),
                    RunTaskAsync(() => Task.Run(() => cpu(), cancellationToken), cancellationToken),
                    RunTaskAsync(() => Task.Run(() => ram(), cancellationToken), cancellationToken),
                    RunTaskAsync(() => Task.Run(() => gpu(), cancellationToken), cancellationToken),
                    RunTaskAsync(() => Task.Run(() => disk(), cancellationToken), cancellationToken),
                    RunTaskAsync(() => Task.Run(() => network(), cancellationToken), cancellationToken),
                    RunTaskAsync(() => Task.Run(() => usb(), cancellationToken), cancellationToken),
                    RunTaskAsync(() => Task.Run(() => sound(), cancellationToken), cancellationToken)
                };
                //
                try{
                    PowerStatus power_status = SystemInformation.PowerStatus;
                    bool isBatteryPresent = power_status.BatteryChargeStatus != BatteryChargeStatus.NoSystemBattery;
                    laptop_mode = isBatteryPresent;
                    //
                    if (isBatteryPresent){
                        battery_visible_on(); // Laptop
                        taskList.Add(RunTaskAsync(() => Task.Run(() => battery(), cancellationToken), cancellationToken));
                        taskList.Add(RunTaskAsync(() => Task.Run(() => laptop_bg_process(), cancellationToken), cancellationToken));
                    }else{
                        battery_visible_off();
                        BATTERY_RotateBtn.Enabled = true;
                        ((Control)BATTERY).Enabled = true;
                        if (Program.debug_mode){
                            Console.WriteLine("<--- Battery Section Loaded --->");
                        }
                    }
                }catch (Exception){ }
                //
                try{
                    taskList.Add(RunTaskAsync(() => osd(), cancellationToken));
                    taskList.Add(RunTaskAsync(() => gs_services(), cancellationToken));
                }catch (Exception){ }
                //
                taskList.Add(RunTaskAsync(() => Task.Run(() => os_bg_process(), cancellationToken), cancellationToken));
                taskList.Add(RunTaskAsync(() => Task.Run(() => processor_bg_process(), cancellationToken), cancellationToken));
                taskList.Add(RunTaskAsync(() => Task.Run(() => cpu_usage_process(), cancellationToken), cancellationToken));
                taskList.Add(RunTaskAsync(() => Task.Run(() => ram_bg_process(), cancellationToken), cancellationToken));
                taskList.Add(RunTaskAsync(() => Task.Run(() => live_network_process(), cancellationToken), cancellationToken));
                taskList.Add(RunTaskAsync(() => Task.Run(() => print_mode_add(), cancellationToken), cancellationToken));
                //
                MainContent.Invoke(new Action(() => {
                    MainContent.SelectedTab = SOUND;
                    MainContent.SelectedTab = OS;
                }));
                //
                try{
                    await Task.WhenAll(taskList);
                    //
                    ((Control)PRINT).Enabled = true;
                    PRINT_RotateBtn.Enabled = true;
                    //
                    if (Program.debug_mode){
                        Console.WriteLine("<--- Export Section Loaded --->");
                        Console.WriteLine("<--------------------------->");
                        Console.WriteLine("<--- ALL TASKS COMPLETED --->");
                        Console.WriteLine("<--------------------------->");
                    }
                }catch (Exception ex){
                    if (Program.debug_mode){
                        Console.WriteLine("<--------------------------------------------->");
                        Console.WriteLine($"Error in one or more tasks: {ex.Message}");
                        Console.WriteLine("<--------------------------------------------->");
                    }
                }
            }catch (Exception){ }
        }
        // SOFTWARE LOAD
        // ======================================================================================================
        private void Glow_Load(object sender, EventArgs e){ 
            Text = TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode); 
            // GLOW LAUNCH PROCESS
            // ====================================
            RunSoftwareEngine();
            // CHECK UPDATE
            // ====================================
            StartSoftwareUpdateCheck(0);
        }
        #region OS_Section
        // OPERATING SYSTEM
        // ======================================================================================================
        List<string> minidump_files_list = new List<string>();
        List<string> minidump_files_date_list = new List<string>();
        private void os(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher search_pfu = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PageFileUsage");
            ManagementObjectSearcher search_av = new ManagementObjectSearcher("root\\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
            ManagementObjectSearcher search_fw = new ManagementObjectSearcher("root\\SecurityCenter2", "SELECT * FROM FirewallProduct");
            ManagementObjectSearcher search_as = new ManagementObjectSearcher("root\\SecurityCenter2", "SELECT * FROM AntiSpywareProduct");
            ManagementObjectSearcher search_desktop = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Desktop");
            try{
                // SYSTEM USER
                OS_SystemUser_V.Text = Environment.UserName;
            }catch (Exception){ }
            try{
                // PC NAME
                OS_ComputerName_V.Text = Dns.GetHostName();
            }catch (Exception){ }
            foreach (ManagementObject query_os_rotate in search_os.Get()){
                try{
                    // REGISTERED USER
                    string os_saved_user = Convert.ToString(query_os_rotate["RegisteredUser"]).Trim();
                    if (hiding_mode_wrapper != 1){
                        OS_SavedUser_V.Text = os_saved_user;
                    }else{
                        OS_SavedUser_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})";
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
                    string os_version_display_version = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "DisplayVersion", "").ToString().Trim();
                    string os_version_release_id = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId", "").ToString().Trim();
                    if (os_version_display_version != string.Empty && os_version_release_id != string.Empty && os_version_display_version != "" && os_version_release_id != ""){
                        OS_SystemVersion_V.Text = os_version_display_version + " - " + os_version_release_id;
                    }else{
                        OS_SystemVersion_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                    }
                }catch (Exception){ }
                try{
                    // OS BUILD NUMBER
                    object os_build_num = query_os_rotate["Version"];
                    OS_Build_V.Text = os_build_num.ToString();
                }catch (Exception){ }
                try{
                    // OS SYSTEM BUILD
                    string os_system_current_build = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentBuild", "").ToString().Trim();
                    string os_system_ubr = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "UBR", "").ToString().Trim();
                    if (os_system_current_build != string.Empty && os_system_ubr != string.Empty && os_system_current_build != "" && os_system_ubr != ""){
                        OS_SystemBuild_V.Text = os_system_current_build + "." + os_system_ubr;
                    }else{
                        OS_SystemBuild_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                    }
                }catch (Exception){ }
                try{
                    // OS ARCHITECTURE
                    string system_bit = Regex.Replace(Convert.ToString(query_os_rotate["OSArchitecture"]), @"\D", string.Empty).Trim();
                    OS_SystemArchitectural_V.Text = system_bit + " " + TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bit")) + " - " + string.Format("(x{0})", system_bit);
                }catch (Exception){ }
                try{
                    // OS FAMILY
                    OS_Family_V.Text = new ComputerInfo().OSPlatform.Trim();
                }catch (Exception){ }
                try{
                    // OS DEVICE ID
                    string os_device_id = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\SQMClient", "MachineId", "").ToString().Trim();
                    if (os_device_id != "" && os_device_id != string.Empty){
                        string os_device_id_replace_1 = os_device_id.Replace("{", string.Empty);
                        string os_device_id_replace_2 = os_device_id_replace_1.Replace("}", string.Empty);
                        if (hiding_mode_wrapper != 1){
                            OS_DeviceID_V.Text = os_device_id_replace_2;
                        }else{
                            OS_DeviceID_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})";
                        }
                    }else{
                        OS_DeviceID_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                    }
                }catch (Exception){ }
                try{
                    // OS SERIAL
                    string os_serial = Convert.ToString(query_os_rotate["SerialNumber"]).Trim();
                    if (hiding_mode_wrapper != 1){
                        OS_Serial_V.Text = os_serial;
                    }else{
                        OS_Serial_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})";
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
                    // OS CHARACTER SET
                    object os_code_set = query_os_rotate["CodeSet"];
                    OS_CharacterSet_V.Text = os_code_set.ToString() + " - " + TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_universal_character_set"));
                }catch (Exception){ }
                try{
                    // OS ENCRYPTION BIT VALUE
                    OS_EncryptionType_V.Text = Convert.ToString(query_os_rotate["EncryptionLevel"]) + " " + TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bit"));
                }catch (Exception){ }
                try{
                    // WINDOWS DIRECTORY
                    object windows_dir = query_os_rotate["WindowsDirectory"];
                    OS_SystemRootIndex_V.Text = windows_dir.ToString().Replace("WINDOWS", "Windows") + @"\";
                }catch (Exception){ }
                try{
                    // BUILD PARTITON
                    object system_yapi_partition = query_os_rotate["SystemDirectory"];
                    OS_SystemBuildPart_V.Text = system_yapi_partition.ToString().Replace("WINDOWS", "Windows") + @"\";
                }catch (Exception){ }
                try{
                    // OS LAST BOOT
                    string last_bt = Convert.ToString(query_os_rotate["LastBootUpTime"]);
                    DateTime last_time = DateTime.ParseExact(last_bt.Substring(0, 14), "yyyyMMddHHmmss", null);
                    OS_LastBootTime_V.Text = $"{last_time:dd.MM.yyyy} - {last_time:HH:mm:ss}";
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
                    // PORTABLE OS STATUS
                    bool system_portable_status = Convert.ToBoolean(query_os_rotate["PortableOperatingSystem"]);
                    if (system_portable_status == true){
                        OS_PortableOS_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_yes"));
                    }else if (system_portable_status == false){
                        OS_PortableOS_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_no"));
                    }
                }catch (Exception){ }
                try{
                    // FASTBOOT STATUS
                    using (RegistryKey get_fastboot_status = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Power")){
                        if (get_fastboot_status != null){
                            object fastboot_status = get_fastboot_status.GetValue("HiberbootEnabled");
                            if (fastboot_status != null){
                                if (fastboot_status.ToString().Trim() == "1"){
                                    OS_FastBoot_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_fastboot_active"));
                                }else{
                                    OS_FastBoot_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_fastboot_deactive"));
                                }
                            }else{
                                OS_FastBoot_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                            }
                        }else{
                            OS_FastBoot_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                        }
                        get_fastboot_status.Close();
                    }
                }catch (Exception){ }
                try{
                    // BOOT PARTITION
                    string boot_device = Convert.ToString(query_os_rotate["BootDevice"])?.Replace(@"\Device\", "").ToLower().Replace("harddiskvolume", string.Empty);
                    string windows_partition_text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_windows_partition"));
                    OS_BootPartition_V.Text = $"{windows_disk} - {windows_partition_text} {boot_device}";
                }catch (Exception){ }
                try{
                    // SYSTEM PARTITION
                    string system_device = Convert.ToString(query_os_rotate["SystemDevice"])?.Replace(@"\Device\", "").ToLower().Replace("harddiskvolume", string.Empty);
                    string windows_partition_text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_windows_partition"));
                    OS_SystemPartition_V.Text = $"{windows_disk} - {windows_partition_text} {system_device}";
                }catch (Exception){ }
                try{
                    // WINDOWS TEMP PAGEFILE
                    string temp_pagefile = Path.Combine(windows_disk, "swapfile.sys");
                    if (File.Exists(temp_pagefile)){
                        OS_TempWinPageFile_V.Text = string.Format("{0} - {1}", temp_pagefile, TS_FormatSize(new FileInfo(temp_pagefile).Length));
                    }else{
                        OS_TempWinPageFile_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                    }
                }catch (Exception){ }
                try{
                    // WINDOWS HIBERFIL FILE
                    string hiberfil_file = Path.Combine(windows_disk, "hiberfil.sys");
                    if (File.Exists(hiberfil_file)){
                        OS_Hiberfil_V.Text = string.Format("{0} - {1}", hiberfil_file, TS_FormatSize(new FileInfo(hiberfil_file).Length));
                    }else{
                        OS_Hiberfil_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                    }
                }catch (Exception){ }
            }
            try{
                // WINDOWS PAGEFILE
                foreach (ManagementObject query_os_pfu in search_pfu.Get()){
                    object pagefile_caption = query_os_pfu["Caption"];
                    object pagefile_name = query_os_pfu["Name"];
                    string pagefile_result;
                    //
                    if (pagefile_caption != null && !string.IsNullOrEmpty(pagefile_caption.ToString())){
                        pagefile_result = string.Format("{0} - {1}", pagefile_caption, TS_FormatSize(new FileInfo(Convert.ToString(pagefile_caption)).Length));
                    }else if (pagefile_name != null && !string.IsNullOrEmpty(pagefile_name.ToString())){
                        pagefile_result = string.Format("{0} - {1}", pagefile_name, TS_FormatSize(new FileInfo(Convert.ToString(pagefile_name)).Length));
                    }else{
                        pagefile_result = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                    }
                    OS_WinPageFile_V.Text = pagefile_result.Trim();
                }
            }catch (Exception){ }
            try{
                // SYSTEM ANTI-VIRUS PRODUCT
                List<string> av_list = new List<string>();
                foreach (ManagementObject query_av in search_av.Get()){
                    string av_name = Convert.ToString(query_av["DisplayName"]).Trim();
                    if (av_name != "" && av_name != string.Empty){
                        av_list.Add(av_name);
                    }
                }
                if (av_list.Count < 1){
                    av_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_anti_virus_count")));
                }
                string av_list_split = string.Join(" - ", av_list);
                OS_AVProgram_V.Text = av_list_split;
            }catch (Exception){ }
            try{
                // SYSTEM FIREWALL PRODUCT
                List<string> fw_list = new List<string>();
                foreach (ManagementObject query_fw in search_fw.Get()){
                    string fw_name = Convert.ToString(query_fw["DisplayName"]).Trim();
                    if (fw_name != "" && fw_name != string.Empty){
                        fw_list.Add(fw_name);
                    }
                }
                if (fw_list.Count < 1){
                    fw_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_firewall_count")));
                }
                string fw_list_split = string.Join(" - ", fw_list);
                OS_FirewallProgram_V.Text = fw_list_split;
            }catch (Exception){ }
            try{
                // SYSTEM ANTI-SPYWARE PRODUCT
                List<string> as_list = new List<string>();
                foreach (ManagementObject query_as in search_as.Get()){
                    string fw_name = Convert.ToString(query_as["DisplayName"]).Trim();
                    if (fw_name != "" && fw_name != string.Empty){
                        as_list.Add(fw_name);
                    }
                }
                if (as_list.Count < 1){
                    as_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_anti_spyware_count")));
                }
                string as_list_split = string.Join(" - ", as_list);
                OS_AntiSpywareProgram_V.Text = as_list_split;
            }catch (Exception){ }
            try{
                // WINDOWS DEFENDER CORE ISOLATION STATUS
                using (RegistryKey get_core_isolation_status = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity")){
                    if (get_core_isolation_status != null){
                        object core_isolation_status = get_core_isolation_status.GetValue("Enabled");
                        if (core_isolation_status != null){
                            if (core_isolation_status.ToString().Trim() == "1"){
                                OS_WinDefCoreIsolation_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_win_core_isolation_active"));
                            }else{
                                OS_WinDefCoreIsolation_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_win_core_isolation_deactive"));
                            }
                        }else{
                            OS_WinDefCoreIsolation_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                        }
                    }else{
                        OS_WinDefCoreIsolation_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                    }
                    get_core_isolation_status.Close();
                }
            }catch (Exception){ }
            try{
                // MS EDGE VERSION
                string ms_edge_path = Path.Combine(windows_disk, @"Program Files (x86)\Microsoft\Edge\Application\msedge.exe");
                if (File.Exists(ms_edge_path)){
                    FileVersionInfo edgeFileVersionInfo = FileVersionInfo.GetVersionInfo(ms_edge_path);
                    OS_MSEdge_V.Text = edgeFileVersionInfo.ProductVersion.Trim();
                }else{
                    OS_MSEdge_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_msedge_not"));
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
                        OS_MSEdgeWebView_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_msedge_webview2_not"));
                    }
                }else{
                    OS_MSEdgeWebView_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_msedge_webview2_not"));
                }
            }catch (Exception){ }
            try{
                // MS STORE VERSION
                StartMsStoreVersionCheck();
            }catch (Exception){ }
            try{
                // WIN PROD KEY
                string win_key = TSWindowsProductKey.GetWindowsProductKey().Trim();
                if (win_key != "null" && win_key != string.Empty){
                    if (hiding_mode_wrapper != 1){
                        OS_WinKey_V.Text = win_key;
                        MainToolTip.SetToolTip(OS_WinKeyCopy, TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_license_copy_btn")));
                    }else{
                        OS_WinKeyCopy.Visible = false;
                        OS_WinKey_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})";
                    }
                }else{
                    OS_WinKeyCopy.Visible = false;
                    OS_WinKey_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_license_null"));
                }
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
                            string version = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_net_framework_null"));
                            foreach (var kvp in netFrameworkVersions.OrderByDescending(k => k.Key)){
                                if (releaseKeyValue >= kvp.Key){
                                    version = kvp.Value;
                                    break;
                                }
                            }
                            OS_NETFrameworkVersion_V.Text = version;
                        }else{
                            OS_NETFrameworkVersion_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_net_framework_null"));
                        }
                    }else{
                        OS_NETFrameworkVersion_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_net_framework_null"));
                    }
                }
            }catch (Exception){ }
            try{
                string mdp_1 = Path.Combine(windows_disk, @"Windows\Minidump");
                string mdp_2 = Path.Combine(windows_disk, @"Windows\memory.dmp");
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
                    OS_MinidumpOpen.Visible = true;
                    OS_BSoDZIP.Visible = true;
                    OS_Minidump_V.Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bsod_detect")), minidump_files_list.Count);
                    MainToolTip.SetToolTip(OS_MinidumpOpen, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bsod_open_folder")), mdp_1));
                    MainToolTip.SetToolTip(OS_BSoDZIP, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bsod_zip_folder")), "'"));
                    //Console.WriteLine($"{minidump_files_list.Count} dosya var");
                }else{
                    OS_MinidumpOpen.Visible = false;
                    OS_BSoDZIP.Visible = false;
                    OS_Minidump_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bsod_not_detect"));
                    OS_BSODDate_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bsod_not_detect"));
                    //Console.WriteLine("Hiçbir dosya yok"); // x2
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
                // GET WALLPAPER
                string wp_resoulation;
                foreach (ManagementObject query_desktop_rotate in search_desktop.Get()){
                    string get_wallpaper = Convert.ToString(query_desktop_rotate["Wallpaper"]);
                    if (get_wallpaper != "" && get_wallpaper != string.Empty){
                        wp_rotate = get_wallpaper;
                        if (File.Exists(get_wallpaper)){
                            // GET WALLPAPER RESOULATION
                            using (var wallpaper_res = new FileStream(get_wallpaper, FileMode.Open, FileAccess.Read, FileShare.Read)){
                                using (var wallpaper_res_x64 = Image.FromStream(wallpaper_res, false, false)){
                                    wp_resoulation = wallpaper_res_x64.Width + "x" + wallpaper_res_x64.Height;
                                }
                            }
                            // GET WALLPAPER SIZE
                            FileInfo wallpaper_size = new FileInfo(get_wallpaper);
                            double wallpaper_size_x64 = Convert.ToDouble(wallpaper_size.Length); // in byte
                            if (hiding_mode_wrapper != 1){
                                OS_Wallpaper_V.Text = Path.GetFileName(get_wallpaper) + " - " + wp_resoulation + " - " + TS_FormatSize(wallpaper_size_x64);
                            }else{
                                OS_Wallpaper_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + Path.GetExtension(get_wallpaper) + " - " + wp_resoulation + " - " + TS_FormatSize(wallpaper_size_x64) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})";
                            }
                            // HOVER HIDING WRAPPER
                            if (hiding_mode_wrapper != 1){
                                MainToolTip.SetToolTip(OS_WallpaperOpen, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_open_wallpaper")), wp_rotate));
                            }else{
                                OS_WallpaperOpen.Visible = false;
                                MainToolTip.SetToolTip(OS_WallpaperOpen, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_open_wallpaper")), new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + Path.GetExtension(wp_rotate)));
                            }
                        }else{
                            OS_Wallpaper_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_wallpaper_not_available"));
                            OS_WallpaperOpen.Visible = false;
                        }
                    }
                }
                if (OS_Wallpaper_V.Text == "N/A" || OS_Wallpaper_V.Text.Trim() == "" || OS_Wallpaper_V.Text == string.Empty){
                    OS_Wallpaper_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_wallpaper_cannot_be_read"));
                    OS_WallpaperOpen.Visible = false;
                }
            }catch (Exception){ }
            try{
                // WINDOWS LICSENSE TYPE
                StartWindowsLicenseTypeCheck();
            }catch (Exception){ }
            // OS PROCESS END ENABLED
            //OS_RotateBtn.Enabled = true;
            //((Control)OS).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Operating System Section Installed --->"); }
        }
        private async void os_bg_process(){
            try{
                // DESCRIPTIVE
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                do{
                    if (loop_status == false){ break; }
                    foreach (ManagementObject query_os_rotate in search_os.Get()){
                        try{
                            // FREE VIRTUAL RAM
                            double free_virtual_ram = Convert.ToDouble(query_os_rotate["FreeVirtualMemory"]) * 1024; // in byte
                            double total_virtual_memory = Convert.ToDouble(query_os_rotate["TotalVirtualMemorySize"]) * 1024; // in byte
                            double tvm = total_virtual_memory - free_virtual_ram;
                            RAM_EmptyVirtualRam_V.Text = TS_FormatSize(free_virtual_ram);
                            RAM_UsageVirtualRam_V.Text = TS_FormatSize(tvm);
                            // GET LAST BOOT
                            string oi_day = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_day"));
                            string oi_hour = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_hour"));
                            string oi_minute = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_minute"));
                            string oi_second = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_second"));
                            string oi_ago = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_ago"));
                            //
                            string last_boot = Convert.ToString(query_os_rotate["LastBootUpTime"]);
                            // Convert DateTime
                            DateTime boot_date_x64 = DateTime.ParseExact(last_boot.Substring(0, 14), "yyyyMMddHHmmss", null);
                            // Current Date and Hour
                            var now_date = DateTime.Now;
                            // System Uptime
                            var system_uptime = now_date.Subtract(boot_date_x64);
                            string system_uptime_x64 = $"{system_uptime.Days} {oi_day}, {system_uptime.Hours} {oi_hour}, {system_uptime.Minutes} {oi_minute}, {system_uptime.Seconds} {oi_second}";
                            OS_SystemWorkTime_V.Text = system_uptime_x64;
                            // SYSTEM INSTALL DATE
                            string os_id = query_os_rotate["InstallDate"].ToString();
                            DateTime osInstallDate = new DateTime(
                                int.Parse(os_id.Substring(0, 4)),
                                int.Parse(os_id.Substring(4, 2)),
                                int.Parse(os_id.Substring(6, 2)),
                                int.Parse(os_id.Substring(8, 2)),
                                int.Parse(os_id.Substring(10, 2)),
                                int.Parse(os_id.Substring(12, 2))
                            );
                            // Current Date and Hour
                            DateTime currentDate = DateTime.Now;
                            // System Uptime
                            TimeSpan uptime = currentDate - osInstallDate;
                            string os_id_x256 = $"{osInstallDate:dd.MM.yyyy} - {osInstallDate:HH:mm:ss} - ({uptime.Days} {oi_day}, {uptime.Hours} {oi_hour}, {uptime.Minutes} {oi_minute} {oi_ago})";
                            OS_Install_V.Text = os_id_x256;
                        }catch (Exception){ }
                    }
                    // SYSTEM TIME
                    OS_SystemTime_V.Text = DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss");
                    // SYSTEM WORK TIME
                    try{
                        // MOUSE WHEEL SPEED
                        int mouse_wheel_speed = new Computer().Mouse.WheelScrollLines;
                        OS_MouseWheelStatus_V.Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_scroll_speed")), mouse_wheel_speed);
                    }catch (Exception){ }
                    try{
                        // SCROLL LOCK STATUS
                        bool scroll_lock_status = new Computer().Keyboard.ScrollLock;
                        if (scroll_lock_status == true){
                            OS_ScrollLockStatus_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_on"));
                        }else if (scroll_lock_status == false){
                            OS_ScrollLockStatus_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_off"));
                        }
                    }catch (Exception){ }
                    try{
                        // NUMLOCK STATUS
                        bool numlock_status = new Computer().Keyboard.NumLock;
                        if (numlock_status == true){
                            OS_NumLockStatus_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_on"));
                        }else if (numlock_status == false){
                            OS_NumLockStatus_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_off"));
                        }
                    }catch (Exception){ }
                    try{
                        // CAPSLOCK STATUS
                        bool capslock_status = new Computer().Keyboard.CapsLock;
                        if (capslock_status == true){
                            OS_CapsLockStatus_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_on"));
                        }else if (capslock_status == false){
                            OS_CapsLockStatus_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_off"));
                        }
                    }catch (Exception){ }
                    await Task.Delay(1000 - DateTime.Now.Millisecond, Program.TS_TokenEngine.Token).ConfigureAwait(false);
                }while (loop_status == true);
            }catch (Exception){ }
        }
        // GET MS STORE VERSION
        private async void StartMsStoreVersionCheck(){
            try{
                await Task.Run(() => ms_store_version(), Program.TS_TokenEngine.Token);
            }catch (Exception){ }
        }
        private void ms_store_version(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                using (Process get_ms_store_version = new Process()){
                    get_ms_store_version.StartInfo.FileName = "powershell.exe";
                    get_ms_store_version.StartInfo.Arguments = "(Get-AppxPackage -Name Microsoft.WindowsStore).Version";
                    get_ms_store_version.StartInfo.RedirectStandardOutput = true;
                    get_ms_store_version.StartInfo.UseShellExecute = false;
                    get_ms_store_version.StartInfo.CreateNoWindow = true;
                    get_ms_store_version.Start();
                    //
                    string get_ms_s_version = get_ms_store_version.StandardOutput.ReadToEnd();
                    get_ms_store_version.WaitForExit();
                    //
                    if (!string.IsNullOrEmpty(get_ms_s_version)){
                        OS_MSStoreVersion_V.Text = get_ms_s_version.Trim();
                    }else{
                        OS_MSStoreVersion_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                    }
                }
            }catch (Exception){ }
        }
        // WIN KEY COPY
        private void OS_WinKeyCopy_Click(object sender, EventArgs e){
            try{
                Clipboard.SetText(OS_WinKey_V.Text);
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 1, TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_license_copy")));
            }catch (Exception){ }
        }
        // WIN LICENSE TYPE
        private async void StartWindowsLicenseTypeCheck(){
            try{
                await Task.Run(() => win_license_type(), Program.TS_TokenEngine.Token);
            }catch (Exception){ }
        }
        private void win_license_type(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                var get_win_license_mode = new Process{
                    StartInfo = new ProcessStartInfo{
                        FileName = "cmd.exe",
                        Arguments = $"/c set LANG=en && cscript //NoLogo {windows_disk}Windows\\System32\\slmgr.vbs /dli",
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                    }
                };
                get_win_license_mode.Start();
                string win_license_mode = get_win_license_mode.StandardOutput.ReadToEnd();
                get_win_license_mode.WaitForExit();
                if (!string.IsNullOrWhiteSpace(win_license_mode)){
                    string[] get_lines = win_license_mode.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    if (get_lines.Length > 1){
                        string[] explode_get_data = get_lines[1].Split(new[] { ',' }, 2);
                        if (explode_get_data.Length > 1){
                            OS_WinActiveChannel_V.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(explode_get_data[1].Trim());
                        }else{
                            OS_WinActiveChannel_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                        }
                    }else{
                        OS_WinActiveChannel_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                    }
                }else{
                    OS_WinActiveChannel_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_unknown"));
                }
            }catch (Exception){ }
        }
        // BSOD TIME DYNAMIC
        private async void StartBsodTimeDynamicCheck(){
            try{
                await Task.Run(() => bsod_time_dynamic(), Program.TS_TokenEngine.Token);
            }catch (Exception){ }
        }
        private async void bsod_time_dynamic(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                DateTime last_bsod_date = Convert.ToDateTime(minidump_files_date_list[0]);
                // Year - Month - Day - Hour - Minute - Second
                var last_bsod_x64 = new DateTime(last_bsod_date.Year, last_bsod_date.Month, last_bsod_date.Day, last_bsod_date.Hour, last_bsod_date.Minute, last_bsod_date.Second);
                do{
                    if (loop_status == false){ break; }
                    var now_date = DateTime.Now;
                    var bsod_to_now = now_date.Subtract(last_bsod_x64);
                    string bsod_days = bsod_to_now.Days + " " + TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_day"));
                    string bsod_hours = bsod_to_now.Hours + " " + TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_hour"));
                    string bsod_miniutes = bsod_to_now.Minutes + " " + TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_minute"));
                    string bsod_seconds = bsod_to_now.Seconds + " " + TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_second")) + " " + TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_ago"));
                    OS_BSODDate_V.Text = string.Format("{0}, {1}, {2}, {3}", bsod_days, bsod_hours, bsod_miniutes, bsod_seconds);
                    await Task.Delay(1000 - DateTime.Now.Millisecond, Program.TS_TokenEngine.Token).ConfigureAwait(false);
                } while (loop_status == true);
            }catch (Exception){ }
        }
        // MINIDIUMP FOLDER OPEN
        private void OS_MinidumpOpen_Click(object sender, EventArgs e){
            try{
                // OPEN MINIDUMP FOLDER
                string minidump_path = Path.Combine(windows_disk, @"Windows\Minidump");
                Process.Start(minidump_path);
            }catch (Exception){
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 3, TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bsod_folder_open_error")));
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
                Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bsod_progress_copy")), TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode));
                foreach (string file_path in minidump_files_list){
                    string file_name = Path.GetFileName(file_path);
                    string target_file_path = Path.Combine(minidump_target_file, file_name);
                    await Task.Run(() => File.Copy(file_path, target_file_path, true), Program.TS_TokenEngine.Token);
                    currentFile++;
                }
                // ZIP ASYNC
                Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bsod_progress_compress")), TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode));
                await Task.Run(() => {
                    using (FileStream zipToOpen = new FileStream(minidump_zip_file, FileMode.Create)){
                        using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Create)){
                            currentFile = 0;
                            foreach (string file_path in minidump_files_list){
                                string file_name = Path.GetFileName(file_path);
                                string target_file_path = Path.Combine(minidump_target_file, file_name);
                                // Create a new entry and set the compression level
                                ZipArchiveEntry zip_entry = archive.CreateEntry(file_name, CompressionLevel.Fastest);
                                // Copy the contents of the file to the input stream
                                using (var entryStream = zip_entry.Open())
                                using (var fileStream = new FileStream(target_file_path, FileMode.Open, FileAccess.Read)){
                                    fileStream.CopyTo(entryStream); // Copies file contents here
                                }
                                currentFile++;
                            }
                        }
                    }
                }, Program.TS_TokenEngine.Token);
                // Temp directory cleanup
                if (Directory.Exists(minidump_target_file)){
                    Directory.Delete(minidump_target_file, true);
                }
                // Once everything is done, show the message
                Text = TS_VersionEngine.TS_SofwareVersion(0, Program.ts_version_mode);
                // Ensure that the message is shown *after* all operations are done
                DialogResult open_minidump_zip_target = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bsod_zip_success")), minidump_zip_file, "\n\n"));
                if (open_minidump_zip_target == DialogResult.Yes){
                    string open_mdzt = string.Format("/select, \"{0}\"", minidump_zip_file.Trim().Replace("/", @"\"));
                    ProcessStartInfo psi = new ProcessStartInfo("explorer.exe", open_mdzt);
                    Process.Start(psi);
                }
            }catch (Exception ex){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_bsod_zip_error")), "\n", ex.Message));
            }
        }
        // OPEN WALLPAPER
        private void GoWallpaperRotate_Click(object sender, EventArgs e){
            try{
                string wallpaper_start_path = string.Format("/select, \"{0}\"", wp_rotate.Trim().Replace("/", @"\"));
                ProcessStartInfo psi = new ProcessStartInfo("explorer.exe", wallpaper_start_path);
                Process.Start(psi);
            }catch (Exception){
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 3, TS_String_Encoder(software_lang.TSReadLangs("Os_Content", "os_c_wallpaper_open_error")));
            }
        }
        #endregion
        #region MB_Section
        // MB
        // ======================================================================================================
        private void mb(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_bb = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
            ManagementObjectSearcher search_cs = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");
            ManagementObjectSearcher search_bios = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS");
            ManagementObjectSearcher search_md = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_MotherboardDevice");
            ManagementObjectSearcher search_tpm = new ManagementObjectSearcher("root\\CIMV2\\Security\\MicrosoftTpm", "SELECT * FROM Win32_Tpm");
            foreach (ManagementObject query_bb_rotate in search_bb.Get()){
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
                    if (hiding_mode_wrapper != 1){
                        MB_MotherBoardSerial_V.Text = mb_serial;
                    }else{
                        MB_MotherBoardSerial_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})";
                    }
                }catch (Exception){ }
                try{
                    // MB VERSION
                    MB_Model_V.Text = Convert.ToString(query_bb_rotate["Version"]);
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
                            MB_SystemModelMan_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected"));
                        }
                    }catch (Exception){ }
                    // SYSTEM MODEL FAMILY
                    try{
                        string systemFamily = Convert.ToString(get_bios_data.GetValue("SystemFamily"));
                        if (!string.IsNullOrEmpty(systemFamily)){
                            MB_SystemModelFamily_V.Text = systemFamily.ToString().Trim();
                        }else{
                            MB_SystemModelFamily_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected"));
                        }
                    }catch (Exception){ }
                    // SYSTEM MODEL
                    try{
                        string systemProductName = Convert.ToString(get_bios_data.GetValue("SystemProductName"));
                        if (!string.IsNullOrEmpty(systemProductName)){
                            MB_SystemModel_V.Text = systemProductName.ToString().Trim();
                        }else{
                            MB_SystemModel_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected"));
                        }
                    }catch (Exception){ }
                }else{
                    MB_SystemModelMan_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected"));
                    MB_SystemModelFamily_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected"));
                    MB_SystemModel_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected"));
                }
                get_bios_data.Close();
            }catch (Exception){ }
            foreach (ManagementObject query_cs in search_cs.Get()){
                // SYSTEM FAMILY
                try{
                    string system_family = Convert.ToString(query_cs["SystemFamily"]).Trim();
                    string system_family_check = system_family.ToLower();
                    if (system_family_check != "default string" && system_family_check == "to be filled by o.e.m."){
                        if (hiding_mode_wrapper != 1){
                            MB_SystemFamily_V.Text = system_family;
                        }else{
                            MB_SystemFamily_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})";
                        }
                    }else{
                        MB_SystemFamily_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected"));
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
                            MB_SystemSKU_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})";
                        }
                    }else{
                        MB_SystemSKU_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected"));
                    }
                }catch (Exception){ }
            }
            foreach (ManagementObject query_bios_rotate in search_bios.Get()){
                try{
                    // BIOS MAN
                    MB_BiosManufacturer_V.Text = Convert.ToString(query_bios_rotate["Manufacturer"]);
                }catch (Exception){ }
                try{
                    // BIOS DATE
                    string bios_date = Convert.ToString(query_bios_rotate["ReleaseDate"]);
                    string b_date_render = bios_date.Substring(6, 2) + "." + bios_date.Substring(4, 2) + "." + bios_date.Substring(0, 4);
                    MB_BiosDate_V.Text = b_date_render;
                }catch (Exception){ }
                try{
                    // BIOS VERSION
                    MB_BiosVersion_V.Text = Convert.ToString(query_bios_rotate["Caption"]);
                }catch (Exception){ }
                try{
                    // SM BIOS VERSION
                    MB_SmBiosVersion_V.Text = Convert.ToString(query_bios_rotate["Version"]);
                }catch (Exception){ }
                try{
                    // BIOS MAJOR MINOR
                    object bios_major = query_bios_rotate["SystemBiosMajorVersion"];
                    object bios_minor = query_bios_rotate["SystemBiosMinorVersion"];
                    MB_BiosMajorMinor_V.Text = bios_major.ToString() + "." + bios_minor.ToString();
                }catch (Exception){ }
                try{
                    // SM-BIOS MAJOR MINOR
                    object sm_bios_major = query_bios_rotate["SMBIOSMajorVersion"];
                    object sm_bios_minor = query_bios_rotate["SMBIOSMinorVersion"];
                    MB_SMBiosMajorMinor_V.Text = sm_bios_major.ToString() + "." + sm_bios_minor.ToString();
                }catch (Exception){ }
            }
            try{
                foreach (ManagementObject query_md_rotate in search_md.Get()){
                    try{
                        // PRIMARY BUS TYPE
                        string mb_primary_bus_type = Convert.ToString(query_md_rotate["PrimaryBusType"]).Trim();
                        if (mb_primary_bus_type != "" || mb_primary_bus_type != string.Empty){
                            MB_PrimaryBusType_V.Text = mb_primary_bus_type;
                        }else{
                            MB_PrimaryBusType_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected"));
                        }
                    }catch (Exception){ }
                    try{
                        // SECONDARY BUS TYPE
                        string mb_secondary_bus_type = Convert.ToString(query_md_rotate["SecondaryBusType"]).Trim();
                        if (mb_secondary_bus_type != "" || mb_secondary_bus_type != string.Empty){
                            MB_SecondaryBusType_V.Text = mb_secondary_bus_type;
                        }else{
                            MB_SecondaryBusType_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_not_detected"));
                        }
                    }catch (Exception){ }
                }
            }catch (Exception){ }
            try{
                // MB SECURE BOOT
                bool mb_secure_boot_status = Convert.ToBoolean(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", ""));
                if (mb_secure_boot_status == true){
                    MB_SecureBoot_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_on"));
                }else if (mb_secure_boot_status == false){
                    MB_SecureBoot_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_off"));
                }
            }catch (Exception){ }
            foreach (ManagementObject query_tpm in search_tpm.Get()){
                try{
                    // TPM VERSION
                    bool tpm_status = Convert.ToBoolean(query_tpm["IsActivated_InitialValue"]);
                    string tpm_version = Convert.ToString(query_tpm["SpecVersion"]);
                    char[] split_char = { ',' };
                    string[] split_keywords = tpm_version.Trim().Split(split_char);
                    if (tpm_status == true){
                        MB_TPMStatus_V.Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_active")), split_keywords[0], tpm_version);
                    }else{
                        MB_TPMStatus_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off"));
                    }
                }catch (Exception){ }
                try{
                    // TPM PHYSICAL VERSION
                    string tpm_phy_version = Convert.ToString(query_tpm["PhysicalPresenceVersionInfo"]).Trim();
                    if (tpm_phy_version != "" || tpm_phy_version != string.Empty){
                        MB_TPMPhysicalVersion_V.Text = tpm_phy_version;
                    }else{
                        MB_TPMPhysicalVersion_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off"));
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER ID TXT
                    string tpm_man_id_txt = Convert.ToString(query_tpm["ManufacturerIdTxt"]).Trim();
                    if (tpm_man_id_txt != "" || tpm_man_id_txt != string.Empty){
                        MB_TPMMan_V.Text = tpm_man_id_txt;
                    }else{
                        MB_TPMMan_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off"));
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER ID
                    string tpm_man_id = Convert.ToString(query_tpm["ManufacturerId"]).Trim();
                    if (tpm_man_id != "" || tpm_man_id != string.Empty){
                        if (hiding_mode_wrapper != 1){
                            MB_TPMManID_V.Text = tpm_man_id;
                        }else{
                            MB_TPMManID_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})";
                        }
                    }else{
                        MB_TPMManID_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off"));
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER VERSION
                    string tpm_man_version = Convert.ToString(query_tpm["ManufacturerVersion"]).Trim();
                    if (tpm_man_version != "" || tpm_man_version != string.Empty){
                        MB_TPMManVersion_V.Text = tpm_man_version;
                    }else{
                        MB_TPMManVersion_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off"));
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER VERSION FULL
                    string tpm_man_version_full = Convert.ToString(query_tpm["ManufacturerVersionFull20"]).Trim();
                    if (tpm_man_version_full != "" || tpm_man_version_full != string.Empty){
                        MB_TPMManFullVersion_V.Text = tpm_man_version_full;
                    }else{
                        MB_TPMManFullVersion_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off"));
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER INFO
                    string tpm_man_version_info = Convert.ToString(query_tpm["ManufacturerVersionInfo"]).Trim();
                    if (tpm_man_version_info != "" || tpm_man_version_info != string.Empty){
                        MB_TPMManPublisher_V.Text = tpm_man_version_info;
                    }else{
                        MB_TPMManPublisher_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off"));
                    }
                }catch (Exception){ }
            }
            // TPM MODE CHECK WRAPPER
            try{
                string tpmOffMessage = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_tpm_off"));
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
                MB_LastBIOSTime_V.Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_boot_time")), last_bios_time_x64);
            }catch (Exception){ }
            // MB PROCESS END ENABLED
            MB_RotateBtn.Enabled = true;
            ((Control)MB).Enabled = true;
            MB_BIOSUpdateBtn.Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Motherboard Section Loaded --->"); }
        }
        private void MB_BIOSUpdateBtn_Click(object sender, EventArgs e){
            try{
                string bios_prompt = "";
                if (laptop_mode == true){
                    bios_prompt = string.Format("{0} {1} {2}", MB_SystemModelMan_V.Text.Trim(), MB_SystemModel_V.Text.Trim(), "BIOS Update");
                }else if (laptop_mode == false){
                    bios_prompt = string.Format("{0} {1} {2}", MB_MotherBoardMan_V.Text.Trim(), MB_MotherBoardName_V.Text.Trim(), "BIOS Update");
                }
                Process.Start(new ProcessStartInfo($"https://www.google.com/search?q={bios_prompt}"){ UseShellExecute = true });
            }catch (Exception){ }
        }
        #endregion
        #region CPU_Section
        // CPU
        // ======================================================================================================
        public static List<string> bench_cpu_info = new List<string>();
        private void cpu(){
            // CPU MODE
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_process = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            ManagementObjectSearcher search_intel_me = new ManagementObjectSearcher("root\\Intel_ME", "SELECT * FROM ME_System");
            double cpu_l1_total_size = 0;
            double cpu_l2_total_size = 0;
            double cpu_l3_total_size = 0;
            foreach (ManagementObject query_process_rotate in search_process.Get()){
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
                    string[] cpu_architectures = { "32 " + TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_bit")) + " - (x86)", "MIPS", "ALPHA", "POWER PC", "ARM", "IA64", "64 " + TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_bit")) + " - (x64)" };
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
                    if (thread_count == String.Empty || thread_count == ""){
                        CPU_LogicalCore_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                    }else{
                        CPU_LogicalCore_V.Text = thread_count;
                    }
                    bench_cpu_info.Add(CPU_LogicalCore_V.Text);
                }catch (Exception){ }
                //
                benchCPUToolStripMenuItem.Enabled = true;
                //
                try{
                    // CPU SOCKET
                    CPU_SocketDefinition_V.Text = Convert.ToString(query_process_rotate["SocketDesignation"]);
                }catch (Exception){ }
                try{
                    // CPU FAMILY
                    string cpu_description = Convert.ToString(query_process_rotate["Description"]);
                    string cpu_tanim = cpu_description.Replace("Family", TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_family")));
                    string cpu_tanim_2 = cpu_tanim.Replace("Model", TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_model")));
                    string cpu_tanim_3 = cpu_tanim_2.Replace("Stepping", TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_stage")));
                    string cpu_tanim_4 = cpu_tanim_3.Replace("64", " X64");
                    CPU_Family_V.Text = cpu_tanim_4;
                }catch (Exception){ }
                try{
                    // REGISTRY CPU INFO
                    using (RegistryKey cpu_key = Registry.LocalMachine.OpenSubKey(@"HARDWARE\DESCRIPTION\System\CentralProcessor\0")){
                        if (cpu_key != null){
                            try{
                                // Update Status (REG_DWORD - 32-bit tam sayı)
                                int updateStatus = (int)cpu_key.GetValue("Update Status");
                                if (!string.IsNullOrEmpty(updateStatus.ToString())){
                                    if (updateStatus == 0){
                                        CPU_MicroCode_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_microcode_yes"));
                                    }
                                    else if (updateStatus > 0){
                                        CPU_MicroCode_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_microcode_yes"));
                                    }
                                }else{
                                    CPU_MicroCode_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                                }
                            }catch (Exception){ }
                            try{
                                // Platform Specific Field 1 (REG_DWORD - 32-bit tam sayı, ondalık ve hexadecimal gösterimi)
                                int platformSpecificField = (int)cpu_key.GetValue("Platform Specific Field 1");
                                if (!string.IsNullOrEmpty(platformSpecificField.ToString())){
                                    CPU_PlatformFeature_V.Text = string.Format("{0} / {1}", platformSpecificField, platformSpecificField.ToString("X"));
                                }else{
                                    CPU_PlatformFeature_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                                }
                            }catch (Exception){ }
                            try{
                                // FeatureSet (REG_DWORD - 32-bit tam sayı, ondalık ve hexadecimal gösterimi)
                                int featureSet = (int)cpu_key.GetValue("FeatureSet");
                                if (!string.IsNullOrEmpty(featureSet.ToString())){
                                    CPU_FeatureSet_V.Text = string.Format("{0} / {1}", featureSet, featureSet.ToString("X"));
                                }else{
                                    CPU_FeatureSet_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                                }
                            }catch (Exception){ }
                            try{
                                // Update Revision (REG_BINARY - Binary, byte dizisi olarak okunup hexadecimal string'e çevrilir)
                                byte[] updateRevision = cpu_key.GetValue("Update Revision") as byte[];
                                if (updateRevision != null){
                                    string updateRevisionHex = BitConverter.ToString(updateRevision).Replace("-", " ");
                                    if (!string.IsNullOrEmpty(updateRevisionHex)){
                                        CPU_Revision_V.Text = updateRevisionHex.Trim();
                                    }else{
                                        CPU_Revision_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                                    }
                                }
                            }catch (Exception){ }
                            try{
                                // Previous Update Revision (REG_BINARY - Binary, byte dizisi olarak okunup hexadecimal string'e çevrilir)
                                byte[] previousUpdateRevision = cpu_key.GetValue("Previous Update Revision") as byte[];
                                if (previousUpdateRevision != null){
                                    string previousUpdateRevisionHex = BitConverter.ToString(previousUpdateRevision).Replace("-", " ");
                                    if (!string.IsNullOrEmpty(previousUpdateRevisionHex)){
                                        CPU_OldRevision_V.Text = previousUpdateRevisionHex.Trim();
                                    }else{
                                        CPU_OldRevision_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                                    }
                                }
                            }catch (Exception){ }
                        }else{
                            CPU_MicroCode_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                            CPU_PlatformFeature_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                            CPU_FeatureSet_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                            CPU_Revision_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                            CPU_OldRevision_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                        }
                        cpu_key.Close();
                    }
                }catch (Exception){ }
                try{
                    // CPU VIRTUALIZATION
                    // bool cpu_virtual_mod = Convert.ToBoolean(query_process_rotate["VirtualizationFirmwareEnabled"]);
                    bool cpu_virtual_mod = IsVirtualizationEnabled();
                    if (cpu_virtual_mod == true){
                        CPU_Virtualization_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_active"));
                    }else if (cpu_virtual_mod == false){
                        CPU_Virtualization_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_disabled"));
                    }
                }catch (Exception){ }
                try{
                    // CPU SERIAL ID
                    string cpu_serial = Convert.ToString(query_process_rotate["ProcessorId"]).Trim();
                    if (hiding_mode_wrapper != 1){
                        CPU_SerialName_V.Text = cpu_serial;
                    }else{
                        CPU_SerialName_V.Text = new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})";
                    }
                }catch (Exception){ }
            }
            ManagementObjectSearcher search_cm = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_CacheMemory WHERE Level = {3}");
            foreach (ManagementObject query_cm_rotate in search_cm.Get()){
                // L1 CACHE
                double l1_size = Convert.ToDouble(query_cm_rotate["MaxCacheSize"]) * 1024;
                cpu_l1_total_size += l1_size;
                CPU_L1_V.Text = TS_FormatSize(cpu_l1_total_size);
            }
            // INTEL ME VERSION
            try{
                CPU_IntelME_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_not_supported"));
                foreach (ManagementObject query_intel_me in search_intel_me.Get()){
                    string intel_me_version = Convert.ToString(query_intel_me["FWVersion"]);
                    if (MB_Chipset_V.Text.Trim().Contains("Intel") || CPU_Name_V.Text.Trim().Contains("Intel")){
                        if (!string.IsNullOrEmpty(intel_me_version)){
                            CPU_IntelME_V.Text = intel_me_version.Trim();
                        }else{
                            CPU_IntelME_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_unknown"));
                        }
                    }else{
                        CPU_IntelME_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Cpu_Content", "cpu_c_not_supported"));
                    }
                }
            }catch (Exception){ }
            // CPU PROCESS END ENABLED
            CPU_RotateBtn.Enabled = true;
            ((Control)CPU).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Processor Section Loaded --->"); }
        }
        private async void processor_bg_process(){
            try{
                ManagementObjectSearcher search_process =  new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                do{
                    if (loop_status == false){ break; }
                    // CODE
                    foreach (ManagementObject query_process in search_process.Get()){
                        var process_count = Convert.ToInt32(query_process["NumberOfProcesses"]);
                        CPU_Process_V.Text = process_count.ToString();
                    }
                    await Task.Delay(1000 - DateTime.Now.Millisecond, Program.TS_TokenEngine.Token).ConfigureAwait(false);
                } while (loop_status == true);
            }catch (Exception){ }
        }
        // CPU ENGINE
        // ======================================================================================================
        private async void cpu_usage_process(){
            try{
                ManagementObjectSearcher cpuSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor");
                do{
                    if (loop_status == false){ break; }
                    ManagementObjectCollection cpuResults = cpuSearcher.Get();
                    ulong totalCpuUsage = 0;
                    int cpuCount = 0;
                    foreach (ManagementObject obj in cpuResults){
                        string cpuName = (string)obj["Name"];
                        ulong cpuUsage = (ulong)obj["PercentProcessorTime"];
                        if (cpuName != "_Total"){
                            totalCpuUsage += cpuUsage;
                            cpuCount++;
                        }
                    }
                    if (cpuCount > 0){
                        float averageCpuUsage = (float)totalCpuUsage / cpuCount;
                        CPU_Usage_V.Text = $"{averageCpuUsage:F2}%";
                    }
                    await Task.Delay(1000 - DateTime.Now.Millisecond, Program.TS_TokenEngine.Token).ConfigureAwait(false);
                } while (loop_status == true);
            }catch (Exception){ }
        }
        #endregion
        #region RAM_Section
        // RAM
        // ======================================================================================================
        List<string> ram_slot_count = new List<string>();
        List<string> ram_slot_list = new List<string>();
        List<string> ram_amount_list = new List<string>();
        List<string> ram_type_list = new List<string>();
        List<string> ram_frekans_list = new List<string>();
        List<string> ram_voltage_list = new List<string>();
        List<string> ram_form_factor = new List<string>();
        List<string> ram_serial_list = new List<string>();
        List<string> ram_manufacturer_list = new List<string>();
        List<string> ram_bank_label_list = new List<string>();
        List<string> ram_data_width_list = new List<string>();
        List<string> bellek_type_list = new List<string>();
        List<string> ram_part_number_list = new List<string>();
        private void ram(){
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
                foreach (ManagementObject query_os_rotate in search_os.Get()){
                    // TOTAL VIRTUAL RAM
                    double total_virtual_ram = Convert.ToDouble(query_os_rotate["TotalVirtualMemorySize"]) * 1024;
                    RAM_TotalVirtualRam_V.Text = TS_FormatSize(total_virtual_ram);
                }
            }catch (Exception){ }
            foreach (ManagementObject queryObj in search_pm.Get()){
                try{
                    // RAM AMOUNT
                    string ram_count = Convert.ToString(queryObj["BankLabel"]);
                    if (ram_count == "" || ram_count == string.Empty){
                        ram_slot_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_no_bank_label")));
                    }else{
                        ram_slot_list.Add(ram_count);
                    }
                }catch (Exception){ }
                try{
                    // RAM SLOT COUNT
                    ram_slot_count.Add(Convert.ToString(queryObj["Capacity"]));
                    RAM_SlotStatus_V.Text = ram_slot_count.Count + " " + TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_slot_count"));
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
                        { 0, TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown")) },
                        { 1, TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_other_memory_type")) },
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
                        ram_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown")));
                    }
                    RAM_Type_V.Text = ram_type_list[0];
                }catch (Exception){ }
                try{
                    // RAM SPEED
                    double ram_speed = Convert.ToInt32(queryObj["Speed"]);
                    ram_frekans_list.Add(string.Format("{0} MHz / {1} MT/s", ram_speed, ram_speed));
                    RAM_Frequency_V.Text = ram_frekans_list[0];
                }catch (Exception){ }
                try{
                    // RAM VOLTAGE
                    string ramVoltStr = queryObj["ConfiguredVoltage"]?.ToString();
                    if (string.IsNullOrEmpty(ramVoltStr) || !double.TryParse(ramVoltStr, out double ramVolt) || ramVolt == 0){
                        ram_voltage_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown")));
                    }else{
                        ram_voltage_list.Add(string.Format("{0:0.00} " + TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_voltage")), ramVolt / 1000.0));
                    }
                    RAM_Volt_V.Text = ram_voltage_list[0];
                }catch (Exception){ }
                try{
                    // FORM FACTOR
                    int form_factor = Convert.ToInt32(queryObj["FormFactor"]);
                    Dictionary<int, string> formFactorDict = new Dictionary<int, string>{
                        { 0, TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown")) },
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
                        ram_serial_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown")));
                    }else{
                        // Hiding mode kontrolü
                        if (hiding_mode_wrapper == 1){
                            int maskLength = vis_m_property.Next(vn_range[0], vn_range[1]);
                            ram_serial_list.Add(new string('*', maskLength) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})");
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
                        ram_manufacturer_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown")));
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
                        ram_bank_label_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown")));
                    }else{
                        ram_bank_label_list.Add(bank_label);
                    }
                    RAM_BankLabel_V.Text = ram_bank_label_list[0];
                }catch (Exception){ }
                try{
                    // RAM TOTAL WIDTH
                    string ram_data_width = Convert.ToString(queryObj["TotalWidth"]);
                    if (string.IsNullOrEmpty(ram_data_width)){
                        ram_data_width_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown")));
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
                        ram_part_number_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_unknown")));
                    }else{
                        ram_part_number_list.Add(part_number);
                    }
                    RAM_PartNumber_V.Text = ram_part_number_list[0];
                }catch (Exception){ }
            }
            // RAM SELECT
            try{
                int ram_amount = ram_slot_list.Count - 1;
                for (int rs = 0; rs <= ram_amount; rs++){
                    RAM_SelectList.Items.Add(string.Format("{0} #{1} - {2} / {3}", TS_String_Encoder(software_lang.TSReadLangs("Ram_Content", "ram_c_ram_slot_select")), (rs + 1), ram_manufacturer_list[rs], ram_amount_list[rs]));
                }
                RAM_SelectList.SelectedIndex = 0;
            }catch (Exception){ }
            // RAM PROCESS END ENABLED
            RAM_RotateBtn.Enabled = true;
            ((Control)RAM).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- RAM Section Loaded --->"); }
        }
        private void RAMSelectList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int ram_slot = RAM_SelectList.SelectedIndex;
                try{ RAM_Amount_V.Text = ram_amount_list[ram_slot]; }catch(Exception){ }
                try{ RAM_Type_V.Text = ram_type_list[ram_slot]; }catch(Exception){ }
                try{ RAM_Frequency_V.Text = ram_frekans_list[ram_slot]; }catch(Exception){ }
                try{ RAM_Volt_V.Text = ram_voltage_list[ram_slot]; }catch(Exception){ }
                try{ RAM_FormFactor_V.Text = ram_form_factor[ram_slot]; }catch(Exception){ }
                try{ RAM_Serial_V.Text = ram_serial_list[ram_slot]; }catch(Exception){ }
                try{ RAM_Manufacturer_V.Text = ram_manufacturer_list[ram_slot]; }catch(Exception){ }
                try{ RAM_BankLabel_V.Text = ram_bank_label_list[ram_slot]; }catch(Exception){ }
                try{ RAM_DataWidth_V.Text = ram_data_width_list[ram_slot]; }catch(Exception){ }
                try{ RAM_BellekType_V.Text = bellek_type_list[ram_slot]; }catch(Exception){ }
                try{ RAM_PartNumber_V.Text = ram_part_number_list[ram_slot]; }catch (Exception){ }
            }catch (Exception){ }
        }
        private async void ram_bg_process(){
            try{
                ComputerInfo get_ram_info = new ComputerInfo();
                do{
                    if (loop_status == false){ break; }
                    ulong total_ram = ulong.Parse(get_ram_info.TotalPhysicalMemory.ToString());
                    ulong usable_ram = ulong.Parse(get_ram_info.AvailablePhysicalMemory.ToString());
                    double usage_ram_percentage = (TS_FormatSizeNoType(total_ram) - TS_FormatSizeNoType(usable_ram)) / TS_FormatSizeNoType(total_ram) * 100;
                    RAM_UsageRAMCount_V.Text = string.Format("{0} - {1}", TS_FormatSize(total_ram - usable_ram), string.Format("{0:0.00}%", usage_ram_percentage));
                    RAM_EmptyRamCount_V.Text = TS_FormatSize(usable_ram);
                    //
                    RAM_ProgressFEPanel.Height = (int)(RAM_ProgressBGPanel.Height * (usage_ram_percentage / 100.0));
                    RAM_ProgressLabel.Text = string.Format("{0:0.0}%", usage_ram_percentage);
                    RAM_ProgressLabel.Top = RAM_ProgressFEPanel.Top + 6;
                    await Task.Delay(1000 - DateTime.Now.Millisecond, Program.TS_TokenEngine.Token).ConfigureAwait(false);
                } while (loop_status == true);
            }catch (Exception){ }
        }
        #endregion
        #region GPU_Section
        // GPU
        // ======================================================================================================
        List<string> gpu_man_list = new List<string>();
        List<string> gpu_driver_version_list = new List<string>();
        List<string> gpu_driver_date_list = new List<string>();
        List<string> gpu_status_list = new List<string>();
        List<string> gpu_device_id_list = new List<string>();
        List<string> gpu_dac_type_list = new List<string>();
        List<string> gpu_drivers_list = new List<string>();
        List<string> gpu_inf_file_list = new List<string>();
        List<string> gpu_inf_file_section_list = new List<string>();
        List<string> gpu_current_colors_list = new List<string>();
        List<string> gpu_monitor_select_list = new List<string>();
        List<string> gpu_monitor_bounds_list = new List<string>();
        List<string> gpu_monitor_work_list = new List<string>();
        List<string> gpu_monitor_res_list = new List<string>();
        List<string> gpu_monitor_refresh_rate_list = new List<string>();
        List<string> gpu_monitor_virtual_res_list = new List<string>();
        List<string> gpu_monitor_bit_deep_list = new List<string>();
        List<string> gpu_monitor_primary_list = new List<string>();
        private void gpu(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_vc = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController");
            foreach (ManagementObject query_vc_rotate in search_vc.Get()){
                try{
                    // GPU NAME
                    string gpu_name = Convert.ToString(query_vc_rotate["Name"]);
                    if (gpu_name != "" && gpu_name != string.Empty){
                        GPU_Select.Items.Add(gpu_name);
                    }
                }catch (Exception){ }
                try{
                    // GPU MAN
                    string gpu_man = Convert.ToString(query_vc_rotate["AdapterCompatibility"]).Trim();
                    if (gpu_man != "" && gpu_man != string.Empty){
                        gpu_man_list.Add(gpu_man);
                        GPU_Manufacturer_V.Text = gpu_man_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU DRIVER VERSION
                    string driver_version = Convert.ToString(query_vc_rotate["DriverVersion"]);
                    if (driver_version != "" && driver_version != string.Empty){
                        gpu_driver_version_list.Add(driver_version);
                        GPU_Version_V.Text = gpu_driver_version_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU DRIVER DATE
                    string gpu_date = Convert.ToString(query_vc_rotate["DriverDate"]);
                    if (gpu_date != "" && gpu_date != string.Empty){
                        string gpu_date_process = gpu_date.Substring(6, 2) + "." + gpu_date.Substring(4, 2) + "." + gpu_date.Substring(0, 4);
                        gpu_driver_date_list.Add(gpu_date_process);
                        GPU_DriverDate_V.Text = gpu_driver_date_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU STATUS
                    int gpu_status = Convert.ToInt32(query_vc_rotate["Availability"]);
                    var gpuStatusMessages = new Dictionary<int, string>{
                        { 1, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_other")) },
                        { 2, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown")) },
                        { 3, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_works_smoothly")) },
                        { 4, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_warning")) },
                        { 5, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_test")) },
                        { 6, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_not_applicable")) },
                        { 7, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_off")) },
                        { 8, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_offline")) },
                        { 9, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_off_duty")) },
                        { 10, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_corrupted")) },
                        { 11, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_not_installed")) },
                        { 12, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_install_error")) },
                        { 13, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_saving_mode")) },
                        { 14, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_saving_mode_low_power")) },
                        { 15, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_saving_mode_wait")) },
                        { 16, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_loop")) },
                        { 17, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_power_saving_warning")) },
                        { 18, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_paused")) },
                        { 19, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_not_ready")) },
                        { 20, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_not_configured")) },
                        { 21, TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_silent_mode")) }
                    };
                    if (gpuStatusMessages.TryGetValue(gpu_status, out string gpuStatusMessage)){
                        gpu_status_list.Add(gpuStatusMessage);
                    }else{
                        gpu_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown")));
                    }
                    GPU_Status_V.Text = gpu_status_list[0];
                }catch (Exception){ }
                try{
                    // GPU DEVICE ID
                    string gpu_device_id = Convert.ToString(query_vc_rotate["PNPDeviceID"]).Trim();
                    if (gpu_device_id != "" && gpu_device_id != string.Empty){
                        char[] split_char = { '\\' };
                        string[] gpu_device_split = gpu_device_id.Trim().Split(split_char);
                        gpu_device_id_list.Add($"{gpu_device_split[0]}\\{gpu_device_split[1]}");
                    }else{
                        gpu_device_id_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown")));
                    }
                    GPU_DeviceID_V.Text = gpu_device_id_list[0];
                }catch (Exception){ }
                try{
                    // GPU DAC TYPE
                    string adaptor_dac_type = Convert.ToString(query_vc_rotate["AdapterDACType"]);
                    if (string.IsNullOrEmpty(adaptor_dac_type)){
                        gpu_dac_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_unknown")));
                    }else{
                        if (adaptor_dac_type == "Integrated RAMDAC"){
                            gpu_dac_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_integrated_ramdac")));
                        }else if (adaptor_dac_type == "Internal"){
                            gpu_dac_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_internal_ramdac")));
                        }else{
                            gpu_dac_type_list.Add(adaptor_dac_type);
                        }
                    }
                    GPU_DacType_V.Text = gpu_dac_type_list[0];
                }catch (Exception){ }
                try{
                    // GPU DRIVERS
                    string gpu_display_drivers = Path.GetFileName(Convert.ToString(query_vc_rotate["InstalledDisplayDrivers"]));
                    if (gpu_display_drivers != "" && gpu_display_drivers != string.Empty){
                        gpu_drivers_list.Add(gpu_display_drivers);
                        GPU_GraphicDriversName_V.Text = gpu_drivers_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU INF FILE NAME
                    string gpu_inf_file = Convert.ToString(query_vc_rotate["InfFilename"]);
                    if (gpu_inf_file != "" && gpu_inf_file != string.Empty){
                        gpu_inf_file_list.Add(gpu_inf_file);
                        GPU_InfFileName_V.Text = gpu_inf_file_list[0];
                    }
                }catch (Exception){ }
                try{
                    // GPU INF FILE GPU INFO PARTITION
                    string gpu_inf_section = Convert.ToString(query_vc_rotate["InfSection"]);
                    if (gpu_inf_section != "" && gpu_inf_section != string.Empty){
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
            try{
                // ALL SCREEN DETECT
                foreach (var all_monitors in Screen.AllScreens){
                    gpu_monitor_select_list.Add(all_monitors.DeviceName.ToString());
                    gpu_monitor_bounds_list.Add(GPUFormatScreenInfo(all_monitors.Bounds));
                    gpu_monitor_work_list.Add(GPUFormatScreenInfo(all_monitors.WorkingArea));
                    gpu_monitor_primary_list.Add(GPUFormatPrimaryScreen(all_monitors.Primary));
                }
            }catch (Exception){ }
            try{
                int desktop_res_width = 0;
                int desktop_res_height = 0;
                //
                foreach (Screen screen in Screen.AllScreens){
                    // SCREEN RESOLUTIONS
                    var dm = new DEVMODE { dmSize = (short)Marshal.SizeOf(typeof(DEVMODE)) };
                    EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);
                    //
                    gpu_monitor_res_list.Add(dm.dmPelsWidth + " x " + dm.dmPelsHeight);
                    desktop_res_width += dm.dmPelsWidth;
                    desktop_res_height += dm.dmPelsHeight;
                    //
                    gpu_monitor_virtual_res_list.Add(screen.Bounds.Width + " x " + screen.Bounds.Height);
                    gpu_monitor_refresh_rate_list.Add(dm.dmDisplayFrequency.ToString() + " Hz");
                    gpu_monitor_bit_deep_list.Add(dm.dmBitsPerPel.ToString() + " Bit");
                }
                //
                GPU_DesktopResolution_V.Text = string.Format("{0}x{1}", desktop_res_width, desktop_res_height);
            }catch (Exception){ }
            try{
                // MONITOR SELECT
                int monitor_amount = gpu_monitor_select_list.Count;
                for (int ma = 1; ma <= monitor_amount; ma++){
                    GPU_MonitorSelectList.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_monitor_select")) + " #" + ma);
                }
                GPU_MonitorSelectList.SelectedIndex = 0;
            }catch (Exception){ }
            // GPU SELECT
            try { GPU_Select.SelectedIndex = 0; }catch(Exception){ }
            // GPU PROCESS END ENABLED
            GPU_RotateBtn.Enabled = true;
            ((Control)GPU).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- GPU Section Loaded --->"); }
        }
        private void GPUSelect_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int gpu_select = GPU_Select.SelectedIndex;
                try{ GPU_Manufacturer_V.Text = gpu_man_list[gpu_select]; }catch(Exception){ }
                try{ GPU_Version_V.Text = gpu_driver_version_list[gpu_select]; }catch(Exception){ }
                try{ GPU_DriverDate_V.Text = gpu_driver_date_list[gpu_select]; }catch(Exception){ }
                try{ GPU_Status_V.Text = gpu_status_list[gpu_select]; }catch(Exception){ }
                try{ GPU_DeviceID_V.Text = gpu_device_id_list[gpu_select]; }catch(Exception){ }
                try{ GPU_DacType_V.Text = gpu_dac_type_list[gpu_select]; }catch(Exception){ }
                try{ GPU_GraphicDriversName_V.Text = gpu_drivers_list[gpu_select]; }catch(Exception){ }
                try{ GPU_InfFileName_V.Text = gpu_inf_file_list[gpu_select]; }catch(Exception){ }
                try{ GPU_INFSectionFile_V.Text = gpu_inf_file_section_list[gpu_select]; }catch(Exception){ }
                try{ GPU_CurrentColor_V.Text = gpu_current_colors_list[gpu_select]; }catch(Exception){ }
            }catch(Exception){ }
        }
        private void MonitorSelectList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int monitor_select = GPU_MonitorSelectList.SelectedIndex;
                try{ GPU_MonitorBounds_V.Text = gpu_monitor_bounds_list[monitor_select]; }catch(Exception){ }
                try{ GPU_MonitorWorking_V.Text = gpu_monitor_work_list[monitor_select]; }catch(Exception){ }
                try{ GPU_MonitorResLabel_V.Text = gpu_monitor_res_list[monitor_select]; }catch(Exception){ }
                try{ GPU_ScreenRefreshRate_V.Text = gpu_monitor_refresh_rate_list[monitor_select]; }catch(Exception){ }
                try{ GPU_MonitorVirtualRes_V.Text = gpu_monitor_virtual_res_list[monitor_select]; }catch(Exception){ }
                try{ GPU_ScreenBit_V.Text = gpu_monitor_bit_deep_list[monitor_select]; }catch(Exception){ }
                try{ GPU_MonitorPrimary_V.Text = gpu_monitor_primary_list[monitor_select]; } catch (Exception){ }
            }catch(Exception){ }
        }
        // FORMATED SCREEN INFO
        // ======================================================================================================
        private string GPUFormatScreenInfo(Rectangle bounds){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            string formatted = bounds.ToString()
                .Replace("Width", TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_width")))
                .Replace("Height", TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_height")))
                .Replace("{", "")
                .Replace("}", "")
                .Replace(",", ", ")
                .Replace("=", ": ");
            return formatted;
        }
        private string GPUFormatPrimaryScreen(bool isPrimary){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            return isPrimary
                ? TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_yes"))
                : TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_no"));
        }
        #endregion
        #region DISK_Section
        // DISK
        // ======================================================================================================
        List<string> disk_man_list = new List<string>();
        List<string> disk_model_list = new List<string>();
        List<string> disk_volume_id_list = new List<string>();
        List<string> disk_volume_name_list = new List<string>();
        List<string> disk_physical_name_list = new List<string>();
        List<string> disk_firmware_list = new List<string>();
        List<string> disk_serial_list = new List<string>();
        List<string> disk_volume_serial_list = new List<string>();
        List<string> disk_total_space_list = new List<string>();
        List<string> disk_free_space_list = new List<string>();
        List<string> disk_file_system_list = new List<string>();
        List<string> disk_formatting_system_list = new List<string>();
        List<string> disk_type_list = new List<string>();
        List<string> disk_drive_type_list = new List<string>();
        List<string> disk_interface_list = new List<string>();
        List<string> disk_partition_list = new List<string>();
        List<string> disk_media_loaded_list = new List<string>();
        List<string> disk_media_status_list = new List<string>();
        List<string> disk_health_status_list = new List<string>();
        List<string> disk_boot_list = new List<string>();
        List<string> disk_bootable_list = new List<string>();
        List<string> disk_bitlocker_status_list = new List<string>();
        List<string> disk_bitlocker_conversionstatus_list = new List<string>();
        List<string> disk_bitlocker_encryptionmethod_list = new List<string>();
        List<string> disk_drive_compressed_list = new List<string>();
        private void disk(){
            // DISK COUNTER
            int disk_ssd_count = 0;
            int disk_hdd_count = 0;
            int disk_usb_count = 0;
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            try{
                var get_drives = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject drive_info in get_drives.Get()){
                    var device_id = drive_info.Properties["DeviceId"].Value;
                    var disk_part_text_query = string.Format("associators of {{{0}}} where AssocClass = Win32_DiskDriveToDiskPartition", drive_info.Path.RelativePath);
                    var disk_part_query = new ManagementObjectSearcher(disk_part_text_query);
                    foreach (ManagementObject disk_partition in disk_part_query.Get()){
                        var logical_disk_text_query = string.Format("associators of {{{0}}} where AssocClass = Win32_LogicalDiskToPartition", disk_partition.Path.RelativePath);
                        var logical_disk_query = new ManagementObjectSearcher(logical_disk_text_query);
                        foreach (ManagementObject logical_drive_info in logical_disk_query.Get()){
                            try{
                                // DISK CAPTION / DISK SELECT LIST
                                var disk_caption = Convert.ToString(drive_info.Properties["Caption"].Value).Trim();
                                if (disk_caption != "" || disk_caption != string.Empty){
                                    DISK_CaptionList.Items.Add(disk_caption);
                                }else{
                                    DISK_CaptionList.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
                                }
                            }catch (Exception){ }
                            try{
                                // DISK MANUFACTURER
                                List<string> disk_mfr = new List<string>(){ "acer", "adata", "a-data", "addlink", "alpin", "apacer", "apple",
                                "asus", "biostar", "codegen", "colorful", "corsair", "crucial", "ezcool", "geil", "gigabyte", "goodram",
                                "hi level", "hikvision", "hp", "intel", "intenso", "james donkey", "kingspec", "kingston", "kioxia",
                                "leven", "lexar", "micron", "mld", "msi", "mushkin", "neo forza", "neoforza", "netac", "patriot", "pioneer",
                                "pny", "samsung", "sandisk", "seagate", "siliconpower", "team", "toshiba", "turbox", "wd", "western digital",
                                "verbatim", "maxtor", "hitachi", "fujitsu", "transcend", "ocz", "hynix", "plextor", "liteon", "verico", "qnap",
                                "lenovo", "maxell", "teac", "quantum", "panasonic", "nec", "iomega", "buffalo", "buslink", "busbi", "centon",
                                "cm storm", "datawrite", "duracell", "dynamode", "emtec", "excelstor", "fuji", "hgst", "hyundai", "i-omega",
                                "imation", "kanguru", "kingmax", "kodak", "lacie", "memorex", "minox", "mio", "olympus", "optiarc", "philips",
                                "pinnacle", "pnypqi", "ramsta", "ricoh", "rokit", "sigma", "sk hynix", "sodimm", "smartbuy", "sony", "steve jobs",
                                "stuart hughes", "super talent", "trimble", "traxdata", "viking", "xerox", "xmedia", "zotac", "zoostorm" };
                                disk_mfr.Sort();
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher disk_model_and_man_query = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                foreach (ManagementObject disk_model_and_man_search in disk_model_and_man_query.Get()){
                                    // DISK MODEL
                                    var disk_model = Convert.ToString(disk_model_and_man_search["Model"]).Trim();
                                    if (disk_model != "" || disk_model != string.Empty){
                                        disk_model_list.Add(disk_model);
                                    }else{
                                        disk_model_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
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
                                                disk_man_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
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
                                if (disk_volume_id != "" || disk_volume_id != string.Empty){
                                    disk_volume_id_list.Add(disk_volume_id + @"\");
                                }else{
                                    disk_volume_id_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
                                }
                                DISK_VolumeID_V.Text = disk_volume_id_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK VOLUME NAME
                                var disk_volume_name = Convert.ToString(logical_drive_info.Properties["VolumeName"].Value).Trim();
                                if (disk_volume_name != "" || disk_volume_name != string.Empty){
                                    disk_volume_name_list.Add(disk_volume_name);
                                }else{
                                    disk_volume_name_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_local_disk")));
                                }
                                DISK_VolumeName_V.Text = disk_volume_name_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK PHYSICAL NAME
                                var disk_physical_name = Convert.ToString(drive_info.Properties["Name"].Value).Trim();
                                if (disk_physical_name != "" || disk_physical_name != string.Empty){
                                    disk_physical_name_list.Add(disk_physical_name);
                                }else{
                                    disk_physical_name_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
                                }
                                DISK_PhysicalName_V.Text = disk_physical_name_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK FIRMWARE VERSION
                                var disk_firmware_version = Convert.ToString(drive_info.Properties["FirmwareRevision"].Value).Trim();
                                if (disk_firmware_version != "" || disk_firmware_version != string.Empty){
                                    disk_firmware_list.Add(disk_firmware_version);
                                }else{
                                    disk_firmware_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
                                }
                                DISK_Firmware_V.Text = disk_firmware_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK SERIAL NUMBER
                                var disk_serial_number = Convert.ToString(drive_info.Properties["SerialNumber"].Value).Trim();
                                if (disk_serial_number != "" || disk_serial_number != string.Empty){
                                    if (hiding_mode_wrapper != 1){
                                        disk_serial_list.Add(disk_serial_number);
                                    }else{
                                        disk_serial_list.Add(new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})");
                                    }
                                }else{
                                    disk_serial_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
                                }
                                DISK_Serial_V.Text = disk_serial_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK VOLUME SERIAL NUMBER
                                var disk_volume_serial_number = Convert.ToString(logical_drive_info.Properties["VolumeSerialNumber"].Value).Trim();
                                if (disk_volume_serial_number != "" || disk_volume_serial_number != string.Empty){
                                    if (hiding_mode_wrapper != 1){
                                        disk_volume_serial_list.Add(disk_volume_serial_number);
                                    }else{
                                        disk_volume_serial_list.Add(new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})");
                                    }
                                }else{
                                    disk_volume_serial_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
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
                                if (disk_file_system != "" || disk_file_system != string.Empty){
                                    disk_file_system_list.Add(disk_file_system);
                                }else{
                                    disk_file_system_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
                                }
                                DISK_FileSystem_V.Text = disk_file_system_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK FORMATTING SYSTEM
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher disk_part_search = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                foreach (ManagementObject dp_search in disk_part_search.Get()){
                                   var disk_part_style = Convert.ToInt32(dp_search["PartitionStyle"]);
                                    switch (disk_part_style){
                                        case 0:
                                            disk_formatting_system_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
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
                                foreach (ManagementObject search_disk_t in search_disk_type.Get()){
                                    // DISK TYPE INTERFACE
                                    var disk_type = Convert.ToInt32(search_disk_t["MediaType"]);
                                    switch (disk_type){
                                        case 0:
                                            // Bilinmiyor
                                            switch (disk_media_type){
                                                case "external hard disk media":
                                                    disk_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_external_disk")));
                                                    disk_usb_count++;
                                                    break;
                                                case "removable media":
                                                    disk_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_usb_disk")));
                                                    disk_usb_count++;
                                                    break;
                                                default:
                                                    disk_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
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
                                                    disk_drive_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_fixed_drive")));
                                                    break;
                                                case 4:
                                                    disk_drive_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_ssd")));
                                                    break;
                                            }
                                            break;
                                        case "external hard disk media":
                                            disk_drive_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_external_drive")));
                                            break;
                                        case "removable media":
                                            disk_drive_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_removable_drive")));
                                            break;
                                        default:
                                            disk_drive_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
                                            break;
                                    }
                                    DISK_DriveType_V.Text = disk_drive_type_list[0];
                                }
                            }catch (Exception){ }
                            try{
                                // DISK INTERFACE
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher disk_interface = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                foreach (ManagementObject disk_int_query in disk_interface.Get()){
                                    var di_query = Convert.ToInt32(disk_int_query["BusType"]);
                                    var busTypeMessages = new Dictionary<int, string>{
                                        { 0, TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")) },
                                        { 1, "SCSI" },
                                        { 2, "ATAPI" },
                                        { 3, "ATA" },
                                        { 4, "1394 - IEEE 1394" },
                                        { 5, "SSA" },
                                        { 6, TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_fiber_channel")) },
                                        { 7, "USB" },
                                        { 8, "RAID" },
                                        { 9, "iSCSI" },
                                        { 10, TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_scsi_sas")) },
                                        { 11, "SATA" },
                                        { 12, TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_secure_digital")) },
                                        { 13, TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_multi_media_card")) },
                                        { 14, TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_virtual")) },
                                        { 15, TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_file_supported_virtual")) },
                                        { 16, TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_storage_area")) },
                                        { 17, "NVM-e" }
                                    };
                                    if (busTypeMessages.TryGetValue(di_query, out string busTypeMessage)){
                                        disk_interface_list.Add(busTypeMessage);
                                    }else{
                                        disk_interface_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
                                    }
                                }
                                DISK_InterFace_V.Text = disk_interface_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK PARTITION COUNT
                                var disk_partitions = Convert.ToString(drive_info.Properties["Partitions"].Value).Trim();
                                if (disk_partitions != "" || disk_partitions != string.Empty){
                                    disk_partition_list.Add(disk_partitions);
                                }else{
                                    disk_partition_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_unknown")));
                                }
                                DISK_PartitionCount_V.Text = disk_partition_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK MEDIA STATUS
                                var disk_media_loaded = Convert.ToBoolean(drive_info.Properties["MediaLoaded"].Value);
                                if (disk_media_loaded == true){
                                    disk_media_loaded_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_disk_write_and_read")));
                                }else if (disk_media_loaded == false){
                                    disk_media_loaded_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_disk_not_write_and_read")));
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
                                    disk_media_status_list.Add(TS_String_Encoder(message));
                                }else{
                                    disk_media_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_disk_status_non")));
                                }
                                DISK_MediaStatus_V.Text = disk_media_status_list[0];
                            }catch (Exception){ }
                            try{
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher search_disk_inf_4 = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                foreach (ManagementObject sdi_4 in search_disk_inf_4.Get()){
                                    // HEALTH STATUS
                                    var disk_health = Convert.ToInt32(sdi_4["HealthStatus"]);
                                    switch (disk_health){
                                        case 0:
                                            disk_health_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_healt_good")));
                                            break;
                                        case 1:
                                            disk_health_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_healt_running_and_error")));
                                            break;
                                        case 2:
                                            disk_health_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_defective")));
                                            break;
                                    }
                                    // BOOT DISK
                                    var disk_boot = Convert.ToBoolean(sdi_4["BootFromDisk"]);
                                    switch (disk_boot){
                                        case true:
                                            disk_boot_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_main_boot_disk")));
                                            break;
                                        case false:
                                            disk_boot_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_not_main_boot_disk")));
                                            break;
                                    }
                                    // BOOTABLE DISK
                                    var disk_bootable = Convert.ToBoolean(sdi_4["IsBoot"]);
                                    switch (disk_bootable){
                                        case true:
                                            disk_bootable_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_bootable_disk")));
                                            break;
                                        case false:
                                            disk_bootable_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_not_bootable_disk")));
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
                                    disk_drive_compressed_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_compressed")));   
                                }else if (disk_compressed_status == false){
                                    disk_drive_compressed_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_not_compressed")));
                                }
                                DISK_DriveCompressed_V.Text = disk_drive_compressed_list[0];
                            }catch (Exception){ }
                            try{
                                // MB BIOS TYPE
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher search_bios_type = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                foreach (ManagementObject sbt in search_bios_type.Get()){
                                    var disk_volume_id = Convert.ToString(logical_drive_info.Properties["Name"].Value).Trim();
                                    if (!string.IsNullOrEmpty(disk_volume_id)){
                                        if (windows_disk.Replace("\\", string.Empty) == disk_volume_id.ToString().Trim()){
                                            var disk_style = Convert.ToInt32(sbt["PartitionStyle"]);
                                            if (disk_style == 1){
                                                // MBR
                                                MB_BiosMode_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_old")) + " (Legacy)";
                                            }else if (disk_style == 2){
                                                // GPT
                                                MB_BiosMode_V.Text = "UEFI";
                                            }else{
                                                // NULL
                                                MB_BiosMode_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Mb_Content", "mb_c_old")) + " (Legacy)";
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
                    foreach (ManagementObject query_bitlocker in get_bitlocker_status.Get()){
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
                                    disk_bitlocker_status_list.Add(TS_String_Encoder(message));
                                }else{
                                    disk_bitlocker_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_ps_unknown")));
                                }
                            }else{
                                disk_bitlocker_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_ps_unknown")));
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
                                    disk_bitlocker_conversionstatus_list.Add(TS_String_Encoder(message));
                                }else{
                                    disk_bitlocker_conversionstatus_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_cs_status_unknown")));
                                }
                            }else{
                                disk_bitlocker_conversionstatus_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_cs_status_unknown")));
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
                                    disk_bitlocker_encryptionmethod_list.Add(TS_String_Encoder(message));
                                }else{
                                    disk_bitlocker_encryptionmethod_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_pass_unknown")));
                                }
                            }else{
                                disk_bitlocker_encryptionmethod_list.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_bitlocker_em_pass_unknown")));
                            }
                            DISK_BitLockerEncryptMehod_V.Text = disk_bitlocker_encryptionmethod_list[0];
                        }catch (Exception){ }
                    }
                }
            }catch (Exception){ }
            // SELECT DISK
            try{
                int c_index = disk_volume_id_list.FindIndex(x => x.Contains(windows_disk));
                DISK_CaptionList.SelectedIndex = c_index;
                if (c_index == -1){
                    DISK_CaptionList.SelectedIndex = 0;
                }
            }catch (Exception){
                DISK_CaptionList.SelectedIndex = 0;
            }
            // DISK COUNTER RENDERER
            DISK_TTLP_P1_L2.Text = disk_ssd_count.ToString();
            DISK_TTLP_P2_L2.Text = disk_hdd_count.ToString();
            DISK_TTLP_P3_L2.Text = disk_usb_count.ToString();
            DISK_TTLP_P4_L2.Text = DISK_CaptionList.Items.Count.ToString();
            // DISK PROCESS END ENABLED
            DISK_RotateBtn.Enabled = true;
            ((Control)DISK).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Storage Section Loaded --->"); }
        }
        private void DISK_CaptionList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int disk_percent = DISK_CaptionList.SelectedIndex;
                try{ DISK_Model_V.Text = disk_model_list[disk_percent]; }catch(Exception){ }
                try{ DISK_Man_V.Text = disk_man_list[disk_percent]; }catch(Exception){ }
                try{ DISK_VolumeID_V.Text = disk_volume_id_list[disk_percent]; }catch(Exception){ }
                try{ DISK_VolumeName_V.Text = disk_volume_name_list[disk_percent]; }catch(Exception){ }
                try{ DISK_PhysicalName_V.Text = disk_physical_name_list[disk_percent]; }catch(Exception){ }
                try{ DISK_Firmware_V.Text = disk_firmware_list[disk_percent]; }catch(Exception){ }
                try{ DISK_Serial_V.Text = disk_serial_list[disk_percent]; }catch(Exception){ }
                try{ DISK_VolumeSerial_V.Text = disk_volume_serial_list[disk_percent]; }catch(Exception){ }
                try{ DISK_Size_V.Text = disk_total_space_list[disk_percent]; }catch(Exception){ }
                try{ DISK_FreeSpace_V.Text = disk_free_space_list[disk_percent]; }catch(Exception){ }
                try{ DISK_FileSystem_V.Text = disk_file_system_list[disk_percent]; }catch(Exception){ }
                try{ DISK_FormattingType_V.Text = disk_formatting_system_list[disk_percent]; }catch(Exception){ }
                try{ DISK_Type_V.Text = disk_type_list[disk_percent]; }catch(Exception){ }
                try{ DISK_DriveType_V.Text = disk_drive_type_list[disk_percent]; }catch(Exception){ }
                try{ DISK_InterFace_V.Text = disk_interface_list[disk_percent]; }catch(Exception){ }
                try{ DISK_PartitionCount_V.Text = disk_partition_list[disk_percent]; }catch(Exception){ }
                try{ DISK_MediaLoaded_V.Text = disk_media_loaded_list[disk_percent]; }catch(Exception){ }
                try{ DISK_MediaStatus_V.Text = disk_media_status_list[disk_percent]; }catch(Exception){ }
                try{ DISK_Health_V.Text = disk_health_status_list[disk_percent]; }catch(Exception){ }
                try{ DISK_Boot_V.Text = disk_boot_list[disk_percent]; }catch(Exception){ }
                try{ DISK_Bootable_V.Text = disk_bootable_list[disk_percent]; }catch(Exception){ }
                try{ DISK_BitLockerStatus_V.Text = disk_bitlocker_status_list[disk_percent]; }catch(Exception){ }
                try{ DISK_BitLockerConversionStatus_V.Text = disk_bitlocker_conversionstatus_list[disk_percent]; }catch(Exception){ }
                try{ DISK_BitLockerEncryptMehod_V.Text = disk_bitlocker_encryptionmethod_list[disk_percent]; }catch(Exception){ }
                try{ DISK_DriveCompressed_V.Text = disk_drive_compressed_list[disk_percent]; }catch(Exception){ }
            }catch (Exception){ }
        }
        #endregion
        #region NETWORK_Section
        // NETWORK
        // ======================================================================================================
        List<string> network_mac_adress_list = new List<string>();
        List<string> network_man_list = new List<string>();
        List<string> network_service_name_list = new List<string>();
        List<string> network_adaptor_type_list = new List<string>();
        List<string> network_physical_list = new List<string>();
        List<string> network_device_id_list = new List<string>();
        List<string> network_guid_list = new List<string>();
        List<string> network_connection_type_list = new List<string>();
        List<string> network_dhcp_status_list = new List<string>();
        List<string> network_dhcp_server_list = new List<string>();
        List<string> network_connection_speed_list = new List<string>();
        List<string> network_ipv4_list = new List<string>();
        List<string> network_ipv6_list = new List<string>();
        private void network(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_na = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter");
            foreach (ManagementObject query_na_rotate in search_na.Get()){
                try{
                    // NET NAME
                    NET_ListNetwork.Items.Add(Convert.ToString(query_na_rotate["Name"]));
                }catch (Exception){ }
                try{
                    // MAC ADRESS
                    string mac_adress = Convert.ToString(query_na_rotate["MACAddress"]);
                    if (string.IsNullOrEmpty(mac_adress)){
                        network_mac_adress_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_not_mac_adress")));
                    }else{
                        if (hiding_mode_wrapper != 1){
                            network_mac_adress_list.Add(mac_adress);
                        }else{
                            network_mac_adress_list.Add(new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})");
                        }
                    }
                    NET_MacAdress_V.Text = network_mac_adress_list[0];
                }catch (Exception){ }
                try{
                    // NET MAN
                    string net_man = Convert.ToString(query_na_rotate["Manufacturer"]);
                    if (string.IsNullOrEmpty(net_man)){
                        network_man_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_not_provider")));
                    }else{
                        network_man_list.Add(net_man);
                    }
                    NET_NetMan_V.Text = network_man_list[0];
                }catch (Exception){ }
                try{
                    // SERVICE NAME
                    string service_name = Convert.ToString(query_na_rotate["ServiceName"]);
                    if (string.IsNullOrEmpty(service_name)){
                        network_service_name_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_not_service_name")));
                    }else{
                        network_service_name_list.Add(service_name);
                    }
                    NET_ServiceName_V.Text = network_service_name_list[0];
                }catch (Exception){ }
                try{
                    // NET ADAPTER TYPE
                    string adaptor_type = Convert.ToString(query_na_rotate["AdapterType"]);
                    if (string.IsNullOrEmpty(adaptor_type)){
                        network_adaptor_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_not_adaptor_type")));
                    }else{
                        network_adaptor_type_list.Add(adaptor_type);
                    }
                    NET_AdapterType_V.Text = network_adaptor_type_list[0];
                }catch (Exception){ }
                try{
                    // NET PHYSICAL
                    bool net_physical = Convert.ToBoolean(query_na_rotate["PhysicalAdapter"]);
                    if (net_physical == true){
                        network_physical_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_yes")));
                    }else if (net_physical == false){
                        network_physical_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_no")));
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
                        network_device_id_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_not_hardware_id")));
                    }
                    NET_DeviceID_V.Text = network_device_id_list[0];
                }catch (Exception){ }
                try{
                    // NET GUID
                    string guid = Convert.ToString(query_na_rotate["GUID"]);
                    if (string.IsNullOrEmpty(guid)){
                        network_guid_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_not_guid")));
                    }else{
                        string net_guid_replace_1 = guid.Replace("{", string.Empty);
                        string net_guid_replace_2 = net_guid_replace_1.Replace("}", string.Empty);
                        if (hiding_mode_wrapper != 1){
                            network_guid_list.Add(net_guid_replace_2);
                        }else{
                            network_guid_list.Add(new string('*', vis_m_property.Next(vn_range[0], vn_range[1])) + $" ({TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on_ui"))})");
                        }
                    }
                    NET_Guid_V.Text = network_guid_list[0];
                }catch (Exception){ }
                try{
                    // NET CONNECTION TYPE
                    string net_con_id = Convert.ToString(query_na_rotate["NetConnectionID"]);
                    if (string.IsNullOrEmpty(net_con_id)){
                        network_connection_type_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_not_connection_type")));
                    }else{
                        network_connection_type_list.Add(net_con_id);
                    }
                    NET_ConnectionType_V.Text = network_connection_type_list[0];
                }catch (Exception){ }
                // NETWORK ADAPTER CONFIG SECTION
                try{
                    var get_na_index = query_na_rotate["Index"];
                    ManagementObjectSearcher search_nac = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index={get_na_index}");
                    foreach (ManagementObject query_nac_rotate in search_nac.Get()){
                        try{
                            // DHCP STATUS
                            bool dhcp_enabled = Convert.ToBoolean(query_nac_rotate["DHCPEnabled"]);
                            if (dhcp_enabled == true){
                                network_dhcp_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_active")));
                            }else if (dhcp_enabled == false){
                                network_dhcp_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_out_of_order")));
                            }
                            NET_Dhcp_status_V.Text = network_dhcp_status_list[0];
                        }catch (Exception){ }
                        try{
                            // DHCP SERVER STATUS
                            string dhcp_server = Convert.ToString(query_nac_rotate["DHCPServer"]);
                            if (string.IsNullOrEmpty(dhcp_server)){
                                network_dhcp_server_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_unknown")));
                            }else{
                                network_dhcp_server_list.Add(dhcp_server);
                            }
                            NET_Dhcp_server_V.Text = network_dhcp_server_list[0];
                        }catch (Exception){ }
                    }
                }catch (Exception){ }
                // NETWORK ADAPTER CONFIG SECTION
                try{
                    // MODEM CONNECT SPEED
                    string local_con_speed = Convert.ToString(query_na_rotate["Speed"]);
                    if (local_con_speed == "" || local_con_speed == "Unknown"){
                        network_connection_speed_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_not_connect")));
                    }else{
                        double net_speed_cal = Convert.ToInt32(local_con_speed) / 1000000.0;
                        double net_speed_download_cal = Convert.ToDouble(net_speed_cal) / 8;
                        network_connection_speed_list.Add(net_speed_cal.ToString() + " Mbps - (" + string.Format("{0:0.0} MB/s)", net_speed_download_cal));
                    }
                    NET_LocalConSpeed_V.Text = network_connection_speed_list[0];
                }catch (Exception){ }
                try{
                    // IPV4 & IPV6 Adress
                    var get_na_index = query_na_rotate["Index"];
                    ManagementObjectSearcher query_na_ip = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index={get_na_index}");
                    foreach (ManagementObject search_ip in query_na_ip.Get()){
                        if (search_ip["IPAddress"] == null){
                            network_ipv4_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_unknown")));
                            network_ipv6_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_unknown")));
                        }else{
                            string[] arrIPAddress = (string[])(search_ip["IPAddress"]);
                            foreach (string ipAddress in arrIPAddress){
                                if (IPAddress.TryParse(ipAddress, out IPAddress address)){
                                    if (address.AddressFamily == AddressFamily.InterNetwork){
                                        network_ipv4_list.Add(ipAddress);
                                    }else if (address.AddressFamily == AddressFamily.InterNetworkV6){
                                        network_ipv6_list.Add(ipAddress);
                                    }
                                }
                            }
                        }
                    }
                }catch (Exception){ }
            }
            // DNS 1 And DNS 2 Adress
            try{
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = True");
                ManagementObjectCollection results = searcher.Get();
                string dns1 = "", dns2 = "";
                foreach (ManagementObject obj in results){
                    string[] dnsAddresses = (string[])obj["DNSServerSearchOrder"];
                    if (dnsAddresses != null && dnsAddresses.Length > 0){
                        dns1 = dnsAddresses[0];
                        if (dnsAddresses.Length > 1){
                            dns2 = dnsAddresses[1];
                        }
                        break;
                    }
                }
                NET_DNS1_V.Text = !string.IsNullOrEmpty(dns1) ? dns1 : TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_dns_not"));
                NET_DNS2_V.Text = !string.IsNullOrEmpty(dns2) ? dns2 : TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_dns_not"));
            }catch (Exception){ }
            // NETWORK SELECT
            try{
                int net_moduler = 0;
                var targetBrands = new[] { "realtek", "qualcomm", "killer", "mediatek" };
                for (int i = 0; i < NET_ListNetwork.Items.Count; i++){
                    string net_list_item = NET_ListNetwork.Items[i].ToString().ToLower();
                    if (targetBrands.Any(brand => net_list_item.Contains(brand))){
                        NET_ListNetwork.SelectedIndex = net_moduler;
                        break;
                    }else{
                        NET_ListNetwork.SelectedIndex = 1;
                        net_moduler++;
                    }
                }
            }catch (Exception){ }
            // NETWORK PROCESS END ENABLED
            NET_RotateBtn.Enabled = true;
            ((Control)NETWORK).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Network Section Loaded --->"); }
        }
        private void listnetwork_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int network_select = NET_ListNetwork.SelectedIndex;
                try{ NET_MacAdress_V.Text = network_mac_adress_list[network_select]; }catch (Exception){ }
                try{ NET_NetMan_V.Text = network_man_list[network_select]; }catch (Exception){ }
                try{ NET_ServiceName_V.Text = network_service_name_list[network_select]; }catch (Exception){ }
                try{ NET_AdapterType_V.Text = network_adaptor_type_list[network_select]; }catch (Exception){ }
                try{ NET_Physical_V.Text = network_physical_list[network_select]; }catch (Exception){ }
                try{ NET_DeviceID_V.Text = network_device_id_list[network_select]; }catch (Exception){ }
                try{ NET_Guid_V.Text = network_guid_list[network_select]; }catch (Exception){ }
                try{ NET_ConnectionType_V.Text = network_connection_type_list[network_select]; }catch (Exception){ }
                try{ NET_Dhcp_status_V.Text = network_dhcp_status_list[network_select]; }catch (Exception){ }
                try{ NET_Dhcp_server_V.Text = network_dhcp_server_list[network_select]; }catch (Exception){ }
                try{ NET_LocalConSpeed_V.Text = network_connection_speed_list[network_select]; }catch (Exception){ }
                try{ NET_IPv4Adress_V.Text = network_ipv4_list[network_select]; }catch (Exception){ }
                try{ NET_IPv6Adress_V.Text = network_ipv6_list[network_select]; }catch (Exception){ }
            }catch(Exception){ }
        }
        private async void live_network_process(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                string activeAdapter = net_replacer(GetActiveNetworkAdapter());
                double activeAdapterBandwidth = 0;
                ManagementObjectSearcher searcher_net_speed = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkInterface");
                foreach (ManagementObject search_speed in searcher_net_speed.Get()){
                    if (net_replacer(search_speed["Name"].ToString().Trim()) == activeAdapter){
                        activeAdapterBandwidth = Math.Round((ulong)search_speed["CurrentBandwidth"] / 1000000.0, 2);
                        NET_LT_Device_V.Text = activeAdapter.Trim();
                        NET_LT_BandWidth_V.Text = string.Format("{0} Mbps", activeAdapterBandwidth);
                        break;
                    }
                }
                do{
                    if (loop_status == false){ break; }
                    foreach (ManagementObject search_active_speed in searcher_net_speed.Get()){
                        if (net_replacer(search_active_speed["Name"].ToString().Trim()) == activeAdapter){
                            ulong bytesReceived = (ulong)search_active_speed["BytesReceivedPerSec"];
                            ulong bytesSent = (ulong)search_active_speed["BytesSentPerSec"];
                            double mbpsReceived = Math.Round(bytesReceived * 8 / 1000000.0, 2);
                            double mbpsSent = Math.Round(bytesSent * 8 / 1000000.0, 2);
                            NET_LT_DL2.Text = string.Format("{0:0.00} Mbps / ({1:0.00} MB/s)", mbpsReceived, mbpsReceived / 8);
                            NET_LT_UL2.Text = string.Format("{0:0.00} Mbps / ({1:0.00} MB/s)", mbpsSent, mbpsSent / 8);
                        }
                    }
                    await Task.Delay(1000 - DateTime.Now.Millisecond, Program.TS_TokenEngine.Token).ConfigureAwait(false);
                } while (loop_status == true);
            }catch (Exception){ }
        }
        private static string GetActiveNetworkAdapter(){
            try{
                ManagementObjectSearcher searcher_active_net = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionStatus=2");
                foreach (ManagementObject search_ip_enabled in searcher_active_net.Get()){
                    ManagementObjectSearcher configSearcher = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index={search_ip_enabled["Index"]} AND IPEnabled=True");
                    foreach (ManagementObject configObj in configSearcher.Get()){
                        return search_ip_enabled["Name"].ToString().Trim();
                    }
                }
            }catch (Exception){ }
            return null;
        }
        #endregion
        #region USB_Section
        // USB
        // ======================================================================================================
        List<string> usb_con_name_list = new List<string>();
        List<string> usb_con_manufacturer_list = new List<string>();
        List<string> usb_con_device_id_list = new List<string>();
        List<string> usb_con_pnp_device_id_list = new List<string>();
        List<string> usb_con_device_status_list = new List<string>();
        List<string> usb_device_name_list = new List<string>();
        List<string> usb_device_id_list = new List<string>();
        List<string> usb_pnp_device_id_list = new List<string>();
        List<string> usb_device_status_list = new List<string>();
        private void usb(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_usb_controller = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_USBController");
            ManagementObjectSearcher search_usb_hub = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_USBHub");
            foreach (ManagementObject query_usb_con in search_usb_controller.Get()){
                // USB CON CAPTION
                try{
                    string usb_con_caption = Convert.ToString(query_usb_con["Caption"]).Trim();
                    if (usb_con_caption != "" && usb_con_caption != string.Empty){
                        USB_ConList.Items.Add(usb_con_caption);
                    }else{
                        USB_ConList.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown_device")));
                    }
                }catch (Exception){ }
                // USB CON NAME
                try{
                    string usb_con_name = Convert.ToString(query_usb_con["Name"]);
                    if (usb_con_name != "" && usb_con_name != string.Empty){
                        usb_con_name_list.Add(usb_con_name);
                    }else{
                        usb_con_name_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown")));
                    }
                    USB_ConName_V.Text = usb_con_name_list[0];
                }catch (Exception){ }
                // USB CON MANUFACTURER
                try{
                    string usb_con_man = Convert.ToString(query_usb_con["Manufacturer"]);
                    if (usb_con_man != "" && usb_con_man != string.Empty){
                        usb_con_manufacturer_list.Add(usb_con_man);
                    }else{
                        usb_con_manufacturer_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown")));
                    }
                    USB_ConMan_V.Text = usb_con_manufacturer_list[0];
                }catch (Exception){ }
                // USB CON DEVICE ID
                try{
                    string usb_con_device_id = Convert.ToString(query_usb_con["DeviceID"]);
                    if (usb_con_device_id != "" && usb_con_device_id != string.Empty){
                        char[] split_char = { '\\' };
                        string[] usb_con_device_split = usb_con_device_id.Trim().Split(split_char);
                        usb_con_device_id_list.Add($"{usb_con_device_split[0]}\\{usb_con_device_split[1]}");
                    }else{
                        usb_con_device_id_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown")));
                    }
                    USB_ConDeviceID_V.Text = usb_con_device_id_list[0];
                }catch (Exception){ }
                // USB CON PNP DEVICE ID
                try{
                    string usb_con_pnp_device_id = Convert.ToString(query_usb_con["PNPDeviceID"]);
                    if (usb_con_pnp_device_id != "" && usb_con_pnp_device_id != string.Empty){
                        char[] split_char = { '\\' };
                        string[] usb_con_pnp_device_split = usb_con_pnp_device_id.Trim().Split(split_char);
                        usb_con_pnp_device_id_list.Add($"{usb_con_pnp_device_split[0]}\\{usb_con_pnp_device_split[1]}");
                    }else{
                        usb_con_pnp_device_id_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown")));
                    }
                    USB_ConPNPDeviceID_V.Text = usb_con_pnp_device_id_list[0];
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
                            usb_con_device_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", statusKey)));
                        }else{
                            usb_con_device_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown")));
                        }
                    }else{
                        usb_con_device_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown")));
                    }
                    USB_ConDeviceStatus_V.Text = usb_con_device_status_list[0];
                }catch (Exception){ }
            }
            foreach (ManagementObject query_usb in search_usb_hub.Get()){
                // USB CAPTION
                try{
                    string usb_caption = Convert.ToString(query_usb["Caption"]).Trim();
                    if (usb_caption != "" && usb_caption != string.Empty){
                        USB_Select.Items.Add(usb_caption);
                    }else{
                        USB_Select.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown_device")));
                    }
                }catch (Exception){ }
                // USB NAME
                try{
                    string usb_name = Convert.ToString(query_usb["Name"]);
                    if (usb_name != "" && usb_name != string.Empty){
                        usb_device_name_list.Add(usb_name);
                    }else{
                        usb_device_name_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown")));
                    }
                    USB_DeviceName_V.Text = usb_device_name_list[0];
                }catch (Exception){ }
                // USB DEVICE ID
                try{
                    string usb_device_id = Convert.ToString(query_usb["DeviceID"]);
                    if (usb_device_id != "" && usb_device_id != string.Empty){
                        char[] split_char = { '\\' };
                        string[] usb_device_split = usb_device_id.Trim().Split(split_char);
                        usb_device_id_list.Add($"{usb_device_split[0]}\\{usb_device_split[1]}");
                    }else{
                        usb_device_id_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown")));
                    }
                    USB_DeviceID_V.Text = usb_device_id_list[0];
                }catch (Exception){ }
                // USB PNP DEVICE ID
                try{
                    string usb_pnp_device_id = Convert.ToString(query_usb["PNPDeviceID"]);
                    if (usb_pnp_device_id != "" && usb_pnp_device_id != string.Empty){
                        char[] split_char = { '\\' };
                        string[] usb_pnp_device_split = usb_pnp_device_id.Trim().Split(split_char);
                        usb_pnp_device_id_list.Add($"{usb_pnp_device_split[0]}\\{usb_pnp_device_split[1]}");
                    }else{
                        usb_pnp_device_id_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown")));
                    }
                    USB_PNPDeviceID_V.Text = usb_pnp_device_id_list[0];
                }catch (Exception){ }
                // USB DEVICE STATUS
                try{
                    string usb_device_status = Convert.ToString(query_usb["Status"]).Trim().ToLower();
                    if (!string.IsNullOrEmpty(usb_device_status)){
                        var usbDeviceStatuses = new Dictionary<string, string>{
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
                        if (usbDeviceStatuses.TryGetValue(usb_device_status, out string statusKey)){
                            usb_device_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", statusKey)));
                        }else{
                            usb_device_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown")));
                        }
                    }else{
                        usb_device_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_unknown")));
                    }
                    USB_DeviceStatus_V.Text = usb_device_status_list[0];
                }catch (Exception){ }
            }
            try{ USB_ConList.SelectedIndex = 0; }catch (Exception){ }
            try{ USB_Select.SelectedIndex = 0; }catch (Exception){ }
            // USB PROCESS END ENABLED
            USB_RotateBtn.Enabled = true;
            ((Control)USB).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- USB Section Installed --->"); }
        }
        private void USB_ConList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int usb_con_select = USB_ConList.SelectedIndex;
                try{ USB_ConName_V.Text = usb_con_name_list[usb_con_select]; }catch(Exception){ }
                try{ USB_ConMan_V.Text = usb_con_manufacturer_list[usb_con_select]; }catch(Exception){ }
                try{ USB_ConDeviceID_V.Text = usb_con_device_id_list[usb_con_select]; }catch(Exception){ }
                try{ USB_ConPNPDeviceID_V.Text = usb_con_pnp_device_id_list[usb_con_select]; }catch(Exception){ }
                try{ USB_ConDeviceStatus_V.Text = usb_con_device_status_list[usb_con_select]; }catch (Exception){ }
            }catch (Exception){ }
        }
        private void USB_Select_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int usb_select = USB_Select.SelectedIndex;
                try{ USB_DeviceName_V.Text = usb_device_name_list[usb_select]; }catch(Exception){ }
                try{ USB_DeviceID_V.Text = usb_device_id_list[usb_select]; }catch(Exception){ }
                try{ USB_PNPDeviceID_V.Text = usb_pnp_device_id_list[usb_select]; }catch(Exception){ }
                try{ USB_DeviceStatus_V.Text = usb_device_status_list[usb_select]; }catch(Exception){ }
            }catch(Exception){ }
        }
        #endregion
        #region SOUND_Section
        // SOUND
        // ======================================================================================================
        List<string> sound_device_name_list = new List<string>();
        List<string> sound_device_manufacturer_list = new List<string>();
        List<string> sound_device_id_list = new List<string>();
        List<string> sound_pnp_device_id_list = new List<string>();
        List<string> sound_device_status_list = new List<string>();
        private void sound(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_sound = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SoundDevice");
            foreach (ManagementObject query_sound in search_sound.Get()){
                // SOUND DEVICE CAPTION
                try{
                    string sound_caption = Convert.ToString(query_sound["Caption"]);
                    if (sound_caption != "" && sound_caption != string.Empty){
                        SOUND_Select.Items.Add(sound_caption);
                    }else{
                        SOUND_Select.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown_device")));
                    }
                }catch (Exception){ }
                // SOUND DEVICE NAME
                try{
                    string sound_name = Convert.ToString(query_sound["Name"]);
                    if (sound_name != "" && sound_name != string.Empty){
                        sound_device_name_list.Add(sound_name);
                    }else{
                        sound_device_name_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown")));
                    }
                    SOUND_DeviceName_V.Text = sound_device_name_list[0];
                }catch (Exception){ }
                // SOUND DEVICE MANUFACTURER
                try{
                    string sound_manfuacturer = Convert.ToString(query_sound["Manufacturer"]);
                    if (sound_manfuacturer != "" && sound_manfuacturer != string.Empty){
                        sound_device_manufacturer_list.Add(sound_manfuacturer);
                    }else{
                        sound_device_manufacturer_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown")));
                    }
                    SOUND_DeviceManufacturer_V.Text = sound_device_manufacturer_list[0];
                }catch (Exception){ }
                // SOUND DEVICE ID
                try{
                    string sound_device_id = Convert.ToString(query_sound["DeviceID"]);
                    if (sound_device_id != "" && sound_device_id != string.Empty){
                        char[] split_char = { '\\' };
                        string[] sound_device_split = sound_device_id.Trim().Split(split_char);
                        sound_device_id_list.Add($"{sound_device_split[0]}\\{sound_device_split[1]}");
                    }else{
                        sound_device_id_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown")));
                    }
                    SOUND_DeviceID_V.Text = sound_device_id_list[0];
                }catch (Exception){ }
                // SOUND PNP DEVICE ID
                try{
                    string sound_pnp_device_id = Convert.ToString(query_sound["PNPDeviceID"]);
                    if (sound_pnp_device_id != "" && sound_pnp_device_id != string.Empty){
                        char[] split_char = { '\\' };
                        string[] sound_pnp_device_split = sound_pnp_device_id.Trim().Split(split_char);
                        sound_pnp_device_id_list.Add($"{sound_pnp_device_split[0]}\\{sound_pnp_device_split[1]}");
                    }else{
                        sound_pnp_device_id_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown")));
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
                            sound_device_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Sound_Content", statusKey)));
                        }else{
                            sound_device_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown")));
                        }
                    }else{
                        sound_device_status_list.Add(TS_String_Encoder(software_lang.TSReadLangs("Sound_Content", "sound_c_unknown")));
                    }
                    SOUND_DeviceStatus_V.Text = sound_device_status_list[0];
                }catch (Exception){ }
            }
            try { SOUND_Select.SelectedIndex = 0; }catch (Exception){ }
            // SOUND PROCESS END ENABLED
            SOUND_RotateBtn.Enabled = true;
            ((Control)SOUND).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Sound Section Loaded --->"); }
        }
        private void SOUND_Select_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int sound_select = SOUND_Select.SelectedIndex;
                try{ SOUND_DeviceName_V.Text = sound_device_name_list[sound_select]; }catch(Exception){ }
                try{ SOUND_DeviceManufacturer_V.Text = sound_device_manufacturer_list[sound_select]; }catch(Exception){ }
                try{ SOUND_DeviceID_V.Text = sound_device_id_list[sound_select]; }catch(Exception){ }
                try{ SOUND_PNPDeviceID_V.Text = sound_pnp_device_id_list[sound_select]; }catch(Exception){ }
                try{ SOUND_DeviceStatus_V.Text = sound_device_status_list[sound_select]; }catch (Exception){ }
            }catch (Exception){ }
        }
        #endregion
        #region BATTERY_Section
        // BATTERY
        // ======================================================================================================
        private void battery(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            ManagementObjectSearcher search_battery = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Battery");
            foreach (ManagementObject query_battery_rotate in search_battery.Get()){
                try{
                    // BATTERY ID                        
                    BATTERY_Model_V.Text = Convert.ToString(query_battery_rotate["DeviceID"]).Trim();
                }catch(Exception){ }
                try{
                    // BATTERY NAME
                    BATTERY_Name_V.Text = Convert.ToString(query_battery_rotate["Name"]).Trim();
                }catch(Exception){ }
                try{
                    // BATTERY TYPE
                    int battery_structure = Convert.ToInt32(query_battery_rotate["Chemistry"]);
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
                        BATTERY_Type_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery_Content", batteryKey));
                    }else{
                        BATTERY_Type_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery_Content", "by_c_unknown"));
                    }
                }catch (Exception){ }
            }
            // BATTERY PROCESS END ENABLED
            BATTERY_RotateBtn.Enabled = true;
            ((Control)BATTERY).Enabled = true;
            if (Program.debug_mode){ Console.WriteLine("<--- Battery Section Loaded --->"); }
        }
        private void battery_visible_off(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            BATTERY_Status_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery_Content", "by_c_battery_not_detect"));
            BATTERY_Model.Visible = false;
            BATTERY_Model_V.Visible = false;
            BATTERY_Name.Visible = false;
            BATTERY_Name_V.Visible = false;
            BATTERY_Voltage.Visible = false;
            BATTERY_Voltage_V.Visible = false;
            BATTERY_Type.Visible = false;
            BATTERY_Type_V.Visible = false;
            BATTERY_ReportBtn.Visible = false;
            BATTERY_PBG_Panel.Visible = false;
            BATTERY_PFE_Panel.Visible = false;
            BATTERY_ProgressLabel.Visible = false;
        }
        private void battery_visible_on(){
            BATTERY_Model.Visible = true;
            BATTERY_Model_V.Visible = true;
            BATTERY_Name.Visible = true;
            BATTERY_Name_V.Visible = true;
            BATTERY_Voltage.Visible = true;
            BATTERY_Voltage_V.Visible = true;
            BATTERY_Type.Visible = true;
            BATTERY_Type_V.Visible = true;
            BATTERY_ReportBtn.Visible = true;
            BATTERY_PBG_Panel.Visible = true;
            BATTERY_PFE_Panel.Visible = true;
            BATTERY_ProgressLabel.Visible = true;
        }
        private async void laptop_bg_process(){
            try{
                ManagementObjectSearcher search_battery = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Battery");
                PowerStatus power = SystemInformation.PowerStatus;
                do{
                    if (loop_status == false){ break; }
                    TSGetLangs software_lang = new TSGetLangs(lang_path);
                    Single battery = power.BatteryLifePercent;
                    Single battery_process = battery * 100;
                    string battery_status = Convert.ToString(battery_process) + "%";
                    foreach (ManagementObject query_battery_rotate in search_battery.Get()){
                        try{
                            // BATTERY VOLTAGE
                            double battery_voltage = Convert.ToDouble(query_battery_rotate["DesignVoltage"]) / 1000.0;
                            BATTERY_Voltage_V.Text = string.Format("{0:0.0} Volt", battery_voltage);
                        }catch (Exception){ }
                    }
                    BATTERY_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_battery")) + " - " + battery_status;
                    if (power.PowerLineStatus == PowerLineStatus.Online){
                        if (battery_process == 100){
                            BATTERY_Status_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery_Content", "by_c_battery_full")) + " - " + battery_status;
                        }else{
                            BATTERY_Status_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery_Content", "by_c_charging")) + " - " + battery_status;
                        }
                        BATTERY_PFE_Panel.Height = (int)(BATTERY_PBG_Panel.Height * (battery_process / 100.0));
                        BATTERY_ProgressLabel.Text = battery_status;
                        BATTERY_ProgressLabel.Top = BATTERY_PFE_Panel.Top + 6;
                    }else{
                        BATTERY_Status_V.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery_Content", "by_c_discharging")) + " - " + battery_status;
                        BATTERY_PFE_Panel.Height = (int)(BATTERY_PBG_Panel.Height * (battery_process / 100.0));
                        BATTERY_ProgressLabel.Text = battery_status;
                        BATTERY_ProgressLabel.Top = BATTERY_PFE_Panel.Top + 6;
                    }
                    await Task.Delay(1000 - DateTime.Now.Millisecond, Program.TS_TokenEngine.Token).ConfigureAwait(false);
                } while (loop_status == true);
            }catch (Exception){ }
        }
        string battery_report_path = Application.StartupPath + @"\battery-report.html";
        private void BATTERY_ReportBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                if (File.Exists(battery_report_path)){
                    File.Delete(battery_report_path);
                }
                Process.Start("cmd.exe", "/k " + $"title {string.Format(TS_String_Encoder(software_lang.TSReadLangs("Battery_Content", "by_report_create_title")), Application.ProductName + " -")} & powercfg /batteryreport & exit");
                StartBatteryReportProcess();
            }catch (Exception){ }
        }
        private async void StartBatteryReportProcess(){
            try{
                await Task.Run(() => battery_report_check_process(), Program.TS_TokenEngine.Token);
            }catch (Exception){ }
        }
        private void battery_report_check_process(){
            try{
                while (true){
                    if (File.Exists(battery_report_path)){
                        while (true) try { new FileInfo(battery_report_path).Open(FileMode.Open, FileAccess.Read, FileShare.None).Close(); break; }catch{ }
                        string new_battery_report_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\" + Application.ProductName + "_" + Dns.GetHostName() + @"_battery_report_" + DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss") + ".html";
                        File.Move(battery_report_path, new_battery_report_path);
                        TSGetLangs software_lang = new TSGetLangs(lang_path);
                        DialogResult br_message = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Battery_Content", "by_report_create_message")), new_battery_report_path, "\n\n"));
                        if (br_message == DialogResult.Yes){
                            Process.Start(new_battery_report_path);
                        }
                        break;
                    }
                }
            }catch (Exception){ }
        }
        #endregion
        #region OSD_Section
        // OSD
        // ======================================================================================================
        private async Task osd(){
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
                var search_drivers = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SystemDriver");
                var get_drivers_result = await Task.Run(() => search_drivers.Get().Cast<ManagementObject>().ToList(), Program.TS_TokenEngine.Token);
                foreach (var queryDriver in get_drivers_result){
                    string driverPath = Convert.ToString(queryDriver["PathName"])?.Replace("\"", string.Empty).Replace("\\??\\", string.Empty).Trim() ?? string.Empty;
                    string driverName = Path.GetFileName(Convert.ToString(queryDriver["PathName"])?.Trim()) ?? string.Empty;
                    string displayName = Convert.ToString(queryDriver["DisplayName"])?.Trim() ?? string.Empty;
                    // Mapping process
                    string driverType = Convert.ToString(queryDriver["ServiceType"])?.Trim();
                    string driverStartMode = Convert.ToString(queryDriver["StartMode"])?.Trim();
                    string driverStatus = Convert.ToString(queryDriver["State"])?.Trim();
                    //
                    driverType = driverTypeMappings.TryGetValue(driverType ?? "", out var mappedType) ? TS_String_Encoder(software_lang.TSReadLangs("Osd_Content", mappedType)) : TS_String_Encoder(software_lang.TSReadLangs("Osd_Content", "osd_c_unknown"));
                    driverStartMode = driverStartModeMappings.TryGetValue(driverStartMode ?? "", out var mappedStart) ? TS_String_Encoder(software_lang.TSReadLangs("Osd_Content", mappedStart)) : TS_String_Encoder(software_lang.TSReadLangs("Osd_Content", "osd_c_unknown"));
                    driverStatus = driverStatusMappings.TryGetValue(driverStatus ?? "", out var mappedStatus) ? TS_String_Encoder(software_lang.TSReadLangs("Osd_Content", mappedStatus)) : TS_String_Encoder(software_lang.TSReadLangs("Osd_Content", "osd_c_unknown"));
                    //
                    string[] driverInfos = { driverPath, driverName, displayName, driverType, driverStartMode, driverStatus };
                    OSD_DataMainTable.Rows.Add(driverInfos);
                }
                string unknownValue = TS_String_Encoder(software_lang.TSReadLangs("Osd_Content", "osd_c_unknown"));
                foreach (DataGridViewRow row in OSD_DataMainTable.Rows){
                    foreach (DataGridViewCell cell in row.Cells){
                        if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString())){
                            cell.Value = unknownValue;
                        }
                    }
                }
                //
                OSD_DataMainTable.Sort(OSD_DataMainTable.Columns[1], ListSortDirection.Ascending);
                OSD_DataMainTable.ClearSelection();
                OSD_TYSS_V.Text = OSD_DataMainTable.Rows.Count.ToString();
            }catch (ManagementException){ }
            finally{
                OSD_RotateBtn.Enabled = true;
                ((Control)OSD).Enabled = true;
                //
                OSD_DataMainTable.Width++;
                OSD_DataMainTable.Width--;
                //
                if (Program.debug_mode){ Console.WriteLine("<--- Installed Drivers Section Loaded --->"); }
            }
        }
        private void OSD_TextBox_TextChanged(object sender, EventArgs e){
            string searchText = OSD_TextBox.Text.Trim().ToLower();
            bool isTextBoxEmpty = string.IsNullOrEmpty(searchText);
            OSD_DataMainTable.ClearSelection();
            OSD_TextBoxClearBtn.Enabled = !isTextBoxEmpty;
            if (!isTextBoxEmpty){
                try{
                    foreach (DataGridViewRow driver_row in OSD_DataMainTable.Rows){
                        if (driver_row.Cells[1].Value.ToString().ToLower().Contains(searchText)){
                            driver_row.Selected = true;
                            OSD_DataMainTable.FirstDisplayedScrollingRowIndex = driver_row.Index;
                            break;
                        }
                    }
                }catch (Exception){ }
            }
        }
        private void OSD_DataMainTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            try{
                if (OSD_DataMainTable.SelectedRows.Count > 0){
                    Clipboard.SetText(string.Format("{0} | {1} | {2} | {3} | {4} | {5}", OSD_DataMainTable.Rows[e.RowIndex].Cells[0].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[1].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[2].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[3].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[4].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[5].Value.ToString()));
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Osd_Content", "osd_c_copy_success")), OSD_DataMainTable.Rows[e.RowIndex].Cells[0].Value));
                }
            }catch (Exception){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Osd_Content", "osd_c_copy_error")), OSD_DataMainTable.Rows[e.RowIndex].Cells[0].Value));
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
        private async Task gs_services(){
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
                var search_services = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Service");
                var get_services_result = await Task.Run(() => search_services.Get().Cast<ManagementObject>().ToList(), Program.TS_TokenEngine.Token);
                foreach (var queryService in get_services_result){
                    string processPath = Convert.ToString(queryService["PathName"])?.Replace("\"", string.Empty).Replace("\\??\\", string.Empty).Trim() ?? string.Empty;
                    string serviceName = Path.GetFileName(Convert.ToString(queryService["Name"])?.Trim()) ?? string.Empty;
                    string displayName = Convert.ToString(queryService["DisplayName"])?.Trim() ?? string.Empty;
                    //
                    string serviceType = Convert.ToString(queryService["ServiceType"])?.Trim();
                    string startMode = Convert.ToString(queryService["StartMode"])?.Trim();
                    string state = Convert.ToString(queryService["State"])?.Trim();
                    // Mapping Process
                    serviceType = serviceTypeMappings.TryGetValue(serviceType ?? "", out var mappedType) ? TS_String_Encoder(software_lang.TSReadLangs("Services_Content", mappedType)) : TS_String_Encoder(software_lang.TSReadLangs("Services_Content", "ss_c_unknown"));
                    startMode = startModeMappings.TryGetValue(startMode ?? "", out var mappedStart) ? TS_String_Encoder(software_lang.TSReadLangs("Services_Content", mappedStart)) : TS_String_Encoder(software_lang.TSReadLangs("Services_Content", "ss_c_unknown"));
                    state = stateMappings.TryGetValue(state ?? "", out var mappedState) ? TS_String_Encoder(software_lang.TSReadLangs("Services_Content", mappedState)) : TS_String_Encoder(software_lang.TSReadLangs("Services_Content", "ss_c_unknown"));
                    //
                    string[] serviceInfo = { processPath, serviceName, displayName, serviceType, startMode, state };
                    SERVICE_DataMainTable.Rows.Add(serviceInfo);
                }
                string unknownValue = TS_String_Encoder(software_lang.TSReadLangs("Services_Content", "ss_c_unknown"));
                foreach (DataGridViewRow row in SERVICE_DataMainTable.Rows){
                    foreach (DataGridViewCell cell in row.Cells){
                        if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString())){
                            cell.Value = unknownValue;
                        }
                    }
                }
                //
                SERVICE_DataMainTable.Sort(SERVICE_DataMainTable.Columns[1], ListSortDirection.Ascending);
                SERVICE_DataMainTable.ClearSelection();
                SERVICE_TYS_V.Text = SERVICE_DataMainTable.Rows.Count.ToString();
            }catch (ManagementException){ }
            finally{
                SERVICES_RotateBtn.Enabled = true;
                ((Control)GSERVICE).Enabled = true;
                //
                SERVICE_DataMainTable.Width++;
                SERVICE_DataMainTable.Width--;
                //
                if (Program.debug_mode){ Console.WriteLine("<--- Installed Services Section Installed --->"); }
            }
        }
        private void Services_SearchTextBox_TextChanged(object sender, EventArgs e){
            string searchText = SERVICE_TextBox.Text.Trim().ToLower();
            bool isTextBoxEmpty = string.IsNullOrEmpty(searchText);
            SERVICE_DataMainTable.ClearSelection();
            SERVICE_TextBoxClearBtn.Enabled = !isTextBoxEmpty;
            if (!isTextBoxEmpty){
                try{
                    foreach (DataGridViewRow service_row in SERVICE_DataMainTable.Rows){
                        if (service_row.Cells[1].Value.ToString().ToLower().Contains(searchText)){
                            service_row.Selected = true;
                            SERVICE_DataMainTable.FirstDisplayedScrollingRowIndex = service_row.Index;
                            break;
                        }
                    }
                }catch (Exception){ }
            }
        }
        private void SERVICE_DataMainTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            try{
                if (SERVICE_DataMainTable.SelectedRows.Count > 0){
                    Clipboard.SetText(string.Format("{0} | {1} | {2} | {3} | {4} | {5}", SERVICE_DataMainTable.Rows[e.RowIndex].Cells[0].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[1].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[2].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[3].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[4].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[5].Value.ToString()));
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Services_Content", "ss_c_copy_success")), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[0].Value));
                }
            }catch (Exception){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Services_Content", "ss_c_copy_error")), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[0].Value));
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
        // BUTTONS ROTATE
        // ======================================================================================================
        private void active_page(object btn_target){
            Button active_btn = null;
            disabled_page();
            if (btn_target != null){
                if (active_btn != (Button)btn_target){
                    active_btn = (Button)btn_target;
                    active_btn.BackColor = TS_ThemeEngine.ColorMode(theme, "BtnActiveColor");
                    active_btn.Cursor = Cursors.Default;
                }
            }
        }
        private void disabled_page(){
            foreach (Control disabled_btn in LeftMenuPanel.Controls){
                disabled_btn.BackColor = TS_ThemeEngine.ColorMode(theme, "BtnDeActiveColor");
                disabled_btn.Cursor = Cursors.Hand;
            }
        }
        // GLOW LEFT MENU SENDERS
        private void OSRotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(1, sender);
        }
        private void MBRotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(2, sender);
        }
        private void CPURotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(3, sender);
        }
        private void RAMRotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(4, sender);
        }
        private void GPURotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(5, sender);
        }
        private void DISKRotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(6, sender);
        }
        private void NETWORKRotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(7, sender);
        }
        private void USB_RotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(8, sender);
        }
        private void SOUND_RotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(9, sender);
        }
        private void PILRotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(10, sender);
        }
        private void OSDRotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(11, sender);
        }
        private void ServicesRotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(12, sender);
        }
        private void PRINT_RotateBtn_Click(object sender, EventArgs e){
            left_menu_preloader(13, sender);
        }
        // GLOW DYNAMIC ARROW KEYS ROTATE
        private void MainContent_Selecting(object sender, TabControlCancelEventArgs e){
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
                    { 12, PRINT_RotateBtn }
                };
                if (!e.TabPage.Enabled){
                    e.Cancel = true;
                }else if (tabButtons.TryGetValue(MainContent.SelectedIndex, out var button)){
                    button.PerformClick();
                }
            }catch (Exception){ }
        }
        // GLOW DYNAMIC LEFT MENU PRELOADER
        private void left_menu_preloader(int target_menu, object sender){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                var menuTargets = new Dictionary<int, (TabPage tab, Button rotateBtn, string headerKey)>{
                    { 1, (OS, OS_RotateBtn, "header_os") },
                    { 2, (MB, MB_RotateBtn, "header_mb") },
                    { 3, (CPU, CPU_RotateBtn, "header_cpu") },
                    { 4, (RAM, RAM_RotateBtn, "header_ram") },
                    { 5, (GPU, GPU_RotateBtn, "header_gpu") },
                    { 6, (DISK, DISK_RotateBtn, "header_storage") },
                    { 7, (NETWORK, NET_RotateBtn, "header_network") },
                    { 8, (USB, USB_RotateBtn, "header_usb") },
                    { 9, (SOUND, SOUND_RotateBtn, "header_sound") },
                    { 10, (BATTERY, BATTERY_RotateBtn, "header_battery") },
                    { 11, (OSD, OSD_RotateBtn, "header_installed_drivers") },
                    { 12, (GSERVICE, SERVICES_RotateBtn, "header_installed_services") },
                    { 13, (PRINT, PRINT_RotateBtn, "header_export") }
                };
                if (menu_btns != target_menu && menuTargets.TryGetValue(target_menu, out var target)){
                    MainContent.SelectedTab = target.tab;
                    if (!btn_colors_active.Contains(target.rotateBtn.BackColor)){
                        active_page(sender);
                    }
                    HeaderText.Text = TS_String_Encoder(software_lang.TSReadLangs("Header", target.headerKey));
                }
                //
                menu_btns = target_menu;
                menu_rp = target_menu;
                header_image_reloader(menu_btns);
            }catch (Exception){ }
        }
        // LANGUAGES SETTINGS
        // ======================================================================================================
        private void select_lang_active(object target_lang){
            ToolStripMenuItem selected_lang = null;
            select_lang_deactive();
            if (target_lang != null){
                if (selected_lang != (ToolStripMenuItem)target_lang){
                    selected_lang = (ToolStripMenuItem)target_lang;
                    selected_lang.Checked = true;
                }
            }
        }
        private void select_lang_deactive(){
            foreach (ToolStripMenuItem disabled_lang in languageToolStripMenuItem.DropDownItems){
                disabled_lang.Checked = false;
            }
        }
        // LANG SWAP
        // ======================================================================================================
        private void chineseToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "zh"){ lang_preload(ts_lang_zh, "zh"); select_lang_active(sender); }
        }
        private void englishToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "en"){ lang_preload(ts_lang_en, "en"); select_lang_active(sender); }
        }
        private void frenchToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "fr"){ lang_preload(ts_lang_fr, "fr"); select_lang_active(sender); }
        }
        private void germanToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "de"){ lang_preload(ts_lang_de, "de"); select_lang_active(sender); }
        }
        private void italianToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "it"){ lang_preload(ts_lang_it, "it"); select_lang_active(sender); }
        }
        private void koreanToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "ko"){ lang_preload(ts_lang_ko, "ko"); select_lang_active(sender); }
        }
        private void portugueseToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "pt"){ lang_preload(ts_lang_pt, "pt"); select_lang_active(sender); }
        }
        private void russianToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "ru"){ lang_preload(ts_lang_ru, "ru"); select_lang_active(sender); }
        }
        private void spanishToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "es"){ lang_preload(ts_lang_es, "es"); select_lang_active(sender); }
        }
        private void turkishToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "tr"){ lang_preload(ts_lang_tr, "tr"); select_lang_active(sender); }
        }
        private void lang_preload(string lang_type, string lang_code){
            lang_engine(lang_type, lang_code);
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "LanguageStatus", lang_code);
            }catch (Exception){ }
            // LANG CHANGE NOTIFICATION
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            DialogResult lang_change_message = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(TS_String_Encoder(software_lang.TSReadLangs("LangChange", "lang_change_notification")), "\n\n", "\n\n"));
            if (lang_change_message == DialogResult.Yes){ Application.Restart(); }
        }
        private void lang_engine(string lang_type, string lang_code){
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
                    { 13, "header_export" }
                };
                if (headers.TryGetValue(menu_rp, out string headerKey)){
                    HeaderText.Text = TS_String_Encoder(software_lang.TSReadLangs("Header", headerKey));
                }
                // SETTINGS
                settingsToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_settings"));
                // THEMES
                themeToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_theme"));
                lightThemeToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderThemes", "theme_light"));
                darkThemeToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderThemes", "theme_dark"));
                // LANGS
                languageToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_language"));
                chineseToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_zh"));
                englishToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_en"));
                frenchToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_fr"));
                germanToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_de"));
                italianToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_it"));
                koreanToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_ko"));
                portugueseToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_pt"));
                russianToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_ru"));
                spanishToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_es"));
                turkishToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderLangs", "lang_tr"));
                // INITIAL VIEW
                initialViewToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_start"));
                windowedToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderViewMode", "header_viev_mode_windowed"));
                fullScreenToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderViewMode", "header_viev_mode_full_screen"));
                // HIDING MODE
                hidingModeToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_privacy_mode"));
                hidingModeOnToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_on"));
                hidingModeOffToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderHidingMode", "header_hiding_mode_off"));
                // UPDATE CHECK
                checkforUpdatesToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_update"));
                // TOOLS
                toolsToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_tools"));
                sFCandDISMAutoToolToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_sfc_and_dism_tool"));
                cacheCleaningToolToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_cache_cleanup_tool"));
                benchCPUToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_bench_cpu"));
                benchRAMToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_bench_ram"));
                benchDiskToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_bench_disk"));
                screenOverlayToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_overlay"));
                dNSTestToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_dns_test_tool"));
                quickAccessToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_quick_access_tool"));
                networkFixToolToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_network_fix_tool"));
                showWiFiPasswordToolToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_show_wifi_password_tool"));
                monitorTestToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_monitor_test"));
                monitorDeadPixelTestToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_monitor_test_dead_pixel"));
                monitorDynamicRangeTestToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_monitor_test_dynamic_range"));
                // ABOUT
                aboutToolStripMenuItem.Text = TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_about"));
                // MENU
                OS_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_os"));
                MB_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_mb"));
                CPU_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_cpu"));
                RAM_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_ram"));
                GPU_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_gpu"));
                DISK_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_storage"));
                NET_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_network"));
                USB_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_usb"));
                SOUND_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_sound"));
                BATTERY_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_battery"));
                OSD_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_installed_drivers"));
                SERVICES_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_installed_services"));
                PRINT_RotateBtn.Text = " " + " " + TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_export"));
                // OS
                OS_SystemUser.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_user"));
                OS_ComputerName.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_computer_name"));
                OS_SavedUser.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_saved_user_account"));
                OS_Name.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_operating_system"));
                OS_Manufacturer.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_operating_system_publisher"));
                OS_SystemVersion.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_system_version"));
                OS_Build.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_build"));
                OS_SystemBuild.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_os_debug"));
                OS_SystemArchitectural.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_system_architecture"));
                OS_Family.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_family"));
                OS_DeviceID.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_device_id"));
                OS_Serial.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_product_id"));
                OS_Country.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_adjustable_language"));
                OS_TimeZone.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_timezone"));
                OS_CharacterSet.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_character_set"));
                OS_EncryptionType.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_encrypt_type"));
                OS_SystemRootIndex.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_root_directory"));
                OS_SystemBuildPart.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_build_partition"));
                OS_SystemTime.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_time"));
                OS_Install.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_install_date"));
                OS_SystemWorkTime.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_system_work_time"));
                OS_LastBootTime.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_last_boot_time"));
                OS_SystemLastShutDown.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_last_shutdown_time"));
                OS_PortableOS.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_portable_os"));
                OS_FastBoot.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_fastboot_status"));
                OS_MouseWheelStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_mouse_scroll_speed"));
                OS_ScrollLockStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_scroll_lock_status"));
                OS_NumLockStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_numpad_lock_status"));
                OS_CapsLockStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_caps_lock_status"));
                OS_BootPartition.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_boot_partition"));
                OS_SystemPartition.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_system_partition"));
                OS_WinPageFile.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_win_pagefile"));
                OS_TempWinPageFile.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_win_temp_pagefile"));
                OS_Hiberfil.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_win_hiberfil"));
                OS_AVProgram.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_installed_anti_virus_apps"));
                OS_FirewallProgram.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_installed_firewall_apps"));
                OS_AntiSpywareProgram.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_installed_anti_spyware_apps"));
                OS_WinDefCoreIsolation.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_win_core_isolation_status"));
                OS_MSEdge.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_msedge_version"));
                OS_MSEdgeWebView.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_msedge_webview2_version"));
                OS_MSStoreVersion.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_ms_store_version"));
                OS_WinKey.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_win_license_key"));
                OS_WinActiveChannel.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_win_activation_channel"));
                OS_NETFrameworkVersion.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_net_framework_version"));
                OS_Minidump.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_detect_minidump_count"));
                OS_BSODDate.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_last_bsod_time"));
                OS_Wallpaper.Text = TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_wallpaper"));
                MainToolTip.SetToolTip(OS_WallpaperOpen, string.Format(TS_String_Encoder(software_lang.TSReadLangs("OperatingSystem", "os_open_wallpaper")), wp_rotate));
                // MB
                MB_BIOSUpdateBtn.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_b_update_btn"));
                MB_MotherBoardName.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_model"));
                MB_MotherBoardMan.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_manufacturer"));
                MB_SystemModelMan.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_system_model_manufacturer"));
                MB_SystemModelFamily.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_system_model_family"));
                MB_SystemModel.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_system_model"));
                MB_MotherBoardSerial.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_serial"));
                MB_SystemFamily.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_system_family"));
                MB_SystemSKU.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_system_sku"));
                MB_Chipset.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_chipset"));
                MB_BiosManufacturer.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_bios_publisher"));
                MB_BiosDate.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_bios_date"));
                MB_BiosVersion.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_bios_version"));
                MB_SmBiosVersion.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_sm_bios_version"));
                MB_BiosMode.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_bios_mode"));
                MB_LastBIOSTime.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_last_bios_time"));
                MB_SecureBoot.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_secure_boot_status"));
                MB_TPMStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_tpm_status"));
                MB_TPMPhysicalVersion.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_tpm_physical_presence_version"));
                MB_TPMMan.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_tpm_manufacturer"));
                MB_TPMManID.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_tpm_manufacturer_id"));
                MB_TPMManVersion.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_tpm_manufacturer_version"));
                MB_TPMManFullVersion.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_tpm_manufacturer_full_version"));
                MB_TPMManPublisher.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_tpm_publisher"));
                MB_Model.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_model_version"));
                MB_PrimaryBusType.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_primary_bus_type"));
                MB_SecondaryBusType.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_secondary_bus_type"));
                MB_BiosMajorMinor.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_bios_major_minor_version"));
                MB_SMBiosMajorMinor.Text = TS_String_Encoder(software_lang.TSReadLangs("Motherboard", "mb_sm_bios_major_minor_version"));
                // CPU
                CPU_Name.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_model"));
                CPU_Manufacturer.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_manufacturer"));
                CPU_Architectural.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_architecture"));
                CPU_IntelME.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_intel_me"));
                CPU_NormalSpeed.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_speed"));
                CPU_DefaultSpeed.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_default_cpu_speed"));
                CPU_L1.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_l1_cache_size"));
                CPU_L2.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_l2_cache_size"));
                CPU_L3.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_l3_cache_size"));
                CPU_CoreCount.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_core_count"));
                CPU_LogicalCore.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_logical_core_count"));
                CPU_Usage.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_usage"));
                CPU_Process.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_process_count"));
                CPU_SocketDefinition.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_socket_definition"));
                CPU_Family.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_family"));
                CPU_MicroCode.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_microcode"));
                CPU_PlatformFeature.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_platformfeature"));
                CPU_FeatureSet.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_featureset"));
                CPU_Revision.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_revision"));
                CPU_OldRevision.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_old_revision"));
                CPU_Virtualization.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_cpu_virtualization"));
                CPU_SerialName.Text = TS_String_Encoder(software_lang.TSReadLangs("Processor", "pr_unique_processor_id"));
                // RAM
                RAM_TotalRAM.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_total_ram_amount"));
                RAM_UsageRAMCount.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_usage_ram_amount"));
                RAM_EmptyRamCount.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_empty_ram_amount"));
                RAM_TotalVirtualRam.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_total_virtual_ram_amount"));
                RAM_UsageVirtualRam.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_usage_virtual_ram_amount"));
                RAM_EmptyVirtualRam.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_empty_virtual_ram_amount"));
                RAM_SlotStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_ram_slot_fullness"));
                RAM_SlotSelectLabel.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_ram_slot"));
                RAM_Amount.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_ram_amount"));
                RAM_Type.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_ram_type"));
                RAM_Frequency.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_ram_frequency"));
                RAM_Volt.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_ram_voltage"));
                RAM_FormFactor.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_ram_form_factor"));
                RAM_Serial.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_ram_serial"));
                RAM_Manufacturer.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_ram_manufacturer"));
                RAM_BankLabel.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_ram_location"));
                RAM_DataWidth.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_ram_width"));
                RAM_BellekType.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_partition_type"));
                RAM_PartNumber.Text = TS_String_Encoder(software_lang.TSReadLangs("Memory", "my_partition_number"));
                // GPU
                GPU_Name.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_model"));
                GPU_Manufacturer.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_publisher"));
                GPU_Version.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_driver_version"));
                GPU_DriverDate.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_driver_date"));
                GPU_Status.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_status"));
                GPU_DeviceID.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_hardware_id"));
                GPU_DacType.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_dac_type"));
                GPU_GraphicDriversName.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_graphic_drivers"));
                GPU_InfFileName.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_inf_file"));
                GPU_INFSectionFile.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_inf_file_gpu_partition"));
                GPU_CurrentColor.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_current_color"));
                GPU_MonitorSelect.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_monitors"));
                GPU_MonitorBounds.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_monitor_limit"));
                GPU_MonitorWorking.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_monitor_work_area_size"));
                GPU_MonitorResLabel.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_monitor_resolution"));
                GPU_MonitorVirtualRes.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_virtual_resolution"));
                GPU_ScreenRefreshRate.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_monitor_refresh_rate"));
                GPU_ScreenBit.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_monitor_bit_rate"));
                GPU_MonitorPrimary.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_primary_screen"));
                GPU_DesktopResolution.Text = TS_String_Encoder(software_lang.TSReadLangs("Gpu", "gpu_desktop_resolution"));
                // DISK
                DISK_TTLP_L1.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_t_title"));
                DISK_TTLP_P1_L1.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_t_ssd"));
                DISK_TTLP_P2_L1.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_t_hdd"));
                DISK_TTLP_P3_L1.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_t_usb"));
                DISK_TTLP_P4_L1.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_t_total"));
                DISK_Caption.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_name"));
                DISK_Model.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_model"));
                DISK_Man.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_manufacturer"));
                DISK_VolumeID.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_partition_letter"));
                DISK_VolumeName.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_partition_name"));
                DISK_PhysicalName.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_physical_name"));
                DISK_Firmware.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_firmware_version"));
                DISK_Serial.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_serial"));
                DISK_VolumeSerial.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_partition_serial"));
                DISK_Size.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_size"));
                DISK_FreeSpace.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_empty_space"));
                DISK_FileSystem.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_file_system"));
                DISK_FormattingType.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_formatting_type"));
                DISK_Type.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_type"));
                DISK_DriveType.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_drive_type"));
                DISK_InterFace.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_interface_type"));
                DISK_PartitionCount.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_partition_count"));
                DISK_MediaLoaded.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_work_status"));
                DISK_MediaStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_status"));
                DISK_Health.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_health"));
                DISK_Boot.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_primary_disk"));
                DISK_Bootable.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_bootable_disk"));
                DISK_BitLockerStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_bitlocker_status"));
                DISK_BitLockerConversionStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_bitlocker_encrypt_status"));
                DISK_BitLockerEncryptMehod.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_bitlocker_encrypt_method"));
                DISK_DriveCompressed.Text = TS_String_Encoder(software_lang.TSReadLangs("Storage", "se_compress_status"));
                // NETWORK
                NET_LT_Device.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_live_speed_adapter"));
                NET_LT_BandWidth.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_live_speed_band_width"));
                NET_LT_DL1.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_live_speed_download"));
                NET_LT_UL1.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_live_speed_upload"));
                // 
                NET_ConnType.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_network_device"));
                NET_MacAdress.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_mac_adress"));
                NET_NetMan.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_driver_provider"));
                NET_ServiceName.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_service_name"));
                NET_AdapterType.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_adaptor_type"));
                NET_Physical.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_physical_adaptor"));
                NET_DeviceID.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_network_hardware_id"));
                NET_Guid.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_guid"));
                NET_ConnectionType.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_connection_type"));
                NET_Dhcp_status.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_dhcp_status"));
                NET_Dhcp_server.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_dhcp_server"));
                NET_LocalConSpeed.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_connection_speed"));
                NET_IPv4Adress.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_appointed_ipv4_adress"));
                NET_IPv6Adress.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_appointed_ipv6_adress"));
                NET_DNS1.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_dns1"));
                NET_DNS2.Text = TS_String_Encoder(software_lang.TSReadLangs("Network", "nk_dns2"));
                // USB
                USB_Con.Text = TS_String_Encoder(software_lang.TSReadLangs("Usb", "usb_controller"));
                USB_ConName.Text = TS_String_Encoder(software_lang.TSReadLangs("Usb", "usb_controller_name"));
                USB_ConMan.Text = TS_String_Encoder(software_lang.TSReadLangs("Usb", "usb_controller_publisher"));
                USB_ConDeviceID.Text = TS_String_Encoder(software_lang.TSReadLangs("Usb", "usb_controller_hardware_id"));
                USB_ConPNPDeviceID.Text = TS_String_Encoder(software_lang.TSReadLangs("Usb", "usb_controller_pnp_hardware_id"));
                USB_ConDeviceStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("Usb", "usb_controller_status"));
                USB_Device.Text = TS_String_Encoder(software_lang.TSReadLangs("Usb", "usb_device"));
                USB_DeviceName.Text = TS_String_Encoder(software_lang.TSReadLangs("Usb", "usb_device_name"));
                USB_DeviceID.Text = TS_String_Encoder(software_lang.TSReadLangs("Usb", "usb_device_hardware_id"));
                USB_PNPDeviceID.Text = TS_String_Encoder(software_lang.TSReadLangs("Usb", "usb_device_pnp_hardware_id"));
                USB_DeviceStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("Usb", "usb_device_status"));
                // SOUND
                SOUND_Device.Text = TS_String_Encoder(software_lang.TSReadLangs("Sound", "sound_device"));
                SOUND_DeviceName.Text = TS_String_Encoder(software_lang.TSReadLangs("Sound", "sound_device_name"));
                SOUND_DeviceManufacturer.Text = TS_String_Encoder(software_lang.TSReadLangs("Sound", "sound_device_publisher"));
                SOUND_DeviceID.Text = TS_String_Encoder(software_lang.TSReadLangs("Sound", "sound_device_hardware_id"));
                SOUND_PNPDeviceID.Text = TS_String_Encoder(software_lang.TSReadLangs("Sound", "sound_device_pnp_hardware_id"));
                SOUND_DeviceStatus.Text = TS_String_Encoder(software_lang.TSReadLangs("Sound", "sound_device_status"));
                // BATTERY
                BATTERY_Status.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery", "by_status"));
                BATTERY_Model.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery", "by_model"));
                BATTERY_Name.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery", "by_name"));
                BATTERY_Voltage.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery", "by_voltage"));
                BATTERY_Type.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery", "by_structure"));
                BATTERY_ReportBtn.Text = TS_String_Encoder(software_lang.TSReadLangs("Battery", "by_report_create"));
                TS_AdjustButtonWidth(BATTERY_ReportBtn); // Dynamic Width
                // OSD
                OSD_DataMainTable.Columns[0].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Osd", "osd_file_path"));
                OSD_DataMainTable.Columns[1].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Osd", "osd_file_name"));
                OSD_DataMainTable.Columns[2].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Osd", "osd_driver_name"));
                OSD_DataMainTable.Columns[3].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Osd", "osd_driver_type"));
                OSD_DataMainTable.Columns[4].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Osd", "osd_start"));
                OSD_DataMainTable.Columns[5].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Osd", "osd_status"));
                OSD_SearchDriverLabel.Text = TS_String_Encoder(software_lang.TSReadLangs("Osd", "osd_search_driver"));
                OSD_TYSS.Text = TS_String_Encoder(software_lang.TSReadLangs("Osd", "osd_installed_driver_count"));
                OSD_SortMode.Text = TS_String_Encoder(software_lang.TSReadLangs("Osd", "osd_order_in_reverse"));
                // SERVICES
                SERVICE_DataMainTable.Columns[0].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Services", "ss_file_path"));
                SERVICE_DataMainTable.Columns[1].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Services", "ss_file_name"));
                SERVICE_DataMainTable.Columns[2].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Services", "ss_service_name"));
                SERVICE_DataMainTable.Columns[3].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Services", "ss_service_type"));
                SERVICE_DataMainTable.Columns[4].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Services", "ss_start"));
                SERVICE_DataMainTable.Columns[5].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("Services", "ss_status"));
                SERVICE_SearchLabel.Text = TS_String_Encoder(software_lang.TSReadLangs("Services", "ss_search_service"));
                SERVICE_TYS.Text = TS_String_Encoder(software_lang.TSReadLangs("Services", "ss_installed_service_count"));
                SERVICE_SortMode.Text = TS_String_Encoder(software_lang.TSReadLangs("Services", "ss_order_in_reverse"));
                // EXPORT
                EXPORT_HeaderLabel.Text = TS_String_Encoder(software_lang.TSReadLangs("Export", "e_mode_title"));
                EXPORT_StartEngineBtn.Text = TS_String_Encoder(software_lang.TSReadLangs("Export", "e_mode_report_start"));
                // OTHER PAGE DYNAMIC UI
                glow_other_page_dynamic_ui();
            }catch (Exception){ }
        }
        // THEME SETTINGS
        // ======================================================================================================
        private void select_theme_active(object target_theme){
            ToolStripMenuItem selected_theme = null;
            select_theme_deactive();
            if (target_theme != null){
                if (selected_theme != (ToolStripMenuItem)target_theme){
                    selected_theme = (ToolStripMenuItem)target_theme;
                    selected_theme.Checked = true;
                }
            }
        }
        private void select_theme_deactive(){
            foreach (ToolStripMenuItem disabled_theme in themeToolStripMenuItem.DropDownItems){
                disabled_theme.Checked = false;
            }
        }
        // THEME SWAP
        // ======================================================================================================
        private void lightThemeToolStripMenuItem_Click(object sender, EventArgs e){
            if (theme != 1) { theme_engine(1); select_theme_active(sender); }
        }
        private void darkThemeToolStripMenuItem_Click(object sender, EventArgs e){
            if (theme != 0) { theme_engine(0); select_theme_active(sender); }
        }
        // THEME ENGINE
        // ======================================================================================================
        private void theme_engine(int ts){
            try{
                theme = ts;
                //
                int set_attribute = theme == 1 ? 20 : 19;
                if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != theme){
                    DwmSetWindowAttribute(Handle, 20, new[] { theme == 1 ? 0 : 1 }, 4);
                }
                //
                if (theme == 1){
                    // LEFT MENU LOGO CHANGE
                    if (windows_mode == 1){
                        OS_RotateBtn.Image = Properties.Resources.left_os_w11_light;
                    }else{
                        OS_RotateBtn.Image = Properties.Resources.left_os_w10_light;
                    }
                    MB_RotateBtn.Image = Properties.Resources.lm_mb_light;
                    CPU_RotateBtn.Image = Properties.Resources.lm_cpu_light;
                    RAM_RotateBtn.Image = Properties.Resources.lm_ram_light;
                    GPU_RotateBtn.Image = Properties.Resources.lm_gpu_light;
                    DISK_RotateBtn.Image = Properties.Resources.lm_disk_light;
                    NET_RotateBtn.Image = Properties.Resources.lm_net_light;
                    USB_RotateBtn.Image = Properties.Resources.lm_usb_light;
                    SOUND_RotateBtn.Image = Properties.Resources.lm_sound_light;
                    BATTERY_RotateBtn.Image = Properties.Resources.lm_battery_light;
                    OSD_RotateBtn.Image = Properties.Resources.lm_drivers_light;
                    SERVICES_RotateBtn.Image = Properties.Resources.lm_services_light;
                    PRINT_RotateBtn.Image = Properties.Resources.lm_export_light;
                    // TOP MENU LOGO CHANGE
                    settingsToolStripMenuItem.Image = Properties.Resources.tm_settings_light;
                    themeToolStripMenuItem.Image = Properties.Resources.tm_theme_light;
                    languageToolStripMenuItem.Image = Properties.Resources.tm_lang_light;
                    initialViewToolStripMenuItem.Image = Properties.Resources.tm_start_light;
                    hidingModeToolStripMenuItem.Image = Properties.Resources.tm_hidden_light;
                    checkforUpdatesToolStripMenuItem.Image = Properties.Resources.tm_update_light;
                    toolsToolStripMenuItem.Image = Properties.Resources.tm_tools_light;
                    sFCandDISMAutoToolToolStripMenuItem.Image = Properties.Resources.tm_sfc_and_dism_light;
                    cacheCleaningToolToolStripMenuItem.Image = Properties.Resources.tm_cache_clean_light;
                    benchCPUToolStripMenuItem.Image = Properties.Resources.tm_bench_cpu_light;
                    benchRAMToolStripMenuItem.Image = Properties.Resources.tm_bench_ram_light;
                    benchDiskToolStripMenuItem.Image = Properties.Resources.tm_bench_disk_light;
                    screenOverlayToolStripMenuItem.Image = Properties.Resources.tm_overlay_light;
                    dNSTestToolStripMenuItem.Image = Properties.Resources.tm_dns_light;
                    quickAccessToolStripMenuItem.Image = Properties.Resources.tm_quick_access_light;
                    networkFixToolToolStripMenuItem.Image = Properties.Resources.tm_network_fix_light;
                    showWiFiPasswordToolToolStripMenuItem.Image = Properties.Resources.tm_swpt_light;
                    monitorTestToolStripMenuItem.Image = Properties.Resources.tm_test_monitor_light;
                    monitorDeadPixelTestToolStripMenuItem.Image = Properties.Resources.tm_test_dead_pixel_light;
                    monitorDynamicRangeTestToolStripMenuItem.Image = Properties.Resources.tm_test_dynamic_range_light;
                    // MIDDLE CONTENT LOGO CHANGE
                    OS_WinKeyCopy.BackgroundImage = Properties.Resources.mid_copy_light;
                    OS_MinidumpOpen.BackgroundImage = Properties.Resources.mid_link_light;
                    OS_BSoDZIP.BackgroundImage = Properties.Resources.mid_zip_light;
                    OS_WallpaperOpen.BackgroundImage = Properties.Resources.mid_link_light;
                    // HELP
                    aboutToolStripMenuItem.Image = Properties.Resources.tm_about_light;
                }else if (theme == 0){
                    // LEFT MENU LOGO CHANGE
                    if (windows_mode == 1){
                        OS_RotateBtn.Image = Properties.Resources.left_os_w11_dark;
                    }else{
                        OS_RotateBtn.Image = Properties.Resources.left_os_w10_dark;
                    }
                    MB_RotateBtn.Image = Properties.Resources.lm_mb_dark;
                    CPU_RotateBtn.Image = Properties.Resources.lm_cpu_dark;
                    RAM_RotateBtn.Image = Properties.Resources.lm_ram_dark;
                    GPU_RotateBtn.Image = Properties.Resources.lm_gpu_dark;
                    DISK_RotateBtn.Image = Properties.Resources.lm_disk_dark;
                    NET_RotateBtn.Image = Properties.Resources.lm_net_dark;
                    USB_RotateBtn.Image = Properties.Resources.lm_usb_dark;
                    SOUND_RotateBtn.Image = Properties.Resources.lm_sound_dark;
                    BATTERY_RotateBtn.Image = Properties.Resources.lm_battery_dark;
                    OSD_RotateBtn.Image = Properties.Resources.lm_drivers_dark;
                    SERVICES_RotateBtn.Image = Properties.Resources.lm_services_dark;
                    PRINT_RotateBtn.Image = Properties.Resources.lm_export_dark;
                    // TOP MENU LOGO CHANGE
                    settingsToolStripMenuItem.Image = Properties.Resources.tm_settings_dark;
                    themeToolStripMenuItem.Image = Properties.Resources.tm_theme_dark;
                    languageToolStripMenuItem.Image = Properties.Resources.tm_lang_dark;
                    initialViewToolStripMenuItem.Image = Properties.Resources.tm_start_dark;
                    hidingModeToolStripMenuItem.Image = Properties.Resources.tm_hidden_dark;
                    checkforUpdatesToolStripMenuItem.Image = Properties.Resources.tm_update_dark;
                    toolsToolStripMenuItem.Image = Properties.Resources.tm_tools_dark;
                    sFCandDISMAutoToolToolStripMenuItem.Image = Properties.Resources.tm_sfc_and_dism_dark;
                    cacheCleaningToolToolStripMenuItem.Image = Properties.Resources.tm_cache_clean_dark;
                    benchCPUToolStripMenuItem.Image = Properties.Resources.tm_bench_cpu_dark;
                    benchRAMToolStripMenuItem.Image = Properties.Resources.tm_bench_ram_dark;
                    benchDiskToolStripMenuItem.Image = Properties.Resources.tm_bench_disk_dark;
                    screenOverlayToolStripMenuItem.Image = Properties.Resources.tm_overlay_dark;
                    dNSTestToolStripMenuItem.Image = Properties.Resources.tm_dns_dark;
                    quickAccessToolStripMenuItem.Image = Properties.Resources.tm_quick_access_dark;
                    networkFixToolToolStripMenuItem.Image = Properties.Resources.tm_network_fix_dark;
                    showWiFiPasswordToolToolStripMenuItem.Image = Properties.Resources.tm_swpt_dark;
                    monitorTestToolStripMenuItem.Image = Properties.Resources.tm_test_monitor_dark;
                    monitorDeadPixelTestToolStripMenuItem.Image = Properties.Resources.tm_test_dead_pixel_dark;
                    monitorDynamicRangeTestToolStripMenuItem.Image = Properties.Resources.tm_test_dynamic_range_dark;
                    // MIDDLE CONTENT LOGO CHANGE
                    OS_WinKeyCopy.BackgroundImage = Properties.Resources.mid_copy_dark;
                    OS_MinidumpOpen.BackgroundImage = Properties.Resources.mid_link_dark;
                    OS_BSoDZIP.BackgroundImage = Properties.Resources.mid_zip_dark;
                    OS_WallpaperOpen.BackgroundImage = Properties.Resources.mid_link_dark;
                    // HELP
                    aboutToolStripMenuItem.Image = Properties.Resources.tm_about_dark;
                }
                // OTHER PAGE DYNAMIC UI
                glow_other_page_dynamic_ui();
                // HEADER
                header_image_reloader(menu_btns);
                header_colors[0] = TS_ThemeEngine.ColorMode(theme, "HeaderBGColorMain");
                header_colors[1] = TS_ThemeEngine.ColorMode(theme, "HeaderFEColorMain");
                HeaderMenu.Renderer = new HeaderMenuColors();
                // ACTIVE BTN 
                btn_colors_active[0] = TS_ThemeEngine.ColorMode(theme, "BtnActiveColor");
                // TOOLTIP
                MainToolTip.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                MainToolTip.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                // HEADER PANEL
                Header_InPanel.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                // HEADER PANEL TEXT
                HeaderText.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                // HEADER MENU
                HeaderMenu.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                HeaderMenu.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                // SETTINGS
                settingsToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                settingsToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                // THEMES
                themeToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                themeToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                lightThemeToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                lightThemeToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                darkThemeToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                darkThemeToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                // LANGS
                languageToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                languageToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                chineseToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                chineseToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                englishToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                englishToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                frenchToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                frenchToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                germanToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                germanToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                italianToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                italianToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                koreanToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                koreanToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                portugueseToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                portugueseToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                russianToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                russianToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                spanishToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                spanishToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                turkishToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                turkishToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                // INITIAL VIEW
                initialViewToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                initialViewToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                windowedToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                windowedToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                fullScreenToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                fullScreenToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                // HIDING MODE
                hidingModeToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                hidingModeToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                hidingModeOnToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                hidingModeOnToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                hidingModeOffToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                hidingModeOffToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                // UPDATE ENGINE
                checkforUpdatesToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                checkforUpdatesToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                // TOOLS
                toolsToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                toolsToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                sFCandDISMAutoToolToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                sFCandDISMAutoToolToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                cacheCleaningToolToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                cacheCleaningToolToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                benchCPUToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                benchCPUToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                benchRAMToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                benchRAMToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                benchDiskToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                benchDiskToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                screenOverlayToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                screenOverlayToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                dNSTestToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                dNSTestToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                quickAccessToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                quickAccessToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                networkFixToolToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                networkFixToolToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                showWiFiPasswordToolToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                showWiFiPasswordToolToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                monitorTestToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                monitorTestToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                monitorDeadPixelTestToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                monitorDeadPixelTestToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                monitorDynamicRangeTestToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                monitorDynamicRangeTestToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
                // ABOUT
                aboutToolStripMenuItem.BackColor = TS_ThemeEngine.ColorMode(theme, "HeaderBGColor");
                aboutToolStripMenuItem.ForeColor = TS_ThemeEngine.ColorMode(theme, "HeaderFEColor");
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
                OSD.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                GSERVICE.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                PRINT.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                // OS
                os_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_3.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_4.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_5.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_6.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                os_panel_7.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                OS_SystemUser.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemUser_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_ComputerName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_ComputerName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_SavedUser.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SavedUser_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_Name.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Name_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_Manufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Manufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_SystemVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_Build.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Build_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_SystemBuild.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemBuild_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_SystemArchitectural.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemArchitectural_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_Family.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Family_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_DeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_DeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_Serial.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Serial_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_Country.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Country_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_TimeZone.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_TimeZone_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_CharacterSet.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_CharacterSet_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_EncryptionType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_EncryptionType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_SystemRootIndex.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemRootIndex_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_SystemBuildPart.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemBuildPart_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_SystemTime.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemTime_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_Install.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Install_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_SystemWorkTime.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemWorkTime_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_LastBootTime.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_LastBootTime_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_SystemLastShutDown.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemLastShutDown_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_PortableOS.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_PortableOS_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_FastBoot.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_FastBoot_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_MouseWheelStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_MouseWheelStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_ScrollLockStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_ScrollLockStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_NumLockStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_NumLockStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_CapsLockStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_CapsLockStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_BootPartition.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_BootPartition_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_SystemPartition.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_SystemPartition_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_WinPageFile.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_WinPageFile_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_TempWinPageFile.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_TempWinPageFile_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_Hiberfil.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Hiberfil_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_AVProgram.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_AVProgram_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_FirewallProgram.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_FirewallProgram_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_AntiSpywareProgram.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_AntiSpywareProgram_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_WinDefCoreIsolation.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_WinDefCoreIsolation_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_MSEdge.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_MSEdge_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_MSEdgeWebView.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_MSEdgeWebView_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_MSStoreVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_MSStoreVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_WinKey.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_WinKey_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_WinActiveChannel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_WinActiveChannel_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_NETFrameworkVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_NETFrameworkVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_Minidump.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Minidump_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_BSODDate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_BSODDate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OS_Wallpaper.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OS_Wallpaper_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                // MB
                mb_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                mb_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                mb_panel_3.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                mb_panel_4.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                MB_BIOSUpdateBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "DynamicThemeActiveBtnBG");
                MB_BIOSUpdateBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_BIOSUpdateBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_BIOSUpdateBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_BIOSUpdateBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRightHover");
                MB_MotherBoardName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_MotherBoardName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_MotherBoardMan.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_MotherBoardMan_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_SystemModelMan.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SystemModelMan_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_SystemModelFamily.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SystemModelFamily_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_SystemModel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SystemModel_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_MotherBoardSerial.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_MotherBoardSerial_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_SystemFamily.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SystemFamily_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_SystemSKU.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SystemSKU_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_Chipset.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_Chipset_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_BiosManufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_BiosManufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_BiosDate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_BiosDate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_BiosVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_BiosVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_SmBiosVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SmBiosVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_BiosMode.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_BiosMode_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_LastBIOSTime.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_LastBIOSTime_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_SecureBoot.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SecureBoot_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_TPMStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_TPMPhysicalVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMPhysicalVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_TPMMan.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMMan_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_TPMManID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMManID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_TPMManVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMManVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_TPMManFullVersion.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMManFullVersion_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_TPMManPublisher.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_TPMManPublisher_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_Model.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_Model_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_PrimaryBusType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_PrimaryBusType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_SecondaryBusType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SecondaryBusType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_BiosMajorMinor.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_BiosMajorMinor_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                MB_SMBiosMajorMinor.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                MB_SMBiosMajorMinor_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                // CPU
                cpu_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                cpu_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                cpu_panel_3.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                CPU_Name.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Name_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_Manufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Manufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_Architectural.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Architectural_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_IntelME.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_IntelME_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_NormalSpeed.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_NormalSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_DefaultSpeed.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_DefaultSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_L1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_L1_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_L2.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_L2_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_L3.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_L3_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_CoreCount.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_CoreCount_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_LogicalCore.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_LogicalCore_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_Usage.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Usage_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_Process.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Process_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_SocketDefinition.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_SocketDefinition_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_Family.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Family_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_MicroCode.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_MicroCode_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_PlatformFeature.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_PlatformFeature_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_FeatureSet.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_FeatureSet_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_Revision.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Revision_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_OldRevision.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_OldRevision_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_Virtualization.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_Virtualization_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                CPU_SerialName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                CPU_SerialName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                // RAM
                ram_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                ram_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                RAM_TotalRAM.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_TotalRAM_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_UsageRAMCount.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_UsageRAMCount_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_EmptyRamCount.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_EmptyRamCount_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_TotalVirtualRam.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_TotalVirtualRam_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_UsageVirtualRam.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_UsageVirtualRam_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_EmptyVirtualRam.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_EmptyVirtualRam_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_SlotStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_SlotStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_SlotSelectLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_SelectList.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                RAM_SelectList.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_Amount.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Amount_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_Type.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Type_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_Frequency.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Frequency_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_Volt.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Volt_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_FormFactor.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_FormFactor_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_Serial.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Serial_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_Manufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_Manufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_BankLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_BankLabel_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_DataWidth.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_DataWidth_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_BellekType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_BellekType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_PartNumber.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                RAM_PartNumber_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_ProgressBGPanel.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                RAM_ProgressFEPanel.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                RAM_ProgressLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                // GPU
                gpu_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                gpu_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                gpu_panel_3.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                GPU_Name.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_Select.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                GPU_Select.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_Manufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_Manufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_Version.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_Version_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_DriverDate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_DriverDate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_Status.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_Status_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_DeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_DeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_DacType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_DacType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_GraphicDriversName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_GraphicDriversName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_InfFileName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_InfFileName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_INFSectionFile.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_INFSectionFile_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_CurrentColor.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_CurrentColor_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_MonitorSelectList.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                GPU_MonitorSelectList.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_MonitorSelect.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorBounds.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorBounds_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_MonitorWorking.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorWorking_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_MonitorResLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorResLabel_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_MonitorVirtualRes.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorVirtualRes_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_ScreenRefreshRate.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_ScreenRefreshRate_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_ScreenBit.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_ScreenBit_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_MonitorPrimary.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_MonitorPrimary_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                GPU_DesktopResolution.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                GPU_DesktopResolution_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
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
                DISK_TLP_PB_1.BackColor = TS_ThemeEngine.ColorMode(theme, "DiskTotalSSDBG");
                DISK_TLP_PB_2.BackColor = TS_ThemeEngine.ColorMode(theme, "DiskTotalHDDBG");
                DISK_TLP_PB_3.BackColor = TS_ThemeEngine.ColorMode(theme, "DiskTotalUSBBG");
                DISK_TLP_PB_4.BackColor = TS_ThemeEngine.ColorMode(theme, "DiskTotalTotalBG");
                DISK_TTLP_P1_L2.ForeColor = TS_ThemeEngine.ColorMode(theme, "DiskTotalSSDBG");
                DISK_TTLP_P2_L2.ForeColor = TS_ThemeEngine.ColorMode(theme, "DiskTotalHDDBG");
                DISK_TTLP_P3_L2.ForeColor = TS_ThemeEngine.ColorMode(theme, "DiskTotalUSBBG");
                DISK_TTLP_P4_L2.ForeColor = TS_ThemeEngine.ColorMode(theme, "DiskTotalTotalBG");
                //
                DISK_TTLP_Panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                DISK_TTLP_Panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                DISK_TTLP_Panel_3.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                DISK_TTLP_Panel_4.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                //
                DISK_Caption.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_CaptionList.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                DISK_CaptionList.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_Model.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Model_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_Man.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Man_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_VolumeID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_VolumeID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_VolumeName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_VolumeName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_PhysicalName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_PhysicalName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_Firmware.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Firmware_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_Serial.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Serial_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_VolumeSerial.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_VolumeSerial_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_Size.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Size_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_FreeSpace.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_FreeSpace_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_FileSystem.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_FileSystem_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_FormattingType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_FormattingType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_Type.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Type_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_DriveType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_DriveType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_InterFace.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_InterFace_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_PartitionCount.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_PartitionCount_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_MediaLoaded.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_MediaLoaded_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_MediaStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_MediaStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_Health.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Health_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_Boot.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Boot_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_Bootable.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_Bootable_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_BitLockerStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_BitLockerStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_BitLockerConversionStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_BitLockerConversionStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_BitLockerEncryptMehod.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_BitLockerEncryptMehod_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                DISK_DriveCompressed.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                DISK_DriveCompressed_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                // NETWORK
                network_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                network_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                network_panel_3.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                //
                NET_LT_Device.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LT_Device_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_LT_BandWidth.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LT_BandWidth_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_LT_P1.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                NET_LT_P2.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                NET_LT_DL1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LT_DL2.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_LT_UL1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LT_UL2.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                //
                NET_ListNetwork.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                NET_ListNetwork.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_ConnType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_MacAdress.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_MacAdress_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_NetMan.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_NetMan_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_ServiceName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_ServiceName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_AdapterType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_AdapterType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_Physical.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_Physical_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_DeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_DeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_Guid.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_Guid_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_ConnectionType.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_ConnectionType_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_Dhcp_status.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_Dhcp_status_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_Dhcp_server.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_Dhcp_server_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_LocalConSpeed.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_LocalConSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_IPv4Adress.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_IPv4Adress_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_IPv6Adress.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_IPv6Adress_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_DNS1.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_DNS1_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                NET_DNS2.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                NET_DNS2_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                // USB
                usb_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                usb_panel_2.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                USB_Con.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_ConList.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                USB_ConList.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                USB_ConName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_ConName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                USB_ConMan.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_ConMan_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                USB_ConDeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_ConDeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                USB_ConPNPDeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_ConPNPDeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                USB_ConDeviceStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_ConDeviceStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                USB_Device.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_Select.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                USB_Select.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                USB_DeviceName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_DeviceName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                USB_DeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_DeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                USB_PNPDeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_PNPDeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                USB_DeviceStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                USB_DeviceStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                // SOUND
                sound_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                SOUND_Device.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_Select.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                SOUND_Select.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                SOUND_DeviceName.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_DeviceName_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                SOUND_DeviceManufacturer.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_DeviceManufacturer_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                SOUND_DeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_DeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                SOUND_PNPDeviceID.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_PNPDeviceID_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                SOUND_DeviceStatus.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SOUND_DeviceStatus_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                // BATTERY
                battery_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                BATTERY_Status.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_Status_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                BATTERY_Model.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_Model_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                BATTERY_Name.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_Name_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                BATTERY_Voltage.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_Voltage_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                BATTERY_Type.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                BATTERY_Type_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                BATTERY_ReportBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "DynamicThemeActiveBtnBG");
                BATTERY_ReportBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                BATTERY_ReportBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                BATTERY_ReportBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                BATTERY_ReportBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRightHover");
                BATTERY_PBG_Panel.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                BATTERY_PFE_Panel.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                BATTERY_ProgressLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                // INSTALLED DRIVERS
                osd_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                OSD_TextBox.BackColor = TS_ThemeEngine.ColorMode(theme, "TextBoxBGColor");
                OSD_TextBox.ForeColor = TS_ThemeEngine.ColorMode(theme, "TextBoxFEColor");
                OSD_TYSS.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OSD_TYSS_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OSD_SearchDriverLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                OSD_TextBoxClearBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                OSD_TextBoxClearBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageFE");
                OSD_TextBoxClearBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OSD_TextBoxClearBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                OSD_TextBoxClearBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRightHover");
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
                SERVICE_TYS_V.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                SERVICE_SearchLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                SERVICE_TextBoxClearBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageBG");
                SERVICE_TextBoxClearBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "OSDAndServicesPageFE");
                SERVICE_TextBoxClearBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                SERVICE_TextBoxClearBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                SERVICE_TextBoxClearBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRightHover");
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
                // PRINT
                print_panel_1.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
                EXPORT_HeaderLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelLeft");
                EXPORT_Select.BackColor = TS_ThemeEngine.ColorMode(theme, "SelectBoxBGColor");
                EXPORT_Select.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                EXPORT_ProgressBGPanel.BackColor = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
                EXPORT_ProgressFEPanel.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                EXPORT_ProgessLabel.ForeColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                EXPORT_StartEngineBtn.ForeColor = TS_ThemeEngine.ColorMode(theme, "DynamicThemeActiveBtnBG");
                EXPORT_StartEngineBtn.BackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                EXPORT_StartEngineBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                EXPORT_StartEngineBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
                EXPORT_StartEngineBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(theme, "ContentLabelRightHover");
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
                    { 13, PRINT_RotateBtn }
                };
                if (buttonMap.TryGetValue(menu_btns, out Button selectedButton)){
                    selectedButton.BackColor = TS_ThemeEngine.ColorMode(theme, "DynamicThemeActiveBtnBG");
                }
                // SAVE CURRENT THEME
                try{
                    TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                    software_setting_save.TSWriteSettings(ts_settings_container, "ThemeStatus", Convert.ToString(ts));
                }catch (Exception){ }
            }catch (Exception){ }
        }
        private void header_image_reloader(int hi_value){
            try{
                var imageMap = new Dictionary<int, string>{
                    { 1, (theme == 1) ? (windows_mode == 1 ? "left_os_w11_light" : "left_os_w10_light") : (windows_mode == 1 ? "left_os_w11_dark" : "left_os_w10_dark") },
                    { 2, theme == 1 ? "lm_mb_light" : "lm_mb_dark" },
                    { 3, theme == 1 ? "lm_cpu_light" : "lm_cpu_dark" },
                    { 4, theme == 1 ? "lm_ram_light" : "lm_ram_dark" },
                    { 5, theme == 1 ? "lm_gpu_light" : "lm_gpu_dark" },
                    { 6, theme == 1 ? "lm_disk_light" : "lm_disk_dark" },
                    { 7, theme == 1 ? "lm_net_light" : "lm_net_dark" },
                    { 8, theme == 1 ? "lm_usb_light" : "lm_usb_dark" },
                    { 9, theme == 1 ? "lm_sound_light" : "lm_sound_dark" },
                    { 10, theme == 1 ? "lm_battery_light" : "lm_battery_dark" },
                    { 11, theme == 1 ? "lm_drivers_light" : "lm_drivers_dark" },
                    { 12, theme == 1 ? "lm_services_light" : "lm_services_dark" },
                    { 13, theme == 1 ? "lm_export_light" : "lm_export_dark" }
                };
                if (imageMap.TryGetValue(hi_value, out string imageName)){
                    HeaderImage.Image = (Image)Properties.Resources.ResourceManager.GetObject(imageName);
                }
            }catch (Exception){ }
        }
        // MODULES PAGE DYNAMIC UI
        // ======================================================================================================
        private void glow_other_page_dynamic_ui(){
            try{
                var glow_other_pages = new (string name, Func<object> createTool, Action<object> applySettings)[]{
                ("glow_sfc_and_dism_tool", () => new GlowSFCandDISMAutoTool(), tool => ((GlowSFCandDISMAutoTool)tool).sadt_theme_settings()),
                ("glow_cache_cleanup_tool", () => new GlowCacheCleanupTool(), tool => ((GlowCacheCleanupTool)tool).cct_theme_settings()),
                ("glow_bench_cpu_tool", () => new GlowBenchCPU(), tool => ((GlowBenchCPU)tool).bench_cpu_theme_settings()),
                ("glow_bench_ram_tool", () => new GlowBenchMemory(), tool => ((GlowBenchMemory)tool).bench_ram_settings()),
                ("glow_bench_disk_tool", () => new GlowBenchDisk(), tool => ((GlowBenchDisk)tool).bench_disk_theme_settings()),
                ("glow_screen_overlay_tool", () => new GlowOverlay(), tool => ((GlowOverlay)tool).screen_overlay_settings()),
                ("glow_dns_test_tool", () => new GlowDNSTestTool(), tool => ((GlowDNSTestTool)tool).dns_test_settings()),
                ("glow_quick_access_tool", () => new GlowQuickAccessTool(), tool => ((GlowQuickAccessTool)tool).quick_access_settings()),
                ("glow_show_wifi_password_tool", () => new GlowShowWiFiPasswordTool(), tool => ((GlowShowWiFiPasswordTool)tool).swpt_theme_settings()),
                ("glow_network_fix_tool", () => new GlowNetworkFixTool(), tool => ((GlowNetworkFixTool)tool).nft_theme_settings()),
                ("glow_monitor_test_engine_dead_pixel", () => new GlowMonitorTestEngine(), tool => ((GlowMonitorTestEngine)tool).monitor_test_engine_theme_settings()),
                ("glow_monitor_test_engine_dynamic_range", () => new GlowMonitorTestEngine(), tool => ((GlowMonitorTestEngine)tool).monitor_test_engine_theme_settings()),
                ("glow_about", () => new GlowAbout(), tool => ((GlowAbout)tool).about_preloader()),
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
        // INITIAL SETINGS
        // ======================================================================================================
        private void select_initial_mode_active(object target_initial_mode){
            ToolStripMenuItem selected_initial_mode = null;
            select_initial_mode_deactive();
            if (target_initial_mode != null){
                if (selected_initial_mode != (ToolStripMenuItem)target_initial_mode){
                    selected_initial_mode = (ToolStripMenuItem)target_initial_mode;
                    selected_initial_mode.Checked = true;
                }
            }
        }
        private void select_initial_mode_deactive(){
            foreach (ToolStripMenuItem disabled_initial in initialViewToolStripMenuItem.DropDownItems){
                disabled_initial.Checked = false;
            }
        }
        private void windowedToolStripMenuItem_Click(object sender, EventArgs e){
            if (initial_status != 0){ initial_status = 0; initial_mode_settings("0"); select_initial_mode_active(sender); }
        }
        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e){
            if (initial_status != 1){ initial_status = 1; initial_mode_settings("1"); select_initial_mode_active(sender); }
        }
        private void initial_mode_settings(string get_inital_value){
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "InitialStatus", get_inital_value);
            }catch (Exception){ }
        }
        // HIDING MODE
        // ======================================================================================================
        private void select_hiding_mode_active(object target_hiding_mode){
            ToolStripMenuItem selected_hiding_mode = null;
            select_hiding_mode_deactive();
            if (target_hiding_mode != null){
                if (selected_hiding_mode != (ToolStripMenuItem)target_hiding_mode){
                    selected_hiding_mode = (ToolStripMenuItem)target_hiding_mode;
                    selected_hiding_mode.Checked = true;
                }
            }
        }
        private void select_hiding_mode_deactive(){
            foreach (ToolStripMenuItem disabled_hiding in hidingModeToolStripMenuItem.DropDownItems){
                disabled_hiding.Checked = false;
            }
        }
        private void hidingModeOnToolStripMenuItem_Click(object sender, EventArgs e){
            if (hiding_status != 1){ hiding_status = 1; hiding_mode_settings("1"); select_hiding_mode_active(sender); }
        }
        private void hidingModeOffToolStripMenuItem_Click(object sender, EventArgs e){
            if (hiding_status != 0){ hiding_status = 0; hiding_mode_settings("0"); select_hiding_mode_active(sender); }
        }
        private void hiding_mode_settings(string get_hiding_value){
            try{
                TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                software_setting_save.TSWriteSettings(ts_settings_container, "HidingStatus", get_hiding_value);
            }catch (Exception){ }
            // HIDING MODE CHANGE NOTIFICATION
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            DialogResult hiding_mode_change_message = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HidingModeChange", "hiding_mode_change_notification")), "\n\n", "\n\n"));
            if (hiding_mode_change_message == DialogResult.Yes){ Application.Restart(); }
        }
        // UPDATE CHECK ENGINE
        // ======================================================================================================
        private void checkforUpdatesToolStripMenuItem_Click(object sender, EventArgs e){
            StartSoftwareUpdateCheck(1);
        }
        private async void StartSoftwareUpdateCheck(int u_mode){
            try{
                await Task.Run(() => software_update_check(u_mode), Program.TS_TokenEngine.Token);
            }catch (Exception){ }
        }
        public bool IsNetworkCheck(){
            Ping check_ping = new Ping();
            try{
                PingReply check_ping_reply = check_ping.Send("www.google.com");
                if (check_ping_reply.Status == IPStatus.Success){
                    return true;
                }
            }catch (PingException){ }
            return false;
        }
        public void software_update_check(int _check_update_ui){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                if (!IsNetworkCheck()){
                    if (_check_update_ui == 1){
                        TS_MessageBoxEngine.TS_MessageBox(this, 2, string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_not_connection")), "\n\n"), string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_title")), Application.ProductName));
                    }
                    return;
                }
                using (WebClient webClient = new WebClient()){
                    string client_version = TS_VersionEngine.TS_SofwareVersion(2, Program.ts_version_mode).Trim();
                    int client_num_version = Convert.ToInt32(client_version.Replace(".", string.Empty));
                    //
                    string[] version_content = webClient.DownloadString(TS_LinkSystem.github_link_lt).Split('=');
                    string last_version = version_content[1].Trim();
                    int last_num_version = Convert.ToInt32(last_version.Replace(".", string.Empty));
                    //
                    if (client_num_version < last_num_version){
                        // Update available
                        DialogResult info_update = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_available")), Application.ProductName, "\n\n", client_version, "\n", last_version, "\n\n"), string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_title")), Application.ProductName));
                        if (info_update == DialogResult.Yes){
                            Process.Start(new ProcessStartInfo(TS_LinkSystem.github_link_lr){ UseShellExecute = true });
                        }
                    }else if (_check_update_ui == 1 && client_num_version == last_num_version){
                        // No update available
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_not_available")), Application.ProductName, "\n", client_version), string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_title")), Application.ProductName));
                    }else if (_check_update_ui == 1 && client_num_version > last_num_version){
                        // Access before public use
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_newer")), "\n\n", string.Format("v{0}", client_version)), string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_title")), Application.ProductName));
                    }
                }
            }catch (Exception ex){
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_error")), "\n\n", ex.Message), string.Format(TS_String_Encoder(software_lang.TSReadLangs("SoftwareUpdate", "su_title")), Application.ProductName));
            }
        }
        // PRINT ENGINES
        // ======================================================================================================
        List<string> PrintEngineList = new List<string>();
        private void print_mode_add(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                EXPORT_Select.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("Export", "e_mode_txt")));
                EXPORT_Select.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("Export", "e_mode_html")));
                EXPORT_Select.SelectedIndex = 0;
            }catch (Exception){ }
        }
        private void EXPORT_StartEngineBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                DialogResult start_print_info = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(TS_String_Encoder(software_lang.TSReadLangs("Export", "e_mode_report_start_info")), EXPORT_Select.SelectedItem.ToString(), "\n\n"));
                if (start_print_info == DialogResult.Yes){
                    EXPORT_ProgressBGPanel.Visible = true;
                    EXPORT_ProgessLabel.Visible = true;
                    //
                    EXPORT_StartEngineBtn.Enabled = false;
                    if (EXPORT_Select.SelectedIndex == 0){
                        print_engine_mode(0);
                    }else if (EXPORT_Select.SelectedIndex == 1){
                        print_engine_mode(1);
                    }
                    EXPORT_StartEngineBtn.Enabled = true;
                }
            }catch (Exception){ }
        }
        private void print_engine_progress_update(int status){
            try{
                const int total_pages = 12;
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                var progressPages = new Dictionary<int, string>{
                    { 1, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_os")) },
                    { 2, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_mb")) },
                    { 3, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_cpu")) },
                    { 4, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_ram")) },
                    { 5, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_gpu")) },
                    { 6, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_storage")) },
                    { 7, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_network")) },
                    { 8, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_usb")) },
                    { 9, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_sound")) },
                    { 10, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_battery")) },
                    { 11, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_installed_drivers")) },
                    { 12, TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_installed_services")) }
                };
                if (status <= total_pages){
                    progressPages.TryGetValue(status, out string progress_page);
                    EXPORT_ProgessLabel.Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("Export", "e_mode_process")), progress_page, status, total_pages);
                    EXPORT_ProgressFEPanel.Width = CalculateProgressWidth(status, total_pages);
                }else{
                    EXPORT_ProgessLabel.Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("Export", "e_mode_save_await")), status - 1, total_pages);
                    EXPORT_ProgressFEPanel.Width = CalculateProgressWidth(status - 1, total_pages);
                }
            }catch (Exception){ }
        }
        private int CalculateProgressWidth(int status, int total_pages){
            return (int)((EXPORT_ProgressBGPanel.Width * status) / total_pages);
        }
        private void print_after_mode(){
            EXPORT_ProgressBGPanel.Visible = false;
            EXPORT_ProgressFEPanel.Width = 0;
            EXPORT_ProgessLabel.Visible = false;
        }
        private void print_engine_mode(int pe_mode){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            try{
                switch (pe_mode){
                    case 0:
                        print_engine_txt();
                    break;
                    case 1:
                        print_engine_html();
                    break;
                }
            }catch (Exception){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_export_info_error")));
            }
        }
        // PRINT ENGINE TXT
        // ======================================================================================================
        private void print_engine_txt(){
            // HEADER
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            PrintEngineList.Add($"<{new string('-', 13)} {Application.ProductName.ToUpper()} - {string.Format(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_report_title").ToUpper()), OS_ComputerName_V.Text)} {new string('-', 13)}>");
            PrintEngineList.Add(Environment.NewLine + new string('-', 60) + Environment.NewLine);
            // OS
            print_engine_progress_update(1);
            PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_os"))} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(OS_SystemUser.Text + " " + OS_SystemUser_V.Text);
            PrintEngineList.Add(OS_ComputerName.Text + " " + OS_ComputerName_V.Text);
            PrintEngineList.Add(OS_SavedUser.Text + " " + OS_SavedUser_V.Text);
            PrintEngineList.Add(OS_Name.Text + " " + OS_Name_V.Text);
            PrintEngineList.Add(OS_Manufacturer.Text + " " + OS_Manufacturer_V.Text);
            PrintEngineList.Add(OS_SystemVersion.Text + " " + OS_SystemVersion_V.Text);
            PrintEngineList.Add(OS_Build.Text + " " + OS_Build_V.Text);
            PrintEngineList.Add(OS_SystemBuild.Text + " " + OS_SystemBuild_V.Text);
            PrintEngineList.Add(OS_SystemArchitectural.Text + " " + OS_SystemArchitectural_V.Text);
            PrintEngineList.Add(OS_Family.Text + " " + OS_Family_V.Text);
            PrintEngineList.Add(OS_DeviceID.Text + " " + OS_DeviceID_V.Text);
            PrintEngineList.Add(OS_Serial.Text + " " + OS_Serial_V.Text);
            PrintEngineList.Add(OS_Country.Text + " " + OS_Country_V.Text);
            PrintEngineList.Add(OS_TimeZone.Text + " " + OS_TimeZone_V.Text);
            PrintEngineList.Add(OS_CharacterSet.Text + " " + OS_CharacterSet_V.Text);
            PrintEngineList.Add(OS_EncryptionType.Text + " " + OS_EncryptionType_V.Text);
            PrintEngineList.Add(OS_SystemRootIndex.Text + " " + OS_SystemRootIndex_V.Text);
            PrintEngineList.Add(OS_SystemBuildPart.Text + " " + OS_SystemBuildPart_V.Text);
            PrintEngineList.Add(OS_SystemTime.Text + " " + OS_SystemTime_V.Text);
            PrintEngineList.Add(OS_Install.Text + " " + OS_Install_V.Text);
            PrintEngineList.Add(OS_SystemWorkTime.Text + " " + OS_SystemWorkTime_V.Text);
            PrintEngineList.Add(OS_LastBootTime.Text + " " + OS_LastBootTime_V.Text);
            PrintEngineList.Add(OS_SystemLastShutDown.Text + " " + OS_SystemLastShutDown_V.Text);
            PrintEngineList.Add(OS_PortableOS.Text + " " + OS_PortableOS_V.Text);
            PrintEngineList.Add(OS_FastBoot.Text + " " + OS_FastBoot_V.Text);
            PrintEngineList.Add(OS_MouseWheelStatus.Text + " " + OS_MouseWheelStatus_V.Text);
            PrintEngineList.Add(OS_ScrollLockStatus.Text + " " + OS_ScrollLockStatus_V.Text);
            PrintEngineList.Add(OS_NumLockStatus.Text + " " + OS_NumLockStatus_V.Text);
            PrintEngineList.Add(OS_CapsLockStatus.Text + " " + OS_CapsLockStatus_V.Text);
            PrintEngineList.Add(OS_BootPartition.Text + " " + OS_BootPartition_V.Text);
            PrintEngineList.Add(OS_SystemPartition.Text + " " + OS_SystemPartition_V.Text);
            PrintEngineList.Add(OS_WinPageFile.Text + " " + OS_WinPageFile_V.Text);
            PrintEngineList.Add(OS_TempWinPageFile.Text + " " + OS_TempWinPageFile_V.Text);
            PrintEngineList.Add(OS_Hiberfil.Text + " " + OS_Hiberfil_V.Text);
            PrintEngineList.Add(OS_AVProgram.Text + " " + OS_AVProgram_V.Text);
            PrintEngineList.Add(OS_FirewallProgram.Text + " " + OS_FirewallProgram_V.Text);
            PrintEngineList.Add(OS_AntiSpywareProgram.Text + " " + OS_AntiSpywareProgram_V.Text);
            PrintEngineList.Add(OS_WinDefCoreIsolation.Text + " " + OS_WinDefCoreIsolation_V.Text);
            PrintEngineList.Add(OS_MSEdge.Text + " " + OS_MSEdge_V.Text);
            PrintEngineList.Add(OS_MSEdgeWebView.Text + " " + OS_MSEdgeWebView_V.Text);
            PrintEngineList.Add(OS_MSStoreVersion.Text + " " + OS_MSStoreVersion_V.Text);
            PrintEngineList.Add(OS_WinKey.Text + " " + OS_WinKey_V.Text);
            PrintEngineList.Add(OS_WinActiveChannel.Text + " " + OS_WinActiveChannel_V.Text);
            PrintEngineList.Add(OS_NETFrameworkVersion.Text + " " + OS_NETFrameworkVersion_V.Text);
            PrintEngineList.Add(OS_Minidump.Text + " " + OS_Minidump_V.Text);
            PrintEngineList.Add(OS_BSODDate.Text + " " + OS_BSODDate_V.Text);
            PrintEngineList.Add(OS_Wallpaper.Text + " " + OS_Wallpaper_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            // MOTHERBOARD
            print_engine_progress_update(2);
            PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_mb"))} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(MB_MotherBoardName.Text + " " + MB_MotherBoardName_V.Text);
            PrintEngineList.Add(MB_MotherBoardMan.Text + " " + MB_MotherBoardMan_V.Text);
            PrintEngineList.Add(MB_SystemModelMan.Text + " " + MB_SystemModelMan_V.Text);
            PrintEngineList.Add(MB_SystemModelFamily.Text + " " + MB_SystemModelFamily_V.Text);
            PrintEngineList.Add(MB_SystemModel.Text + " " + MB_SystemModel_V.Text);
            PrintEngineList.Add(MB_MotherBoardSerial.Text + " " + MB_MotherBoardSerial_V.Text);
            PrintEngineList.Add(MB_SystemFamily.Text + " " + MB_SystemFamily_V.Text);
            PrintEngineList.Add(MB_SystemSKU.Text + " " + MB_SystemSKU_V.Text);
            PrintEngineList.Add(MB_Chipset.Text + " " + MB_Chipset_V.Text);
            PrintEngineList.Add(MB_BiosManufacturer.Text + " " + MB_BiosManufacturer_V.Text);
            PrintEngineList.Add(MB_BiosDate.Text + " " + MB_BiosDate_V.Text);
            PrintEngineList.Add(MB_BiosVersion.Text + " " + MB_BiosVersion_V.Text);
            PrintEngineList.Add(MB_SmBiosVersion.Text + " " + MB_SmBiosVersion_V.Text);
            PrintEngineList.Add(MB_BiosMode.Text + " " + MB_BiosMode_V.Text);
            PrintEngineList.Add(MB_LastBIOSTime.Text + " " + MB_LastBIOSTime_V.Text);
            PrintEngineList.Add(MB_SecureBoot.Text + " " + MB_SecureBoot_V.Text);
            PrintEngineList.Add(MB_TPMStatus.Text + " " + MB_TPMStatus_V.Text);
            PrintEngineList.Add(MB_TPMPhysicalVersion.Text + " " + MB_TPMPhysicalVersion_V.Text);
            PrintEngineList.Add(MB_TPMMan.Text + " " + MB_TPMMan_V.Text);
            PrintEngineList.Add(MB_TPMManID.Text + " " + MB_TPMManID_V.Text);
            PrintEngineList.Add(MB_TPMManVersion.Text + " " + MB_TPMManVersion_V.Text);
            PrintEngineList.Add(MB_TPMManFullVersion.Text + " " + MB_TPMManFullVersion_V.Text);
            PrintEngineList.Add(MB_TPMManPublisher.Text + " " + MB_TPMManPublisher_V.Text);
            PrintEngineList.Add(MB_Model.Text + " " + MB_Model_V.Text);
            PrintEngineList.Add(MB_PrimaryBusType.Text + " " + MB_PrimaryBusType_V.Text);
            PrintEngineList.Add(MB_SecondaryBusType.Text + " " + MB_SecondaryBusType_V.Text);
            PrintEngineList.Add(MB_BiosMajorMinor.Text + " " + MB_BiosMajorMinor_V.Text);
            PrintEngineList.Add(MB_SMBiosMajorMinor.Text + " " + MB_SMBiosMajorMinor_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            // CPU
            print_engine_progress_update(3);
            PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_cpu"))} {new string('-', 7)}>" + Environment.NewLine);
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
            PrintEngineList.Add(CPU_SocketDefinition.Text + " " + CPU_SocketDefinition_V.Text);
            PrintEngineList.Add(CPU_Family.Text + " " + CPU_Family_V.Text);
            PrintEngineList.Add(CPU_MicroCode.Text + " " + CPU_MicroCode_V.Text);
            PrintEngineList.Add(CPU_PlatformFeature.Text + " " + CPU_PlatformFeature_V.Text);
            PrintEngineList.Add(CPU_FeatureSet.Text + " " + CPU_FeatureSet_V.Text);
            PrintEngineList.Add(CPU_Revision.Text + " " + CPU_Revision_V.Text);
            PrintEngineList.Add(CPU_OldRevision.Text + " " + CPU_OldRevision_V.Text);
            PrintEngineList.Add(CPU_Virtualization.Text + " " + CPU_Virtualization_V.Text);
            PrintEngineList.Add(CPU_SerialName.Text + " " + CPU_SerialName_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            // RAM
            print_engine_progress_update(4);
            PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_ram"))} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(RAM_TotalRAM.Text + " " + RAM_TotalRAM_V.Text);
            PrintEngineList.Add(RAM_UsageRAMCount.Text + " " + RAM_UsageRAMCount_V.Text);
            PrintEngineList.Add(RAM_EmptyRamCount.Text + " " + RAM_EmptyRamCount_V.Text);
            PrintEngineList.Add(RAM_TotalVirtualRam.Text + " " + RAM_TotalVirtualRam_V.Text);
            PrintEngineList.Add(RAM_UsageVirtualRam.Text + " " + RAM_UsageVirtualRam_V.Text);
            PrintEngineList.Add(RAM_EmptyVirtualRam.Text + " " + RAM_EmptyVirtualRam_V.Text);
            PrintEngineList.Add(RAM_SlotStatus.Text + " " + RAM_SlotStatus_V.Text + Environment.NewLine);
            try{
                int ram_slot = RAM_SelectList.Items.Count;
                for (int rs = 1; rs <= ram_slot; rs++){
                    RAM_SelectList.SelectedIndex = rs - 1;
                    PrintEngineList.Add(RAM_SelectList.SelectedItem + Environment.NewLine);
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
                RAM_SelectList.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // GPU
            print_engine_progress_update(5);
            PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_gpu"))} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int gpu_amount = GPU_Select.Items.Count;
                for (int gpu_render = 1; gpu_render <= gpu_amount; gpu_render++){
                    GPU_Select.SelectedIndex = gpu_render - 1;
                    PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_gpu_print")) + " " + GPU_Select.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(GPU_Name.Text + " " + GPU_Select.SelectedItem.ToString());
                    PrintEngineList.Add(GPU_Manufacturer.Text + " " + GPU_Manufacturer_V.Text);
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
                GPU_Select.SelectedIndex = 0;
            }catch (Exception){ }
            try{
                int screen_amount = GPU_MonitorSelectList.Items.Count;
                for (int sa = 1; sa <= screen_amount; sa++){
                    GPU_MonitorSelectList.SelectedIndex = sa - 1;
                    PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_monitor_print")) + " #" + sa + Environment.NewLine);
                    PrintEngineList.Add(GPU_MonitorBounds.Text + " " + GPU_MonitorBounds_V.Text);
                    PrintEngineList.Add(GPU_MonitorWorking.Text + " " + GPU_MonitorWorking_V.Text);
                    PrintEngineList.Add(GPU_MonitorResLabel.Text + " " + GPU_MonitorResLabel_V.Text);
                    PrintEngineList.Add(GPU_MonitorVirtualRes.Text + " " + GPU_MonitorVirtualRes_V.Text);
                    PrintEngineList.Add(GPU_ScreenRefreshRate.Text + " " + GPU_ScreenRefreshRate_V.Text);
                    PrintEngineList.Add(GPU_ScreenBit.Text + " " + GPU_ScreenBit_V.Text);
                    PrintEngineList.Add(GPU_MonitorPrimary.Text + " " + GPU_MonitorPrimary_V.Text + Environment.NewLine);
                }
                PrintEngineList.Add(GPU_DesktopResolution.Text + " " + GPU_DesktopResolution_V.Text + Environment.NewLine);
                PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
                GPU_MonitorSelectList.SelectedIndex = 0;
            }catch (Exception){ }
            // STORAGE
            print_engine_progress_update(6);
            PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_storage"))} {new string('-', 7)}>" + Environment.NewLine);
            try{
                PrintEngineList.Add(DISK_TTLP_L1.Text + Environment.NewLine);
                PrintEngineList.Add(DISK_TTLP_P1_L1.Text + " " + DISK_TTLP_P1_L2.Text);
                PrintEngineList.Add(DISK_TTLP_P2_L1.Text + " " + DISK_TTLP_P2_L2.Text);
                PrintEngineList.Add(DISK_TTLP_P3_L1.Text + " " + DISK_TTLP_P3_L2.Text);
                PrintEngineList.Add(DISK_TTLP_P4_L1.Text + " " + DISK_TTLP_P4_L2.Text);
                PrintEngineList.Add(Environment.NewLine + new string('-', 12) + Environment.NewLine);
                int disk_amount = DISK_CaptionList.Items.Count;
                for (int disk_render = 1; disk_render <= disk_amount; disk_render++){
                    DISK_CaptionList.SelectedIndex = disk_render - 1;
                    PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_disk_print")) + " " + DISK_CaptionList.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(DISK_Caption.Text + " " + DISK_CaptionList.SelectedItem.ToString());
                    PrintEngineList.Add(DISK_Model.Text + " " + DISK_Model_V.Text);
                    PrintEngineList.Add(DISK_Man.Text + " " + DISK_Man_V.Text);
                    PrintEngineList.Add(DISK_VolumeID.Text + " " + DISK_VolumeID_V.Text);
                    PrintEngineList.Add(DISK_VolumeName.Text + " " + DISK_VolumeName_V.Text);
                    PrintEngineList.Add(DISK_PhysicalName.Text + " " + DISK_PhysicalName_V.Text);
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
                    DISK_CaptionList.SelectedIndex = c_index;
                    if (c_index == -1){
                        DISK_CaptionList.SelectedIndex = 0;
                    }
                }catch (Exception){
                    DISK_CaptionList.SelectedIndex = 0;
                }
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // NETWORK
            print_engine_progress_update(7);
            PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_network"))} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int net_amount = NET_ListNetwork.Items.Count;
                PrintEngineList.Add(NET_LT_Device.Text + " " + NET_LT_Device_V.Text);
                PrintEngineList.Add(NET_LT_BandWidth.Text + " " + NET_LT_BandWidth_V.Text);
                PrintEngineList.Add(NET_LT_DL1.Text + " " + NET_LT_DL2.Text);
                PrintEngineList.Add(NET_LT_UL1.Text + " " + NET_LT_UL2.Text + Environment.NewLine);
                for (int net_render = 1; net_render <= net_amount; net_render++){
                    NET_ListNetwork.SelectedIndex = net_render - 1;
                    PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_network_print")) + " " + NET_ListNetwork.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(NET_ConnType.Text + " " + NET_ListNetwork.SelectedItem.ToString());
                    PrintEngineList.Add(NET_MacAdress.Text + " " + NET_MacAdress_V.Text);
                    PrintEngineList.Add(NET_NetMan.Text + " " + NET_NetMan_V.Text);
                    PrintEngineList.Add(NET_ServiceName.Text + " " + NET_ServiceName_V.Text);
                    PrintEngineList.Add(NET_AdapterType.Text + " " + NET_AdapterType_V.Text);
                    PrintEngineList.Add(NET_Physical.Text + " " + NET_Physical_V.Text);
                    PrintEngineList.Add(NET_DeviceID.Text + " " + NET_DeviceID_V.Text);
                    PrintEngineList.Add(NET_Guid.Text + " " + NET_Guid_V.Text);
                    PrintEngineList.Add(NET_ConnectionType.Text + " " + NET_ConnectionType_V.Text);
                    PrintEngineList.Add(NET_Dhcp_status.Text + " " + NET_Dhcp_status_V.Text);
                    PrintEngineList.Add(NET_Dhcp_server.Text + " " + NET_Dhcp_server_V.Text);
                    PrintEngineList.Add(NET_LocalConSpeed.Text + " " + NET_LocalConSpeed_V.Text);
                    PrintEngineList.Add(NET_IPv4Adress.Text + " " + NET_IPv4Adress_V.Text);
                    PrintEngineList.Add(NET_IPv6Adress.Text + " " + NET_IPv6Adress_V.Text + Environment.NewLine);
                }
                PrintEngineList.Add(NET_DNS1.Text + " " + NET_DNS1_V.Text);
                PrintEngineList.Add(NET_DNS2.Text + " " + NET_DNS2_V.Text + Environment.NewLine);
                NET_ListNetwork.SelectedIndex = 1;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // USB
            print_engine_progress_update(8);
            PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_usb"))} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int usb_con_amount = USB_ConList.Items.Count;
                for (int usb_con_render = 1; usb_con_render <= usb_con_amount; usb_con_render++){
                    USB_ConList.SelectedIndex = usb_con_render - 1;
                    PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_controller_print")) + " " + USB_ConList.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(USB_Con.Text + " " + USB_ConList.SelectedItem.ToString());
                    PrintEngineList.Add(USB_ConName.Text + " " + USB_ConName_V.Text);
                    PrintEngineList.Add(USB_ConMan.Text + " " + USB_ConMan_V.Text);
                    PrintEngineList.Add(USB_ConDeviceID.Text + " " + USB_ConDeviceID_V.Text);
                    PrintEngineList.Add(USB_ConPNPDeviceID.Text + " " + USB_ConPNPDeviceID_V.Text);
                    PrintEngineList.Add(USB_ConDeviceStatus.Text + " " + USB_ConDeviceStatus_V.Text + Environment.NewLine);
                }
                USB_ConList.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 40) + Environment.NewLine);
            try{
                int usb_amount = USB_Select.Items.Count;
                for (int usb_render = 1; usb_render <= usb_amount; usb_render++){
                    USB_Select.SelectedIndex = usb_render - 1;
                    PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_device_print")) + " " + USB_Select.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(USB_Device.Text + " " + USB_Select.SelectedItem.ToString());
                    PrintEngineList.Add(USB_DeviceName.Text + " " + USB_DeviceName_V.Text);
                    PrintEngineList.Add(USB_DeviceID.Text + " " + USB_DeviceID_V.Text);
                    PrintEngineList.Add(USB_PNPDeviceID.Text + " " + USB_PNPDeviceID_V.Text);
                    PrintEngineList.Add(USB_DeviceStatus.Text + " " + USB_DeviceStatus_V.Text + Environment.NewLine);
                }
                USB_Select.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // SOUND
            print_engine_progress_update(9);
            PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_sound"))} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int sound_amount = SOUND_Select.Items.Count;
                for (int sound_render = 1; sound_render <= sound_amount; sound_render++){
                    SOUND_Select.SelectedIndex = sound_render - 1;
                    PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("Sound_Content", "sound_c_print")) + " " + SOUND_Select.SelectedItem + Environment.NewLine);
                    PrintEngineList.Add(SOUND_Device.Text + " " + SOUND_Select.SelectedItem.ToString());
                    PrintEngineList.Add(SOUND_DeviceName.Text + " " + SOUND_DeviceName_V.Text);
                    PrintEngineList.Add(SOUND_DeviceManufacturer.Text + " " + SOUND_DeviceManufacturer_V.Text);
                    PrintEngineList.Add(SOUND_DeviceID.Text + " " + SOUND_DeviceID_V.Text);
                    PrintEngineList.Add(SOUND_PNPDeviceID.Text + " " + SOUND_PNPDeviceID_V.Text);
                    PrintEngineList.Add(SOUND_DeviceStatus.Text + " " + SOUND_DeviceStatus_V.Text + Environment.NewLine);
                }
                SOUND_Select.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // BATTERY
            print_engine_progress_update(10);
            PowerStatus power_status = SystemInformation.PowerStatus;
            String battery_charging = power_status.BatteryChargeStatus.ToString();
            if (battery_charging == "NoSystemBattery"){
                PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_battery"))} {new string('-', 7)}>" + Environment.NewLine);
                PrintEngineList.Add(BATTERY_Status.Text + " " + BATTERY_Status_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            }else{
                PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_battery"))} {new string('-', 7)}>" + Environment.NewLine);
                PrintEngineList.Add(BATTERY_Status.Text + " " + BATTERY_Status_V.Text);
                PrintEngineList.Add(BATTERY_Model.Text + " " + BATTERY_Model_V.Text);
                PrintEngineList.Add(BATTERY_Name.Text + " " + BATTERY_Name_V.Text);
                PrintEngineList.Add(BATTERY_Voltage.Text + " " + BATTERY_Voltage_V.Text);
                PrintEngineList.Add(BATTERY_Type.Text + " " + BATTERY_Type_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            }
            // OSD
            print_engine_progress_update(11);
            PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_installed_drivers"))} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_driver_sorting")) + Environment.NewLine);
            try{
                for (int i = 0; i < OSD_DataMainTable.Rows.Count; i++){
                    PrintEngineList.Add(OSD_DataMainTable.Rows[i].Cells[0].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[1].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[2].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[3].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[4].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[5].Value.ToString() + Environment.NewLine + new string('-', 155));
                }
            }catch (Exception){ }
            PrintEngineList.Add(Environment.NewLine + TS_String_Encoder(software_lang.TSReadLangs("Osd_Content", "osd_total_installed_driver_count")) + " " + OSD_TYSS_V.Text + Environment.NewLine);
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // SERVICES
            print_engine_progress_update(12);
            PrintEngineList.Add($"<{new string('-', 7)} {TS_String_Encoder(software_lang.TSReadLangs("Header", "header_installed_services"))} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_service_sorting")) + Environment.NewLine);
            try{
                for (int i = 0; i < SERVICE_DataMainTable.Rows.Count; i++){
                    PrintEngineList.Add(SERVICE_DataMainTable.Rows[i].Cells[0].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[1].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[2].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[3].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[4].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[5].Value.ToString() + Environment.NewLine + new string('-', 155));
                }
            }catch (Exception){ }
            PrintEngineList.Add(Environment.NewLine + TS_String_Encoder(software_lang.TSReadLangs("Services_Content", "ss_total_installed_service_count")) + " " + SERVICE_TYS_V.Text + Environment.NewLine);
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // FOOTER
            PrintEngineList.Add(Application.ProductName + " " + TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_version")) + " " + TS_VersionEngine.TS_SofwareVersion(1, Program.ts_version_mode));
            PrintEngineList.Add(TS_SoftwareCopyrightDate.ts_scd_preloader);
            PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_process_time")) + " " + DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss"));
            PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_website")) + " " + TS_LinkSystem.website_link);
            PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_twitter")) + " " + TS_LinkSystem.twitter_x_link);
            PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_instagram")) + " " + TS_LinkSystem.instagram_link);
            PrintEngineList.Add(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_github")) + " " + TS_LinkSystem.github_link);
            print_engine_progress_update(13);
            SaveFileDialog save_engine = new SaveFileDialog{
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = Application.ProductName + " - " + TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_save_directory")),
                DefaultExt = "txt",
                FileName = Application.ProductName + " - " + string.Format(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_save_name")), OS_ComputerName_V.Text),
                Filter = TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_save_txt")) + " (*.txt)|*.txt"
            };
            if (save_engine.ShowDialog() == DialogResult.OK){
                string combinedText = String.Join(Environment.NewLine, PrintEngineList);
                File.WriteAllText(save_engine.FileName, combinedText);
                DialogResult glow_print_engine_query = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_save_success")) + "\n\n" + TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_save_info_open")), Application.ProductName, save_engine.FileName));
                if (glow_print_engine_query == DialogResult.Yes){
                    Process.Start(save_engine.FileName);
                }
                PrintEngineList.Clear();
                save_engine.Dispose();
                print_after_mode();
            }else{
                PrintEngineList.Clear();
                save_engine.Dispose();
                print_after_mode();
            }
        }
        // PRINT ENGINE HTML
        // ======================================================================================================
        private void print_engine_html(){
            // HEADER
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            // COLOR MODES
            Color html_body_bg_color = TS_ThemeEngine.ColorMode(theme, "ContentPanelBGColor");
            string html_bbgc = string.Format("#{0}{1}{2}", html_body_bg_color.R.ToString("X2"), html_body_bg_color.G.ToString("X2"), html_body_bg_color.B.ToString("X2"));
            Color html_middle_bg_color = TS_ThemeEngine.ColorMode(theme, "PageContainerBGAndPageContentTotalColors");
            string html_mbgc = string.Format("#{0}{1}{2}", html_middle_bg_color.R.ToString("X2"), html_middle_bg_color.G.ToString("X2"), html_middle_bg_color.B.ToString("X2"));
            Color html_ui_fe_color = TS_ThemeEngine.ColorMode(theme, "ContentLabelRight");
            string html_uifc = string.Format("#{0}{1}{2}", html_ui_fe_color.R.ToString("X2"), html_ui_fe_color.G.ToString("X2"), html_ui_fe_color.B.ToString("X2"));
            Color html_ui_fe_hover_color = TS_ThemeEngine.ColorMode(theme, "ContentLabelRightHover");
            string html_uifhc = string.Format("#{0}{1}{2}", html_ui_fe_hover_color.R.ToString("X2"), html_ui_fe_hover_color.G.ToString("X2"), html_ui_fe_hover_color.B.ToString("X2"));
            // EXTERNAL LINKS
            string print_html_font_url          = @"https://fonts.googleapis.com/css2?family=Open+Sans:ital,wght@0,300..800;1,300..800&display=swap";
            string print_html_glow_logo_url     = @"https://raw.githubusercontent.com/roines45/glow/main/Glow/glow_images/glow_logo/GlowLogo.ico";
            // HTML MAIN CODES
            PrintEngineList.Add("<!DOCTYPE html>");
            PrintEngineList.Add($"<html lang='{lang}'>");
            PrintEngineList.Add("<head>");
            PrintEngineList.Add("\t<meta charset='utf-8'>");
            PrintEngineList.Add("\t<meta name='author' content='" + Application.CompanyName + "'>");
            PrintEngineList.Add("\t<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            PrintEngineList.Add($"\t<title>{Application.ProductName} - {string.Format(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_report_title")), OS_ComputerName_V.Text)}</title>");
            PrintEngineList.Add("\t<style>");
            PrintEngineList.Add("\t\t@import url('" + print_html_font_url + "');");
            PrintEngineList.Add("\t\t*{ font-family: 'Open Sans', sans-serif; margin: 0; padding: 0; box-sizing: border-box; text-decoration: none; }");
            if (theme == 1){ PrintEngineList.Add("\t\t:root { color-scheme: light; }"); }
            else if (theme == 0){ PrintEngineList.Add("\t\t:root { color-scheme: dark; }"); }
            PrintEngineList.Add("\t\ta:visited{ color: " + html_uifhc + "; }");
            PrintEngineList.Add("\t\t.ts_scroll_top{ background-color: " + html_uifc + "; position: fixed; bottom: -100px; right: 15px; width: 45px; height: 45px; justify-content: center; align-items: center; display: flex; border-radius: 50%; cursor: pointer; z-index: 999; transition: 0.2s; }");
            PrintEngineList.Add("\t\t.ts_scroll_top > .ts_arrow_up { border-bottom: 15px solid " + html_mbgc + "; width: 0; height: 0; border-left: 10px solid transparent; border-right: 10px solid transparent; }");
            PrintEngineList.Add("\t\t.ts_scroll_top:hover{ background-color: " + html_uifhc + "; }");
            PrintEngineList.Add("\t\t.ts_scroll_top.active{ bottom: 15px; }");
            PrintEngineList.Add("\t\t@media (max-width: 700px){ .ts_scroll_top{ bottom: -100px; right: 10px; } .ts_scroll_top.active{ bottom: 10px; } }");
            PrintEngineList.Add("\t\t.ts_page_wrapper{ background-color:" + html_mbgc + "; font-weight: 500; position:fixed; left: 15px; bottom: 15px; width: auto; height: 35px; padding: 0 5px; font-size: 16px; margin: 0; outline: none; border-radius: 5px; border: 1px solid " + html_uifc + "; transition: 0.2s; -webkit-box-shadow: 0px 0px 13px -3px rgba(0,0,0,0.7); -moz-box-shadow: 0px 0px 13px -3px rgba(0,0,0,0.7); box-shadow: 0px 0px 13px -3px rgba(0,0,0,0.7); }");
            PrintEngineList.Add("\t\t@media (max-width: 700px){ .ts_page_wrapper{ left: 10px; bottom: 10px; height: 30px; border-radius: 3px; } }");
            PrintEngineList.Add("\t\tbody{ background-color: " + html_bbgc + "; padding: 5px 10px; justify-content: center; align-items: center; display: flex; }");
            PrintEngineList.Add("\t\t#main_container{ width: 100%; height: auto; justify-content: center; align-items: center; display: flex; flex-direction: column; }");
            PrintEngineList.Add("\t\t#main_container > h2{ margin: 25px 0; font-weight: 500; color: " + html_uifc + "; }");
            PrintEngineList.Add("\t\t#main_container > .b1, .b2, .b3, .b4, .b5, .b6, .b7, .b8, .b9, .b10, .b11, .b12, .b13, .b14{ background-color: " + html_mbgc + "; width: 1250px; height: auto; border-radius: 10px; margin: 5px 0; padding: 15px; box-sizing: border-box; display: inline-block; word-break: break-word; table-layout: fixed; }");
            PrintEngineList.Add("\t\t#main_container > .b8 > h3{ margin: 0 0 15px 0; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1){ justify-content: start; align-items: center; display: flex; gap: 25px; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1) > img{ width: 75px; height: 75px; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1) > .ts_box_text{ justify-content: center; align-items: start; display: flex; flex-direction: column; gap: 5px; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1) > .ts_box_text > h2{ color: " + html_uifc + "; font-weight: 500; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1) > .ts_box_text > span{ font-weight: 500; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper:nth-child(1) > .ts_box_text > span > label{ color: " + html_uifc + "; font-weight: 500; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > h2{ color: " + html_uifc + "; font-weight: 500; text-align: center; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > h3{ color: " + html_uifc + "; font-weight: 500; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > h4{ margin: 13px 0 0 11px; font-weight: 600; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > h4 > span{ margin: 0 5px 0 0; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > h4 > span:nth-child(2){ color: " + html_uifc + "; font-weight: 600; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul{ margin: 15px 0 0 30px; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul > li{ margin: 5px 0; font-weight: 500; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul > li > span{ margin: 0 7px 0 0; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul > li > span:nth-child(2){ color: " + html_uifc + "; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul > li > a{ color: " + html_uifc + "; text-decoration: underline; transition: 0.3s; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > ul > li > a:hover{ color: " + html_uifhc + "; }");
            PrintEngineList.Add("\t\t#main_container > .ts_box_wrapper > hr{ height: 1px; background-color: " + html_uifc + "; border: none; margin: 20px 0; padding: 0; }");
            PrintEngineList.Add("\t\t#main_container > .b14 > ul > li > span{ color: " + html_uifc + "; }");
            PrintEngineList.Add("\t\t@media (max-width: 1260px){ #main_container > .b1, .b2, .b3, .b4, .b5, .b6, .b7, .b8, .b9, .b10, .b11, .b12, .b13, .b14{ width: 100%; } }");
            PrintEngineList.Add("\t\t@media (max-width: 735px){ #main_container > .b1, .b2, .b3, .b4, .b5, .b6, .b7, .b8, .b9, .b10, .b11, .b12, .b13, .b14{ padding: 10px; } #main_container > .ts_box_wrapper > h3{ text-align: center; } }");
            PrintEngineList.Add("\t\t@media (max-width: 495px){ #main_container > .ts_box_wrapper:nth-child(1){ flex-direction: column; justify-content: center; gap: 10px; } #main_container > .ts_box_wrapper:nth-child(1) > .ts_box_text{ text-align: center; align-items: center; } #main_container > .ts_box_wrapper > ul{ margin: 15px 0 0 25px; } #main_container > .ts_box_wrapper > h4{ margin: 13px 0 0 6px; } }");
            PrintEngineList.Add("\t</style>");
            PrintEngineList.Add("\t<link rel='icon' type='image/x-icon' href='" + print_html_glow_logo_url + "'>");
            PrintEngineList.Add("</head>");
            PrintEngineList.Add("<body>");
            // SCROOL TOP BUTTON
            PrintEngineList.Add($"\t<div class='ts_scroll_top' onclick='ts_scroll_to_up();' title='{TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_scroll_top_select"))}'><span class='ts_arrow_up'></span></div>");
            // SELECT WRAPPER
            PrintEngineList.Add($"\t<select title='{TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_html_partition_select"))}' class='ts_page_wrapper' id='ts_session_select'>");
            PrintEngineList.Add($"\t\t<option value='b1'>{TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_html_header_select"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b2'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_os"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b3'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_mb"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b4'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_cpu"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b5'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_ram"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b6'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_gpu"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b7'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_storage"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b8'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_network"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b9'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_usb"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b10'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_sound"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b11'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_battery"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b12'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_installed_drivers"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b13'>{TS_String_Encoder(software_lang.TSReadLangs("LeftMenu", "left_installed_services"))}</option>");
            PrintEngineList.Add($"\t\t<option value='b14'>{TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_html_footer_select"))}</option>");
            PrintEngineList.Add("\t</select>");
            // MAIN CONTAINER
            PrintEngineList.Add("\t<div id='main_container'>");
            // HEADER
            PrintEngineList.Add("\t\t<div class='b1 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<img src='" + print_html_glow_logo_url + "' alt='" + TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_logo")) + "'>");
            PrintEngineList.Add($"\t\t\t<div class='ts_box_text'>");
            PrintEngineList.Add($"\t\t\t\t<h2>{Application.ProductName} - {string.Format(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_report_title").ToUpper()), OS_ComputerName_V.Text)}</h2>");
            PrintEngineList.Add($"\t\t\t\t<span>{TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_process_time")) + " <label>" + DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss")}</label></span>");
            PrintEngineList.Add($"\t\t\t</div>");
            PrintEngineList.Add("\t\t</div>");
            // OS
            print_engine_progress_update(1);
            PrintEngineList.Add("\t\t<div class='b2 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_os"))}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemUser.Text}</span><span>{OS_SystemUser_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ComputerName.Text}</span><span>{OS_ComputerName_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SavedUser.Text}</span><span>{OS_SavedUser_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Name.Text}</span><span>{OS_Name_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Manufacturer.Text}</span><span>{OS_Manufacturer_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemVersion.Text}</span><span>{OS_SystemVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Build.Text}</span><span>{OS_Build_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemBuild.Text}</span><span>{OS_SystemBuild_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemArchitectural.Text}</span><span>{OS_SystemArchitectural_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Family.Text}</span><span>{OS_Family_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_DeviceID.Text}</span><span>{OS_DeviceID_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Serial.Text}</span><span>{OS_Serial_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Country.Text}</span><span>{OS_Country_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_TimeZone.Text}</span><span>{OS_TimeZone_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_CharacterSet.Text}</span><span>{OS_CharacterSet_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_EncryptionType.Text}</span><span>{OS_EncryptionType_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemRootIndex.Text}</span><span>{OS_SystemRootIndex_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemBuildPart.Text}</span><span>{OS_SystemBuildPart_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemTime.Text}</span><span>{OS_SystemTime_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Install.Text}</span><span>{OS_Install_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemWorkTime.Text}</span><span>{OS_SystemWorkTime_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_LastBootTime.Text}</span><span>{OS_LastBootTime_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemLastShutDown.Text}</span><span>{OS_SystemLastShutDown_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_PortableOS.Text}</span><span>{OS_PortableOS_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_FastBoot.Text}</span><span>{OS_FastBoot_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_MouseWheelStatus.Text}</span><span>{OS_MouseWheelStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ScrollLockStatus.Text}</span><span>{OS_ScrollLockStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_NumLockStatus.Text}</span><span>{OS_NumLockStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_CapsLockStatus.Text}</span><span>{OS_CapsLockStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_BootPartition.Text}</span><span>{OS_BootPartition_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemPartition.Text}</span><span>{OS_SystemPartition_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_WinPageFile.Text}</span><span>{OS_WinPageFile_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_TempWinPageFile.Text}</span><span>{OS_TempWinPageFile_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Hiberfil.Text}</span><span>{OS_Hiberfil_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_AVProgram.Text}</span><span>{OS_AVProgram_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_FirewallProgram.Text}</span><span>{OS_FirewallProgram_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_AntiSpywareProgram.Text}</span><span>{OS_AntiSpywareProgram_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_WinDefCoreIsolation.Text}</span><span>{OS_WinDefCoreIsolation_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_MSEdge.Text}</span><span>{OS_MSEdge_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_MSEdgeWebView.Text}</span><span>{OS_MSEdgeWebView_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_MSStoreVersion.Text}</span><span>{OS_MSStoreVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_WinKey.Text}</span><span>{OS_WinKey_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_WinActiveChannel.Text}</span><span>{OS_WinActiveChannel_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_NETFrameworkVersion.Text}</span><span>{OS_NETFrameworkVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Minidump.Text}</span><span>{OS_Minidump_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_BSODDate.Text}</span><span>{OS_BSODDate_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Wallpaper.Text}</span><span>{OS_Wallpaper_V.Text}</span></li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t</div>");
            // MB
            print_engine_progress_update(2);
            PrintEngineList.Add("\t\t<div class='b3 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_mb"))}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_MotherBoardName.Text}</span><span>{MB_MotherBoardName_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_MotherBoardMan.Text}</span><span>{MB_MotherBoardMan_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SystemModelMan.Text}</span><span>{MB_SystemModelMan_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SystemModelFamily.Text}</span><span>{MB_SystemModelFamily_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SystemModel.Text}</span><span>{MB_SystemModel_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_MotherBoardSerial.Text}</span><span>{MB_MotherBoardSerial_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SystemFamily.Text}</span><span>{MB_SystemFamily_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SystemSKU.Text}</span><span>{MB_SystemSKU_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_Chipset.Text}</span><span>{MB_Chipset_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_BiosManufacturer.Text}</span><span>{MB_BiosManufacturer_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_BiosDate.Text}</span><span>{MB_BiosDate_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_BiosVersion.Text}</span><span>{MB_BiosVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SmBiosVersion.Text}</span><span>{MB_SmBiosVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_BiosMode.Text}</span><span>{MB_BiosMode_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_LastBIOSTime.Text}</span><span>{MB_LastBIOSTime_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SecureBoot.Text}</span><span>{MB_SecureBoot_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMStatus.Text}</span><span>{MB_TPMStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMPhysicalVersion.Text}</span><span>{MB_TPMPhysicalVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMMan.Text}</span><span>{MB_TPMMan_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMManID.Text}</span><span>{MB_TPMManID_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMManVersion.Text}</span><span>{MB_TPMManVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMManFullVersion.Text}</span><span>{MB_TPMManFullVersion_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_TPMManPublisher.Text}</span><span>{MB_TPMManPublisher_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_Model.Text}</span><span>{MB_Model_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_PrimaryBusType.Text}</span><span>{MB_PrimaryBusType_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SecondaryBusType.Text}</span><span>{MB_SecondaryBusType_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_BiosMajorMinor.Text}</span><span>{MB_BiosMajorMinor_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_SMBiosMajorMinor.Text}</span><span>{MB_SMBiosMajorMinor_V.Text}</span></li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t</div>");
            // CPU
            print_engine_progress_update(3);
            PrintEngineList.Add("\t\t<div class='b4 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_cpu"))}</h3>");
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
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_SocketDefinition.Text}</span><span>{CPU_SocketDefinition_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Family.Text}</span><span>{CPU_Family_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_MicroCode.Text}</span><span>{CPU_MicroCode_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_PlatformFeature.Text}</span><span>{CPU_PlatformFeature_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_FeatureSet.Text}</span><span>{CPU_FeatureSet_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Revision.Text}</span><span>{CPU_Revision_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_OldRevision.Text}</span><span>{CPU_OldRevision_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Virtualization.Text}</span><span>{CPU_Virtualization_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_SerialName.Text}</span><span>{CPU_SerialName_V.Text}</span></li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t</div>");
            // RAM
            print_engine_progress_update(4);
            PrintEngineList.Add("\t\t<div class='b5 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_ram"))}</h3>");
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
                int ram_slot = RAM_SelectList.Items.Count;
                for (int rs = 1; rs <= ram_slot; rs++){
                    RAM_SelectList.SelectedIndex = rs - 1;

                    string[] ram_explode = RAM_SelectList.SelectedItem.ToString().Split('-');

                    PrintEngineList.Add($"\t\t\t\t<h4><span>{ram_explode[0]}</span>-<span>{ram_explode[1]}</span></h4>");
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
                RAM_SelectList.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add("\t\t</div>");
            // GPU
            print_engine_progress_update(5);
            PrintEngineList.Add("\t\t<div class='b6 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_gpu"))}</h3>");
            try{
                int gpu_amount = GPU_Select.Items.Count;
                for (int gpu_render = 1; gpu_render <= gpu_amount; gpu_render++){
                    GPU_Select.SelectedIndex = gpu_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_gpu_print")) + "</span><span>" + GPU_Select.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_Name.Text}</span><span>{GPU_Select.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_Manufacturer.Text}</span><span>{GPU_Manufacturer_V.Text}</span></li>");
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
                GPU_Select.SelectedIndex = 0;
            }catch (Exception){ }
            try{
                int screen_amount = GPU_MonitorSelectList.Items.Count;
                for (int sa = 1; sa <= screen_amount; sa++){
                    GPU_MonitorSelectList.SelectedIndex = sa - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{TS_String_Encoder(software_lang.TSReadLangs("Gpu_Content", "gpu_c_monitor_print")) + "</span><span>#" + sa}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorBounds.Text}</span><span>{GPU_MonitorBounds_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorWorking.Text}</span><span>{GPU_MonitorWorking_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorResLabel.Text}</span><span>{GPU_MonitorResLabel_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorVirtualRes.Text}</span><span>{GPU_MonitorVirtualRes_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_ScreenRefreshRate.Text}</span><span>{GPU_ScreenRefreshRate_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_ScreenBit.Text}</span><span>{GPU_ScreenBit_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorPrimary.Text}</span><span>{GPU_MonitorPrimary_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                PrintEngineList.Add($"\t\t\t\t<h4><span>{GPU_DesktopResolution.Text}</span><span>{GPU_DesktopResolution_V.Text}</span></h4>");
                GPU_MonitorSelectList.SelectedIndex = 0;
            }catch (Exception) { }
            PrintEngineList.Add("\t\t</div>");
            // STORAGE
            print_engine_progress_update(6);
            PrintEngineList.Add("\t\t<div class='b7 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_storage"))}</h3>");
            PrintEngineList.Add($"\t\t\t\t<h4><span>{DISK_TTLP_L1.Text}</span></h4>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_TTLP_P1_L1.Text}</span><span>{DISK_TTLP_P1_L2.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_TTLP_P2_L1.Text}</span><span>{DISK_TTLP_P2_L2.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_TTLP_P3_L1.Text}</span><span>{DISK_TTLP_P3_L2.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_TTLP_P4_L1.Text}</span><span>{DISK_TTLP_P4_L2.Text}</span></li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t\t<hr>");
            try{
                int disk_amount = DISK_CaptionList.Items.Count;
                for (int disk_render = 1; disk_render <= disk_amount; disk_render++){
                    DISK_CaptionList.SelectedIndex = disk_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{TS_String_Encoder(software_lang.TSReadLangs("StorageContent", "se_c_disk_print")) + "</span><span>" + DISK_CaptionList.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Caption.Text}</span><span>{DISK_CaptionList.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Model.Text}</span><span>{DISK_Model_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_Man.Text}</span><span>{DISK_Man_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_VolumeID.Text}</span><span>{DISK_VolumeID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_VolumeName.Text}</span><span>{DISK_VolumeName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_PhysicalName.Text}</span><span>{DISK_PhysicalName_V.Text}</span></li>");
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
                    DISK_CaptionList.SelectedIndex = c_index;
                    if (c_index == -1){
                        DISK_CaptionList.SelectedIndex = 0;
                    }
                }catch (Exception){
                    DISK_CaptionList.SelectedIndex = 0;
                }
            }catch (Exception){ }
            PrintEngineList.Add("\t\t</div>");
            // NETWORK
            print_engine_progress_update(7);
            PrintEngineList.Add("\t\t<div class='b8 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_network"))}</h3>");
            try{
                int net_amount = NET_ListNetwork.Items.Count;
                PrintEngineList.Add("\t\t\t<ul>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LT_Device.Text}</span><span>{NET_LT_Device_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LT_BandWidth.Text}</span><span>{NET_LT_BandWidth_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LT_DL1.Text}</span><span>{NET_LT_DL2.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LT_UL1.Text}</span><span>{NET_LT_UL2.Text}</span></li>");
                PrintEngineList.Add("\t\t\t</ul>");
                PrintEngineList.Add("\t\t\t<hr>");
                for (int net_render = 1; net_render <= net_amount; net_render++){
                    NET_ListNetwork.SelectedIndex = net_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{TS_String_Encoder(software_lang.TSReadLangs("Network_Content", "nk_c_network_print")) + "</span><span>" + NET_ListNetwork.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_ConnType.Text}</span><span>{NET_ListNetwork.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_MacAdress.Text}</span><span>{NET_MacAdress_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_NetMan.Text}</span><span>{NET_NetMan_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_ServiceName.Text}</span><span>{NET_ServiceName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_AdapterType.Text}</span><span>{NET_AdapterType_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_Physical.Text}</span><span>{NET_Physical_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_DeviceID.Text}</span><span>{NET_DeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_Guid.Text}</span><span>{NET_Guid_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_ConnectionType.Text}</span><span>{NET_ConnectionType_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_Dhcp_status.Text}</span><span>{NET_Dhcp_status_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_Dhcp_server.Text}</span><span>{NET_Dhcp_server_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_LocalConSpeed.Text}</span><span>{NET_LocalConSpeed_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_IPv4Adress.Text}</span><span>{NET_IPv4Adress_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{NET_IPv6Adress.Text}</span><span>{NET_IPv6Adress_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                PrintEngineList.Add("\t\t\t<ul>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_DNS1.Text}</span><span>{NET_DNS1_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_DNS2.Text}</span><span>{NET_DNS2_V.Text}</span></li>");
                PrintEngineList.Add("\t\t\t</ul>");
                NET_ListNetwork.SelectedIndex = 1;
            }catch (Exception){ }
            PrintEngineList.Add("\t\t</div>");
            // USB
            print_engine_progress_update(8);
            PrintEngineList.Add("\t\t<div class='b9 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_usb"))}</h3>");
            try{
                int usb_con_amount = USB_ConList.Items.Count;
                for (int usb_con_render = 1; usb_con_render <= usb_con_amount; usb_con_render++){
                    USB_ConList.SelectedIndex = usb_con_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_controller_print")) + "</span><span>" + USB_ConList.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_Con.Text}</span><span>{USB_ConList.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConName.Text}</span><span>{USB_ConName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConMan.Text}</span><span>{USB_ConMan_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConDeviceID.Text}</span><span>{USB_ConDeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConPNPDeviceID.Text}</span><span>{USB_ConPNPDeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConDeviceStatus.Text}</span><span>{USB_ConDeviceStatus_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                PrintEngineList.Add("\t\t\t<hr>");
                USB_ConList.SelectedIndex = 0;
            }catch (Exception) { }
            try{
                int usb_amount = USB_Select.Items.Count;
                for (int usb_render = 1; usb_render <= usb_amount; usb_render++){
                    USB_Select.SelectedIndex = usb_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{TS_String_Encoder(software_lang.TSReadLangs("Usb_Content", "usb_c_device_print")) + "</span><span>" + USB_Select.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_Device.Text}</span><span>{USB_Select.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_DeviceName.Text}</span><span>{USB_DeviceName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_DeviceID.Text}</span><span>{USB_DeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_PNPDeviceID.Text}</span><span>{USB_PNPDeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_DeviceStatus.Text}</span><span>{USB_DeviceStatus_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                USB_Select.SelectedIndex = 0;
            }catch (Exception) { }
            PrintEngineList.Add("\t\t</div>");
            // SOUND
            print_engine_progress_update(9);
            PrintEngineList.Add("\t\t<div class='b10 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_sound"))}</h3>");
            try{
                int sound_amount = SOUND_Select.Items.Count;
                for (int sound_render = 1; sound_render <= sound_amount; sound_render++){
                    SOUND_Select.SelectedIndex = sound_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{TS_String_Encoder(software_lang.TSReadLangs("Sound_Content", "sound_c_print")) + "</span><span>" + SOUND_Select.SelectedItem}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_Device.Text}</span><span>{SOUND_Select.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_DeviceName.Text}</span><span>{SOUND_DeviceName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_DeviceManufacturer.Text}</span><span>{SOUND_DeviceManufacturer_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_DeviceID.Text}</span><span>{SOUND_DeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_PNPDeviceID.Text}</span><span>{SOUND_PNPDeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{SOUND_DeviceStatus.Text}</span><span>{SOUND_DeviceStatus_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                SOUND_Select.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add("\t\t</div>");
            // BATTERY
            print_engine_progress_update(10);
            PrintEngineList.Add("\t\t<div class='b11 ts_box_wrapper'>");
            PowerStatus power_status = SystemInformation.PowerStatus;
            String battery_charging = power_status.BatteryChargeStatus.ToString();
            if (battery_charging == "NoSystemBattery"){
                PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_battery"))}</h3>");
                PrintEngineList.Add("\t\t\t<ul>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Status.Text}</span><span>{BATTERY_Status_V.Text}</span></li>");
                PrintEngineList.Add("\t\t\t</ul>");
            }else{
                PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_battery"))}</h3>");
                PrintEngineList.Add("\t\t\t<ul>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Status.Text}</span><span>{BATTERY_Status_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Model.Text}</span><span>{BATTERY_Model_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Name.Text}</span><span>{BATTERY_Name_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Voltage.Text}</span><span>{BATTERY_Voltage_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Type.Text}</span><span>{BATTERY_Type_V.Text}</span></li>");
                PrintEngineList.Add("\t\t\t</ul>");
            }
            PrintEngineList.Add("\t\t</div>");
            // OSD
            print_engine_progress_update(11);
            PrintEngineList.Add("\t\t<div class='b12 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_installed_drivers"))}</h3>");
            char[] split_osd = { ':' };
            string osd_header = TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_driver_sorting"));
            string[] osd_texts = osd_header.Split(split_osd);
            PrintEngineList.Add($"\t\t\t<h4><span>{osd_texts[0]}:</span><span>{osd_texts[1]}</span></h4>");
            try{
                PrintEngineList.Add("\t\t\t<ul>");
                for (int i = 0; i < OSD_DataMainTable.Rows.Count; i++){
                    PrintEngineList.Add($"\t\t\t\t<li>{OSD_DataMainTable.Rows[i].Cells[0].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[1].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[2].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[3].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[4].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[5].Value.ToString()}</li>");
                }
                PrintEngineList.Add("\t\t\t</ul>");
            }catch (Exception){ }
            PrintEngineList.Add($"\t\t\t<h4><span>{TS_String_Encoder(software_lang.TSReadLangs("Osd_Content", "osd_total_installed_driver_count"))}</span><span>{OSD_TYSS_V.Text}</span></h4>");
            PrintEngineList.Add("\t\t</div>");
            // SERVICE
            print_engine_progress_update(12);
            PrintEngineList.Add("\t\t<div class='b13 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{TS_String_Encoder(software_lang.TSReadLangs("Header", "header_installed_services"))}</h3>");
            char[] split_service = { ':' };
            string service_header = TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_service_sorting"));
            string[] service_texts = service_header.Split(split_service);
            PrintEngineList.Add($"\t\t\t<h4><span>{service_texts[0]}:</span><span>{service_texts[1]}</span></h4>");
            try{
                PrintEngineList.Add("\t\t\t<ul>");
                for (int i = 0; i < SERVICE_DataMainTable.Rows.Count; i++){
                    PrintEngineList.Add($"\t\t\t\t<li>{SERVICE_DataMainTable.Rows[i].Cells[0].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[1].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[2].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[3].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[4].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[5].Value.ToString()}</li>");
                }
                PrintEngineList.Add("\t\t\t</ul>");
            }catch (Exception){ }
            PrintEngineList.Add($"\t\t\t<h4><span>{TS_String_Encoder(software_lang.TSReadLangs("Services_Content", "ss_total_installed_service_count"))}</span><span>{SERVICE_TYS_V.Text}</span></h4>");
            PrintEngineList.Add("\t\t</div>");
            // FOOTER V1
            PrintEngineList.Add("\t\t<div class='b14 ts_box_wrapper'>");
            PrintEngineList.Add($"\t\t\t<h3>{string.Format(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_html_footer")), Application.ProductName.ToUpper())}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li>{Application.ProductName + " " + TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_version")) + " <span>" + TS_VersionEngine.TS_SofwareVersion(1, Program.ts_version_mode)}</span></li>");
            PrintEngineList.Add("\t\t\t\t<li>" + TS_SoftwareCopyrightDate.ts_scd_preloader + "</li>");
            PrintEngineList.Add($"\t\t\t\t<li>{TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_website")) + " " + "<a target='_blank' href='" + TS_LinkSystem.website_link + "'>" + TS_LinkSystem.website_link + "</a>"}</li>");
            PrintEngineList.Add($"\t\t\t\t<li>{TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_twitter")) + " " + "<a target='_blank' href='" + TS_LinkSystem.twitter_x_link + "'>" + TS_LinkSystem.twitter_x_link + "</a>"}</li>");
            PrintEngineList.Add($"\t\t\t\t<li>{TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_instagram")) + " " + "<a target='_blank' href='" + TS_LinkSystem.instagram_link + "'>" + TS_LinkSystem.instagram_link + "</a>"}</li>");
            PrintEngineList.Add($"\t\t\t\t<li>{TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_github")) + " " + "<a target='_blank' href='" + TS_LinkSystem.github_link + "'>" + TS_LinkSystem.github_link + "</a>"}</li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t</div>");
            // MAIN CONTAINER END
            PrintEngineList.Add("\t</div>");
            // SCROOL TOP SCRIPT
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
            print_engine_progress_update(13);
            SaveFileDialog save_engine = new SaveFileDialog{
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Title = Application.ProductName + " - " + TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_save_directory")),
                DefaultExt = "html",
                FileName = Application.ProductName + " - " + string.Format(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_save_name")), OS_ComputerName_V.Text),
                Filter = TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_save_html")) + " (*.html)|*.html"
            };
            if (save_engine.ShowDialog() == DialogResult.OK){
                string combinedText = String.Join(Environment.NewLine, PrintEngineList);
                File.WriteAllText(save_engine.FileName, combinedText);
                DialogResult glow_print_engine_query = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_save_success")) + "\n\n" + TS_String_Encoder(software_lang.TSReadLangs("PrintEngine", "pe_save_info_open")), Application.ProductName, save_engine.FileName));
                if (glow_print_engine_query == DialogResult.Yes){
                    Process.Start(save_engine.FileName);
                }
                PrintEngineList.Clear();
                save_engine.Dispose();
                print_after_mode();
            }else{
                PrintEngineList.Clear();
                save_engine.Dispose();
                print_after_mode();
            }
        }
        // SFC AND DISM AUTO TOOL
        // ======================================================================================================
        private void sFCandDISMAutoToolToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                GlowSFCandDISMAutoTool sfc_and_dism_tool = new GlowSFCandDISMAutoTool();
                string glow_tool_name = "glow_sfc_and_dism_tool";
                sfc_and_dism_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    sfc_and_dism_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_sfc_and_dism_tool"))));
                    Application.OpenForms[glow_tool_name].Activate();
                }
            }catch (Exception){ }
        }
        // CACHE CLEANUP TOOL
        // ======================================================================================================
        private void cacheCleaningToolToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                GlowCacheCleanupTool cache_cleanup_tool = new GlowCacheCleanupTool();
                string glow_tool_name = "glow_cache_cleanup_tool";
                cache_cleanup_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    cache_cleanup_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_cache_cleanup_tool"))));
                    Application.OpenForms[glow_tool_name].Activate();
                }
            }catch (Exception){ }
        }
        // CPU BENCH TOOL
        // ======================================================================================================
        private void benchCPUToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                GlowBenchCPU bench_cpu_tool = new GlowBenchCPU();
                string glow_tool_name = "glow_bench_cpu_tool";
                bench_cpu_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    bench_cpu_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_bench_cpu"))));
                    Application.OpenForms[glow_tool_name].Activate();
                }
            }catch (Exception){ }
        }
        // RAM BENCH TOOL
        // ======================================================================================================
        private void benchRAMToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                GlowBenchMemory glow_overlay_tool = new GlowBenchMemory();
                string glow_tool_name = "glow_bench_ram_tool";
                glow_overlay_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    glow_overlay_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_bench_ram"))));
                    Application.OpenForms[glow_tool_name].Activate();
                }
            }catch (Exception){ }

        }
        // DISK BENCH TOOL
        // ======================================================================================================
        private void benchDiskToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                GlowBenchDisk bench_disk_tool = new GlowBenchDisk();
                string glow_tool_name = "glow_bench_disk_tool";
                bench_disk_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    bench_disk_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_bench_disk"))));
                    Application.OpenForms[glow_tool_name].Activate();
                }
            }catch (Exception){ }
        }
        // SCREEN OVERLAY
        // ======================================================================================================
        private void screenOverlayToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                GlowOverlay glow_overlay_tool = new GlowOverlay();
                string glow_tool_name = "glow_screen_overlay_tool";
                glow_overlay_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    glow_overlay_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_overlay"))));
                    Application.OpenForms[glow_tool_name].Activate();
                }
            }catch (Exception){ }
        }
        // DNS TEST TOOL
        // ======================================================================================================
        private void dNSTestToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                GlowDNSTestTool glow_dns_test_tool = new GlowDNSTestTool();
                string glow_tool_name = "glow_dns_test_tool";
                glow_dns_test_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    glow_dns_test_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_dns_test_tool"))));
                    Application.OpenForms[glow_tool_name].Activate();
                }
            }catch (Exception){ }
        }
        // QUICK ACCESS TOOL
        // ======================================================================================================
        private void quickAccessToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                GlowQuickAccessTool glow_quick_access_tool = new GlowQuickAccessTool();
                string glow_tool_name = "glow_quick_access_tool";
                glow_quick_access_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    glow_quick_access_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_quick_access_tool"))));
                    Application.OpenForms[glow_tool_name].Activate();
                }
            }catch (Exception){ }
        }
        // NETWORK FIX TOOL
        // ======================================================================================================
        private void networkFixToolToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                GlowNetworkFixTool glow_network_fix_tool = new GlowNetworkFixTool();
                string glow_tool_name = "glow_network_fix_tool";
                glow_network_fix_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    glow_network_fix_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_network_fix_tool"))));
                    Application.OpenForms[glow_tool_name].Activate();
                }
            }catch (Exception){ }
        }
        // SHOW WIFI PASSWORD TOOL
        // ======================================================================================================
        private void showWiFiPasswordToolToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                GlowShowWiFiPasswordTool glow_show_wifi_password_tool = new GlowShowWiFiPasswordTool();
                string glow_tool_name = "glow_show_wifi_password_tool";
                glow_show_wifi_password_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    glow_show_wifi_password_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_show_wifi_password_tool"))));
                    Application.OpenForms[glow_tool_name].Activate();
                }
            }catch (Exception){ }
        }
        // MONITOR TEST TOOL
        // ======================================================================================================
        private void monitorDeadPixelTestToolStripMenuItem_Click(object sender, EventArgs e){
            monitor_engine_mode = 0;
            monitor_start_engine_pending();
        }
        private void monitorDynamicRangeTestToolStripMenuItem_Click(object sender, EventArgs e){
            monitor_engine_mode = 1;
            monitor_start_engine_pending();
        }
        private void monitor_start_engine_pending(){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                if (monitor_engine_mode == 0){
                    DialogResult epilepsy_warning = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(TS_String_Encoder(software_lang.TSReadLangs("MonitorTestTool", "mtt_epilepsy_warning")), "\n\n", TS_String_Encoder(software_lang.TSReadLangs("MonitorTestTool", "mtt_title_dead_pixel")), "\n\n"));
                    if (epilepsy_warning == DialogResult.Yes){
                        monitor_start_engine_select();
                    }
                }else{
                    monitor_start_engine_select();
                }  
            }catch (Exception){ }
        }
        private void monitor_start_engine_select(){
            TSGetLangs software_lang = new TSGetLangs(lang_path);
            GlowMonitorTestEngine glow_monitor_engine_test = new GlowMonitorTestEngine();
            string glow_tool_name = "";
            if (monitor_engine_mode == 0){
                glow_tool_name = "glow_monitor_test_engine_dead_pixel";
            }else if (monitor_engine_mode == 1){
                glow_tool_name = "glow_monitor_test_engine_dynamic_range";
            }
            glow_monitor_engine_test.Name = glow_tool_name;
            if (Application.OpenForms[glow_tool_name] == null){
                glow_monitor_engine_test.Show();
            }else{
                if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                    Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                }
                if (monitor_engine_mode == 0){
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "mtt_title_dead_pixel"))));
                }else if (monitor_engine_mode == 1){
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "mtt_title_dynamic_range"))));
                }
                Application.OpenForms[glow_tool_name].Activate();
            }
        }
        // GLOW ABOUT
        // ======================================================================================================
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(lang_path);
                GlowAbout glow_about = new GlowAbout();
                string glow_about_name = "glow_about";
                glow_about.Name = glow_about_name;
                if (Application.OpenForms[glow_about_name] == null){
                    glow_about.Show();
                }else{
                    if (Application.OpenForms[glow_about_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_about_name].WindowState = FormWindowState.Normal;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("HeaderToolsInfo", "header_tool_info_notification")), TS_String_Encoder(software_lang.TSReadLangs("HeaderMenu", "header_menu_about"))));
                    Application.OpenForms[glow_about_name].Activate();
                }
            }catch (Exception){ }
        }
        // GLOW EXIT
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
        private void software_exit(){ loop_status = false; CancelAllTasks(); Application.Exit(); }
        private void Glow_FormClosing(object sender, FormClosingEventArgs e){ software_exit(); }
    }
}