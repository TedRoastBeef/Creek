using System;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.Unity3.Controls
{
    public partial class ColorChooser : Form
    {
        public ColorChooser(Color color)
        {
            InitializeComponent();
            colorPicker1.Color = color;
        }

        public Color Color
        {
            get { return colorPicker1.Color; }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}