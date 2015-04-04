using Creek.MVC;
using Creek.MVC.Views;
using MVCSharp.Examples.AdvancedCustomization.Presentation;

namespace MVCSharp.Examples.AdvancedCustomization.ApplicationLogic
{
    public class MainViewController : ControllerBase
    {
        public void NavigateToView(string viewName)
        {
            Task.Navigator.Navigate(viewName);
        }

        public void CreateView(ViewCategory c)
        {
            IViewsManager vm = Task.Navigator.ViewsManager;
            InteractionPointInfoEx ip = (vm as IDynamicViewsManager).CreateView(c);
            (View as IMainView).AddViewToNavPane(ip);
        }
    }
}
