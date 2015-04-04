using System;
using Eto.Parse;

namespace Lib.Parsing.Eto.Parsers
{
	public class DigitTerminal : CharTerminal
	{
		protected DigitTerminal(DigitTerminal other, ParserCloneArgs args)
			: base(other, args)
		{
		}

		public DigitTerminal()
		{
		}

		protected override bool Test(char ch)
		{
			return Char.IsDigit(ch);
		}

		protected override string CharName
		{
			get { return "Digit"; }
		}

		public override Parser Clone(ParserCloneArgs args)
		{
			return new DigitTerminal(this, args);
		}
	}
}
