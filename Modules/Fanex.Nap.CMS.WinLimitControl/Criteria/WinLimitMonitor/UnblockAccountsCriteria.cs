namespace Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;

    public class UnblockAccountsCriteria : CriteriaBase
    {
        public List<WinLimitMonitorEntity> WinLimitMonitor { get; set; }

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

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "NAP_WinLimitControl_WinLimitMonitorUnblockAccounts";
        }
    }
}
