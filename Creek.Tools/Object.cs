using System.Collections.Generic;
using System.Dynamic;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public class Object : DynamicObject
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public Object()
        {
        }

        public Object(Dictionary<string, object> props)
        {
            _properties = props;
        }

        public object this[string k]
        {
            get { return _properties[k]; }
            set { _properties[k] = value; }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return _properties.TryGetValue(binder.Name, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _properties[binder.Name] = value;
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _properties.Keys;
        }
    }
}