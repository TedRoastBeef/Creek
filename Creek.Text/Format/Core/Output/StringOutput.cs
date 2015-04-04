using System.Text;
using Lib.Format.Core.Extensions;

namespace Lib.Format.Core.Output
{
    /// <summary>
    /// Wraps a StringBuilder so it can be used for output.
    /// This is used for the default output.
    /// </summary>
    public class StringOutput : IOutput
    {
        private readonly StringBuilder output;
        public StringOutput()
        {
            this.output = new StringBuilder();
        }
        public StringOutput(int capacity)
        {
            this.output = new StringBuilder(capacity);
        }
        public StringOutput(StringBuilder output)
        {
            this.output = output;
        }


        public void Write(string text, FormatDetails formatDetails)
        {
            output.Append(text);
        }
        public void Write(string text, int startIndex, int length, FormatDetails formatDetails)
        {
            output.Append(text, startIndex, length);
        }


        /// <summary>
        /// Returns the results of the StringBuilder.
        /// </summary>
        public override string ToString()
        {
            return output.ToString();
        }
    }
}
