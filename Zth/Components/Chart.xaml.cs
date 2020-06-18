using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
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
            if (File.Exists(@"Dataset.csv") == false)
                return;

            var reader = new StreamReader(File.OpenRead(@"Dataset.csv"));

            MainCartesianChart.Series.Configuration = VM.Mapper;

            var line = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                var values = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

                VM.HeatingCurrentChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.2, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.2));
                VM.HeatingPowerChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.3, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.3));


                VM.TemperatureStructureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.5, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.5));
                VM.CathodeCoolerTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.4, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.4));
                VM.AnodeCoolerTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.3, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.3));
                VM.CathodeBodyTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.2, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.2));
                VM.AnodeBodyTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.1, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.1));
                VM.TemperatureSensitiveParameterChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.1, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.1));
            }

            for (double x = 0.00005, y = 0.3; x < 1; x *= 4, y *= 1.1)
            {
                VM.ZthChartValues.Add(new ObservablePoint(x, y));
                VM.ZthaChartValues.Add(new ObservablePoint(x, y * 1.1));
                VM.ZthkChartValues.Add(new ObservablePoint(x, y * 1.2));
            }
        }

        private double GetYPoint(ChartValues<ObservablePoint> observablePoints, LineSeriesCursor lineSeriesCursor, int numberAxisY)
        {
            var chartValue = Extentions.ConvertToChartValues(MainCartesianChart, new Point(lineSeriesCursor.Margin.Left + lineSeriesCursor.ActualWidth / 2, 0), 0, numberAxisY);
            var x = Math.Pow(10, chartValue.X);
            var point = observablePoints.OrderBy(m => Math.Abs(x - m.X)).First();
            return point.Y;
        }

        private double GetXPoint(LineSeriesCursor lineSeriesCursor)
        {
            var chartValue = Extentions.ConvertToChartValues(MainCartesianChart, new Point(lineSeriesCursor.Margin.Left + lineSeriesCursor.ActualWidth / 2, 0), 0, 0);
            return Math.Pow(10, chartValue.X);
        }


        private void DrawAllValues(LineSeriesCursor lineSeriesCursor)
        {
            VM.Time = GetXPoint(lineSeriesCursor);
            if (VM.HeatingCurrentIsEnabled)
                VM.HeatingCurrent = GetYPoint(VM.HeatingCurrentChartValues, lineSeriesCursor, 1);
            if (VM.HeatingPowerIsEnabled)
                VM.HeatingPower = GetYPoint(VM.HeatingPowerChartValues, lineSeriesCursor, 1);

            if (VM.AnodeBodyTemperatureIsEnabled)
                VM.AnodeBodyTemperature = GetYPoint(VM.AnodeBodyTemperatureChartValues, lineSeriesCursor, 1);
            if (VM.AnodeCoolerTemperatureIsEnabled)
                VM.AnodeCoolerTemperature = GetYPoint(VM.AnodeCoolerTemperatureChartValues, lineSeriesCursor, 1);
            if (VM.CathodeBodyTemperatureIsEnabled)
                VM.CathodeBodyTemperature = GetYPoint(VM.CathodeBodyTemperatureChartValues, lineSeriesCursor, 1);
            if (VM.CathodeCoolerTemperatureIsEnabled)
                VM.CathodeCoolerTemperature = GetYPoint(VM.CathodeCoolerTemperatureChartValues, lineSeriesCursor, 1);

            if (VM.TemperatureStructureIsEnabled)
                VM.TemperatureStructure = GetYPoint(VM.TemperatureStructureChartValues, lineSeriesCursor, 1);
            if (VM.TemperatureSensitiveParameterIsEnabled)
                VM.TemperatureSensitiveParameter = GetYPoint(VM.TemperatureSensitiveParameterChartValues, lineSeriesCursor, 1);

            if (VM.ZthIsEnabled)
                VM.Zth = GetYPoint(VM.ZthChartValues, lineSeriesCursor, 1);
            if (VM.ZthaIsEnabled)
                VM.Ztha = GetYPoint(VM.ZthaChartValues, lineSeriesCursor, 1);
            if (VM.ZthkIsEnabled)
                VM.Zthk = GetYPoint(VM.ZthkChartValues, lineSeriesCursor, 1);

            //if (VM.HeatingCurrentIsEnabled)
            //    VM.HeatingCurrent = GetYPoint(VM.HeatingCurrentChartValues, lineSeriesCursor, 1);
            //if (VM.HeatingCurrentIsEnabled)
            //    VM.HeatingCurrent = GetYPoint(VM.HeatingCurrentChartValues, lineSeriesCursor, 1);
            //if (VM.HeatingCurrentIsEnabled)
            //    VM.HeatingCurrent = GetYPoint(VM.HeatingCurrentChartValues, lineSeriesCursor, 1);
            //if (VM.HeatingCurrentIsEnabled)
            //    VM.HeatingCurrent = GetYPoint(VM.HeatingCurrentChartValues, lineSeriesCursor, 1);
            //if (VM.HeatingCurrentIsEnabled)
            //    VM.HeatingCurrent = GetYPoint(VM.HeatingCurrentChartValues, lineSeriesCursor, 1);
            //if (VM.HeatingCurrentIsEnabled)
            //    VM.HeatingCurrent = GetYPoint(VM.HeatingCurrentChartValues, lineSeriesCursor, 1);
        }

        private void LineSeriesCursorLeft_MouseMove(object sender, MouseEventArgs e)
        {
            DrawAllValues((LineSeriesCursor)sender);
        }



        private void MainCartesianChart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var q = e.GetPosition((IInputElement) sender);
            var r = LiveCharts.Wpf.Extentions.ConvertToChartValues(MainCartesianChart, q, 0, 1);
        }
    }
}
