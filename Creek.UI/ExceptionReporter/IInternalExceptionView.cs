using System;

namespace Creek.UI.ExceptionReporter
{
    /// <summary>
    /// An interface that represents the presentation of an internal exception
    /// </summary>
    public interface IInternalExceptionView
    {
        ///<summary> Show the internal exception</summary>
        void ShowException(string message, Exception exception);
    }
}