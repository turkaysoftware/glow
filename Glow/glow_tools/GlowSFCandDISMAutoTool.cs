using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowSFCandDISMAutoTool : Form{
        // FIELDS
        // ======================================================================================================
        private Stopwatch sadtStopwatch;
        private System.Timers.Timer sadtTimer;
        private bool processStatus = true;
        private string titleMessage;
        private string processStatusMessage;
        public GlowSFCandDISMAutoTool(){ InitializeComponent(); }
        // LOAD
        // ======================================================================================================
        // LOAD
        // ======================================================================================================
        private void GlowSFCandDISMAutoTool_Load(object sender, EventArgs e){
            SADTLoadEngine();
            // PROCESS TIMER
            sadtStopwatch = new Stopwatch();
            sadtTimer = new System.Timers.Timer { Interval = 1000, AutoReset = true };
            // DYNAMIC TIMER
            sadtTimer.Elapsed += (s, ev) =>{
                if (sadtStopwatch.IsRunning){
                    if (InvokeRequired){
                        Invoke(new Action(() =>{
                            this.Text = titleMessage + $" - {sadtStopwatch.Elapsed:hh\\:mm\\:ss}";
                        }));
                    }
                }
            };
        }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void SADTLoadEngine(){
            try{
                TSThemeModeHelper.InitializeThemeForForm(this);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Back_Panel.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                SADT_L1.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                SADT_L2.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                SADT_L3.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                SADT_L4.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                SADT_StartBtn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                SADT_StartBtn.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                SADT_StartBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                SADT_StartBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                SADT_StartBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColorHover");
                //
                TSImageRenderer(SADT_StartBtn, GlowMain.theme == 1 ? Properties.Resources.ct_fix_light : Properties.Resources.ct_fix_dark, 18, ContentAlignment.MiddleRight);
                // SET UI TEXT
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                TSSettingsSave software_read_settings = new TSSettingsSave(ts_sf);
                //
                titleMessage = string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_title"), Application.ProductName);
                Text = titleMessage;
                //
                SADT_L1.Text = software_lang.TSReadLangs("SFCandDISMTool", "sadt_sub_title");
                if (!GlowMain.SFCandDISMprocessStatus){
                    SADT_L2.Text = software_lang.TSReadLangs("SFCandDISMTool", "sadt_description");
                }
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
                DialogResult sadt_start_check = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_engine_start_notification"), "\n\n"));
                if (sadt_start_check == DialogResult.Yes){
                    Task sadt_engine_bg = Task.Run(SadtEngine);
                }
            }catch (Exception) { }
        }
        // SFC AND DISM AUTO TOOL ENGINE
        // ======================================================================================================
        private void SadtEngine(){
            processStatus = true;
            processStatusMessage = string.Empty;
            //
            if (InvokeRequired){
                Invoke(new Action(() => { sadtStopwatch.Restart(); sadtTimer.Start(); }));
            }else{
                sadtStopwatch.Restart();
                sadtTimer.Start();
            }
            //
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            string process_message = software_lang.TSReadLangs("SFCandDISMTool", "sadt_current_running_pro");
            //
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
                    UpdateSafeText(SADT_L2, string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_current_running"), cmdCurrentMod, "\n\n"));
                    using (var processRepair = new Process()){
                        processRepair.StartInfo = new ProcessStartInfo{
                            FileName = "cmd.exe",
                            Arguments = "/c " + cmdCurrentMod,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true,
                            CreateNoWindow = true
                        };
                        //
                        Encoding encoding = cmdCurrentMod.StartsWith("sfc", StringComparison.OrdinalIgnoreCase) ? Encoding.Unicode : Encoding.Default;
                        processRepair.Start();
                        using (var streamReader = new StreamReader(processRepair.StandardOutput.BaseStream, encoding)){
                            string buffer = string.Empty;
                            while (!processRepair.HasExited){
                                int c = streamReader.Read();
                                if (c == -1){
                                    break;
                                }
                                char ch = (char)c;
                                buffer += ch;
                                if (buffer.Length > 3000){
                                    buffer = buffer.Substring(buffer.Length - 3000);
                                }
                                //
                                string cleanLine = Regex.Replace(buffer, @"[^0-9%\.\s]", "");
                                cleanLine = Regex.Replace(cleanLine, @"\s+", " ").Trim();
                                //
                                var match = Regex.Match(cleanLine, @"(\d{1,3}(?:\.\d{1,2})?)%");
                                if (match.Success){
                                    string percentValue = match.Groups[1].Value;
                                    if (double.TryParse(percentValue, out double progress)){
                                        UpdateSafeText(SADT_L2, string.Format(process_message, cmdCurrentMod, "\n\n", "\n", percentValue + "%"));
                                    }
                                    buffer = string.Empty;
                                }
                            }
                        }
                        processRepair.WaitForExit();
                    }
                    //
                    UpdateSafeText(SADT_L2, software_lang.TSReadLangs("SFCandDISMTool", "sadt_description"));
                }
                //
                string current_time = DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss");
                UpdateSafeText(SADT_L4, current_time);
                //
                try{
                    TSSettingsSave software_setting_save = new TSSettingsSave(ts_sf);
                    software_setting_save.TSWriteSettings(ts_settings_container, "SADTime", current_time);
                }catch (Exception) { }
            }catch (Exception ex){
                processStatus = false;
                processStatusMessage = ex.Message.Trim();
            }finally{
                if (InvokeRequired){
                    Invoke(new Action(() => { sadtStopwatch.Stop(); sadtTimer.Stop(); }));
                }else{
                    sadtStopwatch.Stop();
                    sadtTimer.Stop();
                }
                //
                GlowMain.SFCandDISMprocessStatus = false;
                UpdateSafeEnabled(SADT_StartBtn, true);
                UpdateSafeText(this, titleMessage);
                //
                TimeSpan totalTime = sadtStopwatch.Elapsed;
                if (processStatus){
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_process_success"), "\n\n", $"{totalTime:hh\\:mm\\:ss}"));
                }else{
                    TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("SFCandDISMTool", "sadt_process_failed"), "\n\n", processStatusMessage, "\n\n", $"{totalTime:hh\\:mm\\:ss}"));
                }
            }
        }
        // SAFE TEXT UI THREAD
        // ======================================================================================================
        private void UpdateSafeText(Form renderForm, string renderMessage){
            if (InvokeRequired){
                Invoke(new Action(() => { renderForm.Text = renderMessage; }));
            }else{
                renderForm.Text = renderMessage;
            }
        }
        private void UpdateSafeText(Label renderLabel, string renderMessage){
            if (InvokeRequired){
                Invoke(new Action(() =>{ renderLabel.Text = renderMessage; }));
            }else{
                renderLabel.Text = renderMessage;
            }
        }
        private void UpdateSafeEnabled(Button renderBtn, bool renderMode){
            if (InvokeRequired){
                Invoke(new Action(() => { renderBtn.Enabled = renderMode; }));
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