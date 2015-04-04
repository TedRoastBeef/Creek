using System.IO;

namespace Lib.Scripting.ScriptingTypes
{
    public class File
    {
        private string path;
        public File(string file)
        {
            Info = new FileInfo(file);
            path = file;
        }

        public FileInfo Info { get; set; }

        public void Read(dynamic callback)
        {
            System.IO.File.ReadAllText(path);
            if (callback != null)
                callback();
        }

    }
}
