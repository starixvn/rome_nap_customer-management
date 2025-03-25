namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    using Fanex.Data.Repository;

    public class GetSiteCriteria : CriteriaBase
    {
        public bool IsCredit { get; set; }
        
        public override string GetSettingKey()
            => "NAP_GlobalStakeLevelGroup_SiteSel";

        public override bool IsValid()
            => true;
    }
}