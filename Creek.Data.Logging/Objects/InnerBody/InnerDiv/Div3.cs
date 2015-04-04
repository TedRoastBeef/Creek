namespace Creek.Data.Logging.Objects.InnerBody.InnerDiv
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv3;

    public class Div3
    {
        [XmlElement(ElementName = "div")]
        public List<Div4> Div4 { get; set; }
    }
}
