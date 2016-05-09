namespace Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor
{
    using System;
    using Data.Repository;
    using Infrastructure.Helpers;
    using Infrastructure.Web;

    public class UnblockPlayerCriteria : CriteriaBase
    {
        [SpParam(SqlParamName = "custid")]
        public int PlayerId { get; set; }

        [SpParam(SqlParamName = "productid")]
        public int GameId { get; set; }

        public DateTime WinlostDate { get; set; }

        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        public string IP
        {
            get
            {
                return IPHelper.ClientIP();
            }
        }

        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_WinLimit_Game_Unblock";
        }
    }
}
