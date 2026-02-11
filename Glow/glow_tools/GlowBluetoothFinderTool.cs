using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Windows.Forms;
// TS MODULES
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowBluetoothFinderTool : Form{
        public GlowBluetoothFinderTool() { InitializeComponent(); }
        // TS BLUETOOTH FINDER CLASS
        // ======================================================================================================
        private readonly List<TSAdvancedBluetoothAdapterInfo> BluetoothAdaptersList = new List<TSAdvancedBluetoothAdapterInfo>();
        class TSAdvancedBluetoothAdapterInfo{
            public static string TextUnknownAdapter { get; set; }
            public static string TextSuspiciousTag { get; set; }
            public static string TextVidVenTag { get; set; }
            public string AdapterName { get; set; }
            public int LmpVersion { get; set; } = -1;
            public string BluetoothVersion { get; set; }
            public string DriverVersion { get; set; }
            public string DriverDate { get; set; }
            public string Manufacturer { get; set; }
            public string HardwareId { get; set; }
            public string VendorId { get; set; }
            public string[] HardwareIds { get; set; } = Array.Empty<string>();
            public bool IsLikelyRadio { get; set; } = true;
            // Text visible to the user in the ComboBox
            public override string ToString(){
                string name = string.IsNullOrWhiteSpace(AdapterName) ? TextUnknownAdapter : AdapterName.Trim();
                // string vendor = string.IsNullOrWhiteSpace(VendorId) ? "" : $" ({TextVidVenTag}: {VendorId.Trim()})";
                string flag = IsLikelyRadio ? "" : $" [{TextSuspiciousTag}]";
                // Return Combobox Value
                return $"{name}{flag}";
                // return $"{name}{vendor}{flag}";
            }
        }
        // VENDORID MAP (VID/VEN) -> VENDOR NAME (FOR INFORMATIONAL PURPOSES, NOT MANDATORY)
        // ======================================================================================================
        private static readonly Dictionary<string, string> VendorIdMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase){
            ["8087"] = "Intel Corporation",
            ["0BDA"] = "Realtek Semiconductor Corp.",
            ["0A5C"] = "Broadcom Corp.",
            ["0E8D"] = "MediaTek Inc.",
            ["0A12"] = "Cambridge Silicon Radio Ltd (CSR)",
            ["04B4"] = "Cypress Semiconductor (Infineon/Cypress)",
            ["05AC"] = "Apple, Inc.",
            ["045E"] = "Microsoft Corp.",
            ["046D"] = "Logitech, Inc.",
            ["1A40"] = "TERASIC Technologies Inc.",
            ["1BCF"] = "Sunplus Technology Co., Ltd.",
            ["0930"] = "Toshiba Corp.",
            ["0489"] = "Foxconn / Hon Hai",
            ["13D3"] = "AzureWave Technologies",
            ["0955"] = "Qualcomm Atheros (Jiangsu Qinheng Microelectronics)",
            ["18D1"] = "Google, Inc.",
            ["041E"] = "Creative Technology Ltd.",
            ["138A"] = "Samsung Electronics Co., Ltd.",
            ["1912"] = "Ralink Technology Corp.",
            ["0CF3"] = "Qualcomm Atheros",
            ["10C4"] = "Silicon Labs",
            ["067B"] = "Prolific Technology Inc.",
            ["16C0"] = "Van Ooijen Technische Informatica",
            ["0403"] = "Future Technology Devices Intl Ltd (FTDI)"
        };
        // SAFE TEXT FALL BACK
        // ======================================================================================================
        private static string SafeTextFallBack(string s, string fallback) => string.IsNullOrWhiteSpace(s) ? fallback : s.Trim();
        // FILTERING CONNECTED DEVICES
        // ======================================================================================================
        private static bool IsRadioLikeInstanceId(string instanceId){
            if (string.IsNullOrWhiteSpace(instanceId)) return false;
            // Connected/enum devices
            if (instanceId.StartsWith("BTHENUM", StringComparison.OrdinalIgnoreCase)) return false;
            if (instanceId.StartsWith("BTHLEDEVICE", StringComparison.OrdinalIgnoreCase)) return false;
            if (instanceId.StartsWith("BluetoothDevice_", StringComparison.OrdinalIgnoreCase)) return false;
            // Windows virtual/stack devices
            if (instanceId.StartsWith("ROOT\\", StringComparison.OrdinalIgnoreCase)) return false;
            if (instanceId.StartsWith("BTH\\", StringComparison.OrdinalIgnoreCase)) return false;
            // Physical bus requirement
            bool physical = instanceId.StartsWith("USB\\", StringComparison.OrdinalIgnoreCase) || instanceId.StartsWith("PCI\\", StringComparison.OrdinalIgnoreCase) || instanceId.StartsWith("ACPI\\", StringComparison.OrdinalIgnoreCase);
            return physical;
        }
        // "RADIO POSSIBILITY" TAG (DOES NOT PREVENT LISTING)
        // ======================================================================================================
        private static bool LikelyRadioHeuristic(string instanceId, string vendorId, string[] hardwareIds, string name, string manufacturer){
            if (!string.IsNullOrWhiteSpace(instanceId) && (instanceId.IndexOf(@"USB\VID_", StringComparison.OrdinalIgnoreCase) >= 0 || instanceId.IndexOf(@"PCI\VEN_", StringComparison.OrdinalIgnoreCase) >= 0)){
                return true;
            }
            if (!string.IsNullOrWhiteSpace(vendorId)){
                return true;
            }
            if (hardwareIds != null && hardwareIds.Length > 0){
                return true;
            }
            if (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(manufacturer)){
                return true;
            }
            return false;
        }
        // LOAD
        // ======================================================================================================
        private void GlowBluetoothFinderTool_Load(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            try{
                BTFinder_Preloader();
                //
                TSAdvancedBluetoothAdapterInfo.TextUnknownAdapter = software_lang.TSReadLangs("BluetoothFinderTool", "bft_unknown_adapter");
                TSAdvancedBluetoothAdapterInfo.TextSuspiciousTag = software_lang.TSReadLangs("BluetoothFinderTool", "bft_suspicious_tag");
                TSAdvancedBluetoothAdapterInfo.TextVidVenTag = software_lang.TSReadLangs("BluetoothFinderTool", "bft_vidven_tag");
                //
                Text = string.Format(software_lang.TSReadLangs("BluetoothFinderTool", "bft_title_load"), Application.ProductName);
                BT_Adapter_V.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_load");
                BT_Version_V.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_load");
                BT_LMPVersion_V.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_load");
                BT_DriverVersion_V.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_load");
                BT_DriverDate_V.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_load");
                BT_Publisher_V.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_load");
                BT_HardwareID_V.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_load");
                //
                BT_Adapter.Focus();
                LoadBluetoothAdapters();
            }catch (Exception){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, software_lang.TSReadLangs("BluetoothFinderTool", "bft_loader_failed"));
            }
        }
        // DYNAMIC THEME VOID
        // ======================================================================================================
        public void BTFinder_Preloader(){
            try{
                TSThemeModeHelper.InitializeThemeForForm(this);
                //
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                //
                BTSelector.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                BTSelector.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                BTSelector.HoverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                BTSelector.ButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
                BTSelector.ArrowColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                BTSelector.HoverButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
                BTSelector.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                BTSelector.FocusedBorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBorderColor");
                BTSelector.DisabledBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                BTSelector.DisabledForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                BTSelector.DisabledButtonColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor2");
                BTSelector.HoverForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxFEColor");
                BTSelector.SelectedBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                BTSelector.SelectedForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "SelectBoxBGColor");
                //
                foreach (Control ui_buttons in BackPanel.Controls){
                    if (ui_buttons is Button bt_finder_btn){
                        bt_finder_btn.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                        bt_finder_btn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                        bt_finder_btn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                        bt_finder_btn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor");
                        bt_finder_btn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColorHover");
                    }
                }
                //
                foreach (Control ui_panels in BackPanel.Controls){
                    if (ui_panels is Panel bt_finder_panel){
                        bt_finder_panel.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "PageContainerBGAndPageContentTotalColors");
                    }
                }
                //
                Label[] left_labels = { BT_Adapter, BT_Version, BT_LMPVersion, BT_DriverVersion, BT_DriverDate, BT_Publisher, BT_HardwareID };
                Label[] right_labels = { BT_Adapter_V, BT_Version_V, BT_LMPVersion_V, BT_DriverVersion_V, BT_DriverDate_V, BT_Publisher_V, BT_HardwareID_V };
                left_labels.ToList().ForEach(l => l.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentLabelLeft"));
                right_labels.ToList().ForEach(l => l.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentColor"));
                //
                TSImageRenderer(BTCopyInfoBtn, GlowMain.theme == 1 ? Properties.Resources.ct_copy_mc_light : Properties.Resources.ct_copy_mc_dark, 18, ContentAlignment.MiddleRight);
                //
                // ======================================================================================================
                // TEXTS
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                BT_Adapter.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_adapter");
                BT_Version.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_bt_version");
                BT_LMPVersion.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_lmp_version");
                BT_DriverVersion.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_driver_version");
                BT_DriverDate.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_driver_date");
                BT_Publisher.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_publisher");
                BT_HardwareID.Text = software_lang.TSReadLangs("BluetoothFinderTool", "bft_hwid");
                BTCopyInfoBtn.Text = " " + software_lang.TSReadLangs("BluetoothFinderTool", "bft_copy_info_btn");
            }catch (Exception){ }
        }
        // BT ADAPTER CHANGER
        // ======================================================================================================
        private void BTSelector_SelectedIndexChanged(object sender, EventArgs e){
            try{
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                string unknown_msg = software_lang.TSReadLangs("BluetoothFinderTool", "bft_unknown");
                if (!(BTSelector.SelectedItem is TSAdvancedBluetoothAdapterInfo selectedAdapter)) { return; }
                BT_Adapter_V.Text = SafeTextFallBack(selectedAdapter.AdapterName, unknown_msg);
                BT_Version_V.Text = SafeTextFallBack(selectedAdapter.BluetoothVersion, unknown_msg);
                BT_LMPVersion_V.Text = selectedAdapter.LmpVersion >= 0 ? selectedAdapter.LmpVersion.ToString() : unknown_msg;
                BT_DriverVersion_V.Text = SafeTextFallBack(selectedAdapter.DriverVersion, unknown_msg);
                BT_DriverDate_V.Text = SafeTextFallBack(selectedAdapter.DriverDate, unknown_msg);
                // If there is no Manufacturer in the Publisher field, the brand read from VendorIdMap can be displayed
                string publisher = selectedAdapter.Manufacturer;
                if (string.IsNullOrWhiteSpace(publisher) && !string.IsNullOrWhiteSpace(selectedAdapter.VendorId) && VendorIdMap.TryGetValue(selectedAdapter.VendorId.Trim(), out var mapped)){
                    publisher = mapped;
                }
                BT_Publisher_V.Text = SafeTextFallBack(publisher, unknown_msg);
                BT_HardwareID_V.Text = SafeTextFallBack(selectedAdapter.HardwareId, unknown_msg);
            }catch (Exception) { }
        }
        // BT LOADER DYNAMIC SCRIPT
        // ======================================================================================================
        private async void LoadBluetoothAdapters(){
            BluetoothAdaptersList.Clear();
            BTSelector.Items.Clear();
            GlowMain.BTfinderMode = true;
            // PowerShell: Bluetooth class -> manage peripheral devices -> remove radios/adapters
            string psCommand = @"
                $OutputEncoding = [Console]::OutputEncoding = [System.Text.UTF8Encoding]::new()

                $bluetooth = Get-PnpDevice -Class 'Bluetooth' -ErrorAction SilentlyContinue

                $radios = $bluetooth | Where-Object {
                    $_.InstanceId -and
                    ($_.InstanceId -notlike 'BTHENUM*') -and
                    ($_.InstanceId -notlike 'BTHLEDEVICE*') -and
                    ($_.InstanceId -notlike 'BluetoothDevice_*')
                }

                $result = @()

                foreach ($device in $radios) {

                    $m = (Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Device_Manufacturer' -ErrorAction SilentlyContinue).Data
                    $lmp = (Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Bluetooth_RadioLmpVersion' -ErrorAction SilentlyContinue).Data
                    $drvVer = (Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Device_DriverVersion' -ErrorAction SilentlyContinue).Data

                    $driverDateProperty = Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Device_DriverDate' -ErrorAction SilentlyContinue
                    if ($driverDateProperty -and $driverDateProperty.Data) {
                        try { $formattedDate = ([datetime]$driverDateProperty.Data).ToString('dd.MM.yyyy') }
                        catch { $formattedDate = [string]$driverDateProperty.Data }
                    } else {
                        $formattedDate = ''
                    }

                    $hwids = (Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Device_HardwareIds' -ErrorAction SilentlyContinue).Data
                    if ($hwids -isnot [System.Array]) { $hwids = @($hwids) }

                    $vendorId = ''
                    foreach ($h in $hwids) {
                        if ($h -match 'VID_([0-9A-Fa-f]{4})') { $vendorId = $matches[1].ToUpper(); break }
                        if ($h -match 'VEN_([0-9A-Fa-f]{4})') { $vendorId = $matches[1].ToUpper(); break }
                    }

                    $obj = [PSCustomObject]@{
                        Name          = $device.FriendlyName
                        Manufacturer  = [string]$m
                        LmpVersion    = [string]$lmp
                        DriverVersion = [string]$drvVer
                        DriverDate    = [string]$formattedDate
                        InstanceId    = [string]$device.InstanceId
                        VendorId      = [string]$vendorId
                        HardwareIds   = $hwids
                    }

                    $result += $obj
                }

                $result | ConvertTo-Json -Compress
                ";
            //
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            //
            try{
                var psi_bt = new ProcessStartInfo{
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{psCommand}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8
                };
                using (var parser_bt_info = new Process { StartInfo = psi_bt }){
                    parser_bt_info.Start();
                    string output = await parser_bt_info.StandardOutput.ReadToEndAsync();
                    string error = await parser_bt_info.StandardError.ReadToEndAsync();
                    parser_bt_info.WaitForExit();
                    if (!string.IsNullOrWhiteSpace(error)){
                        throw new Exception(error);
                    }
                    this.Invoke((Action)(() =>{
                        try{
                            var serializer = new JavaScriptSerializer();
                            object parsed = serializer.DeserializeObject(output);
                            List<Dictionary<string, object>> devices;
                            if (parsed is object[] arr){
                                devices = arr.OfType<Dictionary<string, object>>().ToList();
                            }else if (parsed is Dictionary<string, object> single){
                                devices = new List<Dictionary<string, object>> { single };
                            }else{
                                devices = new List<Dictionary<string, object>>();
                            }
                            foreach (var d in devices){
                                string adapterName = d.ContainsKey("Name") ? Convert.ToString(d["Name"]) : "";
                                string manufacturer = d.ContainsKey("Manufacturer") ? Convert.ToString(d["Manufacturer"]) : "";
                                string instanceId = d.ContainsKey("InstanceId") ? Convert.ToString(d["InstanceId"]) : "";
                                string vendorId = d.ContainsKey("VendorId") ? Convert.ToString(d["VendorId"]) : "";
                                // HardwareIds array parse
                                string[] hardwareIds = Array.Empty<string>();
                                if (d.ContainsKey("HardwareIds") && d["HardwareIds"] != null){
                                    if (d["HardwareIds"] is object[] hwArr){
                                        hardwareIds = hwArr.Select(x => Convert.ToString(x)).Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
                                    }else{
                                        string one = Convert.ToString(d["HardwareIds"]);
                                        if (!string.IsNullOrWhiteSpace(one)){
                                            hardwareIds = new[] { one };
                                        }
                                    }
                                }
                                // Peripheral devices (do not list connected devices)
                                if (!IsRadioLikeInstanceId(instanceId)){
                                    continue;
                                }
                                // "Radio possibility" tag (does not prevent listing)
                                bool likelyRadio = LikelyRadioHeuristic(instanceId, vendorId, hardwareIds, adapterName, manufacturer);
                                // If AdapterName is empty, let's show the user at least something
                                if (string.IsNullOrWhiteSpace(adapterName)){
                                    if (!string.IsNullOrWhiteSpace(manufacturer)){
                                        adapterName = manufacturer;
                                    }else if (!string.IsNullOrWhiteSpace(vendorId) && VendorIdMap.TryGetValue(vendorId.Trim(), out var mapped)){
                                        adapterName = mapped;
                                    }else{
                                        adapterName = software_lang.TSReadLangs("BluetoothFinderTool", "bft_default_radio_name");
                                    }
                                }
                                TSAdvancedBluetoothAdapterInfo adapter = new TSAdvancedBluetoothAdapterInfo{
                                    AdapterName = adapterName,
                                    Manufacturer = manufacturer,
                                    VendorId = vendorId,
                                    HardwareIds = hardwareIds,
                                    IsLikelyRadio = likelyRadio,
                                    DriverVersion = d.ContainsKey("DriverVersion") ? Convert.ToString(d["DriverVersion"]) : "",
                                    DriverDate = d.ContainsKey("DriverDate") ? Convert.ToString(d["DriverDate"]) : "",
                                    HardwareId = instanceId
                                };
                                if (int.TryParse(Convert.ToString(d.ContainsKey("LmpVersion") ? d["LmpVersion"] : ""), out int lmp)){
                                    adapter.LmpVersion = lmp;
                                    switch (lmp){
                                        case 16: adapter.BluetoothVersion = "6.2"; break;
                                        case 15: adapter.BluetoothVersion = "6.1"; break;
                                        case 14: adapter.BluetoothVersion = "6.0"; break;
                                        case 13: adapter.BluetoothVersion = "5.4"; break;
                                        case 12: adapter.BluetoothVersion = "5.3"; break;
                                        case 11: adapter.BluetoothVersion = "5.2"; break;
                                        case 10: adapter.BluetoothVersion = "5.1"; break;
                                        case 9: adapter.BluetoothVersion = "5.0"; break;
                                        case 8: adapter.BluetoothVersion = "4.2"; break;
                                        case 7: adapter.BluetoothVersion = "4.1"; break;
                                        case 6: adapter.BluetoothVersion = "4.0"; break;
                                        case 5: adapter.BluetoothVersion = "3.0 / HS"; break;
                                        case 4: adapter.BluetoothVersion = "2.1 / EDR"; break;
                                        case 3: adapter.BluetoothVersion = "2.0 / EDR"; break;
                                        case 2: adapter.BluetoothVersion = "1.2"; break;
                                        case 1: adapter.BluetoothVersion = "1.1"; break;
                                        case 0: adapter.BluetoothVersion = "1.0b"; break;
                                        default:
                                            adapter.BluetoothVersion = software_lang.TSReadLangs("BluetoothFinderTool", "bft_lmp_unknown");
                                            break;
                                    }
                                }else{
                                    adapter.LmpVersion = -1;
                                    adapter.BluetoothVersion = software_lang.TSReadLangs("BluetoothFinderTool", "bft_lmp_unknown");
                                }
                                BluetoothAdaptersList.Add(adapter);
                            }
                            //
                            Text = string.Format(software_lang.TSReadLangs("BluetoothFinderTool", "bft_title"), Application.ProductName);
                            BTSelector.Items.Clear();
                            foreach (var adapter in BluetoothAdaptersList){
                                BTSelector.Items.Add(adapter);
                            }
                            GlowMain.BTfinderMode = false;
                            if (BTSelector.Items.Count > 0){
                                BTSelector.SelectedIndex = 0;
                                BTSelector.Enabled = true;
                                BTCopyInfoBtn.Enabled = true;
                                TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BluetoothFinderTool", "bft_bt_finder_success"));
                            }else{
                                TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BluetoothFinderTool", "bft_bt_finder_failed"));
                            }
                        }catch (Exception ex){
                            GlowMain.BTfinderMode = false;
                            Text = string.Format(software_lang.TSReadLangs("BluetoothFinderTool", "bft_title"), Application.ProductName);
                            TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("BluetoothFinderTool", "bft_bt_finder_parse_error"), "\n\n", ex.Message));
                        }
                    }));
                }
            }catch (Exception ex){
                GlowMain.BTfinderMode = false;
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("BluetoothFinderTool", "bft_bt_finder_error"), "\n\n", ex.Message));
            }
        }
        // COPY BT INFO
        // ======================================================================================================
        private void BTCopyInfoBtn_Click(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            try{
                var pairs = new (Label Desc, Label Value)[]{
                    (BT_Adapter, BT_Adapter_V),
                    (BT_Version, BT_Version_V),
                    (BT_LMPVersion, BT_LMPVersion_V),
                    (BT_DriverVersion, BT_DriverVersion_V),
                    (BT_DriverDate, BT_DriverDate_V),
                    (BT_Publisher, BT_Publisher_V),
                    (BT_HardwareID, BT_HardwareID_V)
                };
                int maxDesc = pairs.Max(p => (p.Desc.Text ?? "").Length);
                string header = string.Format(software_lang.TSReadLangs("BluetoothFinderTool", "bfy_copy_head_text"), Application.ProductName);
                int lineLen = Math.Max(header.Length, pairs.Max(p => (p.Desc.Text ?? "").PadRight(maxDesc + 5).Length + (p.Value?.Text ?? "").Length));
                var sb = new StringBuilder(768).AppendLine(header).AppendLine(Environment.NewLine + new string('-', lineLen) + Environment.NewLine);
                foreach (var (d, v) in pairs){
                    sb.AppendLine($"{(d.Text ?? "").PadRight(maxDesc + 5)}{v?.Text ?? ""}");
                }
                Clipboard.SetText(sb.ToString());
                TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("BluetoothFinderTool", "bft_copy_sucess"));
            }catch (Exception ex){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("BluetoothFinderTool", "bft_copy_failed"), "\n\n", ex.Message));
            }
        }
        // CHECK PROCESS STATUS TO EXIT
        // ======================================================================================================
        private void GlowBluetoothFinderTool_FormClosing(object sender, FormClosingEventArgs e){
            if (GlowMain.BTfinderMode){
                TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
                TS_MessageBoxEngine.TS_MessageBox(this, 1, software_lang.TSReadLangs("GToolsMessage", "gtm_bt_finder_prs_msg"));
                e.Cancel = true;
            }
        }
    }
}