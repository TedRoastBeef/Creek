namespace Creek.Extensibility.Addins
{
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;

    public class AddinRegistry : Collection<Addin>
    {
        #region Public Properties

        public string Path { get; set; }

        #endregion

        #region Public Methods and Operators

        public void Initialize(string path)
        {
            path = path.Replace(
                "[ApplicationData]",
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
            path = path.Replace("[StartupPath]", Application.StartupPath);

            this.Path = path;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            if (!Directory.Exists(path + "\\dependencies\\"))
            {
                Directory.CreateDirectory(path + "\\dependencies\\");
            }

            foreach (string f in Directory.GetFiles(path, "*.dll"))
            {
                try
                {
                    Assembly ass;
                    ass = Assembly.LoadFile(f);

                    object[] addAttr = ass.GetCustomAttributes(typeof(AddinAttribute), false);
                    if (addAttr != null && addAttr.Length != 0)
                    {
                        //domain.Load(ass.FullName);

                        string manifest = "manifest.xml";

                        object[] man = ass.GetCustomAttributes(typeof(ManifestAttribute), true);
                        if (man.Length == 1)
                        {
                            if (man[0] != null)
                            {
                                var m = man[0] as ManifestAttribute;
                                manifest = m.Name;
                            }
                        }

                        Stream str = ass.GetManifestResourceStream(ass.GetName().Name + "." + manifest);

                        if (str != null)
                        {
                            string r = new StreamReader(str).ReadToEnd();
                            Addin ad = new ManifestReader().Read(ass, r);
                            ad.Icon = ad.IconPath != ""
                                ? Image.FromStream(
                                    ass.GetManifestResourceStream(ass.GetName().Name + "." + ad.IconPath))
                                : null;

                            AppDomain domain = AppDomain.CreateDomain(ad.Name);

                            foreach (var dependency in ad.Dependencies)
                            {
                                domain.Load(dependency);
                            }

                            ad.Domain = domain;

                            this.Add(ad);
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        #endregion
    }
}