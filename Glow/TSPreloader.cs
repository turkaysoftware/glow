using System;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
// TS Modules
using static Glow.TSModules;

namespace Glow{
    public partial class TSPreloader : Form{
        public TSPreloader(){
            InitializeComponent();
            //
            Program.TS_TokenEngine = new CancellationTokenSource();
            TSSetImagePanelPadding(0);
            //
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, PanelLoaderFE, new object[] { true });
        }
        // VARIABLES
        // ======================================================================================================
        private string load_text;
        // LOAD IMAGE PADDING
        // ======================================================================================================
        private void TSSetImagePanelPadding(int get_padding){
            PanelImg.Padding = new Padding(get_padding, get_padding, get_padding, get_padding);
        }
        // LOAD
        // ======================================================================================================
        private async void TSPreloader_Load(object sender, EventArgs e){
            LabelDeveloper.Text = Application.CompanyName;
            LabelSoftware.Text = Application.ProductName;
            LabelVersion.Text = TS_VersionEngine.TS_SofwareVersion(1, Program.ts_version_mode);
            LabelCopyright.Text = TS_SoftwareCopyrightDate.ts_scd_preloader;
            //
            ImageWelcome.BackgroundImage = Properties.Resources.ts_preloader_release;
            ImageWelcome.BackgroundImageLayout = ImageLayout.Zoom;
            //
            Software_preloader();
            Software_set_launch();
            //
            if (Program.ts_pre_debug_mode == true){
                LabelLoader.Text = "Loading - 50%";
                PanelLoaderFE.Width = (int)(PanelLoaderBG.Width * 0.5);
            }else{
                await Load_animation();
            }
        }
        // SOFTWARE PRELOADER
        // ======================================================================================================
        /*
        |   -- THEME --       |  -- LANGUAGE --       |  -- INITIAL MODE --     |  -- HIDING MODE --
        |   ------------------------------------------------------------------------------------------
        |   0 = Dark Theme    |  Moved to             |  0 = Windowed           |  0 = Off
        |   1 = Light Theme   |  TSModules.cs         |  1 = Full Screen        |  1 = On
        |   ------------------------------------------------------------------------------------------
        */
        private void Software_preloader(){
            try{
                // CHECK LANGS FOLDER
                if (!Directory.Exists(ts_lf)){
                    Software_prelaoder_alert(0);
                    return;
                }
                // CHECK LANGS FILE
                var lang_files = Directory.GetFiles(ts_lf, "*.ini");
                if (lang_files.Length == 0){
                    Software_prelaoder_alert(1);
                    return;
                }
                // CHECK ENGLISH LANG FILE
                if (!File.Exists(ts_lang_en)){
                    Software_prelaoder_alert(2);
                    return;
                }
                // CHECK SETTINGS FILE
                if (!File.Exists(ts_sf)){
                    try{
                        string ui_lang = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName.Trim();
                        TSSettingsSave software_settings_save = new TSSettingsSave(ts_sf);
                        // SET SYSTEM THEME
                        string get_system_theme = (Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", "")?.ToString() ?? "1").Trim();
                        software_settings_save.TSWriteSettings(ts_settings_container, "ThemeStatus", get_system_theme);
                        // SET SOFTWARE LANGUAGE
                        software_settings_save.TSWriteSettings(ts_settings_container, "LanguageStatus", TSPreloaderSetDefaultLanguage(ui_lang));
                        // SET STARTUP MODE
                        software_settings_save.TSWriteSettings(ts_settings_container, "StartupStatus", "0");
                        // SET HIDING MODE
                        software_settings_save.TSWriteSettings(ts_settings_container, "HidingStatus", "0");
                    }catch (Exception ex){
                        // ERROR LOG
                        LogError(ex);
                    }
                }
            }catch (IOException ioEx){
                // IO ERROR LOG
                LogError(ioEx);
            }catch (UnauthorizedAccessException uaEx){
                // ACCESS ERROR LOG
                LogError(uaEx);
            }catch (Exception ex){
                // OTHER ERROR LOG
                LogError(ex);
            }
        }
        // PRELOAD ALERT
        // ======================================================================================================
        private void Software_prelaoder_alert(int pre_mode){
            string set_message = string.Empty;
            switch (pre_mode){
                case 0:
                    set_message = $"{ts_lf} folder not found.\nThe folder seems to be missing.\n\nWould you like to view and download the latest version of {Application.ProductName} again?";
                    break;
                case 1:
                    set_message = $"No language file found.\nThere seems to be a problem with the language files.\n\nWould you like to view and download the latest version of {Application.ProductName} again?";
                    break;
                case 2:
                    set_message = $"English language (English.ini) is a compulsory language. The English.ini file seems to be missing.\n\nWould you like to view and download the latest version of {Application.ProductName} again?";
                    break;
            }
            if (!string.IsNullOrEmpty(set_message)){
                DialogResult open_last_release = TS_MessageBoxEngine.TS_MessageBox(this, 7, set_message);
                if (open_last_release == DialogResult.Yes){
                    Process.Start(new ProcessStartInfo(TS_LinkSystem.github_link_lr){ UseShellExecute = true });
                }else{
                    Application.Exit();
                }
            }
        }
        // ERROR LOG FUNCTION
        // ======================================================================================================
        private void LogError(Exception ex){
            TS_MessageBoxEngine.TS_MessageBox(this, 3, ex.Message);
        }
        // BOOTSTRAPPER PRELOADER
        // ======================================================================================================
        public void Software_set_launch(){
            try{
                TSSettingsSave software_read_settings = new TSSettingsSave(ts_sf);
                //
                string theme_mode = software_read_settings.TSReadSettings(ts_settings_container, "ThemeStatus") ?? "1";
                int global_theme = Convert.ToInt32(theme_mode);
                // DWM SET
                try{
                    TSSetWindowTheme(Handle, GlowMain.theme);
                }catch (Exception){ }
                //
                BackColor = TS_ThemeEngine.ColorMode(global_theme, "TSBT_BGColor");
                PanelTxt.BackColor = TS_ThemeEngine.ColorMode(global_theme, "TSBT_BGColor");
                //
                LabelDeveloper.ForeColor = TS_ThemeEngine.ColorMode(global_theme, "TSBT_LabelColor1");
                LabelSoftware.ForeColor = TS_ThemeEngine.ColorMode(global_theme, "TSBT_AccentColor");
                LabelVersion.ForeColor = TS_ThemeEngine.ColorMode(global_theme, "TSBT_LabelColor2");
                PanelLoaderBG.BackColor = TS_ThemeEngine.ColorMode(global_theme, "TSBT_BGColor2");
                PanelLoaderFE.BackColor = TS_ThemeEngine.ColorMode(global_theme, "TSBT_AccentColor");
                LabelLoader.ForeColor = TS_ThemeEngine.ColorMode(global_theme, "TSBT_LabelColor1");
                LabelCopyright.ForeColor = TS_ThemeEngine.ColorMode(global_theme, "TSBT_LabelColor2");
                // DICTIONARY SYSTEM FOR LANGUAGE
                string lang_mode = software_read_settings.TSReadSettings(ts_settings_container, "LanguageStatus") ?? "en";
                string lang_file;
                bool isFileExist = AllLanguageFiles.ContainsKey(lang_mode) && File.Exists(AllLanguageFiles[lang_mode]);
                //
                if (isFileExist){
                    lang_file = AllLanguageFiles[lang_mode];
                }else{
                    lang_file = AllLanguageFiles.ContainsKey("en") && File.Exists(AllLanguageFiles["en"]) ? AllLanguageFiles["en"] : ts_lang_en;
                    lang_mode = "en";
                }
                //
                try{
                    if (!isFileExist){
                        TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                        software_setting_save.TSWriteSettings(ts_settings_container, "LanguageStatus", lang_mode);
                    }
                }catch (Exception){ }
                //
                TSGetLangs software_lang = new TSGetLangs(lang_file);
                Text = string.Format(software_lang.TSReadLangs("TSPreloader", "tsbt_title"), Application.CompanyName);
                load_text = software_lang.TSReadLangs("TSPreloader", "tsbt_load");
            }catch (Exception){ }
        }
        // PROGRESS BAR & PROGRESS TEXT PROCESS
        // ======================================================================================================
        private void TSProgressExecutive(int get_per){
            if (InvokeRequired){
                Invoke(new Action<int>(TSProgressExecutive), get_per);
                return;
            }
            PanelLoaderFE.Width = (int)(PanelLoaderBG.Width * (get_per / 100.0));
            LabelLoader.Text = string.Format("{0} - {1}%", load_text, get_per);
        }
        // LOAD ANIMATION
        // ======================================================================================================
        private async Task Load_animation(){
            int progress_interval = 0;
            int progress_increment = 3;
            int progress_delay = 5;
            //
            TSProgressExecutive(0);
            //
            while (progress_interval < 100){
                TSProgressExecutive(progress_interval);
                if (progress_interval + progress_increment >= 100){
                    progress_interval = 100;
                    TSProgressExecutive(progress_interval);
                    break;
                }
                progress_interval += progress_increment;
                await Task.Delay(progress_delay);
            }
            //
            Invoke(new Action(() => {
                GlowMain glow = new GlowMain();
                glow.Show();
                Hide();
            }));
        }
    }
}