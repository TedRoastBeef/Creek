using System.Text.RegularExpressions;

namespace Creek.Parsing.Tokenizer
{
    public delegate Token CreateTokenDelegate(string type, string content, int position);
    
    partial class Tokenizer
    {
        private class Entry
        {
            private Regex m_Pattern = null;
            private string m_Type = null;
            private bool m_Ignore = false;
            private CreateTokenDelegate m_CreateToken = null;

            public Entry(Regex pattern, string type, bool ignore, CreateTokenDelegate createToken) {
                m_Pattern = pattern;
                m_Type = type;
                m_Ignore = ignore;
                m_CreateToken = createToken;
            }

            public Regex Pattern {
                get { return m_Pattern; }
            }

            public string Type {
                get { return m_Type; }
            }

            public bool Ignore {
                get { return m_Ignore; }
            }
            
            public CreateTokenDelegate CreateToken {
                get { return m_CreateToken; }
            }
        }
    }
}
