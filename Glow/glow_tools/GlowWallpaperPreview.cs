using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowWallpaperPreview : Form{
        public GlowWallpaperPreview(){ InitializeComponent(); }
        // LOAD
        // ======================================================================================================
        private void GlowWallpaperPreview_Load(object sender, EventArgs e){
            WP_Preview_theme_settings();
            Task.Run(() => { AsyncLoadWallpaper(); }); 
        }
        private void AsyncLoadWallpaper(){
            SetPictureBoxImage(WPImage, Image.FromFile(GlowMain.wp_rotate));
        }
        // THEME SETTINGS
        // ======================================================================================================
        public void WP_Preview_theme_settings(){
            try{
                TSSetWindowTheme(Handle, GlowMain.theme);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                foreach (Control ui_buttons in BackPanel.Controls){
                    if (ui_buttons is Button open_btn){
                        open_btn.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                        open_btn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                        open_btn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                        open_btn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                        open_btn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                    }
                }
                //
                TSImageRenderer(BtnWallpaperLocationBtn, GlowMain.theme == 1 ? Properties.Resources.ct_link_mc_light : Properties.Resources.ct_link_mc_dark, 18, ContentAlignment.MiddleRight);
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                Text = string.Format(software_lang.TSReadLangs("WallpaperPreviewTool", "wpt_title"), Application.ProductName);
                BtnWallpaperLocationBtn.Text = " " + software_lang.TSReadLangs("WallpaperPreviewTool", "wpt_btn");
            }catch (Exception){ }
        }
        // OPEN WALLPAPER TO EXPLORER
        // ======================================================================================================
        private void BtnWallpaperLocationBtn_Click(object sender, EventArgs e){
            try{
                string wallpaper_start_path = string.Format("/select, \"{0}\"", GlowMain.wp_rotate.Trim().Replace("/", @"\"));
                ProcessStartInfo psi = new ProcessStartInfo("explorer.exe", wallpaper_start_path);
                Process.Start(psi);
            }catch (Exception){
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 3, software_lang.TSReadLangs("Os_Content", "os_c_wallpaper_open_error"));
            }
        }
        // DISPOSE AND EXIT
        // ======================================================================================================
        private void GlowWallpaperPreview_FormClosing(object sender, FormClosingEventArgs e){
            SetPictureBoxImage(WPImage, null);
            Hide();
        }
    }
}