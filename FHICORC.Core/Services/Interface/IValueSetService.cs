using System.IO;
using System.Threading.Tasks;

namespace FHICORC.Core.Services.Interface
{
    public interface IValueSetService
    {
        Task CheckFetchAndSaveLatestVersionOfValueSets();
        Task FetchAndSaveLatestVersionOfValueSets(long lastTimeFetched);
        Stream GetValueSet(string fileName);
    }
}
