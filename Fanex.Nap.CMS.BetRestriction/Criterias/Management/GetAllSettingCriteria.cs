namespace Fanex.Nap.CMS.BetRestriction.Criterias.Management
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    public class GetAllSettingCriteria: CriteriaBase
    {
        public bool IsTest => AppSettings.IsTest;

        public override string GetSettingKey()
        {
            return "NAP_BetRestriction_SettingSel";
        }

        public override bool IsValid()
            => true;
    }
}
