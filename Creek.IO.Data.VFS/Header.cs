using System;

namespace Creek.Data.VFS
{
    [Serializable]
    public class Header
    {

        public string Filename { get; set; }
        public int Size { get; set; }
        public string Comment { get; set; }
        public DateTime CreationDate { get; set; }
        
    }
}