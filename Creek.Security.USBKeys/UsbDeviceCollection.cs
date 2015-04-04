using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Creek.Security.USBKeys
{
    public class UsbDeviceCollection : IEnumerable<DriveInfo>
    {
        private List<DriveInfo> inner = new List<DriveInfo>();

        public DriveInfo this[int index] {get { return inner[index]; }}

        public UsbDeviceCollection()
        {
            inner = new List<DriveInfo>(DriveInfo.GetDrives().Where(drive => drive.IsReady && drive.DriveType == DriveType.Removable));
        }

        #region Implementation of IEnumerable

        public IEnumerator<DriveInfo> GetEnumerator()
        {
            return inner.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}
