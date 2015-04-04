using System;
using System.Globalization;
using System.Xml;

namespace Creek.Data.Registry
{
    internal class Storage
    {
        public Storage(string file)
        {
            StorageFile = file;
        }

        private string StorageFile { get; set; }

        public IEntry Read()
        {
            var root = new FolderEntry("Root");
            var settings = new XmlReaderSettings();
            XmlReader reader = null;

            try
            {
                reader = XmlReader.Create(StorageFile, settings);

                if (reader != null)
                {
                    var document = new XmlDocument();
                    document.Load(reader);

                    XmlNode rootFolder = document.SelectSingleNode("Registry/Folder");
                    if (rootFolder != null)
                    {
                        ReadFromDocument(root, rootFolder);
                    }
                    else
                    {
                        throw new RegistryException("Registry root tag not found!");
                    }
                }
                else
                {
                    throw new RegistryException("Could not open file!");
                }
            }
            catch (XmlException xmlEx)
            {
                throw new RegistryException("Could not load xml document.See inner exception.", xmlEx);
            }
            catch (Exception ex)
            {
                throw new RegistryException("Unknown error occured.See inner exception", ex);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return root;
        }

        private void ReadFromDocument(IEntry root, XmlNode parent)
        {
            XmlNodeList children = parent.SelectNodes("*");
            if (children != null)
            {
                foreach (XmlNode childNode in children)
                {
                    XmlAttribute keyAttribute = childNode.Attributes["Key"];
                    if (keyAttribute == null)
                    {
                        throw new RegistryException("Key attribute not found.");
                    }

                    string name = childNode.LocalName;
                    string key = keyAttribute.InnerText;
                    IEntry childEntry = null;
                    if (name.Equals("Folder"))
                    {
                        //
                        //  Create a new folder and read the children
                        //
                        childEntry = root.AddFolder(key);
                        ReadFromDocument(childEntry, childNode);
                    }

                    if (name.Equals("Value"))
                    {
                        //
                        //  Create a value 
                        //
                        childEntry = root.AddValue(key);
                        ReadValue(childEntry, childNode);
                    }
                }
            }
        }

        private void ReadValue(IEntry child, XmlNode childNode)
        {
            XmlNode contentNode = childNode.SelectSingleNode("Content");
            XmlNode formatNode = childNode.SelectSingleNode("Format");
            if (contentNode == null || formatNode == null)
            {
                throw new RegistryException("Invalid value format!");
            }

            string content = contentNode.InnerText;
            string format = formatNode.InnerText;

            switch (format)
            {
                case "Int":
                    child.SetValue(content, ValueFormat.Int);
                    break;

                case "Double":
                    child.SetValue(content, ValueFormat.Double);
                    break;

                case "Long":
                    child.SetValue(content, ValueFormat.Long);
                    break;

                case "String":
                    child.SetValue(content, ValueFormat.String);
                    break;

                case "Date":
                    child.SetValue(content, ValueFormat.Date);
                    break;

                default:
                    throw new RegistryException("Unknown value format found.");
            }
        }

        public void Write(IEntry root)
        {
            var settings = new XmlWriterSettings {Indent = true};

            XmlWriter writer = null;
            try
            {
                writer = XmlWriter.Create(StorageFile, settings);
                if (writer != null)
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("Registry");

                    WriteFolder(writer, root);

                    writer.WriteEndElement();
                }
                else
                {
                    throw new RegistryException("Could not open file");
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (writer != null)
                {
                    writer.Close();
                }
            }
        }

        private void WriteFolder(XmlWriter writer, IEntry root)
        {
            writer.WriteStartElement("Folder");
            writer.WriteAttributeString("Key", root.Key);

            foreach (IEntry child in root.Children)
            {
                if (child.IsFolder)
                {
                    WriteFolder(writer, child);
                }
                else
                {
                    WriteValue(writer, child);
                }
            }

            writer.WriteEndElement();
        }

        private void WriteValue(XmlWriter writer, IEntry child)
        {
            writer.WriteStartElement("Value");
            writer.WriteAttributeString("Key", child.Key);

            object content = child.GetValue();
            ValueFormat format = child.GetValueFormat();

            CultureInfo neutralCulture = CultureInfo.CreateSpecificCulture("en-US");

            writer.WriteElementString("Content", Convert.ToString(content, neutralCulture));
            writer.WriteElementString("Format", format.ToString());

            writer.WriteEndElement();
        }
    }
}