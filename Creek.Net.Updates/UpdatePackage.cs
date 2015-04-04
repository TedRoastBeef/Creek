using System.Collections.Generic;
using System.IO;
using Creek.Net.Updates.Actions;

namespace Creek.Net.Updates
{
    public class UpdatePackage
    {
        public List<Action> Actions { get; set; }
        public string Version;
        public string Changelog;

        public UpdatePackage()
        {
            Actions = new List<Action>();
        }

        public string DownloadInfo()
        {
            return "";
        }

        public string Download()
        {
            var s = new MemoryStream();
            var bw = new BinaryWriter(s);

            bw.Write(Actions.Count);
            foreach (Action action in Actions)
            {
                if(action is DownloadAction)
                {
                    
                }
            }
            return "";
        }
    }
}
