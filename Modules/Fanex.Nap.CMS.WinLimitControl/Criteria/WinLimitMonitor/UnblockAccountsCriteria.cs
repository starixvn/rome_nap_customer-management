namespace Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;

    public class UnblockAccountsCriteria : CriteriaBase
    {
        public List<WinLimitMonitorEntity> WinLimitMonitor { get; set; }

        public override Hashtable GetSpParams()
        {
            return new Hashtable 
            {
                { "MemUnblock", BuildUnblockCustomerXml() },
                { "UserId",  AppSession.CurrentUser.Id },
                { "IP",  IPHelper.ClientIP() },
                { "ErrCode",  0 },
                { "ErrMsg",  string.Empty }
            };
        }

        public override string GetSettingKey()
        {
            return "NAP_WinLimitControl_WinLimitMonitorUnblockAccounts";
        }

        private string BuildUnblockCustomerXml()
        {
            string format = @"<row custid=""{0}"" productid=""{1}""/>";

            StringBuilder builder = new StringBuilder();
            builder.Append("<unblock>");
            foreach (WinLimitMonitorEntity monitor in WinLimitMonitor)
            {
                builder.AppendFormat(format, monitor.PlayerInfo.Id, monitor.Game.Id);
            }

            builder.Append("</unblock>");
            return builder.ToString();
        }
    }
}
