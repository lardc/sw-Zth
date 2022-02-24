using System;
using System.Windows;
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

        private async void StartMeasurement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Параметры измерения
                ushort GateParameter = TopPanelVm.TypeDevice == TypeDevice.Bipolar ? (ushort)(VM.AmplitudeControlCurrent) : (ushort)VM.GateVoltage;
                ushort MeasuringCurrent = (ushort)VM.AmplitudeMeasuringCurrent;
                ushort[] HeatingCurrent = new ushort[]
                {
                    (ushort)VM.AmplitudeHeatingCurrentLess2,
                    (ushort)VM.AmplitudeHeatingCurrentLess10,
                    (ushort)VM.AmplitudeHeatingCurrentAbove10,
                };
                ushort MeasurementDelay = (ushort)VM.TSPMeasurementDelayTime;
                ushort Duration1 = (ushort)(VM.FirstPulseDuration * 10);
                uint Duration2 = (uint)(VM.LastPulseDuration * 10000);
                ushort Pause = (ushort)VM.PauseDurationBetweenAdjacentPulses;
                //Очистка графиков
                Chart.ClearChart();
                App.LogicContainer.StartTime = DateTime.Now;
                App.LogicContainer.CommonVM = VM;
                App.LogicContainer.Chart = Chart;
                await App.LogicContainer.PrepareForMeasure(TopPanelVm.TypeDevice, TopPanelVm.TypeCooling, TopPanelVm.WorkingMode, GateParameter, MeasuringCurrent, HeatingCurrent, MeasurementDelay);
                App.LogicContainer.StartZthSequence(Duration1, Duration2, Pause);
                BottomPanelVM.LeftButtonIsEnabled = false;

                VM.StartMeasurementButtonEnabled = false;
                VM.StopMeasurementButtonEnabled = true;
                VM.RightPanelTextBoxsIsEnabled = false;
            }
            catch { }
        }

        private void StopMeasurement_Click(object sender, RoutedEventArgs e)
        {
            App.LogicContainer.StopProcess();
            App.LogicContainer.ReadEndpointsZthSequence();
            BottomPanelVM.LeftButtonIsEnabled = true;
            BottomPanelVM.RightButtonIsEnabled = true;

            VM.StopMeasurementButtonEnabled = false;
            VM.LineSeriesCursorLeftVisibility = true;
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

            TopPanelVm.CathodeBodyTemperatureIsVisible = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            VM.IsGateVoltageVisible = TopPanelVm.TypeDevice == TypeDevice.Igbt;

            VM.HeatingCurrentIsVisibly = true;
            VM.HeatingPowerIsVisibly = true;
            VM.TemperatureSensitiveParameterIsVisibly = true;
            VM.AnodeBodyTemperatureIsVisibly = true;
            VM.CathodeBodyTemperatureIsVisibly = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            VM.AnodeCoolerTemperatureIsVisibly = true;
            VM.CathodeCoolerTemperatureIsVisibly = TopPanelVm.TypeCooling != TypeCooling.OneSided;

            VM.HeatingCurrentIsEnabled = true;
            VM.HeatingPowerIsEnabled = true;
            VM.TemperatureSensitiveParameterIsEnabled = true;
            VM.AnodeBodyTemperatureIsEnabled = true;
            VM.CathodeBodyTemperatureIsEnabled = true;
            VM.AnodeCoolerTemperatureIsEnabled = true;
            VM.CathodeCoolerTemperatureIsEnabled = true;

            //VM.AxisYAmperesIsVisibly = true;
            //VM.AxisYDegreesCelsiusIsVisibly = true;
            //VM.AxisYKilowattsIsVisibly = true;
            //VM.AxisYMegawattsIsVisibly = true;

            VM.StartMeasurementButtonEnabled = true;
            VM.StopMeasurementButtonEnabled = false;
        }

        private void CommonPage_Unloaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.RightButtonIsEnabled = false;
            BottomPanelVM.RightButtonContent = string.Empty;
        }
    }
}
