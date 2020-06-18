using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Zth.Components
{
    /// <summary>
    /// Логика взаимодействия для BottomCheckBoxsAndTable.xaml
    /// </summary>
    public partial class LineSeriesCursor : StackPanel
    {
        public LineSeriesCursor()
        {
            InitializeComponent();
        }

        private Point? _oldPosition;

        private void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _oldPosition = e.GetPosition((IInputElement)sender);
            System.Diagnostics.Debug.WriteLine(_oldPosition);
        }

        private void StackPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (_oldPosition != null)
            {
                Margin = new Thickness(Margin.Left + (e.GetPosition((IInputElement)sender) - _oldPosition).Value.X, 0, 0, 0);
            }

        }

        private void StackPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _oldPosition = null;
            Debug.WriteLine(_oldPosition);
        }


        private void StackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Rectangle.Height = ActualHeight;
        }

        private void StackPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            _oldPosition = null;
        }
    }
}
