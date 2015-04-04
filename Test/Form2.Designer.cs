namespace Test
{
    partial class Form2
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
            this.pageNavigator1 = new Creek.UI.EFML.Base.Controls.Navigator.PageNavigator();
            this.SuspendLayout();
            // 
            // pageNavigator1
            // 
            this.pageNavigator1.CurrentPage = -1;
            this.pageNavigator1.Location = new System.Drawing.Point(2, 12);
            this.pageNavigator1.Name = "pageNavigator1";
            this.pageNavigator1.Size = new System.Drawing.Size(406, 275);
            this.pageNavigator1.TabIndex = 0;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 295);
            this.Controls.Add(this.pageNavigator1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Creek.UI.EFML.Base.Controls.Navigator.PageNavigator pageNavigator1;
    }
}