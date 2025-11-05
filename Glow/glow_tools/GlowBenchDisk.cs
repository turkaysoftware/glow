using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Management;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowBenchDisk : Form{
        // VARIABLES
        // ======================================================================================================
        private Task DISKBench_benchmarkTask;
        private bool DISKBench_isBenchmarking = false, DISKBench_speedMode = true, DISKBench_stopMode = false;
        private string DISKBench_benchmarkFilePath, DISKBench_selectDisk, DISKBench_globalTimer;
        private readonly int[] DISKBench_sizesInGB = { 1, 5, 10, 15, 20, 25, 32, 64, 128 }, DISKBench_bufferSizesInKB = { 64, 128, 256, 512, 1024, 2048, 4096 };
        private readonly List<string> DISKBench_benchmarkDiskList = new List<string>();
        private readonly List<double> DISKBench_benchmarkDiskListFreeSpace = new List<double>();
        private readonly List<string> DISKBench_benchmarkDiskListType = new List<string>();
        private readonly List<long> DISKBench_testSizes = new List<long>();
        public GlowBenchDisk() { InitializeComponent(); }
        // THEME MODE
        // ======================================================================================================
        public void Bench_disk_theme_settings(){
            try{
                TSThemeModeHelper.InitializeThemeForForm(this);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                BackPanel.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                //
                Bench_P1.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_P2.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_P3.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_P4.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_P5.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_P6.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                Bench_P7.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                //
                Bench_DiskSelector.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_DiskSelector_List.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_DiskSelector_List.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_DiskSelector_List.HoverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_DiskSelector_List.ButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_DiskSelector_List.ArrowColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_DiskSelector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_DiskSelector_List.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_DiskSelector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_DiskSelector_List.DisabledBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_DiskSelector_List.DisabledForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_DiskSelector_List.DisabledButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                //
                Bench_SizeSelector.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_SizeSelector_List.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_SizeSelector_List.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_SizeSelector_List.HoverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_SizeSelector_List.ButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_SizeSelector_List.ArrowColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_SizeSelector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_SizeSelector_List.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_SizeSelector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_SizeSelector_List.DisabledBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_SizeSelector_List.DisabledForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_SizeSelector_List.DisabledButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_SizeCustom.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_SizeCustom.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TextBoxFEColor");
                //
                Bench_BufferSelector.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_BufferSelector_List.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_BufferSelector_List.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_BufferSelector_List.HoverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_BufferSelector_List.ButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_BufferSelector_List.ArrowColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_BufferSelector_List.HoverButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_BufferSelector_List.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_BufferSelector_List.FocusedBorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_BufferSelector_List.DisabledBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_BufferSelector_List.DisabledForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_BufferSelector_List.DisabledButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                //
                Bench_L_WriteSpeed.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_L_WriteSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_R_ReadSpeed.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_R_ReadSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_L_Max_WriteSpeed.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_L_Max_WriteSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                Bench_R_Max_ReadSpeed.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_R_Max_ReadSpeed_V.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
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
                //
                TSImageRenderer(Bench_Start, GlowMain.theme == 1 ? Properties.Resources.ct_test_start_light : Properties.Resources.ct_test_start_dark, 18, ContentAlignment.MiddleRight);
                TSImageRenderer(Bench_Stop, GlowMain.theme == 1 ? Properties.Resources.ct_test_stop_light : Properties.Resources.ct_test_stop_dark, 18, ContentAlignment.MiddleRight);
                //
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                Text = string.Format(software_lang.TSReadLangs("BenchDisk", "bd_title"), Application.ProductName);
                Bench_DiskSelector.Text = software_lang.TSReadLangs("BenchDisk", "bd_select_disk");
                Bench_SizeSelector.Text = software_lang.TSReadLangs("BenchDisk", "bd_test_size");
                Bench_BufferSelector.Text = software_lang.TSReadLangs("BenchDisk", "bd_test_buffer_size");
                //
                Bench_L_WriteSpeed.Text = software_lang.TSReadLangs("BenchDisk", "bd_time_write");
                Bench_L_Max_WriteSpeed.Text = software_lang.TSReadLangs("BenchDisk", "bd_time_max_write");
                Bench_R_ReadSpeed.Text = software_lang.TSReadLangs("BenchDisk", "bd_time_read");
                Bench_R_Max_ReadSpeed.Text = software_lang.TSReadLangs("BenchDisk", "bd_time_max_read");
                //
                Bench_SizeSelector_List.Items[Bench_SizeSelector_List.Items.Count - 1] = software_lang.TSReadLangs("BenchDisk", "bd_test_size_custom");
                //
                Bench_Start.Text = " " + software_lang.TSReadLangs("BenchDisk", "bd_start");
                Bench_Stop.Text = " " + software_lang.TSReadLangs("BenchDisk", "bd_stop");
            }catch (Exception){ }
        }
        // LOAD
        // ======================================================================================================
        private void GlowBenchDisk_Load(object sender, EventArgs e){
            RefreshDriveList();
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            //
            foreach (int size in DISKBench_sizesInGB){
                Bench_SizeSelector_List.Items.Add(size + " GB");
            }
            Bench_SizeSelector_List.Items.Add(software_lang.TSReadLangs("BenchDisk", "bd_test_size_custom"));
            Bench_SizeSelector_List.SelectedIndex = 1;
            //
            DISKBench_testSizes.Clear();
            foreach (int size in DISKBench_sizesInGB){
                DISKBench_testSizes.Add(size);
            }
            //
            foreach (int bufferSize in DISKBench_bufferSizesInKB){
                Bench_BufferSelector_List.Items.Add(bufferSize + " KB");
            }
            Bench_BufferSelector_List.SelectedIndex = 4;
            //
            Bench_L_WriteSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_L_Max_WriteSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_R_ReadSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_R_Max_ReadSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            //
            Bench_disk_theme_settings();
            //
            try{
                Task start_disk_engine_x64 = Task.Run(() => { Disk_engine(); });
            }catch (Exception){ }
        }
        // DISK LIST
        // ======================================================================================================
        private void RefreshDriveList(){
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            Bench_DiskSelector_List.Items.Clear();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives){
                string driveInfo;
                if (string.IsNullOrWhiteSpace(drive.VolumeLabel)){
                    driveInfo = $"{software_lang.TSReadLangs("BenchDisk", "bd_select_local_disk")} ({drive.Name.Replace("\\", string.Empty)}) - {TS_FormatSize(drive.TotalSize)}";
                }else{
                    driveInfo = $"{drive.VolumeLabel} ({drive.Name.Replace("\\", string.Empty)}) - {TS_FormatSize(drive.TotalSize)}";
                }
                Bench_DiskSelector_List.Items.Add(driveInfo);
                DISKBench_benchmarkDiskList.Add(drive.Name);
                DISKBench_benchmarkDiskListFreeSpace.Add(drive.TotalFreeSpace / 1024 / 1024 / 1024);
                DISKBench_benchmarkDiskListType.Add(drive.DriveType.ToString().ToLower().Trim());
            }
            Bench_DiskSelector_List.SelectedIndex = 0;
        }
        // START BTN
        // ======================================================================================================
        private void Bench_Start_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                if (Bench_SizeSelector_List.SelectedIndex == Bench_SizeSelector_List.Items.Count - 1){
                    if (!string.IsNullOrWhiteSpace(Bench_SizeCustom.Text)){
                        if (Convert.ToDouble(Bench_SizeCustom.Text.Trim()) >= 10 && Convert.ToDouble(Bench_SizeCustom.Text.Trim()) <= 256){
                            if (DISKBench_benchmarkDiskListFreeSpace[Bench_DiskSelector_List.SelectedIndex] > Convert.ToInt32(Bench_SizeCustom.Text.Trim())){
                                Check_info_user_warning(Bench_DiskSelector_List.SelectedIndex);
                            }else{
                                TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("BenchDisk", "bd_low_space"));
                            }
                        }else{
                            TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("BenchDisk", "bd_space_req"));
                            Bench_SizeCustom.Text = string.Empty;
                            Bench_SizeCustom.Focus();
                        }
                    }
                }else{
                    if (DISKBench_benchmarkDiskListFreeSpace[Bench_DiskSelector_List.SelectedIndex] > DISKBench_testSizes[Bench_DiskSelector_List.SelectedIndex]){
                        Check_info_user_warning(Bench_DiskSelector_List.SelectedIndex);
                    }else{
                        TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("BenchDisk", "bd_low_space"));
                    }
                }
            }catch (Exception){ }
        }
        // CHECK DISK USER INFO
        // ======================================================================================================
        private void Check_info_user_warning(int info_mode){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
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
                string mode = DISKBench_benchmarkDiskListType[info_mode];
                string messageKey = warnings.ContainsKey(mode) ? warnings[mode].MessageKey : warnings["unknown"].MessageKey;
                bool startEngine = warnings.ContainsKey(mode) && warnings[mode].StartEngine;
                //
                string message = string.Format(software_lang.TSReadLangs("BenchDisk", messageKey), "\n\n", "\n\n", "\n", "\n\n");
                string caption = software_lang.TSReadLangs("BenchDisk", "bd_start_engine_disk") + " " + Bench_DiskSelector_List.SelectedItem.ToString().Trim();
                //
                success_warning = TS_MessageBoxEngine.TS_MessageBox(this, 6, message, caption);
                if (!startEngine){
                    success_warning = TS_MessageBoxEngine.TS_MessageBox(this, 1, message, caption);
                }
                //
                if (startEngine && success_warning == DialogResult.Yes){
                    Start_engine();
                }
            }catch (Exception){ }
        }
        private void Bench_DiskSelector_List_SelectedIndexChanged(object sender, EventArgs e){
            DISKBench_selectDisk = DISKBench_benchmarkDiskList[Bench_DiskSelector_List.SelectedIndex].Trim().Replace("\\", string.Empty);
        }
        // TIMER
        // ======================================================================================================
        private async Task BenchTimerAsync(){
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //
            TSGetLangs g_lang = new TSGetLangs(GlowMain.lang_path);
            string elapsedTimeFormat = g_lang.TSReadLangs("BenchDisk", "bt_elapsed_time");
            //
            try{
                while (GlowMain.DISKbenchMode){
                    TimeSpan elapsed = stopwatch.Elapsed;
                    int fh_second = (int)elapsed.TotalSeconds % 60;
                    int fh_minute = (int)(elapsed.TotalMinutes % 60);
                    int fh_hour = (int)(elapsed.TotalHours);
                    //
                    DISKBench_globalTimer = $"{elapsedTimeFormat} {fh_hour:D2}:{fh_minute:D2}:{fh_second:D2}";
                    //
                    await Task.Delay(1000);
                }
            }catch (Exception){ }
        }
        // START ENGINE
        // ======================================================================================================
        private void Start_engine(){
            GlowMain.DISKbenchMode = true;
            string selectedDrive = DISKBench_benchmarkDiskList[Bench_DiskSelector_List.SelectedIndex];
            // WARNING MESSAGE
            try{
                DISKBench_isBenchmarking = true;
                DISKBench_benchmarkTask = Task.Run(async () => await RunBenchmarkAsync(selectedDrive));
                Bench_Start.Enabled = false;
                Bench_Stop.Enabled = true;
                Bench_DiskSelector_List.Enabled = false;
                Bench_SizeSelector_List.Enabled = false;
                Bench_SizeCustom.Enabled = false;
                Bench_BufferSelector_List.Enabled = false;
                Bench_SizeCustom.Enabled = false;
                DISKBench_stopMode = false;
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
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            if (InvokeRequired){
                Invoke(new Action(() =>{
                    Text = string.Format(software_lang.TSReadLangs("BenchDisk", "bd_title"), Application.ProductName) + " - " + DISKBench_globalTimer + " - " + progress.ToString("0") + "%";
                }));
            }else{
                Text = string.Format(software_lang.TSReadLangs("BenchDisk", "bd_title"), Application.ProductName) + " - " + DISKBench_globalTimer + " - " + progress.ToString("0") + "%";
            }
        }
        // DISK BENCHMARK
        // ======================================================================================================
        private async Task RunBenchmarkAsync(string selectedDrive){
            DISKBench_benchmarkFilePath = Path.Combine(selectedDrive, "GlowBenchDiskTestFile_" + new Random().Next(1000, 9999) + ".glow");
            long fileSizeInBytes = 0;
            int global_buffer = 0;
            int lastIndex = DISKBench_sizesInGB.Length - 1;
            // SIZE SET
            if (Bench_SizeSelector_List.SelectedIndex >= 0 && Bench_SizeSelector_List.SelectedIndex < lastIndex){
                fileSizeInBytes = GigabytesToBytes(DISKBench_sizesInGB[Bench_SizeSelector_List.SelectedIndex]);
            }else if (Bench_SizeSelector_List.SelectedIndex == lastIndex){
                fileSizeInBytes = GigabytesToBytes(Convert.ToInt32(Bench_SizeCustom.Text.Trim()));
            }
            // BUFFER SET
            if (Bench_BufferSelector_List.SelectedIndex >= 0 && Bench_BufferSelector_List.SelectedIndex < DISKBench_bufferSizesInKB.Length){
                global_buffer = DISKBench_bufferSizesInKB[Bench_BufferSelector_List.SelectedIndex];
            }
            byte[] buffer = KilobytesToBytes(global_buffer);
            var timerTask = BenchTimerAsync();
            try{
                // WRITE
                Stopwatch swWrite = Stopwatch.StartNew();
                using (FileStream fs = new FileStream(DISKBench_benchmarkFilePath, FileMode.Create, FileAccess.Write, FileShare.None, buffer.Length, FileOptions.WriteThrough)){
                    long bytesWritten = 0;
                    double lastProgress = 0;
                    while (bytesWritten < fileSizeInBytes && DISKBench_isBenchmarking){
                        int bufferSize = (int)Math.Min(buffer.Length, fileSizeInBytes - bytesWritten);
                        fs.Write(buffer, 0, bufferSize);
                        bytesWritten += bufferSize;
                        double progress = (double)bytesWritten / fileSizeInBytes * 100;
                        if (progress - lastProgress >= 1){
                            UpdateProgress(progress);
                            lastProgress = progress;
                        }
                        if (!GlowMain.DISKbenchMode) break;
                    }
                    fs.Flush();
                }
                swWrite.Stop();
                double writeMBps = (fileSizeInBytes / (1024.0 * 1024.0)) / swWrite.Elapsed.TotalSeconds;
                await Task.Delay(1000);
                // READ
                Stopwatch swRead = Stopwatch.StartNew();
                long totalBytesRead = 0;
                double lastProgressRead = 0;
                using (FileStream fs = new FileStream(DISKBench_benchmarkFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, buffer.Length, FileOptions.SequentialScan)){
                    int bytesRead;
                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0 && DISKBench_isBenchmarking){
                        totalBytesRead += bytesRead;
                        double progress = (double)totalBytesRead / fileSizeInBytes * 100;
                        if (progress - lastProgressRead >= 1){
                            UpdateProgress(progress);
                            lastProgressRead = progress;
                        }
                        if (!GlowMain.DISKbenchMode) break;
                    }
                }
                swRead.Stop();
                double readMBps = (totalBytesRead / (1024.0 * 1024.0)) / swRead.Elapsed.TotalSeconds;
                if (File.Exists(DISKBench_benchmarkFilePath)){
                    File.Delete(DISKBench_benchmarkFilePath);
                    TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                    Invoke(new Action(() =>{
                        Text = string.Format(software_lang.TSReadLangs("BenchDisk", "bd_title"), Application.ProductName);
                        GlowMain.DISKbenchMode = false;
                        if (!DISKBench_stopMode){
                            TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BenchDisk", "bd_result_success"));
                        }else{
                            DISKBench_stopMode = false;
                            TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BenchDisk", "bd_result_exit"));
                        }
                    }));
                }
                //
                DISKBench_isBenchmarking = false;
                Bench_Start.Enabled = true;
                Bench_Stop.Enabled = false;
                Bench_DiskSelector_List.Enabled = true;
                Bench_SizeSelector_List.Enabled = true;
                Bench_SizeCustom.Enabled = true;
                Bench_BufferSelector_List.Enabled = true;
            }catch (Exception){
                DISKBench_isBenchmarking = false;
            }
        }
        // ======================================================================================================
        // DISK ENGINE
        private async void Disk_engine(){
            ulong maxReadSpeed = 0;
            ulong maxWriteSpeed = 0;
            try{
                do{
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, DiskReadBytesPersec, DiskWriteBytesPersec FROM Win32_PerfFormattedData_PerfDisk_PhysicalDisk");
                    ManagementObjectCollection results = searcher.Get();
                    Dictionary<string, (ulong readSpeed, ulong writeSpeed)> diskData = new Dictionary<string, (ulong, ulong)>();
                    foreach (ManagementObject obj in results.Cast<ManagementObject>()){
                        string diskName = (string)obj["Name"];
                        ulong diskReadSpeed = (ulong)obj["DiskReadBytesPersec"];
                        ulong diskWriteSpeed = (ulong)obj["DiskWriteBytesPersec"];
                        if (diskName.Trim() != "_Total"){
                            diskData[diskName] = (diskReadSpeed, diskWriteSpeed);
                        }
                    }
                    if (!string.IsNullOrEmpty(DISKBench_selectDisk)){
                        var selectedDisks = diskData.Where(kvp => kvp.Key.EndsWith(DISKBench_selectDisk));
                        foreach (var diskEntry in selectedDisks){
                            var (readSpeed, writeSpeed) = diskEntry.Value;
                            float diskReadSpeedMB = readSpeed / (1024 * 1024);
                            float diskWriteSpeedMB = writeSpeed / (1024 * 1024);
                            if (InvokeRequired){
                                Invoke(new Action(() => {
                                    if (readSpeed > maxReadSpeed) maxReadSpeed = readSpeed;
                                    if (writeSpeed > maxWriteSpeed) maxWriteSpeed = writeSpeed;
                                    Bench_R_ReadSpeed_V.Text = $"{diskReadSpeedMB:F1} MB/s";
                                    Bench_R_Max_ReadSpeed_V.Text = $"{maxReadSpeed / (1024 * 1024):F1} MB/s";
                                    Bench_L_WriteSpeed_V.Text = $"{diskWriteSpeedMB:F1} MB/s";
                                    Bench_L_Max_WriteSpeed_V.Text = $"{maxWriteSpeed / (1024 * 1024):F1} MB/s";
                                }));
                            }else{
                                if (readSpeed > maxReadSpeed) maxReadSpeed = readSpeed;
                                if (writeSpeed > maxWriteSpeed) maxWriteSpeed = writeSpeed;
                                Bench_R_ReadSpeed_V.Text = $"{diskReadSpeedMB:F1} MB/s";
                                Bench_R_Max_ReadSpeed_V.Text = $"{maxReadSpeed / (1024 * 1024):F1} MB/s";
                                Bench_L_WriteSpeed_V.Text = $"{diskWriteSpeedMB:F1} MB/s";
                                Bench_L_Max_WriteSpeed_V.Text = $"{maxWriteSpeed / (1024 * 1024):F1} MB/s";
                            }
                        }
                    }
                    await Task.Delay(1000 - DateTime.Now.Millisecond);
                } while (DISKBench_speedMode);
            }catch (Exception){ }
        }
        // STOP BENCHMARK
        // ======================================================================================================
        private void Bench_Stop_Click(object sender, EventArgs e){
            Stop_engine();
        }
        private async void Stop_engine(){
            GlowMain.DISKbenchMode = false;
            DISKBench_isBenchmarking = false;
            //
            DISKBench_stopMode = true;
            //
            if (DISKBench_benchmarkTask != null){
                await DISKBench_benchmarkTask;
            }
            Bench_Start.Enabled = true;
            Bench_Stop.Enabled = false;
            Bench_DiskSelector_List.Enabled = true;
            Bench_SizeSelector_List.Enabled = true;
            Bench_SizeCustom.Enabled = true;
            Bench_BufferSelector_List.Enabled = true;
            Bench_SizeCustom.Enabled = true;
            DISKBench_speedMode = false;
            //
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            Bench_L_WriteSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_L_Max_WriteSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_R_ReadSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_R_Max_ReadSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            //
            if (File.Exists(DISKBench_benchmarkFilePath)){
                File.Delete(DISKBench_benchmarkFilePath);
                Text = string.Format(software_lang.TSReadLangs("BenchDisk", "bd_title"), Application.ProductName);
                TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BenchDisk", "bd_result_exit"));
            }
        }
        // CUSTOM SIZE CHANGE
        // ======================================================================================================
        private void Bench_SizeSelector_List_SelectedIndexChanged(object sender, EventArgs e){
            if (Bench_SizeSelector_List.SelectedIndex == Bench_SizeSelector_List.Items.Count - 1){
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
            if (GlowMain.DISKbenchMode){
                e.Cancel = true;
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("GToolsMessage", "gtm_benchmark_disk_prs_msg"));
            }else{
                Stop_engine();
            }
        }
    }
}