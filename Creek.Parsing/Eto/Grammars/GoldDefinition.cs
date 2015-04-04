using System;
using System.Collections.Generic;
using Eto.Parse;
using Lib.Parsing.Eto;
using Lib.Parsing.Eto.Parsers;

namespace Creek.Parsing.Eto.Grammars
{
    public class GoldDefinition
    {
        private Parser separator;

        public GoldDefinition()
        {
            Properties = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            Sets = new Dictionary<string, Parser>(StringComparer.InvariantCultureIgnoreCase)
                       {
                           {"HT", Lib.Parsing.Eto.Terminals.Set(0x09)},
                           {"LF", Lib.Parsing.Eto.Terminals.Set(0x0A)},
                           {"VT", Lib.Parsing.Eto.Terminals.Set(0x0B)},
                           {"FF", Lib.Parsing.Eto.Terminals.Set(0x0C)},
                           {"CR", Lib.Parsing.Eto.Terminals.Set(0x0D)},
                           {"Space", Lib.Parsing.Eto.Terminals.Set(0x20)},
                           {"NBSP", Lib.Parsing.Eto.Terminals.Set(0xA0)},
                           {"LS", Lib.Parsing.Eto.Terminals.Set(0x2028)},
                           {"PS", Lib.Parsing.Eto.Terminals.Set(0x2029)},
                           {"Number", Lib.Parsing.Eto.Terminals.Range(0x30, 0x39)},
                           {"Digit", Lib.Parsing.Eto.Terminals.Range(0x30, 0x39)},
                           {
                               "Letter",
                               Lib.Parsing.Eto.Terminals.Range(0x41, 0x58) | Lib.Parsing.Eto.Terminals.Range(0x61, 0x78)
                           },
                           {
                               "AlphaNumeric",
                               Lib.Parsing.Eto.Terminals.Range(0x30, 0x39) | Lib.Parsing.Eto.Terminals.Range(0x41, 0x5A) |
                               Lib.Parsing.Eto.Terminals.Range(0x61, 0x7A)
                           },
                           {
                               "Printable",
                               Lib.Parsing.Eto.Terminals.Range(0x20, 0x7E) | Lib.Parsing.Eto.Terminals.Set(0xA0)
                           },
                           {
                               "Letter Extended",
                               Lib.Parsing.Eto.Terminals.Range(0xC0, 0xD6) | Lib.Parsing.Eto.Terminals.Range(0xD8, 0xF6) |
                               Lib.Parsing.Eto.Terminals.Range(0xF8, 0xFF)
                           },
                           {"Printable Extended", Lib.Parsing.Eto.Terminals.Range(0xA1, 0xFF)},
                           {
                               "Whitespace",
                               Lib.Parsing.Eto.Terminals.Range(0x09, 0x0D) | Lib.Parsing.Eto.Terminals.Set(0x20, 0xA0)
                           },
                       };
            Terminals = new Dictionary<string, Parser>(StringComparer.InvariantCultureIgnoreCase)
                            {
                                {"Whitespace", +Sets["Whitespace"]}
                            };
            Rules = new Dictionary<string, UnaryParser>(StringComparer.InvariantCultureIgnoreCase);
            CreateSeparator();
        }

        public Dictionary<string, string> Properties { get; private set; }

        public Dictionary<string, Parser> Sets { get; private set; }

        public Dictionary<string, Parser> Terminals { get; private set; }

        public Dictionary<string, UnaryParser> Rules { get; private set; }

        public Parser Comment
        {
            get { return Terminals.ContainsKey("Comment") ? Terminals["Comment"] : null; }
        }

        public Parser Whitespace
        {
            get { return Terminals.ContainsKey("Whitespace") ? Terminals["Whitespace"] : null; }
            set
            {
                Terminals["Whitespace"] = value;
                ClearSeparator();
            }
        }

        public Parser NewLine
        {
            get { return Terminals.ContainsKey("NewLine") ? Terminals["NewLine"] : null; }
        }

        public Parser Separator
        {
            get
            {
                if (separator == null)
                    CreateSeparator();
                return separator;
            }
        }

        internal string GrammarName
        {
            get
            {
                string name;
                if (Properties.TryGetValue("Start Symbol", out name))
                    return name.TrimStart('<').TrimEnd('>');
                else
                    return null;
            }
        }

        public Grammar Grammar
        {
            get
            {
                UnaryParser parser;
                string symbol = GrammarName;
                if (!string.IsNullOrEmpty(symbol) && Rules.TryGetValue(symbol, out parser))
                    return parser as Grammar;
                else
                    return null;
            }
        }

        internal void ClearSeparator()
        {
            separator = null;
        }

        private void CreateSeparator()
        {
            var alt = new AlternativeParser();
            Parser p = Comment;
            if (p != null)
                alt.Items.Add(p);
            p = Whitespace;
            if (p != null)
                alt.Items.Add(p);
            if (alt.Items.Count == 0)
                separator = null;
            else
                separator = -alt;
        }
    }
}