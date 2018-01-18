namespace Fanex.Nap.CMS.PayoutManagement.Services
{
    using System;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.PayoutManagement.Models;
    using Fanex.Nap.CMS.PayoutManagement.Repositories;
    using Fanex.Nap.CMS.PayoutManagement.Resources;

    public class BetTicketService : IBetTicketService
    {
        private const int CashoutBetTypeId = 18002;
        private readonly IBetTicketRepository _betTicketRepository;

        public BetTicketService(IBetTicketRepository betTicketRepository = null)
        {
            _betTicketRepository = betTicketRepository ?? new BetTicketRepository();
        }

        public BetTicket GetMicrogaming(ICriteria criteria)
        {
            return _betTicketRepository.GetMicrogaming(criteria);
        }

        public ColossusBetsTicket GetColossusBets(ICriteria criteria)
        {
            ColossusBetsTicket colossusbets = _betTicketRepository.GetColossusBets(criteria);

            decimal sold = 0;
            decimal totalWinLoss = 0;
            decimal totalWinLossCashout = 0;
            foreach (BetTicketDetail ticket in colossusbets.BetTicketDetails)
            {
                ticket.TransactionType = CacheRef.GetBetType(ticket.BetType);
                if (ticket.BetType == CashoutBetTypeId)
                {
                    totalWinLossCashout += ticket.WinLoss;
                    ticket.TransactionType += " " + ticket.Sold.ToString("#0.##%");
                }

                sold += ticket.Sold;
                totalWinLoss += ticket.WinLoss;
            }

            colossusbets.Own = string.Format(PayoutManagementLabels.TicketDetailOwn, (1 - sold).ToString("#0.##%"));
            colossusbets.Sold = totalWinLossCashout != 0 ?
                string.Format(PayoutManagementLabels.TicketDetailSoldCashout, sold.ToString("#0.##%"), totalWinLossCashout.ToString("N2")) :
                string.Format(PayoutManagementLabels.TicketDetailSold, sold.ToString("#0.##%"));
            colossusbets.TotalWinLoss = totalWinLoss;

            return colossusbets;
        }

        public string GetRNGCasinoJackpotRefNo2(ICriteria criteria)
        {
            return _betTicketRepository.GetRNGCasinoJackpotRefNo2(criteria);
        }

        public string GetSiteIdByCustomerId(ICriteria criteria)
        {
            return _betTicketRepository.GetSiteIdByCustomerId(criteria);
        }
    }
}
