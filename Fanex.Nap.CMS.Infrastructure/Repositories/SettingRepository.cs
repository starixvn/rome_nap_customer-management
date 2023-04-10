namespace Fanex.Nap.CMS.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Data;
    using Fanex.Nap.CMS.Infrastructure.Models;
    using Fanex.NAP.Framework.Data;

    internal interface ISettingRepository
    {
        IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria);

        IEnumerable<SiteGroup> GetAllSites(ICriteria criteria);

        IEnumerable<IdNameModel> GetCreditSites(ICriteria criteria);
    }

    internal class SettingRepository : ISettingRepository
    {
        public IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria)
            => ExecuteReader(criteria, ToCurrency);

        public IEnumerable<SiteGroup> GetAllSites(ICriteria criteria)
            => ExecuteReader(criteria, ToSiteGroup);

        public IEnumerable<IdNameModel> GetCreditSites(ICriteria criteria)
            => ExecuteReader(criteria, ToCreditSite);

        private static IEnumerable<T> ExecuteReader<T>(ICriteria criteria, Func<IDataReader, T> funcMapper)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return funcMapper(reader);
                    }
                }
            }
        }

        private static IdNameModel ToCurrency(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["ExchangeId"]),
                Name = reader["Currency"].ToString()
            };

        private static SiteGroup ToSiteGroup(IDataReader reader)
            => new SiteGroup(
                Convert.ToInt32(reader["SiteId"]),
                reader["Site"].ToString(),
                Convert.ToBoolean(reader["IsLicensee"]));

        private static IdNameModel ToCreditSite(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["SiteId"]),
                Name = reader["Site"].ToString()
            };
    }
}
