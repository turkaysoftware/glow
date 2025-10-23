using System;
using System.IO;
using System.Net;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Linq;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowDNSTestTool : Form{
        // VARIABLES
        // ======================================================================================================
        private readonly TSGetLangs software_lang = new TSGetLangs(GlowMain.lang_path);
        private string DNSTest_pingSendText, DNSTest_pingSendError;
        private List<DnsProvider> DNSTest_dnsProviders;
        public GlowDNSTestTool(){
            InitializeComponent();
            //
            DNSTable.Columns.Add("DNSProvider", "Provider");
            DNSTable.Columns.Add("DNSValue", "Value");
            //
            DNSTable.Columns[0].Width = (int)(175 * this.DeviceDpi / 96f);
            DNSTable.RowTemplate.Height = (int)(26 * this.DeviceDpi / 96f);
            foreach (DataGridViewColumn col in DNSTable.Columns){
                col.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewColumn columnPadding in DNSTable.Columns){
                int scaledPadding = (int)(3 * this.DeviceDpi / 96f);
                columnPadding.DefaultCellStyle.Padding = new Padding(scaledPadding, 0, 0, 0);
            }
            //
            DNS_TestStartBtn.Width = Btn_FLP.Width;
            DNS_CustomTestBtn.Width = Btn_FLP.Width;
            DNS_TestExportBtn.Width = Btn_FLP.Width;
            //
            InitDnsProviders();
        }
        // DNS Provider class
        // ======================================================================================================
        private class DnsProvider{
            public string Name { get; }
            public List<string> Addresses { get; }
            public int RowIndex { get; }
            public bool IsCompleted { get; set; } = false;
            public DnsProvider(string name, List<string> addresses, int rowIndex){
                Name = name;
                Addresses = addresses;
                RowIndex = rowIndex;
            }
        }
        private void InitDnsProviders(){
            DNSTest_dnsProviders = new List<DnsProvider>{
                new DnsProvider("AdGuard DNS",      new List<string>{ "94.140.14.14", "94.140.15.15" }, 0),
                new DnsProvider("Alternate DNS",    new List<string>{ "76.76.19.19", "76.223.122.150" }, 1),
                new DnsProvider("CloudFlare",       new List<string>{ "1.1.1.1", "1.0.0.1" }, 2),
                new DnsProvider("Comodo",           new List<string>{ "8.26.56.26", "8.20.247.20" }, 3),
                new DnsProvider("Control D",        new List<string>{ "76.76.2.0", "76.76.10.0" }, 4),
                new DnsProvider("DNS.WATCH",        new List<string>{ "84.200.69.80", "84.200.70.40" }, 5),
                new DnsProvider("Google",           new List<string>{ "8.8.8.8", "8.8.4.4" }, 6),
                new DnsProvider("Lumen DNS",        new List<string>{ "4.2.2.1", "4.2.2.2" }, 7),
                new DnsProvider("OpenDNS",          new List<string>{ "208.67.222.222", "208.67.220.220" }, 8),
                new DnsProvider("Quad9",            new List<string>{ "9.9.9.9", "149.112.112.112" }, 9),
                new DnsProvider("Yandex DNS",       new List<string>{ "77.88.8.8", "77.88.8.1" }, 10)
            };
        }
        // LOAD SETTINGS
        // ======================================================================================================
        public void Dns_test_settings(){
            try{
                BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "ContentPanelBGColor");
                //
                DNSTable.BackgroundColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridBGColor");
                DNSTable.GridColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridColor");
                DNSTable.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridBGColor");
                DNSTable.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridFEColor");
                DNSTable.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DataGridAlternatingColor");
                DNSTable.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                DNSTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                DNSTable.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageFE");
                DNSTable.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageBG");
                DNSTable.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "OSDAndServicesPageFE");
                //
                DNS_TestStartBtn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                DNS_TestStartBtn.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                DNS_TestStartBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                DNS_TestStartBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                DNS_TestStartBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                //
                DNS_CustomTestBtn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                DNS_CustomTestBtn.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                DNS_CustomTestBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                DNS_CustomTestBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                DNS_CustomTestBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                //
                DNS_TestExportBtn.BackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                DNS_TestExportBtn.ForeColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "DynamicThemeActiveBtnBG");
                DNS_TestExportBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                DNS_TestExportBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMain");
                DNS_TestExportBtn.FlatAppearance.MouseOverBackColor = TS_ThemeEngine.ColorMode(GlowMain.theme, "AccentMainHover");
                //
                TSImageRenderer(DNS_TestStartBtn, GlowMain.theme == 1 ? Properties.Resources.ct_test_start_light : Properties.Resources.ct_test_start_dark, 17, ContentAlignment.MiddleRight);
                TSImageRenderer(DNS_CustomTestBtn, GlowMain.theme == 1 ? Properties.Resources.ct_dns_test_light : Properties.Resources.ct_dns_test_dark, 13, ContentAlignment.MiddleRight);
                TSImageRenderer(DNS_TestExportBtn, GlowMain.theme == 1 ? Properties.Resources.ct_export_light : Properties.Resources.ct_export_dark, 17, ContentAlignment.MiddleRight);
                //
                Text = string.Format(software_lang.TSReadLangs("DNSTestTool", "dtt_title"), Application.ProductName);
                DNSTest_pingSendText = software_lang.TSReadLangs("DNSTestTool", "dtt_success");
                DNSTest_pingSendError = software_lang.TSReadLangs("DNSTestTool", "dtt_error");
                //
                DNSTable.Columns[0].HeaderText = software_lang.TSReadLangs("DNSTestTool", "dtt_column_server");
                DNSTable.Columns[1].HeaderText = software_lang.TSReadLangs("DNSTestTool", "dtt_column_server_response");
                //
                DNS_TestStartBtn.Text = " " + software_lang.TSReadLangs("DNSTestTool", "dtt_start");
                DNS_CustomTestBtn.Text = " " + software_lang.TSReadLangs("DNSTestTool", "dtt_custom");
                DNS_TestExportBtn.Text = " " + software_lang.TSReadLangs("DNSTestTool", "dtt_export");
            }catch (Exception){ }
        }
        // LOAD
        // ======================================================================================================
        private void GlowDNSTestTool_Load(object sender, EventArgs e){
            Dns_test_settings();
            try{
                foreach (var provider in DNSTest_dnsProviders)
                {
                    DNSTable.Rows.Add(provider.Name, software_lang.TSReadLangs("DNSTestTool", "dtt_start_await"));
                }
                DNSTable.ClearSelection();
            }catch (Exception) { }
        }
        // Async DNS Check
        // ======================================================================================================
        private async Task CheckDnsAsync(DnsProvider provider){
            try{
                var ping = new Ping();
                var result = new StringBuilder();
                for (int i = 0; i < provider.Addresses.Count; i++){
                    var reply = await ping.SendPingAsync(provider.Addresses[i]);
                    if (reply.Status == IPStatus.Success)
                        result.Append($"({provider.Addresses[i]}) - {DNSTest_pingSendText} {reply.RoundtripTime} ms");
                    else
                        result.Append($"({provider.Addresses[i]}) - {DNSTest_pingSendError} ({reply.Status})");
                    if (i < provider.Addresses.Count - 1)
                        result.Append("   |   ");
                }
                Invoke(new Action(() => {
                    DNSTable.Rows[provider.RowIndex].Cells[1].Value = result.ToString();
                    provider.IsCompleted = true;
                }));
            }catch (Exception){ }
        }
        // START ENGINE
        // ======================================================================================================
        private async void DNS_TestStartBtn_Click(object sender, EventArgs e){
            DNS_TestStartBtn.Enabled = false;
            DNS_TestExportBtn.Enabled = false;
            Text = string.Format(software_lang.TSReadLangs("DNSTestTool", "dtt_title"), Application.ProductName) + " | " + software_lang.TSReadLangs("DNSTestTool", "dtt_title_test_ruining");
            var tasks = DNSTest_dnsProviders.Select(p => CheckDnsAsync(p)).ToArray();
            await Task.WhenAll(tasks);
            Text = string.Format(software_lang.TSReadLangs("DNSTestTool", "dtt_title"), Application.ProductName);
            DNS_TestExportBtn.Enabled = true;
            DNS_TestStartBtn.Enabled = true;
        }
        // COPY RESULT
        // ======================================================================================================
        private void DNSTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e){
            try{
                if (DNSTest_dnsProviders.All(p => p.IsCompleted)){
                    if (DNSTable.SelectedRows.Count > 0){
                        Clipboard.SetText(DNSTable.Rows[e.RowIndex].Cells[0].Value + ": " + DNSTable.Rows[e.RowIndex].Cells[1].Value);
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, string.Format(software_lang.TSReadLangs("DNSTestTool", "dtt_copy_success"), DNSTable.Rows[e.RowIndex].Cells[0].Value));
                    }
                }else{
                    TS_MessageBoxEngine.TS_MessageBox(this, 2, software_lang.TSReadLangs("DNSTestTool", "dtt_copy_info"));
                }
            }catch (Exception){
                TS_MessageBoxEngine.TS_MessageBox(this, 3, string.Format(software_lang.TSReadLangs("DNSTestTool", "dtt_copy_failed"), DNSTable.Rows[e.RowIndex].Cells[0].Value, "\n"));
            }
        }
        // CUSTOM DNS TEST
        // ======================================================================================================
        private void DNS_CustomTestBtn_Click(object sender, EventArgs e){
            try{
                string custom_ip = Interaction.InputBox(string.Format(software_lang.TSReadLangs("DNSTestTool", "dtt_custom_message"), "\n\n", "1.1.1.1"), string.Format(software_lang.TSReadLangs("DNSTestTool", "dtt_title"), Application.ProductName), "");
                if (string.IsNullOrWhiteSpace(custom_ip))
                    return;
                if (IPAddress.TryParse(custom_ip.Trim(), out IPAddress ip)){
                    var pingSender = new Ping();
                    var reply = pingSender.Send(custom_ip);
                    if (reply.Status == IPStatus.Success)
                        TS_MessageBoxEngine.TS_MessageBox(this, 1, $"IP: {custom_ip}\n{DNSTest_pingSendText} {reply.RoundtripTime} ms");
                    else
                        TS_MessageBoxEngine.TS_MessageBox(this, 2, $"({custom_ip}) - {DNSTest_pingSendError} (({reply.Status}))");
                }else{
                    DialogResult restart = TS_MessageBoxEngine.TS_MessageBox(this, 6, string.Format(software_lang.TSReadLangs("DNSTestTool", "dtt_custom_message_error"), "\n"));
                    if (restart == DialogResult.Yes)
                        DNS_CustomTestBtn.PerformClick();
                }
            }catch (Exception){ }
        }
        // PRINT ENGINE
        // ======================================================================================================
        readonly List<string> PrintDNSList = new List<string>();
        private void DNS_TestExportBtn_Click(object sender, EventArgs e){
            try{
                PrintDNSList.Add(Application.ProductName + " - " + string.Format(software_lang.TSReadLangs("PrintEngine", "pe_save_name"), software_lang.TSReadLangs("DNSTestTool", "dtt_export_name")));
                PrintDNSList.Add(Environment.NewLine + new string('-', 100) + Environment.NewLine);
                int maxLength = DNSTest_dnsProviders.Max(p => p.Name.Length);
                int maxLeftLength = 0;
                foreach (var provider in DNSTest_dnsProviders){
                    string value = DNSTable.Rows[provider.RowIndex].Cells[1].Value.ToString().Trim();
                    string[] parts = value.Split('|');
                    if (parts.Length == 2){
                        int len = parts[0].Trim().Length;
                        if (len > maxLeftLength)
                            maxLeftLength = len;
                    }
                }
                foreach (var provider in DNSTest_dnsProviders){
                    string name = provider.Name.PadRight(maxLength);
                    string value = DNSTable.Rows[provider.RowIndex].Cells[1].Value.ToString().Trim();
                    string[] parts = value.Split('|');
                    if (parts.Length == 2){
                        string left = parts[0].Trim();
                        string right = parts[1].Trim();
                        PrintDNSList.Add($"{name}: {left.PadRight(maxLeftLength)}\t|\t{right}");
                    }else{
                        PrintDNSList.Add($"{name}: {value}");
                    }
                }
                PrintDNSList.Add(Environment.NewLine + new string('-', 100) + Environment.NewLine);
                PrintDNSList.Add(Application.ProductName + " " + software_lang.TSReadLangs("PrintEngine", "pe_version") + " " + TS_VersionEngine.TS_SofwareVersion(1, Program.ts_version_mode));
                PrintDNSList.Add(TS_SoftwareCopyrightDate.ts_scd_preloader);
                PrintDNSList.Add(software_lang.TSReadLangs("PrintEngine", "pe_process_time") + " " + DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss"));
                PrintDNSList.Add(software_lang.TSReadLangs("PrintEngine", "pe_website") + " " + TS_LinkSystem.website_link);
                PrintDNSList.Add(software_lang.TSReadLangs("PrintEngine", "pe_github") + " " + TS_LinkSystem.github_link);
                PrintDNSList.Add(software_lang.TSReadLangs("PrintEngine", "pe_donate") + " " + TS_LinkSystem.ts_donate);
                SaveFileDialog saveDlg = new SaveFileDialog{
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Title = Application.ProductName + " - " + software_lang.TSReadLangs("PrintEngine", "pe_save_directory"),
                    DefaultExt = "txt",
                    FileName = Application.ProductName + " - " + string.Format(software_lang.TSReadLangs("PrintEngine", "pe_save_name"), software_lang.TSReadLangs("DNSTestTool", "dtt_export_name")),
                    Filter = software_lang.TSReadLangs("PrintEngine", "pe_save_txt") + " (*.txt)|*.txt"
                };
                if (saveDlg.ShowDialog() == DialogResult.OK){
                    File.WriteAllText(saveDlg.FileName, string.Join(Environment.NewLine, PrintDNSList));
                    DialogResult res = TS_MessageBoxEngine.TS_MessageBox(this, 5, string.Format(software_lang.TSReadLangs("PrintEngine", "pe_save_success") + Environment.NewLine + Environment.NewLine + software_lang.TSReadLangs("PrintEngine", "pe_save_info_open"), Application.ProductName, saveDlg.FileName));
                    if (res == DialogResult.Yes) { Process.Start(saveDlg.FileName); }
                }
                PrintDNSList.Clear();
                saveDlg.Dispose();
            }catch (Exception){ }
        }
    }
}