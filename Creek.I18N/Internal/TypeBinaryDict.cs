using System;
using System.Collections.Generic;

namespace Creek.I18N.Internal
{
    internal class TypeBinaryDict : Dictionary<Type, Creek.I18N.Internal.Binary>
    {

        public void AddC(Type t, Creek.I18N.Internal.Binary b)
        {
            if(!ContainsKey(t))
                Add(t, b);
        }

        public void AddRange(TypeBinaryDict typeBinaryDict)
        {
            foreach (var t in typeBinaryDict)
            {
                AddC(t.Key, t.Value);
            }
        }
    }
}
