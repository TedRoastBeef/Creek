using System;
using System.Diagnostics;
using Creek.Scripting.Commandparser.Exceptions;

namespace Creek.Scripting.Commandparser.TP
{
    public class UseStatementParser
    {
        [DebuggerStepThrough]
        public static void Parse(string c, Interpreter p)
        {
            foreach (var s in c.Split(';'))
            {
                if (s.StartsWith("use"))
                {
                    if (!p.ImportedNamespaces.Contains(Middle(s, "'", "'")))
                        p.ImportedNamespaces.Add(Middle(s, "'", "'"));
                    else
                        throw new RuntimeError("namespace '" + Middle(s, "'", "'") + "' is always imported");
                }
            }
        }

        private static string Middle(String str, string startchar, string endchar)
        {
            int strStart = str.IndexOf(startchar, StringComparison.Ordinal) + 1;
            int strEnd = str.LastIndexOf(endchar, StringComparison.Ordinal);
            return str.Substring(strStart, strEnd - strStart);
        }

    }
}
