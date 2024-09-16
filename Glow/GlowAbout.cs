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
                if (Glow.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                }else if (Glow.theme == 0){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                }
                BackColor = Glow.ui_colors[5];
                About_BG_Panel.BackColor = Glow.ui_colors[6];
                About_L1.ForeColor = Glow.ui_colors[7];
                About_L2.ForeColor = Glow.ui_colors[7];
                //
                About_WebsiteBtn.BackColor = Glow.ui_colors[8];
                About_WebsiteBtn.FlatAppearance.MouseDownBackColor = Glow.ui_colors[8];
                About_WebsiteBtn.FlatAppearance.BorderColor = Glow.ui_colors[8];
                About_WebsiteBtn.ForeColor = Glow.ui_colors[18];
                //
                About_GitHubBtn.BackColor = Glow.ui_colors[8];
                About_GitHubBtn.FlatAppearance.MouseDownBackColor = Glow.ui_colors[8];
                About_GitHubBtn.FlatAppearance.BorderColor = Glow.ui_colors[8];
                About_GitHubBtn.ForeColor = Glow.ui_colors[18];
                //
                About_XBtn.BackColor = Glow.ui_colors[8];
                About_XBtn.FlatAppearance.MouseDownBackColor = Glow.ui_colors[8];
                About_XBtn.FlatAppearance.BorderColor = Glow.ui_colors[8];
                About_XBtn.ForeColor = Glow.ui_colors[18];
                // ======================================================================================================
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                TS_VersionEngine glow_version = new TS_VersionEngine();
                // TEXTS
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_title").Trim())), Application.ProductName);
                About_L1.Text = glow_version.TS_SofwareVersion(0, Glow.ts_version_mode);
                About_L2.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_copyright").Trim())), "\u00a9", DateTime.Now.Year, Application.CompanyName);
                About_WebsiteBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_website_page").Trim()));
                About_GitHubBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_github_page").Trim()));
                About_XBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_twitter_page").Trim()));
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
        // GITHUB LINK
        // ======================================================================================================
        private void About_GitHubBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(TS_LinkSystem.github_link + "/glow");
            }catch (Exception){ }
        }
    }
}