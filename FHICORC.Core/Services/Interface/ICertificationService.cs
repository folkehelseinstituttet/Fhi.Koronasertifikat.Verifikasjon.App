using System.Threading.Tasks;
using FHICORC.Core.Services.Model.CoseModel;

namespace FHICORC.Core.Services.Interface
{
    public interface ICertificationService
    {
        public Task VerifyCoseSign1Object(CoseSign1Object coseSign1Object);
    }
}