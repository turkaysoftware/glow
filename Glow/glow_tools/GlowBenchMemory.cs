using System;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;
//
using static Glow.TSModules;
using System.Drawing;

namespace Glow.glow_tools{
    public partial class GlowBenchMemory : Form{
        public GlowBenchMemory(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        //
        private long TargetMemoryUsage;
        private const int AllocationSize = 100 * 1024 * 1024; // 100 MB
        private const int SleepTime = 150; // 0.15 seconds
        //
        private long _totalAllocated = 0;
        private readonly List<byte[]> _allocations = new List<byte[]>();
        private CancellationTokenSource _cancellationTokenSource;
        //
        bool ram_mode_loop_status = true;
        bool ram_bench_stopped = false;
        bool loop_mode;
        // LOAD
        // ======================================================================================================
        public void Bench_ram_settings(){
            try{
                int set_attribute = Glow.theme == 1 ? 20 : 19;
                if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != Glow.theme){
                    DwmSetWindowAttribute(Handle, 20, new[] { Glow.theme == 1 ? 0 : 1 }, 4);
                }
                BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_Panel.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                //
                Bench_MStart.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                Bench_MStart.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                Bench_MStart.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                Bench_MStart.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                Bench_MStart.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMainHover");
                Bench_MStop.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                Bench_MStop.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                Bench_MStop.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                Bench_MStop.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMain");
                Bench_MStop.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "AccentMainHover");
                //
                Bench_TLP.BackgroundColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridBGColor");
                Bench_TLP.GridColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridColor");
                Bench_TLP.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridBGColor");
                Bench_TLP.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridFEColor");
                Bench_TLP.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridAlternatingColor");
                Bench_TLP.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageBG");
                Bench_TLP.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageBG");
                Bench_TLP.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageFE");
                Bench_TLP.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridBGColor");
                Bench_TLP.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridFEColor");
                Bench_TLP.ReadOnly = true;
                //
                TSImageRenderer(Bench_MStart, Glow.theme == 1 ? Properties.Resources.ct_test_start_light : Properties.Resources.ct_test_start_dark, 22, ContentAlignment.MiddleRight);
                TSImageRenderer(Bench_MStop, Glow.theme == 1 ? Properties.Resources.ct_test_stop_light : Properties.Resources.ct_test_stop_dark, 22, ContentAlignment.MiddleRight);
                //
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                Text = string.Format(software_lang.TSReadLangs("BenchRAM", "br_title"), Application.ProductName);
                //
                Bench_TLP.Rows[0].Cells[0].Value = software_lang.TSReadLangs("BenchRAM", "br_ram_total");
                Bench_TLP.Rows[1].Cells[0].Value = software_lang.TSReadLangs("BenchRAM", "br_ram_used");
                Bench_TLP.Rows[2].Cells[0].Value = software_lang.TSReadLangs("BenchRAM", "br_ram_empty");
                Bench_TLP.Rows[3].Cells[0].Value = software_lang.TSReadLangs("BenchRAM", "br_ram_allocated");
                //
                Bench_MStart.Text = " " + software_lang.TSReadLangs("BenchRAM", "br_start");
                Bench_MStop.Text = " " + software_lang.TSReadLangs("BenchRAM", "br_stop");
            }catch (Exception){ }
        }
        private void Bench_TLP_CellClick(object sender, DataGridViewCellEventArgs e){
            Bench_TLP.ClearSelection();
        }
        // MAIN LOAD
        // ======================================================================================================
        private void GlowBenchMemory_Load(object sender, EventArgs e){
            Bench_TLP.Columns.Add("x", "x");
            Bench_TLP.Columns.Add("x", "x");
            //
            Bench_TLP.Rows.Add("x", "x");
            Bench_TLP.Rows.Add("x", "x");
            Bench_TLP.Rows.Add("x", "x");
            Bench_TLP.Rows.Add("x", "x");
            //
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            Bench_TLP.Rows[3].Cells[1].Value = software_lang.TSReadLangs("BenchRAM", "br_await_start");
            //
            Bench_ram_settings();
            //
            Bench_TLP.ColumnHeadersVisible = false;
            Bench_TLP.AllowUserToResizeColumns = false;
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, Bench_TLP, new object[] { true });
            //
            Task dynamic_ram_info = new Task(Dynamic_ram_status);
            dynamic_ram_info.Start();
        }
        // DYNAMIC RAM STATUS
        // ======================================================================================================
        private async void Dynamic_ram_status(){
            try{
                ComputerInfo get_ram_info = new ComputerInfo();
                do{
                    ulong total_ram = ulong.Parse(get_ram_info.TotalPhysicalMemory.ToString());
                    ulong usable_ram = ulong.Parse(get_ram_info.AvailablePhysicalMemory.ToString());
                    double usage_ram_percentage = (TS_FormatSizeNoType(total_ram) - TS_FormatSizeNoType(usable_ram)) / TS_FormatSizeNoType(total_ram) * 100;
                    //
                    Bench_TLP.Rows[0].Cells[1].Value = TS_FormatSize(total_ram);
                    Bench_TLP.Rows[1].Cells[1].Value = string.Format("{0} - {1}", TS_FormatSize(total_ram - usable_ram), string.Format("{0:0.00}%", usage_ram_percentage));
                    Bench_TLP.Rows[2].Cells[1].Value = TS_FormatSize(usable_ram);
                    Bench_TLP.ClearSelection();
                    //
                    TargetMemoryUsage = (long)(Math.Round(TS_FormatSizeNoType(total_ram - usable_ram)) - 1) * 1024 * 1024 * 1024;
                    await Task.Delay(1000 - DateTime.Now.Millisecond);
                } while (ram_mode_loop_status);
            }catch (Exception){ }
        }
        // TIMER
        // ======================================================================================================
        private async void BenchTimer(){
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            string titleFormat = software_lang.TSReadLangs("BenchRAM", "br_title");
            string elapsedTimeFormat = software_lang.TSReadLangs("BenchRAM", "br_elapsed_time");
            try{
                while (loop_mode){
                    //
                    TimeSpan elapsed = stopwatch.Elapsed;
                    int fh_second = (int)elapsed.TotalSeconds % 60;
                    int fh_minute = (int)(elapsed.TotalMinutes % 60);
                    int fh_hour = (int)(elapsed.TotalHours);
                    //
                    Text = string.Format(titleFormat, Application.ProductName) + " | " + elapsedTimeFormat + " " + string.Format("{0:D2}:{1:D2}:{2:D2}", fh_hour, fh_minute, fh_second);
                    //
                    await Task.Delay(1000);
                }
            }catch (Exception){ }
        }
        // START ENGINE
        // ======================================================================================================
        private async void Bench_MStart_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                DialogResult warning_test = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(software_lang.TSReadLangs("BenchRAM", "br_warning"), "\n\n", "\n"));
                if (warning_test == DialogResult.Yes){
                    _cancellationTokenSource = new CancellationTokenSource();
                    Bench_MStart.Enabled = false;
                    Bench_MStop.Enabled = true;
                    loop_mode = true;
                    //
                    Task timer_start = new Task(BenchTimer);
                    timer_start.Start();
                    //
                    await Task.Run(() => RAMBenchmarkEngine(_cancellationTokenSource.Token));
                }
            }catch (Exception){ }
        }
        // STOP ENGINE
        // ======================================================================================================
        private void Bench_MStop_Click(object sender, EventArgs e){
            try{
                _cancellationTokenSource.Cancel();
                ram_bench_stopped = true;
                Bench_MStart.Enabled = true;
                Bench_MStop.Enabled = false;
                Gc_run();
            }catch (Exception){ }
        }
        // GC RUN
        // ======================================================================================================
        private void Gc_run(){
            _allocations.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        // RAM BENCHMARK ENGINE
        // ======================================================================================================
        private async void RAMBenchmarkEngine(CancellationToken cancellationToken){
            try{
                while (_totalAllocated < TargetMemoryUsage && !cancellationToken.IsCancellationRequested){
                    byte[] allocation = new byte[AllocationSize];
                    for (int i = 0; i < AllocationSize; i += 4096){
                        allocation[i] = 1;
                    }
                    _allocations.Add(allocation);
                    _totalAllocated += AllocationSize;
                    long totalMemoryDuring = GC.GetTotalMemory(false); // Stop GC Launch Event
                    Invoke(new Action(() =>{
                        Bench_TLP.Rows[3].Cells[1].Value = TS_FormatSize(totalMemoryDuring);
                    }));
                    await Task.Delay(SleepTime);
                }
            }catch (OutOfMemoryException){
                Invoke(new Action(() =>{
                  //  Bench_TLP.Rows[3].Cells[1].Value = "Out of memory exception caught!";
                }));
            }
            finally{
                Invoke(new Action(() =>{
                    Bench_MStart.Enabled = true;
                    Bench_MStop.Enabled = false;
                    Gc_run();
                    loop_mode = false;
                    TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                    Text = string.Format(software_lang.TSReadLangs("BenchRAM", "br_title"), Application.ProductName);
                    Bench_TLP.Rows[3].Cells[1].Value = software_lang.TSReadLangs("BenchRAM", "br_await_start");
                    if (!ram_bench_stopped){
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BenchRAM", "br_end"));
                    }else{
                        ram_bench_stopped = false;
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BenchRAM", "br_stopped"));
                    }
                }));
            }
        }
        // EXIT
        // ======================================================================================================
        private void GlowBenchMemory_FormClosing(object sender, FormClosingEventArgs e){
            Gc_run();
            ram_mode_loop_status = false;
            Hide();
        }
    }
}