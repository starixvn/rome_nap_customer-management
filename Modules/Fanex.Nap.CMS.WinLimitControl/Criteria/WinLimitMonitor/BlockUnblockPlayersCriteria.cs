namespace Fanex.Nap.CMS.WinLimitControl.Criteria.WinLimitMonitor
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.WinLimitControl.Models.WinLimitMonitor;

    public class BlockUnblockPlayersCriteria : CriteriaBase
    {
        public bool IsBlock { get; set; }

        public IList<BlockUnblockPlayerViewModel> Accounts { get; set; }

        public override Hashtable GetSpParams()
        {
            return new Hashtable 
            {
                { "MemBlockUnblock", BuildBlockUnblockAccountsXml() },
                { "IsBlock", IsBlock },
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

        private string BuildBlockUnblockAccountsXml()
        {
            string format = @"<row custid=""{0}"" productid=""{1}""/>";

            StringBuilder builder = new StringBuilder();
            builder.Append("<unblock>");
            foreach (var item in Accounts)
            {
                builder.AppendFormat(format, item.Player, item.Game);
            }

            builder.Append("</unblock>");
            return builder.ToString();
        }
    }
}
