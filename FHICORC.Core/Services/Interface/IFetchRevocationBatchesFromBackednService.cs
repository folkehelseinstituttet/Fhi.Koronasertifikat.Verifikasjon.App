using System.Threading.Tasks;

namespace FHICORC.Core.Services.Interface
{
    public interface IFetchRevocationBatchesFromBackednService
    {

        Task HasChangedTask();
        Task FetchFromBackend();

    }

}
