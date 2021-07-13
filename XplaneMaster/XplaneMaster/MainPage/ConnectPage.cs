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

namespace XplaneMaster
{
    public class ConnectPage : ContentPage
    {
        private Button connectToMasterButton;
        private Label CurrentIp;
        private Label MachinesInNetwork;
        private Label MastersInNetwork;
        private Label ConnectionState;
        private Entry ipEntry;
        private string ip = "";
        public Connection connection;
        public ConnectPage()
        {
            if (Application.Current.Properties.ContainsKey("MasterAdress"))
            {
                string ip = Application.Current.Properties["MasterAdress"] as string;
                //ConnectionCore.MasterIp = ip;
                connection = new Connection(ip);
                PostConnectionShow();
                if (Application.Current.Properties.ContainsKey("Fails"))
                {
                    XplaneControl.Fails.Pages = Application.Current.Properties["Fails"] as List<FailPage>;
                }
            }
            else
            {
                BeforeConnectionNew();
            }
            //BeforeConection();
            
            //Thread updatePage = new Thread(UpdatePage);
            //updatePage.Start();

        }

        private void BeforeConnectionNew()
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
            enterButton.Clicked += EnterButton_Clicked;
            Content = new StackLayout()
            {
                Children = { ipHello, ipEntry, enterButton }
            };
        }

        private void EnterButton_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["MasterAdress"] = ipEntry.Text;
            connection = new Connection(ipEntry.Text);
            PostConnectionShow();
        }
        

        private void FindMaster_DrawContent()
        {
            Label ConnectingNow = new Label()
            {
                Text = "Search for masters"
            };

            String status = "";
            var profiles = Connectivity.ConnectionProfiles;
            var current = Connectivity.NetworkAccess;
            status += $"Access to local network = {current == (NetworkAccess.Local)}, access to Internet = {current == (NetworkAccess.Internet)}," +
                      $" WiFi is enabled = {profiles.Contains(ConnectionProfile.WiFi)}";
            ConnectionState = new Label()
            {
                Text = status
            };

            Content = new StackLayout()
            {
                Children = {ConnectingNow, CurrentIp, MachinesInNetwork, MastersInNetwork, ConnectionState }
            };
        }
        

        private void ConnectToMaster(object sender, EventArgs e)
        {
            Button button = (Button) sender;
            string master = button.Text;
            //CrossSettings.Current.AddOrUpdateValue("MasterIp", master);
            Core.MasterConnected = true;
            string[] splitted = master.Split(' ');
            master = splitted[splitted.Length - 1];

            Application.Current.Properties["MasterAdress"] = master;
            try
            {
                connection = new Connection(master);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
            Content = new StackLayout()
            {
                Children = { new Label
                {
                    Text = $"Connected to {master}",
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center
                } }
            };
            PostConnectionShow();
        }

        private void PostConnectionShow()
        {
            int sum = 0;
            Thread findThread = new Thread(() =>
            {
                Thread.CurrentThread.Name = "Connection status thread";
                while (!connection.forceStop)
                {
                    int temp = connection.xCMD.Count + connection.xFIX.Count + connection.xNAVH.Count +
                               connection.Airports.Count + connection.AcfMessages.Count +
                               connection.FailMessages.Count;
                    if (sum == temp)
                    {
                        Task.Delay(300).Wait();
                        continue;
                    }
                    sum = temp;
                    Label ConnectedTo = new Label()
                    {
                        Text = $"Connected to {connection.MasterIp}"
                    };
                    Label ConnectedFrom = new Label()
                    {
                        Text = $"Your IP is {connection.SlaveIp}"
                    };
                    Label Status = new Label()
                    {
                        Text = $"Currently downloaded:\r\n" +
                               $"   commands - {connection.xCMD.Count}\r\n" +
                               $"   FIX points - {connection.xFIX.Count}\r\n" +
                               $"   NAV points - {connection.xNAVH.Count}\r\n" +
                               $"   airports - {connection.Airports.Count}\r\n" +
                               $"   aircraft - {connection.AcfMessages.Count}\r\n" +
                               $"   fails - {connection.FailMessages.Count}\r\n"
                    };

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Content = new StackLayout()
                        {
                            Children = { ConnectedTo, ConnectedFrom, Status}
                        };
                    });
                }
            });
            findThread.Start();
        }
    }
}