namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails
{
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Extensions;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;
    using Fanex.NAP.CMS.Core.Resources;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    public class VirtualRateDetailsRepository : IVirtualRateDetailsRepository
    {
        private const string Username = "Username";
        private const string SportType = "SportType";
        private const string LevelId = "LevelId";
        private const string ErrorCode = "errcode";
        private const string ErrorMsg = "errmsg";

        public VirtualRateDetailsReport GetVirtualRateDetails(GetVirtualRateDetailsCriteria criteria)
        {
            var ranges = GetVirtualRateRanges();
            var minRange = ranges.Min(r => r.Value);
            var maxRange = ranges.Max(r => r.Value);

            var result = new VirtualRateDetailsReport
            {
                VirtualRateValueRange = ranges
            };

            var virtualRateDTOs = new List<VirtualRateDTO>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();

                if (reader.Read())
                {
                    result.Currency = Converter.ToString2(reader["Currency"]);
                    result.ActualRate = Converter.ToDecimal(reader["ActualRate"]);
                    result.UplineCustId = Converter.ToInt(reader["UplineCustID"]);
                    result.CustId = Converter.ToInt(reader["CustId"]);
                    result.Username = Converter.ToString2(reader[Username]);
                    result.UplineCustName = Converter.ToString2(reader["UplineUsername"]);
                    result.ReportLevel = string.IsNullOrWhiteSpace(result.Username) ? 0 : Converter.ToInt(reader["RoleId"]);
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var dto = new VirtualRateDTO
                        {
                            SportType = Converter.ToInt(reader[SportType]),
                            LeagueGroupName = Converter.ToString2(reader["LeagueGroup"]),
                            LeagueGroupId = Converter.ToInt(reader["LeagueGroupID"], -1),
                            BetTypeGroupName = Converter.ToString2(reader["BetTypeGroup"]),
                            BetTypeGroupId = Converter.ToInt(reader["BetTypeGroupId"], -1),
                            BetTypeId = Converter.ToInt(reader["BetTypeId"], -1),
                            Level = Converter.ToInt(reader[LevelId]),
                            NonliveId = Converter.ToInt(reader["ValueIdNonLive"]),
                            NonliveValue = Converter.ToDecimal(reader["ValueNonLive"], -1),
                            LiveId = Converter.ToInt(reader["ValueIdLive"]),
                            LiveValue = Converter.ToDecimal(reader["ValueLive"], -1),
                            NonliveAutoSuspendThreshold = Converter.ToDecimal(reader["VRAutoSuspendThreshold"], -1),
                            LiveAutoSuspendThreshold = Converter.ToDecimal(reader["VRAutoSuspendThreshold"], -1),
                            DisableNonliveAutoSuspend = Converter.ToInt(reader["IsDisableNonLiveAutoSuspend"]) == 1,
                            DisableLiveAutoSuspend = Converter.ToInt(reader["IsDisableLiveAutoSuspend"]) == 1,
                            NonliveAutoSuspend = Converter.ToBool(reader["IsNonLiveAutoSuspend"], true),
                            LiveAutoSuspend = Converter.ToBool(reader["IsLiveAutoSuspend"], true)
                        };

                        dto.SportName = CacheRef.GetSportName(dto.SportType);
                        dto.BetTypeName = CacheRef.GetBetType(dto.BetTypeId);
                        virtualRateDTOs.Add(dto);
                    }
                }

                reader.Close();
                var output = objDb.GetOuputParams();
                result.ResultCode = Converter.ToInt(output[ErrorCode]);
                result.ResultMessage = Converter.ToString2(output[ErrorMsg]);
            }

            result.Sports = GetSports(virtualRateDTOs, minRange, maxRange);
            return result;
        }

        public Result SaveVirtualRateDetails(SaveVirtualRateDetailsCriteria criteria)
        {
            var result = new Result();

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                objDb.ExecuteNonQuery();
                var output = objDb.GetOuputParams();
                result.Code = Converter.ToInt(output[ErrorCode]);
                result.Message = result.Code == 0 ? ViewLang.UpdateSuccessfull : Converter.ToString2(output[ErrorMsg]);
            }

            return result;
        }

        public ValidateAssociatedAccountsResult ValidateAssociatedAccounts(ValidateAssociatedAccountsCriteria criteria)
        {
            var result = new ValidateAssociatedAccountsResult();
            var accounts = criteria.Accounts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim());
            criteria.Accounts = string.Join(",", accounts);

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());

                var reader = objDb.ExecuteReader();

                while (reader.Read())
                {
                    var isValid = Converter.ToBool(reader["IsValid"]);
                    var username = Converter.ToString2(reader[Username]);
                    if (!isValid)
                    {
                        result.InvalidAccounts.Add(username);
                        continue;
                    }

                    result.ValidAccounts.Add(username);
                    result.ValidAccountIds.Add(Converter.ToString2(reader["CustId"]));
                }

                reader.Close();
            }

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
                    IsMember = criteria.IsMember
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

            var logDTOs = new List<LogDTO>();

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();

                if (reader.Read())
                {
                    result.Currency = Converter.ToString2(reader["Currency"]);
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        logDTOs.Add(MapLogDTO(reader));
                    }
                }

                reader.Close();
                var output = objDb.GetOuputParams();
                result.ResultCode = Converter.ToInt(output[ErrorCode]);
                result.ResultMessage = Converter.ToString2(output[ErrorMsg]);
            }

            result.Logs = GroupLogs(logDTOs);

            return result;
        }

        public VirtualRateRealValues ViewRealValues(ViewRealValuesCriteria criteria)
        {
            var result = new VirtualRateRealValues
            {
                CustId = criteria.CustId
            };

            var virtualRateDTOs = new List<VirtualRateDTO>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();

                if (reader.Read())
                {
                    result.Username = Converter.ToString2(reader[Username]);
                    result.Currency = Converter.ToString2(reader["Currency"]);
                    result.ExchangeRate = Converter.ToDecimal(reader["ActualRate"]);
                    result.ExchangeRateText = Converter.ToString2(reader["ActualRate"]);
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        virtualRateDTOs.Add(MapVirtualRateDTO(reader));
                    }
                }

                reader.Close();
                var output = objDb.GetOuputParams();
                result.ResultCode = Converter.ToInt(output[ErrorCode]);
                result.ResultMessage = Converter.ToString2(output[ErrorMsg]);
            }

            result.Childs = GetSports(virtualRateDTOs, 0, 0);
            return result;
        }

        private IEnumerable<RateInfo> GetVirtualRateRanges()
        {
            var criteria = new GetVirtualRateRangesCriteria();
            var result = new List<RateInfo>();

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();

                while (reader.Read())
                {
                    var id = Converter.ToInt(reader["ValueID"]);

                    if (id == 0)
                    {
                        continue;
                    }

                    var value = (int)(Converter.ToDecimal(reader["Value"]) * 100 - 100);
                    result.Add(new RateInfo
                    {
                        Id = id,
                        Value = value
                    });
                }
            }

            return result;
        }

        private IEnumerable<VirtualRateDetailsByGroup> GetSports(
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
                    Childs = GetLeagues(virtualRateDTOs, leagues, minRange, maxRange),
                    HasRepetition = false
                };
                result.Add(group);
            }

            return result;
        }

        private IEnumerable<VirtualRateDetailsByGroup> GetLeagues(
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
                    Childs = GetBetTypeGroups(rawData, betTypeGroups, minRange, maxRange),
                    HasRepetition = rawData.Count(d => d.LeagueGroupId == league.LeagueGroupId && d.BetTypeGroupId == -1) > 1
                };

                result.Add(group);
            }

            return result;
        }

        private IEnumerable<VirtualRateDetailsByGroup> GetBetTypeGroups(
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
                    Childs = GetBetTypes(rawData, betTypes, minRange, maxRange),
                    HasRepetition = rawData.Count(d => d.BetTypeGroupId == betTypeGroup.BetTypeGroupId && d.BetTypeId == -1) > 1
                };

                result.Add(group);
            }

            return result;
        }

        private IEnumerable<VirtualRateDetailsByGroup> GetBetTypes(
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

        private VirtualRateDTO MapVirtualRateDTO(IDataReader reader)
        {
            var dto = new VirtualRateDTO
            {
                SportType = Converter.ToInt(reader[SportType]),
                LeagueGroupName = Converter.ToString2(reader["LeagueGroup"]),
                LeagueGroupId = Converter.ToInt(reader["LeagueGroupID"], -1),
                BetTypeGroupName = Converter.ToString2(reader["BetTypeGroup"]),
                BetTypeGroupId = Converter.ToInt(reader["BetTypeGroupId"], -1),
                BetTypeId = Converter.ToInt(reader["BetTypeId"], -1),
                Level = Converter.ToInt(reader[LevelId])
            };

            dto.SportName = CacheRef.GetSportName(dto.SportType);
            dto.BetTypeName = CacheRef.GetBetType(dto.BetTypeId);

            dto.NonliveLog = GetValueFollowingLog(reader, "NonLive");
            dto.LiveLog = GetValueFollowingLog(reader, "Live");

            return dto;
        }

        private LogDTO MapLogDTO(IDataReader reader)
        {
            var dto = new LogDTO
            {
                SportType = Converter.ToInt(reader[SportType]),
                LeagueGroupName = Converter.ToString2(reader["LeagueGroup"]),
                LeagueGroupId = reader.ToInt("LeagueGroupID", -1),
                BetTypeGroupName = Converter.ToString2(reader["BetTypeGroup"]),
                BetTypeGroupId = reader.ToInt("BetTypeGroupId", -1),
                BetTypeId = reader.ToInt("BetType", -1),
                Level = Converter.ToInt(reader[LevelId]),

                LogId = Converter.ToString2(reader["LogID"]),
                Username = Converter.ToString2(reader[Username]),
                ExchangeRate = reader.ToDecimal("CurrencyRate", -1),
                ExchangeRateText = Converter.ToString2(reader["CurrencyRate"]),
                UpdatedDate = Converter.ToDateTime(reader["UpdatedTime"]),
                Editor = Converter.ToString2(reader["UpdatedBy"]),
                IsNew = Converter.ToString2(reader["IsNew"]) == "1"
            };

            dto.SportName = CacheRef.GetSportName(dto.SportType);
            dto.BetTypeName = CacheRef.GetBetType(dto.BetTypeId);

            dto.NonliveLog = GetValueFollowingLog(reader, "NonLive");
            dto.LiveLog = GetValueFollowingLog(reader, "Live");
            return dto;
        }

        private string GetValueFollowingLog(IDataReader reader, string column)
        {
            var followingRole = Converter.ToString2(reader[$"{column}_FollowingRole"]);

            var followingLevelId = Converter.ToInt(reader[$"{column}_FollowingLevelId"], -1);
            var followingBetTypeId = Converter.ToInt(reader[$"{column}_FollowingBetTypeID"], -1);
            var followingBetTypeGroup = Converter.ToString2(reader[$"{column}_FollowingBetTypeGroup"]);
            var followingLeagueGroup = Converter.ToString2(reader[$"{column}_FollowingLeagueGroup"]);
            var sportId = Converter.ToInt(reader[SportType]);

            var level = Converter.ToInt(reader[LevelId]);
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
    }
}
