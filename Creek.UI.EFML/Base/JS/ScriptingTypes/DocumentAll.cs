using System.Collections.Generic;
using System.Dynamic;
using System.Windows.Forms;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes
{
    public class DocumentAll : DynamicObject
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        public DocumentAll(FlowLayoutPanel b)
        {
            foreach (Control c in b.Controls)
            {
                _properties.Add(c.Name, c);
            }
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

        public override bool TrySetMember(SetMemberBinder binder, object Content)
        {
            _properties[binder.Name] = Content;
            return true;
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return _properties.Keys;
        }
    }
}