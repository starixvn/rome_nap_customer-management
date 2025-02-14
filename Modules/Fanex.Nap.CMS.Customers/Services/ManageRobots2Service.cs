namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Customers.Criteria.ManageRobots2;
    using Fanex.Nap.CMS.Customers.Models;
    using Fanex.Nap.CMS.Customers.Models.ManageRobots2;
    using Fanex.Nap.CMS.Customers.Repositories;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public class ManageRobots2Service : IManageRobots2Service
    {
        private readonly IManageRobots2Repository repository;

        public ManageRobots2Service() : this(null)
        {
        }

        public ManageRobots2Service(IManageRobots2Repository repository)
        {
            this.repository = repository ?? new ManageRobots2Repository();
        }

        public RobotSitesModel GetSites()
            => repository.GetSites(new GetSitesCriteria());

        public RobotReport GetRobots(GetRobotsCriteria criteria)
        {
            criteria.ToDate = criteria.ToDate?.AddDays(1).AddTicks(-1);
            criteria.Level = criteria.Level == 0 ? null : criteria.Level;
            criteria.Currency = criteria.Currency == 0 ? null : criteria.Currency;

            return repository.GetRobots(criteria);
        }

        public Result DeleteRobot(DeleteRobotCriteria criteria)
            => repository.DeleteRobot(criteria);

        public Result AddRobot(AddRobotCriteria criteria)
        {
            criteria.Usernames = string.Join(",", criteria.Usernames.Trim().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()));

            return repository.AddRobot(criteria);
        }

        public IEnumerable<CurrencyModel> GetCurrencies(GetCurrenciesCriteria criteria)
            => repository.GetCurrencies(criteria);

        public RobotReport GetExportRobots(GetRobotsCriteria criteria)
        {
            criteria.PageSize = -1;

            return GetRobots(criteria);
        }
    }
}