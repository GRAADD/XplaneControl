using System;
using XplaneConnection_NF;

namespace XplaneControl
{
    public class TcpPackage : BytesPackage
    {
        
        public TcpPackage(byte[] bytes, ConnectionType Type) : base(bytes, Type)
        {
            try
            {
                if (bytes.Length == 0)
                    return;
                HeaderString = ByteOperations.GetHeaderString(bytes, Type);
                MakeHeader();
                MessageBytes = new byte[bytes.Length - ByteOperations.headerTcpLength - 1];
                for (int i = 0; i < MessageBytes.Length; i++)
                    MessageBytes[i] = bytes[i + ByteOperations.headerTcpLength + 1];
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
                return;
            }

            if (!string.IsNullOrEmpty(HeaderString) && HeaderBytes == null)
            {
                HeaderBytes = ByteOperations.GetHeaderBytes(HeaderString, Type);
                return;
            }

            if (string.IsNullOrEmpty(HeaderString) && HeaderBytes.Length != 0)
            {
                HeaderString = ByteOperations.GetHeaderString(HeaderBytes, Type);
            }

        }

    }
}