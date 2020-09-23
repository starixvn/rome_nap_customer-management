namespace Fanex.NAP.CMS.BetLimitControl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Customers.Resources;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.NAP.CMS.BetLimitControl.Criteria;
    using Fanex.NAP.CMS.BetLimitControl.Models;
    using Fanex.NAP.CMS.BetLimitControl.Request;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;

    public interface IService
    {
        IndexViewModel GetIndexViewModel();

        SearchModel Search(SearchRequest request);

        IEnumerable<Result> Add(AddRequest addRequest);

        Result Update(UpdateRequest updateRequest);

        Result Delete(DeleteRequest deleteRequest);
    }

    public class Service : IService
    {
        private const int MaxUsernames = 100;

        private readonly IServiceBase serviceBase;
        private readonly ICurrentSession currentSession;
        private readonly ICurrentContext currentContext;

        public Service(ICurrentSession currentSession, ICurrentContext currentContext)
            : this(currentSession, currentContext, null)
        {
        }

        public Service(ICurrentSession currentSession, ICurrentContext currentContext, IServiceBase serviceBase)
        {
            this.currentSession = currentSession;
            this.currentContext = currentContext;
            this.serviceBase = serviceBase ?? new ServiceBase();
        }

        public IEnumerable<Result> Add(AddRequest addRequest)
        {
            if (addRequest.Username.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length > MaxUsernames)
            {
                yield return new Result(MaxUsernames, Labels.Username_LimitCountWarning);
                yield break;
            }

            var criteria = new AddBetLimitControlCriteria
            {
                Usernames = addRequest.Username,
                BetLimitTypeID = addRequest.BetLimitTypeID,
                MaxBet = addRequest.MaxBet,
                MaxPerMatch = addRequest.MaxPerMatch,
                UserId = currentSession.UserId,
                IP = currentContext.IP
            };

            var groupResults = serviceBase
                .GetRepositoryData<AddBetLimitControlResult>(criteria)
                .GroupBy(r => new { r.ErrorCode, r.ErrorMsg });

            foreach (var group in groupResults)
            {
                var usernames = string.Join(", ", group.Select(r => r.Username));

                if (usernames.Length > 0)
                {
                    usernames += ": ";
                }

                yield return new Result(group.Key.ErrorCode, $"{usernames}{group.Key.ErrorMsg}");
            }
        }

        public Result Delete(DeleteRequest deleteRequest)
        {
            var criteria = new DeleteBetLimitControlCriteria
            {
                CustID = deleteRequest.CustID,
                BetLimitTypeID = deleteRequest.BetLimitTypeID,
                UserId = currentSession.UserId,
                IP = currentContext.IP
            };

            serviceBase.ExecuteNonQueryWithMapOutput(criteria);

            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }

        public IndexViewModel GetIndexViewModel()
        {
            var currencies = serviceBase.GetRepositoryData<CurrencyModel>(new GetAllCurrencyCriteria());
            var sites = serviceBase.GetRepositoryData<SiteModel>(new GetSiteByPermissionCriteria(currentSession.UserId));
            var betLimitTypes = serviceBase.GetRepositoryData<BetLimitTypeModel>(new GetAllBetLimitTypeCriteria());
            return new IndexViewModel
            {
                Currencies = currencies,
                Sites = sites,
                BetLimitTypes = betLimitTypes
            };
        }

        public SearchModel Search(SearchRequest request)
        {
            var criteria = new SearchCriteria
            {
                UserId = currentSession.UserId,
                LevelID = request.LevelId,
                Currency = request.Currency,
                SiteID = request.SiteId,
                BetLimitTypeID = request.BetLimitTypeID,
                Username = request.Username,
                PageSize = request.PageSize,
                PageIndex = request.PageIndex,
            };
            var customerBetLimitControls = serviceBase.GetRepositoryData<CustomerBetLimitModel>(criteria);

            var result = new SearchModel
            {
                TotalItems = criteria.TotalItems,
                CustomerBetLimitControls = customerBetLimitControls.GroupBy(control => control.Username)
            };

            return result;
        }

        public Result Update(UpdateRequest updateRequest)
        {
            var criteria = new UpdateBetLimitControlCriteria
            {
                CustID = updateRequest.CustID,
                BetLimitTypeID = updateRequest.BetLimitTypeID,
                NewBetLimitTypeID = updateRequest.NewBetLimitTypeID,
                MaxBet = updateRequest.MaxBet,
                MaxPerMatch = updateRequest.MaxPerMatch,
                UserId = currentSession.UserId,
                IP = currentContext.IP
            };

            serviceBase.ExecuteNonQueryWithMapOutput(criteria);

            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }
    }
}