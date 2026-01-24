using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
// TS Modules
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowSystemIDGenerator : Form{
        private readonly ConcurrentDictionary<string, ConcurrentBag<string>> hwBag = new ConcurrentDictionary<string, ConcurrentBag<string>>();
        public static string __system_processor_id;
        // CONSTRUCTION
        // ======================================================================================================
        public GlowSystemIDGenerator(){
            InitializeComponent();
            //
            hwBag.TryAdd("CPU", new ConcurrentBag<string>());
            hwBag.TryAdd("MOTHERBOARD", new ConcurrentBag<string>());
            hwBag.TryAdd("BIOS", new ConcurrentBag<string>());
            hwBag.TryAdd("MEMORY", new ConcurrentBag<string>());
            hwBag.TryAdd("GPU", new ConcurrentBag<string>());
            hwBag.TryAdd("MONITOR", new ConcurrentBag<string>());
            hwBag.TryAdd("STORAGE", new ConcurrentBag<string>());
            hwBag.TryAdd("BATTERY", new ConcurrentBag<string>());
            //
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, DGV_MainTable, new object[] { true });
            //
            DGV_MainTable.RowTemplate.Height = (int)(24 * this.DeviceDpi / 96f);
            DGV_MainTable.Columns.Add("HardwareName", "HName");
            DGV_MainTable.Columns.Add("HardwareResult", "HResult");
            DGV_MainTable.Columns[0].Width = (int)(325 * this.DeviceDpi / 96f);
            DGV_MainTable.AllowUserToResizeColumns = false;
            foreach (DataGridViewColumn columnPadding in DGV_MainTable.Columns){
                int scaledPadding = (int)(3 * this.DeviceDpi / 96f);
                columnPadding.DefaultCellStyle.Padding = new Padding(scaledPadding, 0, 0, 0);
            }
            foreach (DataGridViewColumn A_Column in DGV_MainTable.Columns) { A_Column.SortMode = DataGridViewColumnSortMode.NotSortable; }
        }
        // BYPASS TABLE CLICK
        // ======================================================================================================
        private void DGV_MainTable_SelectionChanged(object sender, EventArgs e) => DGV_MainTable.ClearSelection();
        // LOAD
        // ======================================================================================================
        private void GlowSystemIDGenerator_Load(object sender, EventArgs e){
            SIG_Preloader();
            //
            Btn_Save.Enabled = false;
            Btn_Compare.Enabled = false;
            //
            DGV_MainTable.Rows.Clear();
            foreach (var key in hwBag.Keys){
                hwBag[key] = new ConcurrentBag<string>();
            }
            //
            var tasks_list = new List<Task>{
                Task.Run(() => SID_Processor()),
                Task.Run(() => SID_Motherboard()),
                Task.Run(() => SID_BIOS()),
                Task.Run(() => SID_Memory()),
                Task.Run(() => SID_GPU()),
                Task.Run(() => SID_Monitor()),
                Task.Run(() => SID_Storage()),
                Task.Run(() => SID_Battery())
            };
            //
            SortGridNatural();
            DGV_MainTable.ClearSelection();
            Btn_Save.Enabled = true;
            Btn_Compare.Enabled = true;
        }
        // PRELOAD
        // ======================================================================================================
        public void SIG_Preloader(){
            try{
                TSThemeModeHelper.InitializeThemeForForm(this);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                //
                Label_Info.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuButtonHoverAndMouseDownColor");
                Label_Info.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft");
                //
                DGV_MainTable.BackgroundColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "LeftMenuButtonHoverAndMouseDownColor");
                DGV_MainTable.GridColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridColor");
                DGV_MainTable.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridBGColor");
                DGV_MainTable.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridFEColor");
                DGV_MainTable.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridAlternatingColor");
                DGV_MainTable.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                DGV_MainTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                DGV_MainTable.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageFE");
                DGV_MainTable.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                DGV_MainTable.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageFE");
                //
                foreach (Control ui_buttons in this.Controls){
                    if (ui_buttons is Button ui_button){
                        ui_button.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                        ui_button.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                        ui_button.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                        ui_button.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                        ui_button.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColorHover");
                    }
                }
                //
                TSImageRenderer(Btn_Save, GlowMain.theme == 1 ? Properties.Resources.ct_export_light : Properties.Resources.ct_export_dark, 18, ContentAlignment.MiddleRight);
                TSImageRenderer(Btn_Compare, GlowMain.theme == 1 ? Properties.Resources.ct_compare_light : Properties.Resources.ct_compare_dark, 18, ContentAlignment.MiddleRight);
                // ======================================================================================================
                // TEXTS
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                Text = string.Format(software_lang.TSReadLangs("SystemIDTool", "sit_title"), Application.ProductName);
                Label_Info.Text = software_lang.TSReadLangs("SystemIDTool", "sit_info");
                //
                DGV_MainTable.Columns[0].HeaderText = software_lang.TSReadLangs("SystemIDTool", "sit_column_1");
                DGV_MainTable.Columns[1].HeaderText = software_lang.TSReadLangs("SystemIDTool", "sit_column_2");
                //
                Btn_Save.Text = " " + software_lang.TSReadLangs("SystemIDTool", "sit_save_btn");
                Btn_Compare.Text = " " + software_lang.TSReadLangs("SystemIDTool", "sit_compare_btn");
            }catch (Exception){ }
        }
        // HARDWARE ID SAVE BTN
        // ======================================================================================================
        private void Btn_Save_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                using (var folder_browser = new FolderBrowserDialog()){
                    folder_browser.Description = software_lang.TSReadLangs("SystemIDTool", "sit_save_info");
                    folder_browser.ShowNewFolderButton = false;
                    folder_browser.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    if (folder_browser.ShowDialog() == DialogResult.OK){
                        string save_file_name = string.Format(software_lang.TSReadLangs("SystemIDTool", "sit_file_name"), Application.ProductName);
                        string sha_name = string.Format(software_lang.TSReadLangs("SystemIDTool", "sit_sha_name") + " - " + DateTime.Now.ToString("dd.MM.yyyy - HH.mm.ss"), Application.ProductName);
                        string snapshot_name = string.Format(software_lang.TSReadLangs("SystemIDTool", "sit_snapshot_name") + " - " + DateTime.Now.ToString("dd.MM.yyyy - HH.mm.ss"), Application.ProductName);
                        string targetDir = Path.Combine(folder_browser.SelectedPath, save_file_name);
                        //
                        if (!Directory.Exists(targetDir)){
                            Directory.CreateDirectory(targetDir);
                        }
                        //
                        string shaPath = Path.Combine(targetDir, sha_name + ".sha256");
                        string glowPath = Path.Combine(targetDir, snapshot_name + ".glow");
                        try{
                            File.WriteAllText(shaPath, $"{software_lang.TSReadLangs("SystemIDTool", "sit_sha_file_tag")}\n{new string('-', 65)}\n{BuildHardwareHash(BuildHardwareSnapshot())}");
                            File.WriteAllText(glowPath, $"{software_lang.TSReadLangs("SystemIDTool", "sit_snapshot_file_tag")}\n{new string('-', 65)}\n{BuildHardwareSnapshot()}");
                        }catch (Exception ex){
                            TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("SystemIDTool", "sit_after_failed"), ex.Message));
                        }
                        //
                        var latest_message = TS_MessageBoxEngine.TS_MessageBox(this, 5, software_lang.TSReadLangs("SystemIDTool", "sit_after_message"));
                        if (latest_message == DialogResult.Yes){
                            Process.Start("explorer.exe", targetDir);
                        }
                    }
                }
            }catch (Exception){ }
        }
        // BUILD HARDWARE SNAPSHOTS
        // ======================================================================================================
        private string BuildHardwareSnapshot(){
            var order = new[] { "CPU", "MOTHERBOARD", "BIOS", "MEMORY", "GPU", "MONITOR", "STORAGE", "BATTERY" };
            var sb = new StringBuilder();
            foreach (var key in order){
                if (!hwBag.TryGetValue(key, out var bag) || bag.Count == 0) continue;
                sb.Append($"|{key}:");
                foreach (var val in bag.Distinct().OrderBy(x => x))
                    sb.Append($"{val.Trim()};");
            }
            return sb.ToString();
        }
        // BUILD HARDWARE HASH
        // ======================================================================================================
        private string BuildHardwareHash(string snapshot){
            using (var sha256 = SHA256.Create()){
                var bytes = Encoding.UTF8.GetBytes(snapshot ?? string.Empty);
                var hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
        }
        // COMPARE HARDWARE BTN
        // ======================================================================================================
        private void Btn_Compare_Click(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                using (OpenFileDialog get_compare_file = new OpenFileDialog()){
                    get_compare_file.Title = string.Format(software_lang.TSReadLangs("SystemIDTool", "sit_compare_filter_title"), Application.ProductName);
                    get_compare_file.Filter = string.Format(software_lang.TSReadLangs("SystemIDTool", "sit_compare_filter_name"), Application.ProductName ,"|*.sha256;*.glow");
                    get_compare_file.Multiselect = false;
                    get_compare_file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    if (get_compare_file.ShowDialog() != DialogResult.OK){
                        return;
                    }
                    string[] get_lines = File.ReadAllLines(get_compare_file.FileName);
                    if (get_lines.Length < 3){
                        TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("SystemIDTool", "sit_compare_file_unvalid"));
                        return;
                    }
                    string get_t_line = get_lines[2].Trim();
                    // SHA-256 Control
                    if (Regex.IsMatch(get_t_line, @"^[0-9a-fA-F]{64}$")){
                        if (get_t_line.Equals(BuildHardwareHash(BuildHardwareSnapshot()), StringComparison.OrdinalIgnoreCase)){
                            TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("SystemIDTool", "sit_compare_sha_success"));
                        }else{
                            TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("SystemIDTool", "sit_compare_sha_failed"), "*.glow"));
                        }
                        return;
                    }
                    // Snapshot Control
                    if (!get_t_line.StartsWith("|")){
                        TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("SystemIDTool", "sit_compare_snapshot_unvalid"));
                        return;
                    }
                    var validSnapshot = ParseHardwareSnapshot(get_t_line);
                    if (validSnapshot.Count == 0){
                        TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("SystemIDTool", "sit_compare_snapshot_unvalid"));
                        return;
                    }
                    TS_MessageBoxEngine.TS_MessageBox(this, 1, CompareSnapshots(validSnapshot));
                }
            }catch (Exception){ }
        }
        // PARSE HARDWARE SNAPSHOTS
        // ======================================================================================================
        private Dictionary<string, List<string>> ParseHardwareSnapshot(string snap_text){
            var end_result = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            if (string.IsNullOrWhiteSpace(snap_text)) return end_result;
            snap_text = snap_text.Trim();
            var parts_mode = snap_text.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var part_single in parts_mode){
                var idx = part_single.IndexOf(':');
                if (idx <= 0 || idx >= part_single.Length - 1) continue;
                var key = part_single.Substring(0, idx).Trim();
                var valuesString = part_single.Substring(idx + 1);
                var values = valuesString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(v => v.Trim()).Where(v => !string.IsNullOrEmpty(v)).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(v => v, StringComparer.OrdinalIgnoreCase).ToList();
                if (values.Count > 0)
                    end_result[key] = values;
            }
            return end_result;
        }
        // COMPARE SNAPSHOTS
        // ======================================================================================================
        private string CompareSnapshots(Dictionary<string, List<string>> oldSnap){
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            try{
                var sb_comp = new StringBuilder();
                //
                Dictionary<string, string> keyMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase){
                    { "CPU", software_lang.TSReadLangs("SystemIDTool", "sit_processor_ui")},
                    { "MOTHERBOARD", software_lang.TSReadLangs("SystemIDTool", "sit_motherboard_ui") },
                    { "BIOS", software_lang.TSReadLangs("SystemIDTool", "sit_bios_ui") },
                    { "MEMORY",software_lang.TSReadLangs("SystemIDTool", "sit_memory_ui") },
                    { "GPU", software_lang.TSReadLangs("SystemIDTool", "sit_gpu_ui") },
                    { "MONITOR", software_lang.TSReadLangs("SystemIDTool", "sit_monitor_ui") },
                    { "STORAGE",software_lang.TSReadLangs("SystemIDTool", "sit_storage_ui") },
                    { "BATTERY", software_lang.TSReadLangs("SystemIDTool", "sit_battery_ui") }
                };
                string LocalizeKey(string key) { return keyMap.TryGetValue(key, out string local) ? local : key; }
                //
                var allCats = new HashSet<string>(hwBag.Keys, StringComparer.OrdinalIgnoreCase);
                if (oldSnap != null){
                    foreach (var k in oldSnap.Keys) allCats.Add(k);
                }
                List<string> PipeMode(IEnumerable<string> seq) => seq.Select(x => x.Trim()).Where(x => x.Length > 0).Distinct(StringComparer.OrdinalIgnoreCase).OrderBy(x => x, StringComparer.OrdinalIgnoreCase).ToList();
                string NormalizationLocal(string s){
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        return string.Empty;
                    }
                    var parts = PipeMode(s.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                    return string.Join(";", parts);
                }
                //
                foreach (var key in allCats.OrderBy(x => x)){
                    var current = hwBag.ContainsKey(key) ? PipeMode(hwBag[key].Select(x => NormalizationLocal(x))) : new List<string>();
                    oldSnap.TryGetValue(key, out var oldList);
                    oldList = oldList != null ? PipeMode(oldList.Select(x => NormalizationLocal(x))) : new List<string>();
                    var removed = oldList.Except(current, StringComparer.OrdinalIgnoreCase).ToList();
                    var added = current.Except(oldList, StringComparer.OrdinalIgnoreCase).ToList();
                    //
                    if (removed.Count == 0 && added.Count == 0){
                        continue;
                    }
                    //
                    sb_comp.AppendLine($"[ {LocalizeKey(key)} ]");
                    sb_comp.AppendLine(new string('-', 35));
                    if (removed.Count > 0){
                        sb_comp.AppendLine("\n" + "- " + software_lang.TSReadLangs("SystemIDTool", "sit_removed") + "\n");
                        foreach (var r in removed) sb_comp.AppendLine('\u25CF' + " " + r);
                    }
                    if (added.Count > 0){
                        sb_comp.AppendLine("\n" + "+ " + software_lang.TSReadLangs("SystemIDTool", "sit_added") + "\n");
                        foreach (var a in added) sb_comp.AppendLine('\u25CF' + " " + a);
                    }
                    sb_comp.AppendLine();
                }
                return sb_comp.Length == 0 ? software_lang.TSReadLangs("SystemIDTool", "sit_compare_snapshot_success") : sb_comp.ToString().TrimEnd();
            }catch (Exception ex){
                return string.Format(software_lang.TSReadLangs("SystemIDTool", "sit_compare_snapshot_failed"), ex.Message);
            }
        }
        // SAFE UI THREAD
        // ======================================================================================================
        private void AddRowSafe(string nameColumn, string valueColumn){
            if (DGV_MainTable.InvokeRequired){
                DGV_MainTable.Invoke(new Action(() => DGV_MainTable.Rows.Add(nameColumn, valueColumn)));
            }else{
                DGV_MainTable.Rows.Add(nameColumn, valueColumn);
            }
        }
        // PROCESSOR SERIAL INFO
        // ======================================================================================================
        private void SID_Processor(){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                if (!string.IsNullOrEmpty(__system_processor_id)){
                    hwBag["CPU"].Add(__system_processor_id.Trim());
                    AddRowSafe(software_lang.TSReadLangs("SystemIDTool", "sit_processor"), __system_processor_id.Trim());
                }
            }catch (Exception){ }
        }
        // MOTHERBOARD SERIAL INFO
        // ======================================================================================================
        private void SID_Motherboard(){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                using (var search_motherboard = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard")){
                    foreach (ManagementObject query_motherboard in search_motherboard.Get().Cast<ManagementObject>()){
                        string motherboard_serial = (string)query_motherboard["SerialNumber"];
                        if (!string.IsNullOrEmpty(motherboard_serial)){
                            hwBag["MOTHERBOARD"].Add(motherboard_serial.Trim());
                            AddRowSafe(software_lang.TSReadLangs("SystemIDTool", "sit_motherboard"), motherboard_serial.Trim());
                        }
                    }
                }
            }catch (Exception){ }
        }
        // BIOS SERIAL INFO
        // ======================================================================================================
        private void SID_BIOS(){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                using (var search_bios = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BIOS")){
                    foreach (ManagementObject query_bios in search_bios.Get().Cast<ManagementObject>()){
                        string bios_serial = (string)query_bios["SerialNumber"];
                        if (!string.IsNullOrEmpty(bios_serial)){
                            hwBag["BIOS"].Add(bios_serial.Trim());
                            AddRowSafe(software_lang.TSReadLangs("SystemIDTool", "sit_bios"), bios_serial.Trim());
                        }
                    }
                }
            }catch (Exception){ }
        }
        // MEMORY SERIAL INFO
        // ======================================================================================================
        private void SID_Memory(){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                using (var search_memory = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory")){
                    foreach (ManagementObject query_memory in search_memory.Get().Cast<ManagementObject>()){
                        string memory_serial = (string)query_memory["SerialNumber"];
                        if (!string.IsNullOrEmpty(memory_serial)){
                            hwBag["MEMORY"].Add(memory_serial.Trim());
                            AddRowSafe(software_lang.TSReadLangs("SystemIDTool", "sit_memory"), memory_serial.Trim());
                        }
                    }
                }
            }catch (Exception){ }
        }
        // GPU PNP DEVICE ID INFO
        // ======================================================================================================
        private void SID_GPU(){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                using (var search_gpu = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_VideoController")){
                    foreach (ManagementObject query_gpu in search_gpu.Get().Cast<ManagementObject>()){
                        string gpu_pnp_device_id = (string)query_gpu["PNPDeviceID"];
                        if (!string.IsNullOrEmpty(gpu_pnp_device_id)){
                            hwBag["GPU"].Add(gpu_pnp_device_id.Trim());
                            AddRowSafe(software_lang.TSReadLangs("SystemIDTool", "sit_gpu"), gpu_pnp_device_id.Trim());
                        }
                    }
                }
            }catch (Exception){ }
        }
        // MONITOR SERIAL ID INFO
        // ======================================================================================================
        private void SID_Monitor(){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                using (var search_monitor = new ManagementObjectSearcher(@"root\WMI", "SELECT * FROM WmiMonitorID")){
                    foreach (ManagementObject query_monitor in search_monitor.Get().Cast<ManagementObject>()){
                        string monitor_serial = GlowMain.GetMonitorFromUShortArray((ushort[])query_monitor["SerialNumberID"]);
                        if (!string.IsNullOrEmpty(monitor_serial)) {
                            hwBag["MONITOR"].Add(monitor_serial.Trim());
                            AddRowSafe(software_lang.TSReadLangs("SystemIDTool", "sit_monitor"), monitor_serial.Trim());
                        }
                    }
                }
            }catch (Exception){ }
        }
        // STORAGE SERIAL INFO
        // ======================================================================================================
        private void SID_Storage(){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                using (var search_storage = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_DiskDrive")){
                    foreach (ManagementObject query_storage in search_storage.Get().Cast<ManagementObject>()){
                        string disk_serial = (string)query_storage["SerialNumber"];
                        if (!string.IsNullOrEmpty(disk_serial)){
                            hwBag["STORAGE"].Add(disk_serial.Trim());
                            AddRowSafe(software_lang.TSReadLangs("SystemIDTool", "sit_storage"), disk_serial.Trim());
                        }
                    }
                }
            }catch (Exception){ }
        }
        // BATTERY SERIAL INFO
        // ======================================================================================================
        private void SID_Battery(){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                using (var search_battery = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM BatteryStaticData")){
                    foreach (ManagementObject query_battery in search_battery.Get().Cast<ManagementObject>()){
                        string battery_serial = (string)query_battery["SerialNumber"];
                        if (!string.IsNullOrEmpty(battery_serial)){
                            hwBag["BATTERY"].Add(battery_serial.Trim());
                            AddRowSafe(software_lang.TSReadLangs("SystemIDTool", "sit_battery"), battery_serial.Trim());
                        }
                    }
                }
            }catch (Exception){ }
        }
        // NATURAL SORTING
        // ======================================================================================================
        private void SortGridNatural(){
            var rows = DGV_MainTable.Rows.Cast<DataGridViewRow>().Where(r => !r.IsNewRow).Select(r => new { Name = r.Cells[0].Value?.ToString(), Value = r.Cells[1].Value?.ToString() }).OrderBy(x => TSNaturalSortKey(x.Name, CultureInfo.CurrentCulture)).ToList();
            DGV_MainTable.Rows.Clear();
            foreach (var item in rows){
                DGV_MainTable.Rows.Add(item.Name, item.Value);
            }
        }
    }
}