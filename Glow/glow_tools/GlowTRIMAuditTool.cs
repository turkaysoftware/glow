using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using static Glow.GlowModules;

namespace Glow.glow_tools{
    public partial class GlowTRIMAuditTool : Form{
        public GlowTRIMAuditTool(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // ======================================================================================================
        // GLOBAL LANGS PATH
        GlowGetLangs g_lang = new GlowGetLangs(Glow.lang_path);
        // ======================================================================================================
        // TRIM SYSTEM
        List<string> trim_check_list = new List<string>();
        List<string> trim_enabled_list = new List<string>();
        string trim_check_name = "trim_check.txt";
        string trim_enabled_name = "trim_enabled.txt";
        bool trim_check_loop = true;
        bool trim_enabled_loop = true;
        private void GlowTRIMAuditTool_Load(object sender, EventArgs e){
            // COLOR SETTINGS
            try{
                if (Glow.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                }else if (Glow.theme == 2){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[] { 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                }
                BackColor = Glow.ui_colors[5];
                TAT_P1.BackColor = Glow.ui_colors[6];
                TAT_P2.BackColor = Glow.ui_colors[6];
                TAT_P3.BackColor = Glow.ui_colors[6];
                TAT_P4.BackColor = Glow.ui_colors[6];
                TAT_L1.ForeColor = Glow.ui_colors[7];
                TAT_L2.ForeColor = Glow.ui_colors[8];
                TAT_L3.ForeColor = Glow.ui_colors[7];
                TAT_L4.ForeColor = Glow.ui_colors[8];
                TAT_CheckBtn.BackColor = Glow.ui_colors[8];
                TAT_CheckBtn.ForeColor = Glow.ui_colors[19];
                TAT_EnabledBtn.BackColor = Glow.ui_colors[8];
                TAT_EnabledBtn.ForeColor = Glow.ui_colors[19];
                // TEXTS
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_1").Trim())), Application.ProductName);
                TAT_L1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_2").Trim()));
                TAT_L3.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_3").Trim()));
                TAT_L2.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_4").Trim()));
                TAT_L4.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_4").Trim()));
                TAT_CheckBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_5").Trim()));
                TAT_EnabledBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_6").Trim()));
                // DISABLED ACTIVE LAYERS
                TAT_P3.Enabled = false;
                TAT_P4.Enabled = false;
            }catch (Exception){ }
        }
        // ======================================================================================================
        // TRIM CHECK BTN
        private void TAT_CheckBtn_Click(object sender, EventArgs e){
            try{
                TAT_CheckBtn.Enabled = false;
                Process.Start("cmd.exe", "/k " + $"fsutil behavior query DisableDeleteNotify > {trim_check_name} & exit");
                Task check_trim_bg = new Task(check_trim);
                check_trim_bg.Start();
            }catch (Exception){ }
        }
        // ======================================================================================================
        // TRIM CHECK
        private void check_trim(){
            try{
                do{
                    if (File.Exists(trim_check_name)){
                        Thread.Sleep(175);
                        using (StreamReader trim_reader = new StreamReader(trim_check_name)){
                            string trim_line;
                            while ((trim_line = trim_reader.ReadLine()) != null){
                                trim_check_list.Add(trim_line);
                            }
                            trim_reader.Close();
                            File.Delete(trim_check_name);
                            trim_check_loop = false;
                            if (trim_check_list[0].Contains("0")){
                                TAT_L2.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_7").Trim()));
                                TAT_L4.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_13").Trim()));
                            }else if (trim_check_list[0].Contains("1")){
                                TAT_L2.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_8").Trim()));
                                // ENABLED
                                TAT_P3.Enabled = true;
                                TAT_P4.Enabled = true;
                            }else{
                                TAT_L2.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_9").Trim()));
                            }
                            trim_check_list.Clear();
                            TAT_CheckBtn.Enabled = true;
                            break;
                        }
                    }
                } while (trim_check_loop);
            }catch (Exception){ }
        }
        // ======================================================================================================
        // TRIM ENABLED BTN
        private void TAT_EnabledBtn_Click(object sender, EventArgs e){
            try{
                TAT_EnabledBtn.Enabled = false;
                Process.Start("cmd.exe", "/k " + $"fsutil behavior set DisableDeleteNotify 0 > {trim_enabled_name} & exit");
                Task enabled_trim_bg = new Task(enabled_trim);
                enabled_trim_bg.Start();
            }catch (Exception){ }
        }
        // ======================================================================================================
        // TRIM ENABLED
        private void enabled_trim(){
            try{
                do{
                    if (File.Exists(trim_enabled_name)){
                        Thread.Sleep(175);
                        using (StreamReader trim_reader = new StreamReader(trim_enabled_name)){
                            string trim_line;
                            while ((trim_line = trim_reader.ReadLine()) != null){
                                trim_enabled_list.Add(trim_line);
                            }
                            trim_reader.Close();
                            File.Delete(trim_enabled_name);
                            trim_enabled_loop = false;
                            if (trim_enabled_list[0].Contains("0")){
                                TAT_L4.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_10").Trim()));
                            }else if (trim_enabled_list[0].Contains("1")){
                                TAT_L4.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_11").Trim()));
                            }else{
                                TAT_L4.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("TRIMAuditTool", "tat_12").Trim()));
                            }
                            trim_enabled_list.Clear();
                            TAT_EnabledBtn.Enabled = false;
                            break;
                        }
                    }
                } while (trim_enabled_loop);
            }catch (Exception){ }
        }
    }
}