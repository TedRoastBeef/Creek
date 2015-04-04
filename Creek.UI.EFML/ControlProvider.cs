using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Creek.UI.EFML.Base;

namespace Creek.UI.EFML
{
    public class ControlProvider
    {
        public byte[] Assembly;
        public Dictionary<Tag, Type> Controls = new Dictionary<Tag, Type>();

        public Control this[Tag key]
        {
            get { return (Control) Activator.CreateInstance(Controls[key]); }
        }

        public void Load(Stream s)
        {
            var br = new BinaryReader(s);
            var raw = new List<byte>();

            // load all Controls
            int cC = br.ReadInt32();
            for (int i = 0; i < cC; i++)
            {
                Controls.Add(Tag.FromString(br.ReadString()), Type.GetType(br.ReadString()));
            }

            // read raw assembly
            int c = br.ReadInt32();
            for (int i = 0; i < c; i++)
            {
                raw.Add(br.ReadByte());
            }
            Assembly = raw.ToArray();

            br.Close();
        }

        public void Save(Stream s)
        {
            var bw = new BinaryWriter(s);

            // write controls
            bw.Write(Controls.Count);
            foreach (var control in Controls)
            {
                bw.Write(control.Key.ToString());
                bw.Write(control.Value.FullName);
            }

            // write raw data
            bw.Write(Assembly.Length);
            foreach (byte raw in Assembly)
            {
                bw.Write(raw);
            }

            bw.Flush();
            bw.Close();
        }

        protected void Add<T>(Tag t)
        {
            Controls.Add(t, typeof (T));
        }
    }
}