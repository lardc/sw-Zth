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
        public MainWindow()
        {
            InitializeComponent();
            _navigationService = NavigationService.GetNavigationService(this);
            _navigationService = MainFrame.NavigationService;
            //_navigationService.Source = new System.Uri("Pages/SelectPage.xaml", System.UriKind.Relative);
            _navigationService.Navigate(new Pages.ZthLongImpulse()
            {
                Vm = new CommonVM()
                {
                    WorkingMode = WorkingMode.ZthLongImpulse
                }
            });
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
