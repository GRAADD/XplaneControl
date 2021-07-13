using System;
using System.Collections.Generic;
using System.Globalization;
using MainLibrary;
using XplaneConnection_NF;

namespace XplaneControl
{
    public class EncoderMisc
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

        public static bool isByteChar(byte bb, bool space = false)
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
            if (bb == 32 || bb == 40 || bb == 41 || bb == 45 || bb == 95)
            {
                return space;//simbols . and / and digits 0-9
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
}