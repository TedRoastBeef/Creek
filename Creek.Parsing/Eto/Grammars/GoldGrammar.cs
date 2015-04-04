using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.IO;
using System.Linq;
using Eto.Parse;
using Lib.Parsing.Eto;
using Lib.Parsing.Eto.Parsers;
using Lib.Parsing.Eto.Writers;

namespace Creek.Parsing.Eto.Grammars
{
    /// <summary>
    /// Grammar to build a parser grammar using Gold Meta-Language
    /// </summary>
    /// <remarks>
    /// See http://goldparser.org/doc/grammars/
    /// 
    /// This grammar is not thread-safe.
    /// </remarks>
    public class GoldGrammar : Grammar
    {
        private readonly Parser handle;
        private readonly Parser parameter;
        private readonly Parser regExp;
        private readonly Parser regExpItem;
        private readonly Parser ruleDecl;
        private readonly Parser setDecl;
        private readonly Parser symbol;
        private readonly Parser terminalDecl;
        private readonly Parser whitespace;
        private GoldDefinition definition;

        public GoldGrammar()
            : base("gold")
        {
            Parser oldSeparator = DefaultSeparator;
            // Special Terminals

            ExceptParser parameterCh = Terminals.Printable - Terminals.Set("\"'");
            AlternativeParser nonterminalCh = Terminals.LetterOrDigit | Terminals.Set("_-. ");
            AlternativeParser terminalCh = Terminals.LetterOrDigit | Terminals.Set("_-.");
            ExceptParser literalCh = Terminals.Printable - Terminals.Set('\'');
            ExceptParser setLiteralCh = Terminals.Printable - Terminals.Set("[]'");
            ExceptParser setNameCh = Terminals.Printable - Terminals.Set("{}");

            SequenceParser parameterName = ('"' & (+parameterCh).WithName("value") & '"').Separate();
            SequenceParser nonterminal = ('<' & (+nonterminalCh).WithName("value") & '>').Separate();
            AlternativeParser terminal =
                ((+terminalCh).WithName("terminal") | ('\'' & (-literalCh).WithName("literal") & '\'')).Separate();
            SequenceParser setLiteral =
                ('[' & +(setLiteralCh.WithName("ch") | '\'' & (-literalCh).WithName("ch") & '\'') & ']').WithName(
                    "setLiteral");
            SequenceParser setName = ('{' & (+setNameCh).WithName("value") & '}').WithName("setName");

            // Line-Based Grammar Declarations

            var comments = new GroupParser("!*", "*!", "!");
            Parser newline = Terminals.Eol;
            whitespace = -(Terminals.SingleLineWhiteSpace | comments);
            DefaultSeparator = whitespace;
            RepeatParser nlOpt = -newline;
            AlternativeParser nl = +newline | Terminals.End;

            // Parameter Definition

            AlternativeParser parameterItem = parameterName | terminal | setLiteral | setName | nonterminal;

            RepeatParser parameterItems = +parameterItem;

            SequenceParser parameterBody = parameterItems & -(nlOpt & '|' & parameterItems);

            parameter =
                (parameterName.Named("name") & nlOpt & '=' & parameterBody.WithName("body") & nl).WithName("parameter");

            // Set Definition

            AlternativeParser setItem = setLiteral | setName;

            var setExp = new AlternativeParser {Name = "setExp"};
            setExp.Add((setExp & nlOpt & '+' & setItem).WithName("add"),
                       (setExp & nlOpt & '-' & setItem).WithName("sub"),
                       setItem);


            setDecl = (setName & nlOpt & '=' & setExp & nl).WithName("setDecl");

            //  Terminal Definition

            var regExp2 = new SequenceParser();

            OptionalParser kleeneOpt = (~((Parser) '+' | '?' | '*')).WithName("kleene");

            regExpItem = ((setLiteral & kleeneOpt)
                          | (setName & kleeneOpt)
                          | (terminal.Named("terminal") & kleeneOpt)
                          | ('(' & regExp2.Named("regExp2") & ')' & kleeneOpt)).WithName("regExpItem");

            RepeatParser regExpSeq = (+regExpItem).WithName("regExpSeq");

            regExp2.Items.Add(regExpSeq);
            regExp2.Items.Add(-('|' & regExpSeq));

            regExp = (regExpSeq & -(nlOpt & '|' & regExpSeq)).WithName("regExp");

            RepeatParser terminalName = +terminal;

            terminalDecl = (terminalName.Named("name") & nlOpt & '=' & regExp & nl).WithName("terminalDecl");

            // Rule Definition
            symbol = (terminal.Named("terminal") | nonterminal.Named("nonterminal")).WithName("symbol");

            handle = (-symbol).WithName("handle");
            SequenceParser handles = handle & -(nlOpt & '|' & handle);
            ruleDecl = (nonterminal.Named("name") & nlOpt & "::=" & handles & nl).WithName("ruleDecl");

            // Rules

            AlternativeParser definitionDecl = parameter | setDecl | terminalDecl | ruleDecl;

            RepeatParser content = -definitionDecl;

            Inner = nlOpt & content & nlOpt;

            DefaultSeparator = oldSeparator;
            AttachEvents();
        }

