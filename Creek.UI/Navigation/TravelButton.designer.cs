namespace Creek.UI.Navigation
{
    partial class TravelButton
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
            if (_backGround != null)
                _backGround.Dispose();

            if (_forwardButton != null)
                _forwardButton.Dispose();

            if (_backButton != null)
                _backButton.Dispose();

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
           toolTip = new System.Windows.Forms.ToolTip(this.components);
           SuspendLayout();
            // 
            // TravelButton
            // 
           Size = new System.Drawing.Size(74, 29);
           TabStop = false;
           ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolTip toolTip;
    }
}
