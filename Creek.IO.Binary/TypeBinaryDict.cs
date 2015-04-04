using System;
using System.Collections.Generic;

namespace Creek.IO.Binary
{
    public class TypeBinaryDict : List<IBinary>
    {

        public void AddC(IBinary b)
        {
            if(!Contains(b))
                Add(b);
        }

        public void AddRange(TypeBinaryDict typeBinaryDict)
        {
            foreach (var t in typeBinaryDict)
            {
                AddC(t);
            }
        }
    }
}
