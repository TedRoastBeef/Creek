using Creek.MVC;
using Creek.MVC.Configuration.Tasks;
using Creek.MVC.Tasks;

namespace MVCSharp.Examples.TasksInTabs.ApplicationLogic
{
    class TabTask2 : TaskBase
    {
        [IPoint(typeof(ControllerBase))]
        public const string View = "View";

        private int taskStartTimes;

        public int TaskStartTimes
        {
            get { return taskStartTimes; }
        }

        public override void OnStart(object param)
        {
            taskStartTimes++;
            Navigator.ActivateView(View);
        }
    }
}
