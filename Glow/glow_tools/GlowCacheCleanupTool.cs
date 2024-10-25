﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using static Glow.TSModules;

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
            @"C:\Windows\Temp",
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp",
            @"C:\Windows\Prefetch",
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData", "Local", "Microsoft", "Windows", "Explorer"),
            @"C:\Windows\SoftwareDistribution\Download"
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
                CCT_StartBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                CCT_StartBtn.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                CCT_StartBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                CCT_StartBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                //
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_title").Trim())), Application.ProductName);
                //
                CCTTable.Columns[0].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_h_info_feature").Trim()));
                CCTTable.Columns[1].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_h_info_path").Trim()));
                CCTTable.Columns[2].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_h_info_size").Trim()));
                //
                CCT_StartBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_clean").Trim()));
            }catch (Exception){ }
        }
        // CCT LOAD
        // ======================================================================================================
        private void GlowCacheCleanupTool_Load(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            // GET THEME
            cct_theme_settings();
            //
            List<string> cct_folder_name = new List<string>(){
                Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_p_system_temp").Trim())),
                Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_p_temp_user").Trim())),
                Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_p_windows_temp").Trim())),
                Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_p_windows_icon_temp").Trim())),
                Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_p_windows_update_temp").Trim()))
            };
            //
            for (int i = 0; i <= cct_path_list.Count - 1; i++){
                CCTTable.Rows.Add(cct_folder_name[i], cct_path_list[i], Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_refreshing_title").Trim())));
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
            cct_title = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_title").Trim())), Application.ProductName);
            //
            // START FOLDER SIZE CHECK ALGORITHM
            Task check_folder_sizes_task = new Task(check_folder_sizes);
            check_folder_sizes_task.Start();
            // START AUTO FOLDER SIZE ALGORITHM
            Task check_folder_size_auto_task = new Task(AutoFolderSizeRefreshAsync);
            check_folder_size_auto_task.Start();
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
                    CCT_SelectLabel.Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_selected_path").Trim())), get_selected_path.Cells[1].Value.ToString());
                }
            }catch (Exception){ }
        }
        // START CLEAN BTN
        // ======================================================================================================
        private void CCT_StartBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                if (CCTTable.SelectedCells.Count > 0){
                    DialogResult cct_check_delete_notifi = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_check_delete_notification").Trim())), CCTTable.Rows[CCTTable.SelectedCells[0].RowIndex].Cells[1].Value.ToString().Trim(), "\n\n"), cct_title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (cct_check_delete_notifi == DialogResult.Yes){
                        cleanup_engine(CCTTable.Rows[CCTTable.SelectedCells[0].RowIndex].Cells[1].Value.ToString().Trim());
                    }
                }else{
                    MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_check_select_clean_patch_info").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }catch (Exception){ }
        }
        // CLEANUP ENGINE
        // ======================================================================================================
        private void cleanup_engine(string target_path){
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
                Task check_folder_sizes_task = new Task(check_folder_sizes);
                check_folder_sizes_task.Start();
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                CCTTable.ClearSelection();
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("CacheCleanupTool", "cct_delete_success_notification").Trim())), target_path), cct_title, MessageBoxButtons.OK, MessageBoxIcon.Information);
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