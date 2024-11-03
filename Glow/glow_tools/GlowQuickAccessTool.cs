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
                        render_color_button.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                        render_color_button.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                        render_color_button.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                        render_color_button.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRightHover");
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
        // ABOUT
        private void QB_About_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:about");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // ADVANCED SCREEN SETTINGS
        private void QB_AdvancedScreenSettings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:display-advancedgraphics");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // BACKGROUND
        private void QB_Background_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:personalization-background");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // BLUETOOTH
        private void QB_Bluetooth_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:bluetooth");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // COLORS
        private void QB_Colors_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:personalization-colors");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // CONTROL PANEL
        private void QB_ControlPanel_Click(object sender, EventArgs e){
            try{
                Process.Start("control");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // COUNTRY / REGION
        private void QB_Country_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:regionformatting");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // DATA USAGE
        private void QB_DataUsage_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:datausage");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // DATE AND TIME
        private void QB_DateAndTime_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:dateandtime");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // DEFAULT APPS
        private void QB_DefaultApps_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:defaultapps");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DEVICE MANAGER
        private void QB_DeviceManager_Click(object sender, EventArgs e){
            try{
                Process.Start("devmgmt.msc");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // DIRECT-X
        private void QB_DirectX_Click(object sender, EventArgs e){
            try{
                Process.Start("dxdiag");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // DISK MANAGEMENT
        private void QB_DiskManagement_Click(object sender, EventArgs e){
            try{
                Process.Start("diskmgmt.msc");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // EVENT VIEWER
        private void QB_EventViewer_Click(object sender, EventArgs e){
            try{
                Process.Start("eventvwr.msc");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // GAMING MODE
        private void QB_GamingMode_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:gaming-gamemode");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // LANGUAGE
        private void QB_Language_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:regionlanguage");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // LICENSE STATUS
        private void QB_LicenseStatus_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:activation");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // LOCK SCREEN
        private void QB_LockScreen_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:lockscreen");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // MOUSE SETTINGS
        private void QB_MouseSettings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:mousetouchpad");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // NOTIFICATION SETTINGS
        private void QB_NotificationSettings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:notifications");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // PHONE
        private void QB_Phone_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:mobile-devices");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // SCREEN SETTINGS
        private void QB_ScreenSettings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:display");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // SETTINGS
        private void QB_Settings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // SOUND
        private void QB_SoundSettings_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:sound");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // START
        private void QB_Start_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:personalization-start");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // STORAGE
        private void QB_Storage_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:storagesense");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // TASKBAR
        private void QB_Taskbar_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:taskbar");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // USB
        private void QB_USB_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:usb");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // WINDOWS DEFENDER
        private void QB_WindowsDefender_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:windowsdefender");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // WINDOWS UPDATE
        private void QB_WindowsUpdate_Click(object sender, EventArgs e){
            try{
                Process.Start("ms-settings:windowsupdate");
            }catch (Exception ex){
                MessageBox.Show(string.Format(TS_String_Encoder(software_lang.TSReadLangs("QuickAccessTool", "qat_launch_error")), ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}