using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Reflection;
using System.Windows.Forms;

namespace Creek.UI.EnumButtonGroup
{
    /// <summary>
    /// Test enumeration with descriptions and obsolete values
    /// </summary>
    public enum TestEnum
    {
        [Description("One is the loneliest number")] One,
        [Description("Two can be as bad as one")] [Obsolete] Two,
        Three,
        Four,
        [Obsolete] Five
    }

    /// <summary>
    /// Windows Forms user-control that allows binding of enumeration values to a group of buttons,
    /// both at design-time and run-time
    /// </summary>
    [DefaultEvent("SelectedValueChanged")]
    public partial class EnumButtonGroup : UserControl
    {
        /// <summary>
        /// Internal variable to stop infinite recursion
        /// </summary>
        private bool alreadyChecked;

        /// <summary>
        /// Hide obsolete enumerated values if true
        /// </summary>
        private bool hideObsoleteValues;

        /// <summary>
        /// Orientation of contained buttons
        /// </summary>
        private Orientation orientation;

        /// <summary>
        /// Currently selected enumerated value
        /// </summary>
        private object selectedValue;

        /// <summary>
        /// Display descriptions of enumerated values if true
        /// </summary>
        private bool showDescriptions;

        /// <summary>
        /// Enumerated type to display
        /// </summary>
        private Type valueType;

        /// <summary>
        /// Constructor
        /// </summary>
        public EnumButtonGroup()
        {
            InitializeComponent();

            // Set default values
            showDescriptions = false;
            hideObsoleteValues = false;
            orientation = Orientation.Vertical;
            valueType = null;
            selectedValue = null;
            alreadyChecked = false;
        }

        /// <summary>
        /// Property wrapper for showDescriptions variable
        /// </summary>
        [Category("EnumButtonGroup")]
        [DisplayName("Show Descriptions")]
        public bool ShowDescriptions
        {
            get { return (showDescriptions); }
            set
            {
                showDescriptions = value;
                CreateButtons();
            }
        }

        /// <summary>
        /// Property wrapper for hideObsoleteValues variable
        /// </summary>
        [Category("EnumButtonGroup")]
        [DisplayName("Hide Obsolete Values")]
        public bool HideObsoleteValues
        {
            get { return (hideObsoleteValues); }
            set
            {
                hideObsoleteValues = value;
                CreateButtons();
            }
        }

        /// <summary>
        /// Property wrapper for orientation variable
        /// </summary>
        [Category("EnumButtonGroup")]
        public Orientation Orientation
        {
            get { return (orientation); }
            set
            {
                orientation = value;
                PerformLayout();
            }
        }

        /// <summary>
        /// Property wrapper for valueType variable
        /// </summary>
        [Category("EnumButtonGroup")]
        [DisplayName("Value Type")]
        [TypeConverter(typeof (StringToTypeConverter))]
        public Type ValueType
        {
            get { return (valueType); }
            set
            {
                /// Make sure value is an enumerated type
                if (value != null && !value.IsEnum)
                {
                    throw new ArgumentException("Argument must be an enumerated type");
                }

                valueType = value;
                if (valueType == null)
                {
                    // Reset
                    Controls.Clear();
                    Controls.Add(placeholderLabel);
                    selectedValue = null;
                    RaiseSelectedValueChangedEvent();
                }
                else
                {
                    // Create button controls based on new valueType
                    CreateButtons();
                }
            }
        }

        /// <summary>
        /// Property wrapper for selectedValue variable
        /// </summary>
        [Category("EnumButtonGroup")]
        [DisplayName("Selected Value")]
        [Editor(typeof (EnumButtonGroupSelectedValueEditor), typeof (UITypeEditor))]
        public object SelectedValue
        {
            get { return (selectedValue); }
            set
            {
                // Don't bother setting if valueType is unknown
                if (valueType != null)
                {
                    selectedValue = value;

                    // If we are here as a result of a button CheckedChanged event
                    // then there is no need to check the button
                    if (!alreadyChecked)
                    {
                        CheckButton();
                    }
                    else
                    {
                        // Tell everyone the selected value has changed
                        RaiseSelectedValueChangedEvent();
                    }
                }
            }
        }

