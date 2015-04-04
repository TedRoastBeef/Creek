using Creek.Data.Storage;

namespace StorageTester {
    public class TestXmlFiles
    {
        private Data data;
        private string testFile = "test.xml";

        public void init()
        {
            data = Data.CreateDataStorage();
        }

        public void TestWriteReadCycle()
        {
            data.SetValue(@"test\abc\v1", "v1");
            data.SetValue(@"test\v2", "v2");
            data.SetValue(@"test\zxy\v3", "3");
            
            XmlFiles xml = new XmlFiles();
            xml.SetData(data);
            xml.WriteData(testFile);
            string xmlString = xml.ToString();

            xml.ReadData(testFile, true);
        }
    }
}
