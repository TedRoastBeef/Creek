namespace Creek.Extensibility.Addins
{
    using System.Collections.Generic;

    public class ComponentStorage
    {
        #region Fields

        private readonly Dictionary<string, object> components = new Dictionary<string, object>();

        #endregion

        #region Public Methods and Operators

        public void Add(string name, object com)
        {
            this.components.Add(name, com);
        }

        public void Add<T>(T com) where T : class, new()
        {
            this.Add(com.GetType().Name, com);
        }

        public object Get(string name)
        {
            return this.components[name];
        }

        public T Get<T>() where T : class, new()
        {
            return (T)this.Get(typeof(T).Name);
        }

        #endregion
    }
}