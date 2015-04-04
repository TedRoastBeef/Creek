namespace Creek.UI.Unity3.Controls
{
    partial class CustomColorPicker
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ColorManager.HSL hsl3 = new ColorManager.HSL();
            ColorManager.HSL hsl4 = new ColorManager.HSL();
           colorBox = new ColorBox();
           lblOriginalColor = new System.Windows.Forms.Label();
           colorSlider = new global::Creek.UI.Unity3.Controls.VerticalColorSlider();
           rbBlue = new System.Windows.Forms.RadioButton();
           rbGreen = new System.Windows.Forms.RadioButton();
           rbRed = new System.Windows.Forms.RadioButton();
           rbBrightness = new System.Windows.Forms.RadioButton();
           rbSat = new System.Windows.Forms.RadioButton();
           rbHue = new System.Windows.Forms.RadioButton();
           txtBlue = new System.Windows.Forms.TextBox();
           txtGreen = new System.Windows.Forms.TextBox();
           txtRed = new System.Windows.Forms.TextBox();
           txtBrightness = new System.Windows.Forms.TextBox();
           txtSat = new System.Windows.Forms.TextBox();
           txtHue = new System.Windows.Forms.TextBox();
           tbAlpha = new System.Windows.Forms.TrackBar();
           lblAlpha = new System.Windows.Forms.Label();
           colorPanelPending = new ColorPanel();
            ((System.ComponentModel.ISupportInitialize)(this.tbAlpha)).BeginInit();
           SuspendLayout();
            // 
            // colorBox
            // 
           colorBox.DrawStyle = ColorBox.eDrawStyle.Hue;
            hsl3.H = 0;
            hsl3.L = 1;
            hsl3.S = 1;
           colorBox.HSL = hsl3;
           colorBox.Location = new System.Drawing.Point(0, 0);
           colorBox.Name = "colorBox";
           colorBox.RGB = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
           colorBox.Size = new System.Drawing.Size(212, 181);
           colorBox.TabIndex = 0;
           colorBox.Scroll += new System.EventHandler(this.colorBox_Scroll);
            // 
            // lblOriginalColor
            // 
           lblOriginalColor.BackColor = System.Drawing.SystemColors.Control;
           lblOriginalColor.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
           lblOriginalColor.Location = new System.Drawing.Point(274, 82);
           lblOriginalColor.Name = "lblOriginalColor";
           lblOriginalColor.Size = new System.Drawing.Size(60, 34);
           lblOriginalColor.TabIndex = 39;
            // 
            // colorSlider
            // 
           colorSlider.DrawStyle = global::Creek.UI.Unity3.Controls.VerticalColorSlider.eDrawStyle.Hue;
            hsl4.H = 0;
            hsl4.L = 1;
            hsl4.S = 1;
           colorSlider.HSL = hsl4;
           colorSlider.Location = new System.Drawing.Point(218, 0);
           colorSlider.Name = "colorSlider";
           colorSlider.RGB = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
           colorSlider.Size = new System.Drawing.Size(40, 181);
           colorSlider.TabIndex = 40;
           colorSlider.Scroll += new System.EventHandler(this.colorSlider_Scroll);
            // 
            // rbBlue
            // 
           rbBlue.Location = new System.Drawing.Point(108, 239);
           rbBlue.Name = "rbBlue";
           rbBlue.Size = new System.Drawing.Size(35, 24);
           rbBlue.TabIndex = 53;
           rbBlue.Text = "B:";
           rbBlue.CheckedChanged += new System.EventHandler(this.rbBlue_CheckedChanged);
            // 
            // rbGreen
            // 
           rbGreen.Location = new System.Drawing.Point(108, 214);
           rbGreen.Name = "rbGreen";
           rbGreen.Size = new System.Drawing.Size(35, 24);
           rbGreen.TabIndex = 52;
           rbGreen.Text = "G:";
           rbGreen.CheckedChanged += new System.EventHandler(this.rbGreen_CheckedChanged);
            // 
            // rbRed
            // 
           rbRed.Location = new System.Drawing.Point(108, 189);
           rbRed.Name = "rbRed";
           rbRed.Size = new System.Drawing.Size(35, 24);
           rbRed.TabIndex = 51;
           rbRed.Text = "R:";
           rbRed.CheckedChanged += new System.EventHandler(this.rbRed_CheckedChanged);
            // 
            // rbBrightness
            // 
           rbBrightness.Location = new System.Drawing.Point(12, 239);
           rbBrightness.Name = "rbBrightness";
           rbBrightness.Size = new System.Drawing.Size(35, 24);
           rbBrightness.TabIndex = 50;
           rbBrightness.Text = "B:";
           rbBrightness.CheckedChanged += new System.EventHandler(this.rbBrightness_CheckedChanged);
            // 
            // rbSat
            // 
           rbSat.Location = new System.Drawing.Point(12, 214);
           rbSat.Name = "rbSat";
           rbSat.Size = new System.Drawing.Size(35, 24);
           rbSat.TabIndex = 49;
           rbSat.Text = "S:";
           rbSat.CheckedChanged += new System.EventHandler(this.rbSat_CheckedChanged);
            // 
            // rbHue
            // 
           rbHue.Location = new System.Drawing.Point(12, 189);
           rbHue.Name = "rbHue";
           rbHue.Size = new System.Drawing.Size(35, 24);
           rbHue.TabIndex = 48;
           rbHue.Text = "H:";
           rbHue.CheckedChanged += new System.EventHandler(this.rbHue_CheckedChanged);
            // 
            // txtBlue
            // 
           txtBlue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
           txtBlue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           txtBlue.Location = new System.Drawing.Point(145, 239);
           txtBlue.Name = "txtBlue";
           txtBlue.Size = new System.Drawing.Size(35, 21);
           txtBlue.TabIndex = 46;
           txtBlue.Leave += new System.EventHandler(this.txtBlue_Leave);
            // 
            // txtGreen
            // 
           txtGreen.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
           txtGreen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           txtGreen.Location = new System.Drawing.Point(145, 214);
           txtGreen.Name = "txtGreen";
           txtGreen.Size = new System.Drawing.Size(35, 21);
           txtGreen.TabIndex = 45;
           txtGreen.Leave += new System.EventHandler(this.txtGreen_Leave);
            // 
            // txtRed
            // 
           txtRed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
           txtRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           txtRed.Location = new System.Drawing.Point(145, 189);
           txtRed.Name = "txtRed";
           txtRed.Size = new System.Drawing.Size(35, 21);
           txtRed.TabIndex = 44;
           txtRed.Leave += new System.EventHandler(this.txtRed_Leave);
            // 
            // txtBrightness
            // 
           txtBrightness.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
           txtBrightness.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           txtBrightness.Location = new System.Drawing.Point(49, 239);
           txtBrightness.Name = "txtBrightness";
           txtBrightness.Size = new System.Drawing.Size(35, 21);
           txtBrightness.TabIndex = 43;
           txtBrightness.Leave += new System.EventHandler(this.txtBrightness_Leave);
            // 
            // txtSat
            // 
           txtSat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
           txtSat.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           txtSat.Location = new System.Drawing.Point(49, 214);
           txtSat.Name = "txtSat";
           txtSat.Size = new System.Drawing.Size(35, 21);
           txtSat.TabIndex = 42;
           txtSat.Leave += new System.EventHandler(this.txtSat_Leave);
            // 
            // txtHue
            // 
           txtHue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
           txtHue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           txtHue.Location = new System.Drawing.Point(49, 189);
           txtHue.Name = "txtHue";
           txtHue.Size = new System.Drawing.Size(35, 21);
           txtHue.TabIndex = 41;
           txtHue.Leave += new System.EventHandler(this.txtHue_Leave);
            // 
            // tbAlpha
            // 
           tbAlpha.Location = new System.Drawing.Point(198, 200);
           tbAlpha.Maximum = 255;
           tbAlpha.Name = "tbAlpha";
           tbAlpha.Size = new System.Drawing.Size(136, 45);
           tbAlpha.TabIndex = 54;
           tbAlpha.TickFrequency = 20;
           tbAlpha.Value = 255;
           tbAlpha.ValueChanged += new System.EventHandler(this.tbAlpha_ValueChanged);
            // 
            // lblAlpha
            // 
           lblAlpha.AutoSize = true;
           lblAlpha.Location = new System.Drawing.Point(229, 232);
           lblAlpha.Name = "lblAlpha";
           lblAlpha.Size = new System.Drawing.Size(72, 13);
           lblAlpha.TabIndex = 55;
           lblAlpha.Text = "Transparency";
            // 
            // colorPanelPending
            // 
           colorPanelPending.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
           colorPanelPending.Color = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
           colorPanelPending.Location = new System.Drawing.Point(274, 48);
           colorPanelPending.Name = "colorPanelPending";
           colorPanelPending.PaintColor = true;
           colorPanelPending.Size = new System.Drawing.Size(60, 34);
           colorPanelPending.TabIndex = 56;
           colorPanelPending.Text = "colorPanel1";
            // 
            // CustomColorPicker
            // 
           AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           Controls.Add(this.colorPanelPending);
           Controls.Add(this.lblAlpha);
           Controls.Add(this.tbAlpha);
           Controls.Add(this.rbBlue);
           Controls.Add(this.rbGreen);
           Controls.Add(this.rbRed);
           Controls.Add(this.rbBrightness);
           Controls.Add(this.rbSat);
           Controls.Add(this.rbHue);
           Controls.Add(this.txtBlue);
           Controls.Add(this.txtGreen);
           Controls.Add(this.txtRed);
           Controls.Add(this.txtBrightness);
           Controls.Add(this.txtSat);
           Controls.Add(this.txtHue);
           Controls.Add(this.colorSlider);
           Controls.Add(this.lblOriginalColor);
           Controls.Add(this.colorBox);
           Name = "CustomColorPicker";
           Size = new System.Drawing.Size(350, 270);
            ((System.ComponentModel.ISupportInitialize)(this.tbAlpha)).EndInit();
           ResumeLayout(false);
           PerformLayout();

        }

        #endregion

        private ColorBox colorBox;
        private System.Windows.Forms.Label lblOriginalColor;
        private VerticalColorSlider colorSlider;
        private System.Windows.Forms.RadioButton rbBlue;
        private System.Windows.Forms.RadioButton rbGreen;
        private System.Windows.Forms.RadioButton rbRed;
        private System.Windows.Forms.RadioButton rbBrightness;
        private System.Windows.Forms.RadioButton rbSat;
        private System.Windows.Forms.RadioButton rbHue;
        private System.Windows.Forms.TextBox txtBlue;
        private System.Windows.Forms.TextBox txtGreen;
        private System.Windows.Forms.TextBox txtRed;
        private System.Windows.Forms.TextBox txtBrightness;
        private System.Windows.Forms.TextBox txtSat;
        private System.Windows.Forms.TextBox txtHue;
        private System.Windows.Forms.TrackBar tbAlpha;
        private System.Windows.Forms.Label lblAlpha;
        private ColorPanel colorPanelPending;
    }
}
