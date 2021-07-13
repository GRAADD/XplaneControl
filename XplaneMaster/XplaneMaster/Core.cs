using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using XplaneControl;
using XplaneMaster.CommandPages;

namespace XplaneMaster
{
    public static class Core
    {
        public static Weather CurrentWeather;
        public static Thread UdpThread;
        public static Thread TcpThread;
        public static bool MasterConnected = false;
        public static Style SliderStyle = new Style(typeof(Slider))
        {
            Setters =
            {
                new Setter()
                {
                    Property = Slider.BackgroundColorProperty,
                    Value = Color.CornflowerBlue
                },
                new Setter()
                {
                    Property = Slider.ThumbColorProperty,
                    Value = Color.White
                }
            }
        };
        
        public static Atmosphere Current = new Atmosphere
        {
            Date = 182,
            Timex = 12,
            Visibility = 20,
            Rain = 0,
            Storm = 0,
            RunWayStat = Atmosphere.RwStat.dry,
            RunWayPatches = false,
            temperature = 19,
            baro = 29.92f,
            termCov = 0,
            termStr = 0,
            CloudsLayer3 = new Atmosphere.CloudsLayer
            {
                CategoryClouds = Atmosphere.CloudsLayer.CloudsCat.ClearClouds,
                BaseHight = 24000,
                TopHight = 26000
            },
            CloudsLayer2 = new Atmosphere.CloudsLayer
            {
                CategoryClouds = Atmosphere.CloudsLayer.CloudsCat.ClearClouds,
                BaseHight = 14000,
                TopHight = 16000
            },
            CloudsLayer1 = new Atmosphere.CloudsLayer
            {
                CategoryClouds = Atmosphere.CloudsLayer.CloudsCat.ClearClouds,
                BaseHight = 5000,
                TopHight = 7000
            },
            WindLayer1 = new Atmosphere.WindLayer
            {
                Altitude = 20000,
                Direction = 0,
                Strength = 0,
                Gusts = 0,
                Turbulence = 0
            },
            WindLayer2 = new Atmosphere.WindLayer
            {
                Altitude = 8000,
                Direction = 0,
                Strength = 0,
                Gusts = 0,
                Turbulence = 0
            },
            WindLayer3 = new Atmosphere.WindLayer
            {
                Altitude = 4000,
                Direction = 0,
                Strength = 0,
                Gusts = 0,
                Turbulence = 0
            }
        };

        public static void CavokWeather()
        {
            Current = new Atmosphere
            {
                Date = 182,
                Timex = 12,
                Visibility = 20,
                Rain = 0,
                Storm = 0,
                RunWayStat = Atmosphere.RwStat.dry,
                RunWayPatches = false,
                temperature = 19,
                baro = 29.92f,
                termCov = 0,
                termStr = 0,
                CloudsLayer3 = new Atmosphere.CloudsLayer
                {
                    CategoryClouds = Atmosphere.CloudsLayer.CloudsCat.ClearClouds,
                    BaseHight = 24000,
                    TopHight = 26000
                },
                CloudsLayer2 = new Atmosphere.CloudsLayer
                {
                    CategoryClouds = Atmosphere.CloudsLayer.CloudsCat.ClearClouds,
                    BaseHight = 14000,
                    TopHight = 16000
                },
                CloudsLayer1 = new Atmosphere.CloudsLayer
                {
                    CategoryClouds = Atmosphere.CloudsLayer.CloudsCat.ClearClouds,
                    BaseHight = 2000,
                    TopHight = 4000
                },
                WindLayer1 = new Atmosphere.WindLayer
                {
                    Altitude = 20000,
                    Direction = 0,
                    Strength = 0,
                    Gusts = 0,
                    Turbulence = 0
                },
                WindLayer2 = new Atmosphere.WindLayer
                {
                    Altitude = 8000,
                    Direction = 0,
                    Strength = 0,
                    Gusts = 0,
                    Turbulence = 0
                },
                WindLayer3 = new Atmosphere.WindLayer
                {
                    Altitude = 4000,
                    Direction = 0,
                    Strength = 0,
                    Gusts = 0,
                    Turbulence = 0
                }
            };
            SendWeather();
        }

        public static void ThunderWeather()
        {
            Current = new Atmosphere
            {
                Date = 182,
                Timex = 20,
                Visibility = 7,
                Rain = 0,
                Storm = 80,
                RunWayStat = Atmosphere.RwStat.damp,
                RunWayPatches = false,
                temperature = 26,
                baro = 29.92f,
                termCov = 0,
                termStr = 0,
                CloudsLayer3 = new Atmosphere.CloudsLayer
                {
                    CategoryClouds = Atmosphere.CloudsLayer.CloudsCat.ClearClouds,
                    BaseHight = 24000,
                    TopHight = 26000
                },
                CloudsLayer2 = new Atmosphere.CloudsLayer
                {
                    CategoryClouds = Atmosphere.CloudsLayer.CloudsCat.ClearClouds,
                    BaseHight = 20000,
                    TopHight = 22000
                },
                CloudsLayer1 = new Atmosphere.CloudsLayer
                {
                    CategoryClouds = Atmosphere.CloudsLayer.CloudsCat.ScatteredClouds,
                    BaseHight = 5000,
                    TopHight = 18000
                },
                WindLayer1 = new Atmosphere.WindLayer
                {
                    Altitude = 20000,
                    Direction = 0,
                    Strength = 0,
                    Gusts = 0,
                    Turbulence = 1
                },
                WindLayer2 = new Atmosphere.WindLayer
                {
                    Altitude = 8000,
                    Direction = 120,
                    Strength = 10,
                    Gusts = 5,
                    Turbulence = 10
                },
                WindLayer3 = new Atmosphere.WindLayer
                {
                    Altitude = 4000,
                    Direction = 0,
                    Strength = 0,
                    Gusts = 0,
                    Turbulence = 4
                }
            };
            SendWeather();
        }
        public static void SendWeather()
        {
            ConnectionUdp.SendMessage(CommandEncoder.MakeWeatherBytes(Current));
        }

    }
}
