using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowCacheCleanupTool : Form{
        public GlowCacheCleanupTool(){
            InitializeComponent();
            //
            CCTTable.RowTemplate.Height = 32;
            CCTTable.Columns.Add("CleanupName", "Name");
            CCTTable.Columns.Add("CleanupPath", "Path");
            CCTTable.Columns.Add("CleanupSize", "Size");
        }
        // CLEAN SYSTEM
        // ======================================================================================================
        readonly List<string> cct_path_list = new List<string>(){
            Path.Combine(GlowMain.windows_disk, "Windows", "Temp"),
            Environment.ExpandEnvironmentVariables("%TEMP%"),
            Path.Combine(GlowMain.windows_disk, "Windows", "Prefetch"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Windows", "Explorer"),
            Path.Combine(GlowMain.windows_disk, "Windows", "SoftwareDistribution", "Download"),
        };
        private readonly List<long> CCleanup_pathSizes = new List<long>();
        private string CCleanup_cctTitle;
        private bool CCleanup_aRefresh = true, CCleanup_aRefreshRepeat = false;
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void Cct_theme_settings(){
            try{
                TSThemeModeHelper.InitializeThemeForForm(this);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                //
                BG_Panel.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                //
                CCTTable.BackgroundColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridBGColor");
                CCTTable.GridColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridColor");
                CCTTable.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridBGColor");
                CCTTable.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridFEColor");
                CCTTable.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridAlternatingColor");
                CCTTable.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                CCTTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                CCTTable.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageFE");
                CCTTable.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                CCTTable.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageFE");
                //
                CCT_SelectLabel.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                CCT_SelectLabel.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                //
                CCT_StartBtn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                CCT_StartBtn.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                CCT_StartBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                CCT_StartBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                CCT_StartBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                //
                TSImageRenderer(CCT_StartBtn, GlowMain.theme == 1 ? Properties.Resources.ct_clean_light : Properties.Resources.ct_clean_dark, 22, ContentAlignment.MiddleRight);
                //
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                Text = string.Format(software_lang.TSReadLangs("CacheCleanupTool", "cct_title"), Application.ProductName);
                //
                CCTTable.Columns[0].HeaderText = software_lang.TSReadLangs("CacheCleanupTool", "cct_h_info_feature");
                CCTTable.Columns[1].HeaderText = software_lang.TSReadLangs("CacheCleanupTool", "cct_h_info_path");
                CCTTable.Columns[2].HeaderText = software_lang.TSReadLangs("CacheCleanupTool", "cct_h_info_size");
                //
                CCT_StartBtn.Text = " " + software_lang.TSReadLangs("CacheCleanupTool", "cct_clean");
            }catch (Exception){ }
        }
        // CCT LOAD
        // ======================================================================================================
        private async void GlowCacheCleanupTool_Load(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            // GET THEME
            Cct_theme_settings();
            List<string> cct_folder_name = new List<string>(){
                software_lang.TSReadLangs("CacheCleanupTool", "cct_p_system_temp"),
                software_lang.TSReadLangs("CacheCleanupTool", "cct_p_temp_user"),
                software_lang.TSReadLangs("CacheCleanupTool", "cct_p_windows_temp"),
                software_lang.TSReadLangs("CacheCleanupTool", "cct_p_windows_icon_temp"),
                software_lang.TSReadLangs("CacheCleanupTool", "cct_p_windows_update_temp")
            };
            for (int i = 0; i <= cct_path_list.Count - 1; i++){
                CCTTable.Rows.Add(cct_folder_name[i], cct_path_list[i], software_lang.TSReadLangs("CacheCleanupTool", "cct_refreshing_title"));
            }
            CCTTable.Columns[0].Width = (int)(175 * this.DeviceDpi / 96f);
            CCTTable.Columns[2].Width = (int)(125 * this.DeviceDpi / 96f);
            foreach (DataGridViewColumn CCT_Column in CCTTable.Columns){
                CCT_Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn columnPadding in CCTTable.Columns){
                int scaledPadding = (int)(3 * this.DeviceDpi / 96f);
                columnPadding.DefaultCellStyle.Padding = new Padding(scaledPadding, 0, 0, 0);
            }
            CCTTable.ClearSelection();
            CCleanup_cctTitle = string.Format(software_lang.TSReadLangs("CacheCleanupTool", "cct_title"), Application.ProductName);
            // START FOLDER SIZE CHECK ALGORITHM & START AUTO FOLDER SIZE ALGORITHM
            await Task.Run(() => Check_folder_sizes());
            await Task.Run(() => AutoFolderSizeRefreshAsync());
        }
        // CHECK FOLDER SIZE ALGORITHM
        // ======================================================================================================
        private void Check_folder_sizes(){
            try{
                CCleanup_pathSizes.Clear();
                foreach (var path in cct_path_list){
                    long path_size = 0;
                    try{
                        string explorerDir = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Windows", "Explorer");
                        string iconCacheDb = Path.Combine( Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IconCache.db");
                        if (string.Equals(path, explorerDir, StringComparison.OrdinalIgnoreCase)){
                            if (Directory.Exists(explorerDir)){
                                foreach (var f in Directory.EnumerateFiles(explorerDir, "iconcache*", SearchOption.TopDirectoryOnly)){
                                    try { path_size += new FileInfo(f).Length; } catch { }
                                }
                            }
                            if (File.Exists(iconCacheDb)){
                                try { path_size += new FileInfo(iconCacheDb).Length; } catch { }
                            }
                        }else if (Directory.Exists(path)){
                            foreach (var f in Directory.EnumerateFiles(path, "*.*", SearchOption.AllDirectories)){
                                try { path_size += new FileInfo(f).Length; } catch { }
                            }
                        }else if (File.Exists(path)){
                            path_size = new FileInfo(path).Length;
                        }
                    }catch(Exception) { }
                    CCleanup_pathSizes.Add(path_size);
                }
                for (int i = 0; i < CCleanup_pathSizes.Count && i < CCTTable.Rows.Count; i++){
                    int index = i;
                    long size = CCleanup_pathSizes[i];
                    if (CCTTable.InvokeRequired){
                        CCTTable.Invoke(new Action(() =>
                            CCTTable.Rows[index].Cells[2].Value = TS_FormatSize(size)
                        ));
                    }else{
                        CCTTable.Rows[index].Cells[2].Value = TS_FormatSize(size);
                    }
                }
            }catch(Exception){ }
            finally { CCleanup_pathSizes.Clear(); }
        }
        // SELECT LABEL WRITE PATH
        // ======================================================================================================
        private void CCTTable_CellClick(object sender, DataGridViewCellEventArgs e){
            try{
                if (e.RowIndex >= 0){
                    TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                    DataGridViewRow get_selected_path = CCTTable.Rows[e.RowIndex];
                    string pathText = string.Format(software_lang.TSReadLangs("CacheCleanupTool", "cct_selected_path"), get_selected_path.Cells[1].Value.ToString());
                    if (CCT_SelectLabel.InvokeRequired){
                        CCT_SelectLabel.Invoke(new Action(() => CCT_SelectLabel.Text = pathText));
                    }else{
                        CCT_SelectLabel.Text = pathText;
                    }
                }
            }catch (Exception){ }
        }
        // START CLEAN BTN
        // ======================================================================================================
        private async void CCT_StartBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                if (CCTTable.SelectedCells.Count > 0){
                    DialogResult cct_check_delete_notifi = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(software_lang.TSReadLangs("CacheCleanupTool", "cct_check_delete_notification"), CCTTable.Rows[CCTTable.SelectedCells[0].RowIndex].Cells[1].Value.ToString().Trim(), "\n\n"));
                    if (cct_check_delete_notifi == DialogResult.Yes){
                        await Task.Run(() => Cleanup_engine(CCTTable.Rows[CCTTable.SelectedCells[0].RowIndex].Cells[1].Value.ToString().Trim()));
                    }
                }else{
                    TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("CacheCleanupTool", "cct_check_select_clean_patch_info"));
                }
            }catch (Exception){ }
        }
        // CLEANUP ENGINE
        // ======================================================================================================
        private async void Cleanup_engine(string target_path){
            try{
                string explorerDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Microsoft", "Windows", "Explorer");
                string iconCacheDb = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "IconCache.db");
                if (string.Equals(target_path, explorerDir, StringComparison.OrdinalIgnoreCase)){
                    if (Directory.Exists(explorerDir)){
                        foreach (var f in Directory.EnumerateFiles(explorerDir, "iconcache*", SearchOption.TopDirectoryOnly)){
                            try { File.Delete(f); } catch { }
                        }
                    }
                    if (File.Exists(iconCacheDb)){
                        try { File.Delete(iconCacheDb); } catch { }
                    }
                }else if (File.Exists(target_path)){
                    try { File.Delete(target_path); } catch { }
                }else if (Directory.Exists(target_path)){
                    DirectoryInfo di = new DirectoryInfo(target_path);
                    foreach (var f in di.GetFiles()) { try { f.Delete(); } catch { } }
                    foreach (var d in di.GetDirectories()) { try { d.Delete(true); } catch { } }
                }
                CCleanup_aRefreshRepeat = true;
                await Task.Run(() => Check_folder_sizes());
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                CCTTable.ClearSelection();
                TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("CacheCleanupTool", "cct_delete_success_notification"), target_path));
            }catch (Exception){ }
        }
        // AUTO REFRESH FOLDER SIZE FUNCTION
        // ======================================================================================================
        private async void AutoFolderSizeRefreshAsync(){
            try{
                int RefreshIntervalInSeconds = 15;
                var softwareLang = new TSGetLangs(GlowMain.lang_path);
                var refreshTitleFormat = softwareLang.TSReadLangs("CacheCleanupTool", "cct_refresh_title").Trim();
                var refreshingTitle = softwareLang.TSReadLangs("CacheCleanupTool", "cct_refreshing_title").Trim();
                while (CCleanup_aRefresh){
                    for (int i = RefreshIntervalInSeconds; i >= 0; i--){
                        if (CCleanup_aRefreshRepeat){
                            CCleanup_aRefreshRepeat = false;
                            break;
                        }
                        Text = i != 0 ? $"{CCleanup_cctTitle} | {string.Format(refreshTitleFormat, i)}" : $"{CCleanup_cctTitle} | {refreshingTitle}";
                        if (i == 0){
                            await CheckFolderSizesAsync();
                        }
                        await Task.Delay(1000);
                    }
                }
            }catch (Exception){ }
        }
        private async Task CheckFolderSizesAsync(){
            await Task.Run(() => Check_folder_sizes());
        }
        // BEFORE CCT TOOL EXIT AUTO REFRESH STOP
        // ======================================================================================================
        private void GlowCacheCleanupTool_FormClosing(object sender, FormClosingEventArgs e) { CCleanup_aRefresh = false; }
    }
}