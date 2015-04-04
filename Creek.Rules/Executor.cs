using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Creek.Rules.Runtime;

namespace Creek.Rules
{
    public class Settings
    {
        public bool CatchExceptions { get; set; }
    }

    public static class Executor
    {
        public static readonly Settings Settings = new Settings {CatchExceptions = false};

        private static volatile Dictionary<string, List<object>> _types = new Dictionary<string, List<object>>();
        private static readonly object SyncRoot = new object();
        public static List<ExceptionInformation> Exceptions { get; private set; }

        [Experimental]
        public static void RegisterObject<T>(T type)
        {
            string fullName = type.GetType().FullName;
            if (string.IsNullOrEmpty(fullName))
                throw new Exception("Only types with full names can be registered");
            lock (SyncRoot)
            {
                if (!_types.ContainsKey(fullName))
                {
                    _types.Add(fullName, new List<object>());
                }
                _types[fullName].Add(type);
            }
        }

        [Experimental]
        public static IEnumerable<T> Resolve<T>()
        {
            string fullName = typeof (T).FullName;
            if (string.IsNullOrEmpty(fullName))
                throw new Exception("Only types with full names can be resolved");
            return _types.ContainsKey(fullName) ? _types[fullName].Select(_ => (T) _) : new List<T>();
        }

        [Experimental]
        public static void ExecuteOn<T>(Action<T> action)
        {
            Resolve<T>().ToList().ForEach(action);
        }

        [Experimental]
        public static void NotifyPolicies<TSubject>(this TSubject subject, Assembly policyLocation = null,
                                                    IEnumerable<Type> policies = null)
        {
            new Task(() => Apply(subject, policyLocation, policies)).Start();
        }

        public static ExecutionTrace ApplyPolicies<TSubject>(this TSubject subject, Assembly policyLocation = null,
                                                             IEnumerable<Type> policies = null)
        {
            return Apply(subject, policyLocation, policies);
        }

        public static ExecutionTrace ApplyPoliciesFor<TSubject, TSource>(this TSubject subject, TSource source,
                                                                         Assembly policyLocation = null,
                                                                         IEnumerable<Type> policies = null)
        {
            return (ExecutionTrace) Apply<object, TSubject, TSource>(subject, source, policyLocation, policies);
        }

        public static ExecutionTrace Apply<TSubject>(TSubject subject, Assembly policyLocation = null,
                                                     IEnumerable<Type> policies = null)
        {
            return (ExecutionTrace) Apply<object, TSubject>(subject, policyLocation, policies);
        }

        public static ExecutionTrace<TReturn> ApplyPolicies<TReturn, TSubject>(this TSubject subject,
                                                                               Assembly policyLocation = null,
                                                                               IEnumerable<Type> policies = null)
        {
            return Apply<TReturn, TSubject>(subject, policyLocation, policies);
        }

        public static ExecutionTrace<TReturn> ApplyPoliciesFor<TReturn, TSubject, TSource>(this TSubject subject,
                                                                                           TSource source,
                                                                                           Assembly policyLocation =
                                                                                               null,
                                                                                           IEnumerable<Type> policies =
                                                                                               null)
        {
            return Apply<TReturn, TSubject, TSource>(subject, source, policyLocation, policies);
        }

        public static ExecutionTrace<TReturn> Apply<TReturn, TSubject>(TSubject subject, Assembly policyLocation = null,
                                                                       IEnumerable<Type> policies = null)
        {
            if (policies == null)
            {
                policies = Enumerable.Empty<Type>();
            }

            Type type = (typeof (TSubject));
            IEnumerable<Assembly> policyLocations = RetrievePolicyLocations(ref policyLocation, policies, type);

            Exceptions = new List<ExceptionInformation>();

            var executionTrace = new ExecutionTrace<TReturn>(policyLocation);

            foreach (
                DotNetRulesContext mon in
                    policyLocations.SelectMany(location => location.GetTypesWithPolicyAttribute(policies.Any(), type)
                                                               .Select(item => new DotNetRulesContext(item))
                                                               .Where(
                                                                   _ =>
                                                                   (!policies.Any()) ||
                                                                   policies.Any(type1 => type1 == _.CurrentPolicy))))
            {
                mon.Establish(subject);
                if (mon.Given() || mon.Or())
                {
                    executionTrace.Called++;
                    executionTrace.By.Enqueue(mon.CurrentPolicy);

                    MonitorThen(mon);
                }
                mon.Finally();
                executionTrace.ReturnType = mon.Return<TReturn>();
            }

            return executionTrace;
        }

