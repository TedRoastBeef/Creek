using System;

namespace Creek.Database.Btree
{
    internal interface IBTreeSingleValuePerKey : IBTree
    {
        object Search(IComparable key);
    }
}