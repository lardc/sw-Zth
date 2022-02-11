using Newtonsoft.Json;
using System.IO;
using System.Windows.Navigation;
using Zth.VM;

namespace Zth
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public NavigationService NavigationService { get; private set; }
        public TopPanelVm TopPanelVM { get; set; } = new TopPanelVm()
        {
            DataIsVisibly = true,
            WorkingMode = WorkingMode.ZthLongImpulse
        };
        public CommonVM FrameVM { get; set; } = new CommonVM();
        public BottomPanelVM BottomPanelVM { get; set; } = new BottomPanelVM();

        public static SettingsModel SettingsModel { private set; get; }

        public MainWindow()
        {

            SettingsModel = JsonConvert.DeserializeObject<SettingsModel>(File.ReadAllText("appsetting.json"));
            InitializeComponent();
            NavigationService = MainFrame.NavigationService;

            FrameVM.SetParentFrameVM = (CommonVM commonVm) =>
           {
               commonVm.SetParentFrameVM = FrameVM.SetParentFrameVM;
               FrameVM = commonVm;
           };

            BottomPanelVM.SetParentFrameVM = (BottomPanelVM bottomPanelVM) =>
            {
                bottomPanelVM.SetParentFrameVM = BottomPanelVM.SetParentFrameVM;
                BottomPanelVM = bottomPanelVM;
            };
        }

        


        private void SelectFirstPage()
        {
            BottomPanelVM.MiddleButtonContent = string.Empty;
            BottomPanelVM.RightButtonContent = Properties.Resource.Next;

        }


        private void RightButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            BottomPanelVM.RightBottomButtonAction();
        }

        private void MiddleButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            BottomPanelVM.MiddleBottomButtonAction();
        }

        private void LeftButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NavigationService.CanGoBack)
                NavigationService.GoBack();
            else
                NavigationService.Navigate(new Pages.SelectPage()
                {
                    DataContext = TopPanelVM,
                });
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            NavigationService.Navigate(new Pages.SelectPage());
            //Запуск установки
            App.LogicContainer.EnablePower();
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            //Выключение установки
            App.LogicContainer.DisablePower();
        }
    }
}
