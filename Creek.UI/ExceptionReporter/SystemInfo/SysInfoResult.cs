using System.Collections.Generic;
using System.Linq;

#pragma warning disable 1591

namespace Creek.UI.ExceptionReporter.SystemInfo
{
    /// <summary>
    /// SysInfoResult holds results from a (ultimately WMI) query into system information
    /// </summary>
    public class SysInfoResult
    {
        private readonly List<SysInfoResult> _childResults = new List<SysInfoResult>();
        private readonly string _name;
        private readonly List<string> _nodes = new List<string>();

        public SysInfoResult(string name)
        {
            _name = name;
        }

        public List<string> Nodes
        {
            get { return _nodes; }
        }

        public string Name
        {
            get { return _name; }
        }

        public List<SysInfoResult> ChildResults
        {
            get { return _childResults; }
        }

        public void AddNode(string node)
        {
            _nodes.Add(node);
        }

        public void AddChildren(IEnumerable<SysInfoResult> children)
        {
            ChildResults.AddRange(children);
        }

        private void Clear()
        {
            _nodes.Clear();
        }

        private void AddRange(IEnumerable<string> nodes)
        {
            _nodes.AddRange(nodes);
        }

        public SysInfoResult Filter(string[] filterStrings)
        {
            List<string> filteredNodes = (from node in ChildResults[0].Nodes
                                          from filter in filterStrings
                                          where node.Contains(filter + " = ")
                                          //TODO a little too primitive
                                          select node).ToList();

            ChildResults[0].Clear();
            ChildResults[0].AddRange(filteredNodes);
            return this;
        }
    }
}

#pragma warning restore 1591