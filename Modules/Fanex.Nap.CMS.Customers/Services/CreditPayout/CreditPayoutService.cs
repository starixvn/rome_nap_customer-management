namespace Fanex.Nap.CMS.Customers.Services.CreditPayout
{
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.Criteria.CreditPayout;
    using Fanex.Nap.CMS.Customers.Models.CreditPayout;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Xml.Linq;

    public class CreditPayoutService : ICreditPayoutService
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly byte StatusVerified = 1;
        private readonly string ErrorFormat = "{0}: {1}";
        private const string FilterMatchPattern = "[{2}] {0} -vs- {1}";
        private const string MatchPattern = "{0} -vs- {1}";

        public CreditPayoutService() : this(new DynamicRepository())
        {
        }

        public CreditPayoutService(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        public IEnumerable<SelectListItem> GetMatches(int typeId)
        {
            return dynamicRepository.Fetch<Match>(new GetMatchCiretia { TypeId = typeId })
                .Select(match => new SelectListItem { Text = string.Format(FilterMatchPattern, match.Home, match.Away, match.MatchID), Value = match.MatchID.ToString() });
        }

        public SearchPayoutResponse SearchPayout(SelPayoutPromotionCriteria request)
        {
            var payouts = dynamicRepository.Fetch<PayoutPromotion>(request);
            var matches = dynamicRepository.Fetch<Match>(new GetMatchCiretia { TypeId = request.TypeId }).ToDictionary(match => match.MatchID);
            foreach (var payout in payouts)
            {
                if(matches.TryGetValue(payout.MatchID, out var match))
                {
                    payout.Match = string.Format(MatchPattern, match.Home, match.Away);
                }
            }

            return new SearchPayoutResponse
            {
                Payouts = payouts,
                TotalItems = request.TotalItems,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex
            };
        }

        public IEnumerable<Result> UpdPayoutPromotion(UpdPayoutPromotionCriteria request)
        {
            if (IsSingleUpdate(request))
            {
                request.Remark = request.Remark ?? string.Empty;
            }
            dynamicRepository.Execute(request);

            return ParseErrorResultUpdate(new Result(request.ErrCode, request.ErrMsg), request.UsernameList);
        }

        public PayoutPromotion ValidateVerifyCode(VerifySelPayoutPromotionCriteria request)
        {
            var payout = dynamicRepository.Get<PayoutPromotion>(request);
            if(payout != null)
            {
                var match = dynamicRepository.Fetch<Match>(new GetMatchCiretia { TypeId = request.TypeID }).FirstOrDefault(m => m.MatchID == payout.MatchID);
                if(match != null)
                {
                    payout.Match = string.Format(MatchPattern, match.Home, match.Away);
                }
            }

            return payout;
        }

        public Result VerifyPayoutPromotion(UpdPayoutPromotionCriteria request)
        {
            request.Status = StatusVerified;
            dynamicRepository.Execute(request);
            var result = new Result(request.ErrCode, request.ErrMsg);

            return result.IsSuccess ? result : ParseErrorResultUpdate(result, request.UsernameList)[0];
        }

        private List<Result> ParseErrorResultUpdate(Result result, List<IdUsername> usernameList)
        {
            if (!string.IsNullOrEmpty(result.Message) && result.Message.TrimStart().StartsWith("<Root>"))
            {
                var errorList = new List<Result>();
                XElement errors = XElement.Parse(result.Message);
                foreach (var errorItem in errors.Element("Root").Elements("Promotion"))
                {
                    var custId = int.Parse(errorItem.Attribute("CustId").ToString());
                    var username = usernameList.FirstOrDefault(usm => usm.CustId == custId).Username;
                    var errorResult = new Result(int.Parse(errorItem.Attribute("ErrCode").ToString()), string.Format(ErrorFormat, username, errorItem.Attribute("ErrMsg").ToString()));
                    errorList.Add(errorResult);
                }

                return errorList;
            }

            return new List<Result> { result };
        }

        private static bool IsSingleUpdate(UpdPayoutPromotionCriteria request) => request.UsernameList.Count == 1;
    }
}
