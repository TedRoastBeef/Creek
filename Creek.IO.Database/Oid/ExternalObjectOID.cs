using Creek.Database.Api;

namespace Creek.Database.Oid
{
    internal sealed class ExternalObjectOID : ObjectOID, IExternalOID
    {
        private readonly IDatabaseId _databaseId;

        public ExternalObjectOID(OID oid, IDatabaseId databaseId) : base(oid.ObjectId)
        {
            _databaseId = databaseId;
        }

        #region IExternalOID Members

        public IDatabaseId GetDatabaseId()
        {
            return _databaseId;
        }

        #endregion
    }
}
