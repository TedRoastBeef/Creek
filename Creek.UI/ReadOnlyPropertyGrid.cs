using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Creek.UI
{
    public class ReadOnlyPropertyGrid : PropertyGrid
    {
        private bool _readOnly;

        public bool ReadOnly
        {
            get { return _readOnly; }
            set
            {
                _readOnly = value;
                SetObjectAsReadOnly(SelectedObject, _readOnly);
            }
        }

        protected override void OnSelectedObjectsChanged(EventArgs e)
        {
            SetObjectAsReadOnly(SelectedObject, _readOnly);
            base.OnSelectedObjectsChanged(e);
        }

        private void SetObjectAsReadOnly(object selectedObject, bool isReadOnly)
        {
            if (selectedObject != null)
            {
                TypeDescriptor.AddAttributes(selectedObject, new Attribute[] {new ReadOnlyAttribute(isReadOnly)});
                Refresh();
            }
        }
    }
}