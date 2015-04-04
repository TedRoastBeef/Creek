using Lib.Parsing.Eto.Parsers;

namespace Lib.Parsing.Eto.Writers.Code
{
	public class SingleCharWriter : InverseWriter<SingleCharTerminal>
	{
		public override void WriteContents(TextParserWriterArgs args, SingleCharTerminal tester, string name)
		{
			base.WriteContents(args, tester, name);
			args.Output.WriteLine("{0}.Character = (char)0x{1:x}; // {2}", name, (int)tester.Character, tester.Character);
		}
	}
}

