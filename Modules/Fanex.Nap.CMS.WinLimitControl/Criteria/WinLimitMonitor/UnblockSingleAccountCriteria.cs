namespace Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor
{
    using System.Collections;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;

    public class UnblockSingleAccountCriteria : CriteriaBase
    {
        public WinLimitMonitorEntity WinLimitMonitor { get; set; }

        [SpParam(Ignored = true)]
        public int UserId
        {
            get
            {
                return AppSession.CurrentUser.Id;
            }
        }

        [SpParam(Ignored = true)]
        public string IP
        {
            get
            {
                return IPHelper.ClientIP();
            }
        }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "NAP_WinLimitControl_WinLimitMonitorUnblockAccounts";
        }

        public override Hashtable GetSpParams()
        {
            var hashtable = new Hashtable();
            hashtable.Add("custid", WinLimitMonitor.PlayerInfo.Id);
            hashtable.Add("productid", WinLimitMonitor.Game.Id);

            hashtable.Add("errcode", 0);
            hashtable.Add("errmsg", string.Empty);
            return hashtable;
        }
    }
}
