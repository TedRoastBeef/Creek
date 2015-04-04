using System.IO;
using Lib.Format.Core.Extensions;

namespace Lib.Format.Core.Output
{
    /// <summary>
    /// Wraps a TextWriter so that it can be used for output.
    /// </summary>
    public class TextWriterOutput : IOutput
    {
        public TextWriterOutput(TextWriter output)
        {
            Output = output;
        }
        public TextWriter Output { get; private set; }

        public void Write(string text, FormatDetails formatDetails)
        {
            Output.Write(text);
        }

        public void Write(string text, int startIndex, int length, FormatDetails formatDetails)
        {
            Output.Write(text.Substring(startIndex, length));
        }
    }
}
