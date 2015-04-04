using System.Drawing;
using System.Windows.Forms;

namespace Test
{
    internal sealed class TestPage : UserControl
    {
        private Button button1;
        private TextBox textBox1;

        public TestPage()
        {
            InitializeComponent();
            Text = "Seite 1";
        }

        private void InitializeComponent()
        {
            button1 = new Button();
            textBox1 = new TextBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(72, 14);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(13, 43);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(100, 20);
            textBox1.TabIndex = 1;
            // 
            // TestPage
            // 
            Controls.Add(textBox1);
            Controls.Add(button1);
            Name = "TestPage";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}