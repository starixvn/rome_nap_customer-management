namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using Fanex.Common;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.VirtualSettingControl.Constants;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails;
    using Fanex.NAP.CMS.Core.Resources;
    using Microsoft.Practices.ObjectBuilder2;

    public class VirtualRateDetailsService : ServiceBase, IVirtualRateDetailsService
    {
        private const int Level1 = 1;
        private const int Level2 = 2;
        private const int Level3 = 3;
        private const int Level4 = 4;

        private readonly IAutoSuspendRepository _autoSuspendRepository;

        public VirtualRateDetailsService(IAutoSuspendRepository autoSuspendRepository = null)
        {
            _autoSuspendRepository = autoSuspendRepository ?? new AutoSuspendRepository();
        }

        public VirtualRateDetailsReport GetVirtualRateDetails(GetVirtualRateDetailsCriteria criteria)
        {
            var ranges = GetVirtualRateRanges();
            var minRange = ranges.Min(r => r.Value);
            var maxRange = ranges.Max(r => r.Value);

            var (reportDetails, virtualRateDetails) = DynamicRepository.FetchMultiple<VirtualRateDetailsReport, VirtualRateDTO>(criteria);
            var result = reportDetails.FirstOrDefault();
            result.VirtualRateValueRange = ranges;
            result.ReportLevel = string.IsNullOrWhiteSpace(result.Username) ? 0 : result.ReportLevel;
            virtualRateDetails.ForEach(virtualRate => 
            {
                virtualRate.SportName = CacheRef.GetSportName(virtualRate.SportType);
                virtualRate.BetTypeName = GetBetTypeName(virtualRate.SportType, virtualRate.BetTypeId);
                if(criteria.IsGlobalSetting)
                {
                    virtualRate.IsOverrideASLive = false;
                    virtualRate.IsOverrideASNonlive = false;
                }
            });

            result.ResultCode = criteria.ErrCode;
            result.ResultMessage = criteria.ErrMsg;
            result.Sports = GetSportsVirtualRateDetail(virtualRateDetails, minRange, maxRange);

            return result;
        }

        public Result SaveVirtualRateDetails(SaveVirtualRateDetailsCriteria criteria)
        {
            criteria.VirtualMultiplier = BuildXML(criteria);

            return DoAction(criteria, ViewLang.UpdateSuccessfull);
        }

        public ValidateAssociatedAccountsResult ValidateAssociatedAccounts(ValidateAssociatedAccountsCriteria criteria)
        {
            var accounts = criteria.Accounts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());
            criteria.Accounts = string.Join(",", accounts);
            var accountsResult = DynamicRepository.Fetch<ValidateAssociatedAccountsDTO>(criteria);
            var result = new ValidateAssociatedAccountsResult();
            accountsResult.ForEach(account =>
            {
                if (!account.IsValid)
                {
                    result.InvalidAccounts.Add(account.Username);
                }
                else
                {
                    result.ValidAccounts.Add(account.Username);
                    result.ValidAccountIds.Add(account.CustId.ToString());
                }
            });

            return result;
        }

        public Result SaveAssociatedAccounts(SaveAssociatedAccountsCriteria criteria)
        {
            if (string.IsNullOrWhiteSpace(criteria.Accounts))
            {
                return new Result(0, ViewLang.UpdateSuccessfull);
            }

            const bool hasFailed = false;
            var errorMessages = new List<string>();
            foreach (var custId in criteria.Accounts.Split(new[] { ',' }))
            {
                var updateResult = SaveVirtualRateDetails(new SaveVirtualRateDetailsCriteria
                {
                    CustId = Converter.ToInt(custId),
                    SportType = criteria.SportType,
                    UserId = criteria.UserId,
                    IP = criteria.IP,
                    Rates = criteria.Rates,
                    IsMember = criteria.IsMember,
                    IsManualUpdate = criteria.IsManualUpdate,
                    IsMemberCompactMode = criteria.IsMemberCompactMode
                });

                if (updateResult.Code != 0)
                {
                    errorMessages.Add(updateResult.Message);
                }
            }

            return new Result
            {
                Code = hasFailed ? 1 : 0,
                Message = hasFailed ? string.Join(",", errorMessages) : ViewLang.UpdateSuccessfull
            };
        }

        public VirtualRateLogReport GetLogs(GetVirtualRateLogsCriteria criteria)
        {
            var result = new VirtualRateLogReport
            {
                Username = criteria.Username,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                IncludeUpline = criteria.IncludeUpline
            };

            var dataSet = DynamicRepository.FetchDataSet(criteria);
            if(dataSet.Tables.Count > 0)
            {
                result.Currency = dataSet.Tables[0].Rows[0]["Currency"].ToString();
            }
            var logDTOs = new List<LogDTO>();
            if (dataSet.Tables.Count > 1)
            {
                foreach (DataRow item in dataSet.Tables[1].Rows)
                {
                    
                    logDTOs.Add(MapLogDTO(item));
                }
            }

            result.Logs = GroupLogs(logDTOs);

            return result;
        }

        public AutoSuspendLogReport GetAutoSuspendLogs(GetAutoSuspendLogsCriteria criteria)
            => _autoSuspendRepository.GetAutoSuspendLogs(criteria);

        public GlobalAutoSuspendThreshold GetGlobalAutoSuspendThreshold(int sportType)
        {
            var ranges = GetVirtualRateRanges();
            var minRange = ranges.Min(r => r.Value);
            var maxRange = ranges.Max(r => r.Value);
            var virtualRateDTOs = DynamicRepository.Fetch<VirtualRateDTO>(new GetAutoSuspendThresholdCriteria(sportType)).ToList();
            virtualRateDTOs.ForEach(d => d.BetTypeName = GetBetTypeName(sportType, d.BetTypeId));

            return new GlobalAutoSuspendThreshold(ranges, GetSports(virtualRateDTOs, minRange, maxRange));
        }

        public IEnumerable<RateInfo> GetVirtualRateRanges()
        {
            var criteria = new GetVirtualRateRangesCriteria();
            var ranges = DynamicRepository.Fetch<RateInfo>(criteria);
            var result = ranges.Where(r => r.Id != 0);
            result.ForEach(r => r.Value = (int)(r.Value * 100 - 100));

            return result;
        }
        public Result UpdateGlobalAutoSuspendThreshold(int sportType, VirtualRateThresholdByGroup[] sports, int userId, string ip)
        {
            Result result = null;

            if (sports?.Length > 0)
            {
                result = DoAction(new UpdateAutoSuspendThresholdCriteria(sportType, sports, userId, ip), ViewLang.UpdateSuccessfull);
            }

            return result ?? new Result();
        }

        public IEnumerable<GlobalAutoSuspendThresholdLog> GetGlobalAutoSuspendThresholdLogs(int sportType, DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetAutoSuspendThresholdLogCriteria(sportType, fromDate, toDate);
            var rawData = DynamicRepository.Fetch<GlobalAutoSuspendThresholdLogDTO>(criteria);
            rawData.ForEach(d => d.BetTypeName = GetBetTypeName(sportType, d.BetType));

            var groupRawData = rawData.GroupBy(d => new { d.LogId, d.LogTime, d.Updatedby });

            foreach (var grp in groupRawData)
            {
                yield return new GlobalAutoSuspendThresholdLog(grp.Key.LogId, grp.Key.LogTime, grp.Key.Updatedby, GetSports(grp));
            }
        }
        public IEnumerable<BetTypeVirtualRateMapping> GetBetTypeVirtualRateMapping(int sportType)
        {
            var mappings = DynamicRepository.Fetch<BetTypeVirtualRateMapping>(new GetBetTypeVirtualRateMappingCriteria(sportType));
            mappings.ForEach(m =>
            {
                m.BetTypeName = CacheRef.GetBetType(sportType, m.BetType);
                if (m.LeagueGroupOrder == 1)
                {
                    m.VRSetting = CacheRef.GetVRBetTypeName(SportTypeConstants.Basketball, int.Parse(m.VRSetting));
                }
            });

            return mappings;

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

        private IEnumerable<VirtualRateDetailsByGroup> GetSportsVirtualRateDetail(
            IEnumerable<VirtualRateDTO> virtualRateDTOs,
            int minRange,
            int maxRange)
        {
            var result = new List<VirtualRateDetailsByGroup>();

            var sports = virtualRateDTOs.Where(d => d.LeagueGroupId == -1 && d.BetTypeGroupId == -1 && d.BetTypeId == -1);
            foreach (var sport in sports)
            {
                var leagues = virtualRateDTOs.Where(d => d.SportType == sport.SportType && d.LeagueGroupId != -1);

                var group = new VirtualRateDetailsByGroup(sport, minRange, maxRange, 1)
                {
                    GroupId = sport.SportType,
                    GroupName = sport.SportName,
                    Childs = GetLeaguesVirtualRateDetail(virtualRateDTOs, leagues, minRange, maxRange),
                    HasRepetition = false
                };
                result.Add(group);
            }

            return result;
        }

        private IEnumerable<VirtualRateDetailsByGroup> GetLeaguesVirtualRateDetail(
            IEnumerable<VirtualRateDTO> rawData,
            IEnumerable<VirtualRateDTO> virtualRateDTOs,
            int minRange,
            int maxRange)
        {
            var result = new List<VirtualRateDetailsByGroup>();

            var leagues = virtualRateDTOs.Where(d => d.BetTypeGroupId == -1 && d.BetTypeId == -1);
            foreach (var league in leagues)
            {
                var betTypeGroups = virtualRateDTOs.Where(d => d.LeagueGroupId == league.LeagueGroupId && d.BetTypeGroupId != -1);

                var group = new VirtualRateDetailsByGroup(league, minRange, maxRange, 2)
                {
                    GroupId = league.LeagueGroupId,
                    GroupName = league.LeagueGroupName,
                    Childs = GetBetTypeGroupsVirtualRateDetail(rawData, betTypeGroups, minRange, maxRange),
                    HasRepetition = rawData.Count(d => d.LeagueGroupId == league.LeagueGroupId && d.BetTypeGroupId == -1) > 1
                };

                result.Add(group);
            }

            return result;
        }

        private IEnumerable<VirtualRateDetailsByGroup> GetBetTypeGroupsVirtualRateDetail(
            IEnumerable<VirtualRateDTO> rawData,
            IEnumerable<VirtualRateDTO> virtualRateDTOs,
            int minRange,
            int maxRange)
        {
            var result = new List<VirtualRateDetailsByGroup>();

            var betTypeGroups = virtualRateDTOs.Where(d => d.BetTypeId == -1);
            foreach (var betTypeGroup in betTypeGroups)
            {
                var betTypes = virtualRateDTOs.Where(d => d.BetTypeGroupId == betTypeGroup.BetTypeGroupId && d.BetTypeId != -1);

                var group = new VirtualRateDetailsByGroup(betTypeGroup, minRange, maxRange, 3)
                {
                    GroupId = betTypeGroup.BetTypeGroupId,
                    GroupName = betTypeGroup.BetTypeGroupName,
                    Childs = GetBetTypesVirtualRateDetail(rawData, betTypes, minRange, maxRange),
                    HasRepetition = rawData.Count(d => d.BetTypeGroupId == betTypeGroup.BetTypeGroupId && d.BetTypeId == -1) > 1
                };

                result.Add(group);
            }

            return result;
        }
        private IEnumerable<VirtualRateDetailsByGroup> GetBetTypesVirtualRateDetail(
           IEnumerable<VirtualRateDTO> rawData,
           IEnumerable<VirtualRateDTO> virtualRateDTOs,
           int minRange,
           int maxRange)
        {
            var result = new List<VirtualRateDetailsByGroup>();

            foreach (var betType in virtualRateDTOs)
            {
                var group = new VirtualRateDetailsByGroup(betType, minRange, maxRange, 4)
                {
                    GroupId = betType.BetTypeId,
                    GroupName = betType.BetTypeName,
                    HasRepetition = rawData.Count(d => d.BetTypeId == betType.BetTypeId) > 1
                };

                result.Add(group);
            }

            return result;
        }

        private IEnumerable<VirtualRateLog> GroupLogs(List<LogDTO> logDTOs)
        {
            var result = new List<VirtualRateLog>();
            if (!logDTOs.Any())
            {
                return result;
            }

            var groups = logDTOs.GroupBy(l => l.LogId);
            foreach (var group in groups)
            {
                var log = GetLog(group.ToList());
                result.Add(log);
            }

            return result;
        }
        private VirtualRateLog GetLog(List<LogDTO> logDTOs)
        {
            var firstLog = logDTOs[0];
            var result = new VirtualRateLog
            {
                LogId = firstLog.LogId,
                Username = firstLog.Username,
                ExchangeRate = firstLog.ExchangeRate,
                ExchangeRateText = firstLog.ExchangeRateText,
                UpdatedDate = firstLog.UpdatedDate,
                Editor = firstLog.Editor,
                Sports = GetSports(logDTOs)
            };

            return result;
        }
        private IEnumerable<VirtualRateDetailsByGroup> GetSports(IEnumerable<LogDTO> logDTOs)
        {
            var result = new List<VirtualRateDetailsByGroup>();
            var sports = logDTOs.Where(d => d.LeagueGroupId == -1 && d.BetTypeGroupId == -1 && d.BetTypeId == -1 && !d.IsNew);

            foreach (var sport in sports)
            {
                var leagues = logDTOs.Where(d => d.SportType == sport.SportType && d.LeagueGroupId != -1);
                var newLog = logDTOs.Where(d => d.SportType == sport.SportType && d.LeagueGroupId == -1 && d.BetTypeGroupId == -1 && d.BetTypeId == -1 && d.IsNew).FirstOrDefault() ?? sport;

                var group = new VirtualRateDetailsByGroup(sport, 1)
                {
                    GroupId = sport.SportType,
                    GroupName = sport.SportName,
                    Childs = GetLeagues(logDTOs, leagues),
                    NewNonliveLog = newLog.NonliveLog,
                    NewLiveLog = newLog.LiveLog
                };

                result.Add(group);
            }

            return result;
        }
        private IEnumerable<VirtualRateDetailsByGroup> GetLeagues(IEnumerable<LogDTO> rawData, IEnumerable<LogDTO> logDTOs)
        {
            var result = new List<VirtualRateDetailsByGroup>();

            var leagues = logDTOs.Where(d => d.BetTypeGroupId == -1 && d.BetTypeId == -1 && !d.IsNew);
            foreach (var league in leagues)
            {
                var betTypeGroups = logDTOs.Where(d => d.LeagueGroupId == league.LeagueGroupId && d.BetTypeGroupId != -1);
                var newLog = logDTOs.Where(d => d.LeagueGroupId == league.LeagueGroupId && d.BetTypeGroupId == -1 && d.BetTypeId == -1 && d.IsNew).FirstOrDefault() ?? league;

                var group = new VirtualRateDetailsByGroup(league, 2)
                {
                    GroupId = league.LeagueGroupId,
                    GroupName = league.LeagueGroupName,
                    Childs = GetBetTypeGroups(rawData, betTypeGroups),
                    NewNonliveLog = newLog.NonliveLog,
                    NewLiveLog = newLog.LiveLog
                };
                result.Add(group);
            }

            return result;
        }
        private IEnumerable<VirtualRateDetailsByGroup> GetBetTypeGroups(IEnumerable<LogDTO> rawData, IEnumerable<LogDTO> logDTOs)
        {
            var result = new List<VirtualRateDetailsByGroup>();

            var betTypeGroups = logDTOs.Where(d => d.BetTypeId == -1 && !d.IsNew);
            foreach (var betTypeGroup in betTypeGroups)
            {
                var betTypes = logDTOs.Where(d => d.BetTypeGroupId == betTypeGroup.BetTypeGroupId && d.BetTypeId != -1);
                var newLog = logDTOs.Where(d => d.BetTypeGroupId == betTypeGroup.BetTypeGroupId && d.BetTypeId == -1 && d.IsNew).FirstOrDefault() ?? betTypeGroup;

                var group = new VirtualRateDetailsByGroup(betTypeGroup, 3)
                {
                    GroupId = betTypeGroup.BetTypeGroupId,
                    GroupName = betTypeGroup.BetTypeGroupName,
                    Childs = GetBetTypes(rawData, betTypes),
                    NewNonliveLog = newLog.NonliveLog,
                    NewLiveLog = newLog.LiveLog
                };
                result.Add(group);
            }

            return result;
        }

        private IEnumerable<VirtualRateDetailsByGroup> GetBetTypes(IEnumerable<LogDTO> rawData, IEnumerable<LogDTO> logDTOs)
        {
            var result = new List<VirtualRateDetailsByGroup>();
            var betTypes = logDTOs.Where(d => !d.IsNew);

            foreach (var betType in betTypes)
            {
                var newLog = logDTOs.Where(d => d.BetTypeId == betType.BetTypeId && d.IsNew).FirstOrDefault() ?? betType;
                var group = new VirtualRateDetailsByGroup(betType, 4)
                {
                    GroupId = betType.BetTypeId,
                    GroupName = betType.BetTypeName,
                    NewNonliveLog = newLog.NonliveLog,
                    NewLiveLog = newLog.LiveLog
                };
                result.Add(group);
            }

            return result;
        }


        private LogDTO MapLogDTO(DataRow reader)
        {
            var dto = new LogDTO
            {
                SportType = Converter.ToInt(reader["SportType"]),
                LeagueGroupName = Converter.ToString2(reader["LeagueGroup"]),
                LeagueGroupId = reader["LeagueGroupID"] != null ? Converter.ToInt(reader["LeagueGroupID"]) : -1,
                BetTypeGroupName = Converter.ToString2(reader["BetTypeGroup"]),
                BetTypeGroupId = reader["BetTypeGroupId"] != null ? Converter.ToInt(reader["BetTypeGroupId"]) : -1,
                BetTypeId = reader["BetType"] != null ? Converter.ToInt(reader["BetType"]) : -1,
                Level = Converter.ToInt(reader["LevelId"]),

                LogId = Converter.ToString2(reader["LogID"]),
                Username = Converter.ToString2(reader["Username"]),
                ExchangeRate = reader["CurrencyRate"] != null ? Converter.ToDecimal(reader["CurrencyRate"]) : -1,
                ExchangeRateText = Converter.ToString2(reader["CurrencyRate"]),
                UpdatedDate = Converter.ToDateTime(reader["UpdatedTime"]),
                Editor = Converter.ToString2(reader["UpdatedBy"]),
                IsNew = Converter.ToBool(reader["IsNew"])
            };

            dto.SportName = CacheRef.GetSportName(dto.SportType);
            dto.BetTypeName = GetBetTypeName(dto.SportType, dto.BetTypeId);

            dto.NonliveLog = GetValueFollowingLog(reader, "NonLive");
            dto.LiveLog = GetValueFollowingLog(reader, "Live");

            return dto;
        }

        public VirtualRateRealValues ViewRealValues(ViewRealValuesCriteria criteria)
        {
            var result = new VirtualRateRealValues
            {
                CustId = criteria.CustId
            };
            var dataSet = DynamicRepository.FetchDataSet(criteria);
            if(dataSet.Tables.Count > 1)
            {
                var row = dataSet.Tables[0].Rows[0];
                result.Username = Converter.ToString2(row["Username"]);
                result.Currency = Converter.ToString2(row["Currency"]);
                result.ExchangeRate = Converter.ToDecimal(row["ActualRate"]);
                result.ExchangeRateText = Converter.ToString2(row["ActualRate"]);
            }
            var virtualRateDTOs = new List<VirtualRateDTO>();
            if (dataSet.Tables.Count > 2)
            {
                foreach (DataRow item in dataSet.Tables[1].Rows)
                {
                    virtualRateDTOs.Add(MapVirtualRateDTO(item));
                }
            }
            result.ResultCode = criteria.ErrCode;
            result.ResultMessage = criteria.ErrMsg;
            result.Childs = GetSportsVirtualRateDetail(virtualRateDTOs, 0, 0);

            return result;
        }

        private VirtualRateDTO MapVirtualRateDTO(DataRow reader)
        {
            var dto = new VirtualRateDTO
            {
                SportType = Converter.ToInt(reader["SportType"]),
                LeagueGroupName = Converter.ToString2(reader["LeagueGroup"]),
                LeagueGroupId = Converter.ToInt(reader["LeagueGroupID"], -1),
                BetTypeGroupName = Converter.ToString2(reader["BetTypeGroup"]),
                BetTypeGroupId = Converter.ToInt(reader["BetTypeGroupId"], -1),
                BetTypeId = Converter.ToInt(reader["BetTypeId"], -1),
                Level = Converter.ToInt(reader["LevelId"])
            };

            dto.SportName = CacheRef.GetSportName(dto.SportType);
            dto.BetTypeName = GetBetTypeName(dto.SportType, dto.BetTypeId);

            dto.NonliveLog = GetValueFollowingLog(reader, "NonLive");
            dto.LiveLog = GetValueFollowingLog(reader, "Live");

            return dto;
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

        private string BuildXML(SaveVirtualRateDetailsCriteria criteria)
        {
            var canSetAutoSuspendOnAllLevels = criteria.SportType == SportTypeConstants.Basketball;

            var xml = new StringBuilder();
            xml.Append("<Root>");

            foreach (var sport in criteria.Rates.Sports)
            {
                xml.Append(BuildXMLRow(criteria, sport, canSetAutoSuspendOnAllLevels));

                foreach (var game in sport.Childs)
                {
                    xml.Append(BuildXMLRow(criteria, game, canSetAutoSuspendOnAllLevels));

                    foreach (var betTypeGroup in game.Childs)
                    {
                        xml.Append(BuildXMLRow(criteria, betTypeGroup, canSetAutoSuspendOnAllLevels));

                        if (betTypeGroup.Childs != null)
                        {
                            foreach (var betType in betTypeGroup.Childs)
                            {
                                xml.Append(BuildXMLRow(criteria, betType, true));
                            }
                        }
                    }
                }
            }

            xml.Append("</Root>");

            return xml.ToString();
        }

        private string BuildXMLRow(SaveVirtualRateDetailsCriteria criteria, VirtualRateDetailsByGroup rate, bool isBetType)
            => !criteria.IsMemberCompactMode && criteria.IsMember && isBetType
            ? BuildXMLRowAutoSuspend(rate)
            : BuildXMLRowWithoutAutoSuspend(rate);

        private string BuildXMLRowAutoSuspend(VirtualRateDetailsByGroup rate)
        {
            var xml = new StringBuilder();
            xml.AppendFormat(
                "<Row LevelID=\"{0}\" ValueIDLive=\"{1}\" ValueIDNonLive=\"{2}\" IsOverrideASNonLive=\"{3}\" IsOverrideASLive=\"{4}\"",
                rate.LevelId,
                rate.LiveId,
                rate.NonliveId,
                BoolToInt(rate.IsOverrideASNonlive),
                BoolToInt(rate.IsOverrideASLive));
            if (!rate.DisableNonliveAutoSuspend)
            {
                xml.AppendFormat(" IsNonLiveAutoSuspend=\"{0}\"", BoolToInt(rate.NonliveAutoSuspend));
            }

            if (!rate.DisableLiveAutoSuspend)
            {
                xml.AppendFormat(" IsLiveAutoSuspend=\"{0}\"", BoolToInt(rate.LiveAutoSuspend));
            }

            xml.Append(" />");

            return xml.ToString();
        }

        private static int BoolToInt(bool value) => value ? 1 : 0;

        private static string BuildXMLRowWithoutAutoSuspend(VirtualRateDetailsByGroup rate)
            => rate.LiveId == 0 && rate.NonliveId == 0
            ? string.Empty
            : $"<Row LevelID=\"{rate.LevelId}\" ValueIDLive=\"{rate.LiveId}\" ValueIDNonLive=\"{rate.NonliveId}\" />";

        private static string GetBetTypeName(int sportType, int betTypeId)
        {
            if (betTypeId == -1)
            {
                return string.Empty;
            }

            return sportType == SportTypeConstants.Basketball
                ? CacheRef.GetVRBetTypeName(sportType, betTypeId)
                : CacheRef.GetBetType(betTypeId);
        }

        private string GetValueFollowingLog(DataRow reader, string column)
        {
            var followingRole = Converter.ToString2(reader[$"{column}_FollowingRole"]);

            var followingLevelId = Converter.ToInt(reader[$"{column}_FollowingLevelId"], -1);
            var followingBetTypeId = Converter.ToInt(reader[$"{column}_FollowingBetTypeID"], -1);
            var followingBetTypeGroup = Converter.ToString2(reader[$"{column}_FollowingBetTypeGroup"]);
            var followingLeagueGroup = Converter.ToString2(reader[$"{column}_FollowingLeagueGroup"]);
            var sportId = Converter.ToInt(reader["SportType"]);

            var level = Converter.ToInt(reader["LevelId"]);
            var followingLevel = followingLevelId == level || followingBetTypeId != -1 ? string.Empty :
                GetSportName(followingBetTypeGroup, followingLeagueGroup, sportId);
            var value = (sportId == 1 && (reader[$"{column}_FollowingValue"] == DBNull.Value)) ? "" : ((Converter.ToDecimal(reader[$"{column}_FollowingValue"], -1) - 1) * 100).ToString("#,##0");

            return value
                + (string.IsNullOrWhiteSpace(followingRole) ? string.Empty : $" ({followingRole})")
                + (string.IsNullOrWhiteSpace(followingLevel) ? string.Empty : $" ({followingLevel})");
        }

        private static string GetSportName(string followingBetTypeGroup, string followingLeagueGroup, int sportId)
        {
            return !string.IsNullOrWhiteSpace(followingBetTypeGroup) ? followingBetTypeGroup :
                            GetSportNameFromCacheRef(followingLeagueGroup, sportId);
        }

        private static string GetSportNameFromCacheRef(string followingLeagueGroup, int sportId)
        {
            return !string.IsNullOrWhiteSpace(followingLeagueGroup) ? followingLeagueGroup :
                                        CacheRef.GetSportName(sportId);
        }
    }
}
