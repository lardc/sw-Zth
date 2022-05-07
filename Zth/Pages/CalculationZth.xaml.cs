using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using Zth.VM;

namespace Zth.Pages
{
    /// <summary>
    /// Логика взаимодействия для RthPulseSequence.xaml
    /// </summary>
    public partial class CalculationZth : CommonPage
    {
        public bool CanLoadInFile { get; set; }
        public bool HeatingIsEnabled { get; set; }
        public Zth.VM.CommonVM VM => DataContext as CommonVM;

        public CalculationZth() : base()
        {
            InitializeComponent();

        }

        private void CommonPage_Loaded(object sender, RoutedEventArgs e)
        {
            TopPanelVm.WorkingModeString_ = Properties.Resource.CalculationZth;

            TopPanelVm.AnodeBodyTemperatureIsVisible = false;
            TopPanelVm.AnodeCoolerTemperatureIsVisible = false;
            TopPanelVm.CathodeBodyTemperatureIsVisible = false;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = false;
            TopPanelVm.HeatingCurrentIsVisible = false;
            TopPanelVm.TemperatureSensitiveParameterIsVisible = false;

            DataContext = new CommonVM();
            FrameVM.SetParentFrameVM(VM);

            BottomPanelVM.RightButtonContent = Properties.Resource.SaveInFile;
            BottomPanelVM.RightBottomButtonAction = () => ZthToFile_Save();

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

            MainChart.SimulateStart();
        }

        private void ZthToFile_Save()
        {
            SaveFileDialog SFD = new SaveFileDialog()
            {
                Filter = "Excel Worksheets|*.csv"
            };
            if (SFD.ShowDialog() == true)
            {
                File.AppendAllText(SFD.FileName, string.Format("Греющий ток,Греющая мощность,ТЧП,Температура структуры,Zth\n"));
                for (int i = 0; i < VM.TemperatureStructureChartValues.Count; i++)
                    File.AppendAllText(SFD.FileName, string.Join(Environment.NewLine, string.Format("{0},{1},{2},{3},{4}", VM.HeatingCurrentChartValues[i].Y, VM.HeatingPowerChartValues[i].Y, VM.TemperatureSensitiveParameterChartValues[i].Y, VM.TemperatureStructureChartValues[i].Y, VM.ZthChartValues[i].Y)));
            }
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