        private void AttachEvents()
        {
            // attach logic to parsers
            parameter.PreMatch += m =>
                                      {
                                          string name = m["name"]["value"].Text;
                                          string value = m["body"].Text;
                                          definition.Properties[name] = value;
                                          bool val;
                                          if (
                                              string.Equals("Auto Whitespace", name, StringComparison.OrdinalIgnoreCase) &&
                                              bool.TryParse(value, out val) && !val)
                                              definition.Whitespace = null;
                                      };

            ruleDecl.Matched += m =>
                                    {
                                        string name = m["name"]["value"].Text;
                                        bool addWhitespace = name == definition.GrammarName;
                                        Parser parser = Alternative(m, "handle",
                                                                    r => Sequence(r, "symbol", Symbol, addWhitespace));
                                        definition.Rules[name].Inner = parser;
                                    };
            ruleDecl.PreMatch += m =>
                                     {
                                         string name = m["name"]["value"].Text;
                                         UnaryParser parser;
                                         if (name == definition.GrammarName)
                                             parser = new Grammar(name);
                                         else
                                             parser = new UnaryParser(name);
                                         definition.Rules.Add(parser.Name, parser);
                                     };

            terminalDecl.Matched += m =>
                                        {
                                            Parser inner = Sequence(m, "regExp", RegExp);
                                            var parser = m.Tag as UnaryParser;
                                            if (parser != null)
                                                parser.Inner = inner;
                                            var groupParser = m.Tag as GroupParser;
                                            string name = m["name"].Text;
                                            if (groupParser != null)
                                            {
                                                if (name.EndsWith(" Start", StringComparison.Ordinal))
                                                    groupParser.Start = inner;
                                                else if (name.EndsWith(" End", StringComparison.Ordinal))
                                                    groupParser.End = inner;
                                                else if (name.EndsWith(" Line", StringComparison.Ordinal))
                                                    groupParser.Line = inner;
                                                int count = name.EndsWith(" Start", StringComparison.Ordinal)
                                                                ? 6
                                                                : name.EndsWith(" Line", StringComparison.Ordinal)
                                                                      ? 5
                                                                      : name.EndsWith(" End", StringComparison.Ordinal)
                                                                            ? 4
                                                                            : 0;
                                                name = name.Substring(0, name.Length - count);
                                            }

                                            if (name.Equals("Comment", StringComparison.OrdinalIgnoreCase)
                                                || name.Equals("Whitespace", StringComparison.OrdinalIgnoreCase)
                                                )
                                            {
                                                definition.ClearSeparator();
                                            }
                                        };

            terminalDecl.PreMatch += m =>
                                         {
                                             string name = m["name"].Text;
                                             if (name.EndsWith(" Start", StringComparison.Ordinal) ||
                                                 name.EndsWith(" End", StringComparison.Ordinal) ||
                                                 name.EndsWith(" Line", StringComparison.Ordinal))
                                             {
                                                 Parser parser;
                                                 int count = name.EndsWith(" Start", StringComparison.Ordinal)
                                                                 ? 6
                                                                 : name.EndsWith(" Line", StringComparison.Ordinal)
                                                                       ? 5
                                                                       : name.EndsWith(" End", StringComparison.Ordinal)
                                                                             ? 4
                                                                             : 0;
                                                 name = name.Substring(0, name.Length - count);
                                                 if (definition.Terminals.TryGetValue(name, out parser))
                                                 {
                                                     parser = parser as GroupParser ?? new GroupParser();
                                                 }
                                                 else
                                                     parser = new GroupParser();
                                                 m.Tag = definition.Terminals[name] = parser;
                                             }
                                             else
                                                 m.Tag = definition.Terminals[name] = new UnaryParser(name);
                                         };

            setDecl.PreMatch += m =>
                                    {
                                        Parser parser = SetMatch(m["setExp"]);
                                        definition.Sets[m["setName"]["value"].Text] = parser;
                                        m.Tag = parser;
                                    };
        }

        private static Parser Alternative(Match m, string innerName, Func<Match, Parser> inner)
        {
            Parser[] parsers = m.Find(innerName).Select(inner).ToArray();
            if (parsers.Length > 1)
                return new AlternativeParser(parsers);
            return parsers.FirstOrDefault();
        }

        private Parser Sequence(Match m, string innerName, Func<Match, Parser> inner, bool addWhitespace = false)
        {
            Parser[] parsers = m.Find(innerName).Select(inner).ToArray();
            if (addWhitespace || parsers.Length > 1)
            {
                Parser sep = definition.Separator;
                var seq = new SequenceParser(parsers) {Separator = sep};
                if (addWhitespace && sep != null)
                {
                    seq.Items.Insert(0, sep);
                    seq.Items.Add(sep);
                }
                return seq;
            }
            return parsers.FirstOrDefault();
        }

