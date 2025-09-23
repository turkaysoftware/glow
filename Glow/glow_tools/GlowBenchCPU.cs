using System;
using System.Linq;
using System.Drawing;
using System.Management;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowBenchCPU : Form{
        // VARIABLES
        // ======================================================================================================
        private Task[] CPUBench_taskList;
        private Stopwatch CPUBench_stopWatch;
        private bool CPUBench_isRunning = false;
        private double[] CPUBench_collector;
        private int CPUBench_singleThreadScore = 0, CPUBench_multiThreadScore = 0;
        public GlowBenchCPU() { InitializeComponent(); }
        // LOAD
        // ======================================================================================================
        private void GlowBenchCPU_Load(object sender, EventArgs e){
            Cpu_bench_add_mode();
            Bench_cpu_theme_settings();
            Bench_CPUName.Text = GlowMain.bench_cpu_info[0];
            // TEXT CPU NAME
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            Bench_CPUCores.Text = string.Format(software_lang.TSReadLangs("BenchCPU", "bc_core_thread"), GlowMain.bench_cpu_info[1], GlowMain.bench_cpu_info[2]);
        }
        // THEME SETTINGS
        // ======================================================================================================
        public void Bench_cpu_theme_settings(){
            try{
                TSSetWindowTheme(Handle, GlowMain.theme);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_BG_Panel.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                //
                Bench_TLP_T_P1.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_TLP_T_P2.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_TLP_T_P3.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_TLP_R_P1.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_TLP_R_P2.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                //
                Bench_CPUName.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_CPUCores.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                //
                Bench_Label_RSingle.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_Label_RSingleResult.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_Label_RMulti.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_Label_RMultiResult.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                //
                Bench_Label_Mode.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                BenchMode.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                BenchMode.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                BenchMode.HoverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                BenchMode.ButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor2");
                BenchMode.HoverButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor2");
                BenchMode.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                BenchMode.FocusedBorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                BenchMode.DisabledBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                BenchMode.DisabledForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                BenchMode.DisabledButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor2");
                //
                Bench_Label_Time.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_Time.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_Time.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_Time.HoverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_Time.ButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor2");
                Bench_Time.HoverButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor2");
                Bench_Time.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_Time.FocusedBorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_Time.DisabledBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_Time.DisabledForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_Time.DisabledButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor2");
                //
                Bench_TimeCustom.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TextBoxBGColor");
                Bench_TimeCustom.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TextBoxFEColor");
                //
                Bench_Start.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_Start.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                Bench_Start.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_Start.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_Start.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                Bench_Stop.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_Stop.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                Bench_Stop.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_Stop.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_Stop.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                ///////
                TSImageRenderer(Bench_Start, GlowMain.theme == 1 ? Properties.Resources.ct_test_start_light : Properties.Resources.ct_test_start_dark, 22, ContentAlignment.MiddleRight);
                TSImageRenderer(Bench_Stop, GlowMain.theme == 1 ? Properties.Resources.ct_test_stop_light : Properties.Resources.ct_test_stop_dark, 22, ContentAlignment.MiddleRight);
                ///////
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                Text = string.Format(software_lang.TSReadLangs("BenchCPU", "bc_title"), Application.ProductName);
                //
                Bench_Label_Mode.Text = software_lang.TSReadLangs("BenchCPU", "bc_level");
                BenchMode.Items[0] = software_lang.TSReadLangs("BenchCPU", "bc_level_1");
                BenchMode.Items[1] = software_lang.TSReadLangs("BenchCPU", "bc_level_2");
                BenchMode.Items[2] = software_lang.TSReadLangs("BenchCPU", "bc_level_3");
                BenchMode.Items[3] = software_lang.TSReadLangs("BenchCPU", "bc_level_4");
                //
                Bench_Label_Time.Text = software_lang.TSReadLangs("BenchCPU", "bc_time");
                Bench_Time.Items[0] = software_lang.TSReadLangs("BenchCPU", "bc_time_1");
                Bench_Time.Items[1] = software_lang.TSReadLangs("BenchCPU", "bc_time_2");
                Bench_Time.Items[2] = software_lang.TSReadLangs("BenchCPU", "bc_time_3");
                Bench_Time.Items[3] = software_lang.TSReadLangs("BenchCPU", "bc_time_4");
                Bench_Time.Items[4] = software_lang.TSReadLangs("BenchCPU", "bc_time_5");
                Bench_Time.Items[5] = software_lang.TSReadLangs("BenchCPU", "bc_time_6");
                //
                Bench_Label_RSingle.Text = software_lang.TSReadLangs("BenchCPU", "bc_score_single");
                Bench_Label_RMulti.Text = software_lang.TSReadLangs("BenchCPU", "bc_score_multi");
                //
                Bench_Label_RSingleResult.Text = software_lang.TSReadLangs("BenchCPU", "bc_score_start_await");
                Bench_Label_RMultiResult.Text = software_lang.TSReadLangs("BenchCPU", "bc_score_start_await");
                //
                Bench_Start.Text = " " + software_lang.TSReadLangs("BenchCPU", "bc_start_engine");
                Bench_Stop.Text = " " + software_lang.TSReadLangs("BenchCPU", "bc_stop_engine");
            }catch (Exception){ }
        }
        private void Cpu_bench_add_mode() {
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                //
                Bench_Label_Mode.Text = software_lang.TSReadLangs("BenchCPU", "bc_level");
                BenchMode.Items.Add(software_lang.TSReadLangs("BenchCPU", "bc_level_1"));
                BenchMode.Items.Add(software_lang.TSReadLangs("BenchCPU", "bc_level_2"));
                BenchMode.Items.Add(software_lang.TSReadLangs("BenchCPU", "bc_level_3"));
                BenchMode.Items.Add(software_lang.TSReadLangs("BenchCPU", "bc_level_4"));
                BenchMode.SelectedIndex = 0;
                //
                Bench_Label_Time.Text = software_lang.TSReadLangs("BenchCPU", "bc_time");
                Bench_Time.Items.Add(software_lang.TSReadLangs("BenchCPU", "bc_time_1"));
                Bench_Time.Items.Add(software_lang.TSReadLangs("BenchCPU", "bc_time_2"));
                Bench_Time.Items.Add(software_lang.TSReadLangs("BenchCPU", "bc_time_3"));
                Bench_Time.Items.Add(software_lang.TSReadLangs("BenchCPU", "bc_time_4"));
                Bench_Time.Items.Add(software_lang.TSReadLangs("BenchCPU", "bc_time_5"));
                Bench_Time.Items.Add(software_lang.TSReadLangs("BenchCPU", "bc_time_6"));
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
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                if (BenchMode.SelectedIndex == 3){
                    DialogResult info_warning_hard = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(software_lang.TSReadLangs("BenchCPU", "bc_lethal_warning"), "\n\n", "\n\n", "\n\n"));
                    if (info_warning_hard == DialogResult.Yes){
                        Bench_start_engine();
                    }
                }else{
                    if (Bench_Time.SelectedIndex == 5){
                        if (!string.IsNullOrEmpty(Bench_TimeCustom.Text.Trim())){
                            DialogResult info_warning_normal = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(software_lang.TSReadLangs("BenchCPU", "bc_test_start_warning"), "\n\n", "\n\n", "\n\n"));
                            if (info_warning_normal == DialogResult.Yes){
                                Bench_start_engine();
                            }
                        }else{
                            TS_MessageBoxEngine.TS_MessageBox(this, 6, software_lang.TSReadLangs("BenchCPU", "bc_time_custom_warning"));
                        }
                    }else{
                        DialogResult info_warning_normal = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(software_lang.TSReadLangs("BenchCPU", "bc_test_start_warning"), "\n\n", "\n\n", "\n\n"));
                        if (info_warning_normal == DialogResult.Yes){
                            Bench_start_engine();
                        }
                    }
                }
            }catch (Exception){ }
        }
        // TIMER
        // ======================================================================================================
        private async Task BenchTimerAsync(){
            CPUBench_stopWatch = new Stopwatch();
            CPUBench_stopWatch.Start();
            var software_lang = new TSGetLangs(GlowMain.lang_path);
            string titleFormat = string.Format(software_lang.TSReadLangs("BenchCPU", "bc_title"), Application.ProductName);
            string elapsedTimeFormat = software_lang.TSReadLangs("BenchCPU", "bc_elapsed_time");
            try{
                while (GlowMain.CPUbenchMode)
                {
                    TimeSpan elapsed = CPUBench_stopWatch.Elapsed;
                    int fh_second = (int)elapsed.TotalSeconds % 60;
                    int fh_minute = (int)(elapsed.TotalMinutes % 60);
                    int fh_hour = (int)elapsed.TotalHours;
                    if (InvokeRequired){
                        Invoke(new Action(() =>{
                            Text = $"{titleFormat} - {elapsedTimeFormat} {fh_hour:D2}:{fh_minute:D2}:{fh_second:D2}";
                        }));
                    }else{
                        Text = $"{titleFormat} - {elapsedTimeFormat} {fh_hour:D2}:{fh_minute:D2}:{fh_second:D2}";
                    }
                    await Task.Delay(1000);
                }
            }catch (Exception){ }
        }
        // CPU BENCHMARK ENGINE
        // ======================================================================================================
        private async void Bench_start_engine(){
            if (!CPUBench_isRunning){
                GlowMain.CPUbenchMode = true;
                CPUBench_isRunning = true;
                //
                Bench_Start.Enabled = false;
                Bench_Stop.Enabled = true;
                BenchMode.Enabled = false;
                Bench_Time.Enabled = false;
                Bench_TimeCustom.Enabled = false;
                // CPU SYSTEM
                double[] coreSpeeds = GetCoreSpeeds();
                double averageSpeed = CalculateAverageSpeed(coreSpeeds);
                int coreCount = Environment.ProcessorCount;
                if (BenchMode.SelectedIndex == 0)
                    coreCount /= 3;
                else if (BenchMode.SelectedIndex == 1)
                    coreCount /= 2;
                else if (BenchMode.SelectedIndex == 2)
                    coreCount -= 1;
                // CPU SCORE
                var updateScoreTask = Task.Run(async () =>{
                    while (CPUBench_isRunning){
                        CPUBench_singleThreadScore = CalculateSingleThreadScore(coreCount, averageSpeed);
                        CPUBench_multiThreadScore = CalculateMultiThreadScore(coreCount, averageSpeed);
                        if (InvokeRequired){
                            Invoke(new Action(() =>{
                                Bench_Label_RSingleResult.Text = CPUBench_singleThreadScore.ToString();
                                Bench_Label_RMultiResult.Text = CPUBench_multiThreadScore.ToString();
                            }));
                        }else{
                            Bench_Label_RSingleResult.Text = CPUBench_singleThreadScore.ToString();
                            Bench_Label_RMultiResult.Text = CPUBench_multiThreadScore.ToString();
                        }
                        await Task.Delay(100);
                    }
                });
                // ENGINE STARTER
                CPUBench_stopWatch = new Stopwatch();
                CPUBench_collector = new double[coreCount];
                //
                var timerTask = BenchTimerAsync();
                //
                CPUBench_stopWatch.Start();
                // ALL CORE WITH TASKS
                CPUBench_taskList = new Task[coreCount];
                for (int i = 0; i < coreCount; i++){
                    int coreIndex = i;
                    CPUBench_taskList[i] = Task.Run(() =>{
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
                        while (CPUBench_isRunning && DateTime.Now < endTime){
                            double number = random.NextDouble();
                            double result = Math.Sqrt(number);
                            CPUBench_collector[coreIndex] += result;
                        }
                        // ENGINE STOPPER
                        if (coreIndex == coreCount - 1){
                            CPUBench_stopWatch.Stop();
                            CPUBench_isRunning = false;
                            if (InvokeRequired){
                                Invoke(new Action(() =>{
                                    Bench_Start.Enabled = true;
                                    Bench_Stop.Enabled = false;
                                    BenchMode.Enabled = true;
                                    Bench_Time.Enabled = true;
                                    Bench_TimeCustom.Enabled = true;
                                    TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                                    Text = $"{string.Format(software_lang.TSReadLangs("BenchCPU", "bc_title"), Application.ProductName)} | {software_lang.TSReadLangs("BenchCPU", "bc_end_test")}";
                                    GlowMain.CPUbenchMode = false;
                                }));
                            }else{
                                Bench_Start.Enabled = true;
                                Bench_Stop.Enabled = false;
                                BenchMode.Enabled = true;
                                Bench_Time.Enabled = true;
                                Bench_TimeCustom.Enabled = true;
                                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                                Text = $"{string.Format(software_lang.TSReadLangs("BenchCPU", "bc_title"), Application.ProductName)} | {software_lang.TSReadLangs("BenchCPU", "bc_end_test")}";
                                GlowMain.CPUbenchMode = false;
                            }
                        }
                    });
                }
                //
                await Task.WhenAll(CPUBench_taskList);
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
            foreach (ManagementObject ts_obj in ts_search.Get().Cast<ManagementObject>()){
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
            Bench_stop_engine();
        }
        // ENGINE STOP MODE
        // ======================================================================================================
        private async void Bench_stop_engine(){
            if (CPUBench_isRunning){
                GlowMain.CPUbenchMode = false;
                CPUBench_isRunning = false;
                //
                Bench_Start.Enabled = true;
                Bench_Stop.Enabled = false;
                //
                try{
                    if (CPUBench_taskList != null){
                        await Task.WhenAll(CPUBench_taskList);
                    }
                }catch (Exception){ }
                //
                CPUBench_stopWatch.Stop();
                //
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                Text = $"{string.Format(software_lang.TSReadLangs("BenchCPU", "bc_title"), Application.ProductName)} | {software_lang.TSReadLangs("BenchCPU", "bc_stop_engine_message")}";
                //
                // Console.WriteLine($"Hesaplama süresi: {stopwatch.Elapsed.Seconds} saniye");
            }
        }
        // EXIT
        // ======================================================================================================
        private void GlowBenchCPU_FormClosing(object sender, FormClosingEventArgs e){
            if (GlowMain.CPUbenchMode){
                e.Cancel = true;
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("GToolsMessage", "gtm_benchmark_cpu_prs_msg"));
            }else{
                Bench_stop_engine();
            }
        }
    }
}