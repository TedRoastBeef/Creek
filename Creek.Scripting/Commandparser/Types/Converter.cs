using Creek.Scripting.Commandparser.Common;

namespace Creek.Scripting.Commandparser.Types
{
   public class Converter : Method
    {

        public override IPropertyObject Parse(string content)
        {
            var returns = new Converter();
            Function f = ToFunction(content);

            if(f.Params[0] == "int")
            {
                //returns.Value = new Int();
            }
            return returns;
        }

        public override bool IsType(string content)
        {
            if (isFunction(content))
            {
                if (ToFunction(content).Name == "Convert")
                {
                    return true;
                }
            }
           
            return base.IsType(content);
        }

    }
}
