﻿using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowMonitorTestEngine : Form {
        // GLOBAL LANGS PATH
        // ======================================================================================================
        TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
        public GlowMonitorTestEngine() {
            InitializeComponent();
            Resize += GlowMonitorTestEngine_Resize;
        }
        // COLORS
        // ======================================================================================================
        private Color[] global_colors = { Color.Black, Color.White, Color.Red, Color.FromArgb(0, 255, 0), Color.FromArgb(0, 0, 255) };
        private Color[] global_colors_2 = { Color.FromArgb(34, 34, 34), Color.White, Color.Red, Color.FromArgb(0, 255, 0), Color.FromArgb(0, 0, 255) };
        //
        public void monitor_test_engine_theme_settings(){
            try{
                int set_attribute = Glow.theme == 1 ? 20 : 19;
                if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != Glow.theme){
                    DwmSetWindowAttribute(Handle, 20, new[] { Glow.theme == 1 ? 0 : 1 }, 4);
                }
            }catch (Exception){ } 
        }
        // LOAD
        // ======================================================================================================
        private void GlowMonitorTestEngine_Load(object sender, EventArgs e){
            monitor_test_loader(Glow.monitor_engine_mode);
            monitor_test_engine_theme_settings();
            if (Glow.monitor_engine_mode == 0){
                Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("MonitorTestTool", "mtt_title")), Application.ProductName, TS_String_Encoder(software_lang.TSReadLangs("MonitorTestTool", "mtt_title_dead_pixel")));
            }else if (Glow.monitor_engine_mode == 1){
                Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("MonitorTestTool", "mtt_title")), Application.ProductName, TS_String_Encoder(software_lang.TSReadLangs("MonitorTestTool", "mtt_title_dynamic_range")));
            }
            InfoLabel.BackColor = Color.FromArgb(200, 255, 255, 255);
            InfoLabel.ForeColor = Color.FromArgb(34, 34, 34);
        }
        // MONITOR TEST LOADER
        // ======================================================================================================
        int size_mode;
        private async void monitor_test_loader(int monitor_test_mode){
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
            InfoLabel.Dispose();
        }
        // DEAD PIXEL TEST
        // ======================================================================================================
        private int dead_pixel_index = 0;
        private bool dead_pixel_test_status = true;
        private bool pause_toggle = false;
        private void monitor_test_dead_pixel(){
            Thread colorChangingThread = new Thread(() => {
                try{
                    while (dead_pixel_test_status){
                        if (!pause_toggle){
                            UpdateBackgroundColor();
                            dead_pixel_index = (dead_pixel_index + 1) % global_colors.Length;
                            Thread.Sleep(4750);
                        }else{
                            Thread.Sleep(250);
                        }
                    }
                }catch (Exception){ }
            });
            colorChangingThread.IsBackground = true;
            colorChangingThread.Start();
            //
            MouseDown += (sender, e) => {
                if (e.Button == MouseButtons.Left){
                    dead_pixel_index = (dead_pixel_index + 1) % global_colors.Length;
                    UpdateBackgroundColor();
                }
            };
            //
            KeyDown += (sender, e) => {
                if (e.KeyCode == Keys.Space){
                    pause_toggle = !pause_toggle;
                }else if (e.KeyCode == Keys.Left){
                    dead_pixel_index = 0;
                }
            };
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
        // ======================================================================================================
        // DYNAMIC RANGE TEST
        // ======================================================================================================
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
        // DYNAMIC RANGE TEST
        // ======================================================================================================
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
                size_mode = 0;
            }
            if (e.KeyCode == Keys.F12){
                if (size_mode == 1){
                    WindowState = FormWindowState.Normal;
                    FormBorderStyle = FormBorderStyle.Sizable;
                    size_mode = 0;
                }else{
                    FormBorderStyle = FormBorderStyle.None;
                    WindowState = FormWindowState.Maximized;
                    size_mode = 1;
                }
            }
        }
        // EXIT
        // ======================================================================================================
        private void GlowMonitorTestEngine_FormClosing(object sender, FormClosingEventArgs e){
            dead_pixel_test_status = false;
        }
    }
}