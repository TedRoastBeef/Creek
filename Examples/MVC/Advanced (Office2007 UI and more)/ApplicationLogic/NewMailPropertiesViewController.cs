using Creek.MVC;
using MVCSharp.Examples.AdvancedCustomization.Model;

namespace MVCSharp.Examples.AdvancedCustomization.ApplicationLogic
{
    public class NewMailPropertiesViewController : ControllerBase
    {
        public void SetNewMailSenderAddress(string address)
        {
            Mail.NewMailSenderAddress = address;
        }
    }
}
