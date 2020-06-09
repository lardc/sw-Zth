using LiveCharts.Configurations;
using LiveCharts.Defaults;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

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


        public bool AxisYDegreesCelsius { get; set; }
        public bool AxisYDegreesMegawatts { get; set; }
        public bool AxisYDegreesAmperes { get; set; }
        public bool AxisYDegreesKilowatts { get; set; }


        #region Right panel
        ///////Zth long impulse


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

        public bool AxisYDegreesCelsiusIsEnabled { get; set; }
        public bool AxisYMegawattsIsEnabled { get; set; }
        public bool AxisYKilowattsIsEnabled { get; set; }
        public bool AxisYAmperesIsEnabled { get; set; }

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
            if (value > 0)
                return "сек";
            else
                return new Dictionary<double, string>()
                {
                    {-5, "0,00001" },
                    {-4, "0,0001" },
                    {-3, "0,001" },
                    {-2, "0,01" },
                    {-1, "0,1" },
                    {0, "1" },
                }[value];
        };

        private CartesianMapper<ObservablePoint> _mapper;


        public CommonVM()
        {
            _mapper = Mappers.Xy<ObservablePoint>()
             .X(point => Math.Log(point.X, Base)) //a 10 base log scale in the X axis
             .Y(point => point.Y);
        }

        #endregion
    }
}
