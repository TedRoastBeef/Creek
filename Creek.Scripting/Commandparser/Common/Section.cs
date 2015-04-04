using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Creek.Scripting.Commandparser.Common
{
    [DebuggerDisplay("{Name}")]
    public class Section
    {
        public BlockHeaderParser Header { get; set; }
        public Dictionary<string, IPropertyObject> Variables { get; set; }
        public Dictionary<Function, Action> Functions { get; set; }

        public Section()
        {
            Variables = new Dictionary<string, IPropertyObject>();
            Functions = new Dictionary<Function, Action>();
            Header = new BlockHeaderParser();
        }

    }
}