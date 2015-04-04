using System;
using System.Runtime.InteropServices;

namespace Creek.UI.Vista
{
    public class rebar
    {
        [DllImport("uxtheme", CharSet = CharSet.Unicode)]
        public static extern Int32 SetWindowTheme(IntPtr hWnd, String textSubAppName, String textSubIdList);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr CreateWindowExW(uint dwExStyle, string lpClassName, string lpWindowName,
                                                    uint dwStyle, int x, int y, int nWidth, int nHeight,
                                                    IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

        //difficult to implement rebar in C#
    }
}