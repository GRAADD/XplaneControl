using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XplaneConnection
{
    public class XMessage
    {
        public string HeaderString = "";
        public byte[] HeaderBytes;
        public string MessageString = "";
        public byte[] MessageBytes;
        public ConnectionType Type;
        public void MakeHeader()
        {

            if (string.IsNullOrEmpty(HeaderString) && HeaderBytes == null && MessageBytes != null)
            {
                HeaderString = EncoderMisc.GetHeaderString(MessageBytes, Type);
                return;
            }

            if (!string.IsNullOrEmpty(HeaderString) && HeaderBytes == null)
            {
                HeaderBytes = EncoderMisc.GetHeaderBytes(HeaderString, Type);
                return;
            }

            if (string.IsNullOrEmpty(HeaderString) && HeaderBytes.Length != 0)
            {
                HeaderString = EncoderMisc.GetHeaderString(HeaderBytes, Type);
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

    }

    public class XAirport
    {
        public byte[] CodeByte = new byte[4];
        public string CodeString = "";
        public string NameString = "";
        public byte[] DataBytes;
        public List<Rw> Rws = new List<Rw>();

        public Rw Add(byte[] data)
        {
            if (data.Length < 4)
                return new Rw();
            Rw rw = new Rw();
            int count = 0;
            byte bb = data[count];
            rw.Rw1Byte = new List<byte>();
            rw.Rw1String = "";
            while (bb != 0)
            {
                rw.Rw1Byte.Add(bb);
                rw.Rw1String += ((Char)bb).ToString();
                count++;
                if (count >= data.Length)
                    return new Rw();
                bb = data[count];
                //Rw
            }
            if (count >= data.Length)
                return rw;
            count++;
            bb = data[count];
            rw.Rw2Byte = new List<byte>();
            rw.Rw2String = "";
            if (EncoderMisc.isCapitalLetter(bb))
            {
                if (count >= data.Length)
                    return new Rw();
                count++;
                bb = data[count];
                while (bb != 0 && EncoderMisc.isNumeric(bb))
                {
                    rw.Rw2Byte.Add(bb);
                    rw.Rw2String += ((Char)bb).ToString();
                    if (count == data.Length)
                        return rw;
                    count++;
                    bb = data[count];
                    //Rw
                }

            }
            rw.DataBytes = new List<byte>();
            for (int i = count; i < data.Length; i++)
                rw.DataBytes.Add(data[count]);
            //Rws.Add(rw);
            return rw;
        }

        public struct Rw
        {
            public List<byte> Rw1Byte;
            public string Rw1String;
            public List<byte> Rw2Byte;
            public string Rw2String;
            public List<byte> DataBytes;
        }
    }

    public enum ConnectionType
    {
        TCP,
        UDP,
        Command
    }
    
    static class ConnectionCore
    {
        public static string MasterIp = "";
        public static string SlaveIp = "";

        private static List<string> pcList = new List<string>();
        private static byte[] IntroduceMessage = CommandEncoder.encodeCommandToBytes("CON1", ConnectionType.UDP);
        public static List<string> MasterIpsList { get; private set; } = new List<string>();


        public static bool MasterFound = false;
        private static int TimeToFindMaster = 1000;

        private const bool DebugMode = false;

        public static async void Init()
        {
            DebugPart.WriteLog(SystemInformation.ComputerName);
            DebugPart.WriteLog($".Net version is {Environment.Version}");
            DebugPart.WriteLog($" Network connection is: {SystemInformation.Network.ToString()}");
            ConnectionUdp.xPlaneUDP = new UdpClient(ConnectionUdp.localUdpPort);
            IPHostEntry ipHostEntry = Dns.GetHostEntry(Dns.GetHostName());
            List<string> ips = new List<string>();
            DebugPart.WriteLog("Initiating connection...");

            pcList = new List<string>();
            while (MasterIp == "" && !ConnectionTcp.forceStopTcp)
            {
                foreach (var slaveIp in ipHostEntry.AddressList)
                {
                    var ipv4 = false;
                    try
                    {
                        string[] ipp = slaveIp.ToString().Split('.');
                        ipv4 = ipp.Length == 4;
                    }
                    catch (Exception)
                    {
                        ipv4 = false;
                    }

                    if (ipv4)// && !_searchStarted
                    {
                        string[] ipSplitted = slaveIp.ToString().Split('.');
                        DebugPart.WriteLog($"Searching in {ipSplitted[0]}.{ipSplitted[1]}.{ipSplitted[2]}.0");
                        for (int i = 1; i < 256; i++)
                        {
                            slaveIpTemp = slaveIp.ToString();
                            string ip = $"{ipSplitted[0]}.{ipSplitted[1]}.{ipSplitted[2]}.{i}";
                            Thread checkThread = new Thread(new ParameterizedThreadStart(MasterFinder)); 
                            checkThread.Start(ip);
                            //MasterFinder(ipMasterAndSlave);
                        }
                    }
                }
                Thread.Sleep(TimeToFindMaster);
            } //получаем все адреса из подсети

        }

        private static string slaveIpTemp = "";
        private static bool _searchStarted = false;

        private static void MasterFinder(object masterIpT)
        {
            string masterIp = masterIpT.ToString();
            try
            {
                Thread.CurrentThread.Name = "ConnectionTCP try to " + masterIp;
                string localIp = slaveIpTemp;
                System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                //p.Site.Component.
                System.Net.NetworkInformation.PingReply rep = p.Send(masterIp, 1500);
                if (rep.Status != System.Net.NetworkInformation.IPStatus.Success)
                {
                    return;
                }
                if (!pcList.Contains(masterIp))
                    pcList.Add(masterIp);
                else return;
                DebugPart.WriteLog($"Checking {masterIp}");

                IPAddress masterAddress = IPAddress.Parse(masterIp);
                UdpClient udp = new UdpClient();
                int ConnectionMessageLength = IntroduceMessage.Length;

                ConnectionUdp.xPlaneUDP.Send(IntroduceMessage, ConnectionMessageLength, masterAddress.ToString(), ConnectionUdp.masterUdpPortIn);
                IPEndPoint xplaneEndPoint = new IPEndPoint(masterAddress, 0);
                ConnectionUdp.xPlaneUDP.Client.ReceiveTimeout = TimeToFindMaster;

                byte[] data = ConnectionUdp.xPlaneUDP.Receive(ref xplaneEndPoint);
                if (data.Length > 0)
                {
                    MasterIp = xplaneEndPoint.Address.ToString();
                    MasterIpsList.Add(MasterIp);
                    ConnectionUdp.masterPortOut = xplaneEndPoint.Port;
                    SlaveIp = localIp;
                    MasterFound = true;
                    DebugPart.WriteLog($"Master ip = {MasterIp}");
                    DebugPart.WriteLog($"Slave ip = {localIp}");
                }

                udp.Close();
            }
            catch (SocketException e)
            {
                //DebugPart.WriteLog($"No connection for {masterIp}");
            }
            catch (Exception e)
            {
                DebugPart.WriteLog(e);
            }
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
    }

    static class EncoderMisc
    {
        public static int headerUdpLength = 4;
        public static int headerTcpLength = 5;
        public static int headerCommandLength = 4;

        public static int GetLength(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.UDP:
                    return EncoderMisc.headerUdpLength;
                case ConnectionType.TCP:
                    return EncoderMisc.headerTcpLength;
                case ConnectionType.Command:
                    return EncoderMisc.headerTcpLength;
                default:
                    return 0;
            }
        }

        public static byte[] CutZeroBytes(byte[] data)
        {
            if (data.Length == 0)
            {
                return data;
            }
            List<byte> bytes = new List<byte>();
            bytes.AddRange(data);
            for (int i = bytes.Count; i > 0; i--)
                if (bytes[i - 1] == 0)
                    bytes.RemoveAt(i - 1);
                else
                    return bytes.ToArray();
            return bytes.ToArray();
        }

        public static string GetHeaderString(byte[] data, ConnectionType connectionType)
        {
            string output = "";
            try
            {
                byte[] headerBytes = GetHeaderBytes(data, connectionType);
                foreach (var bt in headerBytes)
                    output += ((Char)bt).ToString();
                if (output.Contains("xFIX"))
                    return "xFIX";
            }
            catch (Exception e)
            {
                DebugPart.ErrorsList.Add(output);
                DebugPart.ErrorsList.Add(e.StackTrace);
                return "--Corrupted!--";
            }
            return output;
        }

        private static int GetHeaderLength(ConnectionType connectionType)
        {
            switch (connectionType)
            {
                case ConnectionType.TCP:
                    return headerTcpLength;
                case ConnectionType.UDP:
                    return headerUdpLength;
                case ConnectionType.Command:
                    return headerCommandLength;
            }
            return -1;
        }

        public static string GetHeaderString(string message, ConnectionType connectionType)
        {
            return message.Substring(0, GetHeaderLength(connectionType)); ;
        }

        public static byte[] GetHeaderBytes(byte[] message, ConnectionType connectionType)
        {
            int hdrLength = GetHeaderLength(connectionType);
            byte[] output = new byte[hdrLength];
            int max = 0;
            max = message.Length < hdrLength ? message.Length : hdrLength;
            for (int i = 0; i < max; i++)
                output[i] = message[i];
            for (int i = max; i < hdrLength; i++)
                output[i] = 0;
            return output;
        }
        public static byte[] GetHeaderBytes(string message, ConnectionType connectionType)
        {
            int hdrLength = GetHeaderLength(connectionType);
            byte[] output = new byte[hdrLength];
            int max = 0;
            max = message.Length < hdrLength ? message.Length : hdrLength;
            for (int i = 0; i < max; i++)
                output[i] = (byte)message[i];
            for (int i = max; i < hdrLength; i++)
                output[i] = 0;
            return output;
        }

        public static bool isByteChar(byte bb)
        {
            if (isCapitalLetter(bb))
                return true;
            if (isLowercaseLetter(bb))
                return true;
            if (isNumeric(bb))
                return true;
            if (bb >= 46 && bb <= 48)
            {
                return true;//simbols . and / and digits 0-9
            }
            return false;
        }

        public static bool isCapitalLetter(byte bb)
        {
            if (bb >= 65 && bb <= 91)
            {
                return true; //letters A-Z
            }
            else return false;
        }
        public static bool isLowercaseLetter(byte bb)
        {
            if (bb >= 97 && bb <= 122)
            {
                return true;//letters a-z
            }
            else return false;
        }
        public static bool isNumeric(byte bb)
        {
            if (bb >= 48 && bb <= 57)
            {
                return true;//simbols . and / and digits 0-9
            }
            else return false;
        }

        public static bool IsItCorrectHeader(string text, ConnectionType connectionType)//xCMD amd other...
        {
            int hdrLength = GetHeaderLength(connectionType);
            string header = "";
            header = text.Length < hdrLength ? text : text.Substring(0, hdrLength);

            if (connectionType == ConnectionType.TCP)
            {
                return ConnectionTcp.TcpHeaders.Contains(header);
            }
            else
                return ConnectionUdp.UdpHeaders.Contains(header);
        }
        public static bool IsItCorrectHeader(byte[] data, ConnectionType connectionType)//xCMD amd other...
        {
            string header = GetHeaderString(data, ConnectionType.TCP);

            if (connectionType == ConnectionType.TCP)
            {
                if (header == "xFIX")
                    return true;
                return ConnectionTcp.TcpHeaders.Contains(header);
            }
            else
                return ConnectionUdp.UdpHeaders.Contains(header);
        }
        public static string FancyHex(string input, string split)
        {
            string output = "";
            for (int i = 0; i < input.Length / 2; i++)
            {
                if (i != 0)
                    output += split;
                output += input[i * 2].ToString() + input[i * 2 + 1].ToString();
            }
            return output;
        }

        public static string BytesToString(byte[] bytes, string splitter)
        {
            string output = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                if (i != 0)
                    output += splitter;
                string bb = bytes[i].ToString();
                if (bb.Length == 1)
                    bb = "00" + bb;
                if (bb.Length == 2)
                    bb = "0" + bb;
                output += bb;
            }
            return output;
        }

        public static byte[] BytesFromHex(string input)
        {
            byte[] bytes = new byte[input.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                string hex = $"{input[i * 2]}{input[i * 2 + 1]}";
                bytes[i] = Convert.ToByte(Int32.Parse(hex, NumberStyles.HexNumber));
            }
            return bytes;
        }

        public static byte[] TakeBytes(byte[] data, int start, int length)
        {
            byte[] output = new byte[length];
            for (int i = start; i < length; i++)
                output[i - start] = data[i];
            return output;
        }
    }

    class Atmosphere
    {
        public enum Type
        {
            TDate,
            TTime,
            TVisibility,
            TRain,
            TStorm,
            TRunWayStat,
            TRunWayPatches,
            TTemperature,
            TBaro,
            TTermCov,
            TTermStr,
            TWindLayer3Alt,
            TWindLayer2Alt,
            TWindLayer1Alt,
            TWindLayer3Dir,
            TWindLayer2Dir,
            TWindLayer1Dir,
            TWindLayer3Str,
            TWindLayer2Str,
            TWindLayer1Str,
            TWindLayer3Gusts,
            TWindLayer2Gusts,
            TWindLayer1Gusts,
            TWindLayer3Turb,
            TWindLayer2Turb,
            TWindLayer1Turb,
            TCloudsLayer3Top,
            TCloudsLayer2Top,
            TCloudsLayer1Top,
            TCloudsLayer3Base,
            TCloudsLayer2Base,
            TCloudsLayer1Base,
            TCloudsLayer3Cat,
            TCloudsLayer2Cat,
            TCloudsLayer1Cat
        }
        public float Date;
        public float Timex;
        public float Visibility;
        public float Rain;
        public float Storm;
        public enum RwStat : int
        {
            dry,
            damp,
            wet
        }
        public RwStat RunWayStat;

        public bool RunWayPatches;

        public float temperature;
        public float baro;
        public float termCov;
        public float termStr;
        public CloudsLayer CloudsLayer3;
        public CloudsLayer CloudsLayer2;
        public CloudsLayer CloudsLayer1;
        public WindLayer WindLayer3;
        public WindLayer WindLayer2;
        public WindLayer WindLayer1;

        public struct CloudsLayer
        {
            public enum CloudsCat
            {
                ClearClouds,
                CirrusClouds,
                FewClouds,
                ScatteredClouds,
                BrokenClouds,
                OvercastClouds,
                StratusClouds
            }

            public CloudsCat CategoryClouds;
            public float BaseHight;
            public float TopHight;

            public float GetCatNum()
            {
                int cat = 0;
                switch (CategoryClouds)
                {
                    case Atmosphere.CloudsLayer.CloudsCat.ClearClouds:
                        cat = 0;
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.CirrusClouds:
                        cat = 1;
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.FewClouds:
                        cat = 2;
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.ScatteredClouds:
                        cat = 3;
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.BrokenClouds:
                        cat = 4;
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.OvercastClouds:
                        cat = 5;
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.StratusClouds:
                        cat = 6;
                        break;
                }
                return cat;
            }
            public string GetCatString()
            {
                string catString = "";
                switch (CategoryClouds)
                {
                    case Atmosphere.CloudsLayer.CloudsCat.ClearClouds:
                        catString = "Clear Clouds";
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.CirrusClouds:
                        catString = "Cirrus Clouds";
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.FewClouds:
                        catString = "Few Clouds";
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.ScatteredClouds:
                        catString = "Scattered Clouds";
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.BrokenClouds:
                        catString = "Broken Clouds";
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.OvercastClouds:
                        catString = "Overcast Clouds";
                        break;
                    case Atmosphere.CloudsLayer.CloudsCat.StratusClouds:
                        catString = "Stratus Clouds";
                        break;
                }
                return catString;
            }

        }

        public Atmosphere.CloudsLayer.CloudsCat GetCloudsCat(string cat)
        {
            CloudsLayer.CloudsCat cloudsCat = CloudsLayer.CloudsCat.ClearClouds;
            switch (cat)
            {
                case "Clear Clouds":
                    cloudsCat = Atmosphere.CloudsLayer.CloudsCat.ClearClouds;
                    break;
                case "Cirrus Clouds":
                    cloudsCat = Atmosphere.CloudsLayer.CloudsCat.CirrusClouds;
                    break;
                case "Few Clouds":
                    cloudsCat = Atmosphere.CloudsLayer.CloudsCat.FewClouds;
                    break;
                case "Scattered Clouds":
                    cloudsCat = Atmosphere.CloudsLayer.CloudsCat.ScatteredClouds;
                    break;
                case "Broken Clouds":
                    cloudsCat = Atmosphere.CloudsLayer.CloudsCat.BrokenClouds;
                    break;
                case "OvercastClouds":
                    cloudsCat = Atmosphere.CloudsLayer.CloudsCat.OvercastClouds;
                    break;
                case "StratusClouds":
                    cloudsCat = Atmosphere.CloudsLayer.CloudsCat.StratusClouds;
                    break;
            }

            return cloudsCat;
        }

        public struct WindLayer
        {
            public float Altitude;
            public float Direction;
            public float Strength;
            public float Gusts;
            public float Turbulence;
        }

        public string GetStringFromValue(float value, Type type)
        {
            switch (type)
            {
                case Type.TDate:
                    Date = value;
                    int daystoadd = (int)value;
                    int year = DateTime.Now.Year;
                    DateTime dateTime = new DateTime(year, 1, 1);
                    dateTime = dateTime.AddDays(daystoadd);
                    return dateTime.ToShortDateString();

                case Type.TTime:
                    Timex = value / 10;
                    return Timex.ToString();


                case Type.TVisibility:
                    Visibility = value / 1000;
                    if (Visibility < 1)
                        return $"{Visibility * 6076} ft";
                    else
                        return $"{Visibility} sm";

                case Type.TRain:
                    Rain = value;
                    return $"{Rain}%";
                case Type.TStorm:
                    Storm = value;
                    return $"{value}%";
                case Type.TTemperature:
                    temperature = value;
                    return $"{value} °C";
                case Type.TBaro:
                    Storm = value / 100;
                    return $"{Storm} inch";

                case Type.TTermCov:
                    termCov = value / 100;
                    return $"{termCov} rat";
                case Type.TTermStr:
                    termStr = value / 100;
                    return $"{termStr} fpm";


                case Type.TWindLayer3Alt:
                    WindLayer1.Altitude = value;
                    return $"{value} ft";
                case Type.TWindLayer2Alt:
                    WindLayer2.Altitude = value;
                    return $"{value} ft";
                case Type.TWindLayer1Alt:
                    WindLayer3.Altitude = value;
                    return $"{value} ft";

                case Type.TWindLayer3Dir:
                    WindLayer1.Direction = value;
                    return $"{value}°";
                case Type.TWindLayer2Dir:
                    WindLayer2.Direction = value;
                    return $"{value}°";
                case Type.TWindLayer1Dir:
                    WindLayer3.Direction = value;
                    return $"{value}°";

                case Type.TWindLayer3Str:
                    WindLayer1.Strength = value;
                    return $"{value} ft/m";
                case Type.TWindLayer2Str:
                    WindLayer2.Strength = value;
                    return $"{value} ft/m";
                case Type.TWindLayer1Str:
                    WindLayer3.Strength = value;
                    return $"{value} ft/m";

                case Type.TWindLayer3Gusts:
                    WindLayer1.Gusts = value - WindLayer1.Strength;
                    return $"{value} ft/m";
                case Type.TWindLayer2Gusts:
                    WindLayer2.Gusts = value - WindLayer2.Strength;
                    return $"{value} ft/m";
                case Type.TWindLayer1Gusts:
                    WindLayer3.Gusts = value - WindLayer3.Strength;
                    return $"{value} ft/m";

                case Type.TWindLayer3Turb:
                    WindLayer1.Turbulence = value;
                    return $"{value}%";
                case Type.TWindLayer2Turb:
                    WindLayer2.Turbulence = value;
                    return $"{value}%";
                case Type.TWindLayer1Turb:
                    WindLayer3.Turbulence = value;
                    return $"{value}%";


                case Type.TCloudsLayer3Top:
                    CloudsLayer3.TopHight = value;
                    return $"{value} ft";
                case Type.TCloudsLayer2Top:
                    CloudsLayer2.TopHight = value;
                    return $"{value} ft";
                case Type.TCloudsLayer1Top:
                    CloudsLayer1.TopHight = value;
                    return $"{value} ft";

                case Type.TCloudsLayer3Base:
                    CloudsLayer3.BaseHight = value;
                    return $"{value} ft";
                case Type.TCloudsLayer2Base:
                    CloudsLayer2.BaseHight = value;
                    return $"{value} ft";
                case Type.TCloudsLayer1Base:
                    CloudsLayer1.BaseHight = value;
                    return $"{value} ft";
                default:
                    return "";
            }
        }

        public string GetStringFromValue(string value, Type type)
        {
            switch (type)
            {
                case Type.TCloudsLayer1Cat:
                    CloudsLayer1.CategoryClouds = GetCloudsCat(value);
                    return "";
                case Type.TCloudsLayer2Cat:
                    CloudsLayer2.CategoryClouds = GetCloudsCat(value);
                    return "";
                case Type.TCloudsLayer3Cat:
                    CloudsLayer3.CategoryClouds = GetCloudsCat(value);
                    return "";
                default:
                    return "";
            }
        }

        public float GetCatNum(string cat)
        {
            int output = 0;
            switch (cat)
            {
                case "Clear Clouds":
                    output = 0;
                    break;
                case "Cirrus Clouds":
                    output = 1;
                    break;
                case "Few Clouds":
                    output = 2;
                    break;
                case "Scattered Clouds":
                    output = 3;
                    break;
                case "Broken Clouds":
                    output = 4;
                    break;
                case "Overcast Clouds":
                    output = 5;
                    break;
                case "Stratus Clouds":
                    output = 6;
                    break;
            }
            return output;
        }

        public string GetCatString(int cat)
        {
            string catString = "";
            switch (cat)
            {
                case 0:
                    catString = "Clear Clouds";
                    break;
                case 1:
                    catString = "Cirrus Clouds";
                    break;
                case 2:
                    catString = "Few Clouds";
                    break;
                case 3:
                    catString = "Scattered Clouds";
                    break;
                case 4:
                    catString = "Broken Clouds";
                    break;
                case 5:
                    catString = "Overcast Clouds";
                    break;
                case 6:
                    catString = "Stratus Clouds";
                    break;
            }
            return catString;
        }
    }

    static class DebugPart
    {
        public static List<string> ErrorsList = new List<string>();
        private static bool paranoid = false;
        private static string logfile = "log.log";

        public static void SaveLogs()
        {
            //создаем логфайл с ошибками
            if (ErrorsList.Count > 0)
            {
                StreamWriter sw = new StreamWriter(logfile, false);
                foreach (var line in DebugPart.ErrorsList)
                {
                    sw.WriteLine(line);
                }
                sw.Close();
                sw.Dispose();
            }
        }

        public static void WriteLog(Exception e)
        {
            AppendLog($"({DateTime.Now.ToLongTimeString()})Error!: {e.Message}");
            AppendLog(e.StackTrace);
            AppendLog("");
        }
        public static void WriteLog(StackOverflowException e)
        {
            AppendLog($"({DateTime.Now.ToLongTimeString()})Error!: {e.Message}");
            AppendLog(e.StackTrace);
            AppendLog("");
        }
        public static void WriteLog(ThreadAbortException e)
        {
            AppendLog($"({DateTime.Now.ToLongTimeString()})Error!: {e.Message}");
            AppendLog(e.StackTrace);
            AppendLog("");
        }
        public static void WriteLog(SocketException e)
        {
            AppendLog($"({DateTime.Now.ToLongTimeString()})Error!: {e.Message}");
            AppendLog(e.StackTrace);
            AppendLog("");
        }
        public static void WriteLog(string info)
        {
            AppendLog($"({DateTime.Now.ToLongTimeString()})Info: {info}");
        }

        private static void AppendLog(string line)
        {
            ErrorsList.Add(line);
            if (paranoid)
            {
                StreamWriter sw = new StreamWriter(logfile, true);
                sw.WriteLine(line);
                sw.Close();
                sw.Dispose();
            }
        }

        public static void SendLog()
        {
            string filename = $"log_{DateTime.Now.ToShortDateString()}-{DateTime.Now.ToLongTimeString()}.log";
            filename = filename.Replace(':', '.');
            if (!File.Exists(logfile))
                return;
            File.Copy(logfile, filename);
            string logsDirectory = Directory.GetCurrentDirectory();
            logsDirectory = Path.Combine(logsDirectory, "logs");
            if (!Directory.Exists(logsDirectory))
                Directory.CreateDirectory(logsDirectory);
            File.Move(filename, $"{Path.Combine(logsDirectory, filename)}");
                //Path.Combine(logsDirectory,
                //    $"log_{DateTime.Now.Date.ToShortDateString()}{DateTime.Now.Date.ToShortTimeString()}.log"));
        }
    }

}
