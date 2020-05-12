using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Zth
{
    public partial class ComboBoxHelper
    {
      

        private void ComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            Debug.Assert(comboBox != null, nameof(comboBox) + " != null");
            if (comboBox.Items.Count == 0)
                return;

            var item = comboBox.Items[0];
            var genericTypeProperty = item.GetType().GetProperty("Value");
            
            var contentPresenter = Helper.FindChild<ContentPresenter>(sender as DependencyObject, "contentPresenter");
            var textBlock = VisualTreeHelper.GetChild(contentPresenter, 0);

            var textSize = comboBox.ItemsSource.Cast<object>().Select(m =>
            {
                Debug.Assert(genericTypeProperty != null, nameof(genericTypeProperty) + " != null");
                return (string) genericTypeProperty.GetValue(m);
            }).Max(m=> Helper.MeasureString(m,
                (FontFamily)textBlock.GetValue(TextBlock.FontFamilyProperty),
                (FontStyle)textBlock.GetValue(TextBlock.FontStyleProperty),
                (FontWeight)textBlock.GetValue(TextBlock.FontWeightProperty),
                (FontStretch)textBlock.GetValue(TextBlock.FontStretchProperty),
                (double)textBlock.GetValue(TextBlock.FontSizeProperty)).Width);

            textBlock.SetValue(FrameworkElement.MinWidthProperty, textSize);
        }

     
    }
}
