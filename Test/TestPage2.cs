using System.Drawing;
using System.Windows.Forms;

namespace Test
{
    internal sealed class TestPage2 : UserControl
    {
        private RadioButton radioButton1;

        public TestPage2()
        {
            InitializeComponent();
            Text = "Seite 2";
        }

        private void InitializeComponent()
        {
            radioButton1 = new RadioButton();
            SuspendLayout();
            // 
            // radioButton1
            // 
            radioButton1.AutoSize = true;
            radioButton1.Location = new Point(42, 55);
            radioButton1.Name = "radioButton1";
            radioButton1.Size = new Size(85, 17);
            radioButton1.TabIndex = 0;
            radioButton1.TabStop = true;
            radioButton1.Text = "radioButton1";
            radioButton1.UseVisualStyleBackColor = true;
            // 
            // TestPage2
            // 
            Controls.Add(radioButton1);
            Name = "TestPage2";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}