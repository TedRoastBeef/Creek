using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Creek.Macros
{
    public class MacroManager
    {
        public Dictionary<string, Delegate> Actions = new Dictionary<string, Delegate>(); 

        public void Apply(string k, Delegate d)
        {
            Actions.Add(k, d);
        }

        public void Execute(Macro m, params object[] param)
        {
            foreach (var a in m.Actions)
            {
                Actions[a].DynamicInvoke(param);
            }
        }

    }
}
