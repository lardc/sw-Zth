using System;
using System.Windows;
using Zth.VM;

namespace Zth.Pages
{
    /// <summary>
    /// Логика взаимодействия для ZthLongImpulse.xaml
    /// </summary>
    public partial class ZthLongImpulse : CommonPage
    {
        public ZthLongImpulseVM VM => DataContext as ZthLongImpulseVM;

        public ZthLongImpulse()
        {
            InitializeComponent();
        }

        private async void StartHeating_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Параметры измерения
                ushort GateParameter = TopPanelVm.TypeDevice == TypeDevice.Bipolar ? (ushort)VM.AmplitudeControlCurrent : (ushort)VM.GateVoltage;
                ushort MeasuringCurrent = (ushort)VM.AmplitudeMeasuringCurrent;
                ushort[] HeatingCurrent = new ushort[]
                {
                    0, 0, (ushort)VM.AmplitudeHeatingCurrent
                };
                ushort MeasurementDelay = (ushort)VM.DelayTimeTspMeasurements;
                uint Duration = (uint)(VM.DurationPowerPulse * 10000);
                switch (VM.StartHeatingContent)
                {
                    case "Старт нагрев":
                        //Очистка графиков
                        Chart.ClearChart();
                        App.LogicContainer.StartTime = DateTime.Now;                        
                        App.LogicContainer.CommonVM = VM;
                        App.LogicContainer.Chart = Chart;
                        await App.LogicContainer.PrepareForMeasure(TopPanelVm.TypeDevice, TopPanelVm.TypeCooling, TopPanelVm.WorkingMode, GateParameter, MeasuringCurrent, HeatingCurrent, MeasurementDelay);
                        App.LogicContainer.StartZthLongImpulse(Duration);
                        break;
                    case "Обновить задание":
                        App.LogicContainer.UpdateZthLongImpulse(TopPanelVm.TypeDevice, GateParameter, MeasuringCurrent, HeatingCurrent, MeasurementDelay, Duration);
                        break;
                }
                BottomPanelVM.LeftButtonIsEnabled = false;

                VM.StopHeatingButtonIsEnabled = true;
                VM.StartHeatingPressed = true;
            }
            catch { }
        }

        private void StopHeating_Click(object sender, RoutedEventArgs e)
        {
            App.LogicContainer.StopHeating();

            VM.StopHeatingButtonIsEnabled = false;
            VM.StartHeatingButtonIsEnabled = false;
            VM.StopMeasurementButtonIsEnabled = true;
            VM.RightPanelTextBoxsIsEnabled = false;
        }

        private void StopMeasurement_Click(object sender, RoutedEventArgs e)
        {
            App.LogicContainer.StopProcess();
            App.LogicContainer.ReadEndpointsZthLongImpulse();
            BottomPanelVM.LeftButtonIsEnabled = true;
            BottomPanelVM.RightButtonIsEnabled = true;

            VM.StopMeasurementButtonIsEnabled = false;
            
            VM.LineSeriesCursorLeftVisibility = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            TopPanelVm.TemperatureSensitiveParameterIsVisible = false;
            BottomPanelVM.RightButtonContent = Properties.Resource.Graduation;
            BottomPanelVM.RightBottomButtonAction = () => _navigationService.Navigate(new GraduationOnly()
            {
                CanLoadInFile = true
            });

            DataContext = new ZthLongImpulseVM();
            FrameVM.SetParentFrameVM(VM);

            TopPanelVm.CathodeBodyTemperatureIsVisible = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            VM.IsGateVoltageVisible = TopPanelVm.TypeDevice == TypeDevice.Igbt;

            VM.StartHeatingButtonIsEnabled = true;

            VM.HeatingCurrentIsVisibly = true;
            VM.HeatingPowerIsVisibly = true;
            VM.AnodeBodyTemperatureIsVisibly = true;
            VM.CathodeBodyTemperatureIsVisibly = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            VM.AnodeCoolerTemperatureIsVisibly = true;
            VM.CathodeCoolerTemperatureIsVisibly = TopPanelVm.TypeCooling != TypeCooling.OneSided;

            VM.HeatingCurrentIsEnabled = true;
            VM.HeatingPowerIsEnabled = true;
            VM.AnodeBodyTemperatureIsEnabled = true;
            VM.CathodeBodyTemperatureIsEnabled = true;
            VM.AnodeCoolerTemperatureIsEnabled = true;
            VM.CathodeCoolerTemperatureIsEnabled = true;

            //VM.AxisYAmperesIsVisibly = true;
            //VM.AxisYDegreesCelsiusIsVisibly = true;
            //VM.AxisYKilowattsIsVisibly = true;
            //VM.AxisYMegawattsIsVisibly = true;

            VM.StopHeatingButtonIsEnabled = VM.StartHeatingPressed;

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
            TopPanelVm.TemperatureSensitiveParameterIsVisible = true;

            BottomPanelVM.RightButtonIsEnabled = false;
            BottomPanelVM.RightButtonContent = string.Empty;
        }
    }
}
