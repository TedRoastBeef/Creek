/*
_____________________________________
© Pedro Miguel C. Cardoso 2007.
All rights reserved.
http://pmcchp.com/

Redistribution and use in source and binary forms, with or without 
modification, are permitted provided that the following conditions are met:

1) Redistributions of source code must retain the above copyright notice, 
   this list of conditions and the following disclaimer. 
2) Redistributions in binary form must reproduce the above copyright notice,
   this list of conditions and the following disclaimer in the documentation
   and/or other materials provided with the distribution. 
3) Neither the name of the ORGANIZATION nor the names of its contributors
   may be used to endorse or promote products derived from this software
   without specific prior written permission. 

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF
THE POSSIBILITY OF SUCH DAMAGE.
*/
namespace Creek.UI.Vista
{
    partial class CommandDialog : System.Windows.Forms.Form
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        //Required by the Windows Form Designer
        private System.ComponentModel.IContainer components = null;

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
           lblTitle = new System.Windows.Forms.Label();
           lblDescription = new System.Windows.Forms.Label();
           btnCancel = new System.Windows.Forms.Button();
           pnlBottom = new System.Windows.Forms.Panel();
           pnlBottom.SuspendLayout();
           SuspendLayout();
            // 
            // lblTitle
            // 
           lblTitle.AutoSize = true;
           lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(85)))));
           lblTitle.Location = new System.Drawing.Point(12, 9);
           lblTitle.Name = "lblTitle";
           lblTitle.Size = new System.Drawing.Size(39, 21);
           lblTitle.TabIndex = 0;
           lblTitle.Text = "Title";
            // 
            // lblDescription
            // 
           lblDescription.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
           lblDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(28)))), ((int)(((byte)(85)))));
           lblDescription.Location = new System.Drawing.Point(13, 44);
           lblDescription.Name = "lblDescription";
           lblDescription.Size = new System.Drawing.Size(428, 32);
           lblDescription.TabIndex = 1;
           lblDescription.Text = "Description";
            // 
            // btnCancel
            // 
           btnCancel.Location = new System.Drawing.Point(363, 8);
           btnCancel.Name = "btnCancel";
           btnCancel.Size = new System.Drawing.Size(70, 26);
           btnCancel.TabIndex = 1;
           btnCancel.Text = "Cancel";
           btnCancel.UseVisualStyleBackColor = true;
            // 
            // pnlBottom
            // 
           pnlBottom.BackColor = System.Drawing.SystemColors.Control;
           pnlBottom.Controls.Add(this.btnCancel);
           pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
           pnlBottom.Location = new System.Drawing.Point(0, 260);
           pnlBottom.Name = "pnlBottom";
           pnlBottom.Size = new System.Drawing.Size(441, 41);
           pnlBottom.TabIndex = 2;
            // 
            // CommandDialog
            // 
           AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
           AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
           BackColor = System.Drawing.SystemColors.Window;
           ClientSize = new System.Drawing.Size(441, 301);
           Controls.Add(this.pnlBottom);
           Controls.Add(this.lblDescription);
           Controls.Add(this.lblTitle);
           FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
           MaximizeBox = false;
           MinimizeBox = false;
           Name = "CommandDialog";
           ShowIcon = false;
           ShowInTaskbar = false;
           Text = "Command Dialog";
           pnlBottom.ResumeLayout(false);
           ResumeLayout(false);
           PerformLayout();

        }
        internal System.Windows.Forms.Label lblTitle;
        internal System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Button btnCancel;
        internal System.Windows.Forms.Panel pnlBottom;
    }
}