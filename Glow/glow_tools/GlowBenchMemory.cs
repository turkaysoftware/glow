using System;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;
//
using static Glow.GlowModules;

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
                BackColor = Glow.ui_colors[5];
                Bench_Panel.BackColor = Glow.ui_colors[6];
                //
                Bench_MStart.BackColor = Glow.ui_colors[8];
                Bench_MStart.ForeColor = Glow.ui_colors[18];
                Bench_MStop.BackColor = Glow.ui_colors[8];
                Bench_MStop.ForeColor = Glow.ui_colors[18];
                //
                Bench_TLP.BackgroundColor = Glow.ui_colors[12];
                Bench_TLP.GridColor = Glow.ui_colors[14];
                Bench_TLP.DefaultCellStyle.BackColor = Glow.ui_colors[12];
                Bench_TLP.DefaultCellStyle.ForeColor = Glow.ui_colors[13];
                Bench_TLP.AlternatingRowsDefaultCellStyle.BackColor = Glow.ui_colors[15];
                Bench_TLP.ColumnHeadersDefaultCellStyle.BackColor = Glow.ui_colors[16];
                Bench_TLP.ColumnHeadersDefaultCellStyle.SelectionBackColor = Glow.ui_colors[16];
                Bench_TLP.ColumnHeadersDefaultCellStyle.ForeColor = Glow.ui_colors[17];
                Bench_TLP.DefaultCellStyle.SelectionBackColor = Glow.ui_colors[16];
                Bench_TLP.DefaultCellStyle.SelectionForeColor = Glow.ui_colors[17];
                //
                TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_title").Trim())), Application.ProductName);
                //
                Bench_TLP.Rows[0].Cells[0].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_ram_total").Trim()));
                Bench_TLP.Rows[1].Cells[0].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_ram_used").Trim()));
                Bench_TLP.Rows[2].Cells[0].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_ram_empty").Trim()));
                Bench_TLP.Rows[3].Cells[0].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_ram_allocated").Trim()));
                //
                Bench_MStart.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_start").Trim()));
                Bench_MStop.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_stop").Trim()));
            }catch (Exception){ }
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
            TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
            Bench_TLP.Rows[3].Cells[1].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_await_start").Trim()));
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
            int fh_second = 0;
            int fh_minute = 0;
            int fh_hour = 0;
            try{
                do{
                    fh_second++;
                    if (fh_second == 60){
                        fh_second = 0;
                        fh_minute++;
                    }
                    if (fh_minute == 60){
                        fh_minute = 0;
                        fh_hour++;
                    }
                    TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
                    Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_title").Trim())), Application.ProductName) + " | " +  Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_elapsed_time").Trim())) + " " + string.Format("{0:D2}:{1:D2}:{2:D2}", fh_hour, fh_minute, fh_second);
                    Thread.Sleep(1000);
                }while (loop_mode);
            }catch (Exception){ }
        }
        // START ENGINE
        private async void Bench_MStart_Click(object sender, EventArgs e){
            try{
                TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
                DialogResult warning_test = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_warning").Trim())), "\n\n", "\n"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
                    TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
                    Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_title").Trim())), Application.ProductName);
                    Bench_TLP.Rows[3].Cells[1].Value = Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_await_start").Trim()));
                    MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(g_lang.TSReadLangs("BenchRAM", "br_end").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
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