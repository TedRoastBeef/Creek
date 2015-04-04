namespace Creek.UI.Angle
{
    sealed partial class AngleAltitudeSelector
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
           SuspendLayout();
            // 
            // AngleAltitudeSelector
            // 
           AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           Name = "AngleAltitudeSelector";
           Size = new System.Drawing.Size(40, 40);
           Load += new System.EventHandler(this.AngleAltitudeSelector_Load);
           MouseMove += new System.Windows.Forms.MouseEventHandler(this.AngleAltitudeSelector_MouseMove);
           MouseDown += new System.Windows.Forms.MouseEventHandler(this.AngleAltitudeSelector_MouseDown);
           SizeChanged += new System.EventHandler(this.AngleAltitudeSelector_SizeChanged);
           ResumeLayout(false);

        }

        #endregion
    }
}
