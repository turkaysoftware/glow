namespace Glow
{
    partial class GlowAbout
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlowAbout));
            this.About_BG_Panel = new System.Windows.Forms.Panel();
            this.About_MediaTLP = new System.Windows.Forms.TableLayoutPanel();
            this.About_XBtn = new System.Windows.Forms.Button();
            this.About_GitHubBtn = new System.Windows.Forms.Button();
            this.About_HeaderTextPanel = new System.Windows.Forms.Panel();
            this.About_L1 = new System.Windows.Forms.Label();
            this.About_L2 = new System.Windows.Forms.Label();
            this.About_Image = new System.Windows.Forms.PictureBox();
            this.About_BG_Panel.SuspendLayout();
            this.About_MediaTLP.SuspendLayout();
            this.About_HeaderTextPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.About_Image)).BeginInit();
            this.SuspendLayout();
            // 
            // About_BG_Panel
            // 
            this.About_BG_Panel.BackColor = System.Drawing.Color.White;
            this.About_BG_Panel.Controls.Add(this.About_MediaTLP);
            this.About_BG_Panel.Controls.Add(this.About_HeaderTextPanel);
            this.About_BG_Panel.Controls.Add(this.About_Image);
            this.About_BG_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.About_BG_Panel.Location = new System.Drawing.Point(3, 3);
            this.About_BG_Panel.Name = "About_BG_Panel";
            this.About_BG_Panel.Padding = new System.Windows.Forms.Padding(5);
            this.About_BG_Panel.Size = new System.Drawing.Size(503, 132);
            this.About_BG_Panel.TabIndex = 0;
            // 
            // About_MediaTLP
            // 
            this.About_MediaTLP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.About_MediaTLP.BackColor = System.Drawing.Color.Transparent;
            this.About_MediaTLP.ColumnCount = 2;
            this.About_MediaTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.About_MediaTLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.About_MediaTLP.Controls.Add(this.About_XBtn, 0, 0);
            this.About_MediaTLP.Controls.Add(this.About_GitHubBtn, 1, 0);
            this.About_MediaTLP.Location = new System.Drawing.Point(8, 90);
            this.About_MediaTLP.Name = "About_MediaTLP";
            this.About_MediaTLP.RowCount = 1;
            this.About_MediaTLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.About_MediaTLP.Size = new System.Drawing.Size(487, 32);
            this.About_MediaTLP.TabIndex = 8;
            // 
            // About_XBtn
            // 
            this.About_XBtn.BackColor = System.Drawing.Color.RosyBrown;
            this.About_XBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.About_XBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.About_XBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.About_XBtn.FlatAppearance.BorderSize = 0;
            this.About_XBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.About_XBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.About_XBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.About_XBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.About_XBtn.Location = new System.Drawing.Point(0, 0);
            this.About_XBtn.Margin = new System.Windows.Forms.Padding(0, 0, 1, 0);
            this.About_XBtn.Name = "About_XBtn";
            this.About_XBtn.Size = new System.Drawing.Size(242, 32);
            this.About_XBtn.TabIndex = 6;
            this.About_XBtn.Text = "Başlat";
            this.About_XBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.About_XBtn.UseVisualStyleBackColor = false;
            this.About_XBtn.Click += new System.EventHandler(this.About_XBtn_Click);
            // 
            // About_GitHubBtn
            // 
            this.About_GitHubBtn.BackColor = System.Drawing.Color.RosyBrown;
            this.About_GitHubBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.About_GitHubBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.About_GitHubBtn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.About_GitHubBtn.FlatAppearance.BorderSize = 0;
            this.About_GitHubBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.About_GitHubBtn.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.About_GitHubBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.About_GitHubBtn.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.About_GitHubBtn.Location = new System.Drawing.Point(244, 0);
            this.About_GitHubBtn.Margin = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.About_GitHubBtn.Name = "About_GitHubBtn";
            this.About_GitHubBtn.Size = new System.Drawing.Size(243, 32);
            this.About_GitHubBtn.TabIndex = 7;
            this.About_GitHubBtn.Text = "Başlat";
            this.About_GitHubBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.About_GitHubBtn.UseVisualStyleBackColor = false;
            this.About_GitHubBtn.Click += new System.EventHandler(this.About_GitHubBtn_Click);
            // 
            // About_HeaderTextPanel
            // 
            this.About_HeaderTextPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.About_HeaderTextPanel.BackColor = System.Drawing.Color.Transparent;
            this.About_HeaderTextPanel.Controls.Add(this.About_L1);
            this.About_HeaderTextPanel.Controls.Add(this.About_L2);
            this.About_HeaderTextPanel.Location = new System.Drawing.Point(79, 9);
            this.About_HeaderTextPanel.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.About_HeaderTextPanel.Name = "About_HeaderTextPanel";
            this.About_HeaderTextPanel.Size = new System.Drawing.Size(416, 65);
            this.About_HeaderTextPanel.TabIndex = 5;
            // 
            // About_L1
            // 
            this.About_L1.BackColor = System.Drawing.Color.Transparent;
            this.About_L1.Dock = System.Windows.Forms.DockStyle.Top;
            this.About_L1.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.About_L1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.About_L1.Location = new System.Drawing.Point(0, 0);
            this.About_L1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.About_L1.Name = "About_L1";
            this.About_L1.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.About_L1.Size = new System.Drawing.Size(416, 32);
            this.About_L1.TabIndex = 3;
            this.About_L1.Text = "Glow";
            this.About_L1.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // About_L2
            // 
            this.About_L2.BackColor = System.Drawing.Color.Transparent;
            this.About_L2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.About_L2.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.About_L2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.About_L2.Location = new System.Drawing.Point(0, 33);
            this.About_L2.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.About_L2.Name = "About_L2";
            this.About_L2.Padding = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.About_L2.Size = new System.Drawing.Size(416, 32);
            this.About_L2.TabIndex = 4;
            this.About_L2.Text = "Sürüm: v24.08";
            // 
            // About_Image
            // 
            this.About_Image.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.About_Image.Location = new System.Drawing.Point(8, 8);
            this.About_Image.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.About_Image.Name = "About_Image";
            this.About_Image.Size = new System.Drawing.Size(65, 65);
            this.About_Image.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.About_Image.TabIndex = 2;
            this.About_Image.TabStop = false;
            // 
            // GlowAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(509, 138);
            this.Controls.Add(this.About_BG_Panel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(525, 177);
            this.MinimumSize = new System.Drawing.Size(525, 177);
            this.Name = "GlowAbout";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowAbout";
            this.Load += new System.EventHandler(this.GlowAbout_Load);
            this.About_BG_Panel.ResumeLayout(false);
            this.About_MediaTLP.ResumeLayout(false);
            this.About_HeaderTextPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.About_Image)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel About_BG_Panel;
        private System.Windows.Forms.PictureBox About_Image;
        internal System.Windows.Forms.Label About_L2;
        internal System.Windows.Forms.Label About_L1;
        private System.Windows.Forms.Panel About_HeaderTextPanel;
        private System.Windows.Forms.Button About_GitHubBtn;
        private System.Windows.Forms.Button About_XBtn;
        private System.Windows.Forms.TableLayoutPanel About_MediaTLP;
    }
}