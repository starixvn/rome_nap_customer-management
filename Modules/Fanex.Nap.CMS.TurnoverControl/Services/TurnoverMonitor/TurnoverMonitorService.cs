// --------------------------------------------------------------------------
// <copyright file="TurnoverMonitorService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.Nap.CMS.TurnoverControl.Services.TurnoverMonitor
{
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Nap.CMS.Infrastructure.Constants;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor;
    using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;
    using Fanex.Nap.CMS.TurnoverControl.Repositories.TurnoverMonitor;
    using Fanex.Nap.CMS.TurnoverControl.Resources;

    /// <summary>
    /// Turnover Monitor Service.
    /// </summary>
    public class TurnoverMonitorService : ServiceBase, ITurnoverMonitorService
    {
        private const decimal MaxTurnoverPercentage = 100;

        private ITurnoverMonitorRepository _turnoverMonitorRepository;

        private Dictionary<object, string> _teamNameCache;

        public TurnoverMonitorService()
            : this(null)
        {
        }

        public TurnoverMonitorService(ITurnoverMonitorRepository turnoverMonitorRepository)
        {
            _turnoverMonitorRepository = turnoverMonitorRepository ?? new TurnoverMonitorRepository();
        }

        public List<IdNameModel> Sports(GetSportsCriteria criteria)
        {
            return _turnoverMonitorRepository.Sports(criteria);
        }

        public List<IdNameModel> Leagues(GetLeaguesCriteria criteria)
        {
            return _turnoverMonitorRepository.Leagues(criteria);
        }

        /// <summary>
        /// Gets the SMA turnover list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The SMA turnover list.</returns>
        public TurnoverInfoByGroup SMATurnoverList(GetSMATurnoverListCriteria criteria)
        {
            var rawData = _turnoverMonitorRepository.SMATurnoverList(criteria);
            TurnoverInfoByGroup result = new TurnoverInfoByGroup
            {
                Name = Labels.TurnoverMonitor_SMATurnoverList_Title,
                ChildTurnoverInfo = new List<TurnoverInfoByGroup>()
            };

            _teamNameCache = new Dictionary<object, string>();
            var homeIdList = rawData.Select(data => (object)data.MatchInfo.HomeTeam.Id).ToList();
            _teamNameCache = CacheRef.SaveCacheRefResponse(homeIdList, CacheRef.GetTeamName, _teamNameCache);
            var awayIdList = rawData.Select(data => (object)data.MatchInfo.AwayTeam.Id).ToList();
            _teamNameCache = CacheRef.SaveCacheRefResponse(awayIdList, CacheRef.GetTeamName, _teamNameCache);
            var leagueGroups = from turnoverData in rawData
                               group turnoverData by new { Id = turnoverData.League.Id, Name = turnoverData.League.Name }
                                   into leagueGroup
                                   select leagueGroup;

            foreach (var leagueGroup in leagueGroups)
            {
                var leagueTurnover = new TurnoverInfoByGroup
                {
                    Id = leagueGroup.Key.Id,
                    Name = leagueGroup.Key.Name,
                    ChildTurnoverInfo = SMATurnoverListByLeague(leagueGroup)
                };

                result.ChildTurnoverInfo.Add(leagueTurnover);
            }

            return result;
        }

        /// <summary>
        /// Gets the SMA turnover list by league.
        /// </summary>
        /// <param name="leagueGroup">The league group.</param>
        /// <returns>
        /// The SMA turnover list.
        /// </returns>
        public List<TurnoverInfoByGroup> SMATurnoverListByLeague(IGrouping<dynamic, CustomerTurnoverInfo> leagueGroup)
        {
            List<TurnoverInfoByGroup> result = new List<TurnoverInfoByGroup>();
            var matchGroups = from turnoverData in leagueGroup
                              group turnoverData by new { Id = turnoverData.MatchInfo.Id, Name = turnoverData.MatchInfo.Name }
                                  into matchGroup
                                  select matchGroup;

            foreach (var matchGroup in matchGroups)
            {
                var matchTurnover = new TurnoverInfoByGroup
                {
                    Id = matchGroup.Key.Id,
                    Name = matchGroup.Key.Name,
                    TurnoverInfo = new CustomerTurnoverInfo(),
                    ChildTurnoverInfo = SMATurnoverListByMatch(matchGroup)
                };

                matchTurnover.TurnoverInfo.MatchInfo = matchTurnover.ChildTurnoverInfo[0].TurnoverInfo.MatchInfo;
                result.Add(matchTurnover);
            }

            return result;
        }

        /// <summary>
        /// Gets the SMA turnover list by match.
        /// </summary>
        /// <param name="matchGroup">The match group.</param>
        /// <returns>
        /// The SMA turnover list.
        /// </returns>
        public List<TurnoverInfoByGroup> SMATurnoverListByMatch(IGrouping<dynamic, CustomerTurnoverInfo> matchGroup)
        {
            List<TurnoverInfoByGroup> result = new List<TurnoverInfoByGroup>();
            foreach (var detail in matchGroup)
            {
                var customerTurnover = new TurnoverInfoByGroup
                {
                    TurnoverInfo = detail
                };

                customerTurnover.TurnoverInfo.MatchInfo.HomeTeam.Name = _teamNameCache[customerTurnover.TurnoverInfo.MatchInfo.HomeTeam.Id];
                customerTurnover.TurnoverInfo.MatchInfo.AwayTeam.Name = _teamNameCache[customerTurnover.TurnoverInfo.MatchInfo.AwayTeam.Id];
                if (customerTurnover.TurnoverInfo.PercentageUsed > MaxTurnoverPercentage)
                {
                    customerTurnover.TurnoverInfo.PercentageUsed = MaxTurnoverPercentage;
                }

                result.Add(customerTurnover);
            }

            return result;
        }

        /// <summary>
        /// Gets the company turnover.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The company turnover.</returns>
        public List<TurnoverBalance> CompanyTurnover(GetCompanyTurnoverCriteria criteria)
        {
            var rawData = GetRepositoryData<dynamic>(criteria).ToList();
            var result = new List<TurnoverBalance>();
            var balanceLength = rawData.Count;
            for (var i = 0; i < balanceLength; i++)
            {
                var balanceData = rawData[i];
                var turnoverBalance = new TurnoverBalance
                {
                    Id = Converter.ToInt(balanceData.bettype),
                    Balance = Converter.ToString2(balanceData.BalanceInfo)
                };

                turnoverBalance.Name = CacheRef.GetBetType(turnoverBalance.Id);
                turnoverBalance.Name = string.IsNullOrEmpty(turnoverBalance.Name) ?
                    Labels.Others :
                    turnoverBalance.Name;
                result.Add(turnoverBalance);
            }

            return result;
        }

        public Result UpdateCustomerMaxTurnoverByMatch(UpdateMaxTurnoverByMatchCriteria criteria)
        {
            var result = _turnoverMonitorRepository.DoAction(criteria);
            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.GlobalSetting_UpdateMaxTurnover_Success;
            }

            return result;
        }
    }
}
