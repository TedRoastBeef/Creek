using System.Collections.Generic;
using Creek.Parsing.Eto;

namespace Lib.Parsing.Eto.Writers.Code
{
	public class ListWriter<T> : global::Lib.Parsing.Eto.Writers.Code.ParserWriter<T>
		where T: ListParser
	{
		public override void WriteContents(TextParserWriterArgs args, T parser, string name)
		{
			base.WriteContents(args, parser, name);
			var items = new List<string>();
			parser.Items.ForEach(r => {
				var child = r != null ? args.Write(r) : "null";
				items.Add(child);
			});
			args.Output.WriteLine("{0}.Items.AddRange(new Eto.Parse.Parser[] {{ {1} }});", name, string.Join(", ", items));
		}
	}
	
}
