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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlowWallpaperPreview));
            this.BackPanel = new System.Windows.Forms.Panel();
            this.InPanel = new System.Windows.Forms.Panel();
            this.WPImage = new System.Windows.Forms.PictureBox();
            this.BtnWallpaperLocationBtn = new Glow.TSCustomButton();
            this.BackPanel.SuspendLayout();
            this.InPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WPImage)).BeginInit();
            this.SuspendLayout();
            // 
            // BackPanel
            // 
            this.BackPanel.Controls.Add(this.InPanel);
            this.BackPanel.Controls.Add(this.BtnWallpaperLocationBtn);
            this.BackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BackPanel.Location = new System.Drawing.Point(0, 0);
            this.BackPanel.Name = "BackPanel";
            this.BackPanel.Padding = new System.Windows.Forms.Padding(10);
            this.BackPanel.Size = new System.Drawing.Size(359, 211);
            this.BackPanel.TabIndex = 0;
            // 
            // InPanel
            // 
            this.InPanel.Controls.Add(this.WPImage);
            this.InPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InPanel.Location = new System.Drawing.Point(10, 10);
            this.InPanel.Name = "InPanel";
            this.InPanel.Padding = new System.Windows.Forms.Padding(0, 0, 0, 10);
            this.InPanel.Size = new System.Drawing.Size(339, 156);
            this.InPanel.TabIndex = 0;
            // 
            // WPImage
            // 
            this.WPImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.WPImage.Location = new System.Drawing.Point(0, 0);
            this.WPImage.Name = "WPImage";
            this.WPImage.Size = new System.Drawing.Size(339, 146);
            this.WPImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.WPImage.TabIndex = 0;
            this.WPImage.TabStop = false;
            // 
            // BtnWallpaperLocationBtn
            // 
            this.BtnWallpaperLocationBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.BtnWallpaperLocationBtn.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
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
            this.BtnWallpaperLocationBtn.Location = new System.Drawing.Point(10, 166);
            this.BtnWallpaperLocationBtn.Margin = new System.Windows.Forms.Padding(3, 1, 1, 25);
            this.BtnWallpaperLocationBtn.Name = "BtnWallpaperLocationBtn";
            this.BtnWallpaperLocationBtn.Padding = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.BtnWallpaperLocationBtn.Size = new System.Drawing.Size(339, 35);
            this.BtnWallpaperLocationBtn.TabIndex = 1;
            this.BtnWallpaperLocationBtn.Text = "Open Wallpaper";
            this.BtnWallpaperLocationBtn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnWallpaperLocationBtn.TextColor = System.Drawing.Color.White;
            this.BtnWallpaperLocationBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnWallpaperLocationBtn.UseVisualStyleBackColor = false;
            this.BtnWallpaperLocationBtn.Click += new System.EventHandler(this.BtnWallpaperLocationBtn_Click);
            // 
            // GlowWallpaperPreview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(359, 211);
            this.Controls.Add(this.BackPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(375, 250);
            this.MinimumSize = new System.Drawing.Size(375, 250);
            this.Name = "GlowWallpaperPreview";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowWallpaperPreview";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlowWallpaperPreview_FormClosing);
            this.Load += new System.EventHandler(this.GlowWallpaperPreview_Load);
            this.BackPanel.ResumeLayout(false);
            this.InPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WPImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel BackPanel;
        private TSCustomButton BtnWallpaperLocationBtn;
        private System.Windows.Forms.Panel InPanel;
        private System.Windows.Forms.PictureBox WPImage;
    }
}