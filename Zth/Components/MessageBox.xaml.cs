using System.Windows;

namespace Zth.Components
{
    /// <summary>
    /// MessageBox
    /// </summary>
    public partial class MessageBox : Window
    {
        private static MessageBoxResult Result;

        public MessageBox()
        {
            InitializeComponent();
        }

        public static MessageBoxResult Show(string text, string title = "Оповещение", MessageBoxButton buttons = MessageBoxButton.OK)
        {
            App.Current.Dispatcher.Invoke(() =>
            {
                MessageBox MessageBox = new MessageBox();
                MessageBox.MessageText.Text = text;
                MessageBox.MessageTitle.Text = title;
                if (buttons == MessageBoxButton.OK)
                    MessageBox.Cancel.Visibility = Visibility.Collapsed;
                MessageBox.ShowDialog();
            });
            return Result;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }
    }
}
