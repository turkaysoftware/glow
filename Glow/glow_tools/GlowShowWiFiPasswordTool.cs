using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
//
using static Glow.TSModules;
using System.Drawing;

namespace Glow.glow_tools{
    public partial class GlowShowWiFiPasswordTool : Form{
        public GlowShowWiFiPasswordTool(){ InitializeComponent(); }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void Swpt_theme_settings(){
            try{
                TSSetWindowTheme(Handle, GlowMain.theme);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                //
                Panel_BG.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                //
                SWPT_TitleLabel.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                SWPT_TitleLabel.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                //
                SWPT_SelectBox.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                SWPT_SelectBox.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                //
                SWPT_ResultBox.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                SWPT_ResultBox.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                //
                SWPT_CopyBtn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                SWPT_CopyBtn.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                SWPT_CopyBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                SWPT_CopyBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                SWPT_CopyBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                //
                TSImageRenderer(SWPT_CopyBtn, GlowMain.theme == 1 ? Properties.Resources.ct_copy_mc_light : Properties.Resources.ct_copy_mc_dark, 18, ContentAlignment.MiddleRight);
                //
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                Text = string.Format(software_lang.TSReadLangs("ShowWiFiPasswordTool", "swpt_title"), Application.ProductName);
                //
                SWPT_TitleLabel.Text = software_lang.TSReadLangs("ShowWiFiPasswordTool", "swpt_in_title");
                SWPT_CopyBtn.Text = " " + software_lang.TSReadLangs("ShowWiFiPasswordTool", "swpt_copy_btn");
            }catch (Exception){ }
        }
        // LOAD SWPT
        // ======================================================================================================
        private async void GlowShowWiFiPasswordTool_Load(object sender, EventArgs e){
            Swpt_theme_settings();
            //
            try{
                var wifiProfilesTask = Task.Run(() =>{
                    TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                    string[] wifi_profileNames = Ts_ExtractWiFiProfileNames(Ts_GetWiFiPassword("netsh wlan show profile"));
                    return new { software_lang, wifi_profileNames };
                });
                var get_result = await wifiProfilesTask;
                if (get_result.wifi_profileNames.Length == 0){
                    TS_MessageBoxEngine.TS_MessageBox(this, 2, string.Format(get_result.software_lang.TSReadLangs("ShowWiFiPasswordTool", "swpt_no_profile"), "\n", get_result.software_lang.TSReadLangs("HeaderTools", "ht_show_wifi_password_tool")));
                    Close();
                }else{
                    foreach (var wifi_profile in get_result.wifi_profileNames){
                        if (SWPT_SelectBox.InvokeRequired){
                            SWPT_SelectBox.Invoke(new Action(() => SWPT_SelectBox.Items.Add(wifi_profile.Trim())));
                        }else{
                            SWPT_SelectBox.Items.Add(wifi_profile.Trim());
                        }
                    }
                }
            }catch (Exception){ }
        }
        // SELECT WIFI PROFILE GET PASSWORD
        // ======================================================================================================
        private void SWPT_SelectBox_SelectedIndexChanged(object sender, EventArgs e){
            try{
                string networkName = SWPT_SelectBox.SelectedItem.ToString().Trim();
                string wifiDetails = Ts_GetWiFiPassword($"netsh wlan show profile \"{networkName}\" key=clear");
                string passwordKey = "Key Content            : ";
                int startIndex = wifiDetails.IndexOf(passwordKey);
                if (startIndex != -1){
                    startIndex += passwordKey.Length;
                    int endIndex = wifiDetails.IndexOf("\n", startIndex);
                    string password = wifiDetails.Substring(startIndex, endIndex - startIndex).Trim();
                    //Console.WriteLine($"\n{networkName} şifresi: {password}");
                    SWPT_ResultBox.Text = password.Trim();
                    SWPT_CopyBtn.Enabled = true;
                }else{
                    TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                    SWPT_ResultBox.Text = string.Format(software_lang.TSReadLangs("ShowWiFiPasswordTool", "swpt_select_profile_no_password"), networkName);
                }
            }catch (Exception){ }
        }
        // EXECUTE CMD INTERFACE CODE
        // ======================================================================================================
        static string Ts_GetWiFiPassword(string get_command){
            ProcessStartInfo wifi_psi = new ProcessStartInfo{
                FileName = "cmd.exe",
                Arguments = $"/c {get_command}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };
            using (Process process_result = Process.Start(wifi_psi)){
                string wifi_output_result = process_result.StandardOutput.ReadToEnd();
                process_result.WaitForExit();
                return wifi_output_result;
            }
        }
        // GET ONLY THE REQUIRED SITE WITH CMD
        // ======================================================================================================
        static string[] Ts_ExtractWiFiProfileNames(string get_wifi_profiles){
            string userProfilesSection = "User profiles";
            int startIndex = get_wifi_profiles.IndexOf(userProfilesSection);
            if (startIndex == -1){
                return new string[0];
            }
            string profileSection = get_wifi_profiles.Substring(startIndex);
            MatchCollection matches = Regex.Matches(profileSection, @"All User Profile\s*:\s*(.*)");
            string[] profileNames = new string[matches.Count];
            for (int i = 0; i < matches.Count; i++){
                profileNames[i] = matches[i].Groups[1].Value.Trim();
            }
            return profileNames;
        }
        // COPY SELECTED WIFI PASSWORD
        // ======================================================================================================
        private void SWPT_CopyBtn_Click(object sender, EventArgs e){
            try{
                Clipboard.SetText(SWPT_ResultBox.Text.Trim());
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("ShowWiFiPasswordTool", "swpt_copy_txt"));
            }
            catch (Exception){ }
        }
    }
}