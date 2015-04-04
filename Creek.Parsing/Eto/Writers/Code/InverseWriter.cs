using Eto.Parse;

namespace Lib.Parsing.Eto.Writers.Code
{
	public class InverseWriter<T> : global::Lib.Parsing.Eto.Writers.Code.ParserWriter<T>
		where T: Parser, IInverseParser
	{
		public override void WriteContents(TextParserWriterArgs args, T parser, string name)
		{
			base.WriteContents(args, parser, name);
			if (parser.Inverse)
				args.Output.WriteLine("{0}.Inverse = {1};", name, parser.Inverse.ToString().ToLower());
		}
	}
}

