using System;
using MVCSharp.Examples.SimpleFormsViewsManagerExample.TestGUI.ApplicationLogic;

namespace MVCSharp.Examples.SimpleFormsViewsManagerExample.TestGUI.Presentation
{
    using System.Windows.Forms;

    using Creek.MVP;
    using Creek.MVP.Configuration.Views;
    using Creek.MVP.Views;

    [View(typeof(MainTask), MainTask.View2)]
    public partial class Form2 : Form, IView
    {
        private IController controller;
        private string viewName;
        
        public Form2()
        {
            InitializeComponent();
        }

        public IController Controller
        {
            get { return controller; }
            set { controller = value; }
        }

        public string ViewName
        {
            get { return viewName; }
            set { viewName = value; }
        }

        private void toView1Btn_Click(object sender, EventArgs e)
        {
            (Controller as MainController).NavigateToView1();
        }
    }
}