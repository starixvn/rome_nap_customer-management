namespace Fanex.NAP.CMS.BetLimitTypeControl
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Web;
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

        IEnumerable<LeagueGroup> GetLeagueGroups();

        IEnumerable<IdNameModel> GetLeagues(string leagueGroupIds, string leagueGroupListNotIn, string leagues);
    }

    public class BetLimitTypeControlService : ServiceBase, IBetLimitTypeControlService
    {
        private readonly ICurrentSession currentSession;
        private readonly ICurrentContext currentContext;
        private const string CachedLeagues = "BLT-Leagues";
        private const int CachedDefaultInMinutes = 60;
        private const string SportBetTypeMatrixCachedKey = "SportBetTypeMatrixCachedKey";

        public BetLimitTypeControlService(ICurrentSession currentSession, ICurrentContext currentContext) 
        {
            this.currentSession = currentSession;
            this.currentContext = currentContext;
        }

        public QueryFilterModel QueryFilter()
        {
            var (products, sportTypes, betTypes) =  DynamicRepository.FetchMultiple<Product, SportType, BetType>(new GetProductSportBettypeCriteria());

            return new QueryFilterModel(products, sportTypes, betTypes);
        }

        public IEnumerable<SearchModel> Search(SearchRequest request)
            => DynamicRepository.Fetch<SearchModel>(new SearchCriteria
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
            DynamicRepository.Execute(criteria);

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

            DynamicRepository.Execute(criteria);

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

            DynamicRepository.Execute(criteria);

            return new Result(criteria.ErrorCode, criteria.ErrorMsg);
        }
        public IEnumerable<LeagueGroup> GetLeagueGroups()
        {
            var leagues = Nap.CMS.Infrastructure.Web.CacheManager.Get<IEnumerable<LeagueGroup>>(CachedLeagues);

            if (leagues == null)
            {
                leagues = DynamicRepository.Fetch<LeagueGroup>(new GetLeagueGroupCriteria(0));
                Nap.CMS.Infrastructure.Web.CacheManager.Set(CachedLeagues, leagues, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return leagues;
        }

        public IEnumerable<IdNameModel> GetLeagues(
            string leagueGroupIds,
            string leagueGroupListNotIn,
            string leagues)
        {
            //var jsonSportBetType = GetSportBetType();

            //return DynamicRepository.Fetch<IdNameModel>(
            //    new GetLeagueCriteria(leagueGroupIds, leagueGroupListNotIn, leagues, jsonSportBetType));

            return new List<IdNameModel>();
        }

        //private string GetSportBetType()
        //{
        //    var jsonSBM = CacheManager.Get<string>(SportBetTypeMatrixCachedKey);
        //    if (jsonSBM == null)
        //    {
        //        var criteria = new GetSportBettypeCriteria();
        //        var sporttypes = DynamicRepository.Fetch<SportType>(criteria).Select(sport => new { Sporttype = sport.SportTypeId });

        //        jsonSBM = JsonConvert.SerializeObject(sporttypes);
        //        CacheManager.Set(SportBetTypeMatrixCachedKey, jsonSBM, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
        //    }

        //    return jsonSBM;

        //}
    }
}
