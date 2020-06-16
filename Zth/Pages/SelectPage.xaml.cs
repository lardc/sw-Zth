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
    /// Логика взаимодействия для SelectPage.xaml
    /// </summary>
    public partial class SelectPage : CommonPage
    {

        public SelectPage() : base()
        {
            InitializeComponent();
        }

        private void SelectMeasument()
        {
            FrameVM.SetParentFrameVM(new CommonVM());

            switch (TopPanelVm.WorkingMode)
            {

                case WorkingMode.ZthLongImpulse:
                    _navigationService.Navigate(new Pages.ZthLongImpulse());
                    break;
                case WorkingMode.ZthSequence:
                    _navigationService.Navigate(new Pages.ZthPulseSequence());
                    break;
                case WorkingMode.RthSequence:
                    _navigationService.Navigate(new Pages.RthPulseSequence());
                    break;
                case WorkingMode.GraduationOnly:
                    _navigationService.Navigate(new Pages.GraduationOnly());
                    break;
            }
        }

        

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = TopPanelVm;
            BottomPanelVM.RightBottomButtonAction = SelectMeasument;
            BottomPanelVM.RightButtonIsEnabled = true;
            BottomPanelVM.RightButtonContent = Properties.Resource.Next;

            TopPanelVm.WorkingModeIsVisible = false;
            TopPanelVm.TypeDeviceIsVisible = false;
            TopPanelVm.TypeCoolingIsVisible = false;

            TopPanelVm.AnodeBodyTemperatureIsVisible = false;
            TopPanelVm.AnodeCoolerTemperatureIsVisible = false;
            TopPanelVm.CathodeBodyTemperatureIsVisible = false;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = false;
            TopPanelVm.HeatingCurrentIsVisible = false;
            TopPanelVm.TemperatureSensitiveParameterIsVisible = false;


            //////////////////////
            if (MainWindow.SettingsModel.Debug1)
            {
                var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler((sender1, e1) =>
                {
                    dispatcherTimer.Stop();
                    SelectMeasument();
                });
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                dispatcherTimer.Start();
            }
            
        }

        private void CommonPage_Unloaded(object sender, RoutedEventArgs e)
        {
            TopPanelVm.WorkingModeIsVisible = true;
            TopPanelVm.TypeDeviceIsVisible = true;
            TopPanelVm.TypeCoolingIsVisible = true;

            TopPanelVm.AnodeBodyTemperatureIsVisible = true;
            TopPanelVm.AnodeCoolerTemperatureIsVisible = true;
            TopPanelVm.CathodeBodyTemperatureIsVisible = true;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = true;
            TopPanelVm.HeatingCurrentIsVisible = true;
            TopPanelVm.TemperatureSensitiveParameterIsVisible = true;

            BottomPanelVM.RightButtonIsEnabled = false;
            BottomPanelVM.RightButtonContent = string.Empty;
        }
    }
}
