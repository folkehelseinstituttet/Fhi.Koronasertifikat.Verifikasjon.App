using System.Threading.Tasks;

namespace FHICORC.Core.Services.Interface
{
    public interface IFetchRevocationBatchesFromBackendService
    {

        Task HasChangedTask();
        Task FetchFromBackend();

    }

}
