using System;
using Creek.UI.Metro.Forms;

namespace MetroTest
{
    public partial class Form1 : MetroForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            Creek.UI.Metro.MessageBox.MetroMessageBox.Show(this, "message", "title");
        }

        private void metroTile1_Click(object sender, EventArgs e)
        {
        }
    }
}