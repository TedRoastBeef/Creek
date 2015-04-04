using System.Management;
using Microsoft.Win32;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes.Info
{
    public class CPU
    {
        public string Temperature
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\WMI",
                                                                            "SELECT * FROM MSAcpi_ThermalZoneTemperature");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator =
                    managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return double.Parse(enumerator.Current["CurrentTemperature"].ToString()) - 2732.0/10.0 + "° C";
            }
        }

        public string Producer
        {
            get
            {
                return
                    Registry.GetValue("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\SYSTEM\\CentralProcessor\\0",
                                      "VendorIdentifier", null).ToString();
            }
        }

        public string Identifier
        {
            get
            {
                return
                    Registry.GetValue("HKEY_LOCAL_MACHINE\\HARDWARE\\DESCRIPTION\\SYSTEM\\CentralProcessor\\0",
                                      "Identifier", null).ToString();
            }
        }

        public string CurrentClockSpeed
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_Processor");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator =
                    managementObjectSearcher.Get().GetEnumerator();

                enumerator.MoveNext();

                return enumerator.Current["CurrentClockSpeed"].ToString();
            }
        }

        public string MaxClockSpeed
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_Processor");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator =
                    managementObjectSearcher.Get().GetEnumerator();

                enumerator.MoveNext();

                return enumerator.Current["CurrentClockSpeed"].ToString();
            }
        }

        public string Name
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_Processor");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator =
                    managementObjectSearcher.Get().GetEnumerator();

                enumerator.MoveNext();

                return enumerator.Current["Name"].ToString();
            }
        }

        public string Typ
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_Processor");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;
                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["ProcessorType"].ToString();
            }
        }

        public string Version
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_Processor");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;
                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["Version"].ToString();
            }
        }

        public string Manufacturer
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_Processor");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;
                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["Manufacturer"].ToString();
            }
        }

        public string Level
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_Processor");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;
                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["Level"].ToString();
            }
        }

        public string CpuStatus
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_Processor");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;
                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["CpuStatus"].ToString();
            }
        }

        public string ProcessorId
        {
            get
            {
                var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                            "SELECT * FROM Win32_Processor");
                ManagementObjectCollection.ManagementObjectEnumerator enumerator;

                enumerator = managementObjectSearcher.Get().GetEnumerator();
                enumerator.MoveNext();

                return enumerator.Current["ProcessorId"].ToString();
            }
        }
    }
}