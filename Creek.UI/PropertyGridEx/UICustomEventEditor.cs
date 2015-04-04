using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace Creek.UI.PropertyGridEx
{
    public class UICustomEventEditor : UITypeEditor
    {
        #region Delegates

        public delegate object OnClick(object sender, EventArgs e);

        #endregion

        protected OnClick m_MethodDelegate;
        protected CustomProperty.CustomPropertyDescriptor m_sender;

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
            if (m_MethodDelegate == null)
            {
                var attr = (DelegateAttribute) context.PropertyDescriptor.Attributes[typeof (DelegateAttribute)];
                m_MethodDelegate = attr.GetMethod;
            }
            if (m_sender == null)
            {
                m_sender = context.PropertyDescriptor as CustomProperty.CustomPropertyDescriptor;
            }
            return m_MethodDelegate.Invoke(m_sender, null);
        }

        #region Nested type: DelegateAttribute

        [AttributeUsage(AttributeTargets.Property)]
        public class DelegateAttribute : Attribute
        {
            protected OnClick m_MethodDelegate;

            public DelegateAttribute(OnClick MethodDelegate)
            {
                m_MethodDelegate = MethodDelegate;
            }

            public OnClick GetMethod
            {
                get { return m_MethodDelegate; }
            }
        }

        #endregion
    }
}