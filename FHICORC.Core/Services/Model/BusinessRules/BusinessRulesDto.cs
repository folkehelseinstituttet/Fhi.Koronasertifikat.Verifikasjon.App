using System.Collections.Generic;

namespace FHICORC.Core.Services.Model.BusinessRules
{
    public class BusinessRulesDto
    {
        public ICollection<BusinessRule> International { get; set; }
        public ICollection<BusinessRule> Domestic { get; set; }
    }
}
