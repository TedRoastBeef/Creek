namespace Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv3
{
    using System.Xml.Serialization;

    using Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv1.InnerDiv2;
    using Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv3.InnerDiv4;

    public class Div4
    {
        [XmlElement(ElementName = "a")]
        public A A { get; set; }

        [XmlElement(ElementName = "h3")]
        public string H3 { get; set; }

        [XmlElement(ElementName = "table")]
        public Table Table { get; set; }
    }
}
