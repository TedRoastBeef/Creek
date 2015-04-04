namespace Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv1.InnerDiv2.InnerTable
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv1.InnerDiv2.InnerTable.InnerTr;

    public class Tr
    {
        [XmlAttribute(AttributeName = "id")]
        public string Id { get; set; } //class
        public bool ShouldSerializeId()
        {
            return !String.IsNullOrEmpty(this.Id);
        }

        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; } //name
        public bool ShouldSerializeName()
        {
            return !String.IsNullOrEmpty(this.Name);
        }

        [XmlAttribute(AttributeName = "style")]
        public string Style { get; set; } //style
        public bool ShouldSerializeStyle()
        {
            return !String.IsNullOrEmpty(this.Style);
        }

        [XmlElement(ElementName = "th")]
        public List<Th> Th { get; set; }
        public bool ShouldSerializeTh()
        {
            return this.Th != null;
        }
        //public bool ShouldSerializeTh()
        //{
        //    return !string.IsNullOrEmpty(Th);
        //}

        //public Th Th { get; set; }
        //public bool ShouldSerializeTh()
        //{
        //    return Th != null;
        //}



        [XmlElement(ElementName = "td")]
        public List<Td> Td { get; set; }
        //public bool ShouldSerializeTd()
        //{
        //    return !string.IsNullOrEmpty(Td);
        //}

        public bool ShouldSerializeTd()
        {
            return this.Td != null;
        }
    }
}
