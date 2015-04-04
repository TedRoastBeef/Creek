using System.IO;

namespace Creek.Serialize
{
    public static class Extensions
    {
        public static byte[] ToBytes(this object o)
        {
            var f = new Serializer(true);
            var mem = new MemoryStream();
            f.Serialize(o, mem);
            return mem.ToArray();
        }
        public static T FromBytes<T>(this T target, byte[] buffer)
        {
            var f = new Serializer(true);
            var mem = new MemoryStream(buffer);
            return (T) f.Deserialize(mem);
        }
    }
}
