namespace Creek.Data.Logging.Objects
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlRoot(ElementName = "html")]
    public class Html
    {
        [XmlElement(ElementName = "head")]
        public Head Head { get; set; }

        [XmlElement(ElementName = "body")]
        public Body Body { get; set; }
    }
}
