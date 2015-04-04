using Creek.Database.Api;
using Creek.Database.Meta;

namespace Creek.Database.Cache
{
    /// <summary>
    ///   An interface for temporary cache
    /// </summary>
    public interface IReadObjectsCache
    {
        NonNativeObjectInfo GetObjectInfoByOid(OID oid);

        bool IsReadingObjectInfoWithOid(OID oid);

        void StartReadingObjectInfoWithOid(OID oid, NonNativeObjectInfo objectInfo);

        void ClearObjectInfos();
    }
}
