namespace Creek.Compression.Mounting
{
    using System;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;

    using STATSTG = System.Runtime.InteropServices.ComTypes.STATSTG;

    /// <summary>
    /// The <c>ComStream</c> Convert IStream to .NET Stream.
    /// </summary>
    
    public class ComStream : Stream
    {
        // the managed stream being wrapped
        IStream _istream;
        /// <summary>
        /// Create .NET Stream object from IStream.
        /// </summary>
        /// <param name="stream">An IStream object.</param>
        public ComStream(IStream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            
            this._istream = stream;
        }

        /// <summary>
        /// Property to get original stream object.
        /// </summary>
        public IStream IStream
        {
            get
            {
                return this._istream;
            }
        }

        
        /// <summary>
        /// See Stream documents in MSDN.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count)
        {
            IntPtr readed = new IntPtr();
            byte[] newBuf = new byte[count];
            this._istream.Read(buffer, count, readed);
            Array.Copy(newBuf, buffer, readed.ToInt32());
            return readed.ToInt32();
        }

        /// <summary>
        /// See Stream documents in MSDN.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] newBuf = new byte[count];
            Array.Copy(buffer, offset, newBuf, 0, count);
            this._istream.Write(newBuf, count, IntPtr.Zero);
        }

        /// <summary>
        /// See Stream documents in MSDN.
        /// </summary>
        public override long Seek(long offset, SeekOrigin origin)
        {
            long position = 0;
            IntPtr address = new IntPtr();
            this._istream.Seek(offset, (int)origin, address);
            return position;
        }

        /// <summary>
        /// See Stream documents in MSDN.
        /// </summary>
        public override long Length
        {
            get
            {
                STATSTG statstg;
                this._istream.Stat(out statstg, 1 /* STATSFLAG_NONAME*/ );
                return statstg.cbSize;
            }
        }

        /// <summary>
        /// See Stream documents in MSDN.
        /// </summary>
        public override long Position
        {
            get { return this.Seek(0, SeekOrigin.Current); }
            set { this.Seek(value, SeekOrigin.Begin); }
        }

        /// <summary>
        /// See Stream documents in MSDN.
        /// </summary>
        public override void SetLength(long value)
        {
            this._istream.SetSize(value);
        }

        /// <summary>
        /// See Stream documents in MSDN.
        /// </summary>
        public override void Close()
        {
            this._istream.Commit(0);
            this._istream = null;
            Marshal.ReleaseComObject(this._istream);
        }

        /// <summary>
        /// See Stream documents in MSDN.
        /// </summary>
        public override void Flush()
        {
            this._istream.Commit(0);
        }

        /// <summary>
        /// See Stream documents in MSDN.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                try 
	            {	
                    Byte[] test = new Byte[1];
                    this.Read(test, 0, 0);
                    return true;
	            }
	            catch (Exception)
	            {
                    return false;
	            }
            }
        }

        /// <summary>
        /// See Stream documents in MSDN.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                try
                {
                    Byte[] test = new Byte[1];
                    this.Write(test, 0, 0);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// See Stream documents in MSDN.
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                try
                {
                    this.Seek(0, 0);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
