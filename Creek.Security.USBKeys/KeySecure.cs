using System;
using System.IO;
using System.Linq;

namespace Creek.Security.USBKeys
{
    public class KeySecure
    {
        public static void Lock(string filename, string pw)
        {
            var devices = new UsbDeviceCollection();
            File.WriteAllText(devices[0].RootDirectory + filename, Key.From(pw));
            new FileInfo(devices[0].RootDirectory + filename) {Attributes = FileAttributes.Hidden};
        }

        public enum KeyEvent
        {
            inserted, removed
        }
        public static void Event(KeyEvent ke, Action<string> cb)
        {
            var detector = new DriveDetector();
            switch (ke)
            {
                    case KeyEvent.inserted:
                        detector.DeviceArrived += (sender, args) => cb(args.Drive);
                    break;
                case KeyEvent.removed:
                        detector.DeviceRemoved += (sender, args) => cb(args.Drive);
                    break;
            }
        }

        public static Key Release(string filename)
        {
            var r = new Key();

            // continue
            return r;
        }
        public static bool HasLock(string filename)
        {
            var devices = new UsbDeviceCollection();
            return devices.Select(device => Directory.GetFiles(device.RootDirectory.ToString()).ToList().Contains(filename)).FirstOrDefault();
        }
        public static string GetLock(string filename)
        {
            return "";
        }
    }
}
