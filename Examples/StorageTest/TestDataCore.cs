using System;
using Creek.Data.Storage;
using System.IO;
using System.Diagnostics;
using Registry = Creek.Data.Storage.Registry;

namespace StorageTester
{
    
    public class TestDataCore
    {
        Data data;
        public void init()
        {
            //data = new DictionaryData();
            data = Data.CreateDataStorage();
        }

        public void TestRegistryRead()
        {
            var registry = new Registry();
            registry.SetData(data);
            registry.readData(Microsoft.Win32.Registry.CurrentUser, "", false, true);

            var iniFiles = new IniFiles();
            iniFiles.SetData(data);
            string testFile = @"C:\PDFCreator\Test.ini";
            if (File.Exists(testFile))
                File.Delete(testFile);

            iniFiles.WriteData(testFile);
            //Process.Start(testFile);
        }

        public void TestRegistryWrite()
        {
            IniFiles iniFiles = new IniFiles();
            string testFile = @"C:\PDFCreator\Test.ini";
            if (!File.Exists(testFile))
                throw new Exception("Ini file doesn't exist.");
            Process.Start(testFile);

            iniFiles.SetData(data);
            iniFiles.ReadData(testFile);

            data.RemoveSection("Printers");

            Registry registry = new Registry();
            registry.SetData(data);
            registry.writeData(Microsoft.Win32.Registry.CurrentUser, "", true);
        }

        
        public void TestDataSaveAndLoad()
        {
            IniFiles iniFiles = new IniFiles();

            int numentries = 0;

            string file = "test.ini";

            data.Clear();

            data.SetValue(@"test\abc", "test1");
            data.SetValue(@"test\abc\def", "test2");
            data.SetValue(@"test\abc", "test3");
            data.SetValue(@"xyz\sdfdsf", "test4");

            string olddata = data.ToString();

            numentries = data.GetSections().Length;

            iniFiles.SetData(data);
            iniFiles.WriteData(file);

            iniFiles.SetData(data);
            data.Clear();
            iniFiles.ReadData(file);

            string newdata = data.ToString();
     
            File.Delete(file);
        }

        public void TestBulkData()
        {
            int numSections = 500;
            int numValues = 150;

            string file = "test.ini";

            IniFiles iniFiles = new IniFiles();
            
            data.Clear();

            for (int i = 1; i < numSections; i++)
            {
                string section = "Section" + i;
                data.AddSection(section);
                
                for (int j = 1; j < numValues; j++)
                {
                    data.SetValue(section + @"\value" + j, "value" + j);
                }
            }

            int numentries = data.GetSections().Length;

            iniFiles.SetData(data);
            iniFiles.WriteData(file);

            iniFiles.SetData(data);
            data.Clear();
            iniFiles.ReadData(file);

            if (File.Exists(file))
                File.Delete(file);
        }
    }
}
