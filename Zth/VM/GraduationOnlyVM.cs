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
        public double EndValueCaseTemperature { get; set; } = 27;
        public double DuratioHeatingCurrentPulse { get; set; } = 50;
        public double PauseDuration { get; set; } = 1;
        public double AmplitudeHeatingCurrent { get; set; } = 100;
        public double DirectCurrentControlValue { get; set; } = 1000;
        public double DirectCurrentMeasuringValue { get; set; } = 1000;
        public double TSPMeasurementDelayTime { get; set; } = 900;


        public bool DurationHeatingCurrentPulseTextBoxIsEnabled { get; set; } = true;
        public bool AmplitudeControlCurrentTextBoxIsEnabled { get; set; } = true;

        public bool IsZth { get; set; }
    }
}
