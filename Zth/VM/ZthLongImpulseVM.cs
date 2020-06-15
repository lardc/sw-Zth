using PropertyChanged;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Zth.VM
{
    public class ZthLongImpulseVM : CommonVM
    {
    

        public double DurationPowerPulse { get; set; }
        public double AmplitudeHeatingCurrent { get; set; }
        public double AmplitudeControlCurrent { get; set; }
        public double AmplitudeMeasuringCurrent { get; set; }
        public double DelayTimeTspMeasurements { get; set; }
        public double DurationHeatingCurrentPulse { get; set; }
        public double PauseDuration { get; set; }

        public bool StartHeatingButtonIsEnabled { get; set; }
        public bool StopMeasurementButtonIsEnabled { get; set; }

        




    }
}
