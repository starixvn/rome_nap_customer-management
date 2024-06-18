namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetRuleDetailCriteria : QueryCriteriaBase
    {
        public GetRuleDetailCriteria(int ruleId, string sportTypes, string currencies)
        {
            RuleId = ruleId;
            SportTypes = sportTypes;
            Currencies = currencies;
        }

        public int RuleId { get; }

        public string SportTypes { get; }

        public string Currencies { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSetting_RuleDetail";
    }
}
