using System;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using static Glow.GlowModules;

namespace Glow.glow_tools{
    public partial class GlowMonitorTestEngine : Form {

        // ======================================================================================================
        // GLOBAL LANGS PATH
        TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
        public GlowMonitorTestEngine() {
            InitializeComponent();
            Resize += GlowMonitorTestEngine_Resize;
        }
        // COLORS
        private Color[] global_colors = { Color.Black, Color.White, Color.Red, Color.FromArgb(0, 255, 0), Color.FromArgb(0, 0, 255) }; // Kullanılacak renkler
        private Color[] global_colors_2 = { Color.FromArgb(34, 34, 34), Color.White, Color.Red, Color.FromArgb(0, 255, 0), Color.FromArgb(0, 0, 255) }; // Kullanılacak renkler
        //
        public void monitor_test_engine_theme_settings(){
            try{
                if (Glow.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                }else if (Glow.theme == 0 || Glow.theme == 2){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                }
                BackColor = Glow.ui_colors[5];
                if (Glow.monitor_engine_mode == 0){
                    Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("MonitorTestTool", "mtt_title").Trim())), Application.ProductName, Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("MonitorTestTool", "mtt_title_dead_pixel").Trim())));
                }else if (Glow.monitor_engine_mode == 1){
                    Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("MonitorTestTool", "mtt_title").Trim())), Application.ProductName, Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("MonitorTestTool", "mtt_title_dynamic_range").Trim())));
                }
            }catch (Exception){ } 
        }
        // LOAD
        private void GlowMonitorTestEngine_Load(object sender, EventArgs e){
            monitor_test_loader(Glow.monitor_engine_mode);
            monitor_test_engine_theme_settings();
        }
        // MONITOR TEST LOADER
        private void monitor_test_loader(int monitor_test_mode){
            if (monitor_test_mode == 0){
                monitor_test_dead_pixel();
            }else if (monitor_test_mode == 1){
                monitor_dynamic_range_test();
            }
            WindowState = FormWindowState.Maximized;
            FormBorderStyle = FormBorderStyle.None;
        }
        // DEAD PIXEL TEST
        private int dead_pixel_index = 0;
        private bool dead_pixel_test_status = true;
        private void monitor_test_dead_pixel(){
            Thread colorChangingThread = new Thread(() =>{
                try{
                    while (dead_pixel_test_status){
                        UpdateBackgroundColor();
                        Thread.Sleep(5000);
                        dead_pixel_index = (dead_pixel_index + 1) % global_colors.Length;
                    }
                }catch (Exception){ }
            });
            colorChangingThread.IsBackground = true;
            colorChangingThread.Start();
        }
        private void UpdateBackgroundColor(){
            try{
                if (InvokeRequired){
                    Invoke(new MethodInvoker(delegate { UpdateBackgroundColor(); }));
                }else{
                    BackColor = global_colors[dead_pixel_index];
                }
            }catch (Exception){ }
        }
        // DEAD PIXEL TEST
        // DYNAMIC RANGE TEST
        private const int box_count = 15 * 5; // 15 Shades
        private const int color_count = 5; // Total 5 Color
        private Panel[] boxs;
        private void monitor_dynamic_range_test(){
            try{
                boxs = new Panel[box_count];
                for (int colorIndex = 0; colorIndex < color_count; colorIndex++){
                    for (int ShadeIndex = 0; ShadeIndex < 15; ShadeIndex++){
                        int index = colorIndex * 15 + ShadeIndex;
                        boxs[index] = new Panel();
                        boxs[index].BackColor = ShadeGenerator(global_colors_2[colorIndex], (double)ShadeIndex / 15);
                        Controls.Add(boxs[index]);
                    }
                }
                monitor_dynamic_range_box_resize();
            }catch (Exception){ }
        }
        private void monitor_dynamic_range_box_resize(){
            try{
                int boxWidth = ClientSize.Width / color_count;
                int boxHeight = ClientSize.Height / 15; // Shade count
                for (int colorIndex = 0; colorIndex < color_count; colorIndex++){
                    for (int ShadeIndex = 0; ShadeIndex < 15; ShadeIndex++){
                        int index = colorIndex * 15 + ShadeIndex;
                        int columnIndex = colorIndex;
                        int rowIndex = ShadeIndex;
                        int x = columnIndex * boxWidth;
                        int y = rowIndex * boxHeight;
                        boxs[index].Size = new Size(boxWidth, boxHeight);
                        boxs[index].Location = new Point(x, y);
                    }
                }
            }catch (Exception){ }
        }
        private Color ShadeGenerator(Color mainColor, double shadeRatio){
            int red = (int)(mainColor.R * (1 - shadeRatio));
            int green = (int)(mainColor.G * (1 - shadeRatio));
            int blue = (int)(mainColor.B * (1 - shadeRatio));
            return Color.FromArgb(red, green, blue);
        }
        // RESIZE
        private void GlowMonitorTestEngine_Resize(object sender, EventArgs e){
            if (Glow.monitor_engine_mode == 1){
                try{
                    monitor_dynamic_range_box_resize();
                }catch (Exception){ }
            }
        }
        // FULLSCREEN TO NORMAL
        private void GlowMonitorTestEngine_KeyDown(object sender, KeyEventArgs e){
            if (e.KeyCode == Keys.Escape){
                WindowState = FormWindowState.Normal;
                FormBorderStyle = FormBorderStyle.Sizable;
            }
        }
        // EXIT
        private void GlowMonitorTestEngine_FormClosing(object sender, FormClosingEventArgs e){
            dead_pixel_test_status = false;
        }
    }
}