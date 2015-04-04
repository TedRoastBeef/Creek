using System;
using System.Runtime.InteropServices;

namespace Creek.UI.ProgressCursor
{
    public sealed class UnManagedMethodWrapper
    {
        [DllImport("user32.dll")]
        public static extern IntPtr CreateIconIndirect(ref IconInfo iconInfo);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetIconInfo(IntPtr iconHandle, ref IconInfo iconInfo);
    }
}