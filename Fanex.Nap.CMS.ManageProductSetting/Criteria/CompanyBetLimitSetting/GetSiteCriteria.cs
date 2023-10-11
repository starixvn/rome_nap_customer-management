namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    using Fanex.NAP.Framework.Data;
    
    public class GetSiteCriteria : CriteriaBase
    {
        public bool IsCredit { get; set; }
        
        public override string GetSettingKey()
            => "NAP_GlobalStakeLevelGroup_SiteSel";
    }
}