namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    partial class NewMailPropertiesView
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
            this.label1 = new System.Windows.Forms.Label();
            this.newMailSenderAddressText = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(170, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "New Mail Sender Address";
            // 
            // newMailSenderAddressText
            // 
            this.newMailSenderAddressText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.newMailSenderAddressText.Location = new System.Drawing.Point(179, 3);
            this.newMailSenderAddressText.Name = "newMailSenderAddressText";
            this.newMailSenderAddressText.Size = new System.Drawing.Size(194, 22);
            this.newMailSenderAddressText.TabIndex = 1;
            this.newMailSenderAddressText.TextChanged += new System.EventHandler(this.newMailSenderAddressText_TextChanged);
            // 
            // NewMailPropertiesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.newMailSenderAddressText);
            this.Controls.Add(this.label1);
            this.Name = "NewMailPropertiesView";
            this.Size = new System.Drawing.Size(376, 217);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox newMailSenderAddressText;
    }
}
