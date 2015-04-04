using System.Xml;
using Creek.UI.Effects.XML.Converters;

namespace Creek.UI.Effects.XML
{
    internal class Parser
    {

        public Transition Parse(string xml)
        {
            var r = new Transition();
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            var e = doc.GetElementsByTagName("transition")[0];

            r.Type = e.Attributes["type"].Value;
            r.Time = NumberConverter.Convert(e.Attributes["time"].Value);
            if(e.Attributes["flashes"] != null)
                r.FlashTimes = NumberConverter.Convert(e.Attributes["flashes"].Value);

            foreach (XmlNode child in e.ChildNodes)
            {
                if(child.Name == "property")
                {
                    var p = new Property
                                {
                                    Name = child.Attributes["name"].Value,
                                    Type = child.Attributes["type"].Value,
                                    Value = child.Attributes["value"].Value
                                };
                    r.Properties.Add(p);
                }
            }

            return r;
        }
        public Effects.Transition ToTransition<T>(Transition t, T target)
        {
            ITransitionType tp;
            switch (t.Type)
            {
                case "linear":
                    tp = new TransitionType_Linear(t.Time);
                    break;
                case "bounce":
                    tp = new TransitionType_Bounce(t.Time);
                    break;
                case "flash":
                    tp = new TransitionType_Flash(t.FlashTimes, t.Time);
                    break;
                case "deceleration":
                    tp = new TransitionType_Deceleration(t.Time);
                    break;
                case "throwcatch":
                    tp = new TransitionType_ThrowAndCatch(t.Time);
                    break;
                default:
                    tp = new TransitionType_Linear(t.Time);
                    break;
            }
            var r = new Effects.Transition(tp);

            foreach (var property in t.Properties)
            {
                object val = null;

                switch (property.Type)
                {
                    case "int":
                        val = NumberConverter.Convert(property.Value);
                        break;
                    case "color":
                        val = ColorConverter.Convert(property.Value);
                        break;
                }

                r.add(target, property.Name, val);
            }

            return r;
        }
    }
}
