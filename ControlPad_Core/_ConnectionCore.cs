using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
//using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Dns;
////using Xamarin.Android.Net;
using XplaneConnection_NF;


namespace XplaneControl
{
    public static class ConnectionCore
    {
        public static bool Stop = false;
        private static readonly byte[] _introduceMessage = CommandEncoder.encodeCommandToBytes("CON1", ConnectionType.UDP);

        public static List<string> MachinesInNetwork = new List<string>();
        public static List<string> MastersList = new List<string>();

        public static string MasterIp = "";
        public static IPAddress ipAddress;
        public static string SlaveIp = "";

        public static bool MasterFound = false;
        public static bool MasterSet = false;
        public static bool MasterConnected = false;
        private static int TimeToFindMaster = 1000;

        private const bool DebugMode = false;
        public static bool IsXamarin = true;

        public static Thread UDPConnectionThread;
        public static Thread TCPConnectionThread;

        public static void Init()
        {
            ConnectionTcp.TcpFinished = false;
            ConnectionUdp.forceStopUdp = false;

            while (!Stop)//MastersList.Count == 0 &&
            {
                MachinesInNetwork = new List<string>();
                int timeout = 1;
                while (MachinesInNetwork.Count < 2)
                {
                    timeout = timeout * 2;
                    if (IsXamarin)
                    {
                        XamarinScanNetwork(timeout);
                    }
                    else
                    {
                        ScanNetwork();
                    }
                    Task.Delay(50).Wait();
                }

                for (int i = 0; i < MachinesInNetwork.Count; i++)
                {
                    CheckForMaster(MachinesInNetwork[i]);
                }
                //Task.Delay(1000).Wait();
            }
            ConnectionTcp.TcpFinished = false;
            ConnectionUdp.forceStopUdp = false;
        }

        public static void Init(string ip)
        {
            for (int i = 0; i < 3; i++)
            {
                CheckForMaster(ip);
                if (MasterFound)
                {
                    MastersList.Add(ip);
                    break;
                }
            }
        }
        
        private static void ScanNetwork()
        {

            IPHostEntry ipE = GetHostByName(GetHostName());
            IPAddress[] ipA = ipE.AddressList;

            int found = 0;

            for (int i = 0; i < ipA.Length; i++)
            {
                string[] splitted = ipA[i].ToString().Split('.');
                if (splitted.Length != 4)
                    continue;
                if (splitted[3] == "1" || splitted[0] == "10")
                    continue;
                if (splitted[2] == "232")
                {
                    splitted[2] = "5";
                    splitted[3] = "17";
                }

                if (splitted[0] == "192")
                    SlaveIp = ipA[i].ToString();
                GetMachinesByDns($"{splitted[0]}.{splitted[1]}.{splitted[2]}.1");
                //GetMachinesByPing(splitted);
            }
        }

        private static void XamarinScanNetwork(int timeout)
        {
            IPHostEntry ipE = GetHostByName(GetHostName());
            IPAddress[] ipA = ipE.AddressList;
            for (int i = 0; i < ipA.Length; i++)
            {
                string[] splitted = ipA[i].ToString().Split('.');
                if (splitted.Length != 4)
                    continue;
                if (splitted[3] == "1" || splitted[0] == "10")
                    continue;
                if (splitted[2] == "232")
                {
                    splitted[2] = "5";
                    splitted[3] = "17";
                }

                if (splitted[0] == "192")
                    SlaveIp = ipA[i].ToString();

                //CountdownEvent countdown = new CountdownEvent(1);
                int upCount = 0;
                object lockObj = new object();
                bool resolveNames = true;
                //Stopwatch sw = new Stopwatch();
                //sw.Start();
                string ipBase = $"{splitted[0]}.{splitted[1]}.{splitted[2]}.";
                List<string> found = new List<string>();
                List<Thread> pingThreads = new List<Thread>();

                int pingedCount = 0;
                for (int j = 1; j < 255; j++)
                {
                    string ip = ipBase + j.ToString();

                    Ping p = new Ping();
                    p.PingCompleted += (sender, e) =>
                    {
                        pingedCount--;
                        string eUserState = (string)e.UserState;
                        if (e.Reply != null && e.Reply.Status == IPStatus.Success)
                        {
                            if (resolveNames)
                            {
                                string name;
                                try
                                {
                                    IPHostEntry hostEntry = Dns.GetHostEntry(eUserState);
                                    name = hostEntry.HostName;
                                }
                                catch (SocketException ex)
                                {
                                    name = "?";
                                }
                                //Console.WriteLine("{0} ({1}) is up: ({2} ms)", eUserState, name, e.Reply.RoundtripTime);
                                AddToList(found, name);
                            }
                            else
                            {
                                //Console.WriteLine("{0} is up: ({1} ms)", eUserState, e.Reply.RoundtripTime);
                            }
                            lock (lockObj)
                            {
                                upCount++;
                            }
                        }
                        else if (e.Reply == null)
                        {
                            //Console.WriteLine("Pinging {0} failed. (Null Reply object?)", eUserState);
                        }
                        //countdown.Signal();
                    };
                    //countdown.AddCount();
                    //Thread pingThread = new Thread(() =>
                    //{
                    //    p.Send(ip, 10);
                    //});
                    //pingThreads.Add(pingThread);
                    pingedCount++;

                    p.SendAsync(ip, timeout, ip);
                }

                //foreach (Thread pingThread in pingThreads)
                //{
                //    pingThread.Start();
                //}
                while (pingedCount > 0)
                {
                    Task.Delay(timeout).Wait();
                }

                //countdown.Signal();
                //countdown.Wait();
                //sw.Stop();
                //TimeSpan span = new TimeSpan(sw.ElapsedTicks);
                MachinesInNetwork.AddRange(found);
                //Console.WriteLine("Took {0} milliseconds. {1} hosts active.", sw.ElapsedMilliseconds, upCount);
                //GetMachinesByPing(splitted);
            }
        }

