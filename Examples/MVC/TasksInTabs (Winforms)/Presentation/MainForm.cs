using System.Windows.Forms;
using Creek.MVC;
using Creek.MVC.Configuration.Views;
using MVCSharp.Examples.TasksInTabs.ApplicationLogic;

namespace MVCSharp.Examples.TasksInTabs.Presentation
{
    [View(typeof(MainTask), MainTask.MainView)]
    public partial class MainForm : WinFormViewForMainViewController
    {
        public MainForm()
        {
            InitializeComponent();
        }

        public TabPage CurrentTabPage
        {
            get { return tabControl1.SelectedTab; }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            Controller.StartTask(e.TabPage.Text); 
        }
    }

    public class WinFormViewForMainViewController : WinFormView<MainViewController>
    { }
}