namespace Fanex.Nap.CMS.CustomerClass.Services
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.DataAccesses;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.Infrastructure.Services.Implementations;
    using Fanex.Nap.CMS.Infrastructure.Services.Interfaces;

    internal interface IFilterService : ISettingService
    {
        IEnumerable<short> GetCustomerClasses();

        IEnumerable<LeagueGroup> GetLeagueGroups();
    }

    internal class FilterService : SettingService, IFilterService
    {
        private readonly IFilterDataAccess dataAccess;

        public FilterService(IFilterDataAccess dataAccess = null)
            => this.dataAccess = dataAccess ?? new FilterDataAccess();

        public IEnumerable<short> GetCustomerClasses()
            => dataAccess.GetCustomerClasses(new GetCustomerClassCriteria());

        public IEnumerable<LeagueGroup> GetLeagueGroups()
            => dataAccess.GetLeagueGroups(new GetLeagueGroupCriteria());
    }
}
