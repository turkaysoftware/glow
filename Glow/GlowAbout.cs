using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using static Glow.GlowModules;

namespace Glow{
    public partial class GlowAbout : Form{
        public GlowAbout(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        private void GlowAbout_Load(object sender, EventArgs e){
            try{
                // GET PRELOAD SETTINGS
                about_preloader();
                // IMAGES
                About_Image.BackgroundImage = Properties.Resources.glow_logo;
            }catch (Exception){ }
        }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void about_preloader(){
            try{
                // COLOR SETTINGS
                if (Glow.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                }else if (Glow.theme == 0 || Glow.theme == 2){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                }
                BackColor = Glow.ui_colors[5];
                About_BG_Panel.BackColor = Glow.ui_colors[6];
                About_L1.ForeColor = Glow.ui_colors[7];
                About_L2.ForeColor = Glow.ui_colors[7];
                About_L3.ForeColor = Glow.ui_colors[7];
                About_GitHubBtn.BackColor = Glow.ui_colors[8];
                About_GitHubBtn.FlatAppearance.BorderColor = Glow.ui_colors[8];
                About_GitHubBtn.ForeColor = Glow.ui_colors[19];
                // ======================================================================================================
                // GLOBAL LANGS PATH
                GlowGetLangs g_lang = new GlowGetLangs(Glow.lang_path);
                GlowVersionEngine glow_version = new GlowVersionEngine();
                // TEXTS
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("GlowAbout", "ga_title").Trim())), Application.ProductName);
                About_L1.Text = glow_version.GlowVersion(Glow.g_version_mode);
                About_L2.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("GlowAbout", "ga_copyright").Trim())), "\u00a9", DateTime.Now.Year, Application.CompanyName);
                About_L3.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("GlowAbout", "ga_open_source").Trim())), Application.ProductName);
                About_GitHubBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("GlowAbout", "ga_github_page").Trim()));
            }catch (Exception){ }
        }
        // ABOUT GITHUB ROTATE BUTTON
        // ======================================================================================================
        private void About_GitHubBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(Glow.github_link + "/glow");
            }catch (Exception){ }
        }
    }
}