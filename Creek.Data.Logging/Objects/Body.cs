namespace Creek.Data.Logging.Objects
{
    using System.Xml.Serialization;

    using Creek.Data.Logging.Objects.InnerBody;

    public class Body
    {
        [XmlElement(ElementName = "h1")]
        public string H1 { get; set; }
        public bool ShouldSerializeH1()
        {
            return !string.IsNullOrEmpty(this.H1);
        }

        [XmlElement(ElementName = "div")]
        public Div Div { get; set; }
    }
}
