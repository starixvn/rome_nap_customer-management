namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using Fanex.Nap.CMS.CustomerClass.Requests;

    internal class AddSpreadRuleCriteria : CommandCriteriaBase
    {
        internal AddSpreadRuleCriteria(AddEditSpreadRuleRequest request, int userId, string ip, byte type) : base(userId, ip)
        {
            PolicyID = request.PolicyID;
            PolicyTitle = request.PolicyTitle;
            SportType = request.SportType;
            Spread = request.Spread;
            CustomerClasses = request.CustomerClass;
            Currencies = request.Currencies;
            Sites = request.Sites;
            Platforms = request.Platforms;
            Enable = request.Enable;
            LeagueGroups = request.LeagueGroups;
            IsLive = request.IsLive;
            Type = type;
        }

        public int PolicyID { get; }
        public string PolicyTitle { get; }
        public int SportType { get; }
        public decimal Spread { get; }
        public string CustomerClasses { get; }
        public string Currencies { get; }
        public string Sites { get; }
        public string Platforms { get; }
        public bool Enable { get; }
        public string LeagueGroups { get; }
        public byte IsLive { get; }
        public byte Type { get; }

        public override string GetSettingKey()
            => "NAP_CM_CustomerClassSpreadIns";
    }
}
