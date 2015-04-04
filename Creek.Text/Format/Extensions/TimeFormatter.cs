using System;
using System.Globalization;
using Lib.Format.Core.Extensions;
using Lib.Format.Core.Output;
using Lib.Format.Utilities;

namespace Lib.Format.Extensions
{
    public class TimeFormatter : IFormatter
    {

        #region Constructors

        /// <summary>
        /// Initializes the extension with no default TimeTextInfo.
        /// </summary>
        public TimeFormatter() : this(null)
        {
        }
        /// <summary>
        /// Initializes the extension with a default TimeTextInfo.
        /// </summary>
        /// <param name="defaultTwoLetterLanguageName">This will be used when no CultureInfo is supplied.  Can be null.</param>
        public TimeFormatter(string defaultTwoLetterLanguageName)
        {
            this.DefaultFormatOptions = TimeSpanUtility.DefaultFormatOptions;
            this.defaultTwoLetterISOLanguageName = defaultTwoLetterLanguageName;
        }

        #endregion
        
        #region Defaults

        public TimeSpanFormatOptions DefaultFormatOptions { get; set; }
        private string defaultTwoLetterISOLanguageName;

        #endregion

        #region IFormatter

        public void EvaluateFormat(object current, Core.Parsing.Format format, ref bool handled, IOutput output, FormatDetails formatDetails)
        {
            if (format != null && format.HasNested) return;
            var formatText = format != null ? format.Text : "";
            TimeSpan fromTime;
            if (current is TimeSpan)
            {
                fromTime = (TimeSpan)current;
            }
            else if (current is DateTime && formatText.StartsWith("timestring"))
            {
                formatText = formatText.Substring(10);
                fromTime = DateTime.Now.Subtract((DateTime)current);
            }
            else
            {
                return;
            }
            var timeTextInfo = GetTimeTextInfo(formatDetails.Provider);
            if (timeTextInfo == null)
            {
                return;
            }
            var formattingOptions = TimeSpanFormatOptionsConverter.Parse(formatText);
            var timeString = TimeSpanUtility.ToTimeString(fromTime, formattingOptions, timeTextInfo);
            output.Write(timeString, formatDetails);
            handled = true;

        }

        private TimeTextInfo GetTimeTextInfo(IFormatProvider provider)
        {
            if (provider != null)
            {
                // See if the provider can give us what we want:
                var timeTextInfo = (TimeTextInfo)provider.GetFormat(typeof (TimeTextInfo));
                if (timeTextInfo != null)
                {
                    return timeTextInfo;
                }

                // See if there is a rule for this culture:
                var cultureInfo = provider as CultureInfo;
                if (cultureInfo != null)
                {
                    timeTextInfo = CommonLanguagesTimeTextInfo.GetTimeTextInfo(cultureInfo.TwoLetterISOLanguageName);
                    // If cultureInfo was supplied, 
                    // we will always return, even if null:
                    return timeTextInfo;
                }
            }

            // Return the default if the provider couldn't provide:
            return CommonLanguagesTimeTextInfo.GetTimeTextInfo(defaultTwoLetterISOLanguageName);
        }

        #endregion

    }

}
