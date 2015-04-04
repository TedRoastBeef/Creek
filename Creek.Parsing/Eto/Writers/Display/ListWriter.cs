using Creek.Parsing.Eto;

namespace Lib.Parsing.Eto.Writers.Display
{
	public class ListWriter : global::Lib.Parsing.Eto.Writers.Display.ParserWriter<ListParser>
	{
		public override void WriteContents(TextParserWriterArgs args, ListParser parser, string name)
		{
			parser.Items.ForEach(r => args.Write(r));
		}
	}
	
}
