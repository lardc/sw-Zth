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

        public GraduationCalculation() : base()
        {
            InitializeComponent();
        }


        private void CommonPage_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new GraduationZthRthVM();
            FrameVM.SetParentFrameVM(VM);

            for (int i = 0, x = 60, y = 430; i < 10; i++, x += 4, y -= 15)
                VM.GraduationChartValues.Add(new LiveCharts.Defaults.ObservablePoint(x, y));

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
                    break;
                case WorkingMode.RthSequence:
                    BottomPanelVM.RightButtonContent = Properties.Resource.CalculationRth;
                    break;
                case WorkingMode.GraduationOnly:
                    break;
                default:
                    break;
            }
        }

      
        private void CommonPage_Unloaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.RightButtonContent = string.Empty;

            TopPanelVm.AnodeBodyTemperatureIsVisible = true;
            TopPanelVm.AnodeCoolerTemperatureIsVisible = true;
            TopPanelVm.CathodeBodyTemperatureIsVisible = true;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = true;
            TopPanelVm.HeatingCurrentIsVisible = true;
            TopPanelVm.TemperatureSensitiveParameterIsVisible = true;
        }

        private void Extrapolation_Click(object sender, RoutedEventArgs e)
        {
            VM.UpperLineExtrapolationAxis = VM.UpperLineExtrapolation;
            VM.BottomLineExtrapolationAxis = VM.BottomLineExtrapolation;
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
