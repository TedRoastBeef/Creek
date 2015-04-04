using System;
using System.Collections;
using System.Collections.Generic;

namespace Creek.Tools
{
    public class Buffer<T> : IEnumerable<T>, IDisposable
    {
        private T[] buffer;

        public Buffer(int capacity)
        {
            buffer = new T[capacity];
        }

        public Buffer()
        {
            buffer = new List<T>().ToArray();
        }

        #region Implementation of IDisposable

        /// <summary>
        /// Führt anwendungsspezifische Aufgaben durch, die mit der Freigabe, der Zurückgabe oder dem Zurücksetzen von nicht verwalteten Ressourcen zusammenhängen.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            buffer = null;
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return buffer.GetEnumerator();
        }

        #endregion

        public T this[int i]
        {
            get { return buffer[i]; }
            set { buffer[i] = value; }
        }
    }
}