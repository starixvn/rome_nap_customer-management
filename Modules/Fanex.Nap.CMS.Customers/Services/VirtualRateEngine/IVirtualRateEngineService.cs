using System.Collections.Generic;
using Fanex.Nap.CMS.Infrastructure.Models;

namespace Fanex.Nap.CMS.Customers.Services.VirtualRateEngine
{
    public interface IVirtualRateEngineService
    {
        IEnumerable<IdNameModel> GetCustomerClasses();
    }
}
