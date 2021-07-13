using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XplaneConnection_NF;

namespace XplaneControl
{
    public class CommandEncoder
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

        public static byte[] encodeCommandToBytes(string text)
        {
            string header = EncoderMisc.GetHeaderString(text, ConnectionType.Command);
            var messageLength = 0;
            string message = "";
            int pos = 0;
            switch (header)
            {
                case "FAIL":
                    messageLength = 18;
                    break;
                case "ACFN":
                    messageLength = 165;
                    pos = 8;
                    break;
                case "CMND":
                    text = text.Replace("  "," ");
                    messageLength = text.Length;
                    break;
                default:
                    messageLength = text.Length;
                    break;
            }
            message = text.Substring(header.Length + 1);
            byte[] answerBytes = new byte[messageLength];
            for (int i = 0; i < answerBytes.Length; i++)
                answerBytes[i] = 0;
            byte[] hdrBytes = EncoderMisc.GetHeaderBytes(header, ConnectionType.Command);
            for (int i = 0; i < hdrBytes.Length; i++)
            {
                answerBytes[i] = hdrBytes[i];
            }
            if (pos==0)
                pos = hdrBytes.Length;
            //if (header == "ACFN")
            //{
            //    for (int j = 0; j < 5; j++)
            //    {
            //        answerBytes[pos] = 0;
            //    }

            //}

            byte[] bytes = Encoding.ASCII.GetBytes(message);
            for (int i = 0; i < bytes.Length; i++)
                answerBytes[i + pos + 1] = bytes[i];
            //for (int i = pos + 1; i < messageLength; i++)
            //    if (i - pos - 1 < message.Length)
            //        answerBytes[i] = (byte)message[i - pos - 1];
            //    else
            //        answerBytes[i] = 0;
            if (header == "ACFN")
            {
                byte[] acf = Encoding.ASCII.GetBytes(".acf");
                for (int i = 0; i < acf.Length; i++)
                {
                    answerBytes[i + pos + 2 + bytes.Length] = acf[i];
                }
            }
            return answerBytes;
        }

        public byte[] MakeAirportBytes(string code, string place)
        {
            byte[] output = new byte[69];
            XAirport airport = ConnectionTcp.GetAirport(code);

            for (int i = 0; i < output.Length; i++)
                output[i] = 0;

            string header = "PREL";
            for (int i = 0; i < 4; i++)
            {
                output[i] = (byte)header[i];
            }

            string rwPlace = "";
            if (place.Contains('_'))
            {
                rwPlace = place.Split('_')[1];
            }
            else
            {
                rwPlace = place;
            }
            byte bytePLace = 0;
            switch (rwPlace)
            {
                case "10nm":
                    bytePLace = 13;
                    break;
                case "3nm":
                    bytePLace = 12;
                    break;
                case "start":
                    bytePLace = 11;
                    break;
                case "Stand":
                    bytePLace = 11;
                    break;
                default:
                    break;
            }

            output[5] = bytePLace;

            for (int i = 13; i < 17; i++)
                output[i] = airport.CodeByte[i - 13];

            string rwNum = place.Split('_')[0];
            if (rwPlace == "Stand")
            {
                output[5] = 10;
                output[21] = 1;
                output[22] = 0;
                output[23] = 0;
                output[24] = 0;
                output[25] = 0;
                output[26] = 0;
                output[27] = 0;
                output[28] = 0;
                output[29] = 0;
                output[30] = 0;
                output[31] = 0;
                output[32] = 0;
                output[33] = 0;
            }            
            else
            {
                byte[] rwBytes = GetRW(airport, rwNum);
                for (int i = 21; i < 29; i++)
                    output[i] = rwBytes[i - 21];
            }

            return output;
        }

        private static byte[] GetRW(XAirport airport, string rwString)
        {
            byte[] output = new byte[8];
            for (int i = 0; i < output.Length; i++)
                output[i] = 0;
            byte RwNum1 = 0;
            for (int i = 0; i < airport.Rws.Count; i++)
            {
                XAirport.Rw rw = airport.Rws[i];
                if (rw.Rw1String == rwString)
                {
                    output[0] = RwNum1;
                    output[4] = 0;
                    return output;
                }
                if (rw.Rw2String == rwString)
                {
                    output[0] = RwNum1;
                    output[4] = 1;
                    return output;
                }
                RwNum1++;
            }

            return output;
        }

        private static byte[] GetStand(string stand)
        {
            byte[] output = new byte[8];
            string[] splitted = stand.Split(' ');
            for (int i = 0; i < output.Length; i++)
                output[i] = 0;
            for (int i = 0; i < 8; i++)
            {
                if (i > splitted.Length)
                    break;
                output[i] = byte.Parse(splitted[i]);
            }
            return output;
        }
    }
}