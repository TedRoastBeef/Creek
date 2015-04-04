using System.Collections.Generic;
using System.Text.RegularExpressions;
using Creek.Scripting.Commandparser.Common;

namespace Creek.Scripting.Commandparser.Types
{
    class @newLiteral : IPropertyObject
    {
        public List<IPropertyObject> Types;

        public @newLiteral(List<IPropertyObject> types)
        {
            Types = types;
        }

        public override bool IsType(string content)
        {
           // return content.StartsWith("new") && content.EndsWith(")");
           return Regex.IsMatch(content.Remove(0,1), "new [a-z0-9_\\-]+\\((.*?\\)).*");
        }

        public override IPropertyObject Parse(string content)
        {
            IPropertyObject returns = null;
            Constructor c = getConstructor(content);

            foreach (var t in Types)
            {
                if(t.ToString() == c.Name)
                {
                    returns = t;
                }
            }


            return returns;
        }
    }
}
