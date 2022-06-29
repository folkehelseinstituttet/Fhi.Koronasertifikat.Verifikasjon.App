using FHICORC.Core.Services.Interface;
using System.Threading.Tasks;

namespace FHICORC.Core.Services.DecoderServices
{
    public class FetchRevocationBatchesFromBackednService : IFetchRevocationBatchesFromBackednService
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


        //static async void Run()
        //{
        //    var tcs = new TaskCompletionSource<bool>();

        //    var fireAndForgetTask = Task.Delay(5000)
        //                                .ContinueWith(task => tcs.SetResult(true));

        //    
        //}




    }


    //    public class Monitor
    //    {
    //        private TaskCompletionSource<bool> _changedTaskSource = new TaskCompletionSource<bool>();
    //        public Task HasChangedTask => _changedTaskSource.Task;

    //        public bool HasChanged
    //        ...
    //        set
    //        {
    //        ...
    //        _changedTaskSource.TrySetResult(true);
    //    }
    //}
}
