namespace Fanex.Nap.CMS.Customers.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fanex.Nap.CMS.Customers.Criteria.ManageRobots2;
    using Fanex.Nap.CMS.Customers.Models;
    using Fanex.Nap.CMS.Customers.Models.ManageRobots2;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;

    public class ManageRobots2Service : ServiceBase, IManageRobots2Service
    {
        public RobotSitesModel GetSites()
        {
            var sites = DynamicRepository.Fetch<SiteModel>(new GetSitesCriteria());

            return new RobotSitesModel
            {
                Credits = sites.Where(s => !s.IsDeposit).ToList(),
                Licensees = sites.Where(s => s.IsDeposit).ToList()
            };
        }

        public RobotReport GetRobots(GetRobotsCriteria criteria)
        {
            criteria.ToDate = criteria.ToDate?.AddDays(1).AddTicks(-1);
            criteria.Level = criteria.Level == 0 ? null : criteria.Level;
            criteria.Currency = criteria.Currency == 0 ? null : criteria.Currency;

            var robots = DynamicRepository.Fetch<RobotModel>(criteria).ToList();

            return new RobotReport
            {
                Robots = robots,
                PageSize = criteria.PageSize,
                PageIndex = criteria.PageIndex,
                FromDate = criteria.FromDate,
                ToDate = criteria.ToDate,
                TotalItems = criteria.TotalItems
            };
        }

        public Result DeleteRobot(DeleteRobotCriteria criteria)
            => DoAction(criteria);

        public Result AddRobot(AddRobotCriteria criteria)
        {
            criteria.Usernames = string.Join(",", criteria.Usernames.Trim().Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(s => s.Trim()));

            return DoAction(criteria);
        }

        public IEnumerable<CurrencyModel> GetCurrencies(GetCurrenciesCriteria criteria)
            => DynamicRepository.Fetch<CurrencyModel>(criteria);

        public RobotReport GetExportRobots(GetRobotsCriteria criteria)
        {
            criteria.PageSize = -1;

            return GetRobots(criteria);
        }
    }
}