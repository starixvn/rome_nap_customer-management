namespace Fanex.Nap.CMS.Customers.Repositories.VirtualRateEngine
{
    using System.Collections.Generic;
    using Fanex.Data.Repository;
    using Fanex.Nap.CMS.Customers.Models.VirtualRateEngine;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IVREngineRepository
    {
        VREngineMatrix GetVREngineMatrix(ICriteria criteria);

        IEnumerable<IdNameModel> GetLeagues(ICriteria criteria);

        IEnumerable<VREngineRule> GetRules(ICriteria criteria);

        VREngineRule GetRule(ICriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);

        string GetSportBettype(ICriteria criteria);

        Result CheckVREngineRule(ICriteria criteria);
    }
}
