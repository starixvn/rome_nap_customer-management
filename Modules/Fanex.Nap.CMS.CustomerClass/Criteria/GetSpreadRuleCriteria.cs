namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;
    
    internal class GetSpreadRuleCriteria : QueryCriteriaBase
    {
        public GetSpreadRuleCriteria(SearchSpreadRuleRequest request)
        {
            CustomerClasses = request.CustomerClass;
            Sites = request.Site;
            Currencies = request.Currency;
            Spread = request.Spread;
        }

        public string CustomerClasses { get; }
        public string Sites { get; }
        public string Currencies { get; }
        public float Spread { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSpreadSel";
    }
}
