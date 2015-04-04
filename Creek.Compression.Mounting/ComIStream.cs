namespace Creek.Compression.Mounting
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

    /// <summary>
    /// Stream to IStream wrapper for COM interoperation.
    /// </summary>
    public class ComIStream : IStream
    {
        private const int STG_E_INVALIDFUNCTION = unchecked((int)0x80030001);

        private readonly Stream _stream;
        private long position = -1;
        /// <summary>
        /// Create an IStream object from .NET Stream.
        /// </summary>
        /// <param name="stream">.NET Stream</param>
        public ComIStream(Stream stream)
        {
            this._stream = stream;
        }

        private void SetSizeToPosition()
        {
            if (this.position != -1)
            {
                if (this.position > this._stream.Length)
                    this._stream.SetLength(this.position);
                this._stream.Position = this.position;
                this.position = -1;
            }
        }
        /// <summary>
        /// See IStream documents in MSDN.
        /// </summary>
        public void Read(byte[] pv, int cb, IntPtr pcbRead)
        {
            int read = 0;
            if (cb != 0)
            {
                this.SetSizeToPosition();
                read = this._stream.Read(pv, 0, cb);
            }
            if (pcbRead != IntPtr.Zero)
                Marshal.WriteInt32(pcbRead, read);
        }

        /// <summary>
        /// See IStream documents in MSDN.
        /// </summary>
        public void Write(byte[] pv, int cb, IntPtr pcbWritten)
        {
            if (cb != 0)
            {
                this.SetSizeToPosition();
                this._stream.Write(pv, 0, cb);
            }

            if (pcbWritten != IntPtr.Zero)
                Marshal.WriteInt32(pcbWritten, cb);
        }

        /// <summary>
        /// See IStream documents in MSDN.
        /// </summary>
        public void Seek(long dlibMove, int dwOrigin, IntPtr plibNewPosition)
        {
            long newPosition;

            if (this._stream.CanWrite)
            {
                switch ((SeekOrigin)dwOrigin)
                {
                    case SeekOrigin.Begin:
                        newPosition = dlibMove;
                        break;
                    case SeekOrigin.Current:
                        if ((newPosition = this.position) == -1)
                            newPosition = this._stream.Position;
                        newPosition += dlibMove;
                        break;
                    case SeekOrigin.End:
                        newPosition = this._stream.Length + dlibMove;
                        break;
                    default:
                        throw new ExternalException(null, STG_E_INVALIDFUNCTION);
                }

                if (newPosition > this._stream.Length)
                    this.position = newPosition;
                else
                {
                    this._stream.Position = newPosition;
                    this.position = -1;
                }
            }
            else
            {
                try
                {
                    newPosition = this._stream.Seek(dlibMove, (SeekOrigin)dwOrigin);
                }
                catch (ArgumentException)
                {
                    throw new ExternalException(null, STG_E_INVALIDFUNCTION);
                }
                this.position = -1;
            }

            if (plibNewPosition != IntPtr.Zero)
                Marshal.WriteInt64(plibNewPosition, newPosition);
        }

        /// <summary>
        /// See IStream documents in MSDN.
        /// </summary>
        public void SetSize(long libNewSize)
        {
            this._stream.SetLength(libNewSize);
        }

        /// <summary>
        /// See IStream documents in MSDN.
        /// </summary>
        public void CopyTo(IStream pstm, long cb, IntPtr pcbRead, IntPtr pcbWritten)
        {
            byte[] buffer = new byte[4096];
            long written = 0;
            int read;

            if (cb != 0)
            {
                this.SetSizeToPosition();
                do
                {
                    int count = 4096;

                    if (written + 4096 > cb)
                        count = (int)(cb - written);

                    if ((read = this._stream.Read(buffer, 0, count)) == 0)
                        break;
                    pstm.Write(buffer, read, IntPtr.Zero);
                    written += read;
                } while (written < cb);
            }

            if (pcbRead != IntPtr.Zero)
                Marshal.WriteInt64(pcbRead, written);
            if (pcbWritten != IntPtr.Zero)
                Marshal.WriteInt64(pcbWritten, written);
        }

        /// <summary>
        /// See IStream documents in MSDN.
        /// </summary>
        public void Commit(int grfCommitFlags)
        {
            this._stream.Flush();
        }

        /// <summary>
        /// See IStream documents in MSDN.
        /// </summary>
        public void Revert()
        {
            throw new ExternalException(null, STG_E_INVALIDFUNCTION);
        }

        /// <summary>
        /// See IStream documents in MSDN.
        /// </summary>
        public void LockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new ExternalException(null, STG_E_INVALIDFUNCTION);
        }

        /// <summary>
        /// See IStream documents in MSDN.
        /// </summary>
        public void UnlockRegion(long libOffset, long cb, int dwLockType)
        {
            throw new ExternalException(null, STG_E_INVALIDFUNCTION);
        }

        /// <summary>
        /// See IStream documents in MSDN.
        /// </summary>
        public void Stat(out STATSTG pstatstg, int grfStatFlag)
        {
            pstatstg = new STATSTG();
            pstatstg.cbSize = this._stream.Length;
        }

        /// <summary>
        /// See IStream documents in MSDN.
        /// </summary>
        public void Clone(out IStream ppstm)
        {
            ppstm = null;
            throw new ExternalException(null, STG_E_INVALIDFUNCTION);
        }
    }
}
