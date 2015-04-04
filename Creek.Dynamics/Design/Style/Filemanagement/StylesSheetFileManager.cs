//*****************************************************************************
//	Done by : Sylvain BLANCHARD
//	Date : 08/01/2006
//*****************************************************************************

using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Creek.Dynamics.Design.Style.Filemanagement
{
    /// <summary>
    /// Class which affect the control according to the properties
    /// found in the styles sheet file
    /// </summary>
    class StylesSheetFileManager
    {
        private static string stylesSheetFilename = string.Empty;
        private static bool stylesSheetFilenameHasChanged = true;
        private static StylesSheetFile file;

        /// <summary>
        /// Gets or sets the styles sheet filename.
        /// </summary>
        /// <value>The styles sheet filename.</value>
        public static string StylesSheetFilename
        {
            get
            {
                return stylesSheetFilename;
            }
            set
            {
                if (stylesSheetFilename != value)
                    stylesSheetFilenameHasChanged = true;
                else
                    stylesSheetFilenameHasChanged = false;
                stylesSheetFilename = value;
            }
        }

        /// <summary>
        /// Load the styles sheet file
        /// </summary>
        public void LoadStylesSheetFile()
        {
            // If not changed, we don't reload the styles sheet file
            if (stylesSheetFilenameHasChanged || file == null)
            {
                if (!File.Exists(StylesSheetFilename))
                    throw new StylesSheetException(StylesSheetException.ExceptionType.StylesSheetFileNotFound, StylesSheetFilename);

                file = new StylesSheetFile();
                XmlSerializer serializer = new XmlSerializer(file.GetType());
                //ToDo: Add CSS-Style functionallity
                TextReader reader = new StreamReader(StylesSheetFilename);
                file = (StylesSheetFile)serializer.Deserialize(reader);
                reader.Close();
            }
        }

        /// <summary>
        /// Set the properties defined in the styles sheet file
        /// for the "c" control using the style name specified as parameter.
        /// </summary>
        /// <param name="control">the control</param>
        /// <param name="styleName">the name of the style to applied</param>
        public void ApplyStyle(Control control, string styleName)
        {
            if (!styleName.Equals(string.Empty))
            {
                Creek.Dynamics.Design.Style.Filemanagement.Style style = GetStyle(styleName);
                if (style == null)
                    throw new StylesSheetException(StylesSheetException.ExceptionType.StyleNotPresentInStylesSheetFile, styleName, control.Name, String.Empty, String.Empty);

                PropertySetter setter = new PropertySetter();
                foreach (Property property in style.Properties)
                {
                    if (property.Name == null)
                        throw new StylesSheetException(StylesSheetException.ExceptionType.PropertyNameTagNotFound, styleName, String.Empty, String.Empty, String.Empty);

                    if (property.Value == null)
                        throw new StylesSheetException(StylesSheetException.ExceptionType.PropertyValueTagNotFound, styleName, String.Empty, property.Name, String.Empty);
                    setter.Set(styleName, control, property.Name, property.Value);
                }
            }
        }

        /// <summary>
        /// Gets the style from its name.
        /// </summary>
        /// <param name="styleName">Name of the style.</param>
        /// <returns>the style or null if not found</returns>
        private Creek.Dynamics.Design.Style.Filemanagement.Style GetStyle(string styleName)
        {
            foreach (Creek.Dynamics.Design.Style.Filemanagement.Style style in file.Styles)
            {
                if (style.Name == styleName)
                    return style;
            }

            return null;
        }
    }
}
