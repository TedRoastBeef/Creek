using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Creek.UI.PropertyGridEx
{
    public class UIFilenameEditor : UITypeEditor
    {
        #region FileDialogType enum

        public enum FileDialogType
        {
            LoadFileDialog,
            SaveFileDialog
        }

        #endregion

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                if (! context.PropertyDescriptor.IsReadOnly)
                {
                    return UITypeEditorEditStyle.Modal;
                }
            }
            return UITypeEditorEditStyle.None;
        }

        [RefreshProperties(RefreshProperties.All)]
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context == null || provider == null || context.Instance == null)
            {
                return base.EditValue(provider, value);
            }

            FileDialog fileDlg;
            if (context.PropertyDescriptor.Attributes[typeof (SaveFileAttribute)] == null)
            {
                fileDlg = new OpenFileDialog();
            }
            else
            {
                fileDlg = new SaveFileDialog();
            }
            fileDlg.Title = "Select " + context.PropertyDescriptor.DisplayName;
            fileDlg.FileName = (string) value;

            var filterAtt =
                (FileDialogFilterAttribute) context.PropertyDescriptor.Attributes[typeof (FileDialogFilterAttribute)];
            if (filterAtt != null)
            {
                fileDlg.Filter = filterAtt.Filter;
            }
            if (fileDlg.ShowDialog() == DialogResult.OK)
            {
                value = fileDlg.FileName;
            }
            fileDlg.Dispose();
            return value;
        }

        #region Nested type: FileDialogFilterAttribute

        [AttributeUsage(AttributeTargets.Property)]
        public class FileDialogFilterAttribute : Attribute
        {
            private readonly string _filter;

            public FileDialogFilterAttribute(string filter)
            {
                _filter = filter;
            }

            public string Filter
            {
                get { return _filter; }
            }
        }

        #endregion

        #region Nested type: SaveFileAttribute

        [AttributeUsage(AttributeTargets.Property)]
        public class SaveFileAttribute : Attribute
        {
        }

        #endregion
    }
}