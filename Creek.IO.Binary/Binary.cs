using System;
using System.IO;

namespace Creek.IO.Binary
{
    public class PrimBinary<T> : Binary<T>
    {
        private Action<BinaryWriter, T> OnWriting;
        private Func<BinaryReader, T> OnReading;

        public PrimBinary(Action<BinaryWriter, T> onWriting, Func<BinaryReader, T> onReading)
        {
            OnReading = onReading;
            OnWriting = onWriting;
        }

        public override T OnRead(Reader br)
        {
            return OnReading(br.br);
        }
        public override void OnWrite(Writer bw, T value)
        {
            OnWriting(bw.br, value);
        }
    } 

    public class Binary<T> : IBinary
    {

        public Binary()
        {
            OutputType = typeof(T);
        }
        public virtual T OnRead(Reader br)
        {
            return default(T);
        }
        public virtual void OnWrite(Writer bw, T value)
        {
            
        }
    }

    public class IBinary
    {
        public Type OutputType;
    }
}