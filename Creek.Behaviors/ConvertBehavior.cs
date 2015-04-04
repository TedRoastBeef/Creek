using System;
using System.Diagnostics;
using System.Linq;

namespace Creek.Behaviors
{
    public class ConvertBehavior<own>
    {
        #region Delegates

        public delegate bool TryParseHandler<T>(string value, out T result);

        #endregion

        protected Converter<object, own> Own
        {
            get { return input => (own) Convert.ChangeType(input, typeof (own)); }
        }

        protected Converter<object, int> Int
        {
            get { return Convert.ToInt32; }
        }

        protected Converter<object, string> String
        {
            get { return Convert.ToString; }
        }

        protected Converter<object, bool> Bool
        {
            get { return Convert.ToBoolean; }
        }

        protected Converter<object[], own[]> Array
        {
            get { return input => input.Select(o => (own) o).ToArray(); }
        }

        protected T Parse<T>(string s) where T : new()
        {
            return (T) typeof (T).GetMethod("Parse", new[] {typeof (string)}).Invoke(new T(), new object[] {s});
        }

        protected bool TryParse<T>(string s) where T : new()
        {
            T value = default(T);
            try
            {
                value = (T) Convert.ChangeType(s, typeof (T));
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected T TryParse<T>(string value, TryParseHandler<T> handler)
        {
            if (string.IsNullOrEmpty(value))
                return default(T);
            T result;
            if (handler(value, out result))
                return result;
            Trace.TraceWarning("Invalid value '{0}'", value);
            return default(T);
        }
    }
}