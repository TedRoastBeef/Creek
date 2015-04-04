using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.MozOptions
{
    /// <summary>
    /// Summary description for EtchedLine.
    /// </summary>
    public class EtchedLine : UserControl
    {
        private Color _darkColor = SystemColors.ControlDark;
        private Color _lightColor = SystemColors.ControlLightLight;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private Container components;

        public EtchedLine()
        {
            InitializeComponent();
        }

        [Category("Appearance")]
        public Color LightColor
        {
            get { return _lightColor; }
            set { _lightColor = value; }
        }

        [Category("Appearance")]
        public Color DarkColor
        {
            get { return _darkColor; }
            set { _darkColor = value; }
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = ClientRectangle;

            var lightPen = new Pen(LightColor, 1.0F);
            var darkPen = new Pen(DarkColor, 1.0F);

            if (Dock == DockStyle.Top)
            {
                int y0 = rect.Top;
                int y1 = rect.Top + 1;

                g.DrawLine(darkPen, rect.Left, y0, rect.Right, y0);
                g.DrawLine(lightPen, rect.Left, y1, rect.Right, y1);
            }
            else if (Dock == DockStyle.Bottom)
            {
                int y0 = rect.Bottom - 2;
                int y1 = rect.Bottom - 1;

                g.DrawLine(darkPen, rect.Left, y0, rect.Right, y0);
                g.DrawLine(lightPen, rect.Left, y1, rect.Right, y1);
            }

            base.OnPaint(e);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion
    }
}