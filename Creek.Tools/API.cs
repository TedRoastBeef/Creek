using System;
using System.Runtime.InteropServices;

namespace Creek.Tools
{
    public class API
    {
        [DllImport("winmm.dll")]
        public static extern long mciSendString(string strCommand, System.Text.StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);
        
        [DllImport("kernel32.dll", SetLastError=true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool Beep(uint dwFreq, uint dwDuration);

        [DllImport("coredll.dll")]
        public static extern IntPtr LoadIcon(IntPtr hInst, string iconName);

        [DllImport("user32.dll")]
        public static extern bool FlashWindow(IntPtr hwnd, bool bInvert);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(System.Windows.Forms.Keys vKey);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);


    }
}
