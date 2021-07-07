using FHICORC.Core.Services.Interface;

namespace FHICORC.Core.Services.Model.Converter
{
    public class DigitalGreenValueSetTranslatorFactory : IDigitalGreenValueSetTranslatorFactory
    {
        public IDgcValueSetTranslator DgcValueSetTranslator { get; set; }

        public DigitalGreenValueSetTranslatorFactory(IDgcValueSetTranslator dgcValueSetTranslator)
        {
            DgcValueSetTranslator = dgcValueSetTranslator;
        }

        public void Init()
        {
            DgcValueSetTranslator.InitValueSet();
        }
    }
}