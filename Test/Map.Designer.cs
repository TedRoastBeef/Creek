using Creek.UI;

namespace Test
{
    partial class Map
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
            this.imageMap1 = new ImageMap();
            this.SuspendLayout();
            // 
            // imageMap1
            // 
            this.imageMap1.Image = global::Test.Properties.Resources.ich;
            this.imageMap1.Location = new System.Drawing.Point(13, 13);
            this.imageMap1.Name = "imageMap1";
            this.imageMap1.Size = new System.Drawing.Size(259, 237);
            this.imageMap1.TabIndex = 0;
            this.imageMap1.RegionClick += new ImageMap.RegionClickDelegate(this.imageMap1_RegionClick);
            this.imageMap1.RegionHover += new ImageMap.RegionHoverDelegate(this.imageMap1_RegionHover);
            // 
            // Map
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.imageMap1);
            this.Name = "Map";
            this.Text = "Map";
            this.Load += new System.EventHandler(this.Map_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageMap imageMap1;
    }
}