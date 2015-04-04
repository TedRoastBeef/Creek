using System.Windows.Forms;

namespace Creek.UI.EFML.Base.CSS.Converters
{
    public class PaddingConverter : IConverter<Padding>
    {
        public override Padding Convert(string s)
        {
            var r = new Padding();
            string[] spl = s.Split(' ');

            r.Top = (int) NumberConverter.Convert(spl[0]);
            r.Left = (int) NumberConverter.Convert(spl[1]);

            return r;
        }

        public override string Convert(Padding s)
        {
            return s.Top + "px " + s.Left + "px";
        }
    }
}