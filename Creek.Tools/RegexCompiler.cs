namespace Creek.Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.RegularExpressions;

    public class RegexCompiler
    {
        private readonly Dictionary<string, string> patterns = new Dictionary<string, string>(); 

        public void Add(string name, string pattern)
        {
            this.patterns.Add(name, pattern);
        }

        public Assembly Compile(string assemblyname)
        {
            var compilationList = this.patterns.Select(pattern => new RegexCompilationInfo(pattern.Value, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant, pattern.Key, assemblyname + ".RegularExpressions", true)).ToList();
            // Generate assembly with compiled regular expressions
            var compilationArray = new RegexCompilationInfo[compilationList.Count];
            var assemName = new AssemblyName(assemblyname + ", Version=1.0.0.1001, Culture=neutral, PublicKeyToken=null");
            compilationList.CopyTo(compilationArray);
            Regex.CompileToAssembly(compilationArray, assemName);
            return Assembly.Load(assemName);
        }

    }
}
