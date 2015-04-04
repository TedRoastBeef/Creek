using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Creek.UI.EFML.Base;
using Creek.UI.EFML.Base.CSS;
using Creek.UI.EFML.Base.Controls;
using Creek.UI.EFML.Base.Controls.Navigator;
using Creek.UI.EFML.Base.EFML.Elements;
using Creek.UI.EFML.Base.Exceptions;
using Creek.UI.EFML.Base.JS;
using Creek.UI.EFML.Base.JS.ScriptingTypes;
using Creek.UI.EFML.Base.JS.ScriptingTypes.Console;
using Creek.UI.EFML.UI_Elements;
using Creek.UI.Effects;
using Microsoft.ClearScript;
using Microsoft.JScript;
using Microsoft.VisualBasic;
using Button = Creek.UI.EFML.UI_Elements.Button;
using Dropdown = Creek.UI.EFML.UI_Elements.Dropdown;
using Label = Creek.UI.EFML.UI_Elements.Label;
using Line = Creek.UI.EFML.UI_Elements.Line;
using Math = Creek.UI.EFML.Base.JS.ScriptingTypes.Math;
using Object = Creek.UI.EFML.Base.JS.ScriptingTypes.Object;
using Screen = Creek.UI.EFML.Base.JS.ScriptingTypes.Screen;
using TextBox = Creek.UI.EFML.UI_Elements.TextBox;

namespace Creek.UI.EFML
{
    public class EfmlForm
    {
        #region Delegates

        public delegate IUIElement DollarSignFunc(object arg1, object arg2 = null);

        #endregion

        private static ControlProvider provider = new ControlProvider();
        internal static JScriptEngine engine = new JScriptEngine();

        private static Document doc;

        private static Form BuildForm(Document d)
        {
            var f = new Form();
            var layout = new FlowLayoutPanel {Dock = DockStyle.Fill};

            doc = d;


            foreach (MetaElement e in d.Header.Meta)
            {
                switch (e.Name)
                {
                    case "Title":
                        f.Text = e.Content;
                        break;
                    case "ControlProvider":
                        provider.Load(new FileStream(e.Content, FileMode.OpenOrCreate));
                        break;
                }
            }

            if (provider.Controls.Count == 0)
            {
                provider = new WinformsControlProvider();
            }

            BuildControls(layout, d.Body);

            InitJS(f, layout);
            try
            {
                ExecuteScripts(d);
            }
            catch (Exception ex)
            {
                throw new JsException(ex.Message);
            }

            f.Controls.Add(layout);

            return f;
        }

        private static void ExecuteScripts(Document d)
        {
            foreach (ScriptElement s in d.Header.Scripts)
            {
                engine.Execute(s.Source);
            }
        }

        private static void InitJS(Form f, FlowLayoutPanel layout)
        {
            engine.AddGlobal();

            engine.Add("alert", new Func<string, DialogResult>(MessageBox.Show));
            engine.Add("prompt", new Func<string, string>(s => Interaction.InputBox(s)));
            engine.Add("createObj", new Func<string, object>(Functions.CreateObj));
            engine.Add("require", new Action<string>(s => Functions.Require(engine, s)));
            engine.Add("import", new Func<string, string, object[], object>(Functions.Import));

            engine.Add("window", new Window(f));
            engine.Add("document", new Base.JS.ScriptingTypes.Document(doc, layout));

            engine.Add("host", new ExtendedHostFunctions());
            engine.Add("clr",
                       new HostTypeCollection("System", "System.Core", "System.Drawing",
                                              "System.Windows.Forms"));

            engine.Add("XMLHttpRequest", typeof (XmlHttpRequest));
            engine.Add("Extensions", typeof (Extensions));
            engine.Add("Object", typeof (Object));
            engine.Add("JSON", new JSON());
            engine.Add("Math", new Math());
            engine.Add("screen", new Screen());
            engine.Add("console", new FirebugConsole());

            engine.Add("escape", new Func<string, string>(GlobalObject.escape));
            engine.Add("unescape", new Func<string, string>(GlobalObject.unescape));

            engine.Add("parseFloat", new Func<string, float>(float.Parse));
            engine.Add("parseInt", new Func<string, int>(int.Parse));
            engine.Add("String", new Func<string, string>(s => s.toString()));

            engine.Add("setCursor", new Action<Cursor>(c => f.Cursor = c));

            engine.Add("cursors", typeof(Cursors));

            engine.Add("$", new DollarSignFunc((arg1, arg2) =>
                                                   {
                                                       var name = arg1 as string;
                                                       if (name == null)
                                                           new Window(f).AddEventHandler("Load", arg1);
                                                       else if (arg2 != null)
                                                           new Window(f).AddEventHandler(name, arg2);
                                                       else
                                                       {
                                                           return
                                                               new Base.JS.ScriptingTypes.Document(doc, layout)
                                                                   .GetElementById(name);
                                                       }
                                                       return new PlaceholderTextBox();
                                                   }));
            engine.Add("ui", new Func<object, Control>(o => (Control) o));
            engine.Add("color", new Func<string, Color>(s => new Base.CSS.Converters.ColorConverter().Convert(s)));
            engine.Add("eval",
                       new Func<string, object>(
                           o => engine.Evaluate(o)));

            engine.Add("Transition", typeof (Transition));
            engine.Add(new[] { new KeyValuePair<string, Type>("bounce", typeof(TransitionType_Bounce)), new KeyValuePair<string, Type>("linear", typeof(TransitionType_Linear)), new KeyValuePair<string, Type>("ease_inout", typeof(TransitionType_EaseInEaseOut)) });

            //engine.Add("Iterator", new Func<object[], Iterator>(objects => ));
        }

