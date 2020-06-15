using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Documents;

namespace Zth.VM
{
    public class ZthPulseSequenceVM : CommonVM
    {
        public bool StartMeasurementButtonEnabled { get; set; }
        public bool StopMeasurementButtonEnabled { get; set; }
    }
}
