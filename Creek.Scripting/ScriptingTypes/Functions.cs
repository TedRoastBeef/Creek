using System;
using System.Globalization;
using System.Reflection;
using Microsoft.ClearScript.Windows;

namespace Creek.Scripting.ScriptingTypes
{
    public class ModuleAttribute : Attribute {}
    public class Functions
    {
        public static object Import(string dllpath, string typename, params object[] args)
        {
            var ass = Assembly.LoadFile(dllpath);
            var t = ass.GetType(typename);
            return Activator.CreateInstance(t, BindingFlags.Default, null, args, CultureInfo.CurrentCulture);
        }
        public static void Require(WindowsScriptEngine se,  Assembly ass)
        {
            var types = ass.GetTypes();
            foreach (var type in types)
            {
                var ca = type.GetCustomAttributes(typeof (ModuleAttribute), false);
                if(ca != null)
                {
                    foreach (var method in type.GetMethods())
                    {
                        se.AddHostObject(method.Name, Delegate.CreateDelegate(type, method));
                    }
                } 
                else
                {
                    se.AddHostType(type.Name, type);
                }
            }
        }
        public static void Require(WindowsScriptEngine se,  string ass)
        {
            Require(se, Assembly.LoadFile(ass));
        }
        public static object CreateObj(string id)
        {
            return Activator.CreateInstance(Type.GetTypeFromProgID(id));
        }
    }
}
