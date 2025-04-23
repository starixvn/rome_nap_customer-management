namespace Fanex.Nap.CMS.ManageProductSetting.Criteria.CompanyBetLimitSetting
{
    using Fanex.NAP.Framework.Data;
    public class CheckCurrencyGroupCriteria : CriteriaBase
    {
        public string CurrencyGroup { get; set; }
        public bool IsCredit { get; set; }
        public int ErrCode { get; set; }
        public string ErrMsg { get; set; }

        public override string GetSettingKey()
            => "NAP_CurrencyGroup_Ins";
    }
}