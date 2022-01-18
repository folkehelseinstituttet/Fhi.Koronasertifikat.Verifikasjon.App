using System.IO;
using System.Threading.Tasks;
using FHICORC.Core.WebServices;
using FHICORC.Services.Interfaces;

namespace FHICORC.Tests.TestMocks
{
    public class MockTextRepository : ITextRepository
    {
        public Task<ApiResponse<Stream>> GetTexts(string currentVersion)
        {
            return Task.FromResult(new ApiResponse<Stream>((ApiResponse)null));
        }
    }
}
