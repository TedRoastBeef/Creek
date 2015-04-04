namespace Creek.UI.QuickMouse
{
    partial class QuickMouseMenu
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
           components = new System.ComponentModel.Container();
           toolTip1 = new System.Windows.Forms.ToolTip(this.components);
           SuspendLayout();
            // 
            // QuickMouseMenuUC
            // 
           AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           BackColor = System.Drawing.SystemColors.ActiveCaption;
           ClientSize = new System.Drawing.Size(184, 164);
           FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
           Name = "QuickMouseMenuUC";
           ShowInTaskbar = false;
           StartPosition = System.Windows.Forms.FormStartPosition.Manual;
           TopMost = true;
           TransparencyKey = System.Drawing.SystemColors.ActiveCaption;
           MouseLeave += new System.EventHandler(this.QuickMouseMenuUC_MouseLeave);
           ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip1;
    }
}
