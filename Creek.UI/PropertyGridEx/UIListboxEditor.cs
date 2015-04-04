using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Creek.UI.PropertyGridEx
{
    public class UIListboxEditor : UITypeEditor
    {
        private readonly ListBox oList = new ListBox();
        private bool bIsDropDownResizable;
        private IWindowsFormsEditorService oEditorService;
        private object oSelectedValue;

        public override bool IsDropDownResizable
        {
            get { return bIsDropDownResizable; }
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            if (context != null && context.Instance != null)
            {
                var attribute =
                    (UIListboxIsDropDownResizable)
                    context.PropertyDescriptor.Attributes[typeof (UIListboxIsDropDownResizable)];
                if (attribute != null)
                {
                    bIsDropDownResizable = true;
                }
                return UITypeEditorEditStyle.DropDown;
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

            oEditorService = (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));
            if (oEditorService != null)
            {
                // Get the Back reference to the Custom Property
                var oDescriptor = (CustomProperty.CustomPropertyDescriptor) context.PropertyDescriptor;
                var cp = (CustomProperty) oDescriptor.CustomProperty;

                // Declare attributes
                UIListboxDatasource datasource;
                UIListboxValueMember valuemember;
                UIListboxDisplayMember displaymember;

                // Get attributes
                datasource = (UIListboxDatasource) context.PropertyDescriptor.Attributes[typeof (UIListboxDatasource)];
                valuemember =
                    (UIListboxValueMember) context.PropertyDescriptor.Attributes[typeof (UIListboxValueMember)];
                displaymember =
                    (UIListboxDisplayMember) context.PropertyDescriptor.Attributes[typeof (UIListboxDisplayMember)];

                oList.BorderStyle = BorderStyle.None;
                oList.IntegralHeight = true;

                if (datasource != null)
                {
                    oList.DataSource = datasource.Value;
                }

                if (displaymember != null)
                {
                    oList.DisplayMember = displaymember.Value;
                }

                if (valuemember != null)
                {
                    oList.ValueMember = valuemember.Value;
                }

                if (value != null)
                {
                    if (value.GetType().Name == "String")
                    {
                        oList.Text = (string) value;
                    }
                    else
                    {
                        oList.SelectedItem = value;
                    }
                }


                oList.SelectedIndexChanged += SelectedItem;

                oEditorService.DropDownControl(oList);
                if (oList.SelectedIndices.Count == 1)
                {
                    cp.SelectedItem = oList.SelectedItem;
                    cp.SelectedValue = oSelectedValue;
                    value = oList.Text;
                }
                oEditorService.CloseDropDown();
            }
            else
            {
                return base.EditValue(provider, value);
            }

            return value;
        }

        private void SelectedItem(object sender, EventArgs e)
        {
            if (oEditorService != null)
            {
                if (oList.SelectedValue != null)
                {
                    oSelectedValue = oList.SelectedValue;
                }
                oEditorService.CloseDropDown();
            }
        }

        #region Nested type: UIListboxDatasource

        [AttributeUsage(AttributeTargets.Property)]
        public class UIListboxDatasource : Attribute
        {
            private readonly object oDataSource;

            public UIListboxDatasource(ref object Datasource)
            {
                oDataSource = Datasource;
            }

            public object Value
            {
                get { return oDataSource; }
            }
        }

        #endregion

        #region Nested type: UIListboxDisplayMember

        [AttributeUsage(AttributeTargets.Property)]
        public class UIListboxDisplayMember : Attribute
        {
            public UIListboxDisplayMember(string DisplayMember)
            {
                Value = DisplayMember;
            }

            public string Value { get; set; }
        }

        #endregion

        #region Nested type: UIListboxIsDropDownResizable

        [AttributeUsage(AttributeTargets.Property)]
        public class UIListboxIsDropDownResizable : Attribute
        {
        }

        #endregion

        #region Nested type: UIListboxValueMember

        [AttributeUsage(AttributeTargets.Property)]
        public class UIListboxValueMember : Attribute
        {
            public UIListboxValueMember(string ValueMember)
            {
                Value = ValueMember;
            }

            public string Value { get; set; }
        }

        #endregion
    }
}