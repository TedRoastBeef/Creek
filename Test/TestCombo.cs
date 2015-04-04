using System.ComponentModel;
using System.Drawing;
using Creek.UI;
using Creek.UI.Unity3.Controls;

namespace Test
{
    public class TestCombo : DropDownControl
    {
        private RatingBar ratingBar1;

        public TestCombo()
        {
            InitializeComponent();
            InitializeDropDown(ratingBar1);
        }

        private void InitializeComponent()
        {
            var resources = new ComponentResourceManager(typeof (TestCombo));
            ratingBar1 = new RatingBar();
            SuspendLayout();
            // 
            // ratingBar1
            // 
            ratingBar1.BarBackColor = Color.Black;
            ratingBar1.Gap = ((1));
            ratingBar1.IconEmpty = ((Image) (resources.GetObject("ratingBar1.IconEmpty")));
            ratingBar1.IconFull = ((Image) (resources.GetObject("ratingBar1.IconFull")));
            ratingBar1.IconHalf = ((Image) (resources.GetObject("ratingBar1.IconHalf")));
            ratingBar1.IconsCount = ((10));
            ratingBar1.Location = new Point(3, 30);
            ratingBar1.Name = "ratingBar1";
            ratingBar1.Rate = 0F;
            ratingBar1.Size = new Size(167, 44);
            ratingBar1.TabIndex = 0;
            ratingBar1.Text = "ratingBar1";
            ratingBar1.RateChanged += ratingBar1_RateChanged;
            // 
            // TestCombo
            // 
            AnchorSize = new Size(173, 21);
            AutoScaleDimensions = new SizeF(6F, 13F);
            Controls.Add(ratingBar1);
            Name = "TestCombo";
            Size = new Size(173, 150);
            ResumeLayout(false);
        }

        private void ratingBar1_RateChanged(object sender, RatingBarRateEventArgs e)
        {
            Text = e.NewRate.ToString();

            CloseDropDown();
        }
    }
}