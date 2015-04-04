namespace Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv1
{
    using System.Xml.Serialization;

    using Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv1.InnerDiv2;

    public class Div2
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; } //overview

        [XmlElement(ElementName = "table")]
        public Table Table { get; set; }
    }
}
