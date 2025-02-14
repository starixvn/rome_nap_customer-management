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

        IEnumerable<IdNameModel> GetNonsportsProducts(ICriteria criteria);
        
        IEnumerable<SiteGroup> GetSitesMarketCode(ICriteria criteria);

        IEnumerable<Country> GetCountries(ICriteria criteria);
    }

    internal class SettingRepository : ISettingRepository
    {
        public IEnumerable<IdNameModel> GetCurrencies(ICriteria criteria)
            => ExecuteReader(criteria, ToCurrency);

        public IEnumerable<SiteGroup> GetAllSites(ICriteria criteria)
            => ExecuteReader(criteria, ToSiteGroup);

        public IEnumerable<IdNameModel> GetCreditSites(ICriteria criteria)
            => ExecuteReader(criteria, ToCreditSite);

        public IEnumerable<IdNameModel> GetNonsportsProducts(ICriteria criteria)
            => ExecuteReader(criteria, ToProduct);
            
        public IEnumerable<SiteGroup> GetSitesMarketCode(ICriteria criteria)
            => ExecuteReader(criteria, ToSiteMarketCode);

        public IEnumerable<Country> GetCountries(ICriteria criteria)
            => ExecuteReader(criteria, ToCountry);

        private static IEnumerable<T> ExecuteReader<T>(ICriteria criteria, Func<IDataReader, T> funcMapper)
        {
            using (var db = ObjectDbFactory.CreateInstance(criteria.GetSettingKey()))
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

        private static IdNameModel ToProduct(IDataReader reader)
            => new IdNameModel
            {
                Id = Convert.ToInt32(reader["ProductId"]),
                Name = reader["ProductName"].ToString()
            };

        private static SiteGroup ToSiteMarketCode(IDataReader reader)
            => new SiteGroup(
                Convert.ToInt32(reader["SiteId"]),
                reader["Site"].ToString(),
                reader["SiteLabel"].ToString(),
                reader["MarketCode"].ToString());

        private static Country ToCountry(IDataReader reader)
            => new Country(reader["CountryCode"].ToString(), reader["CountryName"].ToString());
    }
}
