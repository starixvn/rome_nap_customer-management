namespace Fanex.Nap.CMS.ColossusBets.Services
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.ColossusBets.Criteria;
    using Fanex.Nap.CMS.ColossusBets.Enums;
    using Fanex.Nap.CMS.ColossusBets.Models;
    using Fanex.Nap.CMS.ColossusBets.Repositories;
    using Fanex.Nap.CMS.ColossusBets.Resources;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;

    public class PayoutService : IPayoutService
    {
        private const int CashoutBetTypeId = 18002;
        private readonly IPayoutRepository _payoutRepository;

        public PayoutService(IPayoutRepository payoutRepository = null)
        {
            _payoutRepository = payoutRepository ?? new PayoutRepository();
        }

        public IList<IdNameModel>[] GetStatusStateMachine()
        {
            IList<IdNameModel> all = new List<IdNameModel> 
            {
                new IdNameModel { Id = 0, Name = ColossusBetsLabels.All },
                new IdNameModel { Id = (int)PayoutStatus.Open, Name = ColossusBetsLabels.Open },
                new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = ColossusBetsLabels.InfoVerified },
                new IdNameModel { Id = (int)PayoutStatus.Disbursed, Name = ColossusBetsLabels.Disbursed },
                new IdNameModel { Id = (int)PayoutStatus.Completed, Name = ColossusBetsLabels.Completed },
                new IdNameModel { Id = (int)PayoutStatus.Expired, Name = ColossusBetsLabels.Expired }
            };

            IList<IdNameModel> open = new List<IdNameModel> 
            {
                new IdNameModel { Id = (int)PayoutStatus.Open, Name = ColossusBetsLabels.Open },
                new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = ColossusBetsLabels.InfoVerified },
                new IdNameModel { Id = (int)PayoutStatus.Expired, Name = ColossusBetsLabels.Expired }
            };

            IList<IdNameModel> infoVerified = new List<IdNameModel> 
            {
                new IdNameModel { Id = (int)PayoutStatus.Open, Name = ColossusBetsLabels.Open },
                new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = ColossusBetsLabels.InfoVerified },
                new IdNameModel { Id = (int)PayoutStatus.Disbursed, Name = ColossusBetsLabels.Disbursed },
            };

            IList<IdNameModel> disbursed = new List<IdNameModel> 
            {
                new IdNameModel { Id = (int)PayoutStatus.InfoVerified, Name = ColossusBetsLabels.InfoVerified },
                new IdNameModel { Id = (int)PayoutStatus.Disbursed, Name = ColossusBetsLabels.Disbursed },
                new IdNameModel { Id = (int)PayoutStatus.Completed, Name = ColossusBetsLabels.Completed },
            };

            IList<IdNameModel> completed = new List<IdNameModel> 
            {
                new IdNameModel { Id = (int)PayoutStatus.Completed, Name = ColossusBetsLabels.Completed }
            };

            IList<IdNameModel> expired = new List<IdNameModel> 
            {
                new IdNameModel { Id = (int)PayoutStatus.Expired, Name = ColossusBetsLabels.Expired }
            };

            return new IList<IdNameModel>[] { all, open, infoVerified, disbursed, completed, expired };
        }

        public IList<Ticket> GetTickets(GetTicketsCriteria criteria)
        {
            return _payoutRepository.GetTickets(criteria);
        }

        public TicketDetailReport GetTicketDetailReport(ICriteria criteria)
        {
            decimal sold = 0, totalWinLoss = 0, totalWinLossCashout = 0;
            var ticketDetails = _payoutRepository.GetTicketDetails(criteria);
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
            return _payoutRepository.GetTicketNotes(criteria);
        }

        public Result UpdateTicketStatus(ICriteria criteria)
        {
            return _payoutRepository.UpdateTicketStatus(criteria);
        }
    }
}
