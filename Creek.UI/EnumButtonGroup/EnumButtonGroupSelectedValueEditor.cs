using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Creek.UI.EnumButtonGroup
{
    /// <summary>
    /// Used by PropertyGrid to edit the selected value of an enumerated type
    /// </summary>
    public class EnumButtonGroupSelectedValueEditor : UITypeEditor
    {
        /// <summary>
        /// Edits the value of the specified object using a drop-down list of values
        /// </summary>
        /// <param name="context">Type descriptor context</param>
        /// <param name="provider">Service provider</param>
        /// <param name="value">Value to edit</param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            object editedValue;
            var editorService = provider.GetService(typeof (IWindowsFormsEditorService)) as IWindowsFormsEditorService;

            if (editorService != null && value != null)
            {
                // Create a list box to place our items in
                var listBox = new EnumButtonGroupSelectedValueListBox(editorService, value);

                // Show the list box in a drop-down control
                editorService.DropDownControl(listBox);

                // Return edited value
                editedValue = listBox.SelectedItem;
            }
            else
            {
                editedValue = base.EditValue(context, provider, value);
            }

            return (editedValue);
        }

        /// <summary>
        /// Returns an enumerated value that indicates the style of editor
        /// </summary>
        /// <param name="context">Type descriptor context</param>
        /// <returns>One of the UITypeEditorEditStyle values</returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            // Our editor will be a list of values
            return (UITypeEditorEditStyle.DropDown);
        }

        #region Nested type: EnumButtonGroupSelectedValueListBox

        /// <summary>
        /// List box that is used to display enumerated values
        /// </summary>
        private class EnumButtonGroupSelectedValueListBox : ListBox
        {
            private readonly IWindowsFormsEditorService editorService;
            private readonly object selectedValue;

            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="editorService">Editor service</param>
            /// <param name="selectedValue">Currently selected value</param>
            public EnumButtonGroupSelectedValueListBox(IWindowsFormsEditorService editorService, object selectedValue)
            {
                this.editorService = editorService;
                this.selectedValue = selectedValue;

                Array values = Enum.GetValues(selectedValue.GetType());

                for (int i = 0; i < values.Length; ++i)
                {
                    Items.Add(values.GetValue(i));
                }

                SelectedItem = this.selectedValue;
                SelectionMode = SelectionMode.One;
                BorderStyle = BorderStyle.None;
                ItemHeight = 18;
                Height = ItemHeight*values.Length;
            }

            /// <summary>
            /// Handles click event
            /// </summary>
            /// <param name="e">Event arguments</param>
            protected override void OnClick(EventArgs e)
            {
                Close();
            }

            /// <summary>
            /// Handles selected value changed event
            /// </summary>
            /// <param name="e">Event arguments</param>
            protected override void OnSelectedValueChanged(EventArgs e)
            {
                base.OnSelectedValueChanged(e);
                Invalidate();
            }

            /// <summary>
            /// Processes a dialog key
            /// </summary>
            /// <param name="keyData">One of the Keys enumerated values that represents the key to process</param>
            /// <returns></returns>
            protected override bool ProcessDialogKey(Keys keyData)
            {
                bool processed;

                if (keyData == Keys.Up || keyData == Keys.Down)
                {
                    RaiseKeyEvent(this, new KeyEventArgs(keyData));
                    processed = true;
                }
                else if (keyData == Keys.Enter)
                {
                    Close();
                    processed = true;
                }
                else if (keyData == Keys.Escape)
                {
                    SelectedItem = selectedValue;
                    Close();
                    processed = true;
                }
                else
                {
                    processed = base.ProcessDialogKey(keyData);
                }

                return (processed);
            }

            /// <summary>
            /// Closes the drop-down control
            /// </summary>
            private void Close()
            {
                if (editorService != null)
                {
                    editorService.CloseDropDown();
                }
            }
        }

        #endregion
    }
}