        private static void BuildControls(Control parent, IEnumerable<ElementBase> d)
        {
            foreach (UiElement b in d)
            {
                Control c = null;
                if (b is Label)
                {
                    var bc = b as Label;
                    c = provider[Tag.Label];
                }
                if (b is Button)
                {
                    var bc = b as Button;
                    Control cc = provider[Tag.Button];
                    c = cc;
                }
                if (b is TextBox)
                {
                    var bc = b as TextBox;
                    c = provider[Tag.Textbox];
                    var cc = c as PlaceholderTextBox;
                    cc.TextChanged += (sender, args) => bc.content = cc.Text;
                    cc.PlaceholderText = bc.placeholder;
                }
                if (b is Checkbox)
                {
                    var bc = b as Checkbox;
                    var bcc = provider[Tag.Checkbox] as CheckBox;
                    bcc.Checked = bc.Content;
                    c = bcc;
                }
                if (b is Div)
                {
                    var bc = b as Div;
                    var bcc = provider[Tag.Div] as DivPanel;
                    bcc.BorderStyle = bc.border;

                    BuildControls(bcc, bc.Childs);

                    c = bcc;
                }
                if (b is Radio)
                {
                    var bc = b as Radio;
                    var bcc = provider[Tag.Radiobutton] as RadioButton;
                    bcc.Checked = bc.Content;
                    c = bcc;
                }
                if (b is FlashElement)
                {
                    //c = (Control) Activator.CreateInstance((b as FlashElement).Type);
                }
                if (b is Dropdown)
                {
                    var bc = b as Dropdown;
                    var bcc = provider[Tag.Dropdown] as Base.Controls.Dropdown;
                    bcc.Items.AddRange(bc.Childs.ToArray());
                    bcc.DropDownStyle = bc.style == DropdownStyle.list
                                            ? ComboBoxStyle.DropDownList
                                            : ComboBoxStyle.DropDown;
                    bcc.SelectedIndexChanged += (sender, args) => engine.Execute(bc.Events["onselectionchange"]);

                    c = bcc;
                }
                if (b is Navigator)
                {
                    var bc = b as Navigator;
                    var bcc = provider[Tag.Navigator] as PageNavigator;

                    if (bc.Pages.Count <= 1)
                        throw new EfmlException("Navigator '" + bc.ID + "' required more than one page");

                    if (bc.backgroundimage != null)
                        bcc.BackGround = bc.backgroundimage;
                    if (bc.backbuttonimage != null)
                        bcc.BackwardButton = bc.backbuttonimage;
                    if (bc.forwardbuttonimage != null)
                        bcc.ForwardButton = bc.forwardbuttonimage;

                    foreach (Page p in bc.Pages)
                    {
                        var pp = new UserControl {Text = p.Caption, Name = p.Caption};

                        BuildControls(pp, p.Childs);
                        bcc.Pages.Add(pp);
                    }
                    bcc.RefreshPages();

                    c = bcc;
                }
                if (b is Link)
                {
                    var bc = b as Link;
                    var cc = provider[Tag.Link] as LinkLabel;
                    cc.Click += (sender, args) => engine.Execute(bc.Events["onclick"]);
                    c = cc;
                }
                if (b is Line)
                {
                    var bc = b as Line;
                    var cc = provider[Tag.Line] as Base.Controls.Line;
                    cc.BorderColor = bc.bordercolor;
                    c = cc;
                }


                if (c != null)
                {
                    c.Text = b.content;
                    c.Name = b.ID;
                    c.Click += (sender, args) => engine.Execute(b.Events["onclick"]);
                    c.MouseEnter += (sender, args) => engine.Execute(b.Events["onhover"]);
                    c.MouseLeave += (sender, args) => engine.Execute(b.Events["onleave"]);

                    parent.Controls.Add(c);
                }
            }
        }

        [DebuggerStepThrough]
        public static Form Build(byte[] by, TagnameProvider tnp = null)
        {
            if (tnp == null)
                tnp = new TagnameProvider();

            Global.TagnameProvider = tnp;

            var b = new Builder();
            b.Load(by);

            StyleChanger.Execute(b.document);

            return BuildForm(b.document);
        }

        public static Form Build(string s)
        {
            return Build(Encoding.ASCII.GetBytes(s));
        }

        public static Form Build(Stream s)
        {
            return Build(new StreamReader(s).ReadToEnd());
        }
    }
}