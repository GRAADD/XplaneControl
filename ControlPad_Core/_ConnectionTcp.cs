using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
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
using System.Threading.Tasks;
using XplaneConnection_NF;

namespace XplaneControl
{
    public static class ConnectionTcp
    {
        public static EncoderTcp TcpEncoder;
        public static TcpClient xPlaneTCP;
        //private static IPEndPoint xPlaneTcpPoint;
        private static NetworkStream streamTCP;
        private static readonly byte[] _introduceMessage = CommandEncoder.encodeCommandToBytes("CON1", ConnectionType.TCP);

        private static int localTcpPort = 49703;
        //private static int localTcpPort2 = 49631;
        private static int _tcpPacketLength = 256;//64294

        public static string responseTCP;


        public static string messageTCP = "";

        public static List<string> TerminaList = new List<string>();
        

        public static List<string> TcpHeaders = new List<string>() { "xCMD8", "xRAD", "xNAVH", "xAPTP", "xFIX", "xAPTl", "xAPT" };//"xLOC", "xFAL", "xDIM", "xACF", "xFIX", 
        public static List<string> CmndHeaders = new List<string>() { "CMND", "xFAL", "xDIM", "xACF", "xFIX", "xCMD", "xRAD", "xCON", "ACFN" };

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
        
        public static List<byte[]> BytesList = new List<byte[]>();

        public static void TcpLoop()
        {
            byte[] conMessage = CommandEncoder.encodeCommandToBytes("CON1", ConnectionType.TCP);
            byte[] emptyBytes = new byte[0];
            Thread.CurrentThread.Name = "Tcp Loop";
            try
            {
                Connect();

                int emptyMessagesCount = 0;
                List<byte> bytes = new List<byte>();
                while (!TcpFinished)
                {
                    byte[] data = new byte[512];

                    while (streamTCP.DataAvailable)
                    {
                        streamTCP.Read(data, 0, data.Length);
                        bytes.AddRange(data);
                        bytes = TcpEncoder.EncodeBytes(bytes.ToArray());
                        //TcpEncoder.TcpDataToMessage(data.ToArray(), ref responseTCP);
                    } 

                    if (bytes.Count == 0)
                    {
                        emptyMessagesCount++;
                        if (emptyMessagesCount/100 == 3)
                        {
                            if (!xPlaneTCP.Client.Connected)
                                xPlaneTCP.Client.Connect(ConnectionCore.MasterIp, ConnectionUdp.masterPortOut);
                            if (!streamTCP.DataAvailable)
                                streamTCP = xPlaneTCP.GetStream();
                            streamTCP.Write(emptyBytes, 0, emptyBytes.Length);
                        }
                        if (emptyMessagesCount / 10000 > 6)
                        {
                            if (NavDone)
                            {
                                TcpFinished = true;
                                ConnectionTcp.AptpDone = true;
                                SaveApts();
                            }
                            else
                            {
                                Connect();
                                emptyMessagesCount = 0;
                            }
                        }
                    }
                    else
                    {
                        //bytes = TcpEncoder.EncodeBytes(bytes.ToArray());
                        emptyMessagesCount = 0;
                    }

                    //Task.Delay(TimeToWait).Wait();

                }

            }
            catch (SocketException se)
            {
                //Logger.WriteLog(se, ErrorLevel.Base); 
                //throw;
            }
            catch (Exception e)
            {
                //Logger.WriteLog(e, ErrorLevel.Error);
                //throw;
            }
            
            TcpFinished = true;
            //Console.WriteLine(TcpEncoder.debug);
            //return Task.CompletedTask;
        }

        public static void TcpConveyer()
        {
            if (TcpEncoder == null)
                TcpEncoder = new EncoderTcp();
            Thread encoderThread = new Thread(TcpEncoder.EncoderConveyer);
            encoderThread.Start();

            byte[] conMessage = CommandEncoder.encodeCommandToBytes("CON1", ConnectionType.TCP);
            byte[] emptyBytes = new byte[0];
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
                //Logger.WriteLog(se, ErrorLevel.Base); 
                //throw;
            }
            catch (Exception e)
            {
                //Logger.WriteLog(e, ErrorLevel.Error);
                //throw;
            }
            
            TcpFinished = true;
            //Console.WriteLine(TcpEncoder.debug);
            //return Task.CompletedTask;
        }

        private static void Connect()
        {
            //Logger.WriteLog("Tcp connection start!", ErrorLevel.Info);
            TcpEncoder = new EncoderTcp();
            xPlaneTCP = new TcpClient(ConnectionCore.MasterIp, ConnectionUdp.masterPortOut);
            Byte[] dataSend = _introduceMessage;
            streamTCP = xPlaneTCP.GetStream();
            streamTCP.Write(dataSend, 0, dataSend.Length);
            Task.Delay(TimeToWait).Wait();

        }


        public static XAirport GetAirport(string name)
        {
            foreach (var apt in Airports)
            {
                if (apt.CodeString == name || apt.NameString == name)
                    return apt;
            }
            return new XAirport(new byte[0]);
        }

        public static List<XAirport> GetAirports(string name)
        {
            List<XAirport> list = new List<XAirport>();
            foreach (var apt in Airports)
            {
                if (apt.CodeString.Contains(name) || apt.NameString.Contains(name))
                    list.Add(apt);
            }
            return list;
        }

        private static readonly string aptsFile = "apts.bin";

        private static void LoadApts()
        {
            if (File.Exists(aptsFile))
            {
                xAPTP = new List<byte[]>();
                StreamReader sr = new StreamReader(aptsFile);
                while (!sr.EndOfStream)
                {
                    //xAPTP.Add((byte[])sr.ReadLine());
                }
            }
        }

        public static void SaveApts()
        {
            StreamWriter sw = new StreamWriter(aptsFile);
            foreach (byte[] bytes in xAPTP)
            {
                sw.Write(bytes);
            }
            sw.Close();
            sw.Dispose();
        }
    }
}