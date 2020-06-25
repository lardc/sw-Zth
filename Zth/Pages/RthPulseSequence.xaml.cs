﻿using System;
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
            BottomPanelVM.RightButtonContent = Properties.Resource.Graduation;
            BottomPanelVM.RightBottomButtonAction = () => _navigationService.Navigate(new GraduationOnly()
            {
                HeatingIsEnabled = true
            });

            DataContext = new RthPulseSequenceVM();
            FrameVM.SetParentFrameVM(VM);

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

        private void StartHeating_Click(object sender, RoutedEventArgs e)
        {
            VM.StopHeatingButtonIsEnabled = true;
            VM.StartHeatingButtonIsEnabled = true;
            VM.StartHeatingPressed = true;
            VM.RecordingResultsButtonIsEnabled = true;
            Chart.SimulateStart();
        }

        private void RecordingResults_Click(object sender, RoutedEventArgs e)
        {
            VM.StartHeatingButtonIsEnabled = false;
            VM.RecordingResultsButtonIsEnabled = false;
            BottomPanelVM.RightButtonIsEnabled = true;
            VM.RightPanelTextBoxsIsEnabled = false;
            Chart.SimulateStop();
        }

        private void StopHeating_Click(object sender, RoutedEventArgs e)
        {
            VM.StopHeatingButtonIsEnabled = false;
            VM.StartHeatingPressed = false;
            _navigationService.Navigate(new GraduationOnly()
            {
                CanLoadInFile = true,
            });
            
        }

        private void CommonPage_Unloaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.RightButtonIsEnabled = false;
            BottomPanelVM.RightButtonContent = string.Empty;
        }
    }
}
