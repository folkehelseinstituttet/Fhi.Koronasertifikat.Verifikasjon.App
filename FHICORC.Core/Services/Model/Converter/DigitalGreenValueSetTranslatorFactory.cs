using FHICORC.Core.Services.Interface;
using FHICORC.Core.Services.Model.EuDCCModel.ValueSet;

namespace FHICORC.Core.Services.Model.Converter
{
    public static class DigitalGreenValueSetTranslatorFactory
    {
        public static IDgcValueSetTranslator DgcValueSetTranslator { get; set; } = new DigitalGreenValueSetDgcValueSetTranslator();

        public static void Init()
        {
            DgcValueSetTranslator.InitValueSet();
        }
    }
}