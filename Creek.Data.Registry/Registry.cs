namespace Creek.Data.Registry
{
    /// <summary>
    /// Represents the entry point for the application. Use this class if you want
    /// to manage your custom settings with this registry implemenation.
    /// </summary>
    public class Registry
    {
        private Storage m_Storage;

        public Registry(RegistrySettings settings)
        {
            var file = settings.StorageFile;
            Init(file);
        }

        public IEntry Root { get; private set; }

        public void Save()
        {
            m_Storage.Write(Root);
        }

        public void Load()
        {
            Root = m_Storage.Read();
        }

        private void Init(string storageFile)
        {
            Root = new FolderEntry("Root");
            m_Storage = new Storage(storageFile);
        }
    }
}