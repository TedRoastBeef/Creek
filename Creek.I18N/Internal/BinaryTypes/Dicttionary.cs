namespace Creek.I18N.Internal.BinaryTypes
{
    class Dictionary<k,v> : Creek.I18N.Internal.Binary
    {

        public Dictionary()
        {
            OnRead = OnReads;
            OnWrite = OnWrites;
        }
        
        private void OnWrites(Writer bw, object value)
        {
            var c = (System.Collections.Generic.Dictionary<k, v>)value; 
            bw.Write(c.Count);
            foreach (var v in c)
            {
                bw.Write<k>(v.Key);
                bw.Write<v>(v.Value);
            }
        }

        private object OnReads(Reader br)
        {
            var r = new System.Collections.Generic.Dictionary<k, v>();

            var c = br.ReadInt32();
            for (var i = 0; i < c; i++)
            {
                r.Add(br.Read<k>().To<k>(), br.Read<v>().To<v>());
            }

            return r;
        }
    }
}
