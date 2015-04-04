using System;
using System.Globalization;
using System.Reflection;
using Microsoft.ClearScript.Windows;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public class ModuleAttribute : Attribute
    {
    }

    public class Functions
    {
        public static object Import(string dllpath, string typename, params object[] args)
        {
            Assembly ass = Assembly.LoadFile(dllpath);
            Type t = ass.GetType(typename);
            return Activator.CreateInstance(t, BindingFlags.Default, null, args, CultureInfo.CurrentCulture);
        }

        public static void Require(WindowsScriptEngine se, Assembly ass)
        {
            Type[] types = ass.GetTypes();
            foreach (Type type in types)
            {
                object[] ca = type.GetCustomAttributes(typeof (ModuleAttribute), false);
                if (ca != null)
                {
                    foreach (MethodInfo method in type.GetMethods())
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

        public static void Require(WindowsScriptEngine se, string ass)
        {
            Require(se, Assembly.LoadFile(ass));
        }

        public static object CreateObj(string id)
        {
            return Activator.CreateInstance(Type.GetTypeFromProgID(id));
        }
    }
}