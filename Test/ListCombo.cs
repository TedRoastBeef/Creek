using System;
using System.Drawing;
using System.Windows.Forms;
using Creek.UI.Unity3.Controls;

namespace Test
{
    public class ListCombo : DropDownControl
    {
        public ListBox listBox1;

        public ListCombo()
        {
            InitializeComponent();

            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;

            InitializeDropDown(listBox1);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Text = listBox1.SelectedItem.ToString();

            CloseDropDown();
        }

        private void InitializeComponent()
        {
            listBox1 = new ListBox();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Location = new Point(0, 23);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(160, 121);
            listBox1.TabIndex = 0;
            // 
            // ListCombo
            // 
            AnchorSize = new Size(160, 21);
            AutoScaleDimensions = new SizeF(6F, 13F);
            Controls.Add(listBox1);
            Name = "ListCombo";
            Size = new Size(160, 150);
            ResumeLayout(false);
        }
    }
}