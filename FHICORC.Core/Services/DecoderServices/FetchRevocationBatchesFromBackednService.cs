using FHICORC.Core.Services.Interface;
using System.Threading.Tasks;

namespace FHICORC.Core.Services.DecoderServices
{
    public class FetchRevocationBatchesFromBackednService : IFetchRevocationBatchesFromBackendService
    {

        private readonly IRevocationBatchService _revocationBatchDataManager;


        public FetchRevocationBatchesFromBackednService(IRevocationBatchService revocationBatchDataManager) {
            _revocationBatchDataManager = revocationBatchDataManager;
        }

        private TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();


        public Task FetchFromBackend(){

            var downloadAndForget = _revocationBatchDataManager.FetchRevocationBatchesFromBackend(true).ContinueWith(task => tcs.SetResult(true));
            return Task.FromResult(downloadAndForget);

        }

        public Task HasChangedTask()
        {
            return tcs.Task; 
        }
    }
}
