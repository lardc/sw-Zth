using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public string[] Labels { get; set; } = new string[] { "55", "60", "70" , "80" ,"90" ,"c"  };

        public Func<double, string> FormatterDegreesCelsius { get; set; } = value =>
        {
            if (value == 95)
                return Properties.Resource.UnitMeasurementDegreeCentigrade;
            else if (new double[] { 60, 70, 80, 90 }.Contains(value))
                return value.ToString();
            return string.Empty;
        };

        public Func<double, string> FormatterMegawatts { get; set; } = value =>
        {
            if (value == 450)
                return "мВ";
            else if (new double[] { 370, 390, 410, 430 }.Contains(value))
                return value.ToString();
            return string.Empty;
        };

        public Func<double, string> FormatterKilowatts { get; set; } = value =>
        {
            if (value == 510)
                return "кВ";
            else if (new double[] { 490, 495, 500, 505 }.Contains(value))
                return value.ToString();
            return string.Empty;
        };

        public Func<double, string> FormatterAmperes { get; set; } = value =>
        {
            if (value == 1550)
                return "А";
            else if (new double[] { 1200, 1300, 1400, 1500 }.Contains(value))
                return value.ToString();
            return string.Empty;
        };

        public Func<double, string> FormatterTimes { get; set; } = value =>
        {
            if (value > 0)
                return "сек";
            else
                return new Dictionary<double, string>()
                {
                    {-5, "0,00001" },
                    {-4, "0,0001" },
                    {-3, "0,001" },
                    {-2, "0,01" },
                    {-1, "0,1" },
                    {0, "1" },
                }[value];
        };

        public TestWindow()
        {
            Base = 10;
            //FormatterTimes = value => value > 0 ? "сек" : Math.Pow(Base, value).ToString("N5");

            //Labels.Reverse();
            InitializeComponent();

            

            var mapper = Mappers.Xy<ObservablePoint>()
                .X(point => Math.Log(point.X, Base)) //a 10 base log scale in the X axis
                .Y(point => point.Y);

            var MariaSeries = new LineSeries
            {
                Values = new ChartValues<ObservablePoint>
                {
                    new ObservablePoint(0.00001, 60),
                    new ObservablePoint(0.00003, 71),
                    new ObservablePoint(0.00008, 57),
                    /*               new ObservablePoint(0.0005, 5),
                                   new ObservablePoint(0.001, 1),
                                   new ObservablePoint(0.01, 3),
                                   new ObservablePoint(0.1, 8),*/
                    /*new ObservablePoint(1, 5),*/

                },
                PointGeometrySize = 0,
                Fill = System.Windows.Media.Brushes.Transparent,
            };

            SeriesCollection = new SeriesCollection(mapper)
            {
                MariaSeries,
                MariaSeries

            };

   




            DataContext = this;
        }

      
    }
}
