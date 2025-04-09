namespace Fanex.NAP.CMS.BetLimitControl.Criteria.BetLimitIncrease
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Configurations;

    public class SearchBetLimitIncreaseCriteria : CriteriaBase
    {
        public string UserName { get; set; }
        public int SiteID { get; set; }
        public int Currency { get; set; }
        public bool IsTest => AppSettings.IsTest;

        public override string GetSettingKey()
                => "NAP_CM_SVIPMaxBetPlayerSel";

        public override bool IsValid()
            => true;
    }
}
