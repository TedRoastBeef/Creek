using System;
using System.Drawing;
using System.Windows.Forms;
using Creek.MVC;
using Creek.MVC.Configuration.Tasks;
using MT.WindowsUI.NavigationPane;
using MVCSharp.Examples.AdvancedCustomization.ApplicationLogic;
using MVCSharp.Examples.AdvancedCustomization.Properties;

namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    [ViewEx(typeof(MainTask), MainTask.MainView, "")]
    public partial class MainForm : WinFormViewForMainViewController, IMainView
    {
        private ListView lvMail = new ListView();
        private ListView lvNotes = new ListView();
        private ListView lvTasks = new ListView();

        private ImageList imgList = new ImageList();

        public MainForm()
        {
            InitializeComponent();
            SetupNavPane();
        }

        private void SetupNavPane()
        {
            navigateBar.NavigateBarButtons[0].RelatedControl = lvMail;
            navigateBar.NavigateBarButtons[1].RelatedControl = lvNotes;
            navigateBar.NavigateBarButtons[2].RelatedControl = lvTasks;
            setupListView(lvMail);
            setupListView(lvNotes);
            setupListView(lvTasks);
            navigateBar.SelectedButton = navigateBar.NavigateBarButtons[1];
            navigateBar.SelectedButton = navigateBar.NavigateBarButtons[0];
        }

        private void setupListView(ListView lv)
        {
            lv.View = View.SmallIcon;
            lv.Activation = ItemActivation.OneClick;
            lv.BorderStyle = BorderStyle.None;
            lv.SmallImageList = imgList;
            lv.ItemActivate += navBarListView_ItemActivate;
        }

        void navBarListView_ItemActivate(object sender, EventArgs e)
        {
            Controller.NavigateToView((sender as ListView).FocusedItem.Text);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ToolStripManager.Renderer = new Office2007Renderer.Office2007Renderer();
        }

        public override void Initialize()
        {
            TaskInfo ti = Controller.Task.Navigator.TaskInfo;
            foreach (InteractionPointInfoEx ip in ti.InteractionPoints)
                AddViewToNavPane(ip);
        }

        public void AddViewToNavPane(InteractionPointInfoEx ip)
        {
            TaskInfo ti = Controller.Task.Navigator.TaskInfo;
            if (ip.ViewCategory == ViewCategory.None) return;
            ViewInfoEx vi = ti.ViewInfos[ip.ViewName] as ViewInfoEx;
            Image i = Resources.ResourceManager.GetObject(vi.ImgName) as Image;
            imgList.Images.Add(ip.ViewName, i);
            ListView lv = lvMail;
            switch (ip.ViewCategory)
            {
                case ViewCategory.Mail: lv = lvMail; break;
                case ViewCategory.Notes: lv = lvNotes; break;
                case ViewCategory.Tasks: lv = lvTasks; break;
            }
            lv.Items.Add(ip.ViewName, ip.ViewName);
        }

        private void navigateBar_OnNavigateBarButtonSelected(NavigateBarButton tNavigateBarButton)
        {
            CurrentCategoryChanged(tNavigateBarButton.Caption);
        }

        private void CurrentCategoryChanged(string catName)
        {
            ViewCategory selectedCat = (ViewCategory)Enum.Parse(typeof(ViewCategory), catName);
            switch (selectedCat)
            {
                case ViewCategory.Mail:
                    navigateBar.SelectedButton = navigateBar.NavigateBarButtons[0];
                    CheckToolBarCategoryButtons(true, false, false); break;
                case ViewCategory.Notes:
                    navigateBar.SelectedButton = navigateBar.NavigateBarButtons[1];
                    CheckToolBarCategoryButtons(false, true, false); break;
                case ViewCategory.Tasks:
                    navigateBar.SelectedButton = navigateBar.NavigateBarButtons[2];
                    CheckToolBarCategoryButtons(false, false, true); break;
            }
        }

        private void CheckToolBarCategoryButtons(bool mailChecked, bool notesChecked, bool tasksChecked)
        {
            mailToolStripButton.Checked = mailChecked;
            notesToolStripButton.Checked = notesChecked;
            tasksToolStripButton.Checked = tasksChecked;
        }

        private void catToolStripItem_Click(object sender, EventArgs e)
        {
            CurrentCategoryChanged((sender as ToolStripItem).Text);
        }

        private void newToolStripItem_Click(object sender, EventArgs e)
        {
            switch ((sender as ToolStripItem).Text)
            {
                case "Mail": Controller.CreateView(ViewCategory.Mail); break;
                case "Note": Controller.CreateView(ViewCategory.Notes); break;
                case "Task": Controller.CreateView(ViewCategory.Tasks); break;
            }
        }
    }

    public class WinFormViewForMainViewController : WinFormView<MainViewController> { }
}