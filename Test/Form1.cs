namespace Test
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;

    using Creek.Behaviors;
    using Creek.Contracts;
    using Creek.Dynamics;
    using Creek.Dynamics.XML.AST;
    using Creek.IO;
    using Creek.IO.Binary;
    using Creek.Library.Fonts;
    using Creek.Net;
    using Creek.Parsers.Mathematics;
    using Creek.Parsing.Generator;
    using Creek.Security.USBKeys;
    using Creek.Text;
    using Creek.Tools;
    using Creek.UI;
    using Creek.UI.ExceptionReporter;
    using Creek.UI.ExceptionReporter.Core;
    using Creek.UI.MozOptions;
    using Creek.UI.Options;
    using Creek.UI.Popups;
    using Creek.UI.QuickMouse;

    using Test.CalculatorParser.Nonterminals;
    using Test.Properties;

    using Extensions = Creek.Extensions.Extensions;
    using FileSystem = Creek.Data.VFS.FileSystem;
    using Message = Creek.Messaging.Message;
    using TabPage = Creek.UI.Options.TabPage;

    public partial class Form1 : Form
    {
        #region Fields

        private readonly QuickMouseMenu quickMouseMenuUC1;

        private Parser m_parser;

        private TerminalReader m_terminalReader;

        #endregion

        #region Constructors and Destructors

        public Form1()
        {
            this.InitializeComponent();
            this.FormClosing += Form_Closing;

            this.quickMouseMenuUC1 = new QuickMouseMenu
                { Size = new Size(200, 200), HideOnMouseLeave = true, NavigateOnHover = true };

            this.quickMouseMenuUC1.QuickMenuItemClicked += this.quickMouseMenuUC1_QuickMenuItemClicked;

            //add some test menu
            this.quickMouseMenuUC1.AddQuickMouseMenuItem(Resources.ich, "Ich", "Adding new value...");
            this.quickMouseMenuUC1.AddQuickMouseMenuItem(Resources.ich, "Ich", "Adding new value...");
            this.quickMouseMenuUC1.AddQuickMouseMenuItem(Resources.ich, "Ich", "Adding new value...");
            this.quickMouseMenuUC1.AddQuickMouseMenuItem(Resources.ich, "Ich", "Adding new value...");

            var tc = new TestCombo();

            this.Controls.Add(tc);

            this.listCombo1.listBox1.Items.Add("t");
        }

        #endregion

        #region Enums

        public enum Gender
        {
            Male,

            Female
        }

        #endregion

        #region Public Methods and Operators

        public void CreateParser()
        {
            Assembly assembly = Assembly.GetAssembly(typeof(Form1));

            var grammar = new Grammar();
            grammar.Load(assembly, "Calculator");
            grammar.LoadRules(assembly, "Calculator");
            grammar.Resolve();

            Trace.WriteLine("TERMINALS");
            foreach (var terminal in grammar.GetTerminals())
            {
                Trace.WriteLine(terminal.Name);
                var sb = new StringBuilder();
                sb.Append("  First:");
                foreach (var first in terminal.First)
                {
                    sb.Append(" ");
                    sb.Append(first == null ? "e" : first.Name);
                }
                Trace.WriteLine(sb.ToString());
            }

            Trace.WriteLine("NONTERMINALS");
            foreach (var nonterminal in grammar.GetNonterminals())
            {
                Trace.WriteLine(nonterminal.Name);

                foreach (var rule in nonterminal.Rules)
                {
                    Trace.WriteLine("  " + rule.ToString());
                }

                var sb = new StringBuilder();
                sb.Append("  First:");
                foreach (var first in nonterminal.First)
                {
                    sb.Append(" ");
                    sb.Append(first == null ? "e" : first.Name);
                }
                Trace.WriteLine(sb.ToString());

                sb = new StringBuilder();
                sb.Append("  Follow:");
                foreach (var first in nonterminal.Follow)
                {
                    sb.Append(" ");
                    sb.Append(first == null ? "e" : first.Name);
                }
                Trace.WriteLine(sb.ToString());
            }

            ITerminalReaderGenerator terminalReaderGenerator = new TerminalReaderGenerator();
            TerminalReaderGeneratorResult terminalReaderGeneratorResult =
                terminalReaderGenerator.GenerateTerminalReader(grammar);
            this.m_terminalReader = (TerminalReader)terminalReaderGeneratorResult.TerminalReader;

            IParserGenerator parserGenerator = new ParserGenerator();
            ParserGeneratorResult parserGeneratorResult = parserGenerator.GenerateParser(grammar);
            this.m_parser = (Parser)parserGeneratorResult.Parser;

            ITerminalReader terminalReader = terminalReaderGeneratorResult.TerminalReader;

            terminalReader.Open("(123 + 34) / 52");

            IParser parser = parserGeneratorResult.Parser;
            parser.Parse(terminalReader);

            var token = terminalReader.ReadTerminal();
            while (token != null)
            {
                if (!token.ElementType.Ignore)
                {
                    Console.WriteLine("{0}: {1}", token.ElementType.Name, token.Text);
                }
                token = terminalReader.ReadTerminal();
            }

            //Console.WriteLine("Press Enter to exit.");
            //Console.ReadLine();
        }

        #endregion

        #region Methods

        private void Form1_Load(object sender, EventArgs e)
        {
            this.firefoxDialog1.AddPage("Home", 0, new PropertyPage());
            this.firefoxDialog1.AddPage("Security", 1, new PropertyPage());
            this.firefoxDialog1.AddPage("Installed", 2, new PropertyPage());
            this.firefoxDialog1.BackColor = OptionColors.FirefoxBackgroundGreyBrown;
            this.firefoxDialog1.Init();

            this.projectProperties1.TabItems.Add(new TabItem("Home", new TabPage()));
            this.projectProperties1.TabItems.Add(new TabItem("Security", new TabPage()));
            this.projectProperties1.TabItems.Add(new TabItem("Debugging", new TabPage()));
            this.projectProperties1.TabItems.Add(new TabItem("Publish", new TabPage()));

            this.groupedComboBox1.ValueMember = "Value";
            this.groupedComboBox1.DisplayMember = "Display";
            this.groupedComboBox1.GroupMember = "Group";

            this.groupedComboBox1.DataSource =
                new ArrayList(
                    new object[]
                        {
                            new { Value = 100, Display = "Apples", Group = "Fruit" },
                            new { Value = 101, Display = "Pears", Group = "Fruit" },
                            new { Value = 102, Display = "Carrots", Group = "Vegetables" },
                            new { Value = 103, Display = "Beef", Group = "Meat" },
                            new { Value = 104, Display = "Cucumbers", Group = "Vegetables" },
                            new { Value = 0, Display = "(other)", Group = String.Empty },
                            new { Value = 105, Display = "Chillies", Group = "Vegetables" },
                            new { Value = 106, Display = "Strawberries", Group = "Fruit" }
                        });
        }

        [DebuggerStepThrough]
        private void TheConditionsWay(int id, IEnumerable<int> col, DayOfWeek day)
        {
            Contract.Requires(id, "id").IsGreaterThan(0);
            Contract.Requires(col, "col").IsNotEmpty();
            Contract.Requires(day, "day").IsInRange(DayOfWeek.Monday, DayOfWeek.Friday);
            // Do method work
        }

        private void button10_Click(object sender, EventArgs e)
        {
            dynamic u = Instance.FromXml("<user><name>filmee24</name></user>");
            MessageBox.Show(u.user.name);
            dynamic arr = Instance.FromString("array=[emile,linus];dictionary={first:one,second:two}");
            MessageBox.Show(arr.array[0]);
            dynamic dic = Instance.FromString("dictionary={first:true,second:0}");
            MessageBox.Show(dic.dictionary.second.ToString());

            dynamic func = Instance.FromFunction("define(key,true)");
            MessageBox.Show(func.key.ToString());

            dynamic fb = Instance.FromFunctionBlock("define(username,filmee24);define(password,123456c)");

            MessageBox.Show(fb.username + " + " + fb.password);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            var l = new List<Color> { Color.Red, Color.Yellow, Color.Blue };

            MessageBox.Show(Rndm.GetList(l).Name);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Focus();
            Keyboard.Push("Chris", Keys.Enter, 0);
            Mouse.SetCursorPosition(150, 250);
            Mouse.RightClick();
            Mouse.RightRelease();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            var h = new HTML();

            h.Show();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            var m = new Map();
            API.SetParent(m.Handle, this.Handle);
            API.SetParent(m.Handle, m.Handle);
            m.Show();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var ob = new ObjectBuilder();

            var o = new ObjectBuilder.DynamicResult(new Dictionary<string, object>());

            o["Bool"] = Rndm.GetBoolean().ToString().ToUpper();
            o["Double"] = Rndm.GetDouble(-9999, 9999);
            o["Int"] = Rndm.GetInt(-9999, 9999);
            o["String"] = Rndm.GetString(
                10, CharacterType.Symbol | CharacterType.UpperCase | CharacterType.LowerCase | CharacterType.Digit);

            ob.Add("alert", new Action<string>(s => MessageBox.Show(s)));
            ob.Add("Random", o);
            dynamic b = ob.Build();

            b.alert(b.Random.Bool.ToString());
        }

        private void button16_Click(object sender, EventArgs e)
        {
            dynamic tc = Functions.trycatch(
                () =>
                    {
                        return "Erfolgreich";
                        throw new Exception("gjfhntjn");
                    });

            MessageBox.Show(tc.hasException ? tc.Exception : tc.Result);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Expression l = "(3+3)";
            MessageBox.Show(l);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            var xhr = new XmlHttpRequest();

            xhr.OnReadyStateChange = () => MessageBox.Show(xhr.ResponseText);

            xhr.Open("GET", @"http://www.google.de/search?q=c#");
            xhr.Send(null);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            try
            {
                this.TheConditionsWay(0, null, DayOfWeek.Tuesday);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //to implement () f(x)
            var vars = new[] { new Var("x", 5), new Var("y", 10) };

            Expression unary = Expression.Parse("hello(5)*1", vars);
            MessageBox.Show(unary);
        }

        private void button20_Click(object sender, EventArgs e)
        {
        }

        private void button21_Click(object sender, EventArgs e)
        {
            this.infoLabel1.Text = "Hallo Welt";
            this.infoLabel1.Show();
        }

        private void button22_Click(object sender, EventArgs e)
        {
        }

        private void button23_Click(object sender, EventArgs e)
        {
            var r = (Range)"[10-15]";
            bool rV = r.ContainsValue(11);
        }

        private void button24_Click(object sender, EventArgs e)
        {
            //KeySecure.Lock(".test", "12356c");
            bool ks = KeySecure.HasLock(".test");
        }

        private void button25_Click(object sender, EventArgs e)
        {
            Global.Add("hello", new Range(0, 1001));
            Global.Add(
                "OnChange", new Event<string> { Handlers = new[] { new Action<string>(s => MessageBox.Show(s)) } });

            var r = Global.Get<Range>("hello");
            var ee = Global.Get<Event<string>>("OnChange");
            ee.Invoke(r.ToString());

            BinaryRuntime.Add(new RangeBinary());

            var bw = new Writer(new FS("hello.binary"));
            bw.Write(r);
            bw.Close();

            var br = new Reader(new FS("hello.binary"));
            var rR = br.Read<Range>();
            br.Close();
        }

        private void button26_Click(object sender, EventArgs e)
        {
        }

        private void button27_Click(object sender, EventArgs e)
        {
            /*
            var t = new Transition(new TransitionType_Linear(500));
            t.add(this, "BackColor", Color.Green);
            t.run();

            Transition xml =
                Transition.ParseXml(
                    "<transition type='linear' time='50000'><property type='color' name='BackColor' value='hsla(237, 58, 29, 1)' /></transition>",
                    this);
            xml.run();*/
        }

        private void button28_Click(object sender, EventArgs e)
        {
            var f = new Function();
            //f.Parse();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            Message m = Message.Create("creekTest", "hallo welt ich bin ein test");
            m.Send();
            Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var r = new Range(1, 15);
            foreach (var rr in r)
            {
                this.richTextBox1.Text += rr + "\r";
            }
            if (r.InRange(15))
            {
                this.projectProperties1.Visible = false;
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            var p = new FacebookPopup();
            p.Show();
        }

        private void button31_Click(object sender, EventArgs e)
        {
            var f = new Loader();
            this.button31.Font = f[Fonts.TheKingQueen];
            IEnumerable<string> fn = f.GetFamilyNames();
            f.Extract("hello.ttf", Fonts.Stilts);
        }

        private void button32_Click(object sender, EventArgs e)
        {
            var f = new Form2();
            f.Show();
        }

        private void button33_Click(object sender, EventArgs e)
        {
            string roman = "VI";

            var context = new Context(roman);

            // Build the 'parse tree'

            var tree = new List<RomanExpression>
                { new ThousandExpression(), new HundredExpression(), new TenExpression(), new OneExpression() };

            // Interpret
            foreach (var exp in tree)
            {
                exp.Interpret(context);
            }

            MessageBox.Show(roman + " = " + context.Output);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (var n in Numbers.GetNames())
            {
                this.richTextBox1.Text += n + "\r";
            }
            if (!Numbers.isDefined("Nine", false))
            {
                MessageBox.Show("Nine is not in Enum Numbers");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            const string input = @"Hello, $(IF $loggedIn $username 'Unknown')!
You are $(IF $loggedIn $(IF $(ISMATCH $role '(admin)|(root)') 'a superuser' 'a normal user') 'not registered')!";

            var sf = new StringFormatter();

            sf.Variables.Add("loggedin", "false");
            sf.Variables.Add("username", "filmee24");
            sf.Variables.Add("role", "user");

            MessageBox.Show(sf.Format(input));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var codew = new CodeWindow();
            codew.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Extensions.IIf(this.button6.Text.Length < 10, "I`m low", "I`m high"));
        }

        private void button7_Click(object sender, EventArgs e)
        {
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.CreateParser();

            this.m_terminalReader.Open("5*5+3*9");
            var result = (Start)this.m_parser.Parse(this.m_terminalReader);
            if (result != null)
            {
                MessageBox.Show(result.Value.ToString());
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
        }

        private void commandLink2_Click(object sender, EventArgs e)
        {
        }

        private void commandLink3_Click(object sender, EventArgs e)
        {
            var vfs = new FileSystem(Application.StartupPath + "\\test.vfs");
            vfs.AddFile("info.json", "hello world", "");
            vfs.Save();
            MessageBox.Show(vfs.GetFile("info.json").Content);
        }

        private void commandLink4_Click(object sender, EventArgs e)
        {
        }

        private void commandLink5_Click(object sender, EventArgs e)
        {
            Pwd.Credentials cred = Pwd.askCred("Creek Login", "Please Login to use Creek");
            if (cred.Success)
            {
                if (cred.Username == "filmee24" && cred.Password == "123456c")
                {
                    MessageBox.Show("logged in :D");
                }
            }
        }

        private void commandLink6_Click(object sender, EventArgs e)
        {
            foreach (var p in
                Instance.FromRegex(@"(?<vorname>\w+)\s+(?<nachname>\w+)\s+\((?<age>\d+)\)", "Max Mustermann (23)"))
            {
                MessageBox.Show(p.Vorname);
                MessageBox.Show(p.Nachname);
                MessageBox.Show(p.Age.ToString());
            }
        }

        private void commandLink7_Click(object sender, EventArgs e)
        {
            try
            {
                var exception =
                    new IOException(
                        "Unable to establish a connection with the Foo bank account service. The error number is #FFF474678.",
                        new Exception(
                            "This is an Inner Exception message - with a message that is not too small but perhaps it should be smaller"));
                throw exception;
            }
            catch (Exception exception)
            {
                var exceptionReporter = new ExceptionReporter();

                exceptionReporter.Config.ShowConfigTab = false;
                exceptionReporter.Config.ShowAssembliesTab = false;
                exceptionReporter.Config.MailMethod = ExceptionReportInfo.EmailMethod.SimpleMAPI;
                exceptionReporter.Config.EmailReportAddress = "filmee24@gmail.com";
                exceptionReporter.Config.ShowSysInfoTab = false;
                exceptionReporter.Config.ShowExceptionsTab = false;

                exceptionReporter.Show(exception);
            }
        }

        private void infoLabel1_Click(object sender, EventArgs e)
        {
            this.infoLabel1.Hide();
        }

        private void quickMouseMenuUC1_QuickMenuItemClicked(object sender, MouseEventArgs e)
        {
            MessageBox.Show((sender as QuickMouseMenuItem).Caption + " CLICKED");
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Keyboard.IsKeyDown(Keys.Escape))
            {
                Application.Exit();
            }
        }

        private void toolbar1_Clicked(int selectedIndex)
        {
            this.quickMouseMenuUC1.ShowMenu(this.angleAltitudeSelector1.Location);
        }

        private void travelButton1_ItemClicked(object sender, TravelButtonItemClickedEventArgs e)
        {
        }

        #endregion

        public class Person
        {
            #region Public Properties

            public int Age { get; set; }

            public Gender Gender { get; set; }

            public string Nachname { get; set; }

            public string Vorname { get; set; }

            #endregion
        }

        private class RangeBinary : Binary<Range>
        {
            #region Public Methods and Operators

            public override Range OnRead(Reader br)
            {
                return new Range(br.Read<int>(), br.Read<int>());
            }

            public override void OnWrite(Writer bw, Range value)
            {
                bw.Write(value.Start);
                bw.Write(value.End);
            }

            #endregion
        }

        private void button34_Click(object sender, EventArgs e)
        {
            var l = new List<int>();
            l.Add(1);
            l.Add(2);
            l.Add(3);
            l.Add(4);

            var it = new DefaultAggregate<int>(l.ToArray()).CreateIterator();
            while (it.IsDone())
            {
                MessageBox.Show(it.Next().ToString());
            }
        }
    }
}