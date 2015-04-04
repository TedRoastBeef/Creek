using System.Linq;

namespace Creek.Behaviors
{
    public class EnumBehavior<t>
    {
        public EnumBehavior(object v)
        {
            Value = v;
        }

        public object Value { get; set; }

        public static t Parse(string s)
        {
            var p = default(t);
            return p;
        }

        public static string[] GetNames()
        {
            var typ = typeof (t);
            return (from prop in typ.GetFields() where prop.DeclaringType.Name == typ.Name select prop.Name).ToArray();
        }
        public static object[] GetValues()
        {
            var typ = typeof(t);
            return (from prop in typ.GetFields() where prop.DeclaringType.Name == typ.Name select (prop.GetValue(default(t)) as EnumBehavior<t>).Value).ToArray();
        }

        public static bool isDefined(string name, bool casesensitive = false)
        {
            if (!casesensitive)
                name = name.ToLower();

            var p = typeof (t).GetFields();
            return p.Any(fieldInfo => fieldInfo.Name.ToLower()== name);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

}
