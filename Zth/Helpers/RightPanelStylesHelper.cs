using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Zth.Helpers
{
    public partial class RightPanelStylesHelper
    {
        private void TextBox_Loaded(object sender, RoutedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var bindingText = textBox.GetBindingExpression(TextBox.TextProperty)?.ParentBinding ;
            if (bindingText == null)
                return;

            if (bindingText.ConverterParameter is string == false)
                return;

            var textSize = Helper.MeasureString((string)bindingText.ConverterParameter,
                (FontFamily)textBox.GetValue(TextBlock.FontFamilyProperty),
                (FontStyle)textBox.GetValue(TextBlock.FontStyleProperty),
                (FontWeight)textBox.GetValue(TextBlock.FontWeightProperty),
                (FontStretch)textBox.GetValue(TextBlock.FontStretchProperty),
                (double)textBox.GetValue(TextBlock.FontSizeProperty)).Width;

            var border = Helper.FindChild<Border>(textBox, "border");
            var textboxView = ((border.Child as ScrollViewer).Content as FrameworkElement);
            var textboxViewMargin = (Thickness)textboxView.GetValue(FrameworkElement.MarginProperty);
            
            textSize += textBox.Padding.Left + textBox.Padding.Right + textBox.BorderThickness.Left + textBox.BorderThickness.Right + textboxViewMargin.Left + textboxViewMargin.Right;

            textBox.SetValue(FrameworkElement.MinWidthProperty, textSize);
        }

     
    }
}
