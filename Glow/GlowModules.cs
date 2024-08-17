using System;
using System.Text;
using Microsoft.Win32;
using System.Collections;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Glow{
    internal class GlowModules{
        // LINK SYSTEM
        // ======================================================================================================
        public class TS_LinkSystem{
            public string
            github_link         = "https://github.com/roines45",
            instagram_link      = "https://www.instagram.com/erayturkay45/",
            twitter_link        = "https://x.com/erayturkay45",
            //
            github_link_vl      = "https://raw.githubusercontent.com/roines45/glow/main/Glow/GlowVersion.txt",
            github_link_lr      = "https://github.com/roines45/glow/releases/latest";
        }
        // ======================================================================================================
        // VERSIONS
        public class GlowVersionEngine{
            public string GlowVersion(int v_type, int v_mode){
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
        // ======================================================================================================
        // LANG PATHS
        public static string glow_lf = @"g_langs";                              // Main Path
        public static string glow_lang_zh = glow_lf + @"\Chinese.ini";          // Chinese      | zh
        public static string glow_lang_en = glow_lf + @"\English.ini";          // English      | en
        public static string glow_lang_fr = glow_lf + @"\French.ini";           // French       | fr
        public static string glow_lang_de = glow_lf + @"\German.ini";           // German       | de
        public static string glow_lang_ko = glow_lf + @"\Korean.ini";           // Korean       | ko
        public static string glow_lang_pt = glow_lf + @"\Portuguese.ini";       // Portuguese   | pt
        public static string glow_lang_ru = glow_lf + @"\Russian.ini";          // Russian      | ru
        public static string glow_lang_es = glow_lf + @"\Spanish.ini";          // Spanish      | es
        public static string glow_lang_tr = glow_lf + @"\Turkish.ini";          // Turkish      | tr
        public class TSGetLangs{
            [DllImport("kernel32.dll")]
            private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            private readonly string _saveFilePath;
            public TSGetLangs(string filePath){ _saveFilePath = filePath; }
            public string TSReadLangs(string episode, string settingName){
                StringBuilder stringBuilder = new StringBuilder(512);
                GetPrivateProfileString(episode, settingName, string.Empty, stringBuilder, 511, _saveFilePath);
                return stringBuilder.ToString();
            }
        }
        // ======================================================================================================
        // SAVE PATHS
        public static string ts_df = Application.StartupPath;
        public static string ts_sf = ts_df + @"\GlowSettings.ini";
        // ======================================================================================================
        // GLOW SETTINGS SAVE CLASS
        public class TSSettingsSave{
            [DllImport("kernel32.dll")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32.dll")]
            private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            private readonly string _saveFilePath;
            public TSSettingsSave(string filePath){ _saveFilePath = filePath; }
            public string TSReadSettings(string episode, string settingName){
                StringBuilder stringBuilder = new StringBuilder(512);
                GetPrivateProfileString(episode, settingName, string.Empty, stringBuilder, 511, _saveFilePath);
                return stringBuilder.ToString();
            }
            public long TSWriteSettings(string episode, string settingName, string value){
                return WritePrivateProfileString(episode, settingName, value, _saveFilePath);
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
        // WINDOWS PROD KEY ALGORITHM
        // ======================================================================================================
        public static class TSWindowsProductKey{
            // Windows Versions
            public enum EnumWindowsVersion{ Windows7, Windows8Up }
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
                var digits_array = new[]{ 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'M', 'P', 'Q', 'R', 'T', 'V', 'W', 'X', 'Y', '2', '3', '4', '6', '7', '8', '9', };
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
        // ======================================================================================================
        // CPU VIRTUALIZATION
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
        // ======================================================================================================
        // CPU CODE SETS
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsProcessorFeaturePresent(uint processorFeature);
        // ======================================================================================================
        // NETWORK NAME REPLACER
        public static string net_replacer(string get_adapter_name){
            string a1 = get_adapter_name.Replace("[", "(");
            string a2 = a1.Replace("]", ")");
            return a2;
        }
        // ======================================================================================================
        // SCREEN API
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
        // TITLE BAR SETTINGS DWM API
        // ======================================================================================================
        [DllImport("DwmApi")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, int[] attrValue, int attrSize);
        // ======================================================================================================
        // DPI AWARE
        [DllImport("user32.dll")]
        public static extern bool SetProcessDPIAware();
    }
}