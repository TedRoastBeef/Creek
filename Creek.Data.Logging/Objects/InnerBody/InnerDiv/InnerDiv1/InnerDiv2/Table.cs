namespace Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv1.InnerDiv2
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv1.InnerDiv2.InnerTable;

    public class Table
    {
        [XmlElement(ElementName = "tr")]
        public List<Tr> Tr { get; set; }
    }
}
