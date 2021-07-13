using System;
using System.Collections.Generic;
using System.Net;

namespace XplaneControl
{
    public class Fail
    {
        public int Num;
        public int Position;
        public int SubPosition;
        public bool Visibility;
        public string Name = "";
        public int Page;
        public int SubPage;

        public void PageName_Set(string name)
        {

        }
    }

    public static class Info
    {
        public static List<string> MasterIpList
        {
            get
            {
                String strHostName = string.Empty;
                // Getting Ip address of local machine...
                // First get the host name of local machine.
                strHostName = Dns.GetHostName();
                // Then using host name, get the IP address list..
                IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;
                List<string> output = new List<string>();
                foreach (IPAddress address in addr)
                {
                    output.Add(address.ToString());
                }
                return output;
            }
            private set
            {

            }
        }

    }
}