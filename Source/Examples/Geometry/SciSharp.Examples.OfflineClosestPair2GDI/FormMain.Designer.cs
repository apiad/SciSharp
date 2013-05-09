namespace SciSharp.Examples.OfflineClosestPair2GDI
{
    partial class FormMain
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
            this.pboxCanvas = new System.Windows.Forms.PictureBox();
            this.lblDrawing = new System.Windows.Forms.Label();
            this.lblPicker = new System.Windows.Forms.Label();
            this.cbxPicker = new System.Windows.Forms.ComboBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.gbxRandom = new System.Windows.Forms.GroupBox();
            this.btnCircular = new System.Windows.Forms.Button();
            this.btnGaussian = new System.Windows.Forms.Button();
            this.btnUniform = new System.Windows.Forms.Button();
            this.tcbInterval = new System.Windows.Forms.TrackBar();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lbScale = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pboxCanvas)).BeginInit();
            this.gbxRandom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tcbInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // pboxCanvas
            // 
            this.pboxCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pboxCanvas.BackColor = System.Drawing.SystemColors.Window;
            this.pboxCanvas.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pboxCanvas.Location = new System.Drawing.Point(12, 33);
            this.pboxCanvas.Name = "pboxCanvas";
            this.pboxCanvas.Size = new System.Drawing.Size(478, 428);
            this.pboxCanvas.TabIndex = 0;
            this.pboxCanvas.TabStop = false;
            this.pboxCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.PboxCanvasPaint);
            this.pboxCanvas.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PboxCanvasMouseClick);
            // 
            // lblDrawing
            // 
            this.lblDrawing.AutoSize = true;
            this.lblDrawing.Location = new System.Drawing.Point(12, 9);
            this.lblDrawing.Name = "lblDrawing";
            this.lblDrawing.Size = new System.Drawing.Size(204, 13);
            this.lblDrawing.TabIndex = 1;
            this.lblDrawing.Text = "Click on the canvas below to draw points.";
            // 
            // lblPicker
            // 
            this.lblPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPicker.AutoSize = true;
            this.lblPicker.Location = new System.Drawing.Point(496, 33);
            this.lblPicker.Name = "lblPicker";
            this.lblPicker.Size = new System.Drawing.Size(146, 13);
            this.lblPicker.TabIndex = 2;
            this.lblPicker.Text = "Select which algorithm to run:";
            // 
            // cbxPicker
            // 
            this.cbxPicker.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbxPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxPicker.FormattingEnabled = true;
            this.cbxPicker.Items.AddRange(new object[] {
            "Line Sweep"});
            this.cbxPicker.Location = new System.Drawing.Point(496, 49);
            this.cbxPicker.Name = "cbxPicker";
            this.cbxPicker.Size = new System.Drawing.Size(199, 21);
            this.cbxPicker.TabIndex = 3;
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Location = new System.Drawing.Point(496, 76);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(199, 32);
            this.btnGo.TabIndex = 4;
            this.btnGo.Text = "Go!";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.BtnGoClick);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(496, 139);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(199, 28);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.BtnClearClick);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(496, 173);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(199, 28);
            this.btnReset.TabIndex = 3;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnResetClick);
            // 
            // gbxRandom
            // 
            this.gbxRandom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxRandom.Controls.Add(this.btnCircular);
            this.gbxRandom.Controls.Add(this.btnGaussian);
            this.gbxRandom.Controls.Add(this.btnUniform);
            this.gbxRandom.Location = new System.Drawing.Point(496, 230);
            this.gbxRandom.Name = "gbxRandom";
            this.gbxRandom.Size = new System.Drawing.Size(199, 130);
            this.gbxRandom.TabIndex = 5;
            this.gbxRandom.TabStop = false;
            this.gbxRandom.Text = "Generate random distribution";
            // 
            // btnCircular
            // 
            this.btnCircular.Location = new System.Drawing.Point(6, 90);
            this.btnCircular.Name = "btnCircular";
            this.btnCircular.Size = new System.Drawing.Size(187, 28);
            this.btnCircular.TabIndex = 6;
            this.btnCircular.Text = "Circular";
            this.btnCircular.UseVisualStyleBackColor = true;
            this.btnCircular.Click += new System.EventHandler(this.BtnCircularClick);
            // 
            // btnGaussian
            // 
            this.btnGaussian.Location = new System.Drawing.Point(6, 56);
            this.btnGaussian.Name = "btnGaussian";
            this.btnGaussian.Size = new System.Drawing.Size(187, 28);
            this.btnGaussian.TabIndex = 5;
            this.btnGaussian.Text = "Gaussian";
            this.btnGaussian.UseVisualStyleBackColor = true;
            this.btnGaussian.Click += new System.EventHandler(this.BtnGaussianClick);
            // 
            // btnUniform
            // 
            this.btnUniform.Location = new System.Drawing.Point(6, 22);
            this.btnUniform.Name = "btnUniform";
            this.btnUniform.Size = new System.Drawing.Size(187, 28);
            this.btnUniform.TabIndex = 4;
            this.btnUniform.Text = "Uniform";
            this.btnUniform.UseVisualStyleBackColor = true;
            this.btnUniform.Click += new System.EventHandler(this.BtnUniformClick);
            // 
            // tcbInterval
            // 
            this.tcbInterval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tcbInterval.Location = new System.Drawing.Point(496, 393);
            this.tcbInterval.Maximum = 100;
            this.tcbInterval.Minimum = 1;
            this.tcbInterval.Name = "tcbInterval";
            this.tcbInterval.Size = new System.Drawing.Size(199, 45);
            this.tcbInterval.TabIndex = 6;
            this.tcbInterval.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tcbInterval.Value = 50;
            this.tcbInterval.Scroll += new System.EventHandler(this.TcbIntervalScroll);
            // 
            // lblSpeed
            // 
            this.lblSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(499, 377);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(170, 13);
            this.lblSpeed.TabIndex = 7;
            this.lblSpeed.Text = "Update interval (log scale) = 32 ms";
            // 
            // lbScale
            // 
            this.lbScale.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbScale.AutoSize = true;
            this.lbScale.Location = new System.Drawing.Point(499, 425);
            this.lbScale.Name = "lbScale";
            this.lbScale.Size = new System.Drawing.Size(187, 13);
            this.lbScale.TabIndex = 8;
            this.lbScale.Text = "1 ms . . . . . . . . . . . . . . . . . . . . . 1K ms";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 473);
            this.Controls.Add(this.lbScale);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.tcbInterval);
            this.Controls.Add(this.gbxRandom);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnGo);
            this.Controls.Add(this.cbxPicker);
            this.Controls.Add(this.lblPicker);
            this.Controls.Add(this.lblDrawing);
            this.Controls.Add(this.pboxCanvas);
            this.MinimumSize = new System.Drawing.Size(661, 493);
            this.Name = "FormMain";
            this.Text = "SciSharp Example: Offline Closest Pair 2D (GDI)";
            ((System.ComponentModel.ISupportInitialize)(this.pboxCanvas)).EndInit();
            this.gbxRandom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tcbInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pboxCanvas;
        private System.Windows.Forms.Label lblDrawing;
        private System.Windows.Forms.Label lblPicker;
        private System.Windows.Forms.ComboBox cbxPicker;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.GroupBox gbxRandom;
        private System.Windows.Forms.Button btnUniform;
        private System.Windows.Forms.Button btnGaussian;
        private System.Windows.Forms.Button btnCircular;
        private System.Windows.Forms.TrackBar tcbInterval;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lbScale;
    }
}

