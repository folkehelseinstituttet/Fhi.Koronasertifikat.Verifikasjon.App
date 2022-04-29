using System.Threading.Tasks;

namespace FHICORC.Services.Interfaces
{
    public interface IRevocationsService
    {
        Task CheckAndFetchRevocationsFromBackend();
        Task FetchRevocationsFromBackend(bool handleErrorsSilently);
    }
}
