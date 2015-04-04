namespace Creek.Data.Logging.Objects
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    using Creek.Data.Logging.Objects.InnerHead;

    public class Head
    {
        [XmlElement(ElementName = "meta")]
        public List<Meta> Meta { get; set; }

        [XmlElement(ElementName = "title")]
        public string Title { get; set; }
        public bool ShouldSerializeTitle()
        {
            return !string.IsNullOrEmpty(this.Title);
        }

        [XmlElement(ElementName = "style")]
        public string Style { get; set; }
        public bool ShouldSerializeStyle()
        {
            return !string.IsNullOrEmpty(this.Style);
        }

        //type="text/javascript" language="javascript"
        //type=\"text/javascript\" language=\"javascript\"
        [XmlElement(ElementName = "script")] 
        public string Script { get; set; }
        public bool ShouldSerializeScript()
        {
            return !string.IsNullOrEmpty(this.Script);
        }
    }
}
