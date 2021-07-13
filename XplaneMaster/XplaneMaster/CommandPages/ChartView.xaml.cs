using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XplaneMaster.CommandPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartView : ContentPage
    {
        private string Name = "";
        private string Url = "";
        private string PdfFile = "";

        Button goBack = new Button();
        public ChartView(string code)
        {
            goBack = new Button
            {
                Text = "Back"
            };
            Name = code;
            Url = $"https://vau.aero/navdb/chart/{code}.pdf";
            PdfFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), $"{Name}.pdf");
            goBack.Clicked += GoBack_Clicked;
            Padding = new Thickness(0, 20, 0, 0);
            InitializeComponent();
            DrawContent();
        }

        async void DrawContent()
        {
            ActivityIndicator indicator = new ActivityIndicator
            {
                IsRunning = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            this.Content = new StackLayout
            {
                Children =
                {
                    indicator,
                    goBack
                }
            };
            DownloadFile();
            //webClient.DownloadFile(url, path);

            //NavigationPage navPage = (NavigationPage)Application.Current.MainPage;

        }

        private void DownloadFile()
        {

            var webClient = new WebClient();
            File.Create(PdfFile).Dispose();
            webClient.DownloadDataAsync(new Uri(Url));
            byte[] data = new byte[0];
            webClient.DownloadDataCompleted += (s, e) =>
            {
                //File.Delete(path);
                try
                {
                    data = e.Result;
                    //File.WriteAllBytes(path, data);
                    FileStream file = new FileStream(PdfFile, FileMode.Create);
                    foreach (byte b in data)
                    {
                        file.WriteByte(b);
                    }
                    file.Close();
                    file.Dispose();

                    if (!File.Exists(PdfFile))
                        File.WriteAllBytes(PdfFile, data);

                    CustomWebView webView = new CustomWebView
                    {
                        Uri = PdfFile,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };
                    this.Content = new StackLayout
                    {
                        Children =
                        {
                            webView,
                            goBack
                        }
                    };

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    Navigation.PopModalAsync();
                }
            };

            //webClient.DownloadFileAsync(new Uri(url), path);
            //webClient.DownloadFile(new Uri(url), path);
            //webClient.DownloadDataAsync(new Uri (url));
        }

        private async void GoBack_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}