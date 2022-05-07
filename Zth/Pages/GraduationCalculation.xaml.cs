using LiveCharts.Defaults;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using Zth.VM;

namespace Zth.Pages
{
    /// <summary>
    /// Логика взаимодействия для RthPulseSequence.xaml
    /// </summary>
    public partial class GraduationCalculation : CommonPage
    {
        public Zth.VM.GraduationZthRthVM VM => DataContext as GraduationZthRthVM;
        private string[] Lines;

        public GraduationCalculation() : base()
        {
            InitializeComponent();
        }

        public GraduationCalculation(string[] lines) : base()
        {
            InitializeComponent();
            Lines = lines;
        }

        private void CommonPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new GraduationZthRthVM();
            FrameVM.SetParentFrameVM(VM);

            TopPanelVm.AnodeBodyTemperatureIsVisible = false;
            TopPanelVm.AnodeCoolerTemperatureIsVisible = false;
            TopPanelVm.CathodeBodyTemperatureIsVisible = false;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = false;
            TopPanelVm.HeatingCurrentIsVisible = false;
            TopPanelVm.TemperatureSensitiveParameterIsVisible = false;

            switch (TopPanelVm.WorkingMode)
            {
                case WorkingMode.ZthLongImpulse:
                case WorkingMode.ZthSequence:
                    BottomPanelVM.RightButtonContent = Properties.Resource.CalculationZth;
                    BottomPanelVM.RightButtonIsEnabled = true;
                    BottomPanelVM.RightBottomButtonAction = () => _navigationService.Navigate(new CalculationZth()
                    {
                    });
                    break;
                case WorkingMode.RthSequence:
                    BottomPanelVM.RightButtonContent = Properties.Resource.CalculationRth;
                    BottomPanelVM.RightButtonIsEnabled = true;
                    BottomPanelVM.RightBottomButtonAction = () => _navigationService.Navigate(new CalculationRth()
                    {
                    });
                    break;
                case WorkingMode.GraduationOnly:
                    break;
                default:
                    break;
            }

            if (Lines != null)
            {
                //Установка крайних границ для экстраполяции
                if (Lines.Length > 0)
                {
                    //Температура
                    VM.BottomLineExtrapolation = VM.BottomLineExtrapolationAxis = Math.Round(double.Parse(Lines[^1].Split(',')[0]) - 5);
                    VM.UpperLineExtrapolation = VM.UpperLineExtrapolationAxis = Math.Round(double.Parse(Lines[0].Split(',')[0]) + 5);
                    //ТЧП
                    VM.MinMegawatts = double.Parse(Lines[0].Split(',')[1]) - 5;
                    VM.MaxMegawatts = double.Parse(Lines[^1].Split(',')[1]) + 5;
                }
                //Добавление градуировочной кривой на график
                foreach (string line in Lines)
                {
                    double x = double.Parse(line.Split(",")[0]);
                    double y = double.Parse(line.Split(",")[1]);
                    VM.GraduationChartValues.Add(new ObservablePoint(x, y));
                }
            }

            //if (MainWindow.SettingsModel.Debug1)
            //{
            //    var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            //    dispatcherTimer.Tick += new EventHandler((sender1, e1) =>
            //    {
            //        dispatcherTimer.Stop();
            //        BottomPanelVM.RightBottomButtonAction();
            //    });
            //    dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            //    dispatcherTimer.Start();
            //}
        }

        private void CommonPage_Unloaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.RightButtonContent = string.Empty;
            BottomPanelVM.RightButtonIsEnabled = false; 

            TopPanelVm.AnodeBodyTemperatureIsVisible = true;
            TopPanelVm.AnodeCoolerTemperatureIsVisible = true;
            TopPanelVm.CathodeBodyTemperatureIsVisible = true;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = true;
            TopPanelVm.HeatingCurrentIsVisible = true;
            TopPanelVm.TemperatureSensitiveParameterIsVisible = true;
        }

        private void Extrapolation_Click(object sender, RoutedEventArgs e)
        {
            VM.BottomLineExtrapolationAxis = VM.BottomLineExtrapolation;
            VM.UpperLineExtrapolationAxis = VM.UpperLineExtrapolation;
            
            //Линейная экстраполяция
            if (VM.BottomLineExtrapolation < VM.GraduationChartValues.Min(m => m.X))
            {
                double x = VM.BottomLineExtrapolation;
                ObservablePoint[] Collection = VM.GraduationChartValues.OrderBy(m => m.X).ToArray();
                ObservablePoint lastPoint = Collection[0];
                ObservablePoint prevPoint = Collection[1];
                foreach (ObservablePoint point in Collection)
                    if (point.X != lastPoint.X)
                    {
                        prevPoint = point;
                        break;
                    }
                double y = Math.Round(prevPoint.Y + (lastPoint.Y - prevPoint.Y) / (lastPoint.X - prevPoint.X) * (x - prevPoint.X));
                VM.GraduationChartValues.Add(new ObservablePoint(x, y));
            }
            if (VM.UpperLineExtrapolation > VM.GraduationChartValues.Max(m => m.X))
            {
                double x = VM.UpperLineExtrapolation;
                ObservablePoint[] Collection = VM.GraduationChartValues.OrderByDescending(m => m.X).ToArray();
                ObservablePoint lastPoint = Collection.ToArray()[0];
                ObservablePoint prevPoint = Collection.ToArray()[1];
                foreach (ObservablePoint point in Collection)
                    if (point.X != lastPoint.X)
                    {
                        prevPoint = point;
                        break;
                    }
                double y = Math.Round(prevPoint.Y + (lastPoint.Y - prevPoint.Y) / (lastPoint.X - prevPoint.X) * (x - prevPoint.X));
                VM.GraduationChartValues.Insert(0, new ObservablePoint(x, y));
            }
            
            VM.MinMegawatts = VM.GraduationChartValues.Min(m => m.Y);
            VM.MaxMegawatts = VM.GraduationChartValues.Max(m => m.Y);

            VM.StepMegawatts = (VM.MaxMegawatts - VM.MinMegawatts) / 16;
        }

        private void SaveInFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog SFD = new SaveFileDialog()
            {
                Filter = "Excel Worksheets|*.csv"
            };
            if(SFD.ShowDialog() == true)
                File.WriteAllText(SFD.FileName, string.Join(Environment.NewLine, VM.GraduationChartValues.Select(m => $"{m.X},{m.Y}")));
        }
    }
}
