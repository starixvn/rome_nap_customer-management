namespace Fanex.Nap.CMS.Infrastructure.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface ISettingService
    {
        IEnumerable<CurrencyModel> GetAllCurrencies();

        IEnumerable<CurrencyModel> GetCreditCurrencies();

        IEnumerable<SiteGroup> GetAllSites(int? userId = null);

        IEnumerable<IdNameModel> GetCreditSites();

        IEnumerable<IdNameModel> GetNonsportsProducts();
        
        IEnumerable<SiteGroup> GetSitesMarketCode();

        IEnumerable<Country> GetCountries();
    }
}
