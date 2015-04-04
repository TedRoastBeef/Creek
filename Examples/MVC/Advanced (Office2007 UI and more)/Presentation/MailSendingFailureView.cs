using MVCSharp.Examples.AdvancedCustomization.ApplicationLogic;

namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    [ViewEx(typeof(MainTask), MainTask.MailSendingFailureView, "")]
    public partial class MailSendingFailureView : MailSendingSuccessView
    {
        public MailSendingFailureView()
        {
            InitializeComponent();
        }
    }
}

