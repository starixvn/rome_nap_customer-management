using System.Collections.Generic;
using Fanex.Nap.CMS.Customers.Models.LimitNewDownline;

namespace Fanex.Nap.CMS.Customers.Services.LimitNewDownline
{
    public interface ILimitNewDownlineService
    {
        IEnumerable<string> GetSites();

        LimitDownlineReport GetLimitDownlineReport(string username, byte level, string site, int pageIndex, int pageSize);

        AddNewOrRemoveLimitResult AddNewLimit(string usernames, int userId, string ip);

        AddNewOrRemoveLimitResult RemoveLimit(string usernames, int userId, string ip);
    }
}
