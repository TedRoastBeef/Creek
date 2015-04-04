using Creek.Database.Tool.Wrappers;

namespace Creek.Database.Tool
{
    internal static class UniqueIdGenerator
    {
        internal static long GetRandomLongId()
        {
            lock (typeof (UniqueIdGenerator))
            {
                return (long) (OdbRandom.GetRandomDouble() * long.MaxValue);
            }
        }
    }
}
