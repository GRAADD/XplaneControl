using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using MainLibrary;
using XplaneConnection_NF;

namespace XplaneControl
{
    public class Connection
    {
        public string MasterIp = "";
        public IPAddress ipAddress;
        public string SlaveIp = "";

        public bool IsXamarin = true;

        public Thread UDPConnectionThread;
        public Thread TCPConnectionThread;
        private bool forceStop = false;

        private TcpClient xPlaneTCP;
        //private IPEndPoint xPlaneTcpPoint;
        private NetworkStream streamTCP;

        private int localTcpPort = 49703;
        //private int localTcpPort2 = 49631;
        private int _tcpPacketLength = 256;//64294
        public string responseTCP;
        public string messageTCP = "";
        public List<string> TerminaList = new List<string>();
        

        internal List<string> TcpHeaders = new List<string>() { "xCMD8", "xRAD", "xNAVH", "xAPTP", "xFIX", "xAPTl", "xAPT" };//"xLOC", "xFAL", "xDIM", "xACF", "xFIX", 
        internal List<string> CmndHeaders = new List<string>() { "CMND", "xFAL", "xDIM", "xACF", "xFIX", "xCMD", "xRAD", "xCON", "ACFN" };

        public List<string> xCMD = new List<string>(); 
        public List<string> xFIX = new List<string>();
        public List<string> xNAVH = new List<string>();
        public List<XAirport> Airports = new List<XAirport>();
        public List<string> xRAD = new List<string>();
        public List<string> unknowMessages = new List<string>();


        public bool CmdStarted = false;
        public bool FixStarted = false;
        public bool NavStarted = false;
        public bool AptStarted = false;
        public bool RadStarted = false;

        //public List<byte[]> MessagesTcpAll = new List<byte[]>();
        //public List<string> TranslatedTcpMessages = new List<string>();


        public bool TcpFinished = false;
        private int TimeToWait = 150;

        //public int StatusMaximum = 0;
        //public int StatusCurrent = 0;
        
        public List<byte[]> BytesList = new List<byte[]>();

        
        public int masterPortOut = 49001;
        public int masterUdpPortIn = 49000;
        public int localUdpPort = 48003;
        public UdpClient xPlaneUDP;
        private int _ucpPacketLength = 256;

        public string responseUDP;

        public List<string> UdpHeaders = new List<string>() { "XWXR", "FAIL", "RECO" };
        public List<byte[]> MessagesUdpAll = new List<byte[]>();
        public List<byte> CommandMessages = new List<byte>();

        public List<UdpMessage> FailMessages = new List<UdpMessage>();
        public List<UdpMessage> AcfMessages = new List<UdpMessage>();
        public List<UdpMessage> RadMessages = new List<UdpMessage>();
        public byte[] xLoc;
        public string xDim = "";
        public string xWgt = "";
        public string xAcf = "";


        public Connection(string master)
        {
            MasterIp = master;
            //DataEncoder.connection = this;
            TCPConnectionThread = new Thread(TcpConveyer);
            TCPConnectionThread.Start();
            UDPConnectionThread = new Thread(UdpLoop);
            UDPConnectionThread.Start();
        }

        public void CloseConnection()
        {
            forceStop = true;
            TcpFinished = true;
        }
        
        //=============================================================================================================================

        #region ConnectionLoops
        
