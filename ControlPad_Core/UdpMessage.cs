using System;
using System.Collections.Generic;
using XplaneConnection_NF;

namespace XplaneControl
{
    public class UdpMessage
    {
        private string HeaderString = "";
        private byte[] HeaderBytes;
        public UdpHeader Header;
        public string MessageString = "";
        public byte[] MessageBytes;
        private ConnectionType Type;

        public UdpMessage(byte[] bytes, ConnectionType Type)
        {
            try
            {
                if (bytes.Length == 0)
                    return;
                HeaderString = DataEncoder.GetHeaderString(bytes, Type);
                MakeHeader();
                MessageBytes = new byte[bytes.Length - DataEncoder.headerUdpLength - 1];
                for (int i = 0; i < MessageBytes.Length; i++)
                    MessageBytes[i] = bytes[i + DataEncoder.headerUdpLength + 1];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void MakeHeader()
        {

            if (string.IsNullOrEmpty(HeaderString) && HeaderBytes == null && MessageBytes != null)
            {
                HeaderString = DataEncoder.GetHeaderString(MessageBytes, Type);
                Header = Make(HeaderString);
                return;
            }

            if (!string.IsNullOrEmpty(HeaderString) && HeaderBytes == null)
            {
                HeaderBytes = DataEncoder.GetHeaderBytes(HeaderString, Type);
                Header = Make(HeaderString);
                return;
            }

            if (string.IsNullOrEmpty(HeaderString) && HeaderBytes.Length != 0)
            {
                HeaderString = DataEncoder.GetHeaderString(HeaderBytes, Type);
                Header = Make(HeaderString);
            }

        }

        public byte[] FullBytes()
        {
            List<byte> bytes = new List<byte>();
            bytes.AddRange(HeaderBytes);
            bytes.AddRange(MessageBytes);
            return bytes.ToArray();
        }

        public string FullString()
        {
            return HeaderString + " " + MessageString;
        }

        private static UdpHeader Make(string header)
        {
            switch (header)
            {
                case "xACF":
                    return UdpHeader.Aircraft;
                case "ACFN":
                    return UdpHeader.CurrentAircraft;
                case "xFAL":
                    return UdpHeader.Fail;
                case "xDIM":
                    return UdpHeader.Dim;
                case "xWGT":
                    return UdpHeader.Weight;
                case "xRAD":
                    return UdpHeader.Radar;
                case "xLOC":
                    return UdpHeader.Location;
                case "xCON":
                    return UdpHeader.Con;
                default:
                    return UdpHeader.Unknow;
            }
        }

    }
}