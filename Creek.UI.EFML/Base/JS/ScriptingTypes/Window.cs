using System;
using System.Reflection;
using System.Windows.Forms;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public class Window
    {
        private readonly Form f;
        public Get get;

        public Window(Form f)
        {
            this.f = f;
            get = new Get(Environment.GetCommandLineArgs()[0]);
        }

        public bool isXMLHttpRequest
        {
            get { return true; }
        }

        public void AddEventHandler(string name, dynamic handler)
        {
            EventInfo evt = f.GetType().GetEvent(name);
            var action = new Action<object, object>((sender, args) => handler(sender, args));
            evt.AddEventHandler(f, Delegate.CreateDelegate(evt.EventHandlerType, action.Target, action.Method));
        }
    }
}