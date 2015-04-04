using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace Creek.Data.VFS
{
    [Serializable]
    public class Folder : IEntry
    {
        public Folder()
        {
            Entries = new List<IEntry>();
        }

        public bool ContainsFile(string name)
        {
            return Entries.Any(e => (e as File).Header.Filename == name);
        }

        public string Name { get; set; }
        public string Comment { get; set; }
        public List<IEntry> Entries { get; set; }
        public int Size { get { return Entries.Count; } }

        public void AddFile(string name, string content, string comment = "")
        {
            Entries.Add(new File { Header = new Header { Filename = name, Size = content.Length, Comment = comment }, Content = content });
        }
        public void AddFile(string path)
        {
            AddFile(Path.GetFileName(path), System.IO.File.ReadAllText(path));
        }
        public void AddFile<t>(string path)
        {
            if (typeof(t) == typeof(Image))
            {
                var content = ImageToBase64(Image.FromFile(path), ImageFormat.Jpeg);
                Entries.Add(new File { Header = new Header { Filename = Path.GetFileName(path), Size = content.Length }, Content = content });
            }
        }
        public void Clear()
        {
            Entries.Clear();
        }
        
        public void AddFile(File f)
        {
            Entries.Add(f);
        }
        private string ImageToBase64(Image image, ImageFormat format)
        {
            using (var ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                var imageBytes = ms.ToArray();

                // Convert byte[] to Base64 String
                var base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }
        public void AddFolder(Folder f)
        {
            for (var index = 0; index < Entries.Count; index++)
            {
                if (Entries[index] is Folder)
                {
                    var entry = Entries[index] as Folder;
                    if (entry.Name == f.Name)
                    {
                        Entries.Remove(f);
                        break;
                    }
                }
            }
            Entries.Add(f);
        }
        public void AddFolder(string name)
        {
            Entries.Add(new Folder { Name = name });
        }

        public Folder GetFolder(string name)
        {
            foreach (var f in Entries)
            {
                if (f is Folder)
                {
                    var ff = f as Folder;
                    if (ff.Name == name)
                    {
                        return ff;
                    }
                }
            }
            return new Folder();
        }
        public List<File> GetFiles()
        {
            return Entries.Cast<File>().ToList();
        }
        public File GetFile(string name)
        {
            return Entries.Where(entry => (entry as File).Header.Filename == name).Cast<File>().FirstOrDefault();
        }

        public void DeleteFile(string name)
        {
            foreach (var entry in Entries.Where(entry => (entry as File).Header.Filename == name))
            {
                Entries.Remove(entry);
                break;
            }
        }
        public void DeleteFolder(string name)
        {
            foreach (var entry in Entries.Where(entry => (entry as Folder).Name == name))
            {
                Entries.Remove(entry);
                break;
            }
        }
    }
}
