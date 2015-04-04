using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace Creek.UI
{
    using Creek.UI.Winforms.Properties;

    public class DonationButton : PictureBox
    {
        public DonationButton()
        {
            Image = Resources.donation_bg;
            Size = Image.Size;
        }

        [Category("Bitcoin")]
        public string Address { get; set; }

        [Category("Bitcoin")]
        public string Label { get; set; }

        [Category("Bitcoin")]
        public string Amount { get; set; }

        [Category("Bitcoin")]
        public string Message { get; set; }

        private string createUri()
        {
            return "bitcoin:" + Address + "?amount=" + Amount + "&label=" + Label +
                   "&message=" + Message;
        }

        private string createAddress()
        {
            return new WebClient().DownloadString("https://blockchain.info/api/receive/method=create&address=" + Address);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            Cursor = Cursors.Hand;
            Image = Resources.donation_bg1;

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            Image = Resources.donation_bg;
            base.OnMouseLeave(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            Image = Resources.donation_bg11;

            MessageBox.Show(createAddress());

            Process.Start(createUri());
            base.OnMouseClick(e);
        }
    }
}