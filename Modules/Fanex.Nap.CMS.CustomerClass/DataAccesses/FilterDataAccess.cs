namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Nap.CMS.CustomerClass.Criteria;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.Data.Repository;

    internal interface IFilterDataAccess
    {
        IEnumerable<short> GetCustomerClasses(CriteriaBase criteria);

        IEnumerable<LeagueGroup> GetLeagueGroups(CriteriaBase criteria);

        IEnumerable<IdNameModel> GetSports(CriteriaBase criteria);
    }

    internal class FilterDataAccess : BaseRepository, IFilterDataAccess
    {
        public IEnumerable<short> GetCustomerClasses(CriteriaBase criteria)
            => ExecuteReader(criteria, ToCustomerClass);

        private static short ToCustomerClass(IDataReader reader)
            => Convert.ToInt16(reader["ResourceId"]);

        public IEnumerable<LeagueGroup> GetLeagueGroups(CriteriaBase criteria)
            => ExecuteReader(criteria, ToLeagueGroup);

        private static LeagueGroup ToLeagueGroup(IDataReader reader)
            => new LeagueGroup(
                Convert.ToInt16(reader["SportType"]),
                Convert.ToInt32(reader["GroupId"]),
                reader["GroupName"].ToString());

        public IEnumerable<IdNameModel> GetSports(CriteriaBase criteria)
            => ExecuteReader(criteria, ToSportType);

        private static IdNameModel ToSportType(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt16(reader["SportType"]),
                Name = Convert.ToString(reader["SportName"])
            };
    }
}
