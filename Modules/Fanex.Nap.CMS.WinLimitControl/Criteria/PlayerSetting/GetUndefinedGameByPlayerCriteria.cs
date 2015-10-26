namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerSetting
{
    using Fanex.Data.Repository;

    public class GetUndefinedGameByPlayerCriteria : CriteriaBase
    {
        public string CustName { get; set; }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustNotConfigProductSel";
        }
    }
}
