using System;
using System.Collections.Generic;

namespace Creek.Scripting.Commandparser.Common
{
    public class Class
    {
        public List<IPropertyObject> Propertys;
        public List<Method> Methods;

        public virtual bool isClass(string s)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(s, "[a-z0-9_\\-]+\\.[a-z0-9_\\-]+\\((.*?\\))");
        }
        public virtual Class Parse()
        {

            return new Class();
        }

        public Class()
        {
            Propertys = new List<IPropertyObject>();
            Methods = new List<Method>();
        }

        internal string getClassName(string s)
        {
            return s.Split('.')[0];
        }
        internal string getClassMethodName(string s)
        {
            return s.Split('.')[1];
        }

        internal string Middle(String str, string startchar, string endchar)
        {
            var strStart = str.IndexOf(startchar, StringComparison.Ordinal) + 1;
            var strEnd = str.LastIndexOf(endchar, StringComparison.Ordinal);
            return str.Substring(strStart, strEnd - strStart);
        }

        public virtual string TypeOf()
        {
            var baseType = GetType().BaseType;
            if (baseType != null && baseType.Name == typeof(IPropertyObject).Name)
            {
                return GetType().Name;
            }
            var type = GetType().BaseType;
            if (type != null) return type.Name;

            return null;
        }

        public override string ToString()
        {
            return TypeOf();
        }
    }
}
