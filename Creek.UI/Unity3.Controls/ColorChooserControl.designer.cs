namespace Creek.UI.Unity3.Controls
{
    partial class ColorChooserControl
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
           btnShowColorPicker = new System.Windows.Forms.Button();
           SuspendLayout();
            // 
            // btnShowColorPicker
            // 
           btnShowColorPicker.Location = new System.Drawing.Point(244, 274);
           btnShowColorPicker.Name = "btnShowColorPicker";
           btnShowColorPicker.Size = new System.Drawing.Size(106, 23);
           btnShowColorPicker.TabIndex = 0;
           btnShowColorPicker.Text = "Custom Color";
           btnShowColorPicker.UseVisualStyleBackColor = true;
           btnShowColorPicker.Click += new System.EventHandler(this.btnShowColorPicker_Click);
            // 
            // ColorPicker
            // 
           AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           Controls.Add(this.btnShowColorPicker);
           Name = "ColorPicker";
           ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnShowColorPicker;
    }
}
