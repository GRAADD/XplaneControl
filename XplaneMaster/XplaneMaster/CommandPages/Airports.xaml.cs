using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using XplaneControl;

namespace XplaneMaster.CommandPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Airports : ContentPage
    {
        Button SetButton = new Button();
        Button clearButton = new Button();
        Entry airportEntry = new Entry();
        StackLayout dynamicStack = new StackLayout();
        private MySlider standSlider;
        private Label standLabel;
        private string icaocode = "";

        public Airports()
        {
            InitializeComponent();
            DrawComponents();
        }

        public delegate void SearchEventHandler(string text);
        public event SearchEventHandler Search;
        private void DrawComponents()
        {
            InitializeComponent();
            SetButton = new Button
            {
                Text = "Поиск"
            };
            SetButton.Clicked += SetBtn_Clicked;

            airportEntry = new Entry
            {
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            airportEntry.Completed += SetBtn_Clicked;
            airportEntry.TextChanged += airportEntry_TextChanged;
            ScrollView scrollView = new ScrollView()
            {
                Content = dynamicStack
            };

            Content = new StackLayout
            {
                Children =
                {
                    airportEntry,
                    SetButton,
                    scrollView
                },
                Spacing = 3
            };
        }

        private void SetBtn_Clicked(object sender, EventArgs e)
        {
            LoadAptData(airportEntry.Text);
        }

        private void ClearBtn_Clicked(object sender, EventArgs e)
        {
            airportEntry.Text = "";
        }

        private void airportEntry_TextChanged(object sender, EventArgs e)
        {
            dynamicStack.Children.Clear();
            if (string.IsNullOrEmpty(airportEntry.Text))
                return;

            try
            {
                List<XAirport> apts = ByteOperations.GetAirports(airportEntry.Text);
                if (apts.Count > 10)
                    apts = apts.GetRange(0, 10);
                foreach (var apt in apts)
                {
                    AddButtonToStackLayout(dynamicStack, apt);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }


        }

        private void AddButtonToStackLayout(StackLayout stackLayout, XAirport apt)
        {
            Button btn = new Button()
            {
                Text = $"[{apt.CodeString}]-{apt.NameString}"
            };
            btn.Clicked += SetApt;
            stackLayout.Children.Add(btn);
        }

        private void SetApt(object sender, EventArgs e)
        {
            Button btn = (Button) sender;
            airportEntry.Text = btn.Text.Split('-')[1];
            LoadAptData(airportEntry.Text);
        }

        private void LoadAptData(string airport)
        {
            dynamicStack.Children.Clear();
            icaocode = airport;
            if (!string.IsNullOrEmpty(airport))
            {
                XAirport apt = ByteOperations.GetAirport(airport);
                if (!string.IsNullOrEmpty(apt.CodeString))
                {
                    icaocode = apt.CodeString;
                    foreach (var rw in apt.Rws)
                    {
                        dynamicStack.Children.Add(
                            MakeGridForRw(rw.Rw1String));

                        if (!string.IsNullOrEmpty(rw.Rw2String))
                        {
                            dynamicStack.Children.Add(
                                MakeGridForRw(rw.Rw2String));
                        }
                    }

                    Grid standGrid = new Grid
                    {
                        RowDefinitions =
                        {
                            new RowDefinition { Height = 60 }
                        },
                        ColumnDefinitions =
                        {
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                            new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                        },
                        ColumnSpacing = 5
                    };
                    
                    standSlider = new MySlider
                    {
                        Maximum = 40,
                        Value = 0,
                        Minimum = 0,
                        Step = 1
                    };
                    standSlider.ValueChanged += (object sender, ValueChangedEventArgs e) =>
                    {
                        standLabel.Text = Math.Round(standSlider.Value).ToString();
                    };
                    Button setStand = new Button()
                    {
                        Text = "Stand"
                    };
                    setStand.Clicked += SetStand_Clicked;
                    standLabel = new Label
                    {
                        Text = "0"
                    };
                    
                    standGrid.Children.Add(standLabel, 0, 0);
                    standGrid.Children.Add(standSlider, 1, 0);
                    standGrid.Children.Add(setStand, 2, 0);
                    dynamicStack.Children.Add(standGrid);
                }
            }

            dynamicStack.Children.Add(new Label
            {
                Text = GetMetar(icaocode)
            });
            Button Charts = new Button
            {
                Text = icaocode
            };
            Charts.Clicked += Charts_Clicked;
            dynamicStack.Children.Add(Charts);
        }

        private async void Charts_Clicked(object sender, EventArgs e)
        {
            Button btn = (Button) sender;
            //ChartView page = new ChartView(url);;
            //NavigationPage navPage = new NavigationPage(this);
            Navigation.PushModalAsync(new ChartView(btn.Text));
            //await navPage.PushAsync(new ChartView(url));
            //TitlePage = new NavigationPage(new TitlePage());
            //await Navigation.PushAsync(page);
        }

        private string GetMetar(string code)
        {
            byte[] htmlCode;
            try
            {
                using (WebClient client = new WebClient())
                {
                    htmlCode = client.DownloadData($"https://metartaf.ru/{code}.json");
                }

                string metar = Encoding.UTF8.GetString(htmlCode);
                metar = metar.Replace("\"", "");
                metar = metar.Replace("{", "");
                metar = metar.Replace("}", "");
                metar = metar.Replace(",", "\r\n");
                metar = metar.Replace("\\n", "\r\n");
                metar = metar.Replace("NOSIG", "\r\nNOSIG");
                metar = metar.Replace("TEMPO", "\r\nTEMPO");
                metar = metar.Replace("BECMG", "\r\nBECMG");
                return metar;
            }
            catch (Exception)
            {
                return "No metar data for this airport...";
                //do nothing
            }

        }

        
        private void SetAptPos_Clicked(object sender, EventArgs e)
        {
            //CommandEncoder commandEncoder = new CommandEncoder();
            Button button = (Button)sender;
            string code = icaocode;
            string place = button.Text.Split(' ')[0];
            //string standNumber = $"{Math.Round(standSlider.Value)}_Stand";
            Core.XplaneConnection.SendMessage(CmdEncoder.MoveToAirportBytes(code, place));
        }
        private void SetStand_Clicked(object sender, EventArgs e)
        {
            //CommandEncoder commandEncoder = new CommandEncoder();
            Button button = (Button)sender;
            string code = icaocode;
            //string place = button.Text.Split(' ')[0];
            string standNumber = $"{Math.Round(standSlider.Value)}_Stand";
            Core.XplaneConnection.SendMessage(CmdEncoder.MoveToAirportBytes(code, standNumber));
        }

        private Grid MakeGridForRw(string rw)
        {
            Button btn_10nm = new Button()
            {
                Text = $"10nm {rw}"
            };
            btn_10nm.Clicked += SetAptPos_Clicked;
            Button btn_3nm = new Button()
            {
                Text = $"3nm {rw}"
            };
            btn_3nm.Clicked += SetAptPos_Clicked;
            Button btn_Start = new Button()
            {
                Text = $"start {rw}"
            };
            btn_Start.Clicked += SetAptPos_Clicked;
            Grid grid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = 60 }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 5
            };

            grid.Children.Add(btn_10nm, 2, 0);
            grid.Children.Add(btn_3nm, 1, 0);
            grid.Children.Add(btn_Start, 0, 0);

            return grid;
        }
    }
}