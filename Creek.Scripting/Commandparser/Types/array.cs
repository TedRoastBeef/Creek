using System;
using System.Collections.Generic;
using Creek.Scripting.Commandparser.Common;

namespace Creek.Scripting.Commandparser.Types
{
   public class @array : IPropertyObject
    {
       public List<IPropertyObject> Values { get; set; }
       public List<IPropertyObject> Types;

        public @array(List<IPropertyObject> types)
        {
            Values = new List<IPropertyObject>();
            Types = types;
        }

        public override IPropertyObject Parse(string content)
        {
            array returns = new @array(Types);

            foreach (string v in Middle(content, "[", "]").Split(Convert.ToChar(",")))
            {
                foreach (IPropertyObject ty in Types)
                {
                    if (ty.IsType(v))
                    {
                        returns.Values.Add(ty.Parse(v));
                    }
                }
                
            }

            return returns;
        }

        public override bool IsType(string content)
        {
            if(content.StartsWith("[") && content.EndsWith("]"))
            {
                return true;
            }
           
            return base.IsType(content);
        }

    }
}
