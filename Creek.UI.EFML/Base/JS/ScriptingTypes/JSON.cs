using Lib.JSON;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public class JSON
    {
        public string stringify(object o)
        {
            return JsonConvert.SerializeObject(o, Formatting.Indented);
        }

        public object parse(string s)
        {
            return JsonConvert.DeserializeObject(s);
        }
    }
}