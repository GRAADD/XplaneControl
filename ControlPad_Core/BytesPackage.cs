using System.Collections.Generic;
using XplaneConnection_NF;

namespace XplaneControl
{
    public class BytesPackage
    {
        internal string HeaderString = "";
        internal byte[] HeaderBytes;
        public string MessageString = "";
        public byte[] MessageBytes;
        internal ConnectionType Type;

        public BytesPackage(byte[] bytes, ConnectionType Type)
        {
            //try
            //{
            //    if (bytes.Length == 0)
            //        return;
            //    HeaderString = ByteOperations.GetHeaderString(bytes, Type);
            //    //MakeHeader();
            //    MessageBytes = new byte[bytes.Length - ByteOperations.headerUdpLength - 1];
            //    for (int i = 0; i < MessageBytes.Length; i++)
            //        MessageBytes[i] = bytes[i + ByteOperations.headerUdpLength + 1];
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}
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

    }
}
