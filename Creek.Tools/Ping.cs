using System;
using System.Net;
using System.Net.Sockets;

namespace Creek.Tools
{
    /// <summary>
    /// The Main Ping Class
    /// </summary>
    public class Ping
    {
        #region Reasons enum

        public enum Reasons
        {
            NoError = 0,
            HostNotFound,
            ErrorCreatingPacket,
            CannotSendPacket,
            HostNotResponding,
            TimeOut
        }

        #endregion

        private const int SOCKET_ERROR = -1;
        private const int ICMP_ECHO = 8;
        private EndPoint epServer;

        private string host;
        private Reasons reason;
        private int time;

        public Ping()
        {
            reason = Reasons.HostNotFound;
            time = -1;
        }

        public string Host
        {
            set { host = value; }
            get { return host; }
        }

        public string IP
        {
            set { host = value; }
            get { return epServer.ToString(); }
        }

        public int Time
        {
            get { return time; }
        }

        public Reasons Reason
        {
            get { return reason; }
        }

        // Declare some Constant Variables

        /// <summary>
        /// This public method takes the "hostname" of the server
        /// and then it pings it and shows the response time
        /// Data is returned as a string. IP addresses are resolved.
        /// </summary>
        public void PingHost()
        {
            // Declare the IPHostEntry
            IPHostEntry serverHE, fromHE;
            int nBytes = 0;
            int dwStart = 0, dwStop = 0;

            // Initialize a Socket of the Type ICMP
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Raw, ProtocolType.Icmp);
            socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1000);

            // Get the server endpoint
            try
            {
#pragma warning disable 612,618
                serverHE = Dns.GetHostByName(host);
#pragma warning restore 612,618
            }
            catch (Exception)
            {
                // fail
                reason = Reasons.HostNotFound;
                return;
            }

            // Convert the server IP_EndPoint to an EndPoint
            var ipepServer = new IPEndPoint(serverHE.AddressList[0], 0);
            epServer = (ipepServer);

            // Set the receiving endpoint to the client machine
#pragma warning disable 612,618
            fromHE = Dns.GetHostByName(Dns.GetHostName());
#pragma warning restore 612,618
            var ipEndPointFrom = new IPEndPoint(fromHE.AddressList[0], 0);
            EndPoint EndPointFrom = (ipEndPointFrom);

            int PacketSize = 0;
            var packet = new IcmpPacket();
            // Construct the packet to send
            packet.Type = ICMP_ECHO; // 8
            packet.SubCode = 0;
            packet.CheckSum = UInt16.Parse("0");
            packet.Identifier = UInt16.Parse("45");
            packet.SequenceNumber = UInt16.Parse("0");
            int PingData = 32; // sizeof(IcmpPacket) - 8;
            packet.Data = new Byte[PingData];
            // Initilize the Packet.Data
            for (int i = 0; i < PingData; i++)
            {
                packet.Data[i] = (byte) '#';
            }

            // Variable to hold the total Packet size
            PacketSize = PingData + 8;
            var icmp_pkt_buffer = new Byte[PacketSize];
            Int32 Index = 0;
            // Call a Method Serialize which counts
            // The total number of Bytes in the Packet
            Index = Serialize(packet, icmp_pkt_buffer, PacketSize, PingData);
            // Error in Packet Size
            if (Index == -1)
            {
                reason = Reasons.ErrorCreatingPacket;
                return;
            }

            // Convert into a Int32 array

            // Get the Half size of the Packet
            Double double_length = Convert.ToDouble(Index);
            Double dtemp = Math.Ceiling(double_length/2);
            int cksum_buffer_length = Convert.ToInt32(dtemp);
            // Create a Byte Array
            var cksum_buffer = new UInt16[cksum_buffer_length];
            // Code to initialize the Int32 array
            int icmp_header_buffer_index = 0;
            for (int i = 0; i < cksum_buffer_length; i++)
            {
                cksum_buffer[i] = BitConverter.ToUInt16(icmp_pkt_buffer, icmp_header_buffer_index);
                icmp_header_buffer_index += 2;
            }

            // Call a method which will return a checksum
            UInt16 u_cksum = checksum(cksum_buffer, cksum_buffer_length);

            // Save the checksum to the Packet
            packet.CheckSum = u_cksum;

            // Now that we have the checksum, serialize the packet again
            var sendbuf = new Byte[PacketSize];

            // Again check the packet size

            Index = Serialize(packet, sendbuf, PacketSize, PingData);

            // If there is a error report it
            if (Index == -1)
            {
                reason = Reasons.ErrorCreatingPacket;
                return;
            }

