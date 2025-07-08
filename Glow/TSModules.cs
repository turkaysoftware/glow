using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Glow{
    internal class TSModules{
        // LINK SYSTEM
        // ======================================================================================================
        public class TS_LinkSystem{
            public static string
            website_link        = "https://www.turkaysoftware.com",
            twitter_x_link      = "https://x.com/turkaysoftware",
            instagram_link      = "https://www.instagram.com/erayturkayy/",
            github_link         = "https://github.com/turkaysoftware",
            //
            github_link_lt      = "https://raw.githubusercontent.com/turkaysoftware/glow/main/Glow/SoftwareVersion.txt",
            github_link_lr      = "https://github.com/turkaysoftware/glow/releases/latest",
            //
            ts_wizard           = "https://www.turkaysoftware.com/ts-wizard",
            //
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
                { 1, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.OK, MessageBoxIcon.Information) },       // Ok ve Bilgi
                { 2, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.OK, MessageBoxIcon.Warning) },           // Ok ve Uyarı
                { 3, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.OK, MessageBoxIcon.Error) },             // Ok ve Hata
                { 4, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Question) },       // Yes/No ve Soru
                { 5, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Information) },    // Yes/No ve Bilgi
                { 6, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Warning) },        // Yes/No ve Uyarı
                { 7, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNo, MessageBoxIcon.Error) },          // Yes/No ve Hata
                { 8, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) },    // Retry/Cancel ve Hata
                { 9, new KeyValuePair<MessageBoxButtons, MessageBoxIcon>(MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) }  // Yes/No/Cancel ve Soru
            };
            public static DialogResult TS_MessageBox(Form m_form, int m_mode, string m_message, string m_title = ""){
                m_form.BringToFront();
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
            [DllImport("kernel32.dll")]
            private static extern int WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32.dll")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            private readonly string _settingFilePath;
            public TSSettingsSave(string filePath){ _settingFilePath = filePath; }
            public string TSReadSettings(string episode, string settingName){
                StringBuilder stringBuilder = new StringBuilder(4096);
                GetPrivateProfileString(episode, settingName, string.Empty, stringBuilder, 4096, _settingFilePath);
                return stringBuilder.ToString();
            }
            public int TSWriteSettings(string episode, string settingName, string value){
                return WritePrivateProfileString(episode, settingName, value, _settingFilePath);
            }
        }
        // READ LANG PATHS
        // ======================================================================================================
        public static string ts_lf = $"g_langs";                            // Main Path
        public static string ts_lang_zh = ts_lf + @"\Chinese.ini";          // Chinese      | zh
        public static string ts_lang_en = ts_lf + @"\English.ini";          // English      | en
        public static string ts_lang_fr = ts_lf + @"\French.ini";           // French       | fr
        public static string ts_lang_de = ts_lf + @"\German.ini";           // German       | de
        public static string ts_lang_it = ts_lf + @"\Italian.ini";          // Italian      | it
        public static string ts_lang_ko = ts_lf + @"\Korean.ini";           // Korean       | ko
        public static string ts_lang_pt = ts_lf + @"\Portuguese.ini";       // Portuguese   | pt
        public static string ts_lang_ru = ts_lf + @"\Russian.ini";          // Russian      | ru
        public static string ts_lang_es = ts_lf + @"\Spanish.ini";          // Spanish      | es
        public static string ts_lang_tr = ts_lf + @"\Turkish.ini";          // Turkish      | tr
        // READ LANG CLASS
        // ======================================================================================================
        public class TSGetLangs{
            [DllImport("kernel32.dll")]
            private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size,string filePath);
            private readonly string _readFilePath;
            public TSGetLangs(string filePath){ _readFilePath = filePath; }
            public string TSReadLangs(string episode, string settingName){
                StringBuilder stringBuilder = new StringBuilder(4096);
                GetPrivateProfileString(episode, settingName, string.Empty, stringBuilder, 4096, _readFilePath);
                return stringBuilder.ToString();
            }
        }
        // TS STRING ENCODER
        // ======================================================================================================
        public static string TS_String_Encoder(string get_text){
            return Encoding.UTF8.GetString(Encoding.Default.GetBytes(get_text)).Trim();
        }
        // TURKISH LETTER CONVERTER
        // ======================================================================================================
        public static string TS_TR_LetterConverter(string called_text){
            if (string.IsNullOrEmpty(called_text)) { return called_text; }
            StringBuilder str_con = new StringBuilder(called_text);
            str_con.Replace('Ç', 'C').Replace('ç', 'c');
            str_con.Replace('Ğ', 'G').Replace('ğ', 'g');
            str_con.Replace('İ', 'I').Replace('ı', 'i');
            str_con.Replace('Ö', 'O').Replace('ö', 'o');
            str_con.Replace('Ş', 'S').Replace('ş', 's');
            str_con.Replace('Ü', 'U').Replace('ü', 'u');
            return str_con.ToString().Trim();
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
                { "TSBT_CloseBG", Color.FromArgb(200, 255, 255, 255) },
                // HEADER MENU COLOR MODE
                { "HeaderBGColorMain", Color.White },
                { "HeaderFEColorMain", Color.FromArgb(51, 51, 51) },
                // ACTIVE PAGE COLOR
                { "BtnActiveColor", Color.White },
                { "BtnDeActiveColor", Color.FromArgb(236, 242, 248) },
                // UI COLOR
                { "HeaderFEColor", Color.FromArgb(51, 51, 51) },
                { "HeaderBGColor", Color.FromArgb(236, 242, 248) },
                { "LeftMenuBGAndBorderColor", Color.FromArgb(236, 242, 248) },
                { "LeftMenuButtonHoverAndMouseDownColor", Color.White },
                { "LeftMenuButtonFEColor", Color.FromArgb(51, 51, 51) },
                { "PageContainerBGAndPageContentTotalColors", Color.White },
                { "ContentPanelBGColor", Color.FromArgb(236, 242, 248) },
                { "ContentLabelLeft", Color.FromArgb(51, 51, 51) },
                { "ContentLabelRight", Color.FromArgb(54, 95, 146) },
                { "ContentLabelRightHover", Color.FromArgb(67, 116, 177) },
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
                // DISK TOTAL BG
                { "DiskTotalSSDBG", Color.FromArgb(54, 95, 146) },
                { "DiskTotalHDDBG", Color.FromArgb(123, 27, 193) },
                { "DiskTotalUSBBG", Color.FromArgb(193, 27, 92) },
                { "DiskTotalTotalBG", Color.FromArgb(21, 140, 84) },
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
                { "TSBT_CloseBG", Color.FromArgb(210, 25, 31, 42) },
                // HEADER MENU COLOR MODE
                { "HeaderBGColorMain", Color.FromArgb(25, 31, 42) },
                { "HeaderFEColorMain", Color.FromArgb(222, 222, 222) },
                 // ACTIVE PAGE COLOR
                { "BtnActiveColor", Color.FromArgb(25, 31, 42) },
                { "BtnDeActiveColor", Color.FromArgb(21, 23, 32) },
                // UI COLOR
                { "HeaderFEColor", Color.WhiteSmoke },
                { "HeaderBGColor", Color.FromArgb(21, 23, 32) },
                { "LeftMenuBGAndBorderColor", Color.FromArgb(21, 23, 32) },
                { "LeftMenuButtonHoverAndMouseDownColor", Color.FromArgb(25, 31, 42) },
                { "LeftMenuButtonFEColor", Color.WhiteSmoke },
                { "PageContainerBGAndPageContentTotalColors", Color.FromArgb(25, 31, 42) },
                { "ContentPanelBGColor", Color.FromArgb(21, 23, 32) },
                { "ContentLabelLeft", Color.WhiteSmoke },
                { "ContentLabelRight", Color.FromArgb(88, 153, 233) },
                { "ContentLabelRightHover", Color.FromArgb(84, 179, 241) },
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
                // DISK TOTAL BG
                { "DiskTotalSSDBG", Color.FromArgb(88, 153, 233) },
                { "DiskTotalHDDBG", Color.FromArgb(202, 35, 251) },
                { "DiskTotalUSBBG", Color.FromArgb(237, 37, 115) },
                { "DiskTotalTotalBG", Color.FromArgb(29, 181, 110) },
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
                render_button.Width = (int)(text_size.Width * 1.24) + padding_btn + 15;
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
                        current_index = current_index * 256;
                        current_index = d_p_id[j + key_offset] + current_index;
                        d_p_id[j + key_offset] = (byte)(current_index / 24);
                        current_index = current_index % 24;
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
        // CPU VIRTUALIZATION
        // ======================================================================================================
        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo(out CPU_VIRTUALIZATION lpCPU_INFO);
        [StructLayout(LayoutKind.Sequential)]
        public struct CPU_VIRTUALIZATION{
            public ushort processorArchitecture;
            ushort reserved;
            public uint pageSize;
            public IntPtr minimumApplicationAddress;
            public IntPtr maximumApplicationAddress;
            public IntPtr activeProcessorMask;
            public uint numberOfProcessors;
            public uint processorType;
            public uint allocationGranularity;
            public ushort processorLevel;
            public ushort processorRevision;
        }
        public static bool IsVirtualizationEnabled(){
            CPU_VIRTUALIZATION ts_cpu_virtualization;
            GetSystemInfo(out ts_cpu_virtualization);
            return ts_cpu_virtualization.processorArchitecture == 0 || // x86
                   ts_cpu_virtualization.processorArchitecture == 5 || // ARM
                   ts_cpu_virtualization.processorArchitecture == 6 || // Itanium
                   ts_cpu_virtualization.processorArchitecture == 9;   // x64
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
        public static string net_replacer(string get_adapter_name) => get_adapter_name.Replace("[", "(").Replace("]", ")");
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
        [DllImport("DwmApi")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        // DPI AWARE
        // ======================================================================================================
        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();
    }
}