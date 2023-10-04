using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using Microsoft.Win32;
using System.Threading;
using System.Reflection;
using System.Management;
using System.Net.Sockets;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;
using System.Runtime.InteropServices;
// GLOW MODULES
using Glow.glow_tools;
using static Glow.GlowModules;

namespace Glow{
    public partial class Glow : Form{
        public Glow(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // GLOBAL VARIABLES
        public static string lang, lang_path;
        public  static int theme;
        // ======================================================================================================
        // VARIABLES
        int menu_btns = 1, menu_rp = 1, initial_status, hiding_status, hiding_mode_wrapper;
        string wp_rotate, wp_resoulation, preloader_os;
        bool loop_status = true, pe_loop_status = true, os_support_check_status;
        readonly string github_link = "https://github.com/roines45";
        // ======================================================================================================
        // PRINT ENGINE ASYNC STATUS
        int os_status = 0, mb_status = 0, cpu_status = 0, ram_status = 0, gpu_status = 0, disk_status = 0, network_status = 0,
        usb_status = 0, sound_status = 0, battery_status = 0, osd_status = 0, service_status = 0;
        // ======================================================================================================
        // COLOR MODES
        public static List<Color> ui_colors = new List<Color>();
        List<Color> btn_colors = new List<Color>(){ Color.FromArgb(235, 235, 235), Color.WhiteSmoke, Color.FromArgb(24, 24, 24), Color.FromArgb(31, 31, 31) };
        static List<Color> header_colors = new List<Color>();
        // ======================================================================================================
        // HEADER SETTINGS
        private class HeaderMenuColors : ToolStripProfessionalRenderer{
            public HeaderMenuColors() : base(new HeaderColors()){ }
            protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e){ e.ArrowColor = header_colors[1]; base.OnRenderArrow(e); }
        }
        private class HeaderColors : ProfessionalColorTable{
            public override Color MenuItemSelected{ get { return header_colors[0]; } }
            public override Color ToolStripDropDownBackground{ get { return header_colors[0]; } }
            public override Color ImageMarginGradientBegin{ get { return header_colors[0]; } }
            public override Color ImageMarginGradientEnd{ get { return header_colors[0]; } }
            public override Color ImageMarginGradientMiddle{ get { return header_colors[0]; } }
            public override Color MenuItemSelectedGradientBegin{ get { return header_colors[0]; } }
            public override Color MenuItemSelectedGradientEnd{ get { return header_colors[0]; } }
            public override Color MenuItemPressedGradientBegin{ get { return header_colors[0]; } }
            public override Color MenuItemPressedGradientMiddle{ get { return header_colors[0]; } }
            public override Color MenuItemPressedGradientEnd{ get { return header_colors[0]; } }
            public override Color MenuItemBorder{ get { return header_colors[0]; } }
            public override Color CheckBackground{ get { return header_colors[0]; } }
            public override Color ButtonSelectedBorder{ get { return header_colors[0]; } }
            public override Color CheckSelectedBackground{ get { return header_colors[0]; } }
            public override Color CheckPressedBackground{ get { return header_colors[0]; } }
            public override Color MenuBorder{ get { return header_colors[0]; } }
            public override Color SeparatorLight{ get { return header_colors[1]; } }
            public override Color SeparatorDark{ get { return header_colors[1]; } }
        }
        // ======================================================================================================
        // TOOLTIP SETTINGS
        private void MainToolTip_Draw(object sender, DrawToolTipEventArgs e){ e.DrawBackground(); e.DrawBorder(); e.DrawText(); }
        // ======================================================================================================
        // GLOW PRELOADER
        /*
            -- THEME --      |  -- LANGUAGE --   |   -- INITIAL MODE --    |   -- HIDING MODE --   |   -- OS SUPPORTED --
            0 = Dark Theme   |  Moved to         |   0 = Normal Windowed   |   0 = Off             |   True = Supported
            1 = Light Theme  |  GlowModules.cs   |   1 = FullScreen Mode   |   1 = On              |   False = Not Supported
        */
        private void glow_preloader(){
            try{
                // CHECK OS NAME
                string ui_lang = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName.Trim();
                ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject query_os in search_os.Get()){
                    var os_name = Convert.ToString(query_os["Caption"]).Trim();
                    string os_name_process = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(os_name);
                    //string os_name_process = "Windows 7";
                    // PRELOADER OS GLOBAL STRING SET VALUE
                    preloader_os = os_name_process;
                    string[] supported_os = { "Windows 10", "Windows 11" };
                    for (int i = 0; i <= supported_os.Length - 1; i++){
                        //Console.WriteLine(supported_os[i]);
                        if (os_name_process.Contains(supported_os[i])){
                            os_support_check_status = true;
                            break;
                        }else{
                            os_support_check_status = false;
                        }
                    }
                }
                // CHECK OS SUPPORTED MODE
                if (os_support_check_status == true){
                    // CHECK GLOW LANG FOLDER
                    if (Directory.Exists(glow_lf)){
                        // CHECK LANG FILES
                        int get_langs_file = Directory.GetFiles(glow_lf, "*.ini", SearchOption.AllDirectories).Length;
                        if (get_langs_file >= g_langs_count){
                            // CHECK SETTINGS
                            try{
                                if (File.Exists(glow_sf)){
                                    GetGlowSetting();
                                }else{
                                    // DETECT SYSTEM THEME
                                    GlowSettingsSave glow_settings_save = new GlowSettingsSave(glow_sf);
                                    string get_system_theme = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", "").ToString().Trim();
                                    glow_settings_save.GlowWriteSettings("GlowSettings", "ThemeStatus", get_system_theme);
                                    // DETECT SYSTEM LANG
                                    glow_settings_save.GlowWriteSettings("GlowSettings", "LanguageStatus", ui_lang);
                                    // SET INITIAL MODE
                                    glow_settings_save.GlowWriteSettings("GlowSettings", "InitialStatus", "0");
                                    // SET HIDING MODE
                                    glow_settings_save.GlowWriteSettings("GlowSettings", "HidingStatus", "0");
                                    GetGlowSetting();
                                }
                            }catch (Exception){ }
                        }else{
                            if (ui_lang == "tr"){
                                MessageBox.Show($"Dil dosyaları eksik veya bulunamadı.\n{Application.ProductName} kapatılıyor", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }else{
                                MessageBox.Show($"Language files missing or not found.\n{Application.ProductName} is shutting down.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            glow_exit();
                        }
                    }else{
                        if (ui_lang == "tr"){
                            MessageBox.Show($"G_langs klasörü bulunamadı.\n{Application.ProductName} kapatılıyor", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }else{
                            MessageBox.Show($"G_langs folder not found.\n{Application.ProductName} is shutting down.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        glow_exit();
                    }
                }else if (os_support_check_status == false){
                    if (ui_lang == "tr"){
                        MessageBox.Show($"{Application.ProductName}, Windows 10 altı hiçbir işletim sistemini desteklememektedir.\n\nKullandığınız işletim sistemi: {preloader_os}\n\n{Application.ProductName}'u kullanabilmek için işletim sisteminizi Windows 10 20H2 veya üzerine yükseltmeniz gerekiyor.\n\n{Application.ProductName} kapatılıyor.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }else{
                        MessageBox.Show($"{Application.ProductName}, does not support any operating system below Windows 10.\n\nThe operating system you are using: {preloader_os}\n\nYou need to upgrade to Windows 10 20H2 or higher to use {Application.ProductName}.\n\n{Application.ProductName} is shutting down.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    glow_exit();
                }
            }catch (Exception){ }
        }
        // ======================================================================================================
        // GLOW LOAD LANGS SETTINGS
        private void GetGlowSetting(){
            // INSTALLED DRIVERS
            OSD_DataMainTable.Columns.Add("osd_1", "x");
            OSD_DataMainTable.Columns.Add("osd_2", "x");
            OSD_DataMainTable.Columns.Add("osd_3", "x");
            OSD_DataMainTable.Columns.Add("osd_4", "x");
            OSD_DataMainTable.Columns.Add("osd_5", "x");
            foreach (DataGridViewColumn OSD_Column in OSD_DataMainTable.Columns){
                OSD_Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            // SERVICES
            SERVICE_DataMainTable.Columns.Add("ser_1", "x");
            SERVICE_DataMainTable.Columns.Add("ser_2", "x");
            SERVICE_DataMainTable.Columns.Add("ser_3", "x");
            SERVICE_DataMainTable.Columns.Add("ser_4", "x");
            SERVICE_DataMainTable.Columns.Add("ser_5", "x");
            foreach (DataGridViewColumn SERVICE_Column in SERVICE_DataMainTable.Columns){
                SERVICE_Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            // ALL DGV AND PANEL WIDTH
            int c1 = 120, c2 = 195, c3 = 90, c4 = 65, c5 = 95;
            // INSTALLED DRIVERS
            OSD_DataMainTable.Columns[0].Width = c1;
            OSD_DataMainTable.Columns[1].Width = c2;
            OSD_DataMainTable.Columns[2].Width = c3;
            OSD_DataMainTable.Columns[3].Width = c4;
            OSD_DataMainTable.Columns[4].Width = c5;
            OSD_DataMainTable.ClearSelection();
            // SERVICES
            SERVICE_DataMainTable.Columns[0].Width = c1;
            SERVICE_DataMainTable.Columns[1].Width = c2;
            SERVICE_DataMainTable.Columns[2].Width = c3;
            SERVICE_DataMainTable.Columns[3].Width = c4;
            SERVICE_DataMainTable.Columns[4].Width = c5;
            SERVICE_DataMainTable.ClearSelection();
            // OSD AND SERVICE CLEAR BTN DPI HEIGHT
            OSD_TextBoxClearBtn.Height = OSD_TextBox.Height;
            SERVICE_TextBoxClearBtn.Height = SERVICE_TextBox.Height;
            // TAB CONTROL DOUBLE BUFFERS
            typeof(TabControl).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, MainContent, new object[]{ true });
            // TLP DOUBLE BUFFER
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, OS_TLP, new object[]{ true });
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, MB_TLP, new object[]{ true });
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, CPU_TLP, new object[]{ true });
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, RAM_TLP, new object[]{ true });
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, GPU_TLP, new object[]{ true });
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DISK_TLP, new object[]{ true });
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, NET_TLP, new object[]{ true });
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, USB_TLP, new object[]{ true });
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, SOUND_TLP, new object[]{ true });
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, BATTERY_TLP, new object[]{ true });
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, OSD_TLP, new object[]{ true });
            typeof(TableLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, SERVICE_TLP, new object[]{ true });
            // DGV DOUBLE BUFFER
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, OSD_DataMainTable, new object[]{ true });
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, SERVICE_DataMainTable, new object[]{ true });
            // THEME - LANG - VIEW MODE PRELOADER
            GlowSettingsSave glow_read_settings = new GlowSettingsSave(glow_sf);
            string theme_mode = glow_read_settings.GlowReadSettings("GlowSettings", "ThemeStatus");
            switch (theme_mode){
                case "0":
                    color_mode(2);
                    darkThemeToolStripMenuItem.Checked = true;
                    break;
                default:
                    color_mode(1);
                    lightThemeToolStripMenuItem.Checked = true;
                    break;
            }
            string lang_mode = glow_read_settings.GlowReadSettings("GlowSettings", "LanguageStatus");
            switch (lang_mode){
                case "en":
                    lang_engine("en");
                    englishToolStripMenuItem.Checked = true;
                    break;
                case "tr":
                    lang_engine("tr");
                    turkishToolStripMenuItem.Checked = true;
                    break;
                default:
                    lang_engine("en");
                    englishToolStripMenuItem.Checked = true;
                    break;
            }
            string initial_mode = glow_read_settings.GlowReadSettings("GlowSettings", "InitialStatus");
            switch (initial_mode){
                case "0":
                    initial_status = 0;
                    windowedToolStripMenuItem.Checked = true;
                    //WindowState = FormWindowState.Normal;
                    break;
                case "1":
                    initial_status = 1;
                    fullScreenToolStripMenuItem.Checked = true;
                    WindowState = FormWindowState.Maximized;
                    break;
                default:
                    initial_status = 0;
                    windowedToolStripMenuItem.Checked = true;
                    //WindowState = FormWindowState.Normal;
                    break;
            }
            string hiding_mode = glow_read_settings.GlowReadSettings("GlowSettings", "HidingStatus");
            switch (hiding_mode){
                case "0":
                    hiding_status = 0;
                    hiding_mode_wrapper = 0;
                    hidingModeOffToolStripMenuItem.Checked = true;
                    break;
                case "1":
                    hiding_status = 1;
                    hiding_mode_wrapper = 1;
                    hidingModeOnToolStripMenuItem.Checked = true;
                    break;
                default:
                    hiding_status = 0;
                    hiding_mode_wrapper = 0;
                    hidingModeOffToolStripMenuItem.Checked = true;
                    break;
            }
            glow_load_tasks();
        }
        // ======================================================================================================
        // GLOW TASK ALL PROCESS
        private void glow_load_tasks(){
            // START OS TASK
            Task task_os = new Task(os);
            task_os.Start();
            // START MOTHERBOARD TASK
            Task task_mb = new Task(mb);
            task_mb.Start();
            // START CPU TASK
            Task task_cpu = new Task(cpu);
            task_cpu.Start();
            // START RAM TASK
            Task task_ram = new Task(ram);
            task_ram.Start();
            // START GPU TASK
            Task task_gpu = new Task(gpu);
            task_gpu.Start();
            // START DISK TASK
            Task task_disk = new Task(disk);
            task_disk.Start();
            // START NETWORK TASK
            Task task_network = new Task(network);
            task_network.Start();
            // START USB TASK
            Task task_usb = new Task(usb);
            task_usb.Start();
            // START SOUND TASK
            Task task_sound = new Task(sound);
            task_sound.Start();
            // START BATTERY TASK
            PowerStatus power_status = SystemInformation.PowerStatus;
            String battery_charging = power_status.BatteryChargeStatus.ToString();
            if (battery_charging == "NoSystemBattery"){
                battery_visible_off();
                // BATTERY PROCESS END ENABLED
                BATTERY_RotateBtn.Enabled = true;
                ((Control)BATTERY).Enabled = true;
                battery_status = 1;
                /*DESKTOP*/
            }else{
                battery_visible_on(); /*LAPTOP*/
                Task task_battery = new Task(battery);
                task_battery.Start();
                Task task_laptop_bg = new Task(laptop_bg_process);
                task_laptop_bg.Start();
            }
            // START OSD TASK
            Task task_osd = new Task(osd);
            task_osd.Start();
            // START GS SERVICES TASK
            Task task_gs_services = new Task(gs_services);
            task_gs_services.Start();
            // OS ASYNC BG TASK
            Task task_os_bg = new Task(os_bg_process);
            task_os_bg.Start();
            // CPU ASYNC BG TASK
            Task task_cpu_bg = new Task(processor_bg_process);
            task_cpu_bg.Start();
            // RAM ASYNC STARTER
            Task task_ram_bg = new Task(ram_bg_process);
            task_ram_bg.Start();
            // PRINT ENGINE ASYNC STARTER
            Task print_engine_bg = new Task(print_engine_async);
            print_engine_bg.Start();
            // ARROWS KEYS PRELOADER SET
            MainContent.SelectedTab = SOUND;
            MainContent.SelectedTab = OS;
        }
        // ======================================================================================================
        // GLOW LOAD
        private void Glow_Load(object sender, EventArgs e){
            // PRELOAD SETTINGS
            rotate_page_disabled();
            Text = Application.ProductName + " " + Application.ProductVersion.Substring(0, 4);
            HeaderMenu.Cursor = Cursors.Hand;
            // GLOW LAUNCH PROCESS
            glow_preloader();
        }
        // ======================================================================================================
        // ROTATE PAGE DISABLED PRELOADER
        private void rotate_page_disabled(){
            //OS_RotateBtn.Enabled = false;
            MB_RotateBtn.Enabled = false;
            CPU_RotateBtn.Enabled = false;
            RAM_RotateBtn.Enabled = false;
            GPU_RotateBtn.Enabled = false;
            DISK_RotateBtn.Enabled = false;
            NET_RotateBtn.Enabled = false;
            USB_RotateBtn.Enabled = false;
            SOUND_RotateBtn.Enabled = false;
            BATTERY_RotateBtn.Enabled = false;
            OSD_RotateBtn.Enabled = false;
            SERVICES_RotateBtn.Enabled = false;
            // DISABLE TAB PAGE
            //((Control)OS).Enabled = false;
            ((Control)MB).Enabled = false;
            ((Control)CPU).Enabled = false;
            ((Control)RAM).Enabled = false;
            ((Control)GPU).Enabled = false;
            ((Control)DISK).Enabled = false;
            ((Control)NETWORK).Enabled = false;
            ((Control)USB).Enabled = false;
            ((Control)SOUND).Enabled = false;
            ((Control)BATTERY).Enabled = false;
            ((Control)OSD).Enabled = false;
            ((Control)GSERVICE).Enabled = false;
        }
        // ======================================================================================================
        // OPERATING SYSTEM
        // BSoD Check Preloader
        string mdp_1 = @"C:\Windows\Minidump";
        string mdp_2 = @"C:\Windows\memory.dmp";
        List<string> minidump_files_list = new List<string>();
        List<string> minidump_files_date_list = new List<string>();
        private void os(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_cs = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystemProduct");
            ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher search_av = new ManagementObjectSearcher("root\\SecurityCenter2", "SELECT * FROM AntiVirusProduct");
            ManagementObjectSearcher search_fw = new ManagementObjectSearcher("root\\SecurityCenter2", "SELECT * FROM FirewallProduct");
            ManagementObjectSearcher search_as = new ManagementObjectSearcher("root\\SecurityCenter2", "SELECT * FROM AntiSpywareProduct");
            ManagementObjectSearcher search_desktop = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Desktop");
            try{
                // SYSTEM USER
                OS_SystemUser_V.Text = SystemInformation.UserName;
            }catch (Exception){ }
            try{
                // PC NAME
                OS_ComputerName_V.Text = SystemInformation.ComputerName;
            }catch (Exception){ }
            try{
                foreach (ManagementObject query_cs_rotate in search_cs.Get()){
                    // SYSTEM MODEL
                    OS_SystemModel_V.Text = Convert.ToString(query_cs_rotate["Name"]);
                }
            }catch (Exception){ }
            foreach (ManagementObject query_os_rotate in search_os.Get()){
                try{
                    // REGISTERED USER
                    string os_saved_user = Convert.ToString(query_os_rotate["RegisteredUser"]).Trim();
                    if (hiding_mode_wrapper != 1){
                        OS_SavedUser_V.Text = os_saved_user;
                    }else{
                        OS_SavedUser_V.Text = new string('*', os_saved_user.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
                    }
                }catch (Exception){ }
                try{
                    // OS NAME
                    OS_Name_V.Text = Convert.ToString(query_os_rotate["Caption"]);
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
                        OS_SystemVersion_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_1").Trim()));
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
                        OS_SystemBuild_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_1").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // OS ARCHITECTURE
                    string system_bit = Convert.ToString(query_os_rotate["OSArchitecture"]).Replace("bit", "");
                    OS_SystemArchitectural_V.Text = system_bit.Trim() + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_2").Trim())) + " - " + string.Format("(x{0})", system_bit.Trim());
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
                            OS_DeviceID_V.Text = new string('*', os_device_id_replace_2.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
                        }
                    }else{
                        OS_DeviceID_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_1").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // OS SERIAL
                    string os_serial = Convert.ToString(query_os_rotate["SerialNumber"]).Trim();
                    if (hiding_mode_wrapper != 1){
                        OS_Serial_V.Text = os_serial;
                    }else{
                        OS_Serial_V.Text = new string('*', os_serial.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
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
                    OS_CharacterSet_V.Text = os_code_set.ToString() + " - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_3").Trim()));
                }catch (Exception){ }
                try{
                    // OS ENCRYPTION BIT VALUE
                    OS_EncryptionType_V.Text = Convert.ToString(query_os_rotate["EncryptionLevel"]) + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_2").Trim()));
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
                    string last_bt_year = last_bt.Substring(0, 4);
                    string last_bt_month = last_bt.Substring(4, 2);
                    string last_bt_day = last_bt.Substring(6, 2);
                    string last_bt_hour = last_bt.Substring(8, 2);
                    string last_bt_minute = last_bt.Substring(10, 2);
                    string last_bt_second = last_bt.Substring(12, 2);
                    string last_bt_process = last_bt_day + "." + last_bt_month + "." + last_bt_year + " - " + last_bt_hour + ":" + last_bt_minute + ":" + last_bt_second;
                    OS_LastBootTime_V.Text = last_bt_process;
                }catch (Exception){ }
                try{
                    // OS SHUTDOWN TIME
                    string sd_time_path = @"System\CurrentControlSet\Control\Windows";
                    RegistryKey sd_time_key = Registry.LocalMachine.OpenSubKey(sd_time_path);
                    byte[] sd_time_val = (byte[])sd_time_key.GetValue("ShutdownTime");
                    long sd_time_as_long = BitConverter.ToInt64(sd_time_val, 0);
                    DateTime shut_down_time = DateTime.FromFileTime(sd_time_as_long);
                    OS_SystemLastShutDown_V.Text = shut_down_time.ToString("dd.MM.yyyy - HH:mm:ss");
                }catch (Exception){ }
                try{
                    // PORTABLE OS STATUS
                    bool system_portable_status = Convert.ToBoolean(query_os_rotate["PortableOperatingSystem"]);
                    if (system_portable_status == true){
                        OS_PortableOS_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_4").Trim()));
                    }else if (system_portable_status == false){
                        OS_PortableOS_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_5").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // BOOT PARTITION
                    object boot_device = query_os_rotate["BootDevice"];
                    string boot_device_1 = Convert.ToString(boot_device).Replace(@"\Device\", "");
                    string boot_device_2 = boot_device_1.Replace("HarddiskVolume", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_6").Trim())) + " - ");
                    OS_BootPartition_V.Text = boot_device_2.Trim();
                }catch (Exception){ }
                try{
                    // SYSTEM PARTITION
                    object system_device = query_os_rotate["SystemDevice"];
                    string system_device_1 = Convert.ToString(system_device).Replace(@"\Device\", "");
                    string system_device_2 = system_device_1.Replace("HarddiskVolume", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_7").Trim())) + " - ");
                    OS_SystemPartition_V.Text = system_device_2.ToString();
                }catch (Exception){ }
            }
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
                    av_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_18").Trim())));
                }
                int av_list_count = av_list.Count;
                switch (av_list_count){
                    case 1:
                        OS_AVProgram_V.Text = av_list[0];
                        break;
                    case 2:
                        OS_AVProgram_V.Text = av_list[0] + " - " + av_list[1];
                        break;
                    case 3:
                        OS_AVProgram_V.Text = av_list[0] + " - " + av_list[1] + " - " + av_list[2];
                        break;
                    case 4:
                        OS_AVProgram_V.Text = av_list[0] + " - " + av_list[1] + " - " + av_list[2] + " - " + av_list[3];
                        break;
                    case 5:
                        OS_AVProgram_V.Text = av_list[0] + " - " + av_list[1] + " - " + av_list[2] + " - " + av_list[3] + " - " + av_list[4];
                        break;
                    case 6:
                        OS_AVProgram_V.Text = av_list[0] + " - " + av_list[1] + " - " + av_list[2] + " - " + av_list[3] + " - " + av_list[4] + " - " + av_list[5];
                        break;
                }
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
                    fw_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_19").Trim())));
                }
                int fw_list_count = fw_list.Count;
                switch (fw_list_count){
                    case 1:
                        OS_FirewallProgram_V.Text = fw_list[0];
                        break;
                    case 2:
                        OS_FirewallProgram_V.Text = fw_list[0] + " - " + fw_list[1];
                        break;
                    case 3:
                        OS_FirewallProgram_V.Text = fw_list[0] + " - " + fw_list[1] + " - " + fw_list[2];
                        break;
                    case 4:
                        OS_FirewallProgram_V.Text = fw_list[0] + " - " + fw_list[1] + " - " + fw_list[2] + " - " + fw_list[3];
                        break;
                    case 5:
                        OS_FirewallProgram_V.Text = fw_list[0] + " - " + fw_list[1] + " - " + fw_list[2] + " - " + fw_list[3] + " - " + fw_list[4];
                        break;
                    case 6:
                        OS_FirewallProgram_V.Text = fw_list[0] + " - " + fw_list[1] + " - " + fw_list[2] + " - " + fw_list[3] + " - " + fw_list[4] + " - " + fw_list[5];
                        break;
                }
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
                    as_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_20").Trim())));
                }
                int as_list_count = as_list.Count;
                switch (as_list_count){
                    case 1:
                        OS_AntiSpywareProgram_V.Text = as_list[0];
                        break;
                    case 2:
                        OS_AntiSpywareProgram_V.Text = as_list[0] + " - " + as_list[1];
                        break;
                    case 3:
                        OS_AntiSpywareProgram_V.Text = as_list[0] + " - " + as_list[1] + " - " + as_list[2];
                        break;
                    case 4:
                        OS_AntiSpywareProgram_V.Text = as_list[0] + " - " + as_list[1] + " - " + as_list[2] + " - " + as_list[3];
                        break;
                    case 5:
                        OS_AntiSpywareProgram_V.Text = as_list[0] + " - " + as_list[1] + " - " + as_list[2] + " - " + as_list[3] + " - " + as_list[4];
                        break;
                    case 6:
                        OS_AntiSpywareProgram_V.Text = as_list[0] + " - " + as_list[1] + " - " + as_list[2] + " - " + as_list[3] + " - " + as_list[4] + " - " + as_list[5];
                        break;
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
                    OS_MinidumpOpen.Visible = true;
                    OS_Minidump_V.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_md_t").Trim())), minidump_files_list.Count);
                    MainToolTip.SetToolTip(OS_MinidumpOpen, string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_md_o").Trim())), mdp_1));
                    //Console.WriteLine($"{minidump_files_list.Count} dosya var");
                }else{
                    OS_MinidumpOpen.Visible = false;
                    OS_Minidump_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_md_f").Trim()));
                    OS_BSODDate_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_md_f").Trim()));
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
                    Task bsod_dynamic_time_bg = new Task(bsod_time_dynamic);
                    bsod_dynamic_time_bg.Start();
                }
            }catch (Exception){ }
            try{
                // GET WALLPAPER
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
                            if (wallpaper_size_x64 > 1024){
                                if ((wallpaper_size_x64 / 1024) > 1024){
                                    if ((wallpaper_size_x64 / 1024 / 1024) > 1024){
                                        // GB
                                        if (hiding_mode_wrapper != 1){
                                            OS_Wallpaper_V.Text = Path.GetFileName(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0.00} GB", wallpaper_size_x64 / 1024 / 1024 / 1024);
                                        }else{
                                            OS_Wallpaper_V.Text = new string('*', Path.GetFileName(get_wallpaper).Length) + Path.GetExtension(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0.00} GB", wallpaper_size_x64 / 1024 / 1024 / 1024) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
                                        }
                                    }else{
                                        // MB
                                        if (hiding_mode_wrapper != 1){
                                            OS_Wallpaper_V.Text = Path.GetFileName(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0.00} MB", wallpaper_size_x64 / 1024 / 1024);
                                        }else{
                                            OS_Wallpaper_V.Text = new string('*', Path.GetFileName(get_wallpaper).Length) + Path.GetExtension(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0.00} MB", wallpaper_size_x64 / 1024 / 1024) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
                                        }
                                    }
                                }else{
                                    // KB
                                    if (hiding_mode_wrapper != 1){
                                        OS_Wallpaper_V.Text = Path.GetFileName(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0.0} KB", wallpaper_size_x64 / 1024);
                                    }else{
                                        OS_Wallpaper_V.Text = new string('*', Path.GetFileName(get_wallpaper).Length) + Path.GetExtension(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0.0} KB", wallpaper_size_x64 / 1024) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
                                    }
                                }
                            }else{
                                // Byte
                                if (hiding_mode_wrapper != 1){
                                    OS_Wallpaper_V.Text = Path.GetFileName(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0.0} " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_byte").Trim())), wallpaper_size_x64);
                                }else{
                                    OS_Wallpaper_V.Text = new string('*', Path.GetFileName(get_wallpaper).Length) + Path.GetExtension(get_wallpaper) + " - " + wp_resoulation + " - " + string.Format("{0:0.0} " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_byte").Trim())), wallpaper_size_x64) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
                                }
                            }
                            // HOVER HIDING WRAPPER
                            if (hiding_mode_wrapper != 1){
                                MainToolTip.SetToolTip(OS_WallpaperOpen, string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_38").Trim())), wp_rotate));
                            }else{
                                MainToolTip.SetToolTip(OS_WallpaperOpen, string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_38").Trim())), new string('*', wp_rotate.Length) + Path.GetExtension(wp_rotate)));
                            }
                        }else{
                            OS_Wallpaper_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_8").Trim()));
                            OS_WallpaperOpen.Visible = false;
                        }
                    }
                }
                if (OS_Wallpaper_V.Text == "N/A" || OS_Wallpaper_V.Text.Trim() == "" || OS_Wallpaper_V.Text == string.Empty){
                    OS_Wallpaper_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_9").Trim()));
                    OS_WallpaperOpen.Visible = false;
                }
            }catch (Exception){ }
            // OS PROCESS END ENABLED
            //OS_RotateBtn.Enabled = true;
            //((Control)OS).Enabled = true;
            os_status = 1;
        }
        private void os_bg_process(){
            try{
                // DESCRIPTIVE
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                do{
                    if (loop_status == false){ break; }
                    foreach (ManagementObject query_os_rotate in search_os.Get()){
                        try{
                            // FREE VIRTUAL RAM
                            double free_virtual_ram = Convert.ToDouble(query_os_rotate["FreeVirtualMemory"]); // in kb
                            double total_virtual_memory = Convert.ToDouble(query_os_rotate["TotalVirtualMemorySize"]); // in kb
                            if (free_virtual_ram > 1024){
                                if ((free_virtual_ram / 1024) > 1024){
                                    // GB
                                    RAM_EmptyVirtualRam_V.Text = string.Format("{0:0.00} GB", free_virtual_ram / 1024 / 1024);
                                    RAM_UsageVirtualRam_V.Text = string.Format("{0:0.00} GB", (total_virtual_memory / 1024 / 1024) - (free_virtual_ram / 1024 / 1024));
                                }else{
                                    // MB
                                    RAM_EmptyVirtualRam_V.Text = string.Format("{0:0.00} MB", free_virtual_ram / 1024);
                                    RAM_UsageVirtualRam_V.Text = string.Format("{0:0.00} MB", (total_virtual_memory / 1024) - (free_virtual_ram / 1024));
                                }
                            }else{
                                // KB
                                RAM_EmptyVirtualRam_V.Text = string.Format("{0:0.0} KB", free_virtual_ram);
                                RAM_UsageVirtualRam_V.Text = string.Format("{0:0.0} KB", total_virtual_memory - free_virtual_ram);
                            }
                            // GET LAST BOOT
                            string last_boot = Convert.ToString(query_os_rotate["LastBootUpTime"]);
                            // Year - Month - Day - Hour - Minute - Second
                            int year = Convert.ToInt32(last_boot.Substring(0, 4));
                            int month = Convert.ToInt32(last_boot.Substring(4, 2));
                            int day = Convert.ToInt32(last_boot.Substring(6, 2));
                            int hour = Convert.ToInt32(last_boot.Substring(8, 2));
                            int minute = Convert.ToInt32(last_boot.Substring(10, 2));
                            int second = Convert.ToInt32(last_boot.Substring(12, 2));
                            // Convert DateTime
                            var boot_date_x64 = new DateTime(year, month, day, hour, minute, second);
                            // Current Date and Hour
                            var now_date = DateTime.Now;
                            // System Uptime
                            var system_uptime = now_date.Subtract(boot_date_x64);
                            string su_days = system_uptime.Days + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_10").Trim()));
                            string su_hours = system_uptime.Hours + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_11").Trim()));
                            string su_minutes = system_uptime.Minutes + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_12").Trim()));
                            string su_seconds = system_uptime.Seconds + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_13").Trim()));
                            string system_uptime_x64 = string.Format("{0}, {1}, {2}, {3}", su_days, su_hours, su_minutes, su_seconds);
                            OS_SystemWorkTime_V.Text = system_uptime_x64;
                            // SYSTEM INSTALL DATE
                            string os_id = Convert.ToString(query_os_rotate["InstallDate"]);
                            int os_id_year = Convert.ToInt32(os_id.Substring(0, 4));
                            string os_id_month = os_id.Substring(4, 2);
                            string os_id_day = os_id.Substring(6, 2);
                            int os_id_hour = Convert.ToInt32(os_id.Substring(8, 2));
                            int os_id_minute = Convert.ToInt32(os_id.Substring(10, 2));
                            int os_id_second = Convert.ToInt32(os_id.Substring(12, 2));
                            // Convert DateTime
                            var os_id_x64 = new DateTime(os_id_year, Convert.ToInt32(os_id_month), Convert.ToInt32(os_id_day), os_id_hour, os_id_minute, os_id_second);
                            // Current Date and Hour
                            var os_id_now_date = DateTime.Now;
                            // System Uptime
                            var os_id_x128 = os_id_now_date.Subtract(os_id_x64);
                            string os_id_days = os_id_x128.Days + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_10").Trim()));
                            string os_id_hours = os_id_x128.Hours + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_11").Trim()));
                            string os_id_minutes = os_id_x128.Minutes + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_12").Trim())) + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_14").Trim()));
                            string os_id_x256 = string.Format("{0}.{1}.{2} - {3}:{4}:{5} - ", os_id_day, os_id_month, os_id_year, os_id_hour, os_id_minute, os_id_second) + string.Format("( {0}, {1}, {2} )", os_id_days, os_id_hours, os_id_minutes);
                            OS_Install_V.Text = os_id_x256;
                        }catch (Exception){ }
                    }
                    // SYSTEM TIME
                    OS_SystemTime_V.Text = DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss");
                    // SYSTEM WORK TIME
                    try{
                        // MOUSE WHEEL SPEED
                        int mouse_wheel_speed = new Computer().Mouse.WheelScrollLines;
                        OS_MouseWheelStatus_V.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_15").Trim())), mouse_wheel_speed);
                    }catch (Exception){ }
                    try{
                        // SCROLL LOCK STATUS
                        bool scroll_lock_status = new Computer().Keyboard.ScrollLock;
                        if (scroll_lock_status == true){
                            OS_ScrollLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_16").Trim()));
                        }else if (scroll_lock_status == false){
                            OS_ScrollLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_17").Trim()));
                        }
                    }catch (Exception){ }
                    try{
                        // NUMLOCK STATUS
                        bool numlock_status = new Computer().Keyboard.NumLock;
                        if (numlock_status == true){
                            OS_NumLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_16").Trim()));
                        }else if (numlock_status == false){
                            OS_NumLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_17").Trim()));
                        }
                    }catch (Exception){ }
                    try{
                        // CAPSLOCK STATUS
                        bool capslock_status = new Computer().Keyboard.CapsLock;
                        if (capslock_status == true){
                            OS_CapsLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_16").Trim()));
                        }else if (capslock_status == false){
                            OS_CapsLockStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_17").Trim()));
                        }
                    }catch (Exception){ }
                    Thread.Sleep(1000 - DateTime.Now.Millisecond);
                }while (loop_status == true);
            }catch (Exception){ }
        }
        private void bsod_time_dynamic(){
            try{
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                DateTime last_bsod_date = Convert.ToDateTime(minidump_files_date_list[0]);
                // Year - Month - Day - Hour - Minute - Second
                var last_bsod_x64 = new DateTime(last_bsod_date.Year, last_bsod_date.Month, last_bsod_date.Day, last_bsod_date.Hour, last_bsod_date.Minute, last_bsod_date.Second);
                do{
                    if (loop_status == false){ break; }
                    var now_date = DateTime.Now;
                    var bsod_to_now = now_date.Subtract(last_bsod_x64);
                    string bsod_days = bsod_to_now.Days + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_10").Trim()));
                    string bsod_hours = bsod_to_now.Hours + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_11").Trim()));
                    string bsod_miniutes = bsod_to_now.Minutes + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_12").Trim()));
                    string bsod_seconds = bsod_to_now.Seconds + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_13").Trim())) + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_14").Trim()));
                    OS_BSODDate_V.Text = string.Format("{0}, {1}, {2}, {3}", bsod_days, bsod_hours, bsod_miniutes, bsod_seconds);
                    Thread.Sleep(1000 - DateTime.Now.Millisecond);
                }while (loop_status == true);
            }catch (Exception){ }
        }
        private void OS_MinidumpOpen_Click(object sender, EventArgs e){
            try{
                // OPEN MINIDUMP FOLDER
                string minidump_path = @"C:\Windows\Minidump";
                Process.Start(minidump_path);
            }catch (Exception){
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_md_e").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void GoWallpaperRotate_Click(object sender, EventArgs e){
            try{
                // OPEN WALLPAPER
                string wallpaper_start_path = string.Format("/select, \"{0}\"", wp_rotate.Trim().Replace("/", @"\"));
                ProcessStartInfo psi = new ProcessStartInfo("explorer.exe", wallpaper_start_path);
                Process.Start(psi);
            }catch (Exception){
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Os_Content", "os_c_21").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // MB
        // ======================================================================================================
        private void mb(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
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
                        MB_MotherBoardSerial_V.Text = new string('*', mb_serial.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
                    }
                }catch (Exception){ }
                try{
                    // MB VERSION
                    MB_Model_V.Text = Convert.ToString(query_bb_rotate["Version"]);
                }catch (Exception){ }
            }
            foreach (ManagementObject query_cs in search_cs.Get()){
                // SYSTEM FAMILY
                try{
                    string system_family = Convert.ToString(query_cs["SystemFamily"]).Trim();
                    string system_family_check = system_family.ToLower();
                    if (system_family_check != "default string"){
                        if (hiding_mode_wrapper != 1){
                            MB_SystemFamily_V.Text = system_family;
                        }else{
                            MB_SystemFamily_V.Text = new string('*', system_family.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
                        }
                    }else{
                        MB_SystemFamily_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_1").Trim()));
                    }
                }catch (Exception){ }
                // SYSTEM SKU
                try{
                    string system_sku = Convert.ToString(query_cs["SystemSKUNumber"]).Trim();
                    string system_sku_check = system_sku.ToLower();
                    if (system_sku_check != "default string"){
                        if (hiding_mode_wrapper != 1){
                            MB_SystemSKU_V.Text = system_sku;
                        }else{
                            MB_SystemSKU_V.Text = new string('*', system_sku.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
                        }
                    }else{
                        MB_SystemSKU_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_1").Trim()));
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
                            MB_PrimaryBusType_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_1").Trim()));
                        }
                    }catch (Exception){ }
                    try{
                        // SECONDARY BUS TYPE
                        string mb_secondary_bus_type = Convert.ToString(query_md_rotate["SecondaryBusType"]).Trim();
                        if (mb_secondary_bus_type != "" || mb_secondary_bus_type != string.Empty){
                            MB_SecondaryBusType_V.Text = mb_secondary_bus_type;
                        }else{
                            MB_SecondaryBusType_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_1").Trim()));
                        }
                    }catch (Exception){ }
                }
            }catch (Exception){ }
            try{
                // MB SECURE BOOT
                bool mb_secure_boot_status = Convert.ToBoolean(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecureBoot\State", "UEFISecureBootEnabled", ""));
                if (mb_secure_boot_status == true){
                    MB_SecureBoot_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_3").Trim()));
                }else if (mb_secure_boot_status == false){
                    MB_SecureBoot_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_4").Trim()));
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
                        MB_TPMStatus_V.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_5").Trim())), split_keywords[0], tpm_version);
                    }else{
                        MB_TPMStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // TPM PHYSICAL VERSION
                    string tpm_phy_version = Convert.ToString(query_tpm["PhysicalPresenceVersionInfo"]).Trim();
                    if (tpm_phy_version != "" || tpm_phy_version != string.Empty){
                        MB_TPMPhysicalVersion_V.Text = tpm_phy_version;
                    }else{
                        MB_TPMPhysicalVersion_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER ID TXT
                    string tpm_man_id_txt = Convert.ToString(query_tpm["ManufacturerIdTxt"]).Trim();
                    if (tpm_man_id_txt != "" || tpm_man_id_txt != string.Empty){
                        MB_TPMMan_V.Text = tpm_man_id_txt;
                    }else{
                        MB_TPMMan_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER ID
                    string tpm_man_id = Convert.ToString(query_tpm["ManufacturerId"]).Trim();
                    if (tpm_man_id != "" || tpm_man_id != string.Empty){
                        if (hiding_mode_wrapper != 1){
                            MB_TPMManID_V.Text = tpm_man_id;
                        }else{
                            MB_TPMManID_V.Text = new string('*', tpm_man_id.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
                        }
                    }else{
                        MB_TPMManID_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER VERSION
                    string tpm_man_version = Convert.ToString(query_tpm["ManufacturerVersion"]).Trim();
                    if (tpm_man_version != "" || tpm_man_version != string.Empty){
                        MB_TPMManVersion_V.Text = tpm_man_version;
                    }else{
                        MB_TPMManVersion_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER VERSION FULL
                    string tpm_man_version_full = Convert.ToString(query_tpm["ManufacturerVersionFull20"]).Trim();
                    if (tpm_man_version_full != "" || tpm_man_version_full != string.Empty){
                        MB_TPMManFullVersion_V.Text = tpm_man_version_full;
                    }else{
                        MB_TPMManFullVersion_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // TPM MANUFACTURER INFO
                    string tpm_man_version_info = Convert.ToString(query_tpm["ManufacturerVersionInfo"]).Trim();
                    if (tpm_man_version_info != "" || tpm_man_version_info != string.Empty){
                        MB_TPMManPublisher_V.Text = tpm_man_version_info;
                    }else{
                        MB_TPMManPublisher_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                    }
                }catch (Exception){ }
            }
            // TPM MODE CHECK WRAPPER
            try{
                if (MB_TPMStatus_V.Text == "N/A" || MB_TPMStatus_V.Text == string.Empty){
                    MB_TPMStatus_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                }
                if (MB_TPMPhysicalVersion_V.Text == "N/A" || MB_TPMPhysicalVersion_V.Text == string.Empty){
                    MB_TPMPhysicalVersion_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                }
                if (MB_TPMMan_V.Text == "N/A" || MB_TPMMan_V.Text == string.Empty){
                    MB_TPMMan_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                }
                if (MB_TPMManID_V.Text == "N/A" || MB_TPMManID_V.Text == string.Empty){
                    MB_TPMManID_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                }
                if (MB_TPMManVersion_V.Text == "N/A" || MB_TPMManVersion_V.Text == string.Empty){
                    MB_TPMManVersion_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                }
                if (MB_TPMManFullVersion_V.Text == "N/A" || MB_TPMManFullVersion_V.Text == string.Empty){
                    MB_TPMManFullVersion_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                }
                if (MB_TPMManPublisher_V.Text == "N/A" || MB_TPMManPublisher_V.Text == string.Empty){
                    MB_TPMManPublisher_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_6").Trim()));
                }
            }catch (Exception){ }
            try{
                // LAST BIOS TIME | POSTTime
                int last_bios_time = Convert.ToInt32(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\Session Manager\Power", "FwPOSTTime", ""));
                string last_bios_time_x64 = string.Format("{0:0.0}", TimeSpan.FromMilliseconds(last_bios_time).TotalSeconds).Replace(",", ".");
                MB_LastBIOSTime_V.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_7").Trim())), last_bios_time_x64);
            }catch (Exception){ }
            // MB PROCESS END ENABLED
            MB_RotateBtn.Enabled = true;
            ((Control)MB).Enabled = true;
            mb_status = 1;
        }
        // CPU
        // ======================================================================================================
        private void cpu(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_process = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject query_process_rotate in search_process.Get()){
                try{
                    // CPU NAME
                    CPU_Name_V.Text = Convert.ToString(query_process_rotate["Name"]).Trim();
                }catch (Exception){ }
                try{
                    // CPU MANUFACTURER
                    string cpu_man = Convert.ToString(query_process_rotate["Manufacturer"]).Trim();
                    bool cpu_man_intel = cpu_man.Contains("Intel");
                    bool cpu_man_amd = cpu_man.Contains("Advanced Micro");
                    // MessageBox.Show(cpu_man_intel.ToString());
                    // MessageBox.Show(cpu_man_amd.ToString());
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
                    string[] cpu_architectures = { "32 " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_1").Trim())) + " - (x86)", "MIPS", "ALPHA", "POWER PC", "ARM", "IA64", "64 " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_1").Trim())) + " - (x64)" };
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
                    double l2_size = Convert.ToDouble(query_process_rotate["L2CacheSize"]);
                    if (l2_size >= 1024){
                        CPU_L2_V.Text = (l2_size / 1024).ToString() + " MB";
                    }else{
                        CPU_L2_V.Text = l2_size.ToString() + " KB";
                    }
                }catch (Exception){ }
                try{
                    // L3 CACHE
                    double l3_size = Convert.ToDouble(query_process_rotate["L3CacheSize"]);
                    CPU_L3_V.Text = l3_size / 1024 + " MB";
                }catch (Exception){ }
                try{
                    // CPU CORES
                    CPU_CoreCount_V.Text = Convert.ToString(query_process_rotate["NumberOfCores"]);
                }catch (Exception){ }
                try{
                    // CPU LOGICAL CORES
                    string thread_count = Convert.ToString(query_process_rotate["ThreadCount"]);
                    if (thread_count == String.Empty || thread_count == ""){
                        CPU_LogicalCore_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_2").Trim()));
                    }else{
                        CPU_LogicalCore_V.Text = thread_count;
                    }
                }catch (Exception){ }
                try{
                    // CPU SOCKET
                    CPU_SocketDefinition_V.Text = Convert.ToString(query_process_rotate["SocketDesignation"]);
                }catch (Exception){ }
                try{
                    // CPU FAMILY
                    string cpu_description = Convert.ToString(query_process_rotate["Description"]);
                    string cpu_tanim = cpu_description.Replace("Family", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_3").Trim())));
                    string cpu_tanim_2 = cpu_tanim.Replace("Model", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_4").Trim())));
                    string cpu_tanim_3 = cpu_tanim_2.Replace("Stepping", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_5").Trim())));
                    string cpu_tanim_4 = cpu_tanim_3.Replace("64", " X64");
                    CPU_Family_V.Text = cpu_tanim_4;
                }catch (Exception){ }
                try{
                    // CPU VIRTUALIZATION
                    bool cpu_virtual_mod = Convert.ToBoolean(query_process_rotate["VirtualizationFirmwareEnabled"]);
                    if (cpu_virtual_mod == true){
                        CPU_Virtualization_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_6").Trim()));
                    }else if (cpu_virtual_mod == false){
                        CPU_Virtualization_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_7").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // CPU VIRTUALIZATION MONITOR EXTENSIONS
                    bool vm_monitor_ext = Convert.ToBoolean(query_process_rotate["VMMonitorModeExtensions"]);
                    if (vm_monitor_ext == true){
                        CPU_VMMonitorExtension_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_8").Trim()));
                    }else if (vm_monitor_ext == false){
                        CPU_VMMonitorExtension_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_9").Trim()));
                    }else{
                        CPU_VMMonitorExtension_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Cpu_Content", "cpu_c_10").Trim()));
                    }
                }catch (Exception){ }
                try{
                    // CPU SERIAL ID
                    string cpu_serial = Convert.ToString(query_process_rotate["ProcessorId"]).Trim();
                    if (hiding_mode_wrapper != 1){
                        CPU_SerialName_V.Text = cpu_serial;
                    }else{
                        CPU_SerialName_V.Text = new string('*', cpu_serial.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})";
                    }
                }catch (Exception){ }
            }
            ManagementObjectSearcher search_cm = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_CacheMemory WHERE Level = {3}");
            foreach (ManagementObject query_cm_rotate in search_cm.Get()){
                // L1 CACHE
                double l1_size = Convert.ToDouble(query_cm_rotate["MaxCacheSize"]);
                if (l1_size >= 1024){
                    CPU_L1_V.Text = (l1_size / 1024).ToString() + " MB";
                }else{
                    CPU_L1_V.Text = l1_size.ToString() + " KB";
                }
            }
            // CPU PROCESS END ENABLED
            CPU_RotateBtn.Enabled = true;
            ((Control)CPU).Enabled = true;
            cpu_status = 1;
        }
        private void processor_bg_process(){
            try{
                ManagementObjectSearcher search_process =  new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                do{
                    if (loop_status == false){ break; }
                    // CODE
                    foreach (ManagementObject query_process in search_process.Get()){
                        var process_count = Convert.ToInt32(query_process["NumberOfProcesses"]);
                        CPU_Process_V.Text = process_count.ToString();
                    }
                    Thread.Sleep(1000 - DateTime.Now.Millisecond);
                }while (loop_status == true);
            }catch (Exception){ }
        }
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
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_os = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher search_pm = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory");
            try{
                // TOTAL RAM
                ComputerInfo main_query = new ComputerInfo();
                ulong total_ram_x64_tick = ulong.Parse(main_query.TotalPhysicalMemory.ToString());
                double total_ram_isle = total_ram_x64_tick / (1024 * 1024);
                if (total_ram_isle > 1024){
                    if ((total_ram_isle / 1024) > 1024){
                        RAM_TotalRAM_V.Text = string.Format("{0:0.00} TB", total_ram_isle / 1024 / 1024);
                    }else{
                        RAM_TotalRAM_V.Text = string.Format("{0:0.00} GB", total_ram_isle / 1024);
                    }
                }else{
                    RAM_TotalRAM_V.Text = total_ram_isle.ToString() + " MB";
                }
            }catch (Exception){ }
            try{
                foreach (ManagementObject query_os_rotate in search_os.Get()){
                    // TOTAL VIRTUAL RAM
                    RAM_TotalVirtualRam_V.Text = string.Format("{0:0.00} GB", Convert.ToDouble(query_os_rotate["TotalVirtualMemorySize"]) / 1024 / 1024);
                }
            }catch (Exception){ }
            foreach (ManagementObject queryObj in search_pm.Get()){
                try{
                    // RAM AMOUNT
                    string ram_count = Convert.ToString(queryObj["BankLabel"]);
                    if (ram_count == "" || ram_count == string.Empty){
                        ram_slot_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_1").Trim())));
                    }else{
                        ram_slot_list.Add(ram_count);
                    }
                }catch (Exception){ }
                try{
                    // RAM SLOT COUNT
                    ram_slot_count.Add(Convert.ToString(queryObj["Capacity"]));
                    RAM_SlotStatus_V.Text = ram_slot_count.Count + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_2").Trim()));
                }catch (Exception){ }
                try{
                    // RAM CAPACITY
                    double ram_amount = Convert.ToDouble(queryObj["Capacity"]) / 1024 / 1024;
                    if (ram_amount > 1024){
                        ram_amount_list.Add(ram_amount / 1024 + " GB");
                    }else{
                        ram_amount_list.Add(ram_amount + " MB");
                    }
                    RAM_Amount_V.Text = ram_amount_list[0];
                }catch (Exception){ }
                try{
                    // MEMORY TYPE
                    int sm_bios_memory_type = Convert.ToInt32(queryObj["SMBIOSMemoryType"]);
                    int memory_type = Convert.ToInt32(queryObj["MemoryType"]);
                    if (sm_bios_memory_type == 1 || memory_type == 1){
                        ram_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_3").Trim())));
                    }else if (sm_bios_memory_type == 2 || memory_type == 2){
                        ram_type_list.Add("DRAM");
                    }else if (sm_bios_memory_type == 3 || memory_type == 3){
                        ram_type_list.Add("Synchronous DRAM");
                    }else if (sm_bios_memory_type == 4 || memory_type == 4){
                        ram_type_list.Add("Cache Ram");
                    }else if (sm_bios_memory_type == 5 || memory_type == 5){
                        ram_type_list.Add("EDO");
                    }else if (sm_bios_memory_type == 6 || memory_type == 6){
                        ram_type_list.Add("EDRAM");
                    }else if (sm_bios_memory_type == 7 || memory_type == 7){
                        ram_type_list.Add("VRAM");
                    }else if (sm_bios_memory_type == 8 || memory_type == 8){
                        ram_type_list.Add("SRAM");
                    }else if (sm_bios_memory_type == 9 || memory_type == 9){
                        ram_type_list.Add("RAM");
                    }else if (sm_bios_memory_type == 10 || memory_type == 10){
                        ram_type_list.Add("ROM");
                    }else if (sm_bios_memory_type == 11 || memory_type == 11){
                        ram_type_list.Add("FLASH");
                    }else if (sm_bios_memory_type == 12 || memory_type == 12){
                        ram_type_list.Add("EEPROM");
                    }else if (sm_bios_memory_type == 13 || memory_type == 13){
                        ram_type_list.Add("FEPROM");
                    }else if (sm_bios_memory_type == 14 || memory_type == 14){
                        ram_type_list.Add("EPROM");
                    }else if (sm_bios_memory_type == 15 || memory_type == 15){
                        ram_type_list.Add("CDRAM");
                    }else if (sm_bios_memory_type == 16 || memory_type == 16){
                        ram_type_list.Add("3DRAM");
                    }else if (sm_bios_memory_type == 17 || memory_type == 17){
                        ram_type_list.Add("SDRAM");
                    }else if (sm_bios_memory_type == 18 || memory_type == 18){
                        ram_type_list.Add("SGRAM");
                    }else if (sm_bios_memory_type == 19 || memory_type == 19){
                        ram_type_list.Add("RDRAM");
                    }else if (sm_bios_memory_type == 20 || memory_type == 20){
                        ram_type_list.Add("DDR");
                    }else if (sm_bios_memory_type == 21 || memory_type == 21){
                        ram_type_list.Add("DDR2");
                    }else if (sm_bios_memory_type == 22 || memory_type == 22){
                        ram_type_list.Add("DDR2 FB-DIMM");
                    }else if (sm_bios_memory_type == 24 || memory_type == 24){
                        ram_type_list.Add("DDR3");
                    }else if (sm_bios_memory_type == 25 || memory_type == 25){
                        ram_type_list.Add("FBD2");
                    }else if (sm_bios_memory_type == 26 || memory_type == 26){
                        ram_type_list.Add("DDR4");
                    }else if (sm_bios_memory_type == 27 || memory_type == 27 || sm_bios_memory_type == 28 || memory_type == 28){
                        ram_type_list.Add("DDR5");
                    }else if (sm_bios_memory_type == 0 || memory_type == 0){
                        ram_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_4").Trim())));
                    }
                    RAM_Type_V.Text = ram_type_list[0];
                }catch (Exception){ }
                try{
                    // RAM SPEED
                    ram_frekans_list.Add(Convert.ToInt32(queryObj["Speed"]) + " MHz");
                    RAM_Frequency_V.Text = ram_frekans_list[0];
                }catch (Exception){ }
                try{
                    // RAM VOLTAGE
                    string ram_voltaj = Convert.ToString(queryObj["ConfiguredVoltage"]);
                    if (ram_voltaj == "" || ram_voltaj == "0" || ram_voltaj == "0.0" || ram_voltaj == "0.00" || ram_voltaj == string.Empty){
                        ram_voltage_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_5").Trim())));
                    }else{
                        ram_voltage_list.Add(string.Format("{0:0.00} " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_6").Trim())), Convert.ToInt32(ram_voltaj) / 1000.0));
                    }
                    RAM_Volt_V.Text = ram_voltage_list[0];
                }catch (Exception){ }
                try{
                    // FORM FACTOR
                    int form_factor = Convert.ToInt32(queryObj["FormFactor"]);
                    if (form_factor == 8){
                        ram_form_factor.Add("DIMM");
                    }else if (form_factor == 12){
                        ram_form_factor.Add("SO-DIMM");
                    }else if (form_factor == 0){
                        ram_form_factor.Add("SO-DIMM / " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_7").Trim())));
                    }else{
                        ram_form_factor.Add(form_factor.ToString());
                    }
                    RAM_FormFactor_V.Text = ram_form_factor[0];
                }catch (Exception){ }
                try{
                    // RAM SERIAL
                    string ram_serial = Convert.ToString(queryObj["SerialNumber"]).Trim();
                    if (ram_serial == "00000000"){
                        ram_serial_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_8").Trim())));
                    }else if (ram_serial == "" || ram_serial == "Unknown" || ram_serial == "unknown" || ram_serial == string.Empty){
                        ram_serial_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_9").Trim())));
                    }else{
                        if (hiding_mode_wrapper != 1){
                            ram_serial_list.Add(ram_serial);
                        }else{
                            ram_serial_list.Add(new string('*', ram_serial.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})");
                        }
                    }
                    RAM_Serial_V.Text = ram_serial_list[0];
                }catch (Exception){ }
                try{
                    // RAM MAN
                    string ram_man = Convert.ToString(queryObj["Manufacturer"]).Trim();
                    if (ram_man == "" || ram_man == string.Empty || ram_man.ToLower() == "unknown"){
                        ram_manufacturer_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_10").Trim())));
                    }else if (ram_man == "017A"){
                        ram_manufacturer_list.Add("Apacer");
                    }else if (ram_man == "059B"){
                        ram_manufacturer_list.Add("Crucial");
                    }else if (ram_man == "04CD"){
                        ram_manufacturer_list.Add("G.Skill");
                    }else if (ram_man == "0198"){
                        ram_manufacturer_list.Add("HyperX");
                    }else if (ram_man == "029E"){
                        ram_manufacturer_list.Add("Corsair");
                    }else if (ram_man == "04CB"){
                        ram_manufacturer_list.Add("A-DATA");
                    }else if (ram_man == "00CE"){
                        ram_manufacturer_list.Add("Samsung");
                    }else{
                        ram_manufacturer_list.Add(ram_man);
                    }
                    RAM_Manufacturer_V.Text = ram_manufacturer_list[0];
                }catch (Exception){ }
                try{
                    // RAM BANK LABEL
                    string bank_label = Convert.ToString(queryObj["BankLabel"]);
                    if (bank_label == "" || bank_label == string.Empty){
                        ram_bank_label_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_11").Trim())));
                    }else{
                        ram_bank_label_list.Add(bank_label);
                    }
                    RAM_BankLabel_V.Text = ram_bank_label_list[0];
                }catch (Exception){ }
                try{
                    // RAM TOTAL WIDTH
                    string ram_data_width = Convert.ToString(queryObj["TotalWidth"]);
                    if (ram_data_width == "" || ram_data_width == string.Empty){
                        ram_data_width_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_12").Trim())));
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
                    if (part_number == "" || part_number == string.Empty){
                        ram_part_number_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_13").Trim())));
                    }else{
                        ram_part_number_list.Add(part_number);
                    }
                    RAM_PartNumber_V.Text = ram_part_number_list[0];
                }catch (Exception){ }
            }
            // RAM SELECT
            try{
                int ram_amount = ram_slot_list.Count;
                for (int rs = 1; rs <= ram_amount; rs++){
                    RAM_SelectList.Items.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Ram_Content", "ram_c_14").Trim())) + " #" + rs);
                }
                RAM_SelectList.SelectedIndex = 0;
            }catch (Exception){ }
            // RAM PROCESS END ENABLED
            RAM_RotateBtn.Enabled = true;
            ((Control)RAM).Enabled = true;
            ram_status = 1;
        }
        private void RAMSelectList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int ram_slot = RAM_SelectList.SelectedIndex;
                RAM_Amount_V.Text = ram_amount_list[ram_slot];
                RAM_Type_V.Text = ram_type_list[ram_slot];
                RAM_Frequency_V.Text = ram_frekans_list[ram_slot];
                RAM_Volt_V.Text = ram_voltage_list[ram_slot];
                RAM_FormFactor_V.Text = ram_form_factor[ram_slot];
                RAM_Serial_V.Text = ram_serial_list[ram_slot];
                RAM_Manufacturer_V.Text = ram_manufacturer_list[ram_slot];
                RAM_BankLabel_V.Text = ram_bank_label_list[ram_slot];
                RAM_DataWidth_V.Text = ram_data_width_list[ram_slot];
                RAM_BellekType_V.Text = bellek_type_list[ram_slot];
                RAM_PartNumber_V.Text = ram_part_number_list[ram_slot];
            }catch (Exception){ }
        }
        private void ram_bg_process(){
            try{
                ComputerInfo get_ram_info = new ComputerInfo();
                do{
                    if (loop_status == false){ break; }
                    ulong total_ram = ulong.Parse(get_ram_info.TotalPhysicalMemory.ToString());
                    double total_ram_x64 = total_ram / (1024 * 1024);
                    ulong usable_ram = ulong.Parse(get_ram_info.AvailablePhysicalMemory.ToString());
                    double usable_ram_x64 = usable_ram / (1024 * 1024);
                    double ram_process = total_ram_x64 - usable_ram_x64;
                    double usage_ram_percentage = (total_ram_x64 - usable_ram_x64) / total_ram_x64 * 100;
                    if (ram_process > 1024){
                        if ((ram_process / 1024) > 1024){
                            RAM_UsageRAMCount_V.Text = string.Format("{0:0.00} TB - ", ram_process / 1024 / 1024) + string.Format("%{0:0.0}", usage_ram_percentage);
                        }else{
                            RAM_UsageRAMCount_V.Text = string.Format("{0:0.00} GB - ", ram_process / 1024) + string.Format("%{0:0.0}", usage_ram_percentage);
                        }
                    }else{
                        RAM_UsageRAMCount_V.Text = ram_process.ToString() + " MB - " + string.Format("%{0:0.0}", usage_ram_percentage);
                    }
                    if (usable_ram_x64 > 1024){
                        if ((usable_ram_x64 / 1024) > 1024){
                            RAM_EmptyRamCount_V.Text = string.Format("{0:0.00} TB", usable_ram_x64 / 1024 / 1024);
                        }else{
                            RAM_EmptyRamCount_V.Text = string.Format("{0:0.00} GB", usable_ram_x64 / 1024);
                        }
                    }else{
                        RAM_EmptyRamCount_V.Text = usable_ram_x64.ToString() + " MB";
                    }
                    Thread.Sleep(1000 - DateTime.Now.Millisecond);
                }while (loop_status == true);
            }catch (Exception){ }
        }
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
        List<string> gpu_monitor_select_list = new List<string>();
        List<string> gpu_monitor_bounds_list = new List<string>();
        List<string> gpu_monitor_work_list = new List<string>();
        List<string> gpu_monitor_res_list = new List<string>();
        List<string> gpu_monitor_refresh_rate_list = new List<string>();
        List<string> gpu_monitor_virtual_res_list = new List<string>();
        List<string> gpu_monitor_primary_list = new List<string>();
        private void gpu(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
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
                    switch (gpu_status){
                        case 1:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_1").Trim())));
                            break;
                        case 2:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_2").Trim())));
                            break;
                        case 3:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_3").Trim())));
                            break;
                        case 4:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_4").Trim())));
                            break;
                        case 5:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_5").Trim())));
                            break;
                        case 6:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_6").Trim())));
                            break;
                        case 7:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_7").Trim())));
                            break;
                        case 8:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_8").Trim())));
                            break;
                        case 9:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_9").Trim())));
                            break;
                        case 10:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_10").Trim())));
                            break;
                        case 11:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_11").Trim())));
                            break;
                        case 12:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_12").Trim())));
                            break;
                        case 13:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_13").Trim())));
                            break;
                        case 14:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_14").Trim())));
                            break;
                        case 15:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_15").Trim())));
                            break;
                        case 16:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_16").Trim())));
                            break;
                        case 17:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_17").Trim())));
                            break;
                        case 18:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_18").Trim())));
                            break;
                        case 19:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_19").Trim())));
                            break;
                        case 20:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_20").Trim())));
                            break;
                        case 21:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_21").Trim())));
                            break;
                        default:
                            gpu_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_22").Trim())));
                            break;
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
                        gpu_device_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_23").Trim())));
                    }
                    GPU_DeviceID_V.Text = gpu_device_id_list[0];
                }catch (Exception){ }
                try{
                    // GPU DAC TYPE
                    string adaptor_dac_type = Convert.ToString(query_vc_rotate["AdapterDACType"]);
                    if (adaptor_dac_type == ""){
                        gpu_dac_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_24").Trim())));
                    }else{
                        if (adaptor_dac_type == "Integrated RAMDAC"){
                            gpu_dac_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_25").Trim())));
                        }else if (adaptor_dac_type == "Internal"){
                            gpu_dac_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_26").Trim())));
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
            }
            try{
                // ALL SCREEN DETECT
                foreach (var all_monitors in Screen.AllScreens){
                    string m_bounds = all_monitors.Bounds.ToString();
                    string m_working_area = all_monitors.WorkingArea.ToString();
                    string m_primary_screen = all_monitors.Primary.ToString();
                    gpu_monitor_select_list.Add(all_monitors.DeviceName.ToString());
                    string m_bounds_v2 = m_bounds.Replace("Width", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_27").Trim())));
                    string m_bounds_v3 = m_bounds_v2.Replace("Height", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_28").Trim())));
                    string m_bounds_v4 = m_bounds_v3.Replace("{", "");
                    string m_bounds_v5 = m_bounds_v4.Replace("}", "");
                    string m_bounds_v6 = m_bounds_v5.Replace(",", ", ");
                    string m_bounds_v7 = m_bounds_v6.Replace("=", " = ");
                    string m_working_area_v2 = m_working_area.Replace("Width", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_29").Trim())));
                    string m_working_area_v3 = m_working_area_v2.Replace("Height", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_30").Trim())));
                    string m_working_area_v4 = m_working_area_v3.Replace("{", "");
                    string m_working_area_v5 = m_working_area_v4.Replace("}", "");
                    string m_working_area_v6 = m_working_area_v5.Replace(",", ", ");
                    string m_working_area_v7 = m_working_area_v6.Replace("=", " = ");
                    string m_primary_screen_v2 = m_primary_screen.Replace("True", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_31").Trim())));
                    string m_primary_screen_v3 = m_primary_screen_v2.Replace("False", Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_32").Trim())));
                    gpu_monitor_bounds_list.Add(m_bounds_v7);
                    gpu_monitor_work_list.Add(m_working_area_v7);
                    gpu_monitor_primary_list.Add(m_primary_screen_v3);
                }
            }catch (Exception){ }
            try{
                foreach (Screen screen in Screen.AllScreens){
                    // SCREEN RESOLUTIONS
                    var dm = new DEVMODE { dmSize = (short)Marshal.SizeOf(typeof(DEVMODE)) };
                    EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm);
                    gpu_monitor_res_list.Add(dm.dmPelsWidth + " x " + dm.dmPelsHeight);
                    gpu_monitor_virtual_res_list.Add(screen.Bounds.Width + " x " + screen.Bounds.Height);
                    // SCREEN REFRESH RATE
                    var dm_2 = new DEVMODE { dmSize = (short)Marshal.SizeOf(typeof(DEVMODE)) };
                    EnumDisplaySettings(screen.DeviceName, ENUM_CURRENT_SETTINGS, ref dm_2);
                    gpu_monitor_refresh_rate_list.Add(dm_2.dmDisplayFrequency.ToString() + " Hz");
                }
            }catch (Exception){ }
            try{
                // MONITOR SELECT
                int monitor_amount = gpu_monitor_select_list.Count;
                for (int ma = 1; ma <= monitor_amount; ma++){
                    GPU_MonitorSelectList.Items.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu_Content", "gpu_c_33").Trim())) + " #" + ma);
                }
                GPU_MonitorSelectList.SelectedIndex = 0;
            }catch (Exception){ }
            // GPU SELECT
            try { GPU_Select.SelectedIndex = 0; }catch(Exception){ }
            // GPU PROCESS END ENABLED
            GPU_RotateBtn.Enabled = true;
            ((Control)GPU).Enabled = true;
            gpu_status = 1;
        }
        private void GPUSelect_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int gpu_select = GPU_Select.SelectedIndex;
                GPU_Manufacturer_V.Text = gpu_man_list[gpu_select];
                GPU_Version_V.Text = gpu_driver_version_list[gpu_select];
                GPU_DriverDate_V.Text = gpu_driver_date_list[gpu_select];
                GPU_Status_V.Text = gpu_status_list[gpu_select];
                GPU_DeviceID_V.Text = gpu_device_id_list[gpu_select];
                GPU_DacType_V.Text = gpu_dac_type_list[gpu_select];
                GPU_GraphicDriversName_V.Text = gpu_drivers_list[gpu_select];
                GPU_InfFileName_V.Text = gpu_inf_file_list[gpu_select];
                GPU_INFSectionFile_V.Text = gpu_inf_file_section_list[gpu_select];
            }catch(Exception){ }
        }
        private void MonitorSelectList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int monitor_select = GPU_MonitorSelectList.SelectedIndex;
                GPU_MonitorBounds_V.Text = gpu_monitor_bounds_list[monitor_select];
                GPU_MonitorWorking_V.Text = gpu_monitor_work_list[monitor_select];
                GPU_MonitorResLabel_V.Text = gpu_monitor_res_list[monitor_select];
                GPU_ScreenRefreshRate_V.Text = gpu_monitor_refresh_rate_list[monitor_select];
                GPU_MonitorVirtualRes_V.Text = gpu_monitor_virtual_res_list[monitor_select];
                GPU_MonitorPrimary_V.Text = gpu_monitor_primary_list[monitor_select];
            }catch(Exception){ }
        }
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
        List<string> disk_drive_compressed_list = new List<string>();
        private void disk(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                var get_drives = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
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
                                    DISK_CaptionList.Items.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
                                }
                            }catch (Exception){ }
                            try{
                                // DISK MANUFACTURER
                                List<string> disk_mfr = new List<string>(){ "acer", "adata", "a-data", "addlink", "alpin", "apacer", "apple",
                                "asus", "biostar", "codegen", "colorful", "corsair", "crucial", "ezcool", "geil", "gigabyte", "goodram",
                                "hi level", "hikvision", "hi-level", "hp", "intel", "intenso", "james donkey", "kingspec", "kingston", "kioxia",
                                "leven", "lexar", "micron", "mld", "msi", "mushkin", "neo forza", "neoforza", "netac", "patriot", "pioneer",
                                "pny", "samsung", "san disk", "sandisk", "seagate", "siliconpower", "team", "toshiba", "turbox", "wd", "western digital" };
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher disk_model_and_man_query = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                foreach (ManagementObject disk_model_and_man_search in disk_model_and_man_query.Get()){
                                    // DISK MODEL
                                    var disk_model = Convert.ToString(disk_model_and_man_search["Model"]);
                                    if (disk_model != "" || disk_model != string.Empty){
                                        disk_model_list.Add(disk_model);
                                    }else{
                                        disk_model_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
                                    }
                                    // DISK MAN
                                    var disk_man = Convert.ToString(disk_model_and_man_search["Manufacturer"]).Trim();
                                    if (disk_man == string.Empty || disk_man == ""){
                                        string disk_mod_lowercase = disk_model.ToLower().Replace("ı", "i");
                                        for (int i = 0; i <= disk_mfr.Count - 1; i++){
                                            bool target_detect_disk = disk_mod_lowercase.Contains(disk_mfr[i]);
                                            if (target_detect_disk == true){
                                                disk_man_list.Add(CultureInfo.CurrentCulture.TextInfo.ToTitleCase(disk_mfr[i]));
                                                //disk_man_list.Add(disk_mans[i].ToUpper());
                                                break;
                                            }
                                            if (i == disk_mfr.Count - 1){
                                                disk_man_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
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
                                    disk_volume_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
                                }
                                DISK_VolumeID_V.Text = disk_volume_id_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK VOLUME NAME
                                var disk_volume_name = Convert.ToString(logical_drive_info.Properties["VolumeName"].Value).Trim();
                                if (disk_volume_name != "" || disk_volume_name != string.Empty){
                                    disk_volume_name_list.Add(disk_volume_name);
                                }else{
                                    disk_volume_name_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_2").Trim())));
                                }
                                DISK_VolumeName_V.Text = disk_volume_name_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK PHYSICAL NAME
                                var disk_physical_name = Convert.ToString(drive_info.Properties["Name"].Value).Trim();
                                if (disk_physical_name != "" || disk_physical_name != string.Empty){
                                    disk_physical_name_list.Add(disk_physical_name);
                                }else{
                                    disk_physical_name_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
                                }
                                DISK_PhysicalName_V.Text = disk_physical_name_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK FIRMWARE VERSION
                                var disk_firmware_version = Convert.ToString(drive_info.Properties["FirmwareRevision"].Value).Trim();
                                if (disk_firmware_version != "" || disk_firmware_version != string.Empty){
                                    disk_firmware_list.Add(disk_firmware_version);
                                }else{
                                    disk_firmware_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
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
                                        disk_serial_list.Add(new string('*', disk_serial_number.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})");
                                    }
                                }else{
                                    disk_serial_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
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
                                        disk_volume_serial_list.Add(new string('*', disk_volume_serial_number.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})");
                                    }
                                }else{
                                    disk_volume_serial_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
                                }
                                DISK_VolumeSerial_V.Text = disk_volume_serial_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK TOTAL SIZE
                                var disk_total_size = Convert.ToDouble(logical_drive_info.Properties["Size"].Value); // in byte
                                if (disk_total_size > 1024){
                                    if ((disk_total_size / 1024) > 1024){
                                        if ((disk_total_size / 1024 / 1024) > 1024){
                                            if ((disk_total_size / 1024 / 1024 / 1024) > 1024){
                                                // TB
                                                disk_total_space_list.Add(string.Format("{0:0.00} TB", disk_total_size / 1024 / 1024 / 1024 / 1024));
                                            }else{
                                                // GB
                                                disk_total_space_list.Add(string.Format("{0:0.00} GB", disk_total_size / 1024 / 1024 / 1024));
                                            }
                                        }else{
                                            // MB
                                            disk_total_space_list.Add(string.Format("{0:0.00} MB", disk_total_size / 1024 / 1024));
                                        }
                                    }else{
                                        // KB
                                        disk_total_space_list.Add(string.Format("{0:0.0} KB", disk_total_size / 1024));
                                    }
                                }else{
                                    // Byte
                                    disk_total_space_list.Add(string.Format("{0:0.0} " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_byte").Trim())), disk_total_size));
                                }
                                DISK_Size_V.Text = disk_total_space_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK FREE SPACE
                                var disk_free_space = Convert.ToDouble(logical_drive_info.Properties["FreeSpace"].Value); // in byte
                                if (disk_free_space > 1024){
                                    if ((disk_free_space / 1024) > 1024){
                                        if ((disk_free_space / 1024 / 1024) > 1024){
                                            if ((disk_free_space / 1024 / 1024 / 1024) > 1024){
                                                // TB
                                                disk_free_space_list.Add(string.Format("{0:0.00} TB", disk_free_space / 1024 / 1024 / 1024 / 1024));
                                            }else{
                                                // GB
                                                disk_free_space_list.Add(string.Format("{0:0.00} GB", disk_free_space / 1024 / 1024 / 1024));
                                            }
                                        }else{
                                            // MB
                                            disk_free_space_list.Add(string.Format("{0:0.00} MB", disk_free_space / 1024 / 1024));
                                        }
                                    }else{
                                        // KB
                                        disk_free_space_list.Add(string.Format("{0:0.0} KB", disk_free_space / 1024));
                                    }
                                }else{
                                    // Byte
                                    disk_free_space_list.Add(string.Format("{0:0.0} " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_byte").Trim())), disk_free_space));
                                }
                                DISK_FreeSpace_V.Text = disk_free_space_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK FILE SYSTEM
                                var disk_file_system = Convert.ToString(logical_drive_info.Properties["FileSystem"].Value).Trim();
                                if (disk_file_system != "" || disk_file_system != string.Empty){
                                    disk_file_system_list.Add(disk_file_system);
                                }else{
                                    disk_file_system_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
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
                                            disk_formatting_system_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
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
                                                    disk_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_3").Trim())));
                                                    break;
                                                case "removable media":
                                                    disk_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_4").Trim())));
                                                    break;
                                                default:
                                                    disk_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
                                                    break;
                                            }
                                            break;
                                        case 3:
                                            // HDD
                                            disk_type_list.Add("HDD");
                                            break;
                                        case 4:
                                            // SSD
                                            disk_type_list.Add("SSD");
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
                                                    disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_5").Trim())));
                                                    break;
                                                case 4:
                                                    disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_6").Trim())));
                                                    break;
                                            }
                                            break;
                                        case "external hard disk media":
                                            disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_7").Trim())));
                                            break;
                                        case "removable media":
                                            disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_8").Trim())));
                                            break;
                                        default:
                                            disk_drive_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
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
                                    switch (di_query){
                                        case 0:
                                            disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
                                            break;
                                        case 1:
                                            disk_interface_list.Add("SCSI");
                                            break;
                                        case 2:
                                            disk_interface_list.Add("ATAPI");
                                            break;
                                        case 3:
                                            disk_interface_list.Add("ATA");
                                            break;
                                        case 4:
                                            disk_interface_list.Add("1394 - IEEE 1394");
                                            break;
                                        case 5:
                                            disk_interface_list.Add("SSA");
                                            break;
                                        case 6:
                                            disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_9").Trim())));
                                            break;
                                        case 7:
                                            disk_interface_list.Add("USB");
                                            break;
                                        case 8:
                                            disk_interface_list.Add("RAID");
                                            break;
                                        case 9:
                                            disk_interface_list.Add("iSCSI");
                                            break;
                                        case 10:
                                            disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_10").Trim())));
                                            break;
                                        case 11:
                                            disk_interface_list.Add("SATA");
                                            break;
                                        case 12:
                                            disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_11").Trim())));
                                            break;
                                        case 13:
                                            disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_12").Trim())));
                                            break;
                                        case 14:
                                            disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_13").Trim())));
                                            break;
                                        case 15:
                                            disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_14").Trim())));
                                            break;
                                        case 16:
                                            disk_interface_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_15").Trim())));
                                            break;
                                        case 17:
                                            disk_interface_list.Add("NVM-e");
                                            break;
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
                                    disk_partition_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_1").Trim())));
                                }
                                DISK_PartitionCount_V.Text = disk_partition_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK MEDIA STATUS
                                var disk_media_loaded = Convert.ToBoolean(drive_info.Properties["MediaLoaded"].Value);
                                if (disk_media_loaded == true){
                                    disk_media_loaded_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_16").Trim())));
                                }else if (disk_media_loaded == false){
                                    disk_media_loaded_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_17").Trim())));
                                }
                                DISK_MediaLoaded_V.Text = disk_media_loaded_list[0];
                            }catch (Exception){ }
                            try{
                                // DISK MEDIA STATUS
                                var disk_media_status = Convert.ToString(drive_info.Properties["Status"].Value).ToLower().Trim();
                                switch (disk_media_status){
                                    case "ok":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_18").Trim())));
                                        break;
                                    case "error":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_19").Trim())));
                                        break;
                                    case "degraded":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_20").Trim())));
                                        break;
                                    case "unknown":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_21").Trim())));
                                        break;
                                    case "pred fail":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_22").Trim())));
                                        break;
                                    case "starting":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_23").Trim())));
                                        break;
                                    case "stopping":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_24").Trim())));
                                        break;
                                    case "service":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_25").Trim())));
                                        break;
                                    case "stressed":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_26").Trim())));
                                        break;
                                    case "nonrecover":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_27").Trim())));
                                        break;
                                    case "no contact":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_28").Trim())));
                                        break;
                                    case "lost comm":
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_29").Trim())));
                                        break;
                                    default:
                                        disk_media_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_30").Trim())));
                                        break;
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
                                            disk_health_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_31").Trim())));
                                            break;
                                        case 1:
                                            disk_health_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_32").Trim())));
                                            break;
                                        case 2:
                                            disk_health_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_33").Trim())));
                                            break;
                                    }
                                    // BOOT DISK
                                    var disk_boot = Convert.ToBoolean(sdi_4["BootFromDisk"]);
                                    switch (disk_boot){
                                        case true:
                                            disk_boot_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_34").Trim())));
                                            break;
                                        case false:
                                            disk_boot_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_35").Trim())));
                                            break;
                                    }
                                    // BOOTABLE DISK
                                    var disk_bootable = Convert.ToBoolean(sdi_4["IsBoot"]);
                                    switch (disk_bootable){
                                        case true:
                                            disk_bootable_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_36").Trim())));
                                            break;
                                        case false:
                                            disk_bootable_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_37").Trim())));
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
                                    disk_drive_compressed_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_38").Trim())));   
                                }else if (disk_compressed_status == false){
                                    disk_drive_compressed_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("StorageContent", "se_c_39").Trim())));
                                }
                                DISK_DriveCompressed_V.Text = disk_drive_compressed_list[0];
                            }catch (Exception){ }
                            try{
                                // MB BIOS TYPE
                                var disk_index = Convert.ToString(drive_info.Properties["Index"].Value).Trim();
                                ManagementObjectSearcher search_bios_type = new ManagementObjectSearcher("root\\Microsoft\\Windows\\Storage", $"SELECT * FROM MSFT_Disk WHERE Number={disk_index}");
                                ManagementObjectSearcher search_windows_disk = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                                foreach (ManagementObject sbt in search_bios_type.Get()){
                                    foreach (ManagementObject swd in search_windows_disk.Get()){
                                        //Console.WriteLine(sbt["Model"].ToString());
                                        var detect_windows_disk = swd["SystemDrive"];
                                        var disk_volume_id = Convert.ToString(logical_drive_info.Properties["Name"].Value).Trim();
                                        if (disk_volume_id != "" || disk_volume_id != string.Empty){
                                            if (detect_windows_disk.ToString().Trim() == disk_volume_id.ToString().Trim()){
                                                //Console.WriteLine("||| " + sbt["Model"].ToString());
                                                var disk_style = Convert.ToInt32(sbt["PartitionStyle"]);
                                                if (disk_style == 1){
                                                    // MBR
                                                    MB_BiosMode_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_2").Trim())) + " (Legacy)";
                                                }else if (disk_style == 2){
                                                    // GPT
                                                    MB_BiosMode_V.Text = "UEFI";
                                                }else{
                                                    // NULL
                                                    MB_BiosMode_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Mb_Content", "mb_c_2").Trim())) + " (Legacy)";
                                                }
                                            }
                                        }
                                    }
                                }
                            }catch (Exception){ }
                        }
                    }
                }
            }catch (Exception){ }
            // SELECT DISK
            try { DISK_CaptionList.SelectedIndex = 0; }catch(Exception){ }
            // DISK PROCESS END ENABLED
            DISK_RotateBtn.Enabled = true;
            ((Control)DISK).Enabled = true;
            disk_status = 1;
        }
        private void DISK_CaptionList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int disk_percent = DISK_CaptionList.SelectedIndex;
                DISK_Model_V.Text = disk_model_list[disk_percent];
                DISK_Man_V.Text = disk_man_list[disk_percent];
                DISK_VolumeID_V.Text = disk_volume_id_list[disk_percent];
                DISK_VolumeName_V.Text = disk_volume_name_list[disk_percent];
                DISK_PhysicalName_V.Text = disk_physical_name_list[disk_percent];
                DISK_Firmware_V.Text = disk_firmware_list[disk_percent];
                DISK_Serial_V.Text = disk_serial_list[disk_percent];
                DISK_VolumeSerial_V.Text = disk_volume_serial_list[disk_percent];
                DISK_Size_V.Text = disk_total_space_list[disk_percent];
                DISK_FreeSpace_V.Text = disk_free_space_list[disk_percent];
                DISK_FileSystem_V.Text = disk_file_system_list[disk_percent];
                DISK_FormattingType_V.Text = disk_formatting_system_list[disk_percent];
                DISK_Type_V.Text = disk_type_list[disk_percent];
                DISK_DriveType_V.Text = disk_drive_type_list[disk_percent];
                DISK_InterFace_V.Text = disk_interface_list[disk_percent];
                DISK_PartitionCount_V.Text = disk_partition_list[disk_percent];
                DISK_MediaLoaded_V.Text = disk_media_loaded_list[disk_percent];
                DISK_MediaStatus_V.Text = disk_media_status_list[disk_percent];
                DISK_Health_V.Text = disk_health_status_list[disk_percent];
                DISK_Boot_V.Text = disk_boot_list[disk_percent];
                DISK_Bootable_V.Text = disk_bootable_list[disk_percent];
                DISK_DriveCompressed_V.Text = disk_drive_compressed_list[disk_percent];
            }catch (Exception){ }
        }
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
        private void network(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_na = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter");
            ManagementObjectSearcher search_nac = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject query_na_rotate in search_na.Get()){
                try{
                    // NET NAME
                    NET_ListNetwork.Items.Add(Convert.ToString(query_na_rotate["Name"]));
                }catch (Exception){ }
                try{
                    // MAC ADRESS
                    string mac_adress = Convert.ToString(query_na_rotate["MACAddress"]);
                    if (mac_adress == ""){
                        network_mac_adress_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_1").Trim())));
                    }else{
                        if (hiding_mode_wrapper != 1){
                            network_mac_adress_list.Add(mac_adress);
                        }else{
                            network_mac_adress_list.Add(new string('*', mac_adress.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})");
                        }
                    }
                    NET_MacAdress_V.Text = network_mac_adress_list[0];
                }catch (Exception){ }
                try{
                    // NET MAN
                    string net_man = Convert.ToString(query_na_rotate["Manufacturer"]);
                    if (net_man == ""){
                        network_man_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_2").Trim())));
                    }else{
                        network_man_list.Add(net_man);
                    }
                    NET_NetMan_V.Text = network_man_list[0];
                }catch (Exception){ }
                try{
                    // SERVICE NAME
                    string service_name = Convert.ToString(query_na_rotate["ServiceName"]);
                    if (service_name == ""){
                        network_service_name_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_3").Trim())));
                    }else{
                        network_service_name_list.Add(service_name);
                    }
                    NET_ServiceName_V.Text = network_service_name_list[0];
                }catch (Exception){ }
                try{
                    // NET ADAPTER TYPE
                    string adaptor_type = Convert.ToString(query_na_rotate["AdapterType"]);
                    if (adaptor_type == ""){
                        network_adaptor_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_4").Trim())));
                    }else{
                        network_adaptor_type_list.Add(adaptor_type);
                    }
                    NET_AdapterType_V.Text = network_adaptor_type_list[0];
                }catch (Exception){ }
                try{
                    // NET PHYSICAL
                    bool net_physical = Convert.ToBoolean(query_na_rotate["PhysicalAdapter"]);
                    if (net_physical == true){
                        network_physical_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_12").Trim())));
                    }else if (net_physical == false){
                        network_physical_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_13").Trim())));
                    }
                    NET_Physical_V.Text = network_physical_list[0];
                }catch (Exception) { }
                try{
                    // NETWORK DEVICE ID
                    string network_device_id = Convert.ToString(query_na_rotate["PNPDeviceID"]).Trim();
                    if (network_device_id != "" && network_device_id != string.Empty){
                        char[] split_char = { '\\' };
                        string[] network_device_split = network_device_id.Trim().Split(split_char);
                        network_device_id_list.Add($"{network_device_split[0]}\\{network_device_split[1]}");
                    }else{
                        network_device_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_5").Trim())));
                    }
                    NET_DeviceID_V.Text = network_device_id_list[0];
                }catch (Exception){ }
                try{
                    // NET GUID
                    string guid = Convert.ToString(query_na_rotate["GUID"]);
                    if (guid == ""){
                        network_guid_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_6").Trim())));
                    }else{
                        string net_guid_replace_1 = guid.Replace("{", string.Empty);
                        string net_guid_replace_2 = net_guid_replace_1.Replace("}", string.Empty);
                        if (hiding_mode_wrapper != 1){
                            network_guid_list.Add(net_guid_replace_2);
                        }else{
                            network_guid_list.Add(new string('*', net_guid_replace_2.Length) + $" ({Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_mode_on").Trim()))})");
                        }
                    }
                    NET_Guid_V.Text = network_guid_list[0];
                }catch (Exception){ }
                try{
                    // NET CONNECTION ID
                    string net_con_id = Convert.ToString(query_na_rotate["NetConnectionID"]);
                    if (net_con_id == ""){
                        network_connection_type_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_7").Trim())));
                    }else{
                        network_connection_type_list.Add(net_con_id);
                    }
                    NET_ConnectionType_V.Text = network_connection_type_list[0];
                }catch (Exception){ }
                try{
                    // MODEM CONNECT SPEED
                    string local_con_speed = Convert.ToString(query_na_rotate["Speed"]);
                    if (local_con_speed == "" || local_con_speed == "Unknown"){
                        network_connection_speed_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_8").Trim())));
                    }else{
                        int net_speed_cal = Convert.ToInt32(local_con_speed) / 1000 / 1000;
                        double net_speed_download_cal = Convert.ToDouble(net_speed_cal) / 8;
                        network_connection_speed_list.Add(net_speed_cal.ToString() + " Mbps - (" + string.Format("{0:0.0} MB/s)", net_speed_download_cal));
                    }
                    NET_LocalConSpeed_V.Text = network_connection_speed_list[0];
                }catch (Exception){ }
            }
            foreach (ManagementObject query_nac_rotate in search_nac.Get()){
                try{
                    // DHCP STATUS
                    bool dhcp_enabled = Convert.ToBoolean(query_nac_rotate["DHCPEnabled"]);
                    if (dhcp_enabled == true){
                        network_dhcp_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_9").Trim())));
                    }else if (dhcp_enabled == false){
                        network_dhcp_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_10").Trim())));
                    }
                    NET_Dhcp_status_V.Text = network_dhcp_status_list[0];
                }catch (Exception){ }
                try{
                    // DHCP SERVER STATUS
                    string dhcp_server = Convert.ToString(query_nac_rotate["DHCPServer"]);
                    if (dhcp_server == ""){
                        network_dhcp_server_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network_Content", "nk_c_11").Trim())));
                    }else{
                        network_dhcp_server_list.Add(dhcp_server);
                    }
                    NET_Dhcp_server_V.Text = network_dhcp_server_list[0];
                }catch (Exception){ }
            }
            // IPv4 And IPv6 Adress
            try{
                IPHostEntry ip_entry = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress[] adress_x64 = ip_entry.AddressList;
                NET_IPv4Adress_V.Text = adress_x64[adress_x64.Length - 1].ToString();
                if (adress_x64[0].AddressFamily == AddressFamily.InterNetworkV6){
                    NET_IPv6Adress_V.Text = adress_x64[0].ToString();
                }
            }catch (Exception){ }
            // NETWORK SELECT
            try{ NET_ListNetwork.SelectedIndex = 1; }catch (Exception){ NET_ListNetwork.SelectedIndex = 0; }
            // NETWORK PROCESS END ENABLED
            NET_RotateBtn.Enabled = true;
            ((Control)NETWORK).Enabled = true;
            network_status = 1;
        }
        private void listnetwork_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int network_select = NET_ListNetwork.SelectedIndex;
                NET_MacAdress_V.Text = network_mac_adress_list[network_select];
                NET_NetMan_V.Text = network_man_list[network_select];
                NET_ServiceName_V.Text = network_service_name_list[network_select];
                NET_AdapterType_V.Text = network_adaptor_type_list[network_select];
                NET_Physical_V.Text = network_physical_list[network_select];
                NET_DeviceID_V.Text = network_device_id_list[network_select];
                NET_Guid_V.Text = network_guid_list[network_select];
                NET_ConnectionType_V.Text = network_connection_type_list[network_select];
                NET_Dhcp_status_V.Text = network_dhcp_status_list[network_select];
                NET_Dhcp_server_V.Text = network_dhcp_server_list[network_select];
                NET_LocalConSpeed_V.Text = network_connection_speed_list[network_select];
            }catch(Exception){ }
        }
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
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_usb_controller = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_USBController");
            ManagementObjectSearcher search_usb_hub = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_USBHub");
            foreach (ManagementObject query_usb_con in search_usb_controller.Get()){
                // USB CON CAPTION
                try{
                    string usb_con_caption = Convert.ToString(query_usb_con["Caption"]).Trim();
                    if (usb_con_caption != "" && usb_con_caption != string.Empty){
                        USB_ConList.Items.Add(usb_con_caption);
                    }else{
                        USB_ConList.Items.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_1").Trim())));
                    }
                }catch (Exception){ }
                // USB CON NAME
                try{
                    string usb_con_name = Convert.ToString(query_usb_con["Name"]);
                    if (usb_con_name != "" && usb_con_name != string.Empty){
                        usb_con_name_list.Add(usb_con_name);
                    }else{
                        usb_con_name_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_2").Trim())));
                    }
                    USB_ConName_V.Text = usb_con_name_list[0];
                }catch (Exception){ }
                // USB CON MANUFACTURER
                try{
                    string usb_con_man = Convert.ToString(query_usb_con["Manufacturer"]);
                    if (usb_con_man != "" && usb_con_man != string.Empty){
                        usb_con_manufacturer_list.Add(usb_con_man);
                    }else{
                        usb_con_manufacturer_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_2").Trim())));
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
                        usb_con_device_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_2").Trim())));
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
                        usb_con_pnp_device_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_2").Trim())));
                    }
                    USB_ConPNPDeviceID_V.Text = usb_con_pnp_device_id_list[0];
                }catch (Exception){ }
                // USB CON DEVICE STATUS
                try{
                    string usb_con_device_status = Convert.ToString(query_usb_con["Status"]).Trim().ToLower();
                    if (usb_con_device_status != "" && usb_con_device_status != string.Empty){
                        switch (usb_con_device_status){
                            case "ok":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_3").Trim())));
                                break;
                            case "error":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_4").Trim())));
                                break;
                            case "degraded":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_5").Trim())));
                                break;
                            case "unknown":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_6").Trim())));
                                break;
                            case "pred fail":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_7").Trim())));
                                break;
                            case "starting":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_8").Trim())));
                                break;
                            case "stopping":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_9").Trim())));
                                break;
                            case "service":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_10").Trim())));
                                break;
                            case "stressed":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_11").Trim())));
                                break;
                            case "nonrecover":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_12").Trim())));
                                break;
                            case "no contact":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_13").Trim())));
                                break;
                            case "lost comm":
                                usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_14").Trim())));
                                break;
                        }
                    }else{
                        usb_con_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_2").Trim())));
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
                        USB_Select.Items.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_1").Trim())));
                    }
                }catch (Exception){ }
                // USB NAME
                try{
                    string usb_name = Convert.ToString(query_usb["Name"]);
                    if (usb_name != "" && usb_name != string.Empty){
                        usb_device_name_list.Add(usb_name);
                    }else{
                        usb_device_name_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_2").Trim())));
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
                        usb_device_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_2").Trim())));
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
                        usb_pnp_device_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_2").Trim())));
                    }
                    USB_PNPDeviceID_V.Text = usb_pnp_device_id_list[0];
                }catch (Exception){ }
                // USB DEVICE STATUS
                try{
                    string usb_device_status = Convert.ToString(query_usb["Status"]).Trim().ToLower();
                    if (usb_device_status != "" && usb_device_status != string.Empty){
                        switch (usb_device_status){
                            case "ok":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_3").Trim())));
                            break;
                            case "error":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_4").Trim())));
                                break;
                            case "degraded":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_5").Trim())));
                                break;
                            case "unknown":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_6").Trim())));
                                break;
                            case "pred fail":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_7").Trim())));
                                break;
                            case "starting":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_8").Trim())));
                                break;
                            case "stopping":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_9").Trim())));
                                break;
                            case "service":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_10").Trim())));
                                break;
                            case "stressed":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_11").Trim())));
                                break;
                            case "nonrecover":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_12").Trim())));
                                break;
                            case "no contact":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_13").Trim())));
                                break;
                            case "lost comm":
                                usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_14").Trim())));
                                break;
                        }
                    }else{
                        usb_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb_Content", "usb_c_2").Trim())));
                    }
                    USB_DeviceStatus_V.Text = usb_device_status_list[0];
                }catch (Exception){ }
            }
            try{ USB_ConList.SelectedIndex = 0; }catch (Exception){ }
            try{ USB_Select.SelectedIndex = 0; }catch (Exception){ }
            // USB PROCESS END ENABLED
            USB_RotateBtn.Enabled = true;
            ((Control)USB).Enabled = true;
            usb_status = 1;
        }
        private void USB_ConList_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int usb_con_select = USB_ConList.SelectedIndex;
                USB_ConName_V.Text = usb_con_name_list[usb_con_select];
                USB_ConMan_V.Text = usb_con_manufacturer_list[usb_con_select];
                USB_ConDeviceID_V.Text = usb_con_device_id_list[usb_con_select];
                USB_ConPNPDeviceID_V.Text = usb_con_pnp_device_id_list[usb_con_select];
                USB_ConDeviceStatus_V.Text = usb_con_device_status_list[usb_con_select];
            }catch (Exception){ }
        }
        private void USB_Select_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int usb_select = USB_Select.SelectedIndex;
                USB_DeviceName_V.Text = usb_device_name_list[usb_select];
                USB_DeviceID_V.Text = usb_device_id_list[usb_select];
                USB_PNPDeviceID_V.Text = usb_pnp_device_id_list[usb_select];
                USB_DeviceStatus_V.Text = usb_device_status_list[usb_select];
            }catch(Exception){ }
        }
        // SOUND
        // ======================================================================================================
        List<string> sound_device_name_list = new List<string>();
        List<string> sound_device_manufacturer_list = new List<string>();
        List<string> sound_device_id_list = new List<string>();
        List<string> sound_pnp_device_id_list = new List<string>();
        List<string> sound_device_status_list = new List<string>();
        private void sound(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            ManagementObjectSearcher search_sound = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_SoundDevice");
            foreach (ManagementObject query_sound in search_sound.Get()){
                // SOUND DEVICE CAPTION
                try{
                    string sound_caption = Convert.ToString(query_sound["Caption"]);
                    if (sound_caption != "" && sound_caption != string.Empty){
                        SOUND_Select.Items.Add(sound_caption);
                    }else{
                        SOUND_Select.Items.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_2").Trim())));
                    }
                }catch (Exception){ }
                // SOUND DEVICE NAME
                try{
                    string sound_name = Convert.ToString(query_sound["Name"]);
                    if (sound_name != "" && sound_name != string.Empty){
                        sound_device_name_list.Add(sound_name);
                    }else{
                        sound_device_name_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_2").Trim())));
                    }
                    SOUND_DeviceName_V.Text = sound_device_name_list[0];
                }catch (Exception){ }
                // SOUND DEVICE MANUFACTURER
                try{
                    string sound_manfuacturer = Convert.ToString(query_sound["Manufacturer"]);
                    if (sound_manfuacturer != "" && sound_manfuacturer != string.Empty){
                        sound_device_manufacturer_list.Add(sound_manfuacturer);
                    }else{
                        sound_device_manufacturer_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_2").Trim())));
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
                        sound_device_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_2").Trim())));
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
                        sound_pnp_device_id_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_2").Trim())));
                    }
                    SOUND_PNPDeviceID_V.Text = sound_pnp_device_id_list[0];
                }catch (Exception){ }
                // SOUND DEVICE STATUS
                try{
                    string sound_device_status = Convert.ToString(query_sound["Status"]).Trim().ToLower();
                    if (sound_device_status != "" && sound_device_status != string.Empty){
                        switch (sound_device_status){
                            case "ok":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_3").Trim())));
                                break;
                            case "error":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_4").Trim())));
                                break;
                            case "degraded":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_5").Trim())));
                                break;
                            case "unknown":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_6").Trim())));
                                break;
                            case "pred fail":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_7").Trim())));
                                break;
                            case "starting":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_8").Trim())));
                                break;
                            case "stopping":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_9").Trim())));
                                break;
                            case "service":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_10").Trim())));
                                break;
                            case "stressed":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_11").Trim())));
                                break;
                            case "nonrecover":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_12").Trim())));
                                break;
                            case "no contact":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_13").Trim())));
                                break;
                            case "lost comm":
                                sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_3").Trim())));
                                break;
                        }
                    }else{
                        sound_device_status_list.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound_Content", "sound_c_2").Trim())));
                    }
                    SOUND_DeviceStatus_V.Text = sound_device_status_list[0];
                }catch (Exception){ }
            }
            try{ SOUND_Select.SelectedIndex = 0; }catch (Exception){ }
            // SOUND PROCESS END ENABLED
            SOUND_RotateBtn.Enabled = true;
            ((Control)SOUND).Enabled = true;
            sound_status = 1;
        }
        private void SOUND_Select_SelectedIndexChanged(object sender, EventArgs e){
            try{
                int sound_select = SOUND_Select.SelectedIndex;
                SOUND_DeviceName_V.Text = sound_device_name_list[sound_select];
                SOUND_DeviceManufacturer_V.Text = sound_device_manufacturer_list[sound_select];
                SOUND_DeviceID_V.Text = sound_device_id_list[sound_select];
                SOUND_PNPDeviceID_V.Text = sound_pnp_device_id_list[sound_select];
                SOUND_DeviceStatus_V.Text = sound_device_status_list[sound_select];
            }catch (Exception){ }
        }
        // BATTERY
        // ======================================================================================================
        private void battery(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
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
                    if (battery_structure == 1){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_1").Trim()));
                    }else if (battery_structure == 2){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_2").Trim()));
                    }else if (battery_structure == 3){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_3").Trim()));
                    }else if (battery_structure == 4){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_4").Trim()));
                    }else if (battery_structure == 5){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_5").Trim()));
                    }else if (battery_structure == 6){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_6").Trim()));
                    }else if (battery_structure == 7){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_7").Trim()));
                    }else if (battery_structure == 8){
                        BATTERY_Type_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_8").Trim()));
                    }
                }catch(ManagementException){ }
            }
            // BATTERY PROCESS END ENABLED
            BATTERY_RotateBtn.Enabled = true;
            ((Control)BATTERY).Enabled = true;
            battery_status = 1;
        }
        private void battery_visible_off(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            BATTERY_Status_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_9").Trim()));
            BATTERY_Model.Visible = false;
            BATTERY_Model_V.Visible = false;
            BATTERY_Name.Visible = false;
            BATTERY_Name_V.Visible = false;
            BATTERY_Voltage.Visible = false;
            BATTERY_Voltage_V.Visible = false;
            BATTERY_Type.Visible = false;
            BATTERY_Type_V.Visible = false;
            BATTERY_ReportBtn.Visible = false;
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
        }
        private void laptop_bg_process(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                ManagementObjectSearcher search_battery = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Battery");
                PowerStatus power = SystemInformation.PowerStatus;
                do{
                    if (loop_status == false){ break; }
                    Single battery = power.BatteryLifePercent;
                    Single battery_process = battery * 100;
                    string battery_status = "%" + Convert.ToString(battery_process);
                    foreach (ManagementObject query_battery_rotate in search_battery.Get()){
                        try{
                            // BATTERY VOLTAGE
                            double battery_voltage = Convert.ToDouble(query_battery_rotate["DesignVoltage"]) / 1000.0;
                            BATTERY_Voltage_V.Text = string.Format("{0:0.0} Volt", battery_voltage);
                        }catch (Exception){ }
                    }
                    BATTERY_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_10").Trim())) + " - " + battery_status;
                    if (power.PowerLineStatus == PowerLineStatus.Online){
                        if (battery_process == 100){
                            BATTERY_Status_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_10").Trim())) + " " + battery_status;
                        }else{
                            BATTERY_Status_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_11").Trim())) + " " + battery_status;
                        }
                    }else{
                        BATTERY_Status_V.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery_Content", "by_c_12").Trim())) + " " + battery_status;
                    }
                    Thread.Sleep(1000 - DateTime.Now.Millisecond);
                }while (loop_status == true);
            }catch (Exception){ }
        }
        string battery_report_path = Application.StartupPath + @"\battery-report.html";
        private void BATTERY_ReportBtn_Click(object sender, EventArgs e){
            try{
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                if (File.Exists(battery_report_path)){
                    File.Delete(battery_report_path);
                }
                Process.Start("cmd.exe", "/k " + $"title {string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_title").Trim())), Application.ProductName + " -")} & powercfg /batteryreport & exit");
                Task battery_report_process_start = new Task(battery_report_check_process);
                battery_report_process_start.Start();
            }catch (Exception){ }
        }
        private void battery_report_check_process(){
            try{
                while (true){
                    if (File.Exists(battery_report_path)){
                        GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                        DialogResult br_message = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_rn").Trim())), battery_report_path, "\n\n"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (br_message == DialogResult.Yes){
                            Process.Start(battery_report_path);
                        }
                        break;
                    }
                }
            }catch (Exception){ }
        }
        // OSD
        // ======================================================================================================
        private void osd(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                ManagementObjectSearcher search_sd = new ManagementObjectSearcher("root\\CIMV2","SELECT * FROM Win32_SystemDriver");
                foreach (ManagementObject query_sd_rotate in search_sd.Get()){
                    string driver_names = Convert.ToString(query_sd_rotate["Name"]).Trim() + ".sys";
                    string driver_paths = Convert.ToString(query_sd_rotate["DisplayName"]).Trim();
                    string driver_types = Convert.ToString(query_sd_rotate["ServiceType"]).Trim();
                    string driver_starts = Convert.ToString(query_sd_rotate["StartMode"]).Trim();
                    string driver_status = Convert.ToString(query_sd_rotate["State"]).Trim();
                    // DRIVER TYPE TR
                    if (driver_types == "Kernel Driver"){
                        driver_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_1").Trim()));
                    }else if (driver_types == "File System Driver"){
                        driver_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_2").Trim()));
                    }
                    // DRIVER STARTUP TR
                    if (driver_starts == "Boot"){
                        driver_starts = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_3").Trim()));
                    }else if (driver_starts == "Manual"){
                        driver_starts = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_4").Trim()));
                    }else if (driver_starts == "System"){
                        driver_starts = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_5").Trim()));
                    }else if (driver_starts == "Auto"){
                        driver_starts = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_6").Trim()));
                    }else if (driver_starts == "Disabled"){
                        driver_starts = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_7").Trim()));
                    }
                    // DRIVER STATUS TR
                    if (driver_status == "Stopped"){
                        driver_status = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_8").Trim()));
                    }else if (driver_status == "Running"){
                        driver_status = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_9").Trim()));
                    }
                    string[] driver_infos = { driver_names, driver_paths, driver_types, driver_starts, driver_status };
                    OSD_DataMainTable.Rows.Add(driver_infos);
                }
            }catch (ManagementException){ }
            OSD_TYSS_V.Text = OSD_DataMainTable.Rows.Count.ToString();
            OSD_DataMainTable.Sort(OSD_DataMainTable.Columns[0], ListSortDirection.Ascending);
            OSD_DataMainTable.ClearSelection();
            // OSD PROCESS END ENABLED
            OSD_RotateBtn.Enabled = true;
            ((Control)OSD).Enabled = true;
            // PRELOADER FIX
            OSD_DataMainTable.Width++;
            OSD_DataMainTable.Width--;
            // PRELOADER FIX
            osd_status = 1;
        }
        private void OSD_TextBox_TextChanged(object sender, EventArgs e){
            if (OSD_TextBox.Text == "" || OSD_TextBox.Text == string.Empty){
                OSD_DataMainTable.ClearSelection();
                OSD_DataMainTable.FirstDisplayedScrollingRowIndex = OSD_DataMainTable.Rows[0].Index;
                OSD_TextBoxClearBtn.Enabled = false;
            }else{ 
                try{
                    OSD_TextBoxClearBtn.Enabled = true;
                    foreach (DataGridViewRow driver_row in OSD_DataMainTable.Rows){
                        if (driver_row.Cells[0].Value.ToString().ToLower().Contains(OSD_TextBox.Text.ToString().Trim().ToLower()) || driver_row.Cells[1].Value.ToString().ToLower().Contains(OSD_TextBox.Text.ToString().Trim().ToLower())){
                            driver_row.Selected = true; OSD_DataMainTable.FirstDisplayedScrollingRowIndex = driver_row.Index; break; 
                        } 
                    } 
                }catch(Exception){ }
            }
        }
        private void OSD_DataMainTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                if (OSD_DataMainTable.SelectedRows.Count > 0){
                    Clipboard.SetText(string.Format("{0} | {1} | {2} | {3} | {4}", OSD_DataMainTable.Rows[e.RowIndex].Cells[0].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[1].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[2].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[3].Value.ToString(), OSD_DataMainTable.Rows[e.RowIndex].Cells[4].Value.ToString()));
                    MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_success").Trim())), OSD_DataMainTable.Rows[e.RowIndex].Cells[0].Value), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }catch (Exception){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd_Content", "osd_c_error").Trim())), OSD_DataMainTable.Rows[e.RowIndex].Cells[0].Value), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    OSD_DataMainTable.Sort(OSD_DataMainTable.Columns[0], ListSortDirection.Descending);
                }else if (OSD_SortMode.CheckState == CheckState.Unchecked){
                    OSD_DataMainTable.Sort(OSD_DataMainTable.Columns[0], ListSortDirection.Ascending);
                }
                OSD_DataMainTable.ClearSelection();
            }catch (Exception){ }
        }
        // GS SERVICE
        // ======================================================================================================
        private void gs_services(){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                ManagementObjectSearcher search_service = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Service");
                foreach (ManagementObject query_service_rotate in search_service.Get()){
                    string service_path_names = Convert.ToString(query_service_rotate["Name"]).Trim();
                    string service_captions = Convert.ToString(query_service_rotate["DisplayName"]).Trim();
                    string service_types = Convert.ToString(query_service_rotate["ServiceType"]).Trim();
                    string service_start_modes = Convert.ToString(query_service_rotate["StartMode"]).Trim();
                    string service_states = Convert.ToString(query_service_rotate["State"]).Trim();
                    // SERVICE TYPE
                    if (service_types == "Kernel Driver"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_1").Trim()));
                    }else if (service_types == "File System Driver"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_2").Trim()));
                    }else if (service_types == "Adapter"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_3").Trim()));
                    }else if (service_types == "Recognizer Driver"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_4").Trim()));
                    }else if (service_types == "Own Process"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_5").Trim()));
                    }else if (service_types == "Share Process"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_6").Trim()));
                    }else if (service_types == "Interactive Process"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_7").Trim()));
                    }else if (service_types == "Unknown"){
                        service_types = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_8").Trim()));
                    }
                    // START MODE
                    if (service_start_modes == "Boot"){
                        service_start_modes = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_9").Trim()));
                    }else if (service_start_modes == "Manual"){
                        service_start_modes = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_10").Trim()));
                    }else if (service_start_modes == "System"){
                        service_start_modes = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_11").Trim()));
                    }else if (service_start_modes == "Auto"){
                        service_start_modes = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_12").Trim()));
                    }else if (service_start_modes == "Disabled"){
                        service_start_modes = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_13").Trim()));
                    }
                    // STATE
                    if (service_states == "Stopped"){
                        service_states = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_14").Trim()));
                    }else if (service_states == "Running"){
                        service_states = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_15").Trim()));
                    }
                    string[] services_infos = { service_path_names, service_captions, service_types, service_start_modes, service_states };
                    SERVICE_DataMainTable.Rows.Add(services_infos);
                }
            }catch (ManagementException){ }
            SERVICE_TYS_V.Text = SERVICE_DataMainTable.Rows.Count.ToString();
            SERVICE_DataMainTable.Sort(SERVICE_DataMainTable.Columns[0], ListSortDirection.Ascending);
            SERVICE_DataMainTable.ClearSelection();
            // SERVICE PROCESS END ENABLED
            SERVICES_RotateBtn.Enabled = true;
            ((Control)GSERVICE).Enabled = true;
            // PRELOADER FIX
            SERVICE_DataMainTable.Width++;
            SERVICE_DataMainTable.Width--;
            // PRELOADER FIX
            service_status = 1;
        }
        private void Services_SearchTextBox_TextChanged(object sender, EventArgs e){
            if (SERVICE_TextBox.Text == "" || SERVICE_TextBox.Text == string.Empty){
                SERVICE_DataMainTable.ClearSelection();
                SERVICE_DataMainTable.FirstDisplayedScrollingRowIndex = SERVICE_DataMainTable.Rows[0].Index;
                SERVICE_TextBoxClearBtn.Enabled = false;
            }else{
                try{
                    SERVICE_TextBoxClearBtn.Enabled = true;
                    foreach (DataGridViewRow service_row in SERVICE_DataMainTable.Rows){
                        if (service_row.Cells[0].Value.ToString().ToLower().Contains(SERVICE_TextBox.Text.ToString().Trim().ToLower()) || service_row.Cells[1].Value.ToString().ToLower().Contains(SERVICE_TextBox.Text.ToString().Trim().ToLower())){
                            service_row.Selected = true; SERVICE_DataMainTable.FirstDisplayedScrollingRowIndex = service_row.Index; break; 
                        } 
                    }
                }catch(Exception){ } 
            }
        }
        private void SERVICE_DataMainTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                if (SERVICE_DataMainTable.SelectedRows.Count > 0){
                    Clipboard.SetText(string.Format("{0} | {1} | {2} | {3} | {4}", SERVICE_DataMainTable.Rows[e.RowIndex].Cells[0].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[1].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[2].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[3].Value.ToString(), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[4].Value.ToString()));
                    MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_success").Trim())), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[0].Value), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }catch (Exception){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services_Content", "ss_c_error").Trim())), SERVICE_DataMainTable.Rows[e.RowIndex].Cells[0].Value), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    SERVICE_DataMainTable.Sort(SERVICE_DataMainTable.Columns[0], ListSortDirection.Descending);
                }else if (SERVICE_SortMode.CheckState == CheckState.Unchecked){
                    SERVICE_DataMainTable.Sort(SERVICE_DataMainTable.Columns[0], ListSortDirection.Ascending);
                }
                SERVICE_DataMainTable.ClearSelection();
            }catch (Exception){ }
        }
        // BUTTONS ROTATE
        // ======================================================================================================
        private Button active_btn;
        private void active_page(object btn_target){
            disabled_page();
            if (btn_target != null){
                if (active_btn != (Button)btn_target){
                    active_btn = (Button)btn_target;
                    if (theme == 1){ active_btn.BackColor = btn_colors[1]; }
                    else if (theme == 2){ active_btn.BackColor = btn_colors[3]; }
                    active_btn.Cursor = Cursors.Default;
                }
            }
        }
        private void disabled_page(){
            foreach (Control disabled_btn in LeftMenuPanel.Controls){
                if (theme == 1){ disabled_btn.BackColor = btn_colors[0]; }
                else if (theme == 2){ disabled_btn.BackColor = btn_colors[2]; }
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
        // GLOW DYNAMIC ARROW KEYS ROTATE
        private void MainContent_Selecting(object sender, TabControlCancelEventArgs e){
            try{
                switch (MainContent.SelectedIndex){
                    case 0:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ OS_RotateBtn.PerformClick(); }
                        break;
                    case 1:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ MB_RotateBtn.PerformClick(); }
                        break;
                    case 2:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ CPU_RotateBtn.PerformClick(); }
                        break;
                    case 3:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ RAM_RotateBtn.PerformClick(); }
                        break;
                    case 4:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ GPU_RotateBtn.PerformClick(); }
                        break;
                    case 5:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ DISK_RotateBtn.PerformClick(); }
                        break;
                    case 6:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ NET_RotateBtn.PerformClick(); }
                        break;
                    case 7:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ USB_RotateBtn.PerformClick(); }
                        break;
                    case 8:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ SOUND_RotateBtn.PerformClick(); }
                        break;
                    case 9:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ BATTERY_RotateBtn.PerformClick(); }
                        break;
                    case 10:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ OSD_RotateBtn.PerformClick(); }
                        break;
                    case 11:
                        if (!e.TabPage.Enabled){ e.Cancel = true; }else{ SERVICES_RotateBtn.PerformClick(); }
                        break;
                }
            }catch (Exception){ }
        }
        // GLOW DYNAMIC LEFT MENU PRELOADER
        private void left_menu_preloader(int target_menu, object sender){
            try{
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                switch (target_menu){
                    case 1:
                        if (menu_btns != 1){
                            MainContent.SelectedTab = OS;
                            menu_btns = 1;
                            menu_rp = 1;
                            if (OS_RotateBtn.BackColor != btn_colors[1] && OS_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_1").Trim()));
                        }
                        break;
                    case 2:
                        if (menu_btns != 2){
                            MainContent.SelectedTab = MB;
                            menu_btns = 2;
                            menu_rp = 2;
                            if (MB_RotateBtn.BackColor != btn_colors[1] && MB_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_2").Trim()));
                        }
                        break;
                    case 3:
                        if (menu_btns != 3){
                            MainContent.SelectedTab = CPU;
                            menu_btns = 3;
                            menu_rp = 3;
                            if (CPU_RotateBtn.BackColor != btn_colors[1] && CPU_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_3").Trim()));
                        }
                        break;
                    case 4:
                        if (menu_btns != 4){
                            MainContent.SelectedTab = RAM;
                            menu_btns = 4;
                            menu_rp = 4;
                            if (RAM_RotateBtn.BackColor != btn_colors[1] && RAM_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_4").Trim()));
                        }
                        break;
                    case 5:
                        if (menu_btns != 5){
                            MainContent.SelectedTab = GPU;
                            menu_btns = 5;
                            menu_rp = 5;
                            if (GPU_RotateBtn.BackColor != btn_colors[1] && GPU_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_5").Trim()));
                        }
                        break;
                    case 6:
                        if (menu_btns != 6){
                            MainContent.SelectedTab = DISK;
                            menu_btns = 6;
                            menu_rp = 6;
                            if (DISK_RotateBtn.BackColor != btn_colors[1] && DISK_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_6").Trim()));
                        }
                        break;
                    case 7:
                        if (menu_btns != 7){
                            MainContent.SelectedTab = NETWORK;
                            menu_btns = 7;
                            menu_rp = 7;
                            if (NET_RotateBtn.BackColor != btn_colors[1] && NET_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_7").Trim()));
                        }
                        break;
                    case 8:
                        if (menu_btns != 8){
                            MainContent.SelectedTab = USB;
                            menu_btns = 8;
                            menu_rp = 8;
                            if (USB_RotateBtn.BackColor != btn_colors[1] && USB_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_8").Trim()));
                        }
                        break;
                    case 9:
                        if (menu_btns != 9){
                            MainContent.SelectedTab = SOUND;
                            menu_btns = 9;
                            menu_rp = 9;
                            if (SOUND_RotateBtn.BackColor != btn_colors[1] && SOUND_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_9").Trim()));
                        }
                        break;
                    case 10:
                        if (menu_btns != 10){
                            MainContent.SelectedTab = BATTERY;
                            menu_btns = 10;
                            menu_rp = 10;
                            if (BATTERY_RotateBtn.BackColor != btn_colors[1] && BATTERY_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()));
                        }
                        break;
                    case 11:
                        if (menu_btns != 11){
                            MainContent.SelectedTab = OSD;
                            menu_btns = 11;
                            menu_rp = 11;
                            if (OSD_RotateBtn.BackColor != btn_colors[1] && OSD_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_11").Trim()));
                        }
                        break;
                    case 12:
                        if (menu_btns != 12){
                            MainContent.SelectedTab = GSERVICE;
                            menu_btns = 12;
                            menu_rp = 12;
                            if (SERVICES_RotateBtn.BackColor != btn_colors[1] && SERVICES_RotateBtn.BackColor != btn_colors[3]){ active_page(sender); }
                            HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_12").Trim()));
                        }
                        break;
                }
                header_image_reloader(menu_btns);
            }catch (Exception){ }
        }
        // LANGUAGES SETTINGS
        // ======================================================================================================
        private ToolStripMenuItem selected_lang;
        private void select_lang_active(object target_lang){
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
        private void englishToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "en"){ lang_preload("en"); select_lang_active(sender); }
        }
        private void turkishToolStripMenuItem_Click(object sender, EventArgs e){
            if (lang != "tr"){ lang_preload("tr"); select_lang_active(sender); }
        }
        private void lang_preload(string lang_type){
            lang_engine(lang_type);
            try{
                GlowSettingsSave glow_setting_save = new GlowSettingsSave(glow_sf);
                glow_setting_save.GlowWriteSettings("GlowSettings", "LanguageStatus", lang_type);
            }catch (Exception){ }
            // LANG CHANGE NOTIFICATION
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            DialogResult lang_change_message = MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LangChange", "le_1").Trim())) + "\n\n" + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LangChange", "le_2").Trim())) + "\n\n" + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LangChange", "le_3").Trim())), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (lang_change_message == DialogResult.Yes){ Application.Restart(); }
        }
        private void lang_engine(string lang_type){
            try{
                switch (lang_type){
                    case "en":
                        lang = "en";
                        lang_path = glow_lang_en;
                        break;
                    case "tr":
                        lang = "tr";
                        lang_path = glow_lang_tr;
                        break;
                }
                // GLOBAL ENGINE
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                // HEADER TITLE
                if (menu_rp == 1){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_1").Trim()));
                }else if (menu_rp == 2){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_2").Trim()));
                }else if (menu_rp == 3){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_3").Trim()));
                }else if (menu_rp == 4){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_4").Trim()));
                }else if (menu_rp == 5){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_5").Trim()));
                }else if (menu_rp == 6){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_6").Trim()));
                }else if (menu_rp == 7){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_7").Trim()));
                }else if (menu_rp == 8){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_8").Trim()));
                }else if (menu_rp == 9){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_9").Trim()));
                }else if (menu_rp == 10){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()));
                }else if (menu_rp == 11){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_11").Trim()));
                }else if (menu_rp == 12){
                    HeaderText.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_12").Trim()));
                }
                // PRINT INFORMATION
                printInformationToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_1").Trim()));
                textDocumentToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderPrint", "header_p_1").Trim()));
                hTMLDocumentToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderPrint", "header_p_2").Trim()));
                // SETTINGS
                settingsToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_2").Trim()));
                // THEMES
                themeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_3").Trim()));
                lightThemeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderThemes", "theme_light").Trim()));
                darkThemeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderThemes", "theme_dark").Trim()));
                // LANGS
                languageToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_4").Trim()));
                englishToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderLangs", "lang_en").Trim()));
                turkishToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderLangs", "lang_tr").Trim()));
                // INITIAL VIEW
                initialViewToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_5").Trim()));
                windowedToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderViewMode", "view_m_1").Trim()));
                fullScreenToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderViewMode", "view_m_2").Trim()));
                // HIDING MODE
                hidingModeToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_6").Trim()));
                hidingModeOnToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_m_1").Trim()));
                hidingModeOffToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderHidingMode", "hiding_m_2").Trim()));
                // TOOLS
                toolsToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_7").Trim()));
                sFCandDISMAutoToolToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderTools", "ht_1").Trim()));
                cacheCleaningToolToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderTools", "ht_2").Trim()));
                // GITHUB
                gitHubToolStripMenuItem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderMenu", "header_m_8").Trim()));
                // MENU
                OS_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_1").Trim()));
                MB_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_2").Trim()));
                CPU_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_3").Trim()));
                RAM_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_4").Trim()));
                GPU_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_5").Trim()));
                DISK_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_6").Trim()));
                NET_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_7").Trim()));
                USB_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_8").Trim()));
                SOUND_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_9").Trim()));
                BATTERY_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_10").Trim()));
                OSD_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_11").Trim()));
                SERVICES_RotateBtn.Text = " " + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("LeftMenu", "left_m_12").Trim()));
                // OS
                OS_SystemUser.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_1").Trim()));
                OS_ComputerName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_2").Trim()));
                OS_SystemModel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_3").Trim()));
                OS_SavedUser.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_4").Trim()));
                OS_Name.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_5").Trim()));
                OS_Manufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_6").Trim()));
                OS_SystemVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_7").Trim()));
                OS_Build.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_8").Trim()));
                OS_SystemBuild.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_9").Trim()));
                OS_SystemArchitectural.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_10").Trim()));
                OS_Family.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_11").Trim()));
                OS_DeviceID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_12").Trim()));
                OS_Serial.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_13").Trim()));
                OS_Country.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_14").Trim()));
                OS_TimeZone.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_15").Trim()));
                OS_CharacterSet.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_16").Trim()));
                OS_EncryptionType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_17").Trim()));
                OS_SystemRootIndex.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_18").Trim()));
                OS_SystemBuildPart.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_19").Trim()));
                OS_SystemTime.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_20").Trim()));
                OS_Install.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_21").Trim()));
                OS_SystemWorkTime.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_22").Trim()));
                OS_LastBootTime.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_23").Trim()));
                OS_SystemLastShutDown.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_24").Trim()));
                OS_PortableOS.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_25").Trim()));
                OS_MouseWheelStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_26").Trim()));
                OS_ScrollLockStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_27").Trim()));
                OS_NumLockStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_28").Trim()));
                OS_CapsLockStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_29").Trim()));
                OS_BootPartition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_30").Trim()));
                OS_SystemPartition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_31").Trim()));
                OS_AVProgram.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_32").Trim()));
                OS_FirewallProgram.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_33").Trim()));
                OS_AntiSpywareProgram.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_34").Trim()));
                OS_Minidump.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_35").Trim()));
                OS_BSODDate.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_36").Trim()));
                OS_Wallpaper.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_37").Trim()));
                MainToolTip.SetToolTip(OS_WallpaperOpen, string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("OperatingSystem", "os_38").Trim())), wp_rotate));
                // MB
                MB_MotherBoardName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_1").Trim()));
                MB_MotherBoardMan.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_2").Trim()));
                MB_MotherBoardSerial.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_3").Trim()));
                MB_SystemFamily.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_4").Trim()));
                MB_SystemSKU.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_5").Trim()));
                MB_Chipset.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_6").Trim()));
                MB_BiosManufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_7").Trim()));
                MB_BiosDate.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_8").Trim()));
                MB_BiosVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_9").Trim()));
                MB_SmBiosVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_10").Trim()));
                MB_BiosMode.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_11").Trim()));
                MB_LastBIOSTime.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_12").Trim()));
                MB_SecureBoot.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_13").Trim()));
                MB_TPMStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_14").Trim()));
                MB_TPMPhysicalVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_15").Trim()));
                MB_TPMMan.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_16").Trim()));
                MB_TPMManID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_17").Trim()));
                MB_TPMManVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_18").Trim()));
                MB_TPMManFullVersion.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_19").Trim()));
                MB_TPMManPublisher.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_20").Trim()));
                MB_Model.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_21").Trim()));
                MB_PrimaryBusType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_22").Trim()));
                MB_SecondaryBusType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_23").Trim()));
                MB_BiosMajorMinor.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_24").Trim()));
                MB_SMBiosMajorMinor.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Motherboard", "mb_25").Trim()));
                // CPU
                CPU_Name.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_1").Trim()));
                CPU_Manufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_2").Trim()));
                CPU_Architectural.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_3").Trim()));
                CPU_NormalSpeed.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_4").Trim()));
                CPU_DefaultSpeed.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_5").Trim()));
                CPU_L1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_6").Trim()));
                CPU_L2.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_7").Trim()));
                CPU_L3.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_8").Trim()));
                CPU_CoreCount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_9").Trim()));
                CPU_LogicalCore.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_10").Trim()));
                CPU_Process.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_11").Trim()));
                CPU_SocketDefinition.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_12").Trim()));
                CPU_Family.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_13").Trim()));
                CPU_Virtualization.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_14").Trim()));
                CPU_VMMonitorExtension.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_15").Trim()));
                CPU_SerialName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Processor", "pr_16").Trim()));
                // RAM
                RAM_TotalRAM.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_1").Trim()));
                RAM_UsageRAMCount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_2").Trim()));
                RAM_EmptyRamCount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_3").Trim()));
                RAM_TotalVirtualRam.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_4").Trim()));
                RAM_UsageVirtualRam.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_5").Trim()));
                RAM_EmptyVirtualRam.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_6").Trim()));
                RAM_SlotStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_7").Trim()));
                RAM_SlotSelectLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_8").Trim()));
                RAM_Amount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_9").Trim()));
                RAM_Type.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_10").Trim()));
                RAM_Frequency.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_11").Trim()));
                RAM_Volt.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_12").Trim()));
                RAM_FormFactor.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_13").Trim()));
                RAM_Serial.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_14").Trim()));
                RAM_Manufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_15").Trim()));
                RAM_BankLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_16").Trim()));
                RAM_DataWidth.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_17").Trim()));
                RAM_BellekType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_18").Trim()));
                RAM_PartNumber.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_19").Trim()));
                // GPU
                GPU_Name.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_1").Trim()));
                GPU_Manufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_2").Trim()));
                GPU_Version.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_3").Trim()));
                GPU_DriverDate.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_4").Trim()));
                GPU_Status.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_5").Trim()));
                GPU_DeviceID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_6").Trim()));
                GPU_DacType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_7").Trim()));
                GPU_GraphicDriversName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_8").Trim()));
                GPU_InfFileName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_9").Trim()));
                GPU_INFSectionFile.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_10").Trim()));
                GPU_MonitorSelect.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_11").Trim()));
                GPU_MonitorBounds.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_12").Trim()));
                GPU_MonitorWorking.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_13").Trim()));
                GPU_MonitorResLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_14").Trim()));
                GPU_MonitorVirtualRes.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_15").Trim()));
                GPU_ScreenRefreshRate.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_16").Trim()));
                GPU_MonitorPrimary.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_17").Trim()));
                // DISK
                DISK_Caption.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_1").Trim()));
                DISK_Model.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_2").Trim()));
                DISK_Man.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_3").Trim()));
                DISK_VolumeID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_4").Trim()));
                DISK_VolumeName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_5").Trim()));
                DISK_PhysicalName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_6").Trim()));
                DISK_Firmware.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_7").Trim()));
                DISK_Serial.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_8").Trim()));
                DISK_VolumeSerial.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_9").Trim()));
                DISK_Size.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_10").Trim()));
                DISK_FreeSpace.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_11").Trim()));
                DISK_FileSystem.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_12").Trim()));
                DISK_FormattingType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_13").Trim()));
                DISK_Type.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_14").Trim()));
                DISK_DriveType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_15").Trim()));
                DISK_InterFace.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_16").Trim()));
                DISK_PartitionCount.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_17").Trim()));
                DISK_MediaLoaded.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_18").Trim()));
                DISK_MediaStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_19").Trim()));
                DISK_Health.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_20").Trim()));
                DISK_Boot.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_21").Trim()));
                DISK_Bootable.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_22").Trim()));
                DISK_DriveCompressed.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_23").Trim()));
                // NETWORK
                NET_ConnType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_1").Trim()));
                NET_MacAdress.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_2").Trim()));
                NET_NetMan.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_3").Trim()));
                NET_ServiceName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_4").Trim()));
                NET_AdapterType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_5").Trim()));
                NET_Physical.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_6").Trim()));
                NET_DeviceID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_7").Trim()));
                NET_Guid.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_8").Trim()));
                NET_ConnectionType.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_9").Trim()));
                NET_Dhcp_status.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_10").Trim()));
                NET_Dhcp_server.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_11").Trim()));
                NET_LocalConSpeed.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_12").Trim()));
                NET_IPv4Adress.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_13").Trim()));
                NET_IPv6Adress.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_14").Trim()));
                // USB
                USB_Con.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_k_1").Trim()));
                USB_ConName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_k_2").Trim()));
                USB_ConMan.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_k_3").Trim()));
                USB_ConDeviceID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_k_4").Trim()));
                USB_ConPNPDeviceID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_k_5").Trim()));
                USB_ConDeviceStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_k_6").Trim()));
                USB_Device.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_1").Trim()));
                USB_DeviceName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_2").Trim()));
                USB_DeviceID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_3").Trim()));
                USB_PNPDeviceID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_4").Trim()));
                USB_DeviceStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_5").Trim()));
                // SOUND
                SOUND_Device.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound", "sound_1").Trim()));
                SOUND_DeviceName.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound", "sound_2").Trim()));
                SOUND_DeviceManufacturer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound", "sound_3").Trim()));
                SOUND_DeviceID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound", "sound_4").Trim()));
                SOUND_PNPDeviceID.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound", "sound_5").Trim()));
                SOUND_DeviceStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound", "sound_6").Trim()));
                // BATTERY
                BATTERY_Status.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_1").Trim()));
                BATTERY_Model.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_2").Trim()));
                BATTERY_Name.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_3").Trim()));
                BATTERY_Voltage.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_4").Trim()));
                BATTERY_Type.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_5").Trim()));
                BATTERY_ReportBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Battery", "by_button").Trim()));
                // OSD
                OSD_DataMainTable.Columns[0].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_1").Trim()));
                OSD_DataMainTable.Columns[1].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_2").Trim()));
                OSD_DataMainTable.Columns[2].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_3").Trim()));
                OSD_DataMainTable.Columns[3].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_4").Trim()));
                OSD_DataMainTable.Columns[4].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_5").Trim()));
                OSD_SearchDriverLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_6").Trim()));
                OSD_TYSS.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_7").Trim()));
                OSD_SortMode.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_8").Trim()));
                // SERVICES
                SERVICE_DataMainTable.Columns[0].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_1").Trim()));
                SERVICE_DataMainTable.Columns[1].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_2").Trim()));
                SERVICE_DataMainTable.Columns[2].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_3").Trim()));
                SERVICE_DataMainTable.Columns[3].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_4").Trim()));
                SERVICE_DataMainTable.Columns[4].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_5").Trim()));
                SERVICE_SearchLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_6").Trim()));
                SERVICE_TYS.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_7").Trim()));
                SERVICE_SortMode.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_8").Trim()));
            }catch (Exception){ }
        }
        // ======================================================================================================
        // THEME SETTINGS
        private ToolStripMenuItem selected_theme;
        private void select_theme_active(object target_theme){
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
            if (theme != 1){ color_mode(1); select_theme_active(sender); }
        }
        private void darkThemeToolStripMenuItem_Click(object sender, EventArgs e){
            if (theme != 2){ color_mode(2); select_theme_active(sender); }
        }
        private void color_mode(int ts){
            switch (ts){
                case 1:
                    theme = ts;
                    break;
                case 2:
                    theme = ts;
                    break;
            }
            if (theme == 1){
                // TITLEBAR CHANGE 
                try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } } catch (Exception){ }
                // CLEAR PRELOAD ITEMS
                if (ui_colors.Count > 0){ ui_colors.Clear(); }
                if (header_colors.Count > 1){ header_colors.Clear(); }
                // HEADER MENU COLOR MODE
                header_colors.Add(Color.FromArgb(222, 222, 222));
                header_colors.Add(Color.FromArgb(31, 31, 31));
                // HEADER AND TOOLTIP COLOR MODE
                ui_colors.Add(Color.FromArgb(32, 32, 32));          // 0
                ui_colors.Add(Color.FromArgb(235, 235, 235));       // 1
                // LEFT MENU COLOR MODE
                ui_colors.Add(Color.FromArgb(235, 235, 235));       // 2
                ui_colors.Add(Color.WhiteSmoke);                    // 3
                ui_colors.Add(Color.FromArgb(32, 32, 32));          // 4
                // CONTENT BG COLOR MODE
                ui_colors.Add(Color.WhiteSmoke);                    // 5
                // UI COLOR MODES
                ui_colors.Add(Color.FromArgb(235, 235, 235));       // 6
                ui_colors.Add(Color.FromArgb(32, 32, 32));          // 7
                ui_colors.Add(Color.FromArgb(0, 103, 192));         // 8
                ui_colors.Add(Color.WhiteSmoke);                    // 9
                ui_colors.Add(Color.FromArgb(235, 235, 235));       // 10
                ui_colors.Add(Color.WhiteSmoke);                    // 11
                ui_colors.Add(Color.FromArgb(32, 32, 32));          // 12
                ui_colors.Add(Color.White);                         // 13
                ui_colors.Add(Color.FromArgb(32, 32, 32));          // 14
                ui_colors.Add(Color.FromArgb(217, 217, 217));       // 15
                ui_colors.Add(Color.FromArgb(235, 235, 235));       // 16
                ui_colors.Add(Color.FromArgb(0, 103, 192));         // 17
                ui_colors.Add(Color.White);                         // 18
                ui_colors.Add(Color.WhiteSmoke);                    // 19
                // LEFT MENU LOGO CHANGE
                OS_RotateBtn.Image = Properties.Resources.left_os_light;
                MB_RotateBtn.Image = Properties.Resources.left_mb_light;
                CPU_RotateBtn.Image = Properties.Resources.left_cpu_light;
                RAM_RotateBtn.Image = Properties.Resources.left_ram_light;
                GPU_RotateBtn.Image = Properties.Resources.left_gpu_light;
                DISK_RotateBtn.Image = Properties.Resources.left_disk_light;
                NET_RotateBtn.Image = Properties.Resources.left_network_light;
                USB_RotateBtn.Image = Properties.Resources.left_usb_light;
                SOUND_RotateBtn.Image = Properties.Resources.left_sound_light;
                BATTERY_RotateBtn.Image = Properties.Resources.left_battery_light;
                OSD_RotateBtn.Image = Properties.Resources.left_osd_light;
                SERVICES_RotateBtn.Image = Properties.Resources.left_services_light;
                // TOP MENU LOGO CHANGE
                printInformationToolStripMenuItem.Image = Properties.Resources.top_export_light;
                textDocumentToolStripMenuItem.Image = Properties.Resources.top_txt_light;
                hTMLDocumentToolStripMenuItem.Image = Properties.Resources.top_html_light;
                settingsToolStripMenuItem.Image = Properties.Resources.top_settings_light;
                themeToolStripMenuItem.Image = Properties.Resources.top_theme_light;
                languageToolStripMenuItem.Image = Properties.Resources.top_language_light;
                initialViewToolStripMenuItem.Image = Properties.Resources.top_launch_light;
                hidingModeToolStripMenuItem.Image = Properties.Resources.top_hiding_light;
                toolsToolStripMenuItem.Image = Properties.Resources.top_tools_light;
                sFCandDISMAutoToolToolStripMenuItem.Image = Properties.Resources.top_sfc_and_dism_light;
                cacheCleaningToolToolStripMenuItem.Image = Properties.Resources.top_cache_clean_light;
                // MIDDLE CONTENT LOGO CHANGE
                OS_MinidumpOpen.BackgroundImage = Properties.Resources.middle_ow_light;
                OS_WallpaperOpen.BackgroundImage = Properties.Resources.middle_ow_light;
                // GITHUB
                gitHubToolStripMenuItem.Image = Properties.Resources.top_github_light;
                // SAVE THEME
                try{
                    GlowSettingsSave glow_setting_save = new GlowSettingsSave(glow_sf);
                    glow_setting_save.GlowWriteSettings("GlowSettings", "ThemeStatus", "1");
                }catch (Exception){ }
            }else if (theme == 2){
                // TITLEBAR CHANGE
                try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } } catch (Exception){ }
                // CLEAR PRELOAD ITEMS
                if (ui_colors.Count > 0){ ui_colors.Clear(); }
                if (header_colors.Count > 1){ header_colors.Clear(); }
                // HEADER MENU COLOR MODE
                header_colors.Add(Color.FromArgb(31, 31, 31));
                header_colors.Add(Color.FromArgb(222, 222, 222));
                // HEADER AND TOOLTIP COLOR MODE
                ui_colors.Add(Color.WhiteSmoke);                    // 0
                ui_colors.Add(Color.FromArgb(24, 24, 24));          // 1
                // LEFT MENU COLOR MODE
                ui_colors.Add(Color.FromArgb(24, 24, 24));          // 2
                ui_colors.Add(Color.FromArgb(31, 31, 31));          // 3
                ui_colors.Add(Color.WhiteSmoke);                    // 4
                // CONTENT BG COLOR MODE
                ui_colors.Add(Color.FromArgb(31, 31, 31));          // 5
                // UI COLOR MODES
                ui_colors.Add(Color.FromArgb(24, 24, 24));          // 6
                ui_colors.Add(Color.WhiteSmoke);                    // 7
                ui_colors.Add(Color.FromArgb(76, 194, 255));        // 8
                ui_colors.Add(Color.FromArgb(31, 31, 31));          // 9
                ui_colors.Add(Color.FromArgb(24, 24, 24));          // 10
                ui_colors.Add(Color.FromArgb(31, 31, 31));          // 11
                ui_colors.Add(Color.WhiteSmoke);                    // 12
                ui_colors.Add(Color.FromArgb(24, 24, 24));          // 13
                ui_colors.Add(Color.WhiteSmoke);                    // 14
                ui_colors.Add(Color.FromArgb(50, 50, 50));          // 15
                ui_colors.Add(Color.FromArgb(31, 31, 31));          // 16
                ui_colors.Add(Color.FromArgb(76, 194, 255));        // 17
                ui_colors.Add(Color.FromArgb(37, 37, 45));          // 18
                ui_colors.Add(Color.FromArgb(31, 31, 31));          // 19
                // LEFT MENU LOGO CHANGE
                OS_RotateBtn.Image = Properties.Resources.left_os_dark;
                MB_RotateBtn.Image = Properties.Resources.left_mb_dark;
                CPU_RotateBtn.Image = Properties.Resources.left_cpu_dark;
                RAM_RotateBtn.Image = Properties.Resources.left_ram_dark;
                GPU_RotateBtn.Image = Properties.Resources.left_gpu_dark;
                DISK_RotateBtn.Image = Properties.Resources.left_disk_dark;
                NET_RotateBtn.Image = Properties.Resources.left_network_dark;
                USB_RotateBtn.Image = Properties.Resources.left_usb_dark;
                SOUND_RotateBtn.Image = Properties.Resources.left_sound_dark;
                BATTERY_RotateBtn.Image = Properties.Resources.left_battery_dark;
                OSD_RotateBtn.Image = Properties.Resources.left_osd_dark;
                SERVICES_RotateBtn.Image = Properties.Resources.left_services_dark;
                // TOP MENU LOGO CHANGE
                printInformationToolStripMenuItem.Image = Properties.Resources.top_export_dark;
                textDocumentToolStripMenuItem.Image = Properties.Resources.top_txt_dark;
                hTMLDocumentToolStripMenuItem.Image = Properties.Resources.top_html_dark;
                settingsToolStripMenuItem.Image = Properties.Resources.top_settings_dark;
                themeToolStripMenuItem.Image = Properties.Resources.top_theme_dark;
                languageToolStripMenuItem.Image = Properties.Resources.top_language_dark;
                initialViewToolStripMenuItem.Image = Properties.Resources.top_launch_dark;
                hidingModeToolStripMenuItem.Image = Properties.Resources.top_hiding_dark;
                toolsToolStripMenuItem.Image = Properties.Resources.top_tools_dark;
                sFCandDISMAutoToolToolStripMenuItem.Image = Properties.Resources.top_sfc_and_dism_dark;
                cacheCleaningToolToolStripMenuItem.Image = Properties.Resources.top_cache_clean_dark;
                // MIDDLE CONTENT LOGO CHANGE
                OS_MinidumpOpen.BackgroundImage = Properties.Resources.middle_ow_dark;
                OS_WallpaperOpen.BackgroundImage = Properties.Resources.middle_ow_dark;
                // GITHUB
                gitHubToolStripMenuItem.Image = Properties.Resources.top_github_dark;
                // SAVE THEME
                try{
                    GlowSettingsSave glow_setting_save = new GlowSettingsSave(glow_sf);
                    glow_setting_save.GlowWriteSettings("GlowSettings", "ThemeStatus", "0");
                }catch (Exception){ }
            }
            theme_engine();
        }
        private void theme_engine(){
            try{
                header_image_reloader(menu_btns);
                HeaderMenu.Renderer = new HeaderMenuColors();
                // TOOLTIP
                MainToolTip.ForeColor = ui_colors[0];
                MainToolTip.BackColor = ui_colors[1];
                // HEADER PANEL
                HeaderPanel.BackColor = ui_colors[1];
                // HEADER PANEL TEXT
                HeaderText.ForeColor = ui_colors[0];
                // HEADER MENU
                HeaderMenu.ForeColor = ui_colors[0];
                HeaderMenu.BackColor = ui_colors[1];
                // HEADER MENU CONTENT
                printInformationToolStripMenuItem.ForeColor = ui_colors[0];
                printInformationToolStripMenuItem.BackColor = ui_colors[1];
                textDocumentToolStripMenuItem.ForeColor = ui_colors[0];
                textDocumentToolStripMenuItem.BackColor = ui_colors[1];
                hTMLDocumentToolStripMenuItem.ForeColor = ui_colors[0];
                hTMLDocumentToolStripMenuItem.BackColor = ui_colors[1];
                // SETTINGS
                settingsToolStripMenuItem.ForeColor = ui_colors[0];
                settingsToolStripMenuItem.BackColor = ui_colors[1];
                // THEMES
                themeToolStripMenuItem.BackColor = ui_colors[1];
                themeToolStripMenuItem.ForeColor = ui_colors[0];
                lightThemeToolStripMenuItem.BackColor = ui_colors[1];
                lightThemeToolStripMenuItem.ForeColor = ui_colors[0];
                darkThemeToolStripMenuItem.BackColor = ui_colors[1];
                darkThemeToolStripMenuItem.ForeColor = ui_colors[0];
                // LANGS
                languageToolStripMenuItem.BackColor = ui_colors[1];
                languageToolStripMenuItem.ForeColor = ui_colors[0];
                englishToolStripMenuItem.BackColor = ui_colors[1];
                englishToolStripMenuItem.ForeColor = ui_colors[0];
                turkishToolStripMenuItem.BackColor = ui_colors[1];
                turkishToolStripMenuItem.ForeColor = ui_colors[0];
                // INITIAL VIEW
                initialViewToolStripMenuItem.BackColor = ui_colors[1];
                initialViewToolStripMenuItem.ForeColor = ui_colors[0];
                windowedToolStripMenuItem.BackColor = ui_colors[1];
                windowedToolStripMenuItem.ForeColor = ui_colors[0];
                fullScreenToolStripMenuItem.BackColor = ui_colors[1];
                fullScreenToolStripMenuItem.ForeColor = ui_colors[0];
                // HIDING MODE
                hidingModeToolStripMenuItem.BackColor = ui_colors[1];
                hidingModeToolStripMenuItem.ForeColor = ui_colors[0];
                hidingModeOnToolStripMenuItem.BackColor = ui_colors[1];
                hidingModeOnToolStripMenuItem.ForeColor = ui_colors[0];
                hidingModeOffToolStripMenuItem.BackColor = ui_colors[1];
                hidingModeOffToolStripMenuItem.ForeColor = ui_colors[0];
                // TOOLS
                toolsToolStripMenuItem.BackColor = ui_colors[1];
                toolsToolStripMenuItem.ForeColor = ui_colors[0];
                sFCandDISMAutoToolToolStripMenuItem.BackColor = ui_colors[1];
                sFCandDISMAutoToolToolStripMenuItem.ForeColor = ui_colors[0];
                cacheCleaningToolToolStripMenuItem.BackColor = ui_colors[1];
                cacheCleaningToolToolStripMenuItem.ForeColor = ui_colors[0];
                // GITHUB
                gitHubToolStripMenuItem.BackColor = ui_colors[1];
                gitHubToolStripMenuItem.ForeColor = ui_colors[0];
                // LEFT MENU
                LeftMenuPanel.BackColor = ui_colors[2];
                OS_RotateBtn.BackColor = ui_colors[2];
                MB_RotateBtn.BackColor = ui_colors[2];
                CPU_RotateBtn.BackColor = ui_colors[2];
                RAM_RotateBtn.BackColor = ui_colors[2];
                GPU_RotateBtn.BackColor = ui_colors[2];
                DISK_RotateBtn.BackColor = ui_colors[2];
                NET_RotateBtn.BackColor = ui_colors[2];
                USB_RotateBtn.BackColor = ui_colors[2];
                SOUND_RotateBtn.BackColor = ui_colors[2];
                BATTERY_RotateBtn.BackColor = ui_colors[2];
                OSD_RotateBtn.BackColor = ui_colors[2];
                SERVICES_RotateBtn.BackColor = ui_colors[2];
                // LEFT MENU BORDER
                OS_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                MB_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                CPU_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                RAM_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                GPU_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                DISK_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                NET_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                USB_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                SOUND_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                BATTERY_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                OSD_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                SERVICES_RotateBtn.FlatAppearance.BorderColor = ui_colors[2];
                // LEFT MENU MOUSE HOVER
                OS_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                MB_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                CPU_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                RAM_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                GPU_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                DISK_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                NET_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                USB_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                SOUND_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                BATTERY_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                OSD_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                SERVICES_RotateBtn.FlatAppearance.MouseOverBackColor = ui_colors[3];
                // LEFT MENU MOUSE DOWN
                OS_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                MB_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                CPU_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                RAM_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                GPU_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                DISK_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                NET_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                USB_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                SOUND_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                BATTERY_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                OSD_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                SERVICES_RotateBtn.FlatAppearance.MouseDownBackColor = ui_colors[3];
                // LEFT MENU BUTTON TEXT COLOR
                OS_RotateBtn.ForeColor = ui_colors[4];
                MB_RotateBtn.ForeColor = ui_colors[4];
                CPU_RotateBtn.ForeColor = ui_colors[4];
                RAM_RotateBtn.ForeColor = ui_colors[4];
                GPU_RotateBtn.ForeColor = ui_colors[4];
                DISK_RotateBtn.ForeColor = ui_colors[4];
                NET_RotateBtn.ForeColor = ui_colors[4];
                USB_RotateBtn.ForeColor = ui_colors[4];
                SOUND_RotateBtn.ForeColor = ui_colors[4];
                BATTERY_RotateBtn.ForeColor = ui_colors[4];
                OSD_RotateBtn.ForeColor = ui_colors[4];
                SERVICES_RotateBtn.ForeColor = ui_colors[4];
                // CONTENT BG
                BackColor = ui_colors[5];
                OS.BackColor = ui_colors[5];
                MB.BackColor = ui_colors[5];
                CPU.BackColor = ui_colors[5];
                RAM.BackColor = ui_colors[5];
                GPU.BackColor = ui_colors[5];
                DISK.BackColor = ui_colors[5];
                NETWORK.BackColor = ui_colors[5];
                USB.BackColor = ui_colors[5];
                SOUND.BackColor = ui_colors[5];
                BATTERY.BackColor = ui_colors[5];
                OSD.BackColor = ui_colors[5];
                GSERVICE.BackColor = ui_colors[5];
                // OS
                os_panel_1.BackColor = ui_colors[6];
                os_panel_2.BackColor = ui_colors[6];
                os_panel_3.BackColor = ui_colors[6];
                os_panel_4.BackColor = ui_colors[6];
                os_panel_5.BackColor = ui_colors[6];
                os_panel_6.BackColor = ui_colors[6];
                os_bottom_label.ForeColor = ui_colors[9];
                OS_SystemUser.ForeColor = ui_colors[7];
                OS_SystemUser_V.ForeColor = ui_colors[8];
                OS_ComputerName.ForeColor = ui_colors[7];
                OS_ComputerName_V.ForeColor = ui_colors[8];
                OS_SystemModel.ForeColor = ui_colors[7];
                OS_SystemModel_V.ForeColor = ui_colors[8];
                OS_SavedUser.ForeColor = ui_colors[7];
                OS_SavedUser_V.ForeColor = ui_colors[8];
                OS_Name.ForeColor = ui_colors[7];
                OS_Name_V.ForeColor = ui_colors[8];
                OS_Manufacturer.ForeColor = ui_colors[7];
                OS_Manufacturer_V.ForeColor = ui_colors[8];
                OS_SystemVersion.ForeColor = ui_colors[7];
                OS_SystemVersion_V.ForeColor = ui_colors[8];
                OS_Build.ForeColor = ui_colors[7];
                OS_Build_V.ForeColor = ui_colors[8];
                OS_SystemBuild.ForeColor = ui_colors[7];
                OS_SystemBuild_V.ForeColor = ui_colors[8];
                OS_SystemArchitectural.ForeColor = ui_colors[7];
                OS_SystemArchitectural_V.ForeColor = ui_colors[8];
                OS_Family.ForeColor = ui_colors[7];
                OS_Family_V.ForeColor = ui_colors[8];
                OS_DeviceID.ForeColor = ui_colors[7];
                OS_DeviceID_V.ForeColor = ui_colors[8];
                OS_Serial.ForeColor = ui_colors[7];
                OS_Serial_V.ForeColor = ui_colors[8];
                OS_Country.ForeColor = ui_colors[7];
                OS_Country_V.ForeColor = ui_colors[8];
                OS_TimeZone.ForeColor = ui_colors[7];
                OS_TimeZone_V.ForeColor = ui_colors[8];
                OS_CharacterSet.ForeColor = ui_colors[7];
                OS_CharacterSet_V.ForeColor = ui_colors[8];
                OS_EncryptionType.ForeColor = ui_colors[7];
                OS_EncryptionType_V.ForeColor = ui_colors[8];
                OS_SystemRootIndex.ForeColor = ui_colors[7];
                OS_SystemRootIndex_V.ForeColor = ui_colors[8];
                OS_SystemBuildPart.ForeColor = ui_colors[7];
                OS_SystemBuildPart_V.ForeColor = ui_colors[8];
                OS_SystemTime.ForeColor = ui_colors[7];
                OS_SystemTime_V.ForeColor = ui_colors[8];
                OS_Install.ForeColor = ui_colors[7];
                OS_Install_V.ForeColor = ui_colors[8];
                OS_SystemWorkTime.ForeColor = ui_colors[7];
                OS_SystemWorkTime_V.ForeColor = ui_colors[8];
                OS_LastBootTime.ForeColor = ui_colors[7];
                OS_LastBootTime_V.ForeColor = ui_colors[8];
                OS_SystemLastShutDown.ForeColor = ui_colors[7];
                OS_SystemLastShutDown_V.ForeColor = ui_colors[8];
                OS_PortableOS.ForeColor = ui_colors[7];
                OS_PortableOS_V.ForeColor = ui_colors[8];
                OS_MouseWheelStatus.ForeColor = ui_colors[7];
                OS_MouseWheelStatus_V.ForeColor = ui_colors[8];
                OS_ScrollLockStatus.ForeColor = ui_colors[7];
                OS_ScrollLockStatus_V.ForeColor = ui_colors[8];
                OS_NumLockStatus.ForeColor = ui_colors[7];
                OS_NumLockStatus_V.ForeColor = ui_colors[8];
                OS_CapsLockStatus.ForeColor = ui_colors[7];
                OS_CapsLockStatus_V.ForeColor = ui_colors[8];
                OS_BootPartition.ForeColor = ui_colors[7];
                OS_BootPartition_V.ForeColor = ui_colors[8];
                OS_SystemPartition.ForeColor = ui_colors[7];
                OS_SystemPartition_V.ForeColor = ui_colors[8];
                OS_AVProgram.ForeColor = ui_colors[7];
                OS_AVProgram_V.ForeColor = ui_colors[8];
                OS_FirewallProgram.ForeColor = ui_colors[7];
                OS_FirewallProgram_V.ForeColor = ui_colors[8];
                OS_AntiSpywareProgram.ForeColor = ui_colors[7];
                OS_AntiSpywareProgram_V.ForeColor = ui_colors[8];
                OS_Minidump.ForeColor = ui_colors[7];
                OS_Minidump_V.ForeColor = ui_colors[8];
                OS_BSODDate.ForeColor = ui_colors[7];
                OS_BSODDate_V.ForeColor = ui_colors[8];
                OS_Wallpaper.ForeColor = ui_colors[7];
                OS_Wallpaper_V.ForeColor = ui_colors[8];
                // MB
                mb_panel_1.BackColor = ui_colors[6];
                mb_panel_2.BackColor = ui_colors[6];
                mb_panel_3.BackColor = ui_colors[6];
                mb_panel_4.BackColor = ui_colors[6];
                mb_bottom_1.ForeColor = ui_colors[9];
                MB_MotherBoardName.ForeColor = ui_colors[7];
                MB_MotherBoardName_V.ForeColor = ui_colors[8];
                MB_MotherBoardMan.ForeColor = ui_colors[7];
                MB_MotherBoardMan_V.ForeColor = ui_colors[8];
                MB_MotherBoardSerial.ForeColor = ui_colors[7];
                MB_MotherBoardSerial_V.ForeColor = ui_colors[8];
                MB_SystemFamily.ForeColor = ui_colors[7];
                MB_SystemFamily_V.ForeColor = ui_colors[8];
                MB_SystemSKU.ForeColor = ui_colors[7];
                MB_SystemSKU_V.ForeColor = ui_colors[8];
                MB_Chipset.ForeColor = ui_colors[7];
                MB_Chipset_V.ForeColor = ui_colors[8];
                MB_BiosManufacturer.ForeColor = ui_colors[7];
                MB_BiosManufacturer_V.ForeColor = ui_colors[8];
                MB_BiosDate.ForeColor = ui_colors[7];
                MB_BiosDate_V.ForeColor = ui_colors[8];
                MB_BiosVersion.ForeColor = ui_colors[7];
                MB_BiosVersion_V.ForeColor = ui_colors[8];
                MB_SmBiosVersion.ForeColor = ui_colors[7];
                MB_SmBiosVersion_V.ForeColor = ui_colors[8];
                MB_BiosMode.ForeColor = ui_colors[7];
                MB_BiosMode_V.ForeColor = ui_colors[8];
                MB_LastBIOSTime.ForeColor = ui_colors[7];
                MB_LastBIOSTime_V.ForeColor = ui_colors[8];
                MB_SecureBoot.ForeColor = ui_colors[7];
                MB_SecureBoot_V.ForeColor = ui_colors[8];
                MB_TPMStatus.ForeColor = ui_colors[7];
                MB_TPMStatus_V.ForeColor = ui_colors[8];
                MB_TPMPhysicalVersion.ForeColor = ui_colors[7];
                MB_TPMPhysicalVersion_V.ForeColor = ui_colors[8];
                MB_TPMMan.ForeColor = ui_colors[7];
                MB_TPMMan_V.ForeColor = ui_colors[8];
                MB_TPMManID.ForeColor = ui_colors[7];
                MB_TPMManID_V.ForeColor = ui_colors[8];
                MB_TPMManVersion.ForeColor = ui_colors[7];
                MB_TPMManVersion_V.ForeColor = ui_colors[8];
                MB_TPMManFullVersion.ForeColor = ui_colors[7];
                MB_TPMManFullVersion_V.ForeColor = ui_colors[8];
                MB_TPMManPublisher.ForeColor = ui_colors[7];
                MB_TPMManPublisher_V.ForeColor = ui_colors[8];
                MB_Model.ForeColor = ui_colors[7];
                MB_Model_V.ForeColor = ui_colors[8];
                MB_PrimaryBusType.ForeColor = ui_colors[7];
                MB_PrimaryBusType_V.ForeColor = ui_colors[8];
                MB_SecondaryBusType.ForeColor = ui_colors[7];
                MB_SecondaryBusType_V.ForeColor = ui_colors[8];
                MB_BiosMajorMinor.ForeColor = ui_colors[7];
                MB_BiosMajorMinor_V.ForeColor = ui_colors[8];
                MB_SMBiosMajorMinor.ForeColor = ui_colors[7];
                MB_SMBiosMajorMinor_V.ForeColor = ui_colors[8];
                // CPU
                cpu_panel_1.BackColor = ui_colors[6];
                cpu_panel_2.BackColor = ui_colors[6];
                cpu_bottom_1.ForeColor = ui_colors[9];
                CPU_Name.ForeColor = ui_colors[7];
                CPU_Name_V.ForeColor = ui_colors[8];
                CPU_Manufacturer.ForeColor = ui_colors[7];
                CPU_Manufacturer_V.ForeColor = ui_colors[8];
                CPU_Architectural.ForeColor = ui_colors[7];
                CPU_Architectural_V.ForeColor = ui_colors[8];
                CPU_NormalSpeed.ForeColor = ui_colors[7];
                CPU_NormalSpeed_V.ForeColor = ui_colors[8];
                CPU_DefaultSpeed.ForeColor = ui_colors[7];
                CPU_DefaultSpeed_V.ForeColor = ui_colors[8];
                CPU_L1.ForeColor = ui_colors[7];
                CPU_L1_V.ForeColor = ui_colors[8];
                CPU_L2.ForeColor = ui_colors[7];
                CPU_L2_V.ForeColor = ui_colors[8];
                CPU_L3.ForeColor = ui_colors[7];
                CPU_L3_V.ForeColor = ui_colors[8];
                CPU_CoreCount.ForeColor = ui_colors[7];
                CPU_CoreCount_V.ForeColor = ui_colors[8];
                CPU_LogicalCore.ForeColor = ui_colors[7];
                CPU_LogicalCore_V.ForeColor = ui_colors[8];
                CPU_Process.ForeColor = ui_colors[7];
                CPU_Process_V.ForeColor = ui_colors[8];
                CPU_SocketDefinition.ForeColor = ui_colors[7];
                CPU_SocketDefinition_V.ForeColor = ui_colors[8];
                CPU_Family.ForeColor = ui_colors[7];
                CPU_Family_V.ForeColor = ui_colors[8];
                CPU_Virtualization.ForeColor = ui_colors[7];
                CPU_Virtualization_V.ForeColor = ui_colors[8];
                CPU_VMMonitorExtension.ForeColor = ui_colors[7];
                CPU_VMMonitorExtension_V.ForeColor = ui_colors[8];
                CPU_SerialName.ForeColor = ui_colors[7];
                CPU_SerialName_V.ForeColor = ui_colors[8];
                // RAM
                ram_panel_1.BackColor = ui_colors[6];
                ram_panel_2.BackColor = ui_colors[6];
                ram_bottom_1.ForeColor = ui_colors[9];
                RAM_TotalRAM.ForeColor = ui_colors[7];
                RAM_TotalRAM_V.ForeColor = ui_colors[8];
                RAM_UsageRAMCount.ForeColor = ui_colors[7];
                RAM_UsageRAMCount_V.ForeColor = ui_colors[8];
                RAM_EmptyRamCount.ForeColor = ui_colors[7];
                RAM_EmptyRamCount_V.ForeColor = ui_colors[8];
                RAM_TotalVirtualRam.ForeColor = ui_colors[7];
                RAM_TotalVirtualRam_V.ForeColor = ui_colors[8];
                RAM_UsageVirtualRam.ForeColor = ui_colors[7];
                RAM_UsageVirtualRam_V.ForeColor = ui_colors[8];
                RAM_EmptyVirtualRam.ForeColor = ui_colors[7];
                RAM_EmptyVirtualRam_V.ForeColor = ui_colors[8];
                RAM_SlotStatus.ForeColor = ui_colors[7];
                RAM_SlotStatus_V.ForeColor = ui_colors[8];
                RAM_SlotSelectLabel.ForeColor = ui_colors[7];
                RAM_SelectList.BackColor = ui_colors[10];
                RAM_SelectList.ForeColor = ui_colors[8];
                RAM_Amount.ForeColor = ui_colors[7];
                RAM_Amount_V.ForeColor = ui_colors[8];
                RAM_Type.ForeColor = ui_colors[7];
                RAM_Type_V.ForeColor = ui_colors[8];
                RAM_Frequency.ForeColor = ui_colors[7];
                RAM_Frequency_V.ForeColor = ui_colors[8];
                RAM_Volt.ForeColor = ui_colors[7];
                RAM_Volt_V.ForeColor = ui_colors[8];
                RAM_FormFactor.ForeColor = ui_colors[7];
                RAM_FormFactor_V.ForeColor = ui_colors[8];
                RAM_Serial.ForeColor = ui_colors[7];
                RAM_Serial_V.ForeColor = ui_colors[8];
                RAM_Manufacturer.ForeColor = ui_colors[7];
                RAM_Manufacturer_V.ForeColor = ui_colors[8];
                RAM_BankLabel.ForeColor = ui_colors[7];
                RAM_BankLabel_V.ForeColor = ui_colors[8];
                RAM_DataWidth.ForeColor = ui_colors[7];
                RAM_DataWidth_V.ForeColor = ui_colors[8];
                RAM_BellekType.ForeColor = ui_colors[7];
                RAM_BellekType_V.ForeColor = ui_colors[8];
                RAM_PartNumber.ForeColor = ui_colors[7];
                RAM_PartNumber_V.ForeColor = ui_colors[8];
                // GPU
                gpu_panel_1.BackColor = ui_colors[6];
                gpu_panel_2.BackColor = ui_colors[6];
                gpu_bottom_1.ForeColor = ui_colors[9];
                GPU_Name.ForeColor = ui_colors[7];
                GPU_Select.BackColor = ui_colors[10];
                GPU_Select.ForeColor = ui_colors[8];
                GPU_Manufacturer.ForeColor = ui_colors[7];
                GPU_Manufacturer_V.ForeColor = ui_colors[8];
                GPU_Version.ForeColor = ui_colors[7];
                GPU_Version_V.ForeColor = ui_colors[8];
                GPU_DriverDate.ForeColor = ui_colors[7];
                GPU_DriverDate_V.ForeColor = ui_colors[8];
                GPU_Status.ForeColor = ui_colors[7];
                GPU_Status_V.ForeColor = ui_colors[8];
                GPU_DeviceID.ForeColor = ui_colors[7];
                GPU_DeviceID_V.ForeColor = ui_colors[8];
                GPU_DacType.ForeColor = ui_colors[7];
                GPU_DacType_V.ForeColor = ui_colors[8];
                GPU_GraphicDriversName.ForeColor = ui_colors[7];
                GPU_GraphicDriversName_V.ForeColor = ui_colors[8];
                GPU_InfFileName.ForeColor = ui_colors[7];
                GPU_InfFileName_V.ForeColor = ui_colors[8];
                GPU_INFSectionFile.ForeColor = ui_colors[7];
                GPU_INFSectionFile_V.ForeColor = ui_colors[8];
                GPU_MonitorSelectList.BackColor = ui_colors[10];
                GPU_MonitorSelectList.ForeColor = ui_colors[8];
                GPU_MonitorSelect.ForeColor = ui_colors[7];
                GPU_MonitorBounds.ForeColor = ui_colors[7];
                GPU_MonitorBounds_V.ForeColor = ui_colors[8];
                GPU_MonitorWorking.ForeColor = ui_colors[7];
                GPU_MonitorWorking_V.ForeColor = ui_colors[8];
                GPU_MonitorResLabel.ForeColor = ui_colors[7];
                GPU_MonitorResLabel_V.ForeColor = ui_colors[8];
                GPU_MonitorVirtualRes.ForeColor = ui_colors[7];
                GPU_MonitorVirtualRes_V.ForeColor = ui_colors[8];
                GPU_ScreenRefreshRate.ForeColor = ui_colors[7];
                GPU_ScreenRefreshRate_V.ForeColor = ui_colors[8];
                GPU_MonitorPrimary.ForeColor = ui_colors[7];
                GPU_MonitorPrimary_V.ForeColor = ui_colors[8];
                // DISK
                disk_panel_1.BackColor = ui_colors[6];
                disk_panel_2.BackColor = ui_colors[6];
                disk_bottom_label.ForeColor = ui_colors[9];
                DISK_Caption.ForeColor = ui_colors[7];
                DISK_CaptionList.BackColor = ui_colors[10];
                DISK_CaptionList.ForeColor = ui_colors[8];
                DISK_Model.ForeColor = ui_colors[7];
                DISK_Model_V.ForeColor = ui_colors[8];
                DISK_Man.ForeColor = ui_colors[7];
                DISK_Man_V.ForeColor = ui_colors[8];
                DISK_VolumeID.ForeColor = ui_colors[7];
                DISK_VolumeID_V.ForeColor = ui_colors[8];
                DISK_VolumeName.ForeColor = ui_colors[7];
                DISK_VolumeName_V.ForeColor = ui_colors[8];
                DISK_PhysicalName.ForeColor = ui_colors[7];
                DISK_PhysicalName_V.ForeColor = ui_colors[8];
                DISK_Firmware.ForeColor = ui_colors[7];
                DISK_Firmware_V.ForeColor = ui_colors[8];
                DISK_Serial.ForeColor = ui_colors[7];
                DISK_Serial_V.ForeColor = ui_colors[8];
                DISK_VolumeSerial.ForeColor = ui_colors[7];
                DISK_VolumeSerial_V.ForeColor = ui_colors[8];
                DISK_Size.ForeColor = ui_colors[7];
                DISK_Size_V.ForeColor = ui_colors[8];
                DISK_FreeSpace.ForeColor = ui_colors[7];
                DISK_FreeSpace_V.ForeColor = ui_colors[8];
                DISK_FileSystem.ForeColor = ui_colors[7];
                DISK_FileSystem_V.ForeColor = ui_colors[8];
                DISK_FormattingType.ForeColor = ui_colors[7];
                DISK_FormattingType_V.ForeColor = ui_colors[8];
                DISK_Type.ForeColor = ui_colors[7];
                DISK_Type_V.ForeColor = ui_colors[8];
                DISK_DriveType.ForeColor = ui_colors[7];
                DISK_DriveType_V.ForeColor = ui_colors[8];
                DISK_InterFace.ForeColor = ui_colors[7];
                DISK_InterFace_V.ForeColor = ui_colors[8];
                DISK_PartitionCount.ForeColor = ui_colors[7];
                DISK_PartitionCount_V.ForeColor = ui_colors[8];
                DISK_MediaLoaded.ForeColor = ui_colors[7];
                DISK_MediaLoaded_V.ForeColor = ui_colors[8];
                DISK_MediaStatus.ForeColor = ui_colors[7];
                DISK_MediaStatus_V.ForeColor = ui_colors[8];
                DISK_Health.ForeColor = ui_colors[7];
                DISK_Health_V.ForeColor = ui_colors[8];
                DISK_Boot.ForeColor = ui_colors[7];
                DISK_Boot_V.ForeColor = ui_colors[8];
                DISK_Bootable.ForeColor = ui_colors[7];
                DISK_Bootable_V.ForeColor = ui_colors[8];
                DISK_DriveCompressed.ForeColor = ui_colors[7];
                DISK_DriveCompressed_V.ForeColor = ui_colors[8];
                // NETWORK
                network_panel_1.BackColor = ui_colors[6];
                network_panel_2.BackColor = ui_colors[6];
                network_bottom_label.ForeColor = ui_colors[9];
                NET_ListNetwork.BackColor = ui_colors[10];
                NET_ListNetwork.ForeColor = ui_colors[8];
                NET_ConnType.ForeColor = ui_colors[7];
                NET_MacAdress.ForeColor = ui_colors[7];
                NET_MacAdress_V.ForeColor = ui_colors[8];
                NET_NetMan.ForeColor = ui_colors[7];
                NET_NetMan_V.ForeColor = ui_colors[8];
                NET_ServiceName.ForeColor = ui_colors[7];
                NET_ServiceName_V.ForeColor = ui_colors[8];
                NET_AdapterType.ForeColor = ui_colors[7];
                NET_AdapterType_V.ForeColor = ui_colors[8];
                NET_Physical.ForeColor = ui_colors[7];
                NET_Physical_V.ForeColor = ui_colors[8];
                NET_DeviceID.ForeColor = ui_colors[7];
                NET_DeviceID_V.ForeColor = ui_colors[8];
                NET_Guid.ForeColor = ui_colors[7];
                NET_Guid_V.ForeColor = ui_colors[8];
                NET_ConnectionType.ForeColor = ui_colors[7];
                NET_ConnectionType_V.ForeColor = ui_colors[8];
                NET_Dhcp_status.ForeColor = ui_colors[7];
                NET_Dhcp_status_V.ForeColor = ui_colors[8];
                NET_Dhcp_server.ForeColor = ui_colors[7];
                NET_Dhcp_server_V.ForeColor = ui_colors[8];
                NET_LocalConSpeed.ForeColor = ui_colors[7];
                NET_LocalConSpeed_V.ForeColor = ui_colors[8];
                NET_IPv4Adress.ForeColor = ui_colors[7];
                NET_IPv4Adress_V.ForeColor = ui_colors[8];
                NET_IPv6Adress.ForeColor = ui_colors[7];
                NET_IPv6Adress_V.ForeColor = ui_colors[8];
                // USB
                usb_panel_1.BackColor = ui_colors[6];
                usb_panel_2.BackColor = ui_colors[6];
                USB_Con.ForeColor = ui_colors[7];
                USB_ConList.BackColor = ui_colors[10];
                USB_ConList.ForeColor = ui_colors[8];
                USB_ConName.ForeColor = ui_colors[7];
                USB_ConName_V.ForeColor = ui_colors[8];
                USB_ConMan.ForeColor = ui_colors[7];
                USB_ConMan_V.ForeColor = ui_colors[8];
                USB_ConDeviceID.ForeColor = ui_colors[7];
                USB_ConDeviceID_V.ForeColor = ui_colors[8];
                USB_ConPNPDeviceID.ForeColor = ui_colors[7];
                USB_ConPNPDeviceID_V.ForeColor = ui_colors[8];
                USB_ConDeviceStatus.ForeColor = ui_colors[7];
                USB_ConDeviceStatus_V.ForeColor = ui_colors[8];
                USB_Device.ForeColor = ui_colors[7];
                USB_Select.BackColor = ui_colors[10];
                USB_Select.ForeColor = ui_colors[8];
                USB_DeviceName.ForeColor = ui_colors[7];
                USB_DeviceName_V.ForeColor = ui_colors[8];
                USB_DeviceID.ForeColor = ui_colors[7];
                USB_DeviceID_V.ForeColor = ui_colors[8];
                USB_PNPDeviceID.ForeColor = ui_colors[7];
                USB_PNPDeviceID_V.ForeColor = ui_colors[8];
                USB_DeviceStatus.ForeColor = ui_colors[7];
                USB_DeviceStatus_V.ForeColor = ui_colors[8];
                // SOUND
                sound_panel_1.BackColor = ui_colors[6];
                SOUND_Device.ForeColor = ui_colors[7];
                SOUND_Select.BackColor = ui_colors[10];
                SOUND_Select.ForeColor = ui_colors[8];
                SOUND_DeviceName.ForeColor = ui_colors[7];
                SOUND_DeviceName_V.ForeColor = ui_colors[8];
                SOUND_DeviceManufacturer.ForeColor = ui_colors[7];
                SOUND_DeviceManufacturer_V.ForeColor = ui_colors[8];
                SOUND_DeviceID.ForeColor = ui_colors[7];
                SOUND_DeviceID_V.ForeColor = ui_colors[8];
                SOUND_PNPDeviceID.ForeColor = ui_colors[7];
                SOUND_PNPDeviceID_V.ForeColor = ui_colors[8];
                SOUND_DeviceStatus.ForeColor = ui_colors[7];
                SOUND_DeviceStatus_V.ForeColor = ui_colors[8];
                // BATTERY
                battery_panel_1.BackColor = ui_colors[6];
                BATTERY_Status.ForeColor = ui_colors[7];
                BATTERY_Status_V.ForeColor = ui_colors[8];
                BATTERY_Model.ForeColor = ui_colors[7];
                BATTERY_Model_V.ForeColor = ui_colors[8];
                BATTERY_Name.ForeColor = ui_colors[7];
                BATTERY_Name_V.ForeColor = ui_colors[8];
                BATTERY_Voltage.ForeColor = ui_colors[7];
                BATTERY_Voltage_V.ForeColor = ui_colors[8];
                BATTERY_Type.ForeColor = ui_colors[7];
                BATTERY_Type_V.ForeColor = ui_colors[8];
                BATTERY_ReportBtn.ForeColor = ui_colors[19];
                BATTERY_ReportBtn.BackColor = ui_colors[8];
                // INSTALLED DRIVERS
                osd_panel_1.BackColor = ui_colors[6];
                OSD_TextBox.BackColor = ui_colors[11];
                OSD_TextBox.ForeColor = ui_colors[12];
                OSD_TYSS.ForeColor = ui_colors[7];
                OSD_TYSS_V.ForeColor = ui_colors[8];
                OSD_SearchDriverLabel.ForeColor = ui_colors[7];
                OSD_TextBoxClearBtn.BackColor = ui_colors[17];
                OSD_TextBoxClearBtn.ForeColor = ui_colors[18];
                OSD_SortMode.ForeColor = ui_colors[7];
                OSD_DataMainTable.BackgroundColor = ui_colors[13];
                OSD_DataMainTable.GridColor = ui_colors[15];
                OSD_DataMainTable.DefaultCellStyle.BackColor = ui_colors[13];
                OSD_DataMainTable.DefaultCellStyle.ForeColor = ui_colors[14];
                OSD_DataMainTable.AlternatingRowsDefaultCellStyle.BackColor = ui_colors[16];
                OSD_DataMainTable.ColumnHeadersDefaultCellStyle.BackColor = ui_colors[17];
                OSD_DataMainTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = ui_colors[17];
                OSD_DataMainTable.ColumnHeadersDefaultCellStyle.ForeColor = ui_colors[18];
                OSD_DataMainTable.DefaultCellStyle.SelectionBackColor = ui_colors[17];
                OSD_DataMainTable.DefaultCellStyle.SelectionForeColor = ui_colors[18];
                // SERVICES
                service_panel_1.BackColor = ui_colors[6];
                SERVICE_TextBox.BackColor = ui_colors[11];
                SERVICE_TextBox.ForeColor = ui_colors[12];
                SERVICE_TYS.ForeColor = ui_colors[7];
                SERVICE_TYS_V.ForeColor = ui_colors[8];
                SERVICE_SearchLabel.ForeColor = ui_colors[7];
                SERVICE_TextBoxClearBtn.BackColor = ui_colors[17];
                SERVICE_TextBoxClearBtn.ForeColor = ui_colors[18];
                SERVICE_SortMode.ForeColor = ui_colors[7];
                SERVICE_DataMainTable.BackgroundColor = ui_colors[13];
                SERVICE_DataMainTable.GridColor = ui_colors[15];
                SERVICE_DataMainTable.DefaultCellStyle.BackColor = ui_colors[13];
                SERVICE_DataMainTable.DefaultCellStyle.ForeColor = ui_colors[14];
                SERVICE_DataMainTable.AlternatingRowsDefaultCellStyle.BackColor = ui_colors[16];
                SERVICE_DataMainTable.ColumnHeadersDefaultCellStyle.BackColor = ui_colors[17];
                SERVICE_DataMainTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = ui_colors[17];
                SERVICE_DataMainTable.ColumnHeadersDefaultCellStyle.ForeColor = ui_colors[18];
                SERVICE_DataMainTable.DefaultCellStyle.SelectionBackColor = ui_colors[17];
                SERVICE_DataMainTable.DefaultCellStyle.SelectionForeColor = ui_colors[18];
                // ROTATE MENU
                if (menu_btns == 1){
                    OS_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 2){
                    MB_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 3){
                    CPU_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 4){
                    RAM_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 5){
                    GPU_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 6){
                    DISK_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 7){
                    NET_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 8){
                    USB_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 9){
                    SOUND_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 10){
                    BATTERY_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 11){
                    OSD_RotateBtn.BackColor = ui_colors[19];
                }else if (menu_btns == 12){
                    SERVICES_RotateBtn.BackColor = ui_colors[19];
                }
            }catch (Exception){ }
        }
        private void header_image_reloader(int hi_value){
            try{
                switch (theme){
                    case 1:
                        switch (hi_value){
                            case 1:
                                HeaderImage.BackgroundImage = Properties.Resources.left_os_light;
                                break;
                            case 2:
                                HeaderImage.BackgroundImage = Properties.Resources.left_mb_light;
                                break;
                            case 3:
                                HeaderImage.BackgroundImage = Properties.Resources.left_cpu_light;
                                break;
                            case 4:
                                HeaderImage.BackgroundImage = Properties.Resources.left_ram_light;
                                break;
                            case 5:
                                HeaderImage.BackgroundImage = Properties.Resources.left_gpu_light;
                                break;
                            case 6:
                                HeaderImage.BackgroundImage = Properties.Resources.left_disk_light;
                                break;
                            case 7:
                                HeaderImage.BackgroundImage = Properties.Resources.left_network_light;
                                break;
                            case 8:
                                HeaderImage.BackgroundImage = Properties.Resources.left_usb_light;
                                break;
                            case 9:
                                HeaderImage.BackgroundImage = Properties.Resources.left_sound_light;
                                break;
                            case 10:
                                HeaderImage.BackgroundImage = Properties.Resources.left_battery_light;
                                break;
                            case 11:
                                HeaderImage.BackgroundImage = Properties.Resources.left_osd_light;
                                break;
                            case 12:
                                HeaderImage.BackgroundImage = Properties.Resources.left_services_light;
                                break;
                        }
                        break;
                    case 2:
                        switch (hi_value){
                            case 1:
                                HeaderImage.BackgroundImage = Properties.Resources.left_os_dark;
                                break;
                            case 2:
                                HeaderImage.BackgroundImage = Properties.Resources.left_mb_dark;
                                break;
                            case 3:
                                HeaderImage.BackgroundImage = Properties.Resources.left_cpu_dark;
                                break;
                            case 4:
                                HeaderImage.BackgroundImage = Properties.Resources.left_ram_dark;
                                break;
                            case 5:
                                HeaderImage.BackgroundImage = Properties.Resources.left_gpu_dark;
                                break;
                            case 6:
                                HeaderImage.BackgroundImage = Properties.Resources.left_disk_dark;
                                break;
                            case 7:
                                HeaderImage.BackgroundImage = Properties.Resources.left_network_dark;
                                break;
                            case 8:
                                HeaderImage.BackgroundImage = Properties.Resources.left_usb_dark;
                                break;
                            case 9:
                                HeaderImage.BackgroundImage = Properties.Resources.left_sound_dark;
                                break;
                            case 10:
                                HeaderImage.BackgroundImage = Properties.Resources.left_battery_dark;
                                break;
                            case 11:
                                HeaderImage.BackgroundImage = Properties.Resources.left_osd_dark;
                                break;
                            case 12:
                                HeaderImage.BackgroundImage = Properties.Resources.left_services_dark;
                                break;
                        }
                        break;
                }
            }catch (Exception){ }
        }
        // INITIAL SETINGS
        // ======================================================================================================
        private ToolStripMenuItem selected_initial_mode;
        private void select_initial_mode_active(object target_initial_mode){
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
                GlowSettingsSave glow_setting_save = new GlowSettingsSave(glow_sf);
                glow_setting_save.GlowWriteSettings("GlowSettings", "InitialStatus", get_inital_value);
            }catch (Exception){ }
        }
        // HIDING MODE
        // ======================================================================================================
        private ToolStripMenuItem selected_hiding_mode;
        private void select_hiding_mode_active(object target_hiding_mode){
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
                GlowSettingsSave glow_setting_save = new GlowSettingsSave(glow_sf);
                glow_setting_save.GlowWriteSettings("GlowSettings", "HidingStatus", get_hiding_value);
            }catch (Exception){ }
            // HIDING MODE CHANGE NOTIFICATION
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            DialogResult hiding_mode_change_message = MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HidingModeChange", "hm_c_1").Trim())) + "\n\n" + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HidingModeChange", "hm_c_2").Trim())) + "\n\n" + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HidingModeChange", "hm_c_3").Trim())), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (hiding_mode_change_message == DialogResult.Yes){ Application.Restart(); }
        }
        // PRINT ENGINES
        // ======================================================================================================
        private void print_engine_async(){
            try{
                do{
                    if (pe_loop_status == false){ break; }
                    if (os_status == 1 && mb_status == 1 && cpu_status == 1 && ram_status == 1 && gpu_status == 1 &&
                    disk_status == 1 && network_status == 1 && usb_status == 1 && sound_status == 1 && battery_status == 1 && 
                    osd_status == 1 && service_status == 1){
                        pe_loop_status = false;
                        printInformationToolStripMenuItem.Enabled = true;
                        textDocumentToolStripMenuItem.Enabled = true;
                        hTMLDocumentToolStripMenuItem.Enabled = true;
                    }
                    Thread.Sleep(100);
                }while (pe_loop_status == true);
            }catch (Exception){ }
        }
        // PRINT ENGINE STARTER
        List<string> PrintEngineList = new List<string>();
        private void textDocumentToolStripMenuItem_Click(object sender, EventArgs e){ print_engine_mode(1); }
        private void hTMLDocumentToolStripMenuItem_Click(object sender, EventArgs e){ print_engine_mode(2); }
        private void print_engine_mode(int pe_mode){
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            try{
                switch (pe_mode){
                    case 1:
                        print_engine_txt();
                    break;
                    case 2:
                        print_engine_html();
                    break;
                }
            }catch (Exception){
                MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_2").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // PRINT ENGINE TXT
        // ======================================================================================================
        private void print_engine_txt(){
            // HEADER
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            PrintEngineList.Add($"<{new string('-', 13)} {Application.ProductName.ToUpper()} - {string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_3").Trim().ToUpper())), OS_ComputerName_V.Text)} {new string('-', 13)}>");
            PrintEngineList.Add(Environment.NewLine + new string('-', 60) + Environment.NewLine);
            // OS
            PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_1").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(OS_SystemUser.Text + " " + OS_SystemUser_V.Text);
            PrintEngineList.Add(OS_ComputerName.Text + " " + OS_ComputerName_V.Text);
            PrintEngineList.Add(OS_SystemModel.Text + " " + OS_SystemModel_V.Text);
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
            PrintEngineList.Add(OS_MouseWheelStatus.Text + " " + OS_MouseWheelStatus_V.Text);
            PrintEngineList.Add(OS_ScrollLockStatus.Text + " " + OS_ScrollLockStatus_V.Text);
            PrintEngineList.Add(OS_NumLockStatus.Text + " " + OS_NumLockStatus_V.Text);
            PrintEngineList.Add(OS_CapsLockStatus.Text + " " + OS_CapsLockStatus_V.Text);
            PrintEngineList.Add(OS_BootPartition.Text + " " + OS_BootPartition_V.Text);
            PrintEngineList.Add(OS_SystemPartition.Text + " " + OS_SystemPartition_V.Text);
            PrintEngineList.Add(OS_AVProgram.Text + " " + OS_AVProgram_V.Text);
            PrintEngineList.Add(OS_FirewallProgram.Text + " " + OS_FirewallProgram_V.Text);
            PrintEngineList.Add(OS_AntiSpywareProgram.Text + " " + OS_AntiSpywareProgram_V.Text);
            PrintEngineList.Add(OS_Minidump.Text + " " + OS_Minidump_V.Text);
            PrintEngineList.Add(OS_BSODDate.Text + " " + OS_BSODDate_V.Text);
            PrintEngineList.Add(OS_Wallpaper.Text + " " + OS_Wallpaper_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            // MOTHERBOARD
            PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_2").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(MB_MotherBoardName.Text + " " + MB_MotherBoardName_V.Text);
            PrintEngineList.Add(MB_MotherBoardMan.Text + " " + MB_MotherBoardMan_V.Text);
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
            PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_3").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(CPU_Name.Text + " " + CPU_Name_V.Text);
            PrintEngineList.Add(CPU_Manufacturer.Text + " " + CPU_Manufacturer_V.Text);
            PrintEngineList.Add(CPU_Architectural.Text + " " + CPU_Architectural_V.Text);
            PrintEngineList.Add(CPU_NormalSpeed.Text + " " + CPU_NormalSpeed_V.Text);
            PrintEngineList.Add(CPU_DefaultSpeed.Text + " " + CPU_DefaultSpeed_V.Text);
            PrintEngineList.Add(CPU_L1.Text + " " + CPU_L1_V.Text);
            PrintEngineList.Add(CPU_L2.Text + " " + CPU_L2_V.Text);
            PrintEngineList.Add(CPU_L3.Text + " " + CPU_L3_V.Text);
            PrintEngineList.Add(CPU_CoreCount.Text + " " + CPU_CoreCount_V.Text);
            PrintEngineList.Add(CPU_LogicalCore.Text + " " + CPU_LogicalCore_V.Text);
            PrintEngineList.Add(CPU_Process.Text + " " + CPU_Process_V.Text);
            PrintEngineList.Add(CPU_SocketDefinition.Text + " " + CPU_SocketDefinition_V.Text);
            PrintEngineList.Add(CPU_Family.Text + " " + CPU_Family_V.Text);
            PrintEngineList.Add(CPU_Virtualization.Text + " " + CPU_Virtualization_V.Text);
            PrintEngineList.Add(CPU_VMMonitorExtension.Text + " " + CPU_VMMonitorExtension_V.Text);
            PrintEngineList.Add(CPU_SerialName.Text + " " + CPU_SerialName_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            // RAM
            PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_4").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
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
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_8").Trim())) + " #" + rs + Environment.NewLine);
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
            PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_5").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int gpu_amount = GPU_Select.Items.Count;
                for (int gpu_render = 1; gpu_render <= gpu_amount; gpu_render++){
                    GPU_Select.SelectedIndex = gpu_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_18").Trim())) + " #" + gpu_render + Environment.NewLine);
                    PrintEngineList.Add(GPU_Name.Text + " " + GPU_Select.SelectedItem.ToString());
                    PrintEngineList.Add(GPU_Manufacturer.Text + " " + GPU_Manufacturer_V.Text);
                    PrintEngineList.Add(GPU_Version.Text + " " + GPU_Version_V.Text);
                    PrintEngineList.Add(GPU_DriverDate.Text + " " + GPU_DriverDate_V.Text);
                    PrintEngineList.Add(GPU_Status.Text + " " + GPU_Status_V.Text);
                    PrintEngineList.Add(GPU_DeviceID.Text + " " + GPU_DeviceID_V.Text);
                    PrintEngineList.Add(GPU_DacType.Text + " " + GPU_DacType_V.Text);
                    PrintEngineList.Add(GPU_GraphicDriversName.Text + " " + GPU_GraphicDriversName_V.Text);
                    PrintEngineList.Add(GPU_InfFileName.Text + " " + GPU_InfFileName_V.Text);
                    PrintEngineList.Add(GPU_INFSectionFile.Text + " " + GPU_INFSectionFile_V.Text + Environment.NewLine);
                }
                GPU_Select.SelectedIndex = 0;
            }catch (Exception){ }
            try{
                int screen_amount = GPU_MonitorSelectList.Items.Count;
                for (int sa = 1; sa <= screen_amount; sa++){
                    GPU_MonitorSelectList.SelectedIndex = sa - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_19").Trim())) + " #" + sa + Environment.NewLine);
                    PrintEngineList.Add(GPU_MonitorBounds.Text + " " + GPU_MonitorBounds_V.Text);
                    PrintEngineList.Add(GPU_MonitorWorking.Text + " " + GPU_MonitorWorking_V.Text);
                    PrintEngineList.Add(GPU_MonitorResLabel.Text + " " + GPU_MonitorResLabel_V.Text);
                    PrintEngineList.Add(GPU_MonitorVirtualRes.Text + " " + GPU_MonitorVirtualRes_V.Text);
                    PrintEngineList.Add(GPU_ScreenRefreshRate.Text + " " + GPU_ScreenRefreshRate_V.Text);
                    PrintEngineList.Add(GPU_MonitorPrimary.Text + " " + GPU_MonitorPrimary_V.Text + Environment.NewLine);
                }
                PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
                GPU_MonitorSelectList.SelectedIndex = 0;
            }catch (Exception){ }
            // STORAGE
            PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_6").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int disk_amount = DISK_CaptionList.Items.Count;
                for (int disk_render = 1; disk_render <= disk_amount; disk_render++){
                    DISK_CaptionList.SelectedIndex = disk_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_24").Trim())) + " #" + disk_render + Environment.NewLine);
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
                    PrintEngineList.Add(DISK_DriveCompressed.Text + " " + DISK_DriveCompressed_V.Text + Environment.NewLine);
                }
                DISK_CaptionList.SelectedIndex = 0;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // NETWORK
            PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_7").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int net_amount = NET_ListNetwork.Items.Count;
                for (int net_render = 1; net_render <= net_amount; net_render++){
                    NET_ListNetwork.SelectedIndex = net_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_15").Trim())) + " #" + net_render + Environment.NewLine);
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
                    PrintEngineList.Add(NET_LocalConSpeed.Text + " " + NET_LocalConSpeed_V.Text + Environment.NewLine);
                }
                PrintEngineList.Add(NET_IPv4Adress.Text + " " + NET_IPv4Adress_V.Text);
                PrintEngineList.Add(NET_IPv6Adress.Text + " " + NET_IPv6Adress_V.Text + Environment.NewLine);
                NET_ListNetwork.SelectedIndex = 1;
            }catch (Exception){ }
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // USB
            PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_8").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int usb_con_amount = USB_ConList.Items.Count;
                for (int usb_con_render = 1; usb_con_render <= usb_con_amount; usb_con_render++){
                    USB_ConList.SelectedIndex = usb_con_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_k_1").Trim())) + " #" + usb_con_render + Environment.NewLine);
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
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_1").Trim())) + " #" + usb_render + Environment.NewLine);
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
            PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_9").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
            try{
                int sound_amount = SOUND_Select.Items.Count;
                for (int sound_render = 1; sound_render <= sound_amount; sound_render++){
                    SOUND_Select.SelectedIndex = sound_render - 1;
                    PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound", "sound_1").Trim())) + " #" + sound_render + Environment.NewLine);
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
            PowerStatus power_status = SystemInformation.PowerStatus;
            String battery_charging = power_status.BatteryChargeStatus.ToString();
            if (battery_charging == "NoSystemBattery"){
                PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
                PrintEngineList.Add(BATTERY_Status.Text + " " + BATTERY_Status_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            }else{
                PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
                PrintEngineList.Add(BATTERY_Status.Text + " " + BATTERY_Status_V.Text);
                PrintEngineList.Add(BATTERY_Model.Text + " " + BATTERY_Model_V.Text);
                PrintEngineList.Add(BATTERY_Name.Text + " " + BATTERY_Name_V.Text);
                PrintEngineList.Add(BATTERY_Voltage.Text + " " + BATTERY_Voltage_V.Text);
                PrintEngineList.Add(BATTERY_Type.Text + " " + BATTERY_Type_V.Text + Environment.NewLine + Environment.NewLine + new string('-', 60) + Environment.NewLine);
            }
            // OSD
            PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_11").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_4").Trim())) + Environment.NewLine);
            try{
                for (int i = 0; i < OSD_DataMainTable.Rows.Count; i++){
                    PrintEngineList.Add(OSD_DataMainTable.Rows[i].Cells[0].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[1].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[2].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[3].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[4].Value.ToString() + Environment.NewLine + new string('-', 155));
                }
            }catch (Exception){ }
            PrintEngineList.Add(Environment.NewLine + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_9").Trim())) + " " + OSD_TYSS_V.Text + Environment.NewLine);
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // SERVICES
            PrintEngineList.Add($"<{new string('-', 7)} {Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_12").Trim()))} {new string('-', 7)}>" + Environment.NewLine);
            PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_5").Trim())) + Environment.NewLine);
            try{
                for (int i = 0; i < SERVICE_DataMainTable.Rows.Count; i++){
                    PrintEngineList.Add(SERVICE_DataMainTable.Rows[i].Cells[0].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[1].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[2].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[3].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[4].Value.ToString() + Environment.NewLine + new string('-', 155));
                }
            }catch (Exception){ }
            PrintEngineList.Add(Environment.NewLine + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_9").Trim())) + " " + SERVICE_TYS_V.Text + Environment.NewLine);
            PrintEngineList.Add(new string('-', 60) + Environment.NewLine);
            // FOOTER
            PrintEngineList.Add(Application.ProductName + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_6").Trim())) + " " + Application.ProductVersion.Substring(0, 4));
            PrintEngineList.Add($"(C) {DateTime.Now.Year} {Application.CompanyName}.");
            PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_7").Trim())) + " " + DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss"));
            PrintEngineList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_8").Trim())) + " " + github_link);
            SaveFileDialog save_engine = new SaveFileDialog{
                InitialDirectory = @"C:\Users\" + SystemInformation.UserName + @"\Desktop\",
                Title = Application.ProductName + " - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_9").Trim())),
                DefaultExt = "txt",
                FileName = Application.ProductName + " - " + string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_10").Trim())), OS_ComputerName_V.Text),
                Filter = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_11").Trim())) + " (*.txt)|*.txt"
            };
            if (save_engine.ShowDialog() == DialogResult.OK){
                String[] text_engine = new String[PrintEngineList.Count];
                PrintEngineList.CopyTo(text_engine, 0);
                File.WriteAllLines(save_engine.FileName, text_engine);
                DialogResult glow_print_engine_query = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_13").Trim())) + Environment.NewLine + Environment.NewLine + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_14").Trim())), Application.ProductName, save_engine.FileName), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (glow_print_engine_query == DialogResult.Yes){ Process.Start(save_engine.FileName); }
                PrintEngineList.Clear(); save_engine.Dispose();
            }else{
                PrintEngineList.Clear(); save_engine.Dispose();
            }
        }
        // ======================================================================================================
        // PRINT ENGINE HTML
        private void print_engine_html(){
            // HEADER
            GlowGetLangs g_lang = new GlowGetLangs(lang_path);
            // COLOR MODES
            Color html_body_bg_color = ui_colors[6];
            string html_bbgc = string.Format("#{0}{1}{2}", html_body_bg_color.R.ToString("X2"), html_body_bg_color.G.ToString("X2"), html_body_bg_color.B.ToString("X2"));
            Color html_middle_bg_color = ui_colors[5];
            string html_mbgc = string.Format("#{0}{1}{2}", html_middle_bg_color.R.ToString("X2"), html_middle_bg_color.G.ToString("X2"), html_middle_bg_color.B.ToString("X2"));
            Color html_ui_fe_color = ui_colors[8];
            string html_uifc = string.Format("#{0}{1}{2}", html_ui_fe_color.R.ToString("X2"), html_ui_fe_color.G.ToString("X2"), html_ui_fe_color.B.ToString("X2"));
            // HTML MAIN CODES
            PrintEngineList.Add("<!DOCTYPE html>");
            if (lang == "tr"){ PrintEngineList.Add("<html lang='tr'>"); }
            else if (lang == "en"){ PrintEngineList.Add("<html lang='en'>"); }
            PrintEngineList.Add("<head>");
            PrintEngineList.Add("\t<meta charset='utf-8'>");
            PrintEngineList.Add("\t<meta name='author' content='" + Application.CompanyName + "'>");
            PrintEngineList.Add("\t<meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            PrintEngineList.Add($"\t<title>{Application.ProductName} - {string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_3").Trim())), OS_ComputerName_V.Text)}</title>");
            PrintEngineList.Add("\t<style>");
            PrintEngineList.Add("\t\t@import url('https://fonts.googleapis.com/css2?family=Alata&display=swap');");
            PrintEngineList.Add("\t\t*{ font-family: 'Alata', sans-serif; margin: 0; padding: 0; box-sizing: border-box; text-decoration: none; }");
            if (theme == 1){ PrintEngineList.Add("\t\t:root { color-scheme: light; }"); }
            else if (theme == 2){ PrintEngineList.Add("\t\t:root { color-scheme: dark; }"); }
            PrintEngineList.Add("\t\ta:visited{ color: rgb(55, 162, 255); }");
            PrintEngineList.Add("\t\t.ts_scroll_top{ position: fixed; bottom: -100px; right: 20px; width: auto; height: auto; justify-content: center; align-items: center; display: flex; font-size: 46px; border-radius: 50%; cursor: pointer; color: " + html_uifc + "; z-index: 999; transition: 0.2s; }");
            PrintEngineList.Add("\t\t.ts_scroll_top:hover{ color: rgb(55, 162, 255); }");
            PrintEngineList.Add("\t\t.ts_scroll_top.active{ bottom: 20px; }");
            PrintEngineList.Add("\t\t@media (max-width: 700px){ .ts_scroll_top{ font-size: 28px; bottom: -100px; right: 10px; } .ts_scroll_top.active{ bottom: 10px; } }");
            PrintEngineList.Add("\t\tbody{ background-color: " + html_bbgc + "; padding: 5px 0; justify-content: center; align-items: center; display: flex; }");
            PrintEngineList.Add("\t\t#main_container{ width: 100%; height: auto; justify-content: center; align-items: center; display: flex; flex-direction: column; }");
            PrintEngineList.Add("\t\t#main_container > h2{ margin: 25px 0; color: " + html_uifc + "; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box{ background-color: " + html_mbgc + "; width: 1000px; height: auto; border-radius: 10px; margin: 5px 0; padding: 15px; box-sizing: border-box; display: inline-block; word-break: break-word; table-layout: fixed; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box:nth-child(1){ justify-content: center; align-items: center; display: flex; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box > h2{ color: " + html_uifc + "; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box > h3{ color: " + html_uifc + "; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box > h4{ font-weight: normal; margin: 13px 0 0 11px; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box > h4 > span{ margin: 0 5px 0 0; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box > h4 > span:nth-child(2){ color: " + html_uifc + "; font-weight: normal; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box > ul{ margin: 15px 0 0 30px; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box > ul > li{ margin: 5px 0; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box > ul > li > span{ margin: 0 7px 0 0; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box > ul > li > span:nth-child(2){ color: " + html_uifc + "; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box > ul > li > a{ color: " + html_uifc + "; transition: 0.3s; }");
            PrintEngineList.Add("\t\t#main_container > .middle_box > ul > li > a:hover{ color: rgb(55, 162, 255); }");
            PrintEngineList.Add("\t\t@media (max-width: 1030px){ #main_container > .middle_box{ width: 900px; } }");
            PrintEngineList.Add("\t\t@media (max-width: 915px){ #main_container > .middle_box{ width: 720px; } }");
            PrintEngineList.Add("\t\t@media (max-width: 735px){ #main_container > .middle_box{ width: 480px; padding: 10px; } #main_container > .middle_box > h3{ text-align: center; } }");
            PrintEngineList.Add("\t\t@media (max-width: 495px){ #main_container > .middle_box{ width: 360px; } #main_container > .middle_box > ul{ margin: 15px 0 0 25px; } #main_container > .middle_box > h4{ margin: 13px 0 0 6px; } button{ padding: 5px 10px; bottom: 5px; right: 5px; } }");
            PrintEngineList.Add("\t</style>");
            PrintEngineList.Add("\t<link rel='icon' type='image/x-icon' href='https://raw.githubusercontent.com/roines45/glow/main/Glow/glow_images/glow_favicon/GlowFavicon.ico'>");
            PrintEngineList.Add("\t<link rel='stylesheet' href='https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css'>");
            PrintEngineList.Add("</head>");
            PrintEngineList.Add("<body>");
            // SCROOL TOP BUTTON
            PrintEngineList.Add("\t\t<div class='ts_scroll_top' onclick='ts_scroll_to_up();'><i class='fa-solid fa-circle-chevron-up'></i></div>");
            // MAIN CONTAINER
            PrintEngineList.Add("\t<div id='main_container'>");
            // HEADER
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h2>{Application.ProductName} - {string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_3").Trim().ToUpper())), OS_ComputerName_V.Text)}</h2>");
            PrintEngineList.Add("\t\t</div>");
            // OS
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_1").Trim()))}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemUser.Text}</span><span>{OS_SystemUser_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ComputerName.Text}</span><span>{OS_ComputerName_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemModel.Text}</span><span>{OS_SystemModel_V.Text}</span></li>");
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
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_MouseWheelStatus.Text}</span><span>{OS_MouseWheelStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_ScrollLockStatus.Text}</span><span>{OS_ScrollLockStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_NumLockStatus.Text}</span><span>{OS_NumLockStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_CapsLockStatus.Text}</span><span>{OS_CapsLockStatus_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_BootPartition.Text}</span><span>{OS_BootPartition_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_SystemPartition.Text}</span><span>{OS_SystemPartition_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_AVProgram.Text}</span><span>{OS_AVProgram_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_FirewallProgram.Text}</span><span>{OS_FirewallProgram_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_AntiSpywareProgram.Text}</span><span>{OS_AntiSpywareProgram_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Minidump.Text}</span><span>{OS_Minidump_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_BSODDate.Text}</span><span>{OS_BSODDate_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{OS_Wallpaper.Text}</span><span>{OS_Wallpaper_V.Text}</span></li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t</div>");
            // MB
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_2").Trim()))}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_MotherBoardName.Text}</span><span>{MB_MotherBoardName_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{MB_MotherBoardMan.Text}</span><span>{MB_MotherBoardMan_V.Text}</span></li>");
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
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_3").Trim()))}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Name.Text}</span><span>{CPU_Name_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Manufacturer.Text}</span><span>{CPU_Manufacturer_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Architectural.Text}</span><span>{CPU_Architectural_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_NormalSpeed.Text}</span><span>{CPU_NormalSpeed_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_DefaultSpeed.Text}</span><span>{CPU_DefaultSpeed_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_L1.Text}</span><span>{CPU_L1_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_L2.Text}</span><span>{CPU_L2_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_L3.Text}</span><span>{CPU_L3_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_CoreCount.Text}</span><span>{CPU_CoreCount_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_LogicalCore.Text}</span><span>{CPU_LogicalCore_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Process.Text}</span><span>{CPU_Process_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_SocketDefinition.Text}</span><span>{CPU_SocketDefinition_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Family.Text}</span><span>{CPU_Family_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_Virtualization.Text}</span><span>{CPU_Virtualization_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_VMMonitorExtension.Text}</span><span>{CPU_VMMonitorExtension_V.Text}</span></li>");
            PrintEngineList.Add($"\t\t\t\t<li><span>{CPU_SerialName.Text}</span><span>{CPU_SerialName_V.Text}</span></li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t</div>");
            // RAM
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_4").Trim()))}</h3>");
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
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Memory", "my_8").Trim())) + "</span><span>#" + rs}</span></h4>");
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
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_5").Trim()))}</h3>");
            try{
                int gpu_amount = GPU_Select.Items.Count;
                for (int gpu_render = 1; gpu_render <= gpu_amount; gpu_render++){
                    GPU_Select.SelectedIndex = gpu_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_18").Trim())) + "</span><span>#" + gpu_render}</span></h4>");
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
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                GPU_Select.SelectedIndex = 0;
            }catch (Exception){ }
            try{
                int screen_amount = GPU_MonitorSelectList.Items.Count;
                for (int sa = 1; sa <= screen_amount; sa++){
                    GPU_MonitorSelectList.SelectedIndex = sa - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Gpu", "gpu_19").Trim())) + "</span><span>#" + sa}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorBounds.Text}</span><span>{GPU_MonitorBounds_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorWorking.Text}</span><span>{GPU_MonitorWorking_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorResLabel.Text}</span><span>{GPU_MonitorResLabel_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorVirtualRes.Text}</span><span>{GPU_MonitorVirtualRes_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_ScreenRefreshRate.Text}</span><span>{GPU_ScreenRefreshRate_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{GPU_MonitorPrimary.Text}</span><span>{GPU_MonitorPrimary_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                GPU_MonitorSelectList.SelectedIndex = 0;
            }catch (Exception) { }
            PrintEngineList.Add("\t\t</div>");
            // STORAGE
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_6").Trim()))}</h3>");
            try{
                int disk_amount = DISK_CaptionList.Items.Count;
                for (int disk_render = 1; disk_render <= disk_amount; disk_render++){
                    DISK_CaptionList.SelectedIndex = disk_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Storage", "se_24").Trim())) + "</span><span>#" + disk_render}</span></h4>");
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
                    PrintEngineList.Add($"\t\t\t\t<li><span>{DISK_DriveCompressed.Text}</span><span>{DISK_DriveCompressed_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                DISK_CaptionList.SelectedIndex = 0;
            }catch (Exception) { }
            PrintEngineList.Add("\t\t</div>");
            // NETWORK
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_7").Trim()))}</h3>");
            try{
                int net_amount = NET_ListNetwork.Items.Count;
                for (int net_render = 1; net_render <= net_amount; net_render++){
                    NET_ListNetwork.SelectedIndex = net_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Network", "nk_15").Trim())) + "</span><span>#" + net_render}</span></h4>");
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
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                PrintEngineList.Add("\t\t\t<ul>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_IPv4Adress.Text}</span><span>{NET_IPv4Adress_V.Text}</span></li>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{NET_IPv6Adress.Text}</span><span>{NET_IPv6Adress_V.Text}</span></li>");
                PrintEngineList.Add("\t\t\t</ul>");
                NET_ListNetwork.SelectedIndex = 1;
            }catch (Exception){ }
            PrintEngineList.Add("\t\t</div>");
            // USB
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_8").Trim()))}</h3>");
            try{
                int usb_con_amount = USB_ConList.Items.Count;
                for (int usb_con_render = 1; usb_con_render <= usb_con_amount; usb_con_render++){
                    USB_ConList.SelectedIndex = usb_con_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_k_1").Trim())) + "</span><span>#" + usb_con_render}</span></h4>");
                    PrintEngineList.Add("\t\t\t<ul>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_Con.Text}</span><span>{USB_ConList.SelectedItem}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConName.Text}</span><span>{USB_ConName_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConMan.Text}</span><span>{USB_ConMan_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConDeviceID.Text}</span><span>{USB_ConDeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConPNPDeviceID.Text}</span><span>{USB_ConPNPDeviceID_V.Text}</span></li>");
                    PrintEngineList.Add($"\t\t\t\t<li><span>{USB_ConDeviceStatus.Text}</span><span>{USB_ConDeviceStatus_V.Text}</span></li>");
                    PrintEngineList.Add("\t\t\t</ul>");
                }
                PrintEngineList.Add("\t\t\t<br>");
                USB_ConList.SelectedIndex = 0;
            }catch (Exception) { }
            try{
                int usb_amount = USB_Select.Items.Count;
                for (int usb_render = 1; usb_render <= usb_amount; usb_render++){
                    USB_Select.SelectedIndex = usb_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Usb", "usb_1").Trim())) + "</span><span>#" + usb_render}</span></h4>");
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
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_9").Trim()))}</h3>");
            try{
                int sound_amount = SOUND_Select.Items.Count;
                for (int sound_render = 1; sound_render <= sound_amount; sound_render++){
                    SOUND_Select.SelectedIndex = sound_render - 1;
                    PrintEngineList.Add($"\t\t\t\t<h4><span>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Sound", "sound_1").Trim())) + "</span><span>#" + sound_render}</span></h4>");
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
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PowerStatus power_status = SystemInformation.PowerStatus;
            String battery_charging = power_status.BatteryChargeStatus.ToString();
            if (battery_charging == "NoSystemBattery"){
                PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()))}</h3>");
                PrintEngineList.Add("\t\t\t<ul>");
                PrintEngineList.Add($"\t\t\t\t<li><span>{BATTERY_Status.Text}</span><span>{BATTERY_Status_V.Text}</span></li>");
                PrintEngineList.Add("\t\t\t</ul>");
            }else{
                PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_10").Trim()))}</h3>");
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
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_11").Trim()))}</h3>");
            char[] split_osd = { ':' };
            string osd_header = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_4").Trim()));
            string[] osd_texts = osd_header.Split(split_osd);
            PrintEngineList.Add($"\t\t\t<h4><span>{osd_texts[0]}:</span><span>{osd_texts[1]}</span></h4>");
            try{
                PrintEngineList.Add("\t\t\t<ul>");
                for (int i = 0; i < OSD_DataMainTable.Rows.Count; i++){
                    PrintEngineList.Add($"\t\t\t\t<li>{OSD_DataMainTable.Rows[i].Cells[0].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[1].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[2].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[3].Value.ToString() + " | " + OSD_DataMainTable.Rows[i].Cells[4].Value.ToString()}</li>");
                }
                PrintEngineList.Add("\t\t\t</ul>");
            }catch (Exception){ }
            PrintEngineList.Add($"\t\t\t<h4><span>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Osd", "osd_9").Trim()))}</span><span>{OSD_TYSS_V.Text}</span></h4>");
            PrintEngineList.Add("\t\t</div>");
            // SERVICE
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Header", "header_12").Trim()))}</h3>");
            char[] split_service = { ':' };
            string service_header = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_5").Trim()));
            string[] service_texts = service_header.Split(split_service);
            PrintEngineList.Add($"\t\t\t<h4><span>{service_texts[0]}:</span><span>{service_texts[1]}</span></h4>");
            try{
                PrintEngineList.Add("\t\t\t<ul>");
                for (int i = 0; i < SERVICE_DataMainTable.Rows.Count; i++){
                    PrintEngineList.Add($"\t\t\t\t<li>{SERVICE_DataMainTable.Rows[i].Cells[0].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[1].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[2].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[3].Value.ToString() + " | " + SERVICE_DataMainTable.Rows[i].Cells[4].Value.ToString()}</li>");
                }
                PrintEngineList.Add("\t\t\t</ul>");
            }catch (Exception){ }
            PrintEngineList.Add($"\t\t\t<h4><span>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("Services", "ss_9").Trim()))}</span><span>{SERVICE_TYS_V.Text}</span></h4>");
            PrintEngineList.Add("\t\t</div>");
            // FOOTER V1
            PrintEngineList.Add("\t\t<div class='middle_box'>");
            PrintEngineList.Add($"\t\t\t<h3>{string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_1").Trim())), Application.ProductName.ToUpper())}</h3>");
            PrintEngineList.Add("\t\t\t<ul>");
            PrintEngineList.Add($"\t\t\t\t<li>{Application.ProductName + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_6").Trim())) + " " + Application.ProductVersion.Substring(0, 4)}</li>");
            PrintEngineList.Add($"\t\t\t\t<li>(C) {DateTime.Now.Year} {Application.CompanyName}.</li>");
            PrintEngineList.Add($"\t\t\t\t<li>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_7").Trim())) + " " + DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss")}</li>");
            PrintEngineList.Add($"\t\t\t\t<li>{Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_8").Trim())) + " " + "<a target='_blank' href='" + github_link + "'>" + github_link + "</a>"}</li>");
            PrintEngineList.Add("\t\t\t</ul>");
            PrintEngineList.Add("\t\t</div>");
            // MAIN CONTAINER END
            PrintEngineList.Add("\t</div>");
            // SCROOL TOP SCRIPT
            PrintEngineList.Add("<script>");
            PrintEngineList.Add("\tconst ts_scroll_top = document.querySelector('.ts_scroll_top');");
            PrintEngineList.Add("\twindow.addEventListener('scroll', function(){ ts_scroll_top.classList.toggle('active', window.scrollY > 350); });");
            PrintEngineList.Add("\tfunction ts_scroll_to_up(){ window.scrollTo({ top: 0, behavior: 'smooth' }); }");
            PrintEngineList.Add("</script>");
            // DOCUMENT END
            PrintEngineList.Add("</body>");
            PrintEngineList.Add("</html>");
            // FOOTER V2
            SaveFileDialog save_engine = new SaveFileDialog{
                InitialDirectory = @"C:\Users\" + SystemInformation.UserName + @"\Desktop\",
                Title = Application.ProductName + " - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_9").Trim())),
                DefaultExt = "html",
                FileName = Application.ProductName + " - " + string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_10").Trim())), OS_ComputerName_V.Text),
                Filter = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_12").Trim())) + " (*.html)|*.html"
            };
            if (save_engine.ShowDialog() == DialogResult.OK){
                String[] text_engine = new String[PrintEngineList.Count];
                PrintEngineList.CopyTo(text_engine, 0);
                File.WriteAllLines(save_engine.FileName, text_engine);
                DialogResult glow_print_engine_query = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_13").Trim())) + Environment.NewLine + Environment.NewLine + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("PrintEngine", "pe_14").Trim())), Application.ProductName, save_engine.FileName), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (glow_print_engine_query == DialogResult.Yes){ Process.Start(save_engine.FileName); }
                PrintEngineList.Clear(); save_engine.Dispose();
            }else{
                PrintEngineList.Clear(); save_engine.Dispose();
            }
        }
        // ======================================================================================================
        // SFC AND DISM AUTO TOOL
        private void sFCandDISMAutoToolToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                GlowSFCandDISMAutoTool sfc_and_dism_tool = new GlowSFCandDISMAutoTool();
                string glow_tool_name = "glow_sfc_and_dism_tool";
                sfc_and_dism_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    sfc_and_dism_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    Application.OpenForms[glow_tool_name].Activate();
                    MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderToolsInfo", "hti_info").Trim())), Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderTools", "ht_1").Trim()))), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }catch (Exception){ }
        }
        // ======================================================================================================
        // CACHE CLEANUP TOOL
        private void cacheCleaningToolToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                GlowGetLangs g_lang = new GlowGetLangs(lang_path);
                GlowCacheCleanupTool cache_cleanup_tool = new GlowCacheCleanupTool();
                string glow_tool_name = "glow_cache_cleanup_tool";
                cache_cleanup_tool.Name = glow_tool_name;
                if (Application.OpenForms[glow_tool_name] == null){
                    cache_cleanup_tool.Show();
                }else{
                    if (Application.OpenForms[glow_tool_name].WindowState == FormWindowState.Minimized){
                        Application.OpenForms[glow_tool_name].WindowState = FormWindowState.Normal;
                    }
                    Application.OpenForms[glow_tool_name].Activate();
                    MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderToolsInfo", "hti_info").Trim())), Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("HeaderTools", "ht_2").Trim()))), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }catch (Exception){ }
        }
        // ======================================================================================================
        // ROINES45 GITHUB
        private void gitHubToolStripMenuItem_Click(object sender, EventArgs e){
            try{
                Process.Start(github_link);
            }catch (Exception){ }
        }
        // ======================================================================================================
        // GLOW EXIT
        private void glow_exit(){ loop_status = false; Application.Exit(); }
        private void Glow_FormClosing(object sender, FormClosingEventArgs e){ glow_exit(); }
    }
}