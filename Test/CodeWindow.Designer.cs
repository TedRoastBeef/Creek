using Creek.UI;

namespace Test
{
    partial class CodeWindow
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
            this.button1 = new System.Windows.Forms.Button();
            this.syntaxRichTextBox1 = new SyntaxRichTextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(365, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // syntaxRichTextBox1
            // 
            this.syntaxRichTextBox1.AcceptsTab = true;
            this.syntaxRichTextBox1.Location = new System.Drawing.Point(0, 34);
            this.syntaxRichTextBox1.Name = "syntaxRichTextBox1";
            this.syntaxRichTextBox1.Size = new System.Drawing.Size(440, 254);
            this.syntaxRichTextBox1.TabIndex = 2;
            this.syntaxRichTextBox1.TabStop = false;
            this.syntaxRichTextBox1.Text = "";
            // 
            // CodeWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 290);
            this.Controls.Add(this.syntaxRichTextBox1);
            this.Controls.Add(this.button1);
            this.Name = "CodeWindow";
            this.Text = "CodeWindow";
            this.Load += new System.EventHandler(this.CodeWindow_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private SyntaxRichTextBox syntaxRichTextBox1;
    }
}