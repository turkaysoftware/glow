using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Glow{
    internal class GlowModules{
        // LINK SYSTEM
        // ======================================================================================================
        public class TS_LinkSystem{
            public string
            twitter_link        = "https://x.com/roines45",
            github_link         = "https://github.com/roines45",
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
        public static string glow_lang_en = glow_lf + @"\English.ini";          // English    | en
        public static string glow_lang_ko = glow_lf + @"\Korean.ini";           // Korean     | ko
        public static string glow_lang_tr = glow_lf + @"\Turkish.ini";          // Turkish    | tr
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
            string[] suffixes = { "Byte", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };
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
        // ======================================================================================================
        // CPU CODE SETS
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsProcessorFeaturePresent(uint processorFeature);
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