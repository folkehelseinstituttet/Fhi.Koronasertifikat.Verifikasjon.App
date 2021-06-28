using System.Threading.Tasks;
using FHICORC.Core.WebServices;

namespace FHICORC.Services
{
    public interface INavigationTaskManager
    {
        Task HandlerErrors(ApiResponse response, bool silently = false);
    }
}
