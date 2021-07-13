using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MainLibrary;
using XplaneConnection_NF;

namespace XplaneControl
{
    public class EncoderUdp
    {
        /// <summary>
        /// Обрабатывает сообщение, и распределяет содержимое
        /// </summary>
        /// <param name="bytesData"></param>
        /// <returns></returns>
        public Task UdpDataToMessage(byte[] bytesData)
        {
            if (bytesData.Length == 0)
                return Task.CompletedTask;
            byte[] data = EncoderMisc.CutZeroBytes(bytesData);
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
        private UdpMessage TranslateUdpMessage(byte[] bytes)
        {
            try
            {
                if (bytes.Length == 0)
                    return new UdpMessage(new byte[0], ConnectionType.UDP);
                
                if (bytes.Length <= EncoderMisc.headerUdpLength)
                    return new UdpMessage(new byte[0], ConnectionType.UDP);

                UdpMessage udpMessage = new UdpMessage(bytes, ConnectionType.UDP);

                byte[] byteValue = new byte[4];
                int bbi = 1;//counter for integers

                switch (udpMessage.Header)
                {
                    case UdpHeader.Aircraft:
                        foreach (var bt in udpMessage.MessageBytes)
                            if (EncoderMisc.isByteChar(bt, true))
                                udpMessage.MessageString += ((Char)bt).ToString();
                            else
                                udpMessage.MessageString += $"~{bt.ToString()}~";
                        break;
                    case UdpHeader.CurrentAircraft:
                        foreach (var bt in udpMessage.MessageBytes)
                            if (EncoderMisc.isByteChar(bt, true))
                                udpMessage.MessageString += ((Char)bt).ToString();
                        break;
                    case UdpHeader.Fail:
                        foreach (var bt in udpMessage.MessageBytes)
                            if (udpMessage.MessageString != "" && EncoderMisc.isByteChar(bt, true))
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
                            if (udpMessage.MessageString != "" && EncoderMisc.isByteChar(bt))
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
                            if (EncoderMisc.isByteChar(bt))
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
        private async void SortMessages(UdpMessage udpMessage)
        {
            switch (udpMessage.Header)
            {
                case UdpHeader.Aircraft:
                    ConnectionUdp.AddMessageToList(udpMessage, ConnectionUdp.AcfMessages);
                    break;
                case UdpHeader.CurrentAircraft:
                    ConnectionUdp.xAcf = udpMessage.MessageString;
                    break;
                case UdpHeader.Fail:
                    ConnectionUdp.AddMessageToList(udpMessage, ConnectionUdp.FailMessages);
                    await Fails.Add(udpMessage);
                    //await FailAdd();
                    break;
                case UdpHeader.Dim:
                    ConnectionUdp.xDim = udpMessage.MessageString;
                    break;
                case UdpHeader.Weight:
                    ConnectionUdp.xWgt = udpMessage.MessageString;
                    break;
                case UdpHeader.Radar:
                    ConnectionUdp.AddMessageToList(udpMessage, ConnectionUdp.RadMessages);
                    break;
                case UdpHeader.Location:
                    ConnectionUdp.xLoc = udpMessage.MessageString;
                    break;
                case UdpHeader.Con:
                    break;
                default:
                    break;
                    throw new ArgumentOutOfRangeException();
            }
        }

        private byte[] ReveseBytes(byte[] inputBytes)
        {
            byte[] outputBytes = new byte[inputBytes.Length];
            for (int i = 0; i < inputBytes.Length; i++)
            {
                outputBytes[i] = inputBytes[inputBytes.Length - i];
            }

            return outputBytes;
        }
        
    }
}