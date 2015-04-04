using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    /// <summary>
    /// User control which is used to simplify page representation of data.
    /// </summary>
    [ToolboxItem(true)]
    public class PageLister : UserControl
    {
        #region Delegates

        /// <summary>
        /// This delegate is called when current page of lister is
        /// changed.
        /// </summary>
        public delegate void PageChangeHandler(uint currentPage);

        #endregion

        private const int gap = 3;

        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private readonly Container components = null;

        private readonly ArrayList pages = new ArrayList();
        private uint currentPage = 1;

        private Label labelPage;
        private LinkLabel linkLabelFirst;
        private LinkLabel linkLabelLast;
        private LinkLabel linkLabelNext;
        private LinkLabel linkLabelPrev;

        private uint numPagesShown = 3;
        private uint pagesCount;

        public PageLister()
        {
            // This call is required by the Windows.Forms Form Designer.
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the total number of pages.
        /// </summary>
        [Bindable(true),
         DefaultValue(7),
         Description("Total number of pages.")]
        public uint PagesCount
        {
            get { return pagesCount; }
            set
            {
                pagesCount = value;

                populateLinks();
            }
        }

        /// <summary>
        /// Gets or sets a number of the current page.
        /// </summary>
        [Bindable(true),
         DefaultValue(1),
         Description("Specifies current page's number")]
        public uint CurrentPage
        {
            get { return currentPage; }
            set
            {
                currentPage = value;

                populateLinks();
            }
        }

        /// <summary>
        /// Gets or sets the number of pages shown on the
        /// each side of the current page. So, the maximal total number
        /// of numbered page links is NumPagesShown * 2 + 1.
        /// </summary>
        [Bindable(true),
         DefaultValue(3),
         Description("Specifies the number of pages shown on the each "
                     + "side of the current page")]
        public uint NumPagesShown
        {
            get { return numPagesShown; }
            set
            {
                numPagesShown = value;

                populateLinks();
            }
        }

        /// <summary>
        /// Event fired when the current page is changed.
        /// </summary>
        public event PageChangeHandler PageChanged;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void populateLinks()
        {
            if (pagesCount < currentPage
                || pagesCount == 0
                || currentPage == 0)
            {
                // info hasn't been entered completely yet
                return;
            }

            // set label's text
            labelPage.Text = "Page " + currentPage.ToString()
                             + " of " + pagesCount.ToString() + ".";

            // set position of the two first (static) links
            linkLabelFirst.Location =
                new Point(
                    labelPage.Location.X + labelPage.Width + gap,
                    labelPage.Location.Y);

            linkLabelPrev.Location =
                new Point(
                    linkLabelFirst.Location.X + linkLabelFirst.Width + gap,
                    linkLabelFirst.Location.Y);

            // calculate number of links on both left and right sides
            // of the current page
            uint pagesLeft = numPagesShown;
            uint pagesRight = numPagesShown;
            uint pagesShownTotal = Math.Min(2*numPagesShown + 1, pagesCount);

            if (currentPage <= numPagesShown)
            {
                pagesLeft = currentPage - 1;
                pagesRight = pagesShownTotal - pagesLeft - 1;
            }
            else if (pagesCount - currentPage <= numPagesShown)
            {
                pagesRight = pagesCount - currentPage;
                pagesLeft = pagesShownTotal - pagesRight - 1;
            }

            // change existing pages without deleting them
            // to avoid blinking
            for (int i = 0; i < Math.Min(pagesShownTotal, pages.Count); i++)
            {
                var pageNum = (uint) (currentPage - pagesLeft + i);
                var page = (LinkLabel) pages[i];
                page.Tag = pageNum;
                page.Text = page.Tag.ToString();
                page.Font = pageNum == currentPage
                                ? new Font(Font, FontStyle.Bold)
                                : null;

                // set page link's location relative to the previous
                // link
                LinkLabel pagePrev = i > 0
                                         ? (LinkLabel) pages[i - 1]
                                         : linkLabelPrev;
                page.Location = new Point(
                    pagePrev.Location.X + pagePrev.Width + gap,
                    pagePrev.Location.Y);
            }

            if (pages.Count > pagesShownTotal)
            {
                // remove unnecessary pages
                for (int i = pages.Count - 1; i >= pagesShownTotal; i--)
                {
                    var page = (LinkLabel) pages[i];
                    page.Parent = null;
                    page.Visible = false;
                    pages.RemoveAt(i);
                    page.Dispose();
                }
            }
            else if (pages.Count < pagesShownTotal)
            {
                // add pages
                for (int i = pages.Count; i < pagesShownTotal; i++)
                {
                    var page = new LinkLabel();
                    page.AutoSize = true;
                    page.Tag = (uint) (currentPage - pagesLeft + i);
                    page.Text = page.Tag.ToString();

                    page.Height = linkLabelFirst.Height;

                    // set page location relative to the previous page
                    LinkLabel pagePrev = pages.Count > 0
                                             ? (LinkLabel) pages[i - 1]
                                             : linkLabelPrev;

                    page.Font = i == currentPage
                                    ? new Font(Font, FontStyle.Bold)
                                    : null;

                    page.Parent = this;

                    page.Location = new Point(
                        pagePrev.Location.X + pagePrev.Width + gap,
                        pagePrev.Location.Y);

                    pages.Add(page);
                    page.LinkClicked +=
                        linkClicked;
                }
            }

            Debug.Assert(pages.Count > 0);

            // change location of the last two links (next & last)
            var pageLast = (LinkLabel) pages[pages.Count - 1];

            linkLabelNext.Location = new Point(
                pageLast.Location.X + pageLast.Width + gap,
                linkLabelNext.Location.Y);

            linkLabelLast.Location = new Point(
                linkLabelNext.Location.X + linkLabelNext.Width,
                linkLabelLast.Location.Y);

            checkPages();
        }

        private void checkPages()
        {
            linkLabelFirst.Enabled =
                linkLabelPrev.Enabled = currentPage > 1;
            linkLabelNext.Enabled =
                linkLabelLast.Enabled = currentPage < pagesCount;
        }

        private void linkClicked(object sender,
                                 LinkLabelLinkClickedEventArgs e)
        {
            var page = (LinkLabel) sender;
            switch (page.Text)
            {
                case "<<":
                    if (currentPage == 1)
                        return;
                    currentPage = 1;
                    break;
                case "<":
                    if (currentPage == 1)
                        return;
                    currentPage--;
                    break;
                case ">":
                    if (currentPage == pagesCount)
                        return;
                    currentPage++;
                    break;
                case ">>":
                    if (currentPage == pagesCount)
                        return;
                    currentPage = pagesCount;
                    break;
                default:
                    currentPage = (uint) page.Tag;
                    break;
            }

            populateLinks();

            if (PageChanged != null)
                PageChanged(currentPage);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            labelPage = new System.Windows.Forms.Label();
            linkLabelFirst = new System.Windows.Forms.LinkLabel();
            linkLabelPrev = new System.Windows.Forms.LinkLabel();
            linkLabelNext = new System.Windows.Forms.LinkLabel();
            linkLabelLast = new System.Windows.Forms.LinkLabel();
            SuspendLayout();
            // 
            // labelPage
            // 
            labelPage.AutoSize = true;
            labelPage.Location = new System.Drawing.Point(8, 8);
            labelPage.Name = "labelPage";
            labelPage.Size = new System.Drawing.Size(80, 18);
            labelPage.TabIndex = 0;
            labelPage.Text = "Page ... of ...";
            // 
            // linkLabelFirst
            // 
            linkLabelFirst.AutoSize = true;
            linkLabelFirst.Location = new System.Drawing.Point(152, 8);
            linkLabelFirst.Name = "linkLabelFirst";
            linkLabelFirst.Size = new System.Drawing.Size(20, 18);
            linkLabelFirst.TabIndex = 1;
            linkLabelFirst.TabStop = true;
            linkLabelFirst.Text = "<<";
            linkLabelFirst.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkClicked);
            // 
            // linkLabelPrev
            // 
            linkLabelPrev.AutoSize = true;
            linkLabelPrev.Location = new System.Drawing.Point(187, 8);
            linkLabelPrev.Name = "linkLabelPrev";
            linkLabelPrev.Size = new System.Drawing.Size(13, 18);
            linkLabelPrev.TabIndex = 2;
            linkLabelPrev.TabStop = true;
            linkLabelPrev.Text = "<";
            linkLabelPrev.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkClicked);
            // 
            // linkLabelNext
            // 
            linkLabelNext.AutoSize = true;
            linkLabelNext.Location = new System.Drawing.Point(280, 8);
            linkLabelNext.Name = "linkLabelNext";
            linkLabelNext.Size = new System.Drawing.Size(13, 18);
            linkLabelNext.TabIndex = 3;
            linkLabelNext.TabStop = true;
            linkLabelNext.Text = ">";
            linkLabelNext.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkClicked);
            // 
            // linkLabelLast
            // 
            linkLabelLast.AutoSize = true;
            linkLabelLast.Location = new System.Drawing.Point(315, 8);
            linkLabelLast.Name = "linkLabelLast";
            linkLabelLast.Size = new System.Drawing.Size(20, 18);
            linkLabelLast.TabIndex = 4;
            linkLabelLast.TabStop = true;
            linkLabelLast.Text = ">>";
            linkLabelLast.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkClicked);
            // 
            // PageLister
            // 
            Controls.Add(this.linkLabelLast);
            Controls.Add(this.linkLabelNext);
            Controls.Add(this.linkLabelPrev);
            Controls.Add(this.linkLabelFirst);
            Controls.Add(this.labelPage);
            Name = "PageLister";
            Size = new System.Drawing.Size(640, 32);
            ResumeLayout(false);
        }

        #endregion
    }
}