        public static ExecutionTrace Apply<TSource, TTarget>(TSource source, TTarget target,
                                                             Assembly policyLocation = null,
                                                             IEnumerable<Type> policies = null)
        {
            return (ExecutionTrace) Apply<object, TSource, TTarget>(source, target, policyLocation, policies);
        }

        public static ExecutionTrace<TReturn> Apply<TReturn, TSource, TTarget>(TSource source, TTarget target,
                                                                               Assembly policyLocation = null,
                                                                               IEnumerable<Type> policies = null)
        {
            if (policies == null)
            {
                policies = Enumerable.Empty<Type>();
            }
            Type type = (typeof (TSource));
            IEnumerable<Assembly> policyLocations = RetrievePolicyLocations(ref policyLocation, policies, type);
            Exceptions = new List<ExceptionInformation>();

            var executionTrace = new ExecutionTrace<TReturn>(policyLocation);

            foreach (
                DotNetRulesContext mon in
                    policyLocations.SelectMany(
                        location => location.GetTypesWithPolicyAttribute(policies.Any(), type, typeof (TTarget))
                                        .Select(item => new DotNetRulesContext(item))
                                        .Where(_ => !policies.Any() || policies.Any(type1 => type1 == _.CurrentPolicy)))
                )
            {
                mon.Establish(source, target);
                if (mon.Given() || mon.Or())
                {
                    executionTrace.Called++;
                    executionTrace.By.Enqueue(mon.CurrentPolicy);

                    MonitorThen(mon);
                }
                mon.Finally();
                executionTrace.ReturnType = mon.Return<TReturn>();
            }
            return executionTrace;
        }

        public static ExecutionTrace Apply(params dynamic[] values)
        {
            return Apply(null, null, values);
        }

        public static ExecutionTrace Apply(IEnumerable<Type> policies, params dynamic[] values)
        {
            return Apply(null, policies, values);
        }

        public static ExecutionTrace Apply(Assembly policyLocation, params dynamic[] values)
        {
            return Apply(null, null, values);
        }

        public static ExecutionTrace Apply(Assembly policyLocation, IEnumerable<Type> policies, params dynamic[] values)
        {
            return (ExecutionTrace) Apply<object>(policyLocation, policies, values);
        }

        public static ExecutionTrace<TReturn> Apply<TReturn>(params dynamic[] values)
        {
            return Apply<TReturn>(null, null, values);
        }

        public static ExecutionTrace<TReturn> Apply<TReturn>(IEnumerable<Type> policies, params dynamic[] values)
        {
            return Apply<TReturn>(null, policies, values);
        }

        public static ExecutionTrace<TReturn> Apply<TReturn>(Assembly policyLocation, params dynamic[] values)
        {
            return Apply<TReturn>(null, null, values);
        }

        public static ExecutionTrace<TReturn> Apply<TReturn>(Assembly policyLocation, IEnumerable<Type> policies,
                                                             params dynamic[] values)
        {
            if (policies == null)
            {
                policies = Enumerable.Empty<Type>();
            }
            Type[] types = values.Select(_ => (Type) _.GetType()).ToArray();
            if (policyLocation == null)
            {
                policyLocation = types.First().Assembly;
            }
            Exceptions = new List<ExceptionInformation>();

            var target = new ExecutionTrace<TReturn>(policyLocation);
            foreach (
                DotNetRulesContext mon in policyLocation.GetTypesWithPolicyAttribute(policies.Any(), types.ToArray())
                    .Select(item => new DotNetRulesContext(item))
                    .Where(_ => !policies.Any() || policies.Any(type1 => type1 == _.CurrentPolicy)))
            {
                mon.EstablishMore(values);
                if (mon.Given() || mon.Or())
                {
                    target.Called++;
                    target.By.Enqueue(mon.CurrentPolicy);

                    MonitorThen(mon);
                }
                mon.Finally();
                for (int i = 0; i < values.Count(); i++)
                {
                    dynamic value = values[i];
                    target.ReturnType = mon.Return<TReturn>();
                }
            }
            return target;
        }

        private static void MonitorThen(DotNetRulesContext mon)
        {
            Exceptions.AddRange(mon.Then(Settings.CatchExceptions));
        }

        private static IEnumerable<Assembly> RetrievePolicyLocations(ref Assembly policyLocation,
                                                                     IEnumerable<Type> policies, Type type)
        {
            var policyLocations = new List<Assembly>();
            if (policyLocation == null)
            {
                policyLocations = policies.Any()
                                      ? policies.Select(_ => _.Assembly).Distinct().ToList()
                                      : AppDomain.CurrentDomain.GetAssemblies().ToList();
                policyLocation = type.Assembly;
            }
            else
            {
                policyLocations.Add(policyLocation);
            }
            return policyLocations;
        }
    }
}