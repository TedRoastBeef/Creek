using System;
using System.Reflection;
using Creek.UI.ExceptionReporter.Core;

namespace Creek.UI.ExceptionReporter
{
    /// <summary>
    /// ViewFactory inspects the assembly and retrieves the appropriate class
    /// </summary>
    internal static class ViewFactory
    {
        public static T Create<T>(ViewResolver viewResolver, ExceptionReportInfo reportInfo) where T : class
        {
            Type view = viewResolver.Resolve<T>();

            ConstructorInfo constructor = view.GetConstructor(new[] {typeof (ExceptionReportInfo)});
            object newInstance = constructor.Invoke(new object[] {reportInfo});
            return newInstance as T;
        }

        public static T Create<T>(ViewResolver viewResolver) where T : class
        {
            return Activator.CreateInstance(viewResolver.Resolve<T>()) as T;
        }
    }
}