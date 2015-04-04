namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    partial class MailSendingFailureView
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
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Text = "Sending failed. Check Sender and Recipient adresses.";
            // 
            // MailSendingFailureView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.ClientSize = new System.Drawing.Size(364, 134);
            this.Name = "MailSendingFailureView";
            this.Text = "Mail Sending Failure";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
