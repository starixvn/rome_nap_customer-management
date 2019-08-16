namespace Fanex.Nap.CMS.Customers.Models.BetList
{
    using System;

    public class AbnormalBetTicket
    {
        public AbnormalBetTicket(DateTime voidDate, long betId, string reason)
        {
            VoidDate = voidDate;
            BetId = betId;
            Reason = reason;
        }

        public DateTime VoidDate { get; }

        public long BetId { get; }

        public string Reason { get; }

        public string VoidDateString
            => VoidDate.ToString("MM/dd/yyyy HH:mm:ss");
    }
}
