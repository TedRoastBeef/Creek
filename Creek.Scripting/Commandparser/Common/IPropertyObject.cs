using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Creek.Scripting.Commandparser.Common
{
    public abstract class IPropertyObject
    {
        public object Value { get; set; }

        public virtual IPropertyObject Parse(string content)
        {
            return null;
        }

        public virtual bool IsType(string content)
        {
            return false;
        }

        public virtual IPropertyObject Construct()
        {

            return null;
        }

        [DebuggerStepThrough]
        internal string Middle(String str, string startchar, string endchar)
        {
            int strStart = str.IndexOf(startchar, StringComparison.Ordinal) + 1;
            int strEnd = str.LastIndexOf(endchar, StringComparison.Ordinal);
            return str.Substring(strStart, strEnd - strStart);
        }

        public virtual string TypeOf()
        {
            if (GetType().BaseType.Name == typeof(IPropertyObject).Name)
            {
                return GetType().Name;
            }
            else
            {
                return GetType().BaseType.Name;
            }
            
        }

        public override string ToString()
        {
            return TypeOf();
        }

        public t Cast<t>() where t : IPropertyObject
        {
            return (t)this;
        }


        internal Constructor getConstructor(string content)
        {
            var returns = new Constructor
                              {Name = Regex.Match(content, "new [a-z0-9_\\-]+\\((.*?\\)).*").Groups[0].Value};

            return returns;
        }

    }
}