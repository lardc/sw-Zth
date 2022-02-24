namespace Zth.VM
{
    public class ZthPulseSequenceVM : CommonVM
    {
        public bool StartMeasurementButtonEnabled { get; set; }
        public bool StopMeasurementButtonEnabled { get; set; }

        public GateVoltage GateVoltage { get; set; }
        public double FirstPulseDuration { get; set; } = 0.2;
        public double LastPulseDuration { get; set; } = 0.02;
        public double PauseDurationBetweenAdjacentPulses { get; set; } = 200;
        public double AmplitudePulsePulsedHeatingCurrent { get; set; }
        public double AmplitudeHeatingCurrentLess2 { get; set; } = 300;
        public double AmplitudeHeatingCurrentLess10 { get; set; } = 200;
        public double AmplitudeHeatingCurrentAbove10 { get; set; } = 100;
        public double AmplitudeControlCurrent { get; set; } = 1000;
        public double AmplitudeMeasuringCurrent { get; set; } = 1000;
        public double TSPMeasurementDelayTime { get; set; } = 750;

    }
}
