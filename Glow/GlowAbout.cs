using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using static Glow.TSModules;

namespace Glow{
    public partial class GlowAbout : Form{
        public GlowAbout(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // ======================================================================================================
        // MEDIA LINK SYSTEM
        TS_LinkSystem TS_LinkSystem = new TS_LinkSystem();
        private void GlowAbout_Load(object sender, EventArgs e){
            try{
                // GET PRELOAD SETTINGS
                about_preloader();
                // IMAGES
                About_Image.Image = Properties.Resources.glow_logo;
            }catch (Exception){ }
        }
        // ======================================================================================================
        // DYNAMIC THEME VOID
        public void about_preloader(){
            try{
                // COLOR SETTINGS
                int set_attribute = Glow.theme == 1 ? 20 : 19;
                if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != Glow.theme){
                    DwmSetWindowAttribute(Handle, 20, new[] { Glow.theme == 1 ? 0 : 1 }, 4);
                }
                BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                About_BG_Panel.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                About_L1.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                About_L2.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                //
                About_WebsiteBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_WebsiteBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_WebsiteBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_WebsiteBtn.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                //
                About_XBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_XBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_XBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_XBtn.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                //
                About_InstagramBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_InstagramBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_InstagramBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_InstagramBtn.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                //
                About_GitHubBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_GitHubBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_GitHubBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                About_GitHubBtn.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                // ======================================================================================================
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                TS_VersionEngine glow_version = new TS_VersionEngine();
                // TEXTS
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_title").Trim())), Application.ProductName);
                About_L1.Text = glow_version.TS_SofwareVersion(0, Program.ts_version_mode);
                About_L2.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_copyright").Trim())), "\u00a9", TS_SoftwareCopyrightDate.ts_scd, Application.CompanyName);
                About_WebsiteBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_website_page").Trim()));
                About_XBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_twitter_page").Trim()));
                About_InstagramBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_instagram_page").Trim()));
                About_GitHubBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_github_page").Trim()));
            }catch (Exception){ }
        }
        // WEBSITE LINK
        private void About_WebsiteBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(TS_LinkSystem.website_link);
            }catch (Exception){ }
        }
        // X LINK
        // ======================================================================================================
        private void About_XBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(TS_LinkSystem.twitter_link);
            }catch (Exception){ }
        }
        // INSTAGRAM LINK
        // ======================================================================================================
        private void About_InstagramBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(TS_LinkSystem.instagram_link);
            }catch (Exception){ }
        }
        // GITHUB LINK
        // ======================================================================================================
        private void About_GitHubBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(TS_LinkSystem.github_link);
            }catch (Exception){ }
        }
    }
}