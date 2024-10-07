using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.NetworkInformation;
//
using static Glow.TSModules;

namespace Glow.glow_tools{
    public partial class GlowDNSTestTool : Form{
        // ======================================================================================================
        // GLOBAL LANGS PATH
        TSGetLangs software_lang = new TSGetLangs(Glow.lang_path);
        static TS_LinkSystem TS_LinkSystem = new TS_LinkSystem();
        static TS_VersionEngine glow_version = new TS_VersionEngine();
        //
        public GlowDNSTestTool(){
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            //
            DNSTable.Columns.Add("x", "x");
            DNSTable.Columns.Add("x", "x");
        }
        //
        string ping_time_text;
        string ping_time_text_error;
        //
        bool test_status = false;
        bool alternate_dns_status   = false,
             cloudflare_dns_status  = false,
             comodo_dns_status      = false,
             control_d_dns_status   = false,
             dns_watch_dns_status   = false,
             google_dns_status      = false,
             lumen_dns_status       = false,
             opendns_dns_status     = false,
             quad9_dns_status       = false;

        // ======================================================================================================
        // DNS SERVERS
        List<string> alternate_dns_list = new List<string>(){ "76.76.19.19", "76.223.122.150" };
        List<string> cloudflare_dns_list = new List<string>(){ "1.1.1.1", "1.0.0.1" };
        List<string> comodo_dns_list = new List<string>(){ "8.26.56.26", "8.20.247.20" };
        List<string> control_d_dns_list = new List<string>(){ "76.76.2.0", "76.76.10.0" };
        List<string> dns_watch_dns_list = new List<string>(){ "84.200.69.80", "84.200.70.40" };
        List<string> google_dns_list = new List<string>(){ "8.8.8.8", "8.8.4.4" };
        List<string> lumen_dns_list = new List<string>(){ "4.2.2.1", "4.2.2.2" };
        List<string> opendns_dns_list = new List<string>(){ "208.67.222.222", "208.67.220.220" };
        List<string> quad9_dns_list = new List<string>(){ "9.9.9.9", "149.112.112.112" };
        // DNS SERVERS
        // ======================================================================================================
        public void dns_test_settings(){
            try{
                if (Glow.theme == 1){
                    try { if (DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4) != 1){ DwmSetWindowAttribute(Handle, 20, new[]{ 0 }, 4); } }catch (Exception){ }
                }else if (Glow.theme == 0){
                    try { if (DwmSetWindowAttribute(Handle, 19, new[]{ 1 }, 4) != 0){ DwmSetWindowAttribute(Handle, 20, new[]{ 1 }, 4); } }catch (Exception){ }
                }
                //
                BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "PageContainerBGAndPageContentTotalColors");
                //
                DNSTable.BackgroundColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridBGColor");
                DNSTable.GridColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridColor");
                DNSTable.DefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridBGColor");
                DNSTable.DefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridFEColor");
                DNSTable.AlternatingRowsDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "DataGridAlternatingColor");
                DNSTable.ColumnHeadersDefaultCellStyle.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageBG");
                DNSTable.ColumnHeadersDefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageBG");
                DNSTable.ColumnHeadersDefaultCellStyle.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageFE");
                DNSTable.DefaultCellStyle.SelectionBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageBG");
                DNSTable.DefaultCellStyle.SelectionForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "OSDAndServicesPageFE");
                //
                DNS_TestStartBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                DNS_TestStartBtn.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                DNS_TestStartBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                DNS_TestStartBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                DNS_CustomTestBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                DNS_CustomTestBtn.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                DNS_CustomTestBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                DNS_CustomTestBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                DNS_TestExportBtn.BackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                DNS_TestExportBtn.ForeColor = TS_ThemeEngine.ColorMode(Glow.theme, "DynamicThemeActiveBtnBG");
                DNS_TestExportBtn.FlatAppearance.BorderColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                DNS_TestExportBtn.FlatAppearance.MouseDownBackColor = TS_ThemeEngine.ColorMode(Glow.theme, "ContentLabelRight");
                //
                Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_title").Trim())), Application.ProductName);
                ping_time_text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_success").Trim()));
                ping_time_text_error = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_error").Trim()));
                //
                DNSTable.Columns[0].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_column_server").Trim()));
                DNSTable.Columns[1].HeaderText = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_column_server_response").Trim()));
                //
                DNS_TestStartBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_start").Trim()));
                DNS_CustomTestBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_custom").Trim()));
                DNS_TestExportBtn.Text = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_export").Trim()));
            }catch (Exception){ }
        }
        // LOAD
        // ======================================================================================================
        private void GlowDNSTestTool_Load(object sender, EventArgs e){
            dns_test_settings();
            try{
                string[] dns_server_list = { "Alternate DNS", "Cloudflare", "Comodo", "Control D", "DNS.WATCH", "Google", "Level3/Lumen DNS", "OpenDNS", "Quad9" };
                for (int i = 0; i<= dns_server_list.Length - 1; i++){
                    DNSTable.Rows.Add(dns_server_list[i], Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_start_await").Trim())));
                }
                DNSTable.Columns[0].Width = 175;
                //
                foreach (DataGridViewColumn DNS_Column in DNSTable.Columns){
                    DNS_Column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                //
                DNSTable.ClearSelection();
            }catch (Exception){ }
        }
        // ALTERATE
        // ======================================================================================================
        private void alternate_dns_check(){
            try{
                Ping pingSender = new Ping();
                StringBuilder resultText = new StringBuilder();
                for (int i = 0; i <= alternate_dns_list.Count - 1; i++){
                    PingReply reply = pingSender.Send(alternate_dns_list[i]);
                    if (reply.Status == IPStatus.Success){
                        resultText.Append($"({alternate_dns_list[i]}) - {ping_time_text} {reply.RoundtripTime} ms");
                    }else{
                        resultText.Append($"({alternate_dns_list[i]}) - {ping_time_text_error} ({reply.Status})");
                    }
                    if (i != alternate_dns_list.Count - 1){
                        resultText.Append("   |   ");
                    }
                }
                DNSTable.Rows[0].Cells[1].Value = resultText.ToString();
                alternate_dns_status = true;
            }catch (Exception){ }
        }
        // CLOUDFLARE
        // ======================================================================================================
        private void cloudflare_dns_check(){
            try{
                Ping pingSender = new Ping();
                StringBuilder resultText = new StringBuilder();
                for (int i = 0; i <= cloudflare_dns_list.Count - 1; i++){
                    PingReply reply = pingSender.Send(cloudflare_dns_list[i]);
                    if (reply.Status == IPStatus.Success){
                        resultText.Append($"({cloudflare_dns_list[i]}) - {ping_time_text} {reply.RoundtripTime} ms");
                    }else{
                        resultText.Append($"({cloudflare_dns_list[i]}) - {ping_time_text_error} ({reply.Status})");
                    }
                    if (i != cloudflare_dns_list.Count - 1){
                        resultText.Append("   |   ");
                    }
                }
                DNSTable.Rows[1].Cells[1].Value = resultText.ToString();
                cloudflare_dns_status = true;
            }catch (Exception){ }
        }
        // COMODO
        // ======================================================================================================
        private void comodo_dns_check(){
            try{
                Ping pingSender = new Ping();
                StringBuilder resultText = new StringBuilder();
                for (int i = 0; i <= comodo_dns_list.Count - 1; i++){
                    PingReply reply = pingSender.Send(comodo_dns_list[i]);
                    if (reply.Status == IPStatus.Success){
                        resultText.Append($"({comodo_dns_list[i]}) - {ping_time_text} {reply.RoundtripTime} ms");
                    }else{
                        resultText.Append($"({comodo_dns_list[i]}) - {ping_time_text_error} ({reply.Status})");
                    }
                    if (i != comodo_dns_list.Count - 1){
                        resultText.Append("   |   ");
                    }
                }
                DNSTable.Rows[2].Cells[1].Value = resultText.ToString();
                comodo_dns_status = true;
            }catch (Exception){ }
        }
        // CONTROL D
        // ======================================================================================================
        private void control_d_dns_check(){
            try{
                Ping pingSender = new Ping();
                StringBuilder resultText = new StringBuilder();
                for (int i = 0; i <= control_d_dns_list.Count - 1; i++){
                    PingReply reply = pingSender.Send(control_d_dns_list[i]);
                    if (reply.Status == IPStatus.Success){
                        resultText.Append($"({control_d_dns_list[i]}) - {ping_time_text} {reply.RoundtripTime} ms");
                    }else{
                        resultText.Append($"({control_d_dns_list[i]}) - {ping_time_text_error} ({reply.Status})");
                    }
                    if (i != control_d_dns_list.Count - 1){
                        resultText.Append("   |   ");
                    }
                }
                DNSTable.Rows[3].Cells[1].Value = resultText.ToString();
                control_d_dns_status = true;
            }catch (Exception){ }
        }
        // DNS.WATCH
        // ======================================================================================================
        private void dns_watchdns_check(){
            try{
                Ping pingSender = new Ping();
                StringBuilder resultText = new StringBuilder();
                for (int i = 0; i <= dns_watch_dns_list.Count - 1; i++){
                    PingReply reply = pingSender.Send(dns_watch_dns_list[i]);
                    if (reply.Status == IPStatus.Success){
                        resultText.Append($"({dns_watch_dns_list[i]}) - {ping_time_text} {reply.RoundtripTime} ms");
                    }else{
                        resultText.Append($"({dns_watch_dns_list[i]}) - {ping_time_text_error} ({reply.Status})");
                    }
                    if (i != dns_watch_dns_list.Count - 1){
                        resultText.Append("   |   ");
                    }
                }
                DNSTable.Rows[4].Cells[1].Value = resultText.ToString();
                dns_watch_dns_status = true;
            }catch (Exception){ }
        }
        // GOOGLE
        // ======================================================================================================
        private void google_dns_check(){
            try{
                Ping pingSender = new Ping();
                StringBuilder resultText = new StringBuilder();
                for (int i = 0; i <= google_dns_list.Count - 1; i++){
                    PingReply reply = pingSender.Send(google_dns_list[i]);
                    if (reply.Status == IPStatus.Success){
                        resultText.Append($"({google_dns_list[i]}) - {ping_time_text} {reply.RoundtripTime} ms");
                    }else{
                        resultText.Append($"({google_dns_list[i]}) - {ping_time_text_error} ({reply.Status})");
                    }
                    if (i != google_dns_list.Count - 1){
                        resultText.Append("   |   ");
                    }
                }
                DNSTable.Rows[5].Cells[1].Value = resultText.ToString();
                google_dns_status = true;
            }catch (Exception){ }
        }
        // LUMEN
        // ======================================================================================================
        private void lumen_dns_check(){
            try{
                Ping pingSender = new Ping();
                StringBuilder resultText = new StringBuilder();
                for (int i = 0; i <= lumen_dns_list.Count - 1; i++){
                    PingReply reply = pingSender.Send(lumen_dns_list[i]);
                    if (reply.Status == IPStatus.Success){
                        resultText.Append($"({lumen_dns_list[i]}) - {ping_time_text} {reply.RoundtripTime} ms");
                    }else{
                        resultText.Append($"({lumen_dns_list[i]}) - {ping_time_text_error} ({reply.Status})");
                    }
                    if (i != lumen_dns_list.Count - 1){
                        resultText.Append("   |   ");
                    }
                }
                DNSTable.Rows[6].Cells[1].Value = resultText.ToString();
                lumen_dns_status = true;
            }catch (Exception){ }
        }
        // OPENDNS
        // ======================================================================================================
        private void opendns_dns_check(){
            try{
                Ping pingSender = new Ping();
                StringBuilder resultText = new StringBuilder();
                for (int i = 0; i <= opendns_dns_list.Count - 1; i++){
                    PingReply reply = pingSender.Send(opendns_dns_list[i]);
                    if (reply.Status == IPStatus.Success){
                        resultText.Append($"({opendns_dns_list[i]}) - {ping_time_text} {reply.RoundtripTime} ms");
                    }else{
                        resultText.Append($"({opendns_dns_list[i]}) - {ping_time_text_error} ({reply.Status})");
                    }
                    if (i != opendns_dns_list.Count - 1){
                        resultText.Append("   |   ");
                    }
                }
                DNSTable.Rows[7].Cells[1].Value = resultText.ToString();
                opendns_dns_status = true;
            }catch (Exception){ }
        }
        // QUAD9
        // ======================================================================================================
        private void quad9_dns_check(){
            try{
                Ping pingSender = new Ping();
                StringBuilder resultText = new StringBuilder();
                for (int i = 0; i <= quad9_dns_list.Count - 1; i++){
                    PingReply reply = pingSender.Send(quad9_dns_list[i]);
                    if (reply.Status == IPStatus.Success){
                        resultText.Append($"({quad9_dns_list[i]}) - {ping_time_text} {reply.RoundtripTime} ms");
                    }else{
                        resultText.Append($"({quad9_dns_list[i]}) - {ping_time_text_error} ({reply.Status})");
                    }
                    if (i != quad9_dns_list.Count - 1){
                        resultText.Append("   |   ");
                    }
                }
                DNSTable.Rows[8].Cells[1].Value = resultText.ToString();
                quad9_dns_status = true;
            }catch (Exception){ }
        }
        // COPY RESULT
        // ======================================================================================================
        private void DNSTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e){
            try{
                if (alternate_dns_status == true && cloudflare_dns_status == true && comodo_dns_status == true && control_d_dns_status == true && dns_watch_dns_status == true && google_dns_status == true && lumen_dns_status == true && opendns_dns_status == true && quad9_dns_status == true){
                    if (DNSTable.SelectedRows.Count > 0){
                        Clipboard.SetText(DNSTable.Rows[e.RowIndex].Cells[0].Value.ToString() + ": " + DNSTable.Rows[e.RowIndex].Cells[1].Value.ToString());
                        MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_copy_success").Trim())), DNSTable.Rows[e.RowIndex].Cells[0].Value), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }else{
                    MessageBox.Show(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_copy_info").Trim())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }catch (Exception){
                MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_copy_failed").Trim())), DNSTable.Rows[e.RowIndex].Cells[0].Value, "\n"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // START ENGINE
        // ======================================================================================================
        private void DNS_TestStartBtn_Click(object sender, EventArgs e){
            DNS_TestStartBtn.Enabled = false;
            test_status = true;
            //
            Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_title").Trim())), Application.ProductName) + " | " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_title_test_ruining").Trim()));
            // Alternate DNS
            Task alternate_dns_test = new Task(alternate_dns_check);
            alternate_dns_test.Start();
            // Cloudflare
            Task cloudflare_dns_test = new Task(cloudflare_dns_check);
            cloudflare_dns_test.Start();
            // Comodo
            Task comodo_dns_test = new Task(comodo_dns_check);
            comodo_dns_test.Start();
            // Control D
            Task control_d_dns_test = new Task(control_d_dns_check);
            control_d_dns_test.Start();
            // DNS.WATCH
            Task dns_watch_dns_test = new Task(dns_watchdns_check);
            dns_watch_dns_test.Start();
            // Google
            Task google_dns_test = new Task(google_dns_check);
            google_dns_test.Start();
            // Lumen
            Task lumen_dns_test = new Task(lumen_dns_check);
            lumen_dns_test.Start();
            // OpenDNS
            Task opendns_dns_test = new Task(opendns_dns_check);
            opendns_dns_test.Start();
            // Quad9
            Task quad9_dns_test = new Task(quad9_dns_check);
            quad9_dns_test.Start();
            // TEST AFTER ASYNC
            Task test_after_async = new Task(test_after_mode);
            test_after_async.Start();
        }
        // CHECK TEST COMPLATE
        // ======================================================================================================
        private void test_after_mode(){
            do{
                if (alternate_dns_status == true && cloudflare_dns_status == true && comodo_dns_status == true && control_d_dns_status == true && dns_watch_dns_status == true && google_dns_status == true && lumen_dns_status == true && opendns_dns_status == true && quad9_dns_status == true){
                    Text = string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_title").Trim())), Application.ProductName);
                    DNS_TestExportBtn.Enabled = true;
                    DNS_TestStartBtn.Enabled = true;
                    test_status = false;
                }
                Thread.Sleep(1000 - DateTime.Now.Millisecond);
            } while (test_status == true);
        }
        // CUSTOM DNS TEST
        // ======================================================================================================
        private void DNS_CustomTestBtn_Click(object sender, EventArgs e){
            try{
                string custom_ip = Interaction.InputBox(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_custom_message").Trim())), "\n\n", cloudflare_dns_list[0]), string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_title").Trim())), Application.ProductName), "");
                if (custom_ip.Trim() != "" && custom_ip.Trim() != string.Empty){
                    if (IPAddress.TryParse(custom_ip.Trim(), out IPAddress ip)){
                        Ping pingSender = new Ping();
                        PingReply reply = pingSender.Send(custom_ip);
                        if (reply.Status == IPStatus.Success){
                            MessageBox.Show(string.Format("IP: {0}\n{1} {2} ms", custom_ip.Trim(), ping_time_text, reply.RoundtripTime), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }else{
                            MessageBox.Show($"({custom_ip}) - {ping_time_text_error} (({reply.Status}))", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }else{
                        DialogResult restart_custom_ip = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_custom_message_error").Trim())), "\n"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (restart_custom_ip == DialogResult.Yes){
                            DNS_CustomTestBtn.PerformClick();
                        }
                    }
                }
            }catch (Exception){ }
        }
        // PRINT ENGINE STARTER
        // ======================================================================================================
        List<string> PrintDNSList = new List<string>();
        private void DNS_TestExportBtn_Click(object sender, EventArgs e){
            try{
                PrintDNSList.Add(Application.ProductName + " - " + string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_save_name").Trim())), Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_export_name").Trim()))));
                PrintDNSList.Add(Environment.NewLine + new string('-', 75) + Environment.NewLine);
                //
                int maxLength = 0;
                for (int i = 0; i < DNSTable.Rows.Count; i++){
                    var dnsProviderName = DNSTable.Rows[i].Cells[0].Value.ToString().Trim();
                    if (dnsProviderName.Length > maxLength){
                        maxLength = dnsProviderName.Length;
                    }
                }
                //
                for (int i = 0; i < DNSTable.Rows.Count; i++){
                    var dnsProviderName = DNSTable.Rows[i].Cells[0].Value.ToString().Trim();
                    var dnsValue = DNSTable.Rows[i].Cells[1].Value.ToString().Trim();
                    string formattedLine = $"{dnsProviderName.PadRight(maxLength)}: {dnsValue}";
                    PrintDNSList.Add(formattedLine);
                }
                //
                PrintDNSList.Add(Environment.NewLine + new string('-', 75) + Environment.NewLine);
                // FOOTER
                PrintDNSList.Add(Application.ProductName + " " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_version").Trim())) + " " + glow_version.TS_SofwareVersion(1, Glow.ts_version_mode));
                PrintDNSList.Add($"{string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("SoftwareAbout", "sa_copyright").Trim())), "\u00a9", TS_SoftwareCopyrightDate.ts_scd, Application.CompanyName)}");
                PrintDNSList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_process_time").Trim())) + " " + DateTime.Now.ToString("dd.MM.yyyy - HH:mm:ss"));
                PrintDNSList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_website").Trim())) + " " + TS_LinkSystem.website_link);
                PrintDNSList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_twitter").Trim())) + " " + TS_LinkSystem.twitter_link);
                PrintDNSList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_instagram").Trim())) + " " + TS_LinkSystem.instagram_link);
                PrintDNSList.Add(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_github").Trim())) + " " + TS_LinkSystem.github_link);
                //
                SaveFileDialog save_engine = new SaveFileDialog{
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    Title = Application.ProductName + " - " + Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_save_directory").Trim())),
                    DefaultExt = "txt",
                    FileName = Application.ProductName + " - " + string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_save_name").Trim())), Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("DNSTestTool", "dtt_export_name").Trim()))),
                    Filter = Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_save_txt").Trim())) + " (*.txt)|*.txt"
                };
                if (save_engine.ShowDialog() == DialogResult.OK){
                    string combinedText = String.Join(Environment.NewLine, PrintDNSList);
                    File.WriteAllText(save_engine.FileName, combinedText);
                    DialogResult glow_print_engine_query = MessageBox.Show(string.Format(Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_save_success").Trim())) + Environment.NewLine + Environment.NewLine + Encoding.UTF8.GetString(Encoding.Default.GetBytes(software_lang.TSReadLangs("PrintEngine", "pe_save_info_open").Trim())), Application.ProductName, save_engine.FileName), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (glow_print_engine_query == DialogResult.Yes){
                        Process.Start(save_engine.FileName);
                    }
                    PrintDNSList.Clear();
                    save_engine.Dispose();
                }else{
                    PrintDNSList.Clear();
                    save_engine.Dispose();
                }
            }catch (Exception){ }
        }
    }
}