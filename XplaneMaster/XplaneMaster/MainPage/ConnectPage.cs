using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Plugin.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;
using XplaneControl;
using XplaneMaster.MainPage;

namespace XplaneMaster
{
    public class ConnectPage : ContentPage
    {
        private Button connectToMasterButton;
        private Label CurrentIp;
        private Label MachinesInNetwork;
        private Label MastersInNetwork;
        private Label ConnectionState;
        private Label ConnectedTo;
        private Label Status;
        private Entry ipEntry;
        private string ip = "";
        //public Connection connection;
        public ConnectPage()
        {
            Label ipHello = new Label
            {
                Text = "Enter ip master ip"
            };
            ipEntry = new Entry
            {
                Text = "192.168.5.17"
            };
            Button enterButton = new Button
            {
                Text = "Connect"
            };
            Status = new Label()
            {
                Text = $"Waiting for connection"
            };
            enterButton.Clicked += EnterButton_Clicked;
            Content = new StackLayout()
            {
                Children = { ipHello, ipEntry, enterButton, Status}
            };
            //if (Application.Current.Properties.ContainsKey("MasterAdress"))
            //{
            //    string ip = Application.Current.Properties["MasterAdress"] as string;
            //    //ConnectionCore.MasterIp = ip;
            //    Core.XplaneConnection = new Connection(ip);
            //    PostConnectionShow();
            //    if (Application.Current.Properties.ContainsKey("Fails"))
            //    {
            //        XplaneControl.Fails.Pages = Application.Current.Properties["Fails"] as List<FailPage>;
            //    }
            //}
            //else
            //{
            //}
            //BeforeConnectionNew();
            //BeforeConection();
            
            //Thread updatePage = new Thread(UpdatePage);
            //updatePage.Start();

        }

        private void EnterButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["MasterAdress"] = ipEntry.Text;
            Core.XplaneConnection = new Connection(ipEntry.Text);
            //_connection = new Connection(MasterIpTxtBx.Text);
            //SendTCP.Enabled = true;
            //SendUDP.Enabled = true;
            PostConnectionShow();
        }
        

        private void PostConnectionShow()
        {
            Thread findThread = new Thread(PostConnectionLoop);
            findThread.Start();
        }

        private void PostConnectionLoop()
        {
            int sum = -1;
            Thread.CurrentThread.Name = "Connection status thread";
            while (!Core.XplaneConnection.TcpFinished)
            {
                int temp = Core.XplaneConnection.xCMD.Count + Core.XplaneConnection.xFIX.Count + Core.XplaneConnection.xNAVH.Count +
                           Core.XplaneConnection.Airports.Count + Core.XplaneConnection.AcfMessages.Count +
                           Core.XplaneConnection.FailMessages.Count;
                if (sum == temp)
                {
                    Task.Delay(300).Wait();
                    continue;
                }
                sum = temp;
                ConnectedTo = new Label()
                    {
                        Text = $"Connected to {Core.XplaneConnection.MasterIp}"
                    };
                    //Label ConnectedFrom = new Label()
                    //{
                    //    Text = $"Your IP is {Core.XplaneConnection.SlaveIp}"
                    //};
                    Status = new Label()
                    {
                        Text = $"Currently downloaded:\r\n" +
                               $"   commands - {Core.XplaneConnection.xCMD.Count}\r\n" +
                               $"   FIX points - {Core.XplaneConnection.xFIX.Count}\r\n" +
                               $"   NAV points - {Core.XplaneConnection.xNAVH.Count}\r\n" +
                               $"   airports - {Core.XplaneConnection.Airports.Count}\r\n" +
                               $"   aircraft - {Core.XplaneConnection.AcfMessages.Count}\r\n" +
                               $"   fails - {Core.XplaneConnection.FailMessages.Count}\r\n"
                    };

                Device.BeginInvokeOnMainThread(() =>
                {
                    Content = new StackLayout()
                    {
                        Children = {Status }// ConnectedTo, 
                    };
                });
            }
        }
    }
}