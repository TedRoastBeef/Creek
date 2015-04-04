using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Creek.Data.Storage
{
    public class XmlFiles : IDataReader, IDataWriter
    {
        public Data Data = null;

        #region IDataReader Members

        public void SetData(Data data)
        {
            Data = data;
        }

        public void ReadData()
        {
            throw new Exception("This may not be called without path");
        }

        public void ReadData(string path)
        {
            ReadData(path, true);
        }

        #endregion

        #region IDataWriter Members

        public void WriteData()
        {
            throw new Exception("This may not be called without path");
        }

        public void WriteData(string path)
        {
            string xmlData = buildXmlData();
            File.WriteAllText(path, xmlData);
        }

        #endregion

        public void ReadData(string path, bool clear)
        {
            if (clear)
                Data.Clear();

            try
            {
                var xml = new XmlTextReader(path);
                readXmlData(xml);
                xml.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool isSubsection(XmlReader xml)
        {
            string type = xml.GetAttribute("type");
            return (!String.IsNullOrEmpty(type) && type.Equals("section"));
        }

        private string composeSectionName(List<string> sections)
        {
            string name = "";

            //foreach (string section in sections)
            //    name += section + "\\";

            // the first xml section "DataStorage" will be ignored
            for (int i = 1; i < sections.Count; i++)
            {
                string section = sections[i];
                name += section + "\\";
            }

            return name;
        }

        private void readXmlData(XmlReader xml)
        {
            var sections = new List<string>();

            while (xml.Read())
            {
                //Console.WriteLine(xml.NodeType);
                switch (xml.NodeType)
                {
                    case XmlNodeType.Element:
                        if (isSubsection(xml))
                        {
                            sections.Add(xml.Name);
                            string sectionName = composeSectionName(sections);
                            //Console.WriteLine("Entering: " + xml.Name);
                            //Console.WriteLine("Section: " + sectionName);
                            Data.AddSection(sectionName);
                        }
                        else
                        {
                            string valueName = composeSectionName(sections) + xml.Name;
                            string value = xml.ReadElementContentAsString();
                            Data.SetValue(valueName, value);
                            //Console.WriteLine(valueName + "=" + value);
                        }
                        break;
                        //
                        //you can handle other cases here
                        //
                    case XmlNodeType.EndElement:
                        //Console.WriteLine("Leaving: " + xml.Name);
                        sections.RemoveAt(sections.Count - 1);
                        break;
                        //case XmlNodeType.Text:
                        //    break;
                    default:
                        break;
                }
            }
            /*while (xml.Read())
            {
                switch (xml.NodeType)
                {
                    case XmlNodeType.Element: // Der Knoten ist ein Element.
                        Console.Write("<" + xml.Name);

                        while (xml.MoveToNextAttribute()) // Attribute lesen.
                            Console.Write(" " + xml.Name + "='" + xml.Value + "'");
                        Console.Write(">");
                        Console.WriteLine(">");
                        break;
                    case XmlNodeType.Text: //Anzeige des Textes in jedem der Elemente.
                        Console.WriteLine(xml.Value);
                        break;
                    case XmlNodeType.EndElement: //Ende der Elementanzeige.
                        Console.Write("</" + xml.Name);
                        Console.WriteLine(">");
                        break;
                }
            }// */
        }

        private string buildXmlData()
        {
            var xml = new XmlStringBuilder();
            xml.openSubSection("DataStorage");

            buildXmlData("", xml);

            xml.closeSubSection();
            return xml.ToString();
        }

        private void buildXmlData(string section, XmlStringBuilder xml)
        {
            var sections = new List<string>();
            sections.AddRange(Data.GetSubSections(section));

            foreach (string s in sections)
            {
                int depth = s.Substring(section.Length).Split('\\').Length - 1;
                if (depth == 1)
                {
                    xml.openSubSection(Data.GetSubsectionName(s));
                    buildXmlData(s, xml);
                    xml.closeSubSection();
                }
            }

            if (!String.IsNullOrEmpty(section))
            {
                foreach (var p in Data.GetValues(section))
                {
                    xml.addValue(p.Key, p.Value);
                }
            }
        }

        public override string ToString()
        {
            return buildXmlData();
        }
    }
}