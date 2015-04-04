using System;

namespace StorageTester
{
    class Program
    {
        static void Main()
        {
            var tdc = new TestDataCore();
            tdc.init();
            tdc.TestBulkData();
            tdc.TestDataSaveAndLoad();
            tdc.TestRegistryWrite();
            tdc.TestRegistryRead();

            var test = new TestXmlFiles();
            test.init();
            test.TestWriteReadCycle();
            
            Console.ReadKey();
        }
    }
}
