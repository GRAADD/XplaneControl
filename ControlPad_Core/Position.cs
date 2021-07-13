using System;

namespace XplaneControl
{
    public static class Position
    {
        public static float Lng;
        public static float Lat;
        public static float Alt_Asl;
        public static float Alt_Agl;
        public static float Hdg;
        public static float Speed;
        public static float[] DataFloats = new float[22];

        public static void GetData(byte[] bytes)
        {
            byte[] byteValue = new byte[4];
            try
            {
                for (int i = 0; i < 22; i++)
                {
                    DataFloats[i] = BitConverter.ToSingle(bytes, i*4);
                }

                Lng = DataFloats[0];
                Lat = DataFloats[1];
                Alt_Asl = DataFloats[2];
                Alt_Agl = DataFloats[3];
                Hdg = DataFloats[6];
                Speed = DataFloats[9];
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}