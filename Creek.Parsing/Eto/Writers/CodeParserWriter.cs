using Creek.Parsing.Eto;
using Eto.Parse;
using Lib.Parsing.Eto.Parsers;

namespace Lib.Parsing.Eto.Writers
{
	public class CodeParserWriter : TextParserWriter
	{
		public string ClassName { get; set; }

		public CodeParserWriter()
			: base(new ParserDictionary
			{
				{ typeof(Parser), new global::Lib.Parsing.Eto.Writers.Code.ParserWriter<Parser>() },
				{ typeof(Grammar), new global::Lib.Parsing.Eto.Writers.Code.GrammarWriter() },
				{ typeof(ListParser), new global::Lib.Parsing.Eto.Writers.Code.ListWriter<ListParser>() },
				{ typeof(UnaryParser), new global::Lib.Parsing.Eto.Writers.Code.UnaryWriter<UnaryParser>() },
				{ typeof(LiteralTerminal), new global::Lib.Parsing.Eto.Writers.Code.LiteralWriter() },
				{ typeof(RepeatParser), new global::Lib.Parsing.Eto.Writers.Code.RepeatWriter() },
				{ typeof(GroupParser), new global::Lib.Parsing.Eto.Writers.Code.GroupWriter() },
				{ typeof(SequenceParser), new global::Lib.Parsing.Eto.Writers.Code.SequenceWriter() },
				{ typeof(ExceptParser), new global::Lib.Parsing.Eto.Writers.Code.ExceptWriter() },
				{ typeof(StringParser), new global::Lib.Parsing.Eto.Writers.Code.StringWriter() },
				{ typeof(NumberParser), new global::Lib.Parsing.Eto.Writers.Code.NumberWriter() },
				{ typeof(CharRangeTerminal), new global::Lib.Parsing.Eto.Writers.Code.CharRangeWriter() },
				{ typeof(CharSetTerminal), new global::Lib.Parsing.Eto.Writers.Code.CharSetWriter() },
				{ typeof(BooleanTerminal), new global::Lib.Parsing.Eto.Writers.Code.BooleanWriter() },
				{ typeof(CharTerminal), new global::Lib.Parsing.Eto.Writers.Code.CharWriter() },
				{ typeof(SingleCharTerminal), new global::Lib.Parsing.Eto.Writers.Code.SingleCharWriter() },
				{ typeof(LookAheadParser), new global::Lib.Parsing.Eto.Writers.Code.InverseWriter<LookAheadParser>() },
			})
		{
		}
	}
}
