using System.Collections.Generic;
using System.Xml;

namespace Creek.Dynamics.XML.AST
{
    public class Function
    {
        public string Name;
        public Dictionary<string, string> Arguments;

        public List<IAst> Body = new List<IAst>();

        #region Implementation of IAst

        public void Parse(XmlElement xmlElement)
        {
            var r = new Function();
            if (xmlElement.Name == "method")
            {
                r.Name = xmlElement.HasAttribute("name") ? xmlElement.Attributes["name"].Value : "anonymous_func_tmp";
                foreach (XmlElement arg in xmlElement.GetElementsByTagName("Argument"))
                {
                    r.Arguments.Add(arg.Attributes["name"].Value, arg.Attributes["type"].Value);
                }
                var body = xmlElement.GetElementsByTagName("body")[0];
                foreach (XmlNode c in body.ChildNodes)
                {
                    IAst elm;
                    Execute(c, out elm);
                    Body.Add(elm);
                }
            }
        }

        private void Execute(XmlNode e, out IAst el)
        {
            if(e.Name == "call")
            {
                var ee = new CallStmt();
                ee.Parse(e, out el);
            }
            el = null;
        }

        #endregion
    }
}
