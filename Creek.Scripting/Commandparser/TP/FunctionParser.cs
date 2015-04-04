using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Creek.Scripting.Commandparser.Common;
using Creek.Scripting.Commandparser.Exceptions;
using Creek.Scripting.Commandparser.Types;

namespace Creek.Scripting.Commandparser.TP
{
    class FunctionCallParser
    {

        public List<Function> Functions {get; set; }

        public FunctionCallParser()
        {
            Functions = new List<Function>();
        }

        [DebuggerStepThrough]
        public static FunctionCallParser Parse(string c, Section cmd, Interpreter ip)
        {
            var returns = new FunctionCallParser();

            string props = c.Split(Convert.ToChar("{"))[1];

            foreach (var p in props.Split(Convert.ToChar(";")))
            {
                if (p != "")
                {
                    if (isFunction(p))
                    {
                        if(!isFunction(p,"alert"))
                        {
                            Function f = ToFunction(p);

                            returns.Functions.Add(f);
                            cmd.Functions.Add(f,null);
                        }
                        else
                        {
                            if(ip.ImportedNamespaces.Contains("System"))
                            {
                                if (isFunction(p, "alert"))
                                {
                                    var v = new Var(cmd.Variables);
                                    MessageBox.Show(
                                        !v.IsType(ToFunction(p).Params[0])
                                            ? new @string().Parse(ToFunction(p).Params[0]).Value.ToString()
                                            : v.Parse(ToFunction(p).Params[0]).Value.ToString(),
                                        "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            else
                            {
                                throw new RuntimeError("System is not imported");
                            }
                        
                        }
                    }
                }
            }

            return returns;
        }

        private static Function ToFunction(string s)
        {
            var f = new Function { Name = s.Split('(')[0] };

            string @params = Middle(s, "(", ")");

            foreach (var p in @params.Split(','))
            {
                f.Params.Add(p);
            }
            
            return f;
        }

        internal static bool isFunction(string c)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(c, "[a-z0-9_\\-]+\\((.*?\\)).*");
        }

        internal static bool isFunction(string c, string name)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(c, name + "\\((.*?\\)).*");
        }

        internal static string Middle(String str, string startchar, string endchar)
        {
            int strStart = str.IndexOf(startchar, StringComparison.Ordinal) + 1;
            int strEnd = str.LastIndexOf(endchar, StringComparison.Ordinal);
            return str.Substring(strStart, strEnd - strStart);
        }
    }
}
