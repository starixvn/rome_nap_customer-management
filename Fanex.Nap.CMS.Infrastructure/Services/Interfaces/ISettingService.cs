namespace Fanex.Nap.CMS.Infrastructure.Services.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Nap.CMS.Infrastructure.Models;

    public interface ISettingService
    {
        IEnumerable<IdNameModel> GetAllCurrencies();

        IEnumerable<IdNameModel> GetCreditCurrencies();

        IEnumerable<SiteGroup> GetAllSites(int userId = 0);

        IEnumerable<IdNameModel> GetCreditSites();

        IEnumerable<SiteGroup> GetSitesMarketCode();

        IEnumerable<Country> GetCountries();
    }
}
