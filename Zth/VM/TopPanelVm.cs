using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Zth.VM
{
    public class TopPanelVm : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TypeCooling TypeCooling { get; set; }
        public TypeDevice TypeDevice { get; set; }
        public WorkingMode WorkingMode { get; set; }


        public string TypeCoolingString => StringResources.TypeCoolingDictionary[TypeCooling];
        public string TypeDeviceString => StringResources.TypesDeviceDictionary[TypeDevice];

        [AlsoNotifyFor(nameof(WorkingModeString))]
        public string WorkingModeString_ { get; set; } = null;
        public string WorkingModeString => WorkingModeString_ != null ? WorkingModeString_ : StringResources.WorkModeDictionary[WorkingMode];


        public bool TypeCoolingIsVisible { get; set; } = true;
        public bool TypeDeviceIsVisible { get; set; } = true;
        public bool WorkingModeIsVisible { get; set; } = true;

        public double AnodeBodyTemperature { get; set; }
        public double AnodeCoolerTemperature { get; set; }
        public double CathodeBodyTemperature { get; set; }
        public double CathodeCoolerTemperature { get; set; }
        public double HeatingCurrent { get; set; }
        public double TemperatureSensitiveParameter { get; set; }

        public bool AnodeBodyTemperatureIsVisible { get; set; } = true;
        public bool AnodeCoolerTemperatureIsVisible { get; set; } = true;
        public bool CathodeBodyTemperatureIsVisible { get; set; } = true;
        public bool CathodeCoolerTemperatureIsVisible { get; set; } = true;
        public bool HeatingCurrentIsVisible { get; set; } = true;
        public bool TemperatureSensitiveParameterIsVisible { get; set; } = true;


        public bool DataIsVisibly { get; set; } = true;
    }
}
