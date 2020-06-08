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

        public Func<double, string> FormatterDegreesCelsius { get; set; }

        public TestWindow()
        {
            //Labels.Reverse();
            InitializeComponent();

            Base = 10;

            var mapper = Mappers.Xy<ObservablePoint>()
                .X(point => Math.Log(point.X, Base)) //a 10 base log scale in the X axis
                .Y(point => point.Y);

            var MariaSeries = new LineSeries
            {
                Values = new ChartValues<ObservablePoint>
                {
                    new ObservablePoint(0.00001, 2),
                    new ObservablePoint(0.00003, 4),
                    new ObservablePoint(0.00008, 1),
                    /*               new ObservablePoint(0.0005, 5),
                                   new ObservablePoint(0.001, 1),
                                   new ObservablePoint(0.01, 3),
                                   new ObservablePoint(0.1, 8),*/
                    /*new ObservablePoint(1, 5),*/

                },
            };

            SeriesCollection = new SeriesCollection(mapper)
            {
                MariaSeries


            };

            Formatter = value => value > 0 ? "сек" : Math.Pow(Base, value).ToString();
            FormatterDegreesCelsius = value =>
            {
                if (value == 95)
                    return Properties.Resource.UnitMeasurementDegreeCentigrade;
                else if (new double[] { 60, 70, 80, 90 }.Contains(value))
                    return value.ToString();
                return string.Empty;
            };


            DataContext = this;
        }

      
    }
}
