using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace Creek.UI.Unity3.Controls
{
    public partial class CustomColorPicker : UserControl, IColorPicker
    {
        #region Class Variables

        public enum eDrawStyle
        {
            Hue,
            Saturation,
            Brightness,
            Red,
            Green,
            Blue
        }

        private ColorManager.CMYK m_cmyk;

        private ColorManager.HSL m_hsl;
        private Color m_rgb;

        #endregion

        private bool isUpdating;

        public CustomColorPicker(Color color)
        {
            InitializeComponent();
            lblOriginalColor.BackColor = color;
            rbHue.Checked = true;
            tbAlpha.Value = 255 - color.A;
            UpdateUI(color);
        }

        #region IColorPicker Members

        public Color Color
        {
            get { return Color.FromArgb(255 - tbAlpha.Value, m_rgb); }
            set
            {
                m_rgb = value;
                tbAlpha.Value = 255 - m_rgb.A;
                UpdateUI(value);
            }
        }

        #endregion

        private void UpdateUI(Color color)
        {
            isUpdating = true;
            m_rgb = color;
            m_hsl = ColorManager.RGB_to_HSL(m_rgb);
            m_cmyk = ColorManager.RGB_to_CMYK(m_rgb);

            txtHue.Text = Round(m_hsl.H*360).ToString();
            txtSat.Text = Round(m_hsl.S*100).ToString();
            txtBrightness.Text = Round(m_hsl.L*100).ToString();
            txtRed.Text = m_rgb.R.ToString();
            txtGreen.Text = m_rgb.G.ToString();
            txtBlue.Text = m_rgb.B.ToString();

            colorBox.HSL = m_hsl;
            colorSlider.HSL = m_hsl;

            colorPanelPending.Color = Color.FromArgb(255 - tbAlpha.Value, m_rgb);

            WriteHexData(m_rgb);
            isUpdating = false;
        }

        private void tbAlpha_ValueChanged(object sender, EventArgs e)
        {
            colorPanelPending.Color = Color.FromArgb(255 - tbAlpha.Value, m_rgb);
        }

        #region Events

        private void colorBox_Scroll(object sender, EventArgs e)
        {
            if (!isUpdating)
                UpdateUI(colorBox.RGB);
        }

        private void colorSlider_Scroll(object sender, EventArgs e)
        {
            if (!isUpdating)
                UpdateUI(colorSlider.RGB);
        }

        #region Hex Box (m_txt_Hex)

        private void txtHex_Leave(object sender, EventArgs e)
        {
            string text = ""; //m_txt_Hex.Text.ToUpper();
            bool has_illegal_chars = false;

            if (text.Length <= 0)
                has_illegal_chars = true;
            foreach (char letter in text)
            {
                if (!char.IsNumber(letter))
                {
                    if (letter >= 'A' && letter <= 'F')
                        continue;
                    has_illegal_chars = true;
                    break;
                }
            }

            if (has_illegal_chars)
            {
                MessageBox.Show("Hex must be a hex value between 0x000000 and 0xFFFFFF");
                WriteHexData(m_rgb);
                return;
            }

            UpdateUI(ParseHexData(text));
        }

        #endregion

        #region Color Boxes

        private void lblOriginalColor_Click(object sender, EventArgs e)
        {
        }


        private void m_lbl_Secondary_Color_Click(object sender, EventArgs e)
        {
        }

        #endregion

        #region Radio Buttons

        private void rbHue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbHue.Checked)
            {
                colorSlider.DrawStyle = VerticalColorSlider.eDrawStyle.Hue;
                colorBox.DrawStyle = ColorBox.eDrawStyle.Hue;
            }
        }


        private void rbSat_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSat.Checked)
            {
                colorSlider.DrawStyle = VerticalColorSlider.eDrawStyle.Saturation;
                colorBox.DrawStyle = ColorBox.eDrawStyle.Saturation;
            }
        }


        private void rbBrightness_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBrightness.Checked)
            {
                colorSlider.DrawStyle = VerticalColorSlider.eDrawStyle.Brightness;
                colorBox.DrawStyle = ColorBox.eDrawStyle.Brightness;
            }
        }


        private void rbRed_CheckedChanged(object sender, EventArgs e)
        {
            if (rbRed.Checked)
            {
                colorSlider.DrawStyle = VerticalColorSlider.eDrawStyle.Red;
                colorBox.DrawStyle = ColorBox.eDrawStyle.Red;
            }
        }


        private void rbGreen_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGreen.Checked)
            {
                colorSlider.DrawStyle = VerticalColorSlider.eDrawStyle.Green;
                colorBox.DrawStyle = ColorBox.eDrawStyle.Green;
            }
        }


        private void rbBlue_CheckedChanged(object sender, EventArgs e)
        {
            if (rbBlue.Checked)
            {
                colorSlider.DrawStyle = VerticalColorSlider.eDrawStyle.Blue;
                colorBox.DrawStyle = ColorBox.eDrawStyle.Blue;
            }
        }

        #endregion

        #region Text Boxes

        private void txtHue_Leave(object sender, EventArgs e)
        {
            string text = txtHue.Text;
            bool has_illegal_chars = false;

            if (text.Length < 1 || text.Length > 3)
                has_illegal_chars = true;
            else
                foreach (char letter in text)
                {
                    if (!char.IsNumber(letter))
                    {
                        has_illegal_chars = true;
                        break;
                    }
                }

            if (has_illegal_chars)
            {
                MessageBox.Show("Hue must be a number value between 0 and 360");
                txtHue.Text = Round(m_hsl.H*360).ToString();
                return;
            }

            int hue = int.Parse(text);

            if (hue < 0 || hue > 360)
            {
                MessageBox.Show("An integer between 0 and 360 is required.\nClosest value inserted.");
                hue = hue < 0 ? 0 : 360;
            }

            m_hsl.H = (double) hue/360;

            UpdateUI(ColorManager.HSL_to_RGB(m_hsl));
        }


        private void txtSat_Leave(object sender, EventArgs e)
        {
            string text = txtSat.Text;
            bool has_illegal_chars = false;

            if (text.Length < 1 || text.Length > 3)
                has_illegal_chars = true;
            else
                foreach (char letter in text)
                {
                    if (!char.IsNumber(letter))
                    {
                        has_illegal_chars = true;
                        break;
                    }
                }

            if (has_illegal_chars)
            {
                MessageBox.Show("Saturation must be a number value between 0 and 100");
                txtSat.Text = Round(m_hsl.S*100).ToString();
                return;
            }

            int sat = int.Parse(text);

            if (sat < 0 || sat > 100)
            {
                MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
                sat = sat < 0 ? 0 : 100;
            }

            m_hsl.S = (double) sat/100;
            UpdateUI(ColorManager.HSL_to_RGB(m_hsl));
        }


        private void txtBrightness_Leave(object sender, EventArgs e)
        {
            string text = txtBrightness.Text;
            bool has_illegal_chars = false;

            if (text.Length < 1 || text.Length > 3)
                has_illegal_chars = true;
            else
                foreach (char letter in text)
                {
                    if (!char.IsNumber(letter))
                    {
                        has_illegal_chars = true;
                        break;
                    }
                }

            if (has_illegal_chars)
            {
                MessageBox.Show("Black must be a number value between 0 and 360");
                txtBrightness.Text = Round(m_hsl.L*100).ToString();
                return;
            }

            int lum = int.Parse(text);

            if (lum < 0 || lum > 360)
            {
                MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
                lum = lum < 0 ? 0 : 360;
            }

            m_hsl.L = (double) lum/100;

            UpdateUI(ColorManager.HSL_to_RGB(m_hsl));
        }


        private void txtRed_Leave(object sender, EventArgs e)
        {
            string text = txtRed.Text;
            bool has_illegal_chars = false;

            if (text.Length < 1 || text.Length > 3)
                has_illegal_chars = true;
            else
                foreach (char letter in text)
                {
                    if (!char.IsNumber(letter))
                    {
                        has_illegal_chars = true;
                        break;
                    }
                }

            if (has_illegal_chars)
            {
                MessageBox.Show("Red must be a number value between 0 and 255");
                txtRed.Text = m_rgb.R.ToString();
                return;
            }

            int red = int.Parse(text);

            if (red < 0 || red > 255)
            {
                MessageBox.Show("An integer between 0 and 255 is required.\nClosest value inserted.");
                red = red < 0 ? 0 : 255;
            }

            m_rgb = Color.FromArgb(red, m_rgb.G, m_rgb.B);
            UpdateUI(m_rgb);
        }


        private void txtGreen_Leave(object sender, EventArgs e)
        {
            string text = txtGreen.Text;
            bool has_illegal_chars = false;

            if (text.Length < 1 || text.Length > 3)
                has_illegal_chars = true;
            else
                foreach (char letter in text)
                {
                    if (!char.IsNumber(letter))
                    {
                        has_illegal_chars = true;
                        break;
                    }
                }

            if (has_illegal_chars)
            {
                MessageBox.Show("Green must be a number value between 0 and 255");
                txtGreen.Text = m_rgb.G.ToString();
                return;
            }

            int green = int.Parse(text);

            if (green < 0 || green > 255)
            {
                MessageBox.Show("An integer between 0 and 255 is required.\nClosest value inserted.");
                green = green < 0 ? 0 : 255;
            }

            m_rgb = Color.FromArgb(m_rgb.R, green, m_rgb.B);
            UpdateUI(m_rgb);
        }


        private void txtBlue_Leave(object sender, EventArgs e)
        {
            string text = txtBlue.Text;
            bool has_illegal_chars = false;

            if (text.Length < 1 || text.Length > 3)
                has_illegal_chars = true;
            else
                foreach (char letter in text)
                {
                    if (!char.IsNumber(letter))
                    {
                        has_illegal_chars = true;
                        break;
                    }
                }

            if (has_illegal_chars)
            {
                MessageBox.Show("Blue must be a number value between 0 and 255");
                txtBlue.Text = m_rgb.B.ToString();
                return;
            }

            int blue = int.Parse(text);

            if (blue < 0 || blue > 255)
            {
                MessageBox.Show("An integer between 0 and 255 is required.\nClosest value inserted.");
                blue = blue < 0 ? 0 : 255;
            }

            m_rgb = Color.FromArgb(m_rgb.R, m_rgb.G, blue);
            UpdateUI(m_rgb);
        }


        //private void m_txt_Cyan_Leave(object sender, System.EventArgs e)
        //{
        //    string text = "";// m_txt_Cyan.Text;
        //    bool has_illegal_chars = false;

        //    if (text.Length <= 0)
        //        has_illegal_chars = true;
        //    else
        //        foreach (char letter in text)
        //        {
        //            if (!char.IsNumber(letter))
        //            {
        //                has_illegal_chars = true;
        //                break;
        //            }
        //        }

        //    if (has_illegal_chars)
        //    {
        //        MessageBox.Show("Cyan must be a number value between 0 and 100");
        //        UpdateTextBoxes();
        //        return;
        //    }

        //    int cyan = int.Parse(text);

        //    if (cyan < 0)
        //    {
        //        MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
        //        m_cmyk.C = 0.0;
        //    }
        //    else if (cyan > 100)
        //    {
        //        MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
        //        m_cmyk.C = 1.0;
        //    }
        //    else
        //    {
        //        m_cmyk.C = (double)cyan / 100;
        //    }

        //    m_rgb = ColorManager.CMYK_to_RGB(m_cmyk);
        //    m_hsl = ColorManager.RGB_to_HSL(m_rgb);
        //    colorBox.HSL = m_hsl;
        //    colorSlider.HSL = m_hsl;
        //    m_lbl_Primary_Color.BackColor = m_rgb;

        //    UpdateTextBoxes();
        //}
        //private void m_txt_Magenta_Leave(object sender, System.EventArgs e)
        //{
        //    string text = "";// m_txt_Magenta.Text;
        //    bool has_illegal_chars = false;

        //    if (text.Length <= 0)
        //        has_illegal_chars = true;
        //    else
        //        foreach (char letter in text)
        //        {
        //            if (!char.IsNumber(letter))
        //            {
        //                has_illegal_chars = true;
        //                break;
        //            }
        //        }

        //    if (has_illegal_chars)
        //    {
        //        MessageBox.Show("Magenta must be a number value between 0 and 100");
        //        UpdateTextBoxes();
        //        return;
        //    }

        //    int magenta = int.Parse(text);

        //    if (magenta < 0)
        //    {
        //        MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
        //        m_txt_Magenta.Text = "0";
        //        m_cmyk.M = 0.0;
        //    }
        //    else if (magenta > 100)
        //    {
        //        MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
        //        m_txt_Magenta.Text = "100";
        //        m_cmyk.M = 1.0;
        //    }
        //    else
        //    {
        //        m_cmyk.M = (double)magenta / 100;
        //    }

        //    m_rgb = ColorManager.CMYK_to_RGB(m_cmyk);
        //    m_hsl = ColorManager.RGB_to_HSL(m_rgb);
        //    colorBox.HSL = m_hsl;
        //    colorSlider.HSL = m_hsl;
        //    m_lbl_Primary_Color.BackColor = m_rgb;

        //    UpdateTextBoxes();
        //}
        //private void m_txt_Yellow_Leave(object sender, System.EventArgs e)
        //{
        //    string text = m_txt_Yellow.Text;
        //    bool has_illegal_chars = false;

        //    if (text.Length <= 0)
        //        has_illegal_chars = true;
        //    else
        //        foreach (char letter in text)
        //        {
        //            if (!char.IsNumber(letter))
        //            {
        //                has_illegal_chars = true;
        //                break;
        //            }
        //        }

        //    if (has_illegal_chars)
        //    {
        //        MessageBox.Show("Yellow must be a number value between 0 and 100");
        //        UpdateTextBoxes();
        //        return;
        //    }

        //    int yellow = int.Parse(text);

        //    if (yellow < 0)
        //    {
        //        MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
        //        m_txt_Yellow.Text = "0";
        //        m_cmyk.Y = 0.0;
        //    }
        //    else if (yellow > 100)
        //    {
        //        MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
        //        m_txt_Yellow.Text = "100";
        //        m_cmyk.Y = 1.0;
        //    }
        //    else
        //    {
        //        m_cmyk.Y = (double)yellow / 100;
        //    }

        //    m_rgb = ColorManager.CMYK_to_RGB(m_cmyk);
        //    m_hsl = ColorManager.RGB_to_HSL(m_rgb);
        //    colorBox.HSL = m_hsl;
        //    colorSlider.HSL = m_hsl;
        //    m_lbl_Primary_Color.BackColor = m_rgb;

        //    UpdateTextBoxes();
        //}
        //private void m_txt_K_Leave(object sender, System.EventArgs e)
        //{
        //    string text = m_txt_K.Text;
        //    bool has_illegal_chars = false;

        //    if (text.Length <= 0)
        //        has_illegal_chars = true;
        //    else
        //        foreach (char letter in text)
        //        {
        //            if (!char.IsNumber(letter))
        //            {
        //                has_illegal_chars = true;
        //                break;
        //            }
        //        }

        //    if (has_illegal_chars)
        //    {
        //        MessageBox.Show("Key must be a number value between 0 and 100");
        //        UpdateTextBoxes();
        //        return;
        //    }

        //    int key = int.Parse(text);

        //    if (key < 0)
        //    {
        //        MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
        //        m_txt_K.Text = "0";
        //        m_cmyk.K = 0.0;
        //    }
        //    else if (key > 100)
        //    {
        //        MessageBox.Show("An integer between 0 and 100 is required.\nClosest value inserted.");
        //        m_txt_K.Text = "100";
        //        m_cmyk.K = 1.0;
        //    }
        //    else
        //    {
        //        m_cmyk.K = (double)key / 100;
        //    }

        //    m_rgb = ColorManager.CMYK_to_RGB(m_cmyk);
        //    m_hsl = ColorManager.RGB_to_HSL(m_rgb);
        //    colorBox.HSL = m_hsl;
        //    colorSlider.HSL = m_hsl;
        //    m_lbl_Primary_Color.BackColor = m_rgb;

        //    UpdateTextBoxes();
        //}

        #endregion

        #endregion

        #region Private Functions

        private int Round(double val)
        {
            var ret_val = (int) val;

            var temp = (int) (val*100);

            if ((temp%100) >= 50)
                ret_val += 1;

            return ret_val;
        }


        private void WriteHexData(Color rgb)
        {
            //string red = Convert.ToString(rgb.R, 16);
            //if (red.Length < 2) red = "0" + red;
            //string green = Convert.ToString(rgb.G, 16);
            //if (green.Length < 2) green = "0" + green;
            //string blue = Convert.ToString(rgb.B, 16);
            //if (blue.Length < 2) blue = "0" + blue;

            //m_txt_Hex.Text = red.ToUpper() + green.ToUpper() + blue.ToUpper();
            //m_txt_Hex.Update();
        }


        private Color ParseHexData(string hex_data)
        {
            if (hex_data.Length != 6)
                return Color.Black;

            string r_text, g_text, b_text;
            int r, g, b;

            r_text = hex_data.Substring(0, 2);
            g_text = hex_data.Substring(2, 2);
            b_text = hex_data.Substring(4, 2);

            r = int.Parse(r_text, NumberStyles.HexNumber);
            g = int.Parse(g_text, NumberStyles.HexNumber);
            b = int.Parse(b_text, NumberStyles.HexNumber);

            return Color.FromArgb(r, g, b);
        }


        //private void UpdateTextBoxes()
        //{
        //    txtHue.Text = Round(m_hsl.H * 360).ToString();
        //    txtSat.Text = Round(m_hsl.S * 100).ToString();
        //    txtBrightness.Text = Round(m_hsl.L * 100).ToString();
        //    //m_txt_Cyan.Text = Round(m_cmyk.C * 100).ToString();
        //    //m_txt_Magenta.Text = Round(m_cmyk.M * 100).ToString();
        //    //m_txt_Yellow.Text = Round(m_cmyk.Y * 100).ToString();
        //    //m_txt_K.Text = Round(m_cmyk.K * 100).ToString();
        //    txtRed.Text = m_rgb.R.ToString();
        //    txtGreen.Text = m_rgb.G.ToString();
        //    txtBlue.Text = m_rgb.B.ToString();

        //    //m_txt_Red.Update();
        //    //m_txt_Green.Update();
        //    //m_txt_Blue.Update();
        //    //m_txt_Hue.Update();
        //    //m_txt_Sat.Update();
        //    //m_txt_Brightness.Update();
        //    //m_txt_Cyan.Update();
        //    //m_txt_Magenta.Update();
        //    //m_txt_Yellow.Update();
        //    //m_txt_K.Update();

        //    WriteHexData(m_rgb);
        //}

        #endregion

        #region Public Methods

        public Color PrimaryColor
        {
            get { return m_rgb; }
            set { UpdateUI(value); }
        }


        public eDrawStyle DrawStyle
        {
            get
            {
                if (rbHue.Checked)
                    return eDrawStyle.Hue;
                else if (rbSat.Checked)
                    return eDrawStyle.Saturation;
                else if (rbBrightness.Checked)
                    return eDrawStyle.Brightness;
                else if (rbRed.Checked)
                    return eDrawStyle.Red;
                else if (rbGreen.Checked)
                    return eDrawStyle.Green;
                else if (rbBlue.Checked)
                    return eDrawStyle.Blue;
                else
                    return eDrawStyle.Hue;
            }
            set
            {
                switch (value)
                {
                    case eDrawStyle.Hue:
                        rbHue.Checked = true;
                        break;
                    case eDrawStyle.Saturation:
                        rbSat.Checked = true;
                        break;
                    case eDrawStyle.Brightness:
                        rbBrightness.Checked = true;
                        break;
                    case eDrawStyle.Red:
                        rbRed.Checked = true;
                        break;
                    case eDrawStyle.Green:
                        rbGreen.Checked = true;
                        break;
                    case eDrawStyle.Blue:
                        rbBlue.Checked = true;
                        break;
                    default:
                        rbHue.Checked = true;
                        break;
                }
            }
        }

        #endregion
    }
}