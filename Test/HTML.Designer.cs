using Creek.UI.HTMLRenderer;

namespace Test
{
    partial class HTML
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
            this.htmlPanel1 = new HtmlPanel();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // htmlPanel1
            // 
            this.htmlPanel1.AutoScroll = true;
            this.htmlPanel1.AutoScrollMinSize = new System.Drawing.Size(75, 17);
            this.htmlPanel1.AvoidGeometryAntialias = false;
            this.htmlPanel1.AvoidImagesLateLoading = false;
            this.htmlPanel1.BackColor = System.Drawing.SystemColors.Window;
            this.htmlPanel1.BaseStylesheet = null;
            this.htmlPanel1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.htmlPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.htmlPanel1.Location = new System.Drawing.Point(0, 136);
            this.htmlPanel1.Name = "htmlPanel1";
            this.htmlPanel1.Size = new System.Drawing.Size(284, 126);
            this.htmlPanel1.TabIndex = 0;
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(284, 130);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "<html>\n    <head>\n\n     </head>\n    <body>\n\n    </body>\n\n</html>";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // HTML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.htmlPanel1);
            this.Name = "HTML";
            this.Text = "HTML";
            this.ResumeLayout(false);

        }

        #endregion

        private HtmlPanel htmlPanel1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}