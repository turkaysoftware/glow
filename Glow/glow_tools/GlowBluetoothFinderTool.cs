using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                // COLOR SETTINGS
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
                        bt_finder_btn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                        bt_finder_btn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                        bt_finder_btn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                        bt_finder_btn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
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
                right_labels.ToList().ForEach(l => l.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain"));
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
            //
            GlowMain.BTfinderMode = true;
            //
            string psCommand = @"
            $OutputEncoding = [Console]::OutputEncoding = [System.Text.UTF8Encoding]::new()
            $bluetoothDevices = Get-PnpDevice -Class 'Bluetooth'
            foreach ($device in $bluetoothDevices) {
                Write-Output $device.FriendlyName
                $manufacturerProperty = Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Device_Manufacturer'
                if ($manufacturerProperty) { Write-Output $manufacturerProperty.Data } else { Write-Output 'NoManufacturer' }
                $lmpVersionProperty = Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Bluetooth_RadioLmpVersion'
                if ($lmpVersionProperty) { Write-Output $lmpVersionProperty.Data } else { Write-Output '-1' }
                $driverVersionProperty = Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Device_DriverVersion'
                if ($driverVersionProperty) { Write-Output $driverVersionProperty.Data } else { Write-Output 'NoDriverVersion' }
                $driverDateProperty = Get-PnpDeviceProperty -InstanceId $device.InstanceId -KeyName 'DEVPKEY_Device_DriverDate'
                if ($driverDateProperty) { 
                    try {
                        $dt = [datetime]$driverDateProperty.Data
                        Write-Output ($dt.ToString('dd.MM.yyyy - HH:mm:ss'))
                    } catch {
                        Write-Output $driverDateProperty.Data
                    }
                } else { Write-Output 'NoDriverDate' }
                Write-Output $device.InstanceId
            }";
            //
            TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
            //
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
                //
                using (var parser_bt_info = new Process { StartInfo = psi }){
                    List<string> outputLines = new List<string>();
                    parser_bt_info.OutputDataReceived += (sender, e) =>{
                        if (!string.IsNullOrEmpty(e.Data)){
                            lock (outputLines){
                                outputLines.Add(e.Data);
                            }
                        }
                    };
                    //
                    parser_bt_info.Start();
                    parser_bt_info.BeginOutputReadLine();
                    parser_bt_info.BeginErrorReadLine();
                    //
                    await Task.Run(() => parser_bt_info.WaitForExit());
                    //
                    this.Invoke((Action)(() =>{
                        try{
                            TSAdvancedBluetoothAdapterInfo currentAdapter = null;
                            // 0 = AdapterName, 1 = Manufacturer, 2 = LmpVersion, 3 = DriverVersion, 4 = DriverDate, 5 = HardwareId
                            int state = 0;
                            //
                            foreach (var line in outputLines){
                                string trimmedLine = line.Trim();
                                if (string.IsNullOrWhiteSpace(trimmedLine)){ continue; }
                                //
                                if (state == 0){
                                    bool isAdapterName = TSAdvancedBluetoothAdapterInfo.AllowedVendors.Any(v => trimmedLine.IndexOf(v, StringComparison.OrdinalIgnoreCase) >= 0);
                                    if (isAdapterName){
                                        if (currentAdapter != null){
                                            BluetoothAdaptersList.Add(currentAdapter);
                                        }
                                        currentAdapter = new TSAdvancedBluetoothAdapterInfo{
                                            AdapterName = trimmedLine
                                        };
                                        state = 1;
                                    }
                                }else if (currentAdapter != null){
                                    switch (state){
                                        case 1:
                                            currentAdapter.Manufacturer = trimmedLine;
                                            state = 2;
                                            break;
                                        case 2:
                                            if (int.TryParse(trimmedLine, out int lmp)){
                                                currentAdapter.LmpVersion = lmp;
                                                string bluetoothVersion;
                                                switch (lmp){
                                                    case 15: bluetoothVersion = "6.1"; break;
                                                    case 14: bluetoothVersion = "6.0"; break;
                                                    case 13: bluetoothVersion = "5.4"; break;
                                                    case 12: bluetoothVersion = "5.3"; break;
                                                    case 11: bluetoothVersion = "5.2"; break;
                                                    case 10: bluetoothVersion = "5.1"; break;
                                                    case 9: bluetoothVersion = "5.0"; break;
                                                    case 8: bluetoothVersion = "4.2"; break;
                                                    case 7: bluetoothVersion = "4.1"; break;
                                                    case 6: bluetoothVersion = "4.0"; break;
                                                    case 5: bluetoothVersion = "3.0 / HS"; break;
                                                    case 4: bluetoothVersion = "2.1 / EDR"; break;
                                                    case 3: bluetoothVersion = "2.0 / EDR"; break;
                                                    case 2: bluetoothVersion = "1.2"; break;
                                                    case 1: bluetoothVersion = "1.1"; break;
                                                    case 0: bluetoothVersion = "1.0b"; break;
                                                    default: bluetoothVersion = software_lang.TSReadLangs("BluetoothFinderTool", "bft_lmp_unknown"); break;
                                                }
                                                currentAdapter.BluetoothVersion = bluetoothVersion;
                                            }
                                            state = 3;
                                            break;
                                        case 3:
                                            if (!trimmedLine.StartsWith("NoDriverVersion", StringComparison.OrdinalIgnoreCase)){
                                                currentAdapter.DriverVersion = trimmedLine;
                                            }
                                            state = 4;
                                            break;
                                        case 4:
                                            if (!trimmedLine.StartsWith("NoDriverDate", StringComparison.OrdinalIgnoreCase)){
                                                currentAdapter.DriverDate = trimmedLine;
                                            }
                                            state = 5;
                                            break;
                                        case 5:
                                            currentAdapter.HardwareId = trimmedLine;
                                            state = 0;
                                            break;
                                    }
                                }
                            }
                            //
                            if (currentAdapter != null){
                                BluetoothAdaptersList.Add(currentAdapter);
                            }
                            // CLEAR ALLOW VENDORS WIDHOUT
                            BluetoothAdaptersList.RemoveAll(adapter => !TSAdvancedBluetoothAdapterInfo.AllowedVendors.Any(v => adapter.AdapterName.IndexOf(v, StringComparison.OrdinalIgnoreCase) >= 0));
                            // SERIALIZE
                            Text = string.Format(software_lang.TSReadLangs("BluetoothFinderTool", "bft_title"), Application.ProductName);
                            BT_Adapter.Focus();
                            //
                            BTSelector.Items.Clear();
                            foreach (var adapter in BluetoothAdaptersList){
                                BTSelector.Items.Add(adapter);
                            }
                            //
                            GlowMain.BTfinderMode = false;
                            //
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
                var sb = new StringBuilder(512).AppendLine(header).AppendLine(new string('-', lineLen));
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