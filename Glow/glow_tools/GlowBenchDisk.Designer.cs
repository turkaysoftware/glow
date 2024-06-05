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
            this.Bench_TLP = new System.Windows.Forms.TableLayoutPanel();
            this.Bench_TLP_BTN = new System.Windows.Forms.TableLayoutPanel();
            this.Bench_Stop = new System.Windows.Forms.Button();
            this.Bench_Start = new System.Windows.Forms.Button();
            this.Bench_TLP_Header = new System.Windows.Forms.TableLayoutPanel();
            this.Bench_P1 = new System.Windows.Forms.Panel();
            this.Bench_Label_Disk = new System.Windows.Forms.Label();
            this.Bench_Disk = new System.Windows.Forms.ComboBox();
            this.Bench_TLP_Modes = new System.Windows.Forms.TableLayoutPanel();
            this.Bench_P5 = new System.Windows.Forms.Panel();
            this.Bench_Buffer = new System.Windows.Forms.ComboBox();
            this.Bench_Label_Buffer = new System.Windows.Forms.Label();
            this.Bench_P2 = new System.Windows.Forms.Panel();
            this.Bench_SizeCustom = new System.Windows.Forms.TextBox();
            this.Bench_Size = new System.Windows.Forms.ComboBox();
            this.Bench_Label_Size = new System.Windows.Forms.Label();
            this.Bench_TLP_Result = new System.Windows.Forms.TableLayoutPanel();
            this.Bench_P7 = new System.Windows.Forms.Panel();
            this.Bench_Label_RSpeedRead = new System.Windows.Forms.Label();
            this.Bench_Label_RSpeedReadResult = new System.Windows.Forms.Label();
            this.Bench_P6 = new System.Windows.Forms.Panel();
            this.Bench_Label_LTimeRead = new System.Windows.Forms.Label();
            this.Bench_Label_LTimeReadResult = new System.Windows.Forms.Label();
            this.Bench_P4 = new System.Windows.Forms.Panel();
            this.Bench_Label_RSpeedWrite = new System.Windows.Forms.Label();
            this.Bench_Label_RSpeedWriteResult = new System.Windows.Forms.Label();
            this.Bench_P3 = new System.Windows.Forms.Panel();
            this.Bench_Label_LTimeWrite = new System.Windows.Forms.Label();
            this.Bench_Label_LTimeWriteResult = new System.Windows.Forms.Label();
            this.Bench_TLP.SuspendLayout();
            this.Bench_TLP_BTN.SuspendLayout();
            this.Bench_TLP_Header.SuspendLayout();
            this.Bench_P1.SuspendLayout();
            this.Bench_TLP_Modes.SuspendLayout();
            this.Bench_P5.SuspendLayout();
            this.Bench_P2.SuspendLayout();
            this.Bench_TLP_Result.SuspendLayout();
            this.Bench_P7.SuspendLayout();
            this.Bench_P6.SuspendLayout();
            this.Bench_P4.SuspendLayout();
            this.Bench_P3.SuspendLayout();
            this.SuspendLayout();
            // 
            // Bench_TLP
            // 
            this.Bench_TLP.ColumnCount = 1;
            this.Bench_TLP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Bench_TLP.Controls.Add(this.Bench_TLP_BTN, 0, 1);
            this.Bench_TLP.Controls.Add(this.Bench_TLP_Header, 0, 0);
            this.Bench_TLP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP.Location = new System.Drawing.Point(5, 5);
            this.Bench_TLP.Name = "Bench_TLP";
            this.Bench_TLP.RowCount = 2;
            this.Bench_TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87F));
            this.Bench_TLP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13F));
            this.Bench_TLP.Size = new System.Drawing.Size(774, 351);
            this.Bench_TLP.TabIndex = 0;
            // 
            // Bench_TLP_BTN
            // 
            this.Bench_TLP_BTN.ColumnCount = 2;
            this.Bench_TLP_BTN.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_BTN.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_BTN.Controls.Add(this.Bench_Stop, 0, 0);
            this.Bench_TLP_BTN.Controls.Add(this.Bench_Start, 0, 0);
            this.Bench_TLP_BTN.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_BTN.Location = new System.Drawing.Point(0, 305);
            this.Bench_TLP_BTN.Margin = new System.Windows.Forms.Padding(0);
            this.Bench_TLP_BTN.Name = "Bench_TLP_BTN";
            this.Bench_TLP_BTN.RowCount = 1;
            this.Bench_TLP_BTN.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_BTN.Size = new System.Drawing.Size(774, 46);
            this.Bench_TLP_BTN.TabIndex = 1;
            // 
            // Bench_Stop
            // 
            this.Bench_Stop.BackColor = System.Drawing.Color.RosyBrown;
            this.Bench_Stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Bench_Stop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Stop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_Stop.Enabled = false;
            this.Bench_Stop.FlatAppearance.BorderSize = 0;
            this.Bench_Stop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bench_Stop.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Bench_Stop.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Bench_Stop.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bench_Stop.Location = new System.Drawing.Point(390, 3);
            this.Bench_Stop.Name = "Bench_Stop";
            this.Bench_Stop.Size = new System.Drawing.Size(381, 40);
            this.Bench_Stop.TabIndex = 1;
            this.Bench_Stop.Text = "Durdur";
            this.Bench_Stop.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Bench_Stop.UseVisualStyleBackColor = false;
            this.Bench_Stop.Click += new System.EventHandler(this.Bench_Stop_Click);
            // 
            // Bench_Start
            // 
            this.Bench_Start.BackColor = System.Drawing.Color.RosyBrown;
            this.Bench_Start.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Bench_Start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Start.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_Start.FlatAppearance.BorderSize = 0;
            this.Bench_Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bench_Start.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Bench_Start.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.Bench_Start.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.Bench_Start.Location = new System.Drawing.Point(3, 3);
            this.Bench_Start.Name = "Bench_Start";
            this.Bench_Start.Size = new System.Drawing.Size(381, 40);
            this.Bench_Start.TabIndex = 0;
            this.Bench_Start.Text = "BAŞLAT";
            this.Bench_Start.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.Bench_Start.UseVisualStyleBackColor = false;
            this.Bench_Start.Click += new System.EventHandler(this.Bench_Start_Click);
            // 
            // Bench_TLP_Header
            // 
            this.Bench_TLP_Header.ColumnCount = 1;
            this.Bench_TLP_Header.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.Bench_TLP_Header.Controls.Add(this.Bench_P1, 0, 0);
            this.Bench_TLP_Header.Controls.Add(this.Bench_TLP_Modes, 0, 1);
            this.Bench_TLP_Header.Controls.Add(this.Bench_TLP_Result, 0, 2);
            this.Bench_TLP_Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_Header.Location = new System.Drawing.Point(0, 0);
            this.Bench_TLP_Header.Margin = new System.Windows.Forms.Padding(0);
            this.Bench_TLP_Header.Name = "Bench_TLP_Header";
            this.Bench_TLP_Header.RowCount = 3;
            this.Bench_TLP_Header.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24F));
            this.Bench_TLP_Header.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26F));
            this.Bench_TLP_Header.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Header.Size = new System.Drawing.Size(774, 305);
            this.Bench_TLP_Header.TabIndex = 0;
            // 
            // Bench_P1
            // 
            this.Bench_P1.BackColor = System.Drawing.Color.Silver;
            this.Bench_P1.Controls.Add(this.Bench_Label_Disk);
            this.Bench_P1.Controls.Add(this.Bench_Disk);
            this.Bench_P1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_P1.Location = new System.Drawing.Point(3, 3);
            this.Bench_P1.Name = "Bench_P1";
            this.Bench_P1.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_P1.Size = new System.Drawing.Size(768, 67);
            this.Bench_P1.TabIndex = 1;
            // 
            // Bench_Label_Disk
            // 
            this.Bench_Label_Disk.AutoSize = true;
            this.Bench_Label_Disk.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_Disk.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_Disk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_Disk.Location = new System.Drawing.Point(8, 5);
            this.Bench_Label_Disk.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_Disk.Name = "Bench_Label_Disk";
            this.Bench_Label_Disk.Size = new System.Drawing.Size(98, 19);
            this.Bench_Label_Disk.TabIndex = 0;
            this.Bench_Label_Disk.Text = "Modu Seçiniz:";
            // 
            // Bench_Disk
            // 
            this.Bench_Disk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.Bench_Disk.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Disk.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bench_Disk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bench_Disk.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.Bench_Disk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            this.Bench_Disk.FormattingEnabled = true;
            this.Bench_Disk.Location = new System.Drawing.Point(12, 30);
            this.Bench_Disk.Name = "Bench_Disk";
            this.Bench_Disk.Size = new System.Drawing.Size(319, 28);
            this.Bench_Disk.TabIndex = 1;
            // 
            // Bench_TLP_Modes
            // 
            this.Bench_TLP_Modes.ColumnCount = 2;
            this.Bench_TLP_Modes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Modes.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Modes.Controls.Add(this.Bench_P5, 1, 0);
            this.Bench_TLP_Modes.Controls.Add(this.Bench_P2, 0, 0);
            this.Bench_TLP_Modes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_Modes.Location = new System.Drawing.Point(0, 73);
            this.Bench_TLP_Modes.Margin = new System.Windows.Forms.Padding(0);
            this.Bench_TLP_Modes.Name = "Bench_TLP_Modes";
            this.Bench_TLP_Modes.RowCount = 1;
            this.Bench_TLP_Modes.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Modes.Size = new System.Drawing.Size(774, 79);
            this.Bench_TLP_Modes.TabIndex = 2;
            // 
            // Bench_P5
            // 
            this.Bench_P5.BackColor = System.Drawing.Color.Silver;
            this.Bench_P5.Controls.Add(this.Bench_Buffer);
            this.Bench_P5.Controls.Add(this.Bench_Label_Buffer);
            this.Bench_P5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_P5.Location = new System.Drawing.Point(390, 3);
            this.Bench_P5.Name = "Bench_P5";
            this.Bench_P5.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_P5.Size = new System.Drawing.Size(381, 73);
            this.Bench_P5.TabIndex = 1;
            // 
            // Bench_Buffer
            // 
            this.Bench_Buffer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.Bench_Buffer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Buffer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bench_Buffer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bench_Buffer.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.Bench_Buffer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            this.Bench_Buffer.FormattingEnabled = true;
            this.Bench_Buffer.Location = new System.Drawing.Point(12, 30);
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
            this.Bench_Label_Buffer.Location = new System.Drawing.Point(8, 5);
            this.Bench_Label_Buffer.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_Buffer.Name = "Bench_Label_Buffer";
            this.Bench_Label_Buffer.Size = new System.Drawing.Size(98, 19);
            this.Bench_Label_Buffer.TabIndex = 0;
            this.Bench_Label_Buffer.Text = "Modu Seçiniz:";
            // 
            // Bench_P2
            // 
            this.Bench_P2.BackColor = System.Drawing.Color.Silver;
            this.Bench_P2.Controls.Add(this.Bench_SizeCustom);
            this.Bench_P2.Controls.Add(this.Bench_Size);
            this.Bench_P2.Controls.Add(this.Bench_Label_Size);
            this.Bench_P2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_P2.Location = new System.Drawing.Point(3, 3);
            this.Bench_P2.Name = "Bench_P2";
            this.Bench_P2.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_P2.Size = new System.Drawing.Size(381, 73);
            this.Bench_P2.TabIndex = 0;
            // 
            // Bench_SizeCustom
            // 
            this.Bench_SizeCustom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Bench_SizeCustom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Bench_SizeCustom.Font = new System.Drawing.Font("Segoe UI Semibold", 10.5F, System.Drawing.FontStyle.Bold);
            this.Bench_SizeCustom.ForeColor = System.Drawing.Color.Black;
            this.Bench_SizeCustom.Location = new System.Drawing.Point(260, 31);
            this.Bench_SizeCustom.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.Bench_SizeCustom.MaxLength = 4;
            this.Bench_SizeCustom.Name = "Bench_SizeCustom";
            this.Bench_SizeCustom.Size = new System.Drawing.Size(60, 26);
            this.Bench_SizeCustom.TabIndex = 2;
            this.Bench_SizeCustom.Visible = false;
            this.Bench_SizeCustom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Bench_SizeCustom_KeyPress);
            // 
            // Bench_Size
            // 
            this.Bench_Size.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.Bench_Size.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Bench_Size.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Bench_Size.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Bench_Size.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.Bench_Size.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            this.Bench_Size.FormattingEnabled = true;
            this.Bench_Size.Location = new System.Drawing.Point(12, 30);
            this.Bench_Size.Name = "Bench_Size";
            this.Bench_Size.Size = new System.Drawing.Size(242, 28);
            this.Bench_Size.TabIndex = 1;
            this.Bench_Size.SelectedIndexChanged += new System.EventHandler(this.Bench_Size_SelectedIndexChanged);
            // 
            // Bench_Label_Size
            // 
            this.Bench_Label_Size.AutoSize = true;
            this.Bench_Label_Size.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_Size.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_Size.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_Size.Location = new System.Drawing.Point(8, 5);
            this.Bench_Label_Size.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_Size.Name = "Bench_Label_Size";
            this.Bench_Label_Size.Size = new System.Drawing.Size(98, 19);
            this.Bench_Label_Size.TabIndex = 0;
            this.Bench_Label_Size.Text = "Modu Seçiniz:";
            // 
            // Bench_TLP_Result
            // 
            this.Bench_TLP_Result.ColumnCount = 2;
            this.Bench_TLP_Result.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Result.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Result.Controls.Add(this.Bench_P7, 1, 1);
            this.Bench_TLP_Result.Controls.Add(this.Bench_P6, 0, 1);
            this.Bench_TLP_Result.Controls.Add(this.Bench_P4, 1, 0);
            this.Bench_TLP_Result.Controls.Add(this.Bench_P3, 0, 0);
            this.Bench_TLP_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_TLP_Result.Location = new System.Drawing.Point(0, 152);
            this.Bench_TLP_Result.Margin = new System.Windows.Forms.Padding(0);
            this.Bench_TLP_Result.Name = "Bench_TLP_Result";
            this.Bench_TLP_Result.RowCount = 2;
            this.Bench_TLP_Result.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Result.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.Bench_TLP_Result.Size = new System.Drawing.Size(774, 153);
            this.Bench_TLP_Result.TabIndex = 0;
            // 
            // Bench_P7
            // 
            this.Bench_P7.BackColor = System.Drawing.Color.Silver;
            this.Bench_P7.Controls.Add(this.Bench_Label_RSpeedRead);
            this.Bench_P7.Controls.Add(this.Bench_Label_RSpeedReadResult);
            this.Bench_P7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_P7.Location = new System.Drawing.Point(390, 79);
            this.Bench_P7.Name = "Bench_P7";
            this.Bench_P7.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_P7.Size = new System.Drawing.Size(381, 71);
            this.Bench_P7.TabIndex = 3;
            // 
            // Bench_Label_RSpeedRead
            // 
            this.Bench_Label_RSpeedRead.AutoSize = true;
            this.Bench_Label_RSpeedRead.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_RSpeedRead.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_RSpeedRead.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RSpeedRead.Location = new System.Drawing.Point(8, 5);
            this.Bench_Label_RSpeedRead.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_RSpeedRead.Name = "Bench_Label_RSpeedRead";
            this.Bench_Label_RSpeedRead.Size = new System.Drawing.Size(182, 19);
            this.Bench_Label_RSpeedRead.TabIndex = 0;
            this.Bench_Label_RSpeedRead.Text = "Çoklu Çekirdek Performansı";
            // 
            // Bench_Label_RSpeedReadResult
            // 
            this.Bench_Label_RSpeedReadResult.AutoSize = true;
            this.Bench_Label_RSpeedReadResult.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_RSpeedReadResult.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_RSpeedReadResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            this.Bench_Label_RSpeedReadResult.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RSpeedReadResult.Location = new System.Drawing.Point(8, 32);
            this.Bench_Label_RSpeedReadResult.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_RSpeedReadResult.Name = "Bench_Label_RSpeedReadResult";
            this.Bench_Label_RSpeedReadResult.Size = new System.Drawing.Size(142, 19);
            this.Bench_Label_RSpeedReadResult.TabIndex = 1;
            this.Bench_Label_RSpeedReadResult.Text = "Başlatma bekleniyor...";
            // 
            // Bench_P6
            // 
            this.Bench_P6.BackColor = System.Drawing.Color.Silver;
            this.Bench_P6.Controls.Add(this.Bench_Label_LTimeRead);
            this.Bench_P6.Controls.Add(this.Bench_Label_LTimeReadResult);
            this.Bench_P6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_P6.Location = new System.Drawing.Point(3, 79);
            this.Bench_P6.Name = "Bench_P6";
            this.Bench_P6.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_P6.Size = new System.Drawing.Size(381, 71);
            this.Bench_P6.TabIndex = 2;
            // 
            // Bench_Label_LTimeRead
            // 
            this.Bench_Label_LTimeRead.AutoSize = true;
            this.Bench_Label_LTimeRead.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_LTimeRead.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_LTimeRead.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_LTimeRead.Location = new System.Drawing.Point(8, 5);
            this.Bench_Label_LTimeRead.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_LTimeRead.Name = "Bench_Label_LTimeRead";
            this.Bench_Label_LTimeRead.Size = new System.Drawing.Size(182, 19);
            this.Bench_Label_LTimeRead.TabIndex = 0;
            this.Bench_Label_LTimeRead.Text = "Çoklu Çekirdek Performansı";
            // 
            // Bench_Label_LTimeReadResult
            // 
            this.Bench_Label_LTimeReadResult.AutoSize = true;
            this.Bench_Label_LTimeReadResult.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_LTimeReadResult.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_LTimeReadResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            this.Bench_Label_LTimeReadResult.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_LTimeReadResult.Location = new System.Drawing.Point(8, 32);
            this.Bench_Label_LTimeReadResult.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_LTimeReadResult.Name = "Bench_Label_LTimeReadResult";
            this.Bench_Label_LTimeReadResult.Size = new System.Drawing.Size(142, 19);
            this.Bench_Label_LTimeReadResult.TabIndex = 1;
            this.Bench_Label_LTimeReadResult.Text = "Başlatma bekleniyor...";
            // 
            // Bench_P4
            // 
            this.Bench_P4.BackColor = System.Drawing.Color.Silver;
            this.Bench_P4.Controls.Add(this.Bench_Label_RSpeedWrite);
            this.Bench_P4.Controls.Add(this.Bench_Label_RSpeedWriteResult);
            this.Bench_P4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_P4.Location = new System.Drawing.Point(390, 3);
            this.Bench_P4.Name = "Bench_P4";
            this.Bench_P4.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_P4.Size = new System.Drawing.Size(381, 70);
            this.Bench_P4.TabIndex = 1;
            // 
            // Bench_Label_RSpeedWrite
            // 
            this.Bench_Label_RSpeedWrite.AutoSize = true;
            this.Bench_Label_RSpeedWrite.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_RSpeedWrite.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_RSpeedWrite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RSpeedWrite.Location = new System.Drawing.Point(8, 5);
            this.Bench_Label_RSpeedWrite.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_RSpeedWrite.Name = "Bench_Label_RSpeedWrite";
            this.Bench_Label_RSpeedWrite.Size = new System.Drawing.Size(182, 19);
            this.Bench_Label_RSpeedWrite.TabIndex = 0;
            this.Bench_Label_RSpeedWrite.Text = "Çoklu Çekirdek Performansı";
            // 
            // Bench_Label_RSpeedWriteResult
            // 
            this.Bench_Label_RSpeedWriteResult.AutoSize = true;
            this.Bench_Label_RSpeedWriteResult.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_RSpeedWriteResult.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_RSpeedWriteResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            this.Bench_Label_RSpeedWriteResult.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_RSpeedWriteResult.Location = new System.Drawing.Point(8, 32);
            this.Bench_Label_RSpeedWriteResult.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_RSpeedWriteResult.Name = "Bench_Label_RSpeedWriteResult";
            this.Bench_Label_RSpeedWriteResult.Size = new System.Drawing.Size(142, 19);
            this.Bench_Label_RSpeedWriteResult.TabIndex = 1;
            this.Bench_Label_RSpeedWriteResult.Text = "Başlatma bekleniyor...";
            // 
            // Bench_P3
            // 
            this.Bench_P3.BackColor = System.Drawing.Color.Silver;
            this.Bench_P3.Controls.Add(this.Bench_Label_LTimeWrite);
            this.Bench_P3.Controls.Add(this.Bench_Label_LTimeWriteResult);
            this.Bench_P3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Bench_P3.Location = new System.Drawing.Point(3, 3);
            this.Bench_P3.Name = "Bench_P3";
            this.Bench_P3.Padding = new System.Windows.Forms.Padding(5);
            this.Bench_P3.Size = new System.Drawing.Size(381, 70);
            this.Bench_P3.TabIndex = 0;
            // 
            // Bench_Label_LTimeWrite
            // 
            this.Bench_Label_LTimeWrite.AutoSize = true;
            this.Bench_Label_LTimeWrite.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_LTimeWrite.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_LTimeWrite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_LTimeWrite.Location = new System.Drawing.Point(8, 5);
            this.Bench_Label_LTimeWrite.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_LTimeWrite.Name = "Bench_Label_LTimeWrite";
            this.Bench_Label_LTimeWrite.Size = new System.Drawing.Size(182, 19);
            this.Bench_Label_LTimeWrite.TabIndex = 0;
            this.Bench_Label_LTimeWrite.Text = "Çoklu Çekirdek Performansı";
            // 
            // Bench_Label_LTimeWriteResult
            // 
            this.Bench_Label_LTimeWriteResult.AutoSize = true;
            this.Bench_Label_LTimeWriteResult.BackColor = System.Drawing.Color.Transparent;
            this.Bench_Label_LTimeWriteResult.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.Bench_Label_LTimeWriteResult.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(87)))), ((int)(((byte)(160)))));
            this.Bench_Label_LTimeWriteResult.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Bench_Label_LTimeWriteResult.Location = new System.Drawing.Point(8, 32);
            this.Bench_Label_LTimeWriteResult.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.Bench_Label_LTimeWriteResult.Name = "Bench_Label_LTimeWriteResult";
            this.Bench_Label_LTimeWriteResult.Size = new System.Drawing.Size(142, 19);
            this.Bench_Label_LTimeWriteResult.TabIndex = 1;
            this.Bench_Label_LTimeWriteResult.Text = "Başlatma bekleniyor...";
            // 
            // GlowBenchDisk
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(784, 361);
            this.Controls.Add(this.Bench_TLP);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(800, 400);
            this.MinimumSize = new System.Drawing.Size(800, 400);
            this.Name = "GlowBenchDisk";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GlowBenchDisk";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GlowBenchDisk_FormClosing);
            this.Load += new System.EventHandler(this.GlowBenchDisk_Load);
            this.Bench_TLP.ResumeLayout(false);
            this.Bench_TLP_BTN.ResumeLayout(false);
            this.Bench_TLP_Header.ResumeLayout(false);
            this.Bench_P1.ResumeLayout(false);
            this.Bench_P1.PerformLayout();
            this.Bench_TLP_Modes.ResumeLayout(false);
            this.Bench_P5.ResumeLayout(false);
            this.Bench_P5.PerformLayout();
            this.Bench_P2.ResumeLayout(false);
            this.Bench_P2.PerformLayout();
            this.Bench_TLP_Result.ResumeLayout(false);
            this.Bench_P7.ResumeLayout(false);
            this.Bench_P7.PerformLayout();
            this.Bench_P6.ResumeLayout(false);
            this.Bench_P6.PerformLayout();
            this.Bench_P4.ResumeLayout(false);
            this.Bench_P4.PerformLayout();
            this.Bench_P3.ResumeLayout(false);
            this.Bench_P3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel Bench_TLP;
        private System.Windows.Forms.TableLayoutPanel Bench_TLP_BTN;
        private System.Windows.Forms.Button Bench_Stop;
        private System.Windows.Forms.Button Bench_Start;
        private System.Windows.Forms.TableLayoutPanel Bench_TLP_Header;
        private System.Windows.Forms.Panel Bench_P1;
        internal System.Windows.Forms.Label Bench_Label_Disk;
        internal System.Windows.Forms.ComboBox Bench_Disk;
        internal System.Windows.Forms.Label Bench_Label_Size;
        internal System.Windows.Forms.ComboBox Bench_Size;
        internal System.Windows.Forms.TextBox Bench_SizeCustom;
        private System.Windows.Forms.TableLayoutPanel Bench_TLP_Modes;
        private System.Windows.Forms.Panel Bench_P5;
        private System.Windows.Forms.Panel Bench_P2;
        internal System.Windows.Forms.ComboBox Bench_Buffer;
        internal System.Windows.Forms.Label Bench_Label_Buffer;
        private System.Windows.Forms.TableLayoutPanel Bench_TLP_Result;
        private System.Windows.Forms.Panel Bench_P7;
        private System.Windows.Forms.Panel Bench_P6;
        private System.Windows.Forms.Panel Bench_P4;
        private System.Windows.Forms.Panel Bench_P3;
        internal System.Windows.Forms.Label Bench_Label_RSpeedRead;
        internal System.Windows.Forms.Label Bench_Label_RSpeedReadResult;
        internal System.Windows.Forms.Label Bench_Label_LTimeRead;
        internal System.Windows.Forms.Label Bench_Label_LTimeReadResult;
        internal System.Windows.Forms.Label Bench_Label_RSpeedWrite;
        internal System.Windows.Forms.Label Bench_Label_RSpeedWriteResult;
        internal System.Windows.Forms.Label Bench_Label_LTimeWrite;
        internal System.Windows.Forms.Label Bench_Label_LTimeWriteResult;
    }
}