using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Creek.UI.Vista
{
    public class ExplorerProgressbar : ProgressBar
    {
        //Constants, derived from Windows Vista SDK

        #region States enum

        public enum States
        {
            Normal,
            Error,
            Paused,
            Partial
        }

        #endregion

        private const int PBST_NORMAL = 0x0001; //Green progressbar, default
        private const int PBST_ERROR = 0x0002; //Red progressbar
        private const int PBST_PAUSED = 0x0003; //Yellow progressbar
        //const int PBST_PARTIAL = 0x0001; //The blue progressbar is found to have "partial" state - aerostyle.xml

        //*Blue progressbar is not available, since it is not easily available as a progressbar state
        private const int PBS_SMOOTHREVERSE = 0x10; //Allows for two-way smooth transition

        private const int WM_USER = 0x0400;
                          //WM_USER value taken from http://msdn2.microsoft.com/en-us/library/ms644931.aspx

        private const int PBM_SETSTATE = WM_USER + 16;

        private Boolean elv;

        private States ps_ = States.Normal;

        public ExplorerProgressbar()
        {
            SendMessage(Handle, PBM_SETSTATE, PBST_NORMAL, 0);
            Paint += ExplorerListView_Load;
        }

        public States ProgressState
        {
            get { return ps_; }
            set
            {
                ps_ = value;
                SetState(ps_);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParams = base.CreateParams;
                //Allows for smooth transition even when progressbar value is subtracted
                cParams.Style |= PBS_SMOOTHREVERSE;
                return cParams;
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        public void ExplorerListView_Load(object sender, PaintEventArgs e)
        {
            MessageBox.Show("");
            if (!elv)
            {
                SetState(ps_);
                elv = true;
            }
        }

        //Note that setting the state to PBST_ERROR or PBST_PAUSED will cause the Progressbar to have delays in the progress (progress updates on next update in value; problem probably due to the progressbar smooth transition state). The progressbar will not have any special effects when using these states. However, the progressbar works just like a normal progressbar in PBST_NORMAL.
        public void SetState(States State)
        {
            SendMessage(Handle, PBM_SETSTATE, PBST_NORMAL, 0);
            switch (State)
            {
                case States.Normal:
                    SendMessage(Handle, PBM_SETSTATE, PBST_NORMAL, 0);
                    break;
                case States.Error:
                    SendMessage(Handle, PBM_SETSTATE, PBST_ERROR, 0);
                    break;
                case States.Paused:
                    SendMessage(Handle, PBM_SETSTATE, PBST_PAUSED, 0);
                    break;
                    //case States.Partial:
                    //The blue progressbar is not available
                    //    SendMessage(this.Handle, PBM_SETSTATE, PBST_PARTIAL, 0);
                    //    break;
                default:
                    SendMessage(Handle, PBM_SETSTATE, PBST_NORMAL, 0);
                    break;
            }
        }

        //1 - ???
        //2 - window closed
        //132 - mouseover
        protected override void WndProc(ref Message m)
        {
            // Listen for operating system messages.
            switch (m.Msg)
            {
                    // The WM_ACTIVATEAPP message occurs when the application
                    // becomes the active application or becomes inactive.
                case 2:
                    //MessageBox.Show("");
                    break;
                case 132:
                    //refresh display
                    SetState(ps_);
                    //MessageBox.Show("");
                    break;
            }
            base.WndProc(ref m);
        }
    }
}