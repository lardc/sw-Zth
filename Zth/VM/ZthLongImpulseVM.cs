namespace Zth.VM
{
    public class ZthLongImpulseVM : CommonVM
    {


        public double DurationPowerPulse { get; set; } = 20;
        public double AmplitudeHeatingCurrent { get; set; } = 100;
        public double AmplitudeControlCurrent { get; set; } = 1000;
        public double AmplitudeMeasuringCurrent { get; set; } = 1000;
        public double DelayTimeTspMeasurements { get; set; } = 100;
        public double DurationHeatingCurrentPulse { get; set; }
        public double PauseDuration { get; set; }
        public GateVoltage GateVoltage { get; set; }

        public bool StartHeatingButtonIsEnabled { get; set; }
        public bool StopMeasurementButtonIsEnabled { get; set; }

        




    }
}
