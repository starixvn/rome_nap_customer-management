namespace Fanex.NAP.CMS.Services.Implements
{
    using Fanex.Data;
    using Fanex.Nap.CMS.BetRestriction.Criterias;
    using Fanex.Nap.CMS.BetRestriction.Models;
    using Fanex.Nap.CMS.BetRestriction.Requests;
    using Fanex.Nap.CMS.BetRestriction.ViewModels;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Extensions;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Resources;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.NAP.CMS.Services.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class BetRestrictionService : ServiceBase, IBetRestrictionService
    {
        public IEnumerable<SelectListItem> GetSites(GetAllSiteCriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    yield return new SelectListItem
                    {
                        Text = reader["Site"].ToString(),
                        Value = reader["SiteID"].ToString()
                    };
                }
            }
        }

        public IEnumerable<SelectListItem> GetCurrency(GetAllCurrencyCriteria criteria)
        {
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    yield return new SelectListItem
                    {
                        Text = reader["currency"].ToString(),
                        Value = reader["exchangeid"].ToString()
                    };
                }
            }
        }

        public BetRestrictionSearchViewModel Search(SearchAccountCriteria criteria)
        {
            var result = new BetRestrictionSearchViewModel();
            result.AccountViewModel = SearchDataGrid<AccountModel>(criteria);
            result.CurrencyViewModel = SearchDataGrid<CurrencyModel>(new SearchCurrencyCriteria(criteria.PageSize, criteria.PageIndex));
            result.SiteViewModel = SearchDataGrid<SiteModel>(new SearchSiteCriteria(criteria.PageSize, criteria.PageIndex));
            return result;
        }

        public BetRestrictionDataGridViewModel<T> SearchDataGrid<T>(PagingCriteria criteria)
        => new BetRestrictionDataGridViewModel<T>
        {
            DataList = this.Repository.Fetch<T>(criteria),
            TotalItems = criteria.TotalItems
        };

        public Result CheckAddAccount(AccountRequest request) => CheckAccount(request, ComposeCheckAddAccountMessage);

        public EditAccountViewModel GetAccountDetailForEdit(int custId) => Repository.Get<EditAccountViewModel>(new GetAccountDetailCriteria { CustId = custId });

        public Result CheckEditAccount(AccountRequest request) => CheckAccount(request, ComposeCheckEditAccountMessage);

        public EditSiteViewModel GetSiteDetailForEdit(int siteId) => Repository.Get<EditSiteViewModel>(new GetSiteDetailCriteria { SiteId = siteId });

        public EditCurrencyViewModel GetCurrencyDetailForEdit(int exchangeId, int type) => Repository.Get<EditCurrencyViewModel>(new GetCurrencyDetailCriteria { ExchangeId = exchangeId, Type = type });

        private Result CheckAccount(AccountRequest request, Func<AccountRequest, CheckAddAccountResultModel, string> composeMsgFunction)
        {
            var criteria = new CheckAccountCriteria
            {
                UserName = request.UserName
            };

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                if (reader.Read())
                {
                    var errMsg = composeMsgFunction(request, new CheckAddAccountResultModel(reader, criteria.UserName));
                    return string.IsNullOrWhiteSpace(errMsg) ? new Result() : new Result(500, errMsg + Labels.BetRestriction_WarningProceed);
                }
                else
                {
                    return new Result();
                }
            }
        }

        private string ComposeCheckAddAccountMessage(AccountRequest request, CheckAddAccountResultModel checkResult)
        {
            var result = string.Empty;
            if (checkResult.IsDownlineExists)
            {
                result += string.Format(Labels.BetRestriction_DownlinesExisted, checkResult.UserName) + Environment.NewLine;
            }

            if (checkResult.IsUplineExists)
            {
                result += string.Format(Labels.BetRestriction_UplineExisted, checkResult.UplineLevel, checkResult.ExistingUplineUserName) + Environment.NewLine;
            }

            return result += ComposeCheckEditAccountMessage(request, checkResult);
        }

        private string ComposeCheckEditAccountMessage(AccountRequest request, CheckAddAccountResultModel checkResult)
        {
            var result = string.Empty;
            if (checkResult.IsUplineExists && checkResult.UplineSeizeBetTicket > request.SeizeBetTicket)
            {
                result += string.Format(Labels.BetRestriction_WarningUplineSeizeBetHigher, checkResult.UplineSeizeBetTicket) + Environment.NewLine;
            }

            if (checkResult.IsUplineExists && checkResult.MinDownlineSeizeBetTicket < request.SeizeBetTicket)
            {
                result += string.Format(Labels.BetRestriction_WarningDownlineSeizeBetLower, checkResult.MinDownlineSeizeBetTicket) + Environment.NewLine;
            }

            return result;
        }
    }
}
