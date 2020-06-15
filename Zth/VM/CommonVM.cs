using LiveCharts.Configurations;
using LiveCharts.Defaults;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using LiveCharts;
using LiveCharts.Wpf;

namespace Zth.VM
{
    public class CommonVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TypeCooling TypeCooling { get; set; }
        public TypeDevice TypeDevice { get; set; }
        public WorkingMode WorkingMode { get; set; }


        public string TypeCoolingString => StringResources.TypeCoolingDictionary[TypeCooling];
        public string TypeDeviceString => StringResources.TypesDeviceDictionary[TypeDevice];
        public string WorkingModeString => StringResources.WorkModeDictionary[WorkingMode];



        public bool StopHeatingButtonIsEnabled { get; set; }

        #region Callback

        public Action<CommonVM> SetParentFrameVM { get; set; }

        #endregion


        #region Right panel

        public bool StartHeatingPressed { get; set; }
        [AlsoNotifyFor(nameof(StartHeatingPressed))]
        public string StartHeatingContent => StartHeatingPressed ? Properties.Resource.UpdateTask : Properties.Resource.StartHeating;

        #endregion





        #region Bottom parameters

        public double Time { get; set; }
        public double HeatingPower { get; set; }
        public double AnodeBodyTemperature { get; set; }
        public double AnodeCoolerTemperature { get; set; }
        public double CathodeBodyTemperature { get; set; }
        public double CathodeCoolerTemperature { get; set; }
        public double HeatingCurrent { get; set; }
        public double TemperatureSensitiveParameter { get; set; }
        public double TemperatureStructure { get; set; }
        public double Ztha { get; set; }
        public double Zthk { get; set; }
        public double Zth { get; set; }

        public bool HeatingCurrentIsEnabled { get; set; }
        public bool HeatingPowerIsEnabled { get; set; }
        public bool TemperatureSensitiveParameterIsEnabled { get; set; }
        public bool AnodeBodyTemperatureIsEnabled { get; set; }
        public bool CathodeBodyTemperatureIsEnabled { get; set; }
        public bool AnodeCoolerTemperatureIsEnabled { get; set; }
        public bool CathodeCoolerTemperatureIsEnabled { get; set; }
        public bool TemperatureStructureIsEnabled { get; set; }
        public bool ZthaIsEnabled { get; set; }
        public bool ZthkIsEnabled { get; set; }
        public bool ZthIsEnabled { get; set; }


        public bool HeatingCurrentIsVisibly { get; set; } 
        public bool HeatingPowerIsVisibly{ get; set; } 
        public bool TemperatureSensitiveParameterIsVisibly{ get; set; } 
        public bool AnodeBodyTemperatureIsVisibly{ get; set; } 
        public bool CathodeBodyTemperatureIsVisibly{ get; set; }
        public bool AnodeCoolerTemperatureIsVisibly{ get; set; } 
        public bool CathodeCoolerTemperatureIsVisibly{ get; set; }
        public bool TemperatureStructureIsVisibly { get; set; }
        public bool ZthaIsVisibly { get; set; }
        public bool ZthkIsVisibly { get; set; }
        public bool ZthIsVisibly { get; set; }
        #endregion

        #region Chart


        public Brush HeatingPowerBrush { get; set; } = Brushes.Green;
        public Brush AnodeBodyTemperatureBrush { get; set; } = Brushes.Gray;
        public Brush AnodeCoolerTemperatureBrush { get; set; } = Brushes.Yellow;
        public Brush CathodeBodyTemperatureBrush { get; set; } = Brushes.Red;
        public Brush CathodeCoolerTemperatureBrush { get; set; } = Brushes.Pink;
        public Brush HeatingCurrentBrush { get; set; } = Brushes.HotPink;
        public Brush TemperatureSensitiveParameterBrush { get; set; } = Brushes.Chocolate;
        public Brush TemperatureStructureBrush { get; set; } = Brushes.Blue;
        public Brush ZthaBrush { get; set; } = Brushes.Brown;
        public Brush ZthkBrush { get; set; } = Brushes.Cyan;
        public Brush ZthBrush { get; set; } = Brushes.Khaki;
        
        
        public ChartValues<ObservablePoint> HeatingPowerChartValues { get; set; } = new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> AnodeBodyTemperatureChartValues { get; set; } = new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> AnodeCoolerTemperatureChartValues { get; set; } = new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> CathodeBodyTemperatureChartValues { get; set; } = new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> CathodeCoolerTemperatureChartValues { get; set; }= new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> HeatingCurrentChartValues { get; set; } = new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> TemperatureSensitiveParameterChartValues { get; set; } = new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> TemperatureStructureChartValues { get; set; } = new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> ZthaChartValues { get; set; } = new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> ZthkChartValues { get; set; } = new ChartValues<ObservablePoint>();
        public ChartValues<ObservablePoint> ZthChartValues { get; set; } = new ChartValues<ObservablePoint>();


