namespace Creek.Extensibility.Addins
{
    using System;

    [AttributeUsage(AttributeTargets.Assembly)]
    public class ManifestAttribute : Attribute
    {
        #region Constructors and Destructors

        public ManifestAttribute(string name)
        {
            this.Name = name;
        }

        #endregion

        #region Public Properties

        public string Name { get; set; }

        #endregion
    }
}