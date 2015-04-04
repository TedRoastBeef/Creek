using System.Collections.Generic;

namespace Creek.Data.Registry
{
    /// <summary>
    /// Represents an entry of a registry. There are two types of entires : folder and value.
    /// A folder supports children but cannot store any content. If you want to store content then
    /// use the Value entry instead. 
    /// </summary>
    public interface IEntry
    {
        /// <summary>
        /// Gets the key of the entry.
        /// </summary>
        string Key { get; set; }
        /// <summary>
        /// Determine whether the entry supports children.
        /// </summary>
        bool IsFolder { get; }
        /// <summary>
        /// Gets the children.
        /// </summary>
        List<IEntry> Children { get; }
        /// <summary>
        /// Get a child
        /// </summary>
        /// <param name="key">the key of the child.</param>
        /// <returns>the child entry</returns>
        IEntry this[string key] { get; }
        /// <summary>
        /// Add a new folder to the entry
        /// </summary>
        /// <param name="key">the key of the new child.</param>
        /// <returns>the new folder.</returns>
        IEntry AddFolder(string key);
        /// <summary>
        /// Add a new value to the entry.
        /// </summary>
        /// <param name="key">the key of the new value.</param>
        /// <returns>the new value</returns>
        IEntry AddValue(string key);

        void Remove(string key);
        bool Contains(string key);

        void SetValue(object value, ValueFormat format);
        object GetValue();
        ValueFormat GetValueFormat();
    }

    public enum ValueFormat
    {
        Unknown,
        Int,
        Double,
        String,
        Long,
        Date
    }
}