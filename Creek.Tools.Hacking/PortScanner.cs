using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace Creek.Tools.Hacking
{
    public class PortScanner
    {
        public event EventHandler<EventArgs> OnScanOK;
        public event EventHandler<EventArgs> OnScanFailed; 

        public IEnumerable<int> Scan(string ipAddress, int start, int end)
        {
            for (int i = start; i < end; i++)
            {
                var port = i;
                var ip = IPAddress.Parse(ipAddress);
                var ipEo = new IPEndPoint(ip, port);
                if (ScanPort(ipEo))
                {
                    OnScanOK(port, null);
                    yield return port;
                }
                else
                {
                    OnScanFailed(port, null);
                }
            }
        }

        private bool ScanPort(IPEndPoint ipEo, ProtocolType protocol = ProtocolType.Tcp, int timeOut = 5000)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, protocol);
            IAsyncResult result = socket.BeginConnect(ipEo, null, null);
            return result.AsyncWaitHandle.WaitOne(timeOut, true);
        }
    }
}
