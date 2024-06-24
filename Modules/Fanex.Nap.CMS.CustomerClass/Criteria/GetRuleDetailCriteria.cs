namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class GetRuleDetailCriteria : QueryCriteriaBase
    {
        public GetRuleDetailCriteria(int ruleId, string sportTypes, string currencies, byte isMultiSite)
        {
            RuleId = ruleId;
            SportTypes = sportTypes;
            Currencies = currencies;
            IsMultiSite = isMultiSite;
        }

        public int RuleId { get; }

        public string SportTypes { get; }

        public string Currencies { get; set; }

        public byte IsMultiSite { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSetting_RuleDetail";
    }
}
