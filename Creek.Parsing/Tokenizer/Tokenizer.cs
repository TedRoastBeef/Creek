using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Creek.Parsing.Tokenizer
{
    public sealed partial class Tokenizer
    {
        private static readonly Regex m_LineReplacer = new Regex("(\r\n|\n|\r)",
                                                                 RegexOptions.Compiled | RegexOptions.Singleline);

        private static readonly Tokenizer m_Tokenizer = new Tokenizer();

        private readonly SortedList<int, Entry> m_Patterns = new SortedList<int, Entry>();

        static Tokenizer()
        {
            m_Tokenizer.Add("PATTERN", @"""(\\""|[^""])*""", 5, CreateToken);
            m_Tokenizer.Add("NUMBER", @"\d+", 4, (t, c, p) => new Token(t, c, p, Int32.Parse(c)));
            m_Tokenizer.Add("KEYWORD", @"var|const|function|print", 3, CreateToken);
            m_Tokenizer.Add("IDENTIFIER", @"\w+", 2, CreateToken);
            m_Tokenizer.Add("OPERATOR", @"\*", 1, CreateToken);
            m_Tokenizer.Add("WHITESPACE", @"\s+", 0, true, CreateToken);
        }

        public Token[] Tokenize(string source)
        {
            if (m_Patterns.Count == 0)
            {
                throw new TokenizerException("No patterns specified.");
            }

            var tokens = new List<Token>();
            int position = 0;

            // After maxLoops loops the source string MUST be empty, because every
            // step must shorten the source string (one out of all patterns has to
            // match!)
            int maxLoops = source.Length;

            while (source != null && !String.IsNullOrEmpty(source))
            {
                if (maxLoops-- <= 0)
                {
                    throw new TokenizerException(
                        "Reached maximum loop count! Check your patterns!");
                }

                bool isMatch = false;

                foreach (var pair in m_Patterns)
                {
                    Entry entry = pair.Value;
                    Match match = entry.Pattern.Match(source);

                    if (match.Success)
                    {
                        string matched = match.Value;

                        if (!entry.Ignore)
                        {
                            tokens.Add(pair.Value.CreateToken(entry.Type, matched, position));
                        }

                        int end = match.Index + match.Length;
                        position += end;
                        source = source.Substring(end);
                        isMatch = true;

                        break;
                    }
                }

                if (!isMatch)
                {
                    throw new TokenizerException(String.Format("No match found at position {0}!", position));
                }
            }

            return tokens.ToArray();
        }

        public void Add(string type, string regex, int weight, CreateTokenDelegate createToken)
        {
            Add(type, regex, weight, false, createToken);
        }

        public void Add(string type, string regex, int weight, bool ignore, CreateTokenDelegate createToken)
        {
            var pattern = new Regex("^(" + regex + ")", RegexOptions.Compiled | RegexOptions.Singleline);

            if (pattern.IsMatch(""))
            {
                throw new ArgumentException("Invalid regex! Must not match an empty string!");
            }

            // Inversed sorting - ensure that entries with highest weight are evaluated first instead of last!
            m_Patterns.Add(-weight, new Entry(pattern, type, ignore, createToken));
        }

        public static Tokenizer CreateFrom(string code)
        {
            Token[] tokens = m_Tokenizer.Tokenize(code);
            var tokenizer = new Tokenizer();
            var createToken = new CreateTokenDelegate(CreateToken);

            for (int i = 0; i < tokens.Length;)
            {
                string name = tokens[i++].Content;

                bool ignore = tokens[i].Type == "OPERATOR";

                if (ignore) ++i;

                string pattern = tokens[i++].Content;
                pattern = pattern.Substring(1, pattern.Length - 2);
                var weight = (int) tokens[i++].State;

                tokenizer.Add(name, pattern, weight, ignore, createToken);
            }

            return tokenizer;
        }

        public static Token CreateToken(string type, string content, int position)
        {
            return new Token(type, content, position, null);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var pair in m_Patterns)
            {
                Entry e = pair.Value;

                sb.Append(e.Type).Append(e.Ignore ? "*" : "").Append(" ");

                string pattern = e.Pattern.ToString();

                sb.Append('"').Append(pattern.Substring(2, pattern.Length - 3)).Append('"');

                // Inversed sorting - invert weight!
                sb.Append(" ").Append(-pair.Key).AppendLine();
            }

            return sb.ToString().Trim(" \r\n".ToCharArray());
        }
    }
}