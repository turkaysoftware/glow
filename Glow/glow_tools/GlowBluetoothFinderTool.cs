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
        public GlowBluetoothFinderTool(){ InitializeComponent(); }
        // TS BLUETOOTH FINDER CLASS
        // ======================================================================================================
        private readonly List<TSAdvancedBluetoothAdapterInfo> BluetoothAdaptersList = new List<TSAdvancedBluetoothAdapterInfo>();
        class TSAdvancedBluetoothAdapterInfo{
            public string AdapterName { get; set; }
            public int LmpVersion { get; set; } = -1;
            public string BluetoothVersion { get; set; }
            public string DriverVersion { get; set; }
            public string DriverDate { get; set; }
            public string Manufacturer { get; set; }
            public string HardwareId { get; set; }
            public static readonly string[] AllowedVendors = new[] {
                "intel", "qualcomm", "broadcom", "realtek",
                "mediatek", "azurewave", "marvell", "cypress", "killer"
            };
            public override string ToString() => AdapterName ?? "Unknown Adapter";
        }
        // LOAD
        // ======================================================================================================
        private void GlowBluetoothFinderTool_Load(object sender, EventArgs e){
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            try{
                BTFinder_Preloader();
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
                //
                if (!(BTSelector.SelectedItem is TSAdvancedBluetoothAdapterInfo selectedAdapter)) { return; }
                //
                BT_Adapter_V.Text = $"{selectedAdapter.AdapterName.Trim() ?? unknown_msg}";
                BT_Version_V.Text = $"{selectedAdapter.BluetoothVersion.Trim() ?? unknown_msg}";
                BT_LMPVersion_V.Text = $"{selectedAdapter.LmpVersion.ToString().Trim() ?? unknown_msg}";
                BT_DriverVersion_V.Text = $"{selectedAdapter.DriverVersion.Trim() ?? unknown_msg}";
                BT_DriverDate_V.Text = $"{selectedAdapter.DriverDate.Trim() ?? unknown_msg}";
                BT_Publisher_V.Text = $"{selectedAdapter.Manufacturer.Trim() ?? unknown_msg}";
                BT_HardwareID_V.Text = $"{selectedAdapter.HardwareId.Trim() ?? unknown_msg}";
            }catch (Exception){ }
        }
        // BT LOADER DYNAMIC SCRIPT
        // ======================================================================================================
        private async void LoadBluetoothAdapters(){
            BluetoothAdaptersList.Clear();
            BTSelector.Items.Clear();
            GlowMain.BTfinderMode = true;
            //
            string psCommand = @"
            $OutputEncoding = [Console]::OutputEncoding = [System.Text.UTF8Encoding]::new()
            $bluetoothDevices = Get-PnpDevice -Class 'Bluetooth'
            $result = @()
            foreach ($device in $bluetoothDevices) {
                $driverDateProperty = Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Device_DriverDate'
                if ($driverDateProperty) {
                    try {
                        $dt = [datetime]$driverDateProperty.Data
                        $formattedDate = $dt.ToString('dd.MM.yyyy')
                    } catch {
                        $formattedDate = $driverDateProperty.Data
                    }
                } else {
                    $formattedDate = ''
                }
                $obj = [PSCustomObject]@{
                    Name = $device.FriendlyName
                    Manufacturer = (Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Device_Manufacturer').Data
                    LmpVersion = (Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Bluetooth_RadioLmpVersion').Data
                    DriverVersion = (Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Device_DriverVersion').Data
                    DriverDate = $formattedDate
                    InstanceId = $device.InstanceId
                }
                $result += $obj
            }
            $result | ConvertTo-Json -Compress
            ";
            //
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            try{
                var psi = new ProcessStartInfo{
                    FileName = "powershell.exe",
                    Arguments = $"-NoProfile -ExecutionPolicy Bypass -Command \"{psCommand}\"",
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    StandardOutputEncoding = Encoding.UTF8
                };
                using (var parser_bt_info = new Process { StartInfo = psi })
                {
                    parser_bt_info.Start();
                    string output = await parser_bt_info.StandardOutput.ReadToEndAsync();
                    string error = await parser_bt_info.StandardError.ReadToEndAsync();
                    parser_bt_info.WaitForExit();
                    if (!string.IsNullOrWhiteSpace(error))
                        throw new Exception(error);
                    this.Invoke((Action)(() =>{
                        try{
                            var serializer = new JavaScriptSerializer();
                            var devices = serializer.Deserialize<List<Dictionary<string, object>>>(output);
                            foreach (var d in devices){
                                string adapterName = d.ContainsKey("Name") ? Convert.ToString(d["Name"]) : "";
                                if (string.IsNullOrWhiteSpace(adapterName))
                                    continue;
                                bool allowed = TSAdvancedBluetoothAdapterInfo.AllowedVendors.Any(v => adapterName.IndexOf(v, StringComparison.OrdinalIgnoreCase) >= 0);
                                if (!allowed)
                                    continue;
                                TSAdvancedBluetoothAdapterInfo adapter = new TSAdvancedBluetoothAdapterInfo{
                                    AdapterName = adapterName,
                                    Manufacturer = Convert.ToString(d["Manufacturer"]),
                                    DriverVersion = Convert.ToString(d["DriverVersion"]),
                                    DriverDate = Convert.ToString(d["DriverDate"]),
                                    HardwareId = Convert.ToString(d["InstanceId"])
                                };
                                if (int.TryParse(Convert.ToString(d["LmpVersion"]), out int lmp)){
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
                                }
                                BluetoothAdaptersList.Add(adapter);
                            }
                            Text = string.Format(software_lang.TSReadLangs("BluetoothFinderTool", "bft_title"), Application.ProductName);
                            BTSelector.Items.Clear();
                            foreach (var adapter in BluetoothAdaptersList)
                                BTSelector.Items.Add(adapter);
                            GlowMain.BTfinderMode = false;
                            if (BTSelector.Items.Count > 0){
                                BTSelector.DisplayMember = "AdapterName";
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
                var sb = new StringBuilder(512).AppendLine(header).AppendLine(Environment.NewLine + new string('-', lineLen) + Environment.NewLine);
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