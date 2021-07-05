using FHICORC.Core.Services.Enum;

namespace FHICORC.Core.Services.Model.BusinessRules
{
    public class RulesFeedbackData
    {
        public string RuleIdentifier { get; set; }

        public string RuleDescription { get; set; }

        public RulesFeedbackResult Result { get; set; }

        public string Current { get; set; }

        public bool IsFirstWithResult { get; set; }
    }
}
