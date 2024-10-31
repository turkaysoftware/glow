namespace Glow.glow_tools
{
    partial class GlowShowWiFiPasswordTool
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlowShowWiFiPasswordTool));
            this.Panel_BG = new System.Windows.Forms.Panel();
            this.SWPT_ResultBox = new System.Windows.Forms.Label();
            this.SWPT_CopyBtn = new System.Windows.Forms.Button();
            this.SWPT_SelectBox = new System.Windows.Forms.ListBox();
            this.SWPT_TitleLabel = new System.Windows.Forms.Label();
            this.Panel_BG.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_BG
            // 
            this.Panel_BG.Controls.Add(this.SWPT_ResultBox);
            this.Panel_BG.Controls.Add(this.SWPT_CopyBtn);
            this.Panel_BG.Controls.Add(this.SWPT_SelectBox);
            this.Panel_BG.Controls.Add(this.SWPT_TitleLabel);
            this.Panel_BG.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Panel_BG.Location = new System.Drawing.Point(3, 3);
            this.Panel_BG.Name = "Panel_BG";
            this.Panel_BG.Padding = new System.Windows.Forms.Padding(10);
            this.Panel_BG.Size = new System.Drawing.Size(478, 308);
            this.Panel_BG.TabIndex = 0;
            // 
            // SWPT_ResultBox
            // 
            this.SWPT_ResultBox.BackColor = System.Drawing.Color.White;
            this.SWPT_ResultBox.Font = new System.Drawing.Font("Segoe UI Semibold", 10.75F, System.Drawing.FontStyle.Bold);
            this.SWPT_ResultBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SWPT_ResultBox.Location = new System.Drawing.Point(10, 206);
            this.SWPT_ResultBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 20);
            this.SWPT_ResultBox.Name = "SWPT_ResultBox";
            this.SWPT_ResultBox.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.SWPT_ResultBox.Size = new System.Drawing.Size(458, 35);
            this.SWPT_ResultBox.TabIndex = 2;
            this.SWPT_ResultBox.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SWPT_CopyBtn
            // 
            this.SWPT_CopyBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(103)))), ((int)(((byte)(192)))));
            this.SWPT_CopyBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.SWPT_CopyBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SWPT_CopyBtn.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.SWPT_CopyBtn.Enabled = false;
            this.SWPT_CopyBtn.FlatAppearance.BorderSize = 0;
            this.SWPT_CopyBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SWPT_CopyBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.SWPT_CopyBtn.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.SWPT_CopyBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.SWPT_CopyBtn.Location = new System.Drawing.Point(10, 261);
            this.SWPT_CopyBtn.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.SWPT_CopyBtn.Name = "SWPT_CopyBtn";
            this.SWPT_CopyBtn.Size = new System.Drawing.Size(458, 37);
            this.SWPT_CopyBtn.TabIndex = 3;
            this.SWPT_CopyBtn.Text = "Wi-Fi Şifre Kopyala";
            this.SWPT_CopyBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.SWPT_CopyBtn.UseVisualStyleBackColor = false;
            this.SWPT_CopyBtn.Click += new System.EventHandler(this.SWPT_CopyBtn_Click);
            // 
            // SWPT_SelectBox
            // 
            this.SWPT_SelectBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SWPT_SelectBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SWPT_SelectBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SWPT_SelectBox.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.SWPT_SelectBox.FormattingEnabled = true;
            this.SWPT_SelectBox.ItemHeight = 17;
            this.SWPT_SelectBox.Location = new System.Drawing.Point(10, 65);
            this.SWPT_SelectBox.Margin = new System.Windows.Forms.Padding(3, 0, 3, 20);
            this.SWPT_SelectBox.Name = "SWPT_SelectBox";
            this.SWPT_SelectBox.Size = new System.Drawing.Size(458, 121);
            this.SWPT_SelectBox.TabIndex = 1;
            this.SWPT_SelectBox.SelectedIndexChanged += new System.EventHandler(this.SWPT_SelectBox_SelectedIndexChanged);
            // 
            // SWPT_TitleLabel
            // 
            this.SWPT_TitleLabel.BackColor = System.Drawing.Color.White;
            this.SWPT_TitleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SWPT_TitleLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 10.75F, System.Drawing.FontStyle.Bold);
            this.SWPT_TitleLabel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.SWPT_TitleLabel.Location = new System.Drawing.Point(10, 10);
            this.SWPT_TitleLabel.Margin = new System.Windows.Forms.Padding(3, 0, 3, 20);
            this.SWPT_TitleLabel.Name = "SWPT_TitleLabel";
            this.SWPT_TitleLabel.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.SWPT_TitleLabel.Size = new System.Drawing.Size(458, 35);
            this.SWPT_TitleLabel.TabIndex = 0;
            this.SWPT_TitleLabel.Text = "Wi-Fi Ağı Seçiniz:";
            this.SWPT_TitleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GlowShowWiFiPasswordTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(484, 314);
            this.Controls.Add(this.Panel_BG);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GlowShowWiFiPasswordTool";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowShowWiFiPasswordTool";
            this.Load += new System.EventHandler(this.GlowShowWiFiPasswordTool_Load);
            this.Panel_BG.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel Panel_BG;
        internal System.Windows.Forms.Label SWPT_TitleLabel;
        private System.Windows.Forms.ListBox SWPT_SelectBox;
        private System.Windows.Forms.Button SWPT_CopyBtn;
        internal System.Windows.Forms.Label SWPT_ResultBox;
    }
}