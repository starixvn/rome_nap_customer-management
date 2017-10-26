using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;
using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;
using Fanex.NAP.Framework.Data;

namespace Fanex.Nap.CMS.TurnoverControl.Repositories.TurnoverMonitor
{
    public interface ITurnoverMonitorRepository
    {
        IEnumerable<IdNameModel> GetSports(ICriteria criteria);

        IEnumerable<IdNameModel> GetLeagues(ICriteria criteria);

        IEnumerable<IdNameModel> GetMatches(ICriteria criteria);

        IEnumerable<CustomerTurnover> GetCustomerTurnovers(ICriteria criteria);

        IEnumerable<HouseTurnover> GetHouseTurnovers(ICriteria criteria);

        Result UpdateMaxTurnover(ICriteria criteria);

        Result UpdateMultipleMaxTurnover(ICriteria criteria);
    }
}
