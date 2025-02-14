namespace Fanex.Nap.CMS.TurnoverControl.Repositories.TurnoverMonitor
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Infrastructure.Criteria;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.TurnoverControl.Models.TurnoverMonitor;

    public interface ITurnoverMonitorRepository
    {
        IEnumerable<IdNameModel> GetSports(CriteriaBase criteria);

        IEnumerable<IdNameModel> GetLeagues(CriteriaBase criteria);

        IEnumerable<IdNameModel> GetMatches(CriteriaBase criteria);

        IEnumerable<TurnoverMonitorItem> GetCustomerTurnovers(CriteriaBase criteria);

        IEnumerable<HouseTurnover> GetHouseTurnovers(CriteriaBase criteria);

        Result ExecuteNonQuery(CommandBaseCriteria criteria);
    }
}
