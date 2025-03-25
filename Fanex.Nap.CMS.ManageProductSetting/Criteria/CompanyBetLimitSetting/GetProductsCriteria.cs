namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    using Fanex.Data.Repository;
    public class GetProductsCriteria : CriteriaBase
    {
        public bool IsCredit { get; set; }
        
        public override string GetSettingKey()
            => "NAP_GlobalStakeLevelGroup_ProductSel";

        public override bool IsValid()
            => true;
    }
}