using System.Management;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes.Info
{
    public class DiskDrive
    {
        public string Size
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_DiskDrive");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;
                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["Size"].ToString();
            }
        }

        public string Name
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_DiskDrive");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;

                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["Name"].ToString();
            }
        }

        public string Partitions
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_DiskDrive");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;

                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["Partitions"].ToString();
            }
        }

        public string Description
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_DiskDrive");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;

                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["Description"].ToString();
            }
        }

        public string Manufacturer
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_DiskDrive");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;

                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["Manufacturer"].ToString();
            }
        }

        public string SerialNumber
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_DiskDrive");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;

                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["SerialNumber"].ToString();
            }
        }

        public string Signature
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_DiskDrive");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;

                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["Signature"].ToString();
            }
        }
    }
}