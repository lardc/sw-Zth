namespace Zth.VM
{
    public class RthPulseSequenceVM : CommonVM
    {
        public bool StartHeatingButtonIsEnabled { get; set; }
        
        public bool RecordingResultsButtonIsEnabled { get; set; }

        public GateVoltage GateVoltage { get; set; }
        public double DurationHeatingCurrentPulse { get; set; } = 50;
        public double PauseDuration { get; set; } = 1;
        public double AmplitudeHeatingCurrent { get; set; } = 100;
        public double AmplitudeControlCurrent { get; set; } = 1000;
        public double AmplitudeMeasuringCurrent { get; set; } = 1000;
        public double DelayTimeTspMeasurements { get; set; } = 900;
                
    }
}
