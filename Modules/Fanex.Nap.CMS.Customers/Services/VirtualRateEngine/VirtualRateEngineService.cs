using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Services.VirtualRateEngine
{
    public class VirtualRateEngineService : IVirtualRateEngineService
    {
        public IEnumerable<IdNameModel> GetCustomerClasses()
        {
            yield return new IdNameModel { Id = 1, Name = "Group Betting" };
            yield return new IdNameModel { Id = 2, Name = "Hedging" };
            yield return new IdNameModel { Id = 3, Name = "Fixed" };
        }
    }
}
