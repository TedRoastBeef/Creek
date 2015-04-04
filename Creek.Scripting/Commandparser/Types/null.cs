using Creek.Scripting.Commandparser.Common;

namespace Creek.Scripting.Commandparser.Types
{
   public class @null : IPropertyObject
    {
        public override IPropertyObject Parse(string content)
        {
            return new @null {Value = ""};
        }

        public override bool IsType(string content)
        {
            return content == "null" || base.IsType(content);
        }
    }
}
