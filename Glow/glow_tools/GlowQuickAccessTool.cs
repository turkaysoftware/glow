using System;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
//
using static Glow.GlowModules;

namespace Glow.glow_tools{
    public partial class GlowQuickAccessTool : Form{
        public GlowQuickAccessTool(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // GLOBAL LANG
        // ======================================================================================================
        TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
        public void quick_access_settings(){
            try{
                if (Glow.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                }else if (Glow.theme == 0){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                }
                //
                BackColor = Glow.ui_colors[5];
                QUICK_FLY.BackColor = Glow.ui_colors[6];
                //
                foreach (Control ui_buttons in QUICK_FLY.Controls){
                    if (ui_buttons is Button render_color_button){
                        render_color_button.ForeColor = Glow.ui_colors[18];
                        render_color_button.BackColor = Glow.ui_colors[8];
                        render_color_button.FlatAppearance.BorderColor = Glow.ui_colors[8];
                    }
                }
                //
                TSGetLangs g_lang_internal = new TSGetLangs(Glow.lang_path);
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_title").Trim())), Application.ProductName);
                //
                QB_About.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_about").Trim()));
                QB_AdvancedScreenSettings.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_ascreen_settings").Trim()));
                QB_Background.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_background").Trim()));
                QB_Bluetooth.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_bluetooth").Trim()));
                QB_Colors.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_colors").Trim()));
                QB_ControlPanel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_control_panel").Trim()));
                QB_Country.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_country_region").Trim()));
                QB_DataUsage.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_data_usage").Trim()));
                QB_DateAndTime.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_date_and_time").Trim()));
                QB_DefaultApps.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_default_apps").Trim()));
                QB_DeviceManager.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_device_manager").Trim()));
                QB_DirectX.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_directx").Trim()));
                QB_DiskManagement.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_disk_management").Trim()));
                QB_EventViewer.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_event_viewer").Trim()));
                QB_GamingMode.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_gaming_mode").Trim()));
                QB_Language.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_language").Trim()));
                QB_LicenseStatus.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_license_status").Trim()));
                QB_LockScreen.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_lock_screen").Trim()));
                QB_MouseSettings.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_mouse_settings").Trim()));
                QB_NotificationSettings.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_notification_settings").Trim()));
                QB_Phone.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_phone").Trim()));
                QB_ScreenSettings.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_screen_settings").Trim()));
                QB_Settings.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_settings").Trim()));
                QB_SoundSettings.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_sound_settings").Trim()));
                QB_Start.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_start").Trim()));
                QB_Storage.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_storage").Trim()));
                QB_Taskbar.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_taskbar").Trim()));
                QB_USB.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_usb").Trim()));
                QB_WindowsDefender.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_windows_defender").Trim()));
                QB_WindowsUpdate.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang_internal.TSReadLangs("QuickAccessTool", "qat_windows_update").Trim()));
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
        private void GlowQuickAccessTool_Load(object sender, EventArgs e){
            typeof(FlowLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, QUICK_FLY, new object[]{ true });
            quick_access_settings();
        }
        // ABOUT
        private void QB_About_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:about");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ADVANCED SCREEN SETTINGS
        private void QB_AdvancedScreenSettings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:display-advancedgraphics");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // BACKGROUND
        private void QB_Background_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:personalization-background");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // BLUETOOTH
        private void QB_Bluetooth_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:bluetooth");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // COLORS
        private void QB_Colors_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:personalization-colors");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // CONTROL PANEL
        private void QB_ControlPanel_Click(object sender, EventArgs e){
            try{
                Process.Start("control");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // COUNTRY / REGION
        private void QB_Country_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:regionformatting");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // DATA USAGE
        private void QB_DataUsage_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:datausage");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // DATE AND TIME
        private void QB_DateAndTime_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:dateandtime");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // DEFAULT APPS
        private void QB_DefaultApps_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:defaultapps");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DEVICE MANAGER
        private void QB_DeviceManager_Click(object sender, EventArgs e){
            try{
                Process.Start("devmgmt.msc");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // DIRECT-X
        private void QB_DirectX_Click(object sender, EventArgs e){
            try{
                Process.Start("dxdiag");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // DISK MANAGEMENT
        private void QB_DiskManagement_Click(object sender, EventArgs e){
            try{
                Process.Start("diskmgmt.msc");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // EVENT VIEWER
        private void QB_EventViewer_Click(object sender, EventArgs e){
            try{
                Process.Start("eventvwr.msc");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // GAMING MODE
        private void QB_GamingMode_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:gaming-gamemode");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // LANGUAGE
        private void QB_Language_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:regionlanguage");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // LICENSE STATUS
        private void QB_LicenseStatus_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:activation");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // LOCK SCREEN
        private void QB_LockScreen_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:lockscreen");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // MOUSE SETTINGS
        private void QB_MouseSettings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:mousetouchpad");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // NOTIFICATION SETTINGS
        private void QB_NotificationSettings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:notifications");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // PHONE
        private void QB_Phone_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:mobile-devices");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // SCREEN SETTINGS
        private void QB_ScreenSettings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:display");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // SETTINGS
        private void QB_Settings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // SOUND
        private void QB_SoundSettings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:sound");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // START
        private void QB_Start_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:personalization-start");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // STORAGE
        private void QB_Storage_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:storagesense");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // TASKBAR
        private void QB_Taskbar_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:taskbar");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // USB
        private void QB_USB_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:usb");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // WINDOWS DEFENDER
        private void QB_WindowsDefender_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:windowsdefender");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // WINDOWS UPDATE
        private void QB_WindowsUpdate_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:windowsupdate");
            }catch (Exception ex){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("QuickAccessTool", "qat_launch_error").Trim())), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}