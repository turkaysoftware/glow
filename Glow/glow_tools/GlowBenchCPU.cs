using System;
using System.Linq;
using System.Threading;
using System.Management;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowBenchCPU : Form{
        private bool isRunning = false;
        private Task[] tasks;
        private Stopwatch stopwatch;
        private double[] results;
        private int singleThreadScore = 0;
        private int multiThreadScore = 0;
        bool loop_mode = true;
        bool bench_mode = false;
        //
        public GlowBenchCPU(){
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //
            Program.TS_TokenEngine = new CancellationTokenSource();
            //
        }
        // LOAD
        // ======================================================================================================
        private void GlowBenchCPU_Load(object sender, EventArgs e){
            cpu_bench_add_mode();
            bench_cpu_theme_settings();
            Bench_CPUName.Text = Glow.bench_cpu_info[0];
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            Bench_CPUCores.Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_core_thread")), Glow.bench_cpu_info[1], Glow.bench_cpu_info[2]);
        }
        // THEME SETTINGS
        // ======================================================================================================
        public void bench_cpu_theme_settings(){
            try{
                int set_attribute = Glow.theme == 1 ? 20 : 19;
                if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != Glow.theme){
                    DwmSetWindowAttribute(Handle, 20, new[] { Glow.theme == 1 ? 0 : 1 }, 4);
                }
                BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                //
                Bench_TLP_T_P1.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                Bench_TLP_T_P2.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                Bench_TLP_T_P3.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                Bench_TLP_R_P1.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                Bench_TLP_R_P2.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                //
                Bench_CPUName.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_CPUCores.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                //
                Bench_Label_RSingle.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_Label_RSingleResult.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_Label_RMulti.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_Label_RMultiResult.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                //
                Bench_Label_Mode.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_Mode.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "SelectBoxBGColor");
                Bench_Mode.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                //
                Bench_Label_Time.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_Time.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "SelectBoxBGColor");
                Bench_Time.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                //
                Bench_TimeCustom.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "TextBoxBGColor");
                Bench_TimeCustom.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "TextBoxFEColor");
                //
                Bench_Start.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_Start.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                Bench_Start.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_Start.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_Start.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRightHover");
                Bench_Stop.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_Stop.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                Bench_Stop.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_Stop.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_Stop.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRightHover");
                ///////
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_title")), Application.ProductName);
                //
                Bench_Label_Mode.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_level"));
                Bench_Mode.Items[0] = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_level_1"));
                Bench_Mode.Items[1] = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_level_2"));
                Bench_Mode.Items[2] = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_level_3"));
                Bench_Mode.Items[3] = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_level_4"));
                //
                Bench_Label_Time.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time"));
                Bench_Time.Items[0] = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_1"));
                Bench_Time.Items[1] = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_2"));
                Bench_Time.Items[2] = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_3"));
                Bench_Time.Items[3] = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_4"));
                Bench_Time.Items[4] = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_5"));
                Bench_Time.Items[5] = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_6"));
                //
                Bench_Label_RSingle.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_score_single"));
                Bench_Label_RMulti.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_score_multi"));
                //
                Bench_Label_RSingleResult.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_score_start_await"));
                Bench_Label_RMultiResult.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_score_start_await"));
                //
                Bench_Start.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_start_engine"));
                Bench_Stop.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_stop_engine"));
            }catch (Exception){ }
        }
        private void cpu_bench_add_mode() {
            try{
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                //
                Bench_Label_Mode.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_level"));
                Bench_Mode.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_level_1")));
                Bench_Mode.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_level_2")));
                Bench_Mode.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_level_3")));
                Bench_Mode.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_level_4")));
                Bench_Mode.SelectedIndex = 0;
                //
                Bench_Label_Time.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time"));
                Bench_Time.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_1")));
                Bench_Time.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_2")));
                Bench_Time.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_3")));
                Bench_Time.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_4")));
                Bench_Time.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_5")));
                Bench_Time.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_6")));
                Bench_Time.SelectedIndex = 0;
            }catch (Exception){ }
        }
        // CUSTOM TIME MODE
        // ======================================================================================================
        private void Bench_Time_SelectedIndexChanged(object sender, EventArgs e){
            if (Bench_Time.SelectedIndex == 5){
                Bench_TimeCustom.Visible = true;
            }else{
                Bench_TimeCustom.Visible = false;
            }
        }
        private void OSD_TextBox_KeyPress(object sender, KeyPressEventArgs e){
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)){
                e.Handled = true;
            }
        }
        // START BTN
        // ======================================================================================================
        private void Bench_Start_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                if (Bench_Mode.SelectedIndex == 3){
                    DialogResult info_warning_hard = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_lethal_warning")), "\n\n", "\n\n", "\n\n"));
                    if (info_warning_hard == DialogResult.Yes){
                        bench_start_engine();
                    }
                }else{
                    if (Bench_Time.SelectedIndex == 5){
                        if (!string.IsNullOrEmpty(Bench_TimeCustom.Text.Trim())){
                            DialogResult info_warning_normal = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_test_start_warning")), "\n\n", "\n\n", "\n\n"));
                            if (info_warning_normal == DialogResult.Yes){
                                bench_start_engine();
                            }
                        }else{
                            TS_MessageBoxEngine.TS_MessageBox(this, 6, TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_time_custom_warning")));
                        }
                    }else{
                        DialogResult info_warning_normal = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_test_start_warning")), "\n\n", "\n\n", "\n\n"));
                        if (info_warning_normal == DialogResult.Yes){
                            bench_start_engine();
                        }
                    }
                }
            }catch (Exception){ }
        }
        // TIMER
        // ======================================================================================================
        private async Task BenchTimerAsync(){
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            //
            var software_lang = new TSGetLangs(Glow.lang_path);
            string titleFormat = string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_title")), Application.ProductName);
            string elapsedTimeFormat = TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_elapsed_time"));
            //
            try{
                while (loop_mode){
                    //
                    TimeSpan elapsed = stopwatch.Elapsed;
                    int fh_second = (int)elapsed.TotalSeconds % 60;
                    int fh_minute = (int)(elapsed.TotalMinutes % 60);
                    int fh_hour = (int)(elapsed.TotalHours);
                    //
                    Text = $"{titleFormat} - {elapsedTimeFormat} {fh_hour:D2}:{fh_minute:D2}:{fh_second:D2}";
                    //
                    await Task.Delay(1000);
                }
            }catch (Exception){ }
        }
        // CPU BENCHMARK ENGINE
        // ======================================================================================================
        private async void bench_start_engine(){
            if (!isRunning){
                bench_mode = true;
                loop_mode = true;
                isRunning = true;
                //
                Bench_Start.Enabled = false;
                Bench_Stop.Enabled = true;
                Bench_Mode.Enabled = false;
                Bench_Time.Enabled = false;
                Bench_TimeCustom.Enabled = false;
                // CPU SYSTEM
                double[] coreSpeeds = GetCoreSpeeds();
                double averageSpeed = CalculateAverageSpeed(coreSpeeds);
                int coreCount = Environment.ProcessorCount;
                if (Bench_Mode.SelectedIndex == 0)
                    coreCount /= 3;
                else if (Bench_Mode.SelectedIndex == 1)
                    coreCount /= 2;
                else if (Bench_Mode.SelectedIndex == 2)
                    coreCount -= 1;
                // CPU SCORE
                var updateScoreTask = Task.Run(async () =>{
                    while (isRunning){
                        singleThreadScore = CalculateSingleThreadScore(coreCount, averageSpeed);
                        multiThreadScore = CalculateMultiThreadScore(coreCount, averageSpeed);
                        //
                        try{
                            Invoke((MethodInvoker)delegate {
                                Bench_Label_RSingleResult.Text = singleThreadScore.ToString();
                                Bench_Label_RMultiResult.Text = multiThreadScore.ToString();
                            });
                        }
                        catch (Exception) { }
                        await Task.Delay(100);
                    }
                });
                // ENGINE STARTER
                stopwatch = new Stopwatch();
                results = new double[coreCount];
                //
                var timerTask = BenchTimerAsync();
                //
                stopwatch.Start();
                // ALL CORE WITH TASKS
                tasks = new Task[coreCount];
                for (int i = 0; i < coreCount; i++){
                    int coreIndex = i;
                    tasks[i] = Task.Run(() =>{
                        DateTime endTime = DateTime.Now.AddMinutes(1);
                        if (Bench_Time.SelectedIndex == 0)
                            endTime = DateTime.Now.AddSeconds(30);
                        else if (Bench_Time.SelectedIndex == 1)
                            endTime = DateTime.Now.AddMinutes(1);
                        else if (Bench_Time.SelectedIndex == 2)
                            endTime = DateTime.Now.AddMinutes(15);
                        else if (Bench_Time.SelectedIndex == 3)
                            endTime = DateTime.Now.AddMinutes(30);
                        else if (Bench_Time.SelectedIndex == 1)
                            endTime = DateTime.Now.AddHours(1);
                        else if (!string.IsNullOrEmpty(Bench_TimeCustom.Text))
                            endTime = DateTime.Now.AddMinutes(Convert.ToDouble(Bench_TimeCustom.Text.Trim()));
                        // ENGINE MODE
                        Random random = new Random();
                        while (isRunning && DateTime.Now < endTime){
                            double number = random.NextDouble();
                            double result = Math.Sqrt(number);
                            results[coreIndex] += result;
                        }
                        // ENGINE STOPPER
                        if (coreIndex == coreCount - 1){
                            stopwatch.Stop();
                            isRunning = false;
                            loop_mode = false;
                            // UI güncelleme
                            Invoke((MethodInvoker)delegate{
                                Bench_Start.Enabled = true;
                                Bench_Stop.Enabled = false;
                                Bench_Mode.Enabled = true;
                                Bench_Time.Enabled = true;
                                Bench_TimeCustom.Enabled = true;
                                //
                                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                                Text = $"{string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_title")), Application.ProductName)} | {TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_end_test"))}";
                                bench_mode = false;
                            });
                        }
                    });
                }
                //
                await Task.WhenAll(tasks);
                await updateScoreTask;
                await timerTask;
            }
        }
        // SINGLE THREAD SCORE
        // ======================================================================================================
        private int CalculateSingleThreadScore(int coreCount, double averageSpeed){
            double singleThreadPerformance = averageSpeed;
            double referencePerformance = coreCount * 2.0;
            int singleThreadScore = (int)((singleThreadPerformance / referencePerformance) * 1000);
            return singleThreadScore;
        }
        // MULTI THREAD SCORE
        // ======================================================================================================
        private int CalculateMultiThreadScore(int coreCount, double averageSpeed){
            double multiThreadPerformance = averageSpeed * coreCount;
            double referencePerformance = coreCount * 2.0;
            int multiThreadScore = (int)((multiThreadPerformance / referencePerformance) * 1000);
            return multiThreadScore;
        }
        // CPU SPEED
        // ======================================================================================================
        private double[] GetCoreSpeeds(){
            double[] speeds = new double[Environment.ProcessorCount];
            ManagementObjectSearcher ts_search = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
            int index = 0;
            foreach (ManagementObject ts_obj in ts_search.Get()){
                speeds[index] = Convert.ToDouble(ts_obj["CurrentClockSpeed"]);
                index++;
            }
            return speeds;
        }
        // AVERAGE SPEED
        // ======================================================================================================
        private double CalculateAverageSpeed(double[] speeds){
            double sum = 0;
            foreach (double speed in speeds){
                sum += speed;
            }
            return sum / speeds.Length;
        }
        // ENGINE STOP BTN
        // ======================================================================================================
        private void Bench_Stop_Click(object sender, EventArgs e){
            bench_stop_engine();
        }
        // ENGINE STOP MODE
        // ======================================================================================================
        private async void bench_stop_engine(){
            if (isRunning){
                bench_mode = false;
                isRunning = false;
                //
                Bench_Start.Enabled = true;
                Bench_Stop.Enabled = false;
                //
                try{
                    if (tasks != null){
                        await Task.WhenAll(tasks);
                    }
                }catch (Exception){ }
                //
                stopwatch.Stop();
                double totalResult = results.Sum();
                //
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                Text = $"{string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_title")), Application.ProductName)} | {TS_String_Encoder(software_lang.TSReadLangs("BenchCPU", "bc_stop_engine_message"))}";
                //
                // Console.WriteLine($"Hesaplama süresi: {stopwatch.Elapsed.Seconds} saniye");
            }
        }
        // EXIT
        // ======================================================================================================
        private void GlowBenchCPU_FormClosing(object sender, FormClosingEventArgs e){
            if (bench_mode == true){
                e.Cancel = true;
            }else{
                bench_stop_engine();
            }
        }
    }
}