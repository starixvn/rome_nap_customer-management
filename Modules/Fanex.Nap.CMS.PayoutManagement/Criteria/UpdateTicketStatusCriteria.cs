namespace Fanex.Nap.CMS.PayoutManagement.Criteria
{
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Helpers;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;

    public class UpdateTicketStatusCriteria : CommandBaseCriteria
    {
        [SpParam(SqlParamName = "PayoutTransId")]
        public int Id { get; set; }

        public IdNameModel Status { get; set; }

        public int PayoutStatusId => Status.Id;

        public string PayoutStatus => Status.Name;

        [SpParam(SqlParamName = "PayoutNote")]
        public string Note { get; set; }

        public int Creator => AppSession.CurrentUser.Id;

        public string IP => IPHelper.ClientIP();

        public override string GetSettingKey()
        {
            return "NAP_PayoutTrans_Upd";
        }

        public override bool IsValid()
            => true;
    }
}
