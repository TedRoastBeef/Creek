using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Creek.Data.VFS
{
    public class FileSystem
    {
        private List<IEntry> Entries = new List<IEntry>();

        public readonly string file;
        public FileSystem(string vfsfile)
        {
            file = vfsfile;
            if (System.IO.File.Exists(vfsfile))
            {
                var c = Encryption.decode(System.IO.File.ReadAllText(vfsfile));

                Entries = (List<IEntry>)DeserializeObject(c);
            }
        }
        public FileSystem()
        {
            Entries = new List<IEntry>();
        }

        public void Load(string vfsfile)
        {
            if (System.IO.File.Exists(vfsfile))
            {
                var c = Encryption.decode(System.IO.File.ReadAllText(vfsfile));

                Entries = (List<IEntry>)DeserializeObject(c);
            }
        }
        public void Clear()
        {
            Entries.Clear();
        }
        public void Save(string vfsfile)
        {
            System.IO.File.WriteAllText(vfsfile, Encryption.Encode(SerializeObject(Entries)));
        }

        private static string SerializeObject(object o)
        {
            if (!o.GetType().IsSerializable)
            {
                return null;
            }

            using (var stream = new MemoryStream())
            {
                new BinaryFormatter().Serialize(stream, o);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        private static object DeserializeObject(string str)
        {
            if (str != "")
            {
                var bytes = Convert.FromBase64String(str);

                using (var stream = new MemoryStream(bytes))
                {
                    return new BinaryFormatter().Deserialize(stream);
                }
            }
            return null;
        }

        public void Save()
        {
            System.IO.File.WriteAllText(file, Encryption.Encode(SerializeObject(Entries)));
        }

        public bool ContainsFile(string name)
        {
            return Entries.Any(e => (e as File).Header.Filename == name);
        }

        public void AddFile(string name, string content, string comment = "")
        {
            Entries.Add(new File {Header = new Header {Filename = name, Size = content.Length, Comment = comment}, Content=content});
        }
        public void AddFile(string path)
        {
            AddFile(Path.GetFileName(path), System.IO.File.ReadAllText(path));
        }
        public void AddFile(File f)
        {
            Entries.Add(f);
        }
        public void AddFile<t>(string path) 
        {
            if(typeof(t) == typeof(Image))
            {
                var content = ImageToBase64(Image.FromFile(path), ImageFormat.Jpeg);
                Entries.Add(new File { Header = new Header { Filename = Path.GetFileName(path), Size = content.Length }, Content = content });
            }
        }
        public void AddFolder(string name)
        {
            Entries.Add(new Folder {Name = name});
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

        public File GetFile(string name)
        {
            return Entries.Where(entry => (entry as File).Header.Filename == name).Cast<File>().FirstOrDefault();
        }
        public object GetFile<t>(string name)
        {
            if(typeof(t) == typeof(Image))
            {
                var img = Base64ToImage(GetFile(name).Content);
                return img;
            }
            return null;
        }
        public File[] GetFiles()
        {
            return (File[]) Entries.ToArray();
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

        public string ImageToBase64(Image image, ImageFormat format)
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
        public Image Base64ToImage(string base64String)
        {
            // Convert Base64 String to byte[]
            var imageBytes = Convert.FromBase64String(base64String);
            var ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            var image = Image.FromStream(ms, true);
            return image;
        }
    }
}
