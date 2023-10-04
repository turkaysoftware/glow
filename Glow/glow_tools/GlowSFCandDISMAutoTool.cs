using System;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using static Glow.GlowModules;

namespace Glow.glow_tools{
    public partial class GlowSFCandDISMAutoTool : Form{
        public GlowSFCandDISMAutoTool(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // ======================================================================================================
        // GLOBAL LANGS PATH
        GlowGetLangs g_lang = new GlowGetLangs(Glow.lang_path);
        // ======================================================================================================
        // GLOBAL STRINGS
        string sadt_title, sadt_start, sadt_starter, sadt_rotate, sadt_ender, sadt_last_text;
        // ======================================================================================================
        // REPAIR CODES
        string sadt_code_1 = "sfc /scannow";
        string sadt_code_2 = "DISM /Online /Cleanup-Image /CheckHealth";
        string sadt_code_3 = "DISM /Online /Cleanup-Image /ScanHealth";
        string sadt_code_4 = "DISM /Online /Cleanup-Image /RestoreHealth";
        private void GlowSFCandDISMAutoTool_Load(object sender, EventArgs e){
            if (Glow.theme == 1){
                try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
            }else if (Glow.theme == 2){
                try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[] { 1 }, 4); } }catch (Exception){ }
            }
            BackColor = Glow.ui_colors[5];
            sadt_panel.BackColor = Glow.ui_colors[6];
            SADT_L1.ForeColor = Glow.ui_colors[8];
            SADT_L2.ForeColor = Glow.ui_colors[7];
            SADT_StartBtn.BackColor = Glow.ui_colors[8];
            SADT_StartBtn.ForeColor = Glow.ui_colors[19];
            // SET TEXT
            sadt_title = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SFCandDISMTool", "sadt_1").Trim())), Application.ProductName);
            sadt_start = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SFCandDISMTool", "sadt_6").Trim()));
            sadt_starter = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SFCandDISMTool", "sadt_7").Trim()));
            sadt_rotate = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SFCandDISMTool", "sadt_8").Trim()));
            sadt_ender = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SFCandDISMTool", "sadt_9").Trim()));
            sadt_last_text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SFCandDISMTool", "sadt_10").Trim()));
            // SET UI TEXT
            Text = sadt_title;
            SADT_L1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SFCandDISMTool", "sadt_2").Trim()));
            SADT_L2.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SFCandDISMTool", "sadt_3").Trim()));
            SADT_StartBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SFCandDISMTool", "sadt_4").Trim()));
        }
        // SFC AND DISM AUTO TOOL START ENGINE BTN
        // ======================================================================================================
        private void SADT_StartBtn_Click(object sender, EventArgs e){
            try{
                DialogResult sadt_start_check = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SFCandDISMTool", "sadt_5").Trim())), "\n"), sadt_title, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (sadt_start_check == DialogResult.Yes){
                    Task sadt_engine_bg = new Task(sadt_engine);
                    sadt_engine_bg.Start();
                }
            }catch (Exception){ }
        }
        // SFC AND DISM AUTO TOOL ENGINE
        // ======================================================================================================
        private void sadt_engine(){
            try{
                Process.Start("cmd.exe", "/k " + $"title {sadt_title} & color 3 & echo {sadt_start} &" +
                $" echo {new string('-', 75)} & echo ({sadt_code_1}) {sadt_starter} & {sadt_code_1} &" +
                $" echo {new string('-', 75)} & echo ({sadt_code_1}) {sadt_rotate} & echo {new string('-', 75)} &" +
                $" {sadt_code_2} & echo {new string('-', 75)} & echo ({sadt_code_2}) {sadt_rotate} & echo {new string('-', 75)} &" +
                $" {sadt_code_3} & echo {new string('-', 75)} & echo ({sadt_code_3}) {sadt_rotate} & echo {new string('-', 75)} &" +
                $" {sadt_code_4} & echo {new string('-', 75)} & echo ({sadt_code_4}) {sadt_ender} & echo {new string('-', 75)} &" +
                $" echo {new string('-', 75)} & echo {new string('-', 75)} & echo {sadt_last_text} & echo {new string('-', 75)} &" +
                $" echo {new string('-', 75)} & echo {new string('-', 75)}");
            }catch (Exception){ }
        }
    }
}