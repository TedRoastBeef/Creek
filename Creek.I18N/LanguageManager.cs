using System.Collections.Generic;
using System.IO;

namespace Creek.I18N
{
    public class LanguageManager
    {
        public Language Current;

        private Dictionary<string, Language> Languages;
        public LanguageManager()
        {
            Languages = new Dictionary<string,Language>();
            Current = new Language();
        }

        public void Load(string path)
        {
            foreach (var l in Directory.GetFiles(path))
            {
                var ll = new Language();
                ll.Load(l);

                Languages.Add(Path.GetFileNameWithoutExtension(l), ll);
            }
        }

    }
}
