using System;

namespace Creek.Behaviors
{
    public class EventBehavior
    {
        public void AddEventListener(string name, Delegate d)
        {
            try
            {
                GetType().GetEvent(name).AddEventHandler(this, d);
            }
            catch (Exception ex)
            {
            }
            try
            {
                GetType().GetField(name).SetValue(this, new Event {Handlers = new[] {d}});
            }
            catch (Exception ex)
            {
            }
        }

        public void AddEventListener<TSender, TArgs>(string name, Action<TSender, TArgs> handler)
            where TArgs : EventArgs
        {
            try
            {
                GetType().GetField(name).SetValue(this, new Event<TSender, TArgs> {Handlers = new[] {handler}});
            }
            catch (Exception ex)
            {
            }
        }

        public void AddEventListener<Arg1>(string name, Action<Arg1> handler)
        {
            try
            {
                GetType().GetField(name).SetValue(this, new Event<Arg1> {Handlers = new[] {handler}});
            }
            catch (Exception ex)
            {
            }
        }

        public void AddEventListener<Arg1, Arg2, Arg3>(string name, Action<Arg1, Arg2, Arg3> handler)
        {
            try
            {
                GetType().GetField(name).SetValue(this, new Event<Arg1, Arg2, Arg3> {Handlers = new[] {handler}});
            }
            catch (Exception ex)
            {
            }
        }
    }
}