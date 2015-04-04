using System.Collections.Generic;

namespace Creek.Tools
{
    public class Global
    {
        private static Dictionary<string, object> items = new Dictionary<string, object>();

        public static void Add<T>() where T : new()
        {
            Add(typeof(T).FullName, new T());
        }
        public static T Get<T>()
        {
            return (T) items[typeof(T).FullName];
        }

        public static void Add(string k, object v)
        {
            items.Add(k, v);
        }
        public static void Remove(string k)
        {
            items.Remove(k);
        }
        public static T Get<T>(string k)
        {
            return (T) items[k];
        }

    }
}
