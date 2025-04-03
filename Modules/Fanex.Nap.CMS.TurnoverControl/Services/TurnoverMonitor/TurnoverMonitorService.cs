using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.Nap.CMS.Infrastructure.Extensions;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
using Fanex.Nap.CMS.Infrastructure.Web;
using Fanex.Nap.CMS.TurnoverControl.Constants;
using Fanex.Nap.CMS.TurnoverControl.Criteria.TurnoverMonitor;
using Fanex.Nap.CMS.TurnoverControl.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;
using Fanex.Nap.CMS.TurnoverControl.Resources;

namespace Fanex.Nap.CMS.TurnoverControl.Services.TurnoverMonitor
{
    public class TurnoverMonitorService : ServiceBase, ITurnoverMonitorService
    {
        public IEnumerable<IdNameModel> GetSports(byte mode, short sportType)
        {
            return DynamicRepository
                .Fetch<SportTypeTurnover>(new GetSportsCriteria(mode, sportType))
                .Select(x => new IdNameModel { Id = x.SportType, Name = x.SportName });
        }

        public IEnumerable<IdNameModel> GetLeagues(string fromDate, string toDate, short sportType, DateTime moveBetsDay)
        {
            var leagues = DynamicRepository
                .Fetch<LeagueTurnover>(new GetLeaguesCriteria(fromDate, toDate, sportType, moveBetsDay))
                .Select(x => new IdNameModel { Id = x.LeagueId, Name = CacheRef.GetLeagueName(x.LeagueId.ToString()) })
                .ToList();
            leagues.Insert(0, new IdNameModel { Id = 0, Name = Labels.All_Leagues });

            return leagues;
        }

        public IEnumerable<IdNameModel> GetMatches(string fromDate, string toDate, int leagueId)
        {
            var matches = DynamicRepository.Fetch<MatchDTO>(new GetMatchesCriteria(fromDate, toDate, leagueId))
                .Select(x => new IdNameModel { Id = x.MatchId, Name = $"{CacheRef.GetTeamName(x.HomeId)} -vs- {CacheRef.GetTeamName(x.AwayId)}" })
                .ToList();
            matches.Insert(0, new IdNameModel { Id = 0, Name = Labels.AllMatches });

            return matches;
        }

        public IEnumerable<LeagueTurnover> GetCustomerTurnovers(
            string fromDate,
            string toDate,
            short sportType,
            int leagueId,
            int matchId,
            string username,
            bool isExcludeCompletedEvent,
            int percentageUsed,
            DateTime moveBetsDay)
        {
            var rawData = DynamicRepository.Fetch<TurnoverMonitorItem>(
                new GetCustomerTurnoversCriteria(
                    fromDate,
                    toDate,
                    sportType,
                    leagueId,
                    matchId,
                    username,
                    isExcludeCompletedEvent,
                    percentageUsed,
                    moveBetsDay));

            if (!rawData.Any())
            {
                yield break;
            }

            var leagueLookup = rawData.ToLookup(kvp => kvp.LeagueId, kvp => kvp);

            foreach (var leagueGroup in leagueLookup)
            {
                yield return GetLeagueTurnover(leagueGroup);
            }
        }

        private static LeagueTurnover GetLeagueTurnover(IGrouping<int, TurnoverMonitorItem> leagueGroup)
        {
            IList<MatchTurnover> matchTurnovers = new List<MatchTurnover>();
            var matchLookup = leagueGroup.ToLookup(kvp => kvp.MatchId, kvp => kvp);

            foreach (var matchGroup in matchLookup)
            {
                matchTurnovers.Add(GetMatchTurnover(matchGroup));
            }

            return new LeagueTurnover(
                leagueGroup.Key,
                CacheRef.GetLeagueName(leagueGroup.Key.ToString()),
                matchTurnovers);
        }

        private static MatchTurnover GetMatchTurnover(IGrouping<int, TurnoverMonitorItem> matchGroup)
        {
            var firstMatch = matchGroup.FirstOrDefault();
            var matchTurnover = new MatchTurnover(
                matchGroup.Key,
                GetMatchName(firstMatch.HomeId, firstMatch.AwayId, firstMatch.IsNeutral),
                firstMatch.EventDate.ToShortFormatedDateWithSlash(),
                firstMatch.EventStatus);

            var customerLookup = matchGroup.ToLookup(kvp => new { kvp.CustId, kvp.Level }, kvp => kvp);

            foreach (var customerGroup in customerLookup)
            {
                matchTurnover.CustomerTurnovers.Add(GetCustomerTurnover(customerGroup));
            }

            return matchTurnover;
        }

