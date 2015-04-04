using System.Collections.Generic;

namespace Creek.UI.EFML.Base
{
    public class TagnameProvider : Dictionary<Tag, string>
    {
        public TagnameProvider()
        {
            Add(Tag.Dropdown, "dropdown");
            Add(Tag.Label, "label");
        }
    }
}