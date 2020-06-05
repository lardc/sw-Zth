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
        public string WorkingModeString => StringResources.WorkModeDictionary[WorkingMode];


        public double AnodeBodyTemperature { get; set; }
        public double AnodeCoolerTemperature { get; set; }
        public double CathodeBodyTemperature { get; set; }
        public double CathodeCoolerTemperature { get; set; }
        public double HeatingCurrent { get; set; }
        public double TemperatureSensitiveParameter { get; set; }


        public bool DataIsVisibly { get; set; } = true;
    }
}
