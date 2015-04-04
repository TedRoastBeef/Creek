using System.Management;

namespace Creek.UI.EFML.Base.JS.ScriptingTypes.Info
{
    public class Hardware
    {
        #region Nested type: Keyboard

        public class Keyboard
        {
            public string KeyboardName
            {
                get
                {
                    var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                                "SELECT * FROM Win32_Keyboard");
                    ManagementObjectCollection.ManagementObjectEnumerator enumerator;

                    enumerator = managementObjectSearcher.Get().GetEnumerator();
                    enumerator.MoveNext();

                    return enumerator.Current["Name"].ToString();
                }
            }
        }

        #endregion

        #region Nested type: Monitor

        public class Monitor
        {
            public string MonitorName
            {
                get
                {
                    var managementObjectSearcher = new ManagementObjectSearcher("root\\CIMV2",
                                                                                "SELECT * FROM Win32_DesktopMonitor");
                    ManagementObjectCollection.ManagementObjectEnumerator enumerator;
                    enumerator = managementObjectSearcher.Get().GetEnumerator();
                    enumerator.MoveNext();

                    return enumerator.Current["Name"].ToString();
                }
            }
        }

        #endregion
    }
}