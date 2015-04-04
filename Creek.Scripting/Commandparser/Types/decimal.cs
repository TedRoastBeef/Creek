using Creek.Scripting.Commandparser.Common;

namespace Creek.Scripting.Commandparser.Types
{
   public class @decimal : IPropertyObject
    {

        public override IPropertyObject Parse(string content)
        {
            decimal result;
            decimal.TryParse(content.Replace(".", ","), out result);
            return new @decimal { Value = result };
        }

        public override bool IsType(string content)
        {
          if(content.Contains("."))
          {
              decimal result;
              return decimal.TryParse(content, out result);
          }
            return false;
        }

    }
}
