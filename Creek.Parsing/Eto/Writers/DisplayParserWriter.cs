using Creek.Parsing.Eto;
using Eto.Parse;
using Lib.Parsing.Eto.Parsers;

namespace Lib.Parsing.Eto.Writers
{
	public class DisplayParserWriter : TextParserWriter
	{
		public DisplayParserWriter()
			: base(new ParserDictionary
			{
				{ typeof(Parser), new global::Lib.Parsing.Eto.Writers.Display.ParserWriter<Parser>() },
				{ typeof(ListParser), new global::Lib.Parsing.Eto.Writers.Display.ListWriter() },
				{ typeof(UnaryParser), new global::Lib.Parsing.Eto.Writers.Display.UnaryWriter<UnaryParser>() },
				{ typeof(LiteralTerminal), new global::Lib.Parsing.Eto.Writers.Display.LiteralWriter() },
				{ typeof(RepeatParser), new global::Lib.Parsing.Eto.Writers.Display.RepeatWriter() }
			})
		{
			Indent = " ";
		}
	}
}
