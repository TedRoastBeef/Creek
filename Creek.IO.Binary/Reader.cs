using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Creek.IO.Binary
{
    public class Reader
    {
        internal BinaryReader br;

        public Reader(Stream input)
        {
            br = new BinaryReader(input);
        }


        //[DebuggerStepThrough]
        public TT Read<TT>()
        {
            TypeBinaryDict readers = Utils.InitTypes();
            readers.AddRange(BinaryRuntime.Gets());

            foreach (IBinary reader in readers)
            {
                if (reader.OutputType == typeof (TT))
                {
                    var r = reader as Binary<TT>;
                    return r.OnRead(this);
                }
            }
            return default(TT);
        }

        public T ReadStruct<T>()
        {
            var buffer = new byte[Marshal.SizeOf(typeof (T))];

            T oReturn;

            try
            {
                br.BaseStream.Read(buffer, 0, buffer.Length);

                GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                oReturn = (T) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof (T));
                handle.Free();

                return oReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public T[] ReadArray<T>()
        {
            var c = Read<int>().To<int>();
            var ret = new List<T>();
            for (int i = 0; i < c; i++)
            {
                ret.Add(Read<T>().To<T>());
            }
            return ret.ToArray();
        }

        public Dictionary<T, TT> ReadDict<T, TT>()
        {
            var r = new Dictionary<T, TT>();
            for (int i = 0; i < Read<int>(); i++)
            {
                r.Add(Read<T>(), Read<TT>());
            }
            return r;
        }


        public void Close()
        {
            br.Close();
        }
    }
}