        public SeriesCollection SeriesCollection { get; set; }
        //public bool AxisYDegreesCelsius { get; set; }
        //public bool AxisYDegreesMegawatts { get; set; }
        //public bool AxisYDegreesAmperes { get; set; }
        //public bool AxisYDegreesKilowatts { get; set; }

        public bool AxisYDegreesCelsiusIsEnabled { get; set; } = true;
        public bool AxisYMegawattsIsEnabled { get; set; } = true;
        public bool AxisYKilowattsIsEnabled { get; set; } = true;
        public bool AxisYAmperesIsEnabled { get; set; } = true;

        public bool AxisYDegreesCelsiusIsVisibly { get; set; }
        public bool AxisYMegawattsIsVisibly { get; set; }
        public bool AxisYKilowattsIsVisibly { get; set; }
        public bool AxisYAmperesIsVisibly { get; set; }

        public double Base { get; set; } = 10;

        public Func<double, string> FormatterDegreesCelsius { get; set; } = value =>
        {
            if (value == 95)
                return Properties.Resource.UnitMeasurementDegreeCentigrade;
            else if (new double[] { 60, 70, 80, 90 }.Contains(value))
                return value.ToString();
            return string.Empty;
        };

        public Func<double, string> FormatterMegawatts { get; set; } = value =>
        {
            if (value == 450)
                return "мВ";
            else if (new double[] { 370, 390, 410, 430 }.Contains(value))
                return value.ToString();
            return string.Empty;
        };

        public Func<double, string> FormatterKilowatts { get; set; } = value =>
        {
            if (value == 510)
                return "кВ";
            else if (new double[] { 490, 495, 500, 505 }.Contains(value))
                return value.ToString();
            return string.Empty;
        };

        public Func<double, string> FormatterAmperes { get; set; } = value =>
        {
            if (value == 1550)
                return "А";
            else if (new double[] { 1200, 1300, 1400, 1500 }.Contains(value))
                return value.ToString();
            return string.Empty;
        };

        public Func<double, string> FormatterTimes { get; set; } = value =>
        {
            var axis = new Dictionary<double, string>()
                {
                    {-5, "0,00001" },
                    {-4, "0,0001" },
                    {-3, "0,001" },
                    {-2, "0,01" },
                    {-1, "0,1" },
                    {0, "1" },
                };
            if (value > 1)
                return "сек";
            else if (axis.ContainsKey(value))
                return axis[value];
            else
                return Math.Pow(10, value).ToString("F6");
        };
        
       

        public CartesianMapper<ObservablePoint> Mapper { get; set; }


        public CommonVM()
        {
            Mapper = Mappers.Xy<ObservablePoint>()
             .X(point => Math.Log(point.X, Base)) //a 10 base log scale in the X axis
             .Y(point => point.Y);
            SeriesCollection = new SeriesCollection(Mapper)
            {
                //new LineSeries()
                //{
                //    Values = TemperatureStructureChartValues,
                //    ScalesYAt = 0,
                //    Fill = TemperatureStructureBrush

                //},
                //new LineSeries()
                //{
                //    Values = AnodeBodyTemperatureChartValues,
                //    ScalesYAt = 0
                //},
                //new LineSeries()
                //{
                //    Values = AnodeCoolerTemperatureChartValues,
                //    ScalesYAt = 0
                //},
                //  new LineSeries()
                //{
                //    Values = CathodeBodyTemperatureChartValues,
                //    ScalesYAt = 0
                //},
                //new LineSeries()
                //{
                //    Values = CathodeCoolerTemperatureChartValues,
                //    ScalesYAt = 0
                //},

                //new LineSeries()
                //{
                //    Values = CathodeCoolerTemperatureChartValues,
                //    ScalesYAt = 0
                //},
                //new LineSeries()
                //{
                //    Values = CathodeCoolerTemperatureChartValues,
                //    ScalesYAt = 0
                //},
            };
        }

        #endregion
    }
}
