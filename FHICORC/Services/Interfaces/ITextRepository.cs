using System;
using System.IO;
using System.Threading.Tasks;
using FHICORC.Core.WebServices;

namespace FHICORC.Services.Interfaces
{
    public interface ITextRepository
    {
        Task<ApiResponse<Stream>> GetTexts(string currentVersion);
    }
}
