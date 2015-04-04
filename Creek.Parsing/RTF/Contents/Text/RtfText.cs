using Creek.Parsing.RTF.Attributes;

namespace Creek.Parsing.RTF.Contents.Text
{
    /// <summary>
    /// Represents plain text.
    /// </summary>
    public class RtfText : RtfTextBase
    {
        /// <summary>
        /// Gets string value of the text.
        /// </summary>
        [RtfTextData]
        public string Text
        {
            get { return sb.ToString(); }
        }


        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfText class.
        /// </summary>
        public RtfText() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of ESCommon.Rtf.RtfText class.
        /// </summary>
        public RtfText(string text) : base(text)
        {
        }
    }
}