        private Parser Symbol(Match m)
        {
            Match child = m["nonterminal"];
            if (child.Success)
            {
                string name = child["value"].Text;
                UnaryParser parser;
                if (!definition.Rules.TryGetValue(name, out parser))
                    throw new FormatException(string.Format("Nonterminal '{0}' not found", name));

                return parser;
            }
            child = m["terminal"];
            if (child)
                return Terminal(child);
            throw new FormatException("Invalid symbol");
        }

        private Parser Terminal(Match m)
        {
            if (!m.Success)
                return null;
            Match l = m["literal"];
            if (l.Success)
                return new LiteralTerminal(l.Text.Length > 0 ? l.Text : "'");

            Match t = m["terminal"];
            if (t.Success)
                return definition.Terminals[t.Text];

            throw new FormatException("Invalid terminal");
        }

        private Parser RegExp(Match m)
        {
            return Alternative(m, "regExpSeq", r => Sequence(r, "regExpItem", RegExpItem));
        }

        private Parser RegExpItem(Match m)
        {
            if (!m.Success)
                return null;
            Parser item = RegExp(m["regExp2"]) ?? SetLiteralOrName(m, false) ?? Terminal(m["terminal"]);
            Match kleene = m["kleene"];
            switch (kleene.Text)
            {
                case "+":
                    return new RepeatParser(item, 1);
                case "*":
                    return new RepeatParser(item, 0);
                case "?":
                    return new OptionalParser(item);
                default:
                    return item;
            }
        }

        private Parser SetLiteralOrName(Match m, bool error = true)
        {
            Match literal = m.Name == "setLiteral" ? m : m["setLiteral"];
            if (literal.Success)
                return Terminals.Set(literal.Find("ch").Select(r => r.Text.Length > 0 ? r.Text[0] : '\'').ToArray());
            Match name = m.Name == "setName" ? m["value"] : m["setName"]["value"];
            if (name.Success)
            {
                Parser parser;
                string nameText = name.Text;
                if (definition.Sets.TryGetValue(nameText, out parser))
                    return parser;
                string[] chars = nameText.Split(new[] {".."}, StringSplitOptions.RemoveEmptyEntries);
                if (chars.Length == 2)
                {
                    // range of characters
                    return new CharRangeTerminal(Character(chars[0]), Character(chars[1]));
                }
                if (chars.Length == 1)
                {
                    // single character
                    return new SingleCharTerminal(Character(chars[0]));
                }
            }
            if (error)
                throw new FormatException("Literal or set name missing or invalid");
            return null;
        }

        private static char Character(string charName)
        {
            int ch;
            if (charName.StartsWith("&", StringComparison.Ordinal))
            {
                // hex value
                if (int.TryParse(charName.Substring(1), NumberStyles.AllowHexSpecifier, null, out ch))
                    return (char) ch;
            }
            else if (charName.StartsWith("#", StringComparison.Ordinal))
            {
                // hex value
                if (int.TryParse(charName.Substring(1), out ch))
                    return (char) ch;
            }
            throw new FormatException("Set characters are invalid");
        }

        private Parser SetMatch(Match m)
        {
            Parser parser = null;
            foreach (Match child in m.Matches)
            {
                if (parser == null)
                    parser = SetLiteralOrName(child);
                else if (child.Name == "add")
                    parser |= SetLiteralOrName(child);
                else if (child.Name == "sub")
                    parser -= SetLiteralOrName(child);
            }
            return parser;
        }

        protected override int InnerParse(ParseArgs args)
        {
            definition = new GoldDefinition();
            return base.InnerParse(args);
        }

        public GoldDefinition Build(string grammar)
        {
            GrammarMatch match = Match(grammar);
            if (!match.Success)
                throw new FormatException(string.Format("Error parsing gold grammar: {0}", match.ErrorMessage));
            return definition;
        }

        public string ToCode(string grammar, string className = "GeneratedGrammar")
        {
            using (var writer = new StringWriter())
            {
                ToCode(grammar, writer, className);
                return writer.ToString();
            }
        }

        public void ToCode(string grammar, TextWriter writer, string className = "GeneratedGrammar")
        {
            GoldDefinition definition = Build(grammar);
            var iw = new IndentedTextWriter(writer, "    ");

            iw.WriteLine("/* Date Created: {0}, Source:", DateTime.Now);
            iw.Indent ++;
            foreach (string line in grammar.Split('\n'))
                iw.WriteLine(line);
            iw.Indent --;
            iw.WriteLine("*/");

            var parserWriter = new CodeParserWriter
                                   {
                                       ClassName = className
                                   };
            parserWriter.Write(definition.Grammar, writer);
        }
    }
}