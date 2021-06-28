using FHICORC.Core.Services.Model.EuDCCModel.ValueSet;

namespace FHICORC.Core.Services.Interface
{
    public interface IDgcValueSetTranslator
    {
        public void InitValueSet();
        public object Translate(DGCValueSetEnum key, string code);
        public string GetDGCCode(DGCValueSetEnum key, object value);
    }
}