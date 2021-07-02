using System.Collections.Generic;
using FHICORC.Core.Services.Model.BusinessRules;
using FHICORC.Core.Services.Model.EuDCCModel._1._3._0;

namespace FHICORC.Core.Interfaces
{
    public interface IRuleSelectorService
    {
        ICollection<BusinessRule> SelectRules(DCCPayload dccPayload, bool international);
        ExternalData ApplyExternalData(DCCPayload dccPayload);
    }
}
