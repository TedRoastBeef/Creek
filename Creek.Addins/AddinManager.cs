namespace Creek.Extensibility.Addins
{
    using System.Collections.Generic;

    public class AddinManager
    {
        #region Static Fields

        public static AddinRegistry Registry = new AddinRegistry();

        #endregion

        #region Public Methods and Operators

        public static IEnumerable<ExtensionNode> GetExtensionObjects(string path)
        {
            foreach (Addin r in Registry)
            {
                foreach (ExtensionNode en in r.ExtensionNodes)
                {
                    if (en.Path == path)
                    {
                        yield return en;
                    }
                }
            }
        }

        #endregion
    }
}