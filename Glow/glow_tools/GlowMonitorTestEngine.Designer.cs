namespace Glow.glow_tools
{
    partial class GlowMonitorTestEngine
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlowMonitorTestEngine));
            this.InfoLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // InfoLabel
            // 
            this.InfoLabel.AutoSize = true;
            this.InfoLabel.BackColor = System.Drawing.Color.Transparent;
            this.InfoLabel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.InfoLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.InfoLabel.Location = new System.Drawing.Point(12, 12);
            this.InfoLabel.Margin = new System.Windows.Forms.Padding(3);
            this.InfoLabel.Name = "InfoLabel";
            this.InfoLabel.Padding = new System.Windows.Forms.Padding(5);
            this.InfoLabel.Size = new System.Drawing.Size(206, 27);
            this.InfoLabel.TabIndex = 0;
            this.InfoLabel.Text = "Çıkmak için ESC tuşuna basınız.";
            this.InfoLabel.Click += new System.EventHandler(this.InfoLabel_Click);
            // 
            // GlowMonitorTestEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(759, 411);
            this.Controls.Add(this.InfoLabel);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(775, 450);
            this.Name = "GlowMonitorTestEngine";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowMonitorTestEngine";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlowMonitorTestEngine_FormClosing);
            this.Load += new System.EventHandler(this.GlowMonitorTestEngine_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GlowMonitorTestEngine_KeyDown);
            this.Resize += new System.EventHandler(this.GlowMonitorTestEngine_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InfoLabel;
    }
}