namespace Fanex.Nap.CMS.Customers.ViewModels.BetList
{
    using System;
    using System.Collections.Generic;
    using Constants;
    using Infrastructure.Extensions;
    using Models.BetList;
    using Resources;

    public class AbnormalBetTicketViewModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int CustId { get; set; }

        public string CustName { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public byte SiteType { get; set; }

        public string GetPageTitle()
        {
            if (SiteTypeConst.IsCN88(SiteType))
            {
                return BetListResource.ProblemAccountCN88;
            }

            return SiteTypeConst.IsCredit(SiteType)
                ? BetListResource.ProblemAccountCreditSites
                : BetListResource.ProblemAccountLicenseeSites;
        }

        public int TotalAbnormalBetTickets { get; set; }

        public IEnumerable<AbnormalBetTicket> AbnormalBetTickets { get; set; }

        public int TotalSportsbookBetTickets { get; set; }

        public string AbnormalBetTicketTitle => string.Format(
            BetListResource.AbnormalBetTicketTitle,
            CustName,
            FromDate.ToShortFormatedDateWithSlash(),
            ToDate.ToShortFormatedDateWithSlash());

        public string AbnormalBetTicketHint => string.Format(
            BetListResource.AbnormalBetTicketHint,
            FromDate.ToShortFormatedDateWithSlash(),
            ToDate.ToShortFormatedDateWithSlash());
    }
}
