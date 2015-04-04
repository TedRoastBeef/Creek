using System;

namespace Creek.Database.Btree
{
    public interface IKeyAndValue
    {
        IComparable GetKey();

        object GetValue();
    }
}