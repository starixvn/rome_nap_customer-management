namespace Fanex.Nap.CMS.Customers.ViewModels.BetList
{
    using System;
    using System.Collections.Generic;
    using Infrastructure.Extensions;
    using Models.BetList;
    using Resources;

    public class AbnormalBetTicketViewModel
    {
        public AbnormalBetTicketViewModel(
            DateTime from,
            DateTime to,
            int custId,
            string custName,
            int totalAbnormalBetTickets,
            IEnumerable<AbnormalBetTicket> abnormalBetTickets,
            int totalSportsbookBetTickets,
            int pageSize)
        {
            From = from;
            To = to;
            CustId = custId;
            CustName = custName;
            TotalAbnormalBetTickets = totalAbnormalBetTickets;
            AbnormalBetTickets = abnormalBetTickets;
            TotalSportsbookBetTickets = totalSportsbookBetTickets;
            PageSize = pageSize;
        }

        public DateTime From { get; }

        public DateTime To { get; }

        public int CustId { get; }

        public string CustName { get; }

        public int TotalAbnormalBetTickets { get; }

        public IEnumerable<AbnormalBetTicket> AbnormalBetTickets { get; }

        public int TotalSportsbookBetTickets { get; }

        public int PageSize { get; }

        public string AbnormalBetTicketTitle => string.Format(
            BetListResource.AbnormalBetTicketTitle,
            CustName,
            From.ToShortFormatedDateWithSlash(),
            To.ToShortFormatedDateWithSlash());

        public string AbnormalBetTicketHint => string.Format(
            BetListResource.AbnormalBetTicketHint,
            From.ToShortFormatedDateWithSlash(),
            To.ToShortFormatedDateWithSlash());
    }
}
