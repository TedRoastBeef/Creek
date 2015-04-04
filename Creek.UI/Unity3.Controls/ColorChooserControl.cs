using System;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.Unity3.Controls
{
    public partial class ColorChooserControl : UserControl
    {
        private Color _color;
        private UserControl curControl;

        public ColorChooserControl()
        {
            showControl(0); //custom picker
        }

        public ColorChooserControl(Color color)
        {
            InitializeComponent();
            _color = color;
            showControl(0); //custom picker
        }

        public Color Color
        {
            get { return ((IColorPicker) curControl).Color; }
            set { ((IColorPicker) curControl).Color = value; }
        }

        private void btnShowColorPicker_Click(object sender, EventArgs e)
        {
            if (btnShowColorPicker.Text == "Color Picker")
            {
                showControl(0);
            }
        }

        private void showControl(byte index)
        {
            if (curControl != null)
            {
                _color = ((IColorPicker) curControl).Color;
                Controls.Remove(curControl);
                curControl.Dispose();
                curControl = null;
            }
            switch (index)
            {
                case 0: //custom picker
                    curControl = new CustomColorPicker(_color);
                    break;
            }
            if (curControl == null)
                throw new ArgumentException("The specified color picker could not be loaded!");

            curControl.Bounds = new Rectangle(0, 0, 350, 270);
            Controls.Add(curControl);
        }
    }
}