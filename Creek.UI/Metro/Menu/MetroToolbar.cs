namespace Creek.UI.Metro.Menu
{
    using System.Windows.Forms;

    /// <summary>
    /// Toolstrip for ModernUI-GUIs
    /// </summary>
    public class MetroToolStrip : System.Windows.Forms.ToolStrip
    {
        /// <summary>
        /// Constructor 
        /// </summary>
        public MetroToolStrip()
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
        protected override void OnItemAdded(ToolStripItemEventArgs e)
        {
            e.Item.Font = MetroUI.Style.BaseFont;
            e.Item.ForeColor = MetroUI.Style.ForeColor;

            base.OnItemAdded(e);
        }
    }
}
