using System;
using Creek.UI.Tabstrip.Control;

namespace Creek.UI.Tabstrip
{

    #region TabStripItemClosingEventArgs

    public class TabStripItemClosingEventArgs : EventArgs
    {
        public TabStripItemClosingEventArgs(FATabStripItem item)
        {
            Item = item;
        }

        public FATabStripItem Item { get; set; }

        public bool Cancel { get; set; }
    }

    #endregion

    #region TabStripItemChangedEventArgs

    public class TabStripItemChangedEventArgs : EventArgs
    {
        private readonly FATabStripItemChangeTypes changeType;
        private readonly FATabStripItem itm;

        public TabStripItemChangedEventArgs(FATabStripItem item, FATabStripItemChangeTypes type)
        {
            changeType = type;
            itm = item;
        }

        public FATabStripItemChangeTypes ChangeType
        {
            get { return changeType; }
        }

        public FATabStripItem Item
        {
            get { return itm; }
        }
    }

    #endregion

    public delegate void TabStripItemChangedHandler(TabStripItemChangedEventArgs e);

    public delegate void TabStripItemClosingHandler(TabStripItemClosingEventArgs e);
}