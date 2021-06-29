using System.Collections.Generic;
using FHICORC.Core.Services.Model.EuDCCModel.ValueSet;

namespace FHICORC.Core.Services.Interface
{
    public interface IDgcValueSetTranslator
    {
        List<ValueSetModel> ValueSetModels { get; set; }
        public void InitValueSet();
        public object Translate(DGCValueSetEnum key, string code);
        public string GetDGCCode(DGCValueSetEnum key, object value);
    }
}