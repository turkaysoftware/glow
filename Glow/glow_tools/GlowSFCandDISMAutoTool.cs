using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowSFCandDISMAutoTool : Form{
        public GlowSFCandDISMAutoTool(){ InitializeComponent(); }
        // LOAD
        // ======================================================================================================
        private void GlowSFCandDISMAutoTool_Load(object sender, EventArgs e){
            SADTLoadEngine();
        }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void SADTLoadEngine(){
            try{
                TSThemeModeHelper.InitializeThemeForForm(this);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
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
                // SET UI TEXT
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                TSSettingsSave software_read_settings = new TSSettingsSave(ts_sf);
                //
                Text = string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_title"), Application.ProductName);
                //
                SADT_L1.Text = software_lang.TSReadLangs("SFCandDISMTool", "sadt_sub_title");
                if (!GlowMain.SFCandDISMprocessStatus)
                    SADT_L2.Text = software_lang.TSReadLangs("SFCandDISMTool", "sadt_description");
                SADT_L3.Text = software_lang.TSReadLangs("SFCandDISMTool", "sadt_last_repair_time");
                //
                string lastFixDate = software_read_settings.TSReadSettings(ts_settings_container, "SADTime");
                SADT_L4.Text = !string.IsNullOrWhiteSpace(lastFixDate) ? lastFixDate : software_lang.TSReadLangs("SFCandDISMTool", "sadt_not_start");
                SADT_StartBtn.Text = " " + software_lang.TSReadLangs("SFCandDISMTool", "sadt_start_engine");
            }catch (Exception){ }
        }
        // SFC AND DISM AUTO TOOL START ENGINE BTN
        // ======================================================================================================
        private void SADT_StartBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                DialogResult sadt_start_check = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_engine_start_notification"), "\n"));
                if (sadt_start_check == DialogResult.Yes){
                    Task sadt_engine_bg = Task.Run(() => SadtEngine());
                }
            }catch (Exception) { }
        }
        // SFC AND DISM AUTO TOOL ENGINE
        // ======================================================================================================
        private void SadtEngine(){
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            try{
                var engineCommands = new[]{
                    "sfc /scannow",
                    "DISM /Online /Cleanup-Image /CheckHealth",
                    "DISM /Online /Cleanup-Image /ScanHealth",
                    "DISM /Online /Cleanup-Image /RestoreHealth"
                };
                //
                GlowMain.SFCandDISMprocessStatus = true;
                UpdateSafeEnabled(SADT_StartBtn, false);
                //
                foreach (var cmdCurrentMod in engineCommands){
                    UpdateSafeText(string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_current_running"), cmdCurrentMod, "\n\n"));
                    var processRepair = new Process{
                        StartInfo = new ProcessStartInfo{
                            FileName = "cmd.exe",
                            Arguments = "/c " + cmdCurrentMod,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                        }
                    };
                    processRepair.Start();
                    processRepair.WaitForExit();
                    UpdateSafeText(software_lang.TSReadLangs("SFCandDISMTool", "sadt_description"));
                }
                // UPDATE UI LAST REPAIR DATE
                string current_time = DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss");
                if (SADT_L4.InvokeRequired){
                    SADT_L4.Invoke(new Action(() => SADT_L4.Text = current_time));
                }else{
                    SADT_L4.Text = current_time;
                }
                //
                TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("SFCandDISMTool", "sadt_process_success"));
                // SAVE LAST REPAIR DATE
                try{
                    TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                    software_setting_save.TSWriteSettings(ts_settings_container, "SADTime", current_time);
                }catch (Exception){ }
            }catch (Exception ex){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_process_failed"), "\n\n", ex.Message));
            }finally{
                GlowMain.SFCandDISMprocessStatus = false;
                UpdateSafeEnabled(SADT_StartBtn, true);
            }
        }
        // SAFE TEXT UI THREAD
        // ======================================================================================================
        private void UpdateSafeText(string renderMessage){
            if (InvokeRequired){
                Invoke(new Action(() =>{
                    SADT_L2.Text = renderMessage;
                }));
            }else{
                SADT_L2.Text = renderMessage;
            }
        }
        private void UpdateSafeEnabled(Button renderBtn, bool renderMode){
            if (InvokeRequired){
                Invoke(new Action(() => {
                    renderBtn.Enabled = renderMode;
                }));
            }else{
                renderBtn.Enabled = renderMode;
            }
        }
        // CLOSE CHECK STATUS
        // ======================================================================================================
        private void GlowSFCandDISMAutoTool_FormClosing(object sender, FormClosingEventArgs e){
            if (GlowMain.SFCandDISMprocessStatus){
                e.Cancel = true;
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("GToolsMessage", "gtm_sfc_and_dism_prs_msg"));
            }
        }
    }
}