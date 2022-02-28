namespace Fanex.NAP.CMS.BetLimitTypeControl
{
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.NAP.CMS.BetLimitTypeControl.Criteria;
    using Fanex.NAP.CMS.BetLimitTypeControl.Models;
    using Fanex.NAP.CMS.BetLimitTypeControl.Requests;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;

    public interface IBetLimitTypeControlService
    {
        QueryFilterModel QueryFilter();

        IEnumerable<SearchModel> Search(SearchRequest request);

        Result Insert(InsertRequest request);

        Result Update(UpdateRequest request);

        Result Delete(DeleteRequest request);
    }

    public class BetLimitTypeControlService : IBetLimitTypeControlService
    {
        private readonly IServiceBase serviceBase;
        private readonly ICurrentSession currentSession;
        private readonly ICurrentContext currentContext;

        public BetLimitTypeControlService(ICurrentSession currentSession, ICurrentContext currentContext)
            : this(currentSession, currentContext, null)
        {
        }

        public BetLimitTypeControlService(ICurrentSession currentSession, ICurrentContext currentContext, IServiceBase serviceBase)
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
            => serviceBase.GetRepositoryData<SearchModel>(new SearchCriteria
            {
                UserId = currentSession.UserId,
                ProductId = request.ProductId,
                SportType = request.SportTypeId,
                BetType = request.BetTypeId,
                LiveIndicator = request.LiveIndicator
            });

        public Result Insert(InsertRequest request)
        {
            var criteria = new InsertCriteria
            {
                BetLimitType = request.BetLimitType,
                ProductList = request.ProductList,
                SportTypeList = request.SportTypeList,
                BetTypeList = request.BetTypeList,
                LiveIndicator = request.LiveIndicator,
                LeagueGroupList = request.LeagueGroupList,
                IsExcludedLeagueGroup = request.IsExcludedLeagueGroup,
                LeagueList = request.LeagueList,
                BeginTime = request.BeginTime,
                EndTime = request.EndTime,
                RestrictVIP = request.RestrictVIP,
                IP = currentContext.IP,
                UserId = currentSession.UserId
            };

            serviceBase.ExecuteNonQueryWithMapOutput(criteria);

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
                LeagueGroupList = request.LeagueGroupList,
                IsExcludedLeagueGroup = request.IsExcludedLeagueGroup,
                LeagueList = request.LeagueList,
                BeginTime = request.BeginTime,
                EndTime = request.EndTime,
                RestrictVIP = request.RestrictVIP,
                IP = currentContext.IP,
                UserId = currentSession.UserId
            };

            serviceBase.ExecuteNonQueryWithMapOutput(criteria);

            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }

        public Result Delete(DeleteRequest request)
        {
            var criteria = new DeleteCriteria
            {
                BetLimitTypeId = request.BetLimitTypeId,
                IP = currentContext.IP,
                UserId = currentSession.UserId
            };

            serviceBase.ExecuteNonQueryWithMapOutput(criteria);

            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }
    }
}
