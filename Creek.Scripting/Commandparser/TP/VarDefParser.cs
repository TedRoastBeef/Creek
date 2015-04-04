using System;
using System.Collections.Generic;
using System.Diagnostics;
using Creek.Scripting.Commandparser.Common;
using Creek.Scripting.Commandparser.Types;

namespace Creek.Scripting.Commandparser.TP
{
    class VarDefParser
    {
        [DebuggerStepThrough]
        public static VarDefParser Parse(List<IPropertyObject> DataTypes, string c, Section cmd)
        {
            var returns = new VarDefParser();

            string props = c.Split(Convert.ToChar("{"))[1];

            foreach (var p in props.Split(Convert.ToChar(";")))
            {
                if (p != "")
                {
                    if (p.StartsWith("var"))
                    {
                        string pname = p.Remove(0,4).Split('=')[0];
                        string pvalue = p.Remove(0, 3).Split('=')[1];

                        pname = pname.Remove(pname.Length - 1, 1);

                        IPropertyObject valuetype = new @null();

                        //check data type of pvalue
                        foreach (var ty in DataTypes)
                        {
                            if (ty.IsType(pvalue))
                            {
                                valuetype = ty.Parse(pvalue);
                            }
                        }

                       cmd.Variables.Add(pname, valuetype);
                        
                    }
                    
                }
            }
            return returns;
        }
        
        private static bool Is<t>(object input)
        {
            return ReferenceEquals(input.GetType(), typeof(t));
        }

    }
}
