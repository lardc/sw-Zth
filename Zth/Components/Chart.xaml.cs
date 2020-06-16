using LiveCharts;
using LiveCharts.Configurations;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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

namespace Zth.Components
{
    /// <summary>
    /// Логика взаимодействия для Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        public CommonVM VM => DataContext as CommonVM;

        public Chart()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(@"Dataset.csv") == false)
                return;

            var reader = new StreamReader(File.OpenRead(@"Dataset.csv"));
            
            var line = reader.ReadLine();
            while (!reader.EndOfStream)
            {
                line = reader.ReadLine();
                var values = line.Split(',', StringSplitOptions.RemoveEmptyEntries);

                VM.HeatingCurrentChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) /5 , double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 10));
                VM.HeatingPowerChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.5, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) * 5));
                VM.TemperatureStructureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.5, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.5));
                VM.CathodeCoolerTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.4, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.4));
                VM.AnodeCoolerTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.3, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.3));
                VM.CathodeBodyTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.2, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.2));
                VM.AnodeBodyTemperatureChartValues.Add(new ObservablePoint(double.Parse(values[2].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.1, double.Parse(values[3].Replace(',', '.'), CultureInfo.InvariantCulture) / 1.1));
            }

        }
    }
}
