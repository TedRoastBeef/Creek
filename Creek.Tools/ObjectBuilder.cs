using System;
using System.Collections.Generic;
using System.Dynamic;

namespace Creek.Tools
{
    internal class ObjectBuilder
    {
        private readonly DynamicResult d;

        public ObjectBuilder()
        {
            d = new DynamicResult(new Dictionary<string, object>());

            d.values.Add("Clear", new Action(Clear));
            d.values.Add("Add", new Action<string, string>(Add));
            d.values.Add("Remove", new Action<string>(Remove));
            d.values.Add("Clone", new Func<dynamic>(() => d));
        }

        public void Add(string k, object v)
        {
            d[k] = v;
        }

        public void Remove(string k)
        {
            d.values.Remove(k);
        }

        public void Clear()
        {
            d.values.Clear();
        }

        public dynamic Build()
        {
            return d;
        }

        #region Nested type: DynamicResult

        public sealed class DynamicResult : DynamicObject
        {
            public Dictionary<string, object> values;

            public DynamicResult(Dictionary<string, object> v)
            {
                values = v;
            }

            public object this[string k]
            {
                get { return values[k]; }
                set { values[k] = value; }
            }

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                object grpValue = null;

                result = values.TryGetValue(binder.Name, out grpValue) ? grpValue : null;
                return result != null;
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                values[binder.Name] = value.ToString();
                return true;
            }
        }

        #endregion
    }
}