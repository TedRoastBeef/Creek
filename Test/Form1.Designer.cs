using System.Windows.Forms;
using Creek.UI;
using Creek.UI.Angle;
using Creek.UI.Mozbar;
using Creek.UI.Vista;

namespace Test
{
    partial class Form1
    {

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            Creek.UI.Toolbar.ColorsTemp colorsTemp1 = new Creek.UI.Toolbar.ColorsTemp();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.button18 = new System.Windows.Forms.Button();
            this.button19 = new System.Windows.Forms.Button();
            this.button20 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.button22 = new System.Windows.Forms.Button();
            this.button24 = new System.Windows.Forms.Button();
            this.button23 = new System.Windows.Forms.Button();
            this.button25 = new System.Windows.Forms.Button();
            this.button26 = new System.Windows.Forms.Button();
            this.button27 = new System.Windows.Forms.Button();
            this.button28 = new System.Windows.Forms.Button();
            this.button29 = new System.Windows.Forms.Button();
            this.button30 = new System.Windows.Forms.Button();
            this.button31 = new System.Windows.Forms.Button();
            this.button32 = new System.Windows.Forms.Button();
            this.button33 = new System.Windows.Forms.Button();
            this.cultureComboBox1 = new Creek.UI.CultureComboBox();
            this.metroButton1 = new Creek.UI.Metro.Controls.MetroButton();
            this.formTitleBarControl1 = new Creek.UI.Titlebar.FormTitleBarControl();
            this.firefoxDialog1 = new Creek.UI.MozOptions.FirefoxDialog();
            this.infoLabel1 = new Creek.UI.InfoLabel();
            this.projectProperties1 = new Creek.UI.Options.ProjectProperties();
            this.commandLink7 = new Creek.UI.Vista.CommandLink();
            this.boxSlider1 = new Creek.UI.BoxSlider();
            this.editableLabel1 = new Creek.UI.EditableLabel();
            this.commandLink6 = new Creek.UI.Vista.CommandLink();
            this.commandLink5 = new Creek.UI.Vista.CommandLink();
            this.groupedComboBox1 = new Creek.UI.GroupedComboBox();
            this.commandLink4 = new Creek.UI.Vista.CommandLink();
            this.commandLink3 = new Creek.UI.Vista.CommandLink();
            this.commandLink2 = new Creek.UI.Vista.CommandLink();
            this.travelButton1 = new Creek.UI.TravelButton();
            this.ratingBar1 = new Creek.UI.RatingBar();
            this.angleAltitudeSelector1 = new Creek.UI.Angle.AngleAltitudeSelector();
            this.angleSelector1 = new Creek.UI.Angle.AngleSelector();
            this.toolbar1 = new Creek.UI.Toolbar();
            this.donationButton1 = new Creek.UI.DonationButton();
            this.mozItem1 = new Creek.UI.Mozbar.MozItem();
            this.mozItem2 = new Creek.UI.Mozbar.MozItem();
            this.listCombo1 = new Test.ListCombo();
            this.button34 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.donationButton1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(196, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Calc";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(12, 31);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 2;
            this.button3.Text = "enums";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 61);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(259, 197);
            this.richTextBox1.TabIndex = 3;
            this.richTextBox1.Text = "";
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(104, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "range";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ich.jpg");
            this.imageList1.Images.SetKeyName(1, "IMG_20130826_215139.jpg");
            this.imageList1.Images.SetKeyName(2, "IMG_20130826_215225.jpg");
            this.imageList1.Images.SetKeyName(3, "logo.jpg");
            this.imageList1.Images.SetKeyName(4, "reload.png");
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(757, 140);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 30;
            this.button4.Text = "formatter";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(649, 30);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 38;
            this.button5.Text = "js";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(548, 31);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 42;
            this.button6.Text = "iff";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(337, 94);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 44;
            this.button7.Text = "keyboard";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(657, 171);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 48;
            this.button8.Text = "parser";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(337, 72);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 50;
            this.button9.Text = "dialog";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(649, 212);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 52;
            this.button10.Text = "DynamicObject";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(576, 194);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(75, 23);
            this.button11.TabIndex = 54;
            this.button11.Text = "random";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(374, 219);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 23);
            this.button12.TabIndex = 56;
            this.button12.Text = "SimulateKey";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(277, 135);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(75, 23);
            this.button13.TabIndex = 58;
            this.button13.Text = "HTML";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(337, 47);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(75, 23);
            this.button14.TabIndex = 60;
            this.button14.Text = "Map";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(594, 93);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 23);
            this.button15.TabIndex = 62;
            this.button15.Text = "objectbuilder";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(311, 194);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(75, 23);
            this.button16.TabIndex = 64;
            this.button16.Text = "trycatch";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(277, 5);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(75, 23);
            this.button17.TabIndex = 65;
            this.button17.Text = "expression";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(733, 31);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(127, 23);
            this.button18.TabIndex = 66;
            this.button18.Text = "XmlHttpRequest, Send";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(715, 93);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(157, 23);
            this.button19.TabIndex = 67;
            this.button19.Text = "condition";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // button20
            // 
            this.button20.Location = new System.Drawing.Point(277, 165);
            this.button20.Name = "button20";
            this.button20.Size = new System.Drawing.Size(75, 23);
            this.button20.TabIndex = 68;
            this.button20.Text = "FileCommand";
            this.button20.UseVisualStyleBackColor = true;
            this.button20.Click += new System.EventHandler(this.button20_Click);
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(320, 264);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(75, 23);
            this.button21.TabIndex = 71;
            this.button21.Text = "info";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // button22
            // 
            this.button22.Location = new System.Drawing.Point(406, 150);
            this.button22.Name = "button22";
            this.button22.Size = new System.Drawing.Size(75, 23);
            this.button22.TabIndex = 73;
            this.button22.Text = "db";
            this.button22.UseVisualStyleBackColor = true;
            this.button22.Click += new System.EventHandler(this.button22_Click);
            // 
            // button24
            // 
            this.button24.Location = new System.Drawing.Point(458, 276);
            this.button24.Name = "button24";
            this.button24.Size = new System.Drawing.Size(75, 23);
            this.button24.TabIndex = 75;
            this.button24.Text = "usblock";
            this.button24.UseVisualStyleBackColor = true;
            this.button24.Click += new System.EventHandler(this.button24_Click);
            // 
            // button23
            // 
            this.button23.Location = new System.Drawing.Point(277, 218);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(75, 23);
            this.button23.TabIndex = 76;
            this.button23.Text = "range";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.button23_Click);
            // 
            // button25
            // 
            this.button25.Location = new System.Drawing.Point(558, 276);
            this.button25.Name = "button25";
            this.button25.Size = new System.Drawing.Size(75, 23);
            this.button25.TabIndex = 77;
            this.button25.Text = "Global";
            this.button25.UseVisualStyleBackColor = true;
            this.button25.Click += new System.EventHandler(this.button25_Click);
            // 
            // button26
            // 
            this.button26.Location = new System.Drawing.Point(539, 350);
            this.button26.Name = "button26";
            this.button26.Size = new System.Drawing.Size(75, 23);
            this.button26.TabIndex = 78;
            this.button26.Text = "mq";
            this.button26.UseVisualStyleBackColor = true;
            this.button26.Click += new System.EventHandler(this.button26_Click);
            // 
            // button27
            // 
            this.button27.Location = new System.Drawing.Point(745, 276);
            this.button27.Name = "button27";
            this.button27.Size = new System.Drawing.Size(75, 23);
            this.button27.TabIndex = 80;
            this.button27.Text = "animation";
            this.button27.UseVisualStyleBackColor = true;
            this.button27.Click += new System.EventHandler(this.button27_Click);
            // 
            // button28
            // 
            this.button28.Location = new System.Drawing.Point(769, 332);
            this.button28.Name = "button28";
            this.button28.Size = new System.Drawing.Size(75, 23);
            this.button28.TabIndex = 81;
            this.button28.Text = "XML Func";
            this.button28.UseVisualStyleBackColor = true;
            this.button28.Click += new System.EventHandler(this.button28_Click);
            // 
            // button29
            // 
            this.button29.Location = new System.Drawing.Point(673, 360);
            this.button29.Name = "button29";
            this.button29.Size = new System.Drawing.Size(75, 23);
            this.button29.TabIndex = 82;
            this.button29.Text = "message";
            this.button29.UseVisualStyleBackColor = true;
            this.button29.Click += new System.EventHandler(this.button29_Click);
            // 
            // button30
            // 
            this.button30.Location = new System.Drawing.Point(715, 441);
            this.button30.Name = "button30";
            this.button30.Size = new System.Drawing.Size(75, 23);
            this.button30.TabIndex = 83;
            this.button30.Text = "popup";
            this.button30.UseVisualStyleBackColor = true;
            this.button30.Click += new System.EventHandler(this.button30_Click);
            // 
            // button31
            // 
            this.button31.Location = new System.Drawing.Point(813, 441);
            this.button31.Name = "button31";
            this.button31.Size = new System.Drawing.Size(75, 23);
            this.button31.TabIndex = 84;
            this.button31.Text = "font";
            this.button31.UseVisualStyleBackColor = true;
            this.button31.Click += new System.EventHandler(this.button31_Click);
            // 
            // button32
            // 
            this.button32.Location = new System.Drawing.Point(715, 470);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(75, 23);
            this.button32.TabIndex = 85;
            this.button32.Text = "navigator";
            this.button32.UseVisualStyleBackColor = true;
            this.button32.Click += new System.EventHandler(this.button32_Click);
            // 
            // button33
            // 
            this.button33.Location = new System.Drawing.Point(715, 499);
            this.button33.Name = "button33";
            this.button33.Size = new System.Drawing.Size(75, 23);
            this.button33.TabIndex = 88;
            this.button33.Text = "roman";
            this.button33.UseVisualStyleBackColor = true;
            this.button33.Click += new System.EventHandler(this.button33_Click);
            // 
            // cultureComboBox1
            // 
            this.cultureComboBox1.FormattingEnabled = true;
            this.cultureComboBox1.Items.AddRange(new object[] {
            "",
            "af-ZA",
            "af-ZA",
            "am-ET",
            "am-ET",
            "ar-AE",
            "ar-BH",
            "ar-DZ",
            "ar-EG",
            "ar-IQ",
            "ar-JO",
            "ar-KW",
            "ar-LB",
            "ar-LY",
            "ar-MA",
            "arn-CL",
            "arn-CL",
            "ar-OM",
            "ar-QA",
            "ar-SA",
            "ar-SA",
            "ar-SY",
            "ar-TN",
            "ar-YE",
            "as-IN",
            "as-IN",
            "az-Cyrl-AZ",
            "az-Cyrl-AZ",
            "az-Latn-AZ",
            "az-Latn-AZ",
            "az-Latn-AZ",
            "ba-RU",
            "ba-RU",
            "be-BY",
            "be-BY",
            "bg-BG",
            "bg-BG",
            "bn-BD",
            "bn-IN",
            "bn-IN",
            "bo-CN",
            "bo-CN",
            "br-FR",
            "br-FR",
            "bs-Cyrl-BA",
            "bs-Cyrl-BA",
            "bs-Latn-BA",
            "bs-Latn-BA",
            "bs-Latn-BA",
            "ca-ES",
            "ca-ES",
            "ca-ES-valencia",
            "chr-Cher-US",
            "chr-Cher-US",
            "chr-Cher-US",
            "co-FR",
            "co-FR",
            "cs-CZ",
            "cs-CZ",
            "cy-GB",
            "cy-GB",
            "da-DK",
            "da-DK",
            "de-AT",
            "de-CH",
            "de-DE",
            "de-DE",
            "de-LI",
            "de-LU",
            "dsb-DE",
            "dsb-DE",
            "dv-MV",
            "dv-MV",
            "el-GR",
            "el-GR",
            "en-029",
            "en-AU",
            "en-BZ",
            "en-CA",
            "en-GB",
            "en-IE",
            "en-IN",
            "en-JM",
            "en-MY",
            "en-NZ",
            "en-PH",
            "en-SG",
            "en-TT",
            "en-US",
            "en-US",
            "en-ZA",
            "en-ZW",
            "es-AR",
            "es-BO",
            "es-CL",
            "es-CO",
            "es-CR",
            "es-DO",
            "es-EC",
            "es-ES",
            "es-ES",
            "es-GT",
            "es-HN",
            "es-MX",
            "es-NI",
            "es-PA",
            "es-PE",
            "es-PR",
            "es-PY",
            "es-SV",
            "es-US",
            "es-UY",
            "es-VE",
            "et-EE",
            "et-EE",
            "eu-ES",
            "eu-ES",
            "fa-IR",
            "fa-IR",
            "ff-Latn-SN",
            "ff-Latn-SN",
            "ff-Latn-SN",
            "fi-FI",
            "fi-FI",
            "fil-PH",
            "fil-PH",
            "fo-FO",
            "fo-FO",
            "fr-BE",
            "fr-CA",
            "fr-CH",
            "fr-FR",
            "fr-FR",
            "fr-LU",
            "fr-MC",
            "fy-NL",
            "fy-NL",
            "ga-IE",
            "ga-IE",
            "gd-GB",
            "gd-GB",
            "gl-ES",
            "gl-ES",
            "gsw-FR",
            "gsw-FR",
            "gu-IN",
            "gu-IN",
            "ha-Latn-NG",
            "ha-Latn-NG",
            "ha-Latn-NG",
            "haw-US",
            "haw-US",
            "he-IL",
            "he-IL",
            "hi-IN",
            "hi-IN",
            "hr-BA",
            "hr-HR",
            "hr-HR",
            "hsb-DE",
            "hsb-DE",
            "hu-HU",
            "hu-HU",
            "hy-AM",
            "hy-AM",
            "id-ID",
            "id-ID",
            "ig-NG",
            "ig-NG",
            "ii-CN",
            "ii-CN",
            "is-IS",
            "is-IS",
            "it-CH",
            "it-IT",
            "it-IT",
            "iu-Cans-CA",
            "iu-Cans-CA",
            "iu-Latn-CA",
            "iu-Latn-CA",
            "iu-Latn-CA",
            "ja-JP",
            "ja-JP",
            "ka-GE",
            "ka-GE",
            "kk-KZ",
            "kk-KZ",
            "kl-GL",
            "kl-GL",
            "km-KH",
            "km-KH",
            "kn-IN",
            "kn-IN",
            "kok-IN",
            "kok-IN",
            "ko-KR",
            "ko-KR",
            "ku-Arab-IQ",
            "ku-Arab-IQ",
            "ku-Arab-IQ",
            "ky-KG",
            "ky-KG",
            "lb-LU",
            "lb-LU",
            "lo-LA",
            "lo-LA",
            "lt-LT",
            "lt-LT",
            "lv-LV",
            "lv-LV",
            "mi-NZ",
            "mi-NZ",
            "mk-MK",
            "mk-MK",
            "ml-IN",
            "ml-IN",
            "mn-MN",
            "mn-MN",
            "mn-MN",
            "mn-Mong-CN",
            "mn-Mong-CN",
            "moh-CA",
            "moh-CA",
            "mr-IN",
            "mr-IN",
            "ms-BN",
            "ms-MY",
            "ms-MY",
            "mt-MT",
            "mt-MT",
            "nb-NO",
            "nb-NO",
            "nb-NO",
            "ne-NP",
            "ne-NP",
            "nl-BE",
            "nl-NL",
            "nl-NL",
            "nn-NO",
            "nn-NO",
            "nso-ZA",
            "nso-ZA",
            "oc-FR",
            "oc-FR",
            "or-IN",
            "or-IN",
            "pa-Arab-PK",
            "pa-Arab-PK",
            "pa-IN",
            "pa-IN",
            "pl-PL",
            "pl-PL",
            "prs-AF",
            "prs-AF",
            "ps-AF",
            "ps-AF",
            "pt-BR",
            "pt-BR",
            "pt-PT",
            "qut-GT",
            "qut-GT",
            "quz-BO",
            "quz-BO",
            "quz-EC",
            "quz-PE",
            "rm-CH",
            "rm-CH",
            "ro-RO",
            "ro-RO",
            "ru-RU",
            "ru-RU",
            "rw-RW",
            "rw-RW",
            "sah-RU",
            "sah-RU",
            "sa-IN",
            "sa-IN",
            "sd-Arab-PK",
            "sd-Arab-PK",
            "sd-Arab-PK",
            "se-FI",
            "se-NO",
            "se-NO",
            "se-SE",
            "si-LK",
            "si-LK",
            "sk-SK",
            "sk-SK",
            "sl-SI",
            "sl-SI",
            "sma-NO",
            "sma-SE",
            "sma-SE",
            "smj-NO",
            "smj-SE",
            "smj-SE",
            "smn-FI",
            "smn-FI",
            "sms-FI",
            "sms-FI",
            "sq-AL",
            "sq-AL",
            "sr-Cyrl-BA",
            "sr-Cyrl-CS",
            "sr-Cyrl-ME",
            "sr-Cyrl-RS",
            "sr-Cyrl-RS",
            "sr-Latn-BA",
            "sr-Latn-CS",
            "sr-Latn-ME",
            "sr-Latn-RS",
            "sr-Latn-RS",
            "sr-Latn-RS",
            "sv-FI",
            "sv-SE",
            "sv-SE",
            "sw-KE",
            "sw-KE",
            "syr-SY",
            "syr-SY",
            "ta-IN",
            "ta-IN",
            "ta-LK",
            "te-IN",
            "te-IN",
            "tg-Cyrl-TJ",
            "tg-Cyrl-TJ",
            "tg-Cyrl-TJ",
            "th-TH",
            "th-TH",
            "ti-ER",
            "ti-ER",
            "ti-ET",
            "tk-TM",
            "tk-TM",
            "tn-BW",
            "tn-ZA",
            "tn-ZA",
            "tr-TR",
            "tr-TR",
            "tt-RU",
            "tt-RU",
            "tzm-Latn-DZ",
            "tzm-Latn-DZ",
            "tzm-Latn-DZ",
            "tzm-Tfng-MA",
            "tzm-Tfng-MA",
            "ug-CN",
            "ug-CN",
            "uk-UA",
            "uk-UA",
            "ur-PK",
            "ur-PK",
            "uz-Cyrl-UZ",
            "uz-Cyrl-UZ",
            "uz-Latn-UZ",
            "uz-Latn-UZ",
            "uz-Latn-UZ",
            "vi-VN",
            "vi-VN",
            "wo-SN",
            "wo-SN",
            "xh-ZA",
            "xh-ZA",
            "yo-NG",
            "yo-NG",
            "zh-CN",
            "zh-CN",
            "zh-CN",
            "zh-CN",
            "zh-HK",
            "zh-HK",
            "zh-HK",
            "zh-MO",
            "zh-SG",
            "zh-TW",
            "zu-ZA",
            "zu-ZA",
            "",
            "af-ZA",
            "af-ZA",
            "am-ET",
            "am-ET",
            "ar-AE",
            "ar-BH",
            "ar-DZ",
            "ar-EG",
            "ar-IQ",
            "ar-JO",
            "ar-KW",
            "ar-LB",
            "ar-LY",
            "ar-MA",
            "arn-CL",
            "arn-CL",
            "ar-OM",
            "ar-QA",
            "ar-SA",
            "ar-SA",
            "ar-SY",
            "ar-TN",
            "ar-YE",
            "as-IN",
            "as-IN",
            "az-Cyrl-AZ",
            "az-Cyrl-AZ",
            "az-Latn-AZ",
            "az-Latn-AZ",
            "az-Latn-AZ",
            "ba-RU",
            "ba-RU",
            "be-BY",
            "be-BY",
            "bg-BG",
            "bg-BG",
            "bn-BD",
            "bn-IN",
            "bn-IN",
            "bo-CN",
            "bo-CN",
            "br-FR",
            "br-FR",
            "bs-Cyrl-BA",
            "bs-Cyrl-BA",
            "bs-Latn-BA",
            "bs-Latn-BA",
            "bs-Latn-BA",
            "ca-ES",
            "ca-ES",
            "ca-ES-valencia",
            "chr-Cher-US",
            "chr-Cher-US",
            "chr-Cher-US",
            "co-FR",
            "co-FR",
            "cs-CZ",
            "cs-CZ",
            "cy-GB",
            "cy-GB",
            "da-DK",
            "da-DK",
            "de-AT",
            "de-CH",
            "de-DE",
            "de-DE",
            "de-LI",
            "de-LU",
            "dsb-DE",
            "dsb-DE",
            "dv-MV",
            "dv-MV",
            "el-GR",
            "el-GR",
            "en-029",
            "en-AU",
            "en-BZ",
            "en-CA",
            "en-GB",
            "en-IE",
            "en-IN",
            "en-JM",
            "en-MY",
            "en-NZ",
            "en-PH",
            "en-SG",
            "en-TT",
            "en-US",
            "en-US",
            "en-ZA",
            "en-ZW",
            "es-AR",
            "es-BO",
            "es-CL",
            "es-CO",
            "es-CR",
            "es-DO",
            "es-EC",
            "es-ES",
            "es-ES",
            "es-GT",
            "es-HN",
            "es-MX",
            "es-NI",
            "es-PA",
            "es-PE",
            "es-PR",
            "es-PY",
            "es-SV",
            "es-US",
            "es-UY",
            "es-VE",
            "et-EE",
            "et-EE",
            "eu-ES",
            "eu-ES",
            "fa-IR",
            "fa-IR",
            "ff-Latn-SN",
            "ff-Latn-SN",
            "ff-Latn-SN",
            "fi-FI",
            "fi-FI",
            "fil-PH",
            "fil-PH",
            "fo-FO",
            "fo-FO",
            "fr-BE",
            "fr-CA",
            "fr-CH",
            "fr-FR",
            "fr-FR",
            "fr-LU",
            "fr-MC",
            "fy-NL",
            "fy-NL",
            "ga-IE",
            "ga-IE",
            "gd-GB",
            "gd-GB",
            "gl-ES",
            "gl-ES",
            "gsw-FR",
            "gsw-FR",
            "gu-IN",
            "gu-IN",
            "ha-Latn-NG",
            "ha-Latn-NG",
            "ha-Latn-NG",
            "haw-US",
            "haw-US",
            "he-IL",
            "he-IL",
            "hi-IN",
            "hi-IN",
            "hr-BA",
            "hr-HR",
            "hr-HR",
            "hsb-DE",
            "hsb-DE",
            "hu-HU",
            "hu-HU",
            "hy-AM",
            "hy-AM",
            "id-ID",
            "id-ID",
            "ig-NG",
            "ig-NG",
            "ii-CN",
            "ii-CN",
            "is-IS",
            "is-IS",
            "it-CH",
            "it-IT",
            "it-IT",
            "iu-Cans-CA",
            "iu-Cans-CA",
            "iu-Latn-CA",
            "iu-Latn-CA",
            "iu-Latn-CA",
            "ja-JP",
            "ja-JP",
            "ka-GE",
            "ka-GE",
            "kk-KZ",
            "kk-KZ",
            "kl-GL",
            "kl-GL",
            "km-KH",
            "km-KH",
            "kn-IN",
            "kn-IN",
            "kok-IN",
            "kok-IN",
            "ko-KR",
            "ko-KR",
            "ku-Arab-IQ",
            "ku-Arab-IQ",
            "ku-Arab-IQ",
            "ky-KG",
            "ky-KG",
            "lb-LU",
            "lb-LU",
            "lo-LA",
            "lo-LA",
            "lt-LT",
            "lt-LT",
            "lv-LV",
            "lv-LV",
            "mi-NZ",
            "mi-NZ",
            "mk-MK",
            "mk-MK",
            "ml-IN",
            "ml-IN",
            "mn-MN",
            "mn-MN",
            "mn-MN",
            "mn-Mong-CN",
            "mn-Mong-CN",
            "moh-CA",
            "moh-CA",
            "mr-IN",
            "mr-IN",
            "ms-BN",
            "ms-MY",
            "ms-MY",
            "mt-MT",
            "mt-MT",
            "nb-NO",
            "nb-NO",
            "nb-NO",
            "ne-NP",
            "ne-NP",
            "nl-BE",
            "nl-NL",
            "nl-NL",
            "nn-NO",
            "nn-NO",
            "nso-ZA",
            "nso-ZA",
            "oc-FR",
            "oc-FR",
            "or-IN",
            "or-IN",
            "pa-Arab-PK",
            "pa-Arab-PK",
            "pa-IN",
            "pa-IN",
            "pl-PL",
            "pl-PL",
            "prs-AF",
            "prs-AF",
            "ps-AF",
            "ps-AF",
            "pt-BR",
            "pt-BR",
            "pt-PT",
            "qut-GT",
            "qut-GT",
            "quz-BO",
            "quz-BO",
            "quz-EC",
            "quz-PE",
            "rm-CH",
            "rm-CH",
            "ro-RO",
            "ro-RO",
            "ru-RU",
            "ru-RU",
            "rw-RW",
            "rw-RW",
            "sah-RU",
            "sah-RU",
            "sa-IN",
            "sa-IN",
            "sd-Arab-PK",
            "sd-Arab-PK",
            "sd-Arab-PK",
            "se-FI",
            "se-NO",
            "se-NO",
            "se-SE",
            "si-LK",
            "si-LK",
            "sk-SK",
            "sk-SK",
            "sl-SI",
            "sl-SI",
            "sma-NO",
            "sma-SE",
            "sma-SE",
            "smj-NO",
            "smj-SE",
            "smj-SE",
            "smn-FI",
            "smn-FI",
            "sms-FI",
            "sms-FI",
            "sq-AL",
            "sq-AL",
            "sr-Cyrl-BA",
            "sr-Cyrl-CS",
            "sr-Cyrl-ME",
            "sr-Cyrl-RS",
            "sr-Cyrl-RS",
            "sr-Latn-BA",
            "sr-Latn-CS",
            "sr-Latn-ME",
            "sr-Latn-RS",
            "sr-Latn-RS",
            "sr-Latn-RS",
            "sv-FI",
            "sv-SE",
            "sv-SE",
            "sw-KE",
            "sw-KE",
            "syr-SY",
            "syr-SY",
            "ta-IN",
            "ta-IN",
            "ta-LK",
            "te-IN",
            "te-IN",
            "tg-Cyrl-TJ",
            "tg-Cyrl-TJ",
            "tg-Cyrl-TJ",
            "th-TH",
            "th-TH",
            "ti-ER",
            "ti-ER",
            "ti-ET",
            "tk-TM",
            "tk-TM",
            "tn-BW",
            "tn-ZA",
            "tn-ZA",
            "tr-TR",
            "tr-TR",
            "tt-RU",
            "tt-RU",
            "tzm-Latn-DZ",
            "tzm-Latn-DZ",
            "tzm-Latn-DZ",
            "tzm-Tfng-MA",
            "tzm-Tfng-MA",
            "ug-CN",
            "ug-CN",
            "uk-UA",
            "uk-UA",
            "ur-PK",
            "ur-PK",
            "uz-Cyrl-UZ",
            "uz-Cyrl-UZ",
            "uz-Latn-UZ",
            "uz-Latn-UZ",
            "uz-Latn-UZ",
            "vi-VN",
            "vi-VN",
            "wo-SN",
            "wo-SN",
            "xh-ZA",
            "xh-ZA",
            "yo-NG",
            "yo-NG",
            "zh-CN",
            "zh-CN",
            "zh-CN",
            "zh-CN",
            "zh-HK",
            "zh-HK",
            "zh-HK",
            "zh-MO",
            "zh-SG",
            "zh-TW",
            "zu-ZA",
            "zu-ZA",
            "",
            "af-ZA",
            "af-ZA",
            "am-ET",
            "am-ET",
            "ar-AE",
            "ar-BH",
            "ar-DZ",
            "ar-EG",
            "ar-IQ",
            "ar-JO",
            "ar-KW",
            "ar-LB",
            "ar-LY",
            "ar-MA",
            "arn-CL",
            "arn-CL",
            "ar-OM",
            "ar-QA",
            "ar-SA",
            "ar-SA",
            "ar-SY",
            "ar-TN",
            "ar-YE",
            "as-IN",
            "as-IN",
            "az-Cyrl-AZ",
            "az-Cyrl-AZ",
            "az-Latn-AZ",
            "az-Latn-AZ",
            "az-Latn-AZ",
            "ba-RU",
            "ba-RU",
            "be-BY",
            "be-BY",
            "bg-BG",
            "bg-BG",
            "bn-BD",
            "bn-IN",
            "bn-IN",
            "bo-CN",
            "bo-CN",
            "br-FR",
            "br-FR",
            "bs-Cyrl-BA",
            "bs-Cyrl-BA",
            "bs-Latn-BA",
            "bs-Latn-BA",
            "bs-Latn-BA",
            "ca-ES",
            "ca-ES",
            "ca-ES-valencia",
            "chr-Cher-US",
            "chr-Cher-US",
            "chr-Cher-US",
            "co-FR",
            "co-FR",
            "cs-CZ",
            "cs-CZ",
            "cy-GB",
            "cy-GB",
            "da-DK",
            "da-DK",
            "de-AT",
            "de-CH",
            "de-DE",
            "de-DE",
            "de-LI",
            "de-LU",
            "dsb-DE",
            "dsb-DE",
            "dv-MV",
            "dv-MV",
            "el-GR",
            "el-GR",
            "en-029",
            "en-AU",
            "en-BZ",
            "en-CA",
            "en-GB",
            "en-IE",
            "en-IN",
            "en-JM",
            "en-MY",
            "en-NZ",
            "en-PH",
            "en-SG",
            "en-TT",
            "en-US",
            "en-US",
            "en-ZA",
            "en-ZW",
            "es-AR",
            "es-BO",
            "es-CL",
            "es-CO",
            "es-CR",
            "es-DO",
            "es-EC",
            "es-ES",
            "es-ES",
            "es-GT",
            "es-HN",
            "es-MX",
            "es-NI",
            "es-PA",
            "es-PE",
            "es-PR",
            "es-PY",
            "es-SV",
            "es-US",
            "es-UY",
            "es-VE",
            "et-EE",
            "et-EE",
            "eu-ES",
            "eu-ES",
            "fa-IR",
            "fa-IR",
            "ff-Latn-SN",
            "ff-Latn-SN",
            "ff-Latn-SN",
            "fi-FI",
            "fi-FI",
            "fil-PH",
            "fil-PH",
            "fo-FO",
            "fo-FO",
            "fr-BE",
            "fr-CA",
            "fr-CH",
            "fr-FR",
            "fr-FR",
            "fr-LU",
            "fr-MC",
            "fy-NL",
            "fy-NL",
            "ga-IE",
            "ga-IE",
            "gd-GB",
            "gd-GB",
            "gl-ES",
            "gl-ES",
            "gsw-FR",
            "gsw-FR",
            "gu-IN",
            "gu-IN",
            "ha-Latn-NG",
            "ha-Latn-NG",
            "ha-Latn-NG",
            "haw-US",
            "haw-US",
            "he-IL",
            "he-IL",
            "hi-IN",
            "hi-IN",
            "hr-BA",
            "hr-HR",
            "hr-HR",
            "hsb-DE",
            "hsb-DE",
            "hu-HU",
            "hu-HU",
            "hy-AM",
            "hy-AM",
            "id-ID",
            "id-ID",
            "ig-NG",
            "ig-NG",
            "ii-CN",
            "ii-CN",
            "is-IS",
            "is-IS",
            "it-CH",
            "it-IT",
            "it-IT",
            "iu-Cans-CA",
            "iu-Cans-CA",
            "iu-Latn-CA",
            "iu-Latn-CA",
            "iu-Latn-CA",
            "ja-JP",
            "ja-JP",
            "ka-GE",
            "ka-GE",
            "kk-KZ",
            "kk-KZ",
            "kl-GL",
            "kl-GL",
            "km-KH",
            "km-KH",
            "kn-IN",
            "kn-IN",
            "kok-IN",
            "kok-IN",
            "ko-KR",
            "ko-KR",
            "ku-Arab-IQ",
            "ku-Arab-IQ",
            "ku-Arab-IQ",
            "ky-KG",
            "ky-KG",
            "lb-LU",
            "lb-LU",
            "lo-LA",
            "lo-LA",
            "lt-LT",
            "lt-LT",
            "lv-LV",
            "lv-LV",
            "mi-NZ",
            "mi-NZ",
            "mk-MK",
            "mk-MK",
            "ml-IN",
            "ml-IN",
            "mn-MN",
            "mn-MN",
            "mn-MN",
            "mn-Mong-CN",
            "mn-Mong-CN",
            "moh-CA",
            "moh-CA",
            "mr-IN",
            "mr-IN",
            "ms-BN",
            "ms-MY",
            "ms-MY",
            "mt-MT",
            "mt-MT",
            "nb-NO",
            "nb-NO",
            "nb-NO",
            "ne-NP",
            "ne-NP",
            "nl-BE",
            "nl-NL",
            "nl-NL",
            "nn-NO",
            "nn-NO",
            "nso-ZA",
            "nso-ZA",
            "oc-FR",
            "oc-FR",
            "or-IN",
            "or-IN",
            "pa-Arab-PK",
            "pa-Arab-PK",
            "pa-IN",
            "pa-IN",
            "pl-PL",
            "pl-PL",
            "prs-AF",
            "prs-AF",
            "ps-AF",
            "ps-AF",
            "pt-BR",
            "pt-BR",
            "pt-PT",
            "qut-GT",
            "qut-GT",
            "quz-BO",
            "quz-BO",
            "quz-EC",
            "quz-PE",
            "rm-CH",
            "rm-CH",
            "ro-RO",
            "ro-RO",
            "ru-RU",
            "ru-RU",
            "rw-RW",
            "rw-RW",
            "sah-RU",
            "sah-RU",
            "sa-IN",
            "sa-IN",
            "sd-Arab-PK",
            "sd-Arab-PK",
            "sd-Arab-PK",
            "se-FI",
            "se-NO",
            "se-NO",
            "se-SE",
            "si-LK",
            "si-LK",
            "sk-SK",
            "sk-SK",
            "sl-SI",
            "sl-SI",
            "sma-NO",
            "sma-SE",
            "sma-SE",
            "smj-NO",
            "smj-SE",
            "smj-SE",
            "smn-FI",
            "smn-FI",
            "sms-FI",
            "sms-FI",
            "sq-AL",
            "sq-AL",
            "sr-Cyrl-BA",
            "sr-Cyrl-CS",
            "sr-Cyrl-ME",
            "sr-Cyrl-RS",
            "sr-Cyrl-RS",
            "sr-Latn-BA",
            "sr-Latn-CS",
            "sr-Latn-ME",
            "sr-Latn-RS",
            "sr-Latn-RS",
            "sr-Latn-RS",
            "sv-FI",
            "sv-SE",
            "sv-SE",
            "sw-KE",
            "sw-KE",
            "syr-SY",
            "syr-SY",
            "ta-IN",
            "ta-IN",
            "ta-LK",
            "te-IN",
            "te-IN",
            "tg-Cyrl-TJ",
            "tg-Cyrl-TJ",
            "tg-Cyrl-TJ",
            "th-TH",
            "th-TH",
            "ti-ER",
            "ti-ER",
            "ti-ET",
            "tk-TM",
            "tk-TM",
            "tn-BW",
            "tn-ZA",
            "tn-ZA",
            "tr-TR",
            "tr-TR",
            "tt-RU",
            "tt-RU",
            "tzm-Latn-DZ",
            "tzm-Latn-DZ",
            "tzm-Latn-DZ",
            "tzm-Tfng-MA",
            "tzm-Tfng-MA",
            "ug-CN",
            "ug-CN",
            "uk-UA",
            "uk-UA",
            "ur-PK",
            "ur-PK",
            "uz-Cyrl-UZ",
            "uz-Cyrl-UZ",
            "uz-Latn-UZ",
            "uz-Latn-UZ",
            "uz-Latn-UZ",
            "vi-VN",
            "vi-VN",
            "wo-SN",
            "wo-SN",
            "xh-ZA",
            "xh-ZA",
            "yo-NG",
            "yo-NG",
            "zh-CN",
            "zh-CN",
            "zh-CN",
            "zh-CN",
            "zh-HK",
            "zh-HK",
            "zh-HK",
            "zh-MO",
            "zh-SG",
            "zh-TW",
            "zu-ZA",
            "zu-ZA"});
            this.cultureComboBox1.Location = new System.Drawing.Point(733, 236);
            this.cultureComboBox1.Name = "cultureComboBox1";
            this.cultureComboBox1.Size = new System.Drawing.Size(121, 21);
            this.cultureComboBox1.TabIndex = 87;
            // 
            // metroButton1
            // 
            this.metroButton1.Location = new System.Drawing.Point(472, 404);
            this.metroButton1.Name = "metroButton1";
            this.metroButton1.Size = new System.Drawing.Size(83, 60);
            this.metroButton1.TabIndex = 86;
            this.metroButton1.Text = "metroButton1";
            this.metroButton1.UseSelectable = true;
            // 
            // formTitleBarControl1
            // 
            this.formTitleBarControl1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("formTitleBarControl1.BackgroundImage")));
            this.formTitleBarControl1.Close = true;
            this.formTitleBarControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.formTitleBarControl1.Location = new System.Drawing.Point(0, 0);
            this.formTitleBarControl1.Maximize = true;
            this.formTitleBarControl1.Minimize = true;
            this.formTitleBarControl1.Name = "formTitleBarControl1";
            this.formTitleBarControl1.Size = new System.Drawing.Size(941, 25);
            this.formTitleBarControl1.TabIndex = 79;
            this.formTitleBarControl1.Title = "Test for Controls";
            this.formTitleBarControl1.TitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.formTitleBarControl1.TitleFont = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.formTitleBarControl1.TitleForeColor = System.Drawing.Color.White;
            // 
            // firefoxDialog1
            // 
            this.firefoxDialog1.ButtonApplyText = "&Übernehmen";
            this.firefoxDialog1.ButtonCancelText = "Abbrechen";
            this.firefoxDialog1.ButtonOkText = "OK";
            this.firefoxDialog1.ImageList = this.imageList1;
            this.firefoxDialog1.Location = new System.Drawing.Point(311, 315);
            this.firefoxDialog1.Name = "firefoxDialog1";
            this.firefoxDialog1.Size = new System.Drawing.Size(322, 187);
            this.firefoxDialog1.TabIndex = 72;
            // 
            // infoLabel1
            // 
            this.infoLabel1.BackColor = System.Drawing.SystemColors.Info;
            this.infoLabel1.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.infoLabel1.Cursor = System.Windows.Forms.Cursors.Default;
            this.infoLabel1.ForeColor = System.Drawing.SystemColors.InfoText;
            this.infoLabel1.HoverBackColor = System.Drawing.SystemColors.Highlight;
            this.infoLabel1.HoverBorderColor = System.Drawing.SystemColors.ControlDark;
            this.infoLabel1.HoverForeColor = System.Drawing.SystemColors.HighlightText;
            this.infoLabel1.IconImage = global::Test.Properties.Resources.ich;
            this.infoLabel1.Location = new System.Drawing.Point(12, 264);
            this.infoLabel1.Name = "infoLabel1";
            this.infoLabel1.Size = new System.Drawing.Size(279, 186);
            this.infoLabel1.SupportHovering = true;
            this.infoLabel1.TabIndex = 70;
            this.infoLabel1.Text = "infoLabel1";
            this.infoLabel1.Visible = false;
            this.infoLabel1.Click += new System.EventHandler(this.infoLabel1_Click);
            // 
            // projectProperties1
            // 
            this.projectProperties1.AutoScroll = true;
            this.projectProperties1.Location = new System.Drawing.Point(24, 89);
            this.projectProperties1.Name = "projectProperties1";
            this.projectProperties1.Size = new System.Drawing.Size(247, 158);
            this.projectProperties1.TabIndex = 69;
            this.projectProperties1.Text = "projectProperties1";
            // 
            // commandLink7
            // 
            this.commandLink7.BackColor = System.Drawing.Color.White;
            this.commandLink7.Description = null;
            this.commandLink7.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.commandLink7.Location = new System.Drawing.Point(620, 59);
            this.commandLink7.Name = "commandLink7";
            this.commandLink7.Size = new System.Drawing.Size(112, 48);
            this.commandLink7.TabIndex = 46;
            this.commandLink7.TabStop = false;
            this.commandLink7.Text = "exception";
            this.commandLink7.UseVisualStyleBackColor = true;
            this.commandLink7.Click += new System.EventHandler(this.commandLink7_Click);
            // 
            // boxSlider1
            // 
            this.boxSlider1.BorderColor = System.Drawing.Color.Black;
            this.boxSlider1.Boxes = 2;
            this.boxSlider1.FillColor = System.Drawing.Color.Red;
            this.boxSlider1.Location = new System.Drawing.Point(548, 212);
            this.boxSlider1.Name = "boxSlider1";
            this.boxSlider1.Size = new System.Drawing.Size(150, 35);
            this.boxSlider1.TabIndex = 40;
            this.boxSlider1.Value = 0;
            // 
            // editableLabel1
            // 
            this.editableLabel1.AutoSize = true;
            this.editableLabel1.Location = new System.Drawing.Point(455, 229);
            this.editableLabel1.Name = "editableLabel1";
            this.editableLabel1.Size = new System.Drawing.Size(76, 13);
            this.editableLabel1.TabIndex = 36;
            this.editableLabel1.Text = "editableLabel1";
            // 
            // commandLink6
            // 
            this.commandLink6.BackColor = System.Drawing.Color.White;
            this.commandLink6.Description = null;
            this.commandLink6.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.commandLink6.Location = new System.Drawing.Point(512, 158);
            this.commandLink6.Name = "commandLink6";
            this.commandLink6.Size = new System.Drawing.Size(112, 48);
            this.commandLink6.TabIndex = 28;
            this.commandLink6.TabStop = false;
            this.commandLink6.Text = "shortcuts";
            this.commandLink6.UseVisualStyleBackColor = true;
            this.commandLink6.Click += new System.EventHandler(this.commandLink6_Click);
            // 
            // commandLink5
            // 
            this.commandLink5.BackColor = System.Drawing.Color.White;
            this.commandLink5.Description = null;
            this.commandLink5.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.commandLink5.Location = new System.Drawing.Point(760, 187);
            this.commandLink5.Name = "commandLink5";
            this.commandLink5.Size = new System.Drawing.Size(112, 48);
            this.commandLink5.TabIndex = 26;
            this.commandLink5.TabStop = false;
            this.commandLink5.Text = "login";
            this.commandLink5.UseVisualStyleBackColor = true;
            this.commandLink5.Click += new System.EventHandler(this.commandLink5_Click);
            // 
            // groupedComboBox1
            // 
            this.groupedComboBox1.FormattingEnabled = true;
            this.groupedComboBox1.GroupMember = "";
            this.groupedComboBox1.Location = new System.Drawing.Point(375, 123);
            this.groupedComboBox1.Name = "groupedComboBox1";
            this.groupedComboBox1.Size = new System.Drawing.Size(121, 21);
            this.groupedComboBox1.TabIndex = 24;
            // 
            // commandLink4
            // 
            this.commandLink4.BackColor = System.Drawing.Color.White;
            this.commandLink4.Description = null;
            this.commandLink4.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.commandLink4.Location = new System.Drawing.Point(410, 140);
            this.commandLink4.Name = "commandLink4";
            this.commandLink4.Size = new System.Drawing.Size(112, 48);
            this.commandLink4.TabIndex = 20;
            this.commandLink4.TabStop = false;
            this.commandLink4.Text = "Content";
            this.commandLink4.UseVisualStyleBackColor = true;
            this.commandLink4.Click += new System.EventHandler(this.commandLink4_Click);
            // 
            // commandLink3
            // 
            this.commandLink3.BackColor = System.Drawing.Color.White;
            this.commandLink3.Description = null;
            this.commandLink3.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.commandLink3.Location = new System.Drawing.Point(377, 175);
            this.commandLink3.Name = "commandLink3";
            this.commandLink3.Size = new System.Drawing.Size(80, 48);
            this.commandLink3.TabIndex = 18;
            this.commandLink3.TabStop = false;
            this.commandLink3.Text = "File";
            this.commandLink3.UseVisualStyleBackColor = true;
            this.commandLink3.Click += new System.EventHandler(this.commandLink3_Click);
            // 
            // commandLink2
            // 
            this.commandLink2.BackColor = System.Drawing.Color.White;
            this.commandLink2.Description = null;
            this.commandLink2.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.commandLink2.Location = new System.Drawing.Point(512, 39);
            this.commandLink2.Name = "commandLink2";
            this.commandLink2.Size = new System.Drawing.Size(102, 48);
            this.commandLink2.TabIndex = 16;
            this.commandLink2.TabStop = false;
            this.commandLink2.Text = "theme";
            this.commandLink2.UseVisualStyleBackColor = true;
            this.commandLink2.Click += new System.EventHandler(this.commandLink2_Click);
            // 
            // travelButton1
            // 
            this.travelButton1.BackToolTip = null;
            this.travelButton1.ForwardToolTip = null;
            this.travelButton1.Location = new System.Drawing.Point(410, 59);
            this.travelButton1.Name = "travelButton1";
            this.travelButton1.Size = new System.Drawing.Size(74, 29);
            this.travelButton1.TabIndex = 13;
            this.travelButton1.TabStop = false;
            this.travelButton1.Text = "travelButton1";
            this.travelButton1.ItemClicked += new Creek.UI.TravelButtonItemClickedEventHandler(this.travelButton1_ItemClicked);
            // 
            // ratingBar1
            // 
            this.ratingBar1.BarBackColor = System.Drawing.SystemColors.Control;
            this.ratingBar1.Gap = ((byte)(2));
            this.ratingBar1.IconEmpty = ((System.Drawing.Image)(resources.GetObject("ratingBar1.IconEmpty")));
            this.ratingBar1.IconFull = ((System.Drawing.Image)(resources.GetObject("ratingBar1.IconFull")));
            this.ratingBar1.IconHalf = ((System.Drawing.Image)(resources.GetObject("ratingBar1.IconHalf")));
            this.ratingBar1.IconsCount = ((byte)(5));
            this.ratingBar1.Location = new System.Drawing.Point(410, 30);
            this.ratingBar1.Name = "ratingBar1";
            this.ratingBar1.Rate = 0F;
            this.ratingBar1.Size = new System.Drawing.Size(112, 23);
            this.ratingBar1.TabIndex = 12;
            this.ratingBar1.Text = "ratingBar1";
            // 
            // angleAltitudeSelector1
            // 
            this.angleAltitudeSelector1.Altitude = 90;
            this.angleAltitudeSelector1.Angle = 0;
            this.angleAltitudeSelector1.Location = new System.Drawing.Point(291, 89);
            this.angleAltitudeSelector1.Name = "angleAltitudeSelector1";
            this.angleAltitudeSelector1.Size = new System.Drawing.Size(40, 40);
            this.angleAltitudeSelector1.TabIndex = 8;
            // 
            // angleSelector1
            // 
            this.angleSelector1.Angle = 90;
            this.angleSelector1.Location = new System.Drawing.Point(291, 30);
            this.angleSelector1.Name = "angleSelector1";
            this.angleSelector1.Size = new System.Drawing.Size(40, 40);
            this.angleSelector1.TabIndex = 7;
            // 
            // toolbar1
            // 
            this.toolbar1.BackgroundColorDown = System.Drawing.Color.Transparent;
            this.toolbar1.BackgroundColorNormal = System.Drawing.Color.Transparent;
            this.toolbar1.BackgroundColorOver = System.Drawing.Color.Transparent;
            this.toolbar1.ContentAlignment = System.Drawing.ContentAlignment.TopCenter;
            this.toolbar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolbar1.ImageList = this.imageList1;
            this.toolbar1.LineColorDown = System.Drawing.Color.Black;
            this.toolbar1.LineColorNormal = System.Drawing.Color.Black;
            this.toolbar1.LineColorOver = System.Drawing.Color.Black;
            colorsTemp1.Down = System.Drawing.Color.Black;
            colorsTemp1.Normal = System.Drawing.Color.Black;
            colorsTemp1.Over = System.Drawing.Color.Black;
            this.toolbar1.LineColors = colorsTemp1;
            this.toolbar1.Location = new System.Drawing.Point(0, 538);
            this.toolbar1.Name = "toolbar1";
            this.toolbar1.Size = new System.Drawing.Size(941, 36);
            this.toolbar1.TabIndex = 6;
            this.toolbar1.Text = "toolbar1";
            this.toolbar1.Clicked += new Creek.UI.Toolbar.EventHandler(this.toolbar1_Clicked);
            // 
            // donationButton1
            // 
            this.donationButton1.Address = "1MkUu7TtzBVG2VZ15pm4H6CrqfdezKMmST";
            this.donationButton1.Amount = null;
            this.donationButton1.Image = ((System.Drawing.Image)(resources.GetObject("donationButton1.Image")));
            this.donationButton1.Label = null;
            this.donationButton1.Location = new System.Drawing.Point(434, 93);
            this.donationButton1.Message = null;
            this.donationButton1.Name = "donationButton1";
            this.donationButton1.Size = new System.Drawing.Size(137, 24);
            this.donationButton1.TabIndex = 4;
            this.donationButton1.TabStop = false;
            // 
            // mozItem1
            // 
            this.mozItem1.Images.Focus = -1;
            this.mozItem1.Images.FocusImage = null;
            this.mozItem1.Images.Normal = -1;
            this.mozItem1.Images.NormalImage = null;
            this.mozItem1.Images.Selected = -1;
            this.mozItem1.Images.SelectedImage = null;
            this.mozItem1.Location = new System.Drawing.Point(2, 2);
            this.mozItem1.Name = "mozItem1";
            this.mozItem1.Size = new System.Drawing.Size(40, 57);
            this.mozItem1.TabIndex = 74;
            // 
            // mozItem2
            // 
            this.mozItem2.Images.Focus = 3;
            this.mozItem2.Images.FocusImage = ((System.Drawing.Image)(resources.GetObject("resource.FocusImage")));
            this.mozItem2.Images.Normal = 2;
            this.mozItem2.Images.NormalImage = ((System.Drawing.Image)(resources.GetObject("resource.NormalImage")));
            this.mozItem2.Images.Selected = 4;
            this.mozItem2.Images.SelectedImage = ((System.Drawing.Image)(resources.GetObject("resource.SelectedImage")));
            this.mozItem2.Location = new System.Drawing.Point(2, 45);
            this.mozItem2.Name = "mozItem2";
            this.mozItem2.Size = new System.Drawing.Size(40, 57);
            this.mozItem2.TabIndex = 75;
            // 
            // listCombo1
            // 
            this.listCombo1.AnchorSize = new System.Drawing.Size(160, 21);
            this.listCombo1.BackColor = System.Drawing.Color.White;
            this.listCombo1.DockSide = Creek.UI.Unity3.Controls.DropDownControl.eDockSide.Left;
            this.listCombo1.Location = new System.Drawing.Point(548, 123);
            this.listCombo1.Name = "listCombo1";
            this.listCombo1.Size = new System.Drawing.Size(160, 21);
            this.listCombo1.TabIndex = 34;
            // 
            // button34
            // 
            this.button34.Location = new System.Drawing.Point(797, 498);
            this.button34.Name = "button34";
            this.button34.Size = new System.Drawing.Size(75, 24);
            this.button34.TabIndex = 89;
            this.button34.Text = "iterator";
            this.button34.UseVisualStyleBackColor = true;
            this.button34.Click += new System.EventHandler(this.button34_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Tan;
            this.ClientSize = new System.Drawing.Size(941, 574);
            this.Controls.Add(this.button34);
            this.Controls.Add(this.button33);
            this.Controls.Add(this.cultureComboBox1);
            this.Controls.Add(this.metroButton1);
            this.Controls.Add(this.button32);
            this.Controls.Add(this.button31);
            this.Controls.Add(this.button30);
            this.Controls.Add(this.button29);
            this.Controls.Add(this.button28);
            this.Controls.Add(this.button27);
            this.Controls.Add(this.formTitleBarControl1);
            this.Controls.Add(this.button26);
            this.Controls.Add(this.button25);
            this.Controls.Add(this.button23);
            this.Controls.Add(this.button24);
            this.Controls.Add(this.button22);
            this.Controls.Add(this.firefoxDialog1);
            this.Controls.Add(this.button21);
            this.Controls.Add(this.infoLabel1);
            this.Controls.Add(this.projectProperties1);
            this.Controls.Add(this.button20);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.commandLink7);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.boxSlider1);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.editableLabel1);
            this.Controls.Add(this.listCombo1);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.commandLink6);
            this.Controls.Add(this.commandLink5);
            this.Controls.Add(this.groupedComboBox1);
            this.Controls.Add(this.commandLink4);
            this.Controls.Add(this.commandLink3);
            this.Controls.Add(this.commandLink2);
            this.Controls.Add(this.travelButton1);
            this.Controls.Add(this.ratingBar1);
            this.Controls.Add(this.angleAltitudeSelector1);
            this.Controls.Add(this.angleSelector1);
            this.Controls.Add(this.toolbar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.donationButton1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.Text = "-";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.donationButton1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.RichTextBox richTextBox1;

        private static void Form_Closing(object sender, FormClosingEventArgs e)
        {
            
        }

        private static void reh(string address, string amount, string label)
        {
            MessageBox.Show("received");
        }

        private DonationButton donationButton1;
        private Button button2;
        private Toolbar toolbar1;
        private System.ComponentModel.IContainer components;
        private ImageList imageList1;
        private AngleSelector angleSelector1;
        private AngleAltitudeSelector angleAltitudeSelector1;
        private RatingBar ratingBar1;
        private TravelButton travelButton1;
        private CommandLink commandLink2;
        private CommandLink commandLink3;
        private CommandLink commandLink4;
        private GroupedComboBox groupedComboBox1;
        private CommandLink commandLink5;
        private CommandLink commandLink6;
        private Button button4;
        private ListCombo listCombo1;
        private EditableLabel editableLabel1;
        private Button button5;
        private BoxSlider boxSlider1;
        private Button button6;
        private Button button7;
        private CommandLink commandLink7;
        private Button button8;
        private Button button9;
        private Button button10;
        private Button button11;
        private Timer timer1;
        private Button button12;
        private Button button13;
        private Button button14;
        private Button button15;
        private Button button16;
        private Button button17;
        private Button button18;
        private Button button19;
        private Button button20;
        private Creek.UI.Options.ProjectProperties projectProperties1;
        private InfoLabel infoLabel1;
        private Button button21;
        private MozItem mozItem1;
        private MozItem mozItem2;
        private Creek.UI.MozOptions.FirefoxDialog firefoxDialog1;
        private Button button22;
        private Button button24;
        private Button button23;
        private Button button25;
        private Button button26;
        private Creek.UI.Titlebar.FormTitleBarControl formTitleBarControl1;
        private Button button27;
        private Button button28;
        private Button button29;
        private Button button30;
        private Button button31;
        private Button button32;
        private Creek.UI.Metro.Controls.MetroButton metroButton1;
        private CultureComboBox cultureComboBox1;
        private Button button33;
        private Button button34;
    }
}

