using Creek.MVC;
using Creek.MVC.Configuration;
using MVCSharp.Examples.WindowsFormsExample.ApplicationLogic;

namespace MVCSharp.Examples.WindowsFormsExample.Presentation
{
    using Creek.MVP.Configuration;

    [WinformsView(typeof(MainTask), "About Dlg View", ShowModal = true)]
    public partial class AboutDialog : WinFormView
    {
        public AboutDialog()
        {
            InitializeComponent();
        }
    }
}