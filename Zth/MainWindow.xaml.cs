using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
        private readonly NavigationService _navigationService;
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
            _navigationService = MainFrame.NavigationService;

            FrameVM.SetParentFrameVM = (CommonVM commonVm) =>
           {
               commonVm.SetParentFrameVM = FrameVM.SetParentFrameVM;
               commonVm.StartHeatingPressed = FrameVM.StartHeatingPressed;
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

        private void LeftButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_navigationService.CanGoBack)
                _navigationService.GoBack();
            else
                _navigationService.Navigate(new Pages.SelectPage()
                {
                    DataContext = TopPanelVM,
                });
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _navigationService.Navigate(new Pages.SelectPage());
        }
    }
}
