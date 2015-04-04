using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Creek.IO.Binary
{
    public class Writer
    {
        internal BinaryWriter br;

        public Writer(Stream s)
        {
            br = new BinaryWriter(s);
        }

        public void Close()
        {
            br.Flush();
            br.Close();
        }

        private byte[] StructToByteArray(object oStruct)
        {
            try
            {
                // This function copys the structure data into a byte[]
                var buffer = new byte[Marshal.SizeOf(oStruct)]; //Set the buffer ot the correct size

                GCHandle h = GCHandle.Alloc(buffer, GCHandleType.Pinned);
                    //Allocate the buffer to memory and pin it so that GC cannot use the space (Disable GC)
                Marshal.StructureToPtr(oStruct, h.AddrOfPinnedObject(), false);
                    // copy the struct into int byte[] mem alloc 
                h.Free(); //Allow GC to do its job

                return buffer; // return the byte[] . After all thats why we are here right.
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [DebuggerStepThrough]
        public void Write<TT>(TT value)
        {
            TypeBinaryDict writers = Utils.InitTypes();
            writers.AddRange(BinaryRuntime.Gets());

            foreach (IBinary reader in writers)
            {
                if (reader.OutputType == typeof (TT))
                {
                    var b = reader as Binary<TT>;
                    b.OnWrite(this, value);
                }
            }
        }

        public void WriteStruct(object o)
        {
            WriteArray(StructToByteArray(o));
        }

        public void WriteArray<T>(T[] value)
        {
            var v = new List<T>(value);
            Write(v.Count);
            foreach (T vv in v)
            {
                Write(vv);
            }
        }

        public void WriteDict<T, TT>(T value) where T : IDictionary<T, TT>
        {
            Write(value.Count);
            foreach (var v in value)
            {
                Write(v.Key);
                Write(v.Value);
            }
        }
    }
}