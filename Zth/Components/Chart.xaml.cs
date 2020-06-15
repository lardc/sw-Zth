using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zth.VM;

namespace Zth.Components
{
    /// <summary>
    /// Логика взаимодействия для Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        public CommonVM VM => DataContext as CommonVM;



        public Chart()
        {

            InitializeComponent();
         
           


        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var reader = new StreamReader(File.OpenRead(@"C:\Users\Ivan\Desktop\Dataset.csv"));
            var mapper = Mappers.Xy<ObservablePoint>()
            .X(point => Math.Log(point.X, 10)) //a 10 base log scale in the X axis
            .Y(point => point.Y);

            var tsp = new ChartValues<ObservablePoint>();
            var tj = new ChartValues<ObservablePoint>();
            var tc = new ChartValues<ObservablePoint>();
            var to = new ChartValues<ObservablePoint>();

            var line = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                var values = line.Split(',', StringSplitOptions.RemoveEmptyEntries);



                //VM.TemperatureSensitiveParameterChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace('.', ',')) / 1.5, double.Parse(values[3].Replace('.', ',')) / 1.5));
                VM.HeatingCurrentChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace('.', ',')) /5 , double.Parse(values[3].Replace('.', ',')) * 10));
                VM.HeatingPowerChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace('.', ',')) / 1.5, double.Parse(values[3].Replace('.', ',')) * 5));

                VM.TemperatureStructureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace('.', ',')) / 1.5, double.Parse(values[3].Replace('.', ',')) / 1.5));
                VM.CathodeCoolerTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace('.', ',')) / 1.4, double.Parse(values[3].Replace('.', ',')) / 1.4));
                VM.AnodeCoolerTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace('.', ',')) / 1.3, double.Parse(values[3].Replace('.', ',')) / 1.3));
                VM.CathodeBodyTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace('.', ',')) / 1.2, double.Parse(values[3].Replace('.', ',')) / 1.2));
                VM.AnodeBodyTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace('.', ',')) / 1.1, double.Parse(values[3].Replace('.', ',')) / 1.1));

                //to.Add(new ObservablePoint(double.Parse(values[6].Replace('.', ',')), double.Parse(values[7].Replace('.', ','))));

            }

            //MainCartesianChart.Series = VM.SeriesCollection;

        }
    }
}
