namespace Lib.Parsing.Eto.Writers.Display
{
	public class UnaryWriter<T> : Lib.Parsing.Eto.Writers.Display.ParserWriter<T>
		where T: UnaryParser
	{
		public override void WriteContents(TextParserWriterArgs args, T parser, string name)
		{
			args.Write(parser.Inner);
		}
	}
	
}
