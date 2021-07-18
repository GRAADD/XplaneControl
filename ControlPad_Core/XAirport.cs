using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
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

        public XAirport(byte[] InputBytes)
        {
            //int step = 0;
            try
            {
                int startIndex = 0; //с какого байта читаем данные
                //List<byte> bytesTemp = new List<byte>();

                int cutStart = 5;
                for (int i = cutStart; i < InputBytes.Length; i++)
                {
                    if (InputBytes[i] != 0)
                    {
                        cutStart = i;
                        break;
                    }
                }

                byte[] AptBytes = Cut(InputBytes, cutStart);


                for (int j = 0; j < 4; j++) //заполняем код аэропорта
                {
                    CodeByte[j] = AptBytes[j];
                    if (ByteOperations.IsByteChar(AptBytes[j]))
                        CodeString += ((Char)AptBytes[j]).ToString();
                    else
                    {
                        CodeString += "";
                    }
                }

                startIndex = 8;
                while (AptBytes[startIndex] != 0)
                {   //заполняем имя аэропорта
                    NameString += ((Char)AptBytes[startIndex]).ToString();
                    startIndex++;
                }
                
                Rw rw = new Rw();
                //if (CodeString == "USSS")
                //{
                //    string temp = "";
                //    for (int i = 0; i < AptBytes.Length; i++)
                //    {
                //        if (ByteOperations.IsByteChar(AptBytes[i]))
                //        {
                //            temp += ((Char)AptBytes[i]).ToString();
                //        }
                //        //temp += (Char) AptBytes[i];
                //    }
                //}
                for (int index = startIndex; index < AptBytes.Length; index++)
                {

                    if (ByteOperations.IsNumeric(AptBytes[index]) && ByteOperations.IsNumeric(AptBytes[index + 1])
                                                                  && index < AptBytes.Length - 8)//пропускаем данные, не являющиеся описанием полосы
                    {
                        if (DataBytes == null)
                            DataBytes = Cut(AptBytes, startIndex, index - 1);
                        else
                        {
                            rw.DataBytes = new List<byte>();
                            rw.DataBytes.AddRange(Cut(AptBytes, startIndex, index - 1));
                        }

                        if (!string.IsNullOrEmpty(rw.Rw1String))
                            Rws.Add(rw);

                        rw.Rw1String = "";
                        rw.Rw1Byte = new List<byte>();
                        for (int btIndex = 0; btIndex < 3; btIndex++)
                        {
                            rw.Rw1Byte.Add(AptBytes[index]);
                            if (ByteOperations.IsByteChar(AptBytes[index]))
                                rw.Rw1String += ((Char)AptBytes[index]).ToString();
                            index++;
                        }

                        index++;

                        rw.Rw2String = "";
                        rw.Rw2Byte = new List<byte>();
                        for (int btIndex = 0; btIndex < 3; btIndex++)
                        {
                            rw.Rw2Byte.Add(AptBytes[index]);
                            if (ByteOperations.IsByteChar(AptBytes[index]))
                                rw.Rw2String += ((Char)AptBytes[index]).ToString();
                            index++;
                        }

                        startIndex = index;
                    }
                }

                if (DataBytes == null)
                    DataBytes = Cut(AptBytes, startIndex, AptBytes.Length - 1);
                else
                {
                    rw.DataBytes = new List<byte>();
                    rw.DataBytes.AddRange(Cut(AptBytes, startIndex, AptBytes.Length - 1));
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
            if (ByteOperations.IsCapitalLetter(bb))
            {
                if (count >= data.Length)
                    return new Rw();
                count++;
                bb = data[count];
                while (bb != 0 && ByteOperations.IsNumeric(bb))
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