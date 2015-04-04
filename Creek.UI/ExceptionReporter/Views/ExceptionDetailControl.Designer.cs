namespace Creek.UI.ExceptionReporter.Views
{
	internal partial class ExceptionDetailControl
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
           label2 = new System.Windows.Forms.Label();
           txtExceptionTabStackTrace = new System.Windows.Forms.TextBox();
           label1 = new System.Windows.Forms.Label();
           txtExceptionTabMessage = new System.Windows.Forms.TextBox();
           listviewExceptions = new System.Windows.Forms.ListView();
           SuspendLayout();
            // 
            // label2
            // 
           label2.AutoSize = true;
           label2.Location = new System.Drawing.Point(0, 208);
           label2.Name = "label2";
           label2.Size = new System.Drawing.Size(66, 13);
           label2.TabIndex = 33;
           label2.Text = "Stack Trace";
            // 
            // txtExceptionTabStackTrace
            // 
           txtExceptionTabStackTrace.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                                                                                           | System.Windows.Forms.AnchorStyles.Left)
                                                                                          | System.Windows.Forms.AnchorStyles.Right)));
           txtExceptionTabStackTrace.BackColor = System.Drawing.SystemColors.Window;
           txtExceptionTabStackTrace.Location = new System.Drawing.Point(13, 226);
           txtExceptionTabStackTrace.Multiline = true;
           txtExceptionTabStackTrace.Name = "txtExceptionTabStackTrace";
           txtExceptionTabStackTrace.ReadOnly = true;
           txtExceptionTabStackTrace.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
           txtExceptionTabStackTrace.Size = new System.Drawing.Size(598, 306);
           txtExceptionTabStackTrace.TabIndex = 31;
            // 
            // label1
            // 
           label1.AutoSize = true;
           label1.Location = new System.Drawing.Point(0, 130);
           label1.Name = "label1";
           label1.Size = new System.Drawing.Size(50, 13);
           label1.TabIndex = 32;
           label1.Text = "Message";
            // 
            // txtExceptionTabMessage
            // 
           txtExceptionTabMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                                       | System.Windows.Forms.AnchorStyles.Right)));
           txtExceptionTabMessage.BackColor = System.Drawing.SystemColors.Window;
           txtExceptionTabMessage.Location = new System.Drawing.Point(13, 148);
           txtExceptionTabMessage.Multiline = true;
           txtExceptionTabMessage.Name = "txtExceptionTabMessage";
           txtExceptionTabMessage.ReadOnly = true;
           txtExceptionTabMessage.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
           txtExceptionTabMessage.Size = new System.Drawing.Size(598, 52);
           txtExceptionTabMessage.TabIndex = 30;
            // 
            // listviewExceptions
            // 
           listviewExceptions.Activation = System.Windows.Forms.ItemActivation.OneClick;
           listviewExceptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                                                                                   | System.Windows.Forms.AnchorStyles.Right)));
           listviewExceptions.FullRowSelect = true;
           listviewExceptions.HotTracking = true;
           listviewExceptions.HoverSelection = true;
           listviewExceptions.Location = new System.Drawing.Point(3, 3);
           listviewExceptions.Name = "listviewExceptions";
           listviewExceptions.Size = new System.Drawing.Size(608, 120);
           listviewExceptions.TabIndex = 29;
           listviewExceptions.UseCompatibleStateImageBehavior = false;
           listviewExceptions.View = System.Windows.Forms.View.Details;
            // 
            // ExceptionDetailControl
            // 
           AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           Controls.Add(this.label2);
           Controls.Add(this.txtExceptionTabStackTrace);
           Controls.Add(this.label1);
           Controls.Add(this.txtExceptionTabMessage);
           Controls.Add(this.listviewExceptions);
           Name = "ExceptionDetailControl";
           Size = new System.Drawing.Size(614, 535);
           ResumeLayout(false);
           PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtExceptionTabStackTrace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExceptionTabMessage;
        private System.Windows.Forms.ListView listviewExceptions;
    }
}