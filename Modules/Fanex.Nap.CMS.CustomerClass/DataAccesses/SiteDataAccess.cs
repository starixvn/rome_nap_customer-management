namespace Fanex.Nap.CMS.CustomerClass.DataAccesses
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Nap.CMS.CustomerClass.Models;
    using Fanex.Nap.CMS.Infrastructure.Repositories;
    using Fanex.NAP.Framework.Data;

    internal interface ISiteDataAccess : IBaseRepository
    {
        Tuple<int, IEnumerable<SiteSetting>> GetPagedSiteSetting(ICriteria criteria);

        IEnumerable<SiteSetting> GetSiteSettings(ICriteria criteria);
    }

    internal class SiteDataAccess : BaseRepository, ISiteDataAccess
    {
        public Tuple<int, IEnumerable<SiteSetting>> GetPagedSiteSetting(ICriteria criteria)
            => ExecuteReaderPaging(criteria, ToSitesSetting);

        public IEnumerable<SiteSetting> GetSiteSettings(ICriteria criteria)
            => ExecuteReader(criteria, ToSiteSetting);

        private static SiteSetting ToSiteSetting(IDataReader reader)
            => new SiteSetting(
                int.Parse(reader["PolicyId"].ToString()),
                reader["PolicyTitle"].ToString(),
                reader["Site"].ToString(),
                reader["CurrencyName"].ToString(),
                string.Empty,
                reader["Platform"].ToString());

        private static SiteSetting ToSitesSetting(IDataReader reader)
            => new SiteSetting(
                int.Parse(reader["PolicyId"].ToString()),
                reader["PolicyTitle"].ToString(),
                reader["Sites"].ToString(),
                reader["CurrencyName"].ToString(),
                reader["CurrencyID"].ToString(),
                reader["Platform"].ToString());
    }
}
