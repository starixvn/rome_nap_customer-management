namespace Fanex.Nap.CMS.ColossusBets.Services
{
    using System.Collections.Generic;
    using System.Net;
    using System.Text;
    using Criteria;
    using Data.Repository;
    using Infrastructure.Helpers;
    using Infrastructure.Models;
    using Models;
    using Newtonsoft.Json;
    using Repositories;
    using Resources;

    public class BonusTicketService : IBonusTicketService
    {
        private readonly IBonusTicketRepository _bonusTicketRepository;

        public BonusTicketService(IBonusTicketRepository bonusTicketRepository = null)
        {
            _bonusTicketRepository = bonusTicketRepository ?? new BonusTicketRepository();
        }

        public BonusTicketDetail GetBonusTicketDetail(long refno)
        {
            ICriteria criteria = new GetBonusTicketDetailCriteria { RefNo = refno };

            return _bonusTicketRepository.GetBonusTicketDetail(criteria);
        }

        public BonusTicketDetail GetBonusTicketDetailByRefNo(long refno)
        {
            ICriteria criteria = new GetBonusTicketDetailByRefNoCriteria { RefNo = refno };

            return _bonusTicketRepository.GetBonusTicketDetailByRefNo(criteria);
        }

        public IEnumerable<BonusTicket> GetBonusTickets(ICriteria criteria)
        {
            return _bonusTicketRepository.GetBonusTickets(criteria);
        }

        public Result SaveBonusTicket(ICriteria criteria)
        {
            var result = _bonusTicketRepository.SaveBonusTicket(criteria);

            if (result.Code == 0)
            {
                result.Message = ColossusBetsLabels.SaveBonusTicketSuccess;
            }

            return result;
        }

        public Result DoSettlementAPI(string eventId, string ticketId, int customerId, decimal amount)
        {
            Result result = null;
            var model = new DoSettlement
            {
                EventId = eventId,
                TicketId = ticketId,
                CustomerId = customerId,
                Ext3 = amount.ToString()
            };

            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                var data = Encoding.UTF8.GetBytes(model.SerializeJSON());

                try
                {
                    var results = client.UploadData(model.SettlementUrl, "POST", data);
                    string jsonResult = Encoding.UTF8.GetString(results);

                    var resultAPI = JsonConvert.DeserializeObject<ResultAPI>(jsonResult);
                    result = new Result { Code = resultAPI.ErrorCode, Message = resultAPI.ErrorMsg };
                }
                catch (System.Exception ex)
                {
                    Logs.Error(ex);

                    result = new Result { Code = 500, Message = ex.Message };
                }
            }

            return result;
        }
    }
}
