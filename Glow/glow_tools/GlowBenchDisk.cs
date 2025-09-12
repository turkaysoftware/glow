﻿using System;
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
        private Task benchmarkTask;
        private bool isBenchmarking = false;
        string benchmarkFilePath;
        readonly int[] sizesInGB = { 1, 5, 10, 15, 20, 25, 32, 64, 128 };
        readonly int[] bufferSizesInKB = { 64, 128, 256, 512, 1024, 2048, 4096 };
        //
        bool bench_mode = false;
        bool loop_mode = true;
        string select_disk;
        string global_timer;
        //
        readonly List<string> benchmarkDiskList = new List<string>();
        readonly List<double> benchmarkDiskListFreeSpace = new List<double>();
        readonly List<string> benchmarkDiskListType = new List<string>();
        readonly List<long> testSizes = new List<long>();
        public GlowBenchDisk() { InitializeComponent(); }
        // THEME MODE
        // ======================================================================================================
        public void Bench_disk_theme_settings(){
            try{
                TSSetWindowTheme(Handle, GlowMain.theme);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                //
                Bench_P1.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_P2.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_P3.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_P4.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_P5.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_P6.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                Bench_P7.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                //
                Bench_Label_Disk.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_Disk.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_Disk.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_Disk.HoverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_Disk.ButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
                Bench_Disk.HoverButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
                Bench_Disk.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_Disk.FocusedBorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_Disk.DisabledBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_Disk.DisabledForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_Disk.DisabledButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
                //
                Bench_Label_Size.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_Size.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_Size.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_Size.HoverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_Size.ButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
                Bench_Size.HoverButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
                Bench_Size.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_Size.FocusedBorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_Size.DisabledBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_Size.DisabledForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_Size.DisabledButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
                Bench_SizeCustom.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TextBoxBGColor");
                Bench_SizeCustom.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "TextBoxFEColor");
                //
                Bench_Label_Buffer.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                Bench_Buffer.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_Buffer.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_Buffer.HoverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_Buffer.ButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
                Bench_Buffer.HoverButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
                Bench_Buffer.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_Buffer.FocusedBorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                Bench_Buffer.DisabledBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                Bench_Buffer.DisabledForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                Bench_Buffer.DisabledButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
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
                TSImageRenderer(Bench_Start, GlowMain.theme == 1 ? Properties.Resources.ct_test_start_light : Properties.Resources.ct_test_start_dark, 22, ContentAlignment.MiddleRight);
                TSImageRenderer(Bench_Stop, GlowMain.theme == 1 ? Properties.Resources.ct_test_stop_light : Properties.Resources.ct_test_stop_dark, 22, ContentAlignment.MiddleRight);
                //
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                Text = string.Format(software_lang.TSReadLangs("BenchDisk", "bd_title"), Application.ProductName);
                Bench_Label_Disk.Text = software_lang.TSReadLangs("BenchDisk", "bd_select_disk");
                Bench_Label_Size.Text = software_lang.TSReadLangs("BenchDisk", "bd_test_size");
                Bench_Label_Buffer.Text = software_lang.TSReadLangs("BenchDisk", "bd_test_buffer_size");
                //
                Bench_L_WriteSpeed.Text = software_lang.TSReadLangs("BenchDisk", "bd_time_write");
                Bench_L_Max_WriteSpeed.Text = software_lang.TSReadLangs("BenchDisk", "bd_time_max_write");
                Bench_R_ReadSpeed.Text = software_lang.TSReadLangs("BenchDisk", "bd_time_read");
                Bench_R_Max_ReadSpeed.Text = software_lang.TSReadLangs("BenchDisk", "bd_time_max_read");
                //
                Bench_Size.Items[Bench_Size.Items.Count - 1] = software_lang.TSReadLangs("BenchDisk", "bd_test_size_custom");
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
            foreach (int size in sizesInGB){
                Bench_Size.Items.Add(size + " GB");
            }
            Bench_Size.Items.Add(software_lang.TSReadLangs("BenchDisk", "bd_test_size_custom"));
            Bench_Size.SelectedIndex = 2;
            //
            testSizes.Clear();
            foreach (int size in sizesInGB){
                testSizes.Add(size);
            }
            //
            foreach (int bufferSize in bufferSizesInKB){
                Bench_Buffer.Items.Add(bufferSize + " KB");
            }
            Bench_Buffer.SelectedIndex = 4;
            //
            Bench_L_WriteSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_L_Max_WriteSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_R_ReadSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_R_Max_ReadSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            //
            Bench_disk_theme_settings();
        }
        // DISK LIST
        // ======================================================================================================
        private void RefreshDriveList(){
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            Bench_Disk.Items.Clear();
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives){
                string driveInfo;
                if (drive.VolumeLabel.ToLower().Trim() == "" || drive.VolumeLabel == string.Empty){
                    driveInfo = $"{software_lang.TSReadLangs("BenchDisk", "bd_select_local_disk")} ({drive.Name.Replace("\\", string.Empty)}) - {TS_FormatSize(drive.TotalSize)}";
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
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                if (Bench_Size.SelectedIndex == Bench_Size.Items.Count - 1){
                    if (Bench_SizeCustom.Text.Trim() != "" || Bench_SizeCustom.Text.Trim() != string.Empty){
                        if (Convert.ToDouble(Bench_SizeCustom.Text.Trim()) >= 10 && Convert.ToDouble(Bench_SizeCustom.Text.Trim()) <= 256){
                            if (benchmarkDiskListFreeSpace[Bench_Disk.SelectedIndex] > Convert.ToInt32(Bench_SizeCustom.Text.Trim())){
                                Check_info_user_warning(Bench_Disk.SelectedIndex);
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
                    if (benchmarkDiskListFreeSpace[Bench_Disk.SelectedIndex] > testSizes[Bench_Disk.SelectedIndex]){
                        Check_info_user_warning(Bench_Disk.SelectedIndex);
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
                string mode = benchmarkDiskListType[info_mode];
                string messageKey = warnings.ContainsKey(mode) ? warnings[mode].MessageKey : warnings["unknown"].MessageKey;
                bool startEngine = warnings.ContainsKey(mode) && warnings[mode].StartEngine;
                //
                string message = string.Format(software_lang.TSReadLangs("BenchDisk", messageKey), "\n\n", "\n\n", "\n", "\n\n");
                string caption = software_lang.TSReadLangs("BenchDisk", "bd_start_engine_disk") + " " + Bench_Disk.SelectedItem.ToString().Trim();
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
        private void Bench_Disk_SelectedIndexChanged(object sender, EventArgs e)
        {
            select_disk = benchmarkDiskList[Bench_Disk.SelectedIndex].Trim().Replace("\\", string.Empty);
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
                while (loop_mode){
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
        private void Start_engine(){
            loop_mode = true;
            Task start_disk_engine_x64 = new Task(Disk_engine);
            start_disk_engine_x64.Start();
            // SELECT DRIVE
            bench_mode = true;
            string selectedDrive = benchmarkDiskList[Bench_Disk.SelectedIndex];
            // WARNING MESSAGE
            try{
                isBenchmarking = true;
                benchmarkTask = Task.Run(async () => await RunBenchmarkAsync(selectedDrive));
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
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            if (InvokeRequired){
                Invoke(new Action(() =>{
                    Text = string.Format(software_lang.TSReadLangs("BenchDisk", "bd_title"), Application.ProductName) + " - " + global_timer + " - " + progress.ToString("0") + "%";
                }));
            }else{
                Text = string.Format(software_lang.TSReadLangs("BenchDisk", "bd_title"), Application.ProductName) + " - " + global_timer + " - " + progress.ToString("0") + "%";
            }
        }
        // DISK BENCHMARK
        // ======================================================================================================
        private async Task RunBenchmarkAsync(string selectedDrive){
            benchmarkFilePath = Path.Combine(selectedDrive, "GlowBenchDiskTestFile_" + new Random().Next(1000, 9999) + ".glow");
            long fileSizeInBytes = 0;
            int global_buffer = 0;
            int lastIndex = sizesInGB.Length - 1;
            // SIZE SET
            if (Bench_Size.SelectedIndex >= 0 && Bench_Size.SelectedIndex < lastIndex){
                fileSizeInBytes = GigabytesToBytes(sizesInGB[Bench_Size.SelectedIndex]);
            }else if (Bench_Size.SelectedIndex == lastIndex){
                fileSizeInBytes = GigabytesToBytes(Convert.ToInt32(Bench_SizeCustom.Text.Trim()));
            }
            // BUFFER SET
            if (Bench_Buffer.SelectedIndex >= 0 && Bench_Buffer.SelectedIndex < bufferSizesInKB.Length){
                global_buffer = bufferSizesInKB[Bench_Buffer.SelectedIndex];
            }
            byte[] buffer = KilobytesToBytes(global_buffer);
            var timerTask = BenchTimerAsync();
            try{
                // WRITE
                Stopwatch swWrite = Stopwatch.StartNew();
                using (FileStream fs = new FileStream(benchmarkFilePath, FileMode.Create, FileAccess.Write, FileShare.None, buffer.Length, FileOptions.WriteThrough)){
                    long bytesWritten = 0;
                    double lastProgress = 0;
                    while (bytesWritten < fileSizeInBytes && isBenchmarking){
                        int bufferSize = (int)Math.Min(buffer.Length, fileSizeInBytes - bytesWritten);
                        fs.Write(buffer, 0, bufferSize);
                        bytesWritten += bufferSize;
                        double progress = (double)bytesWritten / fileSizeInBytes * 100;
                        if (progress - lastProgress >= 1){
                            UpdateProgress(progress);
                            lastProgress = progress;
                        }
                        if (!bench_mode) break;
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
                using (FileStream fs = new FileStream(benchmarkFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, buffer.Length, FileOptions.SequentialScan)){
                    int bytesRead;
                    while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0 && isBenchmarking){
                        totalBytesRead += bytesRead;
                        double progress = (double)totalBytesRead / fileSizeInBytes * 100;
                        if (progress - lastProgressRead >= 1){
                            UpdateProgress(progress);
                            lastProgressRead = progress;
                        }
                        if (!bench_mode) break;
                    }
                }
                swRead.Stop();
                double readMBps = (totalBytesRead / (1024.0 * 1024.0)) / swRead.Elapsed.TotalSeconds;
                if (File.Exists(benchmarkFilePath)){
                    File.Delete(benchmarkFilePath);
                    TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                    Invoke(new Action(() =>{
                        Text = string.Format(software_lang.TSReadLangs("BenchDisk", "bd_title"), Application.ProductName);
                        bench_mode = false;
                        loop_mode = false;
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BenchDisk", "bd_result_success"));
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
            }catch (Exception){
                isBenchmarking = false;
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
                    if (!string.IsNullOrEmpty(select_disk)){
                        var selectedDisks = diskData.Where(kvp => kvp.Key.EndsWith(select_disk));
                        foreach (var diskEntry in selectedDisks){
                            var (readSpeed, writeSpeed) = diskEntry.Value;
                            float diskReadSpeedMB = readSpeed / (1024 * 1024);
                            float diskWriteSpeedMB = writeSpeed / (1024 * 1024);
                            if (InvokeRequired){
                                Invoke(new Action(() =>{
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
                } while (loop_mode == true);
            } catch (Exception){ }
        }
        // STOP BENCHMARK
        // ======================================================================================================
        private void Bench_Stop_Click(object sender, EventArgs e){
            Stop_engine();
        }
        private async void Stop_engine(){
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
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            Bench_L_WriteSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_L_Max_WriteSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_R_ReadSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            Bench_R_Max_ReadSpeed_V.Text = software_lang.TSReadLangs("BenchDisk", "bd_start_test_await");
            //
            if (File.Exists(benchmarkFilePath)){
                File.Delete(benchmarkFilePath);
                Text = string.Format(software_lang.TSReadLangs("BenchDisk", "bd_title"), Application.ProductName);
                TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BenchDisk", "bd_result_exit"));
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
                Stop_engine();
            }
        }
    }
}