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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Zth.VM;

namespace Zth.Pages
{
    /// <summary>
    /// Логика взаимодействия для RthPulseSequence.xaml
    /// </summary>
    public partial class ZthPulseSequence : CommonPage
    {
        public ZthPulseSequenceVM VM => DataContext as ZthPulseSequenceVM;

        public ZthPulseSequence()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.RightButtonContent = Properties.Resource.Graduation;
            BottomPanelVM.RightBottomButtonAction = () => _navigationService.Navigate(new GraduationOnly()
            {
                CanLoadInFile = true
            });

            DataContext = new ZthPulseSequenceVM();
            FrameVM.SetParentFrameVM(VM);

            VM.HeatingCurrentIsVisibly = true;
            VM.HeatingPowerIsVisibly = true;
            VM.TemperatureSensitiveParameterIsVisibly = true;
            VM.AnodeBodyTemperatureIsVisibly = true;
            VM.CathodeBodyTemperatureIsVisibly = true;
            VM.AnodeCoolerTemperatureIsVisibly = true;
            VM.CathodeCoolerTemperatureIsVisibly = true;

            VM.AxisYAmperesIsVisibly = true;
            VM.AxisYDegreesCelsiusIsVisibly = true;
            VM.AxisYKilowattsIsVisibly = true;
            VM.AxisYMegawattsIsVisibly = true;

            VM.StartMeasurementButtonEnabled = true;
            VM.StopMeasurementButtonEnabled = false;
        }

        private void StartMeasurement_Click(object sender, RoutedEventArgs e)
        {
            VM.StartMeasurementButtonEnabled = false;
            VM.StopMeasurementButtonEnabled = true;
        }

        private void StopMeasurement_Click(object sender, RoutedEventArgs e)
        {
            VM.StopMeasurementButtonEnabled = false;
            BottomPanelVM.RightButtonIsEnabled = true;
            VM.LineSeriesCursorLeftVisibility = true;
        }

        private void CommonPage_Unloaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.RightButtonIsEnabled = false;
            BottomPanelVM.RightButtonContent = string.Empty;
        }
    }
}
