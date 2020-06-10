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
                    FrameVM.HeatingCurrentIsVisibly = true;
                    FrameVM.HeatingPowerIsVisibly = true;
                    FrameVM.TemperatureSensitiveParameterIsVisibly = true;
                    FrameVM.AnodeBodyTemperatureIsVisibly = true;
                    FrameVM.CathodeBodyTemperatureIsVisibly = true;
                    FrameVM.AnodeCoolerTemperatureIsVisibly = true;
                    FrameVM.CathodeCoolerTemperatureIsVisibly = true;
                    _navigationService.Navigate(new Pages.ZthPulseSequence()
                    {
                        DataContext = FrameVM
                    });
                    BottomPanelVM.RightButtonContent = "Градуировка";
                    break;
                case WorkingMode.RthSequence:
                  
                    _navigationService.Navigate(new Pages.RthPulseSequence());
                    
                    break;
                case WorkingMode.GraduationOnly:
                    FrameVM.TemperatureSensitiveParameterIsVisibly = true;
                    FrameVM.AnodeBodyTemperatureIsVisibly = true;
                    FrameVM.CathodeBodyTemperatureIsVisibly = true;
                    FrameVM.AnodeCoolerTemperatureIsVisibly = true;
                    FrameVM.CathodeCoolerTemperatureIsVisibly = true;
                    _navigationService.Navigate(new Pages.GraduationOnly()
                    {
                        DataContext = FrameVM
                    });
                    BottomPanelVM.MiddleButtonContent = "Загрузка из файла";
                    BottomPanelVM.RightButtonContent = "Расчёт градуировки";
                    break;
            }
        }

        

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = TopPanelVm;
            BottomPanelVM.RightBottomButtonAction = SelectMeasument;
            BottomPanelVM.RightButtonIsEnabled = true;
            BottomPanelVM.RightButtonContent = Properties.Resource.Next;



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
    }
}
