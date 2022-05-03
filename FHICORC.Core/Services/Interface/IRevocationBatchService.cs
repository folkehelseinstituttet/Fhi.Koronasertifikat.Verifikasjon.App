using FHICORC.Core.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FHICORC.Core.Services.Interface
{
    public interface IRevocationBatchService
    {
        Task<IEnumerable<RevocationBatch>> GetRevocationBatchesFromCountry(string isoCode);
    }
}
