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

            TopPanelVm.CathodeBodyTemperatureIsVisible = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            VM.IsGateVoltageVisible = TopPanelVm.TypeDevice == TypeDevice.Igbt;

            VM.HeatingCurrentIsVisibly = true;
            VM.HeatingPowerIsVisibly = true;
            VM.TemperatureSensitiveParameterIsVisibly = true;
            VM.AnodeBodyTemperatureIsVisibly = true;
            VM.CathodeBodyTemperatureIsVisibly = true;
            VM.AnodeCoolerTemperatureIsVisibly = true;
            VM.CathodeCoolerTemperatureIsVisibly = true;

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

        private void StartMeasurement_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Параметры измерения
                ushort GateParameter = TopPanelVm.TypeDevice == TypeDevice.Bipolar ? (ushort)(VM.AmplitudeControlCurrent * 1000) : (ushort)VM.GateVoltage;
                ushort MeasuringCurrent = (ushort)(VM.AmplitudeMeasuringCurrent * 1000);
                ushort[] HeatingCurrent = new ushort[]
                {
                    (ushort)(VM.AmplitudeHeatingCurrentLess2 * 1000),
                    (ushort)(VM.AmplitudeHeatingCurrentLess10 * 1000),
                    (ushort)(VM.AmplitudeHeatingCurrentAbove10 * 1000),
                };
                ushort MeasurementDelay = (ushort)VM.TSPMeasurementDelayTime;
                ushort Duration1 = (ushort)(VM.FirstPulseDuration * 1000);
                ushort Duration2 = (ushort)(VM.LastPulseDuration * 1000000);
                ushort Pause = (ushort)VM.PauseDurationBetweenAdjacentPulses;
                App.LogicContainer.PrepareForMeasure(TopPanelVm.TypeDevice, TopPanelVm.TypeCooling, TopPanelVm.WorkingMode, GateParameter, MeasuringCurrent, HeatingCurrent, MeasurementDelay);
                App.LogicContainer.StartZthSequence(Duration1, Duration2, Pause);
                App.LogicContainer.CommonVM = VM;

                VM.StartMeasurementButtonEnabled = false;
                VM.StopMeasurementButtonEnabled = true;
                VM.RightPanelTextBoxsIsEnabled = false;
            }
            catch { }
        }

        private void StopMeasurement_Click(object sender, RoutedEventArgs e)
        {
            App.LogicContainer.StopProcess();

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
