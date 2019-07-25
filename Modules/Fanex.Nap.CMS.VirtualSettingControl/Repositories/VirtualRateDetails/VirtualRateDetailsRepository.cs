namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;
    using Fanex.NAP.CMS.Core.Resources;

    public class VirtualRateDetailsRepository : IVirtualRateDetailsRepository
    {
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
                    result.Username = Converter.ToString2(reader["Username"]);
                    result.UplineCustName = Converter.ToString2(reader["UplineUsername"]);
                    result.ReportLevel = string.IsNullOrWhiteSpace(result.Username) ? 0 : 1;
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        var dto = new VirtualRateDTO
                        {
                            SportType = Converter.ToInt(reader["SportType"]),
                            LeagueGroupName = Converter.ToString2(reader["LeagueGroup"]),
                            LeagueGroupId = Converter.ToInt(reader["LeagueGroupID"], -1),
                            BetTypeGroupName = Converter.ToString2(reader["BetTypeGroup"]),
                            BetTypeGroupId = Converter.ToInt(reader["BetTypeGroupId"], -1),
                            BetTypeId = Converter.ToInt(reader["BetTypeId"], -1),
                            Level = Converter.ToInt(reader["LevelId"]),
                            NonliveId = Converter.ToInt(reader["ValueIdNonLive"]),
                            NonliveValue = Converter.ToDecimal(reader["ValueNonLive"], -1),
                            LiveId = Converter.ToInt(reader["ValueIdLive"]),
                            LiveValue = Converter.ToDecimal(reader["ValueLive"], -1)
                        };

                        dto.SportName = CacheRef.GetSportName(dto.SportType);
                        dto.BetTypeName = CacheRef.GetBetType(dto.BetTypeId);
                        virtualRateDTOs.Add(dto);
                    }
                }

                reader.Close();
                var output = objDb.GetOuputParams();
                result.ResultCode = Converter.ToInt(output["errcode"]);
                result.ResultMessage = Converter.ToString2(output["errmsg"]);
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
                result.Code = Converter.ToInt(output["errcode"]);
                result.Message = result.Code == 0 ? ViewLang.UpdateSuccessfull : Converter.ToString2(output["errmsg"]);
            }

            return result;
        }

        public ValidateAssociatedAccountsResult ValidateAssociatedAccounts(ValidateAssociatedAccountsCriteria criteria)
        {
            var result = new ValidateAssociatedAccountsResult();

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());

                var reader = objDb.ExecuteReader();

                while (reader.Read())
                {
                    var isValid = Converter.ToBool(reader["IsValid"]);
                    var username = Converter.ToString2(reader["Username"]);
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

            var hasFailed = false;
            var errorMessages = new List<string>();
            foreach (var custId in criteria.Accounts.Split(new[] { ',' }))
            {
                var updateResult = SaveVirtualRateDetails(new SaveVirtualRateDetailsCriteria
                {
                    CustId = Converter.ToInt(custId),
                    SportType = criteria.SportType,
                    UserId = criteria.UserId,
                    IP = criteria.IP,
                    Rates = criteria.Rates
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
                IncludeUpline = criteria.IncludeUpline,
                ResultCode = 0,
                ResultMessage = "asdas",
                Currency = "HK"
            };

            var logs = VirtualRateDetailsData.GetLogs(criteria);
            result.Logs = logs.Select(l => ParseLog(l));

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

                var group = new VirtualRateDetailsByGroup
                {
                    GroupId = sport.SportType,
                    GroupName = sport.SportName,
                    GroupLevel = 1,
                    LevelId = sport.Level,
                    Childs = GetLeagues(virtualRateDTOs, leagues, minRange, maxRange),
                    HasRepetition = false
                };

                group.AssignData(sport, minRange, maxRange);
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

                var group = new VirtualRateDetailsByGroup
                {
                    GroupId = league.LeagueGroupId,
                    GroupName = league.LeagueGroupName,
                    GroupLevel = 2,
                    LevelId = league.Level,
                    Childs = GetBetTypeGroups(rawData, betTypeGroups, minRange, maxRange),
                    HasRepetition = rawData.Count(d => d.LeagueGroupId == league.LeagueGroupId && d.BetTypeGroupId == -1) > 1
                };

                group.AssignData(league, minRange, maxRange);
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

                var group = new VirtualRateDetailsByGroup
                {
                    GroupId = betTypeGroup.BetTypeGroupId,
                    GroupName = betTypeGroup.BetTypeGroupName,
                    GroupLevel = 3,
                    LevelId = betTypeGroup.Level,
                    Childs = GetBetTypes(rawData, betTypes, minRange, maxRange),
                    HasRepetition = rawData.Count(d => d.BetTypeGroupId == betTypeGroup.BetTypeGroupId && d.BetTypeId == -1) > 1
                };

                group.AssignData(betTypeGroup, minRange, maxRange);
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
                var group = new VirtualRateDetailsByGroup
                {
                    GroupId = betType.BetTypeId,
                    GroupName = betType.BetTypeName,
                    GroupLevel = 4,
                    LevelId = betType.Level,
                    HasRepetition = rawData.Count(d => d.BetTypeId == betType.BetTypeId) > 1
                };

                group.AssignData(betType, minRange, maxRange);
                result.Add(group);
            }

            return result;
        }

        private VirtualRateLog ParseLog(LogDTO log)
        {
            var newXml = new XmlDocument();
            newXml.LoadXml(log.NewValue);
            var newRoot = newXml.ChildNodes[0];

            var oldXml = new XmlDocument();
            oldXml.LoadXml(log.OldValue);
            var oldRoot = oldXml.ChildNodes[0];

            var result = new VirtualRateLog
            {
                Username = log.Username,
                Editor = log.Editor,
                UpdatedDate = log.UpdatedDate,
                ActualRate = Converter.ToDecimal(newRoot.Attributes["ExchangeRate"].Value)
            };

            var newVirtualRateDTOs = newRoot.ChildNodes.Cast<XmlNode>().Select(n => MapVirtualRateDTO(n));
            var oldVirtualRateDTOs = oldRoot.ChildNodes.Cast<XmlNode>().Select(n => MapVirtualRateDTO(n));

            result.NewSports = GetSports(newVirtualRateDTOs, 0, 0);
            result.OldSports = GetSports(oldVirtualRateDTOs, 0, 0);

            return result;
        }

        private VirtualRateDTO MapVirtualRateDTO(XmlNode node)
        {
            var reader = node.Attributes;

            var nonliveLog = reader["IsFollow"].Value == "true"
                ? $"{reader["NonLive"].Value} ({reader["Layer"].Value})"
                : reader["NonLive"].Value;

            var liveLog = reader["IsFollow"].Value == "true"
                ? $"{reader["Live"].Value} ({reader["Layer"].Value})"
                : reader["Live"].Value;

            var dto = new VirtualRateDTO
            {
                SportType = Converter.ToInt(reader["S"].Value),
                LeagueGroupName = Converter.ToString2(reader["LN"].Value),
                LeagueGroupId = Converter.ToInt(reader["L"].Value, -1),
                BetTypeGroupName = Converter.ToString2(reader["BGN"].Value),
                BetTypeGroupId = Converter.ToInt(reader["BG"].Value, -1),
                BetTypeId = Converter.ToInt(reader["BT"].Value, -1),
                Level = Converter.ToInt(reader["LevelID"].Value),
                NonliveLog = nonliveLog,
                LiveLog = liveLog
            };

            dto.SportName = CacheRef.GetSportName(dto.SportType);
            dto.BetTypeName = CacheRef.GetBetType(dto.BetTypeId);

            return dto;
        }
    }
}
