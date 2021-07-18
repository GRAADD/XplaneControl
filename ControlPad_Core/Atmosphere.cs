using System;

namespace XplaneControl
{
    public class Atmosphere
    {
        public enum Type
        {
            TDate,
            TTime,
            TVisibility,
            TRain,
            TStorm,
            TRunWayStat,
            TRunWayPatches,
            TTemperature,
            TBaro,
            TTermCov,
            TTermStr,
            TWindLayer3Alt,
            TWindLayer2Alt,
            TWindLayer1Alt,
            TWindLayer3Dir,
            TWindLayer2Dir,
            TWindLayer1Dir,
            TWindLayer3Str,
            TWindLayer2Str,
            TWindLayer1Str,
            TWindLayer3Gusts,
            TWindLayer2Gusts,
            TWindLayer1Gusts,
            TWindLayer3Turb,
            TWindLayer2Turb,
            TWindLayer1Turb,
            TCloudsLayer3Top,
            TCloudsLayer2Top,
            TCloudsLayer1Top,
            TCloudsLayer3Base,
            TCloudsLayer2Base,
            TCloudsLayer1Base,
            TCloudsLayer3Cat,
            TCloudsLayer2Cat,
            TCloudsLayer1Cat
        }
        public float Date;
        public float Timex;
        public float Visibility;
        public float Rain;
        public float Storm;
        public enum RwStat
        {
            dry,
            damp,
            wet
        }
        public RwStat RunWayStat;

        public bool RunWayPatches;

        public float temperature;
        public float baro;
        public float termCov;
        public float termStr;
        public CloudsLayer CloudsLayer3;
        public CloudsLayer CloudsLayer2;
        public CloudsLayer CloudsLayer1;
        public WindLayer WindLayer3;
        public WindLayer WindLayer2;
        public WindLayer WindLayer1;

        public struct CloudsLayer
        {
            public enum CloudsCat
            {
                ClearClouds,
                CirrusClouds,
                FewClouds,
                ScatteredClouds,
                BrokenClouds,
                OvercastClouds,
                StratusClouds
            }

            public CloudsCat CategoryClouds;
            public float BaseHight;
            public float TopHight;

            public float GetCatNum()
            {
                int cat = 0;
                switch (CategoryClouds)
                {
                    case CloudsCat.ClearClouds:
                        cat = 0;
                        break;
                    case CloudsCat.CirrusClouds:
                        cat = 1;
                        break;
                    case CloudsCat.FewClouds:
                        cat = 2;
                        break;
                    case CloudsCat.ScatteredClouds:
                        cat = 3;
                        break;
                    case CloudsCat.BrokenClouds:
                        cat = 4;
                        break;
                    case CloudsCat.OvercastClouds:
                        cat = 5;
                        break;
                    case CloudsCat.StratusClouds:
                        cat = 6;
                        break;
                }
                return cat;
            }
            
            public byte[] GetCatByte()
            {
                int cat = 0;
                
                switch (CategoryClouds)
                {
                    case CloudsCat.ClearClouds:
                        return new byte[]{0, 0, 0, 0};
                    case CloudsCat.CirrusClouds:
                        return new byte[]{1, 0, 0, 0};
                    case CloudsCat.FewClouds:
                        return new byte[]{2, 0, 0, 0};
                    case CloudsCat.ScatteredClouds:
                        return new byte[]{3, 0, 0, 0};
                    case CloudsCat.BrokenClouds:
                        return new byte[]{4, 0, 0, 0};
                    case CloudsCat.OvercastClouds:
                        return new byte[]{5, 0, 0, 0};
                    case CloudsCat.StratusClouds:
                        return new byte[]{6, 0, 0, 0};
                }
                return new byte[]{0, 0, 0, 0};
            }

            public string GetCatString()
            {
                string catString = "";
                switch (CategoryClouds)
                {
                    case CloudsCat.ClearClouds:
                        catString = "Clear Clouds";
                        break;
                    case CloudsCat.CirrusClouds:
                        catString = "Cirrus Clouds";
                        break;
                    case CloudsCat.FewClouds:
                        catString = "Few Clouds";
                        break;
                    case CloudsCat.ScatteredClouds:
                        catString = "Scattered Clouds";
                        break;
                    case CloudsCat.BrokenClouds:
                        catString = "Broken Clouds";
                        break;
                    case CloudsCat.OvercastClouds:
                        catString = "Overcast Clouds";
                        break;
                    case CloudsCat.StratusClouds:
                        catString = "Stratus Clouds";
                        break;
                }
                return catString;
            }

        }

