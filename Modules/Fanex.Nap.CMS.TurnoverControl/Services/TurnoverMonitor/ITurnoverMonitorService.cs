using System;
using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;

namespace Fanex.Nap.CMS.TurnoverControl.Services.TurnoverMonitor
{
    public interface ITurnoverMonitorService
    {
        IEnumerable<IdNameModel> GetSports(byte mode, short sportType);

        IEnumerable<IdNameModel> GetLeagues(string fromDate, string toDate, short sportType, DateTime moveBetsDay);

        IEnumerable<IdNameModel> GetMatches(string fromDate, string toDate, int leagueId);

        IEnumerable<LeagueTurnover> GetCustomerTurnovers(
            string fromDate,
            string toDate,
            short sportType,
            int leagueId,
            int matchId,
            string username,
            bool isExcludeCompletedEvent,
            int percentageUsed,
            DateTime moveBetsDay);

        IEnumerable<HouseTurnover> GetHouseTurnovers(int custId, int matchId, bool isGroupHeadline, string eventDate);

        Result UpdateMaxTurnover(UpdateMaxTurnoverParam param, int userId, string ip);

        Result UpdateMultipleMaxTurnover(IEnumerable<UpdateMaxTurnoverParam> param, int userId, string ip);

        Result Lock(LockSettingParam param, int userId, string ip);
    }
}
