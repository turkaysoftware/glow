using System;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Glow{
    internal class GlowModules{
        // ======================================================================================================
        // SAVE PATHS
        public static string glow_lf = @"g_langs";                              // Main Path
        public static string glow_lang_en = glow_lf + @"\English.ini";          // English    | en
        public static string glow_lang_tr = glow_lf + @"\Turkish.ini";          // Turkish    | tr
        // Total Langs | Current Langs Count: 2
        public static int g_langs_count = 2;
        // ======================================================================================================
        // GLOW SETTINGS SAVE CLASS
        public class GlowGetLangs{
            [DllImport("kernel32.dll")]
            private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            public GlowGetLangs(string file_path){ save_file_path = file_path; }
            private string save_file_path = glow_lf;
            private string default_save_process { get; set; }
            public string GlowReadLangs(string episode, string setting_name){
                default_save_process = default_save_process ?? string.Empty;
                StringBuilder str_builder = new StringBuilder(256);
                GetPrivateProfileString(episode, setting_name, default_save_process, str_builder, 255, save_file_path);
                return str_builder.ToString();
            }
        }
        // ======================================================================================================
        // SAVE PATHS
        public static string glow_df = Application.StartupPath;
        public static string glow_sf = glow_df + @"\GlowSettings.ini";
        // ======================================================================================================
        // GLOW SETTINGS SAVE CLASS
        public class GlowSettingsSave{
            [DllImport("kernel32.dll")]
            private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
            [DllImport("kernel32.dll")]
            private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
            public GlowSettingsSave(string file_path) { save_file_path = file_path; }
            private string save_file_path = glow_sf;
            private string default_save_process { get; set; }
            public string GlowReadSettings(string episode, string setting_name){
                default_save_process = default_save_process ?? string.Empty;
                StringBuilder str_builder = new StringBuilder(256);
                GetPrivateProfileString(episode, setting_name, default_save_process, str_builder, 255, save_file_path);
                return str_builder.ToString();
            }
            public long GlowWriteSettings(string episode, string setting_name, string value){
                return WritePrivateProfileString(episode, setting_name, value, save_file_path);
            }
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