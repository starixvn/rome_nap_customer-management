namespace Fanex.Nap.CMS.BetRestriction.Criterias.Management
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    public class GetAllSettingCriteria: CriteriaBase
    {
        public bool IsTest { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_SettingSel";
        }
    }
}
