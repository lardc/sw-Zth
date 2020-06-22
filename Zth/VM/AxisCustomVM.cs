using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Zth.VM
{
    public class AxisCustomVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public double Base { get; set; }
        public double MinValue { get; set; }
        public double MaxValue { get; set; }
        public double Step { get; set; }
        public string Name { get; set; }
        public Func<double, string> StringFormat { get; set; }
        public Func<double, string> FormatterLogarithmic => value =>
        {
            int count = (int)((MaxValue - MinValue) / Step);
            if (Math.Abs(value - MaxValue) <= double.Epsilon)
                return Name;
            else
                return Math.Pow(Base, value).ToString("0.#####");
        };
        public Func<double, string> Formatter => value =>
         {
             int count = (int)Math.Round((MaxValue - MinValue) / Step);
             if (Math.Round(Math.Abs((value - MinValue) / Step)) == count)
                 return Name;
             if (Math.Round((value - MinValue) / Step) % 2 == 0)
                 return StringFormat(value); 
             if (Math.Round((value - MinValue) / Step) % 2 == 1)
                 return string.Empty;
             return StringFormat(value); 
         };
    }
}
