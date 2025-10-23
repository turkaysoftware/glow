using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Globalization;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Glow{
    internal class TSModules{
        // LINK SYSTEM
        // ======================================================================================================
        public class TS_LinkSystem{
            public const string
            // Main Control Links
            github_link_lv      = "https://raw.githubusercontent.com/turkaysoftware/glow/main/Glow/SoftwareVersion.txt",
            github_link_lr      = "https://github.com/turkaysoftware/glow/releases/latest",
            // Social Links
            website_link        = "https://www.turkaysoftware.com",
            github_link         = "https://github.com/turkaysoftware",
            // Other Links
            ts_wizard           = "https://www.turkaysoftware.com/ts-wizard",
            ts_donate           = "https://buymeacoffee.com/turkaysoftware";
        }
        // VERSIONS
        // ======================================================================================================
        public class TS_VersionEngine{
            public static string TS_SofwareVersion(int v_type, int v_mode){
                string version_mode = "";
                string versionSubstring = v_mode == 0 ? Application.ProductVersion.Substring(0, 5) : Application.ProductVersion.Substring(0, 7);
                switch (v_type){
                    case 0:
                        version_mode = v_mode == 0 ? $"{Application.ProductName} - v{versionSubstring}" : $"{Application.ProductName} - v{Application.ProductVersion.Substring(0, 7)}";
                        break;
                    case 1:
                        version_mode = $"v{versionSubstring}";
                        break;
                    case 2:
                        version_mode = versionSubstring;
                        break;
                    case 3:
                        version_mode = Application.ProductName;
                        break;
                    default:
                        break;
                }
                return version_mode;
            }
        }
        // TS MESSAGEBOX ENGINE
        // ======================================================================================================
        public static class TS_MessageBoxEngine{
            private static readonly Dictionary<int, KeyValuePair<MessageBoxButtons, MessageBoxIcon>> TSMessageBoxConfig = new Dictionary<int, KeyValuePair<MessageBoxButtons, MessageBoxIcon>>(){
                { 1, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.OK, MessageBoxIcon.Information) },           // Ok and Info
                { 2, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.OK, MessageBoxIcon.Warning) },               // Ok and Warning
                { 3, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.OK, MessageBoxIcon.Error) },                 // Ok and Error
                { 4, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Question) },           // Yes/No and Quest
                { 5, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Information) },        // Yes/No and Info
                { 6, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Warning) },            // Yes/No and Warning
                { 7, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Error) },              // Yes/No and Error
                { 8, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) },        // Retry/Cancel and Error
                { 9, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) },     // Yes/No/Cancel and Quest
                { 10, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) }  // Yes/No/Cancel and Info
            };
            public static DialogResult TS_MessageBox(Form m_form, int m_mode, string m_message, string m_title = ""){
                if (m_form.InvokeRequired){
                    m_form.Invoke((Action)(() => BringFormToFront(m_form)));
                }else{
                    BringFormToFront(m_form);
                }
                //
                string m_box_title = string.IsNullOrEmpty(m_title) ? Application.ProductName : m_title;
                //
                MessageBoxButtons m_button = MessageBoxButtons.OK;
                MessageBoxIcon m_icon = MessageBoxIcon.Information;
                //
                if (TSMessageBoxConfig.ContainsKey(m_mode)){
                    var m_serialize = TSMessageBoxConfig[m_mode];
                    m_button = m_serialize.Key;
                    m_icon = m_serialize.Value;
                }
                //
                return MessageBox.Show(m_form, m_message, m_box_title, m_button, m_icon);
            }
            private static void BringFormToFront(Form m_form){
                if (m_form.WindowState == FormWindowState.Minimized)
                    m_form.WindowState = FormWindowState.Normal;
                m_form.BringToFront();
                m_form.Activate();
            }
        }
        // TS SOFTWARE COPYRIGHT DATE
        // ======================================================================================================
        public class TS_SoftwareCopyrightDate{
            public static string ts_scd_preloader = string.Format("\u00a9 2019-{0}, {1}.", DateTime.Now.Year, Application.CompanyName);
        }
        // SETTINGS SAVE PATHS
        // ======================================================================================================
        public static string ts_df = Application.StartupPath;
        public static string ts_sf = ts_df + @"\" + Application.ProductName + "Settings.ini";
        public static string ts_settings_container = Path.GetFileNameWithoutExtension(ts_sf);
        // SETTINGS SAVE CLASS
        // ======================================================================================================
        public class TSSettingsSave{
            private readonly string _iniFilePath;
            private readonly object _fileLock = new object();
            public TSSettingsSave(string filePath) { _iniFilePath = filePath; }
            public string TSReadSettings(string sectionName, string keyName){
                lock (_fileLock){
                    if (!File.Exists(_iniFilePath)) { return string.Empty; }
                    string[] lines = File.ReadAllLines(_iniFilePath, Encoding.UTF8);
                    bool isInSection = string.IsNullOrEmpty(sectionName);
                    foreach (string rawLine in lines){
                        string line = rawLine.Trim();
                        if (line.Length == 0 || line.StartsWith(";")) { continue; }
                        if (line.StartsWith("[") && line.EndsWith("]")){
                            isInSection = line.Equals("[" + sectionName + "]", StringComparison.OrdinalIgnoreCase);
                            continue;
                        }
                        if (isInSection){
                            int equalsIndex = line.IndexOf('=');
                            if (equalsIndex > 0){
                                string currentKey = line.Substring(0, equalsIndex).Trim();
                                if (currentKey.Equals(keyName, StringComparison.OrdinalIgnoreCase)){
                                    return line.Substring(equalsIndex + 1).Trim();
                                }
                            }
                        }
                    }
                    return string.Empty;
                }
            }
            public void TSWriteSettings(string sectionName, string keyName, string value){
                lock (_fileLock){
                    List<string> lines = File.Exists(_iniFilePath) ? File.ReadAllLines(_iniFilePath, Encoding.UTF8).ToList() : new List<string>();
                    bool sectionFound = string.IsNullOrEmpty(sectionName);
                    bool keyUpdated = false;
                    int insertIndex = lines.Count;
                    for (int i = 0; i < lines.Count; i++){
                        string trimmedLine = lines[i].Trim();
                        if (trimmedLine.Length == 0 || trimmedLine.StartsWith(";")) { continue; }
                        if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]")){
                            if (sectionFound && !keyUpdated){
                                insertIndex = i;
                                break;
                            }
                            sectionFound = trimmedLine.Equals("[" + sectionName + "]", StringComparison.OrdinalIgnoreCase);
                            continue;
                        }
                        if (sectionFound){
                            int equalsIndex = trimmedLine.IndexOf('=');
                            if (equalsIndex > 0){
                                string currentKey = trimmedLine.Substring(0, equalsIndex).Trim();
                                if (currentKey.Equals(keyName, StringComparison.OrdinalIgnoreCase)){
                                    lines[i] = keyName + "=" + value;
                                    keyUpdated = true;
                                    break;
                                }
                            }
                        }
                    }
                    if (!sectionFound){
                        if (lines.Count > 0) { lines.Add(""); }
                        lines.Add("[" + sectionName + "]");
                        lines.Add(keyName + "=" + value);
                    }else if (!keyUpdated){
                        insertIndex = (insertIndex == lines.Count) ? lines.Count : insertIndex;
                        lines.Insert(insertIndex, keyName + "=" + value);
                    }
                    try{
                        File.WriteAllLines(_iniFilePath, lines, Encoding.UTF8);
                    }catch (IOException){
                        //Console.Error.WriteLine(ex.Message);
                    }
                }
            }
        }
        // READ LANG PATHS
        // ======================================================================================================
        public static string ts_lf = $"g_langs";                            // Main Path
        public static string ts_lang_ar = ts_lf + @"\Arabic.ini";           // Arabic       | ar
        public static string ts_lang_zh = ts_lf + @"\Chinese.ini";          // Chinese      | zh
        public static string ts_lang_en = ts_lf + @"\English.ini";          // English      | en
        public static string ts_lang_fr = ts_lf + @"\French.ini";           // French       | fr
        public static string ts_lang_de = ts_lf + @"\German.ini";           // German       | de
        public static string ts_lang_hi = ts_lf + @"\Hindi.ini";            // Hindi        | hi
        public static string ts_lang_it = ts_lf + @"\Italian.ini";          // Italian      | it
        public static string ts_lang_ja = ts_lf + @"\Japanese.ini";         // Japanese     | ja
        public static string ts_lang_ko = ts_lf + @"\Korean.ini";           // Korean       | ko
        public static string ts_lang_pt = ts_lf + @"\Portuguese.ini";       // Portuguese   | pt
        public static string ts_lang_ru = ts_lf + @"\Russian.ini";          // Russian      | ru
        public static string ts_lang_es = ts_lf + @"\Spanish.ini";          // Spanish      | es
        public static string ts_lang_tr = ts_lf + @"\Turkish.ini";          // Turkish      | tr
        // LANGUAGE MANAGE FUNCTIONS
        // ======================================================================================================
        public static Dictionary<string, string> AllLanguageFiles = new Dictionary<string, string> {
            { "ar", ts_lang_ar },
            { "zh", ts_lang_zh },
            { "en", ts_lang_en },
            { "fr", ts_lang_fr },
            { "de", ts_lang_de },
            { "hi", ts_lang_hi },
            { "it", ts_lang_it },
            { "ja", ts_lang_ja },
            { "ko", ts_lang_ko },
            { "pt", ts_lang_pt },
            { "ru", ts_lang_ru },
            { "es", ts_lang_es },
            { "tr", ts_lang_tr },
        };
        public static string TSPreloaderSetDefaultLanguage(string ui_lang){
            bool anyLanguageFileExists = AllLanguageFiles.Values.Any(File.Exists);
            bool isUiLangValid = !string.IsNullOrEmpty(ui_lang) && AllLanguageFiles.ContainsKey(ui_lang) && File.Exists(AllLanguageFiles[ui_lang]);
            return anyLanguageFileExists && isUiLangValid ? ui_lang : "en";
        }
        public static List<string> AvailableLanguages = AllLanguageFiles.Values.Where(filePath => File.Exists(filePath)).ToList();
        // READ LANG CLASS
        // ======================================================================================================
        public class TSGetLangs{
            private readonly string _iniFilePath;
            private readonly object _cacheLock = new object();
            private string[] _cachedLines = null;
            private DateTime _lastFileWriteTime = DateTime.MinValue;
            public TSGetLangs(string iniFilePath) { _iniFilePath = iniFilePath; }
            public string TSReadLangs(string sectionName, string keyName){
                string[] iniLines = GetIniLinesCached();
                bool isInSection = string.IsNullOrEmpty(sectionName);
                foreach (string rawLine in iniLines){
                    string line = rawLine.Trim();
                    if (line.Length == 0 || line.StartsWith(";")) { continue; }
                    if (line.StartsWith("[") && line.EndsWith("]")){
                        isInSection = line.Equals("[" + sectionName + "]", StringComparison.OrdinalIgnoreCase);
                        continue;
                    }
                    if (isInSection){
                        int eqIndex = line.IndexOf('=');
                        if (eqIndex > 0){
                            string currentKey = line.Substring(0, eqIndex).Trim();
                            if (currentKey.Equals(keyName, StringComparison.OrdinalIgnoreCase)){
                                return line.Substring(eqIndex + 1).Trim();
                            }
                        }
                    }
                }
                return string.Empty;
            }
            private string[] GetIniLinesCached(){
                lock (_cacheLock){
                    try{
                        if (!File.Exists(_iniFilePath)) { return new string[0]; }
                        DateTime currentWriteTime = File.GetLastWriteTimeUtc(_iniFilePath);
                        if (_cachedLines == null || currentWriteTime != _lastFileWriteTime){
                            _cachedLines = File.ReadAllLines(_iniFilePath, Encoding.UTF8);
                            _lastFileWriteTime = currentWriteTime;
                        }
                        return _cachedLines;
                    }catch (IOException){
                        // Console.Error.WriteLine(ex.Message);
                        return new string[0];
                    }
                }
            }
        }
        // TURKISH LETTER CONVERTER
        // ======================================================================================================
        public static string ConvertTurkishCharacters(string input){
            if (string.IsNullOrWhiteSpace(input)){ return input; }
            var characterMap = new Dictionary<char, char>{
                { 'Ç', 'C' }, { 'ç', 'c' },
                { 'Ğ', 'G' }, { 'ğ', 'g' },
                { 'İ', 'I' }, { 'ı', 'i' },
                { 'Ö', 'O' }, { 'ö', 'o' },
                { 'Ş', 'S' }, { 'ş', 's' },
                { 'Ü', 'U' }, { 'ü', 'u' }
            };
            var result = new StringBuilder(input.Length);
            foreach (var c in input){
                result.Append(characterMap.TryGetValue(c, out var replacement) ? replacement : c);
            }
            return result.ToString().Trim();
        }
        // TS THEME ENGINE
        // ======================================================================================================
        public class TS_ThemeEngine{
            // LIGHT THEME COLORS
            // ====================================
            public static readonly Dictionary<string, Color> LightTheme = new Dictionary<string, Color>{
                // TS PRELOADER
                { "TSBT_BGColor", Color.FromArgb(236, 242, 248) },
                { "TSBT_BGColor2", Color.White },
                { "TSBT_AccentColor", Color.FromArgb(54, 95, 146) },
                { "TSBT_LabelColor1", Color.FromArgb(51, 51, 51) },
                { "TSBT_LabelColor2", Color.FromArgb(100, 100, 100) },
                { "TSBT_CloseBG", Color.FromArgb(25, 255, 255, 255) },
                { "TSBT_CloseBGHover", Color.FromArgb(50, 255, 255, 255) },
                // HEADER MENU COLOR MODE
                { "HeaderBGColorMain", Color.White },
                { "HeaderFEColorMain", Color.FromArgb(51, 51, 51) },
                { "HeaderFEColor", Color.FromArgb(51, 51, 51) },
                { "HeaderBGColor", Color.FromArgb(236, 242, 248) },
                // ACTIVE PAGE COLOR
                { "BtnActiveColor", Color.White },
                { "BtnDeActiveColor", Color.FromArgb(236, 242, 248) },
                // UI COLOR
                { "LeftMenuBGAndBorderColor", Color.FromArgb(236, 242, 248) },
                { "LeftMenuButtonHoverAndMouseDownColor", Color.White },
                { "LeftMenuButtonAlphaColor", Color.FromArgb(50, 255, 255, 255) },
                { "LeftMenuButtonFEColor", Color.FromArgb(51, 51, 51) },
                { "LeftMenuButtonFEColor2", Color.FromArgb(21, 23, 32) },
                { "PageContainerBGAndPageContentTotalColors", Color.White },
                { "ContentPanelBGColor", Color.FromArgb(236, 242, 248) },
                { "ContentLabelLeft", Color.FromArgb(51, 51, 51) },
                { "AccentMain", Color.FromArgb(54, 95, 146) },
                { "AccentMainHover", Color.FromArgb(63, 109, 165) },
                //
                { "SelectBoxBGColor", Color.White },
                { "SelectBoxBGColor2", Color.FromArgb(236, 242, 248) },
                { "SelectBoxFEColor", Color.FromArgb(51, 51, 51) },
                { "SelectBoxBorderColor", Color.FromArgb(226, 226, 226) },
                //
                { "TextBoxBGColor", Color.White },
                { "TextBoxFEColor", Color.FromArgb(51, 51, 51) },
                { "DataGridBGColor", Color.FromArgb(236, 242, 248) },
                { "DataGridFEColor", Color.FromArgb(51, 51, 51) },
                { "DataGridColor", Color.FromArgb(226, 226, 226) },
                { "DataGridAlternatingColor", Color.White },
                { "OSDAndServicesPageBG", Color.FromArgb(54, 95, 146) },
                { "OSDAndServicesPageFE", Color.White },
                { "DynamicThemeActiveBtnBG", Color.White },
                // ACCENT COLOR
                { "AccentBlue", Color.FromArgb(54, 95, 146) },
                { "AccentPurple", Color.FromArgb(126, 27, 156) },
                { "AccentRed", Color.FromArgb(207, 24, 0) },
                { "AccentGreen", Color.FromArgb(28, 122, 25) },
            };
            // DARK THEME COLORS
            // ====================================
            public static readonly Dictionary<string, Color> DarkTheme = new Dictionary<string, Color>{
                // TS PRELOADER
                { "TSBT_BGColor", Color.FromArgb(21, 23, 32) },
                { "TSBT_BGColor2", Color.FromArgb(25, 31, 42) },
                { "TSBT_AccentColor", Color.FromArgb(88, 153, 233) },
                { "TSBT_LabelColor1", Color.WhiteSmoke },
                { "TSBT_LabelColor2", Color.FromArgb(176, 184, 196) },
                { "TSBT_CloseBG", Color.FromArgb(75, 25, 31, 42) },
                { "TSBT_CloseBGHover", Color.FromArgb(100, 25, 31, 42) },
                // HEADER MENU COLOR MODE
                { "HeaderBGColorMain", Color.FromArgb(25, 31, 42) },
                { "HeaderFEColorMain", Color.FromArgb(222, 222, 222) },
                { "HeaderFEColor", Color.WhiteSmoke },
                { "HeaderBGColor", Color.FromArgb(21, 23, 32) },
                 // ACTIVE PAGE COLOR
                { "BtnActiveColor", Color.FromArgb(25, 31, 42) },
                { "BtnDeActiveColor", Color.FromArgb(21, 23, 32) },
                // UI COLOR
                { "LeftMenuBGAndBorderColor", Color.FromArgb(21, 23, 32) },
                { "LeftMenuButtonHoverAndMouseDownColor", Color.FromArgb(25, 31, 42) },
                { "LeftMenuButtonAlphaColor", Color.FromArgb(50, 25, 31, 42) },
                { "LeftMenuButtonFEColor", Color.WhiteSmoke },
                { "LeftMenuButtonFEColor2", Color.White },
                { "PageContainerBGAndPageContentTotalColors", Color.FromArgb(25, 31, 42) },
                { "ContentPanelBGColor", Color.FromArgb(21, 23, 32) },
                { "ContentLabelLeft", Color.WhiteSmoke },
                { "AccentMain", Color.FromArgb(88, 153, 233) },
                { "AccentMainHover", Color.FromArgb(93, 165, 253) },
                //
                { "SelectBoxBGColor", Color.FromArgb(25, 31, 42) },
                { "SelectBoxBGColor2", Color.FromArgb(21, 23, 32) },
                { "SelectBoxFEColor", Color.WhiteSmoke },
                { "SelectBoxBorderColor", Color.FromArgb(36, 45, 61) },
                //
                { "TextBoxBGColor", Color.FromArgb(25, 31, 42) },
                { "TextBoxFEColor", Color.WhiteSmoke },
                { "DataGridBGColor", Color.FromArgb(21, 23, 32) },
                { "DataGridFEColor", Color.WhiteSmoke },
                { "DataGridColor", Color.FromArgb(36, 45, 61) },
                { "DataGridAlternatingColor", Color.FromArgb(25, 31, 42) },
                { "OSDAndServicesPageBG", Color.FromArgb(88, 153, 233) },
                { "OSDAndServicesPageFE", Color.FromArgb(25, 31, 42) },
                { "DynamicThemeActiveBtnBG", Color.FromArgb(25, 31, 42) },
                // ACCENT COLOR
                { "AccentBlue", Color.FromArgb(88, 153, 233) },
                { "AccentPurple", Color.FromArgb(229, 33, 255) },
                { "AccentRed", Color.FromArgb(255, 51, 51) },
                { "AccentGreen", Color.FromArgb(38, 187, 33) },
            };
            // THEME SWITCHER
            // ====================================
            public static Color ColorMode(int theme, string key){
                if (theme == 0){
                    return DarkTheme.ContainsKey(key) ? DarkTheme[key] : Color.Transparent;
                }else if (theme == 1){
                    return LightTheme.ContainsKey(key) ? LightTheme[key] : Color.Transparent;
                }
                return Color.Transparent;
            }
        }
        // THEME MODE HELPER
        // ======================================================================================================
        public static class TSThemeModeHelper{
            private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            [DllImport("dwmapi.dll", PreserveSig = true)]
            private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
            [DllImport("uxtheme.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
            private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
            private static bool _isDarkModeEnabled = false;
            public static bool IsDarkModeEnabled => _isDarkModeEnabled;
            public static void SetThemeMode(bool enableTMode){
                _isDarkModeEnabled = enableTMode;
                foreach (Form targetForm in Application.OpenForms){
                    ApplyThemeModeToForm(targetForm, enableTMode);
                }
            }
            public static void InitializeGlobalTheme(){
                Application.Idle += (s, e) =>{
                    foreach (Form formListener in Application.OpenForms){
                        if (!formListener.Tag?.Equals("DarkModeApplied") ?? true){
                            ApplyThemeModeToForm(formListener, _isDarkModeEnabled);
                            formListener.Tag = "DarkModeApplied";
                        }
                    }
                };
            }
            private static void ApplyThemeModeToForm(Form targetForm, bool enableRequire){
                if (targetForm == null || targetForm.IsDisposed) return;
                int useDark = enableRequire ? 1 : 0;
                DwmSetWindowAttribute(targetForm.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref useDark, sizeof(int));
                ApplyScrollTheme(targetForm, enableRequire ? "DarkMode_Explorer" : "Explorer");
            }
            private static void ApplyScrollTheme(Control parentMode, string targetTheme){
                if (parentMode == null || parentMode.IsDisposed) return;
                SetWindowTheme(parentMode.Handle, targetTheme, null);
                foreach (Control childControl in parentMode.Controls){
                    ApplyScrollTheme(childControl, targetTheme);
                }
            }
        }
        public static int GetSystemTheme(int theme_mode){
            if (theme_mode == 2){
                using (var getSystemThemeKey = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize")){
                    theme_mode = (int)(getSystemThemeKey?.GetValue("SystemUsesLightTheme") ?? 1);
                }
            }
            return theme_mode;
        }
        // DPI SENSITIVE DYNAMIC IMAGE RENDERER
        // ======================================================================================================
        public static void TSImageRenderer(object baseTarget, Image sourceImage, int basePadding, ContentAlignment imageAlign = ContentAlignment.MiddleCenter){
            if (sourceImage == null || baseTarget == null) return;
            const int minImageSize = 16;
            try{
                int calculatedSize;
                Image previousImage = null;
                Image ResizeImage(Image targetImg, int targetSize){
                    Bitmap resizedEngine = new Bitmap(targetSize, targetSize, PixelFormat.Format32bppArgb);
                    using (Graphics renderGraphics = Graphics.FromImage(resizedEngine)){
                        renderGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        renderGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                        renderGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        renderGraphics.CompositingQuality = CompositingQuality.HighQuality;
                        renderGraphics.DrawImage(targetImg, 0, 0, targetSize, targetSize);
                    }
                    return resizedEngine;
                }
                if (baseTarget is Control targetControl){
                    float dpi = targetControl.DeviceDpi > 0 ? targetControl.DeviceDpi : 96f;
                    float dpiScaleFactor = dpi / 96f;
                    int paddingWithScale = (int)Math.Round(basePadding * dpiScaleFactor);
                    //
                    calculatedSize = targetControl.Height - paddingWithScale;
                    if (calculatedSize <= 0) { calculatedSize = minImageSize; }
                    Image resizedImage = ResizeImage(sourceImage, calculatedSize);
                    if (targetControl is Button buttonMode){
                        previousImage = buttonMode.Image;
                        buttonMode.Image = resizedImage;
                        buttonMode.ImageAlign = imageAlign;
                    }else if (targetControl is PictureBox pictureBoxMode){
                        previousImage = pictureBoxMode.Image;
                        pictureBoxMode.Image = resizedImage;
                        pictureBoxMode.SizeMode = PictureBoxSizeMode.Zoom;
                    }else{
                        resizedImage.Dispose();
                    }
                }else if (baseTarget is ToolStripItem toolStripItemMode){
                    calculatedSize = toolStripItemMode.Height - basePadding;
                    if (calculatedSize <= 0) { calculatedSize = minImageSize; }
                    Image resizedImage = ResizeImage(sourceImage, calculatedSize);
                    previousImage = toolStripItemMode.Image;
                    toolStripItemMode.Image = resizedImage;
                }else{
                    return;
                }
                if (previousImage != null && previousImage != sourceImage) { previousImage.Dispose(); }
            }catch (Exception){ }
        }
        // SET DYNAMIC SIZE ALGORITHM
        // ======================================================================================================
        public static void SetPictureBoxImage(PictureBox pictureBox, Image newImage){
            if (pictureBox.Image != null){
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }
            if (newImage == null){
                pictureBox.Image = null;
                return;
            }
            var resized = ResizeImageToDeviceDpi(newImage, pictureBox.Width, pictureBox.Height, pictureBox.DeviceDpi);
            pictureBox.Image = resized;
            newImage.Dispose();
        }
        public static Image ResizeImageToDeviceDpi(Image img, int maxWidth, int maxHeight, int deviceDpi){
            int newWidth = (int)(img.Width * deviceDpi / img.HorizontalResolution);
            int newHeight = (int)(img.Height * deviceDpi / img.VerticalResolution);
            double ratio = Math.Min((double)maxWidth / newWidth, (double)maxHeight / newHeight);
            newWidth = (int)(newWidth * ratio);
            newHeight = (int)(newHeight * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            newImage.SetResolution(deviceDpi, deviceDpi);
            using (var image_render = Graphics.FromImage(newImage)){
                image_render.CompositingQuality = CompositingQuality.HighQuality;
                image_render.InterpolationMode = InterpolationMode.HighQualityBicubic;
                image_render.SmoothingMode = SmoothingMode.HighQuality;
                image_render.PixelOffsetMode = PixelOffsetMode.HighQuality;
                image_render.DrawImage(img, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }
        // DYNAMIC SIZE COUNT ALGORITHM
        // ======================================================================================================
        public static string TS_FormatSize(double bytes){
            string[] suffixes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
            int suffixIndex = 0;
            double doubleBytes = bytes;
            while (doubleBytes >= 1024 && suffixIndex < suffixes.Length - 1){
                doubleBytes /= 1024;
                suffixIndex++;
            }
            return $"{doubleBytes:0.##} {suffixes[suffixIndex]}";
        }
        public static double TS_FormatSizeNoType(double bytes){
            while (bytes >= 1024){
                bytes /= 1024;
            }
            return Math.Round(bytes, 2);
        }
        // DYNAMIC BUTTON WIDTH
        // ======================================================================================================
        public static void TS_AdjustButtonWidth(Button render_button){
            using (Graphics btn_graphics = render_button.CreateGraphics()){
                SizeF text_size = btn_graphics.MeasureString(render_button.Text, render_button.Font);
                int padding_btn = render_button.Padding.Left + render_button.Padding.Right;
                render_button.Width = (int)(text_size.Width * 1.24) + padding_btn + 25;
                // render_button.Height = Math.Max(render_button.Height, (int)(text_size.Height * 1.2) + render_button.Padding.Top + render_button.Padding.Bottom);
            }
        }
        // WINDOWS PROD KEY ALGORITHM
        // ======================================================================================================
        public static class TSWindowsProductKey{
            public static string GetWindowsProductKey(){
                Version osVer = Environment.OSVersion.Version;
                if (osVer.Major < 6 || (osVer.Major == 6 && osVer.Minor < 2)){
                    return "NS_OS";
                }
                using (RegistryKey localMachineReg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (RegistryKey subKey = localMachineReg.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion")){
                    if (subKey == null) return "NO_SUB_KEY";
                    object regKeyValue = subKey.GetValue("DigitalProductId");
                    if (regKeyValue == null) return "NO_KEY";
                    byte[] digitalProductId = (byte[])regKeyValue;
                    return DecodeWindowsProductKey(digitalProductId);
                }
            }
            private static string DecodeWindowsProductKey(byte[] d_p_id){
                const int key_offset = 52;
                const string digits_list = "BCDFGHJKMPQRTVWXY2346789";
                byte win10Mode = (byte)((d_p_id[66] / 6) & 1);
                d_p_id[66] = (byte)((d_p_id[66] & 0xF7) | (win10Mode & 2) * 4);
                string keyLatest = string.Empty;
                int lastIndex = 0;
                for (int i = 24; i >= 0; i--){
                    int currentIndex = 0;
                    for (int j = 14; j >= 0; j--){
                        currentIndex = d_p_id[j + key_offset] + currentIndex * 256;
                        d_p_id[j + key_offset] = (byte)(currentIndex / 24);
                        currentIndex %= 24;
                        lastIndex = currentIndex;
                    }
                    keyLatest = digits_list[currentIndex] + keyLatest;
                }
                keyLatest = keyLatest.Substring(1, lastIndex) + "N" + keyLatest.Substring(lastIndex + 1);
                for (int i = 5; i < keyLatest.Length; i += 6){
                    keyLatest = keyLatest.Insert(i, "-");
                }
                return keyLatest;
            }
        }
        // SCREEN API
        // ======================================================================================================
        public const int ENUM_CURRENT_SETTINGS = -1;
        [DllImport("user32.dll", PreserveSig = true)]
        public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);
        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE{
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }
        // NETWORK NAME REPLACER
        // ======================================================================================================
        public static string Net_replacer(string get_adapter_name) => get_adapter_name.Replace("[", "(").Replace("]", ")");
        // TS NATURAL SORT KEY ALGORITHM
        // ======================================================================================================
        public static string TSNaturalSortKey(string input, CultureInfo culture, int paddingLength = 30){
            if (input == null) { return ""; }
            string padded = Regex.Replace(input, @"\d+", match => match.Value.PadLeft(paddingLength, '0'));
            string normalized = padded.Normalize(NormalizationForm.FormD);
            if (culture == null) { culture = CultureInfo.CurrentCulture; }
            string lowerCased = normalized.ToLower(culture);
            return lowerCased;
        }
        // INTERNET CONNECTION STATUS
        // ======================================================================================================
        public static bool IsNetworkCheck(){
            try{
                HttpWebRequest server_request = (HttpWebRequest)WebRequest.Create("http://clients3.google.com/generate_204");
                server_request.KeepAlive = false;
                server_request.Timeout = 2500;
                using (var server_response = (HttpWebResponse)server_request.GetResponse()){
                    return server_response.StatusCode == HttpStatusCode.NoContent;
                }
            }catch{
                return false;
            }
        }
        // WINDOWS WALLPAPER CHECK
        // ======================================================================================================
        [DllImport("user32.dll", SetLastError = true, PreserveSig = true)]
        public static extern bool SystemParametersInfo(int uAction, int uParam, StringBuilder lpvParam, int fuWinIni);
        // DPI AWARE V2
        // ======================================================================================================
        [DllImport("user32.dll", PreserveSig = true)]
        public static extern bool SetProcessDpiAwarenessContext(IntPtr dpiFlag);
        public static readonly IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 = new IntPtr(-4);
    }
}