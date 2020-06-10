using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Zth.VM
{
    public class BottomPanelVM : INotifyPropertyChanged
    {
        public string MiddleButtonContent { get; set; }
        public string RightButtonContent { get; set; }


        public bool LeftButtonIsEnabled { get; set; }
        public bool MiddleButtonIsEnabled { get; set; }
        public bool RightButtonIsEnabled { get; set; }


        public Action MiddleBottomButtonAction { get; set; }
        public Action RightBottomButtonAction { get; set; }

        [DependsOn(nameof(MiddleButtonContent))]
        public bool MiddleButtonIsVisibly => !string.IsNullOrWhiteSpace(MiddleButtonContent);
        [DependsOn(nameof(RightButtonContent))]
        public bool RightButtonIsVisibly => !string.IsNullOrWhiteSpace(RightButtonContent);

        public event PropertyChangedEventHandler PropertyChanged;


        #region Callback

        public Action<CommonVM> SetParentFrameVM { get; set; }

        #endregion
    }
}
