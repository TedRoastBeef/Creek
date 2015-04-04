namespace Creek.Data.Logging.Objects.InnerBody
{
    using System.Xml.Serialization;

    using Creek.Data.Logging.Objects.InnerBody.InnerDiv;

    public class Div
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; } //content

        [XmlElement(ElementName = "div1")]
        public Div1 Div1 { get; set; }

        [XmlElement(ElementName = "h2")]
        public string H2 { get; set; }

        [XmlElement(ElementName = "div")]
        public Div3 Div3 { get; set; }
    }
}
