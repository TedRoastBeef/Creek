using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace Creek.Diagnostics
{
    public class Blackbox : IDisposable, ICloneable
    {
        #region BlackBoxType enum

        public enum BlackBoxType
        {
            Binary,
            Xml
        }

        #endregion

        private Dictionary<Date, KeyValuePair<string, string>> data =
            new Dictionary<Date, KeyValuePair<string, string>>();

        public string this[Date date, string key]
        {
            get
            {
                foreach (var v in data.Where(v => v.Key == date).Where(v => v.Value.Key == key))
                {
                    return v.Value.Value;
                }
                return "";
            }
        }

        public void AddMessage(Date date, string key, string value)
        {
            data.Add(date, new KeyValuePair<string, string>(key, value));
        }

        public void Save(BlackBoxType bbt, out byte[] buffer)
        {
            var ms = new MemoryStream();
            if (bbt == BlackBoxType.Binary)
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, data);
                buffer = ms.ToArray();
            }
            buffer = null;
        }

        public void Load(BlackBoxType bbt, byte[] buffer)
        {
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            data = null;
        }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            return this;
        }

        #endregion
    }
}