using LiveCharts;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zth.VM
{
    public class GraduationZthRthVM : CommonVM
    {
        public double BottomLineExtrapolation { get; set; } = 60;
        public double UpperLineExtrapolation { get; set; } = 100;

        public double BottomLineExtrapolationAxis { get; set; } = 60;
        public double UpperLineExtrapolationAxis { get; set; } = 100;

        public bool IsCalculateZth { get; set; }

        public ChartValues<ObservablePoint> GraduationChartValues { get; set; } = new ChartValues<ObservablePoint>();


        public double MinMegawatts { get; set; } = 370;
        public double MaxMegawatts { get; set; } = 450;
        public double StepMegawatts { get; set; } = 5;

        public new Func<double, string> FormatterMegawatts=> value =>
        {
            int count = (int)Math.Round((MaxMegawatts - MinMegawatts) / StepMegawatts);
            if (Math.Round(Math.Abs((value - MinMegawatts) / StepMegawatts)) == count)
                return "ТЧП, мВ";
            if (Math.Round((value - MinMegawatts) / StepMegawatts) % 2 == 0)
                return Math.Round(value).ToString();
            if (Math.Round((value - MinMegawatts) / StepMegawatts) % 2 == 1)
                return string.Empty;
            return Math.Round(value).ToString();
        };

        public new Func<double, string> FormatterDegreesCelsius => value =>
        {
            if (value == UpperLineExtrapolation)
                return Properties.Resource.UnitMeasurementDegreeCentigrade;
            return value.ToString();
            //else if (new double[] { 60, 70, 80, 90 }.Contains(value))
            //    return value.ToString();
            //return string.Empty;
        };

    }
}
