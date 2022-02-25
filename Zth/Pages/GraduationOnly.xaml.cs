using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using Zth.VM;

namespace Zth.Pages
{
    /// <summary>
    /// Логика взаимодействия для RthPulseSequence.xaml
    /// </summary>
    public partial class GraduationOnly : CommonPage
    {
        public GraduationOnlyVM VM => DataContext as GraduationOnlyVM;
        public bool CanLoadInFile { get; set; }
        public bool HeatingIsEnabled { get; set; }     
        
        private string[] lines;

        public GraduationOnly(): base()
        {
            InitializeComponent();
        }

        private async void StartHeating_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Параметры измерения
                ushort GateParameter = TopPanelVm.TypeDevice == TypeDevice.Bipolar ? (ushort)VM.DirectCurrentControlValue : (ushort)VM.GateVoltage;
                ushort MeasuringCurrent = (ushort)VM.DirectCurrentMeasuringValue;
                ushort[] HeatingCurrent = new ushort[]
                {
                    0, 0, (ushort)VM.AmplitudeHeatingCurrent
                };
                ushort MeasurementDelay = (ushort)VM.TSPMeasurementDelayTime;
                uint Duration = (uint)(VM.DuratioHeatingCurrentPulse * 10);
                ushort Pause = (ushort)(VM.PauseDuration * 10);
                ushort Temperature = (ushort)(VM.EndValueCaseTemperature * 10);
                switch (VM.StartHeatingContent)
                {
                    case "Старт нагрев":
                        //Очистка графиков
                        Chart.ClearChart();
                        App.LogicContainer.StartTime = DateTime.Now;
                        App.LogicContainer.CommonVM = VM;
                        App.LogicContainer.Chart = Chart;
                        await App.LogicContainer.PrepareForMeasure(TopPanelVm.TypeDevice, TopPanelVm.TypeCooling, TopPanelVm.WorkingMode, GateParameter, MeasuringCurrent, HeatingCurrent, MeasurementDelay);
                        App.LogicContainer.StartGraduation(Duration, Pause, Temperature);
                        break;
                    case "Обновить задание":
                        App.LogicContainer.UpdateGraduation(HeatingCurrent, Temperature);
                        break;
                }
                BottomPanelVM.LeftButtonIsEnabled = false;

                VM.StopHeatingButtonIsEnabled = true;
                VM.StartHeatingPressed = true;
                VM.RightPanelTextBoxsIsEnabled = false;
            }
            catch { }
        }

        private void StopHeating_Click(object sender, RoutedEventArgs e)
        {
            App.LogicContainer.StopHeating();

            VM.StopHeatingButtonIsEnabled = false;
            VM.StartHeatingButtonIsEnabled = false;
            VM.StopGraduationButtonIsEnabled = true;

            VM.AmplitudeControlCurrentTextBoxIsEnabled = false;
            VM.DurationHeatingCurrentPulseTextBoxIsEnabled = false;
            
        }

        private void StopGraduation_Click(object sender, RoutedEventArgs e)
        {
            App.LogicContainer.StopProcess();
            App.LogicContainer.ReadEndpointsGraduation();
            BottomPanelVM.LeftButtonIsEnabled = true;

            VM.StopGraduationButtonIsEnabled = false;
            VM.CutButtonIsEnabled = true;
            VM.LineSeriesCursorLeftVisibility = true;
            VM.LineSeriesCursorRightVisibility = true;
            VM.RightPanelTextBoxsIsEnabled = false;
        }

        private void GraduationFromFile_Load()
        {
            OpenFileDialog SFD = new OpenFileDialog()
            {
                Filter = "Excel Worksheets|*.csv"
            };
            if (SFD.ShowDialog() == true)
                _navigationService.Navigate(new GraduationCalculation(File.ReadAllLines(SFD.FileName))
                {

                });
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            VM.CutButtonIsEnabled = false;
            BottomPanelVM.RightButtonIsEnabled = true;
            VM.LineSeriesCursorRightVisibility = false;
            var (x1, x2) = Chart.GetXRange();
            VM.AxisCustomVMTime.MinValue = Math.Floor(x1);
            VM.AxisCustomVMTime.MaxValue = Math.Ceiling(x2);

            var (graduationX1, graduationX2) = Chart.GetGraduationRange();
            App.Logger.Info(string.Format("Cutting points of interest out of graduation chart for range {0} to {1}", graduationX1, graduationX2));
            List<string> linesList = new List<string>();
            for (int i = 0; i < VM.AnodeBodyTemperatureChartValues.Count; i++)
                if (VM.AnodeBodyTemperatureChartValues[i].X >= graduationX1 && VM.AnodeBodyTemperatureChartValues[i].X <= graduationX2)
                    linesList.Add(string.Format("{0},{1}", VM.AnodeBodyTemperatureChartValues[i].Y, VM.TemperatureSensitiveParameterChartValues[i].Y));
            lines = linesList.ToArray();
            App.Logger.Info(string.Format("{0} points of interest cut out of graduation chart", lines.Length));
        }

        private void CommonPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (CanLoadInFile)
                BottomPanelVM.MiddleButtonContent = "Загрузка из файла";
            BottomPanelVM.MiddleButtonIsEnabled = true;

            BottomPanelVM.MiddleBottomButtonAction = () => GraduationFromFile_Load();
            
            BottomPanelVM.RightButtonContent = "Расчёт градуировки";

            BottomPanelVM.RightBottomButtonAction = () => _navigationService.Navigate(new GraduationCalculation(lines)
            {
                
            });

            TopPanelVm.WorkingModeString_ = "Градуировка";

            DataContext = new GraduationOnlyVM();
            FrameVM.SetParentFrameVM(VM);

            TopPanelVm.CathodeBodyTemperatureIsVisible = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            VM.IsGateVoltageVisible = TopPanelVm.TypeDevice == TypeDevice.Igbt;

            VM.TemperatureSensitiveParameterIsVisibly = true;
            VM.HeatingCurrentIsVisibly = true;
            VM.HeatingPowerIsVisibly = true;
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
