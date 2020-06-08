using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Zth.VM
{
    public class BottomPanelVM : INotifyPropertyChanged
    {
        public string LeftButtonContent { get; set; }
        public string MiddleButtonContent { get; set; }
        public string RightButtonContent { get; set; }



        [DependsOn(nameof(LeftButtonContent))]
        public bool LeftButtonIsVisibly => !string.IsNullOrWhiteSpace(LeftButtonContent);
        [DependsOn(nameof(MiddleButtonContent))]
        public bool MiddleButtonIsVisibly => !string.IsNullOrWhiteSpace(MiddleButtonContent);
        [DependsOn(nameof(RightButtonContent))]
        public bool RightButtonIsVisibly => !string.IsNullOrWhiteSpace(RightButtonContent);

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
