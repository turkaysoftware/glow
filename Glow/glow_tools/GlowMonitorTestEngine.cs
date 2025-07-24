using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowMonitorTestEngine : Form {
        public GlowMonitorTestEngine() { InitializeComponent(); CheckForIllegalCrossThreadCalls = false; this.ResizeRedraw = true; }
        // VARIABLES
        // ======================================================================================================
        private Color[] dead_pixel_colors = { Color.Black, Color.White, Color.Red, Color.FromArgb(0, 255, 0), Color.FromArgb(0, 0, 255) };
        private Color[] dynamic_range_colors = { Color.FromArgb(34, 34, 34), Color.FromArgb(85, 85, 85), Color.White, Color.Red, Color.FromArgb(0, 255, 0), Color.FromArgb(0, 0, 255) };
        //
        int size_mode;
        private bool message_disposed = false;
        //
        private int dead_pixel_index = 0;
        private bool dead_pixel_test_status = true;
        private bool dead_pixel_pause_toggle = false;
        private readonly object dead_pixel_lockobj = new object();
        //
        private int dynamic_range_color_count => dynamic_range_colors.Length;
        private const int dynamic_range_shade_count = 15;
        private int dynamic_range_box_count => dynamic_range_shade_count * dynamic_range_color_count;
        private Panel[] dynamic_range_boxs;
        //
        public void monitor_test_engine_theme_settings(){
            try{
                int set_attribute = Glow.theme == 1 ? 20 : 19;
                if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != Glow.theme){
                    DwmSetWindowAttribute(Handle, 20, new[] { Glow.theme == 1 ? 0 : 1 }, 4);
                }
                //
                InfoLabel.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                InfoLabel.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                //
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                if (Glow.monitor_engine_mode == 0){
                    Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("MonitorTestTool", "mtt_title")), Application.ProductName, TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_monitor_test_dead_pixel")));
                }else if (Glow.monitor_engine_mode == 1){
                    Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("MonitorTestTool", "mtt_title")), Application.ProductName, TS_String_Encoder(software_lang.TSReadLangs("HeaderTools", "ht_monitor_test_dynamic_range")));
                }
            }catch (Exception){ } 
        }
        // LOAD
        // ======================================================================================================
        private void GlowMonitorTestEngine_Load(object sender, EventArgs e){
            monitor_test_engine_theme_settings();
            monitor_test_loader(Glow.monitor_engine_mode);
        }
        // MONITOR TEST LOADER
        // ======================================================================================================
        private async void monitor_test_loader(int monitor_test_mode){
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            if (monitor_test_mode == 0){
                InfoLabel.Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("MonitorTestTool", "mtt_esc_info_dead_pixel")), "\n");
                monitor_test_dead_pixel();
            }else if (monitor_test_mode == 1){
                InfoLabel.Text = TS_String_Encoder(software_lang.TSReadLangs("MonitorTestTool", "mtt_esc_info_dynamic_range"));
                monitor_dynamic_range_test();
            }
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
            size_mode = 1;
            //
            await Task.Run(() => message_dispose());
        }
        private void InfoLabel_Click(object sender, EventArgs e){
            InfoLabel.Dispose();
        }
        private async void message_dispose(){
            await Task.Delay(7000);
            DisposeLabel();
        }
        private void DisposeLabel(){
            if (!message_disposed && InfoLabel != null && !InfoLabel.IsDisposed){
                message_disposed = true;
                InfoLabel.Dispose();
                InfoLabel = null;
            }
        }
        // DEAD PIXEL TEST
        // ======================================================================================================
        private void monitor_test_dead_pixel(){
            Thread colorChangingThread = new Thread(() => {
                try{
                    while (dead_pixel_test_status){
                        if (!dead_pixel_pause_toggle){
                            this.Invoke((MethodInvoker)(() => {
                                UpdateBackgroundColor();
                            }));
                            lock (dead_pixel_lockobj){
                                dead_pixel_index = (dead_pixel_index + 1) % dead_pixel_colors.Length;
                            }
                            Thread.Sleep(4750);
                        }else{
                            Thread.Sleep(500);
                        }
                    }
                }catch (Exception){ }
            });
            colorChangingThread.IsBackground = true;
            colorChangingThread.Start();
            //
            MouseDown -= MouseDown_Handler;
            MouseDown += MouseDown_Handler;
            KeyDown -= KeyDown_Handler;
            KeyDown += KeyDown_Handler;
        }
        private void MouseDown_Handler(object sender, MouseEventArgs e){
            if (e.Button == MouseButtons.Left){
                lock (dead_pixel_lockobj){
                    dead_pixel_index = (dead_pixel_index + 1) % dead_pixel_colors.Length;
                }
                this.Invoke((MethodInvoker)(() => {
                    UpdateBackgroundColor();
                }));
            }
        }
        private void KeyDown_Handler(object sender, KeyEventArgs e){
            if (e.KeyCode == Keys.Space){
                dead_pixel_pause_toggle = !dead_pixel_pause_toggle;
            }else if (e.KeyCode == Keys.Left){
                lock (dead_pixel_lockobj){
                    dead_pixel_index = 0;
                }
            }
        }
        private void UpdateBackgroundColor(){
            try{
                if (InvokeRequired){
                    Invoke(new MethodInvoker(delegate { UpdateBackgroundColor(); }));
                }else{
                    BackColor = dead_pixel_colors[dead_pixel_index];
                }
            }catch (Exception){ }
        }
        // DYNAMIC RANGE TEST
        // ======================================================================================================
        private void monitor_dynamic_range_test(){
            try{
                dynamic_range_boxs = new Panel[dynamic_range_box_count];
                for (int colorIndex = 0; colorIndex < dynamic_range_color_count; colorIndex++){
                    for (int shadeIndex = 0; shadeIndex < dynamic_range_shade_count; shadeIndex++){
                        int index = colorIndex * dynamic_range_shade_count + shadeIndex;
                        dynamic_range_boxs[index] = new Panel{
                            BackColor = ShadeGenerator(dynamic_range_colors[colorIndex], (double)shadeIndex / (dynamic_range_shade_count - 1)),
                            BorderStyle = BorderStyle.None
                        };
                        Controls.Add(dynamic_range_boxs[index]);
                    }
                }
                monitor_dynamic_range_box_resize();
            }catch (Exception){ }
        }
        private void monitor_dynamic_range_box_resize(){
            try{
                this.SuspendLayout();
                int totalWidth = ClientSize.Width;
                int totalHeight = ClientSize.Height;
                int boxWidth = (int)Math.Ceiling((double)totalWidth / dynamic_range_color_count);
                int boxHeight = (int)Math.Ceiling((double)totalHeight / dynamic_range_shade_count);
                for (int colorIndex = 0; colorIndex < dynamic_range_color_count; colorIndex++){
                    for (int shadeIndex = 0; shadeIndex < dynamic_range_shade_count; shadeIndex++){
                        int index = colorIndex * dynamic_range_shade_count + shadeIndex;
                        int x = colorIndex * boxWidth;
                        int y = shadeIndex * boxHeight;
                        dynamic_range_boxs[index].Size = new Size(boxWidth, boxHeight);
                        dynamic_range_boxs[index].Location = new Point(x, y);
                    }
                }
                this.ResumeLayout();
            }catch (Exception){ }
        }
        private Color ShadeGenerator(Color mainColor, double shadeRatio){
            double gamma = 2.2;
            double factor = Math.Pow(shadeRatio, gamma);
            int red = (int)(mainColor.R * factor);
            int green = (int)(mainColor.G * factor);
            int blue = (int)(mainColor.B * factor);
            return Color.FromArgb(Clamp(red), Clamp(green), Clamp(blue));
        }
        private int Clamp(int value) => Math.Max(0, Math.Min(255, value));
        // RESIZE
        // ======================================================================================================
        private void GlowMonitorTestEngine_Resize(object sender, EventArgs e){
            if (Glow.monitor_engine_mode == 1){
                try{
                    monitor_dynamic_range_box_resize();
                }catch (Exception){ }
            }
        }
        // FULLSCREEN TO NORMAL
        // ======================================================================================================
        private void GlowMonitorTestEngine_KeyDown(object sender, KeyEventArgs e){
            if (e.KeyCode == Keys.F12 || e.KeyCode == Keys.Escape){
                if (size_mode == 1){
                    WindowState = FormWindowState.Normal;
                    FormBorderStyle = FormBorderStyle.Sizable;
                    size_mode = 0;
                }else{
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    size_mode = 1;
                }
            }else{ return; }
        }
        // EXIT
        // ======================================================================================================
        private void GlowMonitorTestEngine_FormClosing(object sender, FormClosingEventArgs e){
            dead_pixel_test_status = false;
        }
    }
}