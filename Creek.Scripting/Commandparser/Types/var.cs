using System.Collections.Generic;
using Creek.Scripting.Commandparser.Common;

namespace Creek.Scripting.Commandparser.Types
{
    public class @Var : IPropertyObject
    {
        public Dictionary<string, IPropertyObject> Variables;

        public @Var(Dictionary<string, IPropertyObject> vars)
        {
            Variables = vars;
        }

        public override bool IsType(string content)
        {
            return Variables.ContainsKey(content);
        }

        public override IPropertyObject Parse(string content)
        {
            return new @Var(Variables) {Value=Variables[content].Value};
        }

    }
}
