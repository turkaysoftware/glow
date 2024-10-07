using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowNetworkFixTool : Form{
        public GlowNetworkFixTool(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void nft_theme_settings(){
            try{
                if (Glow.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                }else if (Glow.theme == 0){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                }
                //
                BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                //
                Panel_BG.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                //
                NFT_TitleLabel.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                NFT_TitleLabel.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                //
                NFT_ResultList.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                NFT_ResultList.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                //
                NFT_StartBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                NFT_StartBtn.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                NFT_StartBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                NFT_StartBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                //
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("NetworkFixTool", "nft_title").Trim())), Application.ProductName);
                //
                NFT_StartBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("NetworkFixTool", "nft_process_start_btn").Trim()));
            }catch (Exception){ }
        }
        // LOADA
        // ======================================================================================================
        private void GlowNetworkFixTool_Load(object sender, EventArgs e){
            nft_theme_settings();
            //
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            NFT_TitleLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("NetworkFixTool", "nft_title_label_before_start").Trim()));
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
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                DialogResult start_engine_query = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("NetworkFixTool", "nft_process_start_query").Trim())), "\n"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (start_engine_query == DialogResult.Yes){
                    start_network_fix_engine();
                    NFT_TitleLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("NetworkFixTool", "nft_title_label_in_process").Trim()));
                }
            }catch (Exception){ }
        }
        // NETWORK FIX ENGINE STARTER
        // ======================================================================================================
        private async void start_network_fix_engine(){
            try{
                NFT_ResultList.Items.Clear();
                // Network Fix Command
                await ts_RunNetworkFixCommandAsync("netsh", "winsock reset");
                await ts_RunNetworkFixCommandAsync("netsh", "int ip reset");
                await ts_RunNetworkFixCommandAsync("ipconfig", "/release");
                await ts_RunNetworkFixCommandAsync("ipconfig", "/renew");
                await ts_RunNetworkFixCommandAsync("ipconfig", "/flushdns");
                //
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                NFT_TitleLabel.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("NetworkFixTool", "nft_title_label_after_end").Trim()));
                // Process After Dialog
                DialogResult end_engine_query = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("NetworkFixTool", "nft_process_after_query").Trim())), "\n\n", "\n\n", "\n\n"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
                        MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("NetworkFixTool", "nft_process_after_restart_info").Trim())), "\n"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }catch (Exception){ }
        }
        // NETWORK FIX ENGINE
        // ======================================================================================================
        private async Task ts_RunNetworkFixCommandAsync(string get_command, string get_arguments){
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
                        TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                        if (!string.IsNullOrEmpty(get_result)){
                            Invoke(new Action(() => {
                                NFT_ResultList.Items.Add(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("NetworkFixTool", "nft_process_code_transfer").Trim())), get_command, get_arguments));
                            }));
                        }
                        if (!string.IsNullOrEmpty(get_error)){
                            Invoke(new Action(() => {
                                NFT_ResultList.Items.Add(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("NetworkFixTool", "nft_process_code_transfer_error").Trim())), get_command, get_arguments, get_error));
                            }));
                        }
                    }
                });
            }catch (Exception){ }
        }
    }
}