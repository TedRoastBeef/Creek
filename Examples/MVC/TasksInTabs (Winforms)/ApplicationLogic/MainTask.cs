using Creek.MVC.Configuration.Tasks;
using Creek.MVC.Tasks;

namespace MVCSharp.Examples.TasksInTabs.ApplicationLogic
{
    using Creek.MVP.Tasks;

    public class MainTask : TaskBase
    {
        [IPoint(typeof(MainViewController))]
        public const string MainView = "Main View";

        public override void OnStart(object param)
        {
            Navigator.NavigateDirectly(MainView);
        }
    }
}
