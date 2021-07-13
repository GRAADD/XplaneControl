using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MainLibrary;
using XplaneConnection_NF;

namespace XplaneControl
{
    public static class ConnectionUdp
    {
        public static int masterPortOut = 49001;
        public static int masterUdpPortIn = 49000;
        public static int localUdpPort = 48003;
        public static UdpClient xPlaneUDP;
        private static int _ucpPacketLength = 256;
        public static bool forceStopUdp = false;

        public static string responseUDP;
        private static readonly byte[] _introduceMessage = CommandEncoder.encodeCommandToBytes("CON1", ConnectionType.UDP);

        public static List<string> UdpHeaders = new List<string>() { "XWXR", "FAIL", "RECO" };
        public static List<byte[]> MessagesUdpAll = new List<byte[]>();
        public static List<byte> CommandMessages = new List<byte>();

        public static List<UdpMessage> FailMessages = new List<UdpMessage>();
        public static List<UdpMessage> AcfMessages = new List<UdpMessage>();
        public static List<UdpMessage> RadMessages = new List<UdpMessage>();
        public static string xLoc = "";
        public static string xDim = "";
        public static string xWgt = "";
        public static string xAcf = "";

        public static async void UdpLoop()
        {
            UdpClient udp = new UdpClient();
            EncoderUdp encoderUdp = new EncoderUdp();
            try
            {
                Thread.CurrentThread.Name = "UdpLoop";
                IPAddress ipAddress = IPAddress.Parse(ConnectionCore.MasterIp);

                Logger.WriteLog("Udp connection start!", ErrorLevel.Info);
                bool connected = false;
                byte[] input = new byte[1];
                IPEndPoint xplaneEndPoint = new IPEndPoint(ipAddress, 0);
                while (!connected)
                {
                    udp = new UdpClient(48003);
                    udp.Send(_introduceMessage, _introduceMessage.Length, ConnectionCore.MasterIp,
                        ConnectionUdp.masterUdpPortIn);
                    xplaneEndPoint = new IPEndPoint(ipAddress, 0);
                    //udp.Client.ReceiveTimeout = 3000;
                    Thread.Sleep(150);
                    input = udp.Receive(ref xplaneEndPoint);
                    if (input.Length > 0)
                        connected = true;
                }
                await encoderUdp.UdpDataToMessage(input);


                while (Thread.CurrentThread.IsAlive && !forceStopUdp)
                {
                    byte[] data = udp.Receive(ref xplaneEndPoint);
                    if (data.Length > 0)
                    {
                        await encoderUdp.UdpDataToMessage(data);
                        //test
                    }

                    if (CommandMessages.Count > 0)
                    {
                        udp.Send(CommandMessages.ToArray(), CommandMessages.ToArray().Length, ConnectionCore.MasterIp,
                            masterUdpPortIn);
                        CommandMessages = new List<byte>();
                    }
                }
            }
            catch (SocketException se)
            {
                //Logger.WriteLog(se, ErrorLevel.Base);
            }
            catch (Exception e)
            {
                //Logger.WriteLog(e, ErrorLevel.Error);
            }

            udp?.Close();
            udp?.Dispose();
            //return;
        }
        
        public static void AddMessageToList(UdpMessage udpMessage, List<UdpMessage> messages)
        {
            bool unic = true;
            foreach (var message in messages)
                if (udpMessage.MessageString == message.MessageString)
                    unic = false;

            if (unic)
                messages.Add(udpMessage);
        }

        public static void SendMessage(string data)
        {
            string header = EncoderMisc.GetHeaderString(data, ConnectionType.Command);

            string message = data.Substring(header.Length);
            switch (header)
            {
                case "CMND":
                    if (message.Contains('['))
                        message = message.Replace("[", "");
                    if (message.Contains(']'))
                        message = message.Replace("]", "");
                    //if (message[0] != ' ')
                    //    message = " " + message;
                    break;
            }

            message = header + message;
            ConnectionUdp.CommandMessages.AddRange(CommandEncoder.encodeCommandToBytes(message));
        }

        public static void SendMessage(byte[] data)
        {
            ConnectionUdp.CommandMessages.AddRange(data);
        }


        private static int FailTemp = 0;
    }
}
