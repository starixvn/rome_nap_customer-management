namespace Fanex.Nap.CMS.ColossusBets.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.ColossusBets.Criteria;
    using Fanex.Nap.CMS.ColossusBets.Models;
    using Fanex.Nap.CMS.ColossusBets.Repositories;
    using Fanex.Nap.CMS.ColossusBets.Resources;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;

    public class PaymentService : IPaymentService
    {
        private const int CashoutBetTypeId = 18002;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IPaymentRepository paymentRepository = null)
        {
            _paymentRepository = paymentRepository ?? new PaymentRepository();
        }

        public IList<Ticket> GetTickets(GetTicketsCriteria criteria)
        {
            return _paymentRepository.GetTickets(criteria);
        }

        public TicketDetailReport GetTicketDetailReport(ICriteria criteria)
        {
            decimal sold = 0, totalWinLoss = 0, totalWinLossCashout = 0;
            var ticketDetails = _paymentRepository.GetTicketDetails(criteria);
            foreach (TicketDetail detail in ticketDetails)
            {
                detail.Date = detail.TransDate.ToLongFormatedDateWithSlash();
                detail.TransactionType = CacheRef.GetBetType(detail.BetType);
                if (detail.BetType == CashoutBetTypeId)
                {
                    totalWinLossCashout += detail.WinLoss;
                    detail.TransactionType += " " + detail.Sold.ToString("#0.##%");
                }

                sold += detail.Sold;
                totalWinLoss += detail.WinLoss;
            }

            return new TicketDetailReport 
            {
                Own = string.Format(ColossusBetsLabels.TicketDetailOwn, (1 - sold).ToString("#0.##%")),
                Sold = totalWinLossCashout != 0 ?
                    string.Format(ColossusBetsLabels.TicketDetailSoldCashout, sold.ToString("#0.##%"), totalWinLossCashout.ToString("N2")) :
                    string.Format(ColossusBetsLabels.TicketDetailSold, sold.ToString("#0.##%")),
                TicketDetails = ticketDetails,
                TotalWinLoss = totalWinLoss
            };
        }

        public IList<TicketNote> GetTicketNotes(ICriteria criteria)
        {
            return _paymentRepository.GetTicketNotes(criteria);
        }

        public Result UpdateTicketStatus(ICriteria criteria)
        {
            return _paymentRepository.UpdateTicketStatus(criteria);
        }
    }
}
