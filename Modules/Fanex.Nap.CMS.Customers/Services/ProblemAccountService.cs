namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Constants;
    using Criteria.ProblemAccount;
    using Fanex.Data.Repository;
    using Infrastructure.Extensions;
    using Infrastructure.Models;
    using Infrastructure.Web;
    using Models.ProblemAccount;
    using Repositories;
    using Resources;

    public class ProblemAccountService : IProblemAccountService
    {
        private const string CurrencyCacheKey = "Fanex.Nap.CMS.Customers.Services.ProblemAccountService.Currency";

        private const int NeedTobeConfirmedCode = 101;
        private const int AlreadyInactiveOrRemoveReduceBL = 102;
        protected readonly IProblemAccountRepository repository;

        public ProblemAccountService(IProblemAccountRepository repository = null)
        {
            this.repository = repository ?? new ProblemAccountRepository();
        }

        #region CN88

        public IEnumerable<IdNameModel> GetReasons(bool isAllReasons)
            => repository.GetReasons(new GetReasonCriteria(isAllReasons ? (byte)0 : (byte)1));

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
            int pageIndex,
            int pageSize)
        {
            ICriteria criteria = new SearchCriteria(
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
                pageIndex,
                pageSize);

            return GetProblemAccounts(criteria, AccountStatusConst.AccountStatusList);
        }

        public Result MarkNew(
            string usernames,
            byte roleId,
            byte reasonId,
            string comment,
            int userId,
            string ip)
            => repository.MarkNew(new MarkNewCriteria(usernames, roleId, reasonId, HtmlEncodeText(comment), userId, ip));

        public Result Unmark(
            string usernames,
            byte roleId,
            bool excludeAbnormalMember,
            bool excludeDirectMark,
            int userId,
            string ip)
            => Unmark(new UnmarkCriteria(usernames, roleId, excludeAbnormalMember, excludeDirectMark, userId, ip));

        public Result ReduceBL(
            string usernames,
            byte roleId,
            int maxPerEvent,
            int maxPerBet,
            int userId,
            string ip)
            => ReduceBL(new ReduceBLCriteria(usernames, roleId, maxPerEvent, maxPerBet, userId, ip));

        public Result Inactivate(string usernames, byte roleId, bool confirmed, int userId, string ip)
        {
            var result = repository.ExecuteNonQuery(
                new InactivateCriteria(usernames, roleId, confirmed, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.InactivateSuccessfully;
            }
            else if (IsNeedTobeConfirmedResult(result))
            {
                result.Message = string.Format(ProblemAccountResource.InactivateConfirmMessage, result.Message);
            }
            else if (IsAlreadyInactiveOrRemoveReduceBL(result))
            {
                result.Message = ProblemAccountResource.AllAccountsAlreadyInactivate;
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
            => RemoveReduceBL(new RemoveReduceBLCriteria(usernames, roleId, isAllDownlines, confirmed, userId, ip));

        public Result UpdateInformingStatus(
            string usernames,
            byte roleId,
            byte informType,
            bool isApplyDownlines,
            int userId,
            string ip)
        {
            var result = repository.ExecuteNonQuery(
                new UpdateInformingStatusCriteria(usernames, roleId, informType, isApplyDownlines, userId, ip));

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.UpdateInformingStatusSuccessfully;
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

        private static string ToDateTimeString(DateTime value)
            => value.ToString("MM/dd/yyyy HH:mm:ss");

        private static string HtmlEncodeText(string text)
            => string.IsNullOrWhiteSpace(text) ? text : WebUtility.HtmlEncode(text);

        private static bool IsNeedTobeConfirmedResult(Result result)
            => result.Code == NeedTobeConfirmedCode;

        private static bool IsAlreadyInactiveOrRemoveReduceBL(Result result)
            => result.Code == AlreadyInactiveOrRemoveReduceBL;

        #endregion

        #region Credit Sites

        public IEnumerable<IdNameModel> GetCurrencies()
        {
            var currencies = CacheManager.Get<List<IdNameModel>>(CurrencyCacheKey);

            if (currencies == null)
            {
                currencies = repository.GetCurrencies(new GetCurrenciesCriteria()).ToList();
                currencies.Insert(0, new IdNameModel { Id = 0, Name = ProblemAccountResource.All });

                CacheManager.Set(CurrencyCacheKey, currencies);
            }

            return currencies;
        }

        public IEnumerable<IdNameModel> GetReasonsCreditSites(bool isAllReasons)
            => repository.GetReasons(new GetReasonCreditDepositCriteria(isAllReasons ? (byte)0 : (byte)1, false));

        public ProblemAccountModel GetProblemAccountsCreditSites(
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
            int pageIndex,
            int pageSize)
        {
            ICriteria criteria = new SearchCreditSitesCriteria(
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
                pageIndex,
                pageSize);

            return GetProblemAccounts(criteria, AccountStatusCreditSitesConst.AccountStatusList);
        }

        public Result MarkNewCreditSites(
            string usernames,
            byte roleId,
            byte reasonId,
            string comment,
            int userId,
            string ip)
            => repository.MarkNew(new MarkNewCreditSitesCriteria(usernames, roleId, reasonId, HtmlEncodeText(comment), userId, ip));

        public Result UnmarkCreditSites(
            string usernames,
            byte roleId,
            bool excludeAbnormalMember,
            bool excludeDirectMark,
            int userId,
            string ip)
            => Unmark(new UnmarkCreditSitesCriteria(usernames, roleId, excludeAbnormalMember, excludeDirectMark, userId, ip));

        public Result ReduceBLCreditSites(
            string usernames,
            byte roleId,
            int maxPerMatch,
            int maxBet,
            int userId,
            string ip)
            => ReduceBL(new ReduceBLCreditSitesCriteria(usernames, roleId, maxPerMatch, maxBet, userId, ip));

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

        public Result RemoveReduceBLCreditSites(
            string usernames,
            byte roleId,
            bool isAllDownlines,
            bool confirmed,
            int userId,
            string ip)
            => RemoveReduceBL(new RemoveReduceBLCreditSitesCriteria(usernames, roleId, isAllDownlines, confirmed, userId, ip));

        #endregion

        private ProblemAccountModel GetProblemAccounts(ICriteria criteria, IDictionary<byte, string> accountStatus)
        {
            var rawData = repository.GetProblemAccounts(criteria);
            var reasonHash = BuildProblemAccountReasons(rawData.Item2);
            var problemAccounts = rawData.Item1;

            foreach (var item in problemAccounts)
            {
                item.StatusName = accountStatus[item.CustStatus];
                item.MarkDateString = ToDateTimeString(item.MarkDate);
                item.Reasons = reasonHash.ContainsKey(item.CustId) ? reasonHash[item.CustId] : null;
            }

            return new ProblemAccountModel(rawData.Item3, problemAccounts);
        }

        private Result Unmark(ICriteria criteria)
        {
            var result = repository.ExecuteNonQuery(criteria);

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.UnmarkSuccessfully;
            }

            return result;
        }

        private Result ReduceBL(ICriteria criteria)
        {
            var result = repository.ExecuteNonQuery(criteria);

            if (result.IsSuccess)
            {
                result.Message = ProblemAccountResource.ReduceBLSuccessfully;
            }

            return result;
        }

        private Result RemoveReduceBL(ICriteria criteria)
        {
            var result = repository.ExecuteNonQuery(criteria);

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
    }
}
