using System;
using Creek.Parsing.Eto;
using Lib.Parsing.Eto.Parsers;

namespace Lib.Parsing.Eto.Grammars.Custom
{
    public class XmlGrammar : Grammar
    {
        public XmlGrammar()
            : base("xml")
        {
            EnableMatchEvents = false;
            var comment = new GroupParser("<!--", "-->");
            RepeatCharTerminal ws = Terminals.Repeat(Char.IsWhiteSpace, 1);
            RepeatCharTerminal ows = Terminals.Repeat(Char.IsWhiteSpace, 0);
            RepeatParser wsc = -(ws | comment);

            RepeatCharTerminal name = Terminals.Repeat(new RepeatCharItem(Char.IsLetter, 1, 1),
                                                       new RepeatCharItem(Char.IsLetterOrDigit, 0));
            RepeatCharTerminal namedName =
                Terminals.Repeat(new RepeatCharItem(Char.IsLetter, 1, 1), new RepeatCharItem(Char.IsLetterOrDigit, 0)).
                    WithName("name");

            UntilParser text = new UntilParser("<", 1).WithName("text");
            var attributeValue = new StringParser {QuoteCharacters = new[] {'"'}, Name = "value"};
            SequenceParser attribute = (namedName & ows & "=" & ows & attributeValue);
            OptionalParser attributes = (ws & (+attribute).SeparatedBy(ws).WithName("attributes")).Optional();

            var content = new RepeatParser {Separator = wsc};

            SequenceParser startTag = "<" & namedName & attributes & ows;
            SequenceParser endTag = "</" & name & ">";
            SequenceParser obj = (startTag & ("/>" | (">" & wsc & content & wsc & endTag))).WithName("object");
            SequenceParser cdata = ("<![CDATA[" & new UntilParser("]]>", 0, skip: true)).WithName("cdata");
            content.Inner = obj | text | cdata;

            SequenceParser declaration = "<?" & name & attributes & ows & "?>";
            Inner = declaration & wsc & obj & wsc;
        }
    }
}