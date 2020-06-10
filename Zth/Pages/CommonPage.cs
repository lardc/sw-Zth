using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Zth.VM;

namespace Zth.Pages
{
    public class CommonPage : Page
    {
        protected NavigationService _navigationService;
        public BottomPanelVM BottomPanelVM { get; set; }
        public TopPanelVm TopPanelVm  {get;set;}
        public VM.CommonVM FrameVM { get; set; }

        public CommonPage()
        {
            Loaded += CommonPage_Loaded;
        }

        private void CommonPage_Loaded(object sender, RoutedEventArgs e)
        {
            _navigationService = NavigationService.GetNavigationService(this);
            var wnd = Window.GetWindow(this) as MainWindow;
            if (wnd != null)
            {
                FrameVM = wnd.FrameVM;
                TopPanelVm = wnd.TopPanelVM;
                BottomPanelVM = wnd.BottomPanelVM;
            }
        }
    }
}