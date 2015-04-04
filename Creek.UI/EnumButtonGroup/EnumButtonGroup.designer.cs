namespace Creek.UI.EnumButtonGroup
{
    partial class EnumButtonGroup
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
            this.placeholderLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // placeholderLabel
            // 
            this.placeholderLabel.AutoSize = true;
            this.placeholderLabel.Location = new System.Drawing.Point(3, 0);
            this.placeholderLabel.Name = "placeholderLabel";
            this.placeholderLabel.Padding = new System.Windows.Forms.Padding(10);
            this.placeholderLabel.Size = new System.Drawing.Size(114, 33);
            this.placeholderLabel.TabIndex = 0;
            this.placeholderLabel.Text = "EnumButtonGroup";
            // 
            // EnumButtonGroup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.placeholderLabel);
            this.Name = "EnumButtonGroup";
            this.Size = new System.Drawing.Size(120, 33);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label placeholderLabel;
    }
}
