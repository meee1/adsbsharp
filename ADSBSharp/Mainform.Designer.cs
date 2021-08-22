namespace ADSBSharp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.startBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.timeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.confidenceNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.framesPerSecLbl = new System.Windows.Forms.Label();
            this.fpsLabel = new System.Windows.Forms.Label();
            this.fpsTimer = new System.Windows.Forms.Timer(this.components);
            this.portNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.waterfall = new SDRSharp.PanView.Waterfall();
            this.label3 = new System.Windows.Forms.Label();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.confidenceNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.portNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(14, 14);
            this.startBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(88, 27);
            this.startBtn.TabIndex = 0;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.timeoutNumericUpDown);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.confidenceNumericUpDown);
            this.groupBox2.Controls.Add(this.framesPerSecLbl);
            this.groupBox2.Controls.Add(this.fpsLabel);
            this.groupBox2.Location = new System.Drawing.Point(14, 77);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(315, 80);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Decoder";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(121, 18);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 15);
            this.label7.TabIndex = 41;
            this.label7.Text = "Timeout (sec)";
            // 
            // timeoutNumericUpDown
            // 
            this.timeoutNumericUpDown.Location = new System.Drawing.Point(121, 39);
            this.timeoutNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.timeoutNumericUpDown.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.timeoutNumericUpDown.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.timeoutNumericUpDown.Name = "timeoutNumericUpDown";
            this.timeoutNumericUpDown.Size = new System.Drawing.Size(83, 23);
            this.timeoutNumericUpDown.TabIndex = 40;
            this.timeoutNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.timeoutNumericUpDown.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.timeoutNumericUpDown.ValueChanged += new System.EventHandler(this.timeoutNumericUpDown_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 18);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 15);
            this.label5.TabIndex = 36;
            this.label5.Text = "Confidence";
            // 
            // confidenceNumericUpDown
            // 
            this.confidenceNumericUpDown.Location = new System.Drawing.Point(18, 39);
            this.confidenceNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.confidenceNumericUpDown.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.confidenceNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.confidenceNumericUpDown.Name = "confidenceNumericUpDown";
            this.confidenceNumericUpDown.Size = new System.Drawing.Size(83, 23);
            this.confidenceNumericUpDown.TabIndex = 0;
            this.confidenceNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.confidenceNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.confidenceNumericUpDown.ValueChanged += new System.EventHandler(this.confidenceNumericUpDown_ValueChanged);
            // 
            // framesPerSecLbl
            // 
            this.framesPerSecLbl.AutoSize = true;
            this.framesPerSecLbl.Location = new System.Drawing.Point(224, 18);
            this.framesPerSecLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.framesPerSecLbl.Name = "framesPerSecLbl";
            this.framesPerSecLbl.Size = new System.Drawing.Size(67, 15);
            this.framesPerSecLbl.TabIndex = 33;
            this.framesPerSecLbl.Text = "Frames/sec";
            // 
            // fpsLabel
            // 
            this.fpsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.fpsLabel.Location = new System.Drawing.Point(227, 39);
            this.fpsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.fpsLabel.Name = "fpsLabel";
            this.fpsLabel.Size = new System.Drawing.Size(70, 23);
            this.fpsLabel.TabIndex = 34;
            this.fpsLabel.Text = "FPS";
            this.fpsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fpsTimer
            // 
            this.fpsTimer.Enabled = true;
            this.fpsTimer.Interval = 500;
            this.fpsTimer.Tick += new System.EventHandler(this.fpsTimer_Tick);
            // 
            // portNumericUpDown
            // 
            this.portNumericUpDown.Location = new System.Drawing.Point(215, 17);
            this.portNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.portNumericUpDown.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.portNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.portNumericUpDown.Name = "portNumericUpDown";
            this.portNumericUpDown.Size = new System.Drawing.Size(114, 23);
            this.portNumericUpDown.TabIndex = 1;
            this.portNumericUpDown.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.portNumericUpDown.Value = new decimal(new int[] {
            47806,
            0,
            0,
            0});
            this.portNumericUpDown.ValueChanged += new System.EventHandler(this.portNumericUpDown_ValueChanged);
            // 
            // waterfall
            // 
            this.waterfall.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.waterfall.Attack = 0D;
            this.waterfall.BandType = SDRSharp.PanView.BandType.Center;
            this.waterfall.CenterFrequency = ((long)(0));
            this.waterfall.Contrast = 0;
            this.waterfall.Decay = 0D;
            this.waterfall.DisplayOffset = 0;
            this.waterfall.DisplayRange = 130;
            this.waterfall.FilterBandwidth = 0;
            this.waterfall.FilterOffset = 0;
            this.waterfall.Frequency = ((long)(0));
            this.waterfall.Location = new System.Drawing.Point(14, 163);
            this.waterfall.Name = "waterfall";
            this.waterfall.Size = new System.Drawing.Size(315, 258);
            this.waterfall.SpectrumWidth = 0;
            this.waterfall.StepSize = 0;
            this.waterfall.TabIndex = 35;
            this.waterfall.TimestampInterval = 0;
            this.waterfall.UseSmoothing = false;
            this.waterfall.UseSnap = false;
            this.waterfall.UseTimestamps = false;
            this.waterfall.Zoom = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(177, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 15);
            this.label3.TabIndex = 34;
            this.label3.Text = "Port";
            // 
            // notifyIcon
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "ADSB#";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 433);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.portNumericUpDown);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.waterfall);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ADSB#";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.confidenceNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.portNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label framesPerSecLbl;
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.Timer fpsTimer;
        private System.Windows.Forms.NumericUpDown portNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown confidenceNumericUpDown;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown timeoutNumericUpDown;
        SDRSharp.PanView.Waterfall waterfall;
    }
}

