using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    /// <summary>
    /// Combobox mit den Systemfarben (KnownColors)
    /// </summary>
    [ToolboxBitmap(typeof (ComboBox))]
    public class ComboBoxColors : ComboBox
    {
        private Color m_DefaultColor = Color.Aqua;

        public ComboBoxColors()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            DropDownStyle = ComboBoxStyle.DropDownList;
        }

        [Description("dargestellte Farbe bei Programmstart"), Category("Darstellung")]
        public Color DefaultColor
        {
            get { return m_DefaultColor; }
            set { m_DefaultColor = value; }
        }

        private void LoadColors()
        {
            // Die KnownColors auslesen
            string[] ColorNames = Enum.GetNames(typeof (KnownColor));
            for (int i = 27; i <= ColorNames.GetUpperBound(0) - 7; i++)
            {
                Items.Add(ColorNames[i]);
            }
        }

        /// <summary>
        /// Eintrag in Combo auswählen über die Bezeichnung der Farbe
        /// </summary>
        /// <param name="ColorName">Bezeichnung der Farbe</param>
        public void SelectColorByName(string ColorName)
        {
            for (int i = 0; i <= Items.Count - 1; i++)
            {
                if (ColorName.ToUpper() == Items[i].ToString().ToUpper())
                {
                    SelectedIndex = i;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
        }

        /// <summary>
        /// Eintrag in Combo auswählen über die Farbe
        /// </summary>
        /// <param name="Color">die Farbe</param>
        public void SelectColor(Color Color)
        {
            for (int i = 0; i <= Items.Count - 1; i++)
            {
                if (Color.Name == Items[i].ToString())
                {
                    SelectedIndex = i;
                    break; // TODO: might not be correct. Was : Exit For
                }
            }
        }

        /// <summary>
        /// aktuell ausgewählte Farbe abrufen
        /// </summary>
        public Color SelectedColor()
        {
            Color c = Color.Transparent;
            if (SelectedIndex >= 0)
            {
                c = Color.FromName(SelectedItem.ToString());
            }
            return c;
        }

        /// <summary>
        /// Bezeichnung der aktuell ausgewählten Farbe abrufen
        /// </summary>
        public string SelectedColorName()
        {
            string s = null;
            if (SelectedIndex >= 0)
            {
                s = SelectedItem.ToString();
            }
            return s;
        }

        /// <summary>
        /// (Erst)Aktivierung der Parentform
        /// </summary>
        protected override void OnCreateControl()
        {
            // Die KnownColors auslesen
            if (!DesignMode)
            {
                LoadColors();
                // Die Defaultfarbe in der Combo auswählen
                if (DefaultColor != null)
                {
                    SelectColor(DefaultColor);
                }
            }

            base.OnCreateControl();
        }

        /// <summary>
        /// Ausklappen der Combo mit Anzeige der Items
        /// </summary>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index >= 0)
            {
                Graphics Gr = e.Graphics;
                float x = e.Bounds.Left;
                float y = e.Bounds.Top;
                float h = e.Bounds.Height;
                float w = h;

                // Farbe über Bezeichnung laden
                using (var SB = new SolidBrush(Color.FromName(Items[e.Index].ToString())))
                {
                    // Ein gefülltes Rechteck zeichnen
                    Gr.FillRectangle(SB, x + 2, y + 1, w, h - 4);
                    Gr.DrawRectangle(Pens.Black, x + 2, y + 1, w, h - 4);

                    // Selected Item Invers, Not Selected Normal
                    using (var BR = new SolidBrush(ForeColor))
                    {
                        if (e.State == DrawItemState.Selected)
                        {
                            SB.Color = Color.DarkBlue;
                            BR.Color = BackColor;
                        }
                        else
                        {
                            SB.Color = BackColor;
                        }
                        Gr.FillRectangle(SB, w + 10, y, e.Bounds.Width - (w + 10), h - 1);
                        Gr.DrawString(Items[e.Index].ToString(), Font, BR, w + 10, y);
                    }
                }
            }
        }
    }
}