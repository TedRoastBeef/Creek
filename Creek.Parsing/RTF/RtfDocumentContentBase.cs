namespace Creek.Parsing.RTF
{
    /// <summary>
    /// Can be used within an RtfDocument
    /// </summary>
    public abstract class RtfDocumentContentBase
    {
        protected RtfDocument _document;

        internal virtual RtfDocument DocumentInternal
        {
            get { return _document; }
            set { _document = value; }
        }
    }
}
