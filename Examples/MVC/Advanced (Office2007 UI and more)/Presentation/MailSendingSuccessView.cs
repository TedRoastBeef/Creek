using System;
using Creek.MVC;
using MVCSharp.Examples.AdvancedCustomization.ApplicationLogic;

namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    [ViewEx(typeof(MainTask), MainTask.MailSendingSuccessView, "")]
    public partial class MailSendingSuccessView : WinFormView
    {
        public MailSendingSuccessView()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}