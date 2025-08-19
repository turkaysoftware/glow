using System;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
//
using static Glow.TSModules;

namespace Glow{
    public partial class GlowAbout : Form{
        public GlowAbout(){
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, AboutTable, new object[]{ true });
            //
            PanelHeader.Parent = ImageAbout;
            CloseAboutBtn.Parent = PanelHeader;
            //
            TSImageRenderer(CloseAboutBtn, Properties.Resources.ts_close, 20);
        }
        // DRAGGING VARIABLES
        // ======================================================================================================
        private bool formIsDragging = false;
        private Point formDraggingStartPoint = new Point(0, 0);
        // ABOUT LOAD
        // ======================================================================================================
        private void GlowAbout_Load(object sender, EventArgs e){
            try{
                LabelDeveloper.Text = Application.CompanyName;
                LabelSoftware.Text = Application.ProductName;
                LabelVersion.Text = TS_VersionEngine.TS_SofwareVersion(1, Program.ts_version_mode);
                LabelCopyright.Text = TS_SoftwareCopyrightDate.ts_scd_preloader;
                //
                AboutTable.Columns.Add("x", "x");
                AboutTable.Columns.Add("x", "x");
                AboutTable.Columns[0].Width = 110;
                foreach (var available_lang_file in AvailableLanguages){
                    TSSettingsSave software_read_settings = new TSSettingsSave(available_lang_file);
                    string[] get_name = software_read_settings.TSReadSettings("Main", "lang_name").Split('/');
                    string get_lang_translator = software_read_settings.TSReadSettings("Main", "translator");
                    AboutTable.Rows.Add(get_name[0].Trim(), get_lang_translator.Trim());
                }
                AboutTable.AllowUserToResizeColumns = false;
                foreach (DataGridViewColumn A_Column in AboutTable.Columns){ A_Column.SortMode = DataGridViewColumnSortMode.NotSortable; }
                AboutTable.ClearSelection();
                // GET PRELOAD SETTINGS
                About_preloader();
            }catch (Exception){ }
        }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void About_preloader(){
            try{
                // COLOR SETTINGS
                TSSetWindowTheme(Handle, GlowMain.theme);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_BGColor2");
                PanelTxt.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_BGColor2");
                //
                LabelDeveloper.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_LabelColor1");
                LabelSoftware.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_AccentColor");
                LabelVersion.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_LabelColor2");
                LabelCopyright.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_LabelColor2");
                //
                foreach (Control ui_buttons in PanelTxt.Controls){
                    if (ui_buttons is Button about_button){
                        about_button.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                        about_button.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                        about_button.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                        about_button.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                        about_button.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                    }
                }
                //
                TSImageRenderer(About_WebsiteBtn, GlowMain.theme == 1 ? Properties.Resources.ct_website_light : Properties.Resources.ct_website_dark, 18, ContentAlignment.MiddleRight);
                TSImageRenderer(About_GitHubBtn, GlowMain.theme == 1 ? Properties.Resources.ct_github_light : Properties.Resources.ct_github_dark, 18, ContentAlignment.MiddleRight);
                TSImageRenderer(About_BmacBtn, GlowMain.theme == 1 ? Properties.Resources.ct_bmac_mc_light : Properties.Resources.ct_bmac_mc_dark, 18, ContentAlignment.MiddleRight);
                //
                AboutTable.BackgroundColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridBGColor");
                AboutTable.GridColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridColor");
                AboutTable.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridBGColor");
                AboutTable.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridFEColor");
                AboutTable.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridAlternatingColor");
                AboutTable.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                AboutTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                AboutTable.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageFE");
                AboutTable.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                AboutTable.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageFE");
                //
                CloseAboutBtn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBG");
                CloseAboutBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBG");
                CloseAboutBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBGHover");
                CloseAboutBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TSBT_CloseBGHover");
                // ======================================================================================================
                // TEXTS
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                Text = string.Format(software_lang.TSReadLangs("SoftwareAbout", "sa_title"), Application.ProductName);
                About_WebsiteBtn.Text = " " + software_lang.TSReadLangs("SoftwareAbout", "sa_website_page");
                About_GitHubBtn.Text = " " + software_lang.TSReadLangs("SoftwareAbout", "sa_github_page");
                About_BmacBtn.Text = " " + software_lang.TSReadLangs("SoftwareAbout", "sa_bmac_page");
                //
                AboutTable.Columns[0].HeaderText = software_lang.TSReadLangs("SoftwareAbout", "sa_lang_name");
                AboutTable.Columns[1].HeaderText = software_lang.TSReadLangs("SoftwareAbout", "sa_lang_translator");
            }catch (Exception){ }
        }
        // DGV CLEAR SELECTION
        // ======================================================================================================
        private void AboutTable_SelectionChanged(object sender, EventArgs e){ AboutTable.ClearSelection(); }
        // WEBSITE LINK
        // ======================================================================================================
        private void About_WebsiteBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(new ProcessStartInfo(TS_LinkSystem.website_link) { UseShellExecute = true });
            }catch (Exception){ }
        }
        // GITHUB LINK
        // ======================================================================================================
        private void About_GitHubBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(new ProcessStartInfo(TS_LinkSystem.github_link) { UseShellExecute = true });
            }catch (Exception){ }
        }
        // BUY ME A COFFEE LINK
        // ======================================================================================================
        private void About_BmacBtn_Click(object sender, EventArgs e){
            try{
                Process.Start(new ProcessStartInfo(TS_LinkSystem.ts_bmac) { UseShellExecute = true });
            }catch (Exception){ }
        }
        // FORM DRAGGING SYSTEM
        // ======================================================================================================
        private void PanelHeader_MouseDown(object sender, MouseEventArgs e){
            if (e.Button == MouseButtons.Left){
                formIsDragging = true;
                formDraggingStartPoint = new Point(e.X, e.Y);
            }
        }
        private void PanelHeader_MouseMove(object sender, MouseEventArgs e){
            if (formIsDragging){
                Point form_location = PointToScreen(e.Location);
                this.Location = new Point(form_location.X - formDraggingStartPoint.X, form_location.Y - formDraggingStartPoint.Y);
            }
        }
        private void PanelHeader_MouseUp(object sender, MouseEventArgs e){ formIsDragging = false; }
        // CLOSE ABOUT
        // ======================================================================================================
        private void CloseAboutBtn_Click(object sender, EventArgs e){ Close(); }
    }
}