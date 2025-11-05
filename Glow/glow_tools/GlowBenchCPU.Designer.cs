namespace Glow.glow_tools
{
    partial class GlowBenchCPU
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlowBenchCPU));
            this.Bench_BG_Panel = new System.Windows.Forms.Panel();
            this.BtnPanel = new System.Windows.Forms.Panel();
            this.Bench_Start = new Glow.TSCustomButton();
            this.Bench_Stop = new Glow.TSCustomButton();
            this.Bench_TLP_R_P2 = new System.Windows.Forms.Panel();
            this.Bench_Label_RMulti = new System.Windows.Forms.Label();
            this.Bench_Label_RMultiResult = new System.Windows.Forms.Label();
            this.Bench_TLP_T_P2 = new System.Windows.Forms.Panel();
            this.Bench_ModeSelector_List = new Glow.TSCustomComboBox();
            this.Bench_ModeSelector = new System.Windows.Forms.Label();
            this.Bench_TLP_R_P1 = new System.Windows.Forms.Panel();
            this.Bench_Label_RSingle = new System.Windows.Forms.Label();
            this.Bench_Label_RSingleResult = new System.Windows.Forms.Label();
            this.Bench_TLP_T_P3 = new System.Windows.Forms.Panel();
            this.Bench_TimeSelector_List = new Glow.TSCustomComboBox();
            this.Bench_TimeCustom = new System.Windows.Forms.TextBox();
            this.Bench_TimeSelector = new System.Windows.Forms.Label();
            this.Bench_TLP_T_P1 = new System.Windows.Forms.Panel();
            this.Bench_CPUName = new System.Windows.Forms.Label();
            this.Bench_CPUCores = new System.Windows.Forms.Label();
            this.Bench_BG_Panel.SuspendLayout();
            this.BtnPanel.SuspendLayout();
            this.Bench_TLP_R_P2.SuspendLayout();
            this.Bench_TLP_T_P2.SuspendLayout();
            this.Bench_TLP_R_P1.SuspendLayout();
            this.Bench_TLP_T_P3.SuspendLayout();
            this.Bench_TLP_T_P1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Bench_BG_Panel
            // 
            this.Bench_BG_Panel.Controls.Add(this.BtnPanel);
            this.Bench_BG_Panel.Controls.Add(this.Bench_TLP_R_P2);
            this.Bench_BG_Panel.Controls.Add(this.Bench_TLP_T_P2);
            this.Bench_BG_Panel.Controls.Add(this.Bench_TLP_R_P1);
            this.Bench_BG_Panel.Controls.Add(this.Bench_TLP_T_P3);
            this.Bench_BG_Panel.Controls.Add(this.Bench_TLP_T_P1);
            this.Bench_BG_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_BG_Panel.Location = new System.Drawing.Point(0, 0);
            this.Bench_BG_Panel.Name = "Bench_BG_Panel";
            this.Bench_BG_Panel.Padding = new System.Windows.Forms.Padding(10);
            this.Bench_BG_Panel.Size = new System.Drawing.Size(734, 311);
            this.Bench_BG_Panel.TabIndex = 0;
            // 
            // BtnPanel
            // 
            this.BtnPanel.AutoSize = true;
            this.BtnPanel.Controls.Add(this.Bench_Start);
            this.BtnPanel.Controls.Add(this.Bench_Stop);
            this.BtnPanel.Location = new System.Drawing.Point(10, 263);
            this.BtnPanel.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPanel.Name = "BtnPanel";
            this.BtnPanel.Size = new System.Drawing.Size(714, 36);
            this.BtnPanel.TabIndex = 5;
            // 
            // Bench_Start
            // 
            this.Bench_Start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Bench_Start.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Bench_Start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Bench_Start.BorderColor = System.Drawing.Color.DodgerBlue;
            this.Bench_Start.BorderRadius = 10;
            this.Bench_Start.BorderSize = 0;
            this.Bench_Start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Start.FlatAppearance.BorderSize = 0;
            this.Bench_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bench_Start.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Bench_Start.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_Start.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bench_Start.Location = new System.Drawing.Point(0, 0);
            this.Bench_Start.Margin = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.Bench_Start.Name = "Bench_Start";
            this.Bench_Start.Size = new System.Drawing.Size(355, 36);
            this.Bench_Start.TabIndex = 0;
            this.Bench_Start.Text = "Başlat";
            this.Bench_Start.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bench_Start.TextColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_Start.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Bench_Start.UseVisualStyleBackColor = false;
            this.Bench_Start.Click += new System.EventHandler(this.Bench_Start_Click);
            // 
            // Bench_Stop
            // 
            this.Bench_Stop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Bench_Stop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Bench_Stop.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Bench_Stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Bench_Stop.BorderColor = System.Drawing.Color.DodgerBlue;
            this.Bench_Stop.BorderRadius = 10;
            this.Bench_Stop.BorderSize = 0;
            this.Bench_Stop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Stop.Enabled = false;
            this.Bench_Stop.FlatAppearance.BorderSize = 0;
            this.Bench_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bench_Stop.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Bench_Stop.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_Stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bench_Stop.Location = new System.Drawing.Point(359, 0);
            this.Bench_Stop.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.Bench_Stop.Name = "Bench_Stop";
            this.Bench_Stop.Size = new System.Drawing.Size(355, 36);
            this.Bench_Stop.TabIndex = 1;
            this.Bench_Stop.Text = "Durdur";
            this.Bench_Stop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bench_Stop.TextColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_Stop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Bench_Stop.UseVisualStyleBackColor = false;
            this.Bench_Stop.Click += new System.EventHandler(this.Bench_Stop_Click);
            // 
            // Bench_TLP_R_P2
            // 
            this.Bench_TLP_R_P2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Bench_TLP_R_P2.BackColor = System.Drawing.Color.Transparent;
            this.Bench_TLP_R_P2.Controls.Add(this.Bench_Label_RMulti);
            this.Bench_TLP_R_P2.Controls.Add(this.Bench_Label_RMultiResult);
            this.Bench_TLP_R_P2.Location = new System.Drawing.Point(369, 176);
            this.Bench_TLP_R_P2.Margin = new System.Windows.Forms.Padding(2, 0, 0, 10);
            this.Bench_TLP_R_P2.Name = "Bench_TLP_R_P2";
            this.Bench_TLP_R_P2.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_TLP_R_P2.Size = new System.Drawing.Size(355, 77);
            this.Bench_TLP_R_P2.TabIndex = 4;
            // 
            // Bench_Label_RMulti
            // 
            this.Bench_Label_RMulti.AutoSize = true;
            this.Bench_Label_RMulti.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_RMulti.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_RMulti.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RMulti.Location = new System.Drawing.Point(8, 10);
            this.Bench_Label_RMulti.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_Label_RMulti.Name = "Bench_Label_RMulti";
            this.Bench_Label_RMulti.Size = new System.Drawing.Size(182, 19);
            this.Bench_Label_RMulti.TabIndex = 0;
            this.Bench_Label_RMulti.Text = "Çoklu Çekirdek Performansı";
            // 
            // Bench_Label_RMultiResult
            // 
            this.Bench_Label_RMultiResult.AutoSize = true;
            this.Bench_Label_RMultiResult.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_RMultiResult.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_RMultiResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Bench_Label_RMultiResult.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RMultiResult.Location = new System.Drawing.Point(8, 39);
            this.Bench_Label_RMultiResult.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_Label_RMultiResult.Name = "Bench_Label_RMultiResult";
            this.Bench_Label_RMultiResult.Size = new System.Drawing.Size(142, 19);
            this.Bench_Label_RMultiResult.TabIndex = 1;
            this.Bench_Label_RMultiResult.Text = "Başlatma bekleniyor...";
            // 
            // Bench_TLP_T_P2
            // 
            this.Bench_TLP_T_P2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Bench_TLP_T_P2.BackColor = System.Drawing.Color.Transparent;
            this.Bench_TLP_T_P2.Controls.Add(this.Bench_ModeSelector_List);
            this.Bench_TLP_T_P2.Controls.Add(this.Bench_ModeSelector);
            this.Bench_TLP_T_P2.Location = new System.Drawing.Point(369, 89);
            this.Bench_TLP_T_P2.Margin = new System.Windows.Forms.Padding(2, 0, 0, 4);
            this.Bench_TLP_T_P2.Name = "Bench_TLP_T_P2";
            this.Bench_TLP_T_P2.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_TLP_T_P2.Size = new System.Drawing.Size(355, 83);
            this.Bench_TLP_T_P2.TabIndex = 2;
            // 
            // Bench_ModeSelector_List
            // 
            this.Bench_ModeSelector_List.ArrowColor = System.Drawing.SystemColors.WindowText;
            this.Bench_ModeSelector_List.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_ModeSelector_List.ButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_ModeSelector_List.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_ModeSelector_List.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.Bench_ModeSelector_List.DisabledButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_ModeSelector_List.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.Bench_ModeSelector_List.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.Bench_ModeSelector_List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bench_ModeSelector_List.FocusedBorderColor = System.Drawing.Color.DodgerBlue;
            this.Bench_ModeSelector_List.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.Bench_ModeSelector_List.FormattingEnabled = true;
            this.Bench_ModeSelector_List.HoverBackColor = System.Drawing.SystemColors.Window;
            this.Bench_ModeSelector_List.HoverButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_ModeSelector_List.Location = new System.Drawing.Point(14, 39);
            this.Bench_ModeSelector_List.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_ModeSelector_List.Name = "Bench_ModeSelector_List";
            this.Bench_ModeSelector_List.Size = new System.Drawing.Size(214, 28);
            this.Bench_ModeSelector_List.TabIndex = 1;
            // 
            // Bench_ModeSelector
            // 
            this.Bench_ModeSelector.AutoSize = true;
            this.Bench_ModeSelector.BackColor = System.Drawing.Color.Transparent;
            this.Bench_ModeSelector.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_ModeSelector.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_ModeSelector.Location = new System.Drawing.Point(10, 10);
            this.Bench_ModeSelector.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_ModeSelector.Name = "Bench_ModeSelector";
            this.Bench_ModeSelector.Size = new System.Drawing.Size(98, 19);
            this.Bench_ModeSelector.TabIndex = 0;
            this.Bench_ModeSelector.Text = "Modu Seçiniz:";
            // 
            // Bench_TLP_R_P1
            // 
            this.Bench_TLP_R_P1.BackColor = System.Drawing.Color.Transparent;
            this.Bench_TLP_R_P1.Controls.Add(this.Bench_Label_RSingle);
            this.Bench_TLP_R_P1.Controls.Add(this.Bench_Label_RSingleResult);
            this.Bench_TLP_R_P1.Location = new System.Drawing.Point(10, 176);
            this.Bench_TLP_R_P1.Margin = new System.Windows.Forms.Padding(0, 0, 2, 10);
            this.Bench_TLP_R_P1.Name = "Bench_TLP_R_P1";
            this.Bench_TLP_R_P1.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_TLP_R_P1.Size = new System.Drawing.Size(355, 77);
            this.Bench_TLP_R_P1.TabIndex = 3;
            // 
            // Bench_Label_RSingle
            // 
            this.Bench_Label_RSingle.AutoSize = true;
            this.Bench_Label_RSingle.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_RSingle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_RSingle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RSingle.Location = new System.Drawing.Point(10, 10);
            this.Bench_Label_RSingle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_Label_RSingle.Name = "Bench_Label_RSingle";
            this.Bench_Label_RSingle.Size = new System.Drawing.Size(175, 19);
            this.Bench_Label_RSingle.TabIndex = 0;
            this.Bench_Label_RSingle.Text = "Tekli Çekirdek Performansı";
            // 
            // Bench_Label_RSingleResult
            // 
            this.Bench_Label_RSingleResult.AutoSize = true;
            this.Bench_Label_RSingleResult.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_RSingleResult.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_RSingleResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Bench_Label_RSingleResult.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RSingleResult.Location = new System.Drawing.Point(10, 39);
            this.Bench_Label_RSingleResult.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_Label_RSingleResult.Name = "Bench_Label_RSingleResult";
            this.Bench_Label_RSingleResult.Size = new System.Drawing.Size(142, 19);
            this.Bench_Label_RSingleResult.TabIndex = 1;
            this.Bench_Label_RSingleResult.Text = "Başlatma bekleniyor...";
            // 
            // Bench_TLP_T_P3
            // 
            this.Bench_TLP_T_P3.BackColor = System.Drawing.Color.Transparent;
            this.Bench_TLP_T_P3.Controls.Add(this.Bench_TimeSelector_List);
            this.Bench_TLP_T_P3.Controls.Add(this.Bench_TimeCustom);
            this.Bench_TLP_T_P3.Controls.Add(this.Bench_TimeSelector);
            this.Bench_TLP_T_P3.Location = new System.Drawing.Point(10, 89);
            this.Bench_TLP_T_P3.Margin = new System.Windows.Forms.Padding(0, 0, 2, 4);
            this.Bench_TLP_T_P3.Name = "Bench_TLP_T_P3";
            this.Bench_TLP_T_P3.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_TLP_T_P3.Size = new System.Drawing.Size(355, 83);
            this.Bench_TLP_T_P3.TabIndex = 1;
            // 
            // Bench_TimeSelector_List
            // 
            this.Bench_TimeSelector_List.ArrowColor = System.Drawing.SystemColors.WindowText;
            this.Bench_TimeSelector_List.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_TimeSelector_List.ButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_TimeSelector_List.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_TimeSelector_List.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.Bench_TimeSelector_List.DisabledButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_TimeSelector_List.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.Bench_TimeSelector_List.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.Bench_TimeSelector_List.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bench_TimeSelector_List.FocusedBorderColor = System.Drawing.Color.DodgerBlue;
            this.Bench_TimeSelector_List.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.Bench_TimeSelector_List.FormattingEnabled = true;
            this.Bench_TimeSelector_List.HoverBackColor = System.Drawing.SystemColors.Window;
            this.Bench_TimeSelector_List.HoverButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_TimeSelector_List.Location = new System.Drawing.Point(14, 39);
            this.Bench_TimeSelector_List.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_TimeSelector_List.Name = "Bench_TimeSelector_List";
            this.Bench_TimeSelector_List.Size = new System.Drawing.Size(214, 28);
            this.Bench_TimeSelector_List.TabIndex = 1;
            this.Bench_TimeSelector_List.SelectedIndexChanged += new System.EventHandler(this.Bench_TimeSelector_List_SelectedIndexChanged);
            // 
            // Bench_TimeCustom
            // 
            this.Bench_TimeCustom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_TimeCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Bench_TimeCustom.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.Bench_TimeCustom.ForeColor = System.Drawing.Color.Black;
            this.Bench_TimeCustom.Location = new System.Drawing.Point(236, 40);
            this.Bench_TimeCustom.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.Bench_TimeCustom.MaxLength = 5;
            this.Bench_TimeCustom.Name = "Bench_TimeCustom";
            this.Bench_TimeCustom.Size = new System.Drawing.Size(60, 26);
            this.Bench_TimeCustom.TabIndex = 2;
            this.Bench_TimeCustom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OSD_TextBox_KeyPress);
            // 
            // Bench_TimeSelector
            // 
            this.Bench_TimeSelector.AutoSize = true;
            this.Bench_TimeSelector.BackColor = System.Drawing.Color.Transparent;
            this.Bench_TimeSelector.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_TimeSelector.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_TimeSelector.Location = new System.Drawing.Point(10, 10);
            this.Bench_TimeSelector.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_TimeSelector.Name = "Bench_TimeSelector";
            this.Bench_TimeSelector.Size = new System.Drawing.Size(100, 19);
            this.Bench_TimeSelector.TabIndex = 0;
            this.Bench_TimeSelector.Text = "Süreyi Seçiniz:";
            // 
            // Bench_TLP_T_P1
            // 
            this.Bench_TLP_T_P1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Bench_TLP_T_P1.Controls.Add(this.Bench_CPUName);
            this.Bench_TLP_T_P1.Controls.Add(this.Bench_CPUCores);
            this.Bench_TLP_T_P1.Location = new System.Drawing.Point(10, 10);
            this.Bench_TLP_T_P1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.Bench_TLP_T_P1.Name = "Bench_TLP_T_P1";
            this.Bench_TLP_T_P1.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_TLP_T_P1.Size = new System.Drawing.Size(714, 75);
            this.Bench_TLP_T_P1.TabIndex = 0;
            // 
            // Bench_CPUName
            // 
            this.Bench_CPUName.AutoSize = true;
            this.Bench_CPUName.BackColor = System.Drawing.Color.Transparent;
            this.Bench_CPUName.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_CPUName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_CPUName.Location = new System.Drawing.Point(10, 10);
            this.Bench_CPUName.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_CPUName.Name = "Bench_CPUName";
            this.Bench_CPUName.Size = new System.Drawing.Size(77, 19);
            this.Bench_CPUName.TabIndex = 0;
            this.Bench_CPUName.Text = "CPU Name";
            // 
            // Bench_CPUCores
            // 
            this.Bench_CPUCores.AutoSize = true;
            this.Bench_CPUCores.BackColor = System.Drawing.Color.Transparent;
            this.Bench_CPUCores.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_CPUCores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Bench_CPUCores.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_CPUCores.Location = new System.Drawing.Point(10, 39);
            this.Bench_CPUCores.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_CPUCores.Name = "Bench_CPUCores";
            this.Bench_CPUCores.Size = new System.Drawing.Size(86, 19);
            this.Bench_CPUCores.TabIndex = 1;
            this.Bench_CPUCores.Text = "CPU Feature";
            // 
            // GlowBenchCPU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(734, 311);
            this.Controls.Add(this.Bench_BG_Panel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = Properties.Resources.GlowLogo;
            this.MaximizeBox = false;
            this.Name = "GlowBenchCPU";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowCPUBench";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlowBenchCPU_FormClosing);
            this.Load += new System.EventHandler(this.GlowBenchCPU_Load);
            this.Bench_BG_Panel.ResumeLayout(false);
            this.Bench_BG_Panel.PerformLayout();
            this.BtnPanel.ResumeLayout(false);
            this.Bench_TLP_R_P2.ResumeLayout(false);
            this.Bench_TLP_R_P2.PerformLayout();
            this.Bench_TLP_T_P2.ResumeLayout(false);
            this.Bench_TLP_T_P2.PerformLayout();
            this.Bench_TLP_R_P1.ResumeLayout(false);
            this.Bench_TLP_R_P1.PerformLayout();
            this.Bench_TLP_T_P3.ResumeLayout(false);
            this.Bench_TLP_T_P3.PerformLayout();
            this.Bench_TLP_T_P1.ResumeLayout(false);
            this.Bench_TLP_T_P1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TSCustomButton Bench_Start;
        private TSCustomButton Bench_Stop;
        private System.Windows.Forms.Panel Bench_BG_Panel;
        private System.Windows.Forms.Panel Bench_TLP_R_P2;
        internal System.Windows.Forms.Label Bench_Label_RMulti;
        internal System.Windows.Forms.Label Bench_Label_RMultiResult;
        private System.Windows.Forms.Panel Bench_TLP_R_P1;
        internal System.Windows.Forms.Label Bench_Label_RSingle;
        internal System.Windows.Forms.Label Bench_Label_RSingleResult;
        private System.Windows.Forms.Panel Bench_TLP_T_P3;
        internal System.Windows.Forms.TextBox Bench_TimeCustom;
        internal System.Windows.Forms.Label Bench_TimeSelector;
        private System.Windows.Forms.Panel Bench_TLP_T_P2;
        internal System.Windows.Forms.Label Bench_ModeSelector;
        private System.Windows.Forms.Panel Bench_TLP_T_P1;
        internal System.Windows.Forms.Label Bench_CPUName;
        internal System.Windows.Forms.Label Bench_CPUCores;
        private TSCustomComboBox Bench_TimeSelector_List;
        private TSCustomComboBox Bench_ModeSelector_List;
        private System.Windows.Forms.Panel BtnPanel;
    }
}