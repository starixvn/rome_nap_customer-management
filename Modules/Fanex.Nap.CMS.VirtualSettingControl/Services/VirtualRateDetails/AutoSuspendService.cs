using System.Collections.Generic;
using System.Data;
using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
using Fanex.Nap.CMS.VirtualSettingControl.Constants;
using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;
using Fanex.Common;
using Fanex.Nap.CMS.Infrastructure.Web;
using System.Linq;

namespace Fanex.Nap.CMS.VirtualSettingControl.Services.VirtualRateDetails
{
    public class AutoSuspendService : ServiceBase, IAutoSuspendService
    {
        public AutoSuspendLogReport GetAutoSuspendLogs(GetAutoSuspendLogsCriteria criteria)
        {
            var result = new AutoSuspendLogReport
            {
                Username = criteria.Username,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate
            };

            var logDTOs = new List<LogDTO>();
            var dataSet = DynamicRepository.FetchDataSet(criteria);
            if(dataSet.Tables.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var l = MapLogDTO(row);
                    l.Username = criteria.Username;
                    logDTOs.Add(l);
                }
            }

            result.ResultCode = criteria.ErrCode;
            result.ResultMessage = criteria.ErrMsg;
            result.Logs = GroupLogs(logDTOs);

            return result;
        }

        private LogDTO MapLogDTO(DataRow reader)
        {
            var dto = new LogDTO
            {
                SportType = Converter.ToInt(reader["SportType"]),
                LeagueGroupName = Converter.ToString2(reader["LeagueGroup"]),
                BetTypeGroupName = Converter.ToString2(reader["BetTypeGroup"]),
                BetTypeId = reader["BetType"] == null ? -1 : Converter.ToInt(reader["BetType"]),
                Level = Converter.ToInt(reader["LevelId"]),
                LogId = Converter.ToString2(reader["LogID"]),
                UpdatedDate = Converter.ToDateTime(reader["LogTime"]),
                Editor = Converter.ToString2(reader["UpdatedBy"]),
                NewNonliveAutoSuspend = Converter.ToBool(reader["IsNonLiveAutoSuspendNew"]),
                NewLiveAutoSuspend = Converter.ToBool(reader["IsLiveAutoSuspendNew"]),
                NonliveAutoSuspend = Converter.ToBool(reader["IsNonLiveAutoSuspendOld"]),
                LiveAutoSuspend = Converter.ToBool(reader["IsLiveAutoSuspendOld"]),
                IsOverrideASLive = Converter.ToBool(reader["IsOverrideASLiveOld"]),
                IsOverrideASLiveNew = Converter.ToBool(reader["IsOverrideASLiveNew"]),
                IsOverrideASNonlive = Converter.ToBool(reader["IsOverrideASNonLiveOld"]),
                IsOverrideASNonliveNew = Converter.ToBool(reader["IsOverrideASNonLiveNew"])
            };

            dto.SportName = CacheRef.GetSportName(dto.SportType);
            dto.BetTypeName = GetBetTypeName(dto.SportType, dto.BetTypeId);

            return dto;
        }

        private IEnumerable<AutoSuspendLog> GroupLogs(List<LogDTO> logDTOs)
        {
            var result = new List<AutoSuspendLog>();

            if (logDTOs.Any())
            {
                var groups = logDTOs.GroupBy(l => l.LogId);
                foreach (var group in groups)
                {
                    var log = GetLog(group.ToList());
                    result.Add(log);
                }
            }

            return result;
        }

        private AutoSuspendLog GetLog(List<LogDTO> logDTOs)
        {
            var firstLog = logDTOs[0];

            var result = new AutoSuspendLog
            {
                LogId = firstLog.LogId,
                Username = firstLog.Username,
                UpdatedDate = firstLog.UpdatedDate,
                Editor = firstLog.Editor,
                Sports = logDTOs
            };

            return result;
        }

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
    }
}
