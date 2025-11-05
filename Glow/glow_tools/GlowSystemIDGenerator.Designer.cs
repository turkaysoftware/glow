namespace Glow.glow_tools
{
    partial class GlowSystemIDGenerator
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Label_Info = new System.Windows.Forms.Label();
            this.DGV_MainTable = new System.Windows.Forms.DataGridView();
            this.Check_Saved = new System.Windows.Forms.CheckBox();
            this.Btn_Compare = new Glow.TSCustomButton();
            this.Btn_Generate = new Glow.TSCustomButton();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_MainTable)).BeginInit();
            this.SuspendLayout();
            // 
            // Label_Info
            // 
            this.Label_Info.BackColor = System.Drawing.Color.White;
            this.Label_Info.Dock = System.Windows.Forms.DockStyle.Top;
            this.Label_Info.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.Label_Info.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Label_Info.Location = new System.Drawing.Point(10, 10);
            this.Label_Info.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.Label_Info.Name = "Label_Info";
            this.Label_Info.Padding = new System.Windows.Forms.Padding(10);
            this.Label_Info.Size = new System.Drawing.Size(864, 140);
            this.Label_Info.TabIndex = 0;
            this.Label_Info.Text = "N/A Text";
            this.Label_Info.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DGV_MainTable
            // 
            this.DGV_MainTable.AllowUserToAddRows = false;
            this.DGV_MainTable.AllowUserToDeleteRows = false;
            this.DGV_MainTable.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.DGV_MainTable.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            this.DGV_MainTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DGV_MainTable.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.DGV_MainTable.BackgroundColor = System.Drawing.Color.White;
            this.DGV_MainTable.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.DGV_MainTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_MainTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.DGV_MainTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.DGV_MainTable.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_MainTable.DefaultCellStyle = dataGridViewCellStyle7;
            this.DGV_MainTable.EnableHeadersVisualStyles = false;
            this.DGV_MainTable.GridColor = System.Drawing.Color.Gray;
            this.DGV_MainTable.Location = new System.Drawing.Point(10, 158);
            this.DGV_MainTable.Margin = new System.Windows.Forms.Padding(3, 3, 3, 5);
            this.DGV_MainTable.MultiSelect = false;
            this.DGV_MainTable.Name = "DGV_MainTable";
            this.DGV_MainTable.ReadOnly = true;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.DGV_MainTable.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.DGV_MainTable.RowHeadersVisible = false;
            this.DGV_MainTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.DGV_MainTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_MainTable.Size = new System.Drawing.Size(864, 221);
            this.DGV_MainTable.TabIndex = 1;
            this.DGV_MainTable.SelectionChanged += new System.EventHandler(this.DGV_MainTable_SelectionChanged);
            // 
            // Check_Saved
            // 
            this.Check_Saved.AutoSize = true;
            this.Check_Saved.Checked = true;
            this.Check_Saved.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Check_Saved.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Check_Saved.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.Check_Saved.Location = new System.Drawing.Point(10, 389);
            this.Check_Saved.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.Check_Saved.Name = "Check_Saved";
            this.Check_Saved.Size = new System.Drawing.Size(106, 21);
            this.Check_Saved.TabIndex = 2;
            this.Check_Saved.Text = "Tersten sırala";
            this.Check_Saved.UseVisualStyleBackColor = true;
            this.Check_Saved.CheckedChanged += new System.EventHandler(this.Check_Saved_CheckedChanged);
            // 
            // Btn_Compare
            // 
            this.Btn_Compare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Compare.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Btn_Compare.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Btn_Compare.BorderColor = System.Drawing.Color.DodgerBlue;
            this.Btn_Compare.BorderRadius = 10;
            this.Btn_Compare.BorderSize = 0;
            this.Btn_Compare.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_Compare.Enabled = false;
            this.Btn_Compare.FlatAppearance.BorderSize = 0;
            this.Btn_Compare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Compare.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.Btn_Compare.ForeColor = System.Drawing.Color.White;
            this.Btn_Compare.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn_Compare.Location = new System.Drawing.Point(10, 462);
            this.Btn_Compare.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.Btn_Compare.Name = "Btn_Compare";
            this.Btn_Compare.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.Btn_Compare.Size = new System.Drawing.Size(864, 36);
            this.Btn_Compare.TabIndex = 4;
            this.Btn_Compare.Text = "Karşılaştır";
            this.Btn_Compare.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn_Compare.TextColor = System.Drawing.Color.White;
            this.Btn_Compare.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Btn_Compare.UseVisualStyleBackColor = false;
            this.Btn_Compare.Click += new System.EventHandler(this.Btn_Compare_Click);
            // 
            // Btn_Generate
            // 
            this.Btn_Generate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_Generate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Btn_Generate.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Btn_Generate.BorderColor = System.Drawing.Color.DodgerBlue;
            this.Btn_Generate.BorderRadius = 10;
            this.Btn_Generate.BorderSize = 0;
            this.Btn_Generate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Btn_Generate.FlatAppearance.BorderSize = 0;
            this.Btn_Generate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Btn_Generate.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.Btn_Generate.ForeColor = System.Drawing.Color.White;
            this.Btn_Generate.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn_Generate.Location = new System.Drawing.Point(10, 418);
            this.Btn_Generate.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.Btn_Generate.Name = "Btn_Generate";
            this.Btn_Generate.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.Btn_Generate.Size = new System.Drawing.Size(864, 36);
            this.Btn_Generate.TabIndex = 3;
            this.Btn_Generate.Text = "Rapor Oluştur";
            this.Btn_Generate.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Btn_Generate.TextColor = System.Drawing.Color.White;
            this.Btn_Generate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Btn_Generate.UseVisualStyleBackColor = false;
            this.Btn_Generate.Click += new System.EventHandler(this.Btn_Generate_Click);
            // 
            // GlowSystemIDGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(884, 511);
            this.Controls.Add(this.Check_Saved);
            this.Controls.Add(this.Btn_Compare);
            this.Controls.Add(this.DGV_MainTable);
            this.Controls.Add(this.Btn_Generate);
            this.Controls.Add(this.Label_Info);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = global::Glow.Properties.Resources.GlowLogo;
            this.MaximizeBox = false;
            this.Name = "GlowSystemIDGenerator";
            this.Padding = new System.Windows.Forms.Padding(10);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowSystemIDGenerator";
            this.Load += new System.EventHandler(this.GlowSystemIDGenerator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_MainTable)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TSCustomButton Btn_Generate;
        internal System.Windows.Forms.Label Label_Info;
        private System.Windows.Forms.DataGridView DGV_MainTable;
        private TSCustomButton Btn_Compare;
        private System.Windows.Forms.CheckBox Check_Saved;
    }
}