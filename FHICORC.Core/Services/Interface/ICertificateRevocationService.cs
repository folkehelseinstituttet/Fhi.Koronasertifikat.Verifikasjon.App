using System.Threading.Tasks;

namespace FHICORC.Core.Services.Interface
{
    public interface ICertificateRevocationService
    {
        Task<bool> IsCertificateRevoked(ITokenPayload token);
    }
}
