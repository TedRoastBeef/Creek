using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Creek.UI.Vista
{
    public class ExplorerTreeview : TreeView
    {
        //some help from http://www.danielmoth.com/Blog/2007/01/treeviewvista.html
        private const int TVS_EX_FADEINOUTEXPANDOS = 0x0040;
        private const int TVS_EX_AUTOHSCROLL = 0x0020;
        //Imports the UXTheme DLL

        public ExplorerTreeview()
        {
            SetWindowTheme(Handle, "explorer", null);
            HotTracking = true; //important
            ShowLines = false;
            //NativeMethods.SendMessage(this.Handle, NativeMethods.TVM_SETEXTENDEDSTYLE, 0, NativeMethods.TVS_EX_AUTOHSCROLL);
            NativeMethods.SendMessage(Handle, NativeMethods.TVM_SETEXTENDEDSTYLE, 0,
                                      NativeMethods.TVS_EX_FADEINOUTEXPANDOS);
        }

        [DllImport("uxtheme", CharSet = CharSet.Unicode)]
        public static extern Int32 SetWindowTheme(IntPtr hWnd, String textSubAppName, String textSubIdList);

        #region Nested type: NativeMethods

        internal class NativeMethods
        {
            public const int TV_FIRST = 0x1100;
            public const int TVM_SETEXTENDEDSTYLE = TV_FIRST + 44;
            public const int TVM_GETEXTENDEDSTYLE = TV_FIRST + 45;
            public const int TVM_SETAUTOSCROLLINFO = TV_FIRST + 59;
            public const int TVS_NOHSCROLL = 0x8000;
            public const int TVS_EX_AUTOHSCROLL = 0x0020;
            public const int TVS_EX_FADEINOUTEXPANDOS = 0x0040;
            public const int GWL_STYLE = -16;

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern int SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern void SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

            [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
            public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);
        }

        #endregion

        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cParams = base.CreateParams;
        //        //Set the button to use external styles
        //        //cParams.Style |= TVS_EX_FADEINOUTEXPANDOS;
        //        cParams.Style |= NativeMethods.TVS_NOHSCROLL; // lose the horizotnal scrollbar
        //        //cParams.Style |= TVS_EX_AUTOHSCROLL;
        //        //Uncomment the code below if you want the button to only show the icon
        //        //cParams.Style |= 0x00000040; // BS_ICON value
        //        return cParams;
        //    }
        //}
    }
}