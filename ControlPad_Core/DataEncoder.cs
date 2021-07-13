using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MainLibrary;
using XplaneConnection_NF;

namespace XplaneControl
{
    public static class DataEncoder
    {
        public static Connection connection;
        public static int headerUdpLength = 4;
        public static int headerTcpLength = 5;
        public static int headerCommandLength = 4;

        public static int GetLength(ConnectionType type)
        {
            switch (type)
            {
                case ConnectionType.UDP:
                    return headerUdpLength;
                case ConnectionType.TCP:
                    return headerTcpLength;
                case ConnectionType.Command:
                    return headerTcpLength;
                default:
                    return 0;
            }
        }
        /// <summary>
        /// Убрать последние нулевые байты
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Возвращает заголовок сообщения
        /// </summary>
        /// <param name="data">Само сообщение</param>
        /// <param name="connectionType">Тип подключения</param>
        /// <returns>Заголовок сообщения</returns>
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
                if (output.Contains("xAPT"))
                    return "xAPTP";
            }
            catch (Exception e)
            {
                Logger.WriteLog(output, ErrorLevel.Error);
                Logger.WriteLog(e, ErrorLevel.Error);
                return "";
            }
            return output;
        }
        /// <summary>
        /// Возвращает длину заголовка
        /// </summary>
        /// <param name="connectionType">Тип подключения</param>
        /// <returns>Длина заголовка</returns>
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

        public static bool IsByteChar(byte bb, bool space = false)
        {
            if (IsCapitalLetter(bb))
                return true;
            if (IsLowercaseLetter(bb))
                return true;
            if (IsNumeric(bb))
                return true;
            if (bb >= 46 && bb <= 48)
            {
                return true;//simbols . and / and digits 0-9
            }
            if (bb == 32 || bb == 40 || bb == 41 || bb == 45 || bb == 95)
            {
                return space;//simbols . and / and digits 0-9
            }
            return false;
        }

        public static bool IsCapitalLetter(byte bb)
        {
            if (bb >= 65 && bb <= 91)
            {
                return true; //letters A-Z
            }
            else return false;
        }
        public static bool IsLowercaseLetter(byte bb)
        {
            if (bb >= 97 && bb <= 122)
            {
                return true;//letters a-z
            }
            else return false;
        }
        public static bool IsNumeric(byte bb)
        {
            if (bb >= 48 && bb <= 57)
            {
                return true;//digits 0-9
            }
            return false;
        }

        public static bool IsItCorrectHeader(string text, ConnectionType connectionType)//xCMD amd other...
        {
            int hdrLength = GetHeaderLength(connectionType);
            string header = "";
            header = text.Length < hdrLength ? text : text.Substring(0, hdrLength);

            if (connectionType == ConnectionType.TCP)
            {
                return connection.TcpHeaders.Contains(header);
            }
            else
                return connection.UdpHeaders.Contains(header);
        }
        public static bool IsItCorrectHeader(byte[] data, ConnectionType connectionType)//xCMD amd other...
        {
            string header = GetHeaderString(data, ConnectionType.TCP);

            if (connectionType == ConnectionType.TCP)
            {
                if (header == "xFIX")
                    return true;
                return connection.TcpHeaders.Contains(header);
            }
            else
                return connection.UdpHeaders.Contains(header);
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

        //==================================================================================================================================

        
        public static List<byte> EncodeBytes(byte[] bytes)
        {
            List<byte> output = new List<byte>();
            List<byte[]> messageList = new List<byte[]>();
            try
            {
                int start = 0;
                for (int i = 1; i < bytes.Length; i++)//1, что бы пропустить первый вход
                    if (bytes[i] == (byte)'x' && i < bytes.Length - headerTcpLength)
                    {
                        byte[] hdrBytes = Cut(bytes, i, i + headerTcpLength);
                        if (IsItCorrectHeader(hdrBytes, ConnectionType.TCP))
                        {
                            byte[] messageBytes = Cut(bytes, start, i);
                            messageList.Add(messageBytes);
                            start = i;
                        }
                    }
                messageList.Add(Cut(bytes, start));//заполняем остаток

                if (messageList.Count == 1)
                {
                    StartWorker(messageList[0]);
                }
                else
                {
                    for (int i = 0; i < messageList.Count - 1; i++)
                        StartWorker(messageList[i]);
                    output = new List<byte>();
                    output.AddRange(messageList[messageList.Count - 1]);
                    return output;
                }
            }
            catch (Exception e)
            {
                Logger.WriteLog(e);
                output.AddRange(bytes);
                return output;
            }

            return output;
        }

        public static void EncoderConveyer()
        {
            while (!connection.TcpFinished)
            {
                try
                {
                    if (connection.BytesList.Count == 0)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    //расшифровываем первое вхождение
                    if (connection.BytesList[0] == null)
                    {
                        connection.BytesList.RemoveAt(0);
                        continue;
                    }
                    byte[] bytes = EncodeBytes(connection.BytesList[0]).ToArray();
                    if (bytes.Length != connection.BytesList[0].Length)
                    {
                        //убираем его из конвеера, а остаток подставляем к следующему
                        connection.BytesList.RemoveAt(0);
                    }
                    List<byte> newList = new List<byte>();
                    byte[] oldBytes = connection.BytesList[0];
                    newList.AddRange(bytes);
                    newList.AddRange(oldBytes);
                    connection.BytesList[0] = newList.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static async void StartWorker(byte[] bytes)
        {
            await MessageWorker(bytes);
        }

        private static Task MessageWorker(byte[] message)
        {
            if (message == null || message.Length < headerTcpLength)
                return Task.CompletedTask;
            string header = GetHeaderString(message, ConnectionType.TCP);
            if (header.Contains("xAPT"))
                header = "xAPTP";

            if (message.Length <= header.Length)
            {
                AddData(connection.unknowMessages, message , header.Length);
                return Task.CompletedTask;
            }
            switch (header)
            {
                case "xCMD8":
                    AddData(connection.xCMD, message, header.Length);
                    connection.CmdStarted = true;
                    return Task.CompletedTask;
                case "xFIX":
                    AddData(connection.xFIX, message, header.Length);
                    connection.FixStarted = true;
                    return Task.CompletedTask;
                case "xNAVH":
                    AddData(connection.xNAVH, message, header.Length);
                    connection.NavStarted = true;
                    return Task.CompletedTask;
                case "xAPTP":
                    //Connection.xAPTP.Add(message);
                    connection.Airports.Add(new XAirport(message));
                    connection.AptStarted = true;
                    return Task.CompletedTask;
                case "xRAD":
                    AddData(connection.xRAD, message, header.Length);
                    connection.RadStarted = true;
                    return Task.CompletedTask;
                default:
                    Logger.WriteLog($"Unknow header ({header} {TranslateTcp(message)})", ErrorLevel.Info);
                    return Task.CompletedTask;
            }

        }
        
        private static string TranslateTcp(byte[] bytes)
        {
            try
            {
                byte[] data = CutZeroBytes(bytes);
                //Connection.MessagesTcpAll.Add(data);
                if (data.Length == 0)
                    return "";
                string header = GetHeaderString(data, ConnectionType.TCP);
                if (!IsItCorrectHeader(header, ConnectionType.TCP))
                    return $"---Corrupted({header})!---";


                int bbi = 1;
                string output = "";
                for (int i = header.Length; i < data.Length; i++)
                {
                    if (i == header.Length && data.Length - headerTcpLength + 2 > i 
                                           && data[i] + data[i + 1] + data[i + 2] == 0)
                    {
                        output += " ";
                        i += 2;
                        continue;
                    }
                    if (IsByteChar(data[i]))
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
                                    Logger.WriteLog($"Corrupted TCP XCMD8 message ({output})");
                                    output = "--corrupted!--";
                                    return output;
                            }
                        else
                            output += $".{data[i]}";
                    }
                }
                if (!IsItCorrectHeader(header, ConnectionType.TCP))
                {
                    //if (header.Contains("FIX"))
                    //Logger.WriteLog($"Corrupted TCP FIX message ({output})");
                }
                return $"{header}{output}";
            }
            catch (Exception e)
            {
                // Logger.WriteLog(e, ErrorLevel.Error);
                return "";
            }
        }

        private static Task<string> TranslateTcpTask(byte[] bytes)
        {
            try
            {
                byte[] data = CutZeroBytes(bytes);
                //Connection.MessagesTcpAll.Add(data);
                if (data.Length == 0)
                    return Task.FromResult("");
                string header = GetHeaderString(data, ConnectionType.TCP);
                if (!IsItCorrectHeader(header, ConnectionType.TCP))
                    return Task.FromResult($"---Corrupted({header})!---");


                int bbi = 1;
                string output = "";
                for (int i = header.Length; i < data.Length; i++)
                {
                    if (i == header.Length && data.Length - headerTcpLength + 2 > i 
                                           && data[i] + data[i + 1] + data[i + 2] == 0)
                    {
                        output += " ";
                        i += 2;
                        continue;
                    }
                    if (IsByteChar(data[i]))
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
                                    Logger.WriteLog($"Corrupted TCP XCMD8 message ({output})");
                                    output = "--corrupted!--";
                                    return Task.FromResult(output);
                                    break;
                            }
                        else
                            output += $".{data[i]}";
                    }
                }
                if (!IsItCorrectHeader(header, ConnectionType.TCP))
                {

                    //if (header.Contains("FIX"))
                    //Logger.WriteLog($"Corrupted TCP FIX message ({output})");
                }
                return Task.FromResult($"{header}{output}");
            }
            catch (Exception e)
            {
                // Logger.WriteLog(e, ErrorLevel.Error);
                return Task.FromResult("");
            }
        }

        private static void AddData(List<string> list, byte[] message, int HeaderLength)
        {
            //string data = await TranslateTcp(message);
            string data = TranslateTcp(message);
            data = data.Substring(HeaderLength);
            list.Add(data);
        }

        private static void AddData(List<string> list, string data)
        {
            //if (!list.Contains(data))
            //    list.Add(data);
            list.Add(data);
        }

        private static void AddData(List<byte[]> list, byte[] data)
        {
            list.Add(data);
        }

        //public string debug = "";
        private static void AirportWork(byte[] bytes)
        {
            connection.Airports.Add(new XAirport(bytes));
        }
        
        private static byte[] Cut(byte[] message, int start, int end = 0)
        {
            if (end == 0)
                end = message.Length;
            byte[] outBytes = message;
            try
            {
                outBytes = new byte[end - start];
                for (int i = 0; i < outBytes.Length; i++)
                    outBytes[i] = message[i + start];
            }
            catch (Exception e)
            {
                //Logger.WriteLog(e);
            }

            return outBytes;
        }

        //==================================================================================================================================

       /// <summary>
        /// Обрабатывает сообщение, и распределяет содержимое
        /// </summary>
        /// <param name="bytesData"></param>
        /// <returns></returns>
        public static Task UdpDataToMessage(byte[] bytesData)
        {
            if (bytesData.Length == 0)
                return Task.CompletedTask;
            byte[] data = CutZeroBytes(bytesData);
            if (data.Length == 0)
                return Task.CompletedTask;
            //BigData(ref data);
            UdpMessage message = TranslateUdpMessage(data);
            SortMessages(message);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Расшифровка сообщения
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        private static UdpMessage TranslateUdpMessage(byte[] bytes)
        {
            try
            {
                if (bytes.Length == 0)
                    return new UdpMessage(new byte[0], ConnectionType.UDP);
                
                if (bytes.Length <= headerUdpLength)
                    return new UdpMessage(new byte[0], ConnectionType.UDP);

                UdpMessage udpMessage = new UdpMessage(bytes, ConnectionType.UDP);

                byte[] byteValue = new byte[4];
                int bbi = 1;//counter for integers

                switch (udpMessage.Header)
                {
                    case UdpHeader.Aircraft:
                        foreach (var bt in udpMessage.MessageBytes)
                            if (IsByteChar(bt, true))
                                udpMessage.MessageString += ((Char)bt).ToString();
                            else
                                udpMessage.MessageString += $"~{bt.ToString()}~";
                        break;
                    case UdpHeader.CurrentAircraft:
                        foreach (var bt in udpMessage.MessageBytes)
                            if (IsByteChar(bt, true))
                                udpMessage.MessageString += ((Char)bt).ToString();
                        break;
                    case UdpHeader.Fail:
                        foreach (var bt in udpMessage.MessageBytes)
                            if (udpMessage.MessageString != "" && IsByteChar(bt, true))
                                udpMessage.MessageString += ((Char) bt).ToString();
                            else
                            {
                                byteValue[bbi - 1] = bt;
                                if (bbi == 4)
                                {
                                    bbi = 1;
                                    udpMessage.MessageString += BitConverter.ToInt32(byteValue, 0).ToString();
                                    udpMessage.MessageString += " ";
                                }
                                else
                                    bbi++;
                            }
                        break;
                    //case UdpHeader.Dim:
                    //    break;
                    case UdpHeader.Weight:
                        foreach (var bt in udpMessage.MessageBytes)
                        {
                            byteValue[bbi - 1] = bt;
                            if (bbi == 4)
                            {
                                bbi = 1;
                                udpMessage.MessageString += BitConverter.ToInt32(byteValue, 0).ToString();
                                udpMessage.MessageString += " ";
                            }
                            else
                                bbi++;
                        }
                        break;
                    case UdpHeader.Radar:
                        byteValue = new byte[8];
                        foreach (var bt in udpMessage.MessageBytes)
                            if (udpMessage.MessageString != "" && IsByteChar(bt))
                                udpMessage.MessageString += ((Char)bt).ToString();
                            else
                            {
                                byteValue[bbi - 1] = bt;
                                if (bbi == 8)
                                {
                                    bbi = 1;
                                    byteValue.Reverse();
                                    udpMessage.MessageString += BitConverter.ToInt64(byteValue, 0).ToString();
                                    udpMessage.MessageString += " ";
                                }
                                else
                                    bbi++;
                            }
                        break;
                    case UdpHeader.Location:
                        Position.GetData(udpMessage.MessageBytes);
                        break;
                    default:
                        foreach (var bt in udpMessage.MessageBytes)
                        {
                            if (IsByteChar(bt))
                            {
                                bbi = 1;
                                udpMessage.MessageString += ((Char)bt).ToString();
                            }
                            else
                            {
                                byteValue[bbi - 1] = bt;
                                if (bbi == 4)
                                {
                                    bbi = 1;
                                    byteValue.Reverse();
                                    udpMessage.MessageString += BitConverter.ToInt32(byteValue, 0).ToString();
                                    udpMessage.MessageString += " ";
                                }
                                else
                                    bbi++;
                            }
                        }
                        break;
                }
                return udpMessage;
            }
            catch (Exception e)
            {
                Logger.WriteLog(e, ErrorLevel.Error);
                return new UdpMessage(new byte[0], ConnectionType.UDP);
            }
        }


        /// <summary>
        /// Сортировка сообщения по местам хранения
        /// </summary>
        /// <param name="udpMessage"></param>
        private static async void SortMessages(UdpMessage udpMessage)
        {
            switch (udpMessage.Header)
            {
                case UdpHeader.Aircraft:
                    connection.AddMessageToList(udpMessage, connection.AcfMessages);
                    break;
                case UdpHeader.CurrentAircraft:
                    connection.xAcf = udpMessage.MessageString;
                    break;
                case UdpHeader.Fail:
                    connection.AddMessageToList(udpMessage, connection.FailMessages);
                    await Fails.Add(udpMessage);
                    //await FailAdd();
                    break;
                case UdpHeader.Dim:
                    connection.xDim = udpMessage.MessageString;
                    break;
                case UdpHeader.Weight:
                    connection.xWgt = udpMessage.MessageString;
                    break;
                case UdpHeader.Radar:
                    connection.AddMessageToList(udpMessage, connection.RadMessages);
                    break;
                case UdpHeader.Location:
                    connection.xLoc = udpMessage.MessageBytes;
                    break;
                case UdpHeader.Con:
                    break;
                default:
                    break;
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static byte[] ReverseBytes(byte[] inputBytes)
        {
            byte[] outputBytes = new byte[inputBytes.Length];
            for (int i = 0; i < inputBytes.Length; i++)
            {
                outputBytes[i] = inputBytes[inputBytes.Length - i];
            }

            return outputBytes;
        }

        //=================================================================================================================================================

        #region Airport

        
        //public static List<XAirport> Airports = new List<XAirport>();
        public static XAirport GetAirport(string name)
        {
            foreach (var apt in connection.Airports)
            {
                if (apt.CodeString == name || apt.NameString == name)
                    return apt;
            }
            return new XAirport(new byte[0]);
        }

        public static List<XAirport> GetAirports(string name)
        {
            List<XAirport> list = new List<XAirport>();
            foreach (var apt in connection.Airports)
            {
                if (apt.CodeString.Contains(name) || apt.NameString.Contains(name))
                    list.Add(apt);
            }
            return list;
        }

        #endregion
    }
}