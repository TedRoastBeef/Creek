using Creek.Scripting.Commandparser.Types;

namespace Creek.Scripting.Commandparser.Common
{
   public class Method : IPropertyObject
    {

       public Function ToFunction(string s)
       {
           var f = new Function { Name = s.Split('(')[0] };

           string @params = Middle(s, "(", ")");

           if (@params.Contains("'"))
           {
               @params = @params.Replace("'", "");
           }

           f.Params.AddRange(@params.Contains(",") ? @params.Split(',') : new[] { @params });

           return f;
       }


        public bool isFunction(string c)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(c, "[a-z0-9_\\-]+\\((.*?\\)).*");
        }

        public bool isFunction(string c, string name)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(c, name + "\\((.*?\\)).*");
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public virtual IPropertyObject Invoke(params object[] param)
        {
            return new @null();
        }

    }
}
