namespace MVCSharp.Examples.AdvancedCustomization.ApplicationLogic
{
    using Creek.MVP;
    using Creek.MVP.Configuration.Tasks;
    using Creek.MVP.Tasks;

    [Task(typeof(NavigatorEx))]
    public class MainTask : TaskBase
    {
        [IPointEx(ViewCategory.None, typeof(MainViewController))]
        public const string MainView = "MainView";

        [IPointEx(ViewCategory.Mail, typeof(NewMailPropertiesViewController))]
        public const string NewMailPropertiesView = "New Mail Properties";

        [IPointEx(ViewCategory.None, typeof(ControllerBase))]
        public const string MailSendingSuccessView = "MailSendingSuccessView";

        [IPointEx(ViewCategory.None, typeof(ControllerBase))]
        public const string MailSendingFailureView = "MailSendingFailureView";

        [IPointEx(ViewCategory.Notes)]
        public const string PinnedNote = "Pinned Note";
        
        public override void OnStart(object param)
        {
            Navigator.NavigateDirectly(MainView);
        }
    }
}
