using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace Creek.UI
{
    public class CultureComboBox : ComboBox
    {
        public CultureComboBox()
        {
            var list = new List<string>();
            foreach (var ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
            {
                var specName = "(none)";
                try
                {
                    specName = CultureInfo.CreateSpecificCulture(ci.Name).Name;
                }
                catch
                {
                }
                list.Add(specName);
            }
            list.Sort();

            Items.AddRange(list.ToArray());
        }
    }
}