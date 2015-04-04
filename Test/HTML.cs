using System;
using System.Windows.Forms;

namespace Test
{
    public partial class HTML : Form
    {
        public HTML()
        {
            InitializeComponent();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            htmlPanel1.Text = richTextBox1.Text;
        }
    }
}