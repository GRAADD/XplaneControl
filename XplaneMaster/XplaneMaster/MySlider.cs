using System;
using Xamarin.Forms;

namespace XplaneMaster
{
    class MySlider : Slider
    {
        public string Tag = "";
        public int Step = 1;
        //private double _value;
        //public override double Value
        //{
        //    set => _value = (value % Step);
        //    get => _value;
        //}
        public double doStep()
        {
            //Value = ;
            return Math.Round(Value);
        }
    }
}