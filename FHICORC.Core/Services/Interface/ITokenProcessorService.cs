using System.Threading.Tasks;
using FHICORC.Core.Services.Model;

namespace FHICORC.Core.Services.Interface
{
    public interface ITokenProcessorService
    {
        Task<TokenValidateResultModel> DecodePassportTokenToModel(string base45String);
        void SetDgcValueSetTranslator(IDgcValueSetTranslator translator);
    }
}