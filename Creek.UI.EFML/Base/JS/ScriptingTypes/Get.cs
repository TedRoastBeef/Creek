using System;
using System.Collections.Generic;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public class Get
    {
        private readonly Dictionary<string, string> inner = new Dictionary<string, string>();

        public Get(string arg)
        {
            try
            {
                string[] spl = arg.Split('?', '&');
                foreach (string s in spl)
                {
                    string[] spl1 = s.Split('=');
                    inner.Add(spl1[0], spl1[1]);
                }
            }
            catch (Exception)
            {
            }
        }

        public string this[string k]
        {
            get { return inner[k]; }
        }
    }
}