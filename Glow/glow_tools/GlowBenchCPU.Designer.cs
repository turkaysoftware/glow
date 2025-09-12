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
            this.Bench_TLP_Top = new System.Windows.Forms.TableLayoutPanel();
            this.Bench_TLP_Result = new System.Windows.Forms.TableLayoutPanel();
            this.Bench_TLP_R_P2 = new System.Windows.Forms.Panel();
            this.Bench_Label_RMulti = new System.Windows.Forms.Label();
            this.Bench_Label_RMultiResult = new System.Windows.Forms.Label();
            this.Bench_TLP_R_P1 = new System.Windows.Forms.Panel();
            this.Bench_Label_RSingle = new System.Windows.Forms.Label();
            this.Bench_Label_RSingleResult = new System.Windows.Forms.Label();
            this.Bench_TLP_Bottom = new System.Windows.Forms.TableLayoutPanel();
            this.Bench_Stop = new Glow.TSCustomButton();
            this.Bench_Start = new Glow.TSCustomButton();
            this.Bench_TLP_T_P1 = new System.Windows.Forms.Panel();
            this.Bench_CPUName = new System.Windows.Forms.Label();
            this.Bench_CPUCores = new System.Windows.Forms.Label();
            this.BenchMode_TLP = new System.Windows.Forms.TableLayoutPanel();
            this.Bench_TLP_T_P2 = new System.Windows.Forms.Panel();
            this.BenchMode = new Glow.TSCustomComboBox();
            this.Bench_Label_Mode = new System.Windows.Forms.Label();
            this.Bench_TLP_T_P3 = new System.Windows.Forms.Panel();
            this.Bench_Time = new Glow.TSCustomComboBox();
            this.Bench_TimeCustom = new System.Windows.Forms.TextBox();
            this.Bench_Label_Time = new System.Windows.Forms.Label();
            this.Bench_BG_Panel.SuspendLayout();
            this.Bench_TLP_Top.SuspendLayout();
            this.Bench_TLP_Result.SuspendLayout();
            this.Bench_TLP_R_P2.SuspendLayout();
            this.Bench_TLP_R_P1.SuspendLayout();
            this.Bench_TLP_Bottom.SuspendLayout();
            this.Bench_TLP_T_P1.SuspendLayout();
            this.BenchMode_TLP.SuspendLayout();
            this.Bench_TLP_T_P2.SuspendLayout();
            this.Bench_TLP_T_P3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Bench_BG_Panel
            // 
            this.Bench_BG_Panel.Controls.Add(this.Bench_TLP_Top);
            this.Bench_BG_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_BG_Panel.Location = new System.Drawing.Point(0, 0);
            this.Bench_BG_Panel.Name = "Bench_BG_Panel";
            this.Bench_BG_Panel.Size = new System.Drawing.Size(734, 267);
            this.Bench_BG_Panel.TabIndex = 8;
            // 
            // Bench_TLP_Top
            // 
            this.Bench_TLP_Top.AutoSize = true;
            this.Bench_TLP_Top.ColumnCount = 1;
            this.Bench_TLP_Top.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Bench_TLP_Top.Controls.Add(this.Bench_TLP_Result, 0, 2);
            this.Bench_TLP_Top.Controls.Add(this.Bench_TLP_Bottom, 0, 3);
            this.Bench_TLP_Top.Controls.Add(this.Bench_TLP_T_P1, 0, 0);
            this.Bench_TLP_Top.Controls.Add(this.BenchMode_TLP, 0, 1);
            this.Bench_TLP_Top.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_Top.Location = new System.Drawing.Point(0, 0);
            this.Bench_TLP_Top.Name = "Bench_TLP_Top";
            this.Bench_TLP_Top.RowCount = 4;
            this.Bench_TLP_Top.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Bench_TLP_Top.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Bench_TLP_Top.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Bench_TLP_Top.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.Bench_TLP_Top.Size = new System.Drawing.Size(734, 267);
            this.Bench_TLP_Top.TabIndex = 0;
            // 
            // Bench_TLP_Result
            // 
            this.Bench_TLP_Result.ColumnCount = 2;
            this.Bench_TLP_Result.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Result.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Result.Controls.Add(this.Bench_TLP_R_P2, 1, 0);
            this.Bench_TLP_Result.Controls.Add(this.Bench_TLP_R_P1, 0, 0);
            this.Bench_TLP_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_Result.Location = new System.Drawing.Point(0, 150);
            this.Bench_TLP_Result.Margin = new System.Windows.Forms.Padding(0);
            this.Bench_TLP_Result.Name = "Bench_TLP_Result";
            this.Bench_TLP_Result.RowCount = 1;
            this.Bench_TLP_Result.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Result.Size = new System.Drawing.Size(734, 70);
            this.Bench_TLP_Result.TabIndex = 0;
            // 
            // Bench_TLP_R_P2
            // 
            this.Bench_TLP_R_P2.BackColor = System.Drawing.Color.Transparent;
            this.Bench_TLP_R_P2.Controls.Add(this.Bench_Label_RMulti);
            this.Bench_TLP_R_P2.Controls.Add(this.Bench_Label_RMultiResult);
            this.Bench_TLP_R_P2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_R_P2.Location = new System.Drawing.Point(368, 1);
            this.Bench_TLP_R_P2.Margin = new System.Windows.Forms.Padding(1, 1, 2, 1);
            this.Bench_TLP_R_P2.Name = "Bench_TLP_R_P2";
            this.Bench_TLP_R_P2.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_TLP_R_P2.Size = new System.Drawing.Size(364, 68);
            this.Bench_TLP_R_P2.TabIndex = 0;
            // 
            // Bench_Label_RMulti
            // 
            this.Bench_Label_RMulti.AutoSize = true;
            this.Bench_Label_RMulti.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_RMulti.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_RMulti.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RMulti.Location = new System.Drawing.Point(8, 5);
            this.Bench_Label_RMulti.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
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
            this.Bench_Label_RMultiResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            this.Bench_Label_RMultiResult.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RMultiResult.Location = new System.Drawing.Point(8, 29);
            this.Bench_Label_RMultiResult.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_RMultiResult.Name = "Bench_Label_RMultiResult";
            this.Bench_Label_RMultiResult.Size = new System.Drawing.Size(142, 19);
            this.Bench_Label_RMultiResult.TabIndex = 1;
            this.Bench_Label_RMultiResult.Text = "Başlatma bekleniyor...";
            // 
            // Bench_TLP_R_P1
            // 
            this.Bench_TLP_R_P1.BackColor = System.Drawing.Color.Transparent;
            this.Bench_TLP_R_P1.Controls.Add(this.Bench_Label_RSingle);
            this.Bench_TLP_R_P1.Controls.Add(this.Bench_Label_RSingleResult);
            this.Bench_TLP_R_P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_R_P1.Location = new System.Drawing.Point(3, 1);
            this.Bench_TLP_R_P1.Margin = new System.Windows.Forms.Padding(3, 1, 2, 1);
            this.Bench_TLP_R_P1.Name = "Bench_TLP_R_P1";
            this.Bench_TLP_R_P1.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_TLP_R_P1.Size = new System.Drawing.Size(362, 68);
            this.Bench_TLP_R_P1.TabIndex = 1;
            // 
            // Bench_Label_RSingle
            // 
            this.Bench_Label_RSingle.AutoSize = true;
            this.Bench_Label_RSingle.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_RSingle.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_RSingle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RSingle.Location = new System.Drawing.Point(6, 5);
            this.Bench_Label_RSingle.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
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
            this.Bench_Label_RSingleResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            this.Bench_Label_RSingleResult.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RSingleResult.Location = new System.Drawing.Point(6, 29);
            this.Bench_Label_RSingleResult.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_RSingleResult.Name = "Bench_Label_RSingleResult";
            this.Bench_Label_RSingleResult.Size = new System.Drawing.Size(142, 19);
            this.Bench_Label_RSingleResult.TabIndex = 1;
            this.Bench_Label_RSingleResult.Text = "Başlatma bekleniyor...";
            // 
            // Bench_TLP_Bottom
            // 
            this.Bench_TLP_Bottom.AutoSize = true;
            this.Bench_TLP_Bottom.ColumnCount = 2;
            this.Bench_TLP_Bottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Bottom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Bottom.Controls.Add(this.Bench_Stop, 1, 0);
            this.Bench_TLP_Bottom.Controls.Add(this.Bench_Start, 0, 0);
            this.Bench_TLP_Bottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_Bottom.Location = new System.Drawing.Point(0, 220);
            this.Bench_TLP_Bottom.Margin = new System.Windows.Forms.Padding(0);
            this.Bench_TLP_Bottom.MaximumSize = new System.Drawing.Size(800, 45);
            this.Bench_TLP_Bottom.MinimumSize = new System.Drawing.Size(600, 45);
            this.Bench_TLP_Bottom.Name = "Bench_TLP_Bottom";
            this.Bench_TLP_Bottom.RowCount = 1;
            this.Bench_TLP_Bottom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Bench_TLP_Bottom.Size = new System.Drawing.Size(734, 45);
            this.Bench_TLP_Bottom.TabIndex = 1;
            // 
            // Bench_Stop
            // 
            this.Bench_Stop.BackColor = System.Drawing.Color.RosyBrown;
            this.Bench_Stop.BackgroundColor = System.Drawing.Color.RosyBrown;
            this.Bench_Stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Bench_Stop.BorderColor = System.Drawing.Color.DodgerBlue;
            this.Bench_Stop.BorderRadius = 10;
            this.Bench_Stop.BorderSize = 0;
            this.Bench_Stop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_Stop.Enabled = false;
            this.Bench_Stop.FlatAppearance.BorderSize = 0;
            this.Bench_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bench_Stop.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Bench_Stop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Bench_Stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bench_Stop.Location = new System.Drawing.Point(369, 3);
            this.Bench_Stop.Margin = new System.Windows.Forms.Padding(2, 3, 3, 3);
            this.Bench_Stop.Name = "Bench_Stop";
            this.Bench_Stop.Size = new System.Drawing.Size(362, 39);
            this.Bench_Stop.TabIndex = 1;
            this.Bench_Stop.Text = "Durdur";
            this.Bench_Stop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bench_Stop.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Bench_Stop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Bench_Stop.UseVisualStyleBackColor = false;
            this.Bench_Stop.Click += new System.EventHandler(this.Bench_Stop_Click);
            // 
            // Bench_Start
            // 
            this.Bench_Start.BackColor = System.Drawing.Color.RosyBrown;
            this.Bench_Start.BackgroundColor = System.Drawing.Color.RosyBrown;
            this.Bench_Start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Bench_Start.BorderColor = System.Drawing.Color.DodgerBlue;
            this.Bench_Start.BorderRadius = 10;
            this.Bench_Start.BorderSize = 0;
            this.Bench_Start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_Start.FlatAppearance.BorderSize = 0;
            this.Bench_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bench_Start.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Bench_Start.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Bench_Start.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bench_Start.Location = new System.Drawing.Point(3, 3);
            this.Bench_Start.Margin = new System.Windows.Forms.Padding(3, 3, 2, 3);
            this.Bench_Start.Name = "Bench_Start";
            this.Bench_Start.Size = new System.Drawing.Size(362, 39);
            this.Bench_Start.TabIndex = 0;
            this.Bench_Start.Text = "Başlat";
            this.Bench_Start.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bench_Start.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Bench_Start.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Bench_Start.UseVisualStyleBackColor = false;
            this.Bench_Start.Click += new System.EventHandler(this.Bench_Start_Click);
            // 
            // Bench_TLP_T_P1
            // 
            this.Bench_TLP_T_P1.Controls.Add(this.Bench_CPUName);
            this.Bench_TLP_T_P1.Controls.Add(this.Bench_CPUCores);
            this.Bench_TLP_T_P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_T_P1.Location = new System.Drawing.Point(3, 3);
            this.Bench_TLP_T_P1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 2);
            this.Bench_TLP_T_P1.Name = "Bench_TLP_T_P1";
            this.Bench_TLP_T_P1.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_TLP_T_P1.Size = new System.Drawing.Size(728, 65);
            this.Bench_TLP_T_P1.TabIndex = 0;
            // 
            // Bench_CPUName
            // 
            this.Bench_CPUName.AutoSize = true;
            this.Bench_CPUName.BackColor = System.Drawing.Color.Transparent;
            this.Bench_CPUName.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_CPUName.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_CPUName.Location = new System.Drawing.Point(8, 5);
            this.Bench_CPUName.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_CPUName.Name = "Bench_CPUName";
            this.Bench_CPUName.Size = new System.Drawing.Size(82, 19);
            this.Bench_CPUName.TabIndex = 0;
            this.Bench_CPUName.Text = "Yükleniyor...";
            // 
            // Bench_CPUCores
            // 
            this.Bench_CPUCores.AutoSize = true;
            this.Bench_CPUCores.BackColor = System.Drawing.Color.Transparent;
            this.Bench_CPUCores.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_CPUCores.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            this.Bench_CPUCores.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_CPUCores.Location = new System.Drawing.Point(8, 34);
            this.Bench_CPUCores.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_CPUCores.Name = "Bench_CPUCores";
            this.Bench_CPUCores.Size = new System.Drawing.Size(82, 19);
            this.Bench_CPUCores.TabIndex = 1;
            this.Bench_CPUCores.Text = "Yükleniyor...";
            // 
            // BenchMode_TLP
            // 
            this.BenchMode_TLP.ColumnCount = 2;
            this.BenchMode_TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BenchMode_TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BenchMode_TLP.Controls.Add(this.Bench_TLP_T_P2, 1, 0);
            this.BenchMode_TLP.Controls.Add(this.Bench_TLP_T_P3, 0, 0);
            this.BenchMode_TLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BenchMode_TLP.Location = new System.Drawing.Point(0, 70);
            this.BenchMode_TLP.Margin = new System.Windows.Forms.Padding(0);
            this.BenchMode_TLP.Name = "BenchMode_TLP";
            this.BenchMode_TLP.RowCount = 1;
            this.BenchMode_TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.BenchMode_TLP.Size = new System.Drawing.Size(734, 80);
            this.BenchMode_TLP.TabIndex = 4;
            // 
            // Bench_TLP_T_P2
            // 
            this.Bench_TLP_T_P2.BackColor = System.Drawing.Color.Transparent;
            this.Bench_TLP_T_P2.Controls.Add(this.BenchMode);
            this.Bench_TLP_T_P2.Controls.Add(this.Bench_Label_Mode);
            this.Bench_TLP_T_P2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_T_P2.Location = new System.Drawing.Point(368, 1);
            this.Bench_TLP_T_P2.Margin = new System.Windows.Forms.Padding(1, 1, 2, 1);
            this.Bench_TLP_T_P2.Name = "Bench_TLP_T_P2";
            this.Bench_TLP_T_P2.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_TLP_T_P2.Size = new System.Drawing.Size(364, 78);
            this.Bench_TLP_T_P2.TabIndex = 1;
            // 
            // BenchMode
            // 
            this.BenchMode.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.BenchMode.ButtonColor = System.Drawing.SystemColors.ControlDark;
            this.BenchMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BenchMode.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.BenchMode.DisabledButtonColor = System.Drawing.SystemColors.ControlDark;
            this.BenchMode.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.BenchMode.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.BenchMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.BenchMode.FocusedBorderColor = System.Drawing.Color.DodgerBlue;
            this.BenchMode.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.BenchMode.FormattingEnabled = true;
            this.BenchMode.HoverBackColor = System.Drawing.SystemColors.Window;
            this.BenchMode.HoverButtonColor = System.Drawing.SystemColors.ControlDark;
            this.BenchMode.Location = new System.Drawing.Point(8, 33);
            this.BenchMode.Name = "BenchMode";
            this.BenchMode.Size = new System.Drawing.Size(214, 28);
            this.BenchMode.TabIndex = 1;
            // 
            // Bench_Label_Mode
            // 
            this.Bench_Label_Mode.AutoSize = true;
            this.Bench_Label_Mode.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_Mode.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_Mode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_Mode.Location = new System.Drawing.Point(6, 5);
            this.Bench_Label_Mode.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_Mode.Name = "Bench_Label_Mode";
            this.Bench_Label_Mode.Size = new System.Drawing.Size(98, 19);
            this.Bench_Label_Mode.TabIndex = 0;
            this.Bench_Label_Mode.Text = "Modu Seçiniz:";
            // 
            // Bench_TLP_T_P3
            // 
            this.Bench_TLP_T_P3.BackColor = System.Drawing.Color.Transparent;
            this.Bench_TLP_T_P3.Controls.Add(this.Bench_Time);
            this.Bench_TLP_T_P3.Controls.Add(this.Bench_TimeCustom);
            this.Bench_TLP_T_P3.Controls.Add(this.Bench_Label_Time);
            this.Bench_TLP_T_P3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_T_P3.Location = new System.Drawing.Point(3, 1);
            this.Bench_TLP_T_P3.Margin = new System.Windows.Forms.Padding(3, 1, 2, 1);
            this.Bench_TLP_T_P3.Name = "Bench_TLP_T_P3";
            this.Bench_TLP_T_P3.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_TLP_T_P3.Size = new System.Drawing.Size(362, 78);
            this.Bench_TLP_T_P3.TabIndex = 0;
            // 
            // Bench_Time
            // 
            this.Bench_Time.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Time.ButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Time.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Time.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.Bench_Time.DisabledButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Time.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.Bench_Time.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.Bench_Time.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bench_Time.FocusedBorderColor = System.Drawing.Color.DodgerBlue;
            this.Bench_Time.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.Bench_Time.FormattingEnabled = true;
            this.Bench_Time.HoverBackColor = System.Drawing.SystemColors.Window;
            this.Bench_Time.HoverButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Time.Location = new System.Drawing.Point(8, 33);
            this.Bench_Time.Name = "Bench_Time";
            this.Bench_Time.Size = new System.Drawing.Size(214, 28);
            this.Bench_Time.TabIndex = 1;
            this.Bench_Time.SelectedIndexChanged += new System.EventHandler(this.Bench_Time_SelectedIndexChanged);
            // 
            // Bench_TimeCustom
            // 
            this.Bench_TimeCustom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_TimeCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Bench_TimeCustom.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.Bench_TimeCustom.ForeColor = System.Drawing.Color.Black;
            this.Bench_TimeCustom.Location = new System.Drawing.Point(228, 34);
            this.Bench_TimeCustom.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.Bench_TimeCustom.MaxLength = 5;
            this.Bench_TimeCustom.Name = "Bench_TimeCustom";
            this.Bench_TimeCustom.Size = new System.Drawing.Size(60, 26);
            this.Bench_TimeCustom.TabIndex = 2;
            this.Bench_TimeCustom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OSD_TextBox_KeyPress);
            // 
            // Bench_Label_Time
            // 
            this.Bench_Label_Time.AutoSize = true;
            this.Bench_Label_Time.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_Time.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_Time.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_Time.Location = new System.Drawing.Point(6, 5);
            this.Bench_Label_Time.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_Time.Name = "Bench_Label_Time";
            this.Bench_Label_Time.Size = new System.Drawing.Size(100, 19);
            this.Bench_Label_Time.TabIndex = 0;
            this.Bench_Label_Time.Text = "Süreyi Seçiniz:";
            // 
            // GlowBenchCPU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(734, 267);
            this.Controls.Add(this.Bench_BG_Panel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GlowBenchCPU";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowCPUBench";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlowBenchCPU_FormClosing);
            this.Load += new System.EventHandler(this.GlowBenchCPU_Load);
            this.Bench_BG_Panel.ResumeLayout(false);
            this.Bench_BG_Panel.PerformLayout();
            this.Bench_TLP_Top.ResumeLayout(false);
            this.Bench_TLP_Top.PerformLayout();
            this.Bench_TLP_Result.ResumeLayout(false);
            this.Bench_TLP_R_P2.ResumeLayout(false);
            this.Bench_TLP_R_P2.PerformLayout();
            this.Bench_TLP_R_P1.ResumeLayout(false);
            this.Bench_TLP_R_P1.PerformLayout();
            this.Bench_TLP_Bottom.ResumeLayout(false);
            this.Bench_TLP_T_P1.ResumeLayout(false);
            this.Bench_TLP_T_P1.PerformLayout();
            this.BenchMode_TLP.ResumeLayout(false);
            this.Bench_TLP_T_P2.ResumeLayout(false);
            this.Bench_TLP_T_P2.PerformLayout();
            this.Bench_TLP_T_P3.ResumeLayout(false);
            this.Bench_TLP_T_P3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TSCustomButton Bench_Start;
        private TSCustomButton Bench_Stop;
        private System.Windows.Forms.Panel Bench_BG_Panel;
        private System.Windows.Forms.TableLayoutPanel Bench_TLP_Bottom;
        private System.Windows.Forms.TableLayoutPanel Bench_TLP_Top;
        private System.Windows.Forms.TableLayoutPanel Bench_TLP_Result;
        private System.Windows.Forms.Panel Bench_TLP_R_P2;
        internal System.Windows.Forms.Label Bench_Label_RMulti;
        internal System.Windows.Forms.Label Bench_Label_RMultiResult;
        private System.Windows.Forms.Panel Bench_TLP_R_P1;
        internal System.Windows.Forms.Label Bench_Label_RSingle;
        internal System.Windows.Forms.Label Bench_Label_RSingleResult;
        private System.Windows.Forms.Panel Bench_TLP_T_P3;
        internal System.Windows.Forms.TextBox Bench_TimeCustom;
        internal System.Windows.Forms.Label Bench_Label_Time;
        private System.Windows.Forms.Panel Bench_TLP_T_P2;
        internal System.Windows.Forms.Label Bench_Label_Mode;
        private System.Windows.Forms.Panel Bench_TLP_T_P1;
        internal System.Windows.Forms.Label Bench_CPUName;
        internal System.Windows.Forms.Label Bench_CPUCores;
        private System.Windows.Forms.TableLayoutPanel BenchMode_TLP;
        private TSCustomComboBox Bench_Time;
        private TSCustomComboBox BenchMode;
    }
}