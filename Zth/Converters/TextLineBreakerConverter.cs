using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Zth.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    public sealed class TextLineBreakerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var str = (string)parameter ;

            var i1 = str.IndexOf(' ', str.Length / 2);
            var i2 = str.Reverse().ToList().IndexOf(' ', str.Length / 2);

            if (i1 == i2)
                return str.Remove(i1, 1).Insert(i1, Environment.NewLine);

            if (i2 == -1)
                return str.Remove(i1, 1).Insert(i1, Environment.NewLine);

            if ((str.Length - i1 >= str.Length - i2) && i1 != -1) 
                return str.Remove(i1, 1).Insert(i1, Environment.NewLine);
            
            i1 = str.Length - i2 - 1;
            
            return str.Remove(i1, 1).Insert(i1, Environment.NewLine);
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("TextLineBreakerConverter ConvertBack");
        }
    }
}
