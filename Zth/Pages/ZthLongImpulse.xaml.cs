using System;
using System.Collections.Generic;
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

namespace Zth.Pages
{
    /// <summary>
    /// Логика взаимодействия для ZthLongImpulse.xaml
    /// </summary>
    public partial class ZthLongImpulse : Page
    {
        public CommonVM Vm { get; set; } = new CommonVM()
        {
            HeatingCurrentIsVisibly = true,
            HeatingPowerIsVisibly = true,
            AnodeBodyTemperatureIsVisibly = true,
            CathodeBodyTemperatureIsVisibly = true,
            AnodeCoolerTemperatureIsVisibly = true,
            CathodeCoolerTemperatureIsVisibly = true
        };

        public ZthLongImpulse()
        {
            InitializeComponent();
        }
    }
}
