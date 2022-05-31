using FHICORC.Core.Services.Interface;
using System.Threading.Tasks;
using FHICORC.Core.Data;
using FHICORC.Core.Data.Models;
using System;

namespace FHICORC.Core.Services
{
    public class RevocationDeleteExpiredBatchService : IRevocationDeleteExpiredBatchService
    {
        private readonly IRepository<RevocationBatch> _revocationBatchRepository;

        public RevocationDeleteExpiredBatchService(IRepository<RevocationBatch> revocationBatchRepository) { 
            _revocationBatchRepository = revocationBatchRepository;
        }

        public Task DeleteExpiredBatches()
        {
            return _revocationBatchRepository.DeleteEntitiesAsync(x => x.ExpirationDate < DateTime.UtcNow);
        }
    }
}
