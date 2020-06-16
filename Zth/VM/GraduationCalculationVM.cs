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


        public new Func<double, string> FormatterMegawatts { get; set; } = value =>
        {
            if (value == 450)
                return "ТЧП, мВ";
            else if (new double[] { 310, 330, 350, 370, 390, 410, 430 }.Contains(value))
                return value.ToString();
            return string.Empty;
        };

        public new Func<double, string> FormatterDegreesCelsius { get; set; } = value =>
        {
            if (value == 100)
                return Properties.Resource.UnitMeasurementDegreeCentigrade;
            return value.ToString();
            //else if (new double[] { 60, 70, 80, 90 }.Contains(value))
            //    return value.ToString();
            //return string.Empty;
        };

    }
}
