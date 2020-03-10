namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails;
    using Fanex.NAP.CMS.Core.Resources;

    public class VirtualRateDetailsService : IVirtualRateDetailsService
    {
        private const int Level1 = 1;
        private const int Level2 = 2;
        private const int Level3 = 3;
        private const int Level4 = 4;

        private readonly IVirtualRateDetailsRepository _repository;
        private readonly IAutoSuspendRepository _autoSuspendRepository;

        public VirtualRateDetailsService(
            IVirtualRateDetailsRepository repository = null,
            IAutoSuspendRepository autoSuspendRepository = null)
        {
            _repository = repository ?? new VirtualRateDetailsRepository();
            _autoSuspendRepository = autoSuspendRepository ?? new AutoSuspendRepository();
        }

        public VirtualRateDetailsReport GetVirtualRateDetails(GetVirtualRateDetailsCriteria criteria)
            => _repository.GetVirtualRateDetails(criteria);

        public Result SaveVirtualRateDetails(SaveVirtualRateDetailsCriteria criteria)
            => _repository.SaveVirtualRateDetails(criteria);

        public ValidateAssociatedAccountsResult ValidateAssociatedAccounts(ValidateAssociatedAccountsCriteria criteria)
            => _repository.ValidateAssociatedAccounts(criteria);

        public Result SaveAssociatedAccounts(SaveAssociatedAccountsCriteria criteria)
            => _repository.SaveAssociatedAccounts(criteria);

        public VirtualRateLogReport GetLogs(GetVirtualRateLogsCriteria criteria)
            => _repository.GetLogs(criteria);

        public VirtualRateRealValues ViewRealValues(ViewRealValuesCriteria criteria)
            => _repository.ViewRealValues(criteria);

        public AutoSuspendLogReport GetAutoSuspendLogs(GetAutoSuspendLogsCriteria criteria)
            => _autoSuspendRepository.GetAutoSuspendLogs(criteria);

        public GlobalAutoSuspendThreshold GetGlobalAutoSuspendThreshold()
        {
            var ranges = _repository.GetVirtualRateRanges();
            var minRange = ranges.Min(r => r.Value);
            var maxRange = ranges.Max(r => r.Value);

            var virtualRateDTOs = _repository
                .GetVirtualRateDetails(new GetAutoSuspendThresholdCriteria())
                .ToList();

            return new GlobalAutoSuspendThreshold(ranges, GetSports(virtualRateDTOs, minRange, maxRange));
        }

        private IEnumerable<VirtualRateThresholdByGroup> GetSports(
            IEnumerable<VirtualRateDTO> virtualRateDTOs,
            int minRange,
            int maxRange)
        {
            var sports = virtualRateDTOs.Where(d => d.LeagueGroupId == -1 && d.BetTypeGroupId == -1 && d.BetTypeId == -1);

            foreach (var sport in sports)
            {
                var leagues = virtualRateDTOs.Where(d => d.SportType == sport.SportType && d.LeagueGroupId != -1);

                yield return new VirtualRateThresholdByGroup(sport, minRange, maxRange, Level1)
                {
                    GroupId = sport.SportType,
                    GroupName = sport.SportName,
                    Childs = GetLeagues(virtualRateDTOs, leagues, minRange, maxRange),
                    HasRepetition = false
                };
            }
        }

        private IEnumerable<VirtualRateThresholdByGroup> GetLeagues(
            IEnumerable<VirtualRateDTO> rawData,
            IEnumerable<VirtualRateDTO> virtualRateDTOs,
            int minRange,
            int maxRange)
        {
            var leagues = virtualRateDTOs.Where(d => d.BetTypeGroupId == -1 && d.BetTypeId == -1);

            foreach (var league in leagues)
            {
                var betTypeGroups = virtualRateDTOs.Where(d => d.LeagueGroupId == league.LeagueGroupId && d.BetTypeGroupId != -1);

                yield return new VirtualRateThresholdByGroup(league, minRange, maxRange, Level2)
                {
                    GroupId = league.LeagueGroupId,
                    GroupName = league.LeagueGroupName,
                    Childs = GetBetTypeGroups(rawData, betTypeGroups, minRange, maxRange),
                    HasRepetition = rawData.Count(d => d.LeagueGroupId == league.LeagueGroupId && d.BetTypeGroupId == -1) > 1
                };
            }
        }

        private IEnumerable<VirtualRateThresholdByGroup> GetBetTypeGroups(
            IEnumerable<VirtualRateDTO> rawData,
            IEnumerable<VirtualRateDTO> virtualRateDTOs,
            int minRange,
            int maxRange)
        {
            var betTypeGroups = virtualRateDTOs.Where(d => d.BetTypeId == -1);

            foreach (var betTypeGroup in betTypeGroups)
            {
                var betTypes = virtualRateDTOs.Where(d => d.BetTypeGroupId == betTypeGroup.BetTypeGroupId && d.BetTypeId != -1);

                yield return new VirtualRateThresholdByGroup(betTypeGroup, minRange, maxRange, Level3)
                {
                    GroupId = betTypeGroup.BetTypeGroupId,
                    GroupName = betTypeGroup.BetTypeGroupName,
                    Childs = GetBetTypes(rawData, betTypes, minRange, maxRange),
                    HasRepetition = rawData.Count(d => d.BetTypeGroupId == betTypeGroup.BetTypeGroupId && d.BetTypeId == -1) > 1
                };
            }
        }

        private IEnumerable<VirtualRateThresholdByGroup> GetBetTypes(
           IEnumerable<VirtualRateDTO> rawData,
           IEnumerable<VirtualRateDTO> virtualRateDTOs,
           int minRange,
           int maxRange)
        {
            foreach (var betType in virtualRateDTOs)
            {
                yield return new VirtualRateThresholdByGroup(betType, minRange, maxRange, Level4)
                {
                    GroupId = betType.BetTypeId,
                    GroupName = betType.BetTypeName,
                    HasRepetition = rawData.Count(d => d.BetTypeId == betType.BetTypeId) > 1
                };
            }
        }

        public Result UpdateGlobalAutoSuspendThreshold(VirtualRateThresholdByGroup[] sports, int userId, string ip)
        {
            Result result = null;

            if (sports?.Length > 0)
            {
                result = _repository.ExecuteNonQuery(new UpdateAutoSuspendThresholdCriteria(sports, userId, ip));

                if (result.IsSuccess)
                {
                    result.Message = ViewLang.UpdateSuccessfull;
                }
            }

            return result ?? new Result();
        }

        public IEnumerable<GlobalAutoSuspendThresholdLog> GetGlobalAutoSuspendThresholdLogs(int sportType, DateTime fromDate, DateTime toDate)
        {
            var rawData = _repository.GetGlobalAutoSuspendThresholdLogs(
                new GetAutoSuspendThresholdLogCriteria(sportType, fromDate, toDate));

            var groupRawData = rawData.GroupBy(d => new { d.LogId, d.LogTime, d.Updatedby });

            foreach (var grp in groupRawData)
            {
                yield return new GlobalAutoSuspendThresholdLog(grp.Key.LogId, grp.Key.LogTime, grp.Key.Updatedby, GetSports(grp));
            }
        }

        private IEnumerable<GlobalAutoSuspendThresholdLogByGroup> GetSports(IEnumerable<GlobalAutoSuspendThresholdLogDTO> logs)
        {
            var sports = logs.Where(d => d.LeagueGroupId == -1 && d.BetTypeGroupId == -1 && d.BetType == -1);

            foreach (var sport in sports)
            {
                var leagues = logs.Where(d => d.SportType == sport.SportType && d.LeagueGroupId != -1);

                yield return new GlobalAutoSuspendThresholdLogByGroup(sport, Level1, sport.SportType, sport.SportName, GetLeagues(logs, leagues));
            }
        }

        private IEnumerable<GlobalAutoSuspendThresholdLogByGroup> GetLeagues(
            IEnumerable<GlobalAutoSuspendThresholdLogDTO> rawData,
            IEnumerable<GlobalAutoSuspendThresholdLogDTO> globalThresholdLogs)
        {
            var leagues = globalThresholdLogs.Where(d => d.BetTypeGroupId == -1 && d.BetType == -1);

            foreach (var league in leagues)
            {
                var betTypeGroups = globalThresholdLogs.Where(d => d.LeagueGroupId == league.LeagueGroupId && d.BetTypeGroupId != -1);

                yield return new GlobalAutoSuspendThresholdLogByGroup(league, Level2, league.LeagueGroupId, league.LeagueGroup, GetBetTypeGroups(rawData, betTypeGroups));
            }
        }

        private IEnumerable<GlobalAutoSuspendThresholdLogByGroup> GetBetTypeGroups(
            IEnumerable<GlobalAutoSuspendThresholdLogDTO> rawData,
            IEnumerable<GlobalAutoSuspendThresholdLogDTO> globalThresholdLogs)
        {
            var betTypeGroups = globalThresholdLogs.Where(d => d.BetType == -1);

            foreach (var betTypeGroup in betTypeGroups)
            {
                var betTypes = globalThresholdLogs.Where(d => d.BetTypeGroupId == betTypeGroup.BetTypeGroupId && d.BetType != -1);

                yield return new GlobalAutoSuspendThresholdLogByGroup(betTypeGroup, Level3, betTypeGroup.BetTypeGroupId, betTypeGroup.BetTypeGroup, GetBetTypes(rawData, betTypes));
            }
        }

        private IEnumerable<GlobalAutoSuspendThresholdLogByGroup> GetBetTypes(
           IEnumerable<GlobalAutoSuspendThresholdLogDTO> rawData,
           IEnumerable<GlobalAutoSuspendThresholdLogDTO> globalThresholdLogs)
        {
            foreach (var betType in globalThresholdLogs)
            {
                yield return new GlobalAutoSuspendThresholdLogByGroup(betType, Level4, betType.BetType, betType.BetTypeName, null);
            }
        }
    }
}
