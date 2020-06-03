namespace Fanex.NAP.CMS.BetLimitControl
{
    using System.Linq;
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

        Result Add(AddRequest addRequest);

        Result Update(UpdateRequest updateRequest);

        Result Delete(DeleteRequest deleteRequest);
    }

    public class Service : IService
    {
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

        public Result Add(AddRequest addRequest)
        {
            var checkUsernameBetLimitTypeResult = CheckUsernameAndType(addRequest.Username, addRequest.BetLimitTypeID);
            if (!checkUsernameBetLimitTypeResult.IsSuccess)
            {
                return checkUsernameBetLimitTypeResult;
            }

            var criteria = new AddBetLimitControlCriteria
            {
                Username = addRequest.Username,
                BetLimitTypeID = addRequest.BetLimitTypeID,
                MaxBet = addRequest.MaxBet,
                MaxPerMatch = addRequest.MaxPerMatch,
                UserId = currentSession.UserId,
                IP = currentContext.IP
            };

            serviceBase.ExecuteNonQueryWithMapOutput(criteria);

            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }

        public Result CheckUsernameAndType(string username, int betLimitType)
        {
            var criteria = new CheckUsernameBetLimitTypeCriteria(username, betLimitType, currentSession.UserId);

            serviceBase.ExecuteNonQueryWithMapOutput(criteria);

            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
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