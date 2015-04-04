using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Creek.UI.CheckboxCombobox.SelectionWrappers
{
    /// <summary>
    /// Used together with the ListSelectionWrapper in order to wrap data sources for a CheckBoxComboBox.
    /// It helps to ensure you don't add an extra "Selected" property to a class that don't really need or want that information.
    /// </summary>
    public class ObjectSelectionWrapper<T> : INotifyPropertyChanged
    {
        public ObjectSelectionWrapper(T item, ListSelectionWrapper<T> container)
        {
            _Container = container;
            Item = item;
        }

        #region PRIVATE PROPERTIES

        /// <summary>
        /// The containing list for these selections.
        /// </summary>
        private readonly ListSelectionWrapper<T> _Container;

        /// <summary>
        /// Is this item selected.
        /// </summary>
        private bool _Selected;

        #endregion

        #region PUBLIC PROPERTIES

        /// <summary>
        /// An indicator of how many items with the specified status is available for the current filter level.
        /// Thaught this would make the app a bit more user-friendly and help not to miss items in Statusses
        /// that are not often used.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// A reference to the item wrapped.
        /// </summary>
        public T Item { get; set; }

        /// <summary>
        /// The item display value. If ShowCount is true, it displays the "Name [Count]".
        /// </summary>
        public string Name
        {
            get
            {
                string Name = null;
                if (string.IsNullOrEmpty(_Container.DisplayNameProperty))
                    Name = Item.ToString();
                else if (Item is DataRow) // A specific implementation for DataRow
                    Name = ((DataRow) ((Object) Item))[_Container.DisplayNameProperty].ToString();
                else
                {
                    PropertyDescriptorCollection PDs = TypeDescriptor.GetProperties(Item);
                    foreach (PropertyDescriptor PD in PDs)
                        if (PD.Name.CompareTo(_Container.DisplayNameProperty) == 0)
                        {
                            Name = PD.GetValue(Item).ToString();
                            break;
                        }
                    if (string.IsNullOrEmpty(Name))
                    {
                        PropertyInfo PI = Item.GetType().GetProperty(_Container.DisplayNameProperty);
                        if (PI == null)
                            throw new Exception(String.Format(
                                "Property {0} cannot be found on {1}.",
                                _Container.DisplayNameProperty,
                                Item.GetType()));
                        Name = PI.GetValue(Item, null).ToString();
                    }
                }
                return _Container.ShowCounts ? String.Format("{0} [{1}]", Name, Count) : Name;
            }
        }

        /// <summary>
        /// The textbox display value. The names concatenated.
        /// </summary>
        public string NameConcatenated
        {
            get { return _Container.SelectedNames; }
        }

        /// <summary>
        /// Indicates whether the item is selected.
        /// </summary>
        public bool Selected
        {
            get { return _Selected; }
            set
            {
                if (_Selected != value)
                {
                    _Selected = value;
                    OnPropertyChanged("Selected");
                    OnPropertyChanged("NameConcatenated");
                }
            }
        }

        #endregion

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}