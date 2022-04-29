using System.Collections.Generic;
using System.Threading.Tasks;
using FHICORC.Core.WebServices;
using FHICORC.Models;

namespace FHICORC.Services.Interfaces
{
    public interface IRevocationsRepository
    {
        public Task<ApiResponse<ICollection<Batches>>> GetBatches();
    }
}
