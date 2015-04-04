using Creek.Database.Cache;
using Creek.Database.Meta;

namespace Creek.Database.Core
{
    public interface IInstanceBuilder
    {
        object BuildOneInstance(NonNativeObjectInfo objectInfo, IOdbCache cache);

        object BuildOneInstance(NonNativeObjectInfo objectInfo);
    }
}