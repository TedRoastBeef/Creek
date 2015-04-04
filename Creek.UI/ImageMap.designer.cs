using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI
{
	/// <summary>
	/// Summary description for ImageMap.
	/// </summary>
	[ToolboxBitmap(typeof(ImageMap))]
	public partial class ImageMap : System.Windows.Forms.UserControl
	{
		private System.Drawing.Drawing2D.GraphicsPath _pathData;
		private int _activeIndex = -1;
		private ArrayList _pathsArray;
		private ToolTip _toolTip;
		private Graphics _graphics;

		private System.Windows.Forms.PictureBox pictureBox;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;


		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(150, 150);
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
			this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
			this.pictureBox.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
			// 
			// ImageMap
			// 
			this.Controls.AddRange(new System.Windows.Forms.Control[] {
																		 pictureBox});
			this.Name = "ImageMap";
			this.ResumeLayout(false);

		}
		#endregion

		private void pictureBox_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			int newIndex =getActiveIndexAtPoint(new Point(e.X, e.Y));
			if(newIndex > -1)
			{
				pictureBox.Cursor = Cursors.Hand;
				if(this._activeIndex != newIndex)
					this._toolTip.SetToolTip(this.pictureBox,_pathsArray[newIndex].ToString());
                RegionHover(newIndex, _pathsArray[newIndex].ToString());
			}
			else
			{
				pictureBox.Cursor = Cursors.Default;
				this._toolTip.RemoveAll();
			}
			this._activeIndex = newIndex;
		}

		private void pictureBox_MouseLeave(object sender, System.EventArgs e)
		{
			this._activeIndex = -1;
			this.Cursor = Cursors.Default;
		}

		private void pictureBox_Click(object sender, System.EventArgs e)
		{
			Point p =PointToClient(Cursor.Position);
			if(this._activeIndex == -1)
               getActiveIndexAtPoint(p);
			if(this._activeIndex > -1 &&RegionClick != null)
				this.RegionClick(this._activeIndex,_pathsArray[this._activeIndex].ToString());
		}

		private int getActiveIndexAtPoint(Point point)
		{
			System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
			System.Drawing.Drawing2D.GraphicsPathIterator iterator = new System.Drawing.Drawing2D.GraphicsPathIterator(_pathData);
			iterator.Rewind();
			for(int current=0; current < iterator.SubpathCount; current++)
			{
				iterator.NextMarker(path);
				if(path.IsVisible(point,_graphics))
					return current;
			}
			return -1;
		}

		[Browsable(false)]
		public override Image BackgroundImage
		{
			get
			{
				return base.BackgroundImage;
			}
			set
			{
				base.BackgroundImage = value;
			}
		}

	    [Category("Appearance")]
	    public Image Image
	    {
	        get
	        {
	            return pictureBox.Image;
	        }
	        set
	        {
	           pictureBox.Image = value;
	        }
	    }
	}
}
