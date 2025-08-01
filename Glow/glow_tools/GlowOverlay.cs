﻿using System;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Management;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.VisualBasic.Devices;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowOverlay : Form{
        // GLOBAL LANGS PATH
        // ======================================================================================================
        TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
        // DRAG MODES
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        // DRAG MODES
        bool overlay_loop = true;
        public GlowOverlay(){
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //
            Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width - 20, 20);
            Opacity = 0.7;
        }
        private void GlowOverlay_Load(object sender, EventArgs e){
            OVERLAY_CPU_V.Text = TS_String_Encoder(software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            OVERLAY_RAM_V.Text = TS_String_Encoder(software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            OVERLAY_DISK_V.Text = TS_String_Encoder(software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            OVERLAY_NETWORK_V.Text = TS_String_Encoder(software_lang.TSReadLangs("ScreenOverlayTool", "sot_loading"));
            // LOAD
            Task cpu_load = new Task(cpu_engine);
            cpu_load.Start();
            //
            Task ram_load = new Task(ram_engine);
            ram_load.Start();
            //
            Task disk_load = new Task(disk_engine);
            disk_load.Start();
            //
            Task network_load = new Task(network_engine);
            network_load.Start();
            //
            screen_overlay_settings();
        }
        // OVERLAY THEME
        // ======================================================================================================
        public void screen_overlay_settings(){
            BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "LeftMenuBGAndBorderColor");
            TransparencyKey = TS_ThemeEngine.ColorMode(Glow.theme, "LeftMenuBGAndBorderColor");
            //
            Color panelcolor = TS_ThemeEngine.ColorMode(Glow.theme, "LeftMenuButtonHoverAndMouseDownColor");
            Color textcolor = TS_ThemeEngine.ColorMode(Glow.theme, "LeftMenuButtonFEColor2");
            //
            OVERLAY_BGPanel.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "LeftMenuButtonAlphaColor");
            //
            OVERLAY_P1.BackColor = panelcolor;
            OVERLAY_P2.BackColor = panelcolor;
            OVERLAY_P3.BackColor = panelcolor;
            OVERLAY_P4.BackColor = panelcolor;
            OVERLAY_P5.BackColor = panelcolor;
            OVERLAY_P6.BackColor = panelcolor;
            OVERLAY_P7.BackColor = panelcolor;
            OVERLAY_P8.BackColor = panelcolor;
            //
            OVERLAY_CPU.ForeColor = textcolor;
            OVERLAY_RAM.ForeColor = textcolor;
            OVERLAY_DISK.ForeColor = textcolor;
            OVERLAY_NETWORK.ForeColor = textcolor;
            //
            OVERLAY_CPU_V.ForeColor = textcolor;
            OVERLAY_RAM_V.ForeColor = textcolor;
            OVERLAY_DISK_V.ForeColor = textcolor;
            OVERLAY_NETWORK_V.ForeColor = textcolor;
            //
            TSImageRenderer(CloseOverlayBtn, Glow.theme == 1 ? Properties.Resources.ct_clear_dark : Properties.Resources.ct_clear_light, 15, ContentAlignment.MiddleCenter);
            CloseOverlayBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "TSBT_CloseBG");
            CloseOverlayBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "TSBT_CloseBG");
            CloseOverlayBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "TSBT_CloseBGHover");
            CloseOverlayBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "TSBT_CloseBGHover");
            //
            Text = string.Format(TS_String_Encoder(software_lang.TSReadLangs("ScreenOverlayTool", "sot_title")), Application.ProductName);
            OVERLAY_CPU.Text = TS_String_Encoder(software_lang.TSReadLangs("ScreenOverlayTool", "sot_cpu"));
            OVERLAY_RAM.Text = TS_String_Encoder(software_lang.TSReadLangs("ScreenOverlayTool", "sot_ram"));
            OVERLAY_DISK.Text = TS_String_Encoder(software_lang.TSReadLangs("ScreenOverlayTool", "sot_disk"));
            OVERLAY_NETWORK.Text = TS_String_Encoder(software_lang.TSReadLangs("ScreenOverlayTool", "sot_net"));
        }
        // ======================================================================================================
        // CPU ENGINE
        private void cpu_engine(){
            try{
                ManagementObjectSearcher cpuSearcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, PercentProcessorTime FROM Win32_PerfFormattedData_PerfOS_Processor");
                do{
                    if (overlay_loop == false){ break; }
                    ManagementObjectCollection cpuResults = cpuSearcher.Get();
                    // Console.WriteLine("CPU Kullanımı:");
                    ulong totalCpuUsage = 0;
                    int cpuCount = 0;
                    foreach (ManagementObject obj in cpuResults){
                        string cpuName = (string)obj["Name"];
                        ulong cpuUsage = (ulong)obj["PercentProcessorTime"];
                        if (cpuName != "_Total"){
                            totalCpuUsage += cpuUsage;
                            cpuCount++;
                            // Console.WriteLine($"  CPU: {cpuName} - Kullanım: {cpuUsage}%");
                        }
                    }
                    if (cpuCount > 0){
                        float averageCpuUsage = (float)totalCpuUsage / cpuCount;
                        OVERLAY_CPU_V.Text = $"{averageCpuUsage:F2}%";
                    }
                    Thread.Sleep(750);
                }while (overlay_loop == true);
            }catch (Exception){ }
        }
        // RAM ENGINE
        // ======================================================================================================
        private void ram_engine(){
            try{
                ComputerInfo get_ram_info = new ComputerInfo();
                do{
                    if (overlay_loop == false){ break; }
                    ulong total_ram = ulong.Parse(get_ram_info.TotalPhysicalMemory.ToString());
                    ulong usable_ram = ulong.Parse(get_ram_info.AvailablePhysicalMemory.ToString());
                    double usage_ram_percentage = (TS_FormatSizeNoType(total_ram) - TS_FormatSizeNoType(usable_ram)) / TS_FormatSizeNoType(total_ram) * 100;
                    OVERLAY_RAM_V.Text = string.Format("{0:0.0} - {1} / {2}", TS_FormatSize(total_ram - usable_ram), string.Format("{0:0.0}%", usage_ram_percentage), TS_FormatSize(total_ram));
                    Thread.Sleep(750);
                }while (overlay_loop == true);
            }catch (Exception){ }
        }
        // DISK ENGINE
        // ======================================================================================================
        private void disk_engine(){
            try{
                do{
                    if (overlay_loop == false){ break; }
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT Name, DiskReadBytesPersec, DiskWriteBytesPersec FROM Win32_PerfFormattedData_PerfDisk_PhysicalDisk");
                    ManagementObjectCollection results = searcher.Get();
                    StringBuilder diskInformation = new StringBuilder();
                    Dictionary<string, (ulong readSpeed, ulong writeSpeed)> diskData = new Dictionary<string, (ulong, ulong)>();
                    foreach (ManagementObject obj in results){
                        string diskName = (string)obj["Name"];
                        ulong diskReadSpeed = (ulong)obj["DiskReadBytesPersec"];
                        ulong diskWriteSpeed = (ulong)obj["DiskWriteBytesPersec"];
                        if (diskName.Trim() != "_Total"){
                            diskData[diskName] = (diskReadSpeed, diskWriteSpeed);
                        }
                    }
                    var cDiskEntry = diskData.FirstOrDefault(kvp => kvp.Key.EndsWith(Glow.windows_disk.Replace("\\", string.Empty)));
                    if (!cDiskEntry.Equals(default(KeyValuePair<string, (ulong readSpeed, ulong writeSpeed)>))){
                        var cDisk = cDiskEntry.Value;
                        float cDiskReadSpeedMB = cDisk.readSpeed / (1024 * 1024);
                        float cDiskWriteSpeedMB = cDisk.writeSpeed / (1024 * 1024);
                        diskInformation.AppendLine($"{Glow.windows_disk.Replace("\\", string.Empty)} - {cDiskReadSpeedMB:F1} MB/s (R) - {cDiskWriteSpeedMB:F1} MB/s (W)");
                    }
                    var dDiskEntry = diskData.FirstOrDefault(kvp => kvp.Key.EndsWith("D:"));
                    if (!dDiskEntry.Equals(default(KeyValuePair<string, (ulong readSpeed, ulong writeSpeed)>))){
                        var dDisk = dDiskEntry.Value;
                        float dDiskReadSpeedMB = dDisk.readSpeed / (1024 * 1024);
                        float dDiskWriteSpeedMB = dDisk.writeSpeed / (1024 * 1024);
                        diskInformation.AppendLine($"D: - {dDiskReadSpeedMB:F1} MB/s (R) - {dDiskWriteSpeedMB:F1} MB/s (W)");
                    }
                    OVERLAY_DISK_V.Text = diskInformation.ToString();
                    Thread.Sleep(750);
                }while (overlay_loop == true);
            }catch (Exception){ }
        }
        // NETWORK MOVE
        // ======================================================================================================
        private void network_engine(){
            try{
                string activeAdapter = net_replacer(GetActiveNetworkAdapter());
                ManagementObjectSearcher searcher_net_speed = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PerfFormattedData_Tcpip_NetworkInterface");
                do{
                    if (overlay_loop == false){ break; }
                    foreach (ManagementObject search_active_speed in searcher_net_speed.Get()){
                        if (net_replacer(search_active_speed["Name"].ToString().Trim()) == activeAdapter){
                            ulong bytesReceived = (ulong)search_active_speed["BytesReceivedPerSec"];
                            ulong bytesSent = (ulong)search_active_speed["BytesSentPerSec"];
                            double mbpsReceived = Math.Round(bytesReceived * 8 / 1000000.0, 1);
                            double mbpsSent = Math.Round(bytesSent * 8 / 1000000.0, 1);
                            OVERLAY_NETWORK_V.Text = string.Format("{0}\n{1}", string.Format("{0:0.00} Mbps", TS_String_Encoder(software_lang.TSReadLangs("ScreenOverlayTool", "sot_net_download")) + " " + mbpsReceived), string.Format("{0:0.00} Mbps", TS_String_Encoder(software_lang.TSReadLangs("ScreenOverlayTool", "sot_net_upload")) + " " + mbpsSent));
                        }
                    }
                    Thread.Sleep(750);
                }while (overlay_loop == true);
            }catch (Exception){ }
        }
        private static string GetActiveNetworkAdapter(){
            try{
                ManagementObjectSearcher searcher_active_net = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_NetworkAdapter WHERE NetConnectionStatus=2");
                foreach (ManagementObject search_ip_enabled in searcher_active_net.Get()){
                    ManagementObjectSearcher configSearcher = new ManagementObjectSearcher("root\\CIMV2", $"SELECT * FROM Win32_NetworkAdapterConfiguration WHERE Index={search_ip_enabled["Index"]} AND IPEnabled=True");
                    foreach (ManagementObject configObj in configSearcher.Get()){
                        return search_ip_enabled["Name"].ToString().Trim();
                    }
                }
            }catch (Exception){ }
            return null;
        }
        // OVERLAY MOVE
        // ======================================================================================================
        private void OVERLAY_TopPanel_MouseDown(object sender, MouseEventArgs e){
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = Location;
        }
        private void OVERLAY_TopPanel_MouseMove(object sender, MouseEventArgs e){
            if (dragging){
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }
        private void OVERLAY_TopPanel_MouseUp(object sender, MouseEventArgs e){
            dragging = false;
        }
        // OVERLAY MOVE
        // ======================================================================================================
        // EXIT
        // ======================================================================================================
        private void CloseOverlayBtn_Click(object sender, EventArgs e){
            overlay_loop = false;
            Close();
        }
    }
}