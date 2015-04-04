using System;
using System.IO;

namespace Creek.Data.Registry
{
    /// <summary>
    /// Configures the registry behaviour.
    /// </summary>
    public class RegistrySettings
    {
        /// <summary>
        /// Creates a new instance of RegistrySettings.
        /// </summary>
        public RegistrySettings()
        {
            var filename = "registry.xml";
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            StorageFile = Path.Combine(folder, filename);
        }

        /// <summary>
        /// Creates a new instance of RegistrySettings.
        /// </summary>
        /// <param name="filename">the complete path of the storage file.</param>
        public RegistrySettings(string filename)
        {
            StorageFile = filename;
        }

        /// <summary>
        /// Gets/sets the storage file.
        /// </summary>
        public string StorageFile { get; set; }
    }
}
