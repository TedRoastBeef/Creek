using System.Text;

namespace Lib.Parsing.Eto.Grammars.Custom.Markdown
{
	static class Extensions
	{
		public static void AppendUnixLine(this StringBuilder builder)
		{
			builder.Append('\n');
		}

		public static void AppendUnixLine(this StringBuilder builder, string line)
		{
			builder.Append(line);
			builder.Append('\n');
		}
	}
}

