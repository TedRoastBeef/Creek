using Creek.Scripting.Commandparser.Common;

namespace Creek.Scripting.Commandparser.Types
{
   public class CreateObjectMethod : Method
    {

        public override bool IsType(string content)
        {
            return isFunction(content, "CreateObj");
        }

        public override IPropertyObject Parse(string content)
        {
            return new CreateObjectMethod { Value = ExpressionEvaluator.Eval(content) };
        }
    }
}
