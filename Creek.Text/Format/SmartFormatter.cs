﻿namespace Creek.Text.Format
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Lib.Format.Core;
    using Lib.Format.Core.Extensions;
    using Lib.Format.Core.Output;
    using Lib.Format.Core.Parsing;

    using FormatException = Creek.Text.Format.Core.FormatException;

    /// <summary>
    /// This class contains the Format method that constructs 
    /// the composite string by invoking each extension.
    /// </summary>
    public class SmartFormatter
    {
        #region: Constructor :

        public SmartFormatter()
#if DEBUG
            : this(ErrorAction.ThrowError)
#else
            : this(ErrorAction.Ignore)
            #endif
        {
        }

        public SmartFormatter(ErrorAction errorAction)
        {
            this.Parser = new Parser(errorAction);
            this.ErrorAction = errorAction;
            this.SourceExtensions = new List<ISource>();
            this.FormatterExtensions = new List<IFormatter>();
        }

        #endregion

        #region: Extension Registration :

        public List<ISource> SourceExtensions { get; private set; }
        public List<IFormatter> FormatterExtensions { get; private set; }

        /// <summary>
        /// Adds each extensions to this formatter.
        /// Each extension must implement ISource, IFormatter, or both.
        /// 
        /// An exception will be thrown if the extension doesn't implement those interfaces.
        /// </summary>
        /// <param name="extensions"></param>
        [Obsolete("Please use the specific overloads of AddExtensions().")]
        public void AddExtensions(params object[] extensions)
        {
            foreach (object extension in extensions)
            {
                // We need to filter each extension to the correct list:
                var source = extension as ISource;
                var formatter = extension as IFormatter;

                // If this object ISN'T a extension, throw an exception:
                if (source == null && formatter == null)
                    throw new ArgumentException(
                        string.Format("{0} does not implement ISource nor IFormatter.", extension.GetType().FullName),
                        "extensions");

                if (source != null)
                    this.SourceExtensions.Add(source);
                if (formatter != null)
                    this.FormatterExtensions.Add(formatter);
            }
        }

        /// <summary>
        /// Adds each extensions to this formatter.
        /// Each extension must implement ISource.
        /// </summary>
        /// <param name="sourceExtensions"></param>
        public void AddExtensions(params ISource[] sourceExtensions)
        {
            this.SourceExtensions.AddRange(sourceExtensions);
        }

        /// <summary>
        /// Adds each extensions to this formatter.
        /// Each extension must implement IFormatter.
        /// </summary>
        /// <param name="formatterExtensions"></param>
        public void AddExtensions(params IFormatter[] formatterExtensions)
        {
            this.FormatterExtensions.AddRange(formatterExtensions);
        }

        #endregion

        #region: Properties :

        public Parser Parser { get; set; }
        public ErrorAction ErrorAction { get; set; }

        #endregion

        #region: Format Overloads :

        public string Format(string format, params object[] args)
        {
            return this.Format(null, format, args);
        }

        public string Format(IFormatProvider provider, string format, params object[] args)
        {
            var output = new StringOutput(format.Length + args.Length*8);

            Lib.Format.Core.Parsing.Format formatParsed = this.Parser.ParseFormat(format);
            object current = (args != null && args.Length > 0) ? args[0] : args; // The first item is the default.
            var formatDetails = new FormatDetails(this, args, null, provider);
            this.Format(output, formatParsed, current, formatDetails);

            return output.ToString();
        }

        public void FormatInto(IOutput output, string format, params object[] args)
        {
            Lib.Format.Core.Parsing.Format formatParsed = this.Parser.ParseFormat(format);
            object current = (args != null && args.Length > 0) ? args[0] : args; // The first item is the default.
            var formatDetails = new FormatDetails(this, args, null, null);
            this.Format(output, formatParsed, current, formatDetails);
        }

        public string FormatWithCache(ref FormatCache cache, string format, params object[] args)
        {
            var output = new StringOutput(format.Length + args.Length*8);

            if (cache == null) cache = new FormatCache(this.Parser.ParseFormat(format));
            object current = (args != null && args.Length > 0) ? args[0] : args; // The first item is the default.
            var formatDetails = new FormatDetails(this, args, cache, null);
            this.Format(output, cache.Format, current, formatDetails);

            return output.ToString();
        }

        public void FormatWithCacheInto(ref FormatCache cache, IOutput output, string format, params object[] args)
        {
            if (cache == null) cache = new FormatCache(this.Parser.ParseFormat(format));
            object current = (args != null && args.Length > 0) ? args[0] : args; // The first item is the default.
            var formatDetails = new FormatDetails(this, args, cache, null);
            this.Format(output, cache.Format, current, formatDetails);
        }

        #endregion

        #region: Format :

        public void Format(IOutput output, Lib.Format.Core.Parsing.Format format, object current, FormatDetails formatDetails)
        {
            // Before we start, make sure we have at least one source extension and one formatter extension:
            this.CheckForExtensions();
            Placeholder originalPlaceholder = formatDetails.Placeholder;
            foreach (FormatItem item in format.Items)
            {
                var literalItem = item as LiteralText;
                if (literalItem != null)
                {
                    formatDetails.Placeholder = originalPlaceholder;
                    output.Write(literalItem.baseString, literalItem.startIndex,
                                 literalItem.endIndex - literalItem.startIndex, formatDetails);
                    continue;
                } // Otherwise, the item is a placeholder.

                var placeholder = (Placeholder) item;
                object context = current;
                formatDetails.Placeholder = placeholder;

                bool handled;
                // Evaluate the selectors:
                foreach (Selector selector in placeholder.Selectors)
                {
                    handled = false;
                    object result = context;
                    this.InvokeSourceExtensions(context, selector, ref handled, ref result, formatDetails);
                    if (!handled)
                    {
                        // The selector wasn't handled, which means it isn't valid
                        this.FormatError(selector, string.Format("Could not evaluate the selector \"{0}\"", selector.Text),
                                    selector.startIndex, output, formatDetails);
                        context = null;
                        break;
                    }
                    context = result;
                }

                // Evaluate the format:
                handled = false;
                try
                {
                    this.InvokeFormatterExtensions(context, placeholder.Format, ref handled, output, formatDetails);
                }
                catch (Exception ex)
                {
                    // An error occurred while formatting.
                    int errorIndex = placeholder.Format != null
                                         ? placeholder.Format.startIndex
                                         : placeholder.Selectors.Last().endIndex;
                    this.FormatError(item, ex, errorIndex, output, formatDetails);
                    continue;
                }
            }
        }

        private void CheckForExtensions()
        {
            if (this.SourceExtensions.Count == 0)
            {
                throw new InvalidOperationException(
                    "No source extensions are available.  Please add at least one source extension, such as the DefaultSource.");
            }
            if (this.FormatterExtensions.Count == 0)
            {
                throw new InvalidOperationException(
                    "No formatter extensions are available.  Please add at least one formatter extension, such as the DefaultFormatter.");
            }
        }

        private void InvokeSourceExtensions(object current, Selector selector, ref bool handled, ref object result,
                                            FormatDetails formatDetails)
        {
            foreach (ISource sourceExtension in this.SourceExtensions)
            {
                sourceExtension.EvaluateSelector(current, selector, ref handled, ref result, formatDetails);
                if (handled) break;
            }
        }

        private void InvokeFormatterExtensions(object current, Lib.Format.Core.Parsing.Format format, ref bool handled,
                                               IOutput output, FormatDetails formatDetails)
        {
            foreach (IFormatter formatterExtension in this.FormatterExtensions)
            {
                formatterExtension.EvaluateFormat(current, format, ref handled, output, formatDetails);
                if (handled) break;
            }
        }

        private void FormatError(FormatItem errorItem, string issue, int startIndex, IOutput output,
                                 FormatDetails formatDetails)
        {
            switch (this.ErrorAction)
            {
                case ErrorAction.Ignore:
                    return;
                case ErrorAction.ThrowError:
                    throw new FormatException(errorItem, issue, startIndex);
                case ErrorAction.OutputErrorInResult:
                    formatDetails.FormatError = new FormatException(errorItem, issue, startIndex);
                    output.Write(issue, formatDetails);
                    formatDetails.FormatError = null;
                    break;
                case ErrorAction.MaintainTokens:
                    output.Write(formatDetails.Placeholder.Text, formatDetails);
                    break;
            }
        }

        private void FormatError(FormatItem errorItem, Exception innerException, int startIndex, IOutput output,
                                 FormatDetails formatDetails)
        {
            switch (this.ErrorAction)
            {
                case ErrorAction.Ignore:
                    return;
                case ErrorAction.ThrowError:
                    throw new FormatException(errorItem, innerException, startIndex);
                case ErrorAction.OutputErrorInResult:
                    formatDetails.FormatError = new FormatException(errorItem, innerException, startIndex);
                    output.Write(innerException.Message, formatDetails);
                    formatDetails.FormatError = null;
                    break;
                case ErrorAction.MaintainTokens:
                    output.Write(formatDetails.Placeholder.Text, formatDetails);
                    break;
            }
        }

        #endregion
    }
}