namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    using Fanex.Data.Repository;
    public class GetBetLimitSettingCriteria : CriteriaBase
    {
        public bool IsCredit { get; set; }
        
        public string PMutilProductID { get; set; }
        
        public string PMutilCurrency { get; set; }
        
        public string   PMutilSite   { get; set; }
        
        public override string GetSettingKey()
            => "NAP_GlobalStakeLevelGroup_Sel";

        public override bool IsValid()
            => true;
    }
}