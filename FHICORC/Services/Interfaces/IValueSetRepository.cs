using System.IO;
using System.Threading.Tasks;
using FHICORC.Core.WebServices;

namespace FHICORC.Services.Interfaces
{
    public interface IValueSetRepository
    {
        Task<ApiResponse<Stream>> GetValueSets(string lastTimeStamp);
    }
}
