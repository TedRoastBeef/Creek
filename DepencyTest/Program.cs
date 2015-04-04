using System;
using System.Collections.Generic;
using Creek.Tools;

namespace DepencyTest
{
    class Program
    {
        static void Main()
        {
            DependencyContainer.Register<ComponentLoader>(() => new ComponentLoader());

            var c = DependencyContainer.Resolve<ComponentLoader>();

            c.Create<Hellocomponent>();
            c.Create<Hellocomponent>();
            c.Create<Hellocomponent>();
            c.Create<Hellocomponent>();

            DependencyContainer.Register<ComponentLoader>(() => c);
            
            c = new ComponentLoader();

            DependencyContainer.Resolve(out c);
            c = DependencyContainer.Resolve<ComponentLoader>();

            c.Components[0].Do();
        }
    }

    public class Icomponent
    {
        public string Name;
        public void Do() {}
    }
    public class ComponentLoader : IDependency
    {
        public List<Icomponent> Components = new List<Icomponent>();

        public TC Create<TC>() where TC : Icomponent
        {
            var i = (Icomponent)Activator.CreateInstance(typeof(TC));
            i.Name = typeof(TC).Name;

            Components.Add(i);
            return (TC) i;
        }

    }
    public class Hellocomponent  : Icomponent{}
}
