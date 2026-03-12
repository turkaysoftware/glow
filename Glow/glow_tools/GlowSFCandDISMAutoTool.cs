using System;
using System.Collections.Generic;
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
        private Timer sadtUiTimer;
        private bool processStatus = true;
        private string titleMessage;
        private string processStatusMessage;
        public GlowSFCandDISMAutoTool() { InitializeComponent(); }
        // LOAD
        // ======================================================================================================
        private void GlowSFCandDISMAutoTool_Load(object sender, EventArgs e){
            SADTLoadEngine();
            sadtStopwatch = new Stopwatch();
            sadtUiTimer = new Timer{
                Interval = 500
            };
            sadtUiTimer.Tick += (s, ev) => {
                if (!sadtStopwatch.IsRunning) return;
                var eTime = sadtStopwatch.Elapsed;
                this.Text = titleMessage + $" - {eTime:hh\\:mm\\:ss}";
                int msToNextSecond = 1000 - eTime.Milliseconds;
                sadtUiTimer.Interval = Math.Max(50, msToNextSecond);
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
                TSSettingsModule software_read_settings = new TSSettingsModule(ts_sf);
                //
                titleMessage = string.Format(software_lang.TSReadLangs("DISMandSFCTool", "sadt_title"), Application.ProductName);
                Text = titleMessage;
                //
                SADT_L1.Text = software_lang.TSReadLangs("DISMandSFCTool", "sadt_sub_title");
                if (!GlowMain.SFCandDISMprocessStatus){
                    SADT_L2.Text = software_lang.TSReadLangs("DISMandSFCTool", "sadt_description");
                }
                SADT_L3.Text = software_lang.TSReadLangs("DISMandSFCTool", "sadt_last_repair_time");
                //
                string lastFixDate = software_read_settings.TSReadSettings(ts_settings_container, "SADTime");
                SADT_L4.Text = !string.IsNullOrWhiteSpace(lastFixDate) ? lastFixDate : software_lang.TSReadLangs("DISMandSFCTool", "sadt_not_start");
                SADT_StartBtn.Text = " " + software_lang.TSReadLangs("DISMandSFCTool", "sadt_start_engine");
            }
            catch (Exception) { }
        }
        // SFC AND DISM AUTO TOOL START ENGINE BTN
        // ======================================================================================================
        private void SADT_StartBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                DialogResult sadt_start_check = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("DISMandSFCTool", "sadt_engine_start_notification"), "\n\n"));
                if (sadt_start_check == DialogResult.Yes){
                    Task sadt_engine_bg = Task.Run(SadtEngine);
                }
            }catch (Exception){ }
        }
        // SFC AND DISM AUTO TOOL ENGINE
        // ======================================================================================================
        private void SadtEngine(){
            processStatus = true;
            processStatusMessage = string.Empty;
            //
            var repairedCommands = new List<string>();
            bool isComponentStoreCorrupt = false;
            //
            BeginInvoke(new Action(() => {
                sadtStopwatch.Restart();
                sadtUiTimer.Start();
            }));
            //
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            string process_message = software_lang.TSReadLangs("DISMandSFCTool", "sadt_current_running_pro");
            //
            try{
                var engineCommands = new[]{
                    "DISM /Online /Cleanup-Image /CheckHealth",
                    "DISM /Online /Cleanup-Image /ScanHealth",
                    "DISM /Online /Cleanup-Image /RestoreHealth",
                    "sfc /scannow"
                };
                //
                GlowMain.SFCandDISMprocessStatus = true;
                UpdateSafeEnabled(SADT_StartBtn, false);
                //
                foreach (var cmdCurrentMod in engineCommands){
                    UpdateSafeText(SADT_L2, string.Format(software_lang.TSReadLangs("DISMandSFCTool", "sadt_current_running"), cmdCurrentMod, "\n\n"));
                    //
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
                        StringBuilder fullStdOut = new StringBuilder();
                        //
                        using (var reader = new StreamReader(processRepair.StandardOutput.BaseStream, encoding)){
                            string buffer = string.Empty;
                            while (!processRepair.HasExited){
                                int c = reader.Read();
                                if (c == -1) break;
                                char ch = (char)c;
                                buffer += ch;
                                fullStdOut.Append(ch);
                                if (buffer.Length > 3000) buffer = buffer.Substring(buffer.Length - 3000);
                                //
                                string cleanLine = Regex.Replace(buffer, @"[^0-9%\.\s]", "");
                                cleanLine = Regex.Replace(cleanLine, @"\s+", " ").Trim();
                                var match = Regex.Match(cleanLine, @"(\d{1,3}(?:\.\d{1,2})?)%");
                                //
                                if (match.Success){
                                    UpdateSafeText(SADT_L2, string.Format(process_message, cmdCurrentMod, "\n\n", "\n", match.Groups[1].Value + "%"));
                                    buffer = string.Empty;
                                }
                            }
                        }
                        //
                        processRepair.WaitForExit();
                        string combinedOutput = (fullStdOut.ToString() + "\n" + processRepair.StandardError.ReadToEnd()).Trim();
                        // DISM CHECK
                        if (cmdCurrentMod.StartsWith("DISM", StringComparison.OrdinalIgnoreCase)){
                            if (combinedOutput.Contains("Error:") || processRepair.ExitCode != 0){
                                processStatus = false;
                                processStatusMessage = combinedOutput;
                                break;
                            }
                            if (cmdCurrentMod.Contains("/CheckHealth") || cmdCurrentMod.Contains("/ScanHealth")){
                                if (combinedOutput.Contains("The component store is repairable")){
                                    isComponentStoreCorrupt = true;
                                    repairedCommands.Add(cmdCurrentMod);
                                }
                            }else if (cmdCurrentMod.Contains("/RestoreHealth")){
                                if (isComponentStoreCorrupt && combinedOutput.Contains("The operation completed successfully")){
                                    repairedCommands.Add(cmdCurrentMod);
                                }
                            }
                        }
                        // SFC CHECK
                        else if (cmdCurrentMod.StartsWith("sfc", StringComparison.OrdinalIgnoreCase)){
                            bool sfcFailed = Regex.IsMatch(combinedOutput, @"found corrupt files but was unable to fix|could not perform|verification failed", RegexOptions.IgnoreCase);
                            if (sfcFailed || processRepair.ExitCode != 0){
                                processStatus = false;
                                processStatusMessage = combinedOutput;
                                break;
                            }
                            if (combinedOutput.Contains("found corrupt files and successfully repaired them")){
                                repairedCommands.Add(cmdCurrentMod);
                            }
                        }
                    }
                    UpdateSafeText(SADT_L2, software_lang.TSReadLangs("DISMandSFCTool", "sadt_description"));
                    if (!processStatus) break;
                }
                //
                string current_time = DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss");
                UpdateSafeText(SADT_L4, current_time);
                try { new TSSettingsModule(ts_sf).TSWriteSettings(ts_settings_container, "SADTime", current_time); } catch { }
            }catch (Exception ex){
                processStatus = false;
                processStatusMessage = ex.Message.Trim();
            }finally{
                BeginInvoke(new Action(() => { sadtUiTimer.Stop(); sadtStopwatch.Stop(); }));
                GlowMain.SFCandDISMprocessStatus = false;
                UpdateSafeEnabled(SADT_StartBtn, true);
                UpdateSafeText(this, titleMessage);
                //
                TimeSpan totalTime = sadtStopwatch.Elapsed;
                if (processStatus){
                    string finalMsg = string.Format(software_lang.TSReadLangs("DISMandSFCTool", "sadt_process_success"), "\n\n", $"{totalTime:hh\\:mm\\:ss}");
                    if (repairedCommands.Count > 0){
                        finalMsg += "\n\n" + software_lang.TSReadLangs("DISMandSFCTool", "sadt_repair_codes") + "\n\n";
                        foreach (var cmd in repairedCommands) finalMsg += $"- {cmd}\n";
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, finalMsg);
                }else{
                    TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("DISMandSFCTool", "sadt_process_failed"), "\n\n", processStatusMessage, "\n\n", $"{totalTime:hh\\:mm\\:ss}"));
                }
            }
        }
        // SAFE TEXT UI THREAD
        // ======================================================================================================
        private void UpdateSafeText(Form renderForm, string renderMessage){
            if (renderForm.InvokeRequired){
                renderForm.BeginInvoke(new Action(() => renderForm.Text = renderMessage));
            }else{
                renderForm.Text = renderMessage;
            }
        }
        private void UpdateSafeText(Label renderLabel, string renderMessage){
            if (renderLabel.InvokeRequired){
                renderLabel.BeginInvoke(new Action(() => renderLabel.Text = renderMessage));
            }else{
                renderLabel.Text = renderMessage;
            }
        }
        private void UpdateSafeEnabled(Button renderBtn, bool renderMode){
            if (renderBtn.InvokeRequired){
                renderBtn.BeginInvoke(new Action(() => renderBtn.Enabled = renderMode));
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
                TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("GToolsMessage", "gtm_dism_and_sfc_prs_msg"));
            }
        }
    }
}