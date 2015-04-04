using System.Windows.Forms;
using System.Reflection;
using System.Drawing;
using Creek.MVC;
using Creek.MVC.Configuration;
using Creek.MVC.Views;
using Owf.Controls;
using MVCSharp.Examples.AdvancedCustomization.Properties;
using MVCSharp.Examples.AdvancedCustomization.ApplicationLogic;

namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    public class OutlookLikeViewsManager : WinformsViewsManager, IDynamicViewsManager
    {
        OutlookPanelEx contentPanel;

        protected override void InitializeFormView(Form form, WinformsViewInfo viewInf)
        {
            base.InitializeFormView(form, viewInf);
            // If it's main form then access its content panel.
            if (form is MainForm)
                contentPanel = form.Controls["contentPanel"] as OutlookPanelEx;
        }

        protected override void InitializeUserControlView(UserControl ucView)
        {
            base.InitializeUserControlView(ucView);
            if (ucView.Parent != null) return;
            ucView.Parent = contentPanel;
            ucView.Dock = DockStyle.Fill;
        }

        protected override void ActivateUserControlView(IView view)
        {
            base.ActivateUserControlView(view);
            (view as UserControl).BringToFront();

            contentPanel.HeaderText = view.ViewName;
            string imgName = (ViewInfos[view.ViewName] as ViewInfoEx).ImgName;
            contentPanel.Icon = Resources.ResourceManager.GetObject(imgName) as Image;
        }

        new public static MVCConfiguration GetDefaultConfig()
        {
            MVCConfiguration defaultConf = WinformsViewsManager.GetDefaultConfig();
            defaultConf.ViewsAssembly = Assembly.GetCallingAssembly();
            defaultConf.ViewsManagerType = typeof(OutlookLikeViewsManager);
            return defaultConf;
        }

        public InteractionPointInfoEx CreateView(ViewCategory vc)
        {
            InteractionPointInfoEx result = new InteractionPointInfoEx();
            result.ViewName = vc.ToString() + " " + (ViewInfos.Count-1).ToString();
            result.IsCommonTarget = true;
            result.ViewCategory = vc;
            Navigator.TaskInfo.InteractionPoints[result.ViewName] = result;
            ViewInfoEx vi = new ViewInfoEx(result.ViewName, "", null);
            switch (vc)
            {
                case ViewCategory.Mail:
                    vi.ImgName = "Mail"; vi.ViewType = typeof(MailView); break;
                case ViewCategory.Notes:
                    vi.ImgName = "Notes"; vi.ViewType = typeof(NoteView); break;
                case ViewCategory.Tasks:
                    vi.ImgName = "Tasks"; vi.ViewType = typeof(TaskView); break;                    
            }
            ViewInfos.Add(vi);
            return result;
        }
    }
}
