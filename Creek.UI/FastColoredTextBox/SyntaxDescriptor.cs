using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Creek.UI.FastColoredTextBox
{
    public class SyntaxDescriptor : IDisposable
    {
        public readonly List<FoldingDesc> foldings = new List<FoldingDesc>();
        public readonly List<RuleDesc> rules = new List<RuleDesc>();
        public readonly List<Style> styles = new List<Style>();
        public BracketsHighlightStrategy bracketsHighlightStrategy = BracketsHighlightStrategy.Strategy2;
        public char leftBracket = '(';
        public char leftBracket2 = '{';
        public char rightBracket = ')';
        public char rightBracket2 = '}';

        #region IDisposable Members

        public void Dispose()
        {
            foreach (Style style in styles)
                style.Dispose();
        }

        #endregion
    }

    public class RuleDesc
    {
        public RegexOptions options = RegexOptions.None;
        public string pattern;
        private Regex regex;
        public Style style;

        public Regex Regex
        {
            get
            {
                if (regex == null)
                {
                    regex = new Regex(pattern, SyntaxHighlighter.RegexCompiledOption | options);
                }
                return regex;
            }
        }
    }

    public class FoldingDesc
    {
        public string finishMarkerRegex;
        public RegexOptions options = RegexOptions.None;
        public string startMarkerRegex;
    }
}