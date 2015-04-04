namespace Creek.Data.Logging.Objects.InnerBody.InnerDiv
{
    using System.Xml.Serialization;

    using Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv1;

    public class Div1
    {
        [XmlElement(ElementName = "h2")]
        public string H2 { get; set; }

        [XmlElement(ElementName = "div")]
        public Div2 Div2 { get; set; }
    }
}