            dwStart = Environment.TickCount; // Start timing
            if ((nBytes = socket.SendTo(sendbuf, PacketSize, 0, epServer)) == SOCKET_ERROR)
            {
                reason = Reasons.CannotSendPacket;
                return;
            }

            // Initialize the buffers. The receive buffer is the size of the
            // ICMP header plus the IP header (20 bytes)
            var ReceiveBuffer = new Byte[256];
            nBytes = 0;

            // Receive the bytes
            bool recd = false;
            int timeout = 0;

            // Loop for checking the time of the server responding
            while (!recd)
            {
                nBytes = socket.ReceiveFrom(ReceiveBuffer, 256, 0, ref EndPointFrom);

                if (nBytes == SOCKET_ERROR)
                {
                    reason = Reasons.HostNotResponding;
                    return;
                }
                else if (nBytes > 0)
                {
                    // Stop timing
                    dwStop = Environment.TickCount - dwStart;
                    time = dwStop;
                    reason = Reasons.NoError;
                    return;
                }

                timeout = Environment.TickCount - dwStart;
                if (timeout > 1000)
                {
                    reason = Reasons.TimeOut;
                    return;
                }
            }

            // Close the socket
            socket.Close();
            reason = Reasons.NoError;
            return;
        }

        /// <summary>
        /// This method get the Packet and calculates the total size
        /// of the Pack by converting it to byte array
        /// </summary>
        public static Int32 Serialize(IcmpPacket packet, Byte[] Buffer, Int32 PacketSize, Int32 PingData)
        {
            Int32 cbReturn = 0;
            // serialize the struct into the array
            int Index = 0;

            var b_type = new Byte[1];
            b_type[0] = (packet.Type);

            var b_code = new Byte[1];
            b_code[0] = (packet.SubCode);

            Byte[] b_cksum = BitConverter.GetBytes(packet.CheckSum);
            Byte[] b_id = BitConverter.GetBytes(packet.Identifier);
            Byte[] b_seq = BitConverter.GetBytes(packet.SequenceNumber);

            // Console.WriteLine("Serialize type ");
            Array.Copy(b_type, 0, Buffer, Index, b_type.Length);
            Index += b_type.Length;

            // Console.WriteLine("Serialize code ");
            Array.Copy(b_code, 0, Buffer, Index, b_code.Length);
            Index += b_code.Length;

            // Console.WriteLine("Serialize cksum ");
            Array.Copy(b_cksum, 0, Buffer, Index, b_cksum.Length);
            Index += b_cksum.Length;

            // Console.WriteLine("Serialize id ");
            Array.Copy(b_id, 0, Buffer, Index, b_id.Length);
            Index += b_id.Length;

            Array.Copy(b_seq, 0, Buffer, Index, b_seq.Length);
            Index += b_seq.Length;

            // copy the data
            Array.Copy(packet.Data, 0, Buffer, Index, PingData);

            Index += PingData;

            if (Index != PacketSize /* sizeof(IcmpPacket) */)
            {
                cbReturn = -1;
                return cbReturn;
            }

            cbReturn = Index;
            return cbReturn;
        }

        /// <summary>
        /// This Method has the algorithm to make a checksum
        /// </summary>
        public static UInt16 checksum(UInt16[] buffer, int size)
        {
            Int32 cksum = 0;
            int counter;
            counter = 0;

            while (size > 0)
            {
                Int32 val = buffer[counter];
                cksum += Convert.ToInt32(buffer[counter]);
                counter += 1;
                size -= 1;
            }

            cksum = (cksum >> 16) + (cksum & 0xffff);
            cksum += (cksum >> 16);

            string cksumHex = Convert.ToString(~cksum, 16);
            string cksumHexConverted = cksumHex.Substring(
                cksumHex.Length - 4, 4);
            return Convert.ToUInt16(cksumHexConverted, 16);
        }

        public static string ReasonToString(Reasons reason)
        {
            switch (reason)
            {
                case Reasons.NoError:
                    return "No error";
                case Reasons.HostNotFound:
                    return "Host not found";
                case Reasons.ErrorCreatingPacket:
                    return "Error creating packet";
                case Reasons.CannotSendPacket:
                    return "Cannot send packet";
                case Reasons.HostNotResponding:
                    return "Host not responding";
                case Reasons.TimeOut:
                    return "Time out";
                default:
                    return "Unknown reason";
            }
        }
    }

    /// <summary>
    /// Class that holds the Pack information
    /// </summary>
    public class IcmpPacket
    {
        public UInt16 CheckSum; // ones complement checksum of struct
        public Byte[] Data;
        public UInt16 Identifier; // identifier
        public UInt16 SequenceNumber; // sequence number
        public Byte SubCode; // type of sub code
        public Byte Type; // type of message
    }
}