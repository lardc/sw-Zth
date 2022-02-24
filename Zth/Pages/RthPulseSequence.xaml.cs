using System;
using System.Windows;
using Zth.VM;

namespace Zth.Pages
{
    /// <summary>
    /// Логика взаимодействия для RthPulseSequence.xaml
    /// </summary>
    public partial class RthPulseSequence : CommonPage
    {
        public RthPulseSequenceVM VM => DataContext as RthPulseSequenceVM;

        public RthPulseSequence(): base()
        {
            InitializeComponent();
        }

        private async void StartHeating_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Параметры измерения
                ushort GateParameter = TopPanelVm.TypeDevice == TypeDevice.Bipolar ? (ushort)(VM.AmplitudeControlCurrent) : (ushort)VM.GateVoltage;
                ushort MeasuringCurrent = (ushort)VM.AmplitudeMeasuringCurrent;
                ushort[] HeatingCurrent = new ushort[]
                {
                    0, 0, (ushort)VM.AmplitudeHeatingCurrent
                };
                ushort MeasurementDelay = (ushort)VM.DelayTimeTspMeasurements;
                uint Duration = (uint)(VM.DurationHeatingCurrentPulse * 10);
                ushort Pause = (ushort)(VM.PauseDuration * 10);
                switch (VM.StartHeatingContent)
                {
                    case "Старт нагрев":
                        //Очистка графиков
                        Chart.ClearChart();
                        App.LogicContainer.StartTime = DateTime.Now;
                        App.LogicContainer.CommonVM = VM;
                        App.LogicContainer.Chart = Chart;
                        await App.LogicContainer.PrepareForMeasure(TopPanelVm.TypeDevice, TopPanelVm.TypeCooling, TopPanelVm.WorkingMode, GateParameter, MeasuringCurrent, HeatingCurrent, MeasurementDelay);
                        App.LogicContainer.StartRthSequence(Duration, Pause);
                        break;
                    case "Обновить задание":
                        App.LogicContainer.UpdateRthSequence(TopPanelVm.TypeDevice, GateParameter, MeasuringCurrent, HeatingCurrent, MeasurementDelay, Duration, Pause);
                        break;
                }
                BottomPanelVM.LeftButtonIsEnabled = false;

                VM.StopHeatingButtonIsEnabled = true;
                VM.StartHeatingButtonIsEnabled = true;
                VM.StartHeatingPressed = true;
                VM.RecordingResultsButtonIsEnabled = true;
            }
            catch { }
        }

        private void StopHeating_Click(object sender, RoutedEventArgs e)
        {
            App.LogicContainer.StopProcess();
            BottomPanelVM.LeftButtonIsEnabled = true;

            VM.StopHeatingButtonIsEnabled = false;
            VM.StartHeatingPressed = false;

        }

        private void RecordingResults_Click(object sender, RoutedEventArgs e)
        {
            App.LogicContainer.StopProcess();
            BottomPanelVM.LeftButtonIsEnabled = true;
            BottomPanelVM.RightButtonIsEnabled = true;

            VM.StartHeatingButtonIsEnabled = false;
            VM.RecordingResultsButtonIsEnabled = false;
            VM.RightPanelTextBoxsIsEnabled = false;
            _navigationService.Navigate(new GraduationOnly()
            {
                CanLoadInFile = true,
            });
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.RightButtonContent = Properties.Resource.Graduation;
            BottomPanelVM.RightBottomButtonAction = () => _navigationService.Navigate(new GraduationOnly()
            {
                HeatingIsEnabled = true
            });

            DataContext = new RthPulseSequenceVM();
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

            VM.RecordingResultsButtonIsEnabled = VM.StopHeatingButtonIsEnabled = VM.StartHeatingPressed;
            VM.StartHeatingButtonIsEnabled = true;

            if (MainWindow.SettingsModel.Debug1)
            {
                var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler((sender1, e1) =>
                {
                    dispatcherTimer.Stop();
                    BottomPanelVM.RightBottomButtonAction();
                });
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                dispatcherTimer.Start();
            }

        }

        private void CommonPage_Unloaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.RightButtonIsEnabled = false;
            BottomPanelVM.RightButtonContent = string.Empty;
        }
    }
}
