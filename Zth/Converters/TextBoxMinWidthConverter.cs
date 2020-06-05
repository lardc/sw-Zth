using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Zth.Helpers;

namespace Zth.Converters
{
    [ValueConversion(typeof(TextBox), typeof(double))]
    public sealed class TextBoxMinWidthConverter : IValueConverter
    {

        public object Convert(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            var textBox = value as TextBox;

            var textSize = Helper.MeasureString((string)parameter,
                (FontFamily)textBox.GetValue(TextBlock.FontFamilyProperty),
                (FontStyle)textBox.GetValue(TextBlock.FontStyleProperty),
                (FontWeight)textBox.GetValue(TextBlock.FontWeightProperty),
                (FontStretch)textBox.GetValue(TextBlock.FontStretchProperty),
                (double)textBox.GetValue(TextBlock.FontSizeProperty)).Width;

            return textSize;
        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("TextBoxMinWidthConverter ConvertBack");
        }
    }
}
