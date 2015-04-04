using System;
using System.Collections;

namespace Creek.Database.Btree
{
    internal interface IBTreeMultipleValuesPerKey : IBTree
    {
        IList Search(IComparable key);
    }
}