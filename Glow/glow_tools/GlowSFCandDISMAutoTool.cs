using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowSFCandDISMAutoTool : Form{
        public GlowSFCandDISMAutoTool(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // ======================================================================================================
        // GLOBAL GLOBAL CLASS
        TSSettingsSave software_read_settings = new TSSettingsSave(ts_sf);
        TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
        // ======================================================================================================
        // GLOBAL STRINGS
        string sadt_title, sadt_start, sadt_starter, sadt_rotate, sadt_ender, sadt_last_text, cmd_c_mode;
        // ======================================================================================================
        // REPAIR CODES
        string sadt_code_1 = "sfc /scannow";
        string sadt_code_2 = "DISM /Online /Cleanup-Image /CheckHealth";
        string sadt_code_3 = "DISM /Online /Cleanup-Image /ScanHealth";
        string sadt_code_4 = "DISM /Online /Cleanup-Image /RestoreHealth";
        private void GlowSFCandDISMAutoTool_Load(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            // GET THEME
            sadt_theme_settings();
            // SET TEXT
            sadt_title = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_title").Trim())), Application.ProductName);
            sadt_start = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_starting_engine").Trim()));
            sadt_starter = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_command_starting").Trim()));
            sadt_rotate = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_command_success_and_next").Trim()));
            sadt_ender = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_command_last_success").Trim()));
            sadt_last_text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_stop_engine").Trim()));
        }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void sadt_theme_settings(){
            try{
                if (Glow.theme == 0){ cmd_c_mode = "color 3"; }
                else if (Glow.theme == 1){ cmd_c_mode = "color 9"; }
                //
                int set_attribute = Glow.theme == 1 ? 20 : 19;
                if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != Glow.theme){
                    DwmSetWindowAttribute(Handle, 20, new[] { Glow.theme == 1 ? 0 : 1 }, 4);
                }
                BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                SFCAndDISM_TLP.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                SADT_L1.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                SADT_L2.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                SADT_L3.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                SADT_L4.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                SADT_StartBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                SADT_StartBtn.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                SADT_StartBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                SADT_StartBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                //
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_title").Trim())), Application.ProductName);
                // SET UI TEXT
                SADT_L1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_sub_title").Trim()));
                SADT_L2.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_description").Trim()));
                SADT_L3.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_last_repair_time").Trim()));
                string last_sfc_and_dism_date = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_read_settings.TSReadSettings(ts_settings_container, "SFCAndDISMDate").Trim()));
                if (last_sfc_and_dism_date != "" && last_sfc_and_dism_date != string.Empty){
                    SADT_L4.Text = last_sfc_and_dism_date;
                }else{
                    SADT_L4.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_not_start").Trim()));
                }
                SADT_StartBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_start_engine").Trim()));
            }catch (Exception){ }
        }
        // SFC AND DISM AUTO TOOL START ENGINE BTN
        // ======================================================================================================
        private void SADT_StartBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                DialogResult sadt_start_check = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SFCandDISMTool", "sadt_engine_start_notification").Trim())), "\n"), sadt_title, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (sadt_start_check == DialogResult.Yes){
                    Task sadt_engine_bg = new Task(sadt_engine);
                    sadt_engine_bg.Start();
                }
            }catch (Exception){ }
        }
        // SFC AND DISM AUTO TOOL ENGINE
        // ======================================================================================================
        private void sadt_engine(){
            try{
                Process.Start("cmd.exe", "/k " + $"title {sadt_title} & {cmd_c_mode} & echo {sadt_start} &" +
                $" echo {new string('-', 75)} & echo ({sadt_code_1}) {sadt_starter} & {sadt_code_1} &" +
                $" echo {new string('-', 75)} & echo ({sadt_code_1}) {sadt_rotate} & echo {new string('-', 75)} &" +
                $" {sadt_code_2} & echo {new string('-', 75)} & echo ({sadt_code_2}) {sadt_rotate} & echo {new string('-', 75)} &" +
                $" {sadt_code_3} & echo {new string('-', 75)} & echo ({sadt_code_3}) {sadt_rotate} & echo {new string('-', 75)} &" +
                $" {sadt_code_4} & echo {new string('-', 75)} & echo ({sadt_code_4}) {sadt_ender} & echo {new string('-', 75)} &" +
                $" echo {new string('-', 75)} & echo {new string('-', 75)} & echo {sadt_last_text} & echo {new string('-', 75)} &" +
                $" echo {new string('-', 75)} & echo {new string('-', 75)}");
                // PROCESS TIME WRITE SCREEN AND SAVE
                try{
                    string current_time = DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss");
                    SADT_L4.Text = current_time;
                    software_setting_save.TSWriteSettings(ts_settings_container, "SFCAndDISMDate", current_time);
                }catch (Exception){ }
            }catch (Exception){ }
        }
    }
}