        public CloudsLayer.CloudsCat GetCloudsCat(string cat)
        {
            CloudsLayer.CloudsCat cloudsCat = CloudsLayer.CloudsCat.ClearClouds;
            switch (cat)
            {
                case "Clear Clouds":
                    cloudsCat = CloudsLayer.CloudsCat.ClearClouds;
                    break;
                case "Cirrus Clouds":
                    cloudsCat = CloudsLayer.CloudsCat.CirrusClouds;
                    break;
                case "Few Clouds":
                    cloudsCat = CloudsLayer.CloudsCat.FewClouds;
                    break;
                case "Scattered Clouds":
                    cloudsCat = CloudsLayer.CloudsCat.ScatteredClouds;
                    break;
                case "Broken Clouds":
                    cloudsCat = CloudsLayer.CloudsCat.BrokenClouds;
                    break;
                case "OvercastClouds":
                    cloudsCat = CloudsLayer.CloudsCat.OvercastClouds;
                    break;
                case "StratusClouds":
                    cloudsCat = CloudsLayer.CloudsCat.StratusClouds;
                    break;
            }

            return cloudsCat;
        }
        public CloudsLayer.CloudsCat GetCloudsCat(float cat)
        {
            CloudsLayer.CloudsCat cloudsCat = CloudsLayer.CloudsCat.ClearClouds;
            switch (cat)
            {
                case 0:
                    cloudsCat = CloudsLayer.CloudsCat.ClearClouds;
                    break;
                case 1:
                    cloudsCat = CloudsLayer.CloudsCat.CirrusClouds;
                    break;
                case 2:
                    cloudsCat = CloudsLayer.CloudsCat.FewClouds;
                    break;
                case 3:
                    cloudsCat = CloudsLayer.CloudsCat.ScatteredClouds;
                    break;
                case 4:
                    cloudsCat = CloudsLayer.CloudsCat.BrokenClouds;
                    break;
                case 5:
                    cloudsCat = CloudsLayer.CloudsCat.OvercastClouds;
                    break;
                case 6:
                    cloudsCat = CloudsLayer.CloudsCat.StratusClouds;
                    break;
            }
            return cloudsCat;
        }

        public string GetCloudsStringFromCat(CloudsLayer.CloudsCat cat)
        {
            switch (cat)
            {
                case CloudsLayer.CloudsCat.ClearClouds:
                    return "Clear Clouds";
                case CloudsLayer.CloudsCat.CirrusClouds:
                    return "Cirrus Clouds";
                case CloudsLayer.CloudsCat.FewClouds:
                    return "Few Clouds";
                case CloudsLayer.CloudsCat.ScatteredClouds:
                    return "Scattered Clouds";
                case CloudsLayer.CloudsCat.BrokenClouds:
                    return "Broken Clouds";
                case CloudsLayer.CloudsCat.OvercastClouds:
                    return "Overcast Clouds";
                case CloudsLayer.CloudsCat.StratusClouds:
                    return "Stratus Clouds";
            }
            return "";
        }

        public struct WindLayer
        {
            public float Altitude;
            public float Direction;
            public float Strength;
            public float Gusts;
            public float Turbulence;
        }

