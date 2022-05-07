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
    public partial class CalculationRth : CommonPage
    {
        public bool CanLoadInFile { get; set; }
        public bool HeatingIsEnabled { get; set; }
        public Zth.VM.CommonVM VM => DataContext as CommonVM;

        public CalculationRth() : base()
        {
            InitializeComponent();

        }

        private void CommonPage_Loaded(object sender, RoutedEventArgs e)
        {
            TopPanelVm.WorkingModeString_ = Properties.Resource.CalculationRth;

            TopPanelVm.AnodeBodyTemperatureIsVisible = false;
            TopPanelVm.AnodeCoolerTemperatureIsVisible = false;
            TopPanelVm.CathodeBodyTemperatureIsVisible = false;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = false;
            TopPanelVm.HeatingCurrentIsVisible = false;
            TopPanelVm.TemperatureSensitiveParameterIsVisible = false;

            DataContext = new CommonVM();
            FrameVM.SetParentFrameVM(VM);

            VM.HeatingCurrentIsVisibly = true;
            VM.HeatingPowerIsVisibly = true;
            VM.TemperatureSensitiveParameterIsVisibly = true;
            VM.AnodeBodyTemperatureIsVisibly = true;
            VM.CathodeBodyTemperatureIsVisibly = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            VM.AnodeCoolerTemperatureIsVisibly = true;
            VM.CathodeCoolerTemperatureIsVisibly = TopPanelVm.TypeCooling != TypeCooling.OneSided;
            VM.TemperatureStructureIsVisibly = true;
            VM.ZthaIsVisibly = true;
            VM.ZthkIsVisibly = true;
            VM.ZthIsVisibly = true;

            VM.HeatingCurrentIsEnabled = true;
            VM.HeatingPowerIsEnabled = true;
            VM.TemperatureSensitiveParameterIsEnabled = true;
            VM.AnodeBodyTemperatureIsEnabled = true;
            VM.CathodeBodyTemperatureIsEnabled = true;
            VM.AnodeCoolerTemperatureIsEnabled = true;
            VM.CathodeCoolerTemperatureIsEnabled = true;
            VM.TemperatureStructureIsEnabled = true;
            VM.ZthaIsEnabled = true;
            VM.ZthkIsEnabled = true;
            VM.ZthIsEnabled = true;

            //VM.AxisYDegreesCelsiusIsVisibly = true;
            //VM.AxisYMegawattsIsVisibly = true;
            //VM.AxisYAmperesIsVisibly = true;
            //VM.AxisYKilowattsIsVisibly = true;
            //VM.AxisYDegreeCelsiusPerWattIsVisibly = true;

        }

        private void CommonPage_Unloaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.RightButtonContent = string.Empty;

            TopPanelVm.AnodeBodyTemperatureIsVisible = true;
            TopPanelVm.AnodeCoolerTemperatureIsVisible = true;
            TopPanelVm.CathodeBodyTemperatureIsVisible = true;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = true;
            TopPanelVm.HeatingCurrentIsVisible = true;
            TopPanelVm.TemperatureSensitiveParameterIsVisible = true;

            if (CanLoadInFile)
                BottomPanelVM.MiddleButtonContent = string.Empty;
            BottomPanelVM.MiddleButtonIsEnabled = false;
            BottomPanelVM.RightButtonContent = string.Empty;
        }
    }
}
