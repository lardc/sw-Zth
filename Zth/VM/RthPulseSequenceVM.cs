using System;
using System.Collections.Generic;
using System.Text;

namespace Zth.VM
{
    public class RthPulseSequenceVM : CommonVM
    {
        public bool StartHeatingButtonIsEnabled { get; set; }
        public bool StopHeatingButtonIsEnabled { get; set; }
        public bool RecordingResultsButtonIsEnabled { get; set; }
    }
}
