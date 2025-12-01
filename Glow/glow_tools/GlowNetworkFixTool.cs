using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
//
using static Glow.TSModules;
using System.Drawing;

namespace Glow.glow_tools{
    public partial class GlowNetworkFixTool : Form{
        public GlowNetworkFixTool(){ InitializeComponent(); }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void Nft_theme_settings(){
            try{
                TSThemeModeHelper.InitializeThemeForForm(this);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                //
                Panel_BG.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                //
                NFT_TitleLabel.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                NFT_TitleLabel.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                //
                NFT_ResultList.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                NFT_ResultList.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                //
                NFT_StartBtn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                NFT_StartBtn.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                NFT_StartBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                NFT_StartBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                NFT_StartBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColorHover");
                //
                TSImageRenderer(NFT_StartBtn, GlowMain.theme == 1 ? Properties.Resources.ct_fix_light : Properties.Resources.ct_fix_dark, 18, ContentAlignment.MiddleRight);
                //
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                Text = string.Format(software_lang.TSReadLangs("NetworkFixTool", "nft_title"), Application.ProductName);
                //
                NFT_StartBtn.Text = " " + software_lang.TSReadLangs("NetworkFixTool", "nft_process_start_btn");
            }catch (Exception){ }
        }
        // LOADA
        // ======================================================================================================
        private void GlowNetworkFixTool_Load(object sender, EventArgs e){
            Nft_theme_settings();
            //
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            NFT_TitleLabel.Text = software_lang.TSReadLangs("NetworkFixTool", "nft_title_label_before_start");
        }
        // RESULT LIST CLEAR SELECTION
        // ======================================================================================================
        private void NFT_ResultList_SelectedIndexChanged(object sender, EventArgs e){
            NFT_ResultList.SelectedIndex = -1;
            NFT_ResultList.ClearSelected();
        }
        // NETWORK FIX ENGINE STARTER BTN
        // ======================================================================================================
        private void NFT_StartBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                DialogResult start_engine_query = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(software_lang.TSReadLangs("NetworkFixTool", "nft_process_start_query"), "\n"));
                if (start_engine_query == DialogResult.Yes){
                    Start_network_fix_engine();
                    NFT_TitleLabel.Text = software_lang.TSReadLangs("NetworkFixTool", "nft_title_label_in_process");
                }
            }catch (Exception){ }
        }
        // NETWORK FIX ENGINE STARTER
        // ======================================================================================================
        private async void Start_network_fix_engine(){
            try{
                NFT_ResultList.Items.Clear();
                NFT_StartBtn.Enabled = false;
                // Network Fix Command
                await Ts_RunNetworkFixCommandAsync("netsh", "winsock reset");
                await Ts_RunNetworkFixCommandAsync("netsh", "int ip reset");
                await Ts_RunNetworkFixCommandAsync("ipconfig", "/release");
                await Ts_RunNetworkFixCommandAsync("ipconfig", "/renew");
                await Ts_RunNetworkFixCommandAsync("ipconfig", "/flushdns");
                //
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                NFT_TitleLabel.Text = software_lang.TSReadLangs("NetworkFixTool", "nft_title_label_after_end");
                NFT_StartBtn.Enabled = true;
                // Process After Dialog
                DialogResult end_engine_query = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(software_lang.TSReadLangs("NetworkFixTool", "nft_process_after_query"), "\n\n", "\n\n", "\n\n"));
                if (end_engine_query == DialogResult.Yes){
                    try{
                        ProcessStartInfo pc_restart_query = new ProcessStartInfo{
                            FileName = "shutdown",
                            Arguments = "/r /t 0",
                            CreateNoWindow = true,
                            UseShellExecute = false,
                            RedirectStandardOutput = true,
                            RedirectStandardError = true
                        };
                        using (Process pc_restart_starter = Process.Start(pc_restart_query)){
                            pc_restart_starter.WaitForExit();
                        }
                    }catch (Exception){
                        TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("NetworkFixTool", "nft_process_after_restart_info"), "\n"));
                    }
                }
            }catch (Exception){ }
        }
        // NETWORK FIX ENGINE
        // ======================================================================================================
        private async Task Ts_RunNetworkFixCommandAsync(string get_command, string get_arguments){
            try{
                await Task.Run(() => {
                    ProcessStartInfo start_network_fix_process = new ProcessStartInfo{
                        FileName = "cmd.exe",
                        Arguments = $"/c {get_command} {get_arguments}",
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    using (Process network_fix_runner = Process.Start(start_network_fix_process)){
                        network_fix_runner.WaitForExit();
                        string get_result = network_fix_runner.StandardOutput.ReadToEnd();
                        string get_error = network_fix_runner.StandardError.ReadToEnd();
                        TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                        if (!string.IsNullOrEmpty(get_result)){
                            Invoke(new Action(() => {
                                NFT_ResultList.Items.Add(string.Format(software_lang.TSReadLangs("NetworkFixTool", "nft_process_code_transfer"), get_command, get_arguments));
                            }));
                        }
                        if (!string.IsNullOrEmpty(get_error)){
                            Invoke(new Action(() => {
                                NFT_ResultList.Items.Add(string.Format(software_lang.TSReadLangs("NetworkFixTool", "nft_process_code_transfer_error"), get_command, get_arguments, get_error));
                            }));
                        }
                    }
                });
            }catch (Exception){ }
        }
    }
}