using Lib.Parsing.Eto.Parsers;

namespace Lib.Parsing.Eto.Writers.Code
{
	public class SequenceWriter : ListWriter<SequenceParser>
	{
		public override void WriteObject(TextParserWriterArgs args, SequenceParser parser, string name)
		{
			base.WriteObject(args, parser, name);
			if (parser.Separator != null)
				args.Output.WriteLine("{0}.Separator = {1};", name, args.Write(parser.Separator));
		}
	}
}
