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


            DataContext = new GraduationOnlyVM();
            FrameVM.SetParentFrameVM(VM);

            VM.TemperatureSensitiveParameterIsVisibly = true;
            VM.AnodeBodyTemperatureIsVisibly = true;
            VM.CathodeBodyTemperatureIsVisibly = true;
            VM.AnodeCoolerTemperatureIsVisibly = true;
            VM.CathodeCoolerTemperatureIsVisibly = true;

            VM.AxisYDegreesCelsiusIsVisibly = true;
            VM.AxisYMegawattsIsVisibly = true;

            VM.StartHeatingButtonIsEnabled = true;
            VM.CutButtonIsEnabled = false;
            VM.StopGraduationButtonIsEnabled = false;

            VM.StopHeatingButtonIsEnabled = VM.StartHeatingPressed;

        }

        private void StartHeating_Click(object sender, RoutedEventArgs e)
        {
            VM.StopHeatingButtonIsEnabled = true;
            VM.StartHeatingPressed = true;
        }

        private void StopHeating_Click(object sender, RoutedEventArgs e)
        {
            VM.StopHeatingButtonIsEnabled = false;
            VM.StartHeatingButtonIsEnabled = false;
            VM.StopGraduationButtonIsEnabled = true;
        }

        private void StopGraduation_Click(object sender, RoutedEventArgs e)
        {
            VM.StopGraduationButtonIsEnabled = false;
            VM.CutButtonIsEnabled = true;
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            VM.CutButtonIsEnabled = false;
            BottomPanelVM.RightButtonIsEnabled = true;
        }

        private void CommonPage_Unloaded(object sender, RoutedEventArgs e)
        {
            if (CanLoadInFile)
                BottomPanelVM.MiddleButtonContent = string.Empty;
            BottomPanelVM.MiddleButtonIsEnabled = false;
            BottomPanelVM.RightButtonContent = string.Empty;
        }
    }
}
