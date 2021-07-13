using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XplaneControl;

namespace XplaneMaster.CommandPages.WeatherPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WxrWind : ContentPage
    {
        MySlider topWindAltSlider = new MySlider
        {
            Maximum = 50000,
            Value = 24000,
            Minimum = 16010,
        };
        Label topWindAltLabel = new Label();
        MySlider topWindDirSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 360
        };
        Label topWindDirLabel = new Label();
        MySlider topWindStrSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 100
        };
        Label topWindStrLabel = new Label();
        MySlider topWindGstSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 100
        };
        Label topWindGustLabel = new Label();
        MySlider topWindTrbSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 100
        };
        Label topWindTrbLabel = new Label();

        MySlider midWindAltSlider = new MySlider
        {
            Maximum = 23990,
            Value = 16000,
            Minimum = 4010
        };
        Label midWindAltLabel = new Label();
        MySlider midWindDirSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 360
        };
        Label midWindDirLabel = new Label();
        MySlider midWindStrSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 100
        };
        Label midWindStrLabel = new Label();
        MySlider midWindGstSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 100
        };
        Label midWindGustLabel = new Label();
        MySlider midWindTrbSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 100
        };
        Label midWindTrbLabel = new Label();

        MySlider lowWindAltSlider = new MySlider
        {
            Value = 4000,
            Minimum = 0,
            Maximum = 15990
        };
        Label lowWindAltLabel = new Label();
        MySlider lowWindDirSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 360
        };
        Label lowWindDirLabel = new Label();
        MySlider lowWindStrSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 100
        };
        Label lowWindStrLabel = new Label();
        MySlider lowWindGstSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 100
        };
        Label lowWindGustLabel = new Label();
        MySlider lowWindTrbSlider = new MySlider
        {
            Value = 0,
            Minimum = 0,
            Maximum = 100
        };
        Label lowWindTrbLabel = new Label();

        public WxrWind()
        {
            InitializeComponent();
            DrawComponents();
        }

        private void DrawComponents()
        {
            //declaring main grid
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
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 2
            };

            //each subgrid is layer
            Grid topLayerDirectionGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = 15 },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 1
            };
            Grid topLayerStrGrid = new Grid
            {

                RowDefinitions =
                {
                    new RowDefinition { Height = 15 },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 1
            };

            Grid midLayerDirectionGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = 15 },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 1
            };
            Grid midLayerStrGrid = new Grid
            {

                RowDefinitions =
                {
                    new RowDefinition { Height = 15 },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 1
            };

            Grid lowLayerDirectionGrid = new Grid
            {
                RowDefinitions =
                {
                    new RowDefinition { Height = 15 },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 1
            };
            Grid lowLayerStrGrid = new Grid
            {

                RowDefinitions =
                {
                    new RowDefinition { Height = 15 },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                },
                ColumnSpacing = 1
            };

            Button setWxrButton = new Button
            {
                Text = "Set weather"
            };
            setWxrButton.Clicked += setWxrButton_Click;

            //adding slider and labels to subgrids
            #region topLayer

            topLayerDirectionGrid.Children.Add(topWindAltLabel, 0, 0);
            topLayerDirectionGrid.Children.Add(topWindAltSlider, 0, 1);
            topLayerDirectionGrid.Children.Add(topWindDirLabel, 1, 0);
            topLayerDirectionGrid.Children.Add(topWindDirSlider, 1, 1);

            topLayerStrGrid.Children.Add(topWindStrLabel, 0, 0);
            topLayerStrGrid.Children.Add(topWindStrSlider, 0, 1);
            topLayerStrGrid.Children.Add(topWindGustLabel, 1, 0);
            topLayerStrGrid.Children.Add(topWindGstSlider, 1, 1);
            topLayerStrGrid.Children.Add(topWindTrbLabel, 2, 0);
            topLayerStrGrid.Children.Add(topWindTrbSlider, 2, 1);

            #endregion

            #region midLayer

            midLayerDirectionGrid.Children.Add(midWindAltLabel, 0, 0);
            midLayerDirectionGrid.Children.Add(midWindAltSlider, 0, 1);
            midLayerDirectionGrid.Children.Add(midWindDirLabel, 1, 0);
            midLayerDirectionGrid.Children.Add(midWindDirSlider, 1, 1);

            midLayerStrGrid.Children.Add(midWindStrLabel, 0, 0);
            midLayerStrGrid.Children.Add(midWindStrSlider, 0, 1);
            midLayerStrGrid.Children.Add(midWindGustLabel, 1, 0);
            midLayerStrGrid.Children.Add(midWindGstSlider, 1, 1);
            midLayerStrGrid.Children.Add(midWindTrbLabel, 2, 0);
            midLayerStrGrid.Children.Add(midWindTrbSlider, 2, 1);

            #endregion

            #region lowLayer

            lowLayerDirectionGrid.Children.Add(lowWindAltLabel, 0, 0);
            lowLayerDirectionGrid.Children.Add(lowWindAltSlider, 0, 1);
            lowLayerDirectionGrid.Children.Add(lowWindDirLabel, 1, 0);
            lowLayerDirectionGrid.Children.Add(lowWindDirSlider, 1, 1);

            lowLayerStrGrid.Children.Add(lowWindStrLabel, 0, 0);
            lowLayerStrGrid.Children.Add(lowWindStrSlider, 0, 1);
            lowLayerStrGrid.Children.Add(lowWindGustLabel, 1, 0);
            lowLayerStrGrid.Children.Add(lowWindGstSlider, 1, 1);
            lowLayerStrGrid.Children.Add(lowWindTrbLabel, 2, 0);
            lowLayerStrGrid.Children.Add(lowWindTrbSlider, 2, 1);

            #endregion
            
            //adding layers to subgrids
            mainGrid.Children.Add(topLayerDirectionGrid, 0, 0);
            mainGrid.Children.Add(topLayerStrGrid, 0, 1);
            mainGrid.Children.Add(midLayerDirectionGrid, 0, 2);
            mainGrid.Children.Add(midLayerStrGrid, 0, 3);
            mainGrid.Children.Add(lowLayerDirectionGrid, 0, 4);
            mainGrid.Children.Add(lowLayerStrGrid, 0, 5);
            mainGrid.Children.Add(setWxrButton, 0, 6);

            topWindAltSlider.ValueChanged += UpdateSlidersPos;
            topWindDirSlider.ValueChanged += UpdateSlidersPos;
            topWindStrSlider.ValueChanged += UpdateSlidersPos;
            topWindGstSlider.ValueChanged += UpdateSlidersPos;
            topWindTrbSlider.ValueChanged += UpdateSlidersPos;

            midWindAltSlider.ValueChanged += UpdateSlidersPos;
            midWindDirSlider.ValueChanged += UpdateSlidersPos;
            midWindStrSlider.ValueChanged += UpdateSlidersPos;
            midWindGstSlider.ValueChanged += UpdateSlidersPos;
            midWindTrbSlider.ValueChanged += UpdateSlidersPos;

            lowWindAltSlider.ValueChanged += UpdateSlidersPos;
            lowWindDirSlider.ValueChanged += UpdateSlidersPos;
            lowWindStrSlider.ValueChanged += UpdateSlidersPos;
            lowWindGstSlider.ValueChanged += UpdateSlidersPos;
            lowWindTrbSlider.ValueChanged += UpdateSlidersPos;
            
            Content = mainGrid;
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            topWindAltLabel.Text = $"Wind alt - {Core.Current.GetStringFromValue((float)topWindAltSlider.doStep(), Atmosphere.Type.TWindLayer3Alt)}";
            topWindDirLabel.Text = $"Wind direction - {Core.Current.GetStringFromValue((float)topWindDirSlider.doStep(), Atmosphere.Type.TWindLayer3Dir)}";
            topWindStrLabel.Text = $"Wind strength - {Core.Current.GetStringFromValue((float)topWindDirSlider.doStep(), Atmosphere.Type.TWindLayer3Dir)}";
            topWindGustLabel.Text = $"Wind gusts - {Core.Current.GetStringFromValue((float)topWindGstSlider.doStep(), Atmosphere.Type.TWindLayer3Gusts)}";

            midWindAltLabel.Text = $"Wind alt - {Core.Current.GetStringFromValue((float)midWindAltSlider.doStep(), Atmosphere.Type.TWindLayer2Alt)}";
            midWindDirLabel.Text = $"Wind direction - {Core.Current.GetStringFromValue((float)midWindDirSlider.doStep(), Atmosphere.Type.TWindLayer2Dir)}";
            midWindStrLabel.Text = $"Wind strength - {Core.Current.GetStringFromValue((float)midWindDirSlider.doStep(), Atmosphere.Type.TWindLayer2Dir)}";
            midWindGustLabel.Text = $"Wind gusts - {Core.Current.GetStringFromValue((float)midWindGstSlider.doStep(), Atmosphere.Type.TWindLayer2Gusts)}";

            lowWindAltLabel.Text = $"Wind alt - {Core.Current.GetStringFromValue((float)lowWindAltSlider.doStep(), Atmosphere.Type.TWindLayer1Alt)}";
            lowWindDirLabel.Text = $"Wind direction - {Core.Current.GetStringFromValue((float)lowWindDirSlider.doStep(), Atmosphere.Type.TWindLayer1Dir)}";
            lowWindStrLabel.Text = $"Wind strength - {Core.Current.GetStringFromValue((float)lowWindDirSlider.doStep(), Atmosphere.Type.TWindLayer1Dir)}";
            lowWindGustLabel.Text = $"Wind gusts - {Core.Current.GetStringFromValue((float)lowWindGstSlider.doStep(), Atmosphere.Type.TWindLayer1Gusts)}";
        }

        private void UpdateSlidersPos(object sender, EventArgs e)
        {
            //low layer
            //lowWindAltSlider.Maximum = midWindAltSlider.Value - 10;
            //Core.Current.WindLayer1.Altitude = (float)lowWindAltSlider.Value;
            //Core.Current.WindLayer1.Direction = (float)lowWindDirSlider.Value;
            //Core.Current.WindLayer1.Strength = (float)lowWindStrSlider.Value;
            //if (lowWindGstSlider.Value > lowWindStrSlider.Value - 100)
            //{
            //    lowWindGstSlider.Value = lowWindStrSlider.Value - 100;
            //}
            //lowWindGstSlider.Maximum = 101 - lowWindStrSlider.Value;
            //Core.Current.WindLayer1.Gusts = (float)lowWindGstSlider.Value;

            ////mid layer
            //midWindAltSlider.Maximum = topWindAltSlider.Value - 10;
            //if (midWindAltSlider.Value < lowWindAltSlider.Value)
            //    midWindAltSlider.Value = lowWindAltSlider.Value + 10;
            //midWindAltSlider.Minimum = lowWindAltSlider.Value + 10;
            //Core.Current.WindLayer2.Altitude = (float)midWindAltSlider.Value;
            //Core.Current.WindLayer2.Direction = (float)midWindDirSlider.Value;
            //Core.Current.WindLayer2.Strength = (float)midWindStrSlider.Value;
            //if (midWindGstSlider.Value > midWindStrSlider.Value - 100)
            //{
            //    midWindGstSlider.Value = midWindStrSlider.Value - 100;
            //}
            //midWindGstSlider.Maximum = 101 - midWindStrSlider.Value;
            //Core.Current.WindLayer2.Gusts = (float)midWindGstSlider.Value;

            ////top layer
            //if (topWindAltSlider.Value < midWindAltSlider.Value)
            //    topWindAltSlider.Value = midWindAltSlider.Value + 10;
            //midWindAltSlider.Minimum = lowWindAltSlider.Value + 10;
            //Core.Current.WindLayer3.Altitude = (float)topWindAltSlider.Value;
            //Core.Current.WindLayer3.Direction = (float)topWindDirSlider.Value;
            //XplaneMaster.Core.Current.WindLayer3.Strength = (float)topWindStrSlider.Value;
            //if (topWindGstSlider.Value > topWindStrSlider.Value - 100)
            //{
            //    topWindGstSlider.Value = topWindStrSlider.Value - 100;
            //}
            //topWindGstSlider.Maximum = 101 - topWindStrSlider.Value;
            //XplaneMaster.Core.Current.WindLayer3.Gusts = (float)topWindGstSlider.Value;

            UpdateLabels();
        }

        private void setWxrButton_Click(object sender, EventArgs e)
        {
            //UpdateSlidersPos(sender, e);
            Core.SendWeather();
        }
    }
}