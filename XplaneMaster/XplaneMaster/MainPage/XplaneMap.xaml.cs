using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using GMap.NET;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using MapType = Xamarin.Forms.Maps.MapType;

namespace XplaneMaster.MainPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class XplaneMap : ContentPage
    {
        private Map mapControl;
        private bool drawed = false;
        public XplaneMap()
        {
            InitializeComponent();
            DrawContent();
        }

        private void DrawContent()
        {
            mapControl = new Map
            {
                BackgroundColor = Color.Cyan,
                MapType = MapType.Hybrid,
            };
            //GMaps Maps = new GMaps();
            //GMaps.Instance.Mode = AccessMode.ServerAndCache;
            //// choose your provider here
            //Maps.MapProvider = GMap.NET.MapProviders.OpenStreetMapProvider.Instance;
            //Maps.MinZoom = 2;
            //Maps.MaxZoom = 17;
            //// whole world zoom
            //mapView.Zoom = 2;
            //// lets the map use the mousewheel to zoom
            //mapView.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            //// lets the user drag the map
            //mapView.CanDragMap = true;
            //// lets the user drag the map with the left mouse button
            //mapView.DragButton = MouseButton.Left;
            Content = new StackLayout
            {
                Children =
                {
                    mapControl
                },
                Spacing = 3
            };
            drawed = true;
            new Thread(UpdatePosition).Start();
        }

        public void UpdatePosition()
        {
            while (true)
            {
                if (drawed)
                {
                    Position position = new Position(XplaneControl.Position.Lat, XplaneControl.Position.Lng);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(0.444));
                    mapControl.MoveToRegion(mapSpan);
                }
            }
        }
    }
}