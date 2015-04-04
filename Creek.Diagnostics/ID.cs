using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Creek.Diagnostics
{
    internal class ID
    {
        public string Data { get; set; }
        public static ID Generate()
        {
            var r = new ID();
            Assembly assembly = Assembly.GetExecutingAssembly();
            var attribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
            r.Data = attribute.Value;

            return r;
        }

        public void Save()
        {
            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\" + Data, Data);
        }

        #region Operators

        public static implicit operator string(ID i)
        {
            return i.Data;
        }
        public static implicit operator ID(string i)
        {
            return new ID { Data = i };
        }

        #endregion
    }
}
