using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Zth
{
    /// <summary>
    /// Логика взаимодействия для TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        public SeriesCollection SeriesCollection { get; set; }
        public Func<double, string> Formatter { get; set; }
        public double Base { get; set; }

        public TestWindow()
        {
            InitializeComponent();

            Base = 10;

            var mapper = Mappers.Xy<ObservablePoint>()
                .X(point => Math.Log(point.X, Base)) //a 10 base log scale in the X axis
                .Y(point => point.Y);

            SeriesCollection = new SeriesCollection(mapper)
            {
                new LineSeries
                {
                    Values = new ChartValues<ObservablePoint>
                    {
                        new ObservablePoint(0.00001, 2),
                        new ObservablePoint(0.0005, 5),
                        new ObservablePoint(0.001, 1),
                        new ObservablePoint(0.01, 3),
                        new ObservablePoint(0.1, 8),
                        new ObservablePoint(1, 5),

                    }
                }
            };

            Formatter = value => Math.Pow(Base, value).ToString();

            DataContext = this;
        }
    }
}
