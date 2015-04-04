namespace Creek.UI.Unity3.Controls
{
    partial class ColorChooser
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
           btnOK = new System.Windows.Forms.Button();
           btnCancel = new System.Windows.Forms.Button();
           colorPicker1 = new global::Creek.UI.Unity3.Controls.ColorChooserControl();
           SuspendLayout();
            // 
            // btnOK
            // 
           btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
           btnOK.Location = new System.Drawing.Point(168, 305);
           btnOK.Name = "btnOK";
           btnOK.Size = new System.Drawing.Size(75, 23);
           btnOK.TabIndex = 1;
           btnOK.Text = "OK";
           btnOK.UseVisualStyleBackColor = true;
           btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
           btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
           btnCancel.Location = new System.Drawing.Point(265, 305);
           btnCancel.Name = "btnCancel";
           btnCancel.Size = new System.Drawing.Size(75, 23);
           btnCancel.TabIndex = 2;
           btnCancel.Text = "Cancel";
           btnCancel.UseVisualStyleBackColor = true;
           btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // colorPicker1
            // 
           colorPicker1.Location = new System.Drawing.Point(0, -1);
           colorPicker1.Name = "colorPicker1";
           colorPicker1.Size = new System.Drawing.Size(350, 300);
           colorPicker1.TabIndex = 0;
            // 
            // ColorChooser
            // 
           AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           ClientSize = new System.Drawing.Size(352, 337);
           Controls.Add(this.btnCancel);
           Controls.Add(this.btnOK);
           Controls.Add(this.colorPicker1);
           FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
           MaximizeBox = false;
           MinimizeBox = false;
           Name = "ColorChooser";
           Text = "ColorChooser";
           ResumeLayout(false);

        }

        #endregion

        private ColorChooserControl colorPicker1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}