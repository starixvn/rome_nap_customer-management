using System.Collections.Generic;
using System.Linq;
using Fanex.Nap.CMS.Infrastructure.Constants;
using Fanex.Nap.CMS.Infrastructure.Extensions;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Web;
using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;
using Fanex.Nap.CMS.TurnoverControl.Repositories.TurnoverMonitor;
using Fanex.Nap.CMS.TurnoverControl.Resources;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Services.TurnoverMonitor
{
    public class TurnoverMonitorService : ITurnoverMonitorService
    {
        private readonly ITurnoverMonitorRepository _turnoverMonitorRepository;

        public TurnoverMonitorService(ITurnoverMonitorRepository turnoverMonitorRepository = null)
        {
            _turnoverMonitorRepository = turnoverMonitorRepository ?? new TurnoverMonitorRepository();
        }

        public IEnumerable<IdNameModel> GetSports(ICriteria criteria)
        {
            return _turnoverMonitorRepository.GetSports(criteria);
        }

        public IEnumerable<IdNameModel> GetLeagues(ICriteria criteria)
        {
            var leagues = _turnoverMonitorRepository.GetLeagues(criteria).ToList();
            leagues.Insert(0, new IdNameModel { Id = 0, Name = Labels.All_Leagues });

            return leagues;
        }

        public IEnumerable<IdNameModel> GetMatches(ICriteria criteria)
        {
            var matches = _turnoverMonitorRepository.GetMatches(criteria).ToList();
            matches.Insert(0, new IdNameModel { Id = 0, Name = Labels.AllMatches });

            return matches;
        }

        public IEnumerable<LeagueTurnover> GetCustomerTurnovers(ICriteria criteria)
        {
            var rawData = _turnoverMonitorRepository.GetCustomerTurnovers(criteria);

            if (!rawData.Any())
            {
                return new List<LeagueTurnover>();
            }

            LeagueTurnover leagueTurnover;
            MatchTurnover matchTurnover;
            IList<LeagueTurnover> leagueTurnovers = new List<LeagueTurnover>();
            var leagueLookup = rawData.ToLookup(kvp => kvp.LeagueId, kvp => kvp);

            foreach (var leagueGroup in leagueLookup)
            {
                leagueTurnover = new LeagueTurnover(
                    leagueGroup.Key,
                    CacheRef.GetLeagueName(leagueGroup.Key.ToString()));

                var matchLookup = leagueGroup.ToLookup(kvp => kvp.MatchId, kvp => kvp);

                foreach (var matchGroup in matchLookup)
                {
                    matchTurnover = new MatchTurnover(matchGroup.Key);

                    foreach (var item in matchGroup)
                    {
                        matchTurnover.HomeId = item.HomeId;
                        matchTurnover.HomeName = CacheRef.GetTeamName(item.HomeId);

                        matchTurnover.AwayId = item.AwayId;
                        matchTurnover.AwayName = CacheRef.GetTeamName(item.AwayId);

                        matchTurnover.IsNeutral = item.IsNeutral;

                        matchTurnover.EventDate = item.EventDate.ToShortFormatedDateWithSlash();
                        matchTurnover.EventStatus = item.EventStatus;

                        matchTurnover.CustomerTurnovers.Add(item);
                    }

                    leagueTurnover.MatchTurnovers.Add(matchTurnover);
                }

                leagueTurnovers.Add(leagueTurnover);
            }

            return leagueTurnovers;
        }

        public IEnumerable<HouseTurnover> GetHouseTurnovers(ICriteria criteria)
        {
            var houseTurnovers = _turnoverMonitorRepository.GetHouseTurnovers(criteria);

            if (houseTurnovers.Any())
            {
                houseTurnovers = houseTurnovers.OrderByDescending(ht => ht.BetTypeId);
            }

            foreach (var item in houseTurnovers)
            {
                item.BetTypeName = CacheRef.GetBetType(item.BetTypeId);

                if (string.IsNullOrWhiteSpace(item.BetTypeName))
                {
                    item.BetTypeName = Labels.Others;
                }

                yield return item;
            }
        }

        public Result UpdateMaxTurnover(ICriteria criteria)
        {
            var result = _turnoverMonitorRepository.UpdateMaxTurnover(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.GlobalSetting_UpdateMaxTurnover_Success;
            }

            return result;
        }

        public Result UpdateMultipleMaxTurnover(ICriteria criteria)
        {
            var result = _turnoverMonitorRepository.UpdateMultipleMaxTurnover(criteria);

            if (result.Code == ErrorCodeConstants.NoneError)
            {
                result.Message = Labels.GlobalSetting_UpdateMaxTurnover_Success;
            }

            return result;
        }
    }
}