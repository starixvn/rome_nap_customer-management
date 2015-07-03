namespace Fanex.Nap.CMS.WinLimitControl.Criteria.PlayerWinLimitManagement
{
    using Fanex.NAP.Framework.Data;
    
    public class GetPlayerWinLimitForUpdateCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "CustId")]
        public int Player { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_CustSettingListSel";
        }
    }
}
