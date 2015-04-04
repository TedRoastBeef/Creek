using Creek.Scripting.Commandparser.Exceptions;

namespace Creek.Scripting.Commandparser.Common
{
    public class BlockHeaderParser
    {
        public string Name;
        public SectionType Type;

        public static BlockHeaderParser Parse(string h)
        {
            var sh = new BlockHeaderParser();

            if (h.StartsWith("func"))
            {
                sh.Type = SectionType.Function;
                sh.Name = h.Remove(0, 4);
            }
            else if (h.StartsWith("class"))
            {
                sh.Type = SectionType.Class;
                sh.Name = h.Remove(0, 5);
            }
            else
            {
                throw new RuntimeError("unknown type of block");
            }

            return sh;
        }

    }
}
