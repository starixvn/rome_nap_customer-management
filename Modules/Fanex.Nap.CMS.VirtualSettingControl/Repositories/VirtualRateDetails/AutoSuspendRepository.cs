namespace Fanex.Nap.CMS.VirtualSettingControl.Repositories.VirtualRateDetails
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Web;
    using Fanex.Nap.CMS.VirtualSettingControl.Criteria.VirtualRateDetails;
    using Fanex.Nap.CMS.VirtualSettingControl.Extensions;
    using Fanex.Nap.CMS.VirtualSettingControl.Models.VirtualRateDetails;

    public class AutoSuspendRepository : IAutoSuspendRepository
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

            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();

                while (reader.Read())
                {
                    var l = MapLogDTO(reader);
                    l.Username = criteria.Username;
                    logDTOs.Add(l);
                }

                reader.Close();
                var output = objDb.GetOuputParams();
                result.ResultCode = Converter.ToInt(output["errcode"]);
                result.ResultMessage = Converter.ToString2(output["errmsg"]);
            }

            result.Logs = GroupLogs(logDTOs);

            return result;
        }

        private LogDTO MapLogDTO(IDataReader reader)
        {
            var dto = new LogDTO
            {
                SportType = Converter.ToInt(reader["SportType"]),
                LeagueGroupName = Converter.ToString2(reader["LeagueGroup"]),
                BetTypeGroupName = Converter.ToString2(reader["BetTypeGroup"]),
                BetTypeId = reader.ToInt("BetType", -1),
                Level = Converter.ToInt(reader["LevelId"]),
                LogId = Converter.ToString2(reader["LogID"]),
                UpdatedDate = Converter.ToDateTime(reader["LogTime"]),
                Editor = Converter.ToString2(reader["UpdatedBy"]),
                NewNonliveAutoSuspend = Converter.ToBool(reader["IsNonLiveAutoSuspendNew"]),
                NewLiveAutoSuspend = Converter.ToBool(reader["IsLiveAutoSuspendNew"]),
                NonliveAutoSuspend = Converter.ToBool(reader["IsNonLiveAutoSuspendOld"]),
                LiveAutoSuspend = Converter.ToBool(reader["IsLiveAutoSuspendOld"])
            };

            dto.SportName = CacheRef.GetSportName(dto.SportType);
            dto.BetTypeName = CacheRef.GetBetType(dto.BetTypeId);
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
    }
}
