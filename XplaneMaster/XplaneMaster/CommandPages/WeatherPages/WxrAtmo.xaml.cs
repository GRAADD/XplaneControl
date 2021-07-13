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
    public partial class WxrAtmo : ContentPage
    {

        MySlider visibilitySlider = new MySlider
        {
            Maximum = 50000,
            Value = 20000,
            Minimum = 0,
        };
        Label visibilityLabel = new Label();

        MySlider thunderSlider = new MySlider
        {
            Maximum = 100,
            Value = 0,
            Minimum = 0,
        };
        Label thunderLabel = new Label();

        MySlider rainSlider = new MySlider
        {
            Maximum = 100,
            Value = 0,
            Minimum = 0,
        };
        Label rainLabel = new Label();

        MySlider temperatureSlider = new MySlider
        {
            Maximum = 40,
            Value = 15,
            Minimum = -40,
        };
        Label temperatureLabel = new Label();

        MySlider pressureSlider = new MySlider
        {
            Maximum = 3050,
            Value = 2992,
            Minimum = 2900,
        };
        Label pressureLabel = new Label();

        MySlider ratSlider = new MySlider
        {
            Maximum = 25,
            Value = 0,
            Minimum = 0,
        };
        Label ratLabel = new Label();

        MySlider fpmSlider = new MySlider
        {
            Maximum = 150000,
            Value = 0,
            Minimum = 0,
        };
        Label fpmLabel = new Label();

        CheckBox patchyBox = new CheckBox();
        Label patchyLabel = new Label();
        CheckBox dryBox = new CheckBox();
        Label dryLabel = new Label();
        CheckBox dampBox = new CheckBox();
        Label dampLabel = new Label();
        CheckBox wetBox = new CheckBox();
        Label wetLabel = new Label();

        Button setWeatherButton = new Button
        {
            Text = "SET WEATHER"
        };

        public WxrAtmo()
        {
            InitializeComponent();
            CreateGrids();
            UpdateLabels(this, null);
        }

        private void CreateGrids()
        {
            //subcruibing for slider changes
            visibilitySlider.ValueChanged += UpdateLabels;
            thunderSlider.ValueChanged += UpdateLabels;
            rainSlider.ValueChanged += UpdateLabels;
            temperatureSlider.ValueChanged += UpdateLabels;
            pressureSlider.ValueChanged += UpdateLabels;
            ratSlider.ValueChanged += UpdateLabels;
            fpmSlider.ValueChanged += UpdateLabels;

            setWeatherButton.Clicked += SendWeather;

            //declaring subgrids
            Grid visibilityGrid = new Grid
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
            visibilityGrid.Children.Add(visibilityLabel, 0 , 0);
            visibilityGrid.Children.Add(visibilitySlider, 1, 0);

            Grid thunderGrid = new Grid
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
            thunderGrid.Children.Add(thunderLabel, 0, 0);
            thunderGrid.Children.Add(thunderSlider, 1, 0);

            Grid rainGrid = new Grid
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
            rainGrid.Children.Add(rainLabel, 0, 0);
            rainGrid.Children.Add(rainSlider, 1, 0);

            Grid rwStatGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star)}
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 2
            };
            rwStatGrid.Children.Add(dryBox, 0, 0);
            rwStatGrid.Children.Add(dampBox, 1, 0);
            rwStatGrid.Children.Add(wetBox, 2, 0);
            rwStatGrid.Children.Add(patchyBox, 3, 0);

            rwStatGrid.Children.Add(dryLabel, 0, 1);
            rwStatGrid.Children.Add(dampLabel, 1, 1);
            rwStatGrid.Children.Add(wetLabel, 2, 1);
            rwStatGrid.Children.Add(patchyLabel, 3, 1);

            Grid temperatureGrid = new Grid
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
            temperatureGrid.Children.Add(temperatureLabel, 0, 0);
            temperatureGrid.Children.Add(temperatureSlider, 1, 0);

            Grid pressureGrid = new Grid
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
            pressureGrid.Children.Add(pressureLabel, 0, 0);
            pressureGrid.Children.Add(pressureSlider, 1, 0);

            Grid ratGrid = new Grid
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
            ratGrid.Children.Add(ratLabel, 0, 0);
            ratGrid.Children.Add(ratSlider, 1, 0);


            Grid fpmGrid = new Grid
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
            fpmGrid.Children.Add(fpmLabel, 0, 0);
            fpmGrid.Children.Add(fpmSlider, 1, 0);

            //decalring main grid
            Grid mainGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },

                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },

                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },

                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },

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
            mainGrid.Children.Add(visibilityGrid,0,0);
            mainGrid.Children.Add(thunderGrid, 0, 1);
            mainGrid.Children.Add(rainGrid, 0, 2);
            mainGrid.Children.Add(rwStatGrid, 0, 3);
            mainGrid.Children.Add(temperatureGrid, 0, 4);
            mainGrid.Children.Add(pressureGrid, 0, 5);
            mainGrid.Children.Add(ratGrid, 0, 6);
            mainGrid.Children.Add(fpmGrid, 0, 7);
            mainGrid.Children.Add(setWeatherButton, 0, 8);
            Content = mainGrid;
        }

        private void UpdateLabels(object sender, EventArgs e)
        {
            visibilityLabel.Text = Core.Current.GetStringFromValue((float) visibilitySlider.doStep(), Atmosphere.Type.TVisibility);
            thunderLabel.Text = Core.Current.GetStringFromValue((float) thunderSlider.doStep(), Atmosphere.Type.TStorm);
            rainLabel.Text = Core.Current.GetStringFromValue((float) rainSlider.doStep(), Atmosphere.Type.TRain);
            temperatureLabel.Text = Core.Current.GetStringFromValue((float) temperatureSlider.doStep(), Atmosphere.Type.TTemperature);
            pressureLabel.Text = Core.Current.GetStringFromValue((float) pressureSlider.doStep(), Atmosphere.Type.TBaro);
            ratLabel.Text = Core.Current.GetStringFromValue((float) rainSlider.doStep(), Atmosphere.Type.TTermCov);
            fpmLabel.Text = Core.Current.GetStringFromValue((float) fpmSlider.doStep(), Atmosphere.Type.TTermStr);
        }

        private void SendWeather(object sender, EventArgs e)
        {
            Core.SendWeather();
        }
    }
}