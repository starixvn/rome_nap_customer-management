namespace Fanex.Nap.CMS.Customers.Services.ProblemAccountLicenseeSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Constants.ProblemAccount;
    using Criteria.ProblemAccountLicenseeSite;
    using Infrastructure.Extensions;
    using Infrastructure.Models;
    using Infrastructure.Web;
    using Models.ProblemAccount;
    using Models.ProblemAccountLicenseeSite;
    using Repositories.ProblemAccountLicenseeSite;
    using Resources;

    public class ProblemAccountLicenseeSiteService : IProblemAccountLicenseeSiteService
    {
        private const string CurrencyCacheKey = "Fanex.Nap.CMS.Customers.Services.ProblemAccountLicenseeSiteService.Currency";
        private const string PreventiveActionCacheKey = "Fanex.Nap.CMS.Customers.Services.ProblemAccountLicenseeSiteService.PreventiveAction";
        private const string ReasonCacheKey = "Fanex.Nap.CMS.Customers.Services.ProblemAccountLicenseeSiteService.Reason_{0}";

        private readonly IProblemAccountLicenseeSiteRepository repository;
        private readonly IProblemAccountCTSService ctsService;

        public ProblemAccountLicenseeSiteService(
            IProblemAccountLicenseeSiteRepository repository = null,
            IProblemAccountCTSService ctsService = null)
        {
            this.repository = repository ?? new ProblemAccountLicenseeSiteRepository();
            this.ctsService = ctsService ?? new ProblemAccountCTSService();
        }

        public IEnumerable<LicenseeAcknowledgement> GetLicenseeAcknowledgements()
            => repository.GetLicenseeAcknowledgements(new GetLicenseeAcknowledgementCriteria());

        public Result UpdateLicenseeAcknowledgement(int licenseeId, bool isAcknowledgement, bool confirmed, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(
                new UpdateLicenseeAcknowledgementCriteria(licenseeId, isAcknowledgement, confirmed, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountCreditDepositResource.UpdateLicenseeAcknowledgementSuccessfully;
            }
            else if (IsNeedTobeConfirmedResult(result))
            {
                result.Message = string.Format(ProblemAccountCreditDepositResource.UpdateAcknowledgementStatusConfirmMessage, result.Message);
            }

            return result;
        }

        public IEnumerable<IdNameModel> GetCurrencies()
        {
            var currencies = CacheManager.Get<List<IdNameModel>>(CurrencyCacheKey);

            if (currencies == null)
            {
                currencies = repository.GetCurrencies(new GetCurrenciesLicenseeSiteCriteria()).ToList();
                currencies.Insert(0, new IdNameModel { Id = 0, Name = ProblemAccountResource.All });

                CacheManager.Set(CurrencyCacheKey, currencies);
            }

            return currencies;
        }

        public IEnumerable<IdNameModel> GetReasons(bool isAllReasons)
        {
            var mode = isAllReasons ? (byte)2 : (byte)1;
            var cacheKey = string.Format(ReasonCacheKey, mode);

            var reasons = CacheManager.Get<List<IdNameModel>>(cacheKey);

            if (reasons == null)
            {
                reasons = repository.GetReasons(new GetReasonLicenseeSiteCriteria(mode)).ToList();
                CacheManager.Set(cacheKey, reasons);
            }

            return reasons;
        }

        public IEnumerable<IdNameModel> GetPreventiveActions()
        {
            var preventiveActions = CacheManager.Get<List<IdNameModel>>(PreventiveActionCacheKey);

            if (preventiveActions == null)
            {
                preventiveActions = repository.GetPreventiveActions(new GetPreventiveActionLicenseeSiteCriteria()).ToList();
                CacheManager.Set(PreventiveActionCacheKey, preventiveActions);
            }

            return preventiveActions;
        }

        public ProblemAccountLicenseeSiteModel GetProblemAccounts(
            DateTime from,
            DateTime to,
            byte acknowledge,
            byte vip,
            int currencyId,
            byte isReduced,
            string reasonIds,
            byte preventiveActionId,
            string member,
            string licensee,
            int pageIndex,
            int pageSize)
        {
            var criteria = new SearchLicenseeSiteCriteria(
                from,
                to.GetEndOfDate(),
                acknowledge,
                vip,
                currencyId,
                isReduced,
                preventiveActionId,
                member,
                licensee,
                reasonIds ?? string.Empty,
                pageIndex,
                pageSize);

            var rawData = repository.GetProblemAccounts(criteria);
            var totalItems = rawData.Item3;

            return new ProblemAccountLicenseeSiteModel(
                totalItems,
                totalItems == 0
                    ? Enumerable.Empty<ProblemAccountSiteGroup>()
                    : BuildProblemAccountSiteGroups(rawData.Item1, rawData.Item2));
        }

        private static IEnumerable<ProblemAccountSiteGroup> BuildProblemAccountSiteGroups(
            IEnumerable<ProblemAccountLicenseeSiteItem> problemAccounts,
            IEnumerable<ProblemAccountReason> reasons)
        {
            var reasonHash = BuildProblemAccountReasons(reasons);
            var siteGroups = problemAccounts.GroupBy(p => new { p.Site, p.IsAcknowledgement });

            foreach (var group in siteGroups)
            {
                foreach (var item in group)
                {
                    item.Reasons = reasonHash.ContainsKey(item.CustId) ? reasonHash[item.CustId] : null;
                }

                yield return new ProblemAccountSiteGroup(
                    group.Key.Site,
                    group.Key.IsAcknowledgement
                        ? ProblemAccountCreditDepositResource.Acknowledged
                        : ProblemAccountCreditDepositResource.NotAcknowledged,
                    group);
            }
        }

        private static IDictionary<int, IList<string>> BuildProblemAccountReasons(IEnumerable<ProblemAccountReason> reasons)
        {
            IList<string> reasonList;
            IDictionary<int, IList<string>> reasonHash = new Dictionary<int, IList<string>>();

            foreach (var item in reasons)
            {
                if (!reasonHash.TryGetValue(item.CustId, out reasonList))
                {
                    reasonList = new List<string>();
                    reasonHash.Add(item.CustId, reasonList);
                }

                reasonList.Add(
                    string.Format("{0}{1}: {2}",
                    item.CreateDate.ToShortFormatedDateWithSlash(),
                    string.IsNullOrWhiteSpace(item.Username) ? string.Empty : $" ({item.Username})",
                    item.Reason));
            }

            return reasonHash;
        }

        public Result MarkNew(
            string usernames,
            byte preventiveActionId,
            byte reasonId,
            string comment,
            string voidTime,
            decimal limitAmount,
            int userId,
            string ip)
        {
            var result = repository.ExecuteNonQuery(
                new MarkNewLicenseeSiteCriteria(usernames, preventiveActionId, reasonId, HtmlEncodeText(comment), voidTime, limitAmount, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.MarkNewSuccessfully;
            }

            return result;
        }

        public Result Update(
            string usernames,
            byte preventiveActionId,
            byte reasonId,
            string comment,
            string voidTime,
            decimal limitAmount,
            bool updateTime,
            bool updateLimitAmount,
            int userId,
            string ip)
        {
            var updateResult = repository.Update(
                new UpdateLicenseeSiteCriteria(usernames, preventiveActionId, reasonId, HtmlEncodeText(comment), voidTime, limitAmount,updateTime, updateLimitAmount, userId, ip),
                reasonId);
            var result = updateResult.Item1;

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountCreditDepositResource.UpdateProblemAccountSuccessfully;

                ctsService.UpdateProblemAccounts(new CTSUpdateProblemAccount(updateResult.Item2, userId));
            }

            return result;
        }

        private static string HtmlEncodeText(string text)
            => string.IsNullOrWhiteSpace(text) ? text : WebUtility.HtmlEncode(text);

        public Result Unmark(string usernames, int userId, string ip)
        {
            var unmarkResult = repository.Unmark(new UnmarkLicenseeSiteCriteria(usernames, userId, ip));

            var result = unmarkResult.Item1;

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.UnmarkSuccessfully;

                ctsService.RemoveProblemAccounts(unmarkResult.Item2);
            }

            return result;
        }

        public Result ReduceBL(
            string usernames,
            int maxPerMatch,
            int maxBet,
            int userId,
            string ip)
        {
            var result = repository.ExecuteNonQuery(
                new ReduceBLLicenseeSiteCriteria(usernames, maxPerMatch, maxBet, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.ReduceBLSuccessfully;
            }

            return result;
        }

        public Result RemoveReduceBL(string usernames, bool confirmed, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(
                new RemoveReduceBLLicenseeSiteCriteria(usernames, confirmed, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.RemoveReduceBLSuccessfully;
            }
            else if (IsNeedTobeConfirmedResult(result))
            {
                result.Message = string.Format(ProblemAccountResource.RemoveReduceBLConfirmMessage, result.Message);
            }
            else if (IsAlreadyInactiveOrRemoveReduceBL(result))
            {
                result.Message = ProblemAccountResource.AllAccountsAlreadyRemoveReduceBL;
            }

            return result;
        }

        private static bool IsNeedTobeConfirmedResult(Result result)
            => result.Code == SQLErrorCodeConst.NeedTobeConfirmedCode;

        private static bool IsAlreadyInactiveOrRemoveReduceBL(Result result)
            => result.Code == SQLErrorCodeConst.AlreadyInactiveOrRemoveReduceBL;

        public Result UpdateInformingStatus(string usernames, byte informType, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(
                new UpdateInformingStatusLicenseeSiteCriteria(usernames, informType, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.UpdateInformingStatusSuccessfully;
            }

            return result;
        }
    }
}
