namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;
    
    internal class GetSpreadRuleCriteria : QueryCriteriaBase
    {
        public GetSpreadRuleCriteria(SearchSpreadRuleRequest request, string sportTypes, byte type)
        {
            CustomerClasses = request.CustomerClass;
            Sites = request.Site;
            Currencies = request.Currency;
            if(float.TryParse(request.Spread, out var spread)) Spread = spread;
            SportTypes = sportTypes;
            Type = type;
        }

        public string CustomerClasses { get; }
        public string SportTypes { get; }
        public string Sites { get; }
        public string Currencies { get; }
        public float? Spread { get; }
        public byte Type { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSpreadSel";
    }
}
