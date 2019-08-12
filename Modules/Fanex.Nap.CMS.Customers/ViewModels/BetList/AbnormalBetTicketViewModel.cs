namespace Fanex.Nap.CMS.Customers.ViewModels.BetList
{
    using Infrastructure.Extensions;
    using ProblemAccount;
    using Resources;

    public class AbnormalBetTicketViewModel : SearchViewModel
    {
        public int CustId { get; set; }

        public string CustName { get; set; }

        public int TotalSportsbookBetTickets { get; } = 1001;

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
