using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XplaneControl;

namespace XplaneMaster.CommandPages.WeatherPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WxrMain : ContentPage
    {

        MySlider dateSlider = new MySlider
        {
            Maximum = 365,
            Value = 182,
            Minimum = 1,
        };
        Label dateLabel = new Label();
        MySlider timeSlider = new MySlider
        {
            Maximum = 240,
            Value = 120,
            Minimum = 1,
        };
        Label timeLabel = new Label();
        Button thunderButton = new Button
        {
            Text = "ThunderStorm"
        };
        Button fogButton = new Button
        {
            Text = "foggy"
        };
        Button setWeatherButton = new Button
        {
            Text = "Set Weather"
        };
        Button cavokButton = new Button
        {
            Text = "CAVOK"
        };

        public WxrMain()
        {
            InitializeComponent();
            DrawGrids();
            setWeatherButton.Clicked += ((object sender, EventArgs e) =>
            {
                Core.SendWeather();
            });
            cavokButton.Clicked += ((object sender, EventArgs e) =>
            {
                Core.CavokWeather();
            });
            thunderButton.Clicked += ((object sender, EventArgs e) =>
            {
                Core.ThunderWeather();
            });
            timeSlider.ValueChanged += UpdateLabels;
            dateSlider.ValueChanged += UpdateLabels;
            UpdateLabels(this, null);
        }

        private void UpdateLabels(object sender, EventArgs e)
        {
            timeLabel.Text = Core.Current.GetStringFromValue((float)timeSlider.doStep(), Atmosphere.Type.TTime);
            dateLabel.Text = Core.Current.GetStringFromValue((float)dateSlider.doStep(), Atmosphere.Type.TDate);
        }

        private void DrawGrids()
        {

            //decalring main grid
            Grid mainGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },

                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 2
            };

            Grid dayGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star)}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(90, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 2
            };
            dayGrid.Children.Add(dateLabel, 0, 0);
            dayGrid.Children.Add(dateSlider, 1, 0);
            Grid timeGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star)}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(90, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 2
            };
            timeGrid.Children.Add(timeLabel, 0, 0);
            timeGrid.Children.Add(timeSlider, 1, 0);
            Grid buttonsGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star)}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 2
            };
            buttonsGrid.Children.Add(fogButton, 0, 0);
            buttonsGrid.Children.Add(thunderButton, 1, 0);
            buttonsGrid.Children.Add(cavokButton, 2, 0);
            buttonsGrid.Children.Add(setWeatherButton, 4, 0);

            mainGrid.Children.Add(dayGrid,0, 0);
            mainGrid.Children.Add(timeGrid, 0, 1);
            mainGrid.Children.Add(buttonsGrid, 0, 2);
            Content = mainGrid;
        }
    }
}