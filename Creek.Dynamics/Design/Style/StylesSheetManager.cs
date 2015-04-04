//*****************************************************************************
//	Dony by : Sylvain BLANCHARD
//	Date : 08/01/2006
//*****************************************************************************

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Creek.Dynamics.Design.Style.Designtime;
using Creek.Dynamics.Design.Style.Filemanagement;

namespace Creek.Dynamics.Design.Style
{
    /// <summary>
    /// Component which permits to manage a styles sheet for the controls of the Windows forms
    /// as the CSS file do it for the ASP .NET pages.
    /// 
    /// How to use this component : 
    /// 
    /// To use this component, do the following :
    /// 1) Drag and drop this component on a Windows forms => a 'Style' property is
    /// automatically added to all the controls of the form.
    /// 2) Set the 'Style' property of each control of the form you want to style with a style name
    /// contained in the styles sheet file (in the designer).
    /// 3) Create a 'StylesSheetFileName' key on the 'appSettings' section of the 'app.config' file
    /// and set it the absolute path of the styles sheet file
    /// 4) That's all so run the application and enjoy it.
    /// 
    /// Notes : 
    /// 
    /// The conversion of the string defined in the styles sheet file in the correct format is done 
    /// with the 'TypeConverter.ConvertFromString()' method so you must respect the format
    /// of the TypeConverter class associated with the property :
    /// * Font => FontConverter
    /// * BackColor => ColorConverter
    /// * ...
    /// For more explanations about TypeConverter, see MSDN site.
    /// </summary>
    [ProvideProperty("Style", typeof (Control))]
    public class StylesSheetManager : Component, IExtenderProvider, ISupportInitialize
    {
        #region Member variables

        //*********************************************************************
        //
        // Member variables
        //
        //*********************************************************************
        private readonly StylesSheetFileManager manager;
        private readonly Hashtable propertieValues;
        private Container components;

        #endregion

        #region Constructor / Destructor

        //*********************************************************************
        //
        // Constructor / Destructor
        //
        //*********************************************************************
        /// <summary>
        /// Initializes a new instance of the <see cref="T:StylesSheetManager"/> class.
        /// </summary>
        public StylesSheetManager()
        {
            InitializeComponent();
            InitializeThreadCulture();

            propertieValues = new Hashtable();
            manager = new StylesSheetFileManager();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:StylesSheetManager"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public StylesSheetManager(IContainer container) : this()
        {
            container.Add(this);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
        }

        /// <summary>
        /// Initializes the thread culture.
        /// </summary>
        private void InitializeThreadCulture()
        {
            // We force the culture to be independent of the regional settings when reading
            // the styles sheet file :
            // => list separator = "," 
            //    decimal separtor = "."
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-EN");
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"></see> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

        #region IExtenderProvider implementation

        //*********************************************************************
        //
        // IExtenderProvider implementation
        //
        //*********************************************************************
        bool IExtenderProvider.CanExtend(object o)
        {
            // Add the Style property for all the 'control' of the form
            if (o is Control)
                return true;
            else
                return false;
        }

        #endregion

        #region ISupportInitialize implementation

        //*********************************************************************
        //
        // ISupportInitialize implementation
        //
        //*********************************************************************
        void ISupportInitialize.BeginInit()
        {
        }

        void ISupportInitialize.EndInit()
        {
            if (DesignMode)
                ApplyStyles();
        }

        #endregion

        #region Styles management

        //*********************************************************************
        //
        // Styles management
        //
        //*********************************************************************
        /// <summary>
        /// Gets or sets the styles sheet filename.
        /// 
        /// The path must be an absolute path.
        /// </summary>
        /// <value>The styles sheet filename.</value>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string StylesSheetFilename
        {
            get
            {
                string filename = null;
                if (DesignMode)
                    filename = GetAppSettingsValue("StylesSheetFilename");
                else
                    filename = ConfigurationManager.AppSettings["StylesSheetFilename"];

                if (String.IsNullOrEmpty(filename))
                    throw new StylesSheetException(
                        StylesSheetException.ExceptionType.StylesSheetFileNameNotDefinedInAppConfig);
                return filename;
            }
        }

        /// <summary>
        /// Gets the style.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <returns></returns>
        [Category("Appearance")]
        [Description("Name of the style")]
        public string GetStyle(Control c)
        {
            if (!propertieValues.ContainsKey(c))
                propertieValues[c] = "";

            return (string) propertieValues[c];
        }

        /// <summary>
        /// Sets the style.
        /// </summary>
        /// <param name="c">The c.</param>
        /// <param name="value">The value.</param>
        public void SetStyle(Control c, string value)
        {
            propertieValues[c] = value;
        }

        /// <summary>
        /// Removes the style defined for this control.
        /// </summary>
        /// <param name="c">The c.</param>
        public void RemoveStyle(Control c)
        {
            if (propertieValues.ContainsKey(c))
                propertieValues.Remove(c);
        }

        /// <summary>
        /// Apply the styles to all the controls of the form which have her 'style' property
        /// defined.
        /// 
        /// This method is defined as public to be able to call it manually.
        /// * set the path of the styles sheet file via 'StylesSheetFileName' key of the app.config.
        /// * for each control, set the name of the style with the 'SetStyle' property.
        /// * apply the styles in calling the 'ApplyStyles' method
        /// </summary>
        public bool ApplyStyles()
        {
            try
            {
                StylesSheetFileManager.StylesSheetFilename = StylesSheetFilename;
                manager.LoadStylesSheetFile();

                foreach (DictionaryEntry values in propertieValues)
                {
                    var c = (Control) values.Key;
                    if (c != null)
                        manager.ApplyStyle(c, (string) values.Value);
                }
                return true;
            }
            catch (Exception exc)
            {
                Exception e;
                if (exc is StylesSheetException)
                    e = exc;
                else
                    e = new StylesSheetException(StylesSheetException.ExceptionType.UnexpectedException, exc);

                MessageBox.Show(e.Message, "StylesSheetManager", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        #endregion

        #region Design time app config management

        /// <summary>
        /// Gets the app settings.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        private string GetAppSettingsValue(string key)
        {
            var env = new VSDesignTimeEnvironment(this);
            string filename = env.GetAppConfigPath();
            if (String.IsNullOrEmpty(filename))
                return null;

            var doc = new XmlDocument();
            doc.Load(filename);
            XmlNode node = doc.SelectSingleNode("//appSettings/add[@key='" + key + "']");
            if (node == null)
                return null;
            else
                return node.Attributes["value"].Value;
        }

        #endregion
    }
}