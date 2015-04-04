using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Creek.Dynamics.XML.AST
{
    class CallStmt : IAst
    {
        public string Target;
        public List<string> Args = new List<string>();
        public string Method;

        #region Implementation of IAst

        public void Parse(XmlNode e, out IAst el)
        {
            Target = e.Attributes["target"].Value;
            Method = e.Attributes["method"].Value;

            el = this;
        }

        #endregion
    }
}
