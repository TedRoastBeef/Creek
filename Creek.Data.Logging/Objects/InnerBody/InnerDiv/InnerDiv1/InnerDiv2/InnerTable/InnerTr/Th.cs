namespace Creek.Data.Logging.Objects.InnerBody.InnerDiv.InnerDiv1.InnerDiv2.InnerTable.InnerTr
{
    using System;
    using System.Xml.Serialization;

    public class Th
    {
        public Th()
        {

        }

        public Th(string locId, string value = "", string clss = "")
        {
            this.Locid = locId;
            this.Value = value;
            this.Clss = clss;
        }

        [XmlAttribute(AttributeName = "class")]
        public string Clss { get; set; } //class
        public bool ShouldSerializeClss()
        {
            return !String.IsNullOrEmpty(this.Clss);
        }

        [XmlAttribute(AttributeName = "_locid")]
        public string Locid { get; set; } //Locid
        public bool ShouldSerializeLocid()
        {
            return !String.IsNullOrEmpty(this.Locid);
        }

        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
        public bool ShouldSerializeValue()
        {
            return !String.IsNullOrEmpty(this.Value);
        }
    }
}
