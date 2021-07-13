using System;
using System.Collections.Generic;
using System.IO;
//using Java.IO;
using File = System.IO.File;

namespace XplaneControl
{
    public class XAirport
    {
        public byte[] CodeByte = new byte[4];
        public string CodeString = "";
        public string NameString = "";
        public byte[] DataBytes;
        public List<Rw> Rws = new List<Rw>();

        public XAirport(byte[] bytes)
        {
            int step = 0;
            try
            {
                int start = 0;
                List<byte> bytesTemp = new List<byte>();

                int cutStart = 5;
                for (int i = cutStart; i < bytes.Length; i++)
                {
                    if (bytes[i] != 0)
                    {
                        cutStart = i;
                        break;
                    }
                }

                byte[] AptBytes = Cut(bytes, cutStart);


                for (int j = 0; j < 4; j++) //заполняем код аэропорта
                {
                    CodeByte[j] = AptBytes[j];
                    if (DataEncoder.IsByteChar(AptBytes[j]))
                        CodeString += ((Char)AptBytes[j]).ToString();
                    else
                    {
                        CodeString += "";
                    }
                }

                start = 8;
                while (AptBytes[start] != 0)
                {
                    NameString += ((Char)AptBytes[start]).ToString();
                    start++;

                }

                Rw rw = new Rw();
                for (int i = start; i < AptBytes.Length; i++)
                {
                    if (DataEncoder.IsNumeric(AptBytes[i]) && DataEncoder.IsNumeric(AptBytes[i + 1]) && i < AptBytes.Length - 8)
                    {
                        if (DataBytes == null)
                            DataBytes = Cut(AptBytes, start, i - 1);
                        else
                        {
                            rw.DataBytes = new List<byte>();
                            rw.DataBytes.AddRange(Cut(AptBytes, start, i - 1));
                        }
                        if (!string.IsNullOrEmpty(rw.Rw1String))
                            Rws.Add(rw);

                        rw.Rw1String = "";
                        rw.Rw1Byte = new List<byte>();
                        for (int btIndex = 0; btIndex < 3; btIndex++)
                        {
                            rw.Rw1Byte.Add(AptBytes[i]);
                            if (DataEncoder.IsByteChar(AptBytes[i]))
                                rw.Rw1String += ((Char)AptBytes[i]).ToString();
                            i++;
                        }
                        i++;
                        rw.Rw2String = "";
                        rw.Rw2Byte = new List<byte>();
                        for (int btIndex = 0; btIndex < 3; btIndex++)
                        {
                            rw.Rw2Byte.Add(AptBytes[i]);
                            if (DataEncoder.IsByteChar(AptBytes[i]))
                                rw.Rw2String += ((Char)AptBytes[i]).ToString();
                            i++;
                        }

                        start = i;
                    }
                }

                if (DataBytes == null)
                    DataBytes = Cut(AptBytes, start, AptBytes.Length - 1);
                else
                {
                    rw.DataBytes = new List<byte>();
                    rw.DataBytes.AddRange(Cut(AptBytes, start, AptBytes.Length - 1));
                }
                if (!string.IsNullOrEmpty(rw.Rw1String))
                    Rws.Add(rw);

                //Rws.Add(Add(bytesTemp.ToArray()));
            }
            catch (Exception e)
            {
                //Logger.WriteLog(e, ErrorLevel.Error);
            }
        }

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
            if (DataEncoder.IsCapitalLetter(bb))
            {
                if (count >= data.Length)
                    return new Rw();
                count++;
                bb = data[count];
                while (bb != 0 && DataEncoder.IsNumeric(bb))
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

        public void SaveData(string path_to_save = "")
        {
            if (string.IsNullOrEmpty(CodeString) || DataBytes==null)
                return;
            File.WriteAllBytes(Path.Combine(path_to_save, $"apt_{CodeString}.bin"),DataBytes);
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