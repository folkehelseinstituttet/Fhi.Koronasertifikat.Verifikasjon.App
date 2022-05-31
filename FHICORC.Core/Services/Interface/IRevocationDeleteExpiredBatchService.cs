using System.IO;
using System.Threading.Tasks;

namespace FHICORC.Core.Services.Interface
{
    public interface IRevocationDeleteExpiredBatchService
    {
        Task DeleteExpiredBatches();

    }
}
