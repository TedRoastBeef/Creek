using System.Collections.Generic;

namespace Creek.UI.Effects.XML
{
    internal class Transition
    {
        public string Type;
        public int Time;
        public int FlashTimes;
        public List<Property> Properties = new List<Property>();
    }
    internal class Property
    {
        public string Type;
        public string Name;
        public string Value;
    }
}
