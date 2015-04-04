
using System;

namespace Creek.Parsing.Tokenizer
{
    public class Token {
        protected bool Equals(Token other)
        {
            return string.Equals(m_Type, other.m_Type) && string.Equals(m_Content, other.m_Content) && m_Position == other.m_Position && Equals(m_State, other.m_State);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Token) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (m_Type != null ? m_Type.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (m_Content != null ? m_Content.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ m_Position;
                hashCode = (hashCode*397) ^ (m_State != null ? m_State.GetHashCode() : 0);
                return hashCode;
            }
        }

        private string m_Type = null;
        private string m_Content = null;
        private int m_Position = -1;
        private object m_State = null;

        public Token(string type, string content, int position, object state) {
            m_Type = type;
            m_Content = content;
            m_Position = position;
            m_State = state;
        }
        
        public string Type {
            get { return m_Type; }
        }
        
        public string Content {
            get { return m_Content; }
        }
        
        public int Position {
            get { return m_Position; }
        }

        public object State {
            get { return m_State; }
        }
        
        public override string ToString() {
            return String.Format("Token [type='{0}', content='{1}', position={2}, state='{3}']", m_Type, m_Content.Replace("\n", "\\n").Replace("\r", "\\r"), m_Position, m_State);
        }
        
        public static bool operator ==(Token t1, Token t2) {
            if ((object)t1 == null && (object)t2 == null)
                return true;
            else if ((object)t1 == null || (object)t2 == null)
                return false;
            else
                return t1.Type == t2.Type && t1.Content == t2.Content && t1.Position == t2.Position &&
                    (t1.State != null ? t1.State.Equals(t2.State) : true);
        }
        
        public static bool operator !=(Token t1, Token t2) {
            return !(t1 == t2);
        }
    }
}
