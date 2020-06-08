using System;
using System.Windows.Navigation;
using Zth.VM;

namespace Zth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly NavigationService _navigationService;
        public TopPanelVm TopPanelVm { get; set; } = new TopPanelVm()
        {
            DataIsVisibly = true,
            WorkingMode = WorkingMode.RthSequence
        };
        public CommonVM FrameVm { get; set; } = new CommonVM();
        public BottomPanelVM BottomPanelVM { get; set; } = new BottomPanelVM();
        public MainWindow()
        {
            InitializeComponent();


            _navigationService = NavigationService.GetNavigationService(this);
            _navigationService = MainFrame.NavigationService;
            _navigationService.Navigate(new Pages.SelectPage()
            {
                DataContext = TopPanelVm
            });

            SelectFirstPage();
            /*_navigationService.Navigate(new Pages.ZthLongImpulse()
            {
                DataContext = FrameVm = new CommonVM()
                {
                    HeatingCurrentIsVisibly = true,
                    HeatingPowerIsVisibly = true,
                    AnodeBodyTemperatureIsVisibly = true,
                    CathodeBodyTemperatureIsVisibly = true,
                    AnodeCoolerTemperatureIsVisibly = true,
                    CathodeCoolerTemperatureIsVisibly = true
                }
            });*/
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            FrameVm = new CommonVM();

            switch (TopPanelVm.WorkingMode)
            {
                case WorkingMode.ZthLongImpulse:

                    FrameVm.HeatingCurrentIsVisibly = true;
                    FrameVm.HeatingPowerIsVisibly = true;
                    FrameVm.AnodeBodyTemperatureIsVisibly = true;
                    FrameVm.CathodeBodyTemperatureIsVisibly = true;
                    FrameVm.AnodeCoolerTemperatureIsVisibly = true;
                    FrameVm.CathodeCoolerTemperatureIsVisibly = true;
                    _navigationService.Navigate(new Pages.ZthLongImpulse()
                    {
                        DataContext = FrameVm
                    });
                    BottomPanelVM.RightButtonContent = "Градуировка";
                    break;
                case WorkingMode.ZthSequence:
                    FrameVm.HeatingCurrentIsVisibly = true;
                    FrameVm.HeatingPowerIsVisibly = true;
                    FrameVm.TemperatureSensitiveParameterIsVisibly = true;
                    FrameVm.AnodeBodyTemperatureIsVisibly = true;
                    FrameVm.CathodeBodyTemperatureIsVisibly = true;
                    FrameVm.AnodeCoolerTemperatureIsVisibly = true;
                    FrameVm.CathodeCoolerTemperatureIsVisibly = true;
                    _navigationService.Navigate(new Pages.ZthPulseSequence()
                    {
                        DataContext = FrameVm
                    });
                    BottomPanelVM.RightButtonContent = "Градуировка";
                    break;
                case WorkingMode.RthSequence:
                    FrameVm.HeatingCurrentIsVisibly = true;
                    FrameVm.HeatingPowerIsVisibly = true;
                    FrameVm.TemperatureSensitiveParameterIsVisibly = true;
                    FrameVm.AnodeBodyTemperatureIsVisibly = true;
                    FrameVm.CathodeBodyTemperatureIsVisibly = true;
                    FrameVm.AnodeCoolerTemperatureIsVisibly = true;
                    FrameVm.CathodeCoolerTemperatureIsVisibly = true;
                    _navigationService.Navigate(new Pages.RthPulseSequence()
                    {
                        DataContext = FrameVm
                    });
                    BottomPanelVM.RightButtonContent = "Градуировка";
                    break;
                case WorkingMode.GraduationOnly:
                    FrameVm.TemperatureSensitiveParameterIsVisibly = true;
                    FrameVm.AnodeBodyTemperatureIsVisibly = true;
                    FrameVm.CathodeBodyTemperatureIsVisibly = true;
                    FrameVm.AnodeCoolerTemperatureIsVisibly = true;
                    FrameVm.CathodeCoolerTemperatureIsVisibly = true;
                    _navigationService.Navigate(new Pages.GraduationOnly()
                    {
                        DataContext = FrameVm
                    });
                    BottomPanelVM.MiddleButtonContent = "Загрузка из файла";
                    BottomPanelVM.RightButtonContent = "Расчёт градуировки";
                    break;


            }
        }

        private void SelectFirstPage()
        {
            BottomPanelVM.LeftButtonContent = Properties.Resource.Back;
            BottomPanelVM.MiddleButtonContent = string.Empty;
            BottomPanelVM.RightButtonContent = Properties.Resource.Next;

        }

        private void Button_Click_1(object sender, System.Windows.RoutedEventArgs e)
        {
            SelectFirstPage();

            if (_navigationService.CanGoBack)
                _navigationService.GoBack();
        }
    }
}
