namespace Fanex.NAP.CMS.DefaultMemberBetLimit.Criteria
{
    using Fanex.Data.Repository;

    public class SearchCriteria : CriteriaBase
    {
        public int BetLimitTypeId { get; set; }

        public string Username { get; set; }

        public override bool IsValid() => BetLimitTypeId >= 0;

        public override string GetSettingKey() => "NAP_BLControl_DefaultSetting_Sel";
    }
}