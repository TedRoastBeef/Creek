using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Creek.UI.Titlebar
{
    using Creek.UI.Winforms.Properties;

    public partial class FormControlBox : UserControl
    {
        public FormControlBox()
        {
            InitializeComponent();

            lblClose.MouseEnter += (sender, args) => Cursor = Cursors.Hand;
        }

        [Category("Appearance")]
        [Description("Gets or sets maximize button visibility")]
        public bool Maximize
        {
            set { lblMaximize.Visible = value; }
            get { return lblMaximize.Visible; }
        }

        [Category("Appearance")]
        [Description("Gets or sets minimize button visibility")]
        public bool Minimize
        {
            set { lblMinimize.Visible = value; }
            get { return lblMinimize.Visible; }
        }

        [Category("Appearance")]
        [Description("Gets or sets close button visibility")]
        public bool Close
        {
            set { lblClose.Visible = value; }
            get { return lblClose.Visible; }
        }

        private void lblClose_MouseMove(object sender, MouseEventArgs e)
        {
            lblClose.Image = Resources.close_sele;
        }

        private void lblClose_MouseLeave(object sender, EventArgs e)
        {
            lblClose.Image = Resources.close;
        }

        private void lblMaximize_MouseLeave(object sender, EventArgs e)
        {
            lblMaximize.Image = Resources.maximize;
        }

        private void lblMaximize_MouseMove(object sender, MouseEventArgs e)
        {
            lblMaximize.Image = Resources.maximize_sele;
        }

        private void lblMinimize_MouseMove(object sender, MouseEventArgs e)
        {
            lblMinimize.Image = Resources.minimize_sele;
        }

        private void lblMinimize_MouseLeave(object sender, EventArgs e)
        {
            lblMinimize.Image = Resources.minimize;
        }

        private void lblClose_Click(object sender, EventArgs e)
        {
            ParentForm.Close();
        }

        private void lblMaximize_Click(object sender, EventArgs e)
        {
            if (ParentForm.WindowState == FormWindowState.Maximized)
            {
                ParentForm.WindowState = FormWindowState.Normal;
            }
            else if (ParentForm.WindowState == FormWindowState.Normal)
            {
                ParentForm.WindowState = FormWindowState.Maximized;
            }
            ParentForm.Show();
        }

        private void lblMinimize_Click(object sender, EventArgs e)
        {
            ParentForm.WindowState = FormWindowState.Minimized;
            ParentForm.Show();
        }

        private void FormControlBox_Load(object sender, EventArgs e)
        {
        }
    }
}