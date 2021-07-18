using System;
using XplaneConnection_NF;

namespace XplaneControl
{
    public class UdpPackage : BytesPackage
    {
        public UdpHeader Header;
        public UdpPackage(byte[] bytes, ConnectionType Type) : base(bytes, Type)
        {
            try
            {
                if (bytes.Length == 0)
                    return;
                HeaderString = ByteOperations.GetHeaderString(bytes, Type);
                MakeHeader();
                MessageBytes = new byte[bytes.Length - ByteOperations.headerUdpLength - 1];
                for (int i = 0; i < MessageBytes.Length; i++)
                    MessageBytes[i] = bytes[i + ByteOperations.headerUdpLength + 1];
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
                HeaderString = ByteOperations.GetHeaderString(MessageBytes, Type);
                Header = Make(HeaderString);
                return;
            }

            if (!string.IsNullOrEmpty(HeaderString) && HeaderBytes == null)
            {
                HeaderBytes = ByteOperations.GetHeaderBytes(HeaderString, Type);
                Header = Make(HeaderString);
                return;
            }

            if (string.IsNullOrEmpty(HeaderString) && HeaderBytes.Length != 0)
            {
                HeaderString = ByteOperations.GetHeaderString(HeaderBytes, Type);
                Header = Make(HeaderString);
            }

        }

        internal static UdpHeader Make(string header)
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