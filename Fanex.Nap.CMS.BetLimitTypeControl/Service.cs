namespace Fanex.NAP.CMS.BetLimitTypeControl
{
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.NAP.CMS.BetLimitTypeControl.Criteria;
    using Fanex.NAP.CMS.BetLimitTypeControl.Models;
    using Fanex.NAP.CMS.BetLimitTypeControl.Requests;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using System.Collections.Generic;

    public interface IService
    {
        QueryFilterModel QueryFilter();

        IEnumerable<SearchModel> Search(SearchRequest request);

        Result Insert(InsertRequest request);

        Result Update(UpdateRequest request);

        Result Delete(DeleteRequest request);
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

        public QueryFilterModel QueryFilter()
        {
            using (var objDb = new ObjectDb("NAP_BLControl_SportBettypeSel"))
            {
                var reader = objDb.QueryMultiple();
                var products = reader.Read<Product>();
                var sportTypes = reader.Read<SportType>();
                var betTypes = reader.Read<BetType>();
                return new QueryFilterModel(products, sportTypes, betTypes);
            }
        }

        public IEnumerable<SearchModel> Search(SearchRequest request)
        {
            var criteria = new SearchCriteria
            {
                UserId = currentSession.UserId,
                ProductId = request.ProductId,
                SportType = request.SportTypeId,
                BetType = request.BetTypeId,
                LiveIndicator = request.LiveIndicator
            };
            return serviceBase.GetRepositoryData<SearchModel>(criteria);
        }

        public Result Insert(InsertRequest request)
        {
            var criteria = new InsertCriteria
            {
                BetLimitType = request.BetLimitType,
                ProductList = request.ProductList,
                SportTypeList = request.SportTypeList,
                BetTypeList = request.BetTypeList,
                LiveIndicator = request.LiveIndicator,
                LeagueList = request.LeagueList,
                BeginTime = request.BeginTime,
                EndTime = request.EndTime,
                RestrictVIP = request.RestrictVIP,
                IP = currentContext.IP,
                UserId = currentSession.UserId
            };
            serviceBase.ExecuteNonQueryWithMap(criteria);
            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }

        public Result Update(UpdateRequest request)
        {
            var criteria = new UpdateCriteria
            {
                BetLimitTypeId = request.BetLimitTypeId,
                BetLimitType = request.BetLimitType,
                ProductList = request.ProductList,
                SportTypeList = request.SportTypeList,
                BetTypeList = request.BetTypeList,
                LiveIndicator = request.LiveIndicator,
                LeagueList = request.LeagueList,
                BeginTime = request.BeginTime,
                EndTime = request.EndTime,
                RestrictVIP = request.RestrictVIP,
                IP = currentContext.IP,
                UserId = currentSession.UserId
            };
            serviceBase.ExecuteNonQueryWithMap(criteria);
            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }

        public Result Delete(DeleteRequest request)
        {
            var criteria = new UpdateCriteria
            {
                BetLimitTypeId = request.BetLimitTypeId,
                IP = currentContext.IP,
                UserId = currentSession.UserId
            };
            serviceBase.ExecuteNonQueryWithMap(criteria);
            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }
    }
}
