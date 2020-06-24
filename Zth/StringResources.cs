using System.Collections.Generic;

namespace Zth
{
    public static class StringResources
    {

        // ReSharper disable once CollectionNeverQueried.Global
        public static readonly Dictionary<TypeCooling, string> TypeCoolingDictionary = new Dictionary<TypeCooling, string>
        {
            {TypeCooling.OneSided, Properties.Resource.OneSided},
            {TypeCooling.TwoWay, Properties.Resource.TwoWay }
        };
        
        // ReSharper disable once CollectionNeverQueried.Global
        public static readonly Dictionary<TypeDevice, string> TypesDeviceDictionary = new Dictionary<TypeDevice, string>
        {
            {TypeDevice.Bipolar, Properties.Resource.Bipolar},
            {TypeDevice.Igbt, Properties.Resource.IGBT }
        };

        // ReSharper disable once CollectionNeverQueried.Global
        public static readonly Dictionary<WorkingMode, string> WorkModeDictionary = new Dictionary<WorkingMode, string>
        {
            {WorkingMode.ZthLongImpulse, Properties.Resource.ZthLongImpulse},
            {WorkingMode.ZthSequence, Properties.Resource.ZthSequence},
            {WorkingMode.RthSequence, Properties.Resource.RthSequence},
            {WorkingMode.GraduationOnly, Properties.Resource.GraduationOnly}
        };

    }
}