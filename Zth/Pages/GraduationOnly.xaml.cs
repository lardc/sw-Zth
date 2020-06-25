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

        private void StartHeating_Click(object sender, RoutedEventArgs e)
        {
            VM.StopHeatingButtonIsEnabled = true;
            VM.StartHeatingPressed = true;
            VM.RightPanelTextBoxsIsEnabled = false;
            Chart.SimulateStart();
        }

        private void StopHeating_Click(object sender, RoutedEventArgs e)
        {
            VM.AmplitudeControlCurrentTextBoxIsEnabled = false;
            VM.DurationHeatingCurrentPulseTextBoxIsEnabled = false;
            VM.StopHeatingButtonIsEnabled = false;
            VM.StartHeatingButtonIsEnabled = false;
            VM.StopGraduationButtonIsEnabled = true;
        }

        private void StopGraduation_Click(object sender, RoutedEventArgs e)
        {
            VM.StopGraduationButtonIsEnabled = false;
            VM.CutButtonIsEnabled = true;
            VM.LineSeriesCursorLeftVisibility = true;
            VM.LineSeriesCursorRightVisibility = true;
            VM.RightPanelTextBoxsIsEnabled = false;
            Chart.SimulateStop();
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
