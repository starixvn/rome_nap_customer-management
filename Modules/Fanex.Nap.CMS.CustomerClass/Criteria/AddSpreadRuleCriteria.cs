namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class AddSpreadRuleCriteria : CommandCriteriaBase
    {
        internal AddSpreadRuleCriteria(AddEditSpreadRuleRequest request, int userId, string ip) : base(userId, ip)
        {
            PolicyID = request.PolicyID;
            PolicyTitle = request.PolicyTitle;
            Spread = request.Spread;
            CustomerClasses = request.CustomerClass;
            Currencies = request.Currencies;
            Sites = request.Sites;
            Platforms = request.Platforms;
            Enable = request.Enable;
        }

        public int PolicyID { get; }
        public string PolicyTitle { get; }
        public decimal Spread { get; }
        public string CustomerClasses { get; }
        public string Currencies { get; }
        public string Sites { get; }
        public string Platforms { get; }
        public bool Enable { get; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassSpreadIns";
    }
}
