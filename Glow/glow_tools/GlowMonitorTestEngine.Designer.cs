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
            this.SuspendLayout();
            // 
            // GlowMonitorTestEngine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(759, 411);
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

        }

        #endregion
    }
}