using System;
using System.Collections.Generic;
using System.Text;

namespace Zth.VM
{
    public class GraduationOnlyVM : CommonVM
    {
        public bool StartHeatingButtonIsEnabled { get; set; }
        public bool StopGraduationButtonIsEnabled { get; set; }
        public bool CutButtonIsEnabled { get; set; }

        public GateVoltage GateVoltage { get; set; }
        public double EndValueCaseTemperature { get; set; }
        public double DuratioHeatingCurrentPulse { get; set; }
        public double PauseDuration { get; set; }
        public double AmplitudeHeatingCurrent { get; set; }
        public double DirectCurrentControlValue { get; set; }
        public double DirectCurrentMeasuringValue { get; set; }
        public double TSPMeasurementDelayTime { get; set; }


        public bool DurationHeatingCurrentPulseTextBoxIsEnabled { get; set; } = true;
        public bool AmplitudeControlCurrentTextBoxIsEnabled { get; set; } = true;

        public bool IsZth { get; set; }
    }
}
