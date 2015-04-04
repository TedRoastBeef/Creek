using System.Xml;
using Creek.UI.EFML.Base.Validators;
using Creek.UI.EFML.UI_Elements;
using Creek.UI.EFML.UI_Elements.EventProviders;

namespace Creek.UI.EFML.Base.EFML.Processors
{
    internal class InputProcessor : ElementProcessor
    {
        #region Implementation of ElementProcessor

        public override string Tagname
        {
            get { return "input"; }
        }

        public override void Process(out UiElement ui, XmlNode t, Builder b)
        {
            UiElement ee = null;
            if (t.Name == "input")
            {
                switch (t.GetAttributeByName("type"))
                {
                    case "button":
                        ee = new Button();
                        break;
                    case "text":
                        ee = new TextBox
                                 {
                                     placeholder =
                                         t.HasAttribute("placeholder") ? t.GetAttributeByName("placeholder") : ""
                                 };
                        new TextEventProvider().Resolve(ee, t);
                        break;
                    case "password":
                        ee = new PasswordBox
                                 {
                                     placeholder =
                                         t.HasAttribute("placeholder") ? t.GetAttributeByName("placeholder") : "",
                                     PasswordMask = t.HasAttribute("mask") ? t.GetAttributeByName("mask") : ""
                                 };
                        new TextEventProvider().Resolve(ee, t);
                        break;
                    case "area":
                        ee = new TextArea();
                        new TextEventProvider().Resolve(ee, t);
                        break;
                    case "radio":
                        var eee = new Radio {Content = bool.Parse(t.GetAttributeByName("value"))};
                        ee = eee;
                        break;
                    case "check":
                        var eeec = new Checkbox {Content = bool.Parse(t.GetAttributeByName("value"))};
                        ee = eeec;
                        new CheckBoxEventProvider().Resolve(ee, t);
                        break;
                    default:
                        if (t.GetAttributeByName("type") == "email")
                        {
                            ee = new TextBox
                                     {
                                         placeholder =
                                             t.HasAttribute("placeholder") ? t.GetAttributeByName("placeholder") : ""
                                     };
                            new TextEventProvider().Resolve(ee, t);
                            ee.Validator = new EmailValidator();
                        }
                        else if (t.GetAttributeByName("type") == "number")
                        {
                            ee = new TextBox
                                     {
                                         placeholder =
                                             t.HasAttribute("placeholder") ? t.GetAttributeByName("placeholder") : ""
                                     };
                            new TextEventProvider().Resolve(ee, t);
                            ee.Validator = new NumberValidator();
                        }
                        else if (t.GetAttributeByName("type") == "date")
                        {
                            ee = new TextBox
                                     {
                                         placeholder =
                                             t.HasAttribute("placeholder") ? t.GetAttributeByName("placeholder") : ""
                                     };
                            new TextEventProvider().Resolve(ee, t);
                            ee.Validator = new DateValidator();
                        }
                        else if (t.GetAttributeByName("type") == "version")
                        {
                            ee = new TextBox
                                     {
                                         placeholder =
                                             t.HasAttribute("placeholder") ? t.GetAttributeByName("placeholder") : ""
                                     };
                            new TextEventProvider().Resolve(ee, t);
                            ee.Validator = new VersionValidator();
                        }
                        else
                        {
                            ee = new UiElement();
                        }
                        break;
                }
            }
            ui = ee;
        }

        #endregion
    }
}