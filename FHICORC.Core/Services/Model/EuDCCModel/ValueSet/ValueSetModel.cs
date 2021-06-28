using System.Collections.Generic;

namespace FHICORC.Core.Services.Model.EuDCCModel.ValueSet
{

    public class ValueSetModel
    {
        public string ValueSetId { get; set; }
        public Dictionary<string, ValueSetItem> ValueSetValues { get; set; }
    }

    public class ValueSetItem
    {
        public string Display { get; set; }
        public bool Active { get; set; }
        public string Lang { get; set; }
    }
}