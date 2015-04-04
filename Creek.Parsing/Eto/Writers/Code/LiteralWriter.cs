using Lib.Parsing.Eto.Parsers;

namespace Lib.Parsing.Eto.Writers.Code
{
	public class LiteralWriter : global::Lib.Parsing.Eto.Writers.Code.ParserWriter<LiteralTerminal>
	{
		public override void WriteObject(TextParserWriterArgs args, LiteralTerminal parser, string name)
		{
			base.WriteObject(args, parser, name);
			if (parser.CaseSensitive != null)
				args.Output.WriteLine("{0}.CaseSensitive = {1};", name, parser.CaseSensitive.ToString().ToLowerInvariant());
			if (parser.Value != null)
				args.Output.WriteLine("{0}.Value = \"{1}\";", name, parser.Value.Replace("\"", "\\\""));
		}
	}
	
}
