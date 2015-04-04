using System;
using System.Windows.Forms;

namespace Test
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            pageNavigator1.Pages.Add(new TestPage());
            pageNavigator1.Pages.Add(new TestPage2());

            // pageNavigator1.RefreshPages();
        }
    }
}