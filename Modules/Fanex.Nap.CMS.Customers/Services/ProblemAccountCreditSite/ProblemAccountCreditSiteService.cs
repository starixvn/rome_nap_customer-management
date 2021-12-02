namespace Fanex.Nap.CMS.Customers.Services.ProblemAccountCreditSite
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Constants.ProblemAccount;
    using Constants.ProblemAccountCreditSite;
    using Criteria.ProblemAccountCreditSite;
    using Infrastructure.Extensions;
    using Infrastructure.Models;
    using Infrastructure.Web;
    using Models.ProblemAccount;
    using Repositories.ProblemAccount;
    using Repositories.ProblemAccountCreditSite;
    using Resources;

    public class ProblemAccountCreditSiteService : IProblemAccountCreditSiteService
    {
        private const string CurrencyCacheKey = "Fanex.Nap.CMS.Customers.Services.ProblemAccountCreditSiteService.Currency";
        private const string PreventiveActionCacheKey = "Fanex.Nap.CMS.Customers.Services.ProblemAccountCreditSiteService.PreventiveAction";
        private const string ReasonCacheKey = "Fanex.Nap.CMS.Customers.Services.ProblemAccountCreditSiteService.Reason_{0}";

        private readonly IProblemAccountRepository repository;
        private readonly IProblemAccountCTSService ctsService;

        public ProblemAccountCreditSiteService(
            IProblemAccountRepository repository = null,
            IProblemAccountCTSService ctsService = null)
        {
            this.repository = repository ?? new ProblemAccountCreditSiteRepository();
            this.ctsService = ctsService ?? new ProblemAccountCTSService();
        }

        public IEnumerable<IdNameModel> GetCurrencies()
        {
            var currencies = CacheManager.Get<List<IdNameModel>>(CurrencyCacheKey);

            if (currencies == null)
            {
                currencies = repository.GetCurrencies(new GetCurrenciesCreditSiteCriteria()).ToList();
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
                reasons = repository.GetReasons(new GetReasonCreditSiteCriteria(mode)).ToList();
                CacheManager.Set(cacheKey, reasons);
            }

            return reasons;
        }

        public IEnumerable<IdNameModel> GetPreventiveActions()
        {
            var preventiveActions = CacheManager.Get<List<IdNameModel>>(PreventiveActionCacheKey);

            if (preventiveActions == null)
            {
                preventiveActions = repository.GetPreventiveActions(new GetPreventiveActionCreditSiteCriteria()).ToList();
                CacheManager.Set(PreventiveActionCacheKey, preventiveActions);
            }

            return preventiveActions;
        }

        public ProblemAccountModel GetProblemAccounts(
            DateTime from,
            DateTime to,
            byte roleId,
            int currencyId,
            byte accountStatus,
            byte isReduced,
            string reasonIds,
            string username,
            bool isSearchDownlines,
            byte orderBy,
            byte isDirectlyMarked,
            byte preventiveActionId,
            int pageIndex,
            int pageSize)
        {
            var criteria = new SearchCreditSiteCriteria(
                from,
                to.GetEndOfDate(),
                roleId,
                currencyId,
                accountStatus,
                isReduced,
                reasonIds ?? string.Empty,
                username,
                isSearchDownlines,
                orderBy,
                isDirectlyMarked,
                preventiveActionId,
                pageIndex,
                pageSize);

            var rawData = repository.GetProblemAccounts(criteria);
            var preventiveActionHash = GetPreventiveActions().ToDictionary(kvp => (byte)kvp.Id, kvp => kvp.Name);
            var accountStatusHash = AccountStatusCreditSiteConst.AccountStatusList;
            var reasonHash = BuildProblemAccountReasons(rawData.Item2);
            var problemAccounts = rawData.Item1;

            foreach (var item in problemAccounts)
            {
                item.PreventiveAction = GetPreventiveAction(preventiveActionHash, item.PreventiveActionId);
                item.StatusName = GetStatusName(accountStatusHash, item.CustStatus);
                item.MarkDateString = ToDateTimeString(item.MarkDate);
                item.Reasons = reasonHash.ContainsKey(item.CustId) ? reasonHash[item.CustId] : null;
            }

            return new ProblemAccountModel(rawData.Item3, problemAccounts);
        }

        public Result MarkNew(
            string usernames,
            byte roleId,
            byte preventiveActionId,
            byte reasonId,
            string comment,
            string voidTime,
            decimal limitAmount,
            int userId,
            string ip)
            => repository.MarkNew(new MarkNewCreditSiteCriteria(usernames, roleId, preventiveActionId, reasonId, HtmlEncodeText(comment), voidTime, limitAmount, userId, ip));

        public Result Update(
            string usernames,
            byte roleId,
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
                new UpdateCreditSiteCriteria(usernames, roleId, preventiveActionId, reasonId, HtmlEncodeText(comment), voidTime, limitAmount, updateTime, updateLimitAmount, userId, ip),
                reasonId);
            var result = updateResult.Item1;

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountCreditDepositResource.UpdateProblemAccountSuccessfully;

                ctsService.UpdateProblemAccounts(new CTSUpdateProblemAccount(updateResult.Item2, userId));
            }

            return result;
        }

        public Result Unmark(
            string usernames,
            byte roleId,
            bool excludeAbnormalMember,
            bool excludeDirectMark,
            int userId,
            string ip)
        {
            var unmarkResult = repository.Unmark(
                new UnmarkCreditSiteCriteria(usernames, roleId, excludeAbnormalMember, excludeDirectMark, userId, ip));

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
            byte roleId,
            int maxPerEvent,
            int maxPerBet,
            int userId,
            string ip)
        {
            var result = repository.ExecuteNonQuery(
                new ReduceBLCreditSiteCriteria(usernames, roleId, maxPerEvent, maxPerBet, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.ReduceBLSuccessfully;
            }

            return result;
        }

        public Result Close(string usernames, byte roleId, bool confirmed, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(
                new CloseAccountCriteria(usernames, roleId, confirmed, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountCreditDepositResource.CloseSuccessfully;
            }
            else if (IsNeedTobeConfirmedResult(result))
            {
                result.Message = string.Format(ProblemAccountCreditDepositResource.CloseConfirmMessage, result.Message);
            }
            else if (IsAlreadyInactiveOrRemoveReduceBL(result))
            {
                result.Message = ProblemAccountCreditDepositResource.AllAccountsAlreadyClosed;
            }

            return result;
        }

        public Result RemoveReduceBL(
            string usernames,
            byte roleId,
            bool isAllDownlines,
            bool confirmed,
            int userId,
            string ip)
        {
            var result = repository.ExecuteNonQuery(
                new RemoveReduceBLCreditSiteCriteria(usernames, roleId, isAllDownlines, confirmed, userId, ip));

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

        private static IDictionary<int, IList<string>> BuildProblemAccountReasons(
            IEnumerable<ProblemAccountReason> reasons)
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

        private static string GetPreventiveAction(IDictionary<byte, string> preventiveActions, byte preventiveActionId)
        {
            preventiveActions.TryGetValue(preventiveActionId, out var preventiveAction);

            return preventiveAction;
        }

        private static string GetStatusName(IDictionary<byte, string> accountStatus, byte status)
        {
            accountStatus.TryGetValue(status, out var statusName);

            return statusName;
        }

        private static string ToDateTimeString(DateTime value)
            => value.ToString("MM/dd/yyyy HH:mm:ss");

        private static string HtmlEncodeText(string text)
            => string.IsNullOrWhiteSpace(text) ? text : WebUtility.HtmlEncode(text);

        private static bool IsNeedTobeConfirmedResult(Result result)
            => result.Code == SQLErrorCodeConst.NeedTobeConfirmedCode;

        private static bool IsAlreadyInactiveOrRemoveReduceBL(Result result)
            => result.Code == SQLErrorCodeConst.AlreadyInactiveOrRemoveReduceBL;

        public IEnumerable<string> GetVoidWinningTimes()
        {
            throw new NotImplementedException();
        }
    }
}
