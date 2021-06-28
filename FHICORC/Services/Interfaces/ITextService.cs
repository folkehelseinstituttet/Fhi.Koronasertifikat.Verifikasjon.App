using System.IO;
using System.Threading.Tasks;

namespace FHICORC.Services.Interfaces
{
    public interface ITextService
    {
        Task LoadSavedLocales();
        Task LoadRemoteLocales();
        void SetLocale(string isoCode);
        Stream GetDgcValueSet();
    }
}
