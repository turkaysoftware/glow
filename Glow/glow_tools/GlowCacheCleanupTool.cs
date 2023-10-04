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
        GlowGetLangs g_lang = new GlowGetLangs(Glow.lang_path);
        // ======================================================================================================
        // GLOBAL STRINGS AND LISTS
        static string clean_path_1 = @"C:\Windows\Temp";
        static string clean_path_2 = @"C:\Users\" + SystemInformation.UserName + @"\AppData\Local\Temp";
        static string clean_path_3 = @"C:\Windows\Prefetch";
        List<string> clean_path_list = new List<string>() { clean_path_1, clean_path_2, clean_path_3 };
        List<double> clean_path_size_list = new List<double>();
        string cct_title;
        bool cct_auto_refresh_mode = true;
        private void GlowCacheCleanupTool_Load(object sender, EventArgs e){
            // COLOR SETTINGS
            if (Glow.theme == 1){
                try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
            }else if (Glow.theme == 2){
                try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
            }
            BackColor = Glow.ui_colors[5];
            CCT_Panel_1.BackColor = Glow.ui_colors[6];
            CCT_Panel_2.BackColor = Glow.ui_colors[6];
            CCT_Panel_3.BackColor = Glow.ui_colors[6];
            CCT_Panel_4.BackColor = Glow.ui_colors[6];
            CCT_Panel_5.BackColor = Glow.ui_colors[6];
            CCT_Panel_6.BackColor = Glow.ui_colors[6];
            CCT_L1.ForeColor = Glow.ui_colors[7];
            CCT_L2.ForeColor = Glow.ui_colors[8];
            CCT_L3.ForeColor = Glow.ui_colors[7];
            CCT_L4.ForeColor = Glow.ui_colors[8];
            CCT_L5.ForeColor = Glow.ui_colors[7];
            CCT_L6.ForeColor = Glow.ui_colors[8];
            CCT_CleanS_TempBtn.BackColor = Glow.ui_colors[8];
            CCT_CleanS_TempBtn.ForeColor = Glow.ui_colors[19];
            CCT_CleanU_TempBtn.BackColor = Glow.ui_colors[8];
            CCT_CleanU_TempBtn.ForeColor = Glow.ui_colors[19];
            CCT_CleanS_PrefetchBtn.BackColor = Glow.ui_colors[8];
            CCT_CleanS_PrefetchBtn.ForeColor = Glow.ui_colors[19];
            // TEXT SET
            cct_title = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("CacheCleanupTool", "cct_1").Trim())), Application.ProductName);
            Text = cct_title;
            CCT_CleanS_TempBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("CacheCleanupTool", "cct_3").Trim()));
            CCT_CleanU_TempBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("CacheCleanupTool", "cct_3").Trim()));
            CCT_CleanS_PrefetchBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("CacheCleanupTool", "cct_3").Trim()));
            // LABEL PATH TEXT SET
            CCT_L1.Text = clean_path_list[0];
            CCT_L3.Text = clean_path_list[1];
            CCT_L5.Text = clean_path_list[2];
            // START FOLDER SIZE CHECK ALGORITHM
            Task check_folder_sizes_task = new Task(check_folder_sizes);
            check_folder_sizes_task.Start();
            // START AUTO FOLDER SIZE ALGORITHM
            Task check_folder_size_auto_task = new Task(auto_folder_size_refresh);
            check_folder_size_auto_task.Start();
        }
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
                        CCT_L2.Text = convert_size(clean_path_size_list[i]);
                    }else if (i == 1){
                        CCT_L4.Text = convert_size(clean_path_size_list[i]);
                    }else if (i == 2){
                        CCT_L6.Text = convert_size(clean_path_size_list[i]);
                    }
                }
                clean_path_size_list.Clear();
            }catch (Exception){ }
        }
        // FOLDER SIZE CONVERT ALGORITHM
        // ======================================================================================================
        private string convert_size(double get_size){
            if (get_size > 1024){
                if ((get_size / 1024) > 1024){
                    if ((get_size / 1024 / 1024) > 1024){
                        if ((get_size / 1024 / 1024 / 1024) > 1024){
                            // TB
                           return string.Format("{0:0.00} TB", get_size / 1024 / 1024 / 1024 / 1024);
                        }else{
                            // GB
                            return string.Format("{0:0.00} GB", get_size / 1024 / 1024 / 1024);
                        }
                    }else{
                        // MB
                        return string.Format("{0:0.00} MB", get_size / 1024 / 1024);
                    }
                }else{
                    // KB
                    return string.Format("{0:0.0} KB", get_size / 1024);
                }
            }else{
                // Byte
                return string.Format("{0:0.0} " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("CacheCleanupTool", "cct_2").Trim())), get_size);
            }
        }
        // CLEANUP SYSTEM TEMP FOLDER
        // ======================================================================================================
        private void CCT_CleanS_TempBtn_Click(object sender, EventArgs e){
            try{
                DialogResult cct_check_delete_notifi = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("CacheCleanupTool", "cct_4").Trim())), clean_path_list[0], "\n\n"), cct_title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (cct_check_delete_notifi == DialogResult.Yes){
                    cleanup_engine(clean_path_list[0]);
                }
            }catch (Exception){ }
        }
        // CLEANUP USER TEMP FOLDER
        // ======================================================================================================
        private void CCT_CleanU_TempBtn_Click(object sender, EventArgs e){
            try{
                DialogResult cct_check_delete_notifi = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("CacheCleanupTool", "cct_4").Trim())), clean_path_list[1], "\n\n"), cct_title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (cct_check_delete_notifi == DialogResult.Yes){
                    cleanup_engine(clean_path_list[1]);
                }
            }catch (Exception){ }
        }
        // CLEANUP SYSTEM PREFETCH FOLDER
        // ======================================================================================================
        private void CCT_CleanS_PrefetchBtn_Click(object sender, EventArgs e){
            try{
                DialogResult cct_check_delete_notifi = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("CacheCleanupTool", "cct_4").Trim())), clean_path_list[2], "\n\n"), cct_title, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (cct_check_delete_notifi == DialogResult.Yes){
                    cleanup_engine(clean_path_list[2]);
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
                Task check_folder_sizes_task = new Task(check_folder_sizes);
                check_folder_sizes_task.Start();
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("CacheCleanupTool", "cct_5").Trim())), target_path), cct_title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }catch (Exception){ }
        }
        // AUTO REFRESH FOLDER SIZE FUNCTION
        // ======================================================================================================
        private void auto_folder_size_refresh(){
            try{
                do{
                    Thread.Sleep(10000); // 10 Sec
                    Task check_folder_sizes_task = new Task(check_folder_sizes);
                    check_folder_sizes_task.Start();
                } while (cct_auto_refresh_mode == true);
            }catch (Exception){ }
        }
        // BEFORE CCT TOOL EXIT AUTO REFRESH STOP
        private void GlowCacheCleanupTool_FormClosing(object sender, FormClosingEventArgs e){ cct_auto_refresh_mode = false; }
    }
}