        public string GetStringFromValue(float value, Type type)
        {
            switch (type)
            {
                case Type.TDate:
                    Date = value;
                    int daystoadd = (int)value;
                    int year = DateTime.Now.Year;
                    DateTime dateTime = new DateTime(year, 1, 1);
                    dateTime = dateTime.AddDays(daystoadd);
                    return dateTime.ToShortDateString();

                case Type.TTime:
                    Timex = value / 10;
                    return Timex.ToString();


                case Type.TVisibility:
                    Visibility = value / 1000;
                    if (Visibility < 1)
                        return $"{Visibility * 6076} ft";
                    else
                        return $"{Visibility} sm";

                case Type.TRain:
                    Rain = value;
                    return $"{Rain}%";
                case Type.TStorm:
                    Storm = value;
                    return $"{value}%";
                case Type.TTemperature:
                    temperature = value;
                    return $"{value} °C";
                case Type.TBaro:
                    baro = value / 100;
                    return $"{baro} inch";

                case Type.TTermCov:
                    termCov = value / 100;
                    return $"{termCov} rat";
                case Type.TTermStr:
                    termStr = value / 100;
                    return $"{termStr} fpm";


                case Type.TWindLayer3Alt:
                    WindLayer1.Altitude = value;
                    return $"{value} ft";
                case Type.TWindLayer2Alt:
                    WindLayer2.Altitude = value;
                    return $"{value} ft";
                case Type.TWindLayer1Alt:
                    WindLayer3.Altitude = value;
                    return $"{value} ft";

                case Type.TWindLayer3Dir:
                    WindLayer1.Direction = value;
                    return $"{value}deg";
                case Type.TWindLayer2Dir:
                    WindLayer2.Direction = value;
                    return $"{value}deg";
                case Type.TWindLayer1Dir:
                    WindLayer3.Direction = value;
                    return $"{value}deg";

                case Type.TWindLayer3Str:
                    WindLayer1.Strength = value;
                    return $"{value} ft/m";
                case Type.TWindLayer2Str:
                    WindLayer2.Strength = value;
                    return $"{value} ft/m";
                case Type.TWindLayer1Str:
                    WindLayer3.Strength = value;
                    return $"{value} ft/m";

                case Type.TWindLayer3Gusts:
                    WindLayer1.Gusts = value - WindLayer1.Strength;
                    return $"{value} ft/m";
                case Type.TWindLayer2Gusts:
                    WindLayer2.Gusts = value - WindLayer2.Strength;
                    return $"{value} ft/m";
                case Type.TWindLayer1Gusts:
                    WindLayer3.Gusts = value - WindLayer3.Strength;
                    return $"{value} ft/m";

                case Type.TWindLayer3Turb:
                    WindLayer1.Turbulence = value;
                    return $"{value}%";
                case Type.TWindLayer2Turb:
                    WindLayer2.Turbulence = value;
                    return $"{value}%";
                case Type.TWindLayer1Turb:
                    WindLayer3.Turbulence = value;
                    return $"{value}%";


                case Type.TCloudsLayer3Top:
                    CloudsLayer3.TopHight = value;
                    return $"{value} ft";
                case Type.TCloudsLayer2Top:
                    CloudsLayer2.TopHight = value;
                    return $"{value} ft";
                case Type.TCloudsLayer1Top:
                    CloudsLayer1.TopHight = value;
                    return $"{value} ft";

                case Type.TCloudsLayer3Base:
                    CloudsLayer3.BaseHight = value;
                    return $"{value} ft";
                case Type.TCloudsLayer2Base:
                    CloudsLayer2.BaseHight = value;
                    return $"{value} ft";
                case Type.TCloudsLayer1Base:
                    CloudsLayer1.BaseHight = value;
                    return $"{value} ft";

                case Type.TCloudsLayer1Cat:
                    CloudsLayer1.CategoryClouds = GetCloudsCat(value);
                    return GetCloudsStringFromCat(GetCloudsCat(value));
                case Type.TCloudsLayer2Cat:
                    CloudsLayer2.CategoryClouds = GetCloudsCat(value);
                    return GetCloudsStringFromCat(GetCloudsCat(value));
                case Type.TCloudsLayer3Cat:
                    CloudsLayer3.CategoryClouds = GetCloudsCat(value);
                    return GetCloudsStringFromCat(GetCloudsCat(value));
                default:
                    return "";
            }
        }

        public string GetStringFromValue(string value, Type type)
        {
            switch (type)
            {
                case Type.TCloudsLayer1Cat:
                    CloudsLayer1.CategoryClouds = GetCloudsCat(value);
                    return "";
                case Type.TCloudsLayer2Cat:
                    CloudsLayer2.CategoryClouds = GetCloudsCat(value);
                    return "";
                case Type.TCloudsLayer3Cat:
                    CloudsLayer3.CategoryClouds = GetCloudsCat(value);
                    return "";
                default:
                    return "";
            }
        }

        public float GetCatNum(string cat)
        {
            int output = 0;
            switch (cat)
            {
                case "Clear Clouds":
                    output = 0;
                    break;
                case "Cirrus Clouds":
                    output = 1;
                    break;
                case "Few Clouds":
                    output = 2;
                    break;
                case "Scattered Clouds":
                    output = 3;
                    break;
                case "Broken Clouds":
                    output = 4;
                    break;
                case "Overcast Clouds":
                    output = 5;
                    break;
                case "Stratus Clouds":
                    output = 6;
                    break;
            }
            return output;
        }

        public string GetCatString(int cat)
        {
            string catString = "";
            switch (cat)
            {
                case 0:
                    catString = "Clear Clouds";
                    break;
                case 1:
                    catString = "Cirrus Clouds";
                    break;
                case 2:
                    catString = "Few Clouds";
                    break;
                case 3:
                    catString = "Scattered Clouds";
                    break;
                case 4:
                    catString = "Broken Clouds";
                    break;
                case 5:
                    catString = "Overcast Clouds";
                    break;
                case 6:
                    catString = "Stratus Clouds";
                    break;
            }
            return catString;
        }
    }
}