        private static string GetMatchName(int homeId, int awayId, bool isNeutral)
            => $"{CacheRef.GetTeamName(homeId)}{(isNeutral ? " (N)" : string.Empty)} -vs- {CacheRef.GetTeamName(awayId)}";

        private static CustomerTurnover GetCustomerTurnover(IGrouping<object, TurnoverMonitorItem> customerGroup)
        {
            var firstCustomer = customerGroup.FirstOrDefault();
            var customerTurnover = new CustomerTurnover(firstCustomer);
            BetTypeTurnover maxTurnover;

            foreach (var item in customerGroup)
            {
                maxTurnover = new BetTypeTurnover(item);

                if (item.GroupName.Equals(GroupNameConst.HDPML, StringComparison.OrdinalIgnoreCase))
                {
                    customerTurnover.HDPML.Add(maxTurnover);
                }
                else if (item.GroupName.Equals(GroupNameConst.OU, StringComparison.OrdinalIgnoreCase))
                {
                    customerTurnover.OU.Add(maxTurnover);
                }
                else if (item.GroupName.Equals(GroupNameConst._1HHDP1HML, StringComparison.OrdinalIgnoreCase))
                {
                    customerTurnover._1HHDP1HML.Add(maxTurnover);
                }
                else if (item.GroupName.Equals(GroupNameConst._1HOU, StringComparison.OrdinalIgnoreCase))
                {
                    customerTurnover._1HOU.Add(maxTurnover);
                }
                else
                {
                    customerTurnover.Others.Add(maxTurnover);
                }
            }

            return customerTurnover;
        }

        public IEnumerable<HouseTurnover> GetHouseTurnovers(int custId, int matchId, bool isGroupHeadline, string eventDate)
        {
            var houseTurnovers = DynamicRepository.Fetch<HouseTurnover>(
                new GetHouseTurnoverCriteria(custId, matchId, isGroupHeadline, eventDate));

            if (houseTurnovers.Any())
            {
                houseTurnovers = houseTurnovers.OrderBy(x => x.BetTypeId == -1).ThenBy(x => x.BetTypeId);
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

        public Result UpdateMaxTurnover(UpdateMaxTurnoverParam param, int userId, string ip)
        {
            var criteria = new UpdateMaxTurnoverCriteria
            {
                MatchId = param.MatchId,
                CustId = param.CustId,
                ProfileId = param.ProfileId,
                MaxTurnover = param.MaxTurnover,
                IsGroupHeadline = param.IsGroupHeadline,
                SportType = param.SportType,
                UserId = userId,
                IP = ip
            };

            var result = DoAction(criteria);

            if (result.IsSuccess)
            {
                result.Message = Labels.GlobalSetting_UpdateMaxTurnover_Success;
            }

            return result;
        }

        public Result UpdateMultipleMaxTurnover(IEnumerable<UpdateMaxTurnoverParam> param, int userId, string ip)
        {
            var criteria = new UpdateMultipleMaxTurnoverCriteria
            {
                GlobalSetting = ToXml(param),
                UserId = userId,
                IP = ip
            };

            var result = DoAction(criteria);

            if (result.IsSuccess)
            {
                result.Message = Labels.GlobalSetting_UpdateMaxTurnover_Success;
            }

            return result;
        }
        private static string ToXml(IEnumerable<UpdateMaxTurnoverParam> globalSettings)
        {
            var xmlBuilder = new StringBuilder();
            xmlBuilder.Append("<ROOT>");

            foreach (var item in globalSettings)
            {
                xmlBuilder.Append(ToXml(item));
            }

            xmlBuilder.Append("</ROOT>");

            return xmlBuilder.ToString();
        }

        private static string ToXml(UpdateMaxTurnoverParam item)
            => string.Format(
                "<gs custID=\"{0}\" mid=\"{1}\" isGH=\"{2}\" maxTO=\"{3}\" spt=\"{4}\" pfID=\"{5}\"/>",
                item.CustId,
                item.MatchId,
                item.IsGroupHeadline ? 1 : 0,
                item.MaxTurnover,
                item.SportType,
                item.ProfileId);

        public Result Lock(LockSettingParam param, int userId, string ip)
        {
            var criteria = new LockEventSettingCriteria
            {
                CustId = param.CustId,
                MatchId = param.MatchId,
                IsEventLockTOLimit = param.IsLock,
                IsGroupHeadline = param.IsGroupHeadline,
                SportType = param.SportType,
                UserId = userId,
                IP = ip
            };

            return DoAction(criteria);
        }
    }
}