using Lib.Parsing.Eto.Parsers;

namespace Lib.Parsing.Eto.Writers.Display
{
	public class LiteralWriter : global::Lib.Parsing.Eto.Writers.Display.ParserWriter<LiteralTerminal>
	{
		public override string GetName(ParserWriterArgs args, LiteralTerminal parser)
		{
			return string.Format("{0} [Value: '{1}']", base.GetName(args, parser), parser.Value);
		}
	}
}
