using System;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using static Glow.GlowModules;

namespace Glow.glow_tools{
    public partial class GlowSystemSoftwareTool : Form{
        public GlowSystemSoftwareTool(){
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            typeof(FlowLayoutPanel).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, SST_FLP, new object[] { true });
        }
        // ======================================================================================================
        // GLOBAL LANGS PATH
        GlowGetLangs g_lang = new GlowGetLangs(Glow.lang_path);
        // ======================================================================================================
        // SYSTEM SOFTWARE LINKS
        Glow_SST_UI glow_ssi_ui = new Glow_SST_UI();
        class Glow_SST_UI{
            // LINKS
            public string link_amd               = @"https://www.amd.com/en/support";
            public string link_autoruns          = @"https://learn.microsoft.com/en-us/sysinternals/downloads/autoruns";
            public string link_ddu               = @"https://www.guru3d.com/download/display-driver-uninstaller-download/";
            public string link_directx           = @"https://www.microsoft.com/en-us/download/details.aspx?id=35";
            public string link_intel             = @"https://www.intel.com/content/www/us/en/search.html#sort=relevancy&f:@tabfilter=[Downloads]&f:@stm_10385_en=[Graphics]";
            public string link_java              = @"https://www.java.com/en/download/manual.jsp";
            public string link_java_jdk          = @"https://www.oracle.com/java/technologies/downloads/";
            public string link_visual_redist     = @"https://www.techpowerup.com/download/visual-c-redistributable-runtime-package-all-in-one/";
            public string link_nvidia            = @"https://www.nvidia.com/en-us/geforce/drivers/";
            public string link_vimera            = @"https://github.com/roines45/vimera/releases/latest";
            // IMAGES
            public Image image_amd = Properties.Resources.sst_amd;
            public Image image_autoruns = Properties.Resources.sst_autoruns;
            public Image image_ddu = Properties.Resources.sst_ddu;
            public Image image_directx = Properties.Resources.sst_directx;
            public Image image_intel = Properties.Resources.sst_intel;
            public Image image_java = Properties.Resources.sst_java;
            public Image image_java_jdk = Properties.Resources.sst_java_jdk;
            public Image image_visual_redist = Properties.Resources.sst_vs;
            public Image image_nvidia = Properties.Resources.sst_nvidia;
            public Image image_vimera = Properties.Resources.sst_vimera;
        }
        // ======================================================================================================
        // SYSTEM SOFTARE TOOL LOAD
        private void GlowSystemSoftwareTool_Load(object sender, EventArgs e){
            // COLOR SETTINGS
            try{
                // GET THEME
                sst_theme_settings();
                // IMAGES
                image_amd.BackgroundImage = glow_ssi_ui.image_amd;
                image_autoruns.BackgroundImage = glow_ssi_ui.image_autoruns;
                image_ddu.BackgroundImage = glow_ssi_ui.image_ddu;
                image_directx.BackgroundImage = glow_ssi_ui.image_directx;
                image_intel.BackgroundImage = glow_ssi_ui.image_intel;
                image_java.BackgroundImage = glow_ssi_ui.image_java;
                image_java_jdk.BackgroundImage = glow_ssi_ui.image_java_jdk;
                image_visual_redist.BackgroundImage = glow_ssi_ui.image_visual_redist;
                image_nvidia.BackgroundImage = glow_ssi_ui.image_nvidia;
                image_vimera.BackgroundImage = glow_ssi_ui.image_vimera;
                // TEXTS
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_title").Trim())), Application.ProductName);
                // LABEL TEXTS
                label_amd_1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_amd").Trim()));
                label_autoruns_1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_autoruns").Trim()));
                label_ddu_1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_ddu").Trim()));
                label_directx_1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_directx").Trim()));
                label_intel_1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_intel").Trim()));
                label_java_1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_java").Trim()));
                label_java_jdk_1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_java_jdk").Trim()));
                label_visual_redist_1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_visual_redist").Trim()));
                label_nvidia_1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_nvidia").Trim()));
                label_vimera_1.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_vimera").Trim()));
                // BUTTON TEXTS
                Btn_AMD.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_link_btn").Trim()));
                Btn_AUTORUNS.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_link_btn").Trim()));
                Btn_DDU.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_link_btn").Trim()));
                Btn_DIRECTX.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_link_btn").Trim()));
                Btn_INTEL.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_link_btn").Trim()));
                Btn_JAVA.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_link_btn").Trim()));
                Btn_JAVA_JDK.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_link_btn").Trim()));
                Btn_VISUAL_REDIST.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_link_btn").Trim()));
                Btn_NVIDIA.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_link_btn").Trim()));
                Btn_VIMERA.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.GlowReadLangs("SystemSoftwareTool", "sst_link_btn").Trim()));
            }catch (Exception) { }
        }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void sst_theme_settings(){
            try{
                if (Glow.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                }else if (Glow.theme == 0 || Glow.theme == 2){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                }
                // BACK COLOR
                BackColor = Glow.ui_colors[5];
                // PANEL BACK COLORS
                panel_amd.BackColor = Glow.ui_colors[6];
                panel_autoruns.BackColor = Glow.ui_colors[6];
                panel_ddu.BackColor = Glow.ui_colors[6];
                panel_directx.BackColor = Glow.ui_colors[6];
                panel_intel.BackColor = Glow.ui_colors[6];
                panel_java.BackColor = Glow.ui_colors[6];
                panel_java_jdk.BackColor = Glow.ui_colors[6];
                panel_visual_redist.BackColor = Glow.ui_colors[6];
                panel_nvidia.BackColor = Glow.ui_colors[6];
                panel_vimera.BackColor = Glow.ui_colors[6];
                // LABEL COLORS
                label_amd_1.ForeColor = Glow.ui_colors[7];
                label_amd_2.ForeColor = Glow.ui_colors[8];
                label_autoruns_1.ForeColor = Glow.ui_colors[7];
                label_autoruns_2.ForeColor = Glow.ui_colors[8];
                label_ddu_1.ForeColor = Glow.ui_colors[7];
                label_ddu_2.ForeColor = Glow.ui_colors[8];
                label_directx_1.ForeColor = Glow.ui_colors[7];
                label_directx_2.ForeColor = Glow.ui_colors[8];
                label_intel_1.ForeColor = Glow.ui_colors[7];
                label_intel_2.ForeColor = Glow.ui_colors[8];
                label_java_1.ForeColor = Glow.ui_colors[7];
                label_java_2.ForeColor = Glow.ui_colors[8];
                label_java_jdk_1.ForeColor = Glow.ui_colors[7];
                label_java_jdk_2.ForeColor = Glow.ui_colors[8];
                label_visual_redist_1.ForeColor = Glow.ui_colors[7];
                label_visual_redist_2.ForeColor = Glow.ui_colors[8];
                label_nvidia_1.ForeColor = Glow.ui_colors[7];
                label_nvidia_2.ForeColor = Glow.ui_colors[8];
                label_vimera_1.ForeColor = Glow.ui_colors[7];
                label_vimera_2.ForeColor = Glow.ui_colors[8];
                // BUTTONS COLORS
                Btn_AMD.BackColor = Glow.ui_colors[8];
                Btn_AMD.ForeColor = Glow.ui_colors[19];
                Btn_AUTORUNS.BackColor = Glow.ui_colors[8];
                Btn_AUTORUNS.ForeColor = Glow.ui_colors[19];
                Btn_DDU.BackColor = Glow.ui_colors[8];
                Btn_DDU.ForeColor = Glow.ui_colors[19];
                Btn_DIRECTX.BackColor = Glow.ui_colors[8];
                Btn_DIRECTX.ForeColor = Glow.ui_colors[19];
                Btn_INTEL.BackColor = Glow.ui_colors[8];
                Btn_INTEL.ForeColor = Glow.ui_colors[19];
                Btn_JAVA.BackColor = Glow.ui_colors[8];
                Btn_JAVA.ForeColor = Glow.ui_colors[19];
                Btn_JAVA_JDK.BackColor = Glow.ui_colors[8];
                Btn_JAVA_JDK.ForeColor = Glow.ui_colors[19];
                Btn_VISUAL_REDIST.BackColor = Glow.ui_colors[8];
                Btn_VISUAL_REDIST.ForeColor = Glow.ui_colors[19];
                Btn_NVIDIA.BackColor = Glow.ui_colors[8];
                Btn_NVIDIA.ForeColor = Glow.ui_colors[19];
                Btn_VIMERA.BackColor = Glow.ui_colors[8];
                Btn_VIMERA.ForeColor = Glow.ui_colors[19];
            }catch (Exception){ }
        }
        // ======================================================================================================
        // BUTTONS CLICK EVENTS
        private void Btn_AMD_Click(object sender, EventArgs e){
            try{
                Process.Start(glow_ssi_ui.link_amd);
            }catch (Exception){ }
        }
        private void Btn_AUTORUNS_Click(object sender, EventArgs e){
            try{
                Process.Start(glow_ssi_ui.link_autoruns);
            }catch (Exception) { }
        }
        private void Btn_DDU_Click(object sender, EventArgs e){
            try{
                Process.Start(glow_ssi_ui.link_ddu);
            }catch (Exception){ }
        }
        private void Btn_DIRECTX_Click(object sender, EventArgs e){
            try{
                Process.Start(glow_ssi_ui.link_directx);
            }catch (Exception){ }
        }
        private void Btn_INTEL_Click(object sender, EventArgs e){
            try{
                Process.Start(glow_ssi_ui.link_intel);
            }catch (Exception){ }
        }
        private void Btn_JAVA_Click(object sender, EventArgs e){
            try{
                Process.Start(glow_ssi_ui.link_java);
            }catch (Exception){ }
        }
        private void Btn_JAVA_JDK_Click(object sender, EventArgs e){
            try{
                Process.Start(glow_ssi_ui.link_java_jdk);
            }catch (Exception){ }
        }
        private void Btn_VISUAL_REDIST_Click(object sender, EventArgs e){
            try{
                Process.Start(glow_ssi_ui.link_visual_redist);
            }catch (Exception){ }
        }
        private void Btn_NVIDIA_Click(object sender, EventArgs e){
            try{
                Process.Start(glow_ssi_ui.link_nvidia);
            }catch (Exception){ }
        }
        private void Btn_VIMERA_Click(object sender, EventArgs e){
            try{
                Process.Start(glow_ssi_ui.link_vimera);
            }catch (Exception){ }
        }
    }
}