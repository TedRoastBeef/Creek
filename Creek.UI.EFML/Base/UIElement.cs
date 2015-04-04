using System.Collections.Generic;
using Creek.UI.Effects;

namespace Creek.UI.EFML.Base
{
    public class UiElement : ElementBase
    {
        public string content;
        public Dictionary<string, string> Events = new Dictionary<string, string>();
        public string ID;
        public IValidator Validator;
        public virtual IStyle style { get; set; }
        public Transition transition { get; set; }

        public UiElement()
        {
            Events.Add("onhover", null);
            Events.Add("onclick", null);
            Events.Add("onleave", null);
        }
    }
}