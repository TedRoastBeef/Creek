using Eto.Parse;
using Lib.Parsing.Eto;
using Lib.Parsing.Eto.Parsers;

namespace Creek.Parsing.Eto.Grammars.Custom
{
    public class AddressGrammar : Grammar
    {
        public AddressGrammar()
            : base("postal-address")
        {
            RepeatParser terminal = +Terminals.LetterOrDigit;

            Parser aptNum = (((Parser) "Apt" | "Suite") & "#" & +Terminals.Digit).Named("apt-num");
            Parser streetType = ((Parser) "Street" | "Drive" | "Ave" | "Avenue").Named("street-type");
            Parser street = (terminal.Named("street-name") & ~streetType).Named("street");
            Parser zipPart =
                (terminal.Named("town-name") & "," & terminal.Named("state-code") & terminal.Named("zip-code")).Named(
                    "zip");
            Parser streetAddress = (terminal.Named("house-num") & street & aptNum).Named("street-address");

            // name
            Parser suffixPart = ((Parser) "Sr." | "Jr." | +Terminals.Set("IVXLCDM")).Named("suffix");
            Parser personalPart = (terminal.Named("first-name") | (Terminals.Letter & ".")).Named("personal");
            var namePart = new UnaryParser("name");
            namePart.Inner = (personalPart & terminal.Named("last-name") & ~suffixPart) | (personalPart & namePart);
                // recursion

            Inner = namePart & Terminals.Eol & streetAddress & ~Terminals.Eol & zipPart;
        }
    }
}