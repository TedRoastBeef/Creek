//*****************************************************************************
//	Done by : Sylvain BLANCHARD
//	Date : 08/01/2006
//*****************************************************************************

using System;
using System.Threading;
using System.Globalization;
using System.Resources;
using System.Reflection;

namespace Creek.Dynamics.Design.Style
{
    /// <summary>
    /// Class which groups all the exception which can be thrown by the StylesSheetManager.
    /// </summary>
	public class StylesSheetException : Exception
	{
		protected ResourceManager rm = new ResourceManager("Lib.Style.StylesSheetManager", Assembly.GetExecutingAssembly());
        protected CultureInfo ci = Thread.CurrentThread.CurrentCulture;
        protected string message;
        protected ExceptionType type;
        protected string _stylesSheetFilePath;
        protected string controlName;
        protected string styleName;
        protected string propertyName;
        protected string propertyValue;
        protected string innerExceptionMessage;



        /// <summary>
        /// Enum of the exception type. Use to retrieve the goog message in 'Message' property
        /// </summary>
        public enum ExceptionType
        {
            StylesSheetFileNameNotDefinedInAppConfig,
            StylesSheetFileNotFound,
            StyleNotPresentInStylesSheetFile,
            PropertyNameTagNotFound,
            PropertyValueTagNotFound,
            IndexedPropertyNotCorrectlyDefined,
            PropertyNotFound,
            UnexpectedException
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:StylesSheetException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public StylesSheetException(ExceptionType type)
        {
           this.type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:StylesSheetException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="stylesSheetFilePath">The styles sheet file path.</param>
        public StylesSheetException(ExceptionType type, string stylesSheetFilePath)
        {
           this.type = type;
            _stylesSheetFilePath = stylesSheetFilePath;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:StylesSheetException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public StylesSheetException(ExceptionType type, Exception innerException)
        {
           this.type = type;
           innerExceptionMessage = innerException.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:StylesSheetException"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="controlName">Name of the control.</param>
        /// <param name="styleName">Name of the style.</param>
        public StylesSheetException(ExceptionType type, string styleName, string controlName, string propertyName, string propertyValue)
        {
           this.type = type;
           this.styleName = styleName;
           this.controlName = controlName;
           this.propertyName = propertyName;
           this.propertyValue = propertyValue;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <value></value>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
		public override string Message
		{
			get
			{
                switch (type)
                {
                    case ExceptionType.StylesSheetFileNameNotDefinedInAppConfig :
                        return rm.GetString("StylesSheetFileNameNotDefinedInAppConfigException", ci);

                    case ExceptionType.StylesSheetFileNotFound :
                        return String.Format(rm.GetString("StylesSheetFileNotFoundException", ci), _stylesSheetFilePath);

                    case ExceptionType.StyleNotPresentInStylesSheetFile :
                        return String.Format(rm.GetString("StyleNotPresentInStylesSheetFileException", ci), styleName, controlName);

                    case ExceptionType.PropertyNameTagNotFound:
                        return String.Format(rm.GetString("PropertyNameTagNotFoundException", ci), styleName, propertyName);

                    case ExceptionType.PropertyValueTagNotFound:
                        return String.Format(rm.GetString("PropertyValueTagNotFoundException", ci), styleName, propertyName);

                    case ExceptionType.IndexedPropertyNotCorrectlyDefined:
                        return String.Format(rm.GetString("IndexedPropertyNotCorrectlyDefinedException", ci), styleName, controlName, propertyName);

                    case ExceptionType.PropertyNotFound:
                        return String.Format(rm.GetString("PropertyNotFoundException", ci), controlName, propertyName);

                    case ExceptionType.UnexpectedException:
                        return String.Format(rm.GetString("UnexpectedException", ci), "\r\n\r", innerExceptionMessage);

                    default: // Must not be reached
                        return base.Message;
                }
			}
		}
	}
}
