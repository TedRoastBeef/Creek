using System.Reflection;

namespace Creek.Messaging
{
    public class Message
    {
        internal string ApplicationId;
        internal string Text;

        public static Message Create(string appId, string content)
        {
            return  new Message {ApplicationId = appId, Text = content};
        }
        public static Message Create(string content)
        {
            return Create(Assembly.GetCallingAssembly().FullName, content);
        }

        public void Send()
        {
            Helper.CreateMessage(this);
        }

        public void Send(string to)
        {
            
        }
        public static explicit operator string(Message m)
        {
            return m.Text;
        }
    }
}
