namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    internal class DelSiteSettingCriteria : CommandCriteriaBase
    {
        protected DelSiteSettingCriteria(int userId, string ip) : base(userId, ip)
        {
        }

        public string PolicySites { get; set; }

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicySiteDel";
    }
}
