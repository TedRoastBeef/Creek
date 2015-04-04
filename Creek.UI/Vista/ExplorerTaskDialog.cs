using System.ComponentModel;

namespace Creek.UI.Vista
{
    public partial class ExplorerTaskDialog : Component
    {
        //[DllImport("CommCtrl.dll", CharSet = CharSet.Unicode)]
        //IntPtr TaskDialogIndirect (IntPtr TASKDIALOGCONFIG, int pnButton, int pnRadioButton, bool pfVerificationFlagChecked );
        public ExplorerTaskDialog()
        {
            InitializeComponent();
        }

        public ExplorerTaskDialog(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }
    }
}