using System.Collections.Generic;
using System.IO;

namespace Creek.Resources
{
    public class ResourceWriter
    {
        private BinaryReader br;
        private Dictionary<string, object> objects = new Dictionary<string, object>();

        public ResourceWriter(Stream s)
        {
            br = new BinaryReader(s);
        }
    }
}