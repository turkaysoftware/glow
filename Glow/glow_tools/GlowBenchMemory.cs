using System;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowBenchMemory : Form{
        // VARIABLES
        // ======================================================================================================
        private long RAMBench_TargetMemoryUsage, RAMBench_totalAllocated = 0;
        private const int RAMBench_AllocationSize = 100 * 1024 * 1024, RAMBench_SleepTime = 150; // 1 = 100 MB | 2 = 0.15 seconds
        private readonly List<byte[]> RAMBench_allocations = new List<byte[]>();
        private CancellationTokenSource RAMBench_cancellationTokenSource;
        private bool RAMBench_dynamicMemoryUsage = true, RAMBench_stopMode = false;
        public GlowBenchMemory(){
            InitializeComponent();
            //
            Bench_TLP.Columns.Add("RAMPref", "Pref");
            Bench_TLP.Columns.Add("RAMValue", "Value");
            //
            Bench_TLP.RowTemplate.Height = (int)(36 * this.DeviceDpi / 96f);
            Bench_TLP.Rows.Add("RAMSpec1", "RAMVal1");
            Bench_TLP.Rows.Add("RAMSpec2", "RAMVal2");
            Bench_TLP.Rows.Add("RAMSpec3", "RAMVal3");
            Bench_TLP.Rows.Add("RAMSpec4", "RAMVal4");
            foreach (DataGridViewColumn columnPadding in Bench_TLP.Columns){
                int scaledPadding = (int)(7 * this.DeviceDpi / 96f);
                columnPadding.DefaultCellStyle.Padding = new Padding(scaledPadding, 0, 0, 0);
            }
            //
            Bench_TLP.ColumnHeadersVisible = false;
            Bench_TLP.AllowUserToResizeColumns = false;
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, Bench_TLP, new object[] { true });
        }
        // LOAD
        // ======================================================================================================
        public void Bench_ram_settings(){
            try{
                TSSetWindowTheme(Handle, GlowMain.theme);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                //
                Bench_MStart.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_MStart.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                Bench_MStart.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_MStart.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_MStart.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                Bench_MStop.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_MStop.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                Bench_MStop.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_MStop.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_MStop.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                //
                Bench_TLP.BackgroundColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_TLP.GridColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridColor");
                Bench_TLP.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridBGColor");
                Bench_TLP.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridFEColor");
                Bench_TLP.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridAlternatingColor");
                Bench_TLP.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                Bench_TLP.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                Bench_TLP.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageFE");
                Bench_TLP.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridBGColor");
                Bench_TLP.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridFEColor");
                Bench_TLP.ReadOnly = true;
                //
                TSImageRenderer(Bench_MStart, GlowMain.theme == 1 ? Properties.Resources.ct_test_start_light : Properties.Resources.ct_test_start_dark, 22, ContentAlignment.MiddleRight);
                TSImageRenderer(Bench_MStop, GlowMain.theme == 1 ? Properties.Resources.ct_test_stop_light : Properties.Resources.ct_test_stop_dark, 22, ContentAlignment.MiddleRight);
                //
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
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
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            Bench_TLP.Rows[3].Cells[1].Value = software_lang.TSReadLangs("BenchRAM", "br_await_start");
            //
            Bench_ram_settings();
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
                    if (Bench_TLP.InvokeRequired){
                        Bench_TLP.Invoke(new Action(() =>{
                            Bench_TLP.Rows[0].Cells[1].Value = TS_FormatSize(total_ram);
                            Bench_TLP.Rows[1].Cells[1].Value = string.Format("{0} - {1}", TS_FormatSize(total_ram - usable_ram), string.Format("{0:0.00}%", usage_ram_percentage));
                            Bench_TLP.Rows[2].Cells[1].Value = TS_FormatSize(usable_ram);
                            Bench_TLP.ClearSelection();
                        }));
                    }else{
                        Bench_TLP.Rows[0].Cells[1].Value = TS_FormatSize(total_ram);
                        Bench_TLP.Rows[1].Cells[1].Value = string.Format("{0} - {1}", TS_FormatSize(total_ram - usable_ram), string.Format("{0:0.00}%", usage_ram_percentage));
                        Bench_TLP.Rows[2].Cells[1].Value = TS_FormatSize(usable_ram);
                        Bench_TLP.ClearSelection();
                    }
                    RAMBench_TargetMemoryUsage = (long)(Math.Round(TS_FormatSizeNoType(total_ram - usable_ram)) - 1) * 1024 * 1024 * 1024;
                    await Task.Delay(1000 - DateTime.Now.Millisecond);
                } while (RAMBench_dynamicMemoryUsage);
            }catch (Exception){ }
        }
        // TIMER
        // ======================================================================================================
        private async void BenchTimer(){
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            string titleFormat = software_lang.TSReadLangs("BenchRAM", "br_title");
            string elapsedTimeFormat = software_lang.TSReadLangs("BenchRAM", "br_elapsed_time");
            try{
                while (GlowMain.RAMbenchMode){
                    //
                    TimeSpan elapsed = stopwatch.Elapsed;
                    int fh_second = (int)elapsed.TotalSeconds % 60;
                    int fh_minute = (int)(elapsed.TotalMinutes % 60);
                    int fh_hour = (int)(elapsed.TotalHours);
                    //
                    if (InvokeRequired){
                        Invoke(new Action(() =>{
                            Text = string.Format(titleFormat, Application.ProductName) + " | " + elapsedTimeFormat + " " + string.Format("{0:D2}:{1:D2}:{2:D2}", fh_hour, fh_minute, fh_second);
                        }));
                    }else{
                        Text = string.Format(titleFormat, Application.ProductName) + " | " + elapsedTimeFormat + " " + string.Format("{0:D2}:{1:D2}:{2:D2}", fh_hour, fh_minute, fh_second);
                    }
                    //
                    await Task.Delay(1000);
                }
            }catch (Exception){ }
        }
        // START ENGINE
        // ======================================================================================================
        private async void Bench_MStart_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                DialogResult warning_test = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(software_lang.TSReadLangs("BenchRAM", "br_warning"), "\n\n", "\n"));
                if (warning_test == DialogResult.Yes){
                    RAMBench_cancellationTokenSource = new CancellationTokenSource();
                    Bench_MStart.Enabled = false;
                    Bench_MStop.Enabled = true;
                    GlowMain.RAMbenchMode = true;
                    RAMBench_stopMode = false;
                    //
                    Task timer_start = new Task(BenchTimer);
                    timer_start.Start();
                    //
                    await Task.Run(() => RAMBenchmarkEngine(RAMBench_cancellationTokenSource.Token));
                }
            }catch (Exception){ }
        }
        // STOP ENGINE
        // ======================================================================================================
        private void Bench_MStop_Click(object sender, EventArgs e){
            try{
                RAMBenchStop();
            }catch (Exception){ }
        }
        private void RAMBenchStop(){
            RAMBench_cancellationTokenSource?.Cancel();
            GlowMain.RAMbenchMode = false;
            RAMBench_stopMode = true;
            RAMBench_dynamicMemoryUsage = false;
            Bench_MStart.Enabled = true;
            Bench_MStop.Enabled = false;
            Gc_run();
        }
        // GC RUN
        // ======================================================================================================
        private void Gc_run(){
            RAMBench_allocations.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        // RAM BENCHMARK ENGINE
        // ======================================================================================================
        private async void RAMBenchmarkEngine(CancellationToken cancellationToken){
            try{
         
                while (RAMBench_totalAllocated < RAMBench_TargetMemoryUsage && !cancellationToken.IsCancellationRequested){
                    byte[] allocation = new byte[RAMBench_AllocationSize];
                    for (int i = 0; i < RAMBench_AllocationSize; i += 4096){
                        allocation[i] = 1;
                    }
                    RAMBench_allocations.Add(allocation);
                    RAMBench_totalAllocated += RAMBench_AllocationSize;
                    long totalMemoryDuring = GC.GetTotalMemory(false); // Stop GC Launch Event
                    Invoke(new Action(() =>{
                        Bench_TLP.Rows[3].Cells[1].Value = TS_FormatSize(totalMemoryDuring);
                    }));
                    await Task.Delay(RAMBench_SleepTime);
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
                    GlowMain.RAMbenchMode = false;
                    TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                    Text = string.Format(software_lang.TSReadLangs("BenchRAM", "br_title"), Application.ProductName);
                    Bench_TLP.Rows[3].Cells[1].Value = software_lang.TSReadLangs("BenchRAM", "br_await_start");
                    if (!RAMBench_stopMode){
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BenchRAM", "br_end"));
                    }else{
                        RAMBench_stopMode = false;
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BenchRAM", "br_stopped"));
                    }
                }));
            }
        }
        // EXIT
        // ======================================================================================================
        private void GlowBenchMemory_FormClosing(object sender, FormClosingEventArgs e){
            if (GlowMain.RAMbenchMode){
                e.Cancel = true;
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("GToolsMessage", "gtm_benchmark_ram_prs_msg"));
            }else{
                RAMBenchStop();
            }
        }
    }
}