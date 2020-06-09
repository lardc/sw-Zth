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
    /// Логика взаимодействия для ZthLongImpulse.xaml
    /// </summary>
    public partial class ZthLongImpulse : Page
    {
        VM.ZthLongImpulseVM VM => DataContext as ZthLongImpulseVM;
        public BottomPanelVM BottomPanelVM { get; set; }

        public ZthLongImpulse()
        {
           
            InitializeComponent();
        }

        private void StartHeating_Click(object sender, RoutedEventArgs e)
        {
            VM.StartHeatingPressed = true;
            VM.StopHeatingButtonIsEnabled = true;
        }

        private void StopHeating_Click(object sender, RoutedEventArgs e)
        {
            VM.StartHeatingButtonIsEnabled = false;
            VM.StopHeatingButtonIsEnabled = false;
            VM.StopMeasurementButtonIsEnabled = true;
        }

        private void StopMeasurement_Click(object sender, RoutedEventArgs e)
        {
            VM.StopMeasurementButtonIsEnabled = false;
            BottomPanelVM.RightButtonIsEnabled = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.LeftButtonIsEnabled = true;
            BottomPanelVM.RightButtonContent = Properties.Resource.Back;
            BottomPanelVM.RightButtonIsEnabled = false;
            BottomPanelVM.RightButtonContent = Properties.Resource.Graduation;

            DataContext = new ZthLongImpulseVM()
            {
                StartHeatingButtonIsEnabled = true,
            };
            VM.HeatingCurrentIsVisibly = true;
            VM.HeatingPowerIsVisibly = true;
            VM.AnodeBodyTemperatureIsVisibly = true;
            VM.CathodeBodyTemperatureIsVisibly = true;
            VM.AnodeCoolerTemperatureIsVisibly = true;
            VM.CathodeCoolerTemperatureIsVisibly = true;
            VM.HeatingCurrentIsEnabled = true;

            VM.AxisYAmperesIsEnabled = true;
            VM.AxisYDegreesCelsiusIsEnabled = true;
            VM.AxisYKilowattsIsEnabled = true;
            VM.AxisYMegawattsIsEnabled = true;
        }
    }
}
