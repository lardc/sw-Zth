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
    public partial class RthPulseSequence : CommonPage
    {
        public RthPulseSequenceVM VM => DataContext as RthPulseSequenceVM;

        public RthPulseSequence() : base()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.LeftButtonIsEnabled = true;
            BottomPanelVM.RightButtonContent = Properties.Resource.Back;
            BottomPanelVM.RightButtonIsEnabled = false;
            BottomPanelVM.RightButtonContent = Properties.Resource.Graduation;

            DataContext = new RthPulseSequenceVM();
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

            VM.StartHeatingButtonIsEnabled = true;


            BottomPanelVM.RightButtonContent = Properties.Resource.Graduation;
            BottomPanelVM.RightBottomButtonAction = () => _navigationService.Navigate(new GraduationOnly()
            {
                HeatingIsEnabled = true
            });

        }

        private void StartHeating_Click(object sender, RoutedEventArgs e)
        {
            VM.StartHeatingButtonIsEnabled = true;
            VM.StartHeatingPressed = true;
            VM.RecordingResultsButtonIsEnabled = true;
            VM.StopHeatingButtonIsEnabled = true;
        }

        private void RecordingResults_Click(object sender, RoutedEventArgs e)
        {
            VM.StartHeatingButtonIsEnabled = false;
            VM.RecordingResultsButtonIsEnabled = false;
            BottomPanelVM.RightButtonIsEnabled = true;

        }

        private void StopHeating_Click(object sender, RoutedEventArgs e)
        {
            _navigationService.Navigate(new GraduationOnly()
            {
                CanLoadInFile = true,
            });
            
        }
    }
}
