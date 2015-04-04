namespace Creek.Extensibility.Addins
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Xml;

    public class ManifestReader
    {
        #region Public Methods and Operators

        public Addin Read(Assembly ass, string manifest)
        {
            var a = new Addin();

            var xml = new XmlDocument();
            xml.LoadXml(manifest);

            foreach (XmlNode c in xml.FirstChild.ChildNodes)
            {
                if (c.Name == "meta")
                {
                    foreach (XmlNode meta in c.ChildNodes)
                    {
                        switch (meta.Name)
                        {
                            case "author":
                                a.Author = meta.InnerText;
                                break;
                            case "version":
                                a.Version =meta.InnerText;
                                break;
                            case "name":
                                a.Name = meta.InnerText;
                                break;
                            case "icon":
                                a.IconPath = meta.InnerText;
                                break;
                            case "description":
                                a.Description = meta.InnerText;
                                break;
                        }
                    }
                }
                else if (c.Name == "extension")
                {
                    var en = new ExtensionNode();
                    en.Path = c.Attributes["path"].Value;

                    foreach (XmlNode cc in c.ChildNodes)
                    {
                        if (cc.Name == "command")
                        {
                            en.Commands.Add(
                                cc.Attributes["class"].Value.Replace(
                                    "{AddinNamespace}",
                                    Path.GetFileNameWithoutExtension(ass.Location)),
                                Activator.CreateInstance(
                                    ass.GetType(
                                        cc.Attributes["class"].Value.Replace(
                                            "{AddinNamespace}",
                                            Path.GetFileNameWithoutExtension(ass.Location)))));
                        }
                        else
                        {
                            if (cc.Attributes.Count > 0)
                            {
                                var obj =
                                    Activator.CreateInstance(
                                        ass.GetType(
                                            cc.Attributes["class"].Value.Replace(
                                                "{AddinNamespace}",
                                                Path.GetFileNameWithoutExtension(ass.Location)))) as ExtensionCommand;
                                Type ot = obj.GetType();

                                obj.Name = cc.Name;

                                foreach (XmlAttribute att in cc.Attributes)
                                {
                                    PropertyInfo prop = ot.GetProperty(att.Name);
                                    if (prop != null)
                                    {
                                        prop.SetValue(obj, att.Value, null);
                                    }
                                }
                                en.Nodes.Add(obj);
                            }
                            else
                            {
                                en.Nodes.Add(new ExtensionCommand { Name = cc.Name });
                            }
                        }
                    }

                    a.ExtensionNodes.Add(en);
                }
                else if (c.Name == "dependencies")
                {
                    foreach (XmlNode dc in c.ChildNodes)
                    {
                        if (dc.Name == "dependency")
                        {
                            if (File.Exists(dc.Attributes["path"].Value))
                            {
                                a.Dependencies.Add(File.ReadAllBytes(dc.Attributes["path"].Value));
                            }
                            else
                            {
                                throw new AddinException(
                                    "Dependency '" + dc.Attributes["path"].Value + "' does not exist!");
                            }
                        }
                    }
                }
            }

            return a;
        }

        #endregion
    }
}