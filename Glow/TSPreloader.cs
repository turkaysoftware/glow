using System;
// File and I/O Operations
using System.IO;
// LINQ Queries
using System.Linq;
// Windows Registry Operations
using Microsoft.Win32;
// Multithreading and Parallel Processing
using System.Threading;
using System.Threading.Tasks;
// Reflection and Runtime Operations
using System.Reflection;
// Diagnostics and Management Tools
using System.Diagnostics;
// Language and Culture Settings
using System.Globalization;
// User Interface Operations
using System.Windows.Forms;
// General Collections
using System.Collections.Generic;
// TS Modules
using static Glow.TSModules;

namespace Glow{
    public partial class TSPreloader : Form{
        public TSPreloader(){
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //
            Program.TS_TokenEngine = new CancellationTokenSource();
            //
            TSSetImagePanelPadding(0);
            //
            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, PanelLoaderBG, new object[] { true });
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
        private void TSPreloader_Load(object sender, EventArgs e){
            LabelDeveloper.Text = Application.CompanyName;
            LabelSoftware.Text = Application.ProductName;
            LabelVersion.Text = TS_VersionEngine.TS_SofwareVersion(1, Program.ts_version_mode);
            LabelCopyright.Text = TS_SoftwareCopyrightDate.ts_scd_preloader;
            //
            ImageWelcome.BackgroundImage = Properties.Resources.ts_preloader_release;
            ImageWelcome.BackgroundImageLayout = ImageLayout.Zoom;
            //
            software_preloader();
            software_set_launch();
            //
            if (Program.ts_pre_debug_mode == true){
                LabelLoader.Text = "Loading - 50%";
                PanelLoaderFE.Width = (int)(PanelLoaderBG.Width * 0.5);
            }else{
                Task.Run(() => load_animation(), Program.TS_TokenEngine.Token);
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
        private void software_preloader(){
            try{
                // CHECK LANGS FOLDER
                if (!Directory.Exists(ts_lf)){
                    software_prelaoder_alert(0);
                    return;
                }
                // CHECK LANGS FILE
                var lang_files = Directory.GetFiles(ts_lf, "*.ini");
                if (lang_files.Length == 0){
                    software_prelaoder_alert(1);
                    return;
                }
                // CHECK ENGLISH LANG FILE
                if (!File.Exists(ts_lang_en)){
                    software_prelaoder_alert(2);
                    return;
                }
                // CHECK SETTINGS FILE
                if (!File.Exists(ts_sf)){
                    try{
                        string ui_lang = CultureInfo.InstalledUICulture.TwoLetterISOLanguageName.Trim();
                        TSSettingsSave software_settings_save = new TSSettingsSave(ts_sf);
                        // SET SYSTEM THEME
                        string get_system_theme = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Themes\Personalize", "SystemUsesLightTheme", "").ToString().Trim();
                        software_settings_save.TSWriteSettings(ts_settings_container, "ThemeStatus", get_system_theme);
                        // SET SOFTWARE LANGUAGE
                        string languageSetting = new[]{
                            ts_lang_zh,
                            ts_lang_fr,
                            ts_lang_de,
                            ts_lang_it,
                            ts_lang_ko,
                            ts_lang_pt,
                            ts_lang_ru,
                            ts_lang_es,
                            ts_lang_tr,
                        }.Any(File.Exists) && !string.IsNullOrEmpty(ui_lang) ? ui_lang : "en";
                        software_settings_save.TSWriteSettings(ts_settings_container, "LanguageStatus", languageSetting);
                        // SET STARTUP MODE
                        software_settings_save.TSWriteSettings(ts_settings_container, "InitialStatus", "0");
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
        private void software_prelaoder_alert(int pre_mode){
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
        public void software_set_launch(){
            try{
                TSSettingsSave software_read_settings = new TSSettingsSave(ts_sf);
                //
                string theme_mode = software_read_settings.TSReadSettings(ts_settings_container, "ThemeStatus") ?? "1";
                int global_theme = Convert.ToInt32(theme_mode);
                // DWM SET
                try{
                    int set_attribute = global_theme == 1 ? 20 : 19;
                    if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != global_theme){
                        DwmSetWindowAttribute(Handle, 20, new[] { global_theme == 1 ? 0 : 1 }, 4);
                    }
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
                var languageFiles = new Dictionary<string, string> {
                    { "zh", ts_lang_zh },
                    { "en", ts_lang_en },
                    { "fr", ts_lang_fr },
                    { "de", ts_lang_de },
                    { "it", ts_lang_it },
                    { "ko", ts_lang_ko },
                    { "pt", ts_lang_pt },
                    { "ru", ts_lang_ru },
                    { "es", ts_lang_es },
                    { "tr", ts_lang_tr },
                };
                //
                string lang_mode = TS_String_Encoder(software_read_settings.TSReadSettings(ts_settings_container, "LanguageStatus")) ?? "en";
                string lang_file;
                bool isFileExist = languageFiles.ContainsKey(lang_mode) && File.Exists(languageFiles[lang_mode]);
                //
                if (isFileExist){
                    lang_file = languageFiles[lang_mode];
                }else{
                    lang_file = languageFiles.ContainsKey("en") && File.Exists(languageFiles["en"]) ? languageFiles["en"] : ts_lang_en;
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
                Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("TSPreloader", "tsbt_title")), Application.CompanyName);
                load_text = TS_String_Encoder(software_lang.TSReadLangs("TSPreloader", "tsbt_load"));
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
        private async Task load_animation(){
            int progress_interval = 0;
            int progress_increment = 2;
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
            await Task.Run(() => {
                Invoke(new Action(() => {
                    Glow glow = new Glow();
                    glow.Show();
                    //
                    Hide();
                }));
            });
        }
    }
}