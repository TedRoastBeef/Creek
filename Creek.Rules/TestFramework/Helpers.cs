using System.Reflection;
using Creek.Rules.Runtime;

namespace Creek.Rules.TestFramework
{
    static class Helpers
    {
        public static bool WasGiven(this DotNetRulesContext pattern)
        {
            var field = pattern.GetType().GetField("_wasGiven", BindingFlags.NonPublic | BindingFlags.Instance);
            return field != null && (bool)field.GetValue(pattern);
        }
    }
}
