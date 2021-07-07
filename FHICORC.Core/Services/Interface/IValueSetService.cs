using System.IO;
using System.Threading.Tasks;

namespace FHICORC.Core.Services.Interface
{
    public interface IValueSetService
    {
        Task FetchAndSaveLatestVersionOfValueSets();
        Stream GetValueSet(string fileName);
    }
}
