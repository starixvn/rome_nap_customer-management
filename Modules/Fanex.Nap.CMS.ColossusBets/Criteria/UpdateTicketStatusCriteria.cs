namespace Fanex.Nap.CMS.ColossusBets.Criteria
{
    using System.Collections;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;

    public class UpdateTicketStatusCriteria : CriteriaBase
    {
        public long BetId { get; set; }

        public IdNameModel Status { get; set; }

        public string Note { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_ColossusBets_PayoutUpd";
        }

        public override Hashtable GetSpParams()
        {
            return new Hashtable 
            {
                { "betid", BetId },
                { "statusid", Status.Id },
                { "paystatus", Status.Name },
                { "paynote", Note },
                { "createdby", AppSession.CurrentUser.Id },
                { "ip", IPHelper.ClientIP() },
                { "errcode", 0 },
                { "errmsg", string.Empty },
            };
        }
    }
}
