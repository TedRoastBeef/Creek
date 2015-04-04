using System;
using System.Windows.Forms;
using Creek.MVC;
using Creek.MVC.Configuration;
using MVCSharp.Examples.WindowsFormsExample.ApplicationLogic;

namespace MVCSharp.Examples.WindowsFormsExample.Presentation
{
    [WinformsView(typeof(MainTask), "Main View", IsMdiParent = true)]
    public partial class MainForm : WinFormView, IMainView
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public void SetCurrViewName(string viewName)
        {
            currentViewStatusLbl.Text = viewName;
        }

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            (Controller as MainViewController).Navigate((sender as ToolStripMenuItem).Text);
        }
    }
}