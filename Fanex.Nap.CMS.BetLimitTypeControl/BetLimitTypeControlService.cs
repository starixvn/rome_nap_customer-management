namespace Fanex.NAP.CMS.BetLimitTypeControl
{
    using System;
    using System.Collections.Generic;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.NAP.CMS.BetLimitTypeControl.Criteria;
    using Fanex.NAP.CMS.BetLimitTypeControl.Models;
    using Fanex.NAP.CMS.BetLimitTypeControl.Requests;
    using Fanex.NAP.Core.Web.Context;
    using Fanex.NAP.Core.Web.Session;
    using Newtonsoft.Json;

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

    public class BetLimitTypeControlService : IBetLimitTypeControlService
    {
        private readonly IServiceBase serviceBase;
        private readonly ICurrentSession currentSession;
        private readonly ICurrentContext currentContext;
        private const string CachedLeagues = "BLT-Leagues";
        private const int CachedDefaultInMinutes = 60;
        private const string SportBetTypeMatrixCachedKey = "SportBetTypeMatrixCachedKey";

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
        public IEnumerable<LeagueGroup> GetLeagueGroups()
        {
            var leagues = CacheManager.Get<IEnumerable<LeagueGroup>>(CachedLeagues);

            if (leagues == null)
            {
                leagues = serviceBase.GetRepositoryData<LeagueGroup>(new GetLeagueGroupCriteria(0));
                CacheManager.Set(CachedLeagues, leagues, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return leagues;
        }

        public IEnumerable<IdNameModel> GetLeagues(
            string leagueGroupIds,
            string leagueGroupListNotIn,
            string leagues)
        {
            var jsonSportBetType = GetSportBetType();

            return serviceBase.GetRepositoryData<IdNameModel>(
                new GetLeagueCriteria(leagueGroupIds, leagueGroupListNotIn, leagues, jsonSportBetType));
        }

        private string GetSportBetType()
        {
            var jsonSBM = CacheManager.Get<string>(SportBetTypeMatrixCachedKey);
            if (jsonSBM == null)
            {
                var criteria = new GetSportBettypeCriteria();
                var sporttypes = new List<object>();
                using (var db = new ObjectDb(criteria.GetSettingKey()))
                {
                    using (var reader = db.ExecuteReader(criteria.GetSpParams()))
                    {
                        while (reader.Read())
                        {
                            sporttypes.Add(new { Sporttype = Convert.ToInt32(reader["sporttype"]) });
                        }
                    }
                }

                jsonSBM = JsonConvert.SerializeObject(sporttypes);
                CacheManager.Set(SportBetTypeMatrixCachedKey, jsonSBM, DateTime.Now.AddMinutes(CachedDefaultInMinutes));
            }

            return jsonSBM;
            
        }
    }
}
