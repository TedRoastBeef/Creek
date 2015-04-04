using System;
using System.IO;

namespace Creek.Messaging
{
    internal class Helper
    {
        public static string Path;
        public static void CreateMessage(Message m)
        {
            Path = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Creek.Messaging\" + m.ApplicationId;
            Directory.CreateDirectory(Path);
            File.WriteAllText(Path, m.Text);
        }
        public static void DeleteMessage(Message m)
        {
            Path = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Creek.Messaging\" + m.ApplicationId;
            File.Delete(Path);
        }
        public static Message ReadMessage(string appId)
        {
            Path = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments) + @"\Creek.Messaging\" + appId;
            var content = File.ReadAllText(Path);
            var r = new Message {Text = content, ApplicationId = appId};
            return r;
        }
    }
}