        public void TcpConveyer()
        {
            Thread encoderThread = new Thread(DataEncoder.EncoderConveyer);
            encoderThread.Start();

            //byte[] conMessage = CmdEncoder.encodeCommandToBytes("CON1", ConnectionType.TCP);
            //byte[] emptyBytes = new byte[0];
            Thread.CurrentThread.Name = "Tcp Loop";
            try
            {
                Connect();
                while (!TcpFinished)
                {
                    byte[] data = new byte[64];
                    if (streamTCP.DataAvailable)
                    {
                        streamTCP.Read(data, 0, data.Length);
                        BytesList.Add(data);
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }
            catch (SocketException se)
            {
            }
            catch (Exception e)
            {
            }
            
            TcpFinished = true;
        }

        public async void UdpLoop()
        {
            UdpClient udp = new UdpClient();
            try
            {
                Thread.CurrentThread.Name = "UdpLoop";
                IPAddress ipAddress = IPAddress.Parse(MasterIp);
                
                bool connected = false;
                byte[] input = new byte[1];
                IPEndPoint xplaneEndPoint = new IPEndPoint(ipAddress, 0);
                while (!connected)
                {
                    udp = new UdpClient(48003);
                    var introduceMessage = CmdEncoder.encodeCommandToBytes("CON1", ConnectionType.UDP);
                    udp.Send(introduceMessage, introduceMessage.Length, MasterIp,
                        masterUdpPortIn);
                    xplaneEndPoint = new IPEndPoint(ipAddress, 0);
                    //udp.Client.ReceiveTimeout = 3000;
                    Thread.Sleep(150);
                    input = udp.Receive(ref xplaneEndPoint);
                    if (input.Length > 0)
                        connected = true;
                }
                await DataEncoder.UdpDataToMessage(input);


                while (Thread.CurrentThread.IsAlive && !forceStop)
                {
                    byte[] data = udp.Receive(ref xplaneEndPoint);
                    if (data.Length > 0)
                    {
                        await DataEncoder.UdpDataToMessage(data);
                        //test
                    }

                    if (CommandMessages.Count > 0)
                    {
                        udp.Send(CommandMessages.ToArray(), CommandMessages.ToArray().Length, MasterIp, masterUdpPortIn);
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

        #endregion
        
        //=============================================================================================================================
        public void MessagesAllAdd(byte[] data, ConnectionType connectionType)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (!Enum.IsDefined(typeof(ConnectionType), connectionType))
                throw new InvalidEnumArgumentException(nameof(connectionType), (int) connectionType,
                    typeof(ConnectionType));
        }

        private void AddToList(List<string> list, string str)
        {
            if (!list.Contains(str))
                list.Add(str);
        }

        public UdpHeader getEnum(string header)
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
                    return UdpHeader.Radar;
            }
        }


        //=============================================================================================================================


        private void Connect()
        {
            xPlaneTCP = new TcpClient(MasterIp, masterPortOut);
            Byte[] dataSend = CmdEncoder.encodeCommandToBytes("CON1", ConnectionType.TCP);;
            streamTCP = xPlaneTCP.GetStream();
            streamTCP.Write(dataSend, 0, dataSend.Length);
            Task.Delay(TimeToWait).Wait();

        }

        //====================================================================================================================================================
        
        public void AddMessageToList(UdpMessage udpMessage, List<UdpMessage> messages)
        {
            bool unic = true;
            foreach (var message in messages)
                if (udpMessage.MessageString == message.MessageString)
                    unic = false;

            if (unic)
                messages.Add(udpMessage);
        }

        public void SendMessage(string data)
        {
            string header = DataEncoder.GetHeaderString(data, ConnectionType.Command);

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
            CommandMessages.AddRange(CmdEncoder.encodeCommandToBytes(message));
        }

        public void SendMessage(byte[] data)
        {
            CommandMessages.AddRange(data);
        }
        
        private int FailTemp = 0;
    }

    class OldShit
    {
        //public void TcpLoop()
        //{
        //    byte[] conMessage = CmdEncoder.encodeCommandToBytes("CON1", ConnectionType.TCP);
        //    byte[] emptyBytes = new byte[0];
        //    Thread.CurrentThread.Name = "Tcp Loop";
        //    try
        //    {
        //        Connect();

        //        int emptyMessagesCount = 0;
        //        List<byte> bytes = new List<byte>();
        //        while (!TcpFinished)
        //        {
        //            byte[] data = new byte[512];

        //            while (streamTCP.DataAvailable)
        //            {
        //                streamTCP.Read(data, 0, data.Length);
        //                bytes.AddRange(data);
        //                bytes = DataEncoder.EncodeBytes(bytes.ToArray());
        //                //TcpEncoder.TcpDataToMessage(data.ToArray(), ref responseTCP);
        //            } 

        //            if (bytes.Count == 0)
        //            {
        //                emptyMessagesCount++;
        //                if (emptyMessagesCount/100 == 3)
        //                {
        //                    if (!xPlaneTCP.Client.Connected)
        //                        xPlaneTCP.Client.Connect(MasterIp, masterPortOut);
        //                    if (!streamTCP.DataAvailable)
        //                        streamTCP = xPlaneTCP.GetStream();
        //                    streamTCP.Write(emptyBytes, 0, emptyBytes.Length);
        //                }
        //                if (emptyMessagesCount / 10000 > 6)
        //                {
        //                    if (NavStarted)
        //                    {
        //                        TcpFinished = true;
        //                        AptStarted = true;
        //                    }
        //                    else
        //                    {
        //                        Connect();
        //                        emptyMessagesCount = 0;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                //bytes = TcpEncoder.EncodeBytes(bytes.ToArray());
        //                emptyMessagesCount = 0;
        //            }
                    

        //        }

        //    }
        //    catch (SocketException se)
        //    {
        //    }
        //    catch (Exception e)
        //    {
        //    }
            
        //    TcpFinished = true;
        //}
    }
}