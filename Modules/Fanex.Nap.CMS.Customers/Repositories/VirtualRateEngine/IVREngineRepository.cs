using System.Collections.Generic;
using Fanex.Data.Repository;
using Fanex.Nap.CMS.Customers.Models.VirtualRateEngine;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Repositories.VirtualRateEngine
{
    public interface IVREngineRepository
    {
        VREngineMatrix GetVREngineMatrix(ICriteria criteria);

        IEnumerable<IdNameModel> GetLeagues(ICriteria criteria);

        IEnumerable<VREngineRule> GetRules(ICriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);
    }
}
