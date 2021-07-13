using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace XplaneConnection
{
    internal static class ConnectionTcp
    {
        private static TcpClient xPlaneTCP;
        private static IPEndPoint xPlaneTcpPoint;
        //private static Socket xPlaneSocket;
        private static NetworkStream streamTCP;

        private static int localTcpPort = 49703;
        //private static int localTcpPort2 = 49631;
        private static int _tcpPacketLength = 256;//64294

        public static string responseTCP;


        public static string messageTCP = "";

        public static List<string> TerminaList = new List<string>();
        
        private const int ReconnectRetries = 3;

        public static List<string> TcpHeaders = new List<string>() { "xCMD8", "xRAD", "xNAVH", "xAPTP", "xFIX"};//"xLOC", "xFAL", "xDIM", "xACF", "xFIX", 
        public static List<string> CmndHeaders = new List<string>() { "CMND", "xFAL", "xDIM", "xACF", "xFIX", "xCMD", "xRAD", "xCON" };

        public static List<string> xCMD = new List<string>(); 
        public static List<string> xFIX = new List<string>();
        public static List<string> xNAVH = new List<string>();
        public static List<byte[]> xAPTP = new List<byte[]>();
        public static List<XAirport> Airports = new List<XAirport>();
        public static List<string> xRAD = new List<string>();
        public static List<string> unknowMessages = new List<string>();


        public static bool CmndDone = false;
        public static bool FixDone = false;
        public static bool NavDone = false;
        public static bool AptpDone = false;

        public static List<byte[]> MessagesTcpAll = new List<byte[]>();
        public static List<string> TranslatedTcpMessages = new List<string>();

        public static bool forceStopTcp = false;

        public static bool TcpFinished = false;

        private static int TimeToWait = 150;

        public static int StatusMaximum = 0;
        public static int StatusCurrent = 0;


        private static void TcpConnect(bool introduce)
        {
            try
            {
                CheckTcpPort(ConnectionUdp.masterPortOut);
                xPlaneTcpPoint = new IPEndPoint(IPAddress.Parse(ConnectionCore.MasterIp), ConnectionUdp.masterPortOut); 
                streamTCP.ReadTimeout = 20000;
                streamTCP.WriteTimeout = 20000;
                //сообщаем игрушке, что мы есть и живы
                xPlaneTCP = new TcpClient(xPlaneTcpPoint);
                xPlaneTCP.Client.ReceiveTimeout = 20000;
                xPlaneTCP.Connect(xPlaneTcpPoint);
                streamTCP = xPlaneTCP.GetStream();

                if (introduce)
                {
                    DebugPart.WriteLog($"Sending TCP CON1 message to MasterPC");
                    byte[] IntroduceMessageTCP = CommandEncoder.encodeCommandToBytes("CON1", ConnectionType.TCP);
                    streamTCP.Write(IntroduceMessageTCP, 0, IntroduceMessageTCP.Length);
                }

                string localEnd = xPlaneTCP.Client.LocalEndPoint.ToString();
                localTcpPort = int.Parse(localEnd.Split(':')[1]);
                Thread.Sleep(TimeToWait);
                TimeToWait++;

            }
            catch (Exception e)
            {
                DebugPart.WriteLog(e);
            }

        }

        private static void CheckTcpPort(int port)
        {

            // Evaluate current system tcp connections. This is the same information provided
            // by the netstat command line application, just in .Net strongly-typed object
            // form.  We will look through the list, and if our port we would like to use
            // in our TcpClient is occupied, we will set isAvailable to false.
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
            TcpConnectionInformation[] tcpConnInfoArray = ipGlobalProperties.GetActiveTcpConnections();

            foreach (TcpConnectionInformation tcpi in tcpConnInfoArray)
            {
                if (tcpi.LocalEndPoint.Port == port)
                {
                    DebugPart.WriteLog($"TCP connection to port {port} is not possible!");
                }
            }
        }

        public static void TCPLoop()
        {
            int reconnectCount = 0;
            Thread.CurrentThread.Name = "TCPLoop";
            EncoderTcp TcpEncoder = new EncoderTcp();
            bool noErrorFlag = true;
            bool connected = false;
            TcpConnect(true);

            int noData = 0;
            while (noErrorFlag && Thread.CurrentThread.IsAlive &&
                   !forceStopTcp && !TcpFinished)
            {
                try
                {

                    if (streamTCP == null || !streamTCP.CanRead || xPlaneTCP == null)// || !streamTCP.DataAvailable
                    {
                        if (connected)
                            reconnectCount++;
                        if (ConnectionCore.MasterIp == "")
                        {
                            return;
                        }
                        if (reconnectCount > ReconnectRetries)
                        {
                            TcpConnect(true);
                            reconnectCount = 0;
                        }//если много попыток перезапуска, перезапустить
                        else
                            TcpConnect(false);
                    }
                    else if (streamTCP.DataAvailable)
                    {
                        connected = true;

                        byte[] data = new byte[_tcpPacketLength];
                        streamTCP.Read(data, 0, data.Length);
                        data = EncoderMisc.CutZeroBytes(data);
                        if (data.Length == 0) 
                            if (CmndDone && FixDone && NavDone)
                            {
                                AptpDone = true;
                                TcpFinished = true;
                                continue;
                            }
                            else
                            {
                                Thread.Sleep(TimeToWait);
                                continue;
                            }
                        TcpEncoder.TcpDataToMessage(data, ref responseTCP);
                        ConnectionCore.MessagesAllAdd(data, ConnectionType.TCP);
                        
                    }//получать сообщения, пока возможно
                    else
                    {
                        if (CmndDone && FixDone && NavDone && xAPTP.Count > 25000)
                            TcpEncoder.ReworkAirportsList();
                        if (CmndDone && AptpDone && FixDone && NavDone)
                            noData++;
                        if (noData > 45000)
                            break;
                        Console.Write($"No Data = {noData}");
                    }
                }
                catch (ThreadAbortException e)
                {
                    noErrorFlag = false;
                    DebugPart.WriteLog(e);
                }
                catch (Exception e)
                {
                    noErrorFlag = false;
                    DebugPart.WriteLog(e);
                }
            }

            TcpFinished = true;
            Console.Write($"TCP connection finished");
            //xPlaneTCP?.Close();
        }
 

    }

    static class ConnectionWWW
    {
        private static UdpClient XplaneUdpClient;
        private static IPEndPoint XplaneEndPoint;
        public static int MasterPortOut;
        private static WWWEncoder _wwwEncoder;


        public static void Init()
        {

            _wwwEncoder = new WWWEncoder();
            Thread.CurrentThread.Name = "UDPLoop";
            IPEndPoint xPlaneRecievePoint = new IPEndPoint(IPAddress.Any, 0);
            UdpClient clientUdpRecieve = new UdpClient(MasterPortOut);
        }

    }

    class EncoderTcp
    {
        List<byte[]> messagesStack = new List<byte[]>();
        private int TcpMessageWorkerLoopPauseTime = 50; 
        private bool SplitterRunning = false;


        public void TcpDataToMessage(byte[] bytesData, ref string message)
        {
            messagesStack.Add(bytesData);
            if (!SplitterRunning)
            {
                SplitterRunning = true;
                Thread tcpMessageThread = new Thread(LoopStartSplitter);
                tcpMessageThread.Start();
            }
        }

        private void LoopStartSplitter()
        {
            Thread.CurrentThread.Name = "LoopStartSplitter";
            while (!ConnectionTcp.forceStopTcp && !ConnectionTcp.TcpFinished)
            {
                if (messagesStack.Count > 0)
                    MessageSplitter();
                else
                    Thread.Sleep(TcpMessageWorkerLoopPauseTime);
            }
            if (messagesStack.Count > 0)
                MessageSplitter();

            SplitterRunning = false;
        }

        private void MessageSplitter()
        {
            List<byte[]> messagesStacktemp = messagesStack;
            messagesStack = new List<byte[]>();

            try
            {
                List<byte> messageBuffer = new List<byte>();
                List<byte[]> messagesList = new List<byte[]>();
                int point = 0;
                {
                    ConnectionTcp.StatusMaximum = messagesStacktemp.Count;
                    for (int messageIndex = 0; messageIndex < messagesStacktemp.Count; messageIndex++)
                    {
                        ConnectionTcp.StatusCurrent = messageIndex;
                        if (messagesStacktemp[messageIndex] == null)
                            continue;


                        messageBuffer.AddRange(messagesStacktemp[messageIndex]);
                        messagesStacktemp[messageIndex] = null;
                        List<byte> messageTemp = new List<byte>();
                        for (int i = 0; i < messageBuffer.Count; i++)
                        {
                            if (ConnectionTcp.forceStopTcp)
                                return;

                            if (messageBuffer[i] == 120 && i < messageBuffer.Count - EncoderMisc.headerTcpLength)//&& messageTemp.Count > EncoderMisc.headerTcpLength
                            {
                                byte[] hdrBytes = { messageBuffer[i], messageBuffer[i + 1], messageBuffer[i + 2], messageBuffer[i + 3], messageBuffer[i + 4] };
                                if (EncoderMisc.IsItCorrectHeader(hdrBytes, ConnectionType.TCP))
                                {
                                    string debug =
                                        EncoderMisc.GetHeaderString(hdrBytes, ConnectionType.TCP);
                                    if (messageTemp.Count > 0)
                                        messagesList.Add(messageTemp.ToArray());
                                    messageTemp = new List<byte>();
                                    point = i;
                                }
                            }
                            messageTemp.Add(messageBuffer[i]);
                        }

                        var temp = new List<byte>();
                        for (int i = point; i < messageBuffer.Count; i++)
                            temp.Add(messageBuffer[i]);
                        messageBuffer = temp;
                    }

                    if (messageBuffer.Count > 0)
                    {
                        if (EncoderMisc.IsItCorrectHeader(EncoderMisc.GetHeaderBytes(messageBuffer.ToArray(), ConnectionType.TCP), ConnectionType.TCP))
                            messagesList.Add(messageBuffer.ToArray());
                        else
                        {
                            List<byte[]> temp = messagesStack;
                            messagesStack.Add(messageBuffer.ToArray());
                            messagesStack.AddRange(temp);
                        }
                    }
                }
                ConnectionTcp.StatusMaximum = 0;
                foreach (var message in messagesList)
                    MessageWorker(message);
            }
            catch (Exception e)
            {
                DebugPart.ErrorsList.Add(e.StackTrace);
                throw;
            }
        }
        private void MessageWorker(byte[] message)
        {
            if (message == null || message.Length < EncoderMisc.headerTcpLength)
                return;
            string translatedMessage = TranslateTcp(message);
            string header = EncoderMisc.GetHeaderString(message, ConnectionType.TCP);

            if (translatedMessage.Length <= header.Length)
            {
                AddData(ConnectionTcp.unknowMessages, translatedMessage);
                return;
            }
            translatedMessage = translatedMessage.Substring(header.Length);
            translatedMessage = translatedMessage.Trim();
            switch (header)
            {
                case "xCMD8":
                    AddData(ConnectionTcp.xCMD, translatedMessage);
                    return;
                case "xFIX":
                    if (!ConnectionTcp.CmndDone)
                        ConnectionTcp.CmndDone = true;
                    AddData(ConnectionTcp.xFIX, translatedMessage);
                    return;
                case "xNAVH":
                    AddData(ConnectionTcp.xNAVH, translatedMessage);
                    if (!ConnectionTcp.FixDone)
                        ConnectionTcp.FixDone = true;
                    return;
                case "xAPTP":
                    AddData(ConnectionTcp.xAPTP, message);
                    if (!ConnectionTcp.NavDone)
                        ConnectionTcp.NavDone = true;
                    return;
                case "xRAD":
                    AddData(ConnectionTcp.xRAD, translatedMessage);
                    return;
                default:
                    AddData(ConnectionTcp.unknowMessages, translatedMessage);
                    DebugPart.WriteLog($"Unknow header ({header} {translatedMessage})");
                    return;
            }

        }

        private string TranslateTcp(byte[] bytes)
        {
            byte[] data = EncoderMisc.CutZeroBytes(bytes);
            //ConnectionTCP.MessagesTcpAll.Add(data);
            if (data.Length == 0)
                return "";
            string header = EncoderMisc.GetHeaderString(data, ConnectionType.TCP);
            if (!EncoderMisc.IsItCorrectHeader(header, ConnectionType.TCP))
                return $"---Corrupted({header})!---";

            try
            {
                int bbi = 1;
                string output = "";
                for (int i = header.Length; i < data.Length; i++)
                {
                    if (i == header.Length && data.Length - EncoderMisc.headerTcpLength + 2 > i 
                                           && data[i] + data[i + 1] + data[i + 2] == 0)
                    {
                        output += " ";
                        i += 2;
                        continue;
                    }
                    if (EncoderMisc.isByteChar(data[i]))
                    {
                        output += ((Char)data[i]).ToString();
                    }
                    else
                    {
                        if (header == "xCMD8")
                            switch (data[i])
                            {
                                case 95:
                                    output += "_";
                                    break;
                                case 45:
                                    output += "-";
                                    break;
                                case 171:
                                    output += "«";
                                    break;
                                case 187:
                                    output += "»";
                                    break;
                                default:
                                    DebugPart.WriteLog($"Corrupted TCP XCMD8 message ({output})");
                                    output = "--corrupted!--";
                                    return output;
                                    break;
                            }
                        else
                            output += $".{data[i]}";
                    }
                }
                if (!EncoderMisc.IsItCorrectHeader(header, ConnectionType.TCP))
                {

                    if (header.Contains("FIX"))
                        DebugPart.WriteLog($"Corrupted TCP FIX message ({output})");
                }
                return $"{header}{output}";
            }
            catch (Exception e)
            {
                DebugPart.WriteLog(e);
                return "";
            }
        }

        private void AddData(List<string> list, string data)
        {
            list.Add(data);
        }
        private void AddData(List<byte[]> list, byte[] data)
        {
            list.Add(data);
        }


        private bool ReworkAirportsListWork = false;
        public void ReworkAirportsList()
        {
            if (!ReworkAirportsListWork)
                ReworkAirportsListWork = true;
            else return;

            ConnectionTcp.Airports = new List<XAirport>();
            List<byte[]> Apts = ConnectionTcp.xAPTP;
            ConnectionTcp.xAPTP = new List<byte[]>();

            byte[] header = new[] {(byte)120, (byte)65, (byte)80, (byte)84, (byte)80, (byte)0, (byte)0, (byte)0 };
            int step = 0;

            #region Steps enum

            /*
            0 - имя аэропорта
            1 - данные аэропорта
            2 - полоса
            -1 - сломанное именование
            */

            #endregion

            foreach (var AptBytes in Apts)
            {
                try
                {
                    XAirport airport = new XAirport();
                    int start = 0;
                    List<byte> bytesTemp = new List<byte>();
                    for (int i = 0; i < AptBytes.Length; i++)
                    {
                        byte bb = AptBytes[i];
                        if (bb == 120 && i + 10 < AptBytes.Length)
                        {
                            byte[] exBytes = EncoderMisc.TakeBytes(AptBytes, i, 8);
                            if (EncoderMisc.GetHeaderString(header, ConnectionType.TCP) == EncoderMisc.GetHeaderString(exBytes, ConnectionType.TCP))
                            {
                                if (airport.CodeString != "")
                                {
                                    ConnectionTcp.Airports.Add(airport);
                                    start = i;

                                }
                                i += header.Length;
                                airport = new XAirport();
                                step = 0;
                                bytesTemp = new List<byte>();

                                //заполняем код аэропорта
                                for (int j = 0; j < 4; j++)
                                {
                                    if (!EncoderMisc.isByteChar(bb))
                                    {
                                        step = -1;
                                        airport.CodeString = "";
                                        break;
                                    }
                                    airport.CodeByte[j] = AptBytes[i];
                                    airport.CodeString += ((Char)airport.CodeByte[j]).ToString();
                                    i++;
                                }

                                //пропускаем 4 байта после кода (3 тут, 1 циклом)
                                i += 3;
                                continue;
                            }
                        }

                        switch (step)
                        {
                            case 0:
                                if (bb != 0)
                                    airport.NameString += ((Char)bb).ToString();
                                else
                                    step++;
                                break;
                            case 1:
                                if (EncoderMisc.isCapitalLetter(bb) && i + 2 < AptBytes.Length
                                                                    && EncoderMisc.isNumeric(AptBytes[i + 1])
                                                                    && EncoderMisc.isNumeric(AptBytes[i + 2]))
                                {
                                    step = 2;
                                    //airport.DataBytes = bytesTemp.ToArray();
                                    bytesTemp = new List<byte>();
                                }
                                else
                                    bytesTemp.Add(bb);
                                break;
                            case 2:
                                if (EncoderMisc.isCapitalLetter(bb) && i + 2 < AptBytes.Length
                                                                    && EncoderMisc.isNumeric(AptBytes[i + 1])
                                                                    && EncoderMisc.isNumeric(AptBytes[i + 2]))
                                {
                                    airport.Add(bytesTemp.ToArray());
                                    airport.Rws.Add(airport.Add(bytesTemp.ToArray()));
                                    bytesTemp = new List<byte>();
                                }
                                else
                                    bytesTemp.Add(bb);
                                break;
                            case -1:
                                break;
                        }


                    }

                    airport.Rws.Add(airport.Add(bytesTemp.ToArray()));
                    ConnectionTcp.Airports.Add(airport);
                }
                catch (Exception e)
                {
                    DebugPart.WriteLog(e);
                }
            }

            ConnectionTcp.AptpDone = true;
        }
    }

    class WWWEncoder
    {

    }

}