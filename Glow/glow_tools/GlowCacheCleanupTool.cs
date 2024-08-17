using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using static Glow.GlowModules;

namespace Glow.glow_tools{
    public partial class GlowCacheCleanupTool : Form{
        public GlowCacheCleanupTool(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // ======================================================================================================
        // GLOBAL LANGS PATH
        // ======================================================================================================
        // GLOBAL STRINGS AND LISTS
        static string clean_path_1 = @"C:\Windows\Temp";
        static string clean_path_2 = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Temp";
        List<string> clean_path_list = new List<string>(){ clean_path_1, clean_path_2 };
        List<long> clean_path_size_list = new List<long>();
        // PATH VALUES
        List<string> path_names = new List<string>();
        // ==========
        string cct_title;
        bool cct_auto_refresh_mode = true;
        bool cct_auto_refresh_repat = false;
        private void GlowCacheCleanupTool_Load(object sender, EventArgs e){
            TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
            // GET THEME
            cct_theme_settings();
            // TEXT SET
            cct_title = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_title").Trim())), Application.ProductName);
            // LABEL PATH TEXT SET
            path_names.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_system_temp").Trim())));
            path_names.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_user_temp").Trim())));
            // START FOLDER SIZE CHECK ALGORITHM
            Task check_folder_sizes_task = new Task(check_folder_sizes);
            check_folder_sizes_task.Start();
            // START AUTO FOLDER SIZE ALGORITHM
            Task check_folder_size_auto_task = new Task(auto_folder_size_refresh);
            check_folder_size_auto_task.Start();
        }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void cct_theme_settings(){
            try{
                if (Glow.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                }else if (Glow.theme == 0){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                }
                BackColor = Glow.ui_colors[5];
                CCT_Panel_1.BackColor = Glow.ui_colors[6];
                CCT_Panel_2.BackColor = Glow.ui_colors[6];
                CCT_Panel_3.BackColor = Glow.ui_colors[6];
                CCT_Panel_4.BackColor = Glow.ui_colors[6];
                CCT_L1.ForeColor = Glow.ui_colors[7];
                CCT_L2.ForeColor = Glow.ui_colors[8];
                CCT_L3.ForeColor = Glow.ui_colors[7];
                CCT_L4.ForeColor = Glow.ui_colors[8];
                CCT_CleanS_TempBtn.BackColor = Glow.ui_colors[8];
                CCT_CleanS_TempBtn.ForeColor = Glow.ui_colors[18];
                CCT_CleanS_TempBtn.FlatAppearance.BorderColor = Glow.ui_colors[8];
                CCT_CleanS_TempBtn.FlatAppearance.MouseDownBackColor = Glow.ui_colors[8];
                CCT_CleanU_TempBtn.BackColor = Glow.ui_colors[8];
                CCT_CleanU_TempBtn.ForeColor = Glow.ui_colors[18];
                CCT_CleanU_TempBtn.FlatAppearance.BorderColor = Glow.ui_colors[8];
                CCT_CleanU_TempBtn.FlatAppearance.MouseDownBackColor = Glow.ui_colors[8];
                //
                TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_title").Trim())), Application.ProductName);
                //
                CCT_L1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_system_temp").Trim()));
                CCT_L3.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_user_temp").Trim()));
                // BTN TEXT
                CCT_CleanS_TempBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_clean").Trim()));
                CCT_CleanU_TempBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_clean").Trim()));
            }catch (Exception){ }
        }
        // ======================================================================================================
        // HOVER NAMES
        private void CCT_L1_MouseHover(object sender, EventArgs e){ CCT_L1.Text = clean_path_list[0]; }
        private void CCT_L1_MouseLeave(object sender, EventArgs e){ CCT_L1.Text = path_names[0]; }
        // L1
        private void CCT_L3_MouseHover(object sender, EventArgs e){ CCT_L3.Text = clean_path_list[1]; }
        private void CCT_L3_MouseLeave(object sender, EventArgs e){ CCT_L3.Text = path_names[1]; }
        // L1
        // CHECK FOLDER SIZE ALGORITHM
        // ======================================================================================================
        private void check_folder_sizes(){
            try{
                long path_sizes = 0;
                for (int i = 0; i <= clean_path_list.Count - 1; i++){
                    string[] path_file_size = Directory.GetFiles(clean_path_list[i], "*.*", SearchOption.AllDirectories); // Byte
                    foreach (string path_files in path_file_size){
                        path_sizes += new FileInfo(path_files).Length;
                    }
                    clean_path_size_list.Add(path_sizes);
                    path_sizes = 0;
                }
                for (int i = 0; i <= clean_path_size_list.Count - 1; i++){
                    if (i == 0){
                        CCT_L2.Text = TS_FormatSize(clean_path_size_list[i]);
                    }else if (i == 1){
                        CCT_L4.Text = TS_FormatSize(clean_path_size_list[i]);
                    }
                }
                clean_path_size_list.Clear();
            }catch (Exception){ }
        }
        // CLEANUP SYSTEM TEMP FOLDER
        // ======================================================================================================
        private void CCT_CleanS_TempBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
                DialogResult cct_check_delete_notifi = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_check_delete_notification").Trim())), Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_system_temp").Trim())), clean_path_list[0], "\n\n"), cct_title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (cct_check_delete_notifi == DialogResult.Yes){
                    cleanup_engine(clean_path_list[0]);
                }
            }catch (Exception){ }
        }
        // CLEANUP USER TEMP FOLDER
        // ======================================================================================================
        private void CCT_CleanU_TempBtn_Click(object sender, EventArgs e){
            try{
                TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
                DialogResult cct_check_delete_notifi = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_check_delete_notification").Trim())), Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_user_temp").Trim())), clean_path_list[1], "\n\n"), cct_title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (cct_check_delete_notifi == DialogResult.Yes){
                    cleanup_engine(clean_path_list[1]);
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
                TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_delete_success_notification").Trim())), target_path), cct_title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }catch (Exception){ }
        }
        // AUTO REFRESH FOLDER SIZE FUNCTION
        // ======================================================================================================
        private void auto_folder_size_refresh(){
            try{
                do{
                    TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
                    int refresh_time = 10;
                    for (int i = refresh_time; i >= 0; i--){
                        if (cct_auto_refresh_repat == true){
                            cct_auto_refresh_repat = false;
                            break;
                        }
                        if (i != 0){
                            Text = string.Format("{0} | {1}", cct_title, string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_refresh_title").Trim())), i));
                        }else{
                            Text = string.Format("{0} | {1}", cct_title, Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("CacheCleanupTool", "cct_refreshing_title").Trim())));
                            Task check_folder_sizes_task = new Task(check_folder_sizes);
                            check_folder_sizes_task.Start();
                        }
                        Thread.Sleep(1000);
                    }
                } while (cct_auto_refresh_mode == true);
            }catch (Exception){ }
        }
        // BEFORE CCT TOOL EXIT AUTO REFRESH STOP
        private void GlowCacheCleanupTool_FormClosing(object sender, FormClosingEventArgs e){ cct_auto_refresh_mode = false; }
    }
}