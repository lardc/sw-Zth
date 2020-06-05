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
            DataIsVisibly = true
        };
        public CommonVM FrameVm { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            _navigationService = NavigationService.GetNavigationService(this);
            _navigationService = MainFrame.NavigationService;
            //_navigationService.Source = new System.Uri("Pages/SelectPage.xaml", System.UriKind.Relative);
            _navigationService.Navigate(new Pages.ZthLongImpulse()
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
            });
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
