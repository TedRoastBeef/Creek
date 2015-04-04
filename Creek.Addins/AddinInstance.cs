namespace Creek.Extensibility.Addins
{
    using System.Drawing;
    using System.IO;
    using System.Reflection;

    public class AddinInstance
    {
        #region Fields

        private readonly object instance;

        #endregion

        #region Constructors and Destructors

        public AddinInstance(object instance)
        {
            this.instance = instance;
        }

        #endregion

        #region Methods

        internal Addin GetAddin()
        {
            Assembly ass = this.instance.GetType().Assembly;

            string manifest = "manifest.xml";

            object[] man = ass.GetCustomAttributes(typeof(ManifestAttribute), true);
            if (man.Length == 1)
            {
                if (man[0] != null)
                {
                    var m = man[0] as ManifestAttribute;
                    manifest = m.Name;
                }
            }
            Stream str = ass.GetManifestResourceStream(ass.GetName().Name + "." + manifest);

            if (str != null)
            {
                string r = new StreamReader(str).ReadToEnd();
                Addin ad = new ManifestReader().Read(ass, r);
                ad.Icon = ad.IconPath != ""
                    ? Image.FromStream(ass.GetManifestResourceStream(ass.GetName().Name + "." + ad.IconPath))
                    : null;

                return ad;
            }
            return null;
        }

        #endregion
    }
}