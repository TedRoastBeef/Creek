using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Creek.UI.EFML.Base.CSS.Converters;
using Creek.UI.EFML.Base.EFML.Elements;
using Creek.UI.EFML.Base.Exceptions;
using Creek.UI.Effects;
using ColorConverter = Creek.UI.EFML.Base.CSS.Converters.ColorConverter;
using FontConverter = Creek.UI.EFML.Base.CSS.Converters.FontConverter;
using ImageConverter = Creek.UI.EFML.Base.CSS.Converters.ImageConverter;
using PaddingConverter = Creek.UI.EFML.Base.CSS.Converters.PaddingConverter;
using SizeConverter = Creek.UI.EFML.Base.CSS.Converters.SizeConverter;

namespace Creek.UI.EFML.Base.CSS
{
    internal class StyleChanger
    {
        [DebuggerStepThrough]
        public static void Execute(Document b)
        {
            foreach (StyleElement elementBase in b.Header.Styles.Where(elementBase => elementBase is StyleElement))
            {
                IEnumerable<CssParserRule> p = new CssParser().ParseAll(elementBase.Source);
                foreach (CssParserRule cssParserRule in p)
                {
                    foreach (string selector in cssParserRule.Selectors)
                    {
                        foreach (ElementBase bb in b.Body)
                        {
                            if (bb is UiElement)
                            {
                                var bbb = bb as UiElement;
                                if ("#" + bbb.ID == selector)
                                {
                                    ExecuteQuery(bbb, cssParserRule);
                                }
                                if (!selector.StartsWith("#"))
                                {
                                    throw new CssException("'" + selector + "'- is not a valid sector");
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void ExecuteQuery(UiElement bbb, CssParserRule cssParserRule)
        {
            foreach (CssParserDeclaration d in cssParserRule.Declarations)
            {
                FieldInfo prop = bbb.GetType().GetField(d.Property);
                if (prop != null)
                {
                    if (prop.FieldType.Name == typeof (Padding).Name)
                        prop.SetValue(bbb, new PaddingConverter().Convert(d.Value));
                    if (prop.FieldType.Name == typeof (Size).Name)
                        prop.SetValue(bbb, new SizeConverter().Convert(d.Value));
                    if (prop.FieldType.Name == typeof (Color).Name)
                        prop.SetValue(bbb, new ColorConverter().Convert(d.Value));
                    if (prop.FieldType.Name == typeof (Font).Name)
                        prop.SetValue(bbb, new FontConverter().Convert(d.Value));
                    if (prop.FieldType.Name == typeof(bool).Name)
                        prop.SetValue(bbb, new BoolConverter().Convert(d.Value));
                    if (prop.FieldType.Name == typeof(Image).Name)
                        prop.SetValue(bbb, d.Value == "null" ? new NullConverter().Convert(d.Value) : new ImageConverter().Convert(d.Value));
                    if (prop.FieldType.IsEnum)
                        prop.SetValue(bbb, EnumConverter.Convert(prop.FieldType, d.Value));
                }
            }
        }
    }
}