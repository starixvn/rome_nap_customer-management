namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.NAP.Framework.Data;

    internal interface IFilterDataAccess
    {
        IEnumerable<short> GetCustomerClasses(ICriteria criteria);

        IEnumerable<LeagueGroup> GetLeagueGroups(ICriteria criteria);
    }

    internal class FilterDataAccess : BaseRepository, IFilterDataAccess
    {
        public IEnumerable<short> GetCustomerClasses(ICriteria criteria)
            => ExecuteReader(criteria, ToCustomerClass);

        private static short ToCustomerClass(IDataReader reader)
            => Convert.ToInt16(reader["ResourceId"]);

        public IEnumerable<LeagueGroup> GetLeagueGroups(ICriteria criteria)
            => ExecuteReader(criteria, ToLeagueGroup);

        private static LeagueGroup ToLeagueGroup(IDataReader reader)
            => new LeagueGroup(
                Convert.ToInt16(reader["SportType"]),
                Convert.ToInt32(reader["GroupId"]),
                reader["GroupName"].ToString());
    }
}
