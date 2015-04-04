using System.Collections.Generic;

namespace Creek.Data.Registry
{
    internal class FolderEntry : IEntry
    {
        private readonly List<IEntry> m_Entries;
        private string m_Key;

        public FolderEntry(string key)
        {
            m_Entries = new List<IEntry>();
            m_Key = key;
        }

        #region IEntry Members

        public string Key
        {
            get { return m_Key; }
            set { m_Key = value; }
        }

        public bool IsFolder
        {
            get { return true; }
        }

        public List<IEntry> Children
        {
            get { return m_Entries; }
        }

        public void SetValue(object value, ValueFormat format)
        {
            throw new RegistryException("Folder supports not SetValue()");
        }

        public object GetValue()
        {
            throw new RegistryException("Folder supports not GetValue()");
        }

        public ValueFormat GetValueFormat()
        {
            throw new RegistryException("Folder supports not GetValue()");
        }

        public bool Contains(string key)
        {
            var entry = GetItem(key);
            return entry != null;
        }

        public IEntry AddFolder(string key)
        {
            var containsKey = Contains(key);
            if (containsKey)
            {
                throw new RegistryException("Key already exists.");
            }

            var entry = new FolderEntry(key);
            m_Entries.Add(entry);
            return entry;
        }

        public IEntry AddValue(string key)
        {
            var containsKey = Contains(key);
            if (containsKey)
            {
                throw new RegistryException("Key already exists.");
            }

            var entry = new ValueEntry(key);
            m_Entries.Add(entry);

            return entry;
        }

        public void Remove(string key)
        {
            var containsKey = Contains(key);
            if (!containsKey)
            {
                throw new RegistryException("Key does not exists");
            }

            var entry = GetItem(key);
            m_Entries.Remove(entry);
        }

        public T GetValue<T>()
        {
            throw new RegistryException("Folder supports not GetValue()");
        }

        public IEntry this[string key]
        {
            get
            {
                var entry = GetItem(key);
                if (entry == null)
                {
                    throw new RegistryException("Key not found");
                }

                return entry;
            }
        }

        #endregion

        private IEntry GetItem(string key)
        {
            foreach (var entry in m_Entries)
            {
                if (entry.Key.Equals(key))
                {
                    return entry;
                }
            }

            return null;
        }
    }
}