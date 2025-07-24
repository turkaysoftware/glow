using System;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowQuickAccessTool : Form{
        public GlowQuickAccessTool(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // GLOBAL LANG
        // ======================================================================================================
        TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
        public void quick_access_settings(){
            try{
                int set_attribute = Glow.theme == 1 ? 20 : 19;
                if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != Glow.theme){
                    DwmSetWindowAttribute(Handle, 20, new[] { Glow.theme == 1 ? 0 : 1 }, 4);
                }
                //
                BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                QUICK_FLY.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                //
                foreach (Control ui_buttons in QUICK_FLY.Controls){
                    if (ui_buttons is Button render_color_button){
                        render_color_button.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                        render_color_button.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                        render_color_button.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                        render_color_button.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                        render_color_button.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMainHover");
                    }
                }
                //
                TSGetLangs g_lang_internal = new TSGetLangs(Glow.lang_path);
                Text = string.Format(TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_title")), Application.ProductName);
                //
                QB_About.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_about"));
                QB_AdvancedScreenSettings.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_ascreen_settings"));
                QB_Background.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_background"));
                QB_Bluetooth.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_bluetooth"));
                QB_Colors.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_colors"));
                QB_ControlPanel.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_control_panel"));
                QB_Country.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_country_region"));
                QB_DataUsage.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_data_usage"));
                QB_DateAndTime.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_date_and_time"));
                QB_DefaultApps.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_default_apps"));
                QB_DeviceManager.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_device_manager"));
                QB_DirectX.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_directx"));
                QB_DiskManagement.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_disk_management"));
                QB_EventViewer.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_event_viewer"));
                QB_GamingMode.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_gaming_mode"));
                QB_Language.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_language"));
                QB_LicenseStatus.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_license_status"));
                QB_LockScreen.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_lock_screen"));
                QB_MouseSettings.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_mouse_settings"));
                QB_NotificationSettings.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_notification_settings"));
                QB_Phone.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_phone"));
                QB_ScreenSettings.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_screen_settings"));
                QB_Settings.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_settings"));
                QB_SoundSettings.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_sound_settings"));
                QB_Start.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_start"));
                QB_Storage.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_storage"));
                QB_Taskbar.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_taskbar"));
                QB_USB.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_usb"));
                QB_WindowsDefender.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_windows_defender"));
                QB_WindowsUpdate.Text = TS_String_Encoder(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_windows_update"));
                // DYNAMIC LOCATION BUTTON ALGORITHM
                var get_fly_buttons = QUICK_FLY.Controls.OfType<Button>().ToList();
                get_fly_buttons.Sort((before_button, after_button) => before_button.Text.CompareTo(after_button.Text));
                QUICK_FLY.SuspendLayout();
                foreach (var render_button in get_fly_buttons){
                    QUICK_FLY.Controls.SetChildIndex(render_button, get_fly_buttons.IndexOf(render_button));
                }
                QUICK_FLY.ResumeLayout();
            }catch (Exception){ }
        }
        // LOAD
        // ======================================================================================================
        private void GlowQuickAccessTool_Load(object sender, EventArgs e){
            typeof(FlowLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, QUICK_FLY, new object[]{ true });
            quick_access_settings();
        }
        // LAUNCHER QUICK TOOLS
        // =============================
        private void QuickLauncher(string run_command){
            try{
                Process.Start(new ProcessStartInfo(run_command){ UseShellExecute = true });
            }catch (Exception ex){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message));
            }
        }
        // ABOUT
        private void QB_About_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:about");
        // ADVANCED SCREEN SETTINGS
        private void QB_AdvancedScreenSettings_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:display-advancedgraphics");
        // BACKGROUND
        private void QB_Background_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:personalization-background");
        // BLUETOOTH
        private void QB_Bluetooth_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:bluetooth");
        // COLORS
        private void QB_Colors_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:personalization-colors");
        // CONTROL PANEL
        private void QB_ControlPanel_Click(object sender, EventArgs e) => QuickLauncher("control");
        // COUNTRY / REGION
        private void QB_Country_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:regionformatting");
        // DATA USAGE
        private void QB_DataUsage_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:datausage");
        // DATE AND TIME
        private void QB_DateAndTime_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:dateandtime");
        // DEFAULT APPS
        private void QB_DefaultApps_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:defaultapps");
        // DEVICE MANAGER
        private void QB_DeviceManager_Click(object sender, EventArgs e) => QuickLauncher("devmgmt.msc");
        // DIRECT-X
        private void QB_DirectX_Click(object sender, EventArgs e) => QuickLauncher("dxdiag");
        // DISK MANAGEMENT
        private void QB_DiskManagement_Click(object sender, EventArgs e) => QuickLauncher("diskmgmt.msc");
        // EVENT VIEWER
        private void QB_EventViewer_Click(object sender, EventArgs e) => QuickLauncher("eventvwr.msc");
        // GAMING MODE
        private void QB_GamingMode_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:gaming-gamemode");
        // LANGUAGE
        private void QB_Language_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:regionlanguage");
        // LICENSE STATUS
        private void QB_LicenseStatus_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:activation");
        // LOCK SCREEN
        private void QB_LockScreen_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:lockscreen");
        // MOUSE SETTINGS
        private void QB_MouseSettings_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:mousetouchpad");
        // NOTIFICATION SETTINGS
        private void QB_NotificationSettings_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:notifications");
        // PHONE
        private void QB_Phone_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:mobile-devices");
        // SCREEN SETTINGS
        private void QB_ScreenSettings_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:display");
        // SETTINGS
        private void QB_Settings_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:");
        // SOUND
        private void QB_SoundSettings_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:sound");
        // START MENU
        private void QB_Start_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:personalization-start");
        // STORAGE
        private void QB_Storage_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:storagesense");
        // TASKBAR
        private void QB_Taskbar_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:taskbar");
        // USB SETTINGS
        private void QB_USB_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:usb");
        // WINDOWS DEFENDER
        private void QB_WindowsDefender_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:windowsdefender");
        // WINDOWS UPDATE
        private void QB_WindowsUpdate_Click(object sender, EventArgs e) => QuickLauncher("ms-settings:windowsupdate");
    }
}