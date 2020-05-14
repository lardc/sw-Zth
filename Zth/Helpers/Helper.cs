using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Zth.Helpers
{
    public static class Helper
    {
        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;

            var childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (var i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (!(child is T childType))
                {
                    foundChild = FindChild<T>(child, childName);
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    if (!(child is FrameworkElement frameworkElement) || frameworkElement.Name != childName) continue;
                    foundChild = (T)child;
                    break;
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }
        
        public static Size MeasureString(string candidate, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize )
        {
            var formattedText = new FormattedText(candidate,CultureInfo.CurrentCulture,FlowDirection.LeftToRight,new Typeface(fontFamily, fontStyle, fontWeight, fontStretch),fontSize,Brushes.Black,new NumberSubstitution(),1);
            return new Size(formattedText.Width, formattedText.Height);
        }
        

        
    }
}