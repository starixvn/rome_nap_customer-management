namespace Fanex.Nap.CMS.PayoutManagement.Criteria
{
    using System.Collections;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;

    public class UpdateTicketStatusCriteria : CriteriaBase
    {
        public int Id { get; set; }

        public IdNameModel Status { get; set; }

        public string Note { get; set; }

        public override string GetSettingKey()
        {
            return "NAP_PayoutTrans_Upd";
        }

        public override Hashtable GetSpParams()
        {
            return new Hashtable 
            {
                { "PayoutTransId", Id },
                { "PayoutStatusId", Status.Id },
                { "PayoutStatus", Status.Name },
                { "PayoutNote", Note },
                { "Creator", AppSession.CurrentUser.Id },
                { "IP", IPHelper.ClientIP() },
                { "ErrCode", 0 },
                { "ErrMsg", string.Empty },
            };
        }
    }
}
