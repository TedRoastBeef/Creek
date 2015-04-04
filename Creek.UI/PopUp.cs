using System;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
    public class Popup : Form
    {
        private readonly Timer timer;
        private int startPosX;
        private int startPosY;

        public Popup()
        {
            InitializeComponent();
            // We want our window to be the top most
            TopMost = true;
            // Pop doesn't need to be shown in task bar
            ShowInTaskbar = false;
            // Create and run timer for animation
            timer = new Timer {Interval = 25};
            timer.Tick += timer_Tick;
        }

        protected override void OnLoad(EventArgs e)
        {
            // Move window out of screen
            startPosX = Screen.PrimaryScreen.WorkingArea.Width - Width;
            startPosY = Screen.PrimaryScreen.WorkingArea.Height;
            SetDesktopLocation(startPosX, startPosY);
            base.OnLoad(e);
            // Begin animation
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //Lift window by 5 pixels
            startPosY -= 5;
            //If window is fully visible stop the timer
            if (startPosY < Screen.PrimaryScreen.WorkingArea.Height - (Height + 3))
                timer.Stop();
            else
                SetDesktopLocation(startPosX, startPosY);
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // Popup
            // 
            ClientSize = new Size(284, 93);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Popup";
            Text = "Popup";
            ResumeLayout(false);
        }
    }
}