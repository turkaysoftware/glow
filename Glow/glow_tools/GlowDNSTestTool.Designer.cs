﻿namespace Glow.glow_tools
{
    partial class GlowDNSTestTool
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlowDNSTestTool));
            this.DNSTable = new System.Windows.Forms.DataGridView();
            this.BGPanel = new System.Windows.Forms.Panel();
            this.Btn_FLP = new System.Windows.Forms.FlowLayoutPanel();
            this.DNS_TestStartBtn = new Glow.TSCustomButton();
            this.DNS_CustomTestBtn = new Glow.TSCustomButton();
            this.DNS_TestExportBtn = new Glow.TSCustomButton();
            ((System.ComponentModel.ISupportInitialize)(this.DNSTable)).BeginInit();
            this.BGPanel.SuspendLayout();
            this.Btn_FLP.SuspendLayout();
            this.SuspendLayout();
            // 
            // DNSTable
            // 
            this.DNSTable.AllowUserToAddRows = false;
            this.DNSTable.AllowUserToDeleteRows = false;
            this.DNSTable.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.DNSTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DNSTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DNSTable.BackgroundColor = System.Drawing.Color.White;
            this.DNSTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DNSTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DNSTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.DNSTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DNSTable.Cursor = System.Windows.Forms.Cursors.Hand;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DNSTable.DefaultCellStyle = dataGridViewCellStyle3;
            this.DNSTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DNSTable.EnableHeadersVisualStyles = false;
            this.DNSTable.GridColor = System.Drawing.Color.Gray;
            this.DNSTable.Location = new System.Drawing.Point(0, 0);
            this.DNSTable.MultiSelect = false;
            this.DNSTable.Name = "DNSTable";
            this.DNSTable.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DNSTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.DNSTable.RowHeadersVisible = false;
            this.DNSTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DNSTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DNSTable.Size = new System.Drawing.Size(614, 428);
            this.DNSTable.TabIndex = 0;
            this.DNSTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DNSTable_CellDoubleClick);
            // 
            // BGPanel
            // 
            this.BGPanel.Controls.Add(this.DNSTable);
            this.BGPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BGPanel.Location = new System.Drawing.Point(10, 10);
            this.BGPanel.Name = "BGPanel";
            this.BGPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.BGPanel.Size = new System.Drawing.Size(614, 438);
            this.BGPanel.TabIndex = 0;
            // 
            // Btn_FLP
            // 
            this.Btn_FLP.AutoSize = true;
            this.Btn_FLP.Controls.Add(this.DNS_TestStartBtn);
            this.Btn_FLP.Controls.Add(this.DNS_CustomTestBtn);
            this.Btn_FLP.Controls.Add(this.DNS_TestExportBtn);
            this.Btn_FLP.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Btn_FLP.Location = new System.Drawing.Point(10, 448);
            this.Btn_FLP.Name = "Btn_FLP";
            this.Btn_FLP.Size = new System.Drawing.Size(614, 128);
            this.Btn_FLP.TabIndex = 1;
            // 
            // DNS_TestStartBtn
            // 
            this.DNS_TestStartBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.DNS_TestStartBtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.DNS_TestStartBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DNS_TestStartBtn.BorderColor = System.Drawing.Color.DodgerBlue;
            this.DNS_TestStartBtn.BorderRadius = 10;
            this.DNS_TestStartBtn.BorderSize = 0;
            this.DNS_TestStartBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DNS_TestStartBtn.FlatAppearance.BorderSize = 0;
            this.DNS_TestStartBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DNS_TestStartBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.DNS_TestStartBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.DNS_TestStartBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DNS_TestStartBtn.Location = new System.Drawing.Point(0, 0);
            this.DNS_TestStartBtn.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.DNS_TestStartBtn.Name = "DNS_TestStartBtn";
            this.DNS_TestStartBtn.Size = new System.Drawing.Size(614, 36);
            this.DNS_TestStartBtn.TabIndex = 0;
            this.DNS_TestStartBtn.Text = "Başlat";
            this.DNS_TestStartBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DNS_TestStartBtn.TextColor = System.Drawing.Color.WhiteSmoke;
            this.DNS_TestStartBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.DNS_TestStartBtn.UseVisualStyleBackColor = false;
            this.DNS_TestStartBtn.Click += new System.EventHandler(this.DNS_TestStartBtn_Click);
            // 
            // DNS_CustomTestBtn
            // 
            this.DNS_CustomTestBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.DNS_CustomTestBtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.DNS_CustomTestBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DNS_CustomTestBtn.BorderColor = System.Drawing.Color.DodgerBlue;
            this.DNS_CustomTestBtn.BorderRadius = 10;
            this.DNS_CustomTestBtn.BorderSize = 0;
            this.DNS_CustomTestBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DNS_CustomTestBtn.FlatAppearance.BorderSize = 0;
            this.DNS_CustomTestBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DNS_CustomTestBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.DNS_CustomTestBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.DNS_CustomTestBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DNS_CustomTestBtn.Location = new System.Drawing.Point(0, 46);
            this.DNS_CustomTestBtn.Margin = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.DNS_CustomTestBtn.Name = "DNS_CustomTestBtn";
            this.DNS_CustomTestBtn.Size = new System.Drawing.Size(614, 36);
            this.DNS_CustomTestBtn.TabIndex = 1;
            this.DNS_CustomTestBtn.Text = "Başlat Custom";
            this.DNS_CustomTestBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DNS_CustomTestBtn.TextColor = System.Drawing.Color.WhiteSmoke;
            this.DNS_CustomTestBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.DNS_CustomTestBtn.UseVisualStyleBackColor = false;
            this.DNS_CustomTestBtn.Click += new System.EventHandler(this.DNS_CustomTestBtn_Click);
            // 
            // DNS_TestExportBtn
            // 
            this.DNS_TestExportBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.DNS_TestExportBtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.DNS_TestExportBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.DNS_TestExportBtn.BorderColor = System.Drawing.Color.DodgerBlue;
            this.DNS_TestExportBtn.BorderRadius = 10;
            this.DNS_TestExportBtn.BorderSize = 0;
            this.DNS_TestExportBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DNS_TestExportBtn.Enabled = false;
            this.DNS_TestExportBtn.FlatAppearance.BorderSize = 0;
            this.DNS_TestExportBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.DNS_TestExportBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.DNS_TestExportBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.DNS_TestExportBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DNS_TestExportBtn.Location = new System.Drawing.Point(0, 92);
            this.DNS_TestExportBtn.Margin = new System.Windows.Forms.Padding(0);
            this.DNS_TestExportBtn.Name = "DNS_TestExportBtn";
            this.DNS_TestExportBtn.Size = new System.Drawing.Size(614, 36);
            this.DNS_TestExportBtn.TabIndex = 2;
            this.DNS_TestExportBtn.Text = "Dışa Aktar";
            this.DNS_TestExportBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.DNS_TestExportBtn.TextColor = System.Drawing.Color.WhiteSmoke;
            this.DNS_TestExportBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.DNS_TestExportBtn.UseVisualStyleBackColor = false;
            this.DNS_TestExportBtn.Click += new System.EventHandler(this.DNS_TestExportBtn_Click);
            // 
            // GlowDNSTestTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(634, 586);
            this.Controls.Add(this.BGPanel);
            this.Controls.Add(this.Btn_FLP);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GlowDNSTestTool";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowDNSTestTool";
            this.Load += new System.EventHandler(this.GlowDNSTestTool_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DNSTable)).EndInit();
            this.BGPanel.ResumeLayout(false);
            this.Btn_FLP.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private TSCustomButton DNS_TestStartBtn;
        private TSCustomButton DNS_TestExportBtn;
        private TSCustomButton DNS_CustomTestBtn;
        private System.Windows.Forms.DataGridView DNSTable;
        private System.Windows.Forms.Panel BGPanel;
        private System.Windows.Forms.FlowLayoutPanel Btn_FLP;
    }
}