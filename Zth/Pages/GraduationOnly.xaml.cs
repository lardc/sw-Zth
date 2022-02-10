using System;
using System.Collections.Generic;
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
    public partial class GraduationOnly : CommonPage
    {
        public bool CanLoadInFile { get; set; }
        public bool HeatingIsEnabled { get; set; }
        public Zth.VM.GraduationOnlyVM VM => DataContext as GraduationOnlyVM;

        public GraduationOnly() : base()
        {
            InitializeComponent();
        }

        private void CommonPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (CanLoadInFile)
                BottomPanelVM.MiddleButtonContent = "Загрузка из файла";
            BottomPanelVM.MiddleButtonIsEnabled = true;
            BottomPanelVM.RightButtonContent = "Расчёт градуировки";
            BottomPanelVM.RightBottomButtonAction = () => _navigationService.Navigate(new GraduationCalculation()
            {
                
            });

            TopPanelVm.WorkingModeString_ = "Градуировка";

            DataContext = new GraduationOnlyVM();
            FrameVM.SetParentFrameVM(VM);

            TopPanelVm.CathodeBodyTemperatureIsVisible = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            VM.IsGateVoltageVisible = TopPanelVm.TypeDevice == TypeDevice.Igbt;

            VM.TemperatureSensitiveParameterIsVisibly = true;
            VM.AnodeBodyTemperatureIsVisibly = true;
            VM.CathodeBodyTemperatureIsVisibly = true;
            VM.AnodeCoolerTemperatureIsVisibly = true;
            VM.CathodeCoolerTemperatureIsVisibly = true;

            VM.TemperatureSensitiveParameterIsEnabled = true;
            VM.AnodeBodyTemperatureIsEnabled = true;
            VM.CathodeBodyTemperatureIsEnabled = true;
            VM.AnodeCoolerTemperatureIsEnabled = true;
            VM.CathodeCoolerTemperatureIsEnabled = true;

            //VM.AxisYDegreesCelsiusIsVisibly = true;
            //VM.AxisYMegawattsIsVisibly = true;

            VM.StartHeatingButtonIsEnabled = true;
            VM.CutButtonIsEnabled = false;
            VM.StopGraduationButtonIsEnabled = false;

            VM.StopHeatingButtonIsEnabled = VM.StartHeatingPressed;

            //////////////////////
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

        private void StartHeating_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Параметры измерения
                ushort GateParameter = TopPanelVm.TypeDevice == TypeDevice.Bipolar ? (ushort)(VM.DirectCurrentControlValue * 1000) : (ushort)VM.GateVoltage;
                ushort MeasuringCurrent = (ushort)(VM.DirectCurrentMeasuringValue * 1000);
                ushort[] HeatingCurrent = new ushort[]
                {
                    0, 0, (ushort)(VM.AmplitudeHeatingCurrent * 1000)
                };
                ushort MeasurementDelay = (ushort)VM.TSPMeasurementDelayTime;
                ushort Duration = (ushort)(VM.DuratioHeatingCurrentPulse * 1000);
                ushort Pause = (ushort)(VM.PauseDuration * 1000);
                ushort Temperature = (ushort)(VM.EndValueCaseTemperature * 10);
                switch (VM.StartHeatingContent)
                {
                    case "Старт нагрев":
                        App.LogicContainer.PrepareForMeasure(TopPanelVm.TypeDevice, TopPanelVm.TypeCooling, TopPanelVm.WorkingMode, GateParameter, MeasuringCurrent, HeatingCurrent, MeasurementDelay);
                        App.LogicContainer.StartGraduation(Duration, Pause, Temperature);
                        break;
                    case "Обновить задание":
                        App.LogicContainer.UpdateGraduation(HeatingCurrent, Temperature);
                        break;
                }

                VM.StopHeatingButtonIsEnabled = true;
                VM.StartHeatingPressed = true;
                VM.RightPanelTextBoxsIsEnabled = false;
            }
            catch { }
        }

        private void StopHeating_Click(object sender, RoutedEventArgs e)
        {
            App.LogicContainer.StopHeating();

            VM.AmplitudeControlCurrentTextBoxIsEnabled = false;
            VM.DurationHeatingCurrentPulseTextBoxIsEnabled = false;
            VM.StopHeatingButtonIsEnabled = false;
            VM.StartHeatingButtonIsEnabled = false;
            VM.StopGraduationButtonIsEnabled = true;
        }

        private void StopGraduation_Click(object sender, RoutedEventArgs e)
        {
            App.LogicContainer.StopProcess();

            VM.StopGraduationButtonIsEnabled = false;
            VM.CutButtonIsEnabled = true;
            VM.LineSeriesCursorLeftVisibility = true;
            VM.LineSeriesCursorRightVisibility = true;
            VM.RightPanelTextBoxsIsEnabled = false;
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            VM.CutButtonIsEnabled = false;
            BottomPanelVM.RightButtonIsEnabled = true;
            VM.LineSeriesCursorRightVisibility = false;
            var (x1, x2) = Chart.GetXRange();
            VM.AxisCustomVMTime.MinValue = Math.Floor(x1);
            VM.AxisCustomVMTime.MaxValue = Math.Ceiling(x2);
            //MainChart.MainCartesianChart.AxisX.First().MinValue = x1 - 1;
            //MainChart.MainCartesianChart.AxisX.First().MaxValue = x2 + 1;

        }

        private void CommonPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (CanLoadInFile)
                BottomPanelVM.MiddleButtonContent = string.Empty;
            BottomPanelVM.MiddleButtonIsEnabled = false;
            BottomPanelVM.RightButtonContent = string.Empty;

            TopPanelVm.WorkingModeString_ = null;
        }
    }
}
