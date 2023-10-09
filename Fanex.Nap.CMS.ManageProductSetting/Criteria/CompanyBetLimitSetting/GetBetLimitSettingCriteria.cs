namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    using Fanex.NAP.Framework.Data;
    public class GetBetLimitSettingCriteria: CriteriaBase
    {
        public bool IsCredit { get; set; }
        
        public string PMutilProductID { get; set; }
        
        public string PMutilCurrency { get; set; }
        
        public int PageIndex { get;  set; }

        public int PageSize { get;  set; }
        
        public override string GetSettingKey()
            => "NAP_GlobalStakeLevelGroup_Sel";
    }
}