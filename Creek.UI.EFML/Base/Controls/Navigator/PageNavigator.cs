using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.EFML.Base.Controls.Navigator
{
    using Creek.UI.Winforms.EFML.Properties;

    public class PageNavigator : UserControl, IUIElement
    {
        public List<UserControl> Pages = new List<UserControl>();
        private HeadlessTabControl headlessTabControl1;
        private bool initialized;
        private Label label1;

        private TravelButton travelButton1;

        public PageNavigator()
        {
            InitializeComponent();
            BackGround = Resources.BACKGROUND_arrowless;
            NavVisible = true;
            label1.Text = "";
        }

        public Image ForwardButton
        {
            get { return travelButton1.ForwardButton; }
            set { travelButton1.ForwardButton = value; }
        }

        public Image BackGround
        {
            get { return travelButton1.BackGround; }
            set { travelButton1.BackGround = value; }
        }

        public Image BackwardButton
        {
            get { return travelButton1.BackwardButton; }
            set { travelButton1.BackwardButton = value; }
        }

        public bool NavVisible { get; set; }

        public int CurrentPage
        {
            get { return headlessTabControl1.SelectedIndex; }
            set { headlessTabControl1.SelectedIndex = value; }
        }


        protected override void OnEnter(EventArgs e)
        {
            if (!initialized)
            {
                //RefreshPages();
                initialized = true;
            }
            base.OnEnter(e);
        }

        public void NavigateTo(int i)
        {
            CurrentPage = i;
            label1.Text = headlessTabControl1.TabPages[i].Text;
        }

        public void Back()
        {
            NavigateTo(CurrentPage - 1);
            if (CurrentPage == 0)
                travelButton1.BackEnabled = false;
            travelButton1.ForwardEnabled = true;
        }

        public void Forward()
        {
            NavigateTo(CurrentPage + 1);
            travelButton1.BackEnabled = true;
            if (CurrentPage == headlessTabControl1.TabPages.Count - 1)
            {
                travelButton1.ForwardEnabled = false;
            }
        }

        public void RefreshPages()
        {
            foreach (UserControl page in Pages)
            {
                var tp = new TabPage();
                tp.Controls.Add(page);
                tp.Name = page.Text;
                tp.Text = page.Text;
                headlessTabControl1.TabPages.Add(tp);
            }
            NavigateTo(0);
        }

        private void InitializeComponent()
        {
            headlessTabControl1 = new HeadlessTabControl();
            travelButton1 = new TravelButton();
            label1 = new Label();
            SuspendLayout();
            // 
            // headlessTabControl1
            // 
            headlessTabControl1.Dock = DockStyle.Fill;
            headlessTabControl1.Location = new Point(0, 29);
            headlessTabControl1.Name = "headlessTabControl1";
            headlessTabControl1.SelectedIndex = 0;
            headlessTabControl1.Size = new Size(406, 246);
            headlessTabControl1.TabIndex = 1;
            // 
            // travelButton1
            // 
            travelButton1.BackEnabled = false;
            travelButton1.BackToolTip = null;
            travelButton1.Dock = DockStyle.Top;
            travelButton1.ForwardToolTip = null;
            travelButton1.Location = new Point(0, 0);
            travelButton1.Name = "travelButton1";
            travelButton1.Size = new Size(406, 29);
            travelButton1.TabIndex = 0;
            travelButton1.TabStop = false;
            travelButton1.Text = "travelButton1";
            travelButton1.ItemClicked += travelButton1_ItemClicked;
            // 
            // label1
            // 
            label1.Anchor = (((((AnchorStyles.Top | AnchorStyles.Bottom)
                                | AnchorStyles.Left)
                               | AnchorStyles.Right)));
            label1.AutoSize = true;
            label1.Location = new Point(173, 13);
            label1.Name = "label1";
            label1.Size = new Size(27, 13);
            label1.TabIndex = 2;
            label1.Text = "Title";
            // 
            // PageNavigator
            // 
            Controls.Add(label1);
            Controls.Add(headlessTabControl1);
            Controls.Add(travelButton1);
            Name = "PageNavigator";
            Size = new Size(406, 275);
            ResumeLayout(false);
            PerformLayout();
        }

        private void travelButton1_ItemClicked(object sender, TravelButtonItemClickedEventArgs e)
        {
            if (e.ClickedItem == TravelButtonItem.BackButton)
            {
                Back();
            }
            if (e.ClickedItem == TravelButtonItem.ForwardButton)
            {
                Forward();
            }
        }

        #region Implementation of IUIElement

        public string ID { get; set; }
        public string Content { get; set; }
        public IValidator Validator { get; set; }
        public IStyle style { get; private set; }

        public Color TitleColor { get { return BackColor; } set { BackColor = value; } }
        public Color ChildColor { get { return Pages[CurrentPage].BackColor; } set { Pages[CurrentPage].BackColor = value; } }

        #endregion
    }
}