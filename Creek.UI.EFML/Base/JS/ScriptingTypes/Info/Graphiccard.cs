using System.Management;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes.Info
{
    public class GraphicCard
    {
        public string Name
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_VideoController");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;

                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["Name"].ToString();
            }
        }

        public string DeviceID
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_VideoController");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;
                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["DeviceID"].ToString();
            }
        }

        public string DriverVersion
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_VideoController");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;

                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["DriverVersion"].ToString();
            }
        }
    }
}