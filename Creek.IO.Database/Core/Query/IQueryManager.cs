using Creek.Database.Api.Query;
using Creek.Database.Meta;

namespace Creek.Database.Core.Query
{
    internal interface IQueryManager
    {
        int[] GetOrderByAttributeIds(ClassInfo classInfo, IInternalQuery query);

        /// <summary>
        ///   Returns a multi class query executor (polymorphic = true)
        /// </summary>
        IQueryExecutor GetQueryExecutor(IQuery query, IStorageEngine engine);
    }
}