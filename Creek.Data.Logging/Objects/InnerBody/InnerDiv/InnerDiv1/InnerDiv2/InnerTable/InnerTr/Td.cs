namespace Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv1.InnerDiv2.InnerTable.InnerTr
{
    using System;
    using System.Xml.Serialization;

    public class Td
    {
        public Td()
        {
            
        }

        public Td(string @class, string value = "", string style = "")
        {
            this.Class = @class;
            this.Value = value;
            this.Style = style;
        }

        [XmlAttribute(AttributeName = "class")]
        public string Class { get; set; } //class
        public bool ShouldSerializeClass()
        {
            return !String.IsNullOrEmpty(this.Class);
        }

        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
        public bool ShouldSerializeValue()
        {
            return !String.IsNullOrEmpty(this.Value);
        }

        [XmlAttribute(AttributeName = "style")]
        public string Style { get; set; }
        public bool ShouldSerializeStyle()
        {
            return !String.IsNullOrEmpty(this.Style);
        }
    }
}
