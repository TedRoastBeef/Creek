using Creek.UI.Metro.Controls;

namespace MetroTest
{
    partial class Form1
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.metroTile1 = new Creek.UI.Metro.Controls.MetroTile();
            this.metroToggle1 = new Creek.UI.Metro.Controls.MetroToggle();
            this.metroButton1 = new MetroButton();
            this.SuspendLayout();
            // 
            // metroTile1
            // 
            this.metroTile1.ActiveControl = null;
            this.metroTile1.Location = new System.Drawing.Point(24, 93);
            this.metroTile1.Name = "metroTile1";
            this.metroTile1.Size = new System.Drawing.Size(93, 37);
            this.metroTile1.TabIndex = 0;
            this.metroTile1.Text = "metroTile1";
            this.metroTile1.UseSelectable = true;
            this.metroTile1.Click += new System.EventHandler(this.metroTile1_Click);
            // 
            // metroToggle1
            // 
            this.metroToggle1.AutoSize = true;
            this.metroToggle1.Location = new System.Drawing.Point(24, 174);
            this.metroToggle1.Name = "metroToggle1";
            this.metroToggle1.Size = new System.Drawing.Size(80, 17);
            this.metroToggle1.TabIndex = 1;
            this.metroToggle1.Text = "~StatusOff";
            this.metroToggle1.UseSelectable = true;
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(173, 38);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(75, 23);
            this.metroButton1.TabIndex = 2;
            this.metroButton1.Text = "msg";
            this.metroButton1.UseSelectable = true;
            this.metroButton1.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 300);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.metroToggle1);
            this.Controls.Add(this.metroTile1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Theme = Creek.UI.Metro.MetroThemeStyle.Default;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Creek.UI.Metro.Controls.MetroTile metroTile1;
        private Creek.UI.Metro.Controls.MetroToggle metroToggle1;
        private MetroButton metroButton1;
    }
}

