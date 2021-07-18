using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
//using XplaneControl;
using XplaneMaster.CommandPages;

namespace XplaneMaster
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            //MainPage = new Page();
            //Core.CavokWeather();
            MainPage = new NavigationPage(new TitlePage());
        }

        private void SetStyles()
        {
            Resources = new ResourceDictionary
            {
                {
                    "buttonStyle", new Style(typeof(Button))
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = Button.TextColorProperty,
                                Value = Color.FromHex("#ffffff")
                            },
                            new Setter
                            {
                                Property = Button.BackgroundColorProperty,
                                Value = Color.FromHex("#424242")
                            },
                            new Setter
                            {
                                Property = Button.CornerRadiusProperty,
                                Value = 16
                            }
                        }
                    }
                },
                {
                    "tabbedPageStyle", new Style(typeof(TabbedPage))
                    {
                        Setters =
                        {
                            new Setter
                            {
                                Property = TabbedPage.BackgroundColorProperty,
                                Value = Color.FromHex("#1b1b1b")
                            }
                        }
                    }
                }
            };
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            //DebugPart.SaveLogs();
            //Console.WriteLine();
        }

        protected override void OnResume()
        {
        }


    }
}
