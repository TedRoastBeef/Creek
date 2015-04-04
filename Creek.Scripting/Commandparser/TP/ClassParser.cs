using System;
using System.Windows.Forms;
using Creek.Scripting.Commandparser.Common;

namespace Creek.Scripting.Commandparser.TP
{
    public class ClassParser
    {
        public static void Parse(string c, Interpreter p)
        {
            string props = c.Split(Convert.ToChar("{"))[1];

            foreach (var l in props.Split(Convert.ToChar(";")))
            {
                var cc = new Class();
                if (cc.isClass(l))
                {
                    MessageBox.Show(l);
                }
                else
                {
                   // MessageBox.Show("no");
                }
            }
        }

        internal static string Middle(String str, string startchar, string endchar)
        {
            int strStart = str.IndexOf(startchar, StringComparison.Ordinal) + 1;
            int strEnd = str.LastIndexOf(endchar, StringComparison.Ordinal);
            return str.Substring(strStart, strEnd - strStart);
        }

    }
}
