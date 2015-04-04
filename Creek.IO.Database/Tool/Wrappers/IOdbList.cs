using System.Collections.Generic;

namespace Creek.Database.Tool.Wrappers
{
    public interface IOdbList<TItem> : IList<TItem>
    {
        void AddAll(IEnumerable<TItem> collection);
        void RemoveAll(IEnumerable<TItem> collection);
    }
}