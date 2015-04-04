/*--------------------------------------------------------------------------------------
 * Author: Rafey
 * 
 * Comments: Firefox Option Dialog User Control for .NET Win Apps
 * 
 * Email: syedrafey@gmail.com
 * 
 -------------------------------------------------------------------------------------*/

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Creek.UI.MozOptions
{
    public class PropertyPage : UserControl
    {
        private Container components;

        protected bool isInit = false;

        public PropertyPage()
        {
            InitComponent();
        }

        public bool IsInit
        {
            get { return isInit; }
            set { isInit = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitComponent()
        {
            components = new System.ComponentModel.Container();
        }

        #endregion

        #region Overridables

        public new virtual string Text
        {
            get { return GetType().Name; }
        }

        public virtual Image Image
        {
            get { return null; }
        }

        public virtual void OnInit()
        {
            isInit = true;
        }

        public virtual void OnSetActive()
        {
        }

        public virtual void OnApply()
        {
        }

        #endregion
    }
}