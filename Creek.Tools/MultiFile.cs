using System;
using System.Collections.Generic;
using System.IO;

namespace Creek.Tools
{
    public class MultiFile
    {
        private Dictionary<string, string> data = new Dictionary<string, string>();
        private Dictionary<string, byte[]> files = new Dictionary<string, byte[]>();

        public void AddData(string key, string value)
        {
            data.Add(key, value);
        }
        public void RemoveData(string key)
        {
            data.Remove(key);
        }

        public void AddFile(string name, Stream s)
        {
            files.Add(name, ReadToEnd(s));
        }
        public void RemoveFile(string name)
        {
            files.Remove(name);
        }

        public void Save(Stream s)
        {
            var bw = new BinaryWriter(s);
            bw.Write(data.Count);
            foreach (var d in data)
            {
                bw.Write(d.Key);
                bw.Write(d.Value);
            }

            bw.Write(files.Count);
            foreach (var file in files)
            {
                bw.Write(file.Key);
                bw.Write(file.Value.Length);
                foreach (var b in file.Value)
                {
                    bw.Write(b);
                }
            }
            bw.Flush();
            bw.Close();
        }

        private static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}