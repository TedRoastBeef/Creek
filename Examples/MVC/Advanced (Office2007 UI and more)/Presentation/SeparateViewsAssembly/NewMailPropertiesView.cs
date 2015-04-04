using System;
using Creek.MVC;
using MVCSharp.Examples.AdvancedCustomization.ApplicationLogic;

namespace MVCSharp.Examples.AdvancedCustomization.Presentation
{
    [ViewEx(typeof(MainTask), MainTask.NewMailPropertiesView, "New")]
    public partial class NewMailPropertiesView : WinUserControlView_For_NewMailPropertiesViewController
    {
        public NewMailPropertiesView()
        {
            InitializeComponent();
        }

        private void newMailSenderAddressText_TextChanged(object sender, EventArgs e)
        {
            Controller.SetNewMailSenderAddress(newMailSenderAddressText.Text);
        }
    }

    public class WinUserControlView_For_NewMailPropertiesViewController : WinUserControlView<NewMailPropertiesViewController>
    { }
}
