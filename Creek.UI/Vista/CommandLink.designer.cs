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
    public partial class CommandLink : System.Windows.Forms.Button
    {

        //UserControl overrides dispose to clean up the component list.
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
        private System.ComponentModel.IContainer components=null;

        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
           lblText = new System.Windows.Forms.Label();
           lblDescription = new System.Windows.Forms.Label();
           picIcon = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)this.picIcon).BeginInit();
           SuspendLayout();
            //
            //lblText
            //
           lblText.AutoSize = true;
           lblText.BackColor = System.Drawing.Color.Transparent;
           lblText.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
           lblText.ForeColor = System.Drawing.Color.FromArgb((int)(byte)21, (int)(byte)28, (int)(byte)85);
           lblText.Location = new System.Drawing.Point(27, 10);
           lblText.Name = "lblText";
           lblText.Size = new System.Drawing.Size(0, 21);
           lblText.TabIndex = 0;
            //
            //lblDescription
            //
           lblDescription.BackColor = System.Drawing.Color.Transparent;
           lblDescription.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, (byte)0);
           lblDescription.ForeColor = System.Drawing.Color.FromArgb((int)(byte)21, (int)(byte)28, (int)(byte)85);
           lblDescription.Location = new System.Drawing.Point(33, 36);
           lblDescription.Name = "lblDescription";
           lblDescription.Size = new System.Drawing.Size(364, 32);
           lblDescription.TabIndex = 1;
           lblDescription.UseCompatibleTextRendering = true;
            //
            //picIcon
            //
           picIcon.BackColor = System.Drawing.Color.Transparent;
           picIcon.Location = new System.Drawing.Point(10, 13);
           picIcon.Name = "picIcon";
           picIcon.Size = new System.Drawing.Size(16, 16);
           picIcon.TabIndex = 2;
           picIcon.TabStop = false;
            //
            //CommandLink
            //
           BackColor = System.Drawing.Color.White;
           Controls.Add(this.picIcon);
           Controls.Add(this.lblDescription);
           Controls.Add(this.lblText);
           Size = new System.Drawing.Size(400, 72);
           TabStop = false;
           UseVisualStyleBackColor = false;
            ((System.ComponentModel.ISupportInitialize)this.picIcon).EndInit();
           ResumeLayout(false);
           PerformLayout();

        }
        internal System.Windows.Forms.Label lblText;
        internal System.Windows.Forms.Label lblDescription;
        internal System.Windows.Forms.PictureBox picIcon;

    }
}