using System;

namespace Creek.IO.Binary
{
    public struct Package
    {
        public string Tag;
        public DateTime CreationTime;
        public byte[] RawData;
    }
}
