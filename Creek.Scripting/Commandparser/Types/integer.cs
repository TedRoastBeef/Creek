using Creek.Scripting.Commandparser.Common;

namespace Creek.Scripting.Commandparser.Types
{
    public class @integer : IPropertyObject
    {

        public override IPropertyObject Parse(string content)
        {
            int result;
            int.TryParse(content, out result);
            return new @integer { Value = result };
        }

        public override bool IsType(string content)
        {
            int result;
            return  int.TryParse(content, out result);
        }
    }
}
