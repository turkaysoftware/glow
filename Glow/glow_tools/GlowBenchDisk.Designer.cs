namespace Glow.glow_tools
{
    partial class GlowBenchDisk
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GlowBenchDisk));
            this.Bench_P1 = new System.Windows.Forms.Panel();
            this.Bench_Disk = new Glow.TSCustomComboBox();
            this.Bench_Label_Disk = new System.Windows.Forms.Label();
            this.Bench_P5 = new System.Windows.Forms.Panel();
            this.Bench_Buffer = new Glow.TSCustomComboBox();
            this.Bench_Label_Buffer = new System.Windows.Forms.Label();
            this.Bench_P2 = new System.Windows.Forms.Panel();
            this.Bench_Size = new Glow.TSCustomComboBox();
            this.Bench_SizeCustom = new System.Windows.Forms.TextBox();
            this.Bench_Label_Size = new System.Windows.Forms.Label();
            this.Bench_P7 = new System.Windows.Forms.Panel();
            this.Bench_R_Max_ReadSpeed = new System.Windows.Forms.Label();
            this.Bench_R_Max_ReadSpeed_V = new System.Windows.Forms.Label();
            this.Bench_P6 = new System.Windows.Forms.Panel();
            this.Bench_L_Max_WriteSpeed = new System.Windows.Forms.Label();
            this.Bench_L_Max_WriteSpeed_V = new System.Windows.Forms.Label();
            this.Bench_P4 = new System.Windows.Forms.Panel();
            this.Bench_R_ReadSpeed = new System.Windows.Forms.Label();
            this.Bench_R_ReadSpeed_V = new System.Windows.Forms.Label();
            this.Bench_P3 = new System.Windows.Forms.Panel();
            this.Bench_L_WriteSpeed = new System.Windows.Forms.Label();
            this.Bench_L_WriteSpeed_V = new System.Windows.Forms.Label();
            this.BackPanel = new System.Windows.Forms.Panel();
            this.BtnPanel = new System.Windows.Forms.Panel();
            this.Bench_Start = new Glow.TSCustomButton();
            this.Bench_Stop = new Glow.TSCustomButton();
            this.Bench_P1.SuspendLayout();
            this.Bench_P5.SuspendLayout();
            this.Bench_P2.SuspendLayout();
            this.Bench_P7.SuspendLayout();
            this.Bench_P6.SuspendLayout();
            this.Bench_P4.SuspendLayout();
            this.Bench_P3.SuspendLayout();
            this.BackPanel.SuspendLayout();
            this.BtnPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Bench_P1
            // 
            this.Bench_P1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Bench_P1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_P1.Controls.Add(this.Bench_Disk);
            this.Bench_P1.Controls.Add(this.Bench_Label_Disk);
            this.Bench_P1.Location = new System.Drawing.Point(10, 10);
            this.Bench_P1.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            this.Bench_P1.Name = "Bench_P1";
            this.Bench_P1.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_P1.Size = new System.Drawing.Size(764, 85);
            this.Bench_P1.TabIndex = 0;
            // 
            // Bench_Disk
            // 
            this.Bench_Disk.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Disk.ButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Disk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Disk.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.Bench_Disk.DisabledButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Disk.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.Bench_Disk.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.Bench_Disk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bench_Disk.FocusedBorderColor = System.Drawing.Color.DodgerBlue;
            this.Bench_Disk.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.Bench_Disk.FormattingEnabled = true;
            this.Bench_Disk.HoverBackColor = System.Drawing.SystemColors.Window;
            this.Bench_Disk.HoverButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Disk.Location = new System.Drawing.Point(14, 39);
            this.Bench_Disk.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_Disk.Name = "Bench_Disk";
            this.Bench_Disk.Size = new System.Drawing.Size(320, 28);
            this.Bench_Disk.TabIndex = 1;
            this.Bench_Disk.SelectedIndexChanged += new System.EventHandler(this.Bench_Disk_SelectedIndexChanged);
            // 
            // Bench_Label_Disk
            // 
            this.Bench_Label_Disk.AutoSize = true;
            this.Bench_Label_Disk.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_Disk.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_Disk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_Disk.Location = new System.Drawing.Point(10, 10);
            this.Bench_Label_Disk.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_Label_Disk.Name = "Bench_Label_Disk";
            this.Bench_Label_Disk.Size = new System.Drawing.Size(98, 19);
            this.Bench_Label_Disk.TabIndex = 0;
            this.Bench_Label_Disk.Text = "Modu Seçiniz:";
            // 
            // Bench_P5
            // 
            this.Bench_P5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Bench_P5.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_P5.Controls.Add(this.Bench_Buffer);
            this.Bench_P5.Controls.Add(this.Bench_Label_Buffer);
            this.Bench_P5.Location = new System.Drawing.Point(394, 99);
            this.Bench_P5.Margin = new System.Windows.Forms.Padding(2, 0, 0, 4);
            this.Bench_P5.Name = "Bench_P5";
            this.Bench_P5.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_P5.Size = new System.Drawing.Size(380, 83);
            this.Bench_P5.TabIndex = 2;
            // 
            // Bench_Buffer
            // 
            this.Bench_Buffer.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Buffer.ButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Buffer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Buffer.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.Bench_Buffer.DisabledButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Buffer.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.Bench_Buffer.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.Bench_Buffer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bench_Buffer.FocusedBorderColor = System.Drawing.Color.DodgerBlue;
            this.Bench_Buffer.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.Bench_Buffer.FormattingEnabled = true;
            this.Bench_Buffer.HoverBackColor = System.Drawing.SystemColors.Window;
            this.Bench_Buffer.HoverButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Buffer.Location = new System.Drawing.Point(12, 39);
            this.Bench_Buffer.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_Buffer.Name = "Bench_Buffer";
            this.Bench_Buffer.Size = new System.Drawing.Size(242, 28);
            this.Bench_Buffer.TabIndex = 1;
            // 
            // Bench_Label_Buffer
            // 
            this.Bench_Label_Buffer.AutoSize = true;
            this.Bench_Label_Buffer.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_Buffer.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_Buffer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_Buffer.Location = new System.Drawing.Point(10, 10);
            this.Bench_Label_Buffer.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_Label_Buffer.Name = "Bench_Label_Buffer";
            this.Bench_Label_Buffer.Size = new System.Drawing.Size(98, 19);
            this.Bench_Label_Buffer.TabIndex = 0;
            this.Bench_Label_Buffer.Text = "Modu Seçiniz:";
            // 
            // Bench_P2
            // 
            this.Bench_P2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_P2.Controls.Add(this.Bench_Size);
            this.Bench_P2.Controls.Add(this.Bench_SizeCustom);
            this.Bench_P2.Controls.Add(this.Bench_Label_Size);
            this.Bench_P2.Location = new System.Drawing.Point(10, 99);
            this.Bench_P2.Margin = new System.Windows.Forms.Padding(0, 0, 2, 4);
            this.Bench_P2.Name = "Bench_P2";
            this.Bench_P2.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_P2.Size = new System.Drawing.Size(380, 83);
            this.Bench_P2.TabIndex = 1;
            // 
            // Bench_Size
            // 
            this.Bench_Size.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Size.ButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Size.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Size.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.Bench_Size.DisabledButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Size.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.Bench_Size.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.Bench_Size.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bench_Size.FocusedBorderColor = System.Drawing.Color.DodgerBlue;
            this.Bench_Size.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold);
            this.Bench_Size.FormattingEnabled = true;
            this.Bench_Size.HoverBackColor = System.Drawing.SystemColors.Window;
            this.Bench_Size.HoverButtonColor = System.Drawing.SystemColors.ControlDark;
            this.Bench_Size.Location = new System.Drawing.Point(14, 39);
            this.Bench_Size.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_Size.Name = "Bench_Size";
            this.Bench_Size.Size = new System.Drawing.Size(242, 28);
            this.Bench_Size.TabIndex = 1;
            this.Bench_Size.SelectedIndexChanged += new System.EventHandler(this.Bench_Size_SelectedIndexChanged);
            // 
            // Bench_SizeCustom
            // 
            this.Bench_SizeCustom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_SizeCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Bench_SizeCustom.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.Bench_SizeCustom.ForeColor = System.Drawing.Color.Black;
            this.Bench_SizeCustom.Location = new System.Drawing.Point(264, 40);
            this.Bench_SizeCustom.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.Bench_SizeCustom.MaxLength = 3;
            this.Bench_SizeCustom.Name = "Bench_SizeCustom";
            this.Bench_SizeCustom.Size = new System.Drawing.Size(60, 26);
            this.Bench_SizeCustom.TabIndex = 2;
            this.Bench_SizeCustom.Visible = false;
            this.Bench_SizeCustom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Bench_SizeCustom_KeyPress);
            // 
            // Bench_Label_Size
            // 
            this.Bench_Label_Size.AutoSize = true;
            this.Bench_Label_Size.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_Size.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_Size.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_Size.Location = new System.Drawing.Point(10, 10);
            this.Bench_Label_Size.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_Label_Size.Name = "Bench_Label_Size";
            this.Bench_Label_Size.Size = new System.Drawing.Size(98, 19);
            this.Bench_Label_Size.TabIndex = 0;
            this.Bench_Label_Size.Text = "Modu Seçiniz:";
            // 
            // Bench_P7
            // 
            this.Bench_P7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Bench_P7.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_P7.Controls.Add(this.Bench_R_Max_ReadSpeed);
            this.Bench_P7.Controls.Add(this.Bench_R_Max_ReadSpeed_V);
            this.Bench_P7.Location = new System.Drawing.Point(394, 273);
            this.Bench_P7.Margin = new System.Windows.Forms.Padding(2, 0, 0, 10);
            this.Bench_P7.Name = "Bench_P7";
            this.Bench_P7.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_P7.Size = new System.Drawing.Size(380, 83);
            this.Bench_P7.TabIndex = 6;
            // 
            // Bench_R_Max_ReadSpeed
            // 
            this.Bench_R_Max_ReadSpeed.AutoSize = true;
            this.Bench_R_Max_ReadSpeed.BackColor = System.Drawing.Color.Transparent;
            this.Bench_R_Max_ReadSpeed.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_R_Max_ReadSpeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_R_Max_ReadSpeed.Location = new System.Drawing.Point(10, 10);
            this.Bench_R_Max_ReadSpeed.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_R_Max_ReadSpeed.Name = "Bench_R_Max_ReadSpeed";
            this.Bench_R_Max_ReadSpeed.Size = new System.Drawing.Size(182, 19);
            this.Bench_R_Max_ReadSpeed.TabIndex = 0;
            this.Bench_R_Max_ReadSpeed.Text = "Çoklu Çekirdek Performansı";
            // 
            // Bench_R_Max_ReadSpeed_V
            // 
            this.Bench_R_Max_ReadSpeed_V.AutoSize = true;
            this.Bench_R_Max_ReadSpeed_V.BackColor = System.Drawing.Color.Transparent;
            this.Bench_R_Max_ReadSpeed_V.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_R_Max_ReadSpeed_V.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Bench_R_Max_ReadSpeed_V.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_R_Max_ReadSpeed_V.Location = new System.Drawing.Point(10, 39);
            this.Bench_R_Max_ReadSpeed_V.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_R_Max_ReadSpeed_V.Name = "Bench_R_Max_ReadSpeed_V";
            this.Bench_R_Max_ReadSpeed_V.Size = new System.Drawing.Size(142, 19);
            this.Bench_R_Max_ReadSpeed_V.TabIndex = 1;
            this.Bench_R_Max_ReadSpeed_V.Text = "Başlatma bekleniyor...";
            // 
            // Bench_P6
            // 
            this.Bench_P6.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_P6.Controls.Add(this.Bench_L_Max_WriteSpeed);
            this.Bench_P6.Controls.Add(this.Bench_L_Max_WriteSpeed_V);
            this.Bench_P6.Location = new System.Drawing.Point(10, 273);
            this.Bench_P6.Margin = new System.Windows.Forms.Padding(0, 0, 2, 10);
            this.Bench_P6.Name = "Bench_P6";
            this.Bench_P6.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_P6.Size = new System.Drawing.Size(380, 83);
            this.Bench_P6.TabIndex = 5;
            // 
            // Bench_L_Max_WriteSpeed
            // 
            this.Bench_L_Max_WriteSpeed.AutoSize = true;
            this.Bench_L_Max_WriteSpeed.BackColor = System.Drawing.Color.Transparent;
            this.Bench_L_Max_WriteSpeed.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_L_Max_WriteSpeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_L_Max_WriteSpeed.Location = new System.Drawing.Point(10, 10);
            this.Bench_L_Max_WriteSpeed.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_L_Max_WriteSpeed.Name = "Bench_L_Max_WriteSpeed";
            this.Bench_L_Max_WriteSpeed.Size = new System.Drawing.Size(182, 19);
            this.Bench_L_Max_WriteSpeed.TabIndex = 0;
            this.Bench_L_Max_WriteSpeed.Text = "Çoklu Çekirdek Performansı";
            // 
            // Bench_L_Max_WriteSpeed_V
            // 
            this.Bench_L_Max_WriteSpeed_V.AutoSize = true;
            this.Bench_L_Max_WriteSpeed_V.BackColor = System.Drawing.Color.Transparent;
            this.Bench_L_Max_WriteSpeed_V.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_L_Max_WriteSpeed_V.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Bench_L_Max_WriteSpeed_V.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_L_Max_WriteSpeed_V.Location = new System.Drawing.Point(10, 39);
            this.Bench_L_Max_WriteSpeed_V.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_L_Max_WriteSpeed_V.Name = "Bench_L_Max_WriteSpeed_V";
            this.Bench_L_Max_WriteSpeed_V.Size = new System.Drawing.Size(142, 19);
            this.Bench_L_Max_WriteSpeed_V.TabIndex = 1;
            this.Bench_L_Max_WriteSpeed_V.Text = "Başlatma bekleniyor...";
            // 
            // Bench_P4
            // 
            this.Bench_P4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Bench_P4.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_P4.Controls.Add(this.Bench_R_ReadSpeed);
            this.Bench_P4.Controls.Add(this.Bench_R_ReadSpeed_V);
            this.Bench_P4.Location = new System.Drawing.Point(394, 186);
            this.Bench_P4.Margin = new System.Windows.Forms.Padding(2, 0, 0, 4);
            this.Bench_P4.Name = "Bench_P4";
            this.Bench_P4.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_P4.Size = new System.Drawing.Size(380, 83);
            this.Bench_P4.TabIndex = 4;
            // 
            // Bench_R_ReadSpeed
            // 
            this.Bench_R_ReadSpeed.AutoSize = true;
            this.Bench_R_ReadSpeed.BackColor = System.Drawing.Color.Transparent;
            this.Bench_R_ReadSpeed.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_R_ReadSpeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_R_ReadSpeed.Location = new System.Drawing.Point(10, 10);
            this.Bench_R_ReadSpeed.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_R_ReadSpeed.Name = "Bench_R_ReadSpeed";
            this.Bench_R_ReadSpeed.Size = new System.Drawing.Size(182, 19);
            this.Bench_R_ReadSpeed.TabIndex = 0;
            this.Bench_R_ReadSpeed.Text = "Çoklu Çekirdek Performansı";
            // 
            // Bench_R_ReadSpeed_V
            // 
            this.Bench_R_ReadSpeed_V.AutoSize = true;
            this.Bench_R_ReadSpeed_V.BackColor = System.Drawing.Color.Transparent;
            this.Bench_R_ReadSpeed_V.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_R_ReadSpeed_V.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Bench_R_ReadSpeed_V.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_R_ReadSpeed_V.Location = new System.Drawing.Point(10, 39);
            this.Bench_R_ReadSpeed_V.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_R_ReadSpeed_V.Name = "Bench_R_ReadSpeed_V";
            this.Bench_R_ReadSpeed_V.Size = new System.Drawing.Size(142, 19);
            this.Bench_R_ReadSpeed_V.TabIndex = 1;
            this.Bench_R_ReadSpeed_V.Text = "Başlatma bekleniyor...";
            // 
            // Bench_P3
            // 
            this.Bench_P3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_P3.Controls.Add(this.Bench_L_WriteSpeed);
            this.Bench_P3.Controls.Add(this.Bench_L_WriteSpeed_V);
            this.Bench_P3.Location = new System.Drawing.Point(10, 186);
            this.Bench_P3.Margin = new System.Windows.Forms.Padding(0, 0, 2, 4);
            this.Bench_P3.Name = "Bench_P3";
            this.Bench_P3.Padding = new System.Windows.Forms.Padding(5, 10, 5, 10);
            this.Bench_P3.Size = new System.Drawing.Size(380, 83);
            this.Bench_P3.TabIndex = 3;
            // 
            // Bench_L_WriteSpeed
            // 
            this.Bench_L_WriteSpeed.AutoSize = true;
            this.Bench_L_WriteSpeed.BackColor = System.Drawing.Color.Transparent;
            this.Bench_L_WriteSpeed.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_L_WriteSpeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_L_WriteSpeed.Location = new System.Drawing.Point(10, 10);
            this.Bench_L_WriteSpeed.Margin = new System.Windows.Forms.Padding(5, 0, 5, 10);
            this.Bench_L_WriteSpeed.Name = "Bench_L_WriteSpeed";
            this.Bench_L_WriteSpeed.Size = new System.Drawing.Size(182, 19);
            this.Bench_L_WriteSpeed.TabIndex = 0;
            this.Bench_L_WriteSpeed.Text = "Çoklu Çekirdek Performansı";
            // 
            // Bench_L_WriteSpeed_V
            // 
            this.Bench_L_WriteSpeed_V.AutoSize = true;
            this.Bench_L_WriteSpeed_V.BackColor = System.Drawing.Color.Transparent;
            this.Bench_L_WriteSpeed_V.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_L_WriteSpeed_V.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(95)))), ((int)(((byte)(146)))));
            this.Bench_L_WriteSpeed_V.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_L_WriteSpeed_V.Location = new System.Drawing.Point(10, 39);
            this.Bench_L_WriteSpeed_V.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Bench_L_WriteSpeed_V.Name = "Bench_L_WriteSpeed_V";
            this.Bench_L_WriteSpeed_V.Size = new System.Drawing.Size(142, 19);
            this.Bench_L_WriteSpeed_V.TabIndex = 1;
            this.Bench_L_WriteSpeed_V.Text = "Başlatma bekleniyor...";
            // 
            // BackPanel
            // 
            this.BackPanel.Controls.Add(this.BtnPanel);
            this.BackPanel.Controls.Add(this.Bench_P1);
            this.BackPanel.Controls.Add(this.Bench_P2);
            this.BackPanel.Controls.Add(this.Bench_P7);
            this.BackPanel.Controls.Add(this.Bench_P5);
            this.BackPanel.Controls.Add(this.Bench_P3);
            this.BackPanel.Controls.Add(this.Bench_P6);
            this.BackPanel.Controls.Add(this.Bench_P4);
            this.BackPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BackPanel.Location = new System.Drawing.Point(0, 0);
            this.BackPanel.Name = "BackPanel";
            this.BackPanel.Padding = new System.Windows.Forms.Padding(10);
            this.BackPanel.Size = new System.Drawing.Size(784, 413);
            this.BackPanel.TabIndex = 0;
            // 
            // BtnPanel
            // 
            this.BtnPanel.Controls.Add(this.Bench_Start);
            this.BtnPanel.Controls.Add(this.Bench_Stop);
            this.BtnPanel.Location = new System.Drawing.Point(10, 366);
            this.BtnPanel.Margin = new System.Windows.Forms.Padding(0);
            this.BtnPanel.Name = "BtnPanel";
            this.BtnPanel.Size = new System.Drawing.Size(764, 36);
            this.BtnPanel.TabIndex = 7;
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
            this.Bench_Start.Size = new System.Drawing.Size(380, 36);
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
            this.Bench_Stop.Location = new System.Drawing.Point(384, 0);
            this.Bench_Stop.Margin = new System.Windows.Forms.Padding(2, 0, 0, 0);
            this.Bench_Stop.Name = "Bench_Stop";
            this.Bench_Stop.Size = new System.Drawing.Size(380, 36);
            this.Bench_Stop.TabIndex = 1;
            this.Bench_Stop.Text = "Durdur";
            this.Bench_Stop.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bench_Stop.TextColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_Stop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Bench_Stop.UseVisualStyleBackColor = false;
            this.Bench_Stop.Click += new System.EventHandler(this.Bench_Stop_Click);
            // 
            // GlowBenchDisk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(784, 413);
            this.Controls.Add(this.BackPanel);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "GlowBenchDisk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowBenchDisk";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlowBenchDisk_FormClosing);
            this.Load += new System.EventHandler(this.GlowBenchDisk_Load);
            this.Bench_P1.ResumeLayout(false);
            this.Bench_P1.PerformLayout();
            this.Bench_P5.ResumeLayout(false);
            this.Bench_P5.PerformLayout();
            this.Bench_P2.ResumeLayout(false);
            this.Bench_P2.PerformLayout();
            this.Bench_P7.ResumeLayout(false);
            this.Bench_P7.PerformLayout();
            this.Bench_P6.ResumeLayout(false);
            this.Bench_P6.PerformLayout();
            this.Bench_P4.ResumeLayout(false);
            this.Bench_P4.PerformLayout();
            this.Bench_P3.ResumeLayout(false);
            this.Bench_P3.PerformLayout();
            this.BackPanel.ResumeLayout(false);
            this.BtnPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private TSCustomButton Bench_Stop;
        private TSCustomButton Bench_Start;
        private System.Windows.Forms.Panel Bench_P1;
        internal System.Windows.Forms.Label Bench_Label_Disk;
        internal System.Windows.Forms.Label Bench_Label_Size;
        internal System.Windows.Forms.TextBox Bench_SizeCustom;
        private System.Windows.Forms.Panel Bench_P5;
        private System.Windows.Forms.Panel Bench_P2;
        internal System.Windows.Forms.Label Bench_Label_Buffer;
        private System.Windows.Forms.Panel Bench_P7;
        private System.Windows.Forms.Panel Bench_P6;
        private System.Windows.Forms.Panel Bench_P4;
        private System.Windows.Forms.Panel Bench_P3;
        internal System.Windows.Forms.Label Bench_R_Max_ReadSpeed;
        internal System.Windows.Forms.Label Bench_R_Max_ReadSpeed_V;
        internal System.Windows.Forms.Label Bench_L_Max_WriteSpeed;
        internal System.Windows.Forms.Label Bench_L_Max_WriteSpeed_V;
        internal System.Windows.Forms.Label Bench_R_ReadSpeed;
        internal System.Windows.Forms.Label Bench_R_ReadSpeed_V;
        internal System.Windows.Forms.Label Bench_L_WriteSpeed;
        internal System.Windows.Forms.Label Bench_L_WriteSpeed_V;
        private TSCustomComboBox Bench_Disk;
        private TSCustomComboBox Bench_Size;
        private TSCustomComboBox Bench_Buffer;
        private System.Windows.Forms.Panel BackPanel;
        private System.Windows.Forms.Panel BtnPanel;
    }
}