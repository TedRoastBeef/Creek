namespace Creek.Data.Logging.Objects.InnerHead
{
    using System.Xml.Serialization;

    public class Meta
    {
        [XmlAttribute(AttributeName = "content")]
        public string Content { get; set; }

        [XmlAttribute(AttributeName = "http-equiv")]
        public string HttpEquiv { get; set; }
    }
}
