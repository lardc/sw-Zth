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

        public bool IsZth { get; set; }
    }
}
