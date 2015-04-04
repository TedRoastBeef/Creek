namespace Creek.UI.Metro.Menu
{
    /// <summary>
    /// Menustrip for ModernUI-GUIs
    /// </summary>
    public class MetroMenuStrip : System.Windows.Forms.MenuStrip
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public MetroMenuStrip()
            : base()
        {
            this.Renderer = new metroToolStripRenderer();
            this.Font = MetroUI.Style.BaseFont;
            this.ForeColor = MetroUI.Style.ForeColor;
        }

        /// <summary>
        /// OnItemAdded-Event we adjust the font and forecolor of this item
        /// </summary>
        /// <param name="e"></param>
        protected override void OnItemAdded(System.Windows.Forms.ToolStripItemEventArgs e)
        {
            base.OnItemAdded(e);

            e.Item.Font = MetroUI.Style.BaseFont;
            e.Item.ForeColor = MetroUI.Style.ForeColor;
        }
    }
}
