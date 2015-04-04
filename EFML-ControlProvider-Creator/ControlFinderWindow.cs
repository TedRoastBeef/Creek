using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EFML_ControlProvider_Creator
{
    public partial class ControlFinderWindow : Form
    {
        public ControlFinderWindow()
        {
            InitializeComponent();
        }

        private void ControlFinderWindow_Load(object sender, EventArgs e)
        {
            var g = new GlobalAssembly();
            
            foreach (Type a in g.a.GetTypes())
            {
                listBox1.Items.Add(a.FullName);
            }
        }
    }
}
