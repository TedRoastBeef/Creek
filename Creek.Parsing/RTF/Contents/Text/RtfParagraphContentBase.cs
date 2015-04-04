using Creek.Parsing.RTF.Contents.Paragraphs;

namespace Creek.Parsing.RTF.Contents.Text
{
    /// <summary>
    /// Can be used within a paragraph
    /// </summary>
    public abstract class RtfParagraphContentBase
    {
        internal RtfParagraphBase ParagraphInternal;

        public RtfParagraphBase Paragraph
        {
            get { return ParagraphInternal; }
        }
    }
}
