using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
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

        public GraduationCalculation(string[] lines)
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
                foreach (string line in Lines)
                {
                    double x = double.Parse(line.Split(",")[0]);
                    double y = double.Parse(line.Split(",")[1]);
                    VM.GraduationChartValues.Add(new LiveCharts.Defaults.ObservablePoint(x, y));
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
            
            //Линейная экстраполяция?
            if (VM.BottomLineExtrapolation < VM.GraduationChartValues.Min(m => m.X))
            {
                double x = VM.BottomLineExtrapolation;
                LiveCharts.Defaults.ObservablePoint lastPoint = VM.GraduationChartValues.OrderBy(m => m.X).ToArray()[0];
                LiveCharts.Defaults.ObservablePoint prevPoint = VM.GraduationChartValues.OrderBy(m => m.X).ToArray()[1];
                double y = prevPoint.Y + (lastPoint.Y - prevPoint.Y) / (lastPoint.X - prevPoint.X) * (x - prevPoint.X);
                VM.GraduationChartValues.Add(new LiveCharts.Defaults.ObservablePoint(x, y));
            }
            if (VM.UpperLineExtrapolation > VM.GraduationChartValues.Max(m => m.X))
            {
                double x = VM.UpperLineExtrapolation;
                LiveCharts.Defaults.ObservablePoint lastPoint = VM.GraduationChartValues.OrderByDescending(m => m.X).ToArray()[0];
                LiveCharts.Defaults.ObservablePoint prevPoint = VM.GraduationChartValues.OrderByDescending(m => m.X).ToArray()[1];
                double y = prevPoint.Y + (lastPoint.Y - prevPoint.Y) / (lastPoint.X - prevPoint.X) * (x - prevPoint.X);
                VM.GraduationChartValues.Add(new LiveCharts.Defaults.ObservablePoint(x, y));
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
