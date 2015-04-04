namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    partial class MailView
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.recipientTextBox = new System.Windows.Forms.TextBox();
            this.sendMailButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.senderTextBox = new System.Windows.Forms.TextBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.recipientTextBox, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.sendMailButton, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.senderTextBox, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.richTextBox1, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(399, 325);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // recipientTextBox
            // 
            this.recipientTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.recipientTextBox.Location = new System.Drawing.Point(198, 33);
            this.recipientTextBox.Name = "recipientTextBox";
            this.recipientTextBox.Size = new System.Drawing.Size(198, 22);
            this.recipientTextBox.TabIndex = 4;
            // 
            // sendMailButton
            // 
            this.sendMailButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sendMailButton.Location = new System.Drawing.Point(3, 3);
            this.sendMailButton.Name = "sendMailButton";
            this.tableLayoutPanel1.SetRowSpan(this.sendMailButton, 2);
            this.sendMailButton.Size = new System.Drawing.Size(114, 54);
            this.sendMailButton.TabIndex = 0;
            this.sendMailButton.Text = "Send";
            this.sendMailButton.UseVisualStyleBackColor = true;
            this.sendMailButton.Click += new System.EventHandler(this.sendMailButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(123, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 30);
            this.label2.TabIndex = 2;
            this.label2.Text = "Recipient";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(123, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 30);
            this.label1.TabIndex = 1;
            this.label1.Text = "Sender";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // senderTextBox
            // 
            this.senderTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.senderTextBox.Location = new System.Drawing.Point(198, 3);
            this.senderTextBox.Name = "senderTextBox";
            this.senderTextBox.Size = new System.Drawing.Size(198, 22);
            this.senderTextBox.TabIndex = 3;
            // 
            // richTextBox1
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.richTextBox1, 3);
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Location = new System.Drawing.Point(3, 63);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(393, 259);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // MailView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "MailView";
            this.Size = new System.Drawing.Size(399, 325);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button sendMailButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox senderTextBox;
        private System.Windows.Forms.TextBox recipientTextBox;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}
