using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MainLibrary;
using XplaneConnection_NF;

namespace XplaneControl
{
    public class EncoderTcp
    {

        public List<byte> EncodeBytes(byte[] bytes)
        {
            List<byte> output = new List<byte>();
            List<byte[]> messageList = new List<byte[]>();
            try
            {
                int start = 0;
                for (int i = 1; i < bytes.Length; i++)//1, что бы пропустить первый вход
                    if (bytes[i] == (byte)'x' && i < bytes.Length - EncoderMisc.headerTcpLength)
                    {
                        byte[] hdrBytes = Cut(bytes, i, i + EncoderMisc.headerTcpLength);
                        if (EncoderMisc.IsItCorrectHeader(hdrBytes, ConnectionType.TCP))
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

        public void EncoderConveyer()
        {
            while (!ConnectionTcp.TcpFinished)
            {
                try
                {
                    if (ConnectionTcp.BytesList.Count == 0)
                    {
                        Thread.Sleep(1000);
                        continue;
                    }
                    //расшифровываем первое вхождение
                    if (ConnectionTcp.BytesList[0] == null)
                    {
                        ConnectionTcp.BytesList.RemoveAt(0);
                        continue;
                    }
                    byte[] bytes = EncodeBytes(ConnectionTcp.BytesList[0]).ToArray();
                    if (bytes.Length != ConnectionTcp.BytesList[0].Length)
                    {
                        //убираем его из конвеера, а остаток подставляем к следующему
                        ConnectionTcp.BytesList.RemoveAt(0);
                    }
                    List<byte> newList = new List<byte>();
                    byte[] oldBytes = ConnectionTcp.BytesList[0];
                    newList.AddRange(bytes);
                    newList.AddRange(oldBytes);
                    ConnectionTcp.BytesList[0] = newList.ToArray();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }


            }
        }

        private async void StartWorker(byte[] bytes)
        {
            await MessageWorker(bytes);
        }

        private Task MessageWorker(byte[] message)
        {
            if (message == null || message.Length < EncoderMisc.headerTcpLength)
                return Task.CompletedTask;
            string header = EncoderMisc.GetHeaderString(message, ConnectionType.TCP);
            if (header.Contains("xAPT"))
                header = "xAPTP";

            if (message.Length <= header.Length)
            {
                AddData(ConnectionTcp.unknowMessages, message , header.Length);
                return Task.CompletedTask;
            }
            switch (header)
            {
                case "xCMD8":
                    AddData(ConnectionTcp.xCMD, message, header.Length);
                    return Task.CompletedTask;
                case "xFIX":
                    if (!ConnectionTcp.CmndDone)
                        ConnectionTcp.CmndDone = true;
                    AddData(ConnectionTcp.xFIX, message, header.Length);
                    return Task.CompletedTask;
                case "xNAVH":
                    AddData(ConnectionTcp.xNAVH, message, header.Length);
                    if (!ConnectionTcp.FixDone)
                        ConnectionTcp.FixDone = true;
                    return Task.CompletedTask;
                case "xAPTP":
                    //ConnectionTcp.xAPTP.Add(message);
                    ConnectionTcp.Airports.Add(new XAirport(message));
                    if (!ConnectionTcp.NavDone)
                        ConnectionTcp.NavDone = true;
                    return Task.CompletedTask;
                case "xRAD":
                    AddData(ConnectionTcp.xRAD, message, header.Length);
                    return Task.CompletedTask;
                default:
                    Logger.WriteLog($"Unknow header ({header} {TranslateTcp(message)})", ErrorLevel.Info);
                    return Task.CompletedTask;
            }

        }
        
        private string TranslateTcp(byte[] bytes)
        {
            try
            {
                byte[] data = EncoderMisc.CutZeroBytes(bytes);
                //ConnectionTCP.MessagesTcpAll.Add(data);
                if (data.Length == 0)
                    return "";
                string header = EncoderMisc.GetHeaderString(data, ConnectionType.TCP);
                if (!EncoderMisc.IsItCorrectHeader(header, ConnectionType.TCP))
                    return $"---Corrupted({header})!---";


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
                                    Logger.WriteLog($"Corrupted TCP XCMD8 message ({output})");
                                    output = "--corrupted!--";
                                    return output;
                            }
                        else
                            output += $".{data[i]}";
                    }
                }
                if (!EncoderMisc.IsItCorrectHeader(header, ConnectionType.TCP))
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
        private Task<string> TranslateTcpTask(byte[] bytes)
        {
            try
            {
                byte[] data = EncoderMisc.CutZeroBytes(bytes);
                //ConnectionTCP.MessagesTcpAll.Add(data);
                if (data.Length == 0)
                    return Task.FromResult("");
                string header = EncoderMisc.GetHeaderString(data, ConnectionType.TCP);
                if (!EncoderMisc.IsItCorrectHeader(header, ConnectionType.TCP))
                    return Task.FromResult($"---Corrupted({header})!---");


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
                                    Logger.WriteLog($"Corrupted TCP XCMD8 message ({output})");
                                    output = "--corrupted!--";
                                    return Task.FromResult(output);
                                    break;
                            }
                        else
                            output += $".{data[i]}";
                    }
                }
                if (!EncoderMisc.IsItCorrectHeader(header, ConnectionType.TCP))
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

        private void AddData(List<string> list, byte[] message, int HeaderLength)
        {
            //string data = await TranslateTcp(message);
            string data = TranslateTcp(message);
            data = data.Substring(HeaderLength);
            list.Add(data);
        }

        private void AddData(List<string> list, string data)
        {
            //if (!list.Contains(data))
            //    list.Add(data);
            list.Add(data);
        }

        private void AddData(List<byte[]> list, byte[] data)
        {
            list.Add(data);
        }

        //public string debug = "";
        private void AirportWork(byte[] bytes)
        {
            ConnectionTcp.Airports.Add(new XAirport(bytes));
            
        }
        
        private byte[] Cut(byte[] message, int start, int end = 0)
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
    }
}