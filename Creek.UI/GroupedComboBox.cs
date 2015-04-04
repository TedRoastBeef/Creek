// A ComboBox Control With Grouping
// Bradley Smith - 2010/06/23

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    /// <summary>
    /// Represents a Windows combo box control that, when bound to a data source, is capable of 
    /// displaying items in groups/categories.
    /// </summary>
    public class GroupedComboBox : ComboBox, IComparer
    {
        private readonly ArrayList mInternalItems; // internal sorted collection of items
        private readonly TextFormatFlags mTextFormatFlags; // used in measuring/painting
        private BindingSource mBindingSource; // used for change detection and grouping
        private Font mGroupFont; // for painting
        private string mGroupMember; // name of group-by property
        private PropertyDescriptor mGroupProperty; // used to get group-by values

        /// <summary>
        /// Initialises a new instance of the GroupedComboBox class.
        /// </summary>
        public GroupedComboBox()
        {
            base.DrawMode = DrawMode.OwnerDrawVariable;
            mGroupMember = String.Empty;
            mInternalItems = new ArrayList();
            mTextFormatFlags = TextFormatFlags.EndEllipsis | TextFormatFlags.NoPrefix | TextFormatFlags.SingleLine |
                               TextFormatFlags.VerticalCenter;
        }

        /// <summary>
        /// Gets or sets the data source for this GroupedComboBox.
        /// </summary>
        public new object DataSource
        {
            get
            {
                // binding source should be transparent to the user
                return (mBindingSource != null) ? mBindingSource.DataSource : null;
            }
            set
            {
                if (value != null)
                {
                    // wrap the object in a binding source and listen for changes
                    mBindingSource = new BindingSource(value, String.Empty);
                    mBindingSource.ListChanged += mBindingSource_ListChanged;
                    SyncInternalItems();
                }
                else
                {
                    // remove binding
                    base.DataSource = mBindingSource = null;
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the drawing of elements in the list will be handled by user code. 
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DrawMode DrawMode
        {
            get { return base.DrawMode; }
        }

        /// <summary>
        /// Gets or sets the property to use when grouping items in the list.
        /// </summary>
        public string GroupMember
        {
            get { return mGroupMember; }
            set
            {
                mGroupMember = value;
                if (mBindingSource != null) SyncInternalItems();
            }
        }

        #region IComparer Members

        /// <summary>
        /// Explicit interface implementation for the IComparer.Compare method. Performs a two-tier comparison 
        /// on two list items so that the list can be sorted by group, then by display value.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        int IComparer.Compare(object x, object y)
        {
            // compare the display values (and return the result if there is no grouping)
            int secondLevelSort = Comparer.Default.Compare(GetItemText(x), GetItemText(y));
            if (mGroupProperty == null) return secondLevelSort;

            // compare the group values - if equal, return the earlier comparison
            int firstLevelSort = Comparer.Default.Compare(
                Convert.ToString(mGroupProperty.GetValue(x)),
                Convert.ToString(mGroupProperty.GetValue(y))
                );

            if (firstLevelSort == 0)
                return secondLevelSort;
            else
                return firstLevelSort;
        }

        #endregion

        /// <summary>
        /// Determines whether the list item at the specified index is the start of a new group. In all 
        /// cases, populates the string respresentation of the group that the item belongs to.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="groupText"></param>
        /// <returns></returns>
        private bool IsGroupStart(int index, out string groupText)
        {
            bool isGroupStart = false;
            groupText = String.Empty;

            if ((mGroupProperty != null) && (index >= 0) && (index < Items.Count))
            {
                // get the group value using the property descriptor
                groupText = Convert.ToString(mGroupProperty.GetValue(Items[index]));

                // this item is the start of a group if it is the first item with a group -or- if
                // the previous item has a different group
                if ((index == 0) && (groupText != String.Empty))
                {
                    isGroupStart = true;
                }
                else if ((index - 1) >= 0)
                {
                    string previousGroupText = Convert.ToString(mGroupProperty.GetValue(Items[index - 1]));
                    if (previousGroupText != groupText) isGroupStart = true;
                }
            }

            return isGroupStart;
        }

        /// <summary>
        /// Re-synchronises the internal sorted collection when the data source changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            SyncInternalItems();
        }

        /// <summary>
        /// When the control font changes, updates the font used to render group names.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            mGroupFont = new Font(Font, FontStyle.Bold);
        }

        /// <summary>
        /// When the parent control changes, updates the font used to render group names.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            mGroupFont = new Font(Font, FontStyle.Bold);
        }

        /// <summary>
        /// Performs custom painting for a list item.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if ((e.Index >= 0) && (e.Index < Items.Count))
            {
                // get noteworthy states
                bool comboBoxEdit = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
                bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
                bool noAccelerator = (e.State & DrawItemState.NoAccelerator) == DrawItemState.NoAccelerator;
                bool disabled = (e.State & DrawItemState.Disabled) == DrawItemState.Disabled;
                bool focus = (e.State & DrawItemState.Focus) == DrawItemState.Focus;

                // determine grouping
                string groupText;
                bool isGroupStart = IsGroupStart(e.Index, out groupText) && !comboBoxEdit;
                bool hasGroup = (groupText != String.Empty) && !comboBoxEdit;

                // the item text will appear in a different colour, depending on its state
                Color textColor;
                if (disabled)
                    textColor = SystemColors.GrayText;
                else if ((comboBoxEdit && Focused && !DroppedDown) || selected)
                    textColor = SystemColors.HighlightText;
                else
                    textColor = ForeColor;

                // items will be indented if they belong to a group
                Rectangle itemBounds = Rectangle.FromLTRB(
                    e.Bounds.X + (hasGroup ? 12 : 0),
                    e.Bounds.Y + (isGroupStart ? (e.Bounds.Height/2) : 0),
                    e.Bounds.Right,
                    e.Bounds.Bottom
                    );
                var groupBounds = new Rectangle(
                    e.Bounds.X,
                    e.Bounds.Y,
                    e.Bounds.Width,
                    e.Bounds.Height/2
                    );

                if (isGroupStart && selected)
                {
                    // ensure that the group header is never highlighted
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                    e.Graphics.FillRectangle(new SolidBrush(BackColor), groupBounds);
                }
                else
                {
                    // use the default background-painting logic
                    e.DrawBackground();
                }

                // render group header text
                if (isGroupStart)
                    TextRenderer.DrawText(
                        e.Graphics,
                        groupText,
                        mGroupFont,
                        groupBounds,
                        ForeColor,
                        mTextFormatFlags
                        );

                // render item text
                TextRenderer.DrawText(
                    e.Graphics,
                    GetItemText(Items[e.Index]),
                    Font,
                    itemBounds,
                    textColor,
                    mTextFormatFlags
                    );

                // paint the focus rectangle if required
                if (focus && !noAccelerator)
                {
                    if (isGroupStart && selected)
                    {
                        // don't draw the focus rectangle around the group header
                        ControlPaint.DrawFocusRectangle(e.Graphics,
                                                        Rectangle.FromLTRB(groupBounds.X, itemBounds.Y, itemBounds.Right,
                                                                           itemBounds.Bottom));
                    }
                    else
                    {
                        // use default focus rectangle painting logic
                        e.DrawFocusRectangle();
                    }
                }
            }
        }

        /// <summary>
        /// Determines the size of a list item.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);

            e.ItemHeight = Font.Height;

            string groupText;
            if (IsGroupStart(e.Index, out groupText))
            {
                // the first item in each group will be twice as tall in order to accommodate the group header
                e.ItemHeight *= 2;
                e.ItemWidth = Math.Max(
                    e.ItemWidth,
                    TextRenderer.MeasureText(
                        e.Graphics,
                        groupText,
                        mGroupFont,
                        new Size(e.ItemWidth, e.ItemHeight),
                        mTextFormatFlags
                        ).Width
                    );
            }
        }

        /// <summary>
        /// Rebuilds the internal sorted collection.
        /// </summary>
        private void SyncInternalItems()
        {
            // locate the property descriptor that corresponds to the value of GroupMember
            mGroupProperty = null;
            foreach (PropertyDescriptor descriptor in mBindingSource.GetItemProperties(null))
            {
                if (descriptor.Name.Equals(mGroupMember))
                {
                    mGroupProperty = descriptor;
                    break;
                }
            }

            // rebuild the collection and sort using custom logic
            mInternalItems.Clear();
            foreach (object item in mBindingSource) mInternalItems.Add(item);
            mInternalItems.Sort(this);

            // bind the underlying ComboBox to the sorted collection
            base.DataSource = mInternalItems;
        }
    }
}