using System;
using System.IO;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
//
using static Glow.TSModules;
using System.Drawing;

namespace Glow.glow_tools{
    public partial class GlowCacheCleanupTool : Form{
        public GlowCacheCleanupTool(){
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //
            CCTTable.Columns.Add("x", "x");
            CCTTable.Columns.Add("x", "x");
            CCTTable.Columns.Add("x", "x");
        }
        // CLEAN SYSTEM
        // ======================================================================================================
        List<string> cct_path_list = new List<string>(){
            Path.Combine(Glow.windows_disk, "Windows", "Temp"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp"),
            Path.Combine(Glow.windows_disk, "Windows", "Prefetch"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "Microsoft", "Windows", "Explorer"),
            Path.Combine(Glow.windows_disk, "Windows", "SoftwareDistribution", "Download")
        };
        //
        List<long> cct_path_size_list = new List<long>();
        //
        string cct_title;
        bool cct_auto_refresh_mode = true;
        bool cct_auto_refresh_repat = false;
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void cct_theme_settings(){
            try{
                int set_attribute = Glow.theme == 1 ? 20 : 19;
                if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != Glow.theme){
                    DwmSetWindowAttribute(Handle, 20, new[] { Glow.theme == 1 ? 0 : 1 }, 4);
                }
                //
                BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                //
                BG_Panel.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                //
                CCTTable.BackgroundColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridBGColor");
                CCTTable.GridColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridColor");
                CCTTable.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridBGColor");
                CCTTable.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridFEColor");
                CCTTable.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridAlternatingColor");
                CCTTable.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageBG");
                CCTTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageBG");
                CCTTable.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageFE");
                CCTTable.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageBG");
                CCTTable.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageFE");
                //
                CCT_SelectLabel.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                CCT_SelectLabel.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                //
                CCT_StartBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                CCT_StartBtn.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                CCT_StartBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                CCT_StartBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                CCT_StartBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMainHover");
                //
                TSImageRenderer(CCT_StartBtn, Glow.theme == 1 ? Properties.Resources.ct_clean_light : Properties.Resources.ct_clean_dark, 22, ContentAlignment.MiddleRight);
                //
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_title")), Application.ProductName);
                //
                CCTTable.Columns[0].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_h_info_feature"));
                CCTTable.Columns[1].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_h_info_path"));
                CCTTable.Columns[2].HeaderText = TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_h_info_size"));
                //
                CCT_StartBtn.Text = " " + TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_clean"));
            }catch (Exception){ }
        }
        // CCT LOAD
        // ======================================================================================================
        private async void GlowCacheCleanupTool_Load(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            // GET THEME
            cct_theme_settings();
            //
            List<string> cct_folder_name = new List<string>(){
                TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_p_system_temp")),
                TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_p_temp_user")),
                TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_p_windows_temp")),
                TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_p_windows_icon_temp")),
                TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_p_windows_update_temp"))
            };
            //
            for (int i = 0; i <= cct_path_list.Count - 1; i++){
                CCTTable.Rows.Add(cct_folder_name[i], cct_path_list[i], TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_refreshing_title")));
            }
            //
            CCTTable.Columns[0].Width = 175;
            CCTTable.Columns[2].Width = 125;
            //
            foreach (DataGridViewColumn CCT_Column in CCTTable.Columns){
                CCT_Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            //
            CCTTable.ClearSelection();
            //
            cct_title = string.Format(TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_title")), Application.ProductName);
            //
            // START FOLDER SIZE CHECK ALGORITHM & START AUTO FOLDER SIZE ALGORITHM
            await Task.Run(() => check_folder_sizes());
            await Task.Run(() => AutoFolderSizeRefreshAsync());
        }
        // CHECK FOLDER SIZE ALGORITHM
        // ======================================================================================================
        private void check_folder_sizes(){
            try{
                cct_path_size_list.Clear();
                foreach (var get_file_path in cct_path_list){
                    long path_size = 0;
                    IEnumerable<string> path_file_size = Directory.EnumerateFiles(get_file_path, "*.*", SearchOption.AllDirectories);
                    foreach (var get_current_file in path_file_size){
                        try{
                            if (File.Exists(get_current_file)){
                                path_size += new FileInfo(get_current_file).Length;
                            }
                        }catch (Exception){
                           // Console.WriteLine($"Error accessing file {file}: {ex.Message}");
                        }
                    }
                    cct_path_size_list.Add(path_size);
                }
                for (int i = 0; i < cct_path_size_list.Count && i < CCTTable.Rows.Count; i++){
                    CCTTable.Rows[i].Cells[2].Value = TS_FormatSize(cct_path_size_list[i]);
                }
            }catch (Exception){ }
            finally{
                cct_path_size_list.Clear();
            }
        }
        // SELECT LABEL WRITE PATH
        // ======================================================================================================
        private void CCTTable_CellClick(object sender, DataGridViewCellEventArgs e){
            try{
                if (e.RowIndex >= 0){
                    TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                    DataGridViewRow get_selected_path = CCTTable.Rows[e.RowIndex];
                    CCT_SelectLabel.Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_selected_path")), get_selected_path.Cells[1].Value.ToString());
                }
            }catch (Exception){ }
        }
        // START CLEAN BTN
        // ======================================================================================================
        private async void CCT_StartBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                if (CCTTable.SelectedCells.Count > 0){
                    DialogResult cct_check_delete_notifi = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_check_delete_notification")), CCTTable.Rows[CCTTable.SelectedCells[0].RowIndex].Cells[1].Value.ToString().Trim(), "\n\n"));
                    if (cct_check_delete_notifi == DialogResult.Yes){
                        await Task.Run(() => cleanup_engine(CCTTable.Rows[CCTTable.SelectedCells[0].RowIndex].Cells[1].Value.ToString().Trim()));
                    }
                }else{
                    TS_MessageBoxEngine.TS_MessageBox(this, 2, TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_check_select_clean_patch_info")));
                }
            }catch (Exception){ }
        }
        // CLEANUP ENGINE
        // ======================================================================================================
        private async void cleanup_engine(string target_path){
            try{
                DirectoryInfo target_folder = new DirectoryInfo(target_path);
                foreach (FileInfo target_files in target_folder.GetFiles()){
                    try{
                        target_files.Delete();
                    }catch (Exception){ }
                }
                foreach (DirectoryInfo target_folders in target_folder.GetDirectories()){
                    try{
                        target_folders.Delete(true);
                    }catch (Exception){ }
                }
                cct_auto_refresh_repat = true;
                await Task.Run(() => check_folder_sizes());
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                CCTTable.ClearSelection();
                TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(TS_String_Encoder(software_lang.TSReadLangs("CacheCleanupTool", "cct_delete_success_notification")), target_path));
            }catch (Exception){ }
        }
        // AUTO REFRESH FOLDER SIZE FUNCTION
        // ======================================================================================================
        private async void AutoFolderSizeRefreshAsync(){
            try{
                int RefreshIntervalInSeconds = 15;
                //
                var softwareLang = new TSGetLangs(Glow.lang_path);
                var refreshTitleFormat = softwareLang.TSReadLangs("CacheCleanupTool", "cct_refresh_title").Trim();
                var refreshingTitle = softwareLang.TSReadLangs("CacheCleanupTool", "cct_refreshing_title").Trim();
                //
                while (cct_auto_refresh_mode){
                    for (int i = RefreshIntervalInSeconds; i >= 0; i--){
                        if (cct_auto_refresh_repat){
                            cct_auto_refresh_repat = false;
                            break;
                        }
                        //
                        Text = i != 0 ? $"{cct_title} | {string.Format(refreshTitleFormat, i)}" : $"{cct_title} | {refreshingTitle}";
                        //
                        if (i == 0){
                            await CheckFolderSizesAsync();
                        }
                        //
                        await Task.Delay(1000);
                    }
                }
            }catch (Exception){ }
        }
        private async Task CheckFolderSizesAsync(){
            await Task.Run(() => check_folder_sizes());
        }
        // BEFORE CCT TOOL EXIT AUTO REFRESH STOP
        // ======================================================================================================
        private void GlowCacheCleanupTool_FormClosing(object sender, FormClosingEventArgs e){ cct_auto_refresh_mode = false; }
    }
}