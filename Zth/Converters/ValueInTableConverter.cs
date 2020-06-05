using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace Zth.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    public class ValueInTableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
          object parameter, CultureInfo culture)
        {
            bool isEnabled = (bool)parameter;
            if (isEnabled)
                return "-";
            return ((double)value).ToString("F1");
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("TextLineBreakerConverter ConvertBack");
        }
    }
}
