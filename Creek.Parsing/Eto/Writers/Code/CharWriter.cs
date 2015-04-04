using Lib.Parsing.Eto.Parsers;

namespace Lib.Parsing.Eto.Writers.Code
{
	public class CharWriter : InverseWriter<CharTerminal>
	{
		public override void WriteContents(TextParserWriterArgs args, CharTerminal parser, string name)
		{
			base.WriteContents(args, parser, name);
			if (parser.CaseSensitive != null)
				args.Output.WriteLine("{0}.CaseSensitive = {1};", name, parser.CaseSensitive.HasValue ? parser.CaseSensitive.ToString().ToLowerInvariant() : "null");
		}
	}
}

