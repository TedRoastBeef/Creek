using Creek.MVC;

namespace MVCSharp.Examples.WindowsFormsExample.Presentation
{
    // Using such base class (instead of WinUserControlView) makes the
    // association to the controller strongly-tyed.
    public partial class UserControlView : WinUserControlView_For_ControllerBase
    {
        public UserControlView()
        {
            InitializeComponent();
        }

        public override void Activate(bool activate)
        {
            textBox.Text = activate ? "Active" : "Inactive";
        }
    }

    // This intermediate class is used as a workaround for the bug
    // in the VS form designer when inheriting a generic user control class.
    public class WinUserControlView_For_ControllerBase : WinUserControlView<ControllerBase>
    { }
}
