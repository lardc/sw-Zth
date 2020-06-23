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

        public double FirstPulseDuration { get; set; }
        public double LastPulseDuration { get; set; }
        public double PauseDurationBetweenAdjacentPulses { get; set; }
        public double AmplitudePulsePulsedHeatingCurrent { get; set; }
        public double AmplitudeHeatingCurrentPulse { get; set; }
        public double AmplitudeControlCurrent { get; set; }
        public double AmplitudeMeasuringCurrent { get; set; }
        public double TSPMeasurementDelayTime { get; set; }

    }
}
