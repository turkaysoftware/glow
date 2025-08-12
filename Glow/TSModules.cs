using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

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
            twitter_x_link      = "https://x.com/turkaysoftware",
            instagram_link      = "https://www.instagram.com/erayturkayy/",
            github_link         = "https://github.com/turkaysoftware",
            youtube_link        = "https://www.youtube.com/@turkaysoftware",
            // Other Links
            ts_wizard           = "https://www.turkaysoftware.com/ts-wizard",
            ts_bmac             = "https://buymeacoffee.com/turkaysoftware";
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
                { 1, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.OK, MessageBoxIcon.Information) },           // Ok ve Bilgi
                { 2, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.OK, MessageBoxIcon.Warning) },               // Ok ve Uyarı
                { 3, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.OK, MessageBoxIcon.Error) },                 // Ok ve Hata
                { 4, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Question) },           // Yes/No ve Soru
                { 5, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Information) },        // Yes/No ve Bilgi
                { 6, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Warning) },            // Yes/No ve Uyarı
                { 7, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Error) },              // Yes/No ve Hata
                { 8, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) },        // Retry/Cancel ve Hata
                { 9, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) },     // Yes/No/Cancel ve Soru
                { 10, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNoCancel, MessageBoxIcon.Information) }  // Yes/No/Cancel ve Bilgi
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
                        // Hata loglanabilir, bu örnekte yazdırıyoruz
                        //Console.Error.WriteLine("INI yazma hatası: " + ex.Message);
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
                        // Console.Error.WriteLine("INI okuma hatası: " + ex.Message);
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
                { "SelectBoxBGColor", Color.White },
                { "TextBoxBGColor", Color.White },
                { "TextBoxFEColor", Color.FromArgb(51, 51, 51) },
                { "DataGridBGColor", Color.White },
                { "DataGridFEColor", Color.FromArgb(51, 51, 51) },
                { "DataGridColor", Color.FromArgb(226, 226, 226) },
                { "DataGridAlternatingColor", Color.FromArgb(236, 242, 248) },
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
                { "SelectBoxBGColor", Color.FromArgb(25, 31, 42) },
                { "TextBoxBGColor", Color.FromArgb(25, 31, 42) },
                { "TextBoxFEColor", Color.WhiteSmoke },
                { "DataGridBGColor", Color.FromArgb(21, 23, 32) },
                { "DataGridFEColor", Color.WhiteSmoke },
                { "DataGridColor", Color.FromArgb(36, 45, 61) },
                { "DataGridAlternatingColor", Color.FromArgb(25, 31, 42) },
                { "OSDAndServicesPageBG", Color.FromArgb(88, 153, 233) },
                { "OSDAndServicesPageFE", Color.FromArgb(37, 37, 45) },
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
            // Windows Versions
            public enum EnumWindowsVersion { Windows7, Windows8Up }
            // Machine Registry Mode
            public static string GetWindowsProductKey(){
                var local_machine_reg = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32);
                var reg_key_value = local_machine_reg.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion")?.GetValue("DigitalProductId");
                if (reg_key_value == null) { return "null"; }
                var dig_p_id = (byte[])reg_key_value;
                local_machine_reg.Close();
                var win_8_up = Environment.OSVersion.Version.Major == 6 && Environment.OSVersion.Version.Minor >= 2 || Environment.OSVersion.Version.Major > 6;
                return GetWindowsProductKeyDID(dig_p_id, win_8_up ? EnumWindowsVersion.Windows8Up : EnumWindowsVersion.Windows7);
            }
            // Windows Mode Key
            public static string GetWindowsProductKeyDID(byte[] d_p_id, EnumWindowsVersion d_p_id_ver){
                var prod_key = d_p_id_ver == EnumWindowsVersion.Windows8Up ? DecoderPKeyW8Up(d_p_id) : DecoderPKeyW7(d_p_id);
                return prod_key;
            }
            // Decoder W7
            private static string DecoderPKeyW7(byte[] digitalProductId){
                const int index_start = 52;
                const int index_end = index_start + 15;
                var digits_array = new[] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'M', 'P', 'Q', 'R', 'T', 'V', 'W', 'X', 'Y', '2', '3', '4', '6', '7', '8', '9', };
                const int decode_length = 29;
                const int decode_s_length = 15;
                var decoded_chars = new char[decode_length];
                var hex_pid_mode = new ArrayList();
                //
                for (var i = index_start; i <= index_end; i++){
                    hex_pid_mode.Add(digitalProductId[i]);
                }
                for (var i = decode_length - 1; i >= 0; i--){
                    if ((i + 1) % 6 == 0){
                        decoded_chars[i] = '-';
                    }else{
                        var digit_map = 0;
                        for (var j = decode_s_length - 1; j >= 0; j--){
                            var byte_value = (digit_map << 8) | (byte)hex_pid_mode[j];
                            hex_pid_mode[j] = (byte)(byte_value / 24);
                            digit_map = byte_value % 24;
                            decoded_chars[i] = digits_array[digit_map];
                        }
                    }
                }
                return new string(decoded_chars);
            }
            // Decoder W8 Up
            public static string DecoderPKeyW8Up(byte[] d_p_id){
                var key_latest = String.Empty;
                const int key_offset = 52;
                var win_8_mode = (byte)((d_p_id[66] / 6) & 1);
                d_p_id[66] = (byte)((d_p_id[66] & 0xf7) | (win_8_mode & 2) * 4);
                //
                const string digits_list = "BCDFGHJKMPQRTVWXY2346789";
                var last_index = 0;
                for (var i = 24; i >= 0; i--){
                    var current_index = 0;
                    for (var j = 14; j >= 0; j--){
                        #pragma warning disable IDE0054
                        current_index = current_index * 256;
                        #pragma warning restore IDE0054
                        current_index = d_p_id[j + key_offset] + current_index;
                        d_p_id[j + key_offset] = (byte)(current_index / 24);
                        #pragma warning disable IDE0054
                        current_index = current_index % 24;
                        #pragma warning restore IDE0054
                        last_index = current_index;
                    }
                    key_latest = digits_list[current_index] + key_latest;
                }
                //
                var key_session_1 = key_latest.Substring(1, last_index);
                var key_session_2 = key_latest.Substring(last_index + 1, key_latest.Length - (last_index + 1));
                key_latest = key_session_1 + "N" + key_session_2;
                //
                for (var i = 5; i < key_latest.Length; i += 6){
                    key_latest = key_latest.Insert(i, "-");
                }
                return key_latest;
            }
        }
        // SCREEN API
        // ======================================================================================================
        public const int ENUM_CURRENT_SETTINGS = -1;
        [DllImport("user32.dll")]
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
        // TITLE BAR SETTINGS DWM API
        // ======================================================================================================
        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        // DPI AWARE V2
        // ======================================================================================================
        [DllImport("user32.dll")]
        public static extern bool SetProcessDpiAwarenessContext(IntPtr dpiFlag);
    }
}