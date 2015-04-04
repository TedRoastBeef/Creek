using System.Diagnostics;
using Creek.Scripting.Commandparser.Common;
using Creek.Scripting.Commandparser.Exceptions;

namespace Creek.Scripting.Commandparser.Types
{
    [DebuggerStepThrough]
    public class @string : IPropertyObject
    {

        public override IPropertyObject Parse(string content)
        {
            return new @string { Value = Middle(content, "'", "'") };
        }

        public override bool IsType(string content)
        {
            if(content.StartsWith("'") && content.EndsWith("'"))
            {
                return true;
            }

            if (content.StartsWith("'") && !content.EndsWith("'"))
            {
                throw new RuntimeError("string is not closed");
            }

           
            return base.IsType(content);
        }
    }
}
