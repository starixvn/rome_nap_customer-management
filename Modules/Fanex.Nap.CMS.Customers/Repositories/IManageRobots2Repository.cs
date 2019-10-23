namespace Fanex.Nap.CMS.Customers.Repositories
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Customers.Criteria.ManageRobots2;
    using Fanex.Nap.CMS.Customers.Models;
    using Fanex.Nap.CMS.Customers.Models.ManageRobots2;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface IManageRobots2Repository
    {
        RobotSitesModel GetSites(GetSitesCriteria criteria);

        RobotReport GetRobots(GetRobotsCriteria criteria);

        Result DeleteRobot(DeleteRobotCriteria criteria);

        Result AddRobot(AddRobotCriteria criteria);

        IEnumerable<CurrencyModel> GetCurrencies(GetCurrenciesCriteria criteria);
    }
}
