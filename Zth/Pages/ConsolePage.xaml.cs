using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Zth.Pages
{
    /// <summary>
    /// Логика взаимодействия для ConsolePage.xaml
    /// </summary>
    public partial class ConsolePage : CommonPage
    {
        public ConsolePage()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            BottomPanelVM.LeftButtonIsEnabled = true;
            BottomPanelVM.MiddleButtonContent = "";

            TopPanelVm.WorkingModeIsVisible = false;
            TopPanelVm.TypeDeviceIsVisible = false;
            TopPanelVm.TypeCoolingIsVisible = false;

            TopPanelVm.AnodeBodyTemperatureIsVisible = false;
            TopPanelVm.AnodeCoolerTemperatureIsVisible = false;
            TopPanelVm.CathodeBodyTemperatureIsVisible = false;
            TopPanelVm.CathodeCoolerTemperatureIsVisible = false;
            TopPanelVm.HeatingCurrentIsVisible = false;
            TopPanelVm.TemperatureSensitiveParameterIsVisible = false;

            RegOpResult.Visibility = Visibility.Hidden;
            CallOpResult.Visibility = Visibility.Hidden;
        }

        private void Read_Click(object sender, RoutedEventArgs e) //Чтение регистра
        {
            try
            {
                RegValue.Text = App.LogicContainer.ReadRegister(ushort.Parse(RegAddress.Text)).ToString();
                Result_Show(RegOpResult, true);
            }
            catch
            {
                Result_Show(RegOpResult, false);
            }
        }

        private void Write_Click(object sender, RoutedEventArgs e) //Запись регистра
        {
            try
            {
                App.LogicContainer.WriteRegister(ushort.Parse(RegAddress.Text), ushort.Parse(RegValue.Text));
                Result_Show(RegOpResult, true);
            }
            catch
            {
                Result_Show(RegOpResult, false);
            }
        }

        private void Call_Click(object sender, RoutedEventArgs e) //Вызов функции
        {
            try
            {
                App.LogicContainer.CallAction(ushort.Parse(CallAddress.Text));
                Result_Show(CallOpResult, true);
            }
            catch
            {
                Result_Show(CallOpResult, false);
            }
        }

        private async void Result_Show(object sender, bool isOk) //Отображение результата операции
        {
            TextBlock Result = (TextBlock)sender;
            switch (isOk)
            {
                case true:
                    Result.Text = "Операция выполнена успешно";
                    Result.Foreground = Brushes.Green;
                    break;
                case false:
                    Result.Text = "Операция завершилась с ошибкой";
                    Result.Foreground = Brushes.Red;
                    break;
            }
            Result.Visibility = Visibility.Visible;
            await Task.Delay(1000);
            Result.Visibility = Visibility.Hidden;
        }
    }
}
