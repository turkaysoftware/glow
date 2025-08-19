using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowSFCandDISMAutoTool : Form{
        public GlowSFCandDISMAutoTool(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // GLOBAL GLOBAL CLASS
        // ======================================================================================================
        readonly TSSettingsSave software_read_settings = new TSSettingsSave(ts_sf);
        readonly TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
        // GLOBAL STRINGS
        // ======================================================================================================
        string sadt_title, sadt_start, sadt_starter, sadt_rotate, sadt_ender, sadt_last_text, cmd_c_mode;
        // REPAIR CODES
        // ======================================================================================================
        readonly string sadt_code_1 = "sfc /scannow";
        readonly string sadt_code_2 = "DISM /Online /Cleanup-Image /CheckHealth";
        readonly string sadt_code_3 = "DISM /Online /Cleanup-Image /ScanHealth";
        readonly string sadt_code_4 = "DISM /Online /Cleanup-Image /RestoreHealth";
        private void GlowSFCandDISMAutoTool_Load(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            // GET THEME
            Sadt_theme_settings();
            // SET TEXT
            sadt_title = string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_title"), Application.ProductName);
            sadt_start = software_lang.TSReadLangs("SFCandDISMTool", "sadt_starting_engine");
            sadt_starter = software_lang.TSReadLangs("SFCandDISMTool", "sadt_command_starting");
            sadt_rotate = software_lang.TSReadLangs("SFCandDISMTool", "sadt_command_success_and_next");
            sadt_ender = software_lang.TSReadLangs("SFCandDISMTool", "sadt_command_last_success");
            sadt_last_text = software_lang.TSReadLangs("SFCandDISMTool", "sadt_stop_engine");
        }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void Sadt_theme_settings(){
            try{
                if (GlowMain.theme == 0){ cmd_c_mode = "color 3"; }
                else if (GlowMain.theme == 1){ cmd_c_mode = "color 9"; }
                //
                TSSetWindowTheme(Handle, GlowMain.theme);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Back_Panel.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                SADT_L1.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                SADT_L2.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                SADT_L3.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                SADT_L4.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                SADT_StartBtn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                SADT_StartBtn.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                SADT_StartBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                SADT_StartBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                SADT_StartBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                //
                TSImageRenderer(SADT_StartBtn, GlowMain.theme == 1 ? Properties.Resources.ct_fix_light : Properties.Resources.ct_fix_dark, 18, ContentAlignment.MiddleRight);
                //
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                Text = string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_title"), Application.ProductName);
                // SET UI TEXT
                SADT_L1.Text = software_lang.TSReadLangs("SFCandDISMTool", "sadt_sub_title");
                SADT_L2.Text = software_lang.TSReadLangs("SFCandDISMTool", "sadt_description");
                SADT_L3.Text = software_lang.TSReadLangs("SFCandDISMTool", "sadt_last_repair_time");
                string last_sfc_and_dism_date = software_read_settings.TSReadSettings(ts_settings_container, "SFCAndDISMDate");
                if (last_sfc_and_dism_date != "" && last_sfc_and_dism_date != string.Empty){
                    SADT_L4.Text = last_sfc_and_dism_date;
                }else{
                    SADT_L4.Text = software_lang.TSReadLangs("SFCandDISMTool", "sadt_not_start");
                }
                SADT_StartBtn.Text = " " + software_lang.TSReadLangs("SFCandDISMTool", "sadt_start_engine");
            }catch (Exception){ }
        }
        // SFC AND DISM AUTO TOOL START ENGINE BTN
        // ======================================================================================================
        private void SADT_StartBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                DialogResult sadt_start_check = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_engine_start_notification"), "\n"), sadt_title);
                if (sadt_start_check == DialogResult.Yes){
                    Task sadt_engine_bg = Task.Run(() => Sadt_engine());
                }
            }catch (Exception) { }
        }
        // SFC AND DISM AUTO TOOL ENGINE
        // ======================================================================================================
        private void Sadt_engine(){
            try{
                var commands = new[] {
                    sadt_code_1,
                    sadt_code_2,
                    sadt_code_3,
                    sadt_code_4
                };
                var builder = new System.Text.StringBuilder();
                builder.Append($"title {sadt_title} & {cmd_c_mode} & echo {sadt_start} & ");
                builder.Append(EchoLine());
                for (int i = 0; i < commands.Length; i++){
                    string currentCmd = commands[i];
                    string echoLabel = i == commands.Length - 1 ? sadt_ender : sadt_rotate;
                    //
                    builder.Append($"echo ({currentCmd}) {sadt_starter} & {currentCmd} & ");
                    builder.Append(EchoLine());
                    builder.Append($"echo ({currentCmd}) {echoLabel} & ");
                    builder.Append(EchoLine());
                }
                //
                builder.Append($"echo {sadt_last_text} & ");
                builder.Append(EchoLine(3));
                //
                Process.Start("cmd.exe", "/k " + builder.ToString());
                // Save Process
                string current_time = DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss");
                SADT_L4.Text = current_time;
                software_setting_save.TSWriteSettings(ts_settings_container, "SFCAndDISMDate", current_time);
            }catch (Exception){ }
        }
        private string EchoLine(int repeat = 1){
            return string.Join(" & ", Enumerable.Repeat($"echo {new string('-', 75)}", repeat)) + " & ";
        }
    }
}