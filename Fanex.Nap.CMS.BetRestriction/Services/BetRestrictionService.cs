namespace Fanex.NAP.CMS.Services.Implements
{
    using Fanex.Data;
    using Fanex.Nap.CMS.BetRestriction.Criterias;
    using Fanex.Nap.CMS.BetRestriction.Models;
    using Fanex.Nap.CMS.BetRestriction.Requests;
    using Fanex.Nap.CMS.BetRestriction.ViewModels;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Resources;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Web;
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
            var result = new BetRestrictionSearchViewModel
            {
                AccountViewModel = SearchDataGrid<AccountModel>(criteria),
                CurrencyViewModel = SearchDataGrid<CurrencyModel>(new SearchCurrencyCriteria(criteria.PageSize, criteria.PageIndex)),
                SiteViewModel = SearchDataGrid<SiteModel>(new SearchSiteCriteria(criteria.PageSize, criteria.PageIndex))

            };           
            return result;
        }

        public BetRestrictionDataGridViewModel<T> SearchDataGrid<T>(PagingCriteria criteria)
        => new BetRestrictionDataGridViewModel<T>
        {
            DataList = this.Repository.Fetch<T>(criteria),
            TotalItems = criteria.TotalItems
        };

        public Result CheckAddAccount(AccountRequest request, short sportType) => CheckAccount(request, ComposeCheckAddAccountMessage, sportType);

        public EditAccountViewModel GetAccountDetailForEdit(int custId, short sportType) => Repository.Get<EditAccountViewModel>(new GetAccountDetailCriteria { CustId = custId, SportType = sportType });

        public EditSiteViewModel GetSiteDetailForEdit(int siteId) => Repository.Get<EditSiteViewModel>(new GetSiteDetailCriteria { SiteId = siteId });

        public EditCurrencyViewModel GetCurrencyDetailForEdit(int exchangeId, int type) 
        => Repository.Get<EditCurrencyViewModel>(new GetCurrencyDetailCriteria
        {
            ExchangeId = exchangeId,
            Type = type
        });

        private Result CheckAccount(AccountRequest request, Func<AccountRequest, CheckAddAccountResultModel, string> composeMsgFunction, short sportType)
        {
            var criteria = new CheckAccountCriteria
            {
                UserName = request.UserName,
                SportType = sportType
            };

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                if (reader.Read())
                {
                    var errMsg = composeMsgFunction(request, new CheckAddAccountResultModel(reader, criteria.UserName, sportType));
                    return string.IsNullOrWhiteSpace(errMsg) ? new Result() : new Result(500, errMsg + Labels.BetRestriction_WarningProceed);
                }
                else
                {
                    return new Result();
                }
            }
        }

        private static string ComposeCheckAddAccountMessage(AccountRequest request, CheckAddAccountResultModel checkResult)
        {
            
            var sportBetRestrictionName = string.Format("Bet Restriction - {0}", CacheRef.GetSportName(checkResult.SportType));
            var result = string.Empty;
            if (checkResult.IsDownlineExists)
            {
                result += string.Format(Labels.BetRestriction_DownlinesExisted, checkResult.UserName, sportBetRestrictionName) + Environment.NewLine;
            }

            if (checkResult.IsUplineExists)
            {
                result += string.Format(Labels.BetRestriction_UplineExisted, checkResult.UplineLevel, checkResult.ExistingUplineUserName, sportBetRestrictionName) + Environment.NewLine;
            }


            return result;
        }
    }
}
