using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
            MainCartesianChart.DataTooltip = null;
        }

        public void AdjustChart()
        {
            foreach (var i in MainCartesianChart.AxisY)
            {
                i.Separator.IsEnabled = false;
                i.ShowLabels = false;
            }

            if (VM.ZthaIsEnabled | VM.ZthIsEnabled | VM.ZthkIsEnabled)
                VM.AxisYDegreeCelsiusPerWattIsEnabled = true;
            else
                VM.AxisYDegreeCelsiusPerWattIsEnabled = false;

            if (VM.HeatingCurrentIsEnabled | VM.HeatingPowerIsEnabled | VM.TemperatureSensitiveParameterIsEnabled | VM.AnodeBodyTemperatureIsEnabled | VM.CathodeBodyTemperatureIsEnabled
            | VM.AnodeCoolerTemperatureIsEnabled | VM.CathodeCoolerTemperatureIsEnabled | VM.TemperatureStructureIsEnabled)
                VM.AxisYDegreesCelsiusIsEnabled = true;
            else
                VM.AxisYDegreesCelsiusIsEnabled = false;

            foreach (var i in MainCartesianChart.Series)
            {
                
                var axisX = MainCartesianChart.AxisX[i.ScalesXAt];
                var axisY = MainCartesianChart.AxisY[i.ScalesYAt];

                if (i.Values == null)
                    continue;
                if (i.Values.Count == 0)
                    continue;

                var minX = ((ChartValues<ObservablePoint>)i.Values).Min(m => m.X);
                var maxX = ((ChartValues<ObservablePoint>)i.Values).Max(m => m.X);
                var minY = ((ChartValues<ObservablePoint>)i.Values).Min(m => m.Y);
                var maxY = ((ChartValues<ObservablePoint>)i.Values).Max(m => m.Y);

                if (axisX.MinValue > Math.Pow(minX, 10))
                {
                    axisX.MinValue = Math.Log10(minX);
                    axisX.Separator.Step = (axisX.MaxValue - axisX.MinValue + double.Epsilon) / 6;
                }
                while (Math.Pow(10, axisX.MaxValue) < maxX)
                    axisX.MaxValue++;
                

                while (axisY.MinValue  > minY || axisY.MaxValue  < maxY)
                {
                    if (axisY.MinValue > minY)
                        axisY.MinValue -= axisY.Separator.Step;
                    if (axisY.MaxValue  < maxY)
                        axisY.MaxValue += axisY.Separator.Step;
                    axisY.Separator.Step += axisY.Separator.Step / 16;

                }
            }
        }

        public void ClearChart(bool endpoints = false)
        {
            //Греющий ток
            VM.HeatingCurrentChartValues.Clear();
            //Греющая мощность
            VM.HeatingPowerChartValues.Clear();
            //ТЧП
            VM.TemperatureSensitiveParameterChartValues.Clear();
            //Температура корпуса
            VM.AnodeBodyTemperatureChartValues.Clear();
            VM.CathodeBodyTemperatureChartValues.Clear();
            //Температура охладителя
            VM.AnodeCoolerTemperatureChartValues.Clear();
            VM.CathodeCoolerTemperatureChartValues.Clear();
            if (!endpoints)
            {
                VM.HeatingCurrentChartValues.Add(new ObservablePoint(0.000001, 0));
                VM.HeatingPowerChartValues.Add(new ObservablePoint(0.000001, 0));
                VM.TemperatureSensitiveParameterChartValues.Add(new ObservablePoint(0.000001, 0));
                VM.AnodeBodyTemperatureChartValues.Add(new ObservablePoint(0.000001, 0));
                VM.CathodeBodyTemperatureChartValues.Add(new ObservablePoint(0.000001, 0));
                VM.AnodeCoolerTemperatureChartValues.Add(new ObservablePoint(0.000001, 0));
                VM.CathodeCoolerTemperatureChartValues.Add(new ObservablePoint(0.000001, 0));
            }
            
            //AdjustChart();
        }

        private CancellationTokenSource _simulateCancellationTokenSource = new CancellationTokenSource();
        
        public void SimulateStop()
        {
            _simulateCancellationTokenSource.Cancel();
        }

        public void SimulateStart()
        {
            Task.Factory.StartNew(() =>
            {
                var lines = File.ReadAllLines(@"Dataset.csv").ToList();
                lines.RemoveAt(0);
                foreach (var line in lines)
                {
                    if (_simulateCancellationTokenSource.IsCancellationRequested)
                        return;
                    var values = line.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    Dispatcher.BeginInvoke(new Action(() =>
                    {

                        VM.HeatingCurrent = double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.2;
                        VM.HeatingCurrentChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.2, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 0.3));

                        VM.HeatingPower = double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.3;
                        VM.HeatingPowerChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.3, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.3));

                        VM.TemperatureStructure = double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.5;
                        VM.TemperatureStructureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.5, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.5));

                        VM.CathodeCoolerTemperature = double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.4;
                        VM.CathodeCoolerTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.4, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.4));

                        VM.AnodeCoolerTemperature = double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.3;
                        VM.AnodeCoolerTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.3, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.3));

                        VM.CathodeBodyTemperature = double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.2;
                        VM.CathodeBodyTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.2, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.2));

                        VM.AnodeBodyTemperature = double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.1;
                        VM.AnodeBodyTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.1, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.1));

                        VM.TemperatureSensitiveParameter = double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.1;
                        VM.TemperatureSensitiveParameterChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.1, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 1.1));

                        AdjustChart();

                    }));
                    Thread.Sleep(10);
                  
                }

           

                for (double x = 0.00005, y = 0.3; x < 1; x *= 4, y *= 1.1)
                {
                    if (_simulateCancellationTokenSource.IsCancellationRequested)
                        return;
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        VM.Zth = y;
                        VM.ZthChartValues.Add(new ObservablePoint(x, y));

                        VM.Ztha = y * 1.1;
                        VM.ZthaChartValues.Add(new ObservablePoint(x, y * 1.1));

                        VM.Zthk = y * 1.2;
                        VM.ZthkChartValues.Add(new ObservablePoint(x, y * 1.2));

                        AdjustChart();
                    }));
                    Thread.Sleep(100);
                }

            }, _simulateCancellationTokenSource.Token);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (VM != null)
                VM.CheckboxParameterCheck = () => AdjustChart();

            if (File.Exists(@"Dataset.csv") == false)
                return;


            MainCartesianChart.Series.Configuration = VM.Mapper;

            //var zthValues = 

          

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
        }

        private LineSeriesCursor lastLineSeriesCursor;

        private void LineSeriesCursor_MouseMove(object sender, MouseEventArgs e)
        {
            DrawAllValues((LineSeriesCursor)sender);
        }

        public (double x1, double x2) GetXRange()
        {
            double x1 = Extentions.ConvertToChartValues(MainCartesianChart, new Point(LineSeriesCursorLeft.Margin.Left + LineSeriesCursorLeft.ActualWidth / 2, 0), 0, 0).X;
            double x2 = Extentions.ConvertToChartValues(MainCartesianChart, new Point(LineSeriesCursorRight.Margin.Left + LineSeriesCursorRight.ActualWidth / 2, 0), 0, 0).X;
            return (Math.Min(x1,x2), Math.Max(x1,x2));
        }

        public (double x1, double x2) GetGraduationRange()
        {
            double x1 = GetXPoint(LineSeriesCursorLeft);
            double x2 = GetXPoint(LineSeriesCursorRight);
            return (Math.Min(x1, x2), Math.Max(x1, x2));
        }

        private void MainCartesianChart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (lastLineSeriesCursor != null)
            {
                lastLineSeriesCursor.Margin = new Thickness(e.GetPosition((IInputElement)sender).X - lastLineSeriesCursor.ActualWidth / 2, 0, 0, 0);
                DrawAllValues(lastLineSeriesCursor);
            }
        }

        private void LineSeriesCursor_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lastLineSeriesCursor = sender as LineSeriesCursor;
        }
    }
}
