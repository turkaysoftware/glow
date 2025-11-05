namespace Glow.glow_tools
{
    partial class GlowWallpaperPreview
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlowWallpaperPreview));
            this.BackPanel = new System.Windows.Forms.Panel();
            this.WPImage = new System.Windows.Forms.PictureBox();
            this.BtnWallpaperLocationBtn = new Glow.TSCustomButton();
            this.ImageDGV = new System.Windows.Forms.DataGridView();
            this.BackPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WPImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // BackPanel
            // 
            this.BackPanel.Controls.Add(this.ImageDGV);
            this.BackPanel.Controls.Add(this.WPImage);
            this.BackPanel.Controls.Add(this.BtnWallpaperLocationBtn);
            this.BackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BackPanel.Location = new System.Drawing.Point(0, 0);
            this.BackPanel.Name = "BackPanel";
            this.BackPanel.Padding = new System.Windows.Forms.Padding(10);
            this.BackPanel.Size = new System.Drawing.Size(489, 466);
            this.BackPanel.TabIndex = 0;
            // 
            // WPImage
            // 
            this.WPImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.WPImage.Location = new System.Drawing.Point(10, 10);
            this.WPImage.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.WPImage.Name = "WPImage";
            this.WPImage.Size = new System.Drawing.Size(469, 165);
            this.WPImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.WPImage.TabIndex = 0;
            this.WPImage.TabStop = false;
            // 
            // BtnWallpaperLocationBtn
            // 
            this.BtnWallpaperLocationBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.BtnWallpaperLocationBtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.BtnWallpaperLocationBtn.BorderColor = System.Drawing.Color.DodgerBlue;
            this.BtnWallpaperLocationBtn.BorderRadius = 10;
            this.BtnWallpaperLocationBtn.BorderSize = 0;
            this.BtnWallpaperLocationBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnWallpaperLocationBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BtnWallpaperLocationBtn.FlatAppearance.BorderSize = 0;
            this.BtnWallpaperLocationBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnWallpaperLocationBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.BtnWallpaperLocationBtn.ForeColor = System.Drawing.Color.White;
            this.BtnWallpaperLocationBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnWallpaperLocationBtn.Location = new System.Drawing.Point(10, 420);
            this.BtnWallpaperLocationBtn.Name = "BtnWallpaperLocationBtn";
            this.BtnWallpaperLocationBtn.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.BtnWallpaperLocationBtn.Size = new System.Drawing.Size(469, 36);
            this.BtnWallpaperLocationBtn.TabIndex = 1;
            this.BtnWallpaperLocationBtn.Text = "Open Wallpaper";
            this.BtnWallpaperLocationBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnWallpaperLocationBtn.TextColor = System.Drawing.Color.White;
            this.BtnWallpaperLocationBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnWallpaperLocationBtn.UseVisualStyleBackColor = false;
            this.BtnWallpaperLocationBtn.Click += new System.EventHandler(this.BtnWallpaperLocationBtn_Click);
            // 
            // ImageDGV
            // 
            this.ImageDGV.AllowUserToAddRows = false;
            this.ImageDGV.AllowUserToDeleteRows = false;
            this.ImageDGV.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.ImageDGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ImageDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.ImageDGV.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.ImageDGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ImageDGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ImageDGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ImageDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ImageDGV.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ImageDGV.DefaultCellStyle = dataGridViewCellStyle3;
            this.ImageDGV.EnableHeadersVisualStyles = false;
            this.ImageDGV.GridColor = System.Drawing.Color.Gray;
            this.ImageDGV.Location = new System.Drawing.Point(10, 198);
            this.ImageDGV.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.ImageDGV.MultiSelect = false;
            this.ImageDGV.Name = "ImageDGV";
            this.ImageDGV.ReadOnly = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ImageDGV.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.ImageDGV.RowHeadersVisible = false;
            this.ImageDGV.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ImageDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ImageDGV.Size = new System.Drawing.Size(469, 199);
            this.ImageDGV.TabIndex = 0;
            this.ImageDGV.SelectionChanged += new System.EventHandler(this.ImageDGV_SelectionChanged);
            // 
            // GlowWallpaperPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(489, 466);
            this.Controls.Add(this.BackPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = Properties.Resources.GlowLogo;
            this.MaximizeBox = false;
            this.Name = "GlowWallpaperPreview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowWallpaperPreview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlowWallpaperPreview_FormClosing);
            this.Load += new System.EventHandler(this.GlowWallpaperPreview_Load);
            this.BackPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WPImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ImageDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BackPanel;
        private TSCustomButton BtnWallpaperLocationBtn;
        private System.Windows.Forms.PictureBox WPImage;
        private System.Windows.Forms.DataGridView ImageDGV;
    }
}