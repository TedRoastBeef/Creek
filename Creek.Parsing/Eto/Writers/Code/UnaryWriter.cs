namespace Lib.Parsing.Eto.Writers.Code
{
	public class UnaryWriter<T> : Lib.Parsing.Eto.Writers.Code.ParserWriter<T>
		where T: UnaryParser
	{
		public override void WriteContents(TextParserWriterArgs args, T parser, string name)
		{
			base.WriteContents(args, parser, name);
			if (parser.Inner != null)
			{
				var child = args.Write(parser.Inner);
				args.Output.WriteLine("{0}.Inner = {1};", name, child);
			}
		}
	}
}
