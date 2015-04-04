using System;
using System.Collections.Generic;
using Microsoft.Win32;

namespace Creek.Data.Storage
{
    public class Registry : IDataReader, IDataWriter
    {
        string keyRoot = @"Software\Creek\";

        public Data data = null;

        public void SetData(Data data)
        {
            this.data = data;
        }

        public void ReadData()
        {
            readData("", true);
        }
        public void ReadData(string path)
        {
            readData(path, true);
        }
        public void readData(string path, bool clear)
        {
            throw new Exception("Cannot call this function without registry key and path");
        }

        private bool RegKeyExists(RegistryKey rk, string path)
        {
            using (RegistryKey RK = rk.OpenSubKey(path))
            {
                if (RK == null)
                    return false;
                else
                    return true;
            }
        }
        private bool RegKeyDelete(RegistryKey rk, string path)
        {
            using (RegistryKey RK = rk.OpenSubKey(path))
            {
                if (RK == null)
                    return false;
                else
                {
                    rk.DeleteSubKeyTree(path);
                    return true;
                }
            }
        }

        public void readData(RegistryKey hk, string path, bool withRoot, bool clear)
        {
            string subKey = keyRoot + @"\" + path;
            if (!RegKeyExists(hk, subKey))
                throw new Exception("Registry key '" + subKey + "' doesn't exist");
            if (subKey.Substring(path.Length) == @"\")
                subKey = subKey.Substring(1, subKey.Length - 1);

            if (clear)
                data.Clear();
            if (withRoot)
            {
                string[] ss = subKey.Split('\\');

                if (ss.Length == 0)
                    ReadRegValues(hk, data, subKey, "");
                else
                {
                    string p = ss[0];
                    for (int i = 1; i < ss.Length - 1; i++)
                        p = p + @"\" + ss[i];
                    ReadRegValues(hk, data, ss[ss.Length - 1], p);
                }
            }
            else
            {
                RegistryKey rk = hk.OpenSubKey(subKey);
                string[] getSubKeyNames = rk.GetSubKeyNames();
                for (int i = 0; i < getSubKeyNames.Length; i++)
                    ReadRegValues(hk, data, getSubKeyNames[i], subKey);
            }
        }

        private void ReadRegValues(RegistryKey hk, Data data, string sectionName, string rootName)
        {
            RegistryKey rk;
            if (rootName.Length == 0)
                rk = hk.OpenSubKey(sectionName);
            else
                rk = hk.OpenSubKey(rootName + @"\" + sectionName);

            string[] getSubKeyNames = rk.GetSubKeyNames();
            for (int i = 0; i < getSubKeyNames.Length; i++)
                ReadRegValues(hk, data, sectionName + @"\" + getSubKeyNames[i], rootName);

            data.AddSection(sectionName);
            string[] getValueNames = rk.GetValueNames();
            for (int i = 0; i < getValueNames.Length; i++)
                data.SetValue(sectionName + @"\" + getValueNames[i], rk.GetValue(getValueNames[i]).ToString());
        }

        public void WriteData()
        {
            WriteData("");
        }

        public void WriteData(string path)
        {
            throw new Exception("Cannot call this function without registry key and path");
        }

        public void writeData(string path, bool sortSection)
        {
            throw new Exception("The parameter sortSection can't used. Please use function writeData()!");
        }

        public void writeData(RegistryKey hk, string path, bool clear)
        {
            if (!RegKeyExists(hk, keyRoot))
                throw new Exception("Registry key '" + keyRoot + "' doesn't exist");
            if (path.Substring(path.Length) == @"\")
                path = path.Substring(1, path.Length - 1);

            string keyPath = keyRoot + @"\" + path;

            if (clear)
                RegKeyDelete(hk, keyPath);

            string[] sections = data.GetSections();

            foreach (string section in sections)
            {
                string subKey = keyPath + @"\" + section;
                if (!RegKeyExists(hk, subKey))
                    hk.CreateSubKey(subKey);
                RegistryKey rk = hk.OpenSubKey(subKey, true);

                List<KeyValuePair<string, string>> values = data.GetValues(section);

                foreach (KeyValuePair<string, string> entry in values)
                    rk.SetValue(entry.Key, entry.Value);
            }
            hk.Flush();
            hk.Close();
        }
        

    }
}
