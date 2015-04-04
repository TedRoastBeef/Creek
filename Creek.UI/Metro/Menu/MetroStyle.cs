namespace Creek.UI.Metro.Menu
{
    using System.ComponentModel;
    using System.Drawing;

    /// <summary>
    /// MetroStyle-Konfiguration
    /// </summary>
    public class MetroStyle : INotifyPropertyChanged
    {
        private Font _baseFont;
        private Font _boldFont;
        private Font _lightFont;
        private Color _backColor;
        private Color _foreColor;
        private Color _accentColor;
        private Color _accentFrontColor;
        private Color _disabledColor;
        private bool _darkStyle;

        /// <summary>
        /// Font for Standard-Output
        /// </summary>
        public Font BaseFont
        {
            get { return this._baseFont; }
            set
            {
                if (value.Equals(this._baseFont) == false)
                {
                    this._baseFont = value;
                    this.OnPropertyChanged("BaseFont");
                }
            }
        }
        /// <summary>
        /// Font for Bold-Output
        /// </summary>
        public Font BoldFont
        {
            get { return this._boldFont; }
            set
            {
                if (value.Equals(this._boldFont) == false)
                {
                    this._boldFont = value;
                    this.OnPropertyChanged("BoldFont");
                }
            }
        }
        /// <summary>
        /// Font for Light-Output (thinner)
        /// </summary>
        public Font LightFont
        {
            get { return this._lightFont; }
            set
            {
                if (value.Equals(this._lightFont) == false)
                {
                    this._lightFont = value;
                    this.OnPropertyChanged("LightFont");
                }
            }
        }

        /// <summary>
        /// Backgroundcolor
        /// </summary>
        public Color BackColor
        {
            get { return this._backColor; }
            set
            {
                if (value.Equals(this._backColor) == false)
                {
                    this._backColor = value;
                    this.OnPropertyChanged("BackColor");
                }
            }
        }
        /// <summary>
        /// ForeColor
        /// </summary>
        public Color ForeColor
        {
            get { return this._foreColor; }
            set
            {
                if (value.Equals(this._foreColor) == false)
                {
                    this._foreColor = value;
                    this.OnPropertyChanged("ForeColor");
                }
            }
        }
        /// <summary>
        /// Accented Elements
        /// </summary>
        public Color AccentColor
        {
            get { return this._accentColor; }
            set
            {
                if (value.Equals(this._accentColor) == false)
                {
                    this._accentColor = value;
                    this.OnPropertyChanged("AccentColor");
                }
            }
        }
        /// <summary>
        /// Accented Elements ForeColor
        /// </summary>
        public Color AccentFrontColor
        {
            get { return this._accentFrontColor; }
            set
            {
                if (value.Equals(this._accentFrontColor) == false)
                {
                    this._accentFrontColor = value;
                    this.OnPropertyChanged("AccentFrontColor");
                }
            }
        }
        /// <summary>
        /// Disabled Elements
        /// </summary>
        public Color DisabledColor
        {
            get { return this._disabledColor; }
            set
            {
                if (value.Equals(this._disabledColor) == false)
                {
                    this._disabledColor = value;
                    this.OnPropertyChanged("DisabledColor");
                }
            }
        }

        /// <summary>
        /// true if the application uses the 'Dark'-Style
        /// </summary>
        public bool DarkStyle
        {
            get { return this._darkStyle; }
            set
            {
                if (value != this._darkStyle)
                {
                    this._darkStyle = value;
                    if (this._darkStyle)
                    {
                        this.BackColor = Color.FromArgb(51, 51, 51);
                        this.ForeColor = Color.White;
                        this.AccentColor = Color.DarkOrange;
                        this.AccentFrontColor = Color.FromArgb(51, 51, 51);
                        this.DisabledColor = Color.DimGray;
                    }
                    else
                    {
                        this.BackColor = Color.White;
                        this.ForeColor = Color.FromArgb(51, 51, 51);
                        this.AccentColor = Color.FromArgb(0, 114, 198);
                        this.AccentFrontColor = Color.White;
                        this.DisabledColor = Color.DimGray;
                    }

                    this.OnPropertyChanged("DarkStyle");
                }
            }
        }
 
        /// <summary>
        /// support the INotifyPropertyChanged Interface
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
       
        /// <summary>
        /// support the INotifyPropertyChanged Interface
        /// </summary>
        /// <param name="property">property name</param>
        public void OnPropertyChanged(string property)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
       
        /// <summary>
        /// Initializes the visual properties
        /// </summary>
        public MetroStyle()
        {
            this.BaseFont = new Font("Segoe UI", 11.25f);
            this.BoldFont = new Font("Segoe UI", 11.25f, FontStyle.Bold);
            this.LightFont = new Font("Segoe UI Light", 11.25f, FontStyle.Regular);

            this.DarkStyle = false;
        }
    }
}
