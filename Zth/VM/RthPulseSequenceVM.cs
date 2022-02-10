using System;
using System.Collections.Generic;
using System.Text;

namespace Zth.VM
{
    public class RthPulseSequenceVM : CommonVM
    {
        public bool StartHeatingButtonIsEnabled { get; set; }
        
        public bool RecordingResultsButtonIsEnabled { get; set; }

        public GateVoltage GateVoltage { get; set; }
        public double DurationHeatingCurrentPulse { get; set; }
        public double PauseDuration { get; set; }
        public double AmplitudeHeatingCurrent { get; set; }
        public double AmplitudeControlCurrent { get; set; }
        public double AmplitudeMeasuringCurrent { get; set; }
        public double DelayTimeTspMeasurements { get; set; }
                
    }
}
