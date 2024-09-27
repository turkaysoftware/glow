using System;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;
//
using static Glow.TSModules;
using System.Diagnostics;

namespace Glow.glow_tools{
    public partial class GlowBenchMemory : Form{
        public GlowBenchMemory(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        //
        private long TargetMemoryUsage;
        private const int AllocationSize = 100 * 1024 * 1024; // 100 MB
        private const int SleepTime = 200; // 0.2 seconds
        //
        private long _totalAllocated = 0;
        private int _allocationCount = 0;
        private List<byte[]> _allocations = new List<byte[]>();
        private CancellationTokenSource _cancellationTokenSource;
        //
        bool ram_mode_loop_status = true;
        bool loop_mode;
        // LOAD
        public void bench_ram_settings(){
            try{
                if (Glow.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                }else if (Glow.theme == 0){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                }
                BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_Panel.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                //
                Bench_MStart.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_MStart.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                Bench_MStart.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_MStart.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_MStop.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_MStop.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                Bench_MStop.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_MStop.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
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
                Bench_TLP.ReadOnly = true;  // VeriGrid'i sadece okunabilir yapar
                //
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_title").Trim())), Application.ProductName);
                //
                Bench_TLP.Rows[0].Cells[0].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_ram_total").Trim()));
                Bench_TLP.Rows[1].Cells[0].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_ram_used").Trim()));
                Bench_TLP.Rows[2].Cells[0].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_ram_empty").Trim()));
                Bench_TLP.Rows[3].Cells[0].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_ram_allocated").Trim()));
                //
                Bench_MStart.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_start").Trim()));
                Bench_MStop.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_stop").Trim()));
            }catch (Exception){ }
        }
        private void Bench_TLP_CellClick(object sender, DataGridViewCellEventArgs e){
            Bench_TLP.ClearSelection();
        }
        // MAIN LOAD
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
            Bench_TLP.Rows[3].Cells[1].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_await_start").Trim()));
            //
            bench_ram_settings();
            //
            Bench_TLP.ColumnHeadersVisible = false;
            Bench_TLP.AllowUserToResizeColumns = false;
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, Bench_TLP, new object[] { true });
            //
            Task dynamic_ram_info = new Task(dynamic_ram_status);
            dynamic_ram_info.Start();
        }
        // DYNAMIC RAM STATUS
        private void dynamic_ram_status(){
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
                    Thread.Sleep(1000 - DateTime.Now.Millisecond);
                } while (ram_mode_loop_status);
            }catch (Exception){ }
        }
        // TIMER
        private void BenchTimer(){
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            string titleFormat = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_title").Trim()));
            string elapsedTimeFormat = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_elapsed_time").Trim()));
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
                    Thread.Sleep(1000);
                }
            }catch (Exception){ }
        }
        // START ENGINE
        private async void Bench_MStart_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                DialogResult warning_test = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_warning").Trim())), "\n\n", "\n"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
        private void Bench_MStop_Click(object sender, EventArgs e){
            try{
                _cancellationTokenSource.Cancel();
                Bench_MStart.Enabled = true;
                Bench_MStop.Enabled = false;
                gc_run();
            }catch (Exception){ }
        }
        // GC RUN
        private void gc_run(){
            _allocations.Clear();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        // RAM BENCHMARK ENGINE
        private void RAMBenchmarkEngine(CancellationToken cancellationToken){
            try{
                while (_totalAllocated < TargetMemoryUsage && !cancellationToken.IsCancellationRequested){
                    byte[] allocation = new byte[AllocationSize];
                    for (int i = 0; i < AllocationSize; i += 4096){
                        allocation[i] = 1;
                    }
                    _allocations.Add(allocation);
                    _totalAllocated += AllocationSize;
                    _allocationCount++; // Step
                    long totalMemoryDuring = GC.GetTotalMemory(false); // Stop GC Launch Event
                    Invoke(new Action(() =>{
                        Bench_TLP.Rows[3].Cells[1].Value = TS_FormatSize(totalMemoryDuring);
                    }));
                    Thread.Sleep(SleepTime);
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
                    gc_run();
                    loop_mode = false;
                    TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                    Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_title").Trim())), Application.ProductName);
                    Bench_TLP.Rows[3].Cells[1].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_await_start").Trim()));
                    MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("BenchRAM", "br_end").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
        }
        // EXIT
        private void GlowBenchMemory_FormClosing(object sender, FormClosingEventArgs e){
            gc_run();
            ram_mode_loop_status = false;
            Hide();
        }
    }
}