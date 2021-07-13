using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XplaneConnection
{
    internal static class ConnectionUdp
    {
        public static int masterPortOut = 49001;
        public static int masterUdpPortIn = 49000;
        public static int localUdpPort = 48003;
        public static UdpClient xPlaneUDP;
        private static int _ucpPacketLength = 256;
        public static bool forceStopUdp = false;

        public static string responseUDP;

        public static List<string> UdpHeaders = new List<string>() { "XWXR", "FAIL", "RECO" };
        public static List<byte[]> MessagesUdpAll = new List<byte[]>();
        public static List<byte[]> CommandMessages = new List<byte[]>();

        public static List<XMessage> FailMessages = new List<XMessage>();
        public static List<XMessage> AcfMessages = new List<XMessage>();
        public static List<XMessage> DimMessages = new List<XMessage>();
        public static string xLoc = "";
        public static string xRad = "";
        public static string xWgt = "";
        public static string xAcf = "";

        public static void UdpLoop()
        {
            try
            {
                #region Connection
                EncoderUdp encoderUdp = new EncoderUdp();
                Thread.CurrentThread.Name = "UDPLoop";
                IPEndPoint xPlaneRecievePoint = new IPEndPoint(IPAddress.Any, 0);
                UdpClient clientUdpRecieve = new UdpClient(masterPortOut);

                bool noErrorFlag = true;
                byte[] IntroduceMessageUDP = CommandEncoder.encodeCommandToBytes("CON1", ConnectionType.UDP);
                byte[] datatemp = new byte[1];
                int errorsCount = 0;

                clientUdpRecieve.Send(IntroduceMessageUDP, IntroduceMessageUDP.Length, ConnectionCore.MasterIp, masterUdpPortIn);
                DebugPart.WriteLog($"Sending UDP CON1 xMessage to MasterPC");
                //clientUdpRecieve.Close();

                #endregion

                #region Send params


                UdpClient xPlaneUdpCommander = new UdpClient(xPlaneRecievePoint.Port);
                xPlaneUdpCommander.Client.ReceiveTimeout = 150;
                string localpoint = xPlaneUDP.Client.LocalEndPoint.ToString();
                int port = Int32.Parse(localpoint.Split(':')[1]);
                IPEndPoint remEndPoint = new IPEndPoint(IPAddress.Parse(ConnectionCore.MasterIp), port);

                #endregion

                while (Thread.CurrentThread.IsAlive && !forceStopUdp)
                {
                    //если много ошибок, соедениться заного
                    if (errorsCount > 5)
                    {
                        errorsCount = 0;
                        datatemp = new byte[1];

                        clientUdpRecieve = new UdpClient(masterPortOut);
                        IntroduceMessageUDP = CommandEncoder.encodeCommandToBytes("CON1", ConnectionType.UDP);
                        clientUdpRecieve.Send(IntroduceMessageUDP, IntroduceMessageUDP.Length, ConnectionCore.MasterIp, masterUdpPortIn);
                        clientUdpRecieve.Close();

                        xPlaneRecievePoint = new IPEndPoint(IPAddress.Any, 0);

                        encoderUdp = new EncoderUdp();
                    }

                    //получаем сообщения
                    try
                    {
                        byte[] data = xPlaneUDP.Receive(ref xPlaneRecievePoint);
                        if (datatemp == data)
                            errorsCount++;
                        else
                            datatemp = data;

                        XMessage xMessage = encoderUdp.UdpDataToMessage(data);
                        switch (xMessage.HeaderString)
                        {
                            case "xFAL":
                                AddMessageToList(xMessage, ConnectionUdp.FailMessages);
                                break;
                            case "xACF":
                                AddMessageToList(xMessage, ConnectionUdp.AcfMessages);
                                break;
                            case "xDIM":
                                AddMessageToList(xMessage, ConnectionUdp.DimMessages);
                                break;
                            case "xLOC":
                                ConnectionUdp.xLoc = xMessage.MessageString;
                                break;
                            case "xRAD":
                                ConnectionUdp.xRad = xMessage.MessageString;
                                break;
                            case "xWGT":
                                ConnectionUdp.xWgt = xMessage.MessageString;
                                break;
                            case "ACFN":
                                ConnectionUdp.xAcf = xMessage.MessageString;
                                break;
                            case "xCON":
                                break;
                            default:
                                responseUDP = xMessage.FullString();
                                break;
                        }
                        //ConnectionCore.MessagesAllAdd(data, ConnectionType.UDP);
                    }
                    catch (ThreadAbortException e)
                    {
                        DebugPart.WriteLog(e);
                        noErrorFlag = false;
                    }
                    catch (Exception e)
                    {
                        DebugPart.WriteLog(e);
                        noErrorFlag = false;
                    }

                    //отправляем сообщения
                    if (CommandMessages.Count > 0)
                    {
                        try
                        {
                            int count = CommandMessages.Count;
                            for (int i = 0; i < count; i++)
                            {
                                clientUdpRecieve.Send(CommandMessages[0], CommandMessages[0].Length, ConnectionCore.MasterIp, masterUdpPortIn);
                                CommandMessages.Remove(CommandMessages[0]);
                            }
                            //CommandMessages = new List<byte[]>();
                        }
                        catch (Exception e)
                        {
                            DebugPart.ErrorsList.Add(e.Message);
                        }
                    }

                    Thread.Sleep(100);
                }

                xPlaneUDP.Close();
                Console.Write($"UDP connection finished");

            }
            catch (Exception e)
            {
                DebugPart.WriteLog(e);
            }

        }

        private static void AddMessageToList(XMessage xMessage, List<XMessage> messages)
        {
            if (!messages.Contains(xMessage))
                messages.Add(xMessage);
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
                    if (message[0] != ' ')
                        message = " " + message;
                    break;
            }

            message = header + message;
            ConnectionUdp.CommandMessages.Add(CommandEncoder.encodeCommandToBytes(message));
        }

        public static void SendMessage(byte[] data)
        {
            ConnectionUdp.CommandMessages.Add(data);
        }
    }

    class EncoderUdp
    {
        public XMessage UdpDataToMessage(byte[] bytesData)
        {
            if (bytesData.Length == 0)
                return new XMessage();
            byte[] data = EncoderMisc.CutZeroBytes(bytesData);
            if (data.Length == 0)
                return new XMessage(); ;
            //BigData(ref data);

            return TranslateUdpMessage(data);
        }

        private XMessage TranslateUdpMessage(byte[] bytes)
        {
            try
            {
                if (bytes.Length == 0)
                    return new XMessage();

                string header = EncoderMisc.GetHeaderString(bytes, ConnectionType.UDP);
                if (bytes.Length <= EncoderMisc.headerUdpLength)
                    return new XMessage();


                XMessage encoded = new XMessage();
                encoded.Type = ConnectionType.UDP;
                encoded.HeaderString = header;
                encoded.MakeHeader();
                encoded.MessageBytes = new byte[bytes.Length - EncoderMisc.headerUdpLength - 1];
                for (int i = 0; i < encoded.MessageBytes.Length; i++)
                    encoded.MessageBytes[i] = bytes[i + EncoderMisc.headerUdpLength + 1];


                byte[] byteValue = new byte[4];
                int bbi = 1;//counter for integers

                foreach (var bt in encoded.MessageBytes)
                {
                    if (EncoderMisc.isByteChar(bt))
                    {
                        bbi = 1;
                        encoded.MessageString += ((Char)bt).ToString();
                    }
                    else
                    {
                        byteValue[bbi - 1] = bt;
                        if (bbi == 4)
                        {
                            bbi = 1;
                            encoded.MessageString += BitConverter.ToInt32(byteValue, 0).ToString();
                        }
                        else
                            bbi++;
                    }
                }

                return encoded;

            }
            catch (Exception e)
            {
                DebugPart.WriteLog(e);
                return new XMessage();
            }
            return new XMessage();
        }

    }

    class CommandEncoder
    {
        public static byte[] encodeCommandToBytes(string message, ConnectionType connectionType)
        {
            byte[] bytes;
            byte[] answerBytes;
            switch (message)
            {
                case "CON1":
                    if (connectionType == ConnectionType.UDP)
                    {
                        return MakeConnectionBytes();
                    }
                    else
                    {
                        answerBytes = MakeConnectionBytes();
                        answerBytes[4] = 10;
                        return answerBytes;
                    }
                case "CMND":
                    return encodeCommandToBytes(message);

                default:
                    bytes = Encoding.Default.GetBytes(message);
                    answerBytes = new byte[bytes.Length + 1];
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        answerBytes[i] = bytes[i];
                    }
                    return answerBytes;

            }
            return answerBytes;
        }

        public static byte[] MakeWeatherBytes(Atmosphere stats)
        {
            //https://www.scadacore.com/tools/programming-calculators/online-hex-converter/
            //https://www.rapidtables.com/convert/number/ascii-hex-bin-dec-converter.html

            List<byte> output = new List<byte>();
            foreach (char chr in "XWXR")
                output.Add((byte)chr);
            output.Add(0);
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.Date), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.Timex), 4, true));

            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.Visibility), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.Rain), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.Storm), 4, true));
            int rwStat = 0;
            switch (stats.RunWayStat)
            {
                case Atmosphere.RwStat.dry:
                    rwStat = 0;
                    break;
                case Atmosphere.RwStat.damp:
                    rwStat = 1;
                    break;
                case Atmosphere.RwStat.wet:
                    rwStat = 2;
                    break;
            }
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(rwStat), 4, false));
            if (stats.RunWayPatches)
                rwStat = 1;
            else
                rwStat = 0;
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(rwStat), 4, false));

            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.temperature), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.baro), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.termCov), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.termStr), 4, true));


            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.CloudsLayer3.GetCatNum()), 4, false));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.CloudsLayer2.GetCatNum()), 4, false));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.CloudsLayer1.GetCatNum()), 4, false));

            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.CloudsLayer3.BaseHight), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.CloudsLayer2.BaseHight), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.CloudsLayer1.BaseHight), 4, true));

            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.CloudsLayer2.TopHight), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.CloudsLayer3.TopHight), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.CloudsLayer1.TopHight), 4, true));


            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer3.Altitude), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer1.Altitude), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer2.Altitude), 4, true));

            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer3.Direction), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer2.Direction), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer1.Direction), 4, true));

            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer3.Strength), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer2.Strength), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer1.Strength), 4, true));

            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer3.Gusts), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer2.Gusts), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer1.Gusts), 4, true));

            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer3.Turbulence), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer2.Turbulence), 4, true));
            output.AddRange(MakeBytesArray(BitConverter.GetBytes(stats.WindLayer1.Turbulence), 4, true));
            return output.ToArray();
        }
        
        private static byte[] MakeBytesArray(byte[] data, int length, bool flip)
        {
            byte[] array = new byte[length];
            for (int i = 0; i < length; i++)
                array[i] = data[i];
            if (flip)
                array.Reverse();
            return array;

        }

        private static byte[] MakeConnectionBytes()
        {
            byte[] bytes;
            byte[] answerBytes;
            bytes = Encoding.ASCII.GetBytes("CON1");
            answerBytes = new byte[bytes.Length + 1];
            for (int i = 0; i < bytes.Length; i++)
                answerBytes[i] = bytes[i];
            return answerBytes;

        }

        public static byte[] encodeCommandToBytes(string message)
        {
            List<byte> answerBytes = new List<byte>();
            for (int i = 0; i < message.Length; i++)
            {
                char chr = message[i];
                if (i == 4)
                {
                    if (ConnectionTcp.CmndHeaders.Contains(EncoderMisc.GetHeaderString(message, ConnectionType.Command)))
                        answerBytes.Add(0);
                }
                else
                    answerBytes.Add(Encoding.Default.GetBytes(message)[i]);
            }
            return answerBytes.ToArray();
        }
    }


}
