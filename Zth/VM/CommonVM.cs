using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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





        #region Right panel
        ///////Zth long impulse
        public double DurationPowerPulse { get; set; }
        public double AmplitudeHeatingCurrent { get; set; }
        public double AmplitudeControlCurrent { get; set; }
        public double AmplitudeMeasuringCurrent { get; set; }
        public double DelayTimeTspMeasurements { get; set; }
        public double DurationHeatingCurrentPulse {get;set;}
        public double PauseDuration { get; set; }

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
    }
}
