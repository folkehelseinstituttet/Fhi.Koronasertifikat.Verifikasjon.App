using System;
namespace FHICORC.Core.Services.Interface
{
    public interface IDigitalGreenValueSetTranslatorFactory
    {
        IDgcValueSetTranslator DgcValueSetTranslator { get; set; }
        void Init();
    }
}
