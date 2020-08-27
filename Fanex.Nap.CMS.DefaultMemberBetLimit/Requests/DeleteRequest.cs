namespace Fanex.NAP.CMS.DefaultMemberBetLimit.Requests
{
    public class DeleteRequest
    {
        public int BetLimitTypeId { get; set; }

        public long CustId { get; set; }

        public bool IsDownlineAffected { get; set; }
    }
}
