using Creek.MVC.Configuration.Tasks;
using Creek.MVC.Tasks;

namespace MVCSharp.Examples.SimpleFormsViewsManagerExample.TestGUI.ApplicationLogic
{
    using Creek.MVP.Tasks;

    class MainTask : TaskBase
    {
        [IPoint(typeof(MainController), View2)]
        public const string View1 = "View1";

        [IPoint(typeof(MainController), View1)]
        public const string View2 = "View2";

        public override void OnStart(object param)
        {
            Navigator.NavigateDirectly(View1);
        }
    }
}
