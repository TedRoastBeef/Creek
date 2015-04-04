using Creek.Scripting.Commandparser.Common;

namespace Creek.Scripting.Commandparser.Types
{
   public class @expression : IPropertyObject
    {

        public override IPropertyObject Parse(string content)
        {
            return new @expression { Value = ExpressionEvaluator.Eval(content) };
        }

        public override bool IsType(string content)
        {
            if (content.Contains("+") | content.Contains("-") | content.Contains("*") | content.Contains("/"))
            {
                return true;
            }
            return false;
        }
        
    }
}
