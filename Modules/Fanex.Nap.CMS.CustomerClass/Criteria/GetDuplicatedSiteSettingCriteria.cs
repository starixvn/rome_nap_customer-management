namespace Fanex.Nap.CMS.CustomerClass.Criteria
{
    using System.Text;
    using Fanex.Nap.CMS.CustomerClass.Requests;
    using Fanex.Nap.CMS.Infrastructure.Configurations;
    using Fanex.Nap.CMS.Infrastructure.Criteria;

    internal class GetDuplicatedSiteSettingCriteria : CommandBaseCriteria
    {
        public GetDuplicatedSiteSettingCriteria(string policySites)
            => PolicySites = policySites;

        public string PolicySites { get; set; }

        public bool IsTest { get; } = AppSettings.IsTest;

        public override string GetSettingKey()
            => "NAP_LM_CustomerClassPolicySiteCheck";
    }
}
