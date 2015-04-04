using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Creek.UI.Popups
{
    public class FacebookPopup : Popup
    {
        private Label label1;
        private Label label2;
        private Label label3;
        private Line line1;
        private PictureBox pictureBox1;

        public FacebookPopup()
        {
            InitializeComponent();

            BackColor = Color.FromArgb(59, 89, 152);
        }

        public static void ShowPopup(string caption, string content, Image icon)
        {
            var p = new FacebookPopup
                        {pictureBox1 = {Image = icon}, label1 = {Text = caption}, label2 = {Text = content}};
            p.Show();
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            label3 = new Label();
            line1 = new Line();
            ((ISupportInitialize) (pictureBox1)).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
            label1.ForeColor = Color.White;
            label1.Location = new Point(106, 3);
            label1.Name = "label1";
            label1.Size = new Size(59, 23);
            label1.TabIndex = 0;
            label1.Text = "label1";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(13, 33);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(50, 50);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
            label2.ForeColor = Color.White;
            label2.Location = new Point(69, 33);
            label2.Name = "label2";
            label2.Size = new Size(39, 14);
            label2.TabIndex = 2;
            label2.Text = "label2";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, ((0)));
            label3.ForeColor = Color.White;
            label3.Location = new Point(263, 10);
            label3.Name = "label3";
            label3.Size = new Size(14, 14);
            label3.TabIndex = 3;
            label3.Text = "X";
            label3.Click += label3_Click;
            label3.MouseEnter += label3_MouseEnter;
            label3.MouseLeave += label3_MouseLeave;
            // 
            // line1
            // 
            line1.BorderColor = Color.White;
            line1.Dark3dColor = SystemColors.ControlDark;
            line1.DashStyle = DashStyle.Dash;
            line1.Light3dColor = Color.White;
            line1.Location = new Point(0, 27);
            line1.Name = "line1";
            line1.Size = new Size(285, 1);
            line1.TabIndex = 4;
            line1.TabStop = false;
            // 
            // FacebookPopup
            // 
            ClientSize = new Size(284, 93);
            Controls.Add(line1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Name = "FacebookPopup";
            ((ISupportInitialize) (pictureBox1)).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void label3_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }
    }
}