using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Creek.Data.Storage
{
    public class IniFiles : IDataReader, IDataWriter
    {
        public Data Data = null;

        #region IDataReader Members

        public void SetData(Data data)
        {
            Data = data;
        }

        public void ReadData()
        {
            throw new Exception("Cannot call this function without path");
        }

        public void ReadData(string path)
        {
            readData(path, true);
        }

        #endregion

        #region IDataWriter Members

        public void WriteData()
        {
            throw new Exception("This may not be called without path");
        }

        public void WriteData(string path)
        {
            var sb = new StringBuilder();

            var sections = new List<string>();
            sections.AddRange(Data.GetSections());

            foreach (string section in sections)
            {
                List<KeyValuePair<string, string>> values = Data.GetValues(section);

                sb.AppendLine('[' + section.Substring(0, section.Length - 1) + ']');

                foreach (var entry in values)
                {
                    if (entry.Key.Equals(section))
                        continue;
                    sb.Append(entry.Key);
                    sb.Append('=');
                    sb.Append(entry.Value);
                    sb.AppendLine();
                }

                sb.AppendLine();
            }
            var f = new FileInfo(path);
            if (!Directory.Exists(f.DirectoryName))
            {
                Directory.CreateDirectory(f.DirectoryName);
            }

            WriteFile(path, sb.ToString());
        }

        #endregion

        public void readData(Stream stream, bool clear)
        {
            string section = "";

            string strLine;
            string[] values;
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(stream, true);
                while (!sr.EndOfStream)
                {
                    strLine = sr.ReadLine();
                    if ((strLine.StartsWith("[")) && (strLine.IndexOf(']') >= 0))
                    {
                        section = strLine.Substring(1, strLine.IndexOf(']') - 1);
                        Data.AddSection(section);
                    }

                    if ((section != "") && (strLine.IndexOf('=') >= 0))
                    {
                        values = strLine.Split('=');
                        Data.SetValue(section + @"\" + values[0].Trim(), values[1].Trim());
                    }
                }
            }
            finally
            {
                // close and free memory
                sr.Close();
                sr.Dispose();
            }
        }

        public void readData(string path, bool clear)
        {
            FileStream fstream = null;

            if (clear)
                Data.Clear();

            fstream = new FileStream(path, FileMode.Open, FileAccess.Read);

            try
            {
                readData(fstream, clear);
            }
            finally
            {
                if (fstream != null)
                {
                    // close and free memory
                    fstream.Dispose();
                }
            }
        }

        /// <summary>
        /// Writes a string into a given file
        /// </summary>
        /// <param name="filename">Full path and filename</param>
        /// <param name="data">Data to write</param>
        /// <returns>true upon success</returns>
        public static bool WriteFile(string filename, string data)
        {
            FileStream fstream = null;
            StreamWriter sw = null;
            try
            {
                //open file
                fstream = new FileStream(filename, FileMode.Create);
                sw = new StreamWriter(fstream, Encoding.UTF8);

                //write data
                sw.WriteLine(data);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return false;
            }
            finally
            {
                if (sw != null)
                {
                    // close and free memory
                    sw.Close();
                    sw.Dispose();
                    fstream.Dispose();
                }
            }
            return true;
        }
    }
}