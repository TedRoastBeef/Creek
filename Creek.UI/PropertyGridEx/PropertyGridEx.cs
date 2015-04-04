using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Creek.UI.PropertyGridEx
{
    public partial class PropertyGridEx : PropertyGrid
    {
        #region "Protected variables and objects"

        // CustomPropertyCollection assigned to MyBase.SelectedObject
        protected bool bAutoSizeProperties;
        protected bool bDrawFlatToolbar;
        protected bool bShowCustomProperties;

        // CustomPropertyCollectionSet assigned to MyBase.SelectedObjects
        protected bool bShowCustomPropertiesSet;
        protected CustomPropertyCollection oCustomPropertyCollection;
        protected CustomPropertyCollectionSet oCustomPropertyCollectionSet;

        // Internal PropertyGrid Controls
        protected object oDocComment;
        protected Label oDocCommentDescription;
        protected Label oDocCommentTitle;
        protected object oHotCommands;
        protected FieldInfo oPropertyGridEntries;
        protected object oPropertyGridView;
        protected ToolStrip oToolStrip;

        // Properties variables

        #endregion

        #region "Public Functions"

        public PropertyGridEx()
        {
            // This call is required by the Windows Form Designer.
            InitializeComponent();

            // Add any initialization after the InitializeComponent() call.
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);

            // Initialize collections
            oCustomPropertyCollection = new CustomPropertyCollection();
            oCustomPropertyCollectionSet = new CustomPropertyCollectionSet();

            // Attach internal controls
            oPropertyGridView = base.GetType().BaseType.InvokeMember("gridView",
                                                                     BindingFlags.NonPublic | BindingFlags.GetField |
                                                                     BindingFlags.Instance, null, this, null);
            oHotCommands = base.GetType().BaseType.InvokeMember("hotcommands",
                                                                BindingFlags.NonPublic | BindingFlags.GetField |
                                                                BindingFlags.Instance, null, this, null);
            oToolStrip =
                (ToolStrip)
                base.GetType().BaseType.InvokeMember("toolStrip",
                                                     BindingFlags.NonPublic | BindingFlags.GetField |
                                                     BindingFlags.Instance, null, this, null);
            oDocComment = base.GetType().BaseType.InvokeMember("doccomment",
                                                               BindingFlags.NonPublic | BindingFlags.GetField |
                                                               BindingFlags.Instance, null, this, null);

            // Attach DocComment internal fields
            if (oDocComment != null)
            {
                oDocCommentTitle =
                    (Label)
                    oDocComment.GetType().InvokeMember("m_labelTitle",
                                                       BindingFlags.NonPublic | BindingFlags.GetField |
                                                       BindingFlags.Instance, null, oDocComment, null);
                oDocCommentDescription =
                    (Label)
                    oDocComment.GetType().InvokeMember("m_labelDesc",
                                                       BindingFlags.NonPublic | BindingFlags.GetField |
                                                       BindingFlags.Instance, null, oDocComment, null);
            }

            // Attach PropertyGridView internal fields
            if (oPropertyGridView != null)
            {
                oPropertyGridEntries = oPropertyGridView.GetType().GetField("allGridEntries",
                                                                            BindingFlags.NonPublic |
                                                                            BindingFlags.Instance |
                                                                            BindingFlags.DeclaredOnly);
            }

            // Apply Toolstrip style
            if (oToolStrip != null)
            {
                ApplyToolStripRenderMode(bDrawFlatToolbar);
            }
        }

        public void MoveSplitterTo(int x)
        {
            oPropertyGridView.GetType().InvokeMember("MoveSplitterTo",
                                                     BindingFlags.InvokeMethod | BindingFlags.NonPublic |
                                                     BindingFlags.Instance, null, oPropertyGridView, new object[] {x});
        }

        public override void Refresh()
        {
            if (bShowCustomPropertiesSet)
            {
                base.SelectedObjects = (object[]) oCustomPropertyCollectionSet.ToArray();
            }
            base.Refresh();
            if (bAutoSizeProperties)
            {
                AutoSizeSplitter(32);
            }
        }

        public void SetComment(string title, string description)
        {
            MethodInfo method = oDocComment.GetType().GetMethod("SetComment");
            method.Invoke(oDocComment, new object[] {title, description});
            //oDocComment.SetComment(title, description);
        }

        #endregion

        #region "Protected Functions"

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (bAutoSizeProperties)
            {
                AutoSizeSplitter(32);
            }
        }

        protected void AutoSizeSplitter(int RightMargin)
        {
            var oItemCollection = (GridItemCollection) oPropertyGridEntries.GetValue(oPropertyGridView);
            if (oItemCollection == null)
            {
                return;
            }
            Graphics oGraphics = Graphics.FromHwnd(Handle);
            int CurWidth = 0;
            int MaxWidth = 0;

            foreach (GridItem oItem in oItemCollection)
            {
                if (oItem.GridItemType == GridItemType.Property)
                {
                    CurWidth = (int) oGraphics.MeasureString(oItem.Label, Font).Width + RightMargin;
                    if (CurWidth > MaxWidth)
                    {
                        MaxWidth = CurWidth;
                    }
                }
            }

            MoveSplitterTo(MaxWidth);
        }

        protected void ApplyToolStripRenderMode(bool value)
        {
            if (value)
            {
                oToolStrip.Renderer = new ToolStripSystemRenderer();
            }
            else
            {
                var renderer = new ToolStripProfessionalRenderer(new CustomColorScheme());
                renderer.RoundedEdges = false;
                oToolStrip.Renderer = renderer;
            }
        }

        #endregion

        #region "Properties"

        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         Description("Set the collection of the CustomProperty. Set ShowCustomProperties to True to enable it."),
         RefreshProperties(RefreshProperties.Repaint)]
        public CustomPropertyCollection Item
        {
            get { return oCustomPropertyCollection; }
        }

        [Category("Behavior"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         Description("Set the CustomPropertyCollectionSet. Set ShowCustomPropertiesSet to True to enable it."),
         RefreshProperties(RefreshProperties.Repaint)]
        public CustomPropertyCollectionSet ItemSet
        {
            get { return oCustomPropertyCollectionSet; }
        }

        [Category("Behavior"), DefaultValue(false),
         Description("Move automatically the splitter to better fit all the properties shown.")]
        public bool AutoSizeProperties
        {
            get { return bAutoSizeProperties; }
            set
            {
                bAutoSizeProperties = value;
                if (value)
                {
                    AutoSizeSplitter(32);
                }
            }
        }

        [Category("Behavior"), DefaultValue(false),
         Description("Use the custom properties collection as SelectedObject."),
         RefreshProperties(RefreshProperties.All)]
        public bool ShowCustomProperties
        {
            get { return bShowCustomProperties; }
            set
            {
                if (value)
                {
                    bShowCustomPropertiesSet = false;
                    base.SelectedObject = oCustomPropertyCollection;
                }
                bShowCustomProperties = value;
            }
        }

        [Category("Behavior"), DefaultValue(false),
         Description("Use the custom properties collections as SelectedObjects."),
         RefreshProperties(RefreshProperties.All)]
        public bool ShowCustomPropertiesSet
        {
            get { return bShowCustomPropertiesSet; }
            set
            {
                if (value)
                {
                    bShowCustomProperties = false;
                    base.SelectedObjects = (object[]) oCustomPropertyCollectionSet.ToArray();
                }
                bShowCustomPropertiesSet = value;
            }
        }

        [Category("Appearance"), DefaultValue(false), Description("Draw a flat toolbar")]
        public new bool DrawFlatToolbar
        {
            get { return bDrawFlatToolbar; }
            set
            {
                bDrawFlatToolbar = value;
                ApplyToolStripRenderMode(bDrawFlatToolbar);
            }
        }

        [Category("Appearance"), DisplayName("Toolstrip"),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Toolbar object"),
         Browsable(true)]
        public ToolStrip ToolStrip
        {
            get { return oToolStrip; }
        }

        [Category("Appearance"), DisplayName("Help"),
         Description("DocComment object. Represent the comments area of the PropertyGrid."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(false)]
        public Control DocComment
        {
            get { return (Control) oDocComment; }
        }

        [Category("Appearance"), DisplayName("HelpTitle"),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content), Description("Help Title Label."),
         Browsable(true)]
        public Label DocCommentTitle
        {
            get { return oDocCommentTitle; }
        }

        [Category("Appearance"), DisplayName("HelpDescription"),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
         Description("Help Description Label."), Browsable(true)]
        public Label DocCommentDescription
        {
            get { return oDocCommentDescription; }
        }

        [Category("Appearance"), DisplayName("HelpImageBackground"), Description("Help Image Background.")]
        public Image DocCommentImage
        {
            get { return ((Control) oDocComment).BackgroundImage; }
            set { ((Control) oDocComment).BackgroundImage = value; }
        }

        #endregion
    }
}