        private static void GetMachinesByDns(string hostString)
        {
            IPHostEntry hostInfo = GetHostEntry(hostString);
            foreach (var ip in hostInfo.AddressList)
            {
                AddToList(MachinesInNetwork, ip.ToString());
            }
        }

        private static void GetMachinesByPing(string[] splitted)
        {
            List<Thread> pingThreads = new List<Thread>();
            int threadsCount = 0;

            for (int j = 1; j < 256; j++)
            {
                string ip = $"{splitted[0]}.{splitted[1]}.{splitted[2]}.{j}";
                pingThreads.Add(new Thread((() =>
                {
                    Ping pingSender = new Ping();
                    IPAddress address = IPAddress.Parse(ip);
                    try
                    {
                        PingReply reply = pingSender.Send(address, 500);
                        if (reply.Status == IPStatus.Success)
                        {
                            if (!MachinesInNetwork.Contains(reply.Address.ToString()))
                            {
                                AddToList(MachinesInNetwork, reply.Address.ToString());
                                //Logger.WriteLog($"Found machine at {reply.Address}", ErrorLevel.Base);
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

                    threadsCount--;
                })));
            }

            threadsCount = pingThreads.Count;
            foreach (var pingthread in pingThreads)
                pingthread.Start();
            while (threadsCount > 0)
            {
                threadsCount = 0;
                foreach (var thread in pingThreads)
                {
                    if (thread.IsAlive)
                        threadsCount++;
                }
                Thread.Sleep(2);
            }
        }

        private static void CheckForMaster(string ipString)
        {
            if (string.IsNullOrEmpty(ipString))
                return;
            UdpClient udp = new UdpClient();
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ipString);
                udp = new UdpClient(48003);
                udp.Send(_introduceMessage, _introduceMessage.Length, ipString, ConnectionUdp.masterUdpPortIn);
                IPEndPoint xplaneEndPoint = new IPEndPoint(ipAddress, 0);
                Task.Delay(150).Wait();
                udp.Client.ReceiveTimeout = 5000;
                byte[] data = udp.Receive(ref xplaneEndPoint);
                if (data.Length > 0 && xplaneEndPoint.Address.ToString() == ipString)
                {
                    AddToList(MastersList, ipString);
                    //Logger.WriteLog($"Found Master at {ipString}", ErrorLevel.Info);
                    MasterIp = ipString;
                    MasterFound = true;
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
            udp?.Dispose();
        }

        public static void MessagesAllAdd(byte[] data, ConnectionType connectionType)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (!Enum.IsDefined(typeof(ConnectionType), connectionType))
                throw new InvalidEnumArgumentException(nameof(connectionType), (int) connectionType,
                    typeof(ConnectionType));
            if (DebugMode)
            {
                if (connectionType == ConnectionType.TCP)
                    ConnectionTcp.MessagesTcpAll.Add(data);
                else
                    ConnectionUdp.MessagesUdpAll.Add(data);

            }
        }

        private static void AddToList(List<string> list, string str)
        {
            if (!list.Contains(str))
                list.Add(str);
        }

        public static void CloseConnections()
        {
            MasterFound = false;
            ConnectionTcp.TcpFinished = true;
            ConnectionUdp.forceStopUdp = true;
        }

        public static UdpHeader getEnum(string header)
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

        public static async void ConnectToMaster(string master)
        {
            if (UDPConnectionThread == null && TCPConnectionThread == null)
            {
                MasterIp = master;
                MasterSet = true;
                MasterConnected = true;
                //await ConnectionUdp.UdpLoop();
                //await ConnectionTcp.TcpLoop();

                //TCPConnectionThread = new Thread(ConnectionTcp.TcpLoop);
                TCPConnectionThread = new Thread(ConnectionTcp.TcpConveyer);
                TCPConnectionThread.Start();
                UDPConnectionThread = new Thread(ConnectionUdp.UdpLoop);
                UDPConnectionThread.Start();

                //Connect_Button.Text = "Disconnect";
            }
            else
            {
                End();
            }

        }

        public static void End()
        {
            CloseConnections();
            MasterSet = false;
            ConnectionCore.Stop = true;
            ConnectionTcp.forceStopTcp = true;
            ConnectionUdp.forceStopUdp = true;
            UDPConnectionThread?.Abort();
            TCPConnectionThread?.Abort();
        }
    }
}
