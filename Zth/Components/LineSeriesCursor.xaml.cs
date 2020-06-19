using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Zth.Components
{
    /// <summary>
    /// Логика взаимодействия для BottomCheckBoxsAndTable.xaml
    /// </summary>
    public partial class LineSeriesCursor : StackPanel
    {
        public LineSeriesCursor()
        {
            InitializeComponent();
        }

        public Point PositionCenterFromChart
        { 
            get
            {
                var res = TranslatePoint(new Point(0, 0), CartesianChart);
                res.X -= Rectangle.ActualWidth;
                return res;
            }
        }


        public LiveCharts.Wpf.CartesianChart CartesianChart
        {
            get { return (LiveCharts.Wpf.CartesianChart)GetValue(CartesianChartProperty); }
            set { SetValue(CartesianChartProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CartesianChart.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CartesianChartProperty =
            DependencyProperty.Register("CartesianChart", typeof(LiveCharts.Wpf.CartesianChart), typeof(LineSeriesCursor), new PropertyMetadata(null));



        private Point? _oldPosition;

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _oldPosition = e.GetPosition((IInputElement)sender);
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_oldPosition != null)
            {
                var left = Extentions.ConvertToPixels(CartesianChart, new Point(CartesianChart.AxisX.First().MinValue, 0), 0, 0).X - ActualWidth / 2;
                var right = Extentions.ConvertToPixels(CartesianChart, new Point(CartesianChart.AxisX.First().MaxValue, 0), 0, 0).X - ActualWidth / 2;

                var newMargin= new Thickness(Margin.Left + (e.GetPosition((IInputElement)sender) - _oldPosition).Value.X, 0, 0, 0);
                if (newMargin.Left < left)
                    Margin = new Thickness(left, 0, 0, 0);
                else if (newMargin.Left > right)
                    Margin = new Thickness(right, 0, 0, 0);
                else
                    Margin = newMargin;
            }

        }

        private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _oldPosition = null;
        }

        private void StackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Rectangle.Height = ActualHeight;
        }

        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            _oldPosition = null;
        }
    }
}
