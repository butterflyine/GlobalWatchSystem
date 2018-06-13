namespace VirtualDTU
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnConn = new System.Windows.Forms.Button();
            this.btnDisconn = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnSendSig = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdBtnDCMode = new System.Windows.Forms.RadioButton();
            this.rdBtnACMode = new System.Windows.Forms.RadioButton();
            this.numUDVolt = new System.Windows.Forms.NumericUpDown();
            this.numUDHumCH1 = new System.Windows.Forms.NumericUpDown();
            this.numUDTempCH1 = new System.Windows.Forms.NumericUpDown();
            this.numUDHum = new System.Windows.Forms.NumericUpDown();
            this.numUDTemp = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.连续发送 = new System.Windows.Forms.GroupBox();
            this.numUDInterval = new System.Windows.Forms.NumericUpDown();
            this.btnSinCos = new System.Windows.Forms.Button();
            this.btnStopTimer = new System.Windows.Forms.Button();
            this.btnRandom = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.numUDPort = new System.Windows.Forms.NumericUpDown();
            this.timerSendData = new System.Windows.Forms.Timer(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.txtIMEI = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numUDlat = new System.Windows.Forms.NumericUpDown();
            this.numUDLng = new System.Windows.Forms.NumericUpDown();
            this.btnSendGPS = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.nudCi = new System.Windows.Forms.NumericUpDown();
            this.nudLac = new System.Windows.Forms.NumericUpDown();
            this.btnSendLacCi = new System.Windows.Forms.Button();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUDVolt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDHumCH1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDTempCH1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDHum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDTemp)).BeginInit();
            this.连续发送.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUDInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDPort)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUDlat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDLng)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCi)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLac)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(68, 12);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(76, 21);
            this.txtIP.TabIndex = 1;
            this.txtIP.Text = "192.168.1.2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "Port";
            // 
            // btnConn
            // 
            this.btnConn.Location = new System.Drawing.Point(163, 11);
            this.btnConn.Name = "btnConn";
            this.btnConn.Size = new System.Drawing.Size(75, 23);
            this.btnConn.TabIndex = 2;
            this.btnConn.Text = "连接";
            this.btnConn.UseVisualStyleBackColor = true;
            this.btnConn.Click += new System.EventHandler(this.btnConn_Click);
            // 
            // btnDisconn
            // 
            this.btnDisconn.Location = new System.Drawing.Point(163, 41);
            this.btnDisconn.Name = "btnDisconn";
            this.btnDisconn.Size = new System.Drawing.Size(75, 23);
            this.btnDisconn.TabIndex = 2;
            this.btnDisconn.Text = "断开";
            this.btnDisconn.UseVisualStyleBackColor = true;
            this.btnDisconn.Click += new System.EventHandler(this.btnDisconn_Click);
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(435, 11);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "注册";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnSendSig
            // 
            this.btnSendSig.Location = new System.Drawing.Point(232, 45);
            this.btnSendSig.Name = "btnSendSig";
            this.btnSendSig.Size = new System.Drawing.Size(75, 23);
            this.btnSendSig.TabIndex = 2;
            this.btnSendSig.Text = "发送";
            this.btnSendSig.UseVisualStyleBackColor = true;
            this.btnSendSig.Click += new System.EventHandler(this.btnSendSig_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "连续发数间隔";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdBtnDCMode);
            this.groupBox1.Controls.Add(this.rdBtnACMode);
            this.groupBox1.Controls.Add(this.numUDVolt);
            this.groupBox1.Controls.Add(this.numUDHumCH1);
            this.groupBox1.Controls.Add(this.numUDTempCH1);
            this.groupBox1.Controls.Add(this.numUDHum);
            this.groupBox1.Controls.Add(this.numUDTemp);
            this.groupBox1.Controls.Add(this.btnSendSig);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Location = new System.Drawing.Point(12, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(314, 154);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单个数据";
            // 
            // rdBtnDCMode
            // 
            this.rdBtnDCMode.AutoSize = true;
            this.rdBtnDCMode.Location = new System.Drawing.Point(100, 20);
            this.rdBtnDCMode.Name = "rdBtnDCMode";
            this.rdBtnDCMode.Size = new System.Drawing.Size(71, 16);
            this.rdBtnDCMode.TabIndex = 4;
            this.rdBtnDCMode.Text = "电池供电";
            this.rdBtnDCMode.UseVisualStyleBackColor = true;
            // 
            // rdBtnACMode
            // 
            this.rdBtnACMode.AutoSize = true;
            this.rdBtnACMode.Checked = true;
            this.rdBtnACMode.Location = new System.Drawing.Point(23, 20);
            this.rdBtnACMode.Name = "rdBtnACMode";
            this.rdBtnACMode.Size = new System.Drawing.Size(71, 16);
            this.rdBtnACMode.TabIndex = 4;
            this.rdBtnACMode.TabStop = true;
            this.rdBtnACMode.Text = "电源供电";
            this.rdBtnACMode.UseVisualStyleBackColor = true;
            // 
            // numUDVolt
            // 
            this.numUDVolt.DecimalPlaces = 1;
            this.numUDVolt.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numUDVolt.Location = new System.Drawing.Point(232, 15);
            this.numUDVolt.Maximum = new decimal(new int[] {
            42,
            0,
            0,
            65536});
            this.numUDVolt.Minimum = new decimal(new int[] {
            36,
            0,
            0,
            65536});
            this.numUDVolt.Name = "numUDVolt";
            this.numUDVolt.Size = new System.Drawing.Size(62, 21);
            this.numUDVolt.TabIndex = 3;
            this.numUDVolt.Value = new decimal(new int[] {
            42,
            0,
            0,
            65536});
            // 
            // numUDHumCH1
            // 
            this.numUDHumCH1.DecimalPlaces = 1;
            this.numUDHumCH1.Location = new System.Drawing.Point(86, 118);
            this.numUDHumCH1.Name = "numUDHumCH1";
            this.numUDHumCH1.Size = new System.Drawing.Size(67, 21);
            this.numUDHumCH1.TabIndex = 3;
            this.numUDHumCH1.Value = new decimal(new int[] {
            125,
            0,
            0,
            65536});
            // 
            // numUDTempCH1
            // 
            this.numUDTempCH1.DecimalPlaces = 2;
            this.numUDTempCH1.Location = new System.Drawing.Point(86, 93);
            this.numUDTempCH1.Name = "numUDTempCH1";
            this.numUDTempCH1.Size = new System.Drawing.Size(67, 21);
            this.numUDTempCH1.TabIndex = 3;
            this.numUDTempCH1.Value = new decimal(new int[] {
            21,
            0,
            0,
            0});
            // 
            // numUDHum
            // 
            this.numUDHum.DecimalPlaces = 1;
            this.numUDHum.Location = new System.Drawing.Point(86, 66);
            this.numUDHum.Name = "numUDHum";
            this.numUDHum.Size = new System.Drawing.Size(67, 21);
            this.numUDHum.TabIndex = 3;
            this.numUDHum.Value = new decimal(new int[] {
            346,
            0,
            0,
            65536});
            // 
            // numUDTemp
            // 
            this.numUDTemp.DecimalPlaces = 2;
            this.numUDTemp.Location = new System.Drawing.Point(86, 41);
            this.numUDTemp.Name = "numUDTemp";
            this.numUDTemp.Size = new System.Drawing.Size(67, 21);
            this.numUDTemp.TabIndex = 3;
            this.numUDTemp.Value = new decimal(new int[] {
            27,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(160, 125);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(23, 12);
            this.label17.TabIndex = 0;
            this.label17.Text = "%RH";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(300, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(11, 12);
            this.label11.TabIndex = 0;
            this.label11.Text = "V";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(160, 96);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 12);
            this.label16.TabIndex = 0;
            this.label16.Text = "摄氏度";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(160, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(23, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "%RH";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(21, 96);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(59, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "通道2温度";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(160, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 0;
            this.label7.Text = "摄氏度";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "通道0温度";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(21, 121);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(59, 12);
            this.label14.TabIndex = 0;
            this.label14.Text = "通道3湿度";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(173, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 0;
            this.label10.Text = "电池电量";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 0;
            this.label5.Text = "通道1湿度";
            // 
            // tbLog
            // 
            this.tbLog.BackColor = System.Drawing.Color.Black;
            this.tbLog.ForeColor = System.Drawing.Color.Lime;
            this.tbLog.Location = new System.Drawing.Point(12, 247);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ReadOnly = true;
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(750, 218);
            this.tbLog.TabIndex = 4;
            // 
            // 连续发送
            // 
            this.连续发送.Controls.Add(this.numUDInterval);
            this.连续发送.Controls.Add(this.label3);
            this.连续发送.Controls.Add(this.btnSinCos);
            this.连续发送.Controls.Add(this.btnStopTimer);
            this.连续发送.Controls.Add(this.btnRandom);
            this.连续发送.Controls.Add(this.label6);
            this.连续发送.Location = new System.Drawing.Point(332, 87);
            this.连续发送.Name = "连续发送";
            this.连续发送.Size = new System.Drawing.Size(232, 139);
            this.连续发送.TabIndex = 5;
            this.连续发送.TabStop = false;
            this.连续发送.Text = "连续发数";
            // 
            // numUDInterval
            // 
            this.numUDInterval.Location = new System.Drawing.Point(93, 20);
            this.numUDInterval.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUDInterval.Name = "numUDInterval";
            this.numUDInterval.Size = new System.Drawing.Size(76, 21);
            this.numUDInterval.TabIndex = 6;
            this.numUDInterval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // btnSinCos
            // 
            this.btnSinCos.Location = new System.Drawing.Point(93, 65);
            this.btnSinCos.Name = "btnSinCos";
            this.btnSinCos.Size = new System.Drawing.Size(125, 23);
            this.btnSinCos.TabIndex = 2;
            this.btnSinCos.Text = "正弦余弦规律发送";
            this.btnSinCos.UseVisualStyleBackColor = true;
            this.btnSinCos.Visible = false;
            this.btnSinCos.Click += new System.EventHandler(this.btnSinCos_Click);
            // 
            // btnStopTimer
            // 
            this.btnStopTimer.Location = new System.Drawing.Point(93, 104);
            this.btnStopTimer.Name = "btnStopTimer";
            this.btnStopTimer.Size = new System.Drawing.Size(99, 23);
            this.btnStopTimer.TabIndex = 2;
            this.btnStopTimer.Text = "停止定时发送";
            this.btnStopTimer.UseVisualStyleBackColor = true;
            this.btnStopTimer.Click += new System.EventHandler(this.btnStopTimer_Click);
            // 
            // btnRandom
            // 
            this.btnRandom.Location = new System.Drawing.Point(12, 66);
            this.btnRandom.Name = "btnRandom";
            this.btnRandom.Size = new System.Drawing.Size(75, 23);
            this.btnRandom.TabIndex = 2;
            this.btnRandom.Text = "随机发送";
            this.btnRandom.UseVisualStyleBackColor = true;
            this.btnRandom.Click += new System.EventHandler(this.btnRandom_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(190, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 0;
            this.label6.Text = "秒";
            // 
            // numUDPort
            // 
            this.numUDPort.Location = new System.Drawing.Point(68, 39);
            this.numUDPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numUDPort.Name = "numUDPort";
            this.numUDPort.Size = new System.Drawing.Size(76, 21);
            this.numUDPort.TabIndex = 6;
            this.numUDPort.Value = new decimal(new int[] {
            5200,
            0,
            0,
            0});
            // 
            // timerSendData
            // 
            this.timerSendData.Interval = 1000;
            this.timerSendData.Tick += new System.EventHandler(this.timerSendData_Tick);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(244, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "IMEI";
            // 
            // txtIMEI
            // 
            this.txtIMEI.Location = new System.Drawing.Point(286, 12);
            this.txtIMEI.MaxLength = 15;
            this.txtIMEI.Name = "txtIMEI";
            this.txtIMEI.Size = new System.Drawing.Size(133, 21);
            this.txtIMEI.TabIndex = 1;
            this.txtIMEI.Text = "123456789123456";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numUDlat);
            this.groupBox2.Controls.Add(this.numUDLng);
            this.groupBox2.Controls.Add(this.btnSendGPS);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Location = new System.Drawing.Point(570, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(211, 112);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "位置";
            // 
            // numUDlat
            // 
            this.numUDlat.DecimalPlaces = 6;
            this.numUDlat.Location = new System.Drawing.Point(56, 51);
            this.numUDlat.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numUDlat.Name = "numUDlat";
            this.numUDlat.Size = new System.Drawing.Size(127, 21);
            this.numUDlat.TabIndex = 3;
            this.numUDlat.Value = new decimal(new int[] {
            34272312,
            0,
            0,
            393216});
            // 
            // numUDLng
            // 
            this.numUDLng.DecimalPlaces = 6;
            this.numUDLng.Location = new System.Drawing.Point(56, 25);
            this.numUDLng.Maximum = new decimal(new int[] {
            18000,
            0,
            0,
            0});
            this.numUDLng.Name = "numUDLng";
            this.numUDLng.Size = new System.Drawing.Size(127, 21);
            this.numUDLng.TabIndex = 3;
            this.numUDLng.Value = new decimal(new int[] {
            108951123,
            0,
            0,
            393216});
            // 
            // btnSendGPS
            // 
            this.btnSendGPS.Location = new System.Drawing.Point(108, 78);
            this.btnSendGPS.Name = "btnSendGPS";
            this.btnSendGPS.Size = new System.Drawing.Size(75, 23);
            this.btnSendGPS.TabIndex = 2;
            this.btnSendGPS.Text = "发送";
            this.btnSendGPS.UseVisualStyleBackColor = true;
            this.btnSendGPS.Click += new System.EventHandler(this.btnSendGPS_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(21, 27);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "经度";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(21, 58);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 0;
            this.label13.Text = "纬度";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.nudCi);
            this.groupBox3.Controls.Add(this.nudLac);
            this.groupBox3.Controls.Add(this.btnSendLacCi);
            this.groupBox3.Controls.Add(this.label18);
            this.groupBox3.Controls.Add(this.label19);
            this.groupBox3.Location = new System.Drawing.Point(570, 132);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(211, 112);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "位置";
            // 
            // nudCi
            // 
            this.nudCi.Location = new System.Drawing.Point(56, 51);
            this.nudCi.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudCi.Name = "nudCi";
            this.nudCi.Size = new System.Drawing.Size(127, 21);
            this.nudCi.TabIndex = 3;
            this.nudCi.Value = new decimal(new int[] {
            5525,
            0,
            0,
            0});
            // 
            // nudLac
            // 
            this.nudLac.Location = new System.Drawing.Point(56, 25);
            this.nudLac.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudLac.Name = "nudLac";
            this.nudLac.Size = new System.Drawing.Size(127, 21);
            this.nudLac.TabIndex = 3;
            this.nudLac.Value = new decimal(new int[] {
            23,
            0,
            0,
            0});
            // 
            // btnSendLacCi
            // 
            this.btnSendLacCi.Location = new System.Drawing.Point(108, 78);
            this.btnSendLacCi.Name = "btnSendLacCi";
            this.btnSendLacCi.Size = new System.Drawing.Size(75, 23);
            this.btnSendLacCi.TabIndex = 2;
            this.btnSendLacCi.Text = "发送";
            this.btnSendLacCi.UseVisualStyleBackColor = true;
            this.btnSendLacCi.Click += new System.EventHandler(this.btnSendLacCi_Click);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(21, 27);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(23, 12);
            this.label18.TabIndex = 0;
            this.label18.Text = "LAC";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(21, 58);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(17, 12);
            this.label19.TabIndex = 0;
            this.label19.Text = "CI";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 485);
            this.Controls.Add(this.numUDPort);
            this.Controls.Add(this.连续发送);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.btnDisconn);
            this.Controls.Add(this.btnConn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtIMEI);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtIP);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "虚拟数据传输单元";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUDVolt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDHumCH1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDTempCH1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDHum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDTemp)).EndInit();
            this.连续发送.ResumeLayout(false);
            this.连续发送.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUDInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDPort)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUDlat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUDLng)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudCi)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLac)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtIP;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnConn;
        private System.Windows.Forms.Button btnDisconn;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnSendSig;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.GroupBox 连续发送;
        private System.Windows.Forms.NumericUpDown numUDHum;
        private System.Windows.Forms.NumericUpDown numUDTemp;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnSinCos;
        private System.Windows.Forms.Button btnRandom;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numUDPort;
        private System.Windows.Forms.NumericUpDown numUDInterval;
        private System.Windows.Forms.Timer timerSendData;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtIMEI;
        private System.Windows.Forms.Button btnStopTimer;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown numUDlat;
        private System.Windows.Forms.NumericUpDown numUDLng;
        private System.Windows.Forms.Button btnSendGPS;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RadioButton rdBtnDCMode;
        private System.Windows.Forms.RadioButton rdBtnACMode;
        private System.Windows.Forms.NumericUpDown numUDVolt;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numUDHumCH1;
        private System.Windows.Forms.NumericUpDown numUDTempCH1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown nudCi;
        private System.Windows.Forms.NumericUpDown nudLac;
        private System.Windows.Forms.Button btnSendLacCi;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label19;
    }
}