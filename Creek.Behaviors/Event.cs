using System;
using System.Linq;

namespace Creek.Behaviors
{
    public class Event
    {
        public Delegate[] Handlers;

        public Event(Action handler)
        {
            Handlers = new[] {handler};
        }

        public Event()
        {
        }

        public static Event Empty
        {
            get { return new Event {Handlers = new Delegate[] {}}; }
        }

        public void Invoke(params object[] p)
        {
            Handlers.Select(h => h.DynamicInvoke(p));
        }

        public void Invoke<Arg1>(Arg1 arg1)
        {
            Handlers.Select(h => h.DynamicInvoke(arg1));
        }

        public void Invoke<Arg1, Arg2>(Arg1 arg1, Arg2 arg2)
        {
            Handlers.Select(h => h.DynamicInvoke(arg1, arg2));
        }

        public void Invoke<Arg1, Arg2, Arg3>(Arg1 arg1, Arg2 arg2, Arg3 arg3)
        {
            Handlers.Select(h => h.DynamicInvoke(arg1, arg2, arg3));
        }
    }

    public class Event<TSender, TArgs>
        where TArgs : EventArgs
    {
        public Action<TSender, TArgs>[] Handlers;

        public Event()
        {
        }

        public Event(Action<TSender, TArgs> handler)
        {
            Handlers = new[] {handler};
        }

        public static Event<TSender, TArgs> Empty
        {
            get { return new Event<TSender, TArgs> {Handlers = new Action<TSender, TArgs>[] {}}; }
        }

        public void Invoke(TSender sender, TArgs e)
        {
            foreach (var handler in Handlers)
            {
                handler(sender, e);
            }
        }
    }

    public class Event<Arg1>
    {
        public Action<Arg1>[] Handlers;

        public Event()
        {
        }

        public Event(Action<Arg1> handler)
        {
            Handlers = new[] {handler};
        }

        public static Event<Arg1> Empty
        {
            get { return new Event<Arg1> {Handlers = new Action<Arg1>[] {}}; }
        }

        public void Invoke(params object[] p)
        {
            Handlers.Select(h => h.DynamicInvoke(p));
        }

        public void Invoke(Arg1 arg1)
        {
            foreach (var handler in Handlers)
            {
                handler(arg1);
            }
        }
    }

    public class Event<Arg1, Arg2, Arg3>
    {
        public Action<Arg1, Arg2, Arg3>[] Handlers;

        public Event()
        {
        }

        public Event(Action<Arg1, Arg2, Arg3> handler)
        {
            Handlers = new[] {handler};
        }

        public static Event<Arg1, Arg2, Arg3> Empty
        {
            get { return new Event<Arg1, Arg2, Arg3> {Handlers = new Action<Arg1, Arg2, Arg3>[] {}}; }
        }

        public void Invoke(params object[] p)
        {
            Handlers.Select(h => h.DynamicInvoke(p));
        }

        public void Invoke(Arg1 arg1, Arg2 arg2, Arg3 arg3)
        {
            foreach (var handler in Handlers)
            {
                handler(arg1, arg2, arg3);
            }
        }
    }
}