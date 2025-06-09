using System;
using System.IO;
using System.Linq;
using System.Management;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowBenchDisk : Form{
        // VARIABLES
        private Task benchmarkTask;
        private bool isBenchmarking = false;
        string benchmarkFilePath;
        int global_buffer;
        bool bench_mode = false;
        //
        bool loop_mode = true;
        string select_disk;
        string global_timer;
        //
        List<string> benchmarkDiskList = new List<string>();
        List<double> benchmarkDiskListFreeSpace = new List<double>();
        List<string> benchmarkDiskListType = new List<string>();
        List<long> testSizes = new List<long>();
        public GlowBenchDisk(){ InitializeComponent(); CheckForIllegalCrossThreadCalls = false; }
        // THEME MODE
        // ======================================================================================================
        public void bench_disk_theme_settings(){
            try{
                int set_attribute = Glow.theme == 1 ? 20 : 19;
                if (DwmSetWindowAttribute(Handle, set_attribute, new[] { 1 }, 4) != Glow.theme){
                    DwmSetWindowAttribute(Handle, 20, new[] { Glow.theme == 1 ? 0 : 1 }, 4);
                }
                BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                //
                Bench_P1.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                Bench_P2.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                Bench_P3.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                Bench_P4.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                Bench_P5.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                Bench_P6.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                Bench_P7.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentPanelBGColor");
                //
                Bench_Label_Disk.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_Disk.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "SelectBoxBGColor");
                Bench_Disk.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                //
                Bench_Label_Size.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_Size.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "SelectBoxBGColor");
                Bench_Size.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_SizeCustom.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "TextBoxBGColor");
                Bench_SizeCustom.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "TextBoxFEColor");
                //
                Bench_Label_Buffer.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_Buffer.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "SelectBoxBGColor");
                Bench_Buffer.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                //
                Bench_L_WriteSpeed.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_L_WriteSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_R_ReadSpeed.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_R_ReadSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_L_Max_WriteSpeed.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_L_Max_WriteSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                Bench_R_Max_ReadSpeed.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelLeft");
                Bench_R_Max_ReadSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
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
                //
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_title")), Application.ProductName);
                Bench_Label_Disk.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_select_disk"));
                Bench_Label_Size.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_test_size"));
                Bench_Label_Buffer.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_test_buffer_size"));
                //
                Bench_L_WriteSpeed.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_time_write"));
                Bench_L_Max_WriteSpeed.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_time_max_write"));
                Bench_R_ReadSpeed.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_time_read"));
                Bench_R_Max_ReadSpeed.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_time_max_read"));
                //
                Bench_Size.Items[7] = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_test_size_custom"));
                //
                Bench_Start.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_start"));
                Bench_Stop.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_stop"));
            }catch (Exception){ }
        }
        // LOAD
        // ======================================================================================================
        private void GlowBenchDisk_Load(object sender, EventArgs e){
            RefreshDriveList();
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            //
            Bench_Size.Items.Add("12 GB");
            Bench_Size.Items.Add("15 GB");
            Bench_Size.Items.Add("20 GB");
            Bench_Size.Items.Add("25 GB");
            Bench_Size.Items.Add("32 GB");
            Bench_Size.Items.Add("64 GB");
            Bench_Size.Items.Add("128 GB");
            Bench_Size.Items.Add(TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_test_size_custom")));
            testSizes.Add(12);
            testSizes.Add(15);
            testSizes.Add(20);
            testSizes.Add(25);
            testSizes.Add(32);
            testSizes.Add(64);
            testSizes.Add(128);
            Bench_Size.SelectedIndex = 0;
            //
            Bench_Buffer.Items.Add("64 KB");
            Bench_Buffer.Items.Add("128 KB");
            Bench_Buffer.Items.Add("256 KB");
            Bench_Buffer.Items.Add("512 KB");
            Bench_Buffer.Items.Add("1024 KB");
            Bench_Buffer.Items.Add("2048 KB");
            Bench_Buffer.Items.Add("4096 KB");
            Bench_Buffer.Items.Add("8192 KB");
            Bench_Buffer.SelectedIndex = 4;
            //
            Bench_L_WriteSpeed_V.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_start_test_await"));
            Bench_L_Max_WriteSpeed_V.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_start_test_await"));
            Bench_R_ReadSpeed_V.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_start_test_await"));
            Bench_R_Max_ReadSpeed_V.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_start_test_await")); 
            //
            bench_disk_theme_settings();
        }
        // DISK LIST
        // ======================================================================================================
        private void RefreshDriveList(){
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            Bench_Disk.Items.Clear();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives){
                string driveInfo = "";
                if (drive.VolumeLabel.ToLower().Trim() == "" || drive.VolumeLabel == string.Empty){
                    driveInfo = $"{TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_select_local_disk"))} ({drive.Name.Replace("\\", string.Empty)}) - {TS_FormatSize(drive.TotalSize)}";
                }else{ 
                    driveInfo = $"{drive.VolumeLabel} ({drive.Name.Replace("\\", string.Empty)}) - {TS_FormatSize(drive.TotalSize)}";
                }
                Bench_Disk.Items.Add(driveInfo);
                benchmarkDiskList.Add(drive.Name);
                benchmarkDiskListFreeSpace.Add(drive.TotalFreeSpace / 1024 / 1024 / 1024);
                benchmarkDiskListType.Add(drive.DriveType.ToString().ToLower().Trim());
            }
            Bench_Disk.SelectedIndex = 0;
        }
        // START BTN
        // ======================================================================================================
        private void Bench_Start_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                if (Bench_Size.SelectedIndex == Bench_Size.Items.Count - 1){
                    if (Bench_SizeCustom.Text.Trim() != "" || Bench_SizeCustom.Text.Trim() != string.Empty){
                        if (Convert.ToDouble(Bench_SizeCustom.Text.Trim()) >= 10 && Convert.ToDouble(Bench_SizeCustom.Text.Trim()) <= 256){
                            if (benchmarkDiskListFreeSpace[Bench_Disk.SelectedIndex] > Convert.ToInt32(Bench_SizeCustom.Text.Trim())){
                                check_info_user_warning(Bench_Disk.SelectedIndex);
                            }else{
                                TS_MessageBoxEngine.TS_MessageBox(this, 2, TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_low_space")));
                            }
                        }else{
                            TS_MessageBoxEngine.TS_MessageBox(this, 2, TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_space_req")));
                            Bench_SizeCustom.Text = string.Empty;
                            Bench_SizeCustom.Focus();
                        }
                    }
                }else{
                    if (benchmarkDiskListFreeSpace[Bench_Disk.SelectedIndex] > testSizes[Bench_Disk.SelectedIndex]){
                        check_info_user_warning(Bench_Disk.SelectedIndex);
                    }else{
                        TS_MessageBoxEngine.TS_MessageBox(this, 2, TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_low_space")));
                    }
                }
            }catch (Exception){ }
        }
        // CHECK DISK USER INFO
        // ======================================================================================================
        private void check_info_user_warning(int info_mode){
            try{
                TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                DialogResult success_warning;
                var warnings = new Dictionary<string, (string MessageKey, bool StartEngine)>{
                    { "cdrom", ( "bd_disk_cdrom", true ) },
                    { "fixed", ( "bd_disk_fixed", true ) },
                    { "network", ( "bd_disk_network", true ) },
                    { "norootdirectory", ( "bd_disk_nrd", false ) },
                    { "ram", ( "bd_disk_ram", false ) },
                    { "removable", ( "bd_disk_removable", true ) },
                    { "unknown", ( "bd_disk_unknown", false ) }
                };
                string mode = benchmarkDiskListType[info_mode];
                string messageKey = warnings.ContainsKey(mode) ? warnings[mode].MessageKey : warnings["unknown"].MessageKey;
                bool startEngine = warnings.ContainsKey(mode) ? warnings[mode].StartEngine : false;
                //
                string message = string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", messageKey)), "\n\n", "\n\n", "\n", "\n\n");
                string caption = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_start_engine_disk")) + " " + Bench_Disk.SelectedItem.ToString().Trim();
                //
                success_warning = TS_MessageBoxEngine.TS_MessageBox(this, 6, message, caption);
                if (!startEngine){
                    success_warning = TS_MessageBoxEngine.TS_MessageBox(this, 1, message, caption);
                }
                //
                if (startEngine && success_warning == DialogResult.Yes){
                    start_engine();
                }
            }catch (Exception){ }
        }
        private void Bench_Disk_SelectedIndexChanged(object sender, EventArgs e){
            select_disk = benchmarkDiskList[Bench_Disk.SelectedIndex].Trim().Replace("\\", string.Empty);
        }
        // TIMER
        // ======================================================================================================
        private async Task BenchTimerAsync()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //
            TSGetLangs g_lang = new TSGetLangs(Glow.lang_path);
            string elapsedTimeFormat = TS_String_Encoder(g_lang.TSReadLangs("BenchDisk", "bt_elapsed_time"));
            //
            try{
                while (loop_mode){
                    //
                    TimeSpan elapsed = stopwatch.Elapsed;
                    int fh_second = (int)elapsed.TotalSeconds % 60;
                    int fh_minute = (int)(elapsed.TotalMinutes % 60);
                    int fh_hour = (int)(elapsed.TotalHours);
                    //
                    global_timer = $"{elapsedTimeFormat} {fh_hour:D2}:{fh_minute:D2}:{fh_second:D2}";
                    //
                    await Task.Delay(1000);
                }
            }catch (Exception){ }
        }
        // START ENGINE
        // ======================================================================================================
        private void start_engine(){
            //
            loop_mode = true;
            Task start_disk_engine_x64 = new Task(disk_engine);
            start_disk_engine_x64.Start();
            // SELECT DRIVE
            bench_mode = true;
            string selectedDrive = benchmarkDiskList[Bench_Disk.SelectedIndex];
            // WARNING MESSAGE
            try{
                isBenchmarking = true;
                benchmarkTask = Task.Run(() => RunBenchmark(selectedDrive));
                Bench_Start.Enabled = false;
                Bench_Stop.Enabled = true;
                Bench_Disk.Enabled = false;
                Bench_Size.Enabled = false;
                Bench_SizeCustom.Enabled = false;
                Bench_Buffer.Enabled = false;
                Bench_SizeCustom.Enabled = false;
            }catch (Exception){ }
        }
        // GB TO BYTE
        static long GigabytesToBytes(double gigabytes){
            return (long)(gigabytes * 1024 * 1024 * 1024);
        }
        // KB TO BYTE
        static byte[] KilobytesToBytes(double kilobytes){
            return new byte[(long)(kilobytes * 1024)];
        }
        // UPDATE PROGRESS
        private void UpdateProgress(double progress){
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_title")), Application.ProductName) + " - " + global_timer + " - " + progress.ToString("0.00") + "%";
        }
        // DISK BENCHMARK
        // ======================================================================================================
        private void RunBenchmark(string selectedDrive){
            var timerTask = BenchTimerAsync();
            // FILE PATH
            benchmarkFilePath = Path.Combine(selectedDrive, "GlowBenchDiskTestFile_" + new Random().Next(1000, 9999) + ".glow");
            // BENCH SIZE
            long fileSizeInBytes = 0;
            int[] sizesInGB = { 12, 15, 20, 25, 32, 64, 128 }; // GB cinsinden boyutlar
            if (Bench_Size.SelectedIndex >= 0 && Bench_Size.SelectedIndex < sizesInGB.Length){
                fileSizeInBytes = GigabytesToBytes(sizesInGB[Bench_Size.SelectedIndex]);
            }
            else if (Bench_Size.SelectedIndex == 7){
                int _cs = Convert.ToInt32(Bench_SizeCustom.Text.Trim());
                fileSizeInBytes = GigabytesToBytes(_cs); // Özel boyut
            }
            // BENCH BUFFER POOL
            byte[] buffer = KilobytesToBytes(0);
            int[] bufferSizesInKB = { 64, 128, 256, 512, 1024, 4096, 8192 }; // KB cinsinden tampon boyutları
            if (Bench_Buffer.SelectedIndex >= 0 && Bench_Buffer.SelectedIndex < bufferSizesInKB.Length){
                global_buffer = bufferSizesInKB[Bench_Buffer.SelectedIndex];
            }
            buffer = KilobytesToBytes(global_buffer);
            try{
                // BENCHMARK SETTINGS
                using (FileStream fs = new FileStream(benchmarkFilePath, FileMode.Create, FileAccess.Write)){
                    long bytesWritten = 0;
                    while (bytesWritten < fileSizeInBytes && isBenchmarking){
                        int bufferSize = (int)Math.Min(buffer.Length, fileSizeInBytes - bytesWritten);
                        fs.Write(buffer, 0, bufferSize);
                        bytesWritten += bufferSize;
                        double progress = (double)bytesWritten / fileSizeInBytes * 100;
                        UpdateProgress(progress);
                        if (bench_mode == false){
                            fs.Close();
                            fs.Dispose();
                            break;
                        }
                    }
                }
                Bench_Stop.Enabled = false;
                // BENCHMARK MODE STATUS
                if (isBenchmarking){
                    // BENCHMARK STREAM
                    using (FileStream fs = new FileStream(benchmarkFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, global_buffer)){
                        long totalBytesRead = 0;
                        long fileSize = fs.Length;
                        int bytesRead;
                        while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0 && isBenchmarking){
                            totalBytesRead += bytesRead;
                            double progress = (double)totalBytesRead / fileSize * 100;
                            UpdateProgress(progress);
                            if (bench_mode == false){
                                fs.Close();
                                fs.Dispose();
                                break;
                            }
                        }
                    }
                    // TEST AFTER DELETE
                    if (File.Exists(benchmarkFilePath)){
                        File.Delete(benchmarkFilePath);
                        TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
                        Invoke(new Action(() => {
                            Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_title")), Application.ProductName);
                            bench_mode = false;
                            loop_mode = false;
                            TS_MessageBoxEngine.TS_MessageBox(this, 1, TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_result_success")));
                        }));
                    }
                    //
                    isBenchmarking = false;
                    Bench_Start.Enabled = true;
                    Bench_Stop.Enabled = false;
                    Bench_Disk.Enabled = true;
                    Bench_Size.Enabled = true;
                    Bench_SizeCustom.Enabled = true;
                    Bench_Buffer.Enabled = true;
                    Bench_SizeCustom.Enabled = true;
                }
            }catch (Exception){ }
        }
        // ======================================================================================================
        // DISK ENGINE
        private async void disk_engine(){
            ulong maxReadSpeed = 0;
            ulong maxWriteSpeed = 0;
            try{
                do{
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, DiskReadBytesPersec, DiskWriteBytesPersec FROM Win32_PerfFormattedData_PerfDisk_PhysicalDisk");
                    ManagementObjectCollection results = searcher.Get();
                    Dictionary<string, (ulong readSpeed, ulong writeSpeed)> diskData = new Dictionary<string, (ulong, ulong)>();
                    foreach (ManagementObject obj in results){
                        string diskName = (string)obj["Name"];
                        ulong diskReadSpeed = (ulong)obj["DiskReadBytesPersec"];
                        ulong diskWriteSpeed = (ulong)obj["DiskWriteBytesPersec"];
                        if (diskName.Trim() != "_Total"){
                            diskData[diskName] = (diskReadSpeed, diskWriteSpeed);
                        }
                    }
                    if (!string.IsNullOrEmpty(select_disk)){
                        var selectedDisks = diskData.Where(kvp => kvp.Key.EndsWith(select_disk));
                        foreach (var diskEntry in selectedDisks){
                            var disk = diskEntry.Value;
                            float diskReadSpeedMB = disk.readSpeed / (1024 * 1024);
                            float diskWriteSpeedMB = disk.writeSpeed / (1024 * 1024);
                            if (disk.readSpeed > maxReadSpeed){
                                maxReadSpeed = disk.readSpeed;
                            }
                            if (disk.writeSpeed > maxWriteSpeed){
                                maxWriteSpeed = disk.writeSpeed;
                            }
                            Bench_R_ReadSpeed_V.Text = $"{diskReadSpeedMB:F1} MB/s";
                            Bench_R_Max_ReadSpeed_V.Text = $"{maxReadSpeed / (1024 * 1024):F1} MB/s";
                            Bench_L_WriteSpeed_V.Text = $"{diskWriteSpeedMB:F1} MB/s";
                            Bench_L_Max_WriteSpeed_V.Text = $"{maxWriteSpeed / (1024 * 1024):F1} MB/s";
                            //Console.WriteLine($"{diskEntry.Key}: - {diskReadSpeedMB:F1} MB/s (R) - {diskWriteSpeedMB:F1} MB/s (W) ");
                        }
                    }
                    await Task.Delay(1000 - DateTime.Now.Millisecond);
                } while (loop_mode == true);
            }catch (Exception) { }
        }
        // STOP BENCHMARK
        // ======================================================================================================
        private void Bench_Stop_Click(object sender, EventArgs e){
            stop_engine();
        }
        private async void stop_engine(){
            bench_mode = false;
            loop_mode = false;
            isBenchmarking = false;
            if (benchmarkTask != null){
                await benchmarkTask;
            }
            Bench_Start.Enabled = true;
            Bench_Stop.Enabled = false;
            Bench_Disk.Enabled = true;
            Bench_Size.Enabled = true;
            Bench_SizeCustom.Enabled = true;
            Bench_Buffer.Enabled = true;
            Bench_SizeCustom.Enabled = true;
            //
            TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
            Bench_L_WriteSpeed_V.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_start_test_await"));
            Bench_L_Max_WriteSpeed_V.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_start_test_await"));
            Bench_R_ReadSpeed_V.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_start_test_await"));
            Bench_R_Max_ReadSpeed_V.Text = TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_start_test_await"));
            //
            if (File.Exists(benchmarkFilePath)){
                File.Delete(benchmarkFilePath);
                Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_title")), Application.ProductName);
                TS_MessageBoxEngine.TS_MessageBox(this, 1, TS_String_Encoder(software_lang.TSReadLangs("BenchDisk", "bd_result_exit")));
            }
        }
        // CUSTOM SIZE CHANGE
        // ======================================================================================================
        private void Bench_Size_SelectedIndexChanged(object sender, EventArgs e){
            if (Bench_Size.SelectedIndex == Bench_Size.Items.Count - 1){
                Bench_SizeCustom.Visible = true;
            }else{
                Bench_SizeCustom.Visible = false;
            }
        }
        // NUMERIC INPUT
        // ======================================================================================================
        private void Bench_SizeCustom_KeyPress(object sender, KeyPressEventArgs e){
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)){
                e.Handled = true;
            }
        }
        // EXIT STOP ENGINE
        // ======================================================================================================
        private void GlowBenchDisk_FormClosing(object sender, FormClosingEventArgs e){
            if (bench_mode == true && loop_mode == true){
                e.Cancel = true;
            }else{
                stop_engine();
            }
        }
    }
}