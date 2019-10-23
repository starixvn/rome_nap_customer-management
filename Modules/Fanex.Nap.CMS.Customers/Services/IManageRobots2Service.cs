namespace Fanex.Nap.CMS.Customers.Services
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Customers.Criteria.ManageRobots2;
    using Fanex.Nap.CMS.Customers.Models;
    using Fanex.Nap.CMS.Customers.Models.ManageRobots2;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IManageRobots2Service
    {
        RobotSitesModel GetSites();
        RobotReport GetRobots(GetRobotsCriteria criteria);
        Result DeleteRobot(DeleteRobotCriteria criteria);
        Result AddRobot(AddRobotCriteria criteria);
        IEnumerable<CurrencyModel> GetCurrencies(GetCurrenciesCriteria criteria);
    }
}
