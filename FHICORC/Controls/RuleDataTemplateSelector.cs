using System;
using FHICORC.Core.Services.Model.BusinessRules;
using Xamarin.Forms;

namespace FHICORC.Controls
{
    public class RuleDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate FailedRuleTemplate { get; set; }
        public DataTemplate PassedRuleTemplate { get; set; }
        public DataTemplate OpenRuleTemplate { get; set; }
        public DataTemplate FailedRuleFirstTemplate { get; set; }
        public DataTemplate PassedRuleFirstTemplate { get; set; }
        public DataTemplate OpenRuleFirstTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            RulesFeedbackData rulesFeedback = (RulesFeedbackData)item;
            if (rulesFeedback.Result == Core.Services.Enum.RulesFeedbackResult.TRUE)
            {
                if (rulesFeedback.IsFirstWithResult)
                    return PassedRuleFirstTemplate;
                return PassedRuleTemplate;
            }
            else if (rulesFeedback.Result == Core.Services.Enum.RulesFeedbackResult.FALSE)
            {
                if (rulesFeedback.IsFirstWithResult)
                    return FailedRuleFirstTemplate;
                return FailedRuleTemplate;
            }
            else
            {
                if (rulesFeedback.IsFirstWithResult)
                    return OpenRuleFirstTemplate;
                return OpenRuleTemplate;
            }
        }
    }
}
