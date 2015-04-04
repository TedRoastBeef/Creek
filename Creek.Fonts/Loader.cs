using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Creek.Library.Fonts.Properties;

namespace Creek.Library.Fonts
{
    public class Loader
    {
        private Dictionary<string, byte[]> buffer = new Dictionary<string, byte[]>(); 
        readonly PrivateFontCollection pfc = new PrivateFontCollection();

        public IEnumerable<string> GetFamilyNames()
        {
            return pfc.Families.Select(fontFamily => fontFamily.Name).ToList();
        }

        public void Extract(string filename, string name)
        {
            File.WriteAllBytes(filename, buffer[name]);
        }

        public Loader()
        {
            Load(Fonts.FacebookLetterFaces, Resources.Facebook_Letter_Faces);
            Load(Fonts.MfKindWitty, Resources.Mf_Kind___Witty);
            Load(Fonts.ReadyBlack, Resources.Ready_Black);
            Load(Fonts.Harrison, Resources.Harrison);
            Load(Fonts.Stilts, Resources.THESTILTS_erc_2006);
            Load(Fonts.TheKingQueen, Resources.the_King__26_Queen_font);
            Load(Fonts.AllanRooster, Resources.Allan_Rooster);
            Load(Fonts.TagItYourself, Resources.TagItYourself);
            Load(Fonts.Track, Resources.Track);
            Load(Fonts.Canter, Resources.Canter_Bold_3D);
        }

        public void Load(Stream s)
        {
            using (var memoryStream = new MemoryStream())
            {
                s.CopyTo(memoryStream);
                Load(memoryStream.ToArray());
            }
        }
        public void Load(byte[] b)
        {
            var fontData = Marshal.AllocCoTaskMem(b.Length);
            Marshal.Copy(b, 0, fontData, b.Length);
            pfc.AddMemoryFont(fontData, b.Length);
            Marshal.FreeCoTaskMem(fontData);
        }
        public void Load(string name, byte[] b)
        {
            buffer.Add(name, b);
            Load(b);
        }

        public Font this[string name]
        {
            get
            {
                foreach (var fontFamily in pfc.Families.Where(fontFamily => fontFamily.Name == name))
                {
                    return new Font(fontFamily, 12);
                }
                return new Font(FontFamily.GenericSansSerif, 12);
            }
        }
    }
    public struct Fonts
    {
        public static string FacebookLetterFaces = "Facebook Letter Faces";
        public static string MfKindWitty = "Mf Kind & Witty";
        public static string ReadyBlack = "Ready Black";
        public static string Harrison = "harrison";
        public static string Stilts = "THE STILTS";
        public static string TheKingQueen = "the King & Queen font";
        public static string AllanRooster = "Allan Rooster";
        public static string TagItYourself = "Tag It Yourself";
        public static string Track = "Track";
        public static string Canter = "Canter Bold 3D";
    }
}
