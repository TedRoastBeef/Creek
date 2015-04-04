using System;
using System.Windows.Forms;

namespace Test
{
    public partial class Map : Form
    {
        public Map()
        {
            InitializeComponent();
        }

        private void Map_Load(object sender, EventArgs e)
        {
            imageMap1.FromXml("<map><rectangle key='chris' x='0' y='0' w='180' h='180' /></map>");
            //  imageMap1.AddRectangle("Chris", new Rectangle(new Point(0, 0), new Size(180, 180)));
        }

        private void imageMap1_RegionHover(int index, string key)
        {
        }

        private void imageMap1_RegionClick(int index, string key)
        {
            MessageBox.Show(key);
        }
    }
}