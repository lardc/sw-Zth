using System.Collections.Generic;

namespace Zth
{
    public enum TypeCooling
    {
        OneSided, TwoWay
    }

    public enum TypeDevices
    {
        Bipolar, Igbt
    }

    public enum WorkingMode
    {
        ZthLongImpulse, RthSequence, GraduationOnly
    }

    public static class StringResources
    {

        // ReSharper disable once CollectionNeverQueried.Global
        public static readonly Dictionary<TypeCooling, string> TypeCoolingDictionary = new Dictionary<TypeCooling, string>
        {
            {TypeCooling.OneSided, Properties.Resource.OneSided},
            {TypeCooling.TwoWay, Properties.Resource.TwoWay }
        };
        
        // ReSharper disable once CollectionNeverQueried.Global
        public static readonly Dictionary<TypeDevices, string> TypesDeviceDictionary = new Dictionary<TypeDevices, string>
        {
            {TypeDevices.Bipolar, Properties.Resource.Bipolar},
            {TypeDevices.Igbt, Properties.Resource.IGBT }
        };

        // ReSharper disable once CollectionNeverQueried.Global
        public static readonly Dictionary<WorkingMode, string> WorkModeDictionary = new Dictionary<WorkingMode, string>
        {
            {WorkingMode.ZthLongImpulse, Properties.Resource.ZthLongImpulse},
            {WorkingMode.RthSequence, Properties.Resource.RthSequence},
            {WorkingMode.GraduationOnly, Properties.Resource.GraduationOnly}
        };

    }

}
