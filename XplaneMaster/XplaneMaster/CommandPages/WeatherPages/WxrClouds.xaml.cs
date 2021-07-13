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
    public partial class WxrClouds : ContentPage
    {
        #region top layer
        MySlider topTopLayerSlider = new MySlider
        {
            Maximum = 50000,
            Value = 26000,
            Minimum = 19000,
        };
        Label topTopLayerLabel = new Label();
        MySlider topBaseLayerSlider = new MySlider
        {
            Maximum = 30000,
            Value = 24000,
            Minimum = 17000,
        };
        Label topBaseLayerLabel = new Label();
        MySlider topNumberSlider = new MySlider
        {
            Maximum = 6,
            Value = 0,
            Minimum = 0
        };
        Label topNumberLabel = new Label();
        #endregion

        #region mid layer
        MySlider midTopLayerSlider = new MySlider
        {
            Maximum = 30000,
            Value = 22000,
            Minimum = 15000,
        };
        MySlider midBaseLayerSlider = new MySlider
        {
            Maximum = 15000,
            Value = 20000,
            Minimum = 8000,
        };
        Label midTopLayerLabel = new Label();
        Label midBaseLayerLabel = new Label();
        MySlider midNumberSlider = new MySlider
        {
            Maximum = 6,
            Value = 0,
            Minimum = 0
        };
        Label midNumberLabel = new Label();
        #endregion

        #region low layer
        MySlider lowTopLayerSlider = new MySlider
        {
            Maximum = 19000,
            Value = 7000,
            Minimum = 5000,
        };
        Label lowTopLayerLabel = new Label();
        MySlider lowBaseLayerSlider = new MySlider
        {
            Maximum = 5000,
            Value = 5000,
            Minimum = 0,
        };
        Label lowBaseLayerLabel = new Label();
        MySlider lowNumberSlider = new MySlider
        {
            Maximum = 6,
            Value = 0,
            Minimum = 0
        };
        Label lowNumberLabel = new Label();
        #endregion
        
        Button setWeatherButton = new Button
        {
            Text = "SET WEATHER"
        };

        public WxrClouds()
        {
            InitializeComponent();
            CreateGrids();
            SliderValueChanged(this, null);
        }

        private void CreateGrids()
        {
            setWeatherButton.Clicked += SendWeather;

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
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },

                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 2
            };

            //declaring top layer
            #region top layer declaration

            Grid topTopLayerGrid = new Grid
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
            Grid topBaseLayerGrid = new Grid
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
            Grid topNumberGrid = new Grid
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
            
            topTopLayerGrid.Children.Add(topTopLayerLabel, 0, 0);
            topTopLayerGrid.Children.Add(topTopLayerSlider, 1, 0);

            topBaseLayerGrid.Children.Add(topBaseLayerLabel, 0, 0);
            topBaseLayerGrid.Children.Add(topBaseLayerSlider, 1, 0);

            topNumberGrid.Children.Add(topNumberLabel, 0, 0);
            topNumberGrid.Children.Add(topNumberSlider, 1, 0);
            #endregion

            //declaring mid layer
            #region mid layer declaration

            Grid midTopLayerGrid = new Grid
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
            Grid midBaseLayerGrid = new Grid
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
            Grid midNumberGrid = new Grid
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

            midTopLayerGrid.Children.Add(midTopLayerLabel, 0, 0);
            midTopLayerGrid.Children.Add(midTopLayerSlider, 1, 0);

            midBaseLayerGrid.Children.Add(midBaseLayerLabel, 0, 0);
            midBaseLayerGrid.Children.Add(midBaseLayerSlider, 1, 0);

            midNumberGrid.Children.Add(midNumberLabel, 0, 0);
            midNumberGrid.Children.Add(midNumberSlider, 1, 0);
            #endregion
            
            //declaring low layer
            #region low layer declaration

            Grid lowTopLayerGrid = new Grid
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
            Grid lowBaseLayerGrid = new Grid
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
            Grid lowNumberGrid = new Grid
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

            lowTopLayerGrid.Children.Add(lowTopLayerLabel, 0, 0);
            lowTopLayerGrid.Children.Add(lowTopLayerSlider, 1, 0);

            lowBaseLayerGrid.Children.Add(lowBaseLayerLabel, 0, 0);
            lowBaseLayerGrid.Children.Add(lowBaseLayerSlider, 1, 0);

            lowNumberGrid.Children.Add(lowNumberLabel, 0, 0);
            lowNumberGrid.Children.Add(lowNumberSlider, 1, 0);
            #endregion
            
            
            #region main grid fill

            mainGrid.Children.Add(topTopLayerGrid, 0, 0);
            mainGrid.Children.Add(topBaseLayerGrid, 0, 1);
            mainGrid.Children.Add(topNumberGrid, 0, 2);

            mainGrid.Children.Add(midTopLayerGrid, 0, 3);
            mainGrid.Children.Add(midBaseLayerGrid, 0, 4);
            mainGrid.Children.Add(midNumberGrid, 0, 5);

            mainGrid.Children.Add(lowTopLayerGrid, 0, 6);
            mainGrid.Children.Add(lowBaseLayerGrid, 0, 7);
            mainGrid.Children.Add(lowNumberGrid, 0, 8);

            mainGrid.Children.Add(setWeatherButton, 0, 9);

            #endregion


            #region value changed subs

            topTopLayerSlider.ValueChanged += SliderValueChanged;
            topBaseLayerSlider.ValueChanged += SliderValueChanged;
            topNumberSlider.ValueChanged += SliderValueChanged;

            midTopLayerSlider.ValueChanged += SliderValueChanged;
            midBaseLayerSlider.ValueChanged += SliderValueChanged;
            midNumberSlider.ValueChanged += SliderValueChanged;

            lowTopLayerSlider.ValueChanged += SliderValueChanged;
            lowBaseLayerSlider.ValueChanged += SliderValueChanged;
            lowNumberSlider.ValueChanged += SliderValueChanged;
            #endregion


            Content = mainGrid;
        }

        private void SliderValueChanged(object sender, EventArgs e)
        {
            topTopLayerLabel.Text = $"{Core.Current.GetStringFromValue((float)topTopLayerSlider.doStep(), Atmosphere.Type.TCloudsLayer3Top)}";
            topBaseLayerLabel.Text = $"{Core.Current.GetStringFromValue((float)topBaseLayerSlider.doStep(), Atmosphere.Type.TCloudsLayer3Base)}";
            topNumberLabel.Text = $"{Core.Current.GetStringFromValue((float)topNumberSlider.doStep(), Atmosphere.Type.TCloudsLayer3Cat)}";

            midTopLayerLabel.Text = $"{Core.Current.GetStringFromValue((float)midTopLayerSlider.doStep(), Atmosphere.Type.TCloudsLayer2Top)}";
            midBaseLayerLabel.Text = $"{Core.Current.GetStringFromValue((float)midBaseLayerSlider.doStep(), Atmosphere.Type.TCloudsLayer2Base)}";
            midNumberLabel.Text = $"{Core.Current.GetStringFromValue((float)midNumberSlider.doStep(), Atmosphere.Type.TCloudsLayer2Cat)}";

            lowTopLayerLabel.Text = $"{Core.Current.GetStringFromValue((float)lowTopLayerSlider.doStep(), Atmosphere.Type.TCloudsLayer1Top)}";
            lowBaseLayerLabel.Text = $"{Core.Current.GetStringFromValue((float)lowBaseLayerSlider.doStep(), Atmosphere.Type.TCloudsLayer1Base)}";
            lowNumberLabel.Text = $"{Core.Current.GetStringFromValue((float)lowNumberSlider.doStep(), Atmosphere.Type.TCloudsLayer1Cat)}";
        }

        private void SendWeather(object sender, EventArgs e)
        {
            Core.SendWeather();
        }
    }
}