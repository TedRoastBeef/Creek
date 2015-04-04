namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    partial class PinnedNoteView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PinnedNoteView));
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // PinnedNoteView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.Name = "PinnedNoteView";
            this.ResumeLayout(false);

        }

        #endregion
    }
}