        /// <summary>
        /// Creates buttons based on valueType
        /// </summary>
        private void CreateButtons()
        {
            // Only create buttons if the enumerated type is known
            if (valueType != null)
            {
                // Get a collection of enumerated values
                Array values = Enum.GetValues(valueType);

                if (values != null && values.Length > 0)
                {
                    SuspendLayout();

                    // Remove any previously created controls
                    Controls.Clear();

                    // Create a button for each enumerated value, storing the value
                    // in the button's Tag property
                    for (int i = 0; i < values.Length; ++i)
                    {
                        object value = values.GetValue(i);
                        bool hideObsoleteValues = this.hideObsoleteValues && IsObsolete(value);

                        // Make sure we should create a button for this enumerated value
                        if (!hideObsoleteValues)
                        {
                            var radioButton = new RadioButton();

                            radioButton.Name = string.Format("{0}{1}RadioButton", Name, value);
                            radioButton.AutoSize = true;
                            radioButton.Text = showDescriptions ? GetDescription(value) : value.ToString();
                            radioButton.Tag = value;
                            radioButton.TextAlign = ContentAlignment.MiddleLeft;

                            // Don't forget to register for CheckChanged notifications
                            radioButton.CheckedChanged += radioButton_CheckedChanged;

                            Controls.Add(radioButton);
                        }
                    }

                    ResumeLayout(false);

                    PerformLayout();

                    // Select first value
                    SelectedValue = values.GetValue(0);
                }
            }
        }

        /// <summary>
        /// Checks the button that represents the selected value
        /// </summary>
        private void CheckButton()
        {
            bool buttonFound = false;

            for (int i = 0; !buttonFound && i < Controls.Count; ++i)
            {
                Control button = Controls[i];

                if (button.Tag != null && button.Tag.Equals(selectedValue))
                {
                    // Found the button representing the selected value so
                    // check it
                    if (button is RadioButton)
                    {
                        var radioButton = button as RadioButton;

                        radioButton.Checked = true;
                    }

                    buttonFound = true;
                }
            }
        }

        /// <summary>
        /// Converts an enumerated value to a string
        /// </summary>
        /// <param name="value">The enumerated value to convert</param>
        /// <returns>A string representation of an enumerated value</returns>
        private string GetDescription(object value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes;

            if (fieldInfo != null)
            {
                attributes =
                    (DescriptionAttribute[]) (fieldInfo.GetCustomAttributes(typeof (DescriptionAttribute), false));
            }
            else
            {
                attributes = null;
            }

            return ((attributes != null && attributes.Length > 0) ? attributes[0].Description : value.ToString());
        }

        /// <summary>
        /// Determines if an enumerated value has been marked as obsolete
        /// </summary>
        /// <param name="value">The enumerated value to check</param>
        /// <returns>True if the enumerated value has been marked as obsolete, false otherwise</returns>
        private bool IsObsolete(object value)
        {
            FieldInfo fieldInfo = value.GetType().GetField(value.ToString());
            ObsoleteAttribute[] attributes;

            if (fieldInfo != null)
            {
                attributes = (ObsoleteAttribute[]) (fieldInfo.GetCustomAttributes(typeof (ObsoleteAttribute), false));
            }
            else
            {
                attributes = null;
            }

            return (attributes != null && attributes.Length > 0);
        }

        /// <summary>
        /// Performs layout of button controls based on orientation
        /// </summary>
        /// <param name="e">Layout event arguments</param>
        protected override void OnLayout(LayoutEventArgs e)
        {
            base.OnLayout(e);

            // Only do a layout if we have a known enumerated type
            if (valueType != null)
            {
                Padding margin = Margin;
                int offset;

                if (orientation == Orientation.Horizontal)
                {
                    offset = margin.Left;
                }
                else
                {
                    offset = margin.Top;
                }

                // Loop through each button and adjust its location
                // based on the current orientation
                foreach (Control control in Controls)
                {
                    Size clientSize = control.ClientSize;

                    if (orientation == Orientation.Horizontal)
                    {
                        control.Location = new Point(offset, margin.Top);
                        offset += (clientSize.Width + margin.Horizontal);
                    }
                    else
                    {
                        control.Location = new Point(margin.Left, offset);
                        offset += (clientSize.Height + margin.Vertical);
                    }
                }
            }
        }

        /// <summary>
        /// Event handler for button check / uncheck notifications
        /// </summary>
        /// <param name="sender">Button that was checked / unchecked</param>
        /// <param name="e">Event arguments</param>
        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;

            if (radioButton.Checked)
            {
                // Prevent infinite recursion
                alreadyChecked = true;

                // Set selected value
                SelectedValue = radioButton.Tag;

                // Back to normal
                alreadyChecked = false;
            }
        }

        /// <summary>
        /// Event that observers will register to receive notifications for
        /// </summary>
        public event EventHandler SelectedValueChanged;

        /// <summary>
        /// Notifies all observers that registered to receive selected value changed notifications
        /// </summary>
        protected void RaiseSelectedValueChangedEvent()
        {
            if (SelectedValueChanged != null)
            {
                SelectedValueChanged(this, EventArgs.Empty);
            }